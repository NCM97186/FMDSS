using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class cls_UserSession:DAL
    {
        public class clsUserSession
        {
            public string SSOId { get; set; }
            public string SessionId { get; set; }
            public string SessionTimeOut { get; set; }
            public string IPAddress { get; set; }
            public DateTime InsertDateTime { get; set; }
            public bool isActive { get; set; }
        }

        public class listFail
        {
            public int INDEX { get; set; }
            public string NAME { get; set; }
            public string SSOID { get; set; }
            public string RequestID { get; set; }
            public string PlaceName { get; set; }
            public string ZoneName { get; set; }
            public string ShiftName { get; set; }
            public string DAteofArrival { get; set; }
            public string TotalMembers { get; set; }
            public string IP_Address { get; set; }
            public string FAIL { get; set; }
        }

        public class clsFailReport
        {
            public string VisitDate { get; set; }
            public string ssoId { get; set; }             
        }

        public DataTable GetFailRecord(FMDSS.Models.cls_UserSession.clsFailReport Details)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spUserSessionTimeOut", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "FAILTRN");
                cmd.Parameters.AddWithValue("@VisitDate", Details.VisitDate);
                cmd.Parameters.AddWithValue("@SSOID", Details.ssoId);
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




        public bool SaveUserLogs(clsUserSession oModal)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spUserSessionTimeOut", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "InsertSession");
                cmd.Parameters.AddWithValue("@SSOId", oModal.SSOId);
                cmd.Parameters.AddWithValue("@SessionId", oModal.SessionId);
                cmd.Parameters.AddWithValue("@SessionTimeOut", oModal.SessionTimeOut);
                cmd.Parameters.AddWithValue("@IPAddress", oModal.IPAddress);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return Convert.ToBoolean(dt.Rows[0][0]);

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

        public void LogoutUser(string ssoId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spUserSessionTimeOut", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DeleteSession");
                cmd.Parameters.AddWithValue("@SSOId", ssoId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

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

    }
}