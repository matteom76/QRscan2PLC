using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Interfaces
{
    public interface IProperties
    {
        JObject GetJsonObject();
    }
}
