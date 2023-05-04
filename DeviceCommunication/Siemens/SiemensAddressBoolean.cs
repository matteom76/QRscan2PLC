using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensAddressBoolean : Notifier,IAddressBoolean
    {
        private int db;
        private int bytevalue;
        private int bit;
        private ICommunicationDevice _communicationDevice;

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

        public int Bit
        {
            get { return bit; }
            set
            {
                bit = value;
            }
        }

        private bool _ActualValue;
        public bool ActualValue { get { return _ActualValue; }  set { _ActualValue = value;OnPropertyChanged("ActualValue"); } }

        private bool _writevalue;
        public bool WriteValue { get { return _writevalue; } set { _writevalue = value; OnPropertyChanged("WriteValue"); } }

        private bool _Error;
        public bool Error { get { return _Error; } set { _Error = value; OnPropertyChanged("Error"); } }


        public SiemensAddressBoolean()
        {
            DB = 0;
            Byte = 0;
            Bit = 0;
        }

        public SiemensAddressBoolean(int db,int byteadr, int bit)
        {
            DB = db;
            Byte = byteadr;
            Bit = bit;
        }

        public SiemensAddressBoolean(JObject adrData)
        {
            try
            {
                DB = int.Parse(adrData["DB"].ToString());
                Byte = int.Parse(adrData["byte"].ToString());
                Bit = int.Parse(adrData["bit"].ToString());
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing PLC address of parameter. More details:" + ex.ToString());
            }
        }

        public SiemensAddressBoolean(JObject adrData,ICommunicationDevice communicationDevice)
        {

            _communicationDevice = communicationDevice;
            try
            {
                DB = int.Parse(adrData["DB"].ToString());
                Byte = int.Parse(adrData["byte"].ToString());
                Bit = int.Parse(adrData["bit"].ToString());
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
                    new JProperty("byte", Byte),
                    new JProperty("bit", Bit)
                );
            return obj;
        }
    }
}
