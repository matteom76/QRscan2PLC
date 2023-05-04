using Newtonsoft.Json.Linq;
using QRScan2PLC.Enums;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class DisplayBargraph : IProperties
    {
        private string _uom;
        public string UOM { get { return _uom; } set { _uom = value; } }

        private int _rangelow;
        public int RangeLow { get { return _rangelow; }set { _rangelow = value; } }

        private int _rangehigh;
        public int RangeHigh { get { return _rangehigh; } set { _rangehigh = value; } }

        private ColorsBargraph _colorBargraph;
        public ColorsBargraph ColorBargraph { get { return _colorBargraph; } set { _colorBargraph = value; } }

        public DisplayBargraph(JObject properties)
        {
            try
            {
                ColorBargraph = (ColorsBargraph)System.Enum.Parse(typeof(ColorsBargraph), properties["color"].ToString(), true);
                UOM = properties["UOM"].ToString();
                RangeHigh = int.Parse(properties["RangeHigh"].ToString());
                RangeLow = int.Parse(properties["RangeLow"].ToString());
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing properties bargraph display. More details:" + ex.ToString());
            }
        }

        public DisplayBargraph(ColorsBargraph color,string uom,int rangeLow,int rangeHigh)
        {
            ColorBargraph = color;
            UOM = uom;
            RangeLow = rangeLow;
            RangeHigh = rangeHigh;
        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("color", ColorBargraph.ToString()),
                    new JProperty("UOM", UOM),
                    new JProperty("RangeLow", RangeLow),
                    new JProperty("RangeHigh", RangeHigh)
                );

            return obj;
        }
    }
}
