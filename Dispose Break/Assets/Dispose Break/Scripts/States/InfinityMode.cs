using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;
using com.TeamPlug.Input;

public class InfinityMode : State
{
    public List<Block> blocks;
    public Ball ball;

    public Transform path;

    private int disposeCount;

    public override IEnumerator Initialize(params object[] _data)
    {
        blocks = new List<Block>();

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
            Debug.Log("hit : " + hitCollider.name);
        }
    }

    public void ShotBall()
    {
        if(disposeCount < blocks.Count)
        {
            return;
        }

        ball.Shot();
    }
}