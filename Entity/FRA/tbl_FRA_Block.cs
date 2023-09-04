using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FMDSS.Entity
{
    public class tbl_FRA_Block
    {
        [Key]
        public long BlockID { get; set; }
        public string BlockCode { get; set; }
        public string BlockName { get; set; }
        public Nullable<long> DistrictID { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
