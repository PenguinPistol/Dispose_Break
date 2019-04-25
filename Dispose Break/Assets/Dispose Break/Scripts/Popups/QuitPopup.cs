using UnityEngine;

public class QuitPopup : TwoButtonPopup
{
    public override void CancelAction()
    {
    }

    public override void ConfirmAction()
    {
        Application.Quit();
    }
}
