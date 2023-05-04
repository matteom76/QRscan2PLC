
using DeviceCommunication.Enums;
using Newtonsoft.Json.Linq;
using System;


namespace QRScan2PLC.Models
{

    public class QRScanDevice
    {

        private String _filename;

        public String FileName
        {
            get { return _filename; }
            set
            {
                _filename = value;
            }
        }
        public ConfDetails ConfDetails { get; set; }


        public QRScanDevice(JObject DeviceJsonData)
        {
            try
            {
                FileName = DeviceJsonData["FileName"].ToString();
                ConfDetails = new ConfDetails((JObject)DeviceJsonData["ConfDetails"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing device configuration. More details:" + ex.ToString());
            }
        }

        public QRScanDevice(string filename,PLCModel model)
        {
            FileName = filename;
            ConfDetails = new ConfDetails(model);
        }



        public JObject GetJsonQRScanDevice()
        {
            JObject obj =
                new JObject(
                    new JProperty("FileName", FileName),
                    new JProperty("ConfDetails", (JObject)ConfDetails.GetJsonConfDetails())
                );

            return obj;
        }

    }
}
