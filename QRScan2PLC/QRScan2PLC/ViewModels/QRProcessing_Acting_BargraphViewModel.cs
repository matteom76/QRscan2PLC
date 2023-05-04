using DeviceCommunication.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Enums;
using QRScan2PLC.Models;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_Acting_BargraphViewModel : BindableBase, IRegionAware
    {
        private int _RangeLow;
        public int RangeLow { get => _RangeLow; set => SetProperty(ref _RangeLow, value); }

        private int _RangeHigh;
        public int RangeHigh { get => _RangeHigh; set => SetProperty(ref _RangeHigh, value); }

        private DisplayBargraph _displayBargraph;
        public DisplayBargraph displayBargraph { get => _displayBargraph; set => SetProperty(ref _displayBargraph, value); }

        private DataPLC _dataPLC;
        public DataPLC DataPLC { get => _dataPLC; set => SetProperty(ref _dataPLC, value); }

        private ICommunicationDevice _Device;
        public ICommunicationDevice Device { get => _Device; set => SetProperty(ref _Device, value); }


        private Color _ColorBargraph;
        public Color ColorBargraph { get => _ColorBargraph; set => SetProperty(ref _ColorBargraph, value); }

        public QRProcessing_Acting_BargraphViewModel(ICommunicationDevice device)
        {
            Device = device;
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
            RangeLow = 0;
            RangeHigh = 0;
            ColorBargraph = GetColorBargraph(ColorsBargraph.Blue);
            DataPLC = navigationContext.Parameters.GetValue<DataPLC>("DataPLC");
            if (DataPLC != null)
            {
                displayBargraph = (DisplayBargraph)DataPLC.Acting.Property;
                RangeLow = displayBargraph.RangeLow;
                RangeHigh = displayBargraph.RangeHigh;
                ColorBargraph = GetColorBargraph(displayBargraph.ColorBargraph);
                Device.MonitoringValueReal(DataPLC.DeviceAdr.addressReal);                
            }
        }

        private Color GetColorBargraph(ColorsBargraph colorCode)
        {
            switch (colorCode)
            {
                case ColorsBargraph.Blue:
                    return Color.FromRgb(0, 0, 255);
                case ColorsBargraph.Cyan:
                    return Color.FromRgb(0, 255, 255);
                case ColorsBargraph.Green:
                    return Color.FromRgb(0, 255, 0);
                case ColorsBargraph.Magenta:
                    return Color.FromRgb(255, 0, 255);
                case ColorsBargraph.Yellow:
                    return Color.FromRgb(255, 255, 0);
            }
            return Color.FromRgb(192, 192, 192);
        }
    }
}
