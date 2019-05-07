using UnityEngine;
using UnityEngine.Events;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using com.TeamPlug.Utility;
using System;

public static class Database
{
    public const string SELECT_BLOCK = "SELECT * FROM Block";
    public const string SELECT_INFINITY_GROUP = "SELECT * FROM InfinityModeGroup";
    public const string SELECT_UNLOCK_GROUP = "SELECT * FROM {0} WHERE Unlock <= {1}";

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
            dbCommand.Dispose();
            dbConnect.Dispose();

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
