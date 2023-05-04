using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_DisplayValueViewModel : BindableBase, IRegionAware
    {
        private DisplayValue _property;
        public DisplayValue Property { get => _property; set => SetProperty(ref _property, value); }


        public DeviceConf_DisplayValueViewModel()
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
            var propertyFactory = navigationContext.Parameters.GetValue<IProperties>("property");
            Property = (DisplayValue)propertyFactory;
        }
    }
}
