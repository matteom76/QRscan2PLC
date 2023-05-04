using DeviceCommunication.Enums;
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
    public class QRProcessing_Acting_DisplayValueViewModel : BindableBase, IRegionAware
    {
        private ICommunicationDevice _Device;
        public ICommunicationDevice Device { get => _Device; set => SetProperty(ref _Device, value); }

        private DataPLC _dataPLC;
        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        private string _uom;
        public string uom { get => _uom; set => SetProperty(ref _uom, value); }

        private bool _BoolValueVisible;
        public bool BoolValueVisible { get => _BoolValueVisible; set => SetProperty(ref _BoolValueVisible, value); }

        private bool _IntValueVisible;
        public bool IntValueVisible { get => _IntValueVisible; set => SetProperty(ref _IntValueVisible, value); }

        private bool _FloatValueVisible;
        public bool FloatValueVisible { get => _FloatValueVisible; set => SetProperty(ref _FloatValueVisible, value); }

        private bool _StringValueVisible;
        public bool StringValueVisible { get => _StringValueVisible; set => SetProperty(ref _StringValueVisible, value); }

        
        public QRProcessing_Acting_DisplayValueViewModel(ICommunicationDevice device)
        {
            BoolValueVisible = false;
            IntValueVisible = false;
            FloatValueVisible = false;
            StringValueVisible = false;
            Device = device;
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
            if (DataPLC != null)
            {
                DisplayValue DataValue = (DisplayValue)DataPLC.Acting.Property;
                uom = DataValue.UOM;
                switch (DataPLC.Type)
                {
                    case DataType.Boolean:
                        BoolValueVisible = true;
                        IntValueVisible = false;
                        FloatValueVisible = false;
                        StringValueVisible = false;
                        Device.MonitoringValueBool(DataPLC.DeviceAdr.addressBoolean);
                        break;
                    case DataType.Integer:
                        BoolValueVisible = false;
                        IntValueVisible = true;
                        FloatValueVisible = false;
                        StringValueVisible = false;
                        Device.MonitoringValueInteger(DataPLC.DeviceAdr.addressInteger);
                        break;
                    case DataType.Real:
                        BoolValueVisible = false;
                        IntValueVisible = false;
                        FloatValueVisible = true;
                        StringValueVisible = false;
                        Device.MonitoringValueReal(DataPLC.DeviceAdr.addressReal);
                        break;
                    case DataType.String:
                        BoolValueVisible = false;
                        IntValueVisible = false;
                        FloatValueVisible = false;
                        StringValueVisible = true;
                        Device.MonitoringValueString(DataPLC.DeviceAdr.addressString);
                        break;
                }

                //DataPLC.DeviceAdr.addressString.ActualValue
                    //DataPLC.DeviceAdr.addressString.Error
            }
        }
    }
}
