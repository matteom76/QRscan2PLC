using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class DisplayValueFactory : IPropertiesFactory
    {
        private DisplayValue displayValue;

        public DisplayValueFactory(JObject properties)
        {
            displayValue = new DisplayValue(properties);
        }

        public DisplayValueFactory(string description,string uom)
        {
            displayValue = new DisplayValue(description,uom);
        }

        public IProperties GetProperties()
        {
            return displayValue;
        }
    }
}
