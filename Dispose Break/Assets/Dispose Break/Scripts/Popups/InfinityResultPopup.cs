using UnityEngine.UI;

public class InfinityResultPopup : ThreeButtonPopup
{
    public Text bestScore;

    public override void OnOpend(params object[] data)
    {
        bestScore.text = string.Format("{0}", 0);

        if (data.Length > 0)
        {
            SetMessage(string.Format("{0}", data[0]));
        }
    }

    // regame
    public override void NegativeAction()
    {
        StateController.Instance.ChangeState(0);
    }

    // home
    public override void NeutralAction()
    {
        StateController.Instance.ChangeState(0);
    }

    // continue
    public override void PositiveAction()
    {
        StateController.Instance.ChangeState(0);
    }
}
