using DeviceCommunication.Enums;
using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class SetDataFromCode : IProperties
    {
        private int _positionStart;
        private int _lenght;
        private int _decimalNumber;
        private bool _visible;
        private DataType _DataType;

        public int PositionStart { get => _positionStart; set => _positionStart = value; }
        public int Lenght { get => _lenght; set => _lenght = value; }
        public int DecimalNumber { get => _decimalNumber; set => _decimalNumber = value; }
        public bool Visible { get => _visible; set => _visible = value; }
        public DataType DataType { get => _DataType; set => _DataType = value; }

        public SetDataFromCode(int positionStart, int lenght, int decimalNumber, bool visible,DataType dataType)
        {
            PositionStart = positionStart;
            Lenght = lenght;
            DecimalNumber = decimalNumber;
            Visible = visible;
            DataType = dataType;
        }

        public SetDataFromCode(DataType dataType)
        {
            PositionStart = 0;
            Lenght = 0;
            DecimalNumber = 0;
            Visible = true;
            DataType = dataType;
        }

        public SetDataFromCode(JObject properties)
        {
            try
            {
                PositionStart = int.Parse(properties["positionstart"].ToString());
                Lenght = int.Parse(properties["lenght"].ToString());
                DecimalNumber = int.Parse(properties["decimal"].ToString());
                Visible = properties["visible"].ToString() == "True" ? true : false;
                DataType = (DataType)System.Enum.Parse(typeof(DataType), properties["DataType"].ToString(), true);

            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing properties SetDataFromCode. More details:" + ex.ToString());
            }
        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("positionstart", PositionStart),
                    new JProperty("lenght", Lenght),
                    new JProperty("decimal", DecimalNumber),
                    new JProperty("visible", Visible ? "True":"False"),
                    new JProperty("DataType", DataType)
                );

            return obj;
        }
    }
}
