using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.View;

public class ShopItem : ListViewItem<BallSkin>
{
    public GameObject lockImage;
    public GameObject selectImage;
    public Image ball;
    public bool isLock;
    public bool isSelected;

    public override void Init(BallSkin _data, int _index)
    {
        base.Init(_data, _index);

        ball.sprite = _data.sprite;
        isLock = true;
        isSelected = false;
    }

    private void Update()
    {
        if(isLock == false)
        {
            selectImage.SetActive(isSelected);
        }
    }

    public void Unlock()
    {
        lockImage.SetActive(false);
        isLock = false;
    }
}
