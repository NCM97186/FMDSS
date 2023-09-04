using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Net;

namespace FMDSS.Models.Master
{
    public class UserProfileDetails : DAL
    {
        public int Index { get; set; }

        public long UserID { get; set; }
        public string Ssoid { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Postal_Address1 { get; set; }
        public string Postal_Code1 { get; set; }
        public string District1 { get; set; }
        public string Postal_Address2 { get; set; }
        public string Postal_Code2 { get; set; }
        public string District2 { get; set; }
        public string City2 { get; set; }
        public string Bhamashah_Id { get; set; }
        public string Aadhar_ID { get; set; }
        public string RoleId { get; set; }
        public int IsKioskUser { get; set; }
        public int IsAgency { get; set; }
        public string AgencyName { get; set; }
        public string AgencyDistrict { get; set; }
        public string AgencyCity { get; set; }
        public string AgencyAddress { get; set; }
        public string AgencySPOC { get; set; }
        public string AgencyContact { get; set; }
        public int Isactive { get; set; }
        public long UpdatedBy { get; set; }
        public int IsSSO { get; set; }
        public int IsBhamashah { get; set; }
        public string KioskId { get; set; }
        public int IsDepartmentalKioskUser { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime EnteredOn { get; set; }
        public bool IsactiveView { get; set; }
        public string OperationType { get; set; }

        public string Boundary { get; set; }
        public string OfficeName { get; set; }


        public DataTable Select_CitizenUserProfileDetailss()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_User_Profile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllCitizenUserProfile");
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

        public DataTable Select_CitizenUserProfileDetailssForFRA()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_User_Profile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllCitizenUserProfileForFRA");
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

        public DataTable Select_UserProfileDetailss()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_User_Profile", Conn);
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


        public DataTable Select_UserProfileDetails(string SsoID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_User_Profile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneUserProfileSSoWise");
                cmd.Parameters.AddWithValue("@Ssoid", SsoID);
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


        public DataTable Select_UserProfileDetails(int UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_User_Profile", Conn);
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

        public DataTable AddUpdateUserProfileDetails(UserProfileDetails oUserProfile)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_User_Profile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateUserProfile");
                cmd.Parameters.AddWithValue("@UserID", oUserProfile.UserID);
                cmd.Parameters.AddWithValue("@Ssoid", oUserProfile.Ssoid);
                cmd.Parameters.AddWithValue("@Name", oUserProfile.Name);
                cmd.Parameters.AddWithValue("@EmailId", oUserProfile.EmailId);
                cmd.Parameters.AddWithValue("@Mobile", oUserProfile.Mobile);
                cmd.Parameters.AddWithValue("@Designation", oUserProfile.Designation);
                cmd.Parameters.AddWithValue("@DOB", oUserProfile.DOB);
                cmd.Parameters.AddWithValue("@Gender", oUserProfile.Gender);
                cmd.Parameters.AddWithValue("@Postal_Address1", oUserProfile.Postal_Address1);
                cmd.Parameters.AddWithValue("@Postal_Code1", oUserProfile.Postal_Code1);
                cmd.Parameters.AddWithValue("@IsKioskUser", oUserProfile.IsKioskUser);
                cmd.Parameters.AddWithValue("@Isactive", oUserProfile.Isactive);
                cmd.Parameters.AddWithValue("@IsSSO", oUserProfile.IsSSO);
                cmd.Parameters.AddWithValue("@IsBhamashah", oUserProfile.IsBhamashah);

                cmd.Parameters.AddWithValue("@Bhamashah_Id", oUserProfile.Bhamashah_Id);
                cmd.Parameters.AddWithValue("@Aadhar_ID", oUserProfile.Aadhar_ID);

                cmd.Parameters.AddWithValue("@IsDepartmentalKioskUser", oUserProfile.IsDepartmentalKioskUser);
                cmd.Parameters.AddWithValue("@UpdatedBy", oUserProfile.UpdatedBy);

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

        public DataTable DesignationName()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_User_Profile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectallDesignation");

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

        public bool Check_DuplicateRecord()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_User_Profile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicateUser");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Ssoid", Ssoid);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                    return false;
                else
                    return true;
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
        public void UpdateMasterRecordStatus(Int64 ID, bool STATUS, string MasterType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UpdateMastersRecordStatus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tablename", MasterType);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@STATUS", STATUS);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

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


        public DataTable AddNewOffice(UserProfileDetails obj)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_AddNewOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Boundary", obj.Boundary);
                cmd.Parameters.AddWithValue("@OfficeName", obj.OfficeName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }

        internal DataTable RemoveDepartmentRole(string id)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("spRemoveDepartmentRole", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "UpdateUserRole");
                cmd.Parameters.AddWithValue("@UserID", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }

        internal DataTable GetUser(string id)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("spRemoveDepartmentRole", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "GetUser");
                cmd.Parameters.AddWithValue("@UserID", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }

    }


}