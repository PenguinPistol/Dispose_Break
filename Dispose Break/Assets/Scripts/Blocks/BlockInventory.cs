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

    public void Initialize(List<Block> blockList, List<int> hp)
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            string key = blockList[i].blockName + hp[i];

            if (items.ContainsKey(key))
            {
                items[key].count += 1;
            }
            else
            {
                items[key] = Instantiate(itemPrefab, contentView);
                items[key].Initialize(blockList[i], 1, hp[i]);
                items[key].pointerDown = () =>
                {
                    items[key].count -= 1;

                    GameManager.Instance.currentGameMode.DisposeBlock(items[key].block);

                    if (items[key].count <= 0)
                    {
                        Destroy(items[key].gameObject);
                        items.Remove(key);
                    }
                };
            }
        }
    }
}
