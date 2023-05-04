using DeviceCommunication;
using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using DeviceCommunication.MQTT;
using DeviceCommunication.Siemens;
using Newtonsoft.Json.Linq;
using QRScan2PLC.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models
{
    public class DataPLC
    {
        private String _name;
        private String _description;
        private DataType _type;

        public DeviceTag DeviceAdr { get; set; }
        public Acting Acting { get; set; }
        
        public PLCModel DeviceCommModel { get; set; }

        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public String Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        public DataType Type
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }


        public DataPLC(ActingType actingType, DataType dataType,PLCModel model)
        {
            Name = "par01";
            Description = "";
            Type = dataType;
            IAddressType DeviceCommAddress;
            DeviceCommModel = model;

            if (model==PLCModel.MQTT)
                DeviceCommAddress = new MQTTAddressType(dataType);            
            else
                DeviceCommAddress = new SiemensAddressType(dataType);

            DeviceAdr = new DeviceTag(DeviceCommAddress);
            Acting = new Acting(actingType, dataType);
        }

        public DataPLC(JObject JSonParObj, PLCModel model)
        {
            try
            {
                Name = JSonParObj["name"].ToString();
                Description = JSonParObj["description"].ToString();
                Type = (DataType)System.Enum.Parse(typeof(DataType), JSonParObj["type"].ToString(), true);
                Acting = new Acting((JObject)JSonParObj["acting"], Type);
                IAddressType DeviceCommAddress;
                DeviceCommModel = model;

                if (model == PLCModel.MQTT)
                    DeviceCommAddress = new MQTTAddressType((JObject)JSonParObj["PLCAdress"], Type);
                else
                    DeviceCommAddress = new SiemensAddressType((JObject)JSonParObj["PLCAdress"], Type);
                DeviceAdr = new DeviceTag(DeviceCommAddress);

            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing data PLC " + Name + ". More details:" + ex.ToString());
            }
        }

        public JObject GetJsonParameter()
        {

            JObject JsonPlcTag = DeviceAdr.GetJsonObject();
            JObject JsonActing = Acting.GetJsonObject();


            JObject obj =
                new JObject(
                    new JProperty("name", Name),
                    new JProperty("description", Description),
                    new JProperty("type", Type),
                    new JProperty("acting", JsonActing),
                    new JProperty("PLCAdress", JsonPlcTag)
                );

            return obj;
        }
    }

}
