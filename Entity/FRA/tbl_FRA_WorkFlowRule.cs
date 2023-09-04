using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMDSS.Entity
{
    public class tbl_FRA_WorkFlowRule
    {
        [Key]
        public long WorkFlowRuleID { get; set; }
        public Nullable<int> DesignationID { get; set; }
        public Nullable<int> ClaimTypeID { get; set; }
        public Nullable<int> ApproverLevel { get; set; }
        public Nullable<long> DesignationPermissionID { get; set; }
        public Nullable<bool> IsLastApprover { get; set; }
        public Nullable<long> AddedBy { get; set; }
        public Nullable<System.DateTime> AddedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
        [NotMapped]
        public string DesignationName { get; set; }
    }
}
