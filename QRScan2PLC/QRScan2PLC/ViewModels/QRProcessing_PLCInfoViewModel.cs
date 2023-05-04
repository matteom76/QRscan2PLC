using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_PLCInfoViewModel : BindableBase,IRegionAware
    {

        private PLCBasicInfo _plcBasicInfo;

        public PLCBasicInfo PlcInfo { get => _plcBasicInfo; set => SetProperty(ref _plcBasicInfo, value); }

        private string _TextDevice;
        public string TextDevice { get => _TextDevice; set => SetProperty(ref _TextDevice, value); }

        private ICommunicationDevice _Device;
        public ICommunicationDevice Device { get => _Device; set => SetProperty(ref _Device, value); }
        
        public QRProcessing_PLCInfoViewModel(ICommunicationDevice communicationDevice)
        {
            Device = communicationDevice;
        }

        public bool IsNavigationTarget(INavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(INavigationContext navigationContext)
        {
           
        }

        public void OnNavigatedTo(INavigationContext navigationContext)
        {
            PlcInfo = navigationContext.Parameters.GetValue<PLCBasicInfo>("PLCBasicInfo");
            if (PlcInfo.Model == PLCModel.MQTT)
                TextDevice = "MQTT";
            else
            if (PlcInfo.Model == PLCModel.Siemens)
                TextDevice = "PLC";
            else
                TextDevice = "";
        }
    }
}
