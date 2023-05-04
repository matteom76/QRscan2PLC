using System;
using Android.Gms.Ads;
using Android.Gms.Ads.Interstitial;

namespace QRScan2PLC.Droid.Core
{
    public class InterstitialAdListener : AdListener
    {
        readonly InterstitialAd _ad;

        public InterstitialAdListener(InterstitialAd ad)
        {
            _ad = ad;
        }

        public override void OnAdLoaded()
        {
            base.OnAdLoaded();

            if (_ad.IsLoaded)
                _ad.Show();
        }
    }
}

