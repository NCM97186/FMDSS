using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.ViewModel
{
    public class cls_FraDetails
    {
        public int ActiveStatus { get; set; }
        public int TransactionStatus { get; set; }
        public int ClaimRequestDetailsID { get; set; }
        public int IsHalkaPatwariGenerated { get; set; }
        public int IsForesterGenerated { get; set; }
        public int IsPattaGenerated { get; set; }

        public string ClaimRequestIDWithPrefix { get; set; }
        public string RaisedBy { get; set; }
        public string ClaimTypeID { get; set; }
        public string ClaimTypeName { get; set; }
        public string PendingAt { get; set; }
        public string RaisedOn { get; set; }
        public string DistrictName { get; set; }
        public string BlockName { get; set; }
        public string VillageName { get; set; }
        public string VillageCode { get; set; }

        public string GPName { get; set; }
        public string RequesterComment { get; set; }
        public string ApplicationType { get; set; }
        public string RejectedRefNumber { get; set; }
        public string RejectedRequestDate { get; set; }
        public string RejectedDate { get; set; }

        public string RejectedReason { get; set; }
        public string CurrentApproverDesignationID { get; set; }
        public string CurrentStatus { get; set; }
        public string ClaimantName { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string KhasraNumber { get; set;}
        public string TotalAreaAgainstKhasra {get; set; }
        

    }
}