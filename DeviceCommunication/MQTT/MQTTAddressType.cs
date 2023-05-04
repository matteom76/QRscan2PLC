using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.MQTT
{
    public class MQTTAddressType : IAddressType
    {
        private IAddressBoolean addressBoolean;
        private IAddressInteger addressInteger;
        private IAddressReal addressReal;
        private IAddressString addressString;

        private DataType type;
        public IAddressBoolean AddressBoolean
        {
            get { return addressBoolean; }
            set
            {
                addressBoolean = value;
            }
        }
        public IAddressInteger AddressInteger
        {
            get { return addressInteger; }
            set
            {
                addressInteger = value;
            }
        }
        public IAddressReal AddressReal
        {
            get { return addressReal; }
            set
            {
                addressReal = value;
            }
        }
        public IAddressString AddressString
        {
            get { return addressString; }
            set
            {
                addressString = value;
            }
        }

        public DataType Type
        {
            get { return type; }
            set
            {
                type = value;
            }
        }

        public MQTTAddressType(DataType type)
        {
            switch (type)
            {
                case DataType.Boolean:
                    AddressBoolean = new MQTTAddressBoolean();
                    break;
                case DataType.Integer:
                    AddressInteger = new MQTTAddressInteger();
                    break;
                case DataType.Real:
                    AddressReal = new MQTTAddressReal();
                    break;
                case DataType.String:
                    AddressString = new MQTTAddressString();
                    break;
            }
            Type = type;
        }

        public MQTTAddressType(JObject adrData, DataType type)
        {
            switch (type)
            {
                case DataType.Boolean:
                    AddressBoolean = new MQTTAddressBoolean(adrData);
                    break;
                case DataType.Integer:
                    AddressInteger = new MQTTAddressInteger(adrData);
                    break;
                case DataType.Real:
                    AddressReal = new MQTTAddressReal(adrData);
                    break;
                case DataType.String:
                    AddressString = new MQTTAddressString(adrData);
                    break;
            }
            Type = type;
        }

        public IAddressBoolean GetAddressBoolean()
        {
            return AddressBoolean;
        }

        public IAddressInteger GetAddressInteger()
        {
            return AddressInteger;
        }

        public IAddressReal GetAddressReal()
        {
            return AddressReal;
        }

        public IAddressString GetAddressString()
        {
            return AddressString;
        }
    }
}
