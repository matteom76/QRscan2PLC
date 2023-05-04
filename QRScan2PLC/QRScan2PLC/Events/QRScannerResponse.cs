using Prism.Events;
using QRScan2PLC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Events
{
    public class QRScannerResponse: PubSubEvent<CodeReadData>
    {
    }
}
