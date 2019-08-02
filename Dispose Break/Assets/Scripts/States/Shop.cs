using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using com.TeamPlug.Patterns;

public class Shop : State
{
    public BallList ballList;
    public List<BallSkin> allSkins;
    public Text goodsText;
    public AudioSource sePlayer;
    public Button lootReward;

    public override IEnumerator Initialize(params object[] _data)
    {
        SoundManager.Instance.sePlayer = sePlayer;
        SoundManager.Instance.PlaySe("Scroll");

        allSkins = GameManager.Instance.ballSkins;

        yield return ballList.Init(allSkins, SaveData.equipSkin);

        foreach(var index in SaveData.unlockSkins)
        {
            Debug.Log("unlock : " + index);
            GameManager.Instance.ballSkins.Find(x => x.index == index).isUnlock = true;
            ballList.UnlockItem(index);
        }

        if (AdsManager.Instance.LoadedReward == false)
        {
            lootReward.interactable = false;
        }
        else
        {
            lootReward.interactable = true;
        }
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

    public void LootGoods()
    {
        if (SaveData.goods < GameConst.LootCost)
        {
            return;
        }

        var lockedSkins = allSkins.FindAll(x => x.isUnlock == false);

        if (lockedSkins.Count == 0)
        {
            return;
        }

        SaveData.goods -= GameConst.LootCost;

        StartCoroutine(UnlockSkin(lockedSkins));
    }

    public void LootAds()
    {
        var lockedSkins = allSkins.FindAll(x => x.isUnlock == false);

        if (lockedSkins.Count == 0)
        {
            return;
        }

        AdsManager.Instance.RewardCallback = () => {
            StartCoroutine(UnlockSkin(lockedSkins));
        };
        AdsManager.Instance.ShowReward();
    }

    public IEnumerator UnlockSkin(List<BallSkin> lockedSkins)
    {
        yield return null;

        int lootIndex = Random.Range(0, lockedSkins.Count);

        lockedSkins[lootIndex].isUnlock = true;
        ballList.UnlockItem(lockedSkins[lootIndex].index);

        PopupContoller.Instance.Show("GetBallPopup", lockedSkins[lootIndex].index);
        SoundManager.Instance.PlaySe("GetBall");
    }
}