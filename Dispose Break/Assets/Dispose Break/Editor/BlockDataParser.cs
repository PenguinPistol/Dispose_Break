using UnityEngine;
using UnityEditor;
using System.Collections;
using com.TeamPlug.Utility;

public class BlockDataParser
{
    const string ROOT = "Assets/Dispose Break";
    const string DATA_FORDER = ROOT + "/Data";
    const string FILE_FORDER = DATA_FORDER + "/Block";
    const string CSV_PATH = DATA_FORDER + "/BlockData.csv";

    [MenuItem("Dispose Break/Parse Data")]
    static void Parse()
    {
        
        // Assets/Resources 경로에 Jobs 폴더가 있는지 확인
        if (!AssetDatabase.IsValidFolder(FILE_FORDER))
        {
            // 없으면 해당 경로에 Jobs 폴더를 만든다.
            AssetDatabase.CreateFolder(DATA_FORDER, "Block");
        }

        var data = CSVReader.ReadData<BlockData>(CSV_PATH);

        for (int i = 0; i < data.Count; i++)
        {
            var asset = ScriptableObject.CreateInstance<BlockData>();

            asset.blockName = data[i].blockName;
            asset.blockIndex = data[i].blockIndex;
            asset.description = data[i].description;

            AssetDatabase.CreateAsset(asset, FILE_FORDER + "/" + data[i].blockName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}