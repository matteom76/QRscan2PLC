using DeviceCommunication.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_NameAndModelViewModel : BindableBase, INavigationAware
    {
        private static string RootFileConfSelected = Xamarin.Essentials.FileSystem.AppDataDirectory;
        private static string RootFileConfiguration = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //private static string RootFileConfiguration = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);

        private string NameFileConf;
        private INavigationService NavigationService;
        private IRegionManager _regionManager { get; }

        public DelegateCommand SaveCmd { get; private set; }
        public DelegateCommand BackCmd { get; private set; }
        public DelegateCommand EditQRTypeCmd { get; private set; }
        public DelegateCommand NewQRTypeCmd { get; private set; }
        public DelegateCommand DeleteQRTypeCmd { get; }

        private QRScanDevice _deviceConf;
        public QRScanDevice DeviceConf
        {
            get => _deviceConf;
            set => SetProperty(ref _deviceConf, value);
        }

        private QRTypes _QRTypeSelected;
        public QRTypes QRTypeSelected
        {
            get => _QRTypeSelected;
            set => SetProperty(ref _QRTypeSelected, value);
        }

        private string _TextPLCInfo;
        public string TextPLCInfo { get => _TextPLCInfo; set => SetProperty(ref _TextPLCInfo, value); }


        public DeviceConf_NameAndModelViewModel(INavigationService navigationService,IRegionManager regionManager)
        {
            SaveCmd = new DelegateCommand(SaveConfiguration);
            BackCmd = new DelegateCommand(BackToDeviceSelection);
            EditQRTypeCmd = new DelegateCommand(EditQRType);
            NewQRTypeCmd = new DelegateCommand(NewQRType);
            DeleteQRTypeCmd = new DelegateCommand(DeleteQRType);
            NavigationService = navigationService;
            _regionManager = regionManager;
            

        }

        private void DeleteQRType()
        {
            if (QRTypeSelected != null)
            {
                DeviceConf.ConfDetails.QRTypesList.Remove(QRTypeSelected);
                QRTypeSelected = null;
            }
            else
                DependencyService.Get<IToastMessage>().Show("Select a QR type from list");
        }

        private async void NewQRType()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("deviceconf", DeviceConf);
            await NavigationService.NavigateAsync("/DeviceConf_QRType", navigationParams);
        }

        private async void EditQRType()
        {
            if (QRTypeSelected != null)
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("qrtype", QRTypeSelected);
                navigationParams.Add("deviceconf", DeviceConf);
                await NavigationService.NavigateAsync("/DeviceConf_QRType", navigationParams);
            }
            else
                DependencyService.Get<IToastMessage>().Show("Select a QR type from list");
        }

        private async void BackToDeviceSelection()
        {
            await NavigationService.NavigateAsync("/DeviceSelection");
        }

        private void SaveConfiguration()
        {
            if (DeviceConf.FileName!=NameFileConf && DeviceConf.FileName!="selected")
            {
                var OldConfiguration = Path.Combine(RootFileConfiguration, NameFileConf + ".qrc");
                if (File.Exists(OldConfiguration))
                    File.Delete(OldConfiguration);
                
                NameFileConf = DeviceConf.FileName;
            }

            JObject JSonConf = DeviceConf.GetJsonQRScanDevice();
            String SerializeData = JsonConvert.SerializeObject(JSonConf);
            var FileConfiguration = Path.Combine(RootFileConfiguration, NameFileConf + ".qrc");
            try
            { 
                File.WriteAllText(FileConfiguration, SerializeData);
        
            }
            catch
            {
                DependencyService.Get<IToastMessage>().Show("Error during save file with " + NameFileConf + "name. Try with another name.");
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            NameFileConf = parameters.GetValue<string>("filename");
            var model = parameters.GetValue<PLCModel>("deviceModel");
            if (NameFileConf!=null)
            {
                var SelectedConfiguration = Path.Combine(RootFileConfiguration, NameFileConf+ ".qrc");
                String JsonContent = File.ReadAllText(SelectedConfiguration);
                JObject JSonConfiguration = JObject.Parse(JsonContent);
                DeviceConf = new QRScanDevice(JSonConfiguration);
                
            }
            else
            {
                int countDevices = Directory.GetFiles(RootFileConfiguration).Count() + 1;
                string confFileName = "conf_" + countDevices.ToString();
                while (File.Exists(Path.Combine(RootFileConfiguration, confFileName + ".qrc")))
                {
                    countDevices += 1;
                    confFileName = "conf_" + countDevices.ToString();
                }
                NameFileConf = confFileName;
                DeviceConf = new QRScanDevice(confFileName,model);
            }
            var PLCInfoParameters = new NavigationParameters();
            PLCInfoParameters.Add("DataConnection", DeviceConf.ConfDetails.PLCBasicInfo.DataConnection);            
            var EnablePLCParameters = new NavigationParameters();
            EnablePLCParameters.Add("EnablePLCInfo", DeviceConf.ConfDetails.PLCBasicInfo.EnableFunctions);

            if (DeviceConf.ConfDetails.PLCBasicInfo.Model==PLCModel.MQTT)
            {
                TextPLCInfo = "MQTT BROKER INFO";
                _regionManager.RequestNavigate("PLCInfoRegion", "DeviceConf_MQTTBrokerInfo", navigationCallback, PLCInfoParameters);
                _regionManager.RequestNavigate("EnablePLCRegion", "DeviceConf_Address_MQTT_Topic", navigationCallback, EnablePLCParameters);
            }
            else
            {
                TextPLCInfo = "SIEMENS PLC INFO";
                _regionManager.RequestNavigate("PLCInfoRegion", "DeviceConf_SiemensPLCInfo", navigationCallback, PLCInfoParameters);
                _regionManager.RequestNavigate("EnablePLCRegion", "DeviceConf_SiemensEnablePLCFunction", navigationCallback, EnablePLCParameters);
            }
        }

        private void navigationCallback(Prism.Regions.Navigation.IRegionNavigationResult obj)
        {

            Console.WriteLine(obj.ToString());
        }
    }
}
