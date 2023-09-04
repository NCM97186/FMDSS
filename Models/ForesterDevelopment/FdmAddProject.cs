
// *****************************************************************************************
// Author : Rajkumar Singh
// Created : 29-Dec-2015
// *****************************************************************************************
// <summary>This Model is Created for Add Program in Forest Development</summary>
// *****************************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace FMDSS.Models.ForestDevelopment
{
    public class FdmAddProject:DAL
    {
        /// <summary>
        /// Property declare
        /// </summary>
        public long ID { get; set; }
        public string Project_Code { get; set; }
        public string Project_Name { get; set; }
        public decimal AreaofRolloutinSQKM { get; set; }
        public long Scheme_Id { get; set; }
        public string ReferenceNo { get; set; }
        public string RefDocument { get; set; }
        public string DPRDocument { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDate1 { get; set; }
        public string EndDate1 { get; set; }
        public decimal EstimatedBudget { get; set; }
        public string Model_Code { get; set; }
        public string Dist_Code { get; set; }
        public DateTime EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public string StatusDesc { get; set; }

        public decimal SchemeBudget { get; set; }
        public string ActivityName { get; set; }
        public string ActivityType { get; set; }
        public string ActivityTotalCost { get; set; }
        /// <summary>
        /// Used for binding scheme
        /// </summary>
        /// <returns></returns>
        public DataSet BindScheme()
        {
            DataSet dsScheme = new DataSet();
            try
            {
                DALConn();            
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","1"),  
                new SqlParameter("@ProjectId",Convert.ToInt64("0")),      
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),            
                };
                Fill(dsScheme, "SP_FDM_GetProjectDetail", parameters);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindScheme" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
     
            }
            finally
            {
                Conn.Close();
            }
            return dsScheme;
        }
        /// <summary>
        /// Used to bind project
        /// </summary>
        /// <returns></returns>
        public DataSet BindProject()
        {
            DataSet dsProject = new DataSet();
            try
            {
                DALConn();
             
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","2"),  
                new SqlParameter("@ProjectId",Convert.ToInt64("0")),      
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),            
                };
                Fill(dsProject, "SP_FDM_GetProjectDetail", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindScheme" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
     
            }
            finally
            {
                Conn.Close();
            }
            return dsProject;

        }
        /// <summary>
        /// Used to bind model
        /// </summary>
        /// <returns></returns>
        public DataSet BindModel()
        {
            DataSet dsModel = new DataSet();
            try
            {
                DALConn();
              
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","3"),  
                new SqlParameter("@ProjectId",Convert.ToInt64("0")),      
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),            
                };
                Fill(dsModel, "SP_FDM_GetProjectDetail", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindModel" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dsModel;
        }
        /// <summary>
        /// Used to bind model
        /// </summary>
        /// <returns></returns>
        public DataSet BindDivision()
        {
            DataSet dsDivision = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","4"),  
                new SqlParameter("@ProjectId",Convert.ToInt64("0")),      
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),            
                };
                Fill(dsDivision, "SP_FDM_GetProjectDetail", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDivision" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dsDivision;
        }
      
        /// <summary>
        /// Final submission of project
        /// </summary>
        /// <returns></returns>
        public Int64 InsertProject()
        {
            DALConn();
            Int64 chk = 0;
            try
            {
                SqlParameter[] parameters =
            {  
            new SqlParameter("@ID",ID),           
            new SqlParameter("@Project_Name", Project_Name),
            new SqlParameter("@Scheme_Id", Scheme_Id),
            new SqlParameter("@Model_Code", Model_Code),
            new SqlParameter("@Dist_Code", Dist_Code),
            new SqlParameter("@RefDocument", RefDocument),   
            new SqlParameter("@RefNoDocument", ReferenceNo),          
            new SqlParameter("@StartDate", StartDate),
            new SqlParameter("@EndDate", EndDate),
            new SqlParameter("@AreaofRolloutinSQKM", AreaofRolloutinSQKM),
            new SqlParameter("@EstimateBudget", EstimatedBudget),
            new SqlParameter("@Upload_DPR", DPRDocument),          
            new SqlParameter("@EnteredBy", EnteredBy),
            new SqlParameter("@UpdatedBy", EnteredBy),
            new SqlParameter("@IsActive", IsActive),
            new SqlParameter("@Status", Status),   
            };
                 chk = Convert.ToInt64(ExecuteScalar("SP_FDM_ADD_UPDATE_PROJECT", parameters));
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertProject" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        public DataSet EditProject()
        {
            DataSet dsProject = new DataSet();
            try
            {
                DALConn();
              
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","5"),  
                new SqlParameter("@ProjectId",ID),      
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),            
                };
                Fill(dsProject, "SP_FDM_GetProjectDetail", parameters);
               
            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "EditProject" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dsProject;
        }

        public DataSet GetModel(Int64 SchemeId)
        {
            DataSet dsModelDist = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","6"),  
                new SqlParameter("@ProjectId",Convert.ToInt64("0")),      
                new SqlParameter("@SchemeId",SchemeId),            
                };
                Fill(dsModelDist, "SP_FDM_GetProjectDetail", parameters);
             
            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetModel" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dsModelDist;
        }
        public DataSet GetToolTipDetail(Int64 ModelId)
        {
            DataSet dsToolTip = new DataSet();
            try
            {
                DALConn();
              
                SqlParameter[] parameters =
                {  
                new SqlParameter("@ModelId",ModelId),                 
                };
                Fill(dsToolTip, "sp_FDM_GetToolTipDetails", parameters);
      
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetToolTipDetail" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dsToolTip;
        }
    }
}