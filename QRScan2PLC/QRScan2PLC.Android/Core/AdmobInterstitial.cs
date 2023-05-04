
using Android.Gms.Ads;
using Android.App;
using Android.Gms.Ads.Interstitial;

[assembly: Xamarin.Forms.Dependency(typeof(AdmobInterstitial))]

namespace QRScan2PLC.Droid.Core
{ 

    public class AdmobInterstitial : IAdmobInterstitial
    {
        InterstitialAd _ad;

        public void Show(string adUnit)
        {
            var context = Application.Context;
            _ad = new InterstitialAd(context);
            _ad.AdUnitId = adUnit;

            var intlistener = new InterstitialAdListener(_ad);
            intlistener.OnAdLoaded();
            _ad.AdListener = intlistener;

            var requestbuilder = new AdRequest.Builder();
            _ad.LoadAd(requestbuilder.Build());
        }
    }
}

