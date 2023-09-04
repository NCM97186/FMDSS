using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.CustomModels.Models
{
    public class RequestModel
    {
    }

   
    public class RequestBaseModel
    {
        public int Status { get; set; } 
        public string CreatedDate { get; set; } //= DateTime.Now.ToString("dd/MM/yyyy");
    }

    public class ViewBudgetAllocationPerposalViewModel
    {
        public ViewBudgetAllocationPerposalViewModel()
        {
            Model = new ViewBudgetAllocationPerposalModel();
            List = new List<ViewBudgetAllocationPerposalModel>();
        }
      public ViewBudgetAllocationPerposalModel Model { get; set; }
      public List<ViewBudgetAllocationPerposalModel> List { get; set; }
    }

    public class OnlineBookingZoneWiseRequestModel
    {
        public string PlaceID { get; set; }
        public string DateOfArrival { get; set; }
        public string ShiftId { get; set; }
        public long UserID  { get; set; }
    }

    public class OnlineBookingZoneWiseDetailRequestModel
    {
        public OnlineBookingZoneWiseDetailRequestModel()
        {
            Model = new OnlineBookingZoneWiseRequestModel() { PlaceID = "2", DateOfArrival = DateTime.Now.ToString("dd/MM/yyyy"), ShiftId = "ALL", UserID = 90185 };
        }
        public OnlineBookingZoneWiseRequestModel Model { get; set; }
    }

    #region for ZooBooking WEB API for Emitra Plus 31-12-2018
    public class UpdateZooTicketRequest
    {
        public UpdateZooTicketRequest()
        {
            Model = new UpdateZooTicketModel();
        }
        public UpdateZooTicketModel Model { get; set; }
    }
    public class UpdateZooTicketRequest_EmitraPlus
    {
        public string MERCHANTCODEs { get; set; }
        public string STATUSs { get; set; }
        public string EmitraCharge { get; set; }
        public string PaymentMode { get; set; }
        public ENCDATAs ENCDATAs { get; set; }
        public string TotalPrice { get; set; }
        public string Ssoid { get; set; }
    }
    public class UpdateZooTicketModel
    {
        public string MERCHANTCODEs { get; set; }
        public string PRNs { get; set; }
        public string STATUSs { get; set; }
        public string EmitraCharge { get; set; }
        public string ENCDATAs { get; set; }
        public string RequestId { get; set; }
        public string TotalPrice { get; set; }
        public string Ssoid { get; set; }
        public string PaymentMode { get; set; }

    }
    public class ENCDATAs
    {
        public string REQUESTID { get; set; }
        public string TRANSACTIONSTATUSCODE { get; set; }
        public string RECEIPTNO { get; set; }
        public string TRANSACTIONID { get; set; }
        public string TRANSAMT { get; set; }
        public string REMAININGWALLET { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string TRANSACTIONSTATUS { get; set; }
        public string MSG { get; set; }
        public string CHECKSUM { get; set; }
    }
    #endregion
    public class BookOnTicketRequestModel
    {
        #region global variable

        public Int64 PlaceId;
        public Int64 ZoneId { get; set; }
        public string DurationFrom { get; set; }
        public string DurationTo { get; set; }
        public string isSafari { get; set; }
        public string isAccomo { get; set; }
        public string isMorning { get; set; }
        public string isEvening { get; set; }
        public string isFullDay { get; set; }

        public string PlaceName { get; set; }
        public string isHalfDay { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string ShiftTime { get; set; }
        public Int32 vehicleID { get; set; }
        public decimal VehicleFees_TigerProject { get; set; }
        public decimal VehicleFees_Surcharge { get; set; }
        public decimal VehicleFees_Total { get; set; }
        public decimal MemberFees_TigerProject { get; set; }
        public decimal MemberFees_Surcharge { get; set; }
        public decimal TRDF { get; set; }
        public decimal CameraFees_TigerProject { get; set; }
        public decimal CameraFees_Surcharge { get; set; }
        public decimal TotalPerMemberFees { get; set; }
        public decimal TotalPerMemberCameraFees { get; set; }

        // added by arvind sharma 25/07/2017
        // begin
        public decimal BoardingVehicleFee { get; set; }
        public decimal BoardingVehicleFeeGSTPercentage { get; set; }
        public decimal BoardingVehicleFeeGSTAmount { get; set; }

        public decimal BoardingGuideFee { get; set; }
        public decimal BoardingGuideFeeGSTPercentage { get; set; }
        public decimal BoardingGuideFeeGSTAmount { get; set; }

        public decimal TotalBoardingFee { get; set; }
        public string GSTMessage { get; set; }
        public decimal Vehicle_TRDF { get; set; }
        public decimal GuidFee_TRDF { get; set; }
        public long UserID { get; set; }
        // end
        // added by arvind sharma 25/07/2017


        public Int64 AccomoID;
        public decimal RoomCharge;

        public int totalRoom;

        public decimal RoomAvailability;
        public string KioskUserId { get; set; }
        public Int64 EnteredBy { get; set; }
        public string RequestId { get; set; }
        public int TotalMember { get; set; }
        public int TotalRoom { get; set; }
        public string IPAddress { get; set; }
        public string EmitraTransactionId { get; set; }
        public int Trn_Status_Code { get; set; }
        public Int64 TicketID { get; set; }
        public decimal TotalAmount { get; set; }
        public string DateOfArrival { get; set; }
        public string SsoToken { get; set; }
        public int CancleTicketStatus { get; set; }
        public string CancleTicketStatusName { get; set; }

        public decimal RefundAmount { get; set; }
        public int CancelStatus { get; set; }

        #endregion

        #region MemberInformation
        public long MemberTableID { get; set; }
        public string MemberSLNo { get; set; }
        public string MemberName { get; set; }
        public string MemberType { get; set; }
        public string MemberGender { get; set; }
        public string MemberIdType { get; set; }
        public string MemberIdNo { get; set; }
        public string MemberTotalCamera { get; set; }
        public string MemberNationality { get; set; }

        public string CitizenRemarksVal { get; set; }
        public string TotalMemberFees { get; set; }
        public string TotalCameraFees { get; set; }
        public string TotalSafariFees { get; set; }
        public string VehicleRent { get; set; }
        public string GSTonVehicleRent { get; set; }
        public string GuideFee { get; set; }
        public string GSTGuideFee { get; set; }
        public string TicketAmount { get; set; }

        public string EmitraCharges { get; set; }
        public string GrandTotal { get; set; }
        public string SSOID { get; set; }

        // bank details for refund
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountType { get; set; }
        public string AccountHolderName { get; set; }
        public bool ConfirmRefundByCitizen { get; set; }

        // bank details for refund
        #endregion
    }
}
