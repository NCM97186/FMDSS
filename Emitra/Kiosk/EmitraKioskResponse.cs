using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS
{
    public class EmitraKioskResponse
    {

        public string RESPONSE { get; set; }
        public int TIMEELAPSED { get; set; }

        public string REQUESTID { get; set; }
        public string TRANSACTIONSTATUSCODE { get; set; }
        public string RECEIPTNO { get; set; }
        public string TRANSACTIONID { get; set; }
        public string TRANSAMT { get; set; }
        public string TRANSACTIONSTATUS { get; set; }
        public string MSG { get; set; }
        public string CHECKSUM { get; set; }
        public string EMITRATIMESTAMP { get; set; }

        public string AMT { get; set; }
        public string TRANSACTIONDATE { get; set; }
        public string SSOTOKEN { get; set; }
        public string USERNAME { get; set; }

        public string MERCHANTCODE { get; set; }
        public string SERVICEID { get; set; }
        public string CONSUMERKEY { get; set; }
        public string RETURNURL { get; set; }
        public string COMMTYPE { get; set; } 
    }
}