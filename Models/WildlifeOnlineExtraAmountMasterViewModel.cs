using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class WildlifeOnlineExtraAmountMasterViewModel
    {
        public int PlaceId { get; set; }
        public string HeadName { get; set; }
        public decimal ExtraAmount { get; set; }
        public int IsUploaderOnandOff { get; set; }
        public int MaxSeatShow { get; set; }
        public bool OpenForDepartmentUser { get; set; }
        public int IsQuotaSeatsOnandOff { get; set; }
        public int KioskSeatOpenCitizenBookingTime { get; set; }
        public string CalculationSeatWiseOrVehicleWise { get; set; }
    }
}