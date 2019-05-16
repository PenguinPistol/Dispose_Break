using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerDownHandler
{
    public Block block;
    public int count;
    public Image image;
    public Text countText;
    public Button button;

    public delegate void PointerDownAction();
    public PointerDownAction pointerDown;

    public void Initialize(Block block, int count)
    {
        this.block = block;
        this.count = count;

        image.sprite = block.Sprite;
        image.SetNativeSize();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown?.Invoke();
    }

    private void Update()
    {
        countText.text = string.Format("{0}", count);
    }
}
