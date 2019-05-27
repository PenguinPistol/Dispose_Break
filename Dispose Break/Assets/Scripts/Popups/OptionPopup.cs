using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopup : TwoButtonPopup
{
    public Toggle toggleBGM;
    public Toggle toggleSE;

    // Resume
    public override void ConfirmAction()
    {
        Close();
    }

    // coupon
    public override void CancelAction()
    {
    }
}
