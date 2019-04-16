using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.Input;
using com.TeamPlug.Patterns;

public class InfinityMode : GameState
{
    public Transform path;
    public Button shotButton;

    private bool isShot;

    public override IEnumerator Initialize(params object[] _data)
    {
        inventoryBlocks = new List<Block>
        {
            usedBlocks[0]
            , usedBlocks[0]
            , usedBlocks[1]
            , usedBlocks[1]
        };

        isShot = false;

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StateController.Instance.ChangeState("Main", false);
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

    public void ShotBall()
    {
        if(isShot)
        {
            return;
        }

        isShot = true;

        StartCoroutine(Shot());
    }

    private IEnumerator Shot()
    {
        ball.Shot();

        while(ball.Finished == false)
        {
            yield return null;
        }

        if(disposedBlocks.FindAll(x => x.isBreaked).Count == disposedBlocks.Count)
        {
            // 성공
            Debug.Log("성공!");

            yield return ball.ResetShot();

            CalculatePath();

            foreach (var item in disposedBlocks)
            {
                Destroy(item.gameObject);
            }
            disposedBlocks.Clear();

            inventory.Initialize(inventoryBlocks);
        }
        else
        {
            // 실패
            Debug.Log("실패...");
        }

        shotButton.interactable = false;
        isShot = false;
    }

    private void CalculatePath()
    {
        for (int i = 0; i < path.childCount; i++)
        {
            path.GetChild(i).localPosition = Vector3.Lerp(ball.direction, ball.direction * ball.speed, i * 0.1f);
        }
    }
}