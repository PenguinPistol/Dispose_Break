
public class ChallengeOptionPopup : TwoButtonPopup
{
    // Resume
    public override void ConfirmAction()
    {
    }

    // Home
    public override void CancelAction()
    {
        StateController.Instance.ChangeState("ChallengeSelect");
    }
}
