using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;
using System.Data;

public class GameManager : Singleton<GameManager>
{
    public GameState currentGameMode;
    public BallSkin equipedBallSkin;

    private IEnumerator Start()
    {
        Application.targetFrameRate = 60;

        CSVToSqlite parser = new CSVToSqlite();

        yield return parser.Parse("Block");
        yield return parser.Parse("BallSkin");
        yield return parser.Parse("InfinityModeGroup");
        yield return parser.Parse("NoGuideChallenge");
        yield return parser.Parse("OnWayChallenge");

        StateController.Instance.Init();
        StateController.Instance.ChangeState(0);

        Database.Load();

        SoundManager.Instance.PlayBgm(0);
    }

    // 
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("application pause");
            Database.Save();
        }
    }

    // 앱 종료 시
    private void OnApplicationQuit()
    {
        Database.Save();
    }
}
