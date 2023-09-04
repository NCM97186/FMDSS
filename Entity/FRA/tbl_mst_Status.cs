using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class tbl_mst_Status
    {
        //public tbl_mst_Status()
        //{
        //    this.tbl_FRA_ClaimRequestDetails = new List<tbl_FRA_ClaimRequestDetails>();
        //    this.tbl_FRA_WorkFlowDetails = new List<tbl_FRA_WorkFlowDetails>();
        //}

        [Key]
        public string StatusID { get; set; }
        public string StatusDesc { get; set; }
    
        //public virtual ICollection<tbl_FRA_ClaimRequestDetails> tbl_FRA_ClaimRequestDetails { get; set; }
        //public virtual ICollection<tbl_FRA_WorkFlowDetails> tbl_FRA_WorkFlowDetails { get; set; }
    }
}
