//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : WorkOrderInvoiceController
//  Description  : File contains calling functions from UI
//  Date Created : 13-06-2016
//  History      :
//  Version      : 1.0
//  Author       : Ashok Yadav
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************



using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment
{
    public class SurveyReport : DAL
    {
        #region Global Variable
        public string UserName { get; set; }
        public string Designation { get; set; }
        public Int64 UserID { get; set; }
        public Int64 SurveyID { get; set; }
        public string RangeCode { get; set; }
        public string VillageCode { get; set; }
        public string Village { get; set; }
        public Int64 MicroPlanID { get; set; }
        public string MicroPlan { get; set; }
        public Int64 WorkOrderID { get; set; }
        public string WorkOrder { get; set; }
        public Int64 SchemeID { get; set; }
        public string Scheme { get; set; }
        public Int64 ProjectID { get; set; }
        public string Project { get; set; }
        public Int64 ActivityID { get; set; }
        public string Activity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SurveyDate { get; set; }
        public string SDate { get; set; }
        public string PhotoURL { get; set; }
        public string FileURL { get; set; }
        public Int64 FileTypeID { get; set; }
        public int ActivityPercentage { get; set; }
        public string ShapeFileURL { get; set; }
        public string ComplitionYear { get; set; }
        public string Description { get; set; }
        public string AreaName { get; set; }
        public string Area { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string SurveyorName { get; set; }
        public Int64 EnteredBy { get; set; }
        public Int64 UpdatedBy { get; set; }

        public int Quantity { get; set; }

        public Int64 SubActivty { get; set; }
        public string  SubActivtyName { get; set; }
        public string Unit { get; set; }
        #endregion

        #region Functions

 
        public DataTable Select_SurveyReports()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
               
                SqlParameter[] parameters = { new SqlParameter("@UserID", UserID) };
                Fill(dt, "select_Survey_Details",parameters);
             
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_SurveyReports" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable Select_FileTypes()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
               
                Fill(dt, "sp_Common_selectTypes");
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_FileTypes" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Use to bind range
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Range()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
               
                SqlParameter[] parameters = { new SqlParameter("@UserID", UserID) };
                Fill(dt, "Select_Range_For_LoginUser", parameters);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Range" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Use to bind Village
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Villages()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters = { new SqlParameter("@RangeCode", RangeCode) };
                Fill(dt, "Select_Village_For_LoginUser", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Villages" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Use to bind micro plan
        /// </summary>
        /// <returns></returns>
        public DataTable Select_MicroPlan_By_Village()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
         
                SqlParameter[] parameters = { new SqlParameter("@Village_Code", VillageCode) };
                Fill(dt, "SP_FDM_Select_MicroPlanByVilageCode", parameters);
           
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_MicroPlan_By_Village" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }  
        /// <summary>
        /// Use to get micro plan by villag
        /// </summary>
        /// <returns></returns>
        public DataTable Select_WorkOrder_By_Village()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
         
                SqlParameter[] parameters = { new SqlParameter("@Village_Code", VillageCode) };
                Fill(dt, "SP_FDM_Select_WorkOrderByVilageCode", parameters);
           
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_WorkOrder_By_Village" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// select Project details
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Project()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
               
                SqlParameter[] parameters = { new SqlParameter("@MicroPlanID", MicroPlanID) };
                Fill(dt, "SP_FDM_Select_ProjectByMicroPlanCode", parameters);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Project" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// use to get workorder
        /// </summary>
        /// <returns></returns>
        public DataTable Select_WorkOrder()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters = { new SqlParameter("@MicroPlanID", MicroPlanID) };
                Fill(dt, "SP_FDM_Select_WorkOrderByMicroPlanID", parameters);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_WorkOrder" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Use to Get scheme
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Scheme()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters = { new SqlParameter("@ProjectID", ProjectID) };
                Fill(dt, "Select_Scheme_By_Project", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Scheme" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Use to get activity by scheme
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Activity_by_Scheme()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
             
                SqlParameter[] parameters = { new SqlParameter("@SchemeID", SchemeID) };
                Fill(dt, "Select_Activity_By_Scheme", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Activity_by_Scheme" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// use to get activity by workorder
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Activity_by_WorkOrder()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
             
                SqlParameter[] parameters = { new SqlParameter("@WorkOrderID", WorkOrderID) };
                Fill(dt, "SP_Select_ActivityByWorkOrderID", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Activity_by_WorkOrder" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// use to get subactivity by workorder and activity
        /// </summary>
        /// <returns></returns>
        public DataTable Select_SubActivity_by_WorkOrder()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlParameter[] parameters = { new SqlParameter("@WorkOrderID", WorkOrderID),
                                            new SqlParameter("@ActivityID", ActivityID) };
                Fill(dt, "SP_Select_SubActivityByWorkOrderID", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_SubActivity_by_WorkOrder" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

       /// <summary>
       /// Use to get activity by project
       /// </summary>
       /// <returns></returns>
        public DataTable Select_Activity_by_Project()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                
                SqlParameter[] parameters = { new SqlParameter("@ProjectID", ProjectID) };
                Fill(dt, "Select_Activity_By_Project", parameters);
          
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Activity_by_Project" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// use to get BSR details
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Activity_BSRAmount()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters = { new SqlParameter("@ActivityID", ActivityID),
                                            new SqlParameter("@subActivityID", SubActivty),
                                            new SqlParameter("@RangeCode", RangeCode)};
                Fill(dt, "select_Activity_BSR_ForSurvey", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Activity_BSRAmount" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Use to submit survey details
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Int64 AddSurveyDetails(DataTable dt)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Insert_Survey_Report", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SurveyDate", SurveyDate == null ? (object)DBNull.Value : SurveyDate);
                cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount == null ? (object)DBNull.Value : TotalAmount);
                cmd.Parameters.AddWithValue("@PhotoURL", PhotoURL == null ? (object)DBNull.Value : PhotoURL);
                cmd.Parameters.AddWithValue("@ComplitionYear", ComplitionYear == null ? (object)DBNull.Value : ComplitionYear);
                cmd.Parameters.AddWithValue("@AreaName", AreaName == null ? (object)DBNull.Value : AreaName);
                cmd.Parameters.AddWithValue("@Area", Area == null ? (object)DBNull.Value : Area);
                cmd.Parameters.AddWithValue("@Latitude", Latitude == null ? (object)DBNull.Value : Latitude);
                cmd.Parameters.AddWithValue("@Longitude", Longitude == null ? (object)DBNull.Value : Longitude);
                cmd.Parameters.AddWithValue("@Description", Description == null ? (object)DBNull.Value : Description);
                cmd.Parameters.AddWithValue("@FileTypeID", FileTypeID == null ? (object)DBNull.Value : FileTypeID);
                cmd.Parameters.AddWithValue("@FileURL", FileURL == null ? (object)DBNull.Value : FileURL);
                cmd.Parameters.AddWithValue("@ShapeFileURL", ShapeFileURL == null ? (object)DBNull.Value : ShapeFileURL);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy == null ? (object)DBNull.Value : EnteredBy);
                cmd.Parameters.AddWithValue("@surveyDetails", dt);
                //cmd.Parameters.AddWithValue("@option", option);
                  chk = Convert.ToInt64(cmd.ExecuteScalar());
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "AddSurveyDetails" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }

        /// <summary>
        /// Use to get the pending quantity of work(code added on (5/11/2016)
        /// </summary>
        /// <param name="objsurvey"></param>
        /// <returns></returns>
        public Int64 GetQuantity( SurveyReport objsurvey)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_GetQuantity", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActivityID", objsurvey.ActivityID);
                cmd.Parameters.AddWithValue("@WorkorderID", objsurvey.WorkOrderID);
                cmd.Parameters.AddWithValue("@SubActivityID", objsurvey.SubActivty);
                chk = Convert.ToInt64(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetQuantity" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        #endregion
    }
}