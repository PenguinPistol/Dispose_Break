using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInventory : MonoBehaviour
{
    private const string KEY_FORTMAT = "{0}_{1}";

    private Dictionary<string, InventoryItem> items;

    public InventoryItem itemPrefab;
    public Transform contentView;

    public int Count { get { return items.Count; } }

    public IEnumerator Initialize(BlockGroup group)
    {
        yield return null;

        items = new Dictionary<string, InventoryItem>();

        for (int i = 0; i < group.Count; i++)
        {
            int index = group.blockIndex[i];
            int hp = group.blockHp[i];

            Add(index, hp);
        }

        SoundManager.Instance.PlaySe("Inventory");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F10))
        {
            foreach (var key in items.Keys)
            {
                Debug.Log(key);
            }
        }
    }

    public void Add(int index, int hp)
    {
        string key = string.Format(KEY_FORTMAT, index, hp);

        if (items.ContainsKey(key))
        {
            // 이미 추가된 항목이면 카운트 증가
            items[key].count++;
        }
        else
        {
            // 새로 추가
            items[key] = Instantiate(itemPrefab, contentView);
            items[key].Initialize(index, hp);
            items[key].PointerDownCallback = (position) =>
            {
                position = Camera.main.ScreenToWorldPoint(position);
                position.z = 0;
                GameManager.Instance.currentGameMode.DisposeBlock(index, hp, position);

                items[key].count--;

                if(items[key].count == 0)
                {
                    Destroy(items[key].gameObject);
                    items.Remove(key);
                }
            };
        }
    }
}
