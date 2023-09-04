using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class ApproverRemarksVM
    {
        public long ClaimRequestDetailsID { get; set; }
        [Required]
        [Display(Name ="Action")]
        public Int32? StatusID { get; set; }
        [Required]
        [Display(Name = "Approver Comment")]
        public string ApproverComment { get; set; }
        public string ApproverComment1 { get; set; }
        public string CaseNumber { get; set; } 
    }

    public class ApproverRemarksMultipleVM
    {
        public string ClaimRequestDetailsID { get; set; }
        [Required]
        public string ApproverComment { get; set; }
        public string StatusID { get; set; }
    }

    public class ApproverRemarksCommonVM
    {
        public string ParentID { get; set; }
        [Required]
        public string ApproverComment { get; set; }
        [Required]
        [Display(Name = "Action")]
        public string StatusID { get; set; }
    }
}