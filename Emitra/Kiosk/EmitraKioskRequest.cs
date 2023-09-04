using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS
{
    public class EmitraKioskRequest
    {
        public string BASEURL { get;set; }
        public Int16 SERVICERESPONSETIME { get; set; }
        public string VERIFICAION_URL { get; set; }
        
        public string MERCHANTCODE { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string SERVICEID { get; set; }
        public string SERVICENAME { get; set; }
        public string SUBSERVICEID { get; set; }
        public string REVENUEHEAD { get; set; }
        public string CONSUMERKEY { get; set; }
        public string CONSUMERNAME { get; set; }
        public string SSOID { get; set; }
        public string OFFICECODE { get; set; }
        public string COMMTYPE { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }
        public string MSG { get; set; }
        public string REQUESTID { get; set; }
        public string ENCRYPTIONKEY { get; set; }
        public string RETURNURL { get; set; }
        public string SUCCESSURL { get; set; }
        public string FAILUREURL { get; set; }
        public string AMOUNT { get; set; }
    }
    public class EmitraKioskChecksum {
        public string SSOID { get; set; }
        public string REQUESTID { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string SSOTOKEN { get; set; }
    }

    public class EmitraKioskVerifyTransactionRequest
    {
        public string MERCHANTCODE { get; set; }
        public string SERVICEID { get; set; }
        public string REQUESTID { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }
    }

    public class EmitraKioskVerifyTransactionChecksum
    {
        public string MERCHANTCODE { get; set; }
        public string REQUESTID { get; set; }      
        public string SSOTOKEN { get; set; }
    }
}