using Newtonsoft.Json.Linq;
using QRScan2PLC.Enums;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class Button : IProperties
    {
        private ButtonMode _buttonMode;
        private string _label;

        public ButtonMode ButtonMode { get { return _buttonMode; } set { _buttonMode = value; } }
        public string Label { get { return _label; } set { _label = value; } }

        public Button(JObject properties)
        {
            try
            {
                ButtonMode = (ButtonMode)System.Enum.Parse(typeof(ButtonMode), properties["mode"].ToString(), true);
                Label = properties["label"].ToString();
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing properties button. More details:" + ex.ToString());
            }
        }

        public Button(ButtonMode buttonMode, string label)
        {
            ButtonMode = buttonMode;
            Label = label;
        }


        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("mode", ButtonMode.ToString()),
                    new JProperty("label", Label)
                );

            return obj;
        }
    }
}
