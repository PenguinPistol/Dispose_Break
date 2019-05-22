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

    public override IEnumerator Initialize(params object[] _data)
    {
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
}
