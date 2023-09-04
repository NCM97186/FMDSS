using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FMDSS.Models.Encroachment.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
namespace FMDSS.Models.FmdssContext
{
    public class FmdssContext:DbContext
    {
        public FmdssContext() : base("name=ConnectionStringName") { 
                
        }

        public DbSet<tbl_mst_Forest_Divisions> tbl_mst_Forest_Divisions { get; set; }
        public DbSet<tbl_mst_Forest_Ranges> tbl_mst_Forest_Ranges { get; set; }
        public DbSet<Tbl_Encroachment> Tbl_Encroachment { get; set; }
        public DbSet<Tbl_Encroacher_Details> Tbl_Encroacher_Details { get; set; }
        public DbSet<tbl_UserProfiles> tbl_UserProfiles { get; set; }
        public DbSet<Tbl_Encroach_InvestigationDetails> Tbl_Encroach_InvestigationDetails { get; set; }
        public DbSet<tbl_mst_ForestEmployees> tbl_mst_ForestEmployees { get; set; }
        public DbSet<Tbl_Encroach_Appearance> Tbl_Encroach_Appearance { get; set; }
        public DbSet<tbl_mst_ForestOffices> tbl_mst_ForestOffices { get; set; }
        public DbSet<tbl_AD_District_Maping> tbl_AD_District_Maping { get; set; }
        public DbSet<tbl_BudgetAllocation_Circle> tbl_BudgetAllocation_Circle { get; set; }
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

    }
}