using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRScan2PLC.Models
{
    public class QRTypeCondition
    {
        private int _qrstringlenght;
        private String _substring;
        private int _substringpos;

        public int qrstringlenght
        {
            get { return _qrstringlenght; }
            set
            {
                _qrstringlenght = value;
            }
        }

        public String substring
        {
            get { return _substring; }
            set
            {
                _substring = value;
            }
        }
        public int substringpos
        {
            get { return _substringpos; }
            set
            {
                _substringpos = value;
            }
        }

        public QRTypeCondition(JObject JSonCondition)
        {
            try
            {
                qrstringlenght = int.Parse(JSonCondition["qrstringlenght"].ToString());
                substring = JSonCondition["substring"].ToString();
                substringpos = int.Parse(JSonCondition["substringpos"].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:Parsing condition of parameter. More details:" + ex.ToString());
            }

        }

        public QRTypeCondition()
        {
            qrstringlenght = 0;
            substring = "";
            substringpos = 0;
        }

        public JObject GetJsonQRCondition()
        {

            JObject obj =
               new JObject(
                   new JProperty("qrstringlenght", qrstringlenght),
                   new JProperty("substring", substring),
                   new JProperty("substringpos", substringpos)
               );
            return obj;
        }
    }
}
