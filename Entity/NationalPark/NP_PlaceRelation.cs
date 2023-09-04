using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class NP_PlaceRelation
    {
        [Key]
        public int Id { get; set; }
        public long PlaceId { get; set; }
        public Nullable<long> ZoneId { get; set; }      
        public Nullable<int> ShiftId { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddDate { get; set; }
        public long UserId { get; set; }

        public virtual tbl_mst_Places tbl_mst_Places { get; set; }
        [ForeignKey("VehicleId")]
        public virtual NP_VehicleMaster NP_VehicleMaster { get; set; }
        public virtual tbl_mst_Zone tbl_mst_Zone { get; set; }
        [ForeignKey("ShiftId")]
        public virtual NP_ShiftMaster NP_ShiftMaster { get; set; }
    }
}