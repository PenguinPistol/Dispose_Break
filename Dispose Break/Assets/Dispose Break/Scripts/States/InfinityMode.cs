using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;
using com.TeamPlug.Input;

public class InfinityMode : State
{
    public Ball ball;

    public Transform path;
    public Transform selectBlock;

    private int disposeCount;

    public override IEnumerator Initialize(params object[] _data)
    {
        TouchController.Instance.AddObservable(this);

        yield return null;
    }

    public override void Begin()
    {
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
                selectBlock = hitCollider.transform;
            }
        }
    }

    public override void TouchMoved(Vector3 touchPosition, int touchIndex)
    {

    }

    public override void TouchEnded(Vector3 touchPosition, int touchIndex)
    {
    }

    public void ShotBall()
    {
        ball.Shot();
    }
}