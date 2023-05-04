using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace QRScan2PLC.Utils
{
    public class Acting_DisplayStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            ushort statusValue = 0;
            if (value[0] != null)
            { 
                statusValue = (ushort)value[0];
                var statusList = value[1] as ObservableCollection<StatusCode>;
                if (statusList != null)
                {
                    foreach (StatusCode status in statusList)
                        if (status.Value == statusValue)
                            return status.Description;
                }
            }
           
            return "";
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
