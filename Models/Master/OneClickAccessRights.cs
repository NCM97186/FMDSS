using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.Master
{
    public class OneClickAccessRights : DAL
    {
        public string OLDUSERID { get; set; }
        public string OLDSSOID { get; set; }
        public string NEWSSOID { get; set; }
        public int DESIGNATION { get; set; }
        public int ReportingTo { get; set; }
        public List<SelectListItem> ListDESIGNATIONs { get; set; }
        public string OfficeID { get; set; }

        public int[] RoleIds { get; set; }
        public string RoleIdstr { get; set; }
        public List<SelectListItem> ListRoleIds { get; set; }
        public int[] PLACEIDs { get; set; }
        public string PLACEIDstr { get; set; }
        public List<SelectListItem> ListPLACEIDs { get; set; }
        public string OffcLevel { get; set; }
        public string ForestBoundaries { get; set; }
        public Int64 USERID { get; set; }

        public DataSet Select_AllPlaceDesignationsRoles()
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_OneClickAccessRights", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETMASTERDATE");

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

        public DataSet GetOneClickAccessData(string action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_OneClickAccessPermission", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@LOGINUSERID", HttpContext.Current.Session["UserId"]);

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


        public DataTable Select_REPORTINGTO(string DESIGNATION)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_OneClickAccessRights", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETREPORTINGTO");
                cmd.Parameters.AddWithValue("@DESIGNATION", DESIGNATION);
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


        public DataTable UPDATESSOID(OneClickAccessRights Models)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_OneClickAccessRights", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "UPDATESSOID");
                cmd.Parameters.AddWithValue("@OLDSSOID", Models.OLDSSOID);
                cmd.Parameters.AddWithValue("@NEWSSOID", Models.NEWSSOID);
                cmd.Parameters.AddWithValue("@DESIGNATION", Models.DESIGNATION);
                cmd.Parameters.AddWithValue("@REPORTINGTO", Models.ReportingTo);
                cmd.Parameters.AddWithValue("@OfficeID", Models.OfficeID);
                cmd.Parameters.AddWithValue("@PLCAEIDs", Models.PLACEIDstr);
                cmd.Parameters.AddWithValue("@RoleIds", Models.RoleIdstr);
                cmd.Parameters.AddWithValue("@LOGINUSERID", Models.USERID);
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

        #region add by sunny for multiple office mapping
        public DataTable InsertMultipleOfficeMapping(OneClickAccessRights Models)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SSODetails_MultipleOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "InsertForMultipleOffice");
                cmd.Parameters.AddWithValue("@OLDSSOID", Models.OLDSSOID);
                cmd.Parameters.AddWithValue("@NEWSSOID", Models.NEWSSOID);
                cmd.Parameters.AddWithValue("@DESIGNATION", Models.DESIGNATION);
                cmd.Parameters.AddWithValue("@REPORTINGTO", Models.ReportingTo);
                cmd.Parameters.AddWithValue("@OfficeID", Models.OfficeID);
                cmd.Parameters.AddWithValue("@PLCAEIDs", Models.PLACEIDstr);
                cmd.Parameters.AddWithValue("@RoleIds", Models.RoleIdstr);
                cmd.Parameters.AddWithValue("@LOGINUSERID", Models.USERID);
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
        public DataTable RemoveTmpSSOID(OneClickAccessRights Models)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SSODetails_MultipleOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "RemoveTmpSSOID");
                cmd.Parameters.AddWithValue("@OLDSSOID", Models.OLDSSOID);
                cmd.Parameters.AddWithValue("@LOGINUSERID", Models.USERID);
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
        #endregion
    }
}