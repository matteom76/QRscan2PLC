using DeviceCommunication.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using Prism.Regions.Navigation;
using QRScan2PLC.Enums;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_DataPLCViewModel : BindableBase, IRegionAware
    {

        private IRegionManager _regionManager { get; }


        private DataPLC _dataPLC;
   
        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        public DeviceConf_DataPLCViewModel(IRegionManager regionManager, INavigationService navigationService)
        {
            _regionManager = regionManager;
        }



        private void navigationCallback(Prism.Regions.Navigation.IRegionNavigationResult obj)
        {
            Console.WriteLine(obj.ToString());
        }

        public void OnNavigatedTo(INavigationContext navigationContext)
        {
            DataPLC = navigationContext.Parameters.GetValue<DataPLC>("dataplc");
            var PropertyParameters = new NavigationParameters();
            PropertyParameters.Add("property", DataPLC.Acting.Property);

            string propertyRegionName = "";
            switch (DataPLC.Acting.Type)
            {
                case ActingType.Button:
                    propertyRegionName = "DeviceConf_Button";
                    break;
                case ActingType.DisplayStatus:
                    propertyRegionName = "DeviceConf_DisplayStatus";
                    break;
                case ActingType.DisplayValue:
                    propertyRegionName = "DeviceConf_DisplayValue";
                    break;
                case ActingType.SetDataFixed:
                    if (DataPLC.Type == DataType.Boolean)
                        propertyRegionName = "DeviceConf_SetDataFixed_Bool";
                    else
                        if (DataPLC.Type == DataType.Integer)
                        propertyRegionName = "DeviceConf_SetDataFixed_Int";
                    else
                        if (DataPLC.Type == DataType.Real)
                        propertyRegionName = "DeviceConf_SetDataFixed_Real";
                    else
                        if (DataPLC.Type == DataType.String)
                        propertyRegionName = "DeviceConf_SetDataFixed_String";
                    break;
                case ActingType.SetDataFromCode:
                    propertyRegionName = "DeviceConf_SetDataFromCode";
                    break;
                case ActingType.SetDataInput:
                    propertyRegionName = "DeviceConf_SetDataInput";
                    break;
                case ActingType.DisplayBargraph:
                    propertyRegionName = "DeviceConf_DisplayBargraph";
                    break;
            }
            _regionManager.RequestNavigate("ActingRegion", propertyRegionName, navigationCallback, PropertyParameters);

            string PlcDataTypeRegionName = "";
            var AdressParameters = new NavigationParameters();
            AdressParameters.Add("addressTag", DataPLC.DeviceAdr);
            AdressParameters.Add("tagType", DataPLC.Type);

            if (DataPLC.DeviceCommModel==PLCModel.MQTT)
            {
                PlcDataTypeRegionName = "DeviceConf_Address_MQTT_Topic";
            }
            else
            { 
                switch (DataPLC.Type)
                {
                    case DataType.Boolean:
                        PlcDataTypeRegionName = "DeviceConf_Address_Siemens_Bool";
                        break;
                    case DataType.Integer:
                        PlcDataTypeRegionName = "DeviceConf_Address_Siemens_Int";
                        break;
                    case DataType.Real:
                        PlcDataTypeRegionName = "DeviceConf_Address_Siemens_Real";
                        break;
                    case DataType.String:
                        PlcDataTypeRegionName = "DeviceConf_Address_Siemens_String";
                        break;
                }
            }

            _regionManager.RequestNavigate("AddressRegion", PlcDataTypeRegionName, navigationCallback, AdressParameters);
        }

        public bool IsNavigationTarget(INavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(INavigationContext navigationContext)
        {

        }
    }
}
