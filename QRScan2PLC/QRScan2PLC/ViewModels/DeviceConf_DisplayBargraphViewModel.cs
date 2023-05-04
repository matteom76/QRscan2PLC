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
    public class DeviceConf_DisplayBargraphViewModel : BindableBase, IRegionAware
    {
        public DelegateCommand ColorSelectCmd { get; private set; }

        private DisplayBargraph _property;
        public DisplayBargraph Property { get => _property; set => SetProperty(ref _property, value); }

        public List<string> ColorList
        {
            get { return Enum.GetNames(typeof(ColorsBargraph)).ToList(); }
        }

        private int _ColorSelected;

        public int ColorSelected
        {
            get { return _ColorSelected; }
            set { SetProperty(ref _ColorSelected, value); }
        }



        public DeviceConf_DisplayBargraphViewModel()
        {
            ColorSelectCmd = new DelegateCommand(ColorSelect);
        }

        private void ColorSelect()
        {
            Property.ColorBargraph = (ColorsBargraph)ColorSelected;
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
            Property = (DisplayBargraph)propertyFactory;
            ColorSelected = (int)Property.ColorBargraph;
           
        }
    }
}
