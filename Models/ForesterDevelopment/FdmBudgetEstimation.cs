using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment
{

    public class FDMDCFBudgetEstimation
    {

        public long ID { get; set; }
        public long Index { get; set; }
        public long BudgetRowID { get; set; }
        public string STATE_CODE { get; set; }
        public string STATE_NAME { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public string Budget_Head { get; set; }
        public string RANGE_CODE { get; set; }
        public string RANGE_NAME { get; set; }
        public string Vill_CODE { get; set; }
        public string Vill_NAME { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal FofficeEstAmount { get; set; }
        public decimal ApprovedFinalAmount { get; set; }
        public decimal ApprovedTotalAmount { get; set; }
        public long EnteredBy { get; set; }
    }

    public class FDMROBudgetEstimation
    {

        public long ID { get; set; }
        public long Index { get; set; }
        public string Vill_CODE { get; set; }
        public string Vill_NAME { get; set; }
        public string RANGE_CODE { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal ApprovedFinalAmount { get; set; }
        public decimal ApprovedTotalAmount { get; set; }
        public long EnteredBy { get; set; }
    }

    public class FdmBudgetEstimation : DAL
    {
        public long ID { get; set; }
        public string ServeyID { get; set; }
        public string SSOID { get; set; }
        public long UserID { get; set; }
        public long Index { get; set; }
        public long FinancialYear { get; set; }
        public string FinancialYeartext { get; set; }
        public string DIST_CODE { get; set; }
        public string BLK_CODE { get; set; }
        public string GP_CODE { get; set; }
        public string VILL_CODE { get; set; }
        public string RANGE_CODE { get; set; }
        public string RANGE_NAME { get; set; }
        public string GP_CODE_Name { get; set; }
        public string VILL_CODE_Name { get; set; }
        public string Model_Name { get; set; }
        public string Activity_Name { get; set; }
        public long Model_ID { get; set; }
        public long Activity_ID { get; set; }
        public decimal EstimatedBudget { get; set; }
        public decimal ApprovedBudget { get; set; }
        public int Status { get; set; }

        public Int64 SubmitBudget(FdmBudgetEstimation _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_EstimatedBudget", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FinancialYear", _objmodel.FinancialYear);
                cmd.Parameters.AddWithValue("@ServeyID", _objmodel.ServeyID);
                cmd.Parameters.AddWithValue("@VILL_CODE", _objmodel.VILL_CODE);
                cmd.Parameters.AddWithValue("@RANGE_CODE", _objmodel.RANGE_CODE);
                cmd.Parameters.AddWithValue("@EstimatedBudget", _objmodel.EstimatedBudget);
                cmd.Parameters.AddWithValue("@EnteredBy", _objmodel.UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitBudget" + "_" + "Development", 4, DateTime.Now, _objmodel.UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public Int64 UpdateBudget(FdmBudgetEstimation _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_EstimatedBudget", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _objmodel.ID);
                cmd.Parameters.AddWithValue("@EstimatedBudget", _objmodel.EstimatedBudget);
                cmd.Parameters.AddWithValue("@UpdatedBy", _objmodel.UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateBudget" + "_" + "Development", 4, DateTime.Now, _objmodel.UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public DataSet GetAllRecords(string Code)
        {
            DataSet ds = new DataSet();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_BudgetList", Conn);
                cmd.Parameters.AddWithValue("@Code", Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetAllRecords" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetAllRecords(Int64 BudgetID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_BudgetList", Conn);
                cmd.Parameters.AddWithValue("@ID", BudgetID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateBudget" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetBudgetServeyRecords(Int64 ServeyID, string Action)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Select_Budget_ServeyDetails_By_VillageCode", Conn);
                cmd.Parameters.AddWithValue("@ServeyID", ServeyID);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateBudget" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// Fetching The Survey Details each level
        /// </summary>
        /// <param name="ServeyID"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public DataSet GetServeyRecords(string Code, string Action)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("FDM_Select_ServeyDetails", Conn);
                cmd.Parameters.AddWithValue("@Code", Code);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SurveyBudget" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable Officedetails(string SSOID, string Statementtype)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Select_FDM_LoginUser_Office", Conn);
                cmd.Parameters.AddWithValue("@SsoID", SSOID);
                cmd.Parameters.AddWithValue("@Statement", Statementtype);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Officedetails" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

        public DataTable GetBudgetByDesignation(Int64 FinancialYear, string Statement, string Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_GETESTBUDGET", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FYEAR", FinancialYear);
                cmd.Parameters.AddWithValue("@Statement", Statement);
                cmd.Parameters.AddWithValue("@CODE", Code);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetBudgetByDesignation" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataSet GetEstBudget(Int64 financialYear, string ID, int designationID)
        {
            DataSet dsEstimatedBudget = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_GETESTBUDGET", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FYEAR", financialYear);
                cmd.Parameters.AddWithValue("@CODE", ID);
                cmd.Parameters.AddWithValue("@DesignationID", designationID);
                cmd.Parameters.AddWithValue("@Statement", "Individual");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsEstimatedBudget);
                return dsEstimatedBudget;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetEstBudget" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return dsEstimatedBudget;
        }

        public Int64 SaveDCFBudgetEstimationData(DataTable dt, Int64 UserID, Int32 FinancialYear, string Code, decimal Amount, string RegionType)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_DCF_EstimatedAmount", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BudgetEstimation", dt);
                cmd.Parameters.AddWithValue("@FinancialYear", FinancialYear);
                cmd.Parameters.AddWithValue("@EnteredBy", UserID);
                cmd.Parameters.AddWithValue("@Code", Code);
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@RegionType", RegionType);



                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveDCFBudgetEstimationData" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }



    }

    public class FdmBudgetFOfficeEstimation : DAL
    {
        public string CODE { get; set; }
        public Int64 FinancialYear { get; set; }
        public int Designation { get; set; }
        public Int64 BudgetHead { get; set; }
        public decimal EstimatedAmount { get; set; }
        public string Action { get; set; }
        public string Region_Type { get; set; }
        public Int64 UserID { get; set; }

        public DataSet GetBudgetFOfficeRecords(FdmBudgetFOfficeEstimation _model)
        {

            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("FDM_BGT_FOfficeEstimate", Conn);
                cmd.Parameters.AddWithValue("@FYear", _model.FinancialYear);
                cmd.Parameters.AddWithValue("@Designation", _model.Designation);
                cmd.Parameters.AddWithValue("@BudgetHead", _model.BudgetHead);
                cmd.Parameters.AddWithValue("@Code", _model.CODE);
                cmd.Parameters.AddWithValue("@Action", _model.Action);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetFOfficeBudget" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public Int64 SubmitFOfficeBudget(FdmBudgetFOfficeEstimation _model)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("FDM_BGT_FOfficeEstimate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FYear", _model.FinancialYear);
                cmd.Parameters.AddWithValue("@Code", _model.CODE);
                cmd.Parameters.AddWithValue("@RegionType", _model.Region_Type);
                cmd.Parameters.AddWithValue("@Designation", _model.Designation);
                cmd.Parameters.AddWithValue("@EstimatedAmt", _model.EstimatedAmount);
                cmd.Parameters.AddWithValue("@BudgetHead", _model.BudgetHead);
                cmd.Parameters.AddWithValue("@Action", _model.Action);
                cmd.Parameters.AddWithValue("@UserID", _model.UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitFOfficeBudget" + "_" + "Development", 4, DateTime.Now, _model.UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        /// <summary>
        /// Function for fetching  Circle from database
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable BindCircle()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Circle1", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindCircle" + "_" + "Forest Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Fetching The Survey Details each level
        /// </summary>
        /// <param name="ServeyID"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public DataSet ExportDataRecords(string Code, int Action)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("FDM_Export_BudgetData", Conn);
                cmd.Parameters.AddWithValue("@Code", Code);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ExportBudget" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

    }

}