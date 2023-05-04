using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models
{
    public class DeviceElementList: BindableBase
    {
        private string _filename;
        private bool _isselected;
        public string FileName
        {
            get { return _filename; }
            set { SetProperty(ref _filename, value); }
        }

        public bool IsSelected
        {
            get { return _isselected; }
            set { SetProperty(ref _isselected, value); }
        }

        public DeviceElementList(string filename,bool isselected)
        {
            FileName = filename;
            IsSelected = isselected;
        }

        public DeviceElementList(JObject jsonObj)
        {
            FileName = jsonObj["FileName"].ToString();
            IsSelected = false;
        }

        public JObject GetJson()
        {
            JObject obj =
                new JObject(
                    new JProperty("FileName", FileName)
                );

            return obj;
        }

    }
}
