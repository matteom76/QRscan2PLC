using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class DisplayStatusFactory : IPropertiesFactory
    {
        private DisplayStatus displayStatus;

        public DisplayStatusFactory(JObject properties)
        {
            displayStatus = new DisplayStatus(properties);
        }

        public DisplayStatusFactory()
        {
            displayStatus = new DisplayStatus();
        }

        public IProperties GetProperties()
        {
            return displayStatus;
        }
    }
}
