using DeviceCommunication.Enums.Siemens;
using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensDataConnection : IDataConnection
    {

        private string _ip;
        private short _rack;
        private short _slot;
        private SiemensPLCType _PLCType;

        public string IP
        {
            get { return _ip; }
            set
            {
                _ip = value;
            }
        }

        public short Rack
        {
            get { return _rack; }
            set
            {
                _rack = value;
            }
        }

        public short Slot
        {
            get { return _slot; }
            set
            {
                _slot = value;
            }
        }

        public SiemensPLCType PLCType {
            get { return _PLCType; }
            set { _PLCType = value; }
        }

        public SiemensDataConnection(JObject DataConnectionInfo)
        {
            try
            {
                IP = DataConnectionInfo["IP"].ToString();
                Rack = short.Parse(DataConnectionInfo["Rack"].ToString());
                Slot = short.Parse(DataConnectionInfo["Slot"].ToString());
                PLCType = (SiemensPLCType)System.Enum.Parse(typeof(SiemensPLCType), DataConnectionInfo["PLCType"].ToString(), true);
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing PLC data connection. More details:" + ex.ToString());
            }
        }

        public SiemensDataConnection()
        {
            IP = "0.0.0.0";
            Rack = 0;
            Slot = 0;
            PLCType = SiemensPLCType.S71500;
        }

        public SiemensDataConnection(string ip,short rack,short slot,SiemensPLCType plcType)
        {
            IP = ip;
            Rack = rack;
            Slot = slot;
            PLCType = plcType;
        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("IP", IP),
                    new JProperty("Rack", Rack),
                    new JProperty("Slot", Slot),
                    new JProperty("PLCType", PLCType)
                );

            return obj;
        }
    }
}
