using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class PageAccessPermission : DAL
    {
        public int RoleId { get; set; }
        public int Index { get; set; }
        public string PageID { get; set; }      
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public string OperationType { get; set; }
        public List<UnMappedPageAccessPermission> UnMappedPageAccessLIST { get; set; }
        public List<MappedPageAccessPermission> MappedPageAccessLIST { get; set; }
        
        public DataTable Select_UseRoles()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_PageAccessPermissions", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllUseRoles");
                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataSet GetMapUnmapPageAccessPermission(int RoleId)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_PageAccessPermissions", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetMapUnmapPageAccessPermissions");
                cmd.Parameters.AddWithValue("@ROLEID", RoleId);
                cmd.CommandType = CommandType.StoredProcedure;

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
        public string MappingForPageAccessPermission(string PageIDs, int RoleIds, bool STATUS, Int64 LOGINSSOID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_PageAccessPermissions", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "MappingPageAccessPermissions");
                cmd.Parameters.AddWithValue("@PageID", PageIDs);
                cmd.Parameters.AddWithValue("@ROLEID", RoleIds);
                cmd.Parameters.AddWithValue("@ISACTIVE", STATUS);
                cmd.Parameters.AddWithValue("@LOGINSSOID", LOGINSSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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

    public class UnMappedPageAccessPermission
    {
     
        public int Index { get; set; }
        public string PageID { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string PageTitle { get; set; }
        public string PageURL { get; set; }
        public bool IsactiveView { get; set; }
    }

    public class MappedPageAccessPermission
    {
        public int Index { get; set; }
        public string PageID { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string PageTitle { get; set; }
        public string PageURL { get; set; }
        public bool IsactiveView { get; set; }
    }


}