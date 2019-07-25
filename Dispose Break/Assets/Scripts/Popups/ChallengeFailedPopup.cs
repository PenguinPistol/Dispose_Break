using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChallengeFailedPopup : ThreeButtonPopup
{
    private string challengeName;

    public override void OnOpend(params object[] data)
    {
        challengeName = string.Format("{0}", data[0]);
        AdsManager.Instance.RewardCallback = (UnityAction)data[1];
        
        if(AdsManager.Instance.LoadedReward == false)
        {
            positiveButton.interactable = false;
        }
        else
        {
            positiveButton.interactable = true;
        }
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
        AdsManager.Instance.CancelCallback = () =>
        {
            PopupContoller.Instance.Show(this.GetType().Name);
        };
        // continue
        AdsManager.Instance.ShowReward();
    }
}
