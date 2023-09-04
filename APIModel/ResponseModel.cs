using FMDSS.Controllers;
using FMDSS.Models;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.CustomModels.Models
{
    public static class Response
    {
        public static string ErrorLog(string ErrorMessage, string Sqlrequest=null, string Sqlresponse = null, string Discorequest = null, string DiscoResponse = null, string methodName = null)
        {
            string guid = string.Empty;
            try
            {
                // guid = SqlFeatures.InsertOrderLogEntries(connectionString, ErrorMessage, OrderId, Sqlrequest, Sqlresponse, Discorequest, DiscoResponse, methodName, DateTime.Now.ToString());

            }
            catch
            {
                // We want to silently catch exceptions adding log entries.
            }
            return guid;
        }

        public static T ErrorLogs<T>(dynamic obj ,string errorMsg,string errorDetails)
        {
            obj.Status= ResponseStatus.Failed;
            obj.Message = errorMsg;
            obj.ErrorDescription = errorDetails;
            return obj;
        }

    }

    public enum ResponseStatus
    {
        Success = 1,
        Info = 2,
        Warning = 3,
        Failed = 0
    }

    

    



    public class ResponseBase
    {
        public ResponseBase()
        {
            Status = ResponseStatus.Success;
            Message = ResponseStatus.Success.ToString();
            ErrorDescription = string.Empty;
        }

        public int CountOfVehicleType { get; set; }

        [Required]
        public ResponseStatus Status { get; set; } 
        [Required]
        public string ErrorDescription { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string ErrorTransactionId { get; set; }

        public void SetStatus(ResponseBase data)
        {
            if (data != null)
            {
                Status = data.Status;
                Message = data.Message;
                ErrorDescription = data.ErrorDescription;
                ErrorTransactionId = data.ErrorTransactionId;
            }
        }
        public  List<Weboption> listItems { get; set; }


    }

    public class Weboption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
    }


    public class DataSetResponse : ResponseBase
    {
        public DataSet Data { get; set; }       
    }
    public class DataTableResponse : ResponseBase
    {
        public DataTableResponse()
        {
            Data = new DataTable();
            Data1 = new DataTable();
            Data2 = new DataTable();
            data3 = new DataTable();
        }
        public DataTable Data { get; set; }
        public DataTable Data1 { get; set; }
        public DataTable Data2 { get; set; }
        public DataTable data3 { get; set; }
        
    }
    public class StringResponse : ResponseBase
    {
        public string Data { get; set; }
    }
    public class BooleanResponse : ResponseBase
    {
        public bool Data { get; set; }
    }
    public class IntegerResponse : ResponseBase
    {
        public int? Data { get; set; }
    }
    public class ObjectResponse : ResponseBase
    {
        public object Data { get; set; }
    }

    public class DynamicResponse : ResponseBase
    {
        public DynamicResponse()
        {
            Data = null;
        }
        public object Data { get; set; }
    }


    public class DropDownResponse:ResponseBase
    {
        public DropDownResponse()
        {
            Data = new List<DropDownValue>();
        }
        public List<DropDownValue> Data { get; set; }
    }

    public class DropDownValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }


    public class SelectedListItemResponce
    {
        public SelectedListItemResponce()
        {
            Data = new List<SelectedListItem>();
        }
        public List<SelectedListItem> Data { get; set; }
    }

    public class BudgetAllocationPerposalResponse : ResponseBase
    {
        public BudgetAllocationPerposalResponse()
        {
            Data = new List<ViewBudgetAllocationPerposalModel>();
        }
        public List<ViewBudgetAllocationPerposalModel> Data { get; set; }
    }

    public class ViewBudgetAllocationPerposalViewResponse : ResponseBase
    {
        public ViewBudgetAllocationPerposalViewResponse()
        {
            Model = new ViewBudgetAllocationPerposalModel();
            List = new List<ViewBudgetAllocationPerposalModel>();
        }
        public ViewBudgetAllocationPerposalModel Model { get; set; }
        public List<ViewBudgetAllocationPerposalModel> List { get; set; }
    }

    public class BudgetMastersModel
    {
        public long SchemeID { get;set;}
        public long BudgetHeadID { get; set; }
        public long SubBudgetHeadID { get; set; }
        public long ActivityID { get; set; }
        public long SubActivityID { get; set; }
    }
    public class BudgetPreSurveyResponse : ResponseBase
    {
        public BudgetPreSurveyResponse()
        {
            Data = new List<ViewBudgetPreSurveyModel>();
            Models = new ViewBudgetPreSurveyModel();
        }
        public List<ViewBudgetPreSurveyModel> Data { get; set; }
        public ViewBudgetPreSurveyModel Models { get; set; }

    }


    public class OnlineBookingZoneWiseModel
    {
        public string PlaceName { get; set; }
        public string ZoneName { get; set; }
        public string VehicleName { get; set; }
        public string GuideName { get; set; }
        public string VehicleNumber { get; set; }
        public string ShiftTime { get; set; }
    }

    public class OnlineBookingZoneWiseModelResponse : ResponseBase
    {
        public OnlineBookingZoneWiseModelResponse()
        {
            Data = new List<OnlineBookingZoneWiseModel>();
        }
        public List<OnlineBookingZoneWiseModel> Data { get; set; }

    }

    #region for ZooBooking Web API For Emitra Plus 31-12-2018
    public class MemberVehicallistModel
    {
        public List<GetMemberFeelstModel> GetMemberFeelstModel { get; set; }
        public List<GetVehicleFeelstModel> GetVehicleFeelstModel { get; set; }
        public List<ShiftModel> GetShiftModel { get; set; }

    }
    public class ENCData
    {
        public string REQUESTID { get; set; }
        public string TRANSACTIONSTATUSCODE { get; set; }
        public string RECEIPTNO { get; set; }
        public string TRANSACTIONID { get; set; }

        public string TRANSAMT { get; set; }

        public string EMITRATIMESTAMP { get; set; }
        public string TRANSACTIONSTATUS { get; set; }
        public string MSG { get; set; }
        public string CHECKSUM { get; set; }


    }
    public class FinalSubmitModel
    {
        public List<GetMemberFeelstModel> lstMember { get; set; }
        public List<GetVehicleFeelstModel> lstVehicle { get; set; }
        public string BookingType { get; set; }
        public string RequestId { get; set; }
        public string InstituteOrganisationName { get; set; }
        public string AddressOfInstitOrgan { get; set; }
        public string PhoneNoOfInstitOrgan { get; set; }
        public string NameOfHead { get; set; }
        public string PhoneOfHead { get; set; }
        public string DocumentForTour { get; set; }
        public string DocumentForTourImageName { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public string UploadId { get; set; }
        public string UploadIdImageName { get; set; }
        public string PlaceOfVisit { get; set; }
        public string DateOfVisit { get; set; }
        public string txtInput { get; set; }
        // public string DocumentForTour { get; set; }
        public bool AdultIndianMember { get; set; }
        public bool AdultNonIndianMember { get; set; }
        public bool Student { get; set; }
        public bool ChildBelowAgeFive { get; set; }

        public string IPAddress { get; set; }
        public string IPAddressAndDeviceKey { get; set; }
        public string KioskUserId { get; set; }

        public bool PrivateVehicle { get; set; }
        public string VSLNo { get; set; }
        public string TypeOfVehicle { get; set; }
        public string FeePerVehicle { get; set; }
        public string NoOfVehicle { get; set; }
        public string TotalVehicleFee { get; set; }

        public string NoOfBus { get; set; }
        public string NoOfJeepCarMotorMiniBus { get; set; }
        public string NoOfTwoWheeler { get; set; }
        public string NoOfAutoRikshaw { get; set; }

        public string TotalFeesOfBus { get; set; }
        public string TotalFeesOfJeepCarMotorMiniBus { get; set; }
        public string TotalFeesOfTwoWheeler { get; set; }
        public string TotalFeesOfAutoRikshaw { get; set; }

        public string MemberEntryFees { get; set; }
        public string CameraFees { get; set; }
        public string VehicleFees { get; set; }
        public string OnlineBookingCharges { get; set; }
        public string TotalPayableCharges { get; set; }
        public string ShiftTypeID { get; set; }
        public string ShiftTypeName { get; set; }
        public string SsoId { get; set; }
    }
    public class GetMemberFeelstModel
    {
        public string FeeChargedOn { get; set; }
        public string TypeOfMember { get; set; }

        public decimal HeadAmount { get; set; }
        public int NoOfMember { get; set; }
        public decimal StillCameraAmount { get; set; }
        public int NoOfStillCamera { get; set; }
        public decimal VideoCameraAmount { get; set; }
        public int NoOfVideoCamera { get; set; }

        public decimal TotalFeesOfMember { get; set; }

        public bool PrivateVehicle { get; set; }
    }
    public class GetVehicleFeelstModel
    {
        public string FeeChargedOn { get; set; }
        public decimal HeadAmount { get; set; }
        public int NoOfVehicle { get; set; }
        public decimal TotalVehicleFee { get; set; }
    }
    public class ZooBookingModel
    {
        public Int64 PlaceID { get; set; }
        public string PlaceName { get; set; }
    }

    public class ShiftModel
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class AvaliableTicketModel
    {
        public Int32 AvaliableTicket { get; set; }
    }
    public class ZooBookingModelResponse : ResponseBase
    {
        public ZooBookingModelResponse()
        {
            Data = new List<ZooBookingModel>();
        }
        public List<ZooBookingModel> Data { get; set; }

    }
    public class AfterSubmitModel
    {
        public string RequestID { get; set; }
        public Int64 MemberFee { get; set; }
        public Int64 CameraFee { get; set; }
        public Int64 VehicleFee { get; set; }
        public decimal EmitraCharges { get; set; }
        public decimal TotalFinalAmount { get; set; }
        public string Status { get; set; }
    }
    public class ShiftModelResponse : ResponseBase
    {
        public ShiftModelResponse()
        {
            Data = new List<ShiftModel>();
        }
        public List<ShiftModel> Data { get; set; }

    }
    public class GetZooTicketResponceSubmitModel
    {
        public string TRANSACTION_STATUS { get; set; }
        public string REQUEST_ID { get; set; }
        public string EMITRA_TRANSACTION_ID { get; set; }
        public string TRANSACTION_TIME { get; set; }
        public string TRANSACTION_AMOUNT { get; set; }
        public string EMITRA_AMOUNT { get; set; }
        public string USER_NAME { get; set; }
        public string TRANSACTION_BANK_DETAILS { get; set; }
        public string ZooTicketHTML { get; set; }
        public string HeadeText { get; set; }
        public string FooterText { get; set; }
    }
    public class AvaliableTicketModelResponse : ResponseBase
    {
        public AvaliableTicketModelResponse()
        {
            Data = new List<AvaliableTicketModel>();
        }
        public List<AvaliableTicketModel> Data { get; set; }

    }

    public class MemberVehicleDetailsResponce : ResponseBase
    {
        public MemberVehicleDetailsResponce()
        {
            Data = new MemberVehicallistModel();
        }
        public MemberVehicallistModel Data { get; set; }
    }
    public class FinalSubmitResponceAfterSubmit : ResponseBase
    {
        public FinalSubmitResponceAfterSubmit()
        {
            Data = new List<AfterSubmitModel>();
        }
        public List<AfterSubmitModel> Data { get; set; }
    }
    public class UpdateZooTicketResponce : ResponseBase
    {
        public UpdateZooTicketResponce()
        {
            Data = new List<GetZooTicketResponceSubmitModel>();
        }
        public List<GetZooTicketResponceSubmitModel> Data { get; set; }

    }

    public class WaterSourceDetailResponse : ResponseBase
    {
        public WaterSourceDetailResponse()
        {
            Data = new List<WaterSourceDetail>();
        }
        public List<WaterSourceDetail> Data { get; set; }

    }
    #endregion
}
