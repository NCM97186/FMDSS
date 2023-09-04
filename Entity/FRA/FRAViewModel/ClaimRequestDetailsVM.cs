using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class ClaimRequestDetailsVM
    {
        public long ClaimRequestDetailsID { get; set; }
        public string ClaimRequestIDWithPrefix { get; set; }
        public string AppealRequestIDWithPrefix { get; set; }
        public Int32 ClaimTypeID { get; set; }
        public string ClaimTypeName { get; set; }
        public string Mobile { get; set; }
        public string RaisedBy { get; set; }
        public string PendingAt { get; set; }
        public string DistrictName { get; set; }
        public string BlockName { get; set; }
        public string VillageName { get; set; }
        public string VillageCode { get; set; }
        public string GPName { get; set; }
        public string KhasraNumber { get; set; }
        public string RequesterComment { get; set; }
        public int? CurrentApproverDesignationID { get; set; }
        public int? CurrentUserDesignationID { get; set; }
        public int? CurrentApprovalLevel { get; set; }
        public bool? IsHalkaPatwariGenerated { get; set; }
        public bool? IsForesterGenerated { get; set; }
        public bool? IsPattaGenerated { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime? RaisedOn { get; set; }

        public string ApplicantName_SpouseName { get; set; }
        public string FatherName { get; set; }
        public string CompartmentNumber { get; set; }
        public string Purpose { get; set; }
        public string KhasraCompartmentNumber { get; set; }
        public string SurvedetialsKhasraNumber { get; set; }
        public string TotalAreaAgainstKhasra { get; set; }
        public string TotalAreaAgainstOccupiedForestLand { get; set; }
        public string OccupancyType { get; set; }
        public string ForestSectionName { get; set; }
        public string GISID { get; set; }
        public string MemberName { get; set; }
        public string Individual_STribe { get; set; }
        public string Relation { get; set; }
        public string ClaimantName { get; set; }
        public string SpouseName { get; set; }
        public string KMLID { get; set; }


    }

    public class ClaimRequestSummaryVM
    {
        public int FinancialYear { get; set; }
        public Int32 ClaimTypeID { get; set; }
        public string ClaimTypeName { get; set; }
        public long? DistrictID { get; set; }
        public string DistrictName { get; set; }
        public int TotalRaisedRequest { get; set; }
        public int TotalAcceptedRequest { get; set; }
        public int TotalRejectedRequest { get; set; }
        public int TotalRequestPendingAtGramSabha { get; set; }
        public int TotalRequestPendingAtSDLC { get; set; }
        public int TotalRequestPendingAtDLC { get; set; }
        public int TotalRequestPendingFromLastSixMonth { get; set; }
    }

    public class ClaimRequestForAppealVM
    {
        public string ClaimTypeID { get; set; }
        public string ClaimRequestDate { get; set; }
        public string RejectionDate { get; set; }
        public string RejectionReason { get; set; }
        public string RejectedAt { get; set; }
        public string ClaimantName { get; set; }
        public string FatherName { get; set; }
        public string Mobile { get; set; }
        public string KhasraNumber { get; set; }
        public string CompartmentNumber { get; set; }
        public string TotalAreaAgainstOccupiedForestLand { get; set; }
        public string OccupancyType { get; set; }
        public string ForestSectionName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DistrictID { get; set; }
        public string TehsilID { get; set; }
        public string TehsilName { get; set; }
        public string BlockID { get; set; }
        public string BlockName { get; set; }
        public string GPID { get; set; }
        public string GPName { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
    }
}