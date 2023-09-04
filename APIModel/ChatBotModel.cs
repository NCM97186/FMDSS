using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.APIModel
{
    public class ChatBotModel
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Place { get; set; }
        public string SHIFT_TYPE { get; set; }
        public string BOOKING_TYPE { get; set; }
        public string PLACE_NAME { get; set; }
        public string SHIFTName { get; set; }
        public string DATETYPE { get; set; }
        public string zone { get; set; }
        public string RequestID { get; set; }
        public string DIST_CODE { get; set; }
        public string NURSERY_CODE { get; set; }
        public string NURSERY_Id { get; set; }

        public string divname { get; set; }

        public string MobileNo { get; set; }
        public string SiteName { get; set; }

        


        public string ReportType { get; set; }
        public string Modules { get; set; }
        public string SSOID { get; set; }

        public string TRNS_Status { get; set; }

        public string Noc_Status { get; set; }

        public string IP_ADDRESS { get; set; }

        public string ModeOfBooking { get; set; }

        //Added by Nitin 08/11/2017
        public List<MISTicketTransactionDetails> MISTicketTransactionDetails { get; set; }
        public string DownloadExcel { get; set; }
        public string TimeDifference { get; set; }
        //Added by Nitin 08/11/2017

        public string TicketId { get; set; }
        public string RemarkStatus { get; set; }
        public string Remark { get; set; }

    }
    public class MISTicketTransactionDetails
    {

        public string Heads { get; set; }
        public string Fees { get; set; }


        public Int64 Index { get; set; }

        public Int64 TicketID { get; set; }
        public string BookingID { get; set; }
        // public string DateOfBooking { get; set; }
        // public string DateOfVisit { get; set; }
        public string Ssoid { get; set; }
        public string Mobile { get; set; }
        public string EmailId { get; set; }
        public string PlaceName { get; set; }
        // public string ZoneName { get; set; }
        public string ShiftTime { get; set; }
        public string NoOfForeignerMembers { get; set; }
        public string NoOfIndianMembers { get; set; }

        // public string TotalMembers { get; set; }

        public string ActualTicketDifference { get; set; }
        public string TotalNoOfCamera { get; set; }
        public string VehicleName { get; set; }

        // public decimal  AmountTobePaid { get; set; }
        public string TransactionStatus { get; set; }
        public string EmitraTransactionID { get; set; }

        public decimal RefundAmount { get; set; }

        public string ResendStatus { get; set; }
        public string DateofCancelation { get; set; }
        public string Manual_Status { get; set; }
        public string Manual_Remarks { get; set; }


        // Head Wise Deposit Details 
        public string HEADS { get; set; }
        public string TOTAL_MEMBER { get; set; }
        public string INCOME_FROM_TOURISM_IN_RTR { get; set; }
        public string INCOME_FROM_ECO_DEV_SURCHARGE_IN_RTR { get; set; }
        public string TRDF { get; set; }
        public string TOTAL { get; set; }

        // TABLE ListHeadWiseDepositDetail

        public string RequestID { get; set; }
        public string DateOfBooking { get; set; }
        public string DateOfVisit { get; set; }
        public string ZoneName { get; set; }
        public string ShiftType { get; set; }
        public string Name { get; set; }
        public string Indian { get; set; }
        public string NonIndian { get; set; }
        public string TotalMembers { get; set; }

        public string CameraForIndian { get; set; }
        public string CameraForNonIndian { get; set; }
        public string IncomeFromTourismIndianMemberEntryFee { get; set; }
        public string IncomeFromTourismNonIndianMemberEntryFee { get; set; }
        public string IncomeFromTourismGypsyEntryFee { get; set; }
        public string IncomeFromTourismCanterEntryFee { get; set; }
        public string IncomeFromTourismIndianCameraEntryFee { get; set; }
        public string IncomeFromTourismNonIndianCameraEntryFee { get; set; }
        public string TOTALIncomeFromTourism { get; set; }
        public string IncomeFromECODEVIndianMemberEntryFee { get; set; }
        public string IncomeFromECODEVNonIndianMemberEntryFee { get; set; }
        public string IncomeFromECODEVGypsyEntryFee { get; set; }
        public string IncomeFromECODEVCanterEntryFee { get; set; }
        public string IncomeFromECODEVIndianCameraEntryFee { get; set; }
        public string IncomeFromECODEVNonIndianCameraEntryFee { get; set; }
        public string TOTALIncomeFromECODEV { get; set; }
        public string FoundationIndianMemberEntryFee { get; set; }
        public string FoundationNonIndianMemberEntryFee { get; set; }

        public string FoundationForVehicleEntryFee { get; set; }
        public string FoundationForGuidFee { get; set; }

        public string TOTALFoundation { get; set; }

        // added by arvind k sharma


        public string VehicleRentFees { get; set; }
        public string VehicleRentFeesGSTPercentage { get; set; }
        public string VehicleRentFeesGSTAmount { get; set; }
        public string TOTALVehicleRentFees { get; set; }


        public string GuideFees { get; set; }
        public string GuideFeesGSTPercentage { get; set; }
        public string GuideFeesGSTAmount { get; set; }
        public string TOTALGuideFees { get; set; }


        // added by arvind k sharma

        public decimal AmountTobePaid { get; set; }
        public string TotalFeeHeadwise { get; set; }

        public string EmitraCharges { get; set; }
        public string TaxOnEmitraCharges { get; set; }
        public string EMitraTotalCharges { get; set; }
        public string AmountWithServiceCharges { get; set; }
        public string TotalPayment { get; set; }

        public string AMOUNT_STATUS { get; set; }
        public string AMOUNT_DIFFERENCE { get; set; }
        public string ModeOfBooking { get; set; }



    }
}