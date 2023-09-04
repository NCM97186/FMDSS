using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMDSS.Entity
{
    public class tbl_FRA_BorderingVillageDetails
    {
        [Key]
        public long BorderingVillageDetailsID { get; set; }
        public Nullable<long> ClaimRequestDetailsID { get; set; }
        public string VillageCode { get; set; }
        [NotMapped]
        public string VillageName { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }

        [ForeignKey("VillageCode")]
        public virtual tbl_FRA_Village tbl_FRA_Village { get; set; }
    }
}
