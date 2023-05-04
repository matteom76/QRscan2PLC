using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Interfaces
{
    public interface IAddressReal
    {
        JObject GetJsonObject();
        public float ActualValue { get;  }
        public bool Error { get; set; }
        public float WriteValue { get; set; }

    }
}
