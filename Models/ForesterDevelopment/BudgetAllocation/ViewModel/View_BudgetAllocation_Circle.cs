using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using FMDSS.Repository;
using FMDSS.Models.CitizenService.PermissionServices;
using System.Data;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{

    public class BudgetAllocationGISInformation : View_BudgetAllocation_Circle
    {
        public BudgetAllocationGISInformation()
        {
            GISInformationList = new List<GISDataBaseModel>();
            BudgetAllocationDetails = new View_BudgetAllocation_Circle();
            GISInformationName = new List<clsPermission>();
        }
        public List<GISDataBaseModel> GISInformationList { get; set; }
        public View_BudgetAllocation_Circle BudgetAllocationDetails { get; set; }

        public List<clsPermission> GISInformationName { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string GISID { get; set; }

        public Decimal AreaShape { get; set; }

        public string BudgetExpenditureID { get; set; }
    }

   
    public class View_BudgetAllocation_Circle : BaseModelSerializable
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
        [My.Data.Annotations.Precision(18, 5)]
        public decimal AllocatedAmount { get; set; }

        [My.Data.Annotations.Precision(18, 5)]
        public decimal TotalAmount { get; set; }

        public decimal ExtraAllocatedAmount { get; set; }
        public decimal RemaningAmount { get; set; }
        public long SubActivityID { get; set; }
        public string SubActivityName { get; set; }
        // public DateTime EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public string Div_Code { get; set; }
        public string Division { get; set; }
        public string ISCircleDivision { get; set; }
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

        public string SanctuaryName { get; set; }

        public string IsWildlifeOrForest { get; set; }

        public int ShowDeleteButton { get; set; }

        public string IsCoreOrBuffer { get; set; }

        public long tbl_BudgetAllocation_ParentID { get; set; }

        public string Colors { get; set; }
    }

    
    /// For Budget Development Mobile APP (28-11-2018)
    public class BugdetExpenditure_RemainingMonthsOnID
    {
        public string MonthName { get; set; }
        public decimal ExpenditureTilldate { get; set; }
        public string SiteNameExpenditure { get; set; }
        public string Remarks { get; set; }

    }
    

    [Serializable]
    public class View_BudgetAllocation_CircleForGIS : BaseModelSerializable
    {
        public string CIRCLE_NAME { get; set; }
        public string SubBudgetHead { get; set; }
        public string HeadType { get; set; }
        public string Division { get; set; }
        public string FinancialYear { get; set; }
        public string SchemeName { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityName { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string ISCircleDivision { get; set; }
        public string IsWildlifeOrForest { get; set; }

    }

    [Serializable]
    public class View_BudgetAllocationExpemditureForGIS : BaseModelSerializable
    {
        public string CIRCLE_NAME { get; set; }
        public string SubBudgetHead { get; set; }
        public string BudgetHead { get; set; }
        public string Division_Name { get; set; }
        public string FinancialYear { get; set; }
        public string SchemeName { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityName { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal RemaningAmount { get; set; }
        public string ISCircleDivision { get; set; }
        public decimal ExpenditureTilldate { get; set; }
        public string IsWildlifeOrForest { get; set; }

    }

    [Serializable]
    public class BudgetAllocationLogModel : BaseModelSerializable
    {
        public string Ssoid { get; set; }
        public string CreatedDate { get; set; }
        public string AllocatedAmountCircleLevel { get; set; }
        public string AllocatedAmountDistribute { get; set; }

        public string ExtraAllocatedAmount { get; set; }

        public string TotalAmount { get; set; }
        public string OfficeName { get; set; }

    }

    [Serializable]
    public class BudgetAllocationModel : BaseModelSerializable
    {
        public BudgetAllocationModel()
        {
            BudgetHeadMasterList = new List<View_BudgetHead_Allocation>();
            BudgetAllocationModels = new View_BudgetAllocation_Circle();
            BudgetAllocationList = new List<View_BudgetAllocation_Circle>();
        }
        public List<View_BudgetHead_Allocation> BudgetHeadMasterList { get; set; }
        public List<View_BudgetAllocation_Circle> BudgetAllocationList { get; set; }
        public View_BudgetAllocation_Circle BudgetAllocationModels { get; set; }
    }

    public class BudgetAllocationRepo : DAL
    {

        public List<View_BudgetAllocation_Circle> GetBudgetAllocationCircleList(int UserID)
        {
            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = new List<View_BudgetAllocation_Circle>();
            try
            {
                Repository<View_BudgetAllocation_Circle> repo = new Repository<View_BudgetAllocation_Circle>();
                var param1 = new SqlParameter("@Option", "CIRCLE");
                var param2 = new SqlParameter("@UserId", UserID);
                var result = repo.GetWithStoredProcedure("BA_getBudgetAllocationList @Option,@UserId", param1, param2).ToList();
                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                lstBudgetAllocationCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);
                #endregion

                //foreach (var item in result)
                //{
                //    lstBudgetAllocationCircle.Add(new View_BudgetAllocation_Circle
                //    {
                //        ID = item.ID,
                //        BudgetHeadAllocationID = item.BudgetHeadAllocationID,
                //        CIRCLE_NAME = item.CIRCLE_NAME + "/" + item.Division,
                //        SchemeName = item.SchemeName,
                //        ActivityName = item.ActivityName + "/" + item.SubActivityName,
                //        BudgetHead = item.BudgetHead + "/" + item.SubBudgetHead,
                //        AllocatedAmount = item.AllocatedAmount,
                //        TotalAmount = item.TotalAmount,
                //        ISCircleDivision = item.ISCircleDivision
                //    });
                //}
                return lstBudgetAllocationCircle;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable InsertGisInformationBudgetExpenditure(string Action, string ExpenditureID, Int64 userID, DataTable GISTable)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("[Sp_BudgetExpentitureGISInformation]", Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@ExpenditureID", ExpenditureID);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@GISModelList", GISTable);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public BudgetAllocationCircleLevelModel GetBudgetAllocationCircleLevel(int UserID, BudgetAllocationCircleLevelModel Model)
        {
            try
            {

                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("BA_getBudgetAllocationListForDivisionLevel", Conn);
                cmd.Parameters.AddWithValue("@Option", "CIRCLE");
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.Parameters.AddWithValue("@ID", 0);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);

                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                Model.BudgetAllocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);

                str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1]);
                Model.BudgetAllocationModelsCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            return Model;
        }

        public List<BudgetAllocationLogModel> GetBudgetAllocationCircleLevelLog(int ID, long UserID)
        {
            List<BudgetAllocationLogModel> List = new List<BudgetAllocationLogModel>();
            try
            {

                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("BA_getBudgetAllocationListForDivisionLevel", Conn);
                cmd.Parameters.AddWithValue("@Option", "GETBUDGETLOG");
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);

                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BudgetAllocationLogModel>>(str);

                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            return List;
        }
    }

    public class BudgetAllocationCircleLevelModel
    {
        public BudgetAllocationCircleLevelModel()
        {
            BudgetAllocationModels = new View_BudgetAllocation_Circle();
            BudgetAllocationModelsCircle = new List<View_BudgetAllocation_Circle>();
            BudgetAllocationList = new List<View_BudgetAllocation_Circle>();
        }
        public List<View_BudgetAllocation_Circle> BudgetAllocationList { get; set; }
        public List<View_BudgetAllocation_Circle> BudgetAllocationModelsCircle { get; set; }
        public View_BudgetAllocation_Circle BudgetAllocationModels { get; set; }
    }
}