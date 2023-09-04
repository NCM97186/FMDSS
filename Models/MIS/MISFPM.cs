using FMDSS.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.MIS
{
    public class MISFPM : DAL
    {

        public int Index { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string HIERARCHY_CODE { get; set; }
        public string OFFENSE_CODE { get; set; }
        public string OffenseStatus {get;set;}
        public string OffenseStatusText { get; set; }

        public string AssignTo { get; set; }
        public string AssignDate { get; set; }
        public List<SelectListItem> HIERARCHY_LEVEL_CODE { get; set; }

        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }

        public string CircleStatus { get; set; }
        public string DivisionStatus { get; set; }
        public string RangeStatus { get; set; }





        public DataTable GET_FPM_HIERARCHY_LEVEL(Int64 UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_SELECTFPM_HIERARCHY_LEVEL", Conn);             
                cmd.Parameters.AddWithValue("@UserID", UserID);               
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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


        public DataTable BASE_DETAILS(MISFPM obj)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_FPM_RangeLevelParivadDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "BASE_DETAILS");
                cmd.Parameters.AddWithValue("@DATETIME_FROM", obj.FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", obj.ToDate);
                cmd.Parameters.AddWithValue("@Code", obj.Range);
                cmd.Parameters.AddWithValue("@OffenseStatus", "");
                cmd.Parameters.AddWithValue("@Offense_code", "");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

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

        public DataTable GET_OFFENSE_CODE(MISFPM obj)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_FPM_RangeLevelParivadDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "GET_OFFENSE_CODE");
                cmd.Parameters.AddWithValue("@DATETIME_FROM", obj.FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", obj.ToDate);
                cmd.Parameters.AddWithValue("@Code", obj.HIERARCHY_CODE);
                cmd.Parameters.AddWithValue("@OffenseStatus", obj.OffenseStatus);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

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

        public DataSet COMPLETE_DETAILS(MISFPM obj)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("MIS_FPM_RangeLevelParivadDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "COMPLETE_DETAILS");
                cmd.Parameters.AddWithValue("@Offense_code", obj.OFFENSE_CODE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

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






    }
}