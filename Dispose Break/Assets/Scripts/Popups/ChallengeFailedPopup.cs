using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeFailedPopup : ThreeButtonPopup
{
    public override void OnOpend(params object[] data)
    {
    }

    public override void NegativeAction()
    {
        // regame
    }

    public override void NeutralAction()
    {
        // home
    }

    public override void PositiveAction()
    {
        // continue
    }
}
