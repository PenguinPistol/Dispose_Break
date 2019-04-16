using System.Collections;
using System.Collections.Generic;
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

    public delegate void PressEvent();

    public PressEvent pressEvent;

    public void Initialize(Block data, int count)
    {
        this.block = data;
        this.count = count;

        image.sprite = data.Sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressEvent();
    }

    private void Update()
    {
        countText.text = string.Format("{0}", count);
    }
}
