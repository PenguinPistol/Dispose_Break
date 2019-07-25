using System;
using GoogleMobileAds.Api;

public class GoogleAdsInterstitial
{
    private const string TEST_UNIT_ID = "ca-app-pub-3940256099942544/1033173712";

    private InterstitialAd m_InterstitialAd;

    private GoogleAdsInterstitial(Builder builder)
    {
        m_InterstitialAd = builder.Ad;
    }

    public void Request()
    {
        var request = new AdRequest.Builder().Build();
        m_InterstitialAd.LoadAd(request);
    }

    public void Show()
    {
        if(m_InterstitialAd == null)
        {
            return;
        }

        m_InterstitialAd.Show();
    }

    public class Builder
    {
        private InterstitialAd interstitialAd;

        public InterstitialAd Ad
        {
            get { return interstitialAd; }
        }

        public Builder(string _unitId)
        {
            if(string.IsNullOrEmpty(_unitId))
            {
                interstitialAd = new InterstitialAd(TEST_UNIT_ID);
            }
            else
            {
                interstitialAd = new InterstitialAd(_unitId);
            }
        }

        public Builder SetTestMode()
        {
            interstitialAd = new InterstitialAd(TEST_UNIT_ID);
            return this;
        }

        public Builder SetOnAdLoaded(EventHandler<EventArgs> _handler)
        {
            interstitialAd.OnAdLoaded += _handler;
            return this;
        }

        public Builder SetOnAdFailedToLoad(EventHandler<AdFailedToLoadEventArgs> _handler)
        {
            interstitialAd.OnAdFailedToLoad += _handler;
            return this;
        }

        public Builder SetOnAdOpening(EventHandler<EventArgs> _handler)
        {
            interstitialAd.OnAdOpening += _handler;
            return this;
        }

        public Builder SetOnAdClose(EventHandler<EventArgs> _handler)
        {
            interstitialAd.OnAdClosed += _handler;
            return this;
        }

        public Builder SetOnAdLeavingApplication(EventHandler<EventArgs> _handler)
        {
            interstitialAd.OnAdLeavingApplication += _handler;
            return this;
        }

        public GoogleAdsInterstitial Build()
        {
            return new GoogleAdsInterstitial(this);
        }
    }
}
