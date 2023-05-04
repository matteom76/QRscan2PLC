using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_Acting_SetDataInputViewModel : BindableBase, IRegionAware
    {
        private ICommunicationDevice _Device;
        public ICommunicationDevice Device { get => _Device; set => SetProperty(ref _Device, value); }

        private IEnableFunctionsPLC _EnableFunctionsPLC;
        public IEnableFunctionsPLC EnableFunctionsPLC { get => _EnableFunctionsPLC; set => SetProperty(ref _EnableFunctionsPLC, value); }

        public DelegateCommand ConfirmCmd { get; private set; }

        private DataPLC _dataPLC;
        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        private string _EnterValue;
        public string EnterValue { get => _EnterValue; set => SetProperty(ref _EnterValue, value); }

        private int _OperationResult;
        public int OperationResult { get => _OperationResult; set => SetProperty(ref _OperationResult, value); }

        private string _uom;
        public string uom { get => _uom; set => SetProperty(ref _uom, value); }

        public QRProcessing_Acting_SetDataInputViewModel(ICommunicationDevice device, IEnableFunctionsPLC enableFunctionsPLC)
        {
            Device = device;
            EnableFunctionsPLC = enableFunctionsPLC;
            OperationResult = 0;
            ConfirmCmd = new DelegateCommand(ConfirmValue);

        }

        private void ConfirmValue()
        {
            if (DataPLC != null && Device!=null)
            {
                if (EnableFunctionsPLC.ActualValue)
                { 
                    try
                    {
                        CheckValueAndSend(EnterValue);
                        OperationResult = 2;
                    }
                    catch (Exception e)
                    {
                        OperationResult = 1;
                        DependencyService.Get<IToastMessage>().Show("Data input error:" + e.Message);
                    }
                }
            }
            else
            {
                OperationResult = 1;
                DependencyService.Get<IToastMessage>().Show("Data input error: No PLC address found.");
            }
        }

        private void CheckValueAndSend(string enterValue)
        {

            switch (DataPLC.Type)
            {
                case DataType.Integer:
                    ushort ValueInt;
                    if (ushort.TryParse(enterValue,out ValueInt))
                    {
                        Device.WriteInteger(DataPLC.DeviceAdr.addressInteger, ValueInt);
                        if (DataPLC.DeviceAdr.addressInteger.Error)
                            throw new Exception("Integer value input error.");
                    }
                    else
                        throw new Exception("Value error.An integer value is required.");
                    break;
                case DataType.Real:
                    float ValueFloat;
                    if (float.TryParse(enterValue, out ValueFloat))
                    {
                        Device.WriteReal(DataPLC.DeviceAdr.addressReal, ValueFloat);
                        if (DataPLC.DeviceAdr.addressReal.Error)
                            throw new Exception("Float value input error.");
                    }
                    else
                        throw new Exception("Value error.An float value is required.");
                    break;
                case DataType.String:
                    if (DataPLC.DeviceAdr.addressString.CheckStringLenght(enterValue))
                    {
                        Device.WriteString(DataPLC.DeviceAdr.addressString, enterValue);
                        if (DataPLC.DeviceAdr.addressString.Error)
                            throw new Exception("String value input error.");
                    }
                    else
                        throw new Exception("String entry too large. Check the PLC address.");
                    break;
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
                SetDataInput DataInput = (SetDataInput)DataPLC.Acting.Property;
                uom = DataInput.UOM;
            }
        }
    }
}
