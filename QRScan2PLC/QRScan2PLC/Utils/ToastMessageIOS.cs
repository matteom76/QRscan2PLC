//using Foundation;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Utils;
using System;
using System.Collections.Generic;
using System.Text;
//using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessageIOS))]
namespace QRScan2PLC.Utils
{
    public class ToastMessageIOS : IToastMessage
    {
        const double SHORT_DELAY = 2.0;

        //NSTimer alertDelay;
        //UIAlertController alert;

        public void Show(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        void ShowAlert(string message, double seconds)
        {
            //alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            //{
            //    dismissMessage();
            //});
            //alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            //UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        private void dismissMessage()
        {
            //if (alert != null)
            //{
            //    alert.DismissViewController(true, null);
            //}
            //if (alertDelay != null)
            //{
            //    alertDelay.Dispose();
            //}
        }
    }
}
