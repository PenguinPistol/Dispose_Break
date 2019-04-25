using System.Collections.Generic;
using UnityEngine;

public class BlockInventory : MonoBehaviour
{
    public Dictionary<string, InventoryItem> items;
    public InventoryItem itemPrefab;
    public Transform contentView;

    public int Count { get { return items.Count; } }

    private void Start()
    {
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
                items[name].pointerDown = () =>
                {
                    items[name].count -= 1;

                    GameManager.Instance.currentGameMode.DisposeBlock(items[name].block);

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
