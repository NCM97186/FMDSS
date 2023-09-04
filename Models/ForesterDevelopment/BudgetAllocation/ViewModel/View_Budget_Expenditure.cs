using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMDSS.Repository;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    //[Serializable]
    public class View_Budget_Expenditure 
    {              
        public long ID { get; set; }
        public long BudgetHeadID { get; set; }
        public long SubBudgetHeadID { get; set; }
        public int FY_ID { get; set; }
        public string FinancialYear { get; set; }
        public decimal AllocatedAmount { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public long SchemeID { get; set; }
        public string SchemeName { get; set; }
        public long ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string rowid { get; set; }
        public decimal AvailableAmount { get; set; }
        public long SubActivityID { get; set; }
        public string SubActivityName { get; set; }
        public decimal ExpenditureTilldate { get; set; }
        public string ExpenditureMonths { get; set; }       
        public long EnteredBy { get; set; }
        public string Division { get; set; }
        public string Division_Name { get; set; }
        public string BudgetHead { get; set; }
        public string SubBudgetHead { get; set; }
        public string ISCircleDivision { get; set; }
        public bool IsActive { get; set; }

        public decimal RemaningAmount { get; set; }
        public long BudgetHeadAllocationID { get; set; }

        public string SanctuaryCode { get; set; }
        public string SanctuaryName { get; set; }

        public string IsWildlifeOrForest { get; set; }
        public int ShowDeleteButton { get; set; }
        public int HeaderPopUpShow { get; set; }
        public string SiteName { get; set; }
        public string WorkProgressDetails { get; set; }
        public string SiteNameExpenditure { get; set; }
        public string Remarks { get; set; }

        public string IsCoreOrBuffer { get; set; }


        
        /// //Budget development (28-11-2018)

        public string Mobile_Web { get; set; }        
        public List<string> Image { get; set; }
        public class Expenditure_Image
        {
            public string Ex_Image { get; set; }
        }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        


        public List<View_Budget_Expenditure> GetExpenditureList(int UserID)
        {                            
            try
            {
                Repository<View_Budget_Expenditure> repo = new Repository<View_Budget_Expenditure>();
                List<View_Budget_Expenditure> lstExpenditure = new List<View_Budget_Expenditure>();
                var param1 = new System.Data.SqlClient.SqlParameter("@Option", "EXPENDITURE");
                var param2 = new System.Data.SqlClient.SqlParameter("@UserId", Convert.ToInt64(UserID));
                var result = repo.GetWithStoredProcedure("BA_getBudgetExpenditure @Option,@UserId", param1, param2).ToList();                
                foreach (var item in result)
                {                   
                    lstExpenditure.Add(new View_Budget_Expenditure
                    {                     
                        FY_ID=item.FY_ID,
                        FinancialYear = item.FinancialYear,
                        rowid = Convert.ToString(item.ID),
                        CIRCLE_CODE = item.CIRCLE_CODE,
                        CIRCLE_NAME = item.CIRCLE_NAME,
                        Division=item.Division,
                        Division_Name= item.Division_Name,
                        SchemeName = item.SchemeName,
                        SchemeID = item.SchemeID,
                        IsActive = item.IsActive,
                        ActivityName = item.ActivityName,
                        ActivityID = item.ActivityID,
                        SubActivityID = item.SubActivityID,
                        SubActivityName=item.SubActivityName,
                        BudgetHead = item.BudgetHead ,
                        BudgetHeadID = item.BudgetHeadID,
                        SubBudgetHeadID = item.SubBudgetHeadID,
                        SubBudgetHead= item.SubBudgetHead,
                        AllocatedAmount = item.AllocatedAmount,
                        ExpenditureTilldate = item.ExpenditureTilldate,
                        ExpenditureMonths=item.ExpenditureMonths,
                        ISCircleDivision=item.ISCircleDivision,
                        BudgetHeadAllocationID=item.BudgetHeadAllocationID,
                        SanctuaryCode=item.SanctuaryCode,
                        SanctuaryName=item.SanctuaryName,
                        IsWildlifeOrForest=item.IsWildlifeOrForest,
                        HeaderPopUpShow = item.HeaderPopUpShow,
                        SiteName=item.SiteName,
                        SiteNameExpenditure=item.SiteNameExpenditure,
                        Remarks=item.Remarks,
                        IsCoreOrBuffer=item.IsCoreOrBuffer,
                        ShowDeleteButton=item.ShowDeleteButton
                    });
                }               
               // HttpContext.Current.Session["ExpenditureList"] = lstExpenditure;
               return lstExpenditure;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
     [Serializable]
    public class BudgetExpenditureModel : BaseModelSerializable
    {
        public BudgetExpenditureModel()
        {
            BudgetAllocationList = new List<View_BudgetAllocation_Circle>();
            BudgetExpenditureModels = new View_Budget_Expenditure();
            BudgetExpenditureList = new List<View_Budget_Expenditure>();
            MonthWiseDataEntry = new MonthlyStatusModel();
            DictionaryList = new Dictionary<string, string>();
        }
        public List<View_BudgetAllocation_Circle> BudgetAllocationList { get; set; }
        public List<View_Budget_Expenditure> BudgetExpenditureList { get; set; }
        public View_Budget_Expenditure BudgetExpenditureModels { get; set; }

        public MonthlyStatusModel MonthWiseDataEntry { get; set; }

        public Dictionary<string, string> DictionaryList { get; set; }
    }


    public class MasterCircleDivisionWithUserID:DAL
    {
        public DataSet Select_CircleDivisionWithUserID(string Action, long UserID)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_GetCirleDivisionSantatuaryByUserID", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataSet Select_SchemeDivisionWise(string Action, string  Circle_Code,string Division_Code,long UserID)
       {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("GetBudgetMasterWithCDivisionWise", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@CircleCode", Circle_Code);
                cmd.Parameters.AddWithValue("@DivisionCode", Division_Code);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}