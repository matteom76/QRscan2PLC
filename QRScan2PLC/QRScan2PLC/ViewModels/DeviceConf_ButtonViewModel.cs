using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Enums;
using QRScan2PLC.Interfaces;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class DeviceConf_ButtonViewModel : BindableBase,IRegionAware
    {
        public DelegateCommand ButtonModeSelectCmd { get; private set; }

        private Button _property;
        public  Button Property { get => _property; set => SetProperty(ref _property, value); }

        public List<string> ButtonMode
        {
            get { return Enum.GetNames(typeof(ButtonMode)).ToList(); }
        }

        private int _ButtonModeSelected;

        public int ButtonModeSelected
        {
            get { return _ButtonModeSelected; }
            set { SetProperty(ref _ButtonModeSelected, value); }
        }


        public DeviceConf_ButtonViewModel()
        {
            ButtonModeSelectCmd = new DelegateCommand(ButtonModeSelect);
        }

        private void ButtonModeSelect()
        {
            Property.ButtonMode = (ButtonMode)ButtonModeSelected;
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
            Property = (Button)propertyFactory;
            ButtonModeSelected = (int)Property.ButtonMode;           
        }
    }
}
