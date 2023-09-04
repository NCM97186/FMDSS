using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class tbl_FRA_District
    {
        [Key]
        public long DistrictID { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public Nullable<bool> IsTSP { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
