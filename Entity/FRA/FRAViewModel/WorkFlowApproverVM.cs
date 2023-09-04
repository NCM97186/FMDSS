using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class WorkFlowApproverVM
    { 
        public ClaimRequestDetailsVM CitizenClaimRequestDetails { get; set; }
        public ApproverRemarksVM ApproverRemarksDetails { get; set; }
        public List<ClaimRequestDetailsVM> ClaimRequestDetailsListForApproval { get; set; }
        public List<WorkFlowDetailsVM> WorkFlowDetailsList { get; set; }
        public ClaimRequestVM ClaimRequest { get; set; }
    }

    public class WorkFlowApproverMultipleVM
    {
        public List<ClaimRequestDetailsVM> ClaimRequestDetails { get; set; }
        public ApproverRemarksMultipleVM ApproverRemarksDetails { get; set; } 
    }
}