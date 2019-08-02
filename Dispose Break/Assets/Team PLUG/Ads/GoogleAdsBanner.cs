using System;
using GoogleMobileAds.Api;

public class GoogleAdsBanner
{
    private const string TEST_UNIT_ID = "ca-app-pub-3940256099942544/6300978111";

    private BannerView m_BannerView = null;

    public float Height { get { return m_BannerView.GetHeightInPixels(); } }

    private GoogleAdsBanner(Builder builder)
    {
        m_BannerView = builder.Ad;
    }

    public void Request()
    {
        // 광고요청
        var request = new AdRequest.Builder().Build();

        UnityEngine.Debug.Log("Banner Reqeust!");

        m_BannerView.LoadAd(request);
    }

    public void SetPosition(int x, int y)
    {
        m_BannerView.SetPosition(x, y);
    }

    public class Builder
    {
        private BannerView bannerView;

        private AdSize adSize;
        private AdPosition adPosition;

        public BannerView Ad
        {
            get { return bannerView; }
        }

        public Builder(string _unitId, AdSize _adSize, AdPosition _adPosition = AdPosition.Bottom)
        {
            bannerView = new BannerView(_unitId, _adSize, _adPosition);

            adSize = _adSize;
            adPosition = _adPosition;
        }

        public Builder(string _unitId, AdSize _adSize, int _x, int _y)
        {
            bannerView = new BannerView(_unitId, _adSize, _x, _y);

            adSize = _adSize;
            adPosition = AdPosition.Bottom;
        }

        public Builder SetTestMode()
        {
            bannerView = new BannerView(TEST_UNIT_ID, adSize, adPosition);
            return this;
        }

        public Builder SetTestMode(int _x, int _y)
        {
            bannerView = new BannerView(TEST_UNIT_ID, adSize, _x, _y);
            return this;
        }

        public Builder SetTestMode(bool _testMode, int _x, int _y)
        {
            bannerView = new BannerView(TEST_UNIT_ID, adSize, _x, _y);

            return this;
        }

        public Builder SetOnAdLaoded(EventHandler<EventArgs> _handler)
        {
            bannerView.OnAdLoaded += _handler;
            return this;
        }

        public Builder SetOnFailedLaoded(EventHandler<AdFailedToLoadEventArgs> _handler)
        {
            bannerView.OnAdFailedToLoad += _handler;
            return this;
        }

        public Builder SetOnAdOpening(EventHandler<EventArgs> _handler)
        {
            bannerView.OnAdOpening += _handler;
            return this;
        }

        public Builder SetOnAdClosed(EventHandler<EventArgs> _handler)
        {
            bannerView.OnAdClosed += _handler;
            return this;
        }

        public Builder SetOnAdLeavingApplication(EventHandler<EventArgs> _handler)
        {
            bannerView.OnAdLeavingApplication += _handler;
            return this;
        }

        public GoogleAdsBanner Build()
        {
            return new GoogleAdsBanner(this);
        }
    }
}
