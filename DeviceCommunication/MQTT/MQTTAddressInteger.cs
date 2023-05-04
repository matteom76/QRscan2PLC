using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceCommunication.MQTT
{
    public class MQTTAddressInteger : Notifier, IAddressInteger
    {
        private ushort _actualValue;
        public ushort ActualValue
        {
            get { return _actualValue; }
            set { _actualValue = value; OnPropertyChanged("ActualValue"); }
        }

        private ushort _writevalue;
        public ushort WriteValue
        {
            get { return _writevalue; }
            set { _writevalue = value; OnPropertyChanged("WriteValue"); }
        }

        private bool _Error;
        public bool Error
        {
            get { return _Error; }
            set { _Error = value; OnPropertyChanged("Error"); }
        }

        private string _Topic;
        public string Topic
        {
            get { return _Topic; }
            set
            {
                char[] charsToExclude = { '$', '#', '+' };
                if (charsToExclude.Any(ch => value.Contains(ch)))
                {
                    _Topic = "";
                    throw new Exception("Topic can't contains special char like $,#,+");

                }
                else
                    _Topic = value;
            }

        }

        public MQTTAddressInteger()
        {
            Topic = "";
        }

        public MQTTAddressInteger(string topic)
        {
            Topic = topic;
        }


        public MQTTAddressInteger(JObject adrData)
        {
            try
            {
                Topic = adrData["Topic"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing MQTT address of parameter. More details:" + ex.ToString());
            }
        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("Topic", Topic)
                );
            return obj;
        }
    }
}
