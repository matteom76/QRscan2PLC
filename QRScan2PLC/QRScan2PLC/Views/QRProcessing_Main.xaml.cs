using Xamarin.Forms;
using MarcTron.Plugin;

namespace QRScan2PLC.Views
{
    public partial class QRProcessing_Main : ContentPage
    {
        public QRProcessing_Main()
        {
            InitializeComponent();

            CrossMTAdmob.Current.OnInterstitialLoaded += Current_OnInterstitialLoaded;

            CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
            //TEST ca-app-pub-3940256099942544/1033173712
        }

        private void Current_OnInterstitialLoaded(object sender, System.EventArgs e)
        {
            CrossMTAdmob.Current.ShowInterstitial();
        }
    }
}
