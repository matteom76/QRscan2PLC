using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Interfaces
{
    public interface IAddressString
    {
        JObject GetJsonObject();
        public string ActualValue { get;  }
        public bool Error { get; set; }
        public string WriteValue { get; set; }
        bool CheckStringLenght(string StringToCheck);
    }
}
