using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeCompletePopup : Popup
{
    public Transform result;

    public override void OnOpend(params object[] data)
    {
        string challenge = string.Format("{0}", data[0]);

        if (challenge.Equals("OneWayChallenge"))
        {
            result.GetChild(0).gameObject.SetActive(true);
            result.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            result.GetChild(0).gameObject.SetActive(false);
            result.GetChild(1).gameObject.SetActive(true);
        }

        SoundManager.Instance.PlaySe("Clear");
    }

    public void BackHome()
    {
        StateController.Instance.ChangeState("ChallengeSelect");
    }
}
