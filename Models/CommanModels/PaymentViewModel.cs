using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CommanModels
{
    public class PaymentViewModel
    {
        public string ActionCode { get; set; }
        public int userid { get; set; }
        public string emitraserviceid { get; set; }
        public string EmitraHeadCode { get; set; }
        public string requestid { get; set; }
        public string parentid { get; set; }
        public string officecode { get; set; }
        public decimal PayAmt { get; set; }
    }
}