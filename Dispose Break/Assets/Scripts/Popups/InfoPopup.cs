using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopup : Popup
{
    public Image infoImage;

    public override void OnOpend(params object[] data)
    {
        string path = string.Format("Sprites/Info{0}", data[0]);

        infoImage.sprite = Resources.Load<Sprite>(path);
    }
}
