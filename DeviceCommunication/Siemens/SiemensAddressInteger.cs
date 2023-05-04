using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensAddressInteger : Notifier,IAddressInteger
    {
        private int db;
        private int bytevalue;

        public int DB
        {
            get { return db; }
            set
            {
                db = value; 
            }
        }

        public int Byte
        {
            get { return bytevalue; }
            set
            {
                bytevalue = value;
            }
        }

        private ushort _actualValue;
        public ushort ActualValue {
            get { return _actualValue; }
            set { _actualValue = value;OnPropertyChanged("ActualValue"); }
        }

        private ushort _writevalue;
        public ushort WriteValue
        {
            get { return _writevalue; }
            set { _writevalue = value; OnPropertyChanged("WriteValue"); }
        }

        private bool _Error;
        public bool Error {
            get { return _Error; }
            set { _Error = value;OnPropertyChanged("Error"); }
        }

        public SiemensAddressInteger()
        {
            DB = 0;
            Byte = 0;
        }

        public SiemensAddressInteger(int db,int byteadr)
        {
            DB = db;
            Byte = byteadr;
        }

        public SiemensAddressInteger(JObject adrData)
        {
            try
            {
                DB = int.Parse(adrData["DB"].ToString());
                Byte = int.Parse(adrData["byte"].ToString());
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing PLC address of parameter. More details:" + ex.ToString());
            }
        }


        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("DB", DB),
                    new JProperty("byte", Byte)
                );
            return obj;
        }
    }
}
