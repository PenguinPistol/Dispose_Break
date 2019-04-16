using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;
using UnityEngine.EventSystems;

public class BlockInventory : MonoBehaviour
{
    public Dictionary<string, InventoryItem> items;
    public InventoryItem itemPrefab;
    public Transform contentView;

    private GameState gameState;

    private void Start()
    {
        gameState = ((GameState)StateController.Instance.CurrentState);

        items = new Dictionary<string, InventoryItem>();
    }

    public void Initialize(List<Block> blockList)
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            string name = blockList[i].blockName;

            if (items.ContainsKey(name))
            {
                items[name].count += 1;
            }
            else
            {
                items[name] = Instantiate(itemPrefab, contentView);
                items[name].Initialize(blockList[i], 1);
                items[name].pressEvent = delegate ()
                {
                    items[name].count -= 1;

                    gameState.DisposeBlock(items[name].block);

                    if (items[name].count <= 0)
                    {
                        Destroy(items[name].gameObject);
                        items.Remove(name);
                    }
                };
            }
        }
    }


}
