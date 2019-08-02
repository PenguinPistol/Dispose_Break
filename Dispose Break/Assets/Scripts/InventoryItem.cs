using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerDownHandler
{
    private const string SPRITE_PATH_FORMAT = "Sprites/Block/Inventory/{0}";

    [Header("UI")]
    public Image    image;
    public Text     countText;
    public Text     hpText;

    public int      blockIndex;
    public int      blockHp;
    public int      count;

    public UnityAction<Vector3> PointerDownCallback;

    public void Initialize(int index, int hp)
    {
        blockIndex = index;
        blockHp = hp;
        count = 1;

        string name = GameManager.Instance.GetBlockByIndex(index).blockName;
        image.sprite = Resources.Load<Sprite>(string.Format(SPRITE_PATH_FORMAT, name));
        image.SetNativeSize();

        if(hp > 1)
        {
            hpText.text = string.Format("{0}", hp);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownCallback?.Invoke(eventData.position);
    }

    private void Update()
    {
        countText.text = string.Format(" x {0}", count);
    }
}
