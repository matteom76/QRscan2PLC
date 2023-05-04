using DeviceCommunication.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace QRScan2PLC.Models
{
    public class QRTypes
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public QRTypeCondition Condition { get; set; }

        public ObservableCollection<DataPLC> DataList { get; set; }

        public QRTypes(JObject JSonQRTypes, PLCModel model)
        {
            Name = JSonQRTypes["name"].ToString();
            Condition = new QRTypeCondition((JObject)JSonQRTypes["condition"]);
            DataList = new ObservableCollection<DataPLC>();
            foreach (JObject Parameter in JSonQRTypes["data"])
            {
                DataList.Add(new DataPLC(Parameter,model));
            }
        }

        public QRTypes()
        {
            Name = "Type1";
            Condition = new QRTypeCondition();
            DataList = new ObservableCollection<DataPLC>();
        }

        public QRTypes(String name)
        {
            Name = name;
            Condition = new QRTypeCondition();
            DataList = new ObservableCollection<DataPLC>();
        }

        public JObject GetJsonQRTypes()
        {
            JArray JDataList = new JArray();

            foreach (DataPLC data in DataList)
            {
                JDataList.Add(data.GetJsonParameter());
            }

            JObject obj =
                new JObject(
                    new JProperty("name", (String)Name),
                    new JProperty("condition", (JObject)Condition.GetJsonQRCondition()),
                    new JProperty("data", (JArray)JDataList)
                );

            return obj;
        }
    }
}
