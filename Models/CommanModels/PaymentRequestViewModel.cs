using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CommanModels
{
    public class PaymentRequestViewModel
    {
        public EmitraLinks Links { get; set; }
        public EmitraKioskRequest emitrarequest { get; set; }
    }
}