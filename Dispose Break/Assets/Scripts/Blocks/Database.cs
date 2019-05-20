using UnityEngine;
using UnityEngine.Events;
using System.Data;
using Mono.Data.Sqlite;
using com.TeamPlug.Utility;
using System;
using System.Text;

public static class Database
{
    public const string CHECK_TABLE = "SELECT COUNT(*) FROM sqlite_master WHERE Name='{0}'";
    public const string SELECT_TABLE = "SELECT {0} FROM {1}";
    public const string SELECT_TABLE_WHERE = "SELECT {0} FROM {1} WHERE {2}";
    public const string SELECT_TABLE_ALL = "SELECT * FROM {0}";
    public const string SELECT_TABLE_ALL_WHERE = "SELECT * FROM {0} WHERE {1}";
    public const string INERT_TABLE = "INSERT INTO {0} VALUES({1});";

    public static bool Query(string query, UnityAction<IDataReader> queryAction)
    {
        try
        {
#if UNITY_EDITOR
            string connect = string.Format("URI=file:{0}/DisposeBreakDB.db", Application.dataPath);
#elif UNITY_ANDROID
            string connect = string.Format("URI=file:{0}/DisposeBreakDB.db", Application.persistentDataPath);
#endif
            IDbConnection dbConnect = new SqliteConnection(connect);
            dbConnect.Open();

            IDbCommand dbCommand = dbConnect.CreateCommand();
            dbCommand.CommandText = query;

            IDataReader reader = dbCommand.ExecuteReader();

            while (reader.Read())
            {
                queryAction(reader);
            }

            reader.Close();
            reader = null;

            dbCommand.Dispose();
            dbCommand = null;

            dbConnect.Dispose();
            dbConnect = null;

            return true;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);

            return false;
        }
    }

    public static void Save()
    {
        StringBuilder query = new StringBuilder();
        query.AppendFormat(CHECK_TABLE, "SaveData");

        int result = 0;

        // table check
        Query(query.ToString(), (reader) => {
            result = reader.GetInt32(0);
        });

        if(result == 0)
        {
            query.Clear();
            query.Append("CREATE TABLE SaveData (");
            query.Append("\'BestScore\' TEXT, ");
            query.Append("\'OneWayClear\' TEXT, ");
            query.Append("\'NoGuideClear\' TEXT, ");
            query.Append("\'Goods\' TEXT, ");
            query.Append("\'EquipSkin\' TEXT, ");
            query.Append("\'UnlockSkins\' TEXT, ");
            query.Append("\'BgmMute\' TEXT, ");
            query.Append("\'SeMute\' TEXT);");

            Query(query.ToString(), (reader) => { });
        }
        else
        {
            query.Clear();
            query.Append("DELETE FROM SaveData;");

            Query(query.ToString(), (reader) => { });
        }

        // insert

        query.Clear();
        query.Append("INSERT INTO SaveData VALUES (");
        query.AppendFormat("\'{0}\',", SaveData.bestScore);
        query.AppendFormat("\'{0}\',", SaveData.oneWayClear);
        query.AppendFormat("\'{0}\',", SaveData.noGuideClear);
        query.AppendFormat("\'{0}\',", SaveData.goods); 
        query.AppendFormat("\'{0}\',", SaveData.equipSkin);
        query.AppendFormat("\'{0}\',", SaveData.UnlockSkins);
        query.AppendFormat("\'{0}\',", SoundManager.Instance.muteBgm);
        query.AppendFormat("\'{0}\');", SoundManager.Instance.muteSe);

        Debug.Log(query.ToString());

        Query(query.ToString(), (reader) => { });
    }

    public static void Load()
    {
        StringBuilder query = new StringBuilder();
        query.AppendFormat(CHECK_TABLE, "SaveData");

        int result = 0;

        // table check
        Query(query.ToString(), (reader) => {
            result = reader.GetInt32(0);
        });

        if(result == 0)
        {
            SaveData.unlockSkins.Add(1);
            return;
        }

        query.Clear();
        query.AppendFormat(SELECT_TABLE_ALL, "SaveData");

        Query(query.ToString(), (reader) =>
        {
            SaveData.bestScore = int.Parse(reader.GetString(0));
            SaveData.oneWayClear = int.Parse(reader.GetString(1));
            SaveData.noGuideClear = int.Parse(reader.GetString(2));
            SaveData.goods = int.Parse(reader.GetString(3));
            SaveData.equipSkin = int.Parse(reader.GetString(4));
            string skins = reader.GetString(5).Trim();

            if(string.IsNullOrEmpty(skins))
            {
                SaveData.unlockSkins.Add(1);
            }
            else
            {
                var unlock = skins.Split(',');

                for (int i = 0; i < unlock.Length; i++)
                {
                    SaveData.unlockSkins.Add(int.Parse(unlock[i]));
                }
            }

            SoundManager.Instance.muteBgm = bool.Parse(reader.GetString(6));
            SoundManager.Instance.muteSe = bool.Parse(reader.GetString(7));
        });

        query.Clear();
        string where = string.Format("SkinIndex = {0};", SaveData.equipSkin);
        query.AppendFormat(SELECT_TABLE_ALL_WHERE, "BallSkin", where);

        Query(query.ToString(), (reader) =>
        {
            BallSkin skin = new BallSkin
            {
                index = int.Parse(reader.GetString(0)),
                name = reader.GetString(1),
                grade = reader.GetString(2),
                unlockType = int.Parse(reader.GetString(3)),
                unlockLevel = int.Parse(reader.GetString(4)),
            };

            skin.sprite = Resources.Load<Sprite>("Sprites/Ball/Ball_" + skin.index);

            GameManager.Instance.equipedBallSkin = skin;
        });
    }

    public static void ReadGameConst()
    {
        GameConst gameConst = new GameConst();

        CSVReader.ReadConst(gameConst, "Data/GameConst");
    }
}
