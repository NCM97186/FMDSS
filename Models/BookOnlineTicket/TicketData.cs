using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.BookOnlineTicket
{
    public class TicketData
    {
        //public Int16 Id { get; set; }
        public TicketData()
        {
            VideoCameraAmount = StillCameraAmount = MemberFees = VechileFees = VechileFeesGST = VechileFeesGSTAmount = GuideFees = GuideFeesGST = GuideFeesGSTAmount = 0;
        }
        [Required(ErrorMessage = "Select Nationality")]
        public int VisitorTypeId { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Select Gender")]
        public short Gender { get; set; }

        [Required(ErrorMessage = "Select Id Type")]
        public short IDType { get; set; }

        [Required(ErrorMessage = "Enter Id")]
        public string IDNo { get; set; }
        public int NoOfStillCamera { get; set; }
        public int NofVideoCamera { get; set; }

        public double StillCameraAmount { get; set; }
        public double VideoCameraAmount { get; set; }
        public double MemberFees { get; set; }
        public double VechileFees { get; set; }
        public double VechileFeesGST { get; set; }
        public double VechileFeesGSTAmount { get; set; }
        public double GuideFees { get; set; }
        public double GuideFeesGST { get; set; }
        public double GuideFeesGSTAmount { get; set; }
    }

    public class Ticket
    {
        public Ticket()
        {
            ReserveStatus = 'R';
        }
        public int PlaceId { get; set; }
        public int ZoneId { get; set; }
        public int ShiftId { get; set; }
        public int VehicleId { get; set; }
        public string VisitingDate { get; set; }
        public int EnteredBy { get; set; }
        public byte BookingType { get; set; }
        public string IPAddress { get; set; }
        public Char ReserveStatus { get; set; }
    }

    public class TicketPayment
    {
        public long TicketId { get; set; }
        public string RequestId { get; set; }
        public DateTime VisitingDate { get; set; }
        public DateTime BookingDate { get; set; }
        public long EnteredBy { get; set; }
        public string Name { get; set; }
        public string PlaceName { get; set; }
        public string Vehicle { get; set; }
        public int TotalMember { get; set; }
        public decimal TotalMemberFees { get; set; }
        public decimal TotalCameraFees { get; set; }
        public decimal TotalVehicleFees { get; set; }
        public decimal TotalGuideFees { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalGSTAmount { get; set; }
        public decimal OdhiCharge { get; set; }
        public bool IsOdhiExist { get; set; }
        public decimal FacilityCharge { get; set; }
        public decimal MaintenanceCharge { get; set; }
        public decimal VehicleRent { get; set; }
        public decimal TotalAmountBePay { get; set; }       
        public TransactionStatus TransactionStatus { get; set; }
        public BookingStatus bookingStatus { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal EmitraAmount { get; set; }
        public string EmitraTransactionID { get; set; }
        public BookingType BookingType { get; set; }

        public PaymentMode PaymentMode { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeIssueDate { get; set; }
        public string IFSCCode { get; set; }


    }

    public class UpdateTicketBooking
    {
        public string RequestId { get; set; }
        public Int64 UserId { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal EmitraAmount { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string EmitraTransactionId { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string ChequeNo { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleName { get; set; }
        public string GuideName { get; set; }
        public DateTime? ChequeIssueDate { get; set; }
        public string EmitraResponse { get; set; }
    }

    public class DepartmentKioskPayment
    {
        public string RequestId { get; set; }
        [Required(ErrorMessage = "Select Payment Mode")]
        public PaymentMode PaymentMode { get; set; }
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Enter Bank Name")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Enter IFSC Code")]
        public string IFSCCode { get; set; }
        [Required(ErrorMessage = "Enter Cheque Number")]
        public string ChequeNo { get; set; }
        [Required(ErrorMessage = "Enter Vehicle Number")]
        public string VehicleNumber { get; set; }
        public string VehicleName { get; set; }
        [Required(ErrorMessage = "Enter Guide Name")]
        public string GuideName { get; set; }
        [Required(ErrorMessage = "Enter Cheque Issue Date")]
        public string ChequeIssueDate { get; set; }

    }


    public class UserBookingWildLifeExtra
    {
        public string CancleTicketStatus { get; set; }
        public string CancleTicketStatusName { get; set; }
        public string DateOfArrival { get; set; }
    }

    public class UserBooking: UserBookingWildLifeExtra
    {
        public Int64 TicketId { get; set; }
        public string RequestId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime VisitDate { get; set; }
        public int TotalMember { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public string PlaceName { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public int COVIDStatus { get; set; }
        public int RefundStatus { get; set; }
        public int TicketMemberBordingStatus { get; set; }
    }
    public class BoardingPassDetails
    {
        public Int64 TicketId { get; set; }
        public string RequestId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime VisitDate { get; set; }
        public int TotalMember { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public string PlaceName { get; set; }
        public string GuideName { get; set; }
        public string VehicleNumber { get; set; }
        public bool BoardingPassStatus { get; set; }
        public BookingStatus BookingStatus { get; set; }
    }
    public class DisplayColumn
    {
        public ItemParent ItemParent { get; set; }
        public int ItemId { get; set; }
        public int PlaceId { get; set; }
    }

    public class PlaceList
    {
        public Int64 PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string URL { get; set; }
    }
}