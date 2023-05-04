using DeviceCommunication.MQTT;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_MQTTBrokerInfoViewModel : BindableBase, IRegionAware
    {
        private MQTTDataConnection _dataConnection;
        public MQTTDataConnection DataConnection { get => _dataConnection; set => SetProperty(ref _dataConnection, value); }

        public DelegateCommand WithCredentialSelectCmd { get; private set; }
        public DelegateCommand WithTlsSelectCmd { get; private set; }

        public List<string> OptionSelectionList
        {
            get { return Enum.GetNames(typeof(OptionSelection)).ToList(); }
        }

        private int _WithCredentialSelected;

        public int WithCredentialSelected
        {
            get { return _WithCredentialSelected; }
            set { SetProperty(ref _WithCredentialSelected, value); }
        }

        private int _WithTlsSelected;

        public int WithTlsSelected
        {
            get { return _WithTlsSelected; }
            set { SetProperty(ref _WithTlsSelected, value); }
        }

        private void WithCredentialSelectAction()
        {
            DataConnection.WithCredentials = (OptionSelection)WithCredentialSelected == OptionSelection.Yes ? true : false;
        }

        private void WithTlsSelectAction()
        {
            DataConnection.WithTls = (OptionSelection)WithTlsSelected == OptionSelection.Yes ? true : false;
        }


        public DeviceConf_MQTTBrokerInfoViewModel()
        {
            WithCredentialSelectCmd = new DelegateCommand(WithCredentialSelectAction);
            WithTlsSelectCmd = new DelegateCommand(WithTlsSelectAction);
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
            DataConnection = navigationContext.Parameters.GetValue<MQTTDataConnection>("DataConnection");
            WithCredentialSelected = DataConnection.WithCredentials ? 1 : 0;
            WithTlsSelected = DataConnection.WithTls ? 1 : 0;            
        }
    }
}
