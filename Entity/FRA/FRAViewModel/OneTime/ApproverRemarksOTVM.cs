using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Entity.FRAViewModel
{ 
    public class ApproverRemarksOTVM
    {
        public long ClaimRequestDetailsID { get; set; }
        [Required]
        [Display(Name = "Action")]
        public Int32? StatusID { get; set; }
        [Required]
        [Display(Name = "Approver Comment")]
        public string ApproverComment { get; set; }
        public string ApproverComment1 { get; set; }
        public string CaseNumber { get; set; }
        [NotMapped]
        [Required] 
        public string SSOID { get; set; }
        public long? AddedBy { get; set; }
        public Nullable<DateTime> AddedOn { get; set; }
        [NotMapped]
        public string EnteredOn { get; set; }
    }
}