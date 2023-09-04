using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.Reports
{
    public class Report : DAL
    {
        public string FinancialYear { get; set; }
        public string FinancialYeartext { get; set; }
        public string DIST_CODE { get; set; }
        public string BLK_CODE { get; set; }
        public string GP_CODE { get; set; }
        public string VILL_CODE { get; set; }
        public string DIST_CODE_Name { get; set; }
        public string BLK_CODE_Name { get; set; }
        public string GP_CODE_Name { get; set; }
        public string VILL_CODE_Name { get; set; }
        public string Type { get; set; }
        public string ReportId { get; set; }
        public string CircleCode { get; set; }
        public string ModuleName { get; set; }
        public string MonthId { get; set; }
        public string MonthName { get; set; }
        public string TableName { get; set; }
        public int Status { get; set; }
        public long ID { get; set; }
        public long UserID { get; set; }
        public DataTable DT { get; set; }


        public DataTable GetFinancialYears()
        {
            try
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Select_FinancialYear", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                DataTable DT = ds.Tables[0];
                return DT;
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

        public DataTable GetReportNames()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetReportNames", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                DataTable DT = ds.Tables[0];
                return DT;
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
        //public DataTable GetPermissionType(Report rept)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        SqlCommand cmd = new SqlCommand("SP_Get_PermissionsType", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Type", Convert.ToInt16(rept.Type));
        //        cmd.Parameters.AddWithValue("@ModuleId", Convert.ToInt32(rept.ModuleId));
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        DataTable DT = ds.Tables[0];
        //        return DT;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //}


        //public DataTable GetReport(Report rept)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        SqlCommand cmd = new SqlCommand("SP_GETFMDSSREPORTS", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@FinancialYear", rept.FinancialYear);
        //        cmd.Parameters.AddWithValue("@SubModuleId", rept.SubModuleId);
        //        cmd.Parameters.AddWithValue("@ModuleId", rept.ModuleId);
        //        cmd.Parameters.AddWithValue("@DISTCODE", rept.DistCode);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        DataTable DT = ds.Tables[0];
        //        return DT;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //}

    }
}