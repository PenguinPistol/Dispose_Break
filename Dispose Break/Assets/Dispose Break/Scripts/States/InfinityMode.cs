using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Input;

public class InfinityMode : GameState
{
    public Transform path;

    public override IEnumerator Initialize(params object[] _data)
    {
        disposeBlocks = new List<Block>
        {
            usedBlocks[0]
            , usedBlocks[0]
            , usedBlocks[0]
            , usedBlocks[0]
            , usedBlocks[0]
        };

        TouchController.Instance.AddObservable(this);

        yield return null;
    }

    public override void Begin()
    {
        inventory.Initialize(disposeBlocks);
    }

    public override void Execute()
    {
        for (int i = 0; i < path.childCount; i++)
        {
            path.GetChild(i).localPosition = Vector3.Lerp(ball.direction, ball.direction * ball.speed, i * 0.1f);
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
    }

    public void ShotBall()
    {
        ball.Shot();
    }
}