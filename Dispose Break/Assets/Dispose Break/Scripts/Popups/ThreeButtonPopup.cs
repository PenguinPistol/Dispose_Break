using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ThreeButtonPopup : Popup
{
    [Header("Buttons")]
    public Button positiveButton;
    public Button negativeButton;
    public Button neutralButton;

    private void Awake()
    {
        if(positiveButton != null)
        {
            positiveButton.onClick.AddListener(PositiveAction);
            positiveButton.onClick.AddListener(Close);
        }

        if (negativeButton != null)
        {
            negativeButton.onClick.AddListener(NegativeAction);
            negativeButton.onClick.AddListener(Close);
        }

        if (neutralButton != null)
        {
            neutralButton.onClick.AddListener(NeutralAction);
            neutralButton.onClick.AddListener(Close);
        }
    }

    public abstract void PositiveAction();
    public abstract void NegativeAction();
    public abstract void NeutralAction();
}
