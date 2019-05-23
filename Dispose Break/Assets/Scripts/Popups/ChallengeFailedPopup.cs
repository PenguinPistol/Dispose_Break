using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeFailedPopup : ThreeButtonPopup
{
    private string challengeName;

    public override void OnOpend(params object[] data)
    {
        challengeName = string.Format("{0}", data[0]);
    }

    public override void NegativeAction()
    {
        // regame

        StateController.Instance.ChangeState(challengeName);
    }

    public override void NeutralAction()
    {
        // home
        StateController.Instance.ChangeState("ChallengeSelect");
    }

    public override void PositiveAction()
    {
        // continue
        // 일단 광고가 없어서 그냥 챌린지 선택으로 넘어감
        StateController.Instance.ChangeState("ChallengeSelect");
    }
}
