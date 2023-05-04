using DeviceCommunication.Enums;
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
    public class DeviceConf_SetDataFromCodeViewModel : BindableBase, IRegionAware
    {
        public DelegateCommand VisibleSelectCmd { get; private set; }

        private SetDataFromCode _property;
        public SetDataFromCode Property { get => _property; set => SetProperty(ref _property, value); }

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

        private bool _DecimalNumberVisible;
        public bool DecimalNumberVisible { get => _DecimalNumberVisible; set => SetProperty(ref _DecimalNumberVisible, value); }


        public DeviceConf_SetDataFromCodeViewModel()
        {
            VisibleSelectCmd = new DelegateCommand(VisibleSelectAction);
        }

        private void VisibleSelectAction()
        {
            Property.Visible = (BooleanType)VisibleTypeSelected == BooleanType.True ? true : false;
        }

        public void OnNavigatedTo(INavigationContext navigationContext)
        {
            var propertyFactory = navigationContext.Parameters.GetValue<IProperties>("property");
            Property = (SetDataFromCode)propertyFactory;
            VisibleTypeSelected = Property.Visible ? 1 : 0;
            DecimalNumberVisible = Property.DataType != DataType.Real ? false : true;

            
        }

        public bool IsNavigationTarget(INavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(INavigationContext navigationContext)
        {

        }
    }
}
