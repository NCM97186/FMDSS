using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment
{
    public class SurveyBudget : DAL
    {
        #region Global Variable

        public string UserName { get; set; }
        public string Designation { get; set; }
        public Int64 UserID { get; set; }
        public Int64 SurveyID { get; set; }

        public Int64 ModelID { get; set; }
        public string RangeCode { get; set; }
        public string VillageCode { get; set; }
        public string Village { get; set; }

        public Int64 MicroPlanID { get; set; }
        public string MicroPlan { get; set; }

        public Int64 SchemeID { get; set; }
        public string SchemeName { get; set; }
        
        public string BudgetHead { get; set; }
        public string ModelName { get; set; }

        public Int64 ProjectID { get; set; }
        public string Project { get; set; }
        public Int64 ActivityID { get; set; }
        public string Activity { get; set; }
        public Int64 SubActivityID { get; set; }

        public string SubActivity { get; set; }
        public decimal EstimatedAmt { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime SurveyDate { get; set; }
        public string SDate { get; set; }

        public string PhotoURL { get; set; }
        public string FileURL { get; set; }
        public string ShapeFileURL { get; set; }
        public string ComplitionYear { get; set; }

        public string Description { get; set; }
        public string AreaName { get; set; }
        public string Area { get; set; }
        public string SurveyorName { get; set; }
        public Int64 EnteredBy { get; set; }
        public Int64 UpdatedBy { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Status { get; set; }
        public string Range_Code { get; set; }
        #endregion
        /// <summary>
        /// Select_SurveyReports
        /// </summary>
        /// <returns></returns>
        public DataTable Select_SurveyReports()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
             
                Fill(dt, "select_Survey_Details");
             
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_SurveyReports" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Range
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Range()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
              
                SqlParameter[] parameters = { new SqlParameter("@UserID", UserID) };
                Fill(dt, "Select_Range_For_LoginUser", parameters);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Range" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Villages
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Villages()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
               
                SqlParameter[] parameters = { new SqlParameter("@RangeCode", RangeCode) };
                Fill(dt, "Select_Village_For_LoginUser", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Villages" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
/// <summary>
        /// Select_MicroPlan_By_Village
/// </summary>
/// <returns></returns>
        public DataTable Select_MicroPlan_By_Village()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
          
                SqlParameter[] parameters = { new SqlParameter("@Village_Code", VillageCode) };
                Fill(dt, "SP_FDM_Select_MicroPlanByVilageCode", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_MicroPlan_By_Village" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Project_By_MicroPlanID
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Project_By_MicroPlanID()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {              
                SqlParameter[] parameters = { new SqlParameter("@MicroPlanID", MicroPlanID) };
                Fill(dt, "SP_FDM_Select_ProjectByMicroPlanCode", parameters);               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Project_By_MicroPlanID" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        
        /// <summary>
        /// Select_Model_By_MicroID
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Model_By_MicroId()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@MicroPlanID", MicroPlanID) };
                Fill(dt, "SP_FDM_Select_ModelByMicroPlanID", parameters);            
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_FDM_Select_ModelByMicroPlanID" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Model_By_ProjectID
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Model_By_ProjectID()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {         
                SqlParameter[] parameters = { new SqlParameter("@ID", ProjectID) };
                Fill(dt, "SP_FDM_Select_ModelByProjectID", parameters);            
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Model_By_ProjectID" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Activity_By_ModelID
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Activity_By_ModelID()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
               
                SqlParameter[] parameters = { new SqlParameter("@ModelID", ModelID) };
                Fill(dt, "SP_FDM_Select_ActivityByModelID_Budget", parameters);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Activity_By_ModelID" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
     
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Scheme
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Scheme()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
               
                SqlParameter[] parameters = { new SqlParameter("@MicroPlanID", MicroPlanID) };
                Fill(dt, "Select_Scheme_by_MicroPlan", parameters);
            
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Scheme" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Project
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Project()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
               
                SqlParameter[] parameters = { new SqlParameter("@schemeID", SchemeID) };
                Fill(dt, "Select_Project_By_Scheme", parameters);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Project" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Activity_by_Project
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Activity_by_Project()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
             
                SqlParameter[] parameters = { new SqlParameter("@ProjectID", ProjectID) };
                Fill(dt, "Select_Activity_By_Project", parameters);
             
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Activity_by_Project" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select_Activity_BSRAmount
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Activity_BSRAmount()
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
              
                SqlParameter[] parameters = { new SqlParameter("@ActivityID", ActivityID) };
                Fill(dt, "select_Activity_BSR_Amount", parameters);
                
            }
            catch (Exception ex)
            {
                new  Common().ErrorLog(ex.Message, "Select_Activity_BSRAmount" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
/// <summary>
        /// Select_Bgt_Estimate_Survey
/// </summary>
/// <param name="UserID"></param>
/// <returns></returns>
        public DataTable Select_Bgt_Estimate_Survey(Int64 UserID)
        {
            DALConn();
            DataTable dt = new DataTable();
            try
            {
             
                SqlParameter[] parameters =
                {  
                new SqlParameter("@Status",1),  
                new SqlParameter("@UserID",UserID),      
                        
                };
               
                Fill(dt, "Select_BGT_Estimate_Survey", parameters);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Bgt_Estimate_Survey" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// AddSurveyDetails
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string AddSurveyDetails(DataTable dt)
        {
            string chk ="";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Insert_Survey_Estimation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TotalEsSurveyAmt", TotalAmount == null ? (object)DBNull.Value : TotalAmount);
                cmd.Parameters.AddWithValue("@SurveyDate", SurveyDate);
                cmd.Parameters.AddWithValue("@CompletionYear", ComplitionYear == null ? (object)DBNull.Value : ComplitionYear);
                cmd.Parameters.AddWithValue("@Description", Description == null ? (object)DBNull.Value : Description);
 
                cmd.Parameters.AddWithValue("@PhotoURL", PhotoURL == null ? (object)DBNull.Value : PhotoURL);
                cmd.Parameters.AddWithValue("@AreaName", AreaName == null ? (object)DBNull.Value : AreaName);
                cmd.Parameters.AddWithValue("@Area", Area == null ? (object)DBNull.Value : Area);
                cmd.Parameters.AddWithValue("@Latitude", Latitude == null ? (object)DBNull.Value : Latitude);
                cmd.Parameters.AddWithValue("@Longitude", Longitude == null ? (object)DBNull.Value : Longitude);
                cmd.Parameters.AddWithValue("@FileURL", FileURL == null ? (object)DBNull.Value : FileURL);
                cmd.Parameters.AddWithValue("@ShapeFileURL", ShapeFileURL == null ? (object)DBNull.Value : ShapeFileURL);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy == null ? (object)DBNull.Value : EnteredBy);
                cmd.Parameters.AddWithValue("@Status", 1);
                cmd.Parameters.AddWithValue("@BudgetEstimateSurvey", dt);
                //cmd.Parameters.AddWithValue("@option", option);
                 chk = cmd.ExecuteScalar().ToString();
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "AddSurveyDetails" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// UpdateSurveyDetails
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>

        public string UpdateSurveyDetails(DataTable dt)
        {
            string chk = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Insert_Survey_Estimation", Conn);
              //  Conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TotalEsSurveyAmt", TotalAmount == null ? (object)DBNull.Value : TotalAmount);
                cmd.Parameters.AddWithValue("@SurveyDate", SurveyDate);
                cmd.Parameters.AddWithValue("@CompletionYear", ComplitionYear == null ? (object)DBNull.Value : ComplitionYear);
                cmd.Parameters.AddWithValue("@Description", Description == null ? (object)DBNull.Value : Description);

                cmd.Parameters.AddWithValue("@PhotoURL", PhotoURL == null ? (object)DBNull.Value : PhotoURL);
                cmd.Parameters.AddWithValue("@AreaName", AreaName == null ? (object)DBNull.Value : AreaName);
                cmd.Parameters.AddWithValue("@Area", Area == null ? (object)DBNull.Value : Area);
                cmd.Parameters.AddWithValue("@Latitude", Latitude == null ? (object)DBNull.Value : Latitude);
                cmd.Parameters.AddWithValue("@Longitude", Longitude == null ? (object)DBNull.Value : Longitude);
                cmd.Parameters.AddWithValue("@FileURL", FileURL == null ? (object)DBNull.Value : FileURL);
                cmd.Parameters.AddWithValue("@ShapeFileURL", ShapeFileURL == null ? (object)DBNull.Value : ShapeFileURL);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy == null ? (object)DBNull.Value : EnteredBy);
                cmd.Parameters.AddWithValue("@Status", 2);
                cmd.Parameters.AddWithValue("@ID",SurveyID);
                cmd.Parameters.AddWithValue("@BudgetEstimateSurvey", dt);
                //cmd.Parameters.AddWithValue("@option", option);
                  chk = cmd.ExecuteScalar().ToString();
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateSurveyDetails" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// EditSurveyDetails
        /// </summary>
        /// <returns></returns>
        public DataSet EditSurveyDetails()
        {
            DataSet DS = new DataSet();
            try
            {
                DALConn();
                
                SqlParameter[] parameters =
                {  
                new SqlParameter("@Status",2),  
                new SqlParameter("@SurveyID",SurveyID),      
                        
                };

                Fill(DS, "Select_BGT_Estimate_Survey", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "EditSurveyDetails" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
     
            }
            finally
            {
                Conn.Close();
            }
            return DS;
        }


    }
}