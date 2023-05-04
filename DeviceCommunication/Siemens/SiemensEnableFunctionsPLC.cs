using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensEnableFunctionsPLC : Notifier,IEnableFunctionsPLC
    {
        private int _db;
        private int _bytevalue;
        private int _bit;

        private bool _ActualValue;
        public bool ActualValue { get { return _ActualValue; } set { _ActualValue = value; OnPropertyChanged("ActualValue"); } }

        private bool _Error;
        public bool Error { get { return _Error; } set { _Error = value; OnPropertyChanged("Error"); } }

        public int DB
        {
            get { return _db; }
            set
            {
                _db = value;
            }
        }

        public int Byte
        {
            get { return _bytevalue; }
            set
            {
                _bytevalue = value;
            }
        }

        public int Bit
        {
            get { return _bit; }
            set
            {
                _bit = value;
            }
        }

        public SiemensEnableFunctionsPLC(JObject EnableCommData)
        {
            try
            {
                DB = int.Parse(EnableCommData["DB"].ToString());
                Byte = int.Parse(EnableCommData["byte"].ToString());
                Bit = int.Parse(EnableCommData["bit"].ToString());

            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing EnableComm informations. More details:" + ex.ToString());
            }
        }

        public SiemensEnableFunctionsPLC()
        {
            DB = 1;
            Byte = 0;
            Bit = 0;
        }

        public SiemensEnableFunctionsPLC(int db,int byteadr,int bit)
        {
            DB = db;
            Byte = byteadr;
            Bit = bit;
        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("DB", DB),
                    new JProperty("byte", Byte),
                    new JProperty("bit", Bit)
                );

            return obj;
        }
    }
}
