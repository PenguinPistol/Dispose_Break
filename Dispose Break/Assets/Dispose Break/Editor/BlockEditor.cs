using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlockEditor : EditorWindow
{
    private const string ROOT = "Assets/DisposeBreak/";
    private const string EXTENSION = ".prefab";

    private const string BASE_PREFAB_PATH = ROOT + "Editor/Base" + EXTENSION;
    private const string CREATED_PATH = ROOT + "Prefabs/Blocks/";

    private static BlockEditor window;

    private string blockName;
    private Sprite blockSprite;
    private int blockHP;

    private static GUIStyle titleStyle;


    [MenuItem("DisposeBreak/New Block")]
    private static void Init()
    {
        window = GetWindow<BlockEditor>();

        window.minSize = new Vector2(300, 400);
        window.maxSize = window.minSize;

        window.Show();
    }

    private void OnEnable()
    {
        window = GetWindow<BlockEditor>();

        titleStyle = new GUIStyle()
        {
            fontSize = 30,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fixedHeight = 50
        };
    }

    private void OnGUI()
    {
        GUILayout.Label("BLOCK EDITOR", titleStyle);

        blockSprite = (Sprite)EditorGUILayout.ObjectField("Image", blockSprite, typeof(Sprite), true);
        blockName = EditorGUILayout.TextField("Name", blockName);
        blockHP = EditorGUILayout.IntSlider("HP", blockHP, 1, 10);

        if (string.IsNullOrEmpty(blockName))
        {
            EditorGUILayout.HelpBox("Input Name", MessageType.Error, true);
            return;
        }

        if (blockSprite == null)
        {
            EditorGUILayout.HelpBox("Select Image", MessageType.Error, true);
            return;
        }

        if (GUILayout.Button("Create"))
        {
            var basePrefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(BASE_PREFAB_PATH);

            var instantRoot = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab);
            instantRoot.GetComponent<SpriteRenderer>().sprite = blockSprite;

            var block = instantRoot.GetComponent<Block>();
            block.hp = blockHP;
            block.name = blockName;

            var variant = PrefabUtility.SaveAsPrefabAsset(instantRoot, CREATED_PATH + blockName + EXTENSION);

            DestroyImmediate(instantRoot);

            window.Close();
        }
    }
}
