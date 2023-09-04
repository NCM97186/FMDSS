using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.NPVM
{
    public class OBookingReportVM
    {
        public int DateType { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        [Display(Name = "Place")]
        public int PlaceId { get; set; }
        public int ZoneId { get; set; }
        public int ShiftId { get; set; }
        public int VehicleId { get; set; }
        public string VisitingDate { get; set; }
        public byte BookingType { get; set; }
        public Int16 BookingStatus { get; set; }
        public Int32? VisitorType { get; set; }
    }

    public class OBookingDetails
    {
        public Int64 TicketId { get; set; }
        public string RequestId { get; set; }
        public string BookingType { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime VisitDate { get; set; }
        public int TotalMember { get; set; }
        public decimal TotalAmountBePay { get; set; }
        public decimal EmitraAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public string EmitraTransactionId { get; set; }
        public string PlaceName { get; set; }
        public string GuideName { get; set; }
        public string VehicleNumber { get; set; }
        public bool BoardingPassStatus { get; set; }
        public string Shift { get; set; }
        public string SSOID { get; set; }
        public BookingStatusForReport BookingStatus { get; set; }
    }

    public class OBookingMemberDetails
    {
        public Int64 TicketId { get; set; }
        public string MemberName { get; set; }
        public string RequestId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime VisitDate { get; set; }
        public int TotalMember { get; set; }
        public decimal TotalAmountBePay { get; set; }
        public decimal EmitraAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public string EmitraTransactionId { get; set; }
        public string VisitorType { get; set; }
        public string PlaceName { get; set; }
        public string GuideName { get; set; }
        public string VehicleNumber { get; set; }
        public bool BoardingPassStatus { get; set; }
        public BookingStatusForReport BookingStatus { get; set; }
    }

    public class OBookingSummaryDetails
    {
        public Int32 StudentVisitors { get; set; }
        public Int32 IndianVisitors { get; set; }
        public Int32 NonIndianVisitors { get; set; }
        public Decimal StudentVisitorsHeadAmt { get; set; }
        public Decimal IndianVisitorsHeadAmt { get; set; }
        public Decimal NonIndianVisitorsHeadAmt { get; set; }
        public Decimal EmitraCharges { get; set; }
        public Decimal HeadAmount { get; set; }
        public string HeadName { get; set; }
        public Decimal GrandTotal { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}