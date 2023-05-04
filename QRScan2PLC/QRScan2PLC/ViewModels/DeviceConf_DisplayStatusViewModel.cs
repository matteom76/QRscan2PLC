using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using Prism.Regions.Navigation;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_DisplayStatusViewModel : BindableBase, IRegionAware
    {

        private IRegionManager _regionManager { get; }

        private DisplayStatus _property;
        public DisplayStatus Property { get => _property; set => SetProperty(ref _property, value); }


        private bool _DisplayConfirmEditStatus;
        public bool DisplayConfirmEditStatus { get => _DisplayConfirmEditStatus; set => SetProperty(ref _DisplayConfirmEditStatus, value); }

        private StatusCode StatusPrev;


        public DelegateCommand EditStatusCmd { get; private set; }
        public DelegateCommand NewStatusCmd { get; private set; }
        public DelegateCommand DeleteStatusCmd { get; private set; }
        public DelegateCommand ConfirmStatusCmd { get; private set; }
        public DelegateCommand CancelStatusCmd { get; private set; }

        public DeviceConf_DisplayStatusViewModel(IRegionManager regionManager)
        {
            EditStatusCmd = new DelegateCommand(EditStatus);
            NewStatusCmd = new DelegateCommand(NewStatus);
            DeleteStatusCmd = new DelegateCommand(DeleteStatus);
            ConfirmStatusCmd = new DelegateCommand(ConfirmStatus);
            CancelStatusCmd = new DelegateCommand(CancelStatus);
            _regionManager = regionManager;
        }

        private void CancelStatus()
        {

            if (StatusPrev!=null)
            { 
                Property.StatusSelected.Value = StatusPrev.Value;
                Property.StatusSelected.Description = StatusPrev.Description;
            }
            
            var PropertyParameters = new NavigationParameters();
            PropertyParameters.Add("statuslist", Property);

            _regionManager.RequestNavigate("StatusRegion", "DeviceConf_DisplayStatus_View", navigationCallback, PropertyParameters);
            DisplayConfirmEditStatus = false;
        }

        private void ConfirmStatus()
        {
            if (!Property.StatusCodeList.Contains(Property.StatusSelected))
                Property.StatusCodeList.Add(Property.StatusSelected);
            
            var PropertyParameters = new NavigationParameters();
            PropertyParameters.Add("statuslist", Property);

            _regionManager.RequestNavigate("StatusRegion", "DeviceConf_DisplayStatus_View", navigationCallback, PropertyParameters);

            DisplayConfirmEditStatus = false;
        }

        private void DeleteStatus()
        {
            if (Property.StatusSelected != null)
            {
                Property.StatusCodeList.Remove(Property.StatusSelected);
                Property.StatusSelected = null;
            }
            else
            {
                DependencyService.Get<IToastMessage>().Show("Select a status from list");
            }
        }
        private void NewStatus()
        {
            Property.StatusSelected = null;
            var PropertyParameters = new NavigationParameters();
            PropertyParameters.Add("statuslist", Property);

            _regionManager.RequestNavigate("StatusRegion", "DeviceConf_DisplayStatus_Edit", navigationCallback, PropertyParameters);
            DisplayConfirmEditStatus = true;
        }

        private void EditStatus()
        {
           if (Property.StatusSelected!=null)
           {
                StatusPrev = new StatusCode(Property.StatusSelected.Value,Property.StatusSelected.Description);
                var PropertyParameters = new NavigationParameters();
                PropertyParameters.Add("statuslist", Property);

                _regionManager.RequestNavigate("StatusRegion", "DeviceConf_DisplayStatus_Edit", navigationCallback, PropertyParameters);
                DisplayConfirmEditStatus = true;
           }
           else
           {
                DependencyService.Get<IToastMessage>().Show("Select a status from list");
           }
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
            Property = (DisplayStatus)propertyFactory;

            var PropertyParameters = new NavigationParameters();
            PropertyParameters.Add("statuslist", Property);

            _regionManager.RequestNavigate("StatusRegion", "DeviceConf_DisplayStatus_View", navigationCallback, PropertyParameters);
            DisplayConfirmEditStatus = false;
        }

        private void navigationCallback(IRegionNavigationResult obj)
        {
            Console.WriteLine(obj.ToString());
        }
    }
}
