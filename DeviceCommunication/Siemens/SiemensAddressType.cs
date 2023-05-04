using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensAddressType : IAddressType
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

        public SiemensAddressType(DataType type)
        {
            switch (type)
            {
                case DataType.Boolean:
                    AddressBoolean = new SiemensAddressBoolean();
                    break;
                case DataType.Integer:
                    AddressInteger = new SiemensAddressInteger();
                    break;
                case DataType.Real:
                    AddressReal = new SiemensAddressReal();
                    break;
                case DataType.String:
                    AddressString = new SiemensAddressString();
                    break;
            }
            Type = type;
        }

        public SiemensAddressType(JObject adrData, DataType type)
        {
            switch (type)
            {
                case DataType.Boolean:
                    AddressBoolean = new SiemensAddressBoolean(adrData);
                    break;
                case DataType.Integer:
                    AddressInteger = new SiemensAddressInteger(adrData);
                    break;
                case DataType.Real:
                    AddressReal = new SiemensAddressReal(adrData);
                    break;
                case DataType.String:
                    AddressString = new SiemensAddressString(adrData);
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
