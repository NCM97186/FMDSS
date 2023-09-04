using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class UseRoleADAward : DAL
    {
        public int RoleId { get; set; }
        public int StatusID { get; set; }
        public int ID { get; set; }
        public int Index { get; set; }
        public string RoleName { get; set; }
        
        public string Desc { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public string OperationType { get; set; }
        public List<UnMappedUserDetails> UnMappedUserLIST { get; set; }
        public List<MappedUserDetails> MappedUserLIST { get; set; }
        
        public DataTable Select_UseRoles()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_AD_AssignUserRoles", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectDDLlists");
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

        public DataTable Select_Status()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_AD_AssignUserRoles", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectDDLlistsStatus");
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

        public DataSet GetMapUnmapROLEwithUser(UseRoleADAward obj)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_AD_AssignUserRoles", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetMapUnmapUseBaseOfADRoles");
                cmd.Parameters.AddWithValue("@ApprovalLevelID", obj.ID);
                cmd.Parameters.AddWithValue("@UserStatusID", obj.StatusID);

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


        public string MappingForROLEwithUserADAward(Int64 USERIDs,string ApprovalLevelID, string UserStatusID, bool STATUS, Int64 LOGINSSOID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_AD_AssignUserRoles", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "MappingUserWithADRoles");
                cmd.Parameters.AddWithValue("@USERID", USERIDs);
                cmd.Parameters.AddWithValue("@ApprovalLevelID", ApprovalLevelID);
                cmd.Parameters.AddWithValue("@UserStatusID", UserStatusID);
                cmd.Parameters.AddWithValue("@ISACTIVE", STATUS);
                cmd.Parameters.AddWithValue("@LOGINSSOID", LOGINSSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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


        public string VaildationsMappingUserWithRolesADAward(Int64 USERIDs, int RoleIds, bool STATUS, Int64 LOGINSSOID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_MappingUserWithRoles", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "VaildationsMappingUserWithRoles");
                cmd.Parameters.AddWithValue("@USERID", USERIDs);
                cmd.Parameters.AddWithValue("@ROLEID", RoleIds);
                cmd.Parameters.AddWithValue("@ISACTIVE", STATUS);
                cmd.Parameters.AddWithValue("@LOGINSSOID", LOGINSSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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


        public DataTable Select_VehicleEquipment(int RoleId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UseRoles", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneUseRole");
                cmd.Parameters.AddWithValue("@RoleId", RoleId);
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

        public DataTable AddUpdateUseRole(UserRole oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateVehicleEquipment");
                cmd.Parameters.AddWithValue("@CategoryID", oPlace.RoleId);
                cmd.Parameters.AddWithValue("@CategoryName", oPlace.RoleName);
                cmd.Parameters.AddWithValue("@Desc", oPlace.Desc);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.Isactive);

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