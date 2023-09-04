using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FMDSS.Models
{
    public class UserProfile : DAL
    {
        public bool IsKioskUser { get; set; }
        public bool IsAgency { get; set; }
        public string SSOId { get; set; }
        public string FullName { get; set; }
        public string AadharId { get; set; }
        public string BhamashahId { get; set; }
        public string DatOFBirth { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailId { get; set; }
        public string Address1 { get; set; }
        public string PINCode1 { get; set; }
        public string District1 { get; set; }
        public string PhotURL { get; set; }
        public string Roles { get; set; }
        public string Designation { get; set; }
        public string Address2 { get; set; }
        public string PINCode2 { get; set; }
        public string District2 { get; set; }
        public string City2 { get; set; }
        public string AgencyName { get; set; }
        public string AgencyDistrict { get; set; }
        public string AgencyCity { get; set; }
        public string AgencyAddress { get; set; }
        public string AgencySPOC { get; set; }
        public string AgencyContact { get; set; }

        public int UserId { get; set; }

        public DataTable InsertUpdateUserInfo(string option)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_ADD_UPDATE_USERINFO", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsAgency", IsAgency);
                cmd.Parameters.AddWithValue("@IsKioskUser", IsKioskUser);
                cmd.Parameters.AddWithValue("@SSOId", SSOId);
                cmd.Parameters.AddWithValue("@FullName", FullName);
                cmd.Parameters.AddWithValue("@AadharId", AadharId);
                cmd.Parameters.AddWithValue("@BhamashahId", BhamashahId);
                cmd.Parameters.AddWithValue("@DatOFBirth", DatOFBirth);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@MobileNumber", MobileNumber);
                cmd.Parameters.AddWithValue("@TelephoneNumber", TelephoneNumber);
                cmd.Parameters.AddWithValue("@MailId", EmailId);
                cmd.Parameters.AddWithValue("@Address1", Address1);
                cmd.Parameters.AddWithValue("@PINCode1", PINCode1);
                cmd.Parameters.AddWithValue("@District1", District1);
                cmd.Parameters.AddWithValue("@PhotURL", PhotURL);
                cmd.Parameters.AddWithValue("@Roles", Roles);
                cmd.Parameters.AddWithValue("@Designation", Designation);
                cmd.Parameters.AddWithValue("@Address2", Address2);
                cmd.Parameters.AddWithValue("@PINCode2", PINCode2);
                cmd.Parameters.AddWithValue("@District2", District2);
                cmd.Parameters.AddWithValue("@City2", City2);
                cmd.Parameters.AddWithValue("@AgencyName", AgencyName);
                cmd.Parameters.AddWithValue("@AgencyDistrict", AgencyDistrict);
                cmd.Parameters.AddWithValue("@AgencyCity", AgencyCity);
                cmd.Parameters.AddWithValue("@AgencyAddress", AgencyAddress);
                cmd.Parameters.AddWithValue("@AgencySPOC", AgencySPOC);
                cmd.Parameters.AddWithValue("@AgencyContact", AgencyContact);
                cmd.Parameters.AddWithValue("@option", option);
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

        public UserProfile AuthenticateUser(string emailId)
        {
            try
            {
                UserProfile user = null;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GetAuthenticateUser", Conn);
                cmd.Parameters.AddWithValue("@emailId", emailId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    user = new UserProfile()
                    {
                        SSOId = "sso3",
                        FullName = FullName,
                        EmailId = emailId,
                        MobileNumber = MobileNumber,
                        Designation = Designation,
                        Address1 = Address1,
                        PINCode1 = PINCode1,
                        District1 = District1,
                        Roles = Roles,
                        IsKioskUser = false,
                        IsAgency = false,
                    };
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        user = new UserProfile()
                        {
                            SSOId = dr["Ssoid"].ToString(),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            FullName = dr["Name"].ToString(),
                            EmailId = dr["EmailId"].ToString(),
                            MobileNumber = dr["Mobile"].ToString(),
                            Designation = dr["Designation"].ToString(),
                            Address1 = dr["Postal_address1"].ToString(),
                            PINCode1 = dr["Postal_code1"].ToString(),
                            District1 = dr["District1"].ToString(),
                            Roles = dr["RoleId"].ToString(),
                            IsKioskUser = Convert.ToBoolean(dr["IsKioskUser"]),
                            IsAgency = Convert.ToBoolean(dr["IsAgency"]),
                        };
                    }
                }
                return user;
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