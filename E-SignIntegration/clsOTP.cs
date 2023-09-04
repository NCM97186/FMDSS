using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.E_SignIntegration
{
    public class clsOTP
    {
        public string aadharid { get; set; }
        public string departmentname { get; set; }
    }

    public class OtpResponce
    {
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
    }


    public class clsOTPbyEmitra
    {
        public string aadharid { get; set; }
    }
}