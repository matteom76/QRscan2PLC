using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Interfaces
{
    public interface IDataConnection
    {
        JObject GetJsonObject();
    }
}
