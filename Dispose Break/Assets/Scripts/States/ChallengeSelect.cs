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

    public Text tempClearOne;
    public Text tempClearNo;

    private int onewayCount;
    private int noguideCount;

    public override IEnumerator Initialize(params object[] _data)
    {
        onewayCount = GetChallengeCount("OneWayChallenge");
        noguideCount = GetChallengeCount("NoGuideChallenge");

        tempClearOne.text = string.Format("{0}/{1}", SaveData.oneWayClear, onewayCount);
        tempClearNo.text = string.Format("{0}/{1}", SaveData.noGuideClear, noguideCount);

        // One Way Challenge 클리어 확인
        for (int i = 0; i < SaveData.oneWayClear; i++)
        {
            // 별 채우기
            //oneWayStars.GetChild(i).GetComponent<Animator>().Play("Clear_Idle");
        }

        // No Guide Challenge 클리어 확인
        for (int i = 0; i < SaveData.noGuideClear; i++)
        {
            // 별 채우기
            //noGuideStars.GetChild(i).GetComponent<Animator>().Play("Clear_Idle");
        }

        yield return null;
    }

    public override void Begin()
    {
    }

    public override void Execute()
    {
        goodsText.text = string.Format("{0}", SaveData.goods);
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

    private int GetChallengeCount(string challenge)
    {
        string query = string.Format("SELECT COUNT(*) FROM {0};", challenge);
        int result = 0;

        Database.Query(query, (reader) =>
        {
            result = reader.GetInt32(0);
        });

        return result;
    }
}
