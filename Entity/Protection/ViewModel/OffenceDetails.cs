using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.Protection.ViewModel
{
    public class SeizedItemsModel
    {
        public long ID { get; set; }
        public long OffenceDetailsID { get; set; }
        public int ItemTypeID { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public bool ActiveStatus { get; set; }
        public string VehicleNumber { get; set; }
    }

    public class OffenceDetailsModel
    {
        public string RequestType { get; set; }
        public long ID { get; set; }
        //[Required]
        [Range(23,29,ErrorMessage = "Enter the GPS latitude between 23° - 29°")]
        public decimal? Latitude { get; set; }
        //[Required]
        [Range(11,int.MaxValue,ErrorMessage = "Enter the GPS longitude greater than 10°")]
        public decimal? Longitude { get; set; }
        [Required]
        public string RangeCode { get; set; }
        [Required]
        public long NakaID { get; set; }
        [Required]
        public string FIRNumber { get; set; }
        [Required]
        public string FIRDate { get; set; }
        public string FIR_UploadFiles { get; set; }
        [Required]
        public string OffenderName { get; set; }
        [Required]
        public string OffenderAddress { get; set; }
        [Required]
        public string OffenceDescription { get; set; }
        [Required (ErrorMessage ="Select Offence Category")]
        public string OffenceCategory { get; set; }
        public int? NoOfTree { get; set; }
        public decimal? VolumeInCubicMetre { get; set; }
        public bool IsWPA { get; set; }
        public string WPADescription { get; set; }
        public bool IsFA { get; set; }
        public string FADescription { get; set; }
        [Required]
        public string InvestigatorOfficer { get; set; }        
        public int CompoundStatus { get; set; } 
        public string CompoundAmount { get; set; }
        public string IsMaterialReleased { get; set; }
        public string IsVehicleReleased { get; set; }
        public string CourtName { get; set; }
        public string FileDate { get; set; }
        public string CourtCaseNumber { get; set; }
        public string NextHearingDate { get; set; }
        public string DateOfFinalReport { get; set; }
        public string DateOfApprovalByDFO { get; set; }
        [Required]
        public string SpecialRemarks { get; set; }
        public string OfficerFileUpload { get; set; }
        public Int16 StatusID { get; set; }
        public string NotCompoundedStatus { get; set; }
        public virtual List<SeizedItemsModel> SeizedItemsList { get; set; }       
        public string CompoundingDate { get; set; }
        //Added on 15-04-2020 Mukesh Jangid         
        public int Status { get; set; }

    }

    public class UploadOffenceDetailsModel
    {
        [Required]
        public string RangeCode { get; set; }
        [Required]
        public HttpPostedFileBase FileUpload { get; set; }
    }


    public class OffenceDetails
    {
        #region Global Variable
        public Int64 UserID { get; set; }
        public Int64 OffenseID { get; set; }
        public string OffenseCode { get; set; }
        public int RegFormNumber { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string RangeCode { get; set; }
        public string Tehsil { get; set; }
        public string Naka { get; set; }
        public string ForestBlock { get; set; }
        public string Compartment { get; set; }
        public string OffensePlace { get; set; }
        public string Latitude { get; set; }
        public string OffenseDate { get; set; }
        public string OffenseTime { get; set; }
        public string Longitude { get; set; }
        public string LandMark { get; set; }
        public string NakaDistance { get; set; }
        public int ForestType { get; set; }
        public string OffenceCategory { get; set; }
        public string OffenseSubCategoryWildLife { get; set; }
        public string OffenseSubCategoryForest { get; set; }
        public string WildlifeProtectionSection { get; set; }
        public string ForestProtectionSection { get; set; }
        public string OffenseSeverity { get; set; }
        public string CrimeScenePhoto1 { get; set; }
        public string CrimeScenePhoto2 { get; set; }
        public string CrimeScenePhoto3 { get; set; }
        public string ParivadiName { get; set; }

        public string ActionStatus { get; set; }
        public bool IsEditMode { get; set; }
        public string UserRole { get; set; }
        public int Status { get; set; }
        public string Offence_Description { get; set; }
        public bool Self { get; set; }
        public bool Name { get; set; }
        public string ApplicantName { get; set; }
        public string ComplaintFound { get; set; }
        public string OffenseStatus { get; set; }
        public string TypeoFForest { get; set; }
        public string AssignTo { get; set; }
        public string AssignDate { get; set; }
        public string Complaint_Found { get; set; }
        public string Mokapunchnama { get; set; }
        public string Najri_Naksha { get; set; }
        public string Witness_Recorded1 { get; set; }
        public string Witness_Recorded2 { get; set; }
        public string Witness_Recorded3 { get; set; }
        public string List_of_ArticalSeized { get; set; }
        public string Recommendation { get; set; }
        public string FieldInspection { get; set; }
        public string InvestigationCompleteDate { get; set; }
        public string DispatchNo { get; set; }
        public string ForestOfficer { get; set; }
        public string Vill_Name { get; set; }
        public string GP_Name { get; set; }
        public string Range_Name { get; set; }
        public string Beat { get; set; }
        public int No_of_offender { get; set; }
        public string ComplaintOnBhalfOf { get; set; }
        public string Description_of_offenders { get; set; }

        public string VisitDate { get; set; }
        public string VisitPlace { get; set; }
        public string VehicleSeized { get; set; }
        public string FilesToBeUploaded { get; set; }

        public string CompoundAmount { get; set; }
        public string CompoundReceipt { get; set; }
        public string CompoundDate { get; set; }
        public string CompoundBudgetHead { get; set; }
        public string ChallanNo { get; set; }
        public string BankName { get; set; }

        //Added additional
        public int ActID { get; set; }
        public int CourtType { get; set; }
        public bool IsCompounding { get; set; }
        public bool IsFIRLaunch { get; set; }
        public string FIRNumber { get; set; }

        public string FIRDate { get; set; }
        public decimal ItemSeized { get; set; }
        #endregion
    }

    public class ViewOffenceDetails
    {
        public long RowID { get; set; }
        public long OffenceDetailsID { get; set; }
        public string RequestType { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string RANGE_CODE { get; set; }
        public string RANGE_NAME { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string NakaName { get; set; }
        public string FIRNumber { get; set; }
        public string FIRDate { get; set; }

        public string RegistrationDate { get; set; }

        public string OffenderName { get; set; }
        public string OffenderAddress { get; set; }
        public string OffenceDescription { get; set; }  
        public string InvestigatorOfficer { get; set; }
        public decimal? TotalItemSeized { get; set; }
        public decimal? CompoundAmount { get; set; }
        public string CourtName { get; set; }
        public string CourtCaseNumber { get; set; }
        public string NextHearingDate { get; set; }
        public string DateOfFinalReport { get; set; }
        public string SpecialRemarks { get; set; }
        public Int16? StatusID { get; set; }
        public string StatusName { get; set; }
        public int TotalRows { get; set; }
        //Added on 15-04-2020 Mukesh JANGID
        public string RefRequestCaseStatus { get; set; }
        public string Remarks { get; set; }

        public int EditCnt { get; set; }
        public long Id { get; set; }
        public string logDate { get; set; }
        public string logTime { get; set; }

    }

    public class ViewOffenceDetailsItemWise
    {
        public long RowID { get; set; }
        public long OffenceDetailsID { get; set; }
        public string RANGE_CODE { get; set; }
        public string RANGE_NAME { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string NakaName { get; set; }
        public string FIRNumber { get; set; }
        public string FIRDate { get; set; }
        public string OffenderName { get; set; }
        public string OffenderAddress { get; set; }
        public string OffenceDescription { get; set; }
        public string InvestigatorOfficer { get; set; }
        public decimal? TotalItemSeized { get; set; }
        public decimal? CompoundAmount { get; set; }
        public string CourtName { get; set; }
        public string CourtCaseNumber { get; set; }
        public string NextHearingDate { get; set; }
        public string DateOfFinalReport { get; set; }
        public string SpecialRemarks { get; set; }
        public long SeizedItemDetailsID { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public string ItemTypeName { get; set; }
    }
     
    public class OffenceLog
    {
        public long SNo { get; set; }
        public string ActionName { get; set; }
        public string ActionBy { get; set; }
        public string Remarks { get; set; }
        public DateTime ActionDate { get; set; }
    }

    public class OffenceDetailsWithLog
    {
        public ViewOffenceDetails ViewOffenceDetails { get; set; }
        public List<OffenceLog> OffenceLogList { get; set; }
    }

    public class ApproverRemarks 
    {
        public long RequestID { get; set; }
        [Required]
        [Display(Name = "Action")]
        public Int32? StatusID { get; set; }
        [Required]
        [Display(Name = "Approver Comment")]
        public string ApproverComment { get; set; } 
    }

    #region [Report]
    #region [OffenceReport]
    public class OffenceReportVM
    {
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        [Display(Name = "Circle")]
        public string CircleCode { get; set; }
        [Display(Name = "Division")]
        public string DivisionCode { get; set; }
        [Display(Name = "Range")]
        public string RangeCode { get; set; }
        [Display(Name = "Naka")]
        public string NakaID { get; set; }
        public Int16 StatusID { get; set; }
        public string[] OffenceCategory { get; set; }

        [Display(Name = "Searchby")]
        public string Searchby { get; set; }
    }

    public class OffenceSubParamVM
    {
        public string ActionCode { get; set; } 
        public int DIST_CODE { get; set; }
        public string DIV_CODE { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    #endregion
    #region [OffenceSummaryReport] 

    public class OffenceSummaryReportVM
    {
        public int TotalNoOfCasesDetected { get; set; }
        public long TotalNoOfTree { get; set; }
        public decimal TotalVolumeInCubicMetre { get; set; }
        public int TotalNoOfCasesDisposedOff { get; set; }
        public int TotalNoOfCasesPendingInCourt { get; set; }
        public virtual List<OffenceSummaryDetailsReportVM> OffenceSummaryDetailsReportList { get; set; }
    }

    public class OffenceSummaryDetailsReportVM
    {
        public long RowNo { get; set; }
        public string StateName { get; set; }
        public string DIST_CODE { get; set; }
        public string DIST_NAME { get; set; }
        public int NoOfCasesDetected { get; set; }
        public long NoOfTree { get; set; }
        public decimal VolumeInCubicMetre { get; set; }
        public int NoOfCasesDisposedOff { get; set; }
        public int NoOfCasesPendingInCourt { get; set; } 
    }
    #endregion

    #region [OffenceSummaryQtrReport]
    public class OffenceSummaryQtrReportVM
    {
        public long RowNo { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public long PendingInCourt_LastQtr { get; set; }
        public long PendingInDept_LastQtr { get; set; }
        public long PendingInDept_LessThanOneYrs { get; set; }
        public long PendingInDept_btwnOneAndThreeYrs { get; set; }
        public long PendingInDept_GtrThanThreeYrs { get; set; }
        public long PendingInCourt_CurrentQtr { get; set; }
        public long PendingInDept_CurrentQtr { get; set; }
        public long Closed_CurrentQtr { get; set; }
        public decimal CompountAmt_CurrentQtr { get; set; }
        public long TotalPendingInCourt { get; set; }
        public long TotalPendingInDept { get; set; }
    }
    #endregion

    #region [EncroachmentSummaryReport] 

    public class EncroachmentSummaryReportVM
    {
        public int TotalNoOfCasesDetected { get; set; } 
        public int TotalNoOfCasesDisposedOff { get; set; }
        public decimal TotalAreaUnderEncroachment { get; set; }
        public decimal TotalAreaRestoredFromEncroachment { get; set; }
        public decimal TotalBalanceArea { get; set; }
        public virtual List<EncroachmentSummaryDetailsReportVM> EncroachmentSummaryDetailsReportList { get; set; }
    }

    public class EncroachmentSummaryDetailsReportVM
    {
        public long RowNo { get; set; }
        public string DIST_CODE { get; set; }
        public string DIST_NAME { get; set; }
        public int NoOfCasesDetected { get; set; }
        public int NoOfCasesDisposedOff { get; set; } 
        public decimal AreaUnderEncroachment { get; set; }
        public decimal AreaRestoredFromEncroachment { get; set; }
        public decimal BalanceArea { get; set; }
    }

    public class ViewEncroachmentDetails
    { 
        public long SNo { get; set; } 
        public long EncroachmentID { get; set; }
        public string EN_Code { get; set; }
        public string LRACTNO { get; set; }
        public string Area { get; set; }
        public string Encroachment_Area { get; set; }
        public string TotalArea { get; set; }
        public string Total_Area_Block { get; set; }
        public string Description { get; set; }
        public string DispatchNo { get; set; }
        public string DispatchDate { get; set; } 
        public string ACF_Remarks { get; set; }
        public string ACF_Date { get; set; }
        public string NoticeNo { get; set; }
        public string NoticeDate { get; set; }
        public string DIST_CODE { get; set; }
        public string DIST_NAME { get; set; }
        public string RANGE_NAME { get; set; }
        public string DIV_NAME { get; set; }
        public string Final_Decision_Taken { get; set; } 
    }
    #endregion
    #endregion
    #region Add by sunny for OffenceAPI
    public class OffenceDetailsModel_API
    {

        public long UserID { get; set; }
        public string RequestType { get; set; }
        public long ID { get; set; }

        [Range(23, 29, ErrorMessage = "Enter the GPS latitude between 23° - 29°")]
        public decimal? Latitude { get; set; }

        [Range(11, int.MaxValue, ErrorMessage = "Enter the GPS longitude greater than 10°")]
        public decimal? Longitude { get; set; }

        public string RangeCode { get; set; }

        public long NakaID { get; set; }

        public string FIRNumber { get; set; }

        public string FIRDate { get; set; }
        public string FIR_UploadFiles { get; set; }

        public string OffenderName { get; set; }

        public string OffenderAddress { get; set; }

        public string OffenceDescription { get; set; }
        public string OffenceCategory { get; set; }
        public int? NoOfTree { get; set; }
        public decimal? VolumeInCubicMetre { get; set; }
        public bool IsWPA { get; set; }
        public string WPADescription { get; set; }
        public bool IsFA { get; set; }
        public string FADescription { get; set; }

        public string InvestigatorOfficer { get; set; }
        public int CompoundStatus { get; set; }
        public string CompoundAmount { get; set; }
        public string IsMaterialReleased { get; set; }
        public string IsVehicleReleased { get; set; }
        public string CourtName { get; set; }
        public string FileDate { get; set; }
        public string CourtCaseNumber { get; set; }
        public string NextHearingDate { get; set; }
        public string DateOfFinalReport { get; set; }
        public string DateOfApprovalByDFO { get; set; }

        public string SpecialRemarks { get; set; }
        public string OfficerFileUpload { get; set; }
        public Int16 StatusID { get; set; }
        public string NotCompoundedStatus { get; set; }
        public virtual List<SeizedItemsModel> SeizedItemsList { get; set; }
        public int docTypeID { get; set; }
        public string MobileDeviceName { get; set; }
        public string MobileVersionNo { get; set; }
        public string UploadDocumnet { get; set; }
        public virtual List<OffenceDocumentUploadAPI> DocList { get; set; }
        public virtual List<CommonDocumentAPI> CommonDocApiList { get; set; }
        public string VehicleNumber { get; set; }
        public string CompoundingDate { get; set; }
    }

    public class OffenceDocumentUploadAPI
    {
        public int docTypeID { get; set; }
        public string UploadDocumnet { get; set; }       
    }
    public class CommonDocumentTypeAPI
    {
        public int DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }
        public int DocumentLevel { get; set; }
        public bool ActiveStatus { get; set; }
    }
    public class CommonDocumentAPI
    {
        public long DocumentID { get; set; }
        public int ObjectTypeID { get; set; }
        public long ObjectID { get; set; }
        public string DocumentName { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentPath { get; set; }
        public String DocumentTypeName { get; set; }
        public bool IsESign { get; set; }
        public bool ActiveStatus { get; set; }
        public string TempID { get; set; }
        public bool IsNew { get; set; }
        public int DocumentLevel { get; set; }
    }
    #endregion
}