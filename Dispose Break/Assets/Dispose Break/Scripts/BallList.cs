using UnityEngine;
using System.Collections;
using com.TeamPlug.View;
using System.Collections.Generic;

public class BallList : ListView<ShopItem, BallSkin>
{
    public int selectIndex;

    public IEnumerator Init(List<BallSkin> _items, int selectIndex)
    {
        yield return base.Init(_items);

        this.selectIndex = selectIndex;
        items[selectIndex].isSelected = true;
        GameManager.Instance.equipedBallSkin = items[selectIndex].Data;
    }

    public override void SelectItem(int index)
    {
        if(items[index].isLock == false)
        {
            items[selectIndex].isSelected = false;

            items[index].isSelected = true;
            selectIndex = index;
            GameManager.Instance.equipedBallSkin = items[index].Data;
        }
    }

    public void UnlockItem(int index)
    {
        items[index].Unlock();
    }
}
