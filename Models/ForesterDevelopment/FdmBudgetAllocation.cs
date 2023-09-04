using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment
{
    public class FdmBudgetAllocation : DAL
    {
        public long ID { get; set; }
        public string SSOID { get; set; }
        public Int64 UserID { get; set; }
        public Int64 EstimatedID { get; set; }
        public Int64 Index { get; set; }
        
        public string Edit_Mode { get; set; }

        public long Model_ID { get; set; }

        public long Activity_ID { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string STATE_CODE { get; set; }
        public string STATE_NAME { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_Name { get; set; }        
        public string RANGE_CODE { get; set; }
        public string RANGE_NAME { get; set; }
        public string VILL_CODE { get; set; }
        public string VIll_Name { get; set; }

        public decimal Allocated_Amount { get; set; }
        public string Budget_Head { get; set; }
        public Int64 Budget_HeadID { get; set; }
        public decimal Estimated_Amount { get; set; }
        public bool ConditionalGridShow { get; set; }
        public int FinancialYear { get; set; }
        public decimal ApprovedTotalAmount { get; set; }
        public DataSet GetHQAllRecords(Int64 FinancialYear, string StatementType = "HQ")
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Get_AllocatedBudgetList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FinancialYear", FinancialYear);
                cmd.Parameters.AddWithValue("@StatementType", @StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetHQAllRecords" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataSet GetCCFAllRecords(Int64 FinancialYear, string Circle_Code, string StatementType)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Get_AllocatedBudgetList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FinancialYear", FinancialYear);
                cmd.Parameters.AddWithValue("@CircleCode", Circle_Code);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetCCFAllRecords" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataSet GetDCFAllRecords(Int64 FinancialYear, string Div_Code, string StatementType)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Get_AllocatedBudgetList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FinancialYear", FinancialYear);
                cmd.Parameters.AddWithValue("@DivCode", Div_Code);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetDCFAllRecords" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetROAllRecords(Int64 FinancialYear, string RANGE_CODE, string StatementType)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Get_AllocatedBudgetList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FinancialYear", FinancialYear);
                cmd.Parameters.AddWithValue("@RangeCode", RANGE_CODE);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetROAllRecords" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataSet EditHQBudget(Int64 BudgetAllocID, string StatementType = "EditHQ")
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Get_AllocatedBudgetList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BudgetAllocID", BudgetAllocID);
                cmd.Parameters.AddWithValue("@StatementType", @StatementType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "EditHQBudget" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public Int64 SaveBudgetAllocationData(DataTable dt,DataTable dt1, Int64 UserID, string StatementType)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Insert_BudgetAllocation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BudgetAllocationData", dt);
                cmd.Parameters.AddWithValue("@SelfBudgetAllocationData", dt1);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                cmd.Parameters.AddWithValue("@UserID", UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveBudgetAllocationData" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public Int64 SaveCCFBudgetAllocationData(DataTable dt, DataTable dt1, Int64 UserID, string CircleCode, string StatementType)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Insert_BudgetAllocation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BudgetAllocationData", dt);
                cmd.Parameters.AddWithValue("@SelfBudgetAllocationData", dt1);
                cmd.Parameters.AddWithValue("@CircleCode", CircleCode);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                cmd.Parameters.AddWithValue("@UserID", UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveCCFBudgetAllocationData" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public Int64 SaveDCFBudgetAllocationData(DataTable dt, DataTable dt1, Int64 UserID, string DivCode, string StatementType)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Insert_BudgetAllocation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BudgetAllocationData", dt);
                cmd.Parameters.AddWithValue("@SelfBudgetAllocationData", dt1);
                cmd.Parameters.AddWithValue("@DivCode", DivCode);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                cmd.Parameters.AddWithValue("@UserID", UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveDCFBudgetAllocationData" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public Int64 SaveROBudgetAllocationData(DataTable dt, DataTable dt1, Int64 UserID, string RangeCode, string StatementType)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Insert_BudgetAllocation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BudgetAllocationData", dt);
                cmd.Parameters.AddWithValue("@SelfBudgetAllocationData", dt1);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                cmd.Parameters.AddWithValue("@UserID", UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveROBudgetAllocationData" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        

    }

}