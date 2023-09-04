using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class ForestCategory : DAL
    {
        public int Index { get; set; }
        public int FOCatID { get; set; }

        public string OperationType { get; set; }

        public string FOCategory { get; set; }

        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }

         
        public DateTime LastUpdatedOn { get; set; }

        public long LastUpdatedBy { get; set; }

        public DataTable Select_ForestCategorys()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ForestCategory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllForestCategory");
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

        public DataTable Select_ForestCategory(int FOCatID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ForestCategory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneForestCategory");
                cmd.Parameters.AddWithValue("@FOCatID ", FOCatID);
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


        public DataTable AddUpdateForestCategory(ForestCategory oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ForestCategory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateForestCategory");
                cmd.Parameters.AddWithValue("@FOCatID ", oPlace.FOCatID);
                cmd.Parameters.AddWithValue("@FOCategory", oPlace.FOCategory);
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