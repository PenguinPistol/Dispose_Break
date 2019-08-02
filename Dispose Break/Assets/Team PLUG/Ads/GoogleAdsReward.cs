using System;
using GoogleMobileAds.Api;

public class GoogleAdsReward
{
    private const string TEST_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";

    private RewardBasedVideoAd m_RewardAd;
    private string m_UnitId;

    public bool Loaded { get { return m_RewardAd.IsLoaded(); } }

    private GoogleAdsReward(Builder builder)
    {
        m_RewardAd = builder.Ad;
        m_UnitId = builder.UnitId;
    }

    public void Request()
    {
        if (m_RewardAd == null)
        {
            return;
        }

        var request = new AdRequest.Builder().Build();

        m_RewardAd.LoadAd(request, m_UnitId);
    }

    public void Show()
    {
        if(m_RewardAd == null)
        {
            return;
        }

        m_RewardAd.Show();
    }

    public class Builder
    {
        private RewardBasedVideoAd rewardAd;
        private string unitId;

        public RewardBasedVideoAd Ad
        {
            get { return rewardAd; }
        }

        public string UnitId
        {
            get { return unitId; }
        }

        public Builder(string _unitId)
        {
            rewardAd = RewardBasedVideoAd.Instance;
            unitId = _unitId;
        }

        public Builder SetTestMode()
        {
            unitId = TEST_UNIT_ID;

            return this;
        }

        //SetOnAdLaoded
        public Builder SetOnAdLoaded(EventHandler<EventArgs> _handler)
        {
            rewardAd.OnAdLoaded += _handler;
            return this;
        }

        public Builder SetOnAdOpening(EventHandler<EventArgs> _handler)
        {
            rewardAd.OnAdOpening += _handler;
            return this;
        }

        public Builder SetOnAdStarted(EventHandler<EventArgs> _handler)
        {
            rewardAd.OnAdStarted += _handler;
            return this;
        }

        public Builder SetOnAdCompleted(EventHandler<EventArgs> _handler)
        {
            rewardAd.OnAdCompleted += _handler;
            return this;
        }

        public Builder SetOnAdClosed(EventHandler<EventArgs> _handler)
        {
            rewardAd.OnAdClosed += _handler;
            return this;
        }

        public Builder SetOnAdLeavingApplication(EventHandler<EventArgs> _handler)
        {
            rewardAd.OnAdLeavingApplication += _handler;
            return this;
        }

        public Builder SetOnAdFailedToLoad(EventHandler<AdFailedToLoadEventArgs> _handler)
        {
            rewardAd.OnAdFailedToLoad += _handler;
            return this;
        }

        public Builder SetOnAdRewarded(EventHandler<Reward> _handler)
        {
            rewardAd.OnAdRewarded += _handler;
            return this;
        }

        public GoogleAdsReward Build()
        {
            return new GoogleAdsReward(this);
        }
    }
}
