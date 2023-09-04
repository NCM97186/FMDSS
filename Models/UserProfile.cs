//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : User Profile Model
//  Description  : File contains functions For Business Rules and DB for User Details Update
//  Date Created : 24-Dec-2015
//  History      : 
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FMDSS.Models
{
    [Serializable]
    public class UserProfile : DAL
    {
        #region Data Members
        public bool IsKioskUser { get; set; }
        public bool IsDepartmentalKioskUser { get; set; }
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
        public long UserId { get; set; }
        public bool IsSSO { get; set; }
        public bool IsBhamashah { get; set; }
        public string KioskSSOId { get; set; }
        public bool isRedirected { get; set; }
        //public bool loggedin { get; set; }
        //public int DesignationId { get; set; }
        //public string DesignationDesc { get; set; }
        //public string AadharID { get; set; }
        //public string UserCDR { get; set; }
        //public string DESIG_NAME { get; set; }
        //public string OfficeName { get; set; }
        //public string DepartmentName { get; set; }
        #endregion

        [Serializable]
        public class eMitraObject
        {
            public string SSOID { get; set; }
            public string SERVICEID { get; set; }
            public string EMSESSIONID { get; set; }
            public string KIOSKCODE { get; set; }
            public string KIOSKNAME { get; set; }
            public string ENTITYTYPE { get; set; }
            public string DISTRICT { get; set; }
            public string DISTRICTCD { get; set; }
            public string TEHSIL { get; set; }
            public string TEHSILCD { get; set; }
            public string VILLAGE { get; set; }
            public string VILLAGECD { get; set; }
            public string WARD { get; set; }
            public string WARDCD { get; set; }
            public string PINCODE { get; set; }
            public string MOBILE { get; set; }
            public string EMAIL { get; set; }
            public string LSPNAME { get; set; }
            public string PARAMETER1 { get; set; }
            public string PARAMETER2 { get; set; }
            public string PARAMETER3 { get; set; }
            public string PARAMETER4 { get; set; }
            public string PARAMETER5 { get; set; }
            public string RETURNURL { get; set; }
            public string EMITRATIMESTAMP { get; set; }
            public string SSOTOKEN { get; set; }
            public string CHECKSUM { get; set; }
        }


        #region Member Functions



        /// <summary>
        /// function responsible for add/edit Logged In User Details
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataTable FmdsServiceDetails(string ServiceId = "0")
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_GetServiceDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceId", Convert.ToInt32(ServiceId));
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

        public DataSet FmdssEmitraServiceDetails(string ServiceId = "0", string ssoID="")
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Common_GetEmitraServiceDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceId", ServiceId);
                cmd.Parameters.AddWithValue("@SSOID", ssoID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
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



        public bool BypassOTPForKioskAsCounterUser( Int64 userid )
        {
            try
            {

                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_BypassOTPForKioskAsCounterUser", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

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

        public DataTable BypassOTPForKioskAsCounterUserForList(Int64 userid)
        {
            DataTable dt = new DataTable();
            try
            {

                DALConn();
                
                SqlCommand cmd = new SqlCommand("Sp_BypassOTPForKioskAsCounterUser", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userid);
                cmd.CommandType = CommandType.StoredProcedure;
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


        /// <summary>
        /// function responsible for add/edit Logged In User Details
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataTable ValidateFMDSSUser(string fmdssUserId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_VALIDATE_FMDSSUSER", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FMDDSID", fmdssUserId);
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

        /// <summary>
        /// function responsible for add/edit Logged In User Details
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet InsertUpdateUserInfo()
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
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
                cmd.Parameters.AddWithValue("@Designation", (Designation=="0" || Designation==null || Designation==""?"10": Designation));
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
                cmd.Parameters.AddWithValue("@IsSSO", IsSSO);
                cmd.Parameters.AddWithValue("@IsBhamashah", IsBhamashah);
                                
               // cmd.Parameters.AddWithValue("@KioskId", KioskSSOId);
                cmd.Parameters.AddWithValue("@KioskId", DBNull.Value); //  passing null to block departmental registration of citizens
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
        #region new code added by mukesh sir 6/2/2022
        public void FatchedUserInfo_FMDSS2_0_Insert_Update(UserProfile userprofile)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FatchedUserInfo_FMDSS_2_0", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userprofile.UserId);
                cmd.Parameters.AddWithValue("@IsAgency", userprofile.IsAgency);
                cmd.Parameters.AddWithValue("@IsKioskUser", userprofile.IsKioskUser);
                cmd.Parameters.AddWithValue("@SSOId", userprofile.SSOId);
                cmd.Parameters.AddWithValue("@FullName", userprofile.FullName);
                cmd.Parameters.AddWithValue("@AadharId", userprofile.AadharId);
                cmd.Parameters.AddWithValue("@BhamashahId", userprofile.BhamashahId);
                cmd.Parameters.AddWithValue("@DatOFBirth", userprofile.DatOFBirth);
                cmd.Parameters.AddWithValue("@Gender", userprofile.Gender);
                cmd.Parameters.AddWithValue("@MobileNumber", userprofile.MobileNumber);
                cmd.Parameters.AddWithValue("@TelephoneNumber", userprofile.TelephoneNumber);
                cmd.Parameters.AddWithValue("@MailId", userprofile.EmailId);
                cmd.Parameters.AddWithValue("@Address1", userprofile.Address1);
                cmd.Parameters.AddWithValue("@PINCode1", userprofile.PINCode1);
                cmd.Parameters.AddWithValue("@District1", userprofile.District1);
                cmd.Parameters.AddWithValue("@PhotURL", userprofile.PhotURL);
                cmd.Parameters.AddWithValue("@Roles", userprofile.Roles);
                cmd.Parameters.AddWithValue("@Designation", userprofile.Designation);
                cmd.Parameters.AddWithValue("@Address2", userprofile.Address2);
                cmd.Parameters.AddWithValue("@PINCode2", userprofile.PINCode2);
                cmd.Parameters.AddWithValue("@District2", userprofile.District2);
                cmd.Parameters.AddWithValue("@City2", userprofile.City2);
                cmd.Parameters.AddWithValue("@AgencyName", userprofile.AgencyName);
                cmd.Parameters.AddWithValue("@AgencyDistrict", userprofile.AgencyDistrict);
                cmd.Parameters.AddWithValue("@AgencyCity", userprofile.AgencyCity);
                cmd.Parameters.AddWithValue("@AgencyAddress", userprofile.AgencyAddress);
                cmd.Parameters.AddWithValue("@AgencySPOC", userprofile.AgencySPOC);
                cmd.Parameters.AddWithValue("@AgencyContact", userprofile.AgencyContact);
                cmd.Parameters.AddWithValue("@IsSSO", userprofile.IsSSO);
                cmd.Parameters.AddWithValue("@IsBhamashah", userprofile.IsBhamashah);
                // cmd.Parameters.AddWithValue("@KioskId", KioskSSOId);
                cmd.Parameters.AddWithValue("@KioskId", DBNull.Value); //  passing null to block departmental registration of citizens
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException sqEx)
            {

                throw sqEx;
            }
            finally
            {
                Conn.Close();
            }
        }
        #endregion
        public DataTable UserProfileSessionMaintain(string Action, string SSOName, string SSOToken, string Ip_Address)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_UserProfileSessionMaintain", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@SSOName", SSOName);
                cmd.Parameters.AddWithValue("@SSOToken", SSOToken);
                cmd.Parameters.AddWithValue("@Ip_Address", Ip_Address);

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


        /// <summary>
        /// function responsible for authenticating the user on Staging/Production Environment
        /// </summary>
        /// <param name="ssoid"></param>
        /// <returns></returns>
        public DataTable AuthenticateUser(string ssoid)
        {
            try
            {
                DALConn();
                UserProfile user = null;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GetAuthenticateUser", Conn);
                cmd.Parameters.AddWithValue("@ssoid", ssoid);
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

        public DataTable GetMultipleOffice(string ssoid)
        {
            try
            {
                DALConn();
                UserProfile user = null;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_GetOfficeID", Conn);
                cmd.Parameters.AddWithValue("@ssoid", ssoid);
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
        public DataSet FindBlockedStatus(string ssoid = "", string IpAddress = "", string MobileNo = "", string EmailAddress = "")
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("findBlockedUser", Conn);
                cmd.Parameters.AddWithValue("@SSO_ID", ssoid);
                cmd.Parameters.AddWithValue("@IpAddress", IpAddress);
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("@EmailAddress", EmailAddress);
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
        /// <summary>
        /// function responsible for authenticating the user on Dev Environment
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public UserProfile AuthenticateUser_Local(string emailId)
        {
            try
            {
                DALConn();
               // string pass = 
                UserProfile user = null;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GetAuthenticateUser_local", Conn);
                cmd.Parameters.AddWithValue("@ssoid", emailId);
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
                        AadharId=AadharId
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
                            AadharId = Convert.ToString(dr["Aadhar_ID"]),
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

        public DataSet GetUserCDR(long UserId)//CDR=Circle or Division or Range according to designation
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@Action", "GetUserCDR");               
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
        #endregion

    }
     [Serializable]
    public class UserRolesMenusDetails
    {
        public List<ROLEGROUPS> ROLEGROUPS { get; set; }
        public List<Menus> Menus { get; set; }
    }
     [Serializable]
    public class ROLEGROUPS
    {
        public Int16 RoleId { get; set; }
        public string RoleName { get; set; }
        public Int16 RolePriority { get; set; }
        public string DefaultPage { get; set; }
        public string DefaultLayout { get; set; }
        
    }
    [Serializable]
    public class Menus
    {
        public Int64 UserID { get; set; }
        public string SSOID { get; set; }
        public Int16 RoleId { get; set; }
        public string RoleName { get; set; }
        public string PageTitle { get; set; }
        public string PageURL { get; set; }
        public string IsActive { get; set; }
        public string IconClass { get; set; }
        public string isIcon { get; set; }
        public string Layout { get; set; }
        public Int64 PageID { get; set; }
        public Int64 ParentID { get; set; }



        public bool IsNested { get; set; }
        public bool IsTargetBlank { get; set; }
        

    }
     [Serializable]
    public class MultiOffice
    {
        public string SSOID { get; set; }
        public string AnotherSSOId { get; set; }

        public string OfficeName { get; set; }
        public string RoleName { get; set; }
        
    }

}