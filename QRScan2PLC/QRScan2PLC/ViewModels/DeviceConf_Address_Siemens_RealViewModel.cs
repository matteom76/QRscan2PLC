using DeviceCommunication;
using DeviceCommunication.Siemens;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_Address_Siemens_RealViewModel : BindableBase, IRegionAware
    {
        private SiemensAddressReal _Address;
        public SiemensAddressReal Address { get => _Address; set => SetProperty(ref _Address, value); }


        public DeviceConf_Address_Siemens_RealViewModel()
        {

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
            var addressFactory = navigationContext.Parameters.GetValue<DeviceTag>("addressTag");
            Address = (SiemensAddressReal)addressFactory.addressReal;
        }
    }
}
