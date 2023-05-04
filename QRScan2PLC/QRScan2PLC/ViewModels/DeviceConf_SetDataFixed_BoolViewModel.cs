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
    public class DeviceConf_SetDataFixed_BoolViewModel : BindableBase, IRegionAware
    {
        public DelegateCommand VisibleSelectCmd { get; private set; }

        public DelegateCommand ValueSelectCmd { get; private set; }

        private SetDataFixed<bool> _property;
        public SetDataFixed<bool> Property { get => _property; set => SetProperty(ref _property, value); }

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

        private int _ValueTypeSelected;

        public int ValueTypeSelected
        {
            get { return _ValueTypeSelected; }
            set { SetProperty(ref _ValueTypeSelected, value); }
        }


        public DeviceConf_SetDataFixed_BoolViewModel()
        {
            VisibleSelectCmd = new DelegateCommand(VisibleSelectAction);
            ValueSelectCmd = new DelegateCommand(ValueSelectAction);
        }

        private void ValueSelectAction()
        {
            Property.Value = (BooleanType)ValueTypeSelected == BooleanType.True ? true : false;
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
            Property = (SetDataFixed<bool>)propertyFactory;
            VisibleTypeSelected = Property.Visible ? 1 : 0;
            ValueTypeSelected = Property.Value ? 1 : 0;
        }
    }
}
