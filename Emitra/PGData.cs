using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class PGRequest
    {
        public string MERCHANTCODE { get; set; }
        public string PRN { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string AMOUNT { get; set; }
        public string SUCCESSURL { get; set; }
        public string FAILUREURL { get; set; }
        public string USERNAME { get; set; }
        public string USERMOBILE { get; set; }
        public string USEREMAIL { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string SERVICEID { get; set; }
        public string OFFICECODE { get; set; }
        public string REVENUEHEAD { get; set; }
        public string COMMTYPE { get; set; }
        public string CHECKSUM { get; set; }
    }
    public class PGResponse
    {
        public string FilePath { get; set; }
        public string MERCHANTCODE { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string PRN { get; set; }
        public string AMOUNT { get; set; }
        public string PAIDAMOUNT { get; set; }
        public string SERVICEID { get; set; }
        public string TRANSACTIONID { get; set; }
        public string RECEIPTNO { get; set; }
        public string EMITRATIMESTAMP{get;set;}
        public string STATUS { get; set; }
        public string PAYMENTMODE { get; set; }
        public string PAYMENTMODEBID { get; set; }
        public string PAYMENTMODETIMESTAMP { get; set; }
        public string RESPONSECODE {get;set;}
        public string RESPONSEMESSAGE {get;set;}
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string CHECKSUM { get; set; }

    }

    public class PGBackToBackRequest // for Refund Request
    {
        public string MERCHANTCODE { get; set; }
        public string REQUESTID { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string SERVICEID { get; set; }
        public string SUBSERVICEID { get; set; }
        public string REVENUEHEAD { get; set; }
        public string CONSUMERKEY { get; set; }
        public string CONSUMERNAME { get; set; }
        public string COMMTYPE { get; set; }
        public string SSOID { get; set; }
        public string OFFICECODE { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }
        public string PAYMODE { get; set; }
        public string BANKREFNUMBER { get; set; }
        
    }
    public class PGBackToBackResponse // for Refund Request
    {
        public string  REQUESTID { get; set; }
        public string  TRANSACTIONSTATUSCODE { get; set; }
        public string  RECEIPTNO { get; set; }
        public string  TRANSACTIONID { get; set; }
        public string  TRANSAMT { get; set; }
        public string  REMAININGWALLET { get; set; }
        public string  EMITRATIMESTAMP { get; set; }
        public string  TRANSACTIONSTATUS { get; set; }
        public string  MSG { get; set; }
        public string  CHECKSUM { get; set; }
    }
    public class PGBackToBackCheckSum
    {
        public string SSOID { get; set; }
        public string REQUESTID { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string SSOTOKEN { get; set; }
    }
}