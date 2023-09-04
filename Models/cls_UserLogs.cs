using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{

    public class clsModuleDetails
    {
        public string ModuleDesc { get; set; }
        public string ServiceTypeDesc { get; set; }
        public string SubPermissionDesc { get; set; }
        public string ModuleId { get; set; }
        
    }

    public class cls_UserLogs : DAL
    {

        public bool SaveUserLogs(string ActivityName, DateTime ActivityDateTime, string ActivityType, string ControllerName, string ActionName, string ModuleName, string SSOId, string IPAddress)
        {
            try
            {
                var ModuleDetails = GetModuleName(ActionName);
                DALConn();
                SqlCommand cmd = new SqlCommand("spUserLogs", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "AddLogs");
                cmd.Parameters.AddWithValue("@ActivityName", ActivityName);
                
                cmd.Parameters.AddWithValue("@ActivityType", ActivityType);
                cmd.Parameters.AddWithValue("@ControllerName", ControllerName);
                cmd.Parameters.AddWithValue("@ActionName", ActionName);

                //var ModuleDetails = GetModuleName(ActionName);

                cmd.Parameters.AddWithValue("@ModuleName", ModuleDetails.ModuleDesc);
                cmd.Parameters.AddWithValue("@ServiceTypeDesc", ModuleDetails.ServiceTypeDesc);
                cmd.Parameters.AddWithValue("@SubPermissionDesc", ModuleDetails.SubPermissionDesc);
                cmd.Parameters.AddWithValue("@ModuleId", ModuleDetails.ModuleId);
                cmd.Parameters.AddWithValue("@SSOID", SSOId);
                cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                
                cmd.ExecuteNonQuery();

                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, ActivityName + "_" + "production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return true;
        }


        private clsModuleDetails GetModuleName(string ActionName)
        {
            DataTable dt = new DataTable();
            clsModuleDetails M = new clsModuleDetails();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spUserLogs", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETModuleDetails");
                cmd.Parameters.AddWithValue("@ActionName", ActionName.Replace("messagetype=","").Trim());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
                
                M.ModuleId = Convert.ToString(dt.Rows[0]["ModuleId"]);
                M.ModuleDesc=Convert.ToString(dt.Rows[0]["ModuleDesc"]);
                M.ServiceTypeDesc = Convert.ToString(dt.Rows[0]["ServiceTypeDesc"]);
                M.SubPermissionDesc = Convert.ToString(dt.Rows[0]["SubPermissionDesc"]);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return M;
        }

    }
}