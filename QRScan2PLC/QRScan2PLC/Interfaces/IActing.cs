using Newtonsoft.Json.Linq;
using QRScan2PLC.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Interfaces
{
    public interface IActing
    {
        ActingType Type { get; set; }
        IProperties Property { get; set; }
        JObject GetJsonObject();

    }
}
