using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class SetDataInputFactory : IPropertiesFactory
    {
        private SetDataInput setDataInput;

        public SetDataInputFactory(JObject properties)
        {
            setDataInput = new SetDataInput(properties);
        }

        public SetDataInputFactory(string description,string uom)
        {
            setDataInput = new SetDataInput(description,uom);
        }

        public IProperties GetProperties()
        {
            return setDataInput;
        }
    }
}
