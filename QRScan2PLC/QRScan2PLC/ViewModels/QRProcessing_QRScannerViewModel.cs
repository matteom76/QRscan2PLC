
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions.Navigation;
using QRScan2PLC.Events;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRScan2PLC.ViewModels
{
    public class QRProcessing_QRScannerViewModel : BindableBase
    {

        public DelegateCommand<object> QRScanResultCmd { get; private set; }
        public DelegateCommand CancelCmd { get; private set; }

        private IEventAggregator _ea;


        public QRProcessing_QRScannerViewModel(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            QRScanResultCmd = new DelegateCommand<object>(QRScanResult);
            CancelCmd = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            _ea.GetEvent<QRScannerResponse>().Publish(new CodeReadData() { ResultCode="",Format=""});
        }

        private void QRScanResult(object result)
        {
            var resultScan = (ZXing.Result)result;            
            _ea.GetEvent<QRScannerResponse>().Publish(new CodeReadData() {ResultCode= resultScan.Text,Format=resultScan.BarcodeFormat.ToString() });
        }

    }
}
