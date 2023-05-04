using DeviceCommunication.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_Acting_DisplayStatusViewModel : BindableBase, IRegionAware
    {
        private DisplayStatus _displayStatus;
        public DisplayStatus displayStatus { get => _displayStatus; set => SetProperty(ref _displayStatus, value); }
        
        private DataPLC _dataPLC;

        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        private ICommunicationDevice _Device;
        public ICommunicationDevice Device { get => _Device; set => SetProperty(ref _Device, value); }

        public QRProcessing_Acting_DisplayStatusViewModel(ICommunicationDevice communicationDevice)
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
            DataPLC = navigationContext.Parameters.GetValue<DataPLC>("DataPLC");
            if (DataPLC!=null)
            {
                displayStatus = (DisplayStatus)DataPLC.Acting.Property;
                Device.MonitoringValueInteger(DataPLC.DeviceAdr.addressInteger);
                
            }
            
            
        }
    }
}
