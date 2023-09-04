using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class Users: DAL
    {
        public int Index { get; set; }
        public string UserID { get; set; }
        public string Ssoid { get; set; }
        public string Name { get; set; }

        public string RoleId { get; set; }
        public string EmailId { get; set; }

        public string Designation { get; set; }
        public string OperationType { get; set; } 

       
        public DataTable BindROLE()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UserProfileDetail", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetUserROLE");
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable Select_UserProfiles()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UserProfileDetail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllUserProfile");
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

        public DataTable Select_UserProfile(int UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UserProfileDetail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneUserProfile");
                cmd.Parameters.AddWithValue("@UserID", UserID);
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

    
        public DataTable AddUpdateUserProfile(Users oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UserProfileDetail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateUserProfile");
                cmd.Parameters.AddWithValue("@UserID", oPlace.UserID);
                cmd.Parameters.AddWithValue("@Ssoid", oPlace.Ssoid);               
                cmd.Parameters.AddWithValue("@Name", oPlace.Name);
                cmd.Parameters.AddWithValue("@RoleId", oPlace.RoleId);
                cmd.Parameters.AddWithValue("@EmailId", oPlace.EmailId);
                cmd.Parameters.AddWithValue("@Designation", oPlace.Designation);
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