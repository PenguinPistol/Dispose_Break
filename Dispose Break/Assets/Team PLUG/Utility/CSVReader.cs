using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace com.TeamPlug.Utility
{
    public class CSVReader
    {
        private const char SEPARATOR = ',';
        private const char DATA_START_CHAR = '@';

        private const int DATA_TYPE_INDEX = 0;
        private const int DATA_HEADER_INDEX = 1;
        private const int DATA_START_INDEX = 2;

        public const string KEY_TYPE = "type";
        public const string KEY_HEADER = "header";

        /// <summary>
        /// Const CSV 파일 읽기
        /// </summary>
        /// <param name="path">Assets/(path)</param>
        public static List<Dictionary<string, string>> ReadConstCSV(string path)
        {
            var list = new List<Dictionary<string, string>>();
            var textAsset = Resources.Load<TextAsset>(path);
            var lines = textAsset.text.Split('\n');

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


        /// <summary>
        /// Data CSV 파일 읽기
        /// </summary>
        /// <param name="path">CSV File Path</param>
        public static Dictionary<string, string[]> ReadDataCSV(string path)
        {
            var data = new Dictionary<string, string[]>();
            var textAsset = Resources.Load<TextAsset>(path);
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
                    i++;    // 시작 첫줄 넘기기
                    continue;
                }

                var values = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                //var values = lines[i].Split(SEPARATOR);

                if (values.Length == 0 || values[0] == "")
                {
                    continue;
                }

                for (int j = 0; j < headers.Length; j++)
                {
                    values[j] = values[j].Replace("\"", "");
                    datas[headers[j]].Add(values[j]);
                }
            }

            data.Add(KEY_TYPE, types);
            data.Add(KEY_HEADER, headers);

            for (int i = 0; i < headers.Length; i++)
            {
                data.Add(headers[i], datas[headers[i]].ToArray());
            }

            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constObejct"></param>
        /// <param name="path"></param>
        public static void ReadConst<T>(T constObejct, string path)
        {
            var readData = ReadConstCSV(path);

            foreach (var data in readData)
            {
                string type = data["타입"];
                string header = data["이름"];
                string value = data["값"];

                if (type.Equals("float"))
                {
                    SetFieldData<T, float>(constObejct, header.Trim(), value);
                }
                else if (type.Equals("int"))
                {
                    SetFieldData<T, int>(constObejct, header.Trim(), value);
                }
                else if (type.Equals("bool"))
                {
                    SetFieldData<T, bool>(constObejct, header.Trim(), value);
                }
                else if (type.Equals("string"))
                {
                    SetFieldData<T, string>(constObejct, header.Trim(), value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">데이터를 저장할 클래스</typeparam>
        /// <param name="path">Assets/(path)</param>
        /// <returns></returns>
        public static List<T> ReadData<T>(string path)
            where T : new()
        {
            var readData = ReadDataCSV(path);
            var types = readData[KEY_TYPE];
            var headers = readData[KEY_HEADER];

            List<T> result = new List<T>();

            for (int i = 0; i < readData[headers[0]].Length; i++)
            {
                T data = new T();

                for (int j = 0; j < headers.Length; j++)
                {
                    string value = readData[headers[j]][i];
                    value.Trim();

                    if (value.Length <= 0)
                        continue;

                    if (types[j].Equals("float"))
                    {
                        SetFieldData<T, float>(data, headers[j].Trim(), value);
                    }
                    else if (types[j].Equals("int"))
                    {
                        SetFieldData<T, int>(data, headers[j].Trim(), value);
                    }
                    else if (types[j].Equals("bool"))
                    {
                        SetFieldData<T, bool>(data, headers[j].Trim(), value);
                    }
                    else if (types[j].Equals("string"))
                    {
                        SetFieldData<T, string>(data, headers[j].Trim(), value);
                    }
                }

                result.Add(data);
            }


            return result;
        }


        /// <summary>
        /// 임의의 데이터 연결
        /// </summary>
        /// <typeparam name="T1">class</typeparam>
        /// <typeparam name="T2">variable type</typeparam>
        /// <param name="_object">값을 저장할 객체</param>
        /// <param name="_name">변수 이름</param>
        /// <param name="_value">변환해서 넣을 값</param>
        public static void SetFieldData<T1, T2>(T1 _object, string _name, object _value)
        {
            Type type = typeof(T1);

            // 인스턴스 멤버 포함, 정적멤버 포함, 퍼블릭 포함, 논 퍼블릭 포함, 대소문자 구분안함
            var info = type.GetField(_name
                , BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            if (info == null)
            {
                return;
            }

            T2 value = (T2)Convert.ChangeType(_value, typeof(T2));

            info.SetValue(_object, value);
        }
    }
}