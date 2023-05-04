using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    class SetDataFixedFactory<T> : IPropertiesFactory
    {

        private SetDataFixed<T> setDataFixed;

        public SetDataFixedFactory(JObject properties)
        {
            setDataFixed = new SetDataFixed<T>(properties);
        }

        public SetDataFixedFactory(bool visible)
        {
            setDataFixed = new SetDataFixed<T>(visible);
        }


        public IProperties GetProperties()
        {
            return setDataFixed;
        }
    }
}
