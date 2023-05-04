using DeviceCommunication.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCommunication.MQTT
{
    public class MQTTDataConnection : IDataConnection
    {
        private string _ClientID;
        private string _TcpServer_Name;
        private int _TcpServer_Port;
        private bool _WithCredentials;
        private bool _WithTls;
        private string _Credentials_Name;
        private string _Credentials_Password;

        public string ClientID { get => _ClientID; set => _ClientID = value; }
        public string TcpServer_Name { get => _TcpServer_Name; set => _TcpServer_Name = value; }
        public int TcpServer_Port { get => _TcpServer_Port; set => _TcpServer_Port = value; }
        public bool WithCredentials { get => _WithCredentials; set => _WithCredentials = value; }
        public bool WithTls { get => _WithTls; set => _WithTls = value; }
        public string Credentials_Name { get => _Credentials_Name; set => _Credentials_Name = value; }
        public string Credentials_Password { get => _Credentials_Password; set => _Credentials_Password = value; }

        public MQTTDataConnection(JObject DataConnectionInfo)
        {
            try
            {
                ClientID = DataConnectionInfo["ClientID"].ToString();
                TcpServer_Name = DataConnectionInfo["TcpServer_Name"].ToString();
                TcpServer_Port = int.Parse(DataConnectionInfo["TcpServer_Port"].ToString());
                WithCredentials = DataConnectionInfo["WithCredentials"].ToString() == "True" ? true : false;
                WithTls = DataConnectionInfo["WithTls"].ToString() == "True" ? true : false;
                Credentials_Name = DataConnectionInfo["Credentials_Name"].ToString();
                Credentials_Password = DataConnectionInfo["Credentials_Password"].ToString();
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing MQTT data connection. More details:" + ex.ToString());
            }
        }

        public MQTTDataConnection()
        {
            Random rnd = new Random();
            ClientID = "ClientID_" + rnd.Next(1000).ToString();
            TcpServer_Name = "";
            TcpServer_Port = 1883;
            WithCredentials = false;
            WithTls = false;
            Credentials_Name = "";
            Credentials_Password = "";
        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("ClientID", ClientID),
                    new JProperty("TcpServer_Name", TcpServer_Name),
                    new JProperty("TcpServer_Port", TcpServer_Port),
                    new JProperty("WithCredentials", WithCredentials ? "True" : "False"),
                    new JProperty("WithTls", WithTls ? "True" : "False"),
                    new JProperty("Credentials_Name", Credentials_Name),
                    new JProperty("Credentials_Password", Credentials_Password)                    
                );
            return obj;
        }
    }
}
