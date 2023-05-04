using DeviceCommunication.Enums.Siemens;
using DeviceCommunication.Siemens;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_SiemensPLCInfoViewModel : BindableBase,IRegionAware
    {

        public DelegateCommand PLCTypeSelectCmd { get; private set; }
        private SiemensDataConnection _dataConnection;
        public SiemensDataConnection DataConnection
        {
            get { return _dataConnection; }
            set { SetProperty(ref _dataConnection, value); }
        }

        public List<string> PLCSiemensType
        {
            get { return Enum.GetNames(typeof(SiemensPLCType)).ToList(); }
        }

        private int _PLCTypeSelected;
        
        public int PLCTypeSelected
        {
            get { return _PLCTypeSelected; }
            set { SetProperty(ref _PLCTypeSelected, value);}
        }

        public DeviceConf_SiemensPLCInfoViewModel()
        {
            PLCTypeSelectCmd = new DelegateCommand(PLCtypeSelectAction);
        }

        private void PLCtypeSelectAction()
        {
            DataConnection.PLCType = (SiemensPLCType)PLCTypeSelected;
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
            DataConnection = navigationContext.Parameters.GetValue<SiemensDataConnection>("DataConnection");
            PLCTypeSelected = (int)DataConnection.PLCType;
        }






    }


}
