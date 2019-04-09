using UnityEngine;
using System.Collections.Generic;

namespace com.TeamPlug.Utility
{
    public class CSVReader
    {
        private const char SEPARATOR = ',';
        private const char DATA_START_CHAR = '@';

        private const int DATA_TYPE_INDEX = 0;
        private const int DATA_HEADER_INDEX = 1;
        private const int DATA_START_INDEX = 2;

        public static List<Dictionary<string, string>> ReadConst(string file)
        {
            var list = new List<Dictionary<string, string>>();
            TextAsset data = Resources.Load(file) as TextAsset;

            var lines = data.text.Split('\n');

            if (lines.Length <= 1)
            {
                return list;
            }

            string[] header = new string[0];

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] == '#')
                {
                    lines[i] = lines[i].Replace("#", "");
                    header = lines[i].Trim().Split(SEPARATOR);
                    continue;
                }

                var values = lines[i].Split(SEPARATOR);

                if (values.Length == 0 || values[0] == "")
                {
                    continue;
                }

                var entry = new Dictionary<string, string>();

                for (int j = 0; j < header.Length; j++)
                {
                    entry[header[j]] = values[j];
                }

                list.Add(entry);
            }

            return list;
        }

        public static Dictionary<string, string[]> ReadData(string _path)
        {
            var data = new Dictionary<string, string[]>();
            var textAsset = Resources.Load<TextAsset>(_path);
            var lines = textAsset.text.Split('\n');

            if (lines.Length < 1)
            {
                return data;
            }

            string[] types = lines[DATA_TYPE_INDEX].Trim().Split(SEPARATOR);
            string[] headers = lines[DATA_HEADER_INDEX].Trim().Split(SEPARATOR);

            var datas = new Dictionary<string, List<string>>();

            var haaderList = new List<string>();

            bool dataStart = false;

            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Length <= 0)
                    continue;
                haaderList.Add(headers[i]);
            }

            headers = haaderList.ToArray();

            for (int i = 0; i < headers.Length; i++)
            {
                datas.Add(headers[i], new List<string>());
            }


            for (int i = DATA_START_INDEX; i < lines.Length; i++)
            {
                // 데이터 시작/종료 체크
                if (lines[i][0] == DATA_START_CHAR)
                {
                    if (dataStart)
                    {
                        // 데이터 읽기 종료
                        break;
                    }

                    dataStart = true;
                    i++;    // 시작 첫줄 주석 넘기기
                    continue;
                }

                var values = lines[i].Split(SEPARATOR);

                if (values.Length == 0 || values[0] == "")
                {
                    continue;
                }

                for (int j = 0; j < headers.Length; j++)
                {
                    datas[headers[j]].Add(values[j]);
                }
            }

            data.Add("type", types);
            data.Add("header", headers);

            for (int i = 0; i < headers.Length; i++)
            {
                data.Add(headers[i] + "_data", datas[headers[i]].ToArray());
            }

            return data;
        }
    }
}