using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;

public class GameManager : Singleton<GameManager>
{
    public GameState currentGameMode;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        StateController.Instance.Init();
        StateController.Instance.ChangeState(0);

        currentGameMode = (GameState)StateController.Instance.CurrentState;
    }

    // 
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("application pause");
        }
    }

    // 앱 종료 시
    private void OnApplicationQuit()
    {
    }
}
