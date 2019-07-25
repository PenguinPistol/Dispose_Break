using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;
using System.Data;

public class GameManager : Singleton<GameManager>
{
    // 사용되는 블럭들
    public List<Block> usedBlocks;
    // 공 스킨들
    public List<BallSkin> ballSkins;
    // 장착한 공 스킨
    public BallSkin equipedBallSkin;

    // 현재 게임모드
    public GameState currentGameMode;

    private int onewayCount;
    private int noguideCount;

    public int OneWayCount { get { return onewayCount; } }
    public int NoGuideCount {  get { return noguideCount; } }

    private void Start()
    {
        Application.targetFrameRate = 60;

        StateController.Instance.Init();
        StateController.Instance.ChangeState(0);

        Database.Load();
        ballSkins = Database.LoadBallSkins();
        if(SaveData.unlockSkins.Contains(1) == false)
        {
            equipedBallSkin = ballSkins[0];
            ballSkins[0].isUnlock = true;
            SaveData.unlockSkins.Add(1);
        }

        onewayCount = GetChallengeCount("OneWayChallenge");
        noguideCount = GetChallengeCount("NoGuideChallenge");

        SoundManager.Instance.PlayBgm(0);

        AdsManager.Instance.ShowBanner();
    }

    private void Update()
    {
        //if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F11))
        //{
        //    SaveData.oneWayClear = 0;
        //}
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

    public Block GetBlockByIndex(int index)
    {
        return usedBlocks.Find(x => x.index == index);
    }

    
}
