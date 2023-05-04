using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceCommunication.Interfaces
{
    public interface ICommunicationDevice
    {
        Task Connect(IEnableFunctionsPLC addressEnableFunctionPLC);
        void Disconnect();
        bool ConnectionStatus { get; }
        Task WriteBool(IAddressBoolean addressBoolean,bool value);
        Task WriteInteger(IAddressInteger addressInteger,ushort value);
        Task WriteReal(IAddressReal addressReal,float value);
        Task WriteString(IAddressString addressString,string value);
        void MonitoringValueBool(IAddressBoolean addressBoolean);
        void MonitoringValueInteger(IAddressInteger addressInteger);
        void MonitoringValueReal(IAddressReal addressReal);
        void MonitoringValueString(IAddressString addressString);
        void RefreshValueToMonitoring();
        void StopScanningForcing();
    }
}
