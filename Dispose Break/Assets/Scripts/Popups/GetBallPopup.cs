using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetBallPopup : Popup
{
    public Image skin;

    public override void OnOpend(params object[] data)
    {
        int index = (int)data[0];

        Debug.Log("popup index : " + index);

        Sprite sprite = Resources.Load<Sprite>("Ball/Ball_" + index);

        Debug.Log(sprite);

        skin.sprite = sprite;
    }
}
