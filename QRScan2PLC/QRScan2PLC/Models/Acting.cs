using DeviceCommunication.Enums;
using Newtonsoft.Json.Linq;
using QRScan2PLC.Enums;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models
{
    public class Acting : IActing
    {
        private ActingType _actingType;
        private IPropertiesFactory propertiesFactory;

        public ActingType Type { get { return _actingType; } set { _actingType = value; } }
        
        public IProperties Property { get; set; }

        public Acting(JObject acting,DataType dataType)
        {
            try
            {
                Type = (ActingType)System.Enum.Parse(typeof(ActingType), acting["type"].ToString(), true);
                
                switch(Type)
                {
                    case (ActingType.Button):
                        propertiesFactory = new ButtonFactory((JObject)acting["properties"]);
                        break;
                    case (ActingType.DisplayStatus):
                        propertiesFactory = new DisplayStatusFactory((JObject)acting["properties"]);
                        break;
                    case ActingType.SetDataFixed:
                        if (dataType == DataType.Real)
                            propertiesFactory = new SetDataFixedFactory<double>((JObject)acting["properties"]);
                        else
                            if (dataType == DataType.Integer)
                                propertiesFactory = new SetDataFixedFactory<int>((JObject)acting["properties"]);
                            else
                                if (dataType == DataType.String)
                                    propertiesFactory = new SetDataFixedFactory<string>((JObject)acting["properties"]);
                                else
                                    if (dataType == DataType.Boolean)
                                        propertiesFactory = new SetDataFixedFactory<bool>((JObject)acting["properties"]);
                        break;
                    case ActingType.SetDataInput:
                        propertiesFactory = new SetDataInputFactory((JObject)acting["properties"]);
                        break;
                    case ActingType.SetDataFromCode:
                        propertiesFactory = new SetDataFromCodeFactory((JObject)acting["properties"]);
                        break;
                    case ActingType.DisplayValue:
                        propertiesFactory = new DisplayValueFactory((JObject)acting["properties"]);
                        break;
                    case ActingType.DisplayBargraph:
                        propertiesFactory = new DisplayBargraphFactory((JObject)acting["properties"]);
                        break;
                    default:
                        break;

                }

                Property = propertiesFactory.GetProperties();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing acting object. More details:" + ex.ToString());
            }
        }

        public Acting(ActingType type, DataType dataType)
        {
            Type = type;
            switch (Type)
            {
                case (ActingType.Button):
                    propertiesFactory = new ButtonFactory(ButtonMode.SetBit, "label");
                    break;
                case (ActingType.DisplayStatus):
                    propertiesFactory = new DisplayStatusFactory();
                    break;
                case ActingType.SetDataFixed:
                    if (dataType == DataType.Real)
                        propertiesFactory = new SetDataFixedFactory<double>(true);
                    else
                        if (dataType == DataType.Integer)
                        propertiesFactory = new SetDataFixedFactory<int>(true);
                    else
                        if (dataType == DataType.String)
                        propertiesFactory = new SetDataFixedFactory<string>(true);
                    else
                        if (dataType == DataType.Boolean)
                        propertiesFactory = new SetDataFixedFactory<bool>(true);
                    break;
                case ActingType.SetDataInput:
                    propertiesFactory = new SetDataInputFactory("DataInput", "");
                    break;
                case ActingType.SetDataFromCode:
                    propertiesFactory = new SetDataFromCodeFactory(0, 0, 0, true,dataType);
                    break;
                case ActingType.DisplayValue:
                    propertiesFactory = new DisplayValueFactory("DisplayValue", "");
                    break;
                case ActingType.DisplayBargraph:
                    propertiesFactory = new DisplayBargraphFactory(ColorsBargraph.Blue, "", 0, 100);
                    break;
                default:
                    break;

            }

            Property = propertiesFactory.GetProperties();            

        }


        
        public JObject GetJsonObject()
        {

            JObject propertiesJsonObj = Property.GetJsonObject();


            JObject obj =
                new JObject(
                    new JProperty("type", Type.ToString()),
                    new JProperty("properties", propertiesJsonObj)
                );

            return obj;
        }
    }
}
