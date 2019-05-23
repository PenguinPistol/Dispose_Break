using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeClearPopup : TwoButtonPopup
{
    public GameObject noRewardText;
    public Image rewardImage;

    private string challengeName;

    public override void OnOpend(params object[] data)
    {
        SoundManager.Instance.PlaySe("Clear");

        challengeName = string.Format("{0}", data[0]);
        BallSkin unlockSkin = (BallSkin)data[1];

        if (unlockSkin != null)
        {
            SaveData.unlockSkins.Add(unlockSkin.index);
            rewardImage.sprite = unlockSkin.sprite;

            noRewardText.SetActive(false);
            rewardImage.gameObject.SetActive(true);
        }
        else
        {
            noRewardText.SetActive(true);
            rewardImage.gameObject.SetActive(false);
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
