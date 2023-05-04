using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceCommunication.MQTT
{
    public class MQTTDevice : Notifier, ICommunicationDevice
    {
        private const int QOS = 1;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object ListCacheLock = new object();

        private MQTTEnableFunctionsPLC functionsPLC;

        private bool _connectionstatus;
        public bool ConnectionStatus
        {
            get { return _connectionstatus; }
            set { _connectionstatus = value; OnPropertyChanged("ConnectionStatus"); }
        }

        private List<MQTTMonitoringTopic> TopicToMonitoring;
        private List<MQTTMonitoringTopic> TopicToMonitoringCache;
        private bool RefreshValueToMonitoringFlag = false;

        private ManagedMqttClientOptions options;
        private MqttClientOptionsBuilder builder;
        private IManagedMqttClient _mqttClient;


        public MQTTDevice(MQTTDataConnection dataConnection)
        {
            builder = new MqttClientOptionsBuilder()
                                        .WithClientId(dataConnection.ClientID)
                                        .WithTcpServer(dataConnection.TcpServer_Name, dataConnection.TcpServer_Port);

            if (dataConnection.WithCredentials)
                builder.WithCredentials(dataConnection.Credentials_Name, dataConnection.Credentials_Password);

            if (dataConnection.WithTls)
                builder.WithTls();


            // Create client options objects
            options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
                                    .WithClientOptions(builder.Build())
                                    .Build();

            // Creates the client object
            _mqttClient = new MqttFactory().CreateManagedMqttClient();

            // Set up handlers
            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;
                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        MQTTMonitoringTopic findTopic = TopicToMonitoring.Find(t => t.Topic == topic);
                        if (findTopic!=null)
                        {
                            if (findTopic.Address is MQTTEnableFunctionsPLC)
                                ValueReadSetting_EnableFunctionsPLC((MQTTEnableFunctionsPLC)findTopic.Address, payload);
                            else
                            if (findTopic.Address is MQTTAddressBoolean)
                                ValueReadSetting_Bool((MQTTAddressBoolean)findTopic.Address, payload);
                            else
                            if (findTopic.Address is MQTTAddressInteger)
                                ValueReadSetting_Integer((MQTTAddressInteger)findTopic.Address, payload);
                            else
                            if (findTopic.Address is MQTTAddressReal)
                                ValueReadSetting_Real((MQTTAddressReal)findTopic.Address, payload);
                            else
                            if (findTopic.Address is MQTTAddressString)
                                ValueReadSetting_String((MQTTAddressString)findTopic.Address, payload);
                        }                        
                    }

                    if (RefreshValueToMonitoringFlag)
                    {
                        List<string> unsubscribeTopicList = new List<string>();
                        foreach (MQTTMonitoringTopic MQTTTopic in TopicToMonitoring)
                        {
                            if (MQTTTopic.Address is not MQTTEnableFunctionsPLC)
                                unsubscribeTopicList.Add(MQTTTopic.Topic);
                        }

                        if (unsubscribeTopicList.Count > 0)
                            _mqttClient.UnsubscribeAsync(unsubscribeTopicList.ToArray());

                        TopicToMonitoring.RemoveAll(r => r.Address is not MQTTEnableFunctionsPLC);
                        RefreshValueToMonitoringFlag = false;
                    }

                    foreach (MQTTMonitoringTopic cacheValue in TopicToMonitoringCache)
                    {
                        if (!TopicToMonitoring.Contains(cacheValue))
                            TopicToMonitoring.Add(cacheValue);
                    }

                    TopicToMonitoringCache.Clear();

                }
                catch (Exception ex)
                {
                    log.Error("Error during message receiving. Details:" + ex.Message);
                }
            });



        }

        private void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            ConnectionStatus = false;
            log.Error("Connection establish fault. Details:" + obj.Exception.Message);
        }

        private void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            ConnectionStatus = false;
            log.Error("Connection establish error. The system retry to establish communication. Details:" + obj.Reason.ToString());
        }

        private void OnConnected(MqttClientConnectedEventArgs obj)
        {
            ConnectionStatus = true;
            if (TopicToMonitoring==null)
            {
                TopicToMonitoring = new List<MQTTMonitoringTopic>();
                TopicToMonitoringCache = new List<MQTTMonitoringTopic>();
                TopicToMonitoring.Add(new MQTTMonitoringTopic() { Topic = functionsPLC.Topic, Address = functionsPLC });
            }
            log.Info("MQTT Connection established.");
        }

        private async Task SubscribeAsync(string topic)
        {
            await _mqttClient.SubscribeAsync(new TopicFilterBuilder()
              .WithTopic(topic)
              .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)QOS)
              .Build());
        }

        public async Task Connect(IEnableFunctionsPLC addressEnableFunctionPLC)
        {
            _mqttClient.StartAsync(options).GetAwaiter().GetResult();
            functionsPLC = (MQTTEnableFunctionsPLC)addressEnableFunctionPLC;
            await SubscribeAsync(functionsPLC.Topic);
        }

        public void Disconnect()
        {
            _mqttClient.StopAsync();
        }

        private void ValueReadSetting_Bool(MQTTAddressBoolean addressBoolean, string value)
        {
            addressBoolean.ActualValue = (value == "true" || value == "True" || value == "1");
        }

        private void ValueReadSetting_EnableFunctionsPLC(MQTTEnableFunctionsPLC addressEnableFunctionsPLC, string value)
        {
            addressEnableFunctionsPLC.ActualValue = (value == "true" || value == "True" || value == "1");
        }

        private void ValueReadSetting_Integer(MQTTAddressInteger addressInteger, string value)
        {
            ushort valueConv;
            if (ushort.TryParse(value,out valueConv))
            {
                addressInteger.ActualValue = valueConv;
                addressInteger.Error = false;
            }
            else
                addressInteger.Error = true;
        }

        private void ValueReadSetting_Real(MQTTAddressReal addressReal, string value)
        {
            float valueConv;
            if (float.TryParse(value, out valueConv))
            {
                addressReal.ActualValue = valueConv;
                addressReal.Error = false;
            }
            else
                addressReal.Error = true;
        }


        private void ValueReadSetting_String(MQTTAddressString addressString, string value)
        {
            addressString.ActualValue = value;
        }


        public void MonitoringValueBool(IAddressBoolean addressBoolean)
        {            
            if (_mqttClient.IsConnected)
            { 
                MQTTAddressBoolean adrBool = (MQTTAddressBoolean)addressBoolean;
                Task SubscribeTask =  Task.Run(async () => { await SubscribeAsync(adrBool.Topic); });
                SubscribeTask.Wait();

                if (TopicToMonitoringCache != null)
                {
                    lock (ListCacheLock)
                    {
                        TopicToMonitoringCache.Add(new MQTTMonitoringTopic() { Topic = adrBool.Topic, Address = adrBool });
                    }

                }
            }
        }

        public void MonitoringValueInteger(IAddressInteger addressInteger)
        {
            if (_mqttClient.IsConnected)
            {
                MQTTAddressInteger adrInteger = (MQTTAddressInteger)addressInteger;
                Task SubscribeTask = Task.Run(async () => { await SubscribeAsync(adrInteger.Topic); });
                SubscribeTask.Wait();

                if (TopicToMonitoringCache != null)
                {
                    lock (ListCacheLock)
                    {
                        TopicToMonitoringCache.Add(new MQTTMonitoringTopic() { Topic = adrInteger.Topic, Address = adrInteger });
                    }

                }
            }
        }

        public void MonitoringValueReal(IAddressReal addressReal)
        {
            if (_mqttClient.IsConnected)
            {
                MQTTAddressReal adrReal = (MQTTAddressReal)addressReal;
                Task SubscribeTask = Task.Run(async () => { await SubscribeAsync(adrReal.Topic); });
                SubscribeTask.Wait();

                if (TopicToMonitoringCache != null)
                {
                    lock (ListCacheLock)
                    {
                        TopicToMonitoringCache.Add(new MQTTMonitoringTopic() { Topic = adrReal.Topic, Address = adrReal });
                    }

                }
            }
        }

        public void MonitoringValueString(IAddressString addressString)
        {
            if (_mqttClient.IsConnected)
            {
                MQTTAddressString adrString = (MQTTAddressString)addressString;
                Task SubscribeTask = Task.Run(async () => { await SubscribeAsync(adrString.Topic); });
                SubscribeTask.Wait();

                if (TopicToMonitoringCache != null)
                {
                    lock (ListCacheLock)
                    {
                        TopicToMonitoringCache.Add(new MQTTMonitoringTopic() { Topic = adrString.Topic, Address = adrString });
                    }

                }
            }
        }

        public void RefreshValueToMonitoring()
        {
            RefreshValueToMonitoringFlag = true;

            lock (ListCacheLock)
            {
                if (TopicToMonitoringCache != null)
                    TopicToMonitoringCache.Clear();
            }
        }

        public void StopScanningForcing()
        {
            _mqttClient.StopAsync();
            _mqttClient.Dispose();
        }

        public async Task WriteBool(IAddressBoolean addressBoolean, bool value)
        {
            if (_mqttClient.IsConnected)
            {
                MQTTAddressBoolean adr = (MQTTAddressBoolean)addressBoolean;
                await _mqttClient.PublishAsync(adr.Topic, value.ToString());
                addressBoolean.Error = false;
            }
            else
            {
                addressBoolean.Error = true;
            }
        }

        public async Task WriteInteger(IAddressInteger addressInteger, ushort value)
        {
            if (_mqttClient.IsConnected)
            {
                MQTTAddressInteger adr = (MQTTAddressInteger)addressInteger;
                await _mqttClient.PublishAsync(adr.Topic, value.ToString());
                addressInteger.Error = false;
            }
            else
            {
                addressInteger.Error = true;
            }
        }

        public async Task WriteReal(IAddressReal addressReal, float value)
        {
            if (_mqttClient.IsConnected)
            {
                MQTTAddressReal adr = (MQTTAddressReal)addressReal;
                await _mqttClient.PublishAsync(adr.Topic, value.ToString());
                addressReal.Error = false;
            }
            else
            {
                addressReal.Error = true;
            }
        }

        public async Task WriteString(IAddressString addressString, string value)
        {
            if (_mqttClient.IsConnected)
            {
                MQTTAddressString adr = (MQTTAddressString)addressString;
                await _mqttClient.PublishAsync(adr.Topic, value);
                addressString.Error = false;
            }
            else
            {
                addressString.Error = true;
            }
        }
    }
}
