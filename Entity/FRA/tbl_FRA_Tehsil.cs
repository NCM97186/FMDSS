using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class tbl_FRA_Tehsil
    {
        [Key]
        public long TehsilID { get; set; } 
        public string TehsilCode { get; set; }
        public string TehsilName { get; set; }
        public Nullable<long> DistrictID { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}