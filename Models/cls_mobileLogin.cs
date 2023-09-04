using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{


    public class LoginDetail
    {
        public string Status { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string Designation { get; set; }
        public string DesignationCode { get; set; }
        public string Gender { get; set; }
        public string DesignationID { get; set; }
        public string UserID { get; set; }
        public string DIV_CODE { get; set; }
        public string RoleId { get; set; }
        
    }

    public class OffenceSummery
    {
        public int Pending { get; set; }
        public int CaseInCourt { get; set; }
        public int Closed { get; set; }
        public int ClosedAtDepartment { get; set; }
        public int TotalCases { get; set; }
    }

    public class TotCountCircle
    {
        public int TotCountCircles { get; set; }
    }

    public class TotCountDivision
    {
        public int TotCountDivisions { get; set; }
    }

    public class TotCountRange
    {
        public int TotCountRanges { get; set; }
    }

    public class TotCountNaka
    {
        public int TotCountNakas { get; set; }
    }

    public class NurseryStockCount
    {
        //public int TotalSells { get; set; }
        //public int TotalBal { get; set; }
        public int TotCitizen { get; set; }
        public int TotalDeptQty { get; set; }
        public int Citizen_RemainingQty { get; set; }
        public int Dept_RemainingQty { get; set; }
        public int Citizen_StockOut { get; set; }
        public int Dept_StockOut { get; set; }
    }

    public class WaterResourecCount
    {
        public int TotalSourceCount { get; set; }
        public int TotalDestCount { get; set; }
    }   
    public class FireAlertCount
    {
        public int TotalFireAlerts { get; set; }
        public int TotalAlertUpdated { get; set; }
        public int TotalPendingAlerts { get; set; }
    }

    public class RescueList
    {
         
        public string RescueDate { get; set; }
        public string AnimalName { get; set; }
        public string DIST_NAME { get; set; }
        public string VILL_NAME { get; set; } 
        public bool isActive { get; set; }  
        public string PostMortemReportPath { get; set; }
        public string FactualReportPath { get; set; }
        public string Remarks { get; set; }
        public string StatusDesc { get; set; }
        public string Name { get; set; }
        
    }


    public class ModuleList
    {

        
        public long id { get; set; }
        public string ModuleName { get; set; }
        public string RoleName { get; set; }
        

    }

    public class RootObject
    {
        public int Status { get; set; }
        public string Ssoid { get; set; }
        public string Message { get; set; }
        public List<LoginDetail> loginDetail { get; set; }
        public List<OffenceSummery> OffenceSummery { get; set; }
        public List<TotCountCircle> TotCountCircles { get; set; }
        public List<TotCountDivision> TotCountDivisions { get; set; }
        public List<TotCountRange> TotCountRanges { get; set; }
        public List<TotCountNaka> TotCountNakas { get; set; }
        public List<NurseryStockCount> NurseryStockCount { get; set; }
        public List<WaterResourecCount> WaterResourecCount { get; set; }
        public List<FireAlertCount> FireAlertCount { get; set; }
        public List<RescueList> RescueList { get; set; }
        public List<ModuleList> ModuleList { get; set; }
        public List<UserPlaceDetail> userPlaceDetails { get; set; }
        public List<FBBookingType> fB_bookingTypeList { get; set; }
    }




    public class cls_UserOTPResponceCode
    {
        public string Mobile = string.Empty;
        public string Email = string.Empty;
        public string OTP = string.Empty;
        public string Status = string.Empty;
        public string UserSmsBody = string.Empty;
    }
    public class cls_LoginResponceCode
    {
        public bool status;
    }
    public class UserPlaceDetail
    {
        public Int64 UserID { get; set; }
        public int PlaceID { get; set; }
        public string PlaceName { get; set; }
        public int Office_ID { get; set; }
        public string OfficeName { get; set; }
        public string DIV_CODE { get; set; }
        public string PlaceType { get; set; }
    }
    public class FBBookingType
    {       
        public string SSOID { get; set; }
        public int PlaceTypeId { get; set; }
        public string PlaceType { get; set; }
    }
    public class cls_mobileLogin:DAL
    {
        public DataTable GetOTP(string SsoId)
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();               
                SqlCommand cmd = new SqlCommand("spMobileLogin", Conn);
                cmd.Parameters.AddWithValue("@Action", "GETOTP"); 
                cmd.Parameters.AddWithValue("@SSOID", SsoId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ValidateMobileUser" + "_" + "DepartmentMobileUser", 1, DateTime.Now,1);

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        internal DataTable CheckOTP(string ssoId, string oTP)
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spMobileLogin", Conn);
                cmd.Parameters.AddWithValue("@Action", "CheckOTP");
                cmd.Parameters.AddWithValue("@SSOID", ssoId);
                cmd.Parameters.AddWithValue("@OTP", oTP);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ValidateMobileUser" + "_" + "DepartmentMobileUser", 1, DateTime.Now, 1);

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        //public DataTable LoginMobileUser(string SsoId)
        public DataSet LoginMobileUserafterotp(string SsoId)
        {
            //DataTable ds = new DataTable();            
            DataSet ds = new DataSet();
            
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spMobileLogin", Conn);
                cmd.Parameters.AddWithValue("@Action", "Login");
                cmd.Parameters.AddWithValue("@SSOID", SsoId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                //var sss = ds.Tables.Count;
                //ds.Tables[0].TableName = "LoginDetail";
                //ds.Tables[1].TableName = "OffenceSummery";
                //ds.Tables[2].TableName = "TotCountCircle";
                //ds.Tables[3].TableName = "TotCountDivision";
                //ds.Tables[4].TableName = "TotCountRange";
                //ds.Tables[5].TableName = "TotCountNaka";
                //ds.Tables[6].TableName = "NurseryStockCount";
                //ds.Tables[7].TableName = "WaterResourecCount";
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ValidateMobileUser" + "_" + "DepartmentMobileUser", 1, DateTime.Now, 1);

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        //public DataSet LoginMobileUser(string SsoId)
        //{
        //    //DataTable ds = new DataTable();            
        //    DataSet ds = new DataSet();

        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("sp_mobilelogin_details", Conn); 
        //        cmd.Parameters.AddWithValue("@SSOID", SsoId);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        var sss = ds.Tables.Count;
        //        ds.Tables[0].TableName = "LoginDetail";
        //        ds.Tables[1].TableName = "RoleWiseModule";
        //        //ds.Tables[2].TableName = "TotCountCircle";
        //        //ds.Tables[3].TableName = "TotCountDivision";
        //        //ds.Tables[4].TableName = "TotCountRange";
        //        //ds.Tables[5].TableName = "TotCountNaka";
        //        //ds.Tables[6].TableName = "NurseryStockCount";
        //        //ds.Tables[7].TableName = "WaterResourecCount";
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ValidateMobileUser" + "_" + "DepartmentMobileUser", 1, DateTime.Now, 1);

        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //    return ds;
        //}

        public DataSet LoginMobileUser(string SsoId)
        {
            //DataTable ds = new DataTable();            
            DataSet ds = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spMobileLogin", Conn);
                cmd.Parameters.AddWithValue("@Action", "Login");
                cmd.Parameters.AddWithValue("@SSOID", SsoId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                ds.Tables[0].TableName = "LoginDetail";
                ds.Tables[1].TableName = "OffenceSummery";
                ds.Tables[2].TableName = "TotCountCircle";
                ds.Tables[3].TableName = "TotCountDivision";
                ds.Tables[4].TableName = "TotCountRange";
                ds.Tables[5].TableName = "TotCountNaka";
                ds.Tables[6].TableName = "NurseryStockCount";
                ds.Tables[7].TableName = "WaterResourecCount";
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ValidateMobileUser" + "_" + "DepartmentMobileUser", 1, DateTime.Now, 1);

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataSet GetZooPlaceDetails(string SsoId)
        {
            DataSet dt = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GetZooPlaceDetails", Conn); 
                cmd.Parameters.AddWithValue("@SSOID", SsoId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ValidateMobileUser" + "_" + "DepartmentMobileUser", 1, DateTime.Now, 1);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable GetUseridbySsoid(string SsoId)
        {
            DataTable dt = new DataTable();
            try
            {
               
                DALConn();
                SqlCommand cmd = new SqlCommand("GetUseridbySsoid", Conn); 
                cmd.Parameters.AddWithValue("@SSOID", SsoId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ValidateMobileUser" + "_" + "DepartmentMobileUser", 1, DateTime.Now, 1);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataSet getRescueList(string SsoId)
        {
            DataSet dt = new DataSet();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("getRescueList", Conn);
                cmd.Parameters.AddWithValue("@Userid", SsoId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ValidateMobileUser" + "_" + "DepartmentMobileUser", 1, DateTime.Now, 1);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataTable getssoid(string MobileNo)
        {
            DataTable dt = new DataTable();
            try
            {

               
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_getSSoidbymobile", Conn);
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "getssoid" + "_" + "ChatbotApi", 1, DateTime.Now, 1);

            }
            finally
            {
                Conn.Close();
            }
            return dt;


        }

    }
}