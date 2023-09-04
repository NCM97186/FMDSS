//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : ManageAssetController
//  Description  : File contains calling functions from UI
//  Date Created : 24-Dec-2015
//  History      : 
//  Version      : 1.0
//  Author       : Ashok Yadav
//  Modified By  : Ashok Yadav
//  Modified On  : 12-Feb-2016
//  Reviewed By  : Amar swain
//  Reviewed On  : 12-Feb-2016
//********************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{

    public class CitizenDashboard : DAL
    {
        #region data Members
        public string TransactionID { get; set; }
        public string RequstedID { get; set; }
        public string RequestType { get; set; }
        public string Date { get; set; }
        public string StatusDesc { get; set; }
        public string Final_Amount { get; set; }

        public string Status { get; set; }
        public string ApprovedDate { get; set; }
        public string ApprovedStatus { get; set; }
        public string ApprovedID { get; set; }
        public string ReviewedDate { get; set; }
        public string ReviewedStatus { get; set; }
        public string ApplicationPDF { get; set; }
        public string ReviewedID { get; set; }
        public string RequestId { get; set; }
        public string TableName { get; set; }
        public string PermissionID { get; set; }
        public string ActionID { get; set; }
        public string PageURl { get; set; }
        public string PageName { get; set; }
        public string ModuleName { get; set; }
        public string UserName { get; set; }
        public int FavouritelinkID { get; set; }
        public string SubPermissionDesc { get; set; }
        public string District_Name { get; set; }
        public string EnteredOn { get; set; }
        public string SSOID { get; set; }
        public string Name { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string Designation { get; set; }
        public string Postal_Address { get; set; }
        public string Postal_Code { get; set; }
        public string DurationFrom { get; set; }
        public string DurationTo { get; set; }
        public string FInalAmount { get; set; }
        public string EmitraTransactionID { get; set; }
        public string ActionTakenBy { get; set; }
        public string ActionTakenOn { get; set; }
        public string ForestStatus { get; set; }

        #endregion

        #region Member Functions

        /// <summary>
        /// To get dashboard reuest details
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ActionID"></param>
        /// <returns></returns>
        public DataSet GetTransactionDashaboard(Int64 UserID, int ActionID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
               
                SqlCommand cmd = new SqlCommand("Sp_Citizen_DashBoard", Conn);
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.Parameters.AddWithValue("@ActionId", ActionID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetTransactionDashaboard" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        ///  To get dashboard request details for kiosk user
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ActionID"></param>
        /// <returns></returns>
        public DataSet GetTransactionDashaboardKiosk(Int64 UserID, int ActionID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Citizen_DashBoard_Kiosk", Conn);
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.Parameters.AddWithValue("@ActionId", ActionID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetTransactionDashaboard" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public List<Menus> GetKioskUserMenuForWildlife(Int64 UserID, string Action, string SSOID)
        {
            List<Menus> Menus=new List<Menus>();
            DataTable ds = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_GetKioskUserMenuForWildLife", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if(ds!=null && ds.Rows.Count>0)
                {
                    foreach (DataRow dr in ds.Rows)
                    {
                        Menus.Add(
                        new Menus()
                        {
                            UserID = Convert.ToInt64(dr["UserID"]),
                            SSOID = Convert.ToString(dr["SSOID"]),
                            RoleId = Convert.ToInt16(dr["RoleId"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            PageTitle = Convert.ToString(dr["PageTitle"]),
                            PageURL = Convert.ToString(dr["PageURL"]),
                            IsActive = Convert.ToString(dr["IsActive"]),
                            IconClass = Convert.ToString(dr["IconClass"]),
                            isIcon = Convert.ToString(dr["isIcon"]),
                            Layout = Convert.ToString(dr["Layout"]),
                            PageID = Convert.ToInt64(dr["PageID"]),
                            ParentID = Convert.ToInt64(dr["ParentID"]),
                            IsNested = Convert.ToBoolean(dr["IsNested"]),
                            IsTargetBlank = Convert.ToBoolean(dr["IsTargetBlank"]),
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Menus = new List<Menus>();
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetKioskUserMenuForWildlife" + "_" + "CitizenDashboard", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return Menus;
        }


        /// <summary>
        /// Use to retrive data for print
        /// </summary>
        /// <param name="RequestID"></param>
        /// <returns></returns>
        public DataSet GetPrintDetails(string RequestID)
        {
            DataSet ds = new DataSet();
            try
            {
              
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GetDetails_for_print", Conn);
                cmd.Parameters.AddWithValue("@RequestedID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
               
            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetPrintDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// use to bind all action for all service link
        /// </summary>
        /// <param name="reqid"></param>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public DataTable BindActionList(string reqid, string tblName)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                RequestId = reqid;
                TableName = tblName;
            
                SqlCommand cmd = new SqlCommand("Select_PendingActionResult_V1", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestID", RequestId);
                cmd.Parameters.AddWithValue("@TableName", TableName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindActionList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Get details based on requestid and table name
        /// </summary>
        /// <param name="reqid"></param>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public DataTable ReasonList(string CommmaSeperatedReason)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
         
                SqlCommand cmd = new SqlCommand("Sp_GetMultiReason", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Reason", CommmaSeperatedReason);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ReasonList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
        
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// To display Favourit list on dashboard in datatable
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="ServiceDesc"></param>
        /// <returns></returns>
        public DataTable FavouriteList(string ServiceId, string ServiceDesc)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
             
                SqlCommand cmd = new SqlCommand("Sp_InsertSelectFavServices", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceId", Convert.ToInt64(ServiceId));
                cmd.Parameters.AddWithValue("@ServiceDesc", ServiceDesc);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@Opt", "1");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "FavouriteList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// To display Favourit list on dashboard in datatable
        /// </summary>
        /// <returns></returns>
        public DataTable GetFavouriteList()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
              
                SqlCommand cmd = new SqlCommand("Sp_InsertSelectFavServices", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceId", 0);
                cmd.Parameters.AddWithValue("@ServiceDesc", "");
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@Opt", "2");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetFavouriteList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// To delete Favourit list on dashboard in datatable
        /// </summary>
        /// <param name="serid"></param>
        /// <returns></returns>
        public DataTable DelFavouriteList(string serid)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
              
                SqlCommand cmd = new SqlCommand("Sp_InsertSelectFavServices", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceId", Convert.ToInt64(serid));
                cmd.Parameters.AddWithValue("@ServiceDesc", "");
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@Opt", "3");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "DelFavouriteList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// To print application after approval
        /// </summary>
        /// <param name="RequestID"></param>
        /// <param name="Status"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DataSet GetPrintApplicationAfterApproval(string RequestID, string Status, string TableName)
        {

            DataSet ds = new DataSet();
            try
            {
                DALConn();
              
                SqlCommand cmd = new SqlCommand("[Sp_GetRequestDetailsForPrint]", Conn);
                cmd.Parameters.AddWithValue("@RequestedId", RequestID);
                //cmd.Parameters.AddWithValue("@Status",Convert.ToInt32(Status));
                cmd.Parameters.AddWithValue("@TableName", TableName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);



                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetPrintApplicationAfterApproval" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetRequestforreassignedPdf(string RequestID, string TableName)
        {

            DataSet ds = new DataSet();
            try
            {
                DALConn();
              
                SqlCommand cmd = new SqlCommand("[Sp_GetRequestforreassignedPdf]", Conn);
                cmd.Parameters.AddWithValue("@RequestedId", RequestID);
                //cmd.Parameters.AddWithValue("@Status",Convert.ToInt32(Status));
                cmd.Parameters.AddWithValue("@TableName", TableName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);



               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetRequestforreassignedPdf" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// To get lasest request
        /// </summary>
        /// <param name="serid"></param>
        /// <returns></returns>
        public DataSet Get_LatestRequest(string UserID)
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();
               
                SqlCommand cmd = new SqlCommand("sp_get_latestrequest", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(UserID));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Get_LatestRequest" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        #endregion

        #region PDF Information
        public DataSet PDFInformation(string Action,string RequestID,string TableName,string PDFwithoutSign,string PDFwithSign)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_insertGenratePDFWithSign", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@ModuleName", TableName);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.Parameters.AddWithValue("@PDFwithoutSign", PDFwithoutSign);
                cmd.Parameters.AddWithValue("@PDFwithSign", PDFwithSign);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "PDFInformation" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        #endregion
    }
}