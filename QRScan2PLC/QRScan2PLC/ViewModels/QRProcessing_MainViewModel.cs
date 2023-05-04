using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using DeviceCommunication.MQTT;
using DeviceCommunication.Siemens;
using Newtonsoft.Json.Linq;
using Prism.AppModel;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using QRScan2PLC.Events;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_MainViewModel : BindableBase, IPageLifecycleAware
    {

        private static string RootFileConfSelected = Xamarin.Essentials.FileSystem.AppDataDirectory;
        private static string RootFileConfiguration = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //private static string RootFileConfiguration = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);


        private IRegionManager _regionManager { get; }
        private IEventAggregator _ea;
        private INavigationService NavigationService;
        private QRScanDevice DeviceConf;
        private ICommunicationDevice PlcDevice;
        private IContainerRegistry ContainerRegistry;

        public DelegateCommand CallQRScannerCmd { get; private set; }
        public DelegateCommand DeviceSelectionCmd { get; private set; }



        public QRProcessing_MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,INavigationService navigationService,IContainerRegistry containerRegistry)
        {
            _regionManager = regionManager;
            _ea = eventAggregator;
            NavigationService = navigationService;
            ContainerRegistry = containerRegistry;
            CallQRScannerCmd = new DelegateCommand(CallQRScanner);
            DeviceSelectionCmd = new DelegateCommand(SetDeviceCall);
            _ea.GetEvent<QRScannerResponse>().Subscribe(QRScanReceived,ThreadOption.UIThread);

            var SelectedConfiguration = Path.Combine(RootFileConfSelected, "selected.qrc");

            if (File.Exists(SelectedConfiguration))
            {
                try
                {
                    String JsonContent = File.ReadAllText(SelectedConfiguration);
                    JObject JSonConfiguration = JObject.Parse(JsonContent);
                    DeviceElementList deviceSelected = new DeviceElementList(JSonConfiguration);

                    foreach (string ConfFile in Directory.GetFiles(RootFileConfiguration))
                    {
                        string ConfFileName = Path.GetFileNameWithoutExtension(ConfFile);
                        if (ConfFileName != "selected" && ConfFile.Contains(".qrc") && ConfFileName.Equals(deviceSelected.FileName))
                        {
                            var SelectedDeviceConf = Path.Combine(RootFileConfiguration, ConfFile);
                            String JsonContentDeviceConf = File.ReadAllText(SelectedDeviceConf);
                            JObject JSonDeviceConf = JObject.Parse(JsonContentDeviceConf);
                            DeviceConf = new QRScanDevice(JSonDeviceConf);
                            break;

                        }
                    }

                    if (!ContainerRegistry.IsRegistered<ICommunicationDevice>())
                    {
                        ContainerRegistry.RegisterInstance<ICommunicationDevice>(null);
                    }

                    if (DeviceConf.ConfDetails.PLCBasicInfo.Model == PLCModel.MQTT)
                        PlcDevice = new MQTTDevice((MQTTDataConnection)DeviceConf.ConfDetails.PLCBasicInfo.DataConnection);
                    else
                        PlcDevice = new SiemensPLC((SiemensDataConnection)DeviceConf.ConfDetails.PLCBasicInfo.DataConnection);
                    
                    PlcDevice.Connect(DeviceConf.ConfDetails.PLCBasicInfo.EnableFunctions);
                    ContainerRegistry.RegisterInstance<ICommunicationDevice>(PlcDevice);
                    ContainerRegistry.RegisterInstance<IEnableFunctionsPLC>(DeviceConf.ConfDetails.PLCBasicInfo.EnableFunctions);

                }
                catch
                {
                    DependencyService.Get<IToastMessage>().Show("Problem during parsing configuration file selected.");
                }
            }
        }

        private async void SetDeviceCall()
        {

            if (PlcDevice != null)
            {
                PlcDevice.StopScanningForcing();
            }

            await NavigationService.NavigateAsync("/DeviceSelection");
        }

        private void CallQRScanner()
        {

            if (PlcDevice != null)
               PlcDevice.RefreshValueToMonitoring();

            _regionManager.RequestNavigate("OperationRegion", "QRProcessing_QRScanner", navigationCallback);
        }

        private void navigationCallback(Prism.Regions.Navigation.IRegionNavigationResult obj)
        {
            Console.WriteLine(obj.ToString());
        }

        private void QRScanReceived(CodeReadData scanResult)
        {
            
            var PropertyParameters = new NavigationParameters();
            PropertyParameters.Add("QRResult", scanResult);
            if (DeviceConf != null)
                PropertyParameters.Add("QRTypesList", DeviceConf.ConfDetails.QRTypesList);

            _regionManager.RequestNavigate("OperationRegion", "QRProcessing_QRTypeActing_Main", navigationCallback, PropertyParameters);
        }

        public void OnAppearing()
        {
            if (DeviceConf != null)
            {
                var PropertyParameters = new NavigationParameters();
                PropertyParameters.Add("PLCBasicInfo", DeviceConf.ConfDetails.PLCBasicInfo);
                _regionManager.RequestNavigate("PLCInfoRegion", "QRProcessing_PLCInfo", navigationCallback, PropertyParameters);
            }
        }

        public void OnDisappearing()
        {

        }
    }
}
