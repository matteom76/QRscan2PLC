using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Interfaces
{
    public interface IAddressInteger
    {
        JObject GetJsonObject();
        public ushort ActualValue { get; }
        public bool Error { get; set; }
        public ushort WriteValue { get; set; }

    }
}
