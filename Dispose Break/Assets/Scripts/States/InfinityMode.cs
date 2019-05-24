using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.Input;

public class InfinityMode : GameState
{
    public enum State
    {
        Ready, Play, Pause, Finished
    }
    
    public Animator animator;
    public Text scoreText;
    public Text goodsText;
    public Goods goods;
    public AudioSource sePlayer;
    public Animator wallPoint;

    private int rallyScore = 0;
    private int currentScore = 0;
    private State state = State.Ready;

    private int rallyCount = 0;

    private List<BlockGroup> blockGroup;
    private List<int> hps;

    public override IEnumerator Initialize(params object[] _data)
    {
        SoundManager.Instance.sePlayer = sePlayer;
        SoundManager.Instance.PlaySe("Scroll");
        GameManager.Instance.currentGameMode = this;

        blockGroup = GetUnlockGroup();

        SetInventoryBlock(blockGroup[0]);

        ball.shotDegree = GameConst.BallAngleDefault;
        ball.wallAction = () =>
        {
            wallPoint.transform.position = ball.transform.position;
            wallPoint.Play("WallPoint");
            rallyScore++;
        };

        isShot = false;

        selectBlock = null;

        yield return null;
    }

    public override void Begin()
    {
        inventory.Initialize(inventoryBlocks, hps);
        shotButton.interactable = false;
        path.Calculate(ball.shotDegree);

        TouchController.Instance.AddObservable(this);
    }

    public override void Execute()
    {
        switch (state)
        {
            case State.Ready:
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PopupContoller.Instance.Show("QuitPopup");
                }
                break;

            case State.Play:
                scoreText.text = string.Format("{0}", currentScore);
                goodsText.text = string.Format("{0}", SaveData.goods);
                break;
        }
    }

    public override void Release()
    {
        TouchController.Instance.RemoveObservable(this);
    }

    public override void TouchBegan(Vector3 touchPosition, int touchIndex)
    {
        var hitCollider = TouchController.Raycast2D(touchPosition);

        if(hitCollider != null)
        {
            if(hitCollider.tag.Equals("Block"))
            {
                // 이동 
                selectBlock = hitCollider.GetComponent<Block>();
                selectBlock.StartMoved();
            }
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
        }

        selectBlock = null;

        if (inventory.Count == 0)
        {
            shotButton.interactable = true;
        }
    }

    public override void DisposeBlock(Block block)
    {
        if(state == State.Ready)
        {
            state = State.Play;
            animator.Play("Play");
        }

        selectBlock = Instantiate(block, transform);
        selectBlock.StartMoved();
        selectBlock.breakedAction = () => {
            rallyScore++;
        };

        disposedBlocks.Add(selectBlock);
    }

    protected override IEnumerator Shot()
    {
        ball.Shot();

        while (ball.Finished == false)
        {
            yield return null;
        }

        if (disposedBlocks.FindAll(x => x.isBreaked).Count == disposedBlocks.Count)
        {
            // 성공
            yield return ball.ResetShot();

            currentScore += rallyScore;
            rallyScore = 0;
            rallyCount++;

            goods.Pass();

            if(rallyCount % GameConst.GoodsSpawnCondition == 0)
            {
                goods.Show();
            }

            path.Calculate(ball.shotDegree);
            path.gameObject.SetActive(true);

            foreach (var item in disposedBlocks)
            {
                Destroy(item.gameObject);
            }
            disposedBlocks.Clear();

            var unlockGroup = GetUnlockGroup();
            int groupIndex = Random.Range(1, unlockGroup.Count);

            SoundManager.Instance.PlaySe("Inventory");
            SetInventoryBlock(unlockGroup[groupIndex]);

            inventory.Initialize(inventoryBlocks, hps);
        }
        else
        {
            // 실패
            if(SaveData.bestScore < currentScore)
            {
                SaveData.bestScore = currentScore;
            }

            SoundManager.Instance.PlaySe("Clear");
            PopupContoller.Instance.Show("InfinityResultPopup", currentScore);
        }

        shotButton.interactable = false;
        isShot = false;
    }

    public void ShowOptionPopup()
    {
        PopupContoller.Instance.Show("OptionPopup");
    }

    public void SetInventoryBlock(BlockGroup group)
    {
        inventoryBlocks = new List<Block>();
        hps = new List<int>();

        // block index로 찾을 수 있게 변경해야됨
        for (int i = 0; i < group.Count; i++)
        {
            Block block = usedBlocks.Find(x => x.index == group.blockIndex[i]);
            inventoryBlocks.Add(block);
            hps.Add(group.blockHp[i]);
        }
    }

    private List<BlockGroup> GetUnlockGroup()
    {
        List<BlockGroup> result = new List<BlockGroup>();

        string where = string.Format("cast(UnlockLevel as integer) <= {0};", rallyCount);
        string query = string.Format(Database.SELECT_TABLE_ALL_WHERE, "InfinityModeGroup", where);

        Database.Query(query, (reader) =>
        {
            BlockGroup group = new BlockGroup
            {
                index = int.Parse(reader.GetString(0)),
                unlock = int.Parse(reader.GetString(1))
            };

            string[] blockIndex = reader.GetString(2).Split(',');
            string[] blockHp = reader.GetString(3).Split(',');

            for (int i = 0; i < blockIndex.Length; i++)
            {
                group.blockIndex.Add(int.Parse(blockIndex[i]));
                group.blockHp.Add(int.Parse(blockHp[i]));
            }

            result.Add(group);
        });

        return result;
    }
}