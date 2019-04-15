using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;

public class Main : State
{
    public Transform point;
    public Transform moved;

    public override IEnumerator Initialize(params object[] _data)
    {
        yield return null;
    }

    public override void Begin()
    {
    }

    public override void Execute()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            var relative = point.InverseTransformPoint(moved.position);

            Debug.Log(relative);
        }
    }

    public override void Release()
    {
    }

    public void StartGame()
    {
        StateController.Instance.ChangeState("InfinityMode", false);
    }
}
