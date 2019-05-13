using UnityEngine;
using UnityEngine.Events;
using System.Data;
using Mono.Data.Sqlite;
using com.TeamPlug.Utility;
using System;

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
            string connect = string.Format("URI=file:{0}/DisposeBreakDB.db",
            Application.dataPath);

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

    public static void ReadGameConst()
    {
        GameConst gameConst = new GameConst();

        CSVReader.ReadConst(gameConst, "Assets/Dispose Break/Data/GameConst.csv");
    }
}
