using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerDownHandler
{
    public Block block;
    public int count;
    public Image image;
    public Text countText;
    public Text hpText;
    public Button button;

    public delegate void PointerDownAction();
    public PointerDownAction pointerDown;

    public void Initialize(Block block, int count, int hp)
    {
        this.block = block;
        this.block.hp = hp;
        this.count = count;

        image.sprite = Resources.Load<Sprite>(string.Format("Sprites/Block/Inventory/{0}", block.name));
        image.SetNativeSize();

        if(hp > 1)
        {
            hpText.text = string.Format("{0}", hp);
        }
        else
        {
            hpText.text = "";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown?.Invoke();
    }

    private void Update()
    {
        countText.text = string.Format(" x {0}", count);
    }
}
