using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.Patterns;

public class PopupContoller : Singleton<PopupContoller>
{
    public Canvas container;
    public List<Popup> popupList;

    private Dictionary<string, Popup> popups;

    private void Awake()
    {
        popups = new Dictionary<string, Popup>();

        foreach(var item in popupList)
        {
            var popup = Instantiate(item, container.transform);
            popup.gameObject.SetActive(false);

            popups.Add(string.Format("{0}", item.GetType()), popup);
        }
    }

    public void Show(string name, params object[] data)
    {
        if(popups.ContainsKey(name))
        {
            popups[name].Show(data);
        }
    }
}
