using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensAddressString : Notifier,IAddressString
    {
        private int db;
        private int bytevalue;
        private int lenght;
        
        private string _ActualValue;
        public string ActualValue { get { return _ActualValue; } set { _ActualValue = value; OnPropertyChanged("ActualValue"); } }

        private string _writevalue;
        public string WriteValue { get { return _writevalue; } set { _writevalue = value; OnPropertyChanged("WriteValue"); } }

        private bool _Error;
        public bool Error { get { return _Error; } set { _Error = value; OnPropertyChanged("Error"); } }


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

        public int Lenght
        {
            get { return lenght; }
            set
            {
                lenght = value;
            }
        }

        public SiemensAddressString()
        {
            DB = 0;
            Byte = 0;
            Lenght = 0;
        }

        public SiemensAddressString(int db,int byteadr,int lenght)
        {
            DB = db;
            Byte = byteadr;
            Lenght = lenght;
        }

        public SiemensAddressString(JObject adrData)
        {
            try
            {
                DB = int.Parse(adrData["DB"].ToString());
                Byte = int.Parse(adrData["byte"].ToString());
                Lenght = int.Parse(adrData["lenght"].ToString());
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
                    new JProperty("lenght", Lenght)
                );
            return obj;
        }

        public bool CheckStringLenght(string StringToCheck)
        {
            return StringToCheck.Length <= Lenght;
        }
    }
}
