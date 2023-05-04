using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Interfaces
{
    public interface IAddressBoolean
    {
        JObject GetJsonObject();
        public bool ActualValue { get;  }
        public bool Error { get;  set; }

        public bool WriteValue { get; set; }
    }
}
