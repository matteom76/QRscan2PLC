using DeviceCommunication.Enums.Siemens;
using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.Siemens
{
    public class SiemensDataConnectionFactory : IDataConnectionFactory
    {

        private SiemensDataConnection dataConnection;

        public SiemensDataConnectionFactory()
        {
            dataConnection = new SiemensDataConnection();
        }

        public SiemensDataConnectionFactory(JObject JSonDataConnection)
        {
            dataConnection = new SiemensDataConnection(JSonDataConnection);
        }

        public IDataConnection GetDataConnection()
        {
            return dataConnection;
        }
    }
}
