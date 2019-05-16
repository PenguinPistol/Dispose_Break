using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using com.TeamPlug.Utility;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class CSVToSqlite
{
    public IEnumerator Parse(string fileName)
    {
        string path = string.Format("Data/{0}", fileName);
        var data = CSVReader.ReadDataCSV(string.Format("{0}", path));

        var headers = data[CSVReader.KEY_HEADER];
        var types = data[CSVReader.KEY_TYPE];

        yield return CreateTable(fileName, headers, types);

        for (int i = 0; i < data[headers[0]].Length; i++)
        {
            string[] values = new string[headers.Length];

            for (int j = 0; j < headers.Length; j++)
            {
                values[j] = data[headers[j]][i];
            }

            yield return InsertData(fileName, values);
        }

        //EditorUtility.ClearProgressBar();
    }

    private IEnumerator CreateTable(string tableName, string[] header, string[] types)
    {
        string query = string.Format(Database.CHECK_TABLE, tableName);

        int result = -1;

        Database.Query(query, (reader) => {
            result = reader.GetInt32(0);
        });

        var sb = new System.Text.StringBuilder();

        if(result == 0)
        {
            sb.AppendFormat("CREATE TABLE \'{0}\' (", tableName);

            for (int i = 0; i < header.Length; i++)
            {
                sb.AppendFormat("\'{0}\' TEXT", header[i]);

                if(i < header.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(");");

            query = sb.ToString();

            Database.Query(query, (reader) => { });
        }
        else
        {
            query = string.Format("DELETE FROM {0}", tableName);

            Database.Query(query, (reader) => { });
        }

        yield return null;
    }

    private IEnumerator InsertData(string tableName, string[] values)
    {
        var sb = new System.Text.StringBuilder();
        // INSERT INTO tableName (headers) VALUES (values);
        sb.AppendFormat("INSERT INTO {0} VALUES (", tableName);

        for (int i = 0; i < values.Length; i++)
        {
            sb.AppendFormat("\'{0}\'", values[i]);

            if (i < values.Length - 1)
            {
                sb.Append(", ");
            }
        }
        sb.Append(");");

        string query = sb.ToString();

        //Debug.Log("Insert : " + query);
        Database.Query(query, (reader) => { });

        yield return null;
    }

}
