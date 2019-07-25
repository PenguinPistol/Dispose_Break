using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.TeamPlug.Patterns;
using com.TeamPlug.Input;
using UnityEngine.Events;

public abstract class GameState : State
{
    #region Variables

    [Header("Data")]
    // 공
    public Ball ball;
    // 블럭 인벤토리 오브젝트
    public BlockInventory inventory;
    // 발사 예상경로
    public GuidePath path;
    // 효과음 플레이어
    public AudioSource sePlayer;
    // Escape 키 입력시 출력할 팝업이름(기본 : 앱 종료 팝업)
    public string escapePopupName = "QuitPopup";

    [Header("UI")]
    // 발사 버튼
    public Button shotButton;
    // 공 내리기 버튼
    public Button dropButton;
    // 재화 텍스트
    public Text goodsText;

    [HideInInspector]
    // 선택한 블록
    public Block selectBlock;

    // 배치된 블럭들
    protected List<Block> disposedBlocks;
    // 현재 블럭 그룹
    protected BlockGroup blockGroup;
    // 공 발사 위치
    protected Vector3 shotPosition;
    // 발사 여부
    protected bool isShot;
    // 드롭 여부
    protected bool isDroped;
    // 이어하기광고 보상콜백
    protected UnityAction RewardCallback;

    #endregion

    #region State class override functions

    public override IEnumerator Initialize(params object[] _data)
    {
        GameManager.Instance.currentGameMode = this;
        SoundManager.Instance.sePlayer = sePlayer;

        disposedBlocks = new List<Block>();

        isShot = false;
        isDroped = false;

        shotPosition = ball.transform.position;
        shotButton.interactable = false;

        RewardCallback = Continue;

        yield return null;
    }

    public override void Begin()
    {
        StartCoroutine(inventory.Initialize(blockGroup));

        if(path != null)
        {
            path.Calculate(ball.degree);
        }

        TouchController.Instance.AddObservable(this);
    }

    public override void Execute()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PopupContoller.Instance.Show(escapePopupName);
        }

        goodsText.text = string.Format("{0}", SaveData.goods);
    }

    public override void Release()
    {
        selectBlock = null;

        TouchController.Instance.RemoveObservable(this);
    }

    public override void TouchBegan(Vector3 touchPosition, int touchIndex)
    {
        var hitCollider = TouchController.Raycast2D(touchPosition);

        if (hitCollider != null)
        {
            if (hitCollider.tag.Equals("Block"))
            {
                // 이동 
                selectBlock = hitCollider.GetComponent<Block>();
                selectBlock.StartMoved();
            }
        }
    }

    public override void TouchMoved(Vector3 touchPosition, int touchIndex)
    {
        if (selectBlock != null)
        {
            float z = selectBlock.transform.position.z;

            selectBlock.transform.position = new Vector3(touchPosition.x, touchPosition.y, z);
        }
    }

    public override void TouchEnded(Vector3 touchPosition, int touchIndex)
    {
        if (selectBlock != null)
        {
            bool result = selectBlock.CheckPosition();

            if (result == false)
            {
                inventory.Add(selectBlock.index, selectBlock.hp);
                disposedBlocks.Remove(selectBlock);
                Destroy(selectBlock.gameObject);

                shotButton.interactable = false;
            }

            selectBlock = null;
        }

        if (inventory.Count == 0)
        {
            shotButton.interactable = true;
        }
    }

    #endregion

    #region Function
    // 부서진 블럭 체크
    public bool CheckSuccess()
    {
        var result = disposedBlocks.Find(x => x.isBreaked == false);

        return result == null;
    }

    // 발사 코루틴
    protected IEnumerator Shot()
    {
        ball.Shot();

        // 공이 발사라인에 도달했는지 체크
        while (ball.Finished == false)
        {
            // 일정횟수이상 튕겼는데 도달하지 못할경우 드롭버튼 활성화
            if (ball.bounceCount >= GameConst.DropCount)
            {
                dropButton.gameObject.SetActive(true);
            }

            yield return null;
        }

        // 드롭여부 체크
        if (isDroped)
        {
            // 공 원위치;
            StartCoroutine(ResetShot());
        }
        else
        {
            if (CheckSuccess())
            {
                Successe();
            }
            else
            {
                Failed();
                AdsManager.Instance.ShowInterstitial();
            }
        }

        isDroped = false;
        isShot = false;

        dropButton.gameObject.SetActive(false);
        shotButton.interactable = false;
    }

    public IEnumerator ResetShot()
    {
        yield return ResetBall();
        yield return inventory.Initialize(blockGroup);

        foreach (var item in disposedBlocks)
        {
            Destroy(item.gameObject);
        }
        disposedBlocks.Clear();

        path.Calculate(ball.degree);
    }

    // 발사 버튼에서 실행
    public void ShotBall()
    {
        if (isShot == false)
        {
            isShot = true;

            if (path != null)
            {
                path.gameObject.SetActive(false);
            }

            StartCoroutine(Shot());
        }
    }

    // 공 떨어뜨리기
    public virtual void DropBall()
    {
        dropButton.gameObject.SetActive(false);

        isDroped = true;

        ball.Drop();
    }

    public IEnumerator ResetBall()
    {
        Vector3 startPosition = ball.transform.position;
        float distance = Vector3.Distance(ball.transform.position, shotPosition);
        float startTime = Time.time;

        while(distance > 0.1f)
        {
            float discovered = (Time.time - startTime) * GameConst.DefaultSpeed * 2;
            float fracJouney = discovered / distance;

            ball.transform.position = Vector3.Lerp(startPosition, shotPosition, fracJouney);

            distance = Vector3.Distance(ball.transform.position, shotPosition);

            yield return null;
        }

        ball.transform.position = shotPosition;
    }

    public void ShowPopup(string popupName)
    {
        PopupContoller.Instance.Show(popupName);
    }
    #endregion

    #region virtual functions

    // 블럭 배치
    public virtual void DisposeBlock(int index, int hp, Vector3 position)
    {
        Debug.Log("position : " + position);
        selectBlock = Instantiate(GameManager.Instance.GetBlockByIndex(index), position, Quaternion.identity, transform);
        selectBlock.hp = hp;
        selectBlock.StartMoved();

        disposedBlocks.Add(selectBlock);
    }

    #endregion

    #region abstract functions
    // 성공 액션
    public abstract void Successe();
    // 실패 액션
    public abstract void Failed();
    // 이어하기
    public abstract void Continue();

    #endregion
}
