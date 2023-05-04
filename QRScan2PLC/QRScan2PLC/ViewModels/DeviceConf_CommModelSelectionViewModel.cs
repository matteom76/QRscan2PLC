using DeviceCommunication.Enums;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_CommModelSelectionViewModel : BindableBase
    {
        private INavigationService NavigationService;

        public DelegateCommand ModelSelectCmd { get; private set; }
        public DelegateCommand BackCmd { get; private set; }

        public List<string> ModelSelectionList
        {
            get { return Enum.GetNames(typeof(PLCModel)).ToList(); }
        }

        private int _DeviceModelSelected;

        public int DeviceModelSelected
        {
            get { return _DeviceModelSelected; }
            set { SetProperty(ref _DeviceModelSelected, value); }
        }

        private async void ModelSelectAction()
        {
            var navigationParams = new NavigationParameters();
            if ((PLCModel)DeviceModelSelected == PLCModel.MQTT)
            { 
                navigationParams.Add("deviceModel", PLCModel.MQTT);
                await NavigationService.NavigateAsync("/DeviceConf_NameAndModel",navigationParams);
            }
            else
            if ((PLCModel)DeviceModelSelected == PLCModel.Siemens)
            { 
                navigationParams.Add("deviceModel", PLCModel.Siemens);
                await NavigationService.NavigateAsync("/DeviceConf_NameAndModel",navigationParams);
            }            
        }


        public DeviceConf_CommModelSelectionViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            ModelSelectCmd = new DelegateCommand(ModelSelectAction);
            BackCmd = new DelegateCommand(BackToDeviceSelection);
        }

        private async void BackToDeviceSelection()
        {
            await NavigationService.NavigateAsync("/DeviceSelection");
        }
    }
}
