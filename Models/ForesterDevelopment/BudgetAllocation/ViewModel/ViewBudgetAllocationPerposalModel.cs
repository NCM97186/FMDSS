using FMDSS.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class ViewBudgetAllocationPerposalModel
    {
        public long ID { get; set; }
        public long BudgetHeadID { get; set; }
        public long SubBudgetHeadID { get; set; }
        public int FY_ID { get; set; }
        public string FinancialYear { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public long SchemeID { get; set; }
        public string SchemeName { get; set; }
        public long ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string rowid { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal RemaningAmount { get; set; }
        public long SubActivityID { get; set; }
        public string SubActivityName { get; set; }
        // public DateTime EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public string Div_Code { get; set; }
        public string Division { get; set; }
        public string ISCircleDivision { get; set; }
        public string BudgetPerposalType { get; set; }
        public string BudgetHead { get; set; }
        public string SubBudgetHead { get; set; }

        public bool isActive { get; set; }

        public long BudgetHeadAllocationID { get; set; }

        public string SiteName { get; set; }

        public int IsRecurring { get; set; }

        public string RecurringName { get; set; }

        public string HeadType { get; set; }

        public string Unit { get; set; }
        public decimal NumberPerUnit { get; set; }
        public decimal RatePerUnit { get; set; }

        public string SanctuaryCode { get; set; }
        public string RangeCode { get; set; }//Added by dipak
        public long? NakaID { get; set; }//Added by dipak
        public string SanctuaryName { get; set; }

        public string IsWildlifeOrForest { get; set; }

        public int ShowDeleteButton { get; set; }

        public string PlaceArea { get; set; }
        public string GISID { get; set; }

        public List<string> UploadFilesList { get; set; }
    }

    public class BudgetPerposalRepo : DAL
    {

        public List<ViewBudgetAllocationPerposalModel> GetBudgetPerposalCircleList(int UserID)
        {
            List<ViewBudgetAllocationPerposalModel> lstBudgetAllocationCircle = new List<ViewBudgetAllocationPerposalModel>();
            try
            {
                Repository<ViewBudgetAllocationPerposalModel> repo = new Repository<ViewBudgetAllocationPerposalModel>();
                var param1 = new SqlParameter("@Option", "CIRCLE");
                var param2 = new SqlParameter("@UserId", UserID);
                var result = repo.GetWithStoredProcedure("BA_getBudgetPerposalList @Option,@UserId", param1, param2).ToList();
                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                lstBudgetAllocationCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ViewBudgetAllocationPerposalModel>>(str);
                #endregion
                return lstBudgetAllocationCircle;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<string> GetBudgetPerposalUploadFiles(int PerposalID, string Action)
        {
            List<string> UploadFiles = new List<string>();
            try
            {
                Repository<string> repo = new Repository<string>();
                var param1 = new SqlParameter("@PerposalID", PerposalID);
                var param2 = new SqlParameter("@Action", Action);
                UploadFiles = repo.GetWithStoredProcedure("SP_GetBudgetPerposalUploadFiles @PerposalID,@Action", param1, param2).ToList();

                return UploadFiles;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}