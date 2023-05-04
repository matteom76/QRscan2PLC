using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class SetDataFixed<T> : IProperties
    {
        private T _value;
        private bool _visible;
        public T Value { get { return _value; } set { _value = value; } }
        public bool Visible { get => _visible; set => _visible = value; }

        public SetDataFixed(JObject properties)
        {
            try
            {
                Visible = properties["visible"].ToString() == "True" ? true : false;
                Type valueType = typeof(T);
                if (valueType==typeof(int))
                {
                    int valueInt = int.Parse(properties["value"].ToString());
                    Value = (T)Convert.ChangeType(valueInt, typeof(T));
                }
                else
                    if (valueType==typeof(double))
                    {
                        double valueDouble = double.Parse(properties["value"].ToString());
                        Value = (T)Convert.ChangeType(valueDouble, typeof(T));
                    }
                    else
                        if (valueType == typeof(string))
                        {
                            Value = (T)Convert.ChangeType(properties["value"].ToString(), typeof(T));
                        }
                        else                        
                            if (valueType == typeof(bool))
                            {
                                bool valuebool = bool.Parse(properties["value"].ToString());
                                Value = (T)Convert.ChangeType(valuebool, typeof(T));
                            }
                        
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing properties set data fixed. More details:" + ex.ToString());
            }

        }

        public SetDataFixed(bool visible)
        {
            
            try
            {
                Visible = visible;
                Type valueType = typeof(T);
                if (valueType == typeof(int))
                {
                    int valueInt = 0;
                    Value = (T)Convert.ChangeType(valueInt, typeof(T));
                }
                else
                    if (valueType == typeof(double))
                    {
                        double valueDouble = 0.0;
                        Value = (T)Convert.ChangeType(valueDouble, typeof(T));
                    }
                else
                    if (valueType == typeof(string))
                    {
                        Value = (T)Convert.ChangeType("", typeof(T));
                    }
                else
                    if (valueType == typeof(bool))
                    {
                        bool valuebool = false;
                        Value = (T)Convert.ChangeType(valuebool, typeof(T));
                    }
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing properties set data fixed. More details:" + ex.ToString());
            }

        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("value", Value),
                    new JProperty("visible", Visible ? "True" : "False")
                );

            return obj;
        }
    }
}
