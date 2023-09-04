using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.E_SignIntegration
{
    public class clsVerifyOTP
    {
        public string otp { get; set; }
        public string transactionid { get; set; }
    }

    public class clsVerifyOTPResponce
    {
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
    }

}