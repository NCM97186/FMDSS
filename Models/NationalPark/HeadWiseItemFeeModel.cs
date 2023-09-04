using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.NationalPark
{
    public class NPPlace
    {
        public Int64 PlaceId { get; set; }
        public string PlaceName { get; set; }
    }
    public class HeadWiseItemFeeModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage="Select Place")]
        public long PlaceId { get; set; }        
        [Required(ErrorMessage = "Select Head")]
        public int HeadId { get; set; }        
        public Nullable<int> VisitorTypeId { get; set; }
        public Nullable<int> VehicleId { get; set; }
        [Required(ErrorMessage = "Select Item")]
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Select Item")]
        public ItemParent ItemParent { get; set; }

        public decimal Amount { get; set; }

        public string HeadName { get; set; }
        public string VisitorType { get; set; }
        public string VehicleName { get; set; }
        public string ItemName { get; set; }
        public string ItemParentName { get; set; }        
        public bool IsActive { get; set; }
        public bool GSTApplicable { get; set; }
    }
}