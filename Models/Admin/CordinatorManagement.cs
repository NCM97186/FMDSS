using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Admin
{
    public class CordinatorManagement : DAL
    {
        #region data members
        public Int64 CoordinatorId { get; set; }
        public string CoordinatorNo { get; set; }
        public string CoordinatorName { get; set; }
        public string SSOID { get; set; }
        public string Address { get; set; }
        public string DistrictCode { get; set; }
        public string Pincode { get; set; }
        public Int64 EnteredBy { get; set; }
        public string ActionFlag { get; set; }

        #endregion

        public string CreateCoorditor(CordinatorManagement cmng)
        {
            string cordinatorNo = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Admin_Insert_Coordinator", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_CoordinatorId", cmng.CoordinatorId);
                cmd.Parameters.AddWithValue("@P_CoordinatorName", cmng.CoordinatorName);
                cmd.Parameters.AddWithValue("@P_SSOID", cmng.SSOID);
                cmd.Parameters.AddWithValue("@P_Address", cmng.Address);
                cmd.Parameters.AddWithValue("@P_DistrictCode", cmng.DistrictCode);
                cmd.Parameters.AddWithValue("@P_Pincode", cmng.Pincode);
                cmd.Parameters.AddWithValue("@P_EnteredBy", cmng.EnteredBy);
                cmd.Parameters.AddWithValue("@P_ActionFlag", cmng.ActionFlag);
                  cordinatorNo = cmd.ExecuteScalar().ToString();
       

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "CreateCoorditor" + "_" + "Citizen", 1, DateTime.Now, 0);
              
            }
            finally
            {
                Conn.Close();
            }
            return cordinatorNo;

        }

        public DataTable BindCordinator(Int64 userId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Admin_Select_CoordinatorDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_UserID", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
             
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindCordinator" + "_" + "Citizen", 1, DateTime.Now, 0);
                //throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable getCoordinatorVal(Int64 coordinatorID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Admin_Select_CoordinatorByID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_coordinatorID", coordinatorID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "getCoordinatorVal" + "_" + "Citizen", 1, DateTime.Now, 0);
            
               
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindCoordinatorName()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
         
                SqlCommand cmd = new SqlCommand("Sp_Select_Citizen_Coordinator", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindCoordinatorName" + "_" + "Citizen", 1, DateTime.Now, 0);
            
                
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable IUCNcategory()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_IUCNCategory", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetIUCN");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "sp_IUCNcategory" + "_" + "Citizen", 1, DateTime.Now, 0);


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetWildlifeSchedule()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_IUCNcategory", Conn);
                cmd.Parameters.AddWithValue("@Action", "WildlifeSchedule");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "sp_IUCNcategory" + "_" + "Citizen", 1, DateTime.Now, 0);


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
    }
}