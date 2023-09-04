using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class WorkFlowVM
    {
        public Nullable<long> WorkFlowDetailsID { get; set; }
        public Nullable<long> ClaimRequestDetailsID { get; set; }
        public Nullable<bool> IsEmitraCheckRequired { get; set; }
        public string DisplayRequestID { get; set; }
        public bool IsError { get; set; }
        public string ReturnMsg { get; set; }
    }
}