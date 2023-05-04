using DeviceCommunication;
using DeviceCommunication.Enums;
using DeviceCommunication.Interfaces;
using QRScan2PLC.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Utils
{
    public class DataFromCodeProcessing: Notifier
    {
        private ICommunicationDevice Device;
        private SetDataFromCode DataFromCode;
        
        private string _DataResultToDisplay;
        public string DataResultToDisplay { get { return _DataResultToDisplay; } set { _DataResultToDisplay = value; OnPropertyChanged("DataResultToDisplay"); } }
    
        public DataFromCodeProcessing(ICommunicationDevice device,SetDataFromCode dataFromCode)
        {
            Device = device;
            DataFromCode = dataFromCode;
            DataResultToDisplay = "";
        }

        public string CheckInfoFromCode(string qrCode)
        {
            if (DataFromCode.PositionStart < qrCode.Length)
            {
                if ((DataFromCode.PositionStart + DataFromCode.Lenght)<=qrCode.Length)
                {
                    try
                    {
                        return qrCode.Substring(DataFromCode.PositionStart, DataFromCode.Lenght);                        
                    }
                    catch
                    {
                        throw new Exception("Data processing from qrcode failed.");
                    }
                }
                else
                {
                    throw new Exception("The length of the substring is greater than that of the qrcode.");
                }
            }
            else
                throw new Exception("Position start invalid.");
        }

        public void WriteDataToPLC(string data,DeviceTag tagAdr)
        {
            switch (DataFromCode.DataType)
            {
                case DataType.Integer:
                    ushort valueToWriteInt;
                    if (ushort.TryParse(data,out valueToWriteInt))
                    {
                        DataResultToDisplay = valueToWriteInt.ToString();
                        Device.WriteInteger(tagAdr.addressInteger, valueToWriteInt);
                        if (tagAdr.addressInteger.Error)
                            throw new Exception("Write integer value to PLC error.");
                    }
                    else
                    {
                        throw new Exception("Conversion to integer value failed.");
                    }                   
                    break;

                case DataType.Real:
                    float valueToWriteFloat;
                    if (float.TryParse(data, out valueToWriteFloat))
                    {
                        if (DataFromCode.DecimalNumber>0 && DataFromCode.DecimalNumber<4)
                        {
                            valueToWriteFloat = valueToWriteFloat / (float)Math.Pow(10, DataFromCode.DecimalNumber);
                        }

                        DataResultToDisplay = valueToWriteFloat.ToString();
                        Device.WriteReal(tagAdr.addressReal, valueToWriteFloat);
                        if (tagAdr.addressReal.Error)
                            throw new Exception("Write float value to PLC error.");
                    }
                    else
                    {
                        throw new Exception("Conversion to float value failed.");
                    }
                    break;

                case DataType.String:
                    DataResultToDisplay = data;
                    if (tagAdr.addressString.CheckStringLenght(data))
                    { 
                        Device.WriteString(tagAdr.addressString, data);
                        if (tagAdr.addressString.Error)
                            throw new Exception("Write string value to PLC error.");
                    }
                    else
                        throw new Exception("string value to PLC too large for the address.");
                    break;

            }
        }
    
    }
}
