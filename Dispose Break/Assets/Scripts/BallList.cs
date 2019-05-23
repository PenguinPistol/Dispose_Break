using UnityEngine;
using System.Collections;
using com.TeamPlug.View;
using System.Collections.Generic;

public class BallList : ListView<ShopItem, BallSkin>
{
    public int selectIndex = 0;

    public IEnumerator Init(List<BallSkin> _items, int _selectIndex)
    {
        yield return base.Init(_items);

        selectIndex = _selectIndex - 1;
        items[selectIndex].isSelected = true;
    }

    public override void SelectItem(int index)
    {
        if(items[index].isLock == false)
        {
            items[selectIndex].isSelected = false;

            items[index].isSelected = true;
            selectIndex = index;
            SaveData.equipSkin = index + 1;
            GameManager.Instance.equipedBallSkin = items[index].Data;
        }
    }

    public void UnlockItem(int index)
    {
        items.Find(x => x.Data.index == index).Unlock();

        if(SaveData.unlockSkins.Contains(index))
        {
            return;
        }

        SaveData.unlockSkins.Add(index);
    }
}
