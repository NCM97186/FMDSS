using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class ReasonCaseFailed : DAL
    {
        public int Index { get; set; }
        public int RID { get; set; }

        public string OperationType { get; set; }

        public string RName { get; set; }
        public string ReasonDefinition { get; set; }
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }

         
        public DateTime LastUpdatedOn { get; set; }

        public long LastUpdatedBy { get; set; }

        public DataTable Select_ReasonCaseFaileds()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ReasonCaseFailed", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllReasonCaseFailed");
                cmd.CommandType = CommandType.StoredProcedure;
               
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

        public DataTable Select_ReasonCaseFailed(int RID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ReasonCaseFailed", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneReasonCaseFailed");
                cmd.Parameters.AddWithValue("@RID ", RID);
                cmd.CommandType = CommandType.StoredProcedure;
               
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


        public DataTable AddUpdateReasonCaseFailed(ReasonCaseFailed oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ReasonCaseFailed", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateReasonCaseFailed");
                cmd.Parameters.AddWithValue("@RID ", oPlace.RID);
                cmd.Parameters.AddWithValue("@RName", oPlace.RName);
                cmd.Parameters.AddWithValue("@ReasonDefinition", oPlace.ReasonDefinition);
                cmd.Parameters.AddWithValue("@IsActive", oPlace.IsActive);
                cmd.Parameters.AddWithValue("@LastUpdatedBy", oPlace.LastUpdatedBy);
                
               cmd.CommandType = CommandType.StoredProcedure;

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