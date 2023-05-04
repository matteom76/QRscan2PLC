using DeviceCommunication.Enums;
using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    class SetDataFromCodeFactory : IPropertiesFactory
    {
        private SetDataFromCode setDataFromCode;

        public SetDataFromCodeFactory(JObject properties)
        {
            setDataFromCode = new SetDataFromCode(properties);
        }

        public SetDataFromCodeFactory(int positionStart, int lenght,int decimalNumber,bool visible,DataType dataType)
        {
            setDataFromCode = new SetDataFromCode(positionStart, lenght, decimalNumber, visible,dataType);
        }

        public IProperties GetProperties()
        {
            return setDataFromCode;
        }
    }
}
