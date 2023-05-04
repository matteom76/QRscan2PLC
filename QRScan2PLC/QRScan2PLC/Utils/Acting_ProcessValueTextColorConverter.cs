using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace QRScan2PLC.Utils
{
    public class Acting_ProcessValueTextColorConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int RangeLow = 0;
            if (values[0] != null)
                RangeLow = (int)values[0];
            int RangeHigh = 0;
            if (values[1] != null)
                RangeHigh = (int)values[1];
            float ActValueFloat = 0;
            if (values[2] != null)
                ActValueFloat = (float)values[2];

            int ActValue = (int)Math.Round(ActValueFloat);
            if (ActValue < RangeLow)
                return Color.Red;
            if (ActValue > RangeHigh)
                return Color.Red;
            return Color.White;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
