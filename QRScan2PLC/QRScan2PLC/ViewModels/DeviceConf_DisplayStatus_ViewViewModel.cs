using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_DisplayStatus_ViewViewModel : BindableBase, IRegionAware
    {

        private DisplayStatus _property;
        public DisplayStatus Property { get => _property; set => SetProperty(ref _property, value); }



        public DeviceConf_DisplayStatus_ViewViewModel()
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
            var statusList = navigationContext.Parameters.GetValue<DisplayStatus>("statuslist");
            Property = statusList;
        }
    }
}
