using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.FinanceManagement
{
    public class LCGenration : DAL
    {
        public int Index { get; set; }
        public string STATE_CODE { get; set; }
        public string STATE_NAME { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public decimal EstAmount { get; set; }
        public decimal AllocAmount { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal ReleaseAmount { get; set; }
        public long UserID { get; set; }
        public long LCreditID { get; set; }
        public string SSOID { get; set; }
        public string Div_Code { get; set; }
        public string DIV_NAME { get; set; }
        public string Status { get; set; }
        public string OfficeName { get; set; }
        public string OfficeIDNumber { get; set; }

        public string TOfficeName { get; set; }

        public string TOfficeCode { get; set; }

        public string BankBranch_Name { get; set; }

        public string Month_Year { get; set; }
        public string IFSC_Code { get; set; }

        public string Budget_HeadNumber { get; set; }

        public int BudgetTypeID { get; set; }
        public string BudgetTypeName { get; set; }
        public decimal Requested_BudgetAmount { get; set; }
        public decimal Assigned_BudgetAmount { get; set; }
        public decimal Approved_BudgetAmount { get; set; }

        public decimal RemainingBudget_Amount { get; set; }

        public decimal Past_Credit_Amount { get; set; }

        public decimal Remaining_Credit_Amount { get; set; }

        public decimal Offices_SpentAmount { get; set; }

        public string Form51_Status { get; set; }

        public int Isactive { get; set; }
        public int IsComplete { get; set; }



        public DataSet BindDDL(string Type)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Select_LC_DropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", Type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataSet GetLCDetails(string SsoID, string StatementType)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FMM_DetailsLC", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SsoID", SsoID);
                cmd.Parameters.AddWithValue("@Statement", StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public Int64 InsertDFOLCDetails(LCGenration _objModel, string StatementType)
        {
            DALConn();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("SP_FMM_DetailsLC", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OfficeName", _objModel.OfficeName);
            cmd.Parameters.AddWithValue("@OfficeIDNumber", _objModel.OfficeIDNumber);
            cmd.Parameters.AddWithValue("@TOfficeName", _objModel.TOfficeName);
            cmd.Parameters.AddWithValue("@TOfficeCode", _objModel.TOfficeCode);
            cmd.Parameters.AddWithValue("@BankBranch_Name", _objModel.BankBranch_Name);
            cmd.Parameters.AddWithValue("@IFSC_Code", _objModel.IFSC_Code);
            cmd.Parameters.AddWithValue("@Budget_HeadNumber", _objModel.Budget_HeadNumber);
            cmd.Parameters.AddWithValue("@BudgetTypeID", _objModel.BudgetTypeID);
            cmd.Parameters.AddWithValue("@DIV_CODE", _objModel.Div_Code);
            cmd.Parameters.AddWithValue("@Requested_BudgetAmount", _objModel.Requested_BudgetAmount);
            cmd.Parameters.AddWithValue("@Assigned_BudgetAmount", _objModel.Assigned_BudgetAmount);
            cmd.Parameters.AddWithValue("@RemainingBudget_Amount", _objModel.RemainingBudget_Amount);
            cmd.Parameters.AddWithValue("@Past_Credit_Amount", _objModel.Past_Credit_Amount);
            cmd.Parameters.AddWithValue("@Remaining_Credit_Amount", _objModel.Remaining_Credit_Amount);
            cmd.Parameters.AddWithValue("@Offices_SpentAmount", _objModel.Offices_SpentAmount);
            cmd.Parameters.AddWithValue("@Month_Year", _objModel.Month_Year);
            cmd.Parameters.AddWithValue("@Isactive", _objModel.Isactive);
            cmd.Parameters.AddWithValue("@UserID", _objModel.UserID);
            cmd.Parameters.AddWithValue("@Statement", StatementType);

            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        public DataTable GetAllRecords(Int64 UserID, string StatementType, Int64 ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FMM_DetailsLC", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@CreditID", ID);
                cmd.Parameters.AddWithValue("@Statement", StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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

        public DataTable GetAllHOFRec()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FMM_GETESTLC", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Statement", "HOFESTINDIVIDUAL");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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

        public DataTable GetAllCCFRec(string Cirecle_Code)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FMM_GETESTLC", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CODE", Cirecle_Code);
                cmd.Parameters.AddWithValue("@Statement", "CCFESTINDIVIDUAL");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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

        public DataTable GetHOFCredit()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FMM_GETESTLC", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Statement", "HOF");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetCCFCredit()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FMM_GETESTLC", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Statement", "CCF");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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

        public Int64 SaveDCFBudgetEstimationData(DataTable dt, Int64 UserID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_DCF_LCEstimatedAmount", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LCEstimation", dt);
                cmd.Parameters.AddWithValue("@EnteredBy", UserID);



                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveDCFBudgetEstimationData" + "_" + "FinanaceManagement", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

    }

    public class LCAllocation : DAL
    {
        public long ID { get; set; }
        public int Index { get; set; }
        public long UserID { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_Name { get; set; }
        
        public decimal Allocated_Amount { get; set; }
        public decimal ApprovedTotalAmount { get; set; }
        public decimal Estimated_Amount { get; set; }

        public string Region_Type { get; set; }

        public int IsApproved_LevelStatus { get; set; }
        public string Edit_Mode { get; set; }
        public DataTable GetHOFLOCAllocation(string StatementType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FMM_Get_AllocatedBudgetLOC", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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

        public Int64 SaveLOCAllocationData(DataTable dt, Int64 UserID, string StatementType)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FMM_Insert_LOCAllocation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AllocationData", dt);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                cmd.Parameters.AddWithValue("@UserID", UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveBudgetAllocationData" + "_" + "FinanaceManagement", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public DataTable GetCCFAllRecords(string Circle_Code, string StatementType)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FMM_Insert_LOCAllocation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CircleCode", Circle_Code);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetCCFAllRecords" + "_" + "FinanaceManagement", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public Int64 SaveCCFBudgetAllocationData(DataTable dt, Int64 UserID, string CircleCode, string StatementType)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FMM_Insert_LOCAllocation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AllocationData", dt);
                cmd.Parameters.AddWithValue("@CircleCode", CircleCode);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                cmd.Parameters.AddWithValue("@UserID", UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveCCFBudgetAllocationData" + "_" + "FinanaceManagement", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }
    }
}