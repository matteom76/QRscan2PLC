using Newtonsoft.Json.Linq;
using QRScan2PLC.Enums;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class ButtonFactory : IPropertiesFactory
    {
        private Button button;

        public ButtonFactory(JObject properties)
        {
            button = new Button(properties);
        }

        public ButtonFactory(ButtonMode mode, string label)
        {
            button = new Button(mode, label);
        }


        public IProperties GetProperties()
        {
            return button;
        }
    }
}
