using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensEnableFunctionsPLCFactory : IEnableFunctionsPLCFactory
    {

        private IEnableFunctionsPLC enableFunctionsPLC;

        public SiemensEnableFunctionsPLCFactory()
        {
            enableFunctionsPLC = new SiemensEnableFunctionsPLC();
        }

        public SiemensEnableFunctionsPLCFactory(JObject JSonEnableFunctionPLC)
        {
            enableFunctionsPLC = new SiemensEnableFunctionsPLC(JSonEnableFunctionPLC);
        }

        public IEnableFunctionsPLC GetEnableFunctionPLC()
        {
            return enableFunctionsPLC;
        }
    }
}
