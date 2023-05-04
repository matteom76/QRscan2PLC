using DeviceCommunication.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Enums;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_Acting_ButtonViewModel : BindableBase, IRegionAware
    {
        private Button _btn;

        public Button Btn { get => _btn; set => SetProperty(ref _btn, value); }
        
        public DelegateCommand ButtonCmd { get; private set; }

        private DataPLC _dataPLC;

        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        private ICommunicationDevice _Device;
        public ICommunicationDevice Device { get => _Device; set => SetProperty(ref _Device, value); }

        private IEnableFunctionsPLC _EnableFunctionsPLC;
        public IEnableFunctionsPLC EnableFunctionsPLC { get => _EnableFunctionsPLC; set => SetProperty(ref _EnableFunctionsPLC, value); }

        public QRProcessing_Acting_ButtonViewModel(ICommunicationDevice device,IEnableFunctionsPLC enableFunctionsPLC)
        {
            Device = device;
            EnableFunctionsPLC = enableFunctionsPLC;          
            ButtonCmd = new DelegateCommand(ButtonAction);
        }

        private void ButtonAction()
        {
            if (DataPLC!=null && EnableFunctionsPLC.ActualValue)
            {
                
                if (Btn.ButtonMode==ButtonMode.SetBit)
                {
                    Device.WriteBool(DataPLC.DeviceAdr.addressBoolean, true);
                }
                else
                {
                    if (Btn.ButtonMode==ButtonMode.ToggleBit)
                    {
                        bool actValue = !DataPLC.DeviceAdr.addressBoolean.ActualValue;
                        Device.WriteBool(DataPLC.DeviceAdr.addressBoolean, actValue);
                    }
                }
            }
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
                
                Device.MonitoringValueBool(DataPLC.DeviceAdr.addressBoolean);
                Btn = (Button)DataPLC.Acting.Property;
            }
        }
    }
}
