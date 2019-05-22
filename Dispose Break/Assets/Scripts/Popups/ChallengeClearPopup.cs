using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeClearPopup : TwoButtonPopup
{
    private string challengeName;

    public override void OnOpend(params object[] data)
    {
        if(data.Length > 0)
        {
            challengeName = string.Format("{0}", data[0]);
        }
    }

    public override void CancelAction()
    {
        // home
        StateController.Instance.ChangeState("ChallengeSelect");
    }

    public override void ConfirmAction()
    {
        // next
        StateController.Instance.ChangeState(challengeName);
    }
}
