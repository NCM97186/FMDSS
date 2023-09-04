using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class AppealRequestVM
    {
        public Nullable<int> ID { get; set; }
        public Nullable<int> EntryType { get; set; }
        [Display(Name = "Application No")]
        public string ClaimRequestID { get; set; } 
        [Required]
        public string AppealReason { get; set; }
        [Required]
        public Nullable<int> ClaimTypeID { get; set; }
        [Required]
        public string ClaimRequestDate { get; set; }
        [Required]
        public string RejectionDate { get; set; }
        [Required]
        public string RejectionReason { get; set; }
        [Required]
        public Nullable<int> RejectedAt { get; set; }
        [Required]
        public string ClaimantName { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public string Mobile { get; set; }
        public Nullable<bool> Individual_STribe { get; set; }
        [Required]
        [Display(Name = "District")]
        public Nullable<int> DistrictID { get; set; }
        [Required]
        [Display(Name = "Tehsil")]
        public Nullable<int> TehsilID { get; set; }
        [Required]
        [Display(Name = "Village")]
        public string VillageCode { get; set; }
        [Required]
        [Display(Name = "Gram Panchayat")]
        public Nullable<int> GPID { get; set; }
        [Required]
        [Display(Name = "Block")]
        public Nullable<int> BlockID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CompartmentNumber { get; set; }
        public string KhasraNumber { get; set; } 
        [Required]
        [Display(Name = "Occupied Forest Land")]
        public string TotalAreaAgainstOccupiedForestLand { get; set; } 
        public string OccupancyType { get; set; }
        public string ForestSectionName { get; set; }
        [Required]
        [Display(Name = "Appeal Note")]
        public string Remarks { get; set; }
        public HttpPostedFileBase UploadRejectionNoticeOrPatta { get; set; }
        public HttpPostedFileBase UploadOtherEvidenceOrdocuments { get; set; }
    }

    public class AppealRequestDetailsVM
    {
        public Nullable<long> AppealID { get; set; }
        public string AppealRequestIDWithPrefix { get; set; }
        public string ClaimRequestIDWithPrefix { get; set; }
        public string EntryType { get; set; } 
        public string ClaimRequestID { get; set; } 
        public string AppealReason { get; set; } 
        public string ClaimTypeName { get; set; } 
        public string ClaimRequestDate { get; set; } 
        public string RejectionDate { get; set; } 
        public string RejectionReason { get; set; } 
        public string RejectedAt { get; set; } 
        public string ClaimantName { get; set; } 
        public string FatherName { get; set; } 
        public string Mobile { get; set; }
        public Nullable<bool> Individual_STribe { get; set; }  
        public string DistrictName { get; set; }  
        public string BlockName { get; set; } 
        public string GPName { get; set; } 
        public string VillageName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CompartmentNumber { get; set; }
        public string KhasraNumber { get; set; } 
        public string TotalAreaAgainstOccupiedForestLand { get; set; }
        public string OccupancyType { get; set; }
        public string ForestSectionName { get; set; } 
        public string SpecialRemarks { get; set; }
        public string RaisedBy { get; set; }
        public string AppealStatus { get; set; }
    }
}