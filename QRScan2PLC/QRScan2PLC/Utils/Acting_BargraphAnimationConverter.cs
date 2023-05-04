using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace QRScan2PLC.Utils
{
    public class Acting_BargraphAnimationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int RangeLow = 0;
            if (values[0] != null)
                RangeLow = (int)values[0];
            int RangeHigh = 0;
            if (values[1] != null)
                RangeHigh = (int)values[1];
            float ActValue = 0;
            if (values[2] != null)
                ActValue = (float)values[2];

            int ValueResult = 0;
            if (RangeHigh > RangeLow)
            { 
                ValueResult = (int)Math.Round((Math.Abs(RangeLow) + ActValue) * 292 / (RangeHigh + Math.Abs(RangeLow)));
                if (ValueResult > 292)
                    ValueResult = 292;
                if (ValueResult < 0)
                    ValueResult = 0;
            }
            return ValueResult;
        
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
