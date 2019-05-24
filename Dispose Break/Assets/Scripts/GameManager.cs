using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;
using System.Data;

public class GameManager : Singleton<GameManager>
{
    public GameState currentGameMode;
    public List<BallSkin> ballSkins = new List<BallSkin>();
    public BallSkin equipedBallSkin;

    private int onewayCount;
    private int noguideCount;

    public int OneWayCount { get { return onewayCount; } }
    public int NoGuideCount {  get { return noguideCount; } }

    private IEnumerator Start()
    {
        Application.targetFrameRate = 60;

        Database.ReadGameConst();
        CSVToSqlite parser = new CSVToSqlite();

        yield return parser.Parse("Block");
        yield return parser.Parse("BallSkin");
        yield return parser.Parse("InfinityModeGroup");
        yield return parser.Parse("NoGuideChallenge");
        yield return parser.Parse("OneWayChallenge");

        StateController.Instance.Init();
        StateController.Instance.ChangeState(0);

        Database.Load();
        ballSkins = Database.LoadBallSkins();

        onewayCount = GetChallengeCount("OneWayChallenge");
        noguideCount = GetChallengeCount("NoGuideChallenge");

        SoundManager.Instance.PlayBgm(0);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    SaveData.goods += 10;
        //}

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F11))
        {
            SaveData.oneWayClear = 0;
        }
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

    private int GetChallengeCount(string challenge)
    {
        string query = string.Format("SELECT COUNT(*) FROM {0};", challenge);
        int result = 0;

        Database.Query(query, (reader) =>
        {
            result = reader.GetInt32(0);
        });

        return result;
    }
}
