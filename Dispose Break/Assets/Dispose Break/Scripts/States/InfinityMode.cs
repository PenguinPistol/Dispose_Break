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
    public Goods goods;

    private int rallyScore = 0;
    private int currentScore = 0;
    private State state = State.Ready;

    private int currentGroup = 1;
    private int rallyCount = 0;

    private List<BlockGroup> blockGroup;

    public override IEnumerator Initialize(params object[] _data)
    {
        GameManager.Instance.currentGameMode = this;

        blockGroup = GetUnlockGroup();


        SetInventoryBlock(blockGroup[0]);

        ball.wallAction = () =>
        {
            rallyScore++;
        };

        isShot = false;

        selectBlock = null;

        yield return null;
    }

    public override void Begin()
    {
        inventory.Initialize(inventoryBlocks);
        shotButton.interactable = false;
        CalculatePath();

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
                break;
        }
    }

    public override void Release()
    {
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


            CalculatePath();
            path.gameObject.SetActive(true);

            foreach (var item in disposedBlocks)
            {
                Destroy(item.gameObject);
            }
            disposedBlocks.Clear();

            var unlockGroup = GetUnlockGroup();
            int groupIndex = Random.Range(1, unlockGroup.Count);
            
            SetInventoryBlock(unlockGroup[groupIndex]);

            inventory.Initialize(inventoryBlocks);
        }
        else
        {
            // 실패
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

        // block index로 찾을 수 있게 변경해야됨
        for (int i = 0; i < group.Count; i++)
        {
            Block block = usedBlocks.Find(x => x.index == group.blockIndex[i]);
            block.hp = group.blockHp[i];
            inventoryBlocks.Add(block);
        }
    }

    private List<BlockGroup> GetUnlockGroup()
    {
        List<BlockGroup> result = new List<BlockGroup>();
        string query = string.Format(Database.SELECT_UNLOCK_GROUP, "InfinityModeGroup", rallyCount);

        Database.Query(query, (reader) =>
        {
            BlockGroup group = new BlockGroup
            {
                index = reader.GetInt32(0),
                unlock = reader.GetInt32(1)
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