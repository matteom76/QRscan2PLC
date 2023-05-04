using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_QRTypeViewModel : BindableBase,INavigationAware
    {

        private static string RootFileConfiguration = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //private static string RootFileConfiguration = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);

        private QRTypes _qrtypesSelected;

        public QRTypes QRtypeSelected
        {
            get => _qrtypesSelected;
            set => SetProperty(ref _qrtypesSelected, value);
        }

        private DataPLC _DataPLCSelected;
        public DataPLC DataPLCSelected
        {
            get => _DataPLCSelected;
            set => SetProperty(ref _DataPLCSelected, value);
        }

        private QRScanDevice _QRScanDevice;
        private string fileName;
        private INavigationService NavigationService;
        
        public DelegateCommand SaveCmd { get; }
        public DelegateCommand BackCmd { get; }
        public DelegateCommand EditDataPLCCmd { get;}
        public DelegateCommand NewDataPLCCmd { get; }
        public DelegateCommand DeleteDataPLCCmd { get; }
        public DelegateCommand DatalistMoveUpCmd { get; }
        public DelegateCommand DatalistMoveDownCmd { get; }

        public DeviceConf_QRTypeViewModel(INavigationService navigationService)
        {
            SaveCmd = new DelegateCommand(Save);
            BackCmd = new DelegateCommand(BackAsync);
            EditDataPLCCmd = new DelegateCommand(EditDataPLC);
            NewDataPLCCmd = new DelegateCommand(NewDataPLC);
            DeleteDataPLCCmd = new DelegateCommand(DeleteDataPLC);
            DatalistMoveUpCmd = new DelegateCommand(DatalistMoveUp);
            DatalistMoveDownCmd = new DelegateCommand(DatalistMoveDown);
            NavigationService = navigationService;

        }

        private void DatalistMoveDown()
        {
            if (DataPLCSelected != null)
            {
                int DataPLCIndex = QRtypeSelected.DataList.IndexOf(DataPLCSelected);
                int DataPLCListLenght = QRtypeSelected.DataList.Count - 1;
                if (DataPLCIndex!=-1 && DataPLCIndex < DataPLCListLenght)
                {
                    QRtypeSelected.DataList.Move(DataPLCIndex, DataPLCIndex + 1);
                }
            }
        }


        private void DatalistMoveUp()
        {
            if (DataPLCSelected != null)
            {
                int DataPLCIndex = QRtypeSelected.DataList.IndexOf(DataPLCSelected);
                if (DataPLCIndex > 0)
                {
                    QRtypeSelected.DataList.Move(DataPLCIndex, DataPLCIndex - 1);
                }
            }
        }

        private void DeleteDataPLC()
        {
            if (DataPLCSelected != null)
            {
                QRtypeSelected.DataList.Remove(DataPLCSelected);
                DataPLCSelected = null;
            }
            else
                DependencyService.Get<IToastMessage>().Show("Select a Data PLC from list");
        }

        private async void NewDataPLC()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("deviceconf", _QRScanDevice);
            navigationParams.Add("qrtypeselected", QRtypeSelected);
            await NavigationService.NavigateAsync("/DeviceConf_DataPLC_Selection", navigationParams);
        }

        private async void EditDataPLC()
        {
            if (DataPLCSelected != null)
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("deviceconf", _QRScanDevice);
                navigationParams.Add("qrtypeselected", QRtypeSelected);
                navigationParams.Add("dataselected", DataPLCSelected);
                await NavigationService.NavigateAsync("/DeviceConf_DataPLC_Selection", navigationParams);
            }
            else
                DependencyService.Get<IToastMessage>().Show("Select a Data PLC from list");
        }

        private async void BackAsync()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("filename", fileName);
            await NavigationService.NavigateAsync("/DeviceConf_NameAndModel", navigationParams);
        }

        private void Save()
        {
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
            QRtypeSelected = parameters.GetValue<QRTypes>("qrtype");
            _QRScanDevice = parameters.GetValue<QRScanDevice>("deviceconf");
            fileName = _QRScanDevice.FileName;
            if (QRtypeSelected==null)
            {
                int count = 0;
                do
                {
                    count++;
                }
                while (_QRScanDevice.ConfDetails.QRTypesList.Count(q => q.Name == "Type" + count)>0);
                QRtypeSelected = new QRTypes("Type"+count);
                _QRScanDevice.ConfDetails.QRTypesList.Add(QRtypeSelected);
                
            }

        }
    }
}
