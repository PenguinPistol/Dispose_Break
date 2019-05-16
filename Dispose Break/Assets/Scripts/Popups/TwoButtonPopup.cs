using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class TwoButtonPopup : Popup
{
    public Button confirm;
    public Button cancel;

    private void Awake()
    {
        if(confirm != null)
        {
            confirm.onClick.AddListener(ConfirmAction);
            confirm.onClick.AddListener(Close);
        }

        if(cancel != null)
        {
            cancel.onClick.AddListener(CancelAction);
            cancel.onClick.AddListener(Close);
        }
    }

    public abstract void ConfirmAction();
    public abstract void CancelAction();
}
