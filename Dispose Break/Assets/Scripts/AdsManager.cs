using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using GoogleMobileAds.Api;
using com.TeamPlug.Patterns;

public class AdsManager : Singleton<AdsManager>
{
    private const string APP_ID = "ca-app-pub-3117214092102716~3202771270";

    private const string BANNER_ID = "ca-app-pub-3117214092102716/1138770479";
    private const string INTERSTITIAL_ID = "ca-app-pub-3117214092102716/1371377216";
    private const string REWARD_ID = "ca-app-pub-3117214092102716/3864187926";

    // 배너 광고
    private GoogleAdsBanner banner;
    // 전면 광고
    private GoogleAdsInterstitial inter;
    // 보상형 광고
    private GoogleAdsReward reward;

    private bool isCooldown;
    private bool loadedBanner;
    private bool isRewardComplete;

    public UnityAction RewardCallback;
    public UnityAction CancelCallback;

    public bool LoadedReward { get { return reward.Loaded; } }
    public bool LoadedBanner { get { return loadedBanner; } }
    public float BannerHeight
    {
        get
        {
            if (loadedBanner == false)
                return 0;
            return banner.Height;
        }
    }

    private int PixelsToDp(int _pixel)
    {
        return (int)(_pixel / (Screen.dpi / 160));
    }

    private int DpToPixels(int _dp)
    {
        return (int)(_dp * (Screen.dpi / 160));
    }

    private void Awake()
    {
        // firbase 초기화
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });

        MobileAds.Initialize(APP_ID);

        AdSize bannerSize = new AdSize(AdSize.FullWidth, 50);

        int notchHeight = Display.main.systemHeight - Screen.height;

        if (notchHeight > 0)
        {
            notchHeight = Mathf.RoundToInt(PixelsToDp(notchHeight));
        }

        int y = Mathf.RoundToInt(PixelsToDp(Display.main.systemHeight)) * 2 - notchHeight;

        banner = new GoogleAdsBanner.Builder(BANNER_ID, bannerSize, 0, 0)
                  //.SetTestMode(0, 0)
                  .SetOnFailedLaoded(BannerFailedToLoad)
                  .SetOnAdLaoded(BannerSuccessLoaded)
                  .Build();

        banner.SetPosition(0, y);

        inter = new GoogleAdsInterstitial.Builder(INTERSTITIAL_ID)
            //.SetTestMode()
            .SetOnAdFailedToLoad(InterFailed)
            .SetOnAdClose(InterClosed)
            .Build();
        inter.Request();

        reward = new GoogleAdsReward.Builder(REWARD_ID)
            //.SetTestMode()
            .SetOnAdFailedToLoad(RewardFailedToLoad)
            .SetOnAdClosed(RewardClosed)
            .SetOnAdCompleted(RewardComplete)
            .SetOnAdRewarded(Rewarded)
            .Build();
        reward.Request();

        isCooldown = false;

        DontDestroyOnLoad(gameObject);
    }

    public void ShowBanner()
    {
        banner.Request();
    }

    public void RequestReward()
    {
        if (LoadedReward)
        {
            return;
        }

        reward.Request();
    }

    public void ShowInterstitial()
    {
        if (isCooldown)
        {
            return;
        }

        isCooldown = true;

        inter.Request();
        inter.Show();

        StartCoroutine(InterstitalAdCooltime());
    }

    public void ShowReward()
    {
        isRewardComplete = false;

        reward.Show();
    }

    private void InterClosed(object sender, EventArgs args)
    {
        inter.Request();
    }

    private void InterFailed(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("## Interstitial Ad Load Failed ## " + args.Message);
    }

    private void RewardClosed(object sender, EventArgs args)
    {
        if(isRewardComplete)
        {
            RewardCallback?.Invoke();
            //°]
        }
        else
        {
            CancelCallback?.Invoke();
        }

        reward.Request();
    }

    private void RewardComplete(object sender, EventArgs args)
    {
        isRewardComplete = true;
    }

    private void RewardFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("## Reward Vidio Load Failed ## " + args.Message);
    }

    private void Rewarded(object sender, Reward args)
    {
    }

    public void BannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.LogFormat("## Banner Failed to load : {0} ##", args.Message);

        loadedBanner = false;
    }

    public void BannerSuccessLoaded(object sender, EventArgs args)
    {
        Debug.Log("########## Banner Load Successed! ##########");
        loadedBanner = true;
    }

    private IEnumerator InterstitalAdCooltime()
    {
        float time = GameConst.InterstitialAdsCooltime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        isCooldown = false;
    }
}
