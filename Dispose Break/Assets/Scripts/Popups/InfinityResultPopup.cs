using UnityEngine.Events;
using UnityEngine.UI;

public class InfinityResultPopup : TwoButtonPopup
{
    public Text bestScore;

    public override void OnOpend(params object[] data)
    {
        UnityEngine.Debug.Log("type : " + this.GetType().Name);

        SoundManager.Instance.PlaySe("Clear");

        bestScore.text = string.Format("{0}", SaveData.bestScore);

        SetMessage(string.Format("{0}", data[0]));
        AdsManager.Instance.RewardCallback = (UnityAction)data[1];

        if (AdsManager.Instance.LoadedReward == false)
        {
            confirm.interactable = false;
        }
        else
        {
            confirm.interactable = true;
        }
    }

    // continue
    public override void ConfirmAction()
    {
        PopupContoller.Instance.Show(this.GetType().Name);
        AdsManager.Instance.ShowReward();
    }

    // regame
    public override void CancelAction()
    {
        StateController.Instance.ChangeState(0);
    }
}
