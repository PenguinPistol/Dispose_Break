using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Input;

public class OneWayChallenge : GameState
{
    private List<BlockGroup> blockGroup;

    public override IEnumerator Initialize(params object[] _data)
    {
        GameManager.Instance.currentGameMode = this;
        GetLevelData();
        // 획득할수 있는 볼 스킨 종류 체크

        yield return null;
    }

    public override void Begin()
    {
        inventory.Initialize(inventoryBlocks);
        shotButton.interactable = false;
        path.Calculate(ball.shotDegree);

        TouchController.Instance.AddObservable(this);
    }

    public override void Execute()
    {
    }

    public override void Release()
    {
        TouchController.Instance.RemoveObservable(this);
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
        selectBlock.breakedAction = () =>
        {

        };

        disposedBlocks.Add(selectBlock);
    }

    protected override IEnumerator Shot()
    {
        ball.Shot();

        while(ball.Finished == false)
        {
            yield return null;
        }

        if (disposedBlocks.FindAll(x => x.isBreaked).Count == disposedBlocks.Count)
        {
            // clear
            // 클리어보상 있는지 체크
            SaveData.oneWayClear += 1;

            PopupContoller.Instance.Show("ChallengeClearPopup", StateController.Instance.CurrentStateName);
        }
        else
        {
            // failed

            PopupContoller.Instance.Show("ChallengeFailedPopup");
        }
    }

    public void GetLevelData()
    {
        string where = string.Format("Stage = {0};", SaveData.oneWayClear+1);
        string query = string.Format(Database.SELECT_TABLE_ALL_WHERE, "OneWayChallenge", where);

        inventoryBlocks = new List<Block>();

        Database.Query(query, (reader) =>
        {
            string level = reader.GetString(0);

            ball.shotDegree = float.Parse(reader.GetString(1));

            string[] index = reader.GetString(2).Split(',');
            string[] hp = reader.GetString(3).Split(',');

            for (int i = 0; i < index.Length; i++)
            {
                Block block = usedBlocks.Find(x => x.index == int.Parse(index[i]));
                block.hp = int.Parse(hp[i]);

                inventoryBlocks.Add(block);
            }
        });
    }
}
