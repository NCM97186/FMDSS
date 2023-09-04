//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Apply Auction
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class FeedBack : DAL
    {
        /// <summary>
        /// Model contain public property and methods
        /// </summary>
        public Int64 FeedBackId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Query { get; set; }
        public string Answer { get; set; }
        public DateTime CreationDate { get; set; }
        public string EnteredOn { get; set; }
        public Int64 CreatedBy { get; set; }

        public Int64 UpdatedBy { get; set; }
        public int IsActive { get; set; }
        public string Status { get; set; }

        /// <summary>
        /// Used for save query detail into database
        /// </summary>
        /// <returns></returns>
        public Int64 AddFeedbackData()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Insert_Feedback", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ID", FeedBackId);
                cmd.Parameters.AddWithValue("@P_Name", Name);
                cmd.Parameters.AddWithValue("@P_Email", Email);
                cmd.Parameters.AddWithValue("@P_Phone", Phone);
                //cmd.Parameters.AddWithValue("@CreationDate", CreationDate);
                cmd.Parameters.AddWithValue("@P_Query", Query);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                 chk = Convert.ToInt64(cmd.ExecuteScalar());
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "AddFeedbackData" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }

            return chk;
        }

        public Int64 AddAnswer()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Admin_Answer_Feedback", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ID", FeedBackId);
                cmd.Parameters.AddWithValue("@P_Answer", Answer);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_Status", Status);
                  chk = Convert.ToInt64(cmd.ExecuteScalar());
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "AddAnswer" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return chk;

        }

        public DataTable BindSubmitedQuery(string action)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Admin_Select_SubmitedQuery", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ActionFlag", action);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindSubmitedQuery" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable FetchQueryById(Int64 feedbackId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Admin_Select_QueryById", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_FeedbackId", feedbackId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "FetchQueryById" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }



    }
}