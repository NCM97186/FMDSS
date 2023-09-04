using FMDSS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{

    public class SubPermission
    {
        public int SubPermissionId { get; set; }
        public string SubPermissionDesc { get; set; }
    }

    public class OnlineUser
    {
        public int Index { get; set; }
        public string StartTime { get; set; }
        public string ENDTIME { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string EmailId { get; set; }
        public string IPAddress { get; set; }

        public string OfficeBoundary { get; set; }
        public string OfficeName { get; set; }
    }
    //Edit by Sunny
    public class GetMySchedularEmail : DAL
    {

       



        public string EmailContent { get; set; }
        public List<GetMySchedularEmail> GetEmailStatusContent()
        {
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();
            DataTable dt = new DataTable();
            List<GetMySchedularEmail> oObj = new List<GetMySchedularEmail>();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SPGETDATAFOREventDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 3);
                cmd.Parameters.AddWithValue("@EnteredBy", Convert.ToString(HttpContext.Current.Session["UserID"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetMySchedularEmail Content = new GetMySchedularEmail();
                    Content.EmailContent = Convert.ToString(dt.Rows[i]["EmailTemplate"]);
                    oObj.Add(Content);
                }
                return oObj;


            }
            catch (Exception ex)
            {
            }
            finally
            {
                Conn.Close();
            }
            return oObj;
        }
    }

    public class ForestBoundares
    {
        public string BoundaryID { get; set; }
        public string BoundaryName { get; set; }
    }

    public class ForestOFFICES
    {
        public string OFFICE_ID { get; set; }
        public string OFFICENAME { get; set; }

    }

    public class ForestEmpDetails
    {
        public string SSO_ID { get; set; }
        public string EMPNAME { get; set; }
        public int UserID { get; set; }
        public int DesignationId { get; set; }
        public string EMPDESIGNATION { get; set; }

        public bool IsReviewer { get; set; }
        public bool IsApprover { get; set; }
    }



    public class SuperAdminOperations : DAL
    {
        public DataTable InsertOnlineUser(string ModuleName, string IPAddress, string SSOID, string ActivityName)
        {
            // spOnlineUser

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spOnlineUser", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "InsertNewUser");
                cmd.Parameters.AddWithValue("@ModuleName", ModuleName);
                cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
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

        public List<OnlineUser> GetOnlineUser()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spOnlineUser", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETONLINEUSER");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                List<OnlineUser> oUser = new List<OnlineUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    OnlineUser oU = new OnlineUser();
                    oU.Index = i + 1;
                    oU.StartTime = Convert.ToString(dt.Rows[i]["StartTime"]);
                    oU.ENDTIME = Convert.ToString(dt.Rows[i]["ENDTIME"]);
                    oU.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    oU.Mobile = Convert.ToString(dt.Rows[i]["Mobile"]);
                    oU.EmailId = Convert.ToString(dt.Rows[i]["EmailId"]);
                    oU.IPAddress = Convert.ToString(dt.Rows[i]["IPAddress"]);
                    oU.OfficeBoundary = Convert.ToString(dt.Rows[i]["OfficeBoundary"]);

                    oU.OfficeName = Convert.ToString(dt.Rows[i]["OfficeName"]);
                    oUser.Add(oU);
                }



                return oUser;
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


        public List<ForestBoundares> GetForestBoundaryes(string OfficeLevel)
        {
            DataTable dt = new DataTable();
            List<ForestBoundares> objList = new List<ForestBoundares>();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spReviewerApprover", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@officeLevel", OfficeLevel);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ForestBoundares obj = new ForestBoundares();
                    obj.BoundaryID = Convert.ToString(dt.Rows[i]["BoundaryID"]);
                    obj.BoundaryName = Convert.ToString(dt.Rows[i]["BoundaryName"]);
                    objList.Add(obj);
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetForestBoundaries" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return objList;

        }

        public List<SubPermission> GetSubPermission(int PermissionId)
        {
            DataTable dt = new DataTable();
            List<SubPermission> oObj = new List<SubPermission>();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spReviewerApprover", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PermissionId", PermissionId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubPermission Permission = new SubPermission();
                    Permission.SubPermissionId = Convert.ToInt16(dt.Rows[i]["SubPermissionId"]);
                    Permission.SubPermissionDesc = Convert.ToString(dt.Rows[i]["SubPermissionDesc"]);
                    oObj.Add(Permission);
                }
                return oObj;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetSuperAdmin" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return oObj;
        }

        public List<ForestOFFICES> GetForestOFFICES(string Div_Code)
        {
            DataTable ds = new DataTable();
            List<ForestOFFICES> objList = new List<ForestOFFICES>();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spReviewerApprover", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Div_Code", Div_Code);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    ForestOFFICES obj = new ForestOFFICES();
                    obj.OFFICE_ID = Convert.ToString(ds.Rows[i]["OFFICE_ID"]);
                    obj.OFFICENAME = Convert.ToString(ds.Rows[i]["OFFICENAME"]);
                    objList.Add(obj);
                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "getForestOffices" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally { Conn.Close(); }
            return objList;
        }

        public List<ForestEmpDetails> GetForestEmployees(int RowId, int SubPermissionId, string OfficeCode)
        {
            DataTable ds = new DataTable();
            List<ForestEmpDetails> objList = new List<ForestEmpDetails>();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spGetEmp", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "EmpDetails");
                cmd.Parameters.AddWithValue("@RowId", RowId);
                cmd.Parameters.AddWithValue("@SubPermissionId", SubPermissionId);
                cmd.Parameters.AddWithValue("@OfficeCode", OfficeCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    ForestEmpDetails obj = new ForestEmpDetails();
                    obj.SSO_ID = Convert.ToString(ds.Rows[i]["SSO_ID"]);
                    obj.DesignationId = Convert.ToInt16(ds.Rows[i]["Designation"]);
                    obj.EMPDESIGNATION = Convert.ToString(ds.Rows[i]["EMPDESIGNATION"]);
                    obj.UserID = Convert.ToInt16(ds.Rows[i]["UserID"]);
                    obj.EMPNAME = Convert.ToString(ds.Rows[i]["EMPNAME"]);
                    obj.IsReviewer = Convert.ToBoolean(ds.Rows[i]["IsReviewer"]);
                    obj.IsApprover = Convert.ToBoolean(ds.Rows[i]["IsApprover"]);
                    //obj.OFFICENAME = Convert.ToString(ds.Rows[i]["OFFICENAME"]);
                    objList.Add(obj);
                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "getForestOffices" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally { Conn.Close(); }
            return objList;
        }

        public DataSet ExecuteQuery(string Qry)
        {

            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_common_ExecuteQuery", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Query", Qry);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveUserApproval" + "_" + "Admin", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public Int64 SaveUserApprovalDetails(UserApproval uA)
        {
            Int64 chk = 0;
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spSAVEReviewerApprover", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveReviewApprove");
                cmd.Parameters.AddWithValue("@RowId", uA.PermissionId);
                // cmd.Parameters.AddWithValue("@PermissionId", uA.PermissionId);
                cmd.Parameters.AddWithValue("@SubPermissionId", uA.SubPermissionId);
                cmd.Parameters.AddWithValue("@UserId", uA.UserId);
                cmd.Parameters.AddWithValue("@RoleId", uA.RoleId);
                cmd.Parameters.AddWithValue("@designation", uA.Designation);
                cmd.Parameters.AddWithValue("@Office", uA.Office);
                cmd.Parameters.AddWithValue("@department", uA.Department);
                cmd.Parameters.AddWithValue("@Jurisdiction", uA.Jurisdiction);
                cmd.Parameters.AddWithValue("@PecuniaryLimit", uA.PecuniaryLimit);
                cmd.Parameters.AddWithValue("@IsReviewer", uA.IsReviewer);
                cmd.Parameters.AddWithValue("@IsApprover", uA.IsApprover);
                cmd.Parameters.AddWithValue("@UpdatedBy", Convert.ToString(HttpContext.Current.Session["User"]));//uA.UpdatedBy);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(ds);
                chk = Convert.ToInt64(cmd.ExecuteScalar());
                //SMS_EMail_Services oEmail = new SMS_EMail_Services();
                //string 
                //if(uA.IsReviewer)
                //string sBody="Reviewer";
                //oEmail.sendEMail("Reviewer/Approver Status",)

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveUserApproval" + "_" + "Admin", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
    }
}