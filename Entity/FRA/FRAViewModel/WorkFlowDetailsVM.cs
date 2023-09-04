using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class WorkFlowDetailsVM
    {
        public long WorkFlowDetailsID { get; set; }
        public Nullable<long> ClaimRequestDetailsID { get; set; }
        public string ApproverComment { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> ReferBackToDesignation { get; set; }
        public Nullable<int> ApproverDesignationID { get; set; }
        public string ReferBackToDesignationName { get; set; }
        public string ApproverDesignationName { get; set; }
        public string ApproverAction { get; set; }
        public DateTime? AddedOn { get; set; }
    }
}