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
    public class QRProcessing_Acting_SetDataFromCodeViewModel : BindableBase, IRegionAware
    {
        private DataPLC _dataPLC;

        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        private ICommunicationDevice _Device;
        public ICommunicationDevice Device { get => _Device; set => SetProperty(ref _Device, value); }

        private IEnableFunctionsPLC _EnableFunctionsPLC;
        public IEnableFunctionsPLC EnableFunctionsPLC { get => _EnableFunctionsPLC; set => SetProperty(ref _EnableFunctionsPLC, value); }

        private bool _OperationResult;
        public bool OperationResult { get => _OperationResult; set => SetProperty(ref _OperationResult, value); }

        private DataFromCodeProcessing _OperationData;
        public DataFromCodeProcessing OperationData { get => _OperationData; set => SetProperty(ref _OperationData, value); }


        public QRProcessing_Acting_SetDataFromCodeViewModel(ICommunicationDevice device, IEnableFunctionsPLC enableFunctionsPLC)
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
            string QRResult = navigationContext.Parameters.GetValue<string>("QRCodeData");
           
            if (DataPLC != null && QRResult!=null && EnableFunctionsPLC.ActualValue)
            {
                SetDataFromCode DataFromBarCode = (SetDataFromCode)DataPLC.Acting.Property;
                OperationData = new DataFromCodeProcessing(Device, DataFromBarCode);
                try
                {
                    string InfoFromBarCode = OperationData.CheckInfoFromCode(QRResult);
                    OperationData.WriteDataToPLC(InfoFromBarCode, DataPLC.DeviceAdr);                    
                    OperationResult = true;


                }
                catch (Exception e)
                {
                    DependencyService.Get<IToastMessage>().Show("Data from barcode error:" + e.Message);
                    OperationResult = false;
                }
            }
        }
    }
}
