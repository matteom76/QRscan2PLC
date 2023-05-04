using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_DisplayStatus_EditViewModel : BindableBase, IRegionAware
    {

        private DisplayStatus _StatusList;

        public DisplayStatus StatusList
        {
            get { return _StatusList; }
            set { SetProperty(ref _StatusList, value); }
        }


        public DeviceConf_DisplayStatus_EditViewModel()
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
            StatusList = navigationContext.Parameters.GetValue<DisplayStatus>("statuslist");
            if (StatusList.StatusSelected==null)
            {
                int value = -1;
                do
                {
                    value++;
                }
                while (StatusList.StatusCodeList.Count(q => q.Value == value) > 0);
                StatusList.StatusSelected = new StatusCode(value, "NONE");
            }
        }
    }
}
