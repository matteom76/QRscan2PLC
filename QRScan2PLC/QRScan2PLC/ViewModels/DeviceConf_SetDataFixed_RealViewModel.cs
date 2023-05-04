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
    public class DeviceConf_SetDataFixed_RealViewModel : BindableBase, IRegionAware
    {
        public DelegateCommand VisibleSelectCmd { get; private set; }

        private SetDataFixed<double> _property;
        public SetDataFixed<double> Property { get => _property; set => SetProperty(ref _property, value); }

        public List<string> VisibleType
        {
            get { return Enum.GetNames(typeof(BooleanType)).ToList(); }
        }

        private int _VisibleTypeSelected;

        public int VisibleTypeSelected
        {
            get { return _VisibleTypeSelected; }
            set { SetProperty(ref _VisibleTypeSelected, value); }
        }


        public DeviceConf_SetDataFixed_RealViewModel()
        {
            VisibleSelectCmd = new DelegateCommand(VisibleSelectAction);
        }

        private void VisibleSelectAction()
        {
            Property.Visible = (BooleanType)VisibleTypeSelected == BooleanType.True ? true : false;
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
            Property = (SetDataFixed<double>)propertyFactory;
            VisibleTypeSelected = Property.Visible ? 1 : 0;
        }
    }
}
