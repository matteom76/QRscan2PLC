using DeviceCommunication.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace QRScan2PLC.Models
{
    public class ConfDetails
    {
        public PLCBasicInfo PLCBasicInfo { get; set; }
        public ObservableCollection<QRTypes> QRTypesList { get; set; }

        public ConfDetails(JObject DeviceJsonData)
        {
            try
            {
                PLCBasicInfo = new PLCBasicInfo((JObject)DeviceJsonData["PLCBasicInfo"]);
                QRTypesList = new ObservableCollection<QRTypes>();
                foreach (JObject QRtype in DeviceJsonData["QRTypes"])
                {
                    QRTypesList.Add(new QRTypes(QRtype,PLCBasicInfo.Model));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing BR device configuration. More details:" + ex.ToString());
            }
        }

        public ConfDetails(PLCModel model)
        {
            PLCBasicInfo = new PLCBasicInfo(model);
            QRTypesList = new ObservableCollection<QRTypes>();
        }



        public JObject GetJsonConfDetails()
        {

            JArray JsonQRTypesList = new JArray();

            foreach (QRTypes qrType in QRTypesList)
            {
                JsonQRTypesList.Add(qrType.GetJsonQRTypes());
            }

            JObject obj =
                new JObject(
                    new JProperty("PLCBasicInfo", (JObject)PLCBasicInfo.GetJsonPLCBasicInfo()),
                    new JProperty("QRTypes", (JArray)JsonQRTypesList)
                );

            return obj;
        }

    }
}
