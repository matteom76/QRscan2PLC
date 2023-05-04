using DeviceCommunication.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Interfaces
{
    public interface IAddressType
    {

        DataType Type { get; set; }

        IAddressInteger GetAddressInteger();
        IAddressReal GetAddressReal();
        IAddressBoolean GetAddressBoolean();
        IAddressString GetAddressString();
    }
}
