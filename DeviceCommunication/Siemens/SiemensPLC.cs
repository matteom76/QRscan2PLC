using DeviceCommunication.Enums.Siemens;
using DeviceCommunication.Interfaces;
using DeviceCommunication.Utils;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceCommunication.Siemens
{
    public class SiemensPLC : Notifier,ICommunicationDevice,IDisposable
    {
        private const int READTIMEOUT = 3000;
        private const int WRITETIMEOUT = 3000;
        private const int SCANVALUE = 1000;
        private const int CHECKCONNECTION = 3000;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        private static System.Timers.Timer ScanPLCAddress;
        private static System.Timers.Timer CheckConnectionPLC;

        private readonly object ListCacheLock = new object();


        private List<Object> ValueToMonitoring;
        private List<Object> ValueToMonitoringCache;


        private bool _connectionstatus;
        public bool ConnectionStatus {
            get { return _connectionstatus; }
            set { _connectionstatus = value;OnPropertyChanged("ConnectionStatus"); }
        }
        
        public Plc PLC { get; private set; }

        public ErrorCode LastErrorCode { get; }
        private bool RefreshValueToMonitoringFlag = false;
        private bool CheckConnectionActive = false;

        public SiemensPLC(SiemensDataConnection dataConnection)
        {
            CpuType cpu;
            switch ((int)dataConnection.PLCType)
            {
                case (int)SiemensPLCType.S71200:
                    cpu = CpuType.S71200;
                    break;
                case (int)SiemensPLCType.S71500:
                    cpu = CpuType.S71500;
                    break;
                case (int)SiemensPLCType.S7300:
                    cpu = CpuType.S7300;
                    break;
                case (int)SiemensPLCType.S7400:
                    cpu = CpuType.S7400;
                    break;
                default:
                    cpu = CpuType.S71500;
                    break;
            }

            
            PLC = new Plc(cpu, dataConnection.IP, dataConnection.Rack, dataConnection.Slot);
            PLC.ReadTimeout = READTIMEOUT;
            PLC.WriteTimeout = WRITETIMEOUT;
        }


        public async Task Connect(IEnableFunctionsPLC addressEnableFunctionPLC)
        {
            try
            { 
                await PLC.OpenAsync(new System.Threading.CancellationToken());
                ConnectionStatus = true;
                log.Info("Connection established with PLC.");
            }
            catch
            {
                ConnectionStatus = false;
                log.Error("Connection establish error. The system retry to establish communication.");
            }

            
            ValueToMonitoring = new List<Object>();
            ValueToMonitoringCache = new List<object>();
            ValueToMonitoring.Add(addressEnableFunctionPLC);
            
            ScanPLCAddress = new System.Timers.Timer(SCANVALUE);
            ScanPLCAddress.Elapsed += MonitoringPLCValue;
            ScanPLCAddress.AutoReset = true;

            CheckConnectionPLC = new System.Timers.Timer(CHECKCONNECTION);
            CheckConnectionPLC.Elapsed += async (sender, e) => await RestablishConnection();
            CheckConnectionPLC.AutoReset = true;

            if (ConnectionStatus)
            { 
                ScanPLCAddress.Enabled = true;
                ScanPLCAddress.Start();
            }
            else
            {
                CheckConnectionPLC.Enabled = true;
                CheckConnectionPLC.Start();
            }
        }

        private async Task RestablishConnection()
        {
            CheckConnectionActive = true;
            CheckConnectionPLC.Stop();
            CancellationTokenSource s_cts = new CancellationTokenSource();
            try
            {
                s_cts.CancelAfter(READTIMEOUT);
                PLC.Close();
                await PLC.OpenAsync(s_cts.Token);

                ConnectionStatus = true;
                log.Info("Attempt to restablish communication with PLC OK.");
            }
            catch
            {
                ConnectionStatus = false;
                log.Error("The attempt to restablish communication failed. Error details:" + LastErrorCode);
            }
            finally
            {
                s_cts.Dispose();
            }

            if (ConnectionStatus)
            {
                
                CheckConnectionPLC.Enabled = false;                                
                ScanPLCAddress.Enabled = true;
                ScanPLCAddress.Start();
                CheckConnectionActive = false;
            }
            else
            { 
                CheckConnectionPLC.Start();
            }
        }

        public void Disconnect()
        {
            ScanPLCAddress.Stop();
            CheckConnectionPLC.Stop();
            ScanPLCAddress.Dispose();
            CheckConnectionPLC.Dispose();
            ValueToMonitoring.Clear();
            ValueToMonitoringCache.Clear();
            PLC.Close();
        }

        private void MonitoringPLCValue(Object source, System.Timers.ElapsedEventArgs e)
        {
            ScanPLCAddress.Stop();
            var tasks = new List<Task>();
            
            foreach (Object adr in ValueToMonitoring)
            {                
                tasks.Add(Task.Run(async () => {

                    CancellationTokenSource s_cts = new CancellationTokenSource();
                    try
                    {
                        s_cts.CancelAfter(READTIMEOUT);
                        if (adr is IAddressBoolean)
                        {
                            if (await ReadBool((IAddressBoolean)adr, s_cts.Token))
                            {
                                throw new Exception("Read error");
                            }
                        }
                        else
                        if (adr is IAddressInteger)
                        {
                            if (await ReadInteger((IAddressInteger)adr, s_cts.Token))
                            {
                                throw new Exception("Read error");
                            }
                        }
                        else
                        if (adr is IAddressReal)
                        {
                            if (await ReadReal((IAddressReal)adr, s_cts.Token))
                            {
                                throw new Exception("Read error");
                            }
                        }
                        else
                        if (adr is IAddressString)
                        {
                            if (await ReadString((IAddressString)adr, s_cts.Token))
                            {
                                throw new Exception("Read error");
                            }
                        }
                        else
                        if (adr is IEnableFunctionsPLC)
                        {
                            if (await ReadEnableFunctionsPLC((IEnableFunctionsPLC)adr, s_cts.Token))
                            {
                                throw new Exception("Read error");
                            }
                        }
                    }
                    catch (Exception e)
                    {                        
                        throw new Exception("Read error" + e.Message);                        
                    }
                    finally
                    {
                        s_cts.Dispose();
                    }

                }));                
            }
         
            Task t = Task.WhenAll(tasks);
            try
            {
                t.Wait();
            }
            catch { }

            if (t.Status == TaskStatus.RanToCompletion)
            {
                if (RefreshValueToMonitoringFlag)
                {

                    ValueToMonitoring.RemoveAll(r => r is not IEnableFunctionsPLC);

                    RefreshValueToMonitoringFlag = false;
                }

                foreach (object cacheValue in ValueToMonitoringCache)
                {
                    if (!ValueToMonitoring.Contains(cacheValue))
                        ValueToMonitoring.Add(cacheValue);
                }

                ValueToMonitoringCache.Clear();               
                ScanPLCAddress.Start();

            }

            else if (t.Status == TaskStatus.Faulted)
            {
                ScanPLCAddress.Enabled = false;
                CheckConnectionPLC.Enabled = true;
                CheckConnectionPLC.Start();
            }

        }


        public void MonitoringValueBool(IAddressBoolean addressBoolean)
        {
            if (ValueToMonitoringCache != null)
            { 
                
            
                lock (ListCacheLock)
                {
                    ValueToMonitoringCache.Add(addressBoolean);
                }
                
            }
        }

        public void MonitoringValueInteger(IAddressInteger addressInteger)
        {
            if (ValueToMonitoringCache != null)
            {
                
                lock (ListCacheLock)
                {
                    ValueToMonitoringCache.Add(addressInteger);
                }
                
            }
        }

        public void MonitoringValueReal(IAddressReal addressReal)
        {
            if (ValueToMonitoringCache != null)
            {
                
                lock (ListCacheLock)
                {
                    ValueToMonitoringCache.Add(addressReal);
                }
               
            }
        }

        public void MonitoringValueString(IAddressString addressString)
        {
            if (ValueToMonitoringCache != null)
            {
                
                lock (ListCacheLock)
                {
                    ValueToMonitoringCache.Add(addressString);
                }
               
            }
        }

        private async Task<bool> ReadBool(IAddressBoolean addressBoolean, CancellationToken token)
        {       
            try
            {
                SiemensAddressBoolean adrBool = (SiemensAddressBoolean)addressBoolean;
                adrBool.ActualValue = (bool)await PLC.ReadAsync("DB" + adrBool.DB + ".DBX" + adrBool.Byte + "." + adrBool.Bit, token);
                addressBoolean.Error = false;
                
            }
            catch
            {
                addressBoolean.Error = true;
                return true;
            }
            return false;
        }

        private async Task<bool> ReadEnableFunctionsPLC(IEnableFunctionsPLC addressEnableFunctionPLC,CancellationToken token)
        {
            try
            {
                SiemensEnableFunctionsPLC adrBool = (SiemensEnableFunctionsPLC)addressEnableFunctionPLC;

                adrBool.ActualValue = (bool)await PLC.ReadAsync("DB" + adrBool.DB + ".DBX" + adrBool.Byte + "." + adrBool.Bit,token);
                addressEnableFunctionPLC.Error = false;

            }
            catch
            {
                addressEnableFunctionPLC.Error = true;
                return true;
            }
            return false;
        }

        private async Task<bool> ReadInteger(IAddressInteger addressInteger, CancellationToken token)
        {
            try
            {
                SiemensAddressInteger adrInt = (SiemensAddressInteger)addressInteger;
                adrInt.ActualValue = (ushort)await PLC.ReadAsync(S7.Net.DataType.DataBlock, adrInt.DB, adrInt.Byte, VarType.Word, 1,0,token);
                addressInteger.Error = false;
            }
            catch
            {
                addressInteger.Error = true;
                return true;
            }
            return false;
        }

        private async Task<bool> ReadReal(IAddressReal addressReal, CancellationToken token)
        {
            try
            {
                SiemensAddressReal adrReal = (SiemensAddressReal)addressReal;
                adrReal.ActualValue = (float)await PLC.ReadAsync(S7.Net.DataType.DataBlock, adrReal.DB, adrReal.Byte, VarType.Real, 1,0, token);
                addressReal.Error = false;
            }
            catch
            {
                addressReal.Error= true;
                return true;
            }
            return false;
        }

        private async Task<bool> ReadString(IAddressString addressString, CancellationToken token)
        {
            try
            {
                SiemensAddressString adrString = (SiemensAddressString)addressString;
                adrString.ActualValue = (string)await PLC.ReadAsync(S7.Net.DataType.DataBlock, adrString.DB, adrString.Byte, VarType.S7String, adrString.Lenght,0, token);
                addressString.Error = false;
            }
            catch
            {
                addressString.Error = true;
                return true;
            }
            return false;
        }

        public async Task WriteBool(IAddressBoolean addressBoolean, bool value)
        {
            if (!CheckConnectionActive)
                ScanPLCAddress.Stop();

            CancellationTokenSource s_cts = new CancellationTokenSource();

            try
            {
                s_cts.CancelAfter(WRITETIMEOUT);
                SiemensAddressBoolean adrBool = (SiemensAddressBoolean)addressBoolean;
                await PLC.WriteAsync(S7.Net.DataType.DataBlock, adrBool.DB, adrBool.Byte, (bool)value, adrBool.Bit,s_cts.Token);                
                addressBoolean.Error = false;
            }
            catch
            {                
                addressBoolean.Error = true;
            }
            finally
            {
                s_cts.Dispose();
            }

            if (!CheckConnectionActive)
                ScanPLCAddress.Start();

        }

        public async Task WriteInteger(IAddressInteger addressInteger, ushort value)
        {

            if (!CheckConnectionActive)
                ScanPLCAddress.Stop();

            CancellationTokenSource s_cts = new CancellationTokenSource();

            try
            {
                s_cts.CancelAfter(WRITETIMEOUT);
                SiemensAddressInteger adrInt = (SiemensAddressInteger)addressInteger;
                await PLC.WriteAsync(S7.Net.DataType.DataBlock, adrInt.DB, adrInt.Byte, (ushort)value,-1, s_cts.Token);
                addressInteger.Error = false;
            }
            catch
            {
                addressInteger.Error = true;
            }
            finally
            {
                s_cts.Dispose();
            }

            if (!CheckConnectionActive)
                ScanPLCAddress.Start();
        }

        public async Task WriteReal(IAddressReal addressReal, float value)
        {
            if (!CheckConnectionActive)
                ScanPLCAddress.Stop();

            CancellationTokenSource s_cts = new CancellationTokenSource();

            try
            {
                s_cts.CancelAfter(WRITETIMEOUT);
                SiemensAddressReal adrFloat = (SiemensAddressReal)addressReal;
                await PLC.WriteAsync(S7.Net.DataType.DataBlock, adrFloat.DB, adrFloat.Byte, (float)value,-1, s_cts.Token);
                addressReal.Error = false;
            }
            catch
            {
                addressReal.Error = true;
            }
            finally
            {
                s_cts.Dispose();
            }

            if (!CheckConnectionActive)
                ScanPLCAddress.Start();
        }

        public async Task WriteString(IAddressString addressString, string value)
        {

            if (!CheckConnectionActive)
                ScanPLCAddress.Stop();

            CancellationTokenSource s_cts = new CancellationTokenSource();

            try
            {
                s_cts.CancelAfter(WRITETIMEOUT);
                SiemensAddressString adrString = (SiemensAddressString)addressString;
                byte[] valueConvert = StringConvertToByteArray(value, adrString.Lenght);
                if (valueConvert.Length > 0)
                {
                    await PLC.WriteAsync(S7.Net.DataType.DataBlock, adrString.DB, adrString.Byte, valueConvert,-1, s_cts.Token);
                    addressString.Error = false;
                }
                else
                {
                    addressString.Error = true;
                }
            }
            catch
            {
                addressString.Error = true;
            }
            finally
            {
                s_cts.Dispose();
            }

            if (!CheckConnectionActive)
                ScanPLCAddress.Start();
        }

        private byte[] StringConvertToByteArray(string value, int maxlenght)
        {
            byte[] setting = new byte[maxlenght + 2];
            setting[0] = (byte)maxlenght;
            setting[1] = (byte)value.Length;
            byte[] values = S7.Net.Types.String.ToByteArray(value, maxlenght);
            values.CopyTo(setting, 2);
            return setting;
        }

        public void Dispose()
        {
            if (ScanPLCAddress!=null && ScanPLCAddress.Enabled)
            { 
                ScanPLCAddress.Stop();
                try
                { 
                    ScanPLCAddress.Dispose();
                }
                catch
                { }
            }
            
            if (CheckConnectionPLC!=null && CheckConnectionPLC.Enabled)
            { 
                CheckConnectionPLC.Stop();
                try
                { 
                    CheckConnectionPLC.Dispose();
                }
                catch
                { }
            }
            
        }

        public void RefreshValueToMonitoring()
        {

            if (!CheckConnectionActive)
                ScanPLCAddress.Stop();

            RefreshValueToMonitoringFlag = true;

            lock (ListCacheLock)
            {
                if (ValueToMonitoringCache != null)
                    ValueToMonitoringCache.Clear();
            }

            if (!CheckConnectionActive)
                ScanPLCAddress.Start();

        }

        public void StopScanningForcing()
        {
            if (ScanPLCAddress != null && ScanPLCAddress.Enabled)
                ScanPLCAddress.Stop();

            if (CheckConnectionPLC != null && CheckConnectionPLC.Enabled)
                CheckConnectionPLC.Stop();
        }
    }
}
