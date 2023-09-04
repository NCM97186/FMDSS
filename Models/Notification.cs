using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class Notification:DAL
    {

        /// Model contain public property and methods
        /// </summary>
        public Int64 MessageId { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string EnteredOn { get; set; }
        public Int64 CreatedBy { get; set; }
        public int IsActive { get; set; }
        public int Status { get; set; }
        public bool IsRead { get; set; }


        public DataTable GetAllmessage(Int64 userId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Select_GetAllMessagesB_Byuser", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_UserId", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
               
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetAllmessage" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetNewmessage(Int64 userId, string today)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Select_GetNewmessage_Byuser", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_UserId", userId);
                cmd.Parameters.AddWithValue("@P_Today", today);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
           
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetNewmessage" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataSet GetEmailDetail(int msgId)
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Select_EmailDetail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_MsgId", msgId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
             
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetEmailDetail" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
    }
}