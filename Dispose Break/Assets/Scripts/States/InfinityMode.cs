using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfinityMode : GameState
{
    //SELECT * FROM InfinityModeGroup WHERE CAST(UnlockLevel AS INTEGER) <= 3 ORDER BY random() LIMIT 1;
    private const string SELECT_WHERE_FORMAT = "CAST(UnlockLevel AS INTEGER) <= {0} ORDER BY RANDOM() LIMIT 1;";

    public Animator animator;
    public Text scoreText;
    public Goods goods;
    public Animator wallPoint;

    private int rallyCount = 0;
    private int rallyScore = 0;
    private int currentScore = 0;
    private bool isPlay = false;

    public override IEnumerator Initialize(params object[] _data)
    {
        yield return base.Initialize(_data);

        // 블록 그룹 불러오기
        blockGroup = GetUnlockGroup();

        // 공 방향 세팅
        ball.Initialize(GameConst.BallAngleDefault);
        ball.WallHitCallback = () =>
        {
            // 생성으로 변경
            wallPoint.transform.position = ball.transform.position;
            wallPoint.Play("WallPoint");
            // 랠리 포인트 누적
            rallyScore++;
        };
    }

    public override void Execute()
    {
        base.Execute();

        scoreText.text = string.Format("{0}", currentScore);
    }

    public override void DisposeBlock(int index, int hp, Vector3 position)
    {
        base.DisposeBlock(index, hp, position);

        if(isPlay == false)
        {
            isPlay = true;
            animator.Play("Play");
        }
       
        selectBlock.breakedAction = () => {
            rallyScore++;
        };
    }

    public override void Successe()
    {
        // 획득점수 합산
        currentScore += rallyScore;
        rallyScore = 0;
        rallyCount++;

        // 다음 블럭 로드
        blockGroup = GetUnlockGroup();

        // 공 원위치
        ball.SetDirection();
        StartCoroutine(ResetShot());

        // 튜토리얼 체크
        CheckTutorial();

        // 재화 생성체크
        goods.Pass();

        if(rallyCount == GameConst.GoodsSpawnCondition + goods.LivingCount)
        {
            goods.Show();
        }
    }

    public override void Failed()
    {
        // 최고점수 체크
        if (SaveData.bestScore < currentScore)
        {
            SaveData.bestScore = currentScore;
        }

        // 결과 팝업 출력
        PopupContoller.Instance.Show("InfinityResultPopup", currentScore, RewardCallback);
    }

    public override void Continue()
    {
        StartCoroutine(ResetShot());
    }

    public override void DropBall()
    {
        base.DropBall();
        rallyScore = 0;
    }

    private BlockGroup GetUnlockGroup()
    {
        // query
        string where = string.Format(SELECT_WHERE_FORMAT, rallyCount);
        string query = string.Format(Database.SELECT_TABLE_ALL_WHERE, "InfinityModeGroup", where);

        //Debug.Log(query);

        var result = new BlockGroup();

        Database.Query(query, (reader) =>
        {
            result.index = int.Parse(reader.GetString(0));
            result.unlock = int.Parse(reader.GetString(1));

            string[] indexSplit = reader.GetString(2).Split(',');
            string[] hpSplit = reader.GetString(3).Split(',');

            for (int i = 0; i < indexSplit.Length; i++)
            {
                result.blockIndex.Add(int.Parse(indexSplit[i]));
                result.blockHp.Add(int.Parse(hpSplit[i]));
            }
        });

        return result;
    }

    private void CheckTutorial()
    {
        if (SaveData.infoShield == false)
        {
            if (blockGroup.ContainsBlock(7) || blockGroup.ContainsBlock(8))
            {
                SaveData.infoShield = true;
                PopupContoller.Instance.Show("InfoPopup", 1);
            }
        }

        if (SaveData.infoHalf == false)
        {
            if (blockGroup.ContainsBlock(6))
            {
                SaveData.infoHalf = true;
                PopupContoller.Instance.Show("InfoPopup", 2);
            }
        }

        if (SaveData.infoReverse == false)
        {
            if (blockGroup.ContainsBlock(9))
            {
                SaveData.infoReverse = true;
                PopupContoller.Instance.Show("InfoPopup", 3);
            }
        }
    }
}