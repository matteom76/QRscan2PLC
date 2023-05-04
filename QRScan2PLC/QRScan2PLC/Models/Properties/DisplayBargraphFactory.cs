using Newtonsoft.Json.Linq;
using QRScan2PLC.Enums;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class DisplayBargraphFactory : IPropertiesFactory
    {
        private DisplayBargraph _displayBargraph;

        public DisplayBargraphFactory(JObject properties)
        {
            _displayBargraph = new DisplayBargraph(properties);
        }

        public DisplayBargraphFactory(ColorsBargraph color, string uom, int rangeLow, int rangeHigh)
        {
            _displayBargraph = new DisplayBargraph(color, uom, rangeLow, rangeHigh);
        }

        public IProperties GetProperties()
        {
            return _displayBargraph;
        }
    }
}
