using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace QRScan2PLC.ViewModels
{
    public class DeviceSelectionViewModel : BindableBase
    {
        public DelegateCommand NewConfigurationCmd { get; private set; }

        public DelegateCommand DeleteConfigurationCmd { get; private set; }

        public DelegateCommand EditConfigurationCmd { get; private set; }

        public DelegateCommand SelectConfigurationCmd { get; private set; }

        public DelegateCommand BackToQRProcessingCmd { get; private set; }

        public DelegateCommand CloseApplicationCmd { get; private set; }

        public DelegateCommand ExportConfigurationCmd { get; private set; }

        public DelegateCommand ImportConfigurationCmd { get; private set; }

        private INavigationService NavigationService;

        private static string RootFileConfSelected = Xamarin.Essentials.FileSystem.AppDataDirectory;
        private static string RootFileConfiguration = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //private static string RootFileConfiguration = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);

        private string ActualSelectedConfFileName;

        private ObservableCollection<DeviceElementList> _filedeviceList;

        public ObservableCollection<DeviceElementList> FileDeviceList
        {
            get { return _filedeviceList; }
            set { SetProperty(ref _filedeviceList,value); }
        }

        private DeviceElementList DeviceSelectedForProcess;

        private DeviceElementList _DeviceSelected;
        public DeviceElementList DeviceSelected
        {
            get { return _DeviceSelected; }
            set { SetProperty(ref _DeviceSelected, value); }
        }

        
        public DeviceSelectionViewModel(INavigationService navigationService)
        {

            ActualSelectedConfFileName = "";
            NavigationService = navigationService;
            FileDeviceList = new ObservableCollection<DeviceElementList>();
            NewConfigurationCmd = new DelegateCommand(NewConfiguration);
            DeleteConfigurationCmd = new DelegateCommand(DeleteConfiguration, CanExecute);
            EditConfigurationCmd = new DelegateCommand(EditConfiguration, CanExecute);
            SelectConfigurationCmd = new DelegateCommand(SelectConfForProcess);
            BackToQRProcessingCmd = new DelegateCommand(BackToQRProcessing);
            CloseApplicationCmd = new DelegateCommand(CloseApplication);
            ExportConfigurationCmd = new DelegateCommand(ExportConfiguration);
            ImportConfigurationCmd = new DelegateCommand(ImportConfiguration);


            var SelectedConfiguration = Path.Combine(RootFileConfSelected, "selected.qrc");

            if (File.Exists(SelectedConfiguration))
            {
                try
                {
                    String JsonContent = File.ReadAllText(SelectedConfiguration);
                    JObject JSonConfiguration = JObject.Parse(JsonContent);
                    DeviceElementList DeviceConf = new DeviceElementList(JSonConfiguration);
                    ActualSelectedConfFileName = DeviceConf.FileName;

                }
                catch
                {

                }
            }

            PopulateFileDeviceList();

        }

        private void PopulateFileDeviceList()
        {
            foreach (string ConfFile in Directory.GetFiles(RootFileConfiguration))
            {
                string ConfFileName = Path.GetFileNameWithoutExtension(ConfFile);
                if (ConfFileName != "selected" && ConfFile.Contains(".qrc"))
                {
                    DeviceElementList device = new DeviceElementList(ConfFileName, (ConfFileName.Equals(ActualSelectedConfFileName) ? true : false));
                    if (device.IsSelected)
                        DeviceSelectedForProcess = device;

                    FileDeviceList.Add(device);
                }

            }
        }

        private void CloseApplication()
        {
            Environment.Exit(0);
        }

        private async void BackToQRProcessing()
        {
            await NavigationService.NavigateAsync("/QRProcessing_Main");
        }

        private void SelectConfForProcess()
        {
            if (DeviceSelected != null)
            {
                if (!DeviceSelected.IsSelected)
                {
                    var SelectedConfiguration = Path.Combine(RootFileConfSelected, "selected.qrc");
                    if (File.Exists(SelectedConfiguration))
                        File.Delete(SelectedConfiguration);
                    try
                    {
                        JObject JSonConf = DeviceSelected.GetJson();
                        String SerializeData = JsonConvert.SerializeObject(JSonConf);
                        File.WriteAllText(SelectedConfiguration, SerializeData);
                        if (DeviceSelectedForProcess!=null)
                            DeviceSelectedForProcess.IsSelected = false;
                        
                        DeviceSelected.IsSelected = true;
                        DeviceSelectedForProcess = DeviceSelected;
                    }
                    catch
                    {

                    }
                }
            }
            else
                DependencyService.Get<IToastMessage>().Show("Select a configuration from list");
        }

        private async void EditConfiguration()
        {
            if (DeviceSelected!=null)
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("filename", DeviceSelected.FileName);
                await NavigationService.NavigateAsync("/DeviceConf_NameAndModel", navigationParams);
            }
            else
                DependencyService.Get<IToastMessage>().Show("Select a configuration from list");
        }

        private bool CanExecute()
        {
            return true;
        }

        private void DeleteConfiguration()
        {
            if (DeviceSelected != null)
            {
                
                if (DeviceSelected.IsSelected)
                { 
                    var SelectedConfiguration = Path.Combine(RootFileConfSelected, "selected.qrc");
                    File.Delete(SelectedConfiguration);
                    DeviceSelectedForProcess = null;
                }

                FileDeviceList.Remove(DeviceSelected);
                var ConfigurationToDelete = Path.Combine(RootFileConfiguration, DeviceSelected.FileName + ".qrc");
                File.Delete(ConfigurationToDelete);
                DeviceSelected = null;
            }
            else
                DependencyService.Get<IToastMessage>().Show("Select a configuration from list");
        }
 
        
        private async void NewConfiguration()
        {
            await NavigationService.NavigateAsync("/DeviceConf_CommModelSelection");
        }

        private void ExportConfiguration()
        {
            if (DeviceSelected != null)
            {
                var SelectedConfiguration = Path.Combine(RootFileConfiguration, DeviceSelected.FileName + ".qrc");
                String JsonContent = File.ReadAllText(SelectedConfiguration);
                
                
                string pathDestination = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;


                var dateTimePrefix = DateTime.Now.ToString("yyMMddhhmmss");
                var newFileName = GiveNewFileName(pathDestination, DeviceSelected.FileName + "_" + dateTimePrefix);
                var fileConfigurationPath = Path.Combine(pathDestination, newFileName + ".qrc");

                try
                {          
                    File.WriteAllText(fileConfigurationPath, JsonContent);
                }
                catch (Exception ex)
                {
                    DependencyService.Get<IToastMessage>().Show($"Error during save file {DeviceSelected.FileName}.qrc in download folder.");
                }

                DependencyService.Get<IToastMessage>().Show($"The configuration has been successfully exported to the download folder of your mobile device with name {newFileName}.qrc.");

            }
            else
                DependencyService.Get<IToastMessage>().Show("Select a configuration from list");
        }

        private async void ImportConfiguration()
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    
                    if (result.FileName.EndsWith("qrc", StringComparison.OrdinalIgnoreCase))
                    {
                        var stream = await result.OpenReadAsync();
                        StreamReader reader = new StreamReader(stream);
                        string serialQrCodeConf = reader.ReadToEnd();

                        var deviceConf = ConvertFileToQrCodeConf(serialQrCodeConf);

                        string fileName = result.FileName;
                        string fileNameWithoutExtension = fileName.Substring(0, fileName.IndexOf(".qrc"));


                        //Controlla se esiste già una configurazione con lo stesso nome e se esiste crea un nuovo nome assegnandolo alla configurazione.
                        var newFileName = GiveNewFileName(RootFileConfiguration, fileNameWithoutExtension);
                        deviceConf.FileName = newFileName;

                        JObject JSonConf = deviceConf.GetJsonQRScanDevice();

                        String serializeData = JsonConvert.SerializeObject(JSonConf);

                        var pathNewFileConfiguration = Path.Combine(RootFileConfiguration, newFileName + ".qrc");

                        try
                        {
                            File.WriteAllText(pathNewFileConfiguration, serializeData);
                        }
                        catch
                        {
                            throw new Exception("Error during create new file configuration.");
                        }

                        FileDeviceList.Clear();
                        PopulateFileDeviceList();
                        DependencyService.Get<IToastMessage>().Show($"The configuration file has been imported successfully.");
                    }
                    else
                    {
                        DependencyService.Get<IToastMessage>().Show($"The selected file does not have the qrc extension.");
                    }
                }

            }
            catch (Exception ex)
            {
                DependencyService.Get<IToastMessage>().Show($"Error during open file to import.");
            }

        }

        private static string GiveNewFileName(string destinationFolder,string fileNameWithoutExtension)
        {
            var newFileConfiguration = Path.Combine(destinationFolder, fileNameWithoutExtension + ".qrc");

            while (File.Exists(newFileConfiguration))
            {
                var rnd = new Random();
                int positionUnderScore = fileNameWithoutExtension.IndexOf("_");
                if (positionUnderScore != -1)
                    fileNameWithoutExtension = fileNameWithoutExtension.Substring(0, positionUnderScore);

                fileNameWithoutExtension = fileNameWithoutExtension + $"_{rnd.Next(1000)}";

                newFileConfiguration = Path.Combine(destinationFolder, fileNameWithoutExtension + ".qrc");
            }

            return fileNameWithoutExtension;
        }



        private static QRScanDevice ConvertFileToQrCodeConf(string serialQrCodeConf)
        {
            try
            {
                JObject JSonConfiguration = JObject.Parse(serialQrCodeConf);
                return new QRScanDevice(JSonConfiguration);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
