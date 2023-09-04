using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class tbl_FRA_DesignationPermission
    {
        public tbl_FRA_DesignationPermission()
        {
            this.tbl_FRA_WorkFlowRule = new List<tbl_FRA_WorkFlowRule>();
        }
        [Key]
        public long DesignationPermissionID { get; set; }
        public Nullable<int> DesignationID { get; set; }
        public string PermissionIDs { get; set; }
        public string ModuleName { get; set; }
        public Nullable<bool> ActiveStatus { get; set; } 
        public virtual List<tbl_FRA_WorkFlowRule> tbl_FRA_WorkFlowRule { get; set; }
    }
}
