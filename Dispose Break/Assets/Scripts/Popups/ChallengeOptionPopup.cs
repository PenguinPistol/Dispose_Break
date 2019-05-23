using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeOptionPopup : TwoButtonPopup
{
    public Toggle toggleBGM;
    public Toggle toggleSE;

    // Resume
    public override void ConfirmAction()
    {
        Close();
    }

    // Home
    public override void CancelAction()
    {
        StateController.Instance.ChangeState("ChallengeSelect");
    }
}
