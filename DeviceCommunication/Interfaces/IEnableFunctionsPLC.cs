using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Interfaces
{
    public interface IEnableFunctionsPLC
    {
        JObject GetJsonObject();
        public bool ActualValue { get; }
        public bool Error { get; set; }

    }
}
