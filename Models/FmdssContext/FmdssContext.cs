using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FMDSS.Models.Encroachment.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Entity;
using FMDSS.Models.Master;

namespace FMDSS.Models.FmdssContext
{
    public class FmdssContext : DbContext
    {
        public FmdssContext()
            : base("name=ConnectionStringName")
        {

        }

        //Added by Sunny for decimal Issues
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Precision attribute for decimals
            My.Data.Annotations.Precision.ConfigureModelBuilder(modelBuilder);
        }
        //done
        public DbSet<tbl_mst_Forest_Divisions> tbl_mst_Forest_Divisions { get; set; }
        public DbSet<tbl_mst_Forest_Ranges> tbl_mst_Forest_Ranges { get; set; }
        public DbSet<Tbl_Encroachment> Tbl_Encroachment { get; set; }
        public DbSet<Tbl_Encroacher_Details> Tbl_Encroacher_Details { get; set; }
        public DbSet<tbl_UserProfiles> tbl_UserProfiles { get; set; }
        public DbSet<Tbl_Encroach_InvestigationDetails> Tbl_Encroach_InvestigationDetails { get; set; }
        public DbSet<tbl_mst_ForestEmployees> tbl_mst_ForestEmployees { get; set; }
        public DbSet<Tbl_Encroach_Appearance> Tbl_Encroach_Appearance { get; set; }
        public DbSet<FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel.tbl_mst_ForestOffices> tbl_mst_ForestOffices { get; set; }
        public DbSet<tbl_AD_District_Maping> tbl_AD_District_Maping { get; set; }
        public DbSet<tbl_BudgetAllocation_Circle> tbl_BudgetAllocation_Circle { get; set; }
        public DbSet<tbl_BudgetPreSurvey> tbl_BudgetPreSurvey { get; set; }
        public DbSet<tbl_RoleMapping> tbl_RoleMapping { get; set; }


        public DbSet<tbl_BudgetAllocation_CircleLevels> tbl_BudgetAllocation_CircleLevels { get; set; }

        public DbSet<tbl_BudgetAllocationPerposal> tbl_BudgetAllocationPerposal { get; set; }
        public DbSet<tbl_BudgetPerposalFilesUpload> tbl_BudgetPerposalFilesUpload { get; set; }

        public DbSet<tbl_BudgetPreSurveyFilesUpload> tbl_BudgetPreSurveyFilesUpload { get; set; }
        public DbSet<tbl_BudgetAllocation_Division> tbl_BudgetAllocation_Division { get; set; }
        public DbSet<tbl_BudgetAllocation_Village> tbl_BudgetAllocation_Village { get; set; }
        public DbSet<tbl_BudgetHead_Allocation> tbl_BudgetHead_Allocation { get; set; }
        public DbSet<tbl_mst_BudgetHead> tbl_mst_BudgetHead { get; set; }
        public DbSet<tbl_mst_FinancialYear> tbl_mst_FinancialYear { get; set; }
        public DbSet<tbl_mst_SubBudgetHead> tbl_mst_SubBudgetHead { get; set; }

        public DbSet<tbl_FDM_Scheme> tbl_FDM_Scheme { get; set; }
        public DbSet<tbl_FDM_SchemeForWidelife> tbl_FDM_SchemeForWidelife { get; set; }
        public DbSet<tbl_mst_Forest_WildLifeCircles> tbl_mst_Forest_WildLifeCircles { get; set; }
        public DbSet<tbl_mst_Villages> tbl_mst_Villages { get; set; }
        public DbSet<tbl_mst_ActivityForWidelife> tbl_mst_ActivityForWidelife { get; set; }
        public DbSet<tbl_mst_SUBActivityForWidelife> tbl_mst_SUBActivityForWidelife { get; set; }
        public DbSet<tbl_mst_FDM_Activity> tbl_mst_FDM_Activity { get; set; }
        public DbSet<tbl_mst_FDM_Sub_Activity> tbl_mst_FDM_Sub_Activity { get; set; }


        public DbSet<tbl_Budget_Expenditure> tbl_Budget_Expenditure { get; set; }

        public DbSet<tbl_mst_Places> tbl_mst_Places { get; set; }
        public DbSet<tbl_mst_Zone> tbl_mst_Zone { get; set; }
        public DbSet<NP_VisitorTypeMaster> NP_VisitorTypeMaster { get; set; }
        public DbSet<NP_HeadMaster> NP_HeadMaster { get; set; }
        public DbSet<NP_VehicleMaster> NP_VehicleMaster { get; set; }
        public DbSet<NP_ShiftMaster> NP_ShiftMaster { get; set; }
        public DbSet<NP_FeesItemMaster> NP_FeesItemMaster { get; set; }
        public DbSet<NP_PlaceRelation> NP_PlaceRelation { get; set; }
        public DbSet<NP_HeadWiseItemFee> NP_HeadWiseItemFee { get; set; }

        #region FRA
        public DbSet<tbl_FRA_ClaimRequestDetails> tbl_FRA_ClaimRequestDetails { get; set; }
        public DbSet<tbl_FRA_ClaimantDetails> tbl_FRA_ClaimantDetails { get; set; }
        public DbSet<tbl_FRA_BorderingVillageDetails> tbl_FRA_BorderingVillageDetails { get; set; }
        public DbSet<tbl_FRA_MemberDetails> tbl_FRA_MemberDetails { get; set; }
        public DbSet<tbl_FRA_ClaimRequestDocument> tbl_FRA_ClaimRequestDocument { get; set; }
        public DbSet<tbl_FRA_WorkFlowRule> tbl_FRA_WorkFlowRule { get; set; }
        public DbSet<tbl_FRA_WorkFlowDetails> tbl_FRA_WorkFlowDetails { get; set; }
        public DbSet<tbl_FRA_DesignationPermission> tbl_FRA_DesignationPermission { get; set; }
        public DbSet<tbl_FRA_ClaimType> tbl_FRA_ClaimType { get; set; }
        //public DbSet<tbl_FRA_DocumentType> tbl_FRA_DocumentType { get; set; }
        public DbSet<tbl_FRA_ClaimRequestPurpose> tbl_FRA_ClaimRequestPurpose { get; set; }
        public DbSet<tbl_FRA_SurveyDetails> tbl_FRA_SurveyDetails { get; set; }
        public DbSet<tbl_FRA_KhasraDetails> tbl_FRA_KhasraDetails { get; set; }
        public DbSet<tbl_FRA_ActionReason> tbl_FRA_ActionReason { get; set; }
        //New
        public DbSet<tbl_FRA_Tehsil> tbl_FRA_Tehsil { get; set; }
        public DbSet<tbl_FRA_GPs> tbl_FRA_GPs { get; set; }
        public DbSet<tbl_FRA_Block> tbl_FRA_Block { get; set; }
        public DbSet<tbl_FRA_Village> tbl_FRA_Village { get; set; }
        public DbSet<tbl_FRA_District> tbl_FRA_District { get; set; }

        ////For Budget Development (28-11-2018)
        public DbSet<tbl_BudgetExpenditure_Image> tbl_BudgetExpenditure_Image { get; set; }


        #endregion
    }
}