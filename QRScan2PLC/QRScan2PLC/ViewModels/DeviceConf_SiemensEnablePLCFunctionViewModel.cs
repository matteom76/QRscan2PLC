using DeviceCommunication.Siemens;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_SiemensEnablePLCFunctionViewModel : BindableBase,IRegionAware
    {

        private SiemensEnableFunctionsPLC _siemensEnableFunctionsPLC;
        public SiemensEnableFunctionsPLC EnableFunctionsPLC { get => _siemensEnableFunctionsPLC; set => SetProperty(ref _siemensEnableFunctionsPLC, value); }
        
        public DeviceConf_SiemensEnablePLCFunctionViewModel()
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
            EnableFunctionsPLC = navigationContext.Parameters.GetValue<SiemensEnableFunctionsPLC>("EnablePLCInfo");
        }
    }
}
