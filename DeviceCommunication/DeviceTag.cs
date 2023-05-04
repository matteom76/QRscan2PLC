using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication
{
    public class DeviceTag
    {
        public IAddressBoolean addressBoolean { get; set; }
        public IAddressInteger addressInteger { get; set; }
        public IAddressReal addressReal { get; set; }
        public IAddressString addressString { get; set; }
        public DataType type;

        public DeviceTag(IAddressType addressType)
        {
            addressBoolean = addressType.GetAddressBoolean();
            addressInteger = addressType.GetAddressInteger();
            addressReal = addressType.GetAddressReal();
            addressString = addressType.GetAddressString();
            type = addressType.Type;
        }

        public IAddressBoolean AddressBoolean()
        {
            return addressBoolean;
        }

        public IAddressInteger AddressInteger()
        {
            return addressInteger;
        }

        public IAddressReal AddressReal()
        {
            return addressReal;
        }

        public IAddressString AddressString()
        {
            return addressString;
        }

        public JObject GetJsonObject()
        {
            switch (type)
            {
                case DataType.Boolean:
                    return addressBoolean.GetJsonObject();
                case DataType.Integer:
                    return addressInteger.GetJsonObject();
                case DataType.Real:
                    return addressReal.GetJsonObject();
                case DataType.String:
                    return addressString.GetJsonObject();
            }
            return null;
        }
    }
}
