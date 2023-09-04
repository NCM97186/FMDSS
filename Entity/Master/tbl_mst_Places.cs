using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class tbl_mst_Places
    {
        [Key]
        public long PlaceID { get; set; }
        public string DIST_CODE { get; set; }
        public string PlaceName { get; set; }
        public string Category { get; set; }
        public string MorningShiftFrom { get; set; }
        public string MorningShiftTo { get; set; }
        public string EveningShiftFrom { get; set; }
        public string EveningShiftTo { get; set; }
        public string FullDayShift { get; set; }
        public Nullable<int> TicketAllocatedPerShift { get; set; }
        public int TotalTicketAllocatedPerShift { get; set; }
        public string IsAccommodation { get; set; }
        public string IsSafari { get; set; }
        public Nullable<int> Isactive { get; set; }
        public Nullable<System.DateTime> EnteredOn { get; set; }
        public Nullable<long> EnteredBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<bool> IsZone { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Boarding_Point { get; set; }
        public bool IsOnlineBooking { get; set; }
        public bool IsCamping { get; set; }
        public bool IsResearch { get; set; }
        public int MaxBookingDuration { get; set; }
        public Nullable<bool> isMorning { get; set; }
        public Nullable<bool> isEvening { get; set; }
        public Nullable<bool> isFullDay { get; set; }
        public Nullable<bool> isDptKiosk { get; set; }
        public Nullable<bool> isCitizen { get; set; }
        public decimal Tax { get; set; }
        public decimal EmitraCharges { get; set; }
        public string Code { get; set; }
        public bool isZooAvailable { get; set; }
        public bool isFilmShootong { get; set; }
        public bool isZooMemberDetailRequired { get; set; }
        public short ZooOnlinePrintID { get; set; }
        public short ZooDeptKioskPrintID { get; set; }
        public short ZooEmitraKioskPrintID { get; set; }
        public bool IsZooBoardingPassRequired { get; set; }
        public string GuideName { get; set; }
        public string VehicleNumber { get; set; }
        public long OnlineEmiraPaymentServiceID { get; set; }
    }
}