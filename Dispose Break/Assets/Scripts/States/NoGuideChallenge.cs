using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.Input;

public class NoGuideChallenge : GameState
{
    private const string CHALLENGE_NAME = "NoGuideChallenge";

    public Text goodsText;
    public Transform arrow;
    public AudioSource sePlayer;
    public Animator tutorial;

    private BlockGroup blockGroup;
    private List<BallSkin> unlockSkins;
    private List<int> hps;
    private int level;


    public override IEnumerator Initialize(params object[] _data)
    {
        SoundManager.Instance.sePlayer = sePlayer;
        SoundManager.Instance.PlaySe("Scroll");

        GameManager.Instance.currentGameMode = this;
        GetLevelData();
        // 획득할수 있는 볼 스킨 종류 체크
        unlockSkins = GameManager.Instance.ballSkins.FindAll(x => x.unlockType == 2);

        yield return null;
    }

    public override void Begin()
    {
        inventory.Initialize(inventoryBlocks, hps);
        shotButton.interactable = false;

        float rad = Mathf.Deg2Rad * ball.shotDegree;

        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
        float angle = Vector3.Angle(Vector3.right, dir);
        arrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        TouchController.Instance.AddObservable(this);
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowOptionPopup();
        }

        goodsText.text = string.Format("{0}", SaveData.goods);
    }

    public override void Release()
    {
        TouchController.Instance.RemoveObservable(this);
    }

    public override void TouchBegan(Vector3 touchPosition, int touchIndex)
    {
        if (tutorial.gameObject.activeSelf)
        {
            tutorial.Play("Fadeout");
        }
    }

    public override void TouchMoved(Vector3 touchPosition, int touchIndex)
    {
        if(selectBlock != null)
        {
            float z = selectBlock.transform.position.z;

            selectBlock.transform.position = new Vector3(touchPosition.x, touchPosition.y, z);
        }
    }

    public override void TouchEnded(Vector3 touchPosition, int touchIndex)
    {
        if(selectBlock != null)
        {
            selectBlock.CheckPosition();
            selectBlock = null;

            if(inventory.Count == 0)
            {
                shotButton.interactable = true;
            }
        }
    }

    public override void DisposeBlock(Block block)
    {
        selectBlock = Instantiate(block, transform);
        selectBlock.StartMoved();
        selectBlock.breakedAction = () => {
            // 블록 부서짐
        };

        disposedBlocks.Add(selectBlock);
    }

    protected override IEnumerator Shot()
    {
        ball.Shot();

        while(ball.Finished == false)
        {
            if (ball.bounceCount >= GameConst.DropCount)
            {
                dropButton.gameObject.SetActive(true);
            }

            yield return null;
        }

        if (disposedBlocks.FindAll(x => x.isBreaked).Count == disposedBlocks.Count && ball.isDroped == false)
        {
            // clear
            // 클리어보상 있는지 체크
            SaveData.noGuideClear += 1;

            if (SaveData.noGuideClear == GameManager.Instance.NoGuideCount)
            {
                PopupContoller.Instance.Show("ChallengeCompletePopup", CHALLENGE_NAME);
            }
            else
            {
                var unlockSkin = unlockSkins.Find(x => x.unlockLevel == level);

                PopupContoller.Instance.Show("ChallengeClearPopup", CHALLENGE_NAME, unlockSkin);
            }

        }
        else
        {
            // failed
            PopupContoller.Instance.Show("ChallengeFailedPopup", CHALLENGE_NAME);
        }
    }

    public void GetLevelData()
    {
        string where = string.Format("Stage = {0};", SaveData.noGuideClear+1);
        string query = string.Format(Database.SELECT_TABLE_ALL_WHERE, CHALLENGE_NAME, where);

        Database.Query(query, (reader) =>
        {
            level = int.Parse(reader.GetString(0));

            blockGroup = new BlockGroup
            {
                index = 0,
                unlock = 0
            };

            ball.shotDegree = float.Parse(reader.GetString(1));

            string[] index = reader.GetString(2).Split(',');
            string[] hp = reader.GetString(3).Split(',');

            for (int i = 0; i < index.Length; i++)
            {
                blockGroup.blockIndex.Add(int.Parse(index[i]));
                blockGroup.blockHp.Add(int.Parse(hp[i]));
            }
        });

        inventoryBlocks = new List<Block>();
        hps = new List<int>();

        for (int i = 0; i < blockGroup.Count; i++)
        {
            Block block = usedBlocks.Find(x => x.index == blockGroup.blockIndex[i]);
            inventoryBlocks.Add(block);
            hps.Add(blockGroup.blockHp[i]);
        }
    }

    public void ShowOptionPopup()
    {
        PopupContoller.Instance.Show("ChallengeOptionPopup");
    }
}
