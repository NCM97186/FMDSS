using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class tbl_FRA_Village
    {
        [Key]
        public string VillageCode { get; set; }
        public long VillageID { get; set; } 
        public string VillageName { get; set; }
        public Nullable<long> GPID { get; set; }
        public Nullable<long> PatwarID { get; set; }
        public Nullable<long> ACID { get; set; }
        public string PSID { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
