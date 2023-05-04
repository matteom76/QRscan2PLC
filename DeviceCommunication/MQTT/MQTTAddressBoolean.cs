using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceCommunication.MQTT
{
    public class MQTTAddressBoolean : Notifier, IAddressBoolean
    {
        private bool _ActualValue;
        public bool ActualValue { get { return _ActualValue; } set { _ActualValue = value; OnPropertyChanged("ActualValue"); } }

        private bool _writevalue;
        public bool WriteValue { get { return _writevalue; } set { _writevalue = value; OnPropertyChanged("WriteValue"); } }

        private bool _Error;
        public bool Error { get { return _Error; } set { _Error = value; OnPropertyChanged("Error"); } }

        private string _Topic;
        public string Topic {
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

        public MQTTAddressBoolean()
        {
            Topic = "";
        }

        public MQTTAddressBoolean(string topic)
        {
            Topic = topic;
        }


        public MQTTAddressBoolean(JObject adrData)
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
