using DeviceCommunication;
using DeviceCommunication.Interfaces;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Utils
{
    public class DataFixedProcessing<T>
    {
        private SetDataFixed<T> DataFixed;
        private ICommunicationDevice Device;

        public DataFixedProcessing(ICommunicationDevice device, SetDataFixed<T> dataFixed)
        {
            DataFixed = dataFixed;
            Device = device;
        }

        public void WriteDataToPLC(DeviceTag tagAdr)
        {
            Type valueType = typeof(T);
            if (valueType==typeof(bool))
            {
                bool valueBool = (bool)Convert.ChangeType(DataFixed.Value, typeof(bool));
                Device.WriteBool(tagAdr.addressBoolean, valueBool);
                if (tagAdr.addressBoolean.Error)
                    throw new Exception("Write boolean fixed value to PLC error.");
            }
            else
                if (valueType==typeof(int))
                {
                    int valueInt = (int)Convert.ChangeType(DataFixed.Value, typeof(int));
                    ushort valueUshort = Convert.ToUInt16(valueInt);
                    Device.WriteInteger(tagAdr.addressInteger, valueUshort);
                    if (tagAdr.addressInteger.Error)
                        throw new Exception("Write integer fixed value to PLC error.");
                }
            else
                if (valueType == typeof(double))
                {
                    double valueDouble = (double)Convert.ChangeType(DataFixed.Value, typeof(double));
                    float valueFloat = Convert.ToSingle(valueDouble);
                    Device.WriteReal(tagAdr.addressReal, valueFloat);
                    if (tagAdr.addressReal.Error)
                        throw new Exception("Write float fixed value to PLC error.");
                }
            else
               if (valueType == typeof(string))
               {
                    string valueString = (string)Convert.ChangeType(DataFixed.Value, typeof(string));
                    Device.WriteString(tagAdr.addressString, valueString);
                    if (tagAdr.addressString.Error)
                        throw new Exception("Write string fixed value to PLC error.");
                }

        }

    }
}
