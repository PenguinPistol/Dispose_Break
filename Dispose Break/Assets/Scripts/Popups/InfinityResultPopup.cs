using UnityEngine.UI;

public class InfinityResultPopup : TwoButtonPopup
{
    public Text bestScore;

    public override void OnOpend(params object[] data)
    {
        bestScore.text = string.Format("{0}", SaveData.bestScore);

        if (data.Length > 0)
        {
            SetMessage(string.Format("{0}", data[0]));
        }
    }

    // regame
    public override void ConfirmAction()
    {
        StateController.Instance.ChangeState(0);
    }

    // continue
    public override void CancelAction()
    {
        StateController.Instance.ChangeState(0);
    }
}
