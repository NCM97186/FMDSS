using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMDSS.Entity
{
    public class NP_HeadWiseItemFee
    {
        [Key]
        public int Id { get; set; }
        public long PlaceId { get; set; }
        public int HeadId { get; set; }
        public Nullable<int> VisitorTypeId { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public int ItemId { get; set; }
        public byte ItemParent { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public bool GSTApplicable { get; set; }
        public System.DateTime AddDate { get; set; }
        public long UserId { get; set; }

        [ForeignKey("ItemId")]
        public virtual NP_FeesItemMaster NP_FeesItemMaster { get; set; }
        [ForeignKey("HeadId")]
        public virtual NP_HeadMaster NP_HeadMaster { get; set; }
        public virtual tbl_mst_Places tbl_mst_Places { get; set; }
        [ForeignKey("VehicleId")]
        public virtual NP_VehicleMaster NP_VehicleMaster { get; set; }
        [ForeignKey("VisitorTypeId")]
        public virtual NP_VisitorTypeMaster NP_VisitorTypeMaster { get; set; }
    }
}