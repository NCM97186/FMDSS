using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class Navigation : DAL
    {

        public long NavigationID { get; set; }
        public int Index { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string TextString { get; set; }


        public int isActive { get; set; }
        public bool IsactiveView { get; set; }
       
        public string OperationType { get; set; }



        public DataTable Select_Navigations()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Navigation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllNavigation");
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

        public DataTable Select_Navigation(int NavigationID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Navigation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneNavigation");
                cmd.Parameters.AddWithValue("@NavigationID", NavigationID);
                cmd.Parameters.AddWithValue("@Link", Link);
                cmd.Parameters.AddWithValue("@TextString", TextString);
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



        public DataTable AddUpdateNavigation(Navigation oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Navigation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateNavigation");
                cmd.Parameters.AddWithValue("@NavigationID", oPlace.NavigationID);
                cmd.Parameters.AddWithValue("@Link", oPlace.Link);
                cmd.Parameters.AddWithValue("@TextString", oPlace.TextString);
                cmd.Parameters.AddWithValue("@isActive", oPlace.isActive);
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