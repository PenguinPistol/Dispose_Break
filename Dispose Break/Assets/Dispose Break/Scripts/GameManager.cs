using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;
using System.Data;

public class GameManager : Singleton<GameManager>
{
    public GameState currentGameMode;
    public List<BlockData> blocks;
    public BallSkin equipedBallSkin;
    public int goods;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        blocks = new List<BlockData>();

        string query = string.Format(Database.SELECT_TABLE_ALL, "Block");

        Database.Query(query, (reader) =>
        {
            BlockData block = new BlockData(reader.GetInt32(0))
            {
                blockName = reader.GetString(1),
                description = reader.GetString(2)
            };

            blocks.Add(block);
        });

        StateController.Instance.Init();
        StateController.Instance.ChangeState(0);
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
