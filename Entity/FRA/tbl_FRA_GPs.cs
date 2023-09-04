using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class tbl_FRA_GPs
    {
        [Key]
        public long GPID { get; set; }
        public string GPCode { get; set; }
        public string GPName { get; set; }
        public Nullable<long> BlockID { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
