using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.Patterns;

public class ChallengeSelect : State
{
    public Transform oneWayStars;
    public Transform noGuideStars;

    public Text goodsText;

    private int onewayCount;
    private int noguideCount;

    private static int onewayLastClearLevel = -1;
    private static int noguideLastClearLevel = -1;

    public override IEnumerator Initialize(params object[] _data)
    {
        onewayCount = GameManager.Instance.OneWayCount;
        noguideCount = GameManager.Instance.NoGuideCount;

        // One Way Challenge 클리어 확인
        for (int i = 0; i < SaveData.oneWayClear; i++)
        {
            // 별 채우기
            oneWayStars.GetChild(i).GetComponent<Animator>().Play("Complete");
        }

        // No Guide Challenge 클리어 확인
        for (int i = 0; i < SaveData.noGuideClear; i++)
        {
            // 별 채우기
            noGuideStars.GetChild(i).GetComponent<Animator>().Play("Complete");
        }

        if(onewayLastClearLevel == -1)
        {
            onewayLastClearLevel = SaveData.oneWayClear;
        }

        if (noguideLastClearLevel == -1)
        {
            noguideLastClearLevel = SaveData.noGuideClear;
        }

        yield return null;
    }

    public override void Begin()
    {
        if(onewayLastClearLevel != SaveData.oneWayClear)
        {
            oneWayStars.GetChild(SaveData.oneWayClear - 1).GetComponent<Animator>().Play("Clear");
            onewayLastClearLevel = SaveData.oneWayClear;
        }

        if (noguideLastClearLevel != SaveData.noGuideClear)
        {
            noGuideStars.GetChild(SaveData.noGuideClear - 1).GetComponent<Animator>().Play("Clear");
            noguideLastClearLevel = SaveData.noGuideClear;
        }
    }

    public override void Execute()
    {
        goodsText.text = string.Format("{0}", SaveData.goods);

        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    oneWayStars.GetChild(0).GetComponent<Animator>().Play("Clear");
        //}
    }


    public override void Release()
    {
    }

    public void ShowOptionPopup()
    {
        PopupContoller.Instance.Show("OptionPopup");
    }

    public void StartChallenge(bool isOneWayChallenge)
    {
        if(isOneWayChallenge)
        {
            if(SaveData.oneWayClear < onewayCount)
            {
                StateController.Instance.ChangeState("OneWayChallenge");
            }
        }
        else
        {
            if (SaveData.noGuideClear < noguideCount)
            {
                StateController.Instance.ChangeState("NoGuideChallenge");
            }
        }
    }
}
