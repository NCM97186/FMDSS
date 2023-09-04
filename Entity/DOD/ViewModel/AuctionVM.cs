using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity.DOD.ViewModel
{
    public class AuctionVM
    {
        public long AuctionID { get; set; }
        public long WinnerAuctionID { get; set; }
        public string RequestedId { get; set; }
        public long NoticeID { get; set; }
        public string NoticeNumber { get; set; }
        public string Quantity { get; set; }
        public decimal ReservedPrice { get; set; }
        public decimal BiddingAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        [Range(1.0, 79228162514264337593543950335.0, ErrorMessage = "Enter valid amount.")]
        public decimal PayAmount { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string ChequeNumber { get; set; }
        public string ChequeDate { get; set; }
        public string Depot_Name { get; set; }
        public string ProduceType { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string GSTINNumber { get; set; }
        public DateTime? BidOpeningDate { get; set; }
        public DateTime? BidClosingDate { get; set; }
        public string NoticeDuration { get; set; }
        public string BidderName { get; set; }
        public string WinnerName { get; set; }
        public string InventoryLotNumber { get; set; }
        public string PlaceOfAuction { get; set; }
        public string EarnMoneyDeposit { get; set; }
        public string Notice_Status { get; set; }
        public string NoticeStatusDesc { get; set; }
        public string CircleName { get; set; }
        public string DivisionName { get; set; }
        public string DIV_CODE { get; set; }
        public string RangeName { get; set; }
        public string RegionName { get; set; }
        public string EmitraHeadCode { get; set; }
        public virtual List<Models.ForestDevelopment.DODProductDetails> DODProductList { get; set; }
        public virtual List<DropDownList2> EmitraHeadList { get; set; }
    }

    public class AuctionPayment
    {
        public string NoticeNumber { get; set; }
        public string Quantity { get; set; }
        public decimal ReservedPrice { get; set; }
        public decimal BiddingAmount { get; set; }
        public string RANGE_NAME { get; set; }
        public string Depot_Name { get; set; }
        public string ProduceType { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public DateTime? BidOpeningDate { get; set; }
        public DateTime? BidClosingDate { get; set; }
        public string BidderName { get; set; }
        public string InventoryLotNumber { get; set; }
        public string PlaceOfAuction { get; set; }
        public string EarnMoneyDeposit { get; set; }
    }

    public class AuctionRequest
    {
        public long NoticeId { get; set; }
        public string RequestedId { get; set; }
        public string ParentID { get; set; }
        public int Trn_Status_Code { get; set; }
        public string EmitraTransactionID { get; set; }
        public string Comments { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal EmitraAmount { get; set; }
    }

    public class AuctionTransaction
    {
        public long? AuctionID { get; set; }
        public string RequestedId { get; set; }
        public string Notice_Number { get; set; }
        public DateTime? BidClosingDate { get; set; }
        public decimal? EMD_Amount { get; set; }
        public decimal? BiddingAmount { get; set; }
        public decimal? Emitra_Amount { get; set; }
        public decimal? PaidAmt { get; set; }
        public decimal? TotalPaidAmt { get; set; }
        public decimal? PendingAmount { get; set; }
        public string ApplicantName { get; set; }
        public string Comments { get; set; }
        public int? TransactionStatus { get; set; }
        public string AuctionStatus { get; set; }
        public string RequestedOn { get; set; }
    }

    public class AuctionPaymentDetails
    {
        public decimal? PaidAmount { get; set; }
        public decimal? EmitraAmount { get; set; }
        public decimal? TotalPaidAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMode { get; set; }
        public string EmitraTransactionID { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string ChequeNumber { get; set; }
        public string ChequeDate { get; set; }
        public string Comment { get; set; }
    }

    public class AuctionDetailsForCustomer
    {
        public virtual List<AuctionVM> CurrentAuctionList { get; set; }
        public virtual List<AuctionTransaction> AppliedAuctionList { get; set; }
    }

    public class AuctionClearanceVM
    {
        [Required]
        [Display(Name = "InventoryLotNumber")]
        public string InventoryID { get; set; }
        [Required]
        [Display(Name = "AuctionNumber")]
        public string AuctionID { get; set; }
        public string FatherName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string DestinationAddress { get; set; }
        [Required]
        [Display(Name = "StartDateOfClearance")]
        public string ClearanceFromDate { get; set; }
        [Required]
        [Display(Name = "EndDateOfClearance")]
        public string ClearanceToDate { get; set; }
        [Required]
        public string ModeofTransport { get; set; }
        public string VehicleNumber { get; set; }
        public string Driver_License_No { get; set; }
        public string Driver_Name { get; set; }
        public string Driver_MobNo { get; set; }
        public string Remarks { get; set; }
        public virtual List<Models.ForestDevelopment.DODProductDetails> DODProductList { get; set; }
    }

    public class AuctionClearanceDetails
    {
        public long SNo { get; set; }
        public long ClearanceDetailsID { get; set; }
        public string WinnerAuctionID { get; set; }
        public string BookNumber { get; set; }
        public string FatherName { get; set; }
        public string DestinationAddress { get; set; }
        public string ClearanceFromDate { get; set; }
        public string ClearanceToDate { get; set; }
        public string Remarks { get; set; }
        public string RequestedId { get; set; }
        public string ClearanceDocPath { get; set; }
        public string ProduceWiseDocPath { get; set; }
        public string ProductWiseDocPath { get; set; }
        public string WinnerWiseDocPath { get; set; }
        public string AuctionRegisterDocPath { get; set; }
        public string Depot_Name { get; set; }
        public string WinnerName { get; set; }
        public string VehicleDescription { get; set; }
        public virtual List<Models.ForestDevelopment.DODProductDetails> DODProductList { get; set; }
    }
}