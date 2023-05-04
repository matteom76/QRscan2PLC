using Newtonsoft.Json.Linq;
using QRScan2PLC.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models.Properties
{
    public class DisplayValue : IProperties
    {
        private string _description;
        private string _uom;

        public string Description { get { return _description; } set { _description = value; } }
        public string UOM { get { return _uom; } set { _uom = value; } }

        public DisplayValue(string description,string uom)
        {
            Description = description;
            UOM = uom;
        }

        public DisplayValue(JObject properties)
        {
            try
            {                
                Description = properties["description"].ToString();
                UOM = properties["UOM"].ToString();
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing properties display value. More details:" + ex.ToString());
            }
        }

        public JObject GetJsonObject()
        {
            JObject obj =
                new JObject(
                    new JProperty("description", Description),
                    new JProperty("UOM", UOM)
                );

            return obj;
        }
    }
}
