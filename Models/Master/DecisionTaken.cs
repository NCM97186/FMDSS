using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class DecisionTaken : DAL
    {
        public int Index { get; set; }
        public int DID { get; set; }

        public string OperationType { get; set; }

        public string DName { get; set; }
        public string DecisionDefinition { get; set; }
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }

         
        public DateTime LastUpdatedOn { get; set; }

        public long LastUpdatedBy { get; set; }

        public DataTable Select_DecisionTakens()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_DecisionTaken", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllDecisionTaken");
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

        public DataTable Select_DecisionTaken(int DID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_DecisionTaken", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneDecisionTaken");
                cmd.Parameters.AddWithValue("@DID ", DID);
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


        public DataTable AddUpdateDecisionTaken(DecisionTaken oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_DecisionTaken", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateDecisionTaken");
                cmd.Parameters.AddWithValue("@DID ", oPlace.DID);
                cmd.Parameters.AddWithValue("@DName", oPlace.DName);
                cmd.Parameters.AddWithValue("@DecisionDefinition", oPlace.DecisionDefinition);
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