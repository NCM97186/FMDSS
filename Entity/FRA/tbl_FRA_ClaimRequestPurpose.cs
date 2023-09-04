using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class tbl_FRA_ClaimRequestPurpose
    {
        [Key]
        public int ClaimRequestPurposeID { get; set; }
        public int ClaimTypeID { get; set; }
        public string Purpose { get; set; } 
        public Nullable<bool> ActiveStatus { get; set; }
    }
}