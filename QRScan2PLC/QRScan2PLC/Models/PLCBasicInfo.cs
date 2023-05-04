using DeviceCommunication.Enums;
using DeviceCommunication.Enums.Siemens;
using DeviceCommunication.Interfaces;
using DeviceCommunication.MQTT;
using DeviceCommunication.Siemens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models
{
    public class PLCBasicInfo
    {
        private PLCModel _model;

        public PLCModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
            }
        }

        public IDataConnection DataConnection { get; set; }
        private IDataConnectionFactory connectionFactory;
        public IEnableFunctionsPLC EnableFunctions { get; set; }
        private IEnableFunctionsPLCFactory enableFunctionsPLCFactory;

        public PLCBasicInfo(JObject stationConfData)
        {
            try
            {             
                Model = (PLCModel)System.Enum.Parse(typeof(PLCModel), stationConfData["Model"].ToString(), true);
                switch (Model)
                {
                    case (PLCModel.Siemens):
                        connectionFactory = new SiemensDataConnectionFactory((JObject)stationConfData["DataConnection"]);
                        DataConnection = connectionFactory.GetDataConnection();
                        enableFunctionsPLCFactory = new SiemensEnableFunctionsPLCFactory((JObject)stationConfData["EnableFunctions"]);
                        EnableFunctions = enableFunctionsPLCFactory.GetEnableFunctionPLC();
                        break;
                    case (PLCModel.MQTT):
                        connectionFactory = new MQTTDataConnectionFactory((JObject)stationConfData["DataConnection"]);
                        DataConnection = connectionFactory.GetDataConnection();
                        enableFunctionsPLCFactory = new MQTTEnableFunctionsPLCFactory((JObject)stationConfData["EnableFunctions"]);
                        EnableFunctions = enableFunctionsPLCFactory.GetEnableFunctionPLC();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing PLC Basic info object. More details:" + ex.ToString());
            }
        }

        public PLCBasicInfo(PLCModel model)
        {
            Model = model;
            
            if (model==PLCModel.Siemens)
            { 
                connectionFactory = new SiemensDataConnectionFactory();
                DataConnection = connectionFactory.GetDataConnection();
                enableFunctionsPLCFactory = new SiemensEnableFunctionsPLCFactory();
                EnableFunctions = enableFunctionsPLCFactory.GetEnableFunctionPLC();
            }
            else
            if (model==PLCModel.MQTT)
            {
                connectionFactory = new MQTTDataConnectionFactory();
                DataConnection = connectionFactory.GetDataConnection();
                enableFunctionsPLCFactory = new MQTTEnableFunctionsPLCFactory();
                EnableFunctions = enableFunctionsPLCFactory.GetEnableFunctionPLC();
            }
        }

        public JObject GetJsonPLCBasicInfo()
        {

            JObject obj =
                new JObject(
                    new JProperty("Model", Model.ToString()),
                    new JProperty("DataConnection", (JObject)DataConnection.GetJsonObject()),
                    new JProperty("EnableFunctions", (JObject)EnableFunctions.GetJsonObject())
                );

            return obj;
        }
    }
}
