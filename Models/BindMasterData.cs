//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : Bind Master Data Class
//  Description  : File contains functions for fetching master data from DB
//  Date Created : 24-Dec-2015
//  History      : 
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FMDSS.Models;

namespace FMDSS.Models
{
    public class BindMasterData : DAL
    {
        #region Member Functions

        /// <summary>
        /// function responsible for fetching district master
        /// </summary>
        /// <returns></returns>
        public DataTable getDistricts()
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_District", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetVehicleType" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// function responsible for fetching filtered Tehsil master
        /// </summary>
        /// <param name="DistID"></param>
        /// <returns></returns>
        public DataTable getTehsils(string DistID)
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_Common_Select_tehsil", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@distid", DistID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "getTehsils" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// function responsible for fetching Persmissions master
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        public DataTable GetPermission(int moduleId, int ServiceId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GetServicePermissions", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@moduleId", moduleId);
                cmd.Parameters.AddWithValue("@ServiceId", ServiceId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetPermission" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// function responsible for fetching SubPersmissions master
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <param name="PermissionId"></param>
        /// <returns></returns>
        public DataTable GetSubPermission(int moduleId, int ServiceId, int PermissionId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GetServicePermissions", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@moduleId", moduleId);
                cmd.Parameters.AddWithValue("@ServiceId", ServiceId);
                cmd.Parameters.AddWithValue("@PermissionId", PermissionId);
                cmd.Parameters.AddWithValue("@IsSubPermission", 1);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetSubPermission" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// function responsible for fetching Forest boundary master
        /// </summary>
        /// <param name="OfficeLevel"></param>
        /// <returns></returns>
        public DataTable GetForestBoundaries(string OfficeLevel)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FindForestBoundaries", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@officeLevel", OfficeLevel);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetForestBoundaries" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// function responsible for fetching Forest boundary master user wise
        /// </summary>
        /// <param name="OfficeLevel"></param>
        /// <returns></returns>
        public DataTable GetForestBoundariesUserWise(string OfficeLevel)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_OneClickAccessPermission", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetForestBoundaries");
                cmd.Parameters.AddWithValue("@ParentID", OfficeLevel);
                cmd.Parameters.AddWithValue("@LOGINUSERID", HttpContext.Current.Session["UserId"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetForestBoundaries" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// function responsible for fetching forest offices
        /// </summary>
        /// <param name="ForestCode"></param>
        /// <returns></returns>
        public DataTable getForestOffices(string ForestCode)
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GETFORESTOFFICES", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ForestCode", ForestCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "getForestOffices" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally { Conn.Close(); }
            return ds;
        }

        /// <summary>
        /// function responsible for fetching forest Employees
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <param name="PermissionId"></param>
        /// <param name="SubPermissionId"></param>
        /// <param name="officeCode"></param>
        /// <returns></returns>
        public DataTable getForestEmployees(int moduleId, int ServiceId, int PermissionId, int SubPermissionId, string officeCode)
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GETFORESTEMPLOYEES", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                cmd.Parameters.AddWithValue("@ServiceTypeId", ServiceId);
                cmd.Parameters.AddWithValue("@PermissionId", PermissionId);
                cmd.Parameters.AddWithValue("@SubPermissionId", SubPermissionId);
                cmd.Parameters.AddWithValue("@OFFICECODE", officeCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "getForestEmployees" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// Use for fetching Cast Details
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="StatementType"></param>
        /// <returns></returns>
        public DataTable GetCastDetails()
        {
            try
            {
                DALConn();
                DataTable ds = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetCast", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
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

        public DataTable GetPoliceStationDetails(string OffenseCode, string UserRole)
        {
            try
            {
                DALConn();
                DataTable ds = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetPoliceStation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@UserRole", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
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

        public DataTable GetFixedLandNOCPurpose()
        {
            try
            {
                DALConn();
                DataTable ds = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FETCH_NOCMASTER", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
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

        public DataTable GetFixedLandNOCNames(int NOCPurpose)
        {
            try
            {
                int EmitraServiceId = 0;
                if (HttpContext.Current.Session["EmitrServiceId"] == null || Convert.ToString(HttpContext.Current.Session["EmitrServiceId"]) == "")
                {
                    EmitraServiceId = 0;
                }
                else
                {
                    EmitraServiceId = Convert.ToInt32(HttpContext.Current.Session["EmitrServiceId"]);
                }
                DALConn();
                DataTable ds = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FETCH_NOCMASTER", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOCPurpose", NOCPurpose);
                cmd.Parameters.AddWithValue("@ServiceCode", EmitraServiceId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
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