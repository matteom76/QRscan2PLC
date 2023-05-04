using DeviceCommunication.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using QRScan2PLC.Enums;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_DataPLC_SelectionViewModel : BindableBase, INavigationAware
    {
        private static string RootFileConfiguration = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //private static string RootFileConfiguration = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);

        private IRegionManager _regionManager { get; }
        private INavigationService NavigationService;

        public DelegateCommand SaveCmd { get; }
        public DelegateCommand BackCmd { get; }
        public DelegateCommand ActingTypeSelectCmd { get; private set; }
        public DelegateCommand DataTypeSelectCmd { get; private set; }

        private QRTypes _QRType;
        private QRScanDevice _QRScanDevice;
        private string fileName;
        private DataPLC _dataPLC;

        private bool ExistingActingType = false;
        private bool ExistingDataType = false;

        public List<string> ActingTypeList
        {
            get { return Enum.GetNames(typeof(ActingType)).ToList(); }
        }

        public List<string> DataTypeList
        {
            get { return Enum.GetNames(typeof(DataType)).ToList(); }
        }

        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        public DataPLC NewDataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        private int _ActingTypeSelected;

        public int ActingTypeSelected {get { return _ActingTypeSelected; } set { SetProperty(ref _ActingTypeSelected, value); } }

        private int _DataTypeSelected;
        public int DataTypeSelected { get { return _DataTypeSelected; } set { SetProperty(ref _DataTypeSelected, value); } }

        private bool _EnableActingTypeSelection;
        public bool EnableActingTypeSelection { get => _EnableActingTypeSelection; set => SetProperty(ref _EnableActingTypeSelection, value); }

        private bool _EnableDataTypeSelection;
        public bool EnableDataTypeSelection { get => _EnableDataTypeSelection; set => SetProperty(ref _EnableDataTypeSelection, value); }

        public DeviceConf_DataPLC_SelectionViewModel(IRegionManager regionManager, INavigationService navigationService)
        {
            _regionManager = regionManager;
            NavigationService = navigationService;
            SaveCmd = new DelegateCommand(Save);
            BackCmd = new DelegateCommand(BackAsync);
            ActingTypeSelectCmd = new DelegateCommand(ActingTypeSelect);
            DataTypeSelectCmd = new DelegateCommand(DataTypeSelect);
        }

        private void DataTypeSelect()
        {
            if (!ExistingDataType)
            {
                try
                {
                    CheckSelections();
                    if (CreateDataPLC())
                    { 
                        EnableDataTypeSelection = false;
                        EnableActingTypeSelection = false;
                    }
                }
                catch (Exception e)
                {
                    DependencyService.Get<IToastMessage>().Show(e.Message);
                }
            }
            else
                ExistingDataType = false;
        }

        private void ActingTypeSelect()
        {
            if (!ExistingActingType)
            {
                try
                {
                    CheckSelections();
                    if (CreateDataPLC())
                    {
                        EnableDataTypeSelection = false;
                        EnableActingTypeSelection = false;
                    }


                }
                catch (Exception e)
                {
                    DependencyService.Get<IToastMessage>().Show(e.Message);
                }
            }
            else
                ExistingActingType = false;
        }

        private bool CreateDataPLC()
        {
            if (ActingTypeSelected>-1 && DataTypeSelected>-1)
            {
                NewDataPLC = new DataPLC((ActingType)ActingTypeSelected, (DataType)DataTypeSelected, _QRScanDevice.ConfDetails.PLCBasicInfo.Model);
                var PropertyParameters = new NavigationParameters();
                PropertyParameters.Add("dataplc", NewDataPLC);
                _regionManager.RequestNavigate("DataPLCRegion", "DeviceConf_DataPLC", navigationCallback, PropertyParameters);
                return true;
            }

            return false;
        }

        private void CheckSelections()
        {
            switch ((ActingType)ActingTypeSelected)
            {
                case ActingType.Button:
                    if ((DataType)DataTypeSelected != DataType.Boolean)
                        throw new Exception("For button you must to select a boolean data type.");
                    break;
                case ActingType.DisplayStatus:
                    if ((DataType)DataTypeSelected != DataType.Integer)
                        throw new Exception("For display status you must to select a integer data type.");
                    break;
                case ActingType.SetDataInput:
                    if ((DataType)DataTypeSelected==DataType.Boolean)
                        throw new Exception("For set data input you can't select a boolean data type.");
                    break;
                case ActingType.DisplayBargraph:
                    if ((DataType)DataTypeSelected != DataType.Real)
                        throw new Exception("For bargraph display you must to select a float data type.");
                    break;
            }

        }

        private async void BackAsync()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("qrtype", _QRType);
            navigationParams.Add("deviceconf", _QRScanDevice);
            await NavigationService.NavigateAsync("/DeviceConf_QRType", navigationParams);
        }

        private void Save()
        {
            if (NewDataPLC!=null)
            {
                if (DataPLC != null)
                    _QRType.DataList.Remove(DataPLC);
                
                _QRType.DataList.Add(NewDataPLC);
            }            
            
            JObject JSonConf = _QRScanDevice.GetJsonQRScanDevice();
            String SerializeData = JsonConvert.SerializeObject(JSonConf);
            var FileConfiguration = Path.Combine(RootFileConfiguration, fileName + ".qrc");
            try
            {
                File.WriteAllText(FileConfiguration, SerializeData);

            }
            catch
            {
                DependencyService.Get<IToastMessage>().Show("Error during save file. File corrupted.");
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _QRType = parameters.GetValue<QRTypes>("qrtypeselected");
            _QRScanDevice = parameters.GetValue<QRScanDevice>("deviceconf");
            DataPLC = parameters.GetValue<DataPLC>("dataselected");
            fileName = _QRScanDevice.FileName;
            ExistingActingType = true;
            ExistingDataType = true;
            if (DataPLC != null)
            {
                var PropertyParameters = new NavigationParameters();
                PropertyParameters.Add("dataplc", DataPLC);
                _regionManager.RequestNavigate("DataPLCRegion", "DeviceConf_DataPLC", navigationCallback, PropertyParameters);
                ActingTypeSelected = (int)DataPLC.Acting.Type;
                DataTypeSelected = (int)DataPLC.Type;
                EnableActingTypeSelection = false;
                EnableDataTypeSelection = false;
            }
            else
            {
                ActingTypeSelected = -1;
                DataTypeSelected = -1;
                EnableActingTypeSelection = true;
                EnableDataTypeSelection = true;
            }

        }

        private void navigationCallback(Prism.Regions.Navigation.IRegionNavigationResult obj)
        {
            Console.WriteLine(obj.ToString());
        }
    }
}
