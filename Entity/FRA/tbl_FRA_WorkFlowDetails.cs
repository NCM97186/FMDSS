using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMDSS.Entity
{
    public class tbl_FRA_WorkFlowDetails: CommonEntity
    { 
        [Key]
        public long WorkFlowDetailsID { get; set; }
        public Nullable<long> ClaimRequestDetailsID { get; set; }
        public string ApproverComment { get; set; }
        public string ApproverComment1 { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> ReferBackToDesignation { get; set; }
        public Nullable<int> ApproverDesignationID { get; set; }
        [NotMapped]
        public string ReferBackToDesignationName { get; set; }
        [NotMapped]
        public string ApproverDesignationName { get; set; }
        [NotMapped]
        public string ApproverAction { get; set; } 
        public virtual List<tbl_FRA_ClaimRequestDocument> ClaimRequestDocument { get; set; }
    }
}
