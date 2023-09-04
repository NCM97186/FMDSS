
// *****************************************************************************************
// Author : Rajkumar Singh
// Created : 23-Dec-2015
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
    public class FdmProgram:DAL
    {
        /// <summary>
        /// Property declare
        /// </summary>
        public string ProgramName { get; set; }
        public string ProgramDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string StartDate1 { get; set; }
        public string EndDate1 { get; set; }
        public long ID { get; set; }
        public string FundingAgency { get; set; }
        public string Terms_Ref_Doc { get; set; }
        public DateTime? Extended_ToDate { get; set; }
        public string Extended_ToDate1 { get; set; }
        public string Revised_Ref_Doc { get; set; }

        /// <summary>
        /// Bind Program details into dropdown
        /// </summary>
        /// <returns></returns>
        public DataSet GetProgram() {
            DataSet dsModel = new DataSet();
            try
            {
                DALConn();
               
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","1"),                   
                new SqlParameter("@ProgramId",Convert.ToInt64("0")),            
                };
                Fill(dsModel, "SP_Fdm_GetProgramDetail", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetProgram" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dsModel;             
        }

        public DataSet BindFAgency()
        {
            DataSet dsAgency = new DataSet();
            try
            {
                DALConn();
              
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","2"),                   
                new SqlParameter("@ProgramId",Convert.ToInt64("0")),            
                };
                Fill(dsAgency, "SP_Fdm_GetProgramDetail", parameters);
                   
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindFAgency" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dsAgency;          
        }

        public DataSet EditProgram()
        {
            DataSet dsEdit = new DataSet();

            try
            {
                DALConn();
              
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","3"),                   
                new SqlParameter("@ProgramId",ID),            
                };
                Fill(dsEdit, "SP_Fdm_GetProgramDetail", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "EditProgram" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dsEdit;
        } 
        /// <summary>
        /// Function to add new program 
        /// </summary>
        /// <returns></returns>
        public Int64 InsertProgram() {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {  
            new SqlParameter("@ID",ID),         
            new SqlParameter("@ProgramName",ProgramName),           
            new SqlParameter("@ProgramDesc", ProgramDesc),
            new SqlParameter("@FundingAgency",FundingAgency),
            new SqlParameter("@Term_Ref_Doc",Terms_Ref_Doc),
            new SqlParameter("@StartDate", StartDate),      
            new SqlParameter("@EndDate", EndDate),              
            new SqlParameter("@IsActive", true),
            new SqlParameter("@EnterBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])),
            };
                  chk = Convert.ToInt64(ExecuteScalar("Sp_AddProgram", parameters));
            
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertProgram" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return chk;      
        }

        public Int64 UpdateProgram()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {  
            new SqlParameter("@ID",ID),                   
            new SqlParameter("@ExtendedDate",Extended_ToDate), 
            new SqlParameter("@Revised_Ref_Doc",Revised_Ref_Doc),          
            new SqlParameter("@EnterBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])),
            };
                  chk = Convert.ToInt64(ExecuteScalar("Sp_UpdateProgram", parameters));
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateProgram" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      

            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }


    }
}