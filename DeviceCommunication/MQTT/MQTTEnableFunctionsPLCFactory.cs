using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.MQTT
{
    public class MQTTEnableFunctionsPLCFactory : IEnableFunctionsPLCFactory
    {
        private IEnableFunctionsPLC enableFunctionsPLC;

        public MQTTEnableFunctionsPLCFactory()
        {
            enableFunctionsPLC = new MQTTEnableFunctionsPLC();
        }

        public MQTTEnableFunctionsPLCFactory(JObject JSonEnableFunctionPLC)
        {
            enableFunctionsPLC = new MQTTEnableFunctionsPLC(JSonEnableFunctionPLC);
        }

        public IEnableFunctionsPLC GetEnableFunctionPLC()
        {
            return enableFunctionsPLC;
        }
    }
}
