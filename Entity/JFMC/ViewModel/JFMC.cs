using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.JFMC.ViewModel
{
    public class JFMCRegistration
    {
        public long JFMCRegistrationID { get; set; }
        [Required]
        public string CircleCode { get; set; }
        [Required]
        public string DivisionCode { get; set; }
        [Required]
        public string RangeCode { get; set; }
        [Required]
        public long NakaID { get; set; }
        [Required]
        [Range(23, 29, ErrorMessage = "Enter the GPS latitude between 23° - 29°")]
        public decimal? Latitude { get; set; }
        [Required]
        [Range(11, int.MaxValue, ErrorMessage = "Enter the GPS longitude greater than 10°")]
        public decimal? Longitude { get; set; }
        [Required]
        public string CommitteeName { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string RegistrationDate { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public decimal? CorpusFundDeposit { get; set; }
        public decimal? TotalRevenueDuringYear { get; set; }
        public bool? IsEcotourismManagementExist { get; set; }
        public HttpPostedFileBase UploadPlanEvidence { get; set; }
        public string Grade { get; set; }
        public string Audited { get; set; }
        public HttpPostedFileBase UploadAuditEvidence { get; set; }
        public string LatestGeneralBodyMeetingDate { get; set; }
        public string LastAuditDate { get; set; }
        public HttpPostedFileBase UploadGeneralBodyMOMEvidence { get; set; }
        public string LatestExecutiveBodyMeetingDate { get; set; }
        [Required]
        public string Remarks { get; set; }
        public HttpPostedFileBase UploadCMemberMOMEvidence { get; set; }

        public int? TotalSCCategory { get; set; }
        public int? TotalSTCategory { get; set; }
        public int? TotalFemaleCategory { get; set; }
        public int? TotalOthersCategory { get; set; }
        public string ManagedAreaOrEcotourismSiteName { get; set; }
        public virtual List<Member> MemberList { get; set; }
    }

    public class Member
    {
        public long MemberID { get; set; }
        public string MemberName { get; set; }
    }

    public class ViewJFMCRegistration
    {
        public long RowNo { get; set; }
        public long JFMCRegistrationID { get; set; }
        public string CircleName { get; set; }
        public string DivisionName { get; set; }
        public string RangeName { get; set; }
        public string NakaName { get; set; }
        public string CommitteeName { get; set; }
        public string RegistrationNumber { get; set; }
        public string RegistrationDate { get; set; }
        public string ManagedAreaOrEcotourismSiteName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public decimal? CorpusFundDeposit { get; set; }
        public decimal? TotalRevenueDuringYear { get; set; }
        public bool? IsEcotourismManagementExist { get; set; }
        public string UploadPlanEvidence { get; set; }
        public string Grade { get; set; }
        public string LastAuditDate { get; set; }
        public string Audited { get; set; }
        public string LatestGeneralBodyMeetingDate { get; set; }
        public string LatestExecutiveBodyMeetingDate { get; set; }
        public string ExecutiveCommitteeMemberNames { get; set; }
        public string UploadMinutesOfMeetingEvidence { get; set; }
        public int? TotalSCCategory { get; set; }
        public int? TotalSTCategory { get; set; }
        public int? TotalFemaleCategory { get; set; }
        public int? TotalOthersCategory { get; set; }
        public int? TotalMember { get; set; }
    }
}


