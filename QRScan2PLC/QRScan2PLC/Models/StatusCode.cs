using Newtonsoft.Json.Linq;
using QRScan2PLC.Utils;
using System;

namespace QRScan2PLC.Models
{
    public class StatusCode: Notifier
    {
        private int _value;
        private string _description;



        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }


        public string Description {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }

        public StatusCode(int value, string description)
        {
            Value = value;
            Description = description;
        }


        public StatusCode(JObject status)
        {
            try
            {
                Value = int.Parse(status["value"].ToString());
                Description = status["description"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing status code object. More details:" + ex.ToString());
            }
        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("value", Value),
                    new JProperty("description", Description)
                );

            return obj;
        }

    }
}