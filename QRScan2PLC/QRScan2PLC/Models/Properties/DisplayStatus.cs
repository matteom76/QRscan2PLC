using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class DisplayStatus:Notifier,IProperties
    {
        public ObservableCollection<StatusCode> StatusCodeList { get; set; }

        private StatusCode _StatusSelected;

        public StatusCode StatusSelected
        {
            get { return _StatusSelected; }
            set
            {
                _StatusSelected = value; OnPropertyChanged("StatusSelected");
            }
        }

        public DisplayStatus(JObject properties)
        {
            try
            {
                StatusCodeList = new ObservableCollection<StatusCode>();
                foreach (JObject Parameter in properties["StatusCodes"])
                {
                    StatusCodeList.Add(new StatusCode(Parameter));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing Status code list of properties. More details:" + ex.ToString());
            }

        }

        public DisplayStatus()
        {
            StatusCodeList = new ObservableCollection<StatusCode>();
            StatusCodeList.Add(new StatusCode(0,"NONE"));
        }

        public JObject GetJsonObject()
        {
            JArray StatusList = new JArray();

            foreach (StatusCode status in StatusCodeList)
            {
                StatusList.Add(status.GetJsonObject());
            }

            JObject obj =
               new JObject(
                   new JProperty("StatusCodes", (JArray)StatusList)
               );

            return obj;
        }
    }
}
