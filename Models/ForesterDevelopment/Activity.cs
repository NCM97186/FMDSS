//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : Activity.cs
//  Description  : Object / Properies creation of Model
//  Date Created : 11-Jan-2016
//  History      : 
//  Version      : 1.0
//  Author       : Gaurav Pandey
//  Modified By  : Gaurav Pandey
//  Modified On  : 05-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment
{
    #region "Property Dist Mapping"
    public class SubActvityMap
    {
        public string ID { get; set; }

        public string ActivityID { get; set; }

        public string Sub_Activity_Name { get; set; }

        public string Sub_Activity_Unit { get; set; }
        public string Sub_Activity_BSRType { get; set; }
        public string Sub_Activity_totalCost { get; set; }

        public string BitStaus { get; set; }
        public decimal RatePerUnit { get; set; }

    }
    #endregion
    public class Activity : DAL
    {
        #region data members
        public long ID { get; set; }
        public Int64 UserID { get; set; }
        public string Sub_Activity_Unit { get; set; }
        public long Model_ID { get; set; }
        public string Activity_Year_Name { get; set; }
        public string Model_Name { get; set; }
        public long Index { get; set; }


        public string SSOID { get; set; }
        public string Activity_Name { get; set; }
        public string Activity_DocumentPath { get; set; }
        public string Activity_Type { get; set; }
        public string TableName { get; set; }


        public decimal Activity_BSR_Per_Unit { get; set; }
        public decimal Activity_BSR_Material_Cost { get; set; }
        public decimal Activity_BSR_Labour_Cost { get; set; }
        public decimal Activity_TotalCost { get; set; }

        public string Activity_RefNo { get; set; }
        public string Activity_StartDate { get; set; }

        public string Activity_EndDate { get; set; }
        public string Activity_Desc { get; set; }
        public bool ConditionFileEditMode { get; set; }
        public string IsChkSubActivity { get; set; }
        public string ConditionEdit { get; set; }
        public string BSR_Type { get; set; }
        public int Activity_Year { get; set; }
        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public decimal RatePerUnit { get; set; }

        #endregion
        /// <summary>
        /// Function for fetching  Model data fron Databse
        /// </summary>
        /// <param name=""></param>
        /// <returns>datatable</returns>
        public DataTable BindDDlModel()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Model", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDDlModel" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Function for Save the records of Activity
        /// </summary>
        /// <param name=""></param>
        /// <returns>datatable</returns>
        public Int64 SubmitActivity(Activity _objmodel)
        {

            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_Activity", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsSubActvity", _objmodel.IsChkSubActivity);
                cmd.Parameters.AddWithValue("@Activity_Name", _objmodel.Activity_Name);
                cmd.Parameters.AddWithValue("@Activity_Year", _objmodel.Activity_Year);
                //cmd.Parameters.AddWithValue("@Activity_BSR_Per_Unit", _objmodel.Activity_BSR_Per_Unit);
                cmd.Parameters.AddWithValue("@Sub_Activity_Unit", _objmodel.Sub_Activity_Unit);
                cmd.Parameters.AddWithValue("@Activity_BSR_Material_Cost", _objmodel.Activity_BSR_Material_Cost);
                cmd.Parameters.AddWithValue("@Activity_BSR_Labour_Cost", _objmodel.Activity_BSR_Labour_Cost);
                cmd.Parameters.AddWithValue("@Activity_TotalCost", _objmodel.Activity_TotalCost);
                cmd.Parameters.AddWithValue("@Activity_Type", _objmodel.Activity_Type);
                cmd.Parameters.AddWithValue("@Activity_DocumentPath", _objmodel.Activity_DocumentPath);
                cmd.Parameters.AddWithValue("@Activity_Desc", _objmodel.Activity_Desc);
                //cmd.Parameters.AddWithValue("@BSR_Type", _objmodel.BSR_Type);
                cmd.Parameters.AddWithValue("@Activity_RefNo", _objmodel.Activity_RefNo);
                //cmd.Parameters.AddWithValue("@Activity_StartDate", DateTime.ParseExact(_objmodel.Activity_StartDate.ToString(), "dd/MM/yyyy", null));
                //cmd.Parameters.AddWithValue("@Activity_EndDate", DateTime.ParseExact(_objmodel.Activity_EndDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@Activity_StartDate", "");
                cmd.Parameters.AddWithValue("@Activity_EndDate", "");
                cmd.Parameters.AddWithValue("@EnteredBy", _objmodel.UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitActivity" + "_" + "Development", 4, DateTime.Now, _objmodel.UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public DataSet GetAllRecords()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_Activity", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetAllRecords" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetAllRecords(Int64 ID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_Activity", Conn);
                cmd.Parameters.AddWithValue("@ActivityID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetAllRecords" + "_" + "BYID_Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataTable GetBSRByDivActivity(string Div_Code,Int64 Activity_ID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_FDM_select_DivActivity_BSR", Conn); 
                cmd.Parameters.AddWithValue("@ActivityID", Activity_ID);
                cmd.Parameters.AddWithValue("@Div_Code", Div_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetBSRByDivActivity" + "_" + "BYID_Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public Int64 DeleteActivity(Activity _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DeleteByTablenameandID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _objmodel.ID);
                cmd.Parameters.AddWithValue("@Tablename", _objmodel.TableName);


                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "DeleteActivity" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }


        public DataTable Division(Activity _objmodel)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_GetDivision", Conn);
                cmd.Parameters.AddWithValue("@SSOID", _objmodel.SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Division" + "_" + "Development", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public Int64 UpdateActivity(Activity _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_Activity", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _objmodel.ID);
                cmd.Parameters.AddWithValue("@IsSubActvity", _objmodel.IsChkSubActivity);
                cmd.Parameters.AddWithValue("@Activity_Year", _objmodel.Activity_Year);
                cmd.Parameters.AddWithValue("@Activity_Name", _objmodel.Activity_Name);
                cmd.Parameters.AddWithValue("@Sub_Activity_Unit", _objmodel.Sub_Activity_Unit);
                //cmd.Parameters.AddWithValue("@Activity_BSR_Per_Unit", _objmodel.Activity_BSR_Per_Unit);
                cmd.Parameters.AddWithValue("@Activity_BSR_Material_Cost", _objmodel.Activity_BSR_Material_Cost);
                cmd.Parameters.AddWithValue("@Activity_BSR_Labour_Cost", _objmodel.Activity_BSR_Labour_Cost);
                cmd.Parameters.AddWithValue("@Activity_Type", _objmodel.Activity_Type);
                cmd.Parameters.AddWithValue("@Activity_DocumentPath", _objmodel.Activity_DocumentPath);
                cmd.Parameters.AddWithValue("@Activity_TotalCost", _objmodel.Activity_TotalCost);
                //cmd.Parameters.AddWithValue("@BSR_Type", _objmodel.BSR_Type);
                cmd.Parameters.AddWithValue("@Activity_Desc", _objmodel.Activity_Desc);
                cmd.Parameters.AddWithValue("@Activity_RefNo", _objmodel.Activity_RefNo);
                //cmd.Parameters.AddWithValue("@Activity_StartDate", DateTime.ParseExact(_objmodel.Activity_StartDate.ToString(), "dd/MM/yyyy", null));
                //cmd.Parameters.AddWithValue("@Activity_EndDate", DateTime.ParseExact(_objmodel.Activity_EndDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@Activity_StartDate", "");
                cmd.Parameters.AddWithValue("@Activity_EndDate", "");
                cmd.Parameters.AddWithValue("@UpdatedBy", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@RatePerUnit", _objmodel.RatePerUnit);
                


                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitDefineModel" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }
        public void SaveActSubActMapping(List<SubActivity> list, Int64 UserID)
        {
            try
            {
                DataSet ds = new DataSet();
                DALConn();


                for (int i = 0; i < list.Count; i++)
                {

                    SqlCommand cmd = new SqlCommand("[Sp_Insert_ActvitySubActivityMap]", Conn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActivityID", list[i].Activity_ID);
                    cmd.Parameters.AddWithValue("@SubActivity", list[i].ID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Action", "INSERT");

                    cmd.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveActSubActMapping" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
   
            }
            finally
            {
                Conn.Close();
            }

        }

        public void DeleteActSubActMapping(Int64 ActivityID, Int64 SubActivityID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("[Sp_Insert_ActvitySubActivityMap]", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.Parameters.AddWithValue("@SubActivity", SubActivityID);
                cmd.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "DeleteActSubActMapping" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
   
            }
            finally
            {
                Conn.Close();
            }

        }


        public DataSet GetMapRecords(Int64 ID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_ModelWithActMap", Conn);
                cmd.Parameters.AddWithValue("@ModelID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetMapRecords" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
   
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataTable BindActivityYear()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
             
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_ActivityYear", Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindActivityYear" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
   

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for Save the records of Activity
        /// </summary>
        /// <param name=""></param>
        /// <returns>datatable</returns>
        public Int64 SubmitDivwiseActivity(string ActivityID, string Div_Code, string SubActivityID, string BSRLabour, string BSRMat, string type, Int64 UserId)
        {

            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_SaveCirclewiseActivitySubActivity", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Div_Code",ActivityID );
                cmd.Parameters.AddWithValue("@ActivityID", Convert.ToInt64(Div_Code));
                cmd.Parameters.AddWithValue("@SubActivityID", Convert.ToInt64(SubActivityID));
                cmd.Parameters.AddWithValue("@BSRLabour", Convert.ToDecimal(BSRLabour));
                cmd.Parameters.AddWithValue("@BSRMat", Convert.ToDecimal(BSRMat));
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(UserId));

         
                //cmd.Parameters.AddWithValue("@Activity_BSR_Per_Unit", _objmodel.Activity_BSR_Per_Unit);               

                Int64 chId = cmd.ExecuteNonQuery();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitActivity" + "_" + "Development", 4, DateTime.Now, 1);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }
    }

    public class CircleActivity
    {

       public int ID { get; set; }
        public string Name{ get; set; }
       public string type{ get; set; }
      public  string bsrLab{ get; set; }
       public string bsrMat{ get; set; }
       public string divbsrLab{ get; set; }
       public string divbsrMat{ get; set; }
    }
    public class ActivityModels
    {
        public string ID { get; set; }
        [Required]
        public string Activity_Name { get; set; }
        [Required]
        public string ActivityDescription { get; set; }
        [Required]
        public string ActivityGroup { get; set; }
        [Required]
        public string ActivityCategory { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceDoc { get; set; }
        public string CampaCategory { get; set; }
        public int IsActive { get; set; }
        public int Index { get; set; }
        [Required]
        public string Activity_FullName { get; set; }
    }

    public class ActivityModel : ActivityModels
    {
        public ActivityModel()
        {
            List = new List<ActivityModels>();
        }
        public List<ActivityModels> List { get; set; }
    }

    public class ActivityRepo : DAL
    {
        public DataTable BindActivityYear()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_ActivityYear", Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindActivityYear" + "_" + "ActivityRepo", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable BindDDlModel()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Model", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDDlModel" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataSet InsertActivity(ActivityModel model, string ActionName, long UserID)
        {
            DataSet dsScheme = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@Action",ActionName),                   
                new SqlParameter("@ID",Convert.ToInt64(model.ID)),
                 new SqlParameter("@Activity_FullName",model.Activity_FullName),
                 new SqlParameter("@Activity_Name",model.Activity_Name),  
                  new SqlParameter("@ActivityDescription",model.ActivityDescription),  
                   new SqlParameter("@ActivityGroup",model.ActivityGroup),  
                    new SqlParameter("@ActivityCategory",model.ActivityCategory),  
                  new SqlParameter("@ReferenceNo",model.ReferenceNo),  
                   new SqlParameter("@ReferenceDoc",model.ReferenceDoc),  
                     new SqlParameter("@CampaCategory",model.CampaCategory),
                    new SqlParameter("@IsActive",1),  
                    new SqlParameter("@CreatedBy",UserID),  
                };
                Fill(dsScheme, "sp_NewActivityForWideLife", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertActivity" + "_" + "ActivityRepo", 4, DateTime.Now, UserID);

            }
            finally
            {
                Conn.Close();
            }
            return dsScheme;
        }
    }

}