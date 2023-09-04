using FMDSS.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{ 
    public class ClaimRequestOTVM
    {
        public ClaimRequestOTVM()
        {
            DocumentTypeList = new List<DropDownListVM>();
            ClaimTypeList = new List<DropDownListVM>();
            DistrictList = new List<DropDownListVM>();
            TehsilList = new List<DropDownListVM>();
            BlockList = new List<DropDownListVM>();
            GPList = new List<DropDownListVM>();
            VillageList = new List<DropDownList2VM>();
            ClaimRequestPurposeList = new List<DropDownListVM>();
        }
        #region Navigation Property
        public virtual tbl_FRA_ClaimRequestDetailsOT ClaimRequestDetails { get; set; }
        public virtual tbl_FRA_ClaimantDetails ClaimantDetails { get; set; }
        public virtual tbl_FRA_MemberDetails MemberDetails { get; set; }
        public virtual List<tbl_FRA_BorderingVillageDetails> BorderingVillageDetails { get; set; }
        public virtual List<tbl_FRA_ClaimantDetails> ClaimantDetailsList { get; set; }
        public virtual List<CommonDocument> ClaimRequestDocument { get; set; }
        public virtual List<tbl_FRA_MemberDetails> MemberDetailsList { get; set; }
        public virtual List<DropDownListVM> DocumentTypeList { get; set; }
        public virtual List<DropDownListVM> ClaimTypeList { get; set; }
        public virtual List<DropDownListVM> DistrictList { get; set; }
        public virtual List<DropDownListVM> TehsilList { get; set; }
        public virtual List<DropDownListVM> BlockList { get; set; }
        public virtual List<DropDownListVM> GPList { get; set; }
        public virtual List<DropDownList2VM> VillageList { get; set; }
        public virtual List<DropDownListVM> ClaimRequestPurposeList { get; set; }
        public virtual List<tbl_FRA_WorkFlowRule> WorkFlowRuleList { get; set; }
        public virtual List<ApproverRemarksOTVM> ApproverRemarksVM { get; set; }
        #endregion
    }
}