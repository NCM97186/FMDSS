using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FMDSS.Models;

namespace FMDSS
{
    public class DMSmodel : DAL
    {
        public DataTable GetModuleForDMS()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetModuleForDMS");

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

        public DataTable GetServiceTypeForDMS(string ModuleCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetServiceTypeForDMS");
                cmd.Parameters.AddWithValue("@PARAMETER1", ModuleCode);
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
        public DataTable GetPermissionForDMS(string ModuleCode, string ServiceTypeCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetPermissionForDMS");
                cmd.Parameters.AddWithValue("@PARAMETER1", ModuleCode);
                cmd.Parameters.AddWithValue("@PARAMETER2", ServiceTypeCode);
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

        public DataTable GetSubPermissionForDMS(string ModuleCode, string ServiceTypeCode, string PermissionCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetSubPermissionForDMS");
                cmd.Parameters.AddWithValue("@PARAMETER1", ModuleCode);
                cmd.Parameters.AddWithValue("@PARAMETER2", ServiceTypeCode);
                cmd.Parameters.AddWithValue("@PARAMETER3", PermissionCode);
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

        public DataTable GetFetchRepoData(string ModuleCode, string ServiceTypeCode, string PermissionCode, string SubPermissionCode, string FromDate, string ToDate)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("DMS_GetTablesForDMSFetch", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ModuleId", ModuleCode);
                cmd.Parameters.AddWithValue("@ServiceTypeId", ServiceTypeCode);
                cmd.Parameters.AddWithValue("@PermissionId", PermissionCode);
                cmd.Parameters.AddWithValue("@SubPermissionId", SubPermissionCode);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
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

        public DataTable FetchRepoDataforWildLifeAndZoo(string WildLifeAndZoo, string DateType, string FromDate, string ToDate, string PlaceID, string RequestID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("DMS_GetWildLifeAndZooTablesForDMSFetch", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ACTION", "FACTCHDATA");
                cmd.Parameters.AddWithValue("@TABLETYPE", WildLifeAndZoo);
                cmd.Parameters.AddWithValue("@DATETYPE", DateType);               
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceID);
                cmd.Parameters.AddWithValue("@RequestId", RequestID);
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

        public DataTable FetchPLACES(string WildLifeAndZoo)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("DMS_GetWildLifeAndZooTablesForDMSFetch", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ACTION", "GETPLACE");
                cmd.Parameters.AddWithValue("@TABLETYPE", WildLifeAndZoo);
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

        
    }

    public class ListDMSFetchData
    {
        public string RequestId { get; set; }
        public string FileExtension { get; set; }
        public string FileType { get; set; }
        public string DownloadDocument { get; set; }       
    }

 
        public class oListDMSFetchData
        {
            public string RequestId { get; set; }
            public List<DMSdata> DMSFields { get; set; }
            
        }
        public class DMSdata
        {
            public string FileExtension { get; set; }
            public string FileType { get; set; }
            public string DownloadDocument { get; set; }
        }
    }
 