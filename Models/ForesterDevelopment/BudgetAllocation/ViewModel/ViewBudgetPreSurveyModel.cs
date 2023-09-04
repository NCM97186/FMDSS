using FMDSS.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    public class ViewBudgetPreSurveyModel
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
        public long SubActivityID { get; set; }
        public string SubActivityName { get; set; }
        // public DateTime EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string ISCircleDivision { get; set; }
        public string BudgetHead { get; set; }
        public string SubBudgetHead { get; set; }
        public int isActive { get; set; }
        public string SiteName { get; set; }
        public string IsRecurring { get; set; }
        public string Unit { get; set; }
        public decimal NumberPerUnit { get; set; }
        public decimal RatePerUnit { get; set; }
        public string RangeCode { get; set; }
        public string RangeName { get; set; }
        public int ShowDeleteButton { get; set; }
        public List<string> UploadFilesList { get; set; }
    }

    public class BudgetPreSurveyRepo : DAL
    {
        public List<ViewBudgetPreSurveyModel> GetBudgetPreSurveyList(int UserID,string Action)
        {
            List<ViewBudgetPreSurveyModel> lstBudgetAllocationCircle = new List<ViewBudgetPreSurveyModel>();
            try
            {
                Repository<ViewBudgetPreSurveyModel> repo = new Repository<ViewBudgetPreSurveyModel>();
                var param1 = new SqlParameter("@Option", Action);
                var param2 = new SqlParameter("@UserId", UserID);
                var result = repo.GetWithStoredProcedure("BA_getBudgetPreSurveyList @Option,@UserId", param1, param2).ToList();
                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                lstBudgetAllocationCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ViewBudgetPreSurveyModel>>(str);
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstBudgetAllocationCircle;
        }


        public List<string> GetBudgetPerposalUploadFiles(int PerposalID)
        {
            List<string> UploadFiles = new List<string>();
            try
            {
                Repository<string> repo = new Repository<string>();
                var param1 = new SqlParameter("@PerposalID", PerposalID);
                var param2 = new SqlParameter("@Action", "GetUploadedFilesBudgetPerposal");
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