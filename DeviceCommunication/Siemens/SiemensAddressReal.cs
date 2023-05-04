using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensAddressReal : Notifier,IAddressReal
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

        private float _ActualValue;
        public float ActualValue { get { return _ActualValue; } set { _ActualValue = value; OnPropertyChanged("ActualValue"); } }

        private float _writevalue;
        public float WriteValue { get { return _writevalue; } set { _writevalue = value; OnPropertyChanged("WriteValue"); } }

        private bool _Error;
        public bool Error { get { return _Error; } set { _Error = value; OnPropertyChanged("Error"); } }

        public SiemensAddressReal()
        {
            DB = 0;
            Byte = 0;
        }

        public SiemensAddressReal(int db,int byteadr)
        {
            DB = db;
            Byte = byteadr;
        }

        public SiemensAddressReal(JObject adrData)
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
