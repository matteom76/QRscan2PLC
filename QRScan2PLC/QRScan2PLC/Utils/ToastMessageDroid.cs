using Android.Widget;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;



[assembly: Xamarin.Forms.Dependency(typeof(ToastMessageDroid))]
namespace QRScan2PLC.Utils
{
    public class ToastMessageDroid : IToastMessage
    {
        public void Show(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
           
        }
    }
}
