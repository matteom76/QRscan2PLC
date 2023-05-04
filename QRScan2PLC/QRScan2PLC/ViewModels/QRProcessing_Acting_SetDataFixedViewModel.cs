using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using QRScan2PLC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_Acting_SetDataFixedViewModel : BindableBase, IRegionAware
    {
        private DataPLC _dataPLC;

        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        private ICommunicationDevice _Device;
        public ICommunicationDevice Device { get => _Device; set => SetProperty(ref _Device, value); }

        private IEnableFunctionsPLC _EnableFunctionsPLC;
        public IEnableFunctionsPLC EnableFunctionsPLC { get => _EnableFunctionsPLC; set => SetProperty(ref _EnableFunctionsPLC, value); }

        private bool _OperationResult;
        public bool OperationResult { get => _OperationResult; set => SetProperty(ref _OperationResult, value); }

        private string _OperationData;
        public string OperationData { get => _OperationData; set => SetProperty(ref _OperationData, value); }

        public QRProcessing_Acting_SetDataFixedViewModel(ICommunicationDevice device, IEnableFunctionsPLC enableFunctionsPLC)
        {
            Device = device;
            EnableFunctionsPLC = enableFunctionsPLC;
            OperationResult = false;
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

            if (DataPLC != null && EnableFunctionsPLC.ActualValue)
            {
                if (DataPLC.DeviceAdr.type == DataType.Integer)
                {
                    SetDataFixed<int> dataFixed = (SetDataFixed<int>)DataPLC.Acting.Property;
                    OperationData = dataFixed.Value.ToString();
                    DataFixedProcessing<int> DataProcessing = new DataFixedProcessing<int>(Device, dataFixed);
                    try
                    {
                        DataProcessing.WriteDataToPLC(DataPLC.DeviceAdr);
                        OperationResult = true;
                    }
                    catch (Exception e)
                    {
                        DependencyService.Get<IToastMessage>().Show("Datafixed error:" + e.Message);
                    }
                }
                else
                    if (DataPLC.DeviceAdr.type == DataType.Boolean)
                    {
                        SetDataFixed<bool> dataFixed = (SetDataFixed<bool>)DataPLC.Acting.Property;
                        OperationData = dataFixed.Value.ToString();

                        DataFixedProcessing<bool> DataProcessing = new DataFixedProcessing<bool>(Device, dataFixed);
                        try
                        {
                            DataProcessing.WriteDataToPLC(DataPLC.DeviceAdr);
                            OperationResult = true;
                        }
                        catch (Exception e)
                        {
                            DependencyService.Get<IToastMessage>().Show("Datafixed error:" + e.Message);
                        }
                    }
                else
                    if (DataPLC.DeviceAdr.type == DataType.Real)
                    {
                        SetDataFixed<double> dataFixed = (SetDataFixed<double>)DataPLC.Acting.Property;
                        OperationData = dataFixed.Value.ToString();

                        DataFixedProcessing<double> DataProcessing = new DataFixedProcessing<double>(Device, dataFixed);
                        try
                        {
                            DataProcessing.WriteDataToPLC(DataPLC.DeviceAdr);
                            OperationResult = true;
                        }
                        catch (Exception e)
                        {
                            DependencyService.Get<IToastMessage>().Show("Datafixed error:" + e.Message);
                        }
                    }
                else
                    if (DataPLC.DeviceAdr.type == DataType.String)
                    {
                        SetDataFixed<string> dataFixed = (SetDataFixed<string>)DataPLC.Acting.Property;
                        OperationData = dataFixed.Value.ToString();

                        DataFixedProcessing<string> DataProcessing = new DataFixedProcessing<string>(Device, dataFixed);
                        try
                        {
                            DataProcessing.WriteDataToPLC(DataPLC.DeviceAdr);
                            OperationResult = true;
                        }
                        catch (Exception e)
                        {
                            DependencyService.Get<IToastMessage>().Show("Datafixed error:" + e.Message);
                        }
                    }
            }
        }
    }
}
