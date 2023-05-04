using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.MQTT
{
    public class MQTTDataConnectionFactory : IDataConnectionFactory
    {

        private MQTTDataConnection dataConnection;

        public MQTTDataConnectionFactory()
        {
            dataConnection = new MQTTDataConnection();
        }

        public MQTTDataConnectionFactory(JObject JSonDataConnection)
        {
            dataConnection = new MQTTDataConnection(JSonDataConnection);
        }

        public IDataConnection GetDataConnection()
        {
            return dataConnection;
        }
    }
}
