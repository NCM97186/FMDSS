//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Depot Management
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Manoj Kumar
//  Modified By  : Manoj Kumar
//  Modified On  : 06-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@



using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace FMDSS.Models.ForestProduction
{
    public class DepotManagement :DAL
    {
        #region Global Variables

        public Int64 RowID { get; set; }
        public Int64 DepotId { get; set; }
        public Int64 UserID { get; set; }
        public string DepotType { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string CircleCode { get; set; }
        public string CircleName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string RangeCode { get; set; }
        public string RangeName { get; set; }
        public string DepotName { get; set; }
        public string DepotIncharge { get; set; }
        public string DesignationID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; } 
        public string EnteredOn { get; set; }
        public Int64 EnteredBy { get; set; }
        public int IsActive { get; set; }
        public int Status { get; set; }
        public bool IsRead { get; set; }
        public Int64 UpdatedBy { get; set; }
        #endregion

        /// <summary>
        /// Used for save Depot detail into database
        /// </summary>
        /// <returns></returns>
        public Int64 AddDepotData(string option)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Admin_Insert_Depot", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ID", DepotId);
                cmd.Parameters.AddWithValue("@P_DepotType", DepotType);
                cmd.Parameters.AddWithValue("@P_RegionCode", RegionCode);
                cmd.Parameters.AddWithValue("@P_CircleCode", CircleCode);
                cmd.Parameters.AddWithValue("@P_DivisionCode", DivisionCode);
                cmd.Parameters.AddWithValue("@P_RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@P_DepotName", DepotName);
                cmd.Parameters.AddWithValue("@P_DepotIncharge", DepotIncharge);
                cmd.Parameters.AddWithValue("@DesignationID", DesignationID);
                cmd.Parameters.AddWithValue("@Latitude", Latitude);
                cmd.Parameters.AddWithValue("@Longitude", Longitude);
                cmd.Parameters.AddWithValue("@P_EnteredBy", EnteredBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                cmd.Parameters.AddWithValue("@option", option);
                  chk = Convert.ToInt64(cmd.ExecuteScalar());
           
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "AddDepotData" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }

        /// <summary>
        /// Select All depot details from databse
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Depot(bool IsDepotAdmin = false)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
           
                SqlCommand cmd = new SqlCommand("Sp_FDP_Select_Depot", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@IsDepotAdmin", IsDepotAdmin);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Depot" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// select range designation rangeID
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable BindRangeWiseDesignation(string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Select_Designation_By_Range", Conn);
                cmd.Parameters.AddWithValue("@rangeCode", rangeCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "BindRangeWiseDesignation" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
           
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        
        /// <summary>
        /// Select Range employee by designation
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable EmpByRangeDesignation(string rangeCode, string desigID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Select_RangeEmp_by_Designation", Conn);
                cmd.Parameters.AddWithValue("@rangeCode", rangeCode);
                cmd.Parameters.AddWithValue("@desigID", desigID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "EmpByRangeDesignation" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
           
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }  

        /// <summary>
        /// Function for fetching Depot details by Depot ID from database
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable Select_DepotByID(string DepotID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDP_Select_Depot_byID", Conn);
                cmd.Parameters.AddWithValue("@DepotId", Convert.ToInt64(DepotID));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_DepotByID" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }      

        /// <summary>
        /// Delete depot from database by ID
        /// </summary>
        /// <returns></returns>
        public Int64 DeleteDepot()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDP_delete_Depot", Conn);
                cmd.Parameters.AddWithValue("@DepotId", DepotId);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                 chk = Convert.ToInt64(cmd.ExecuteScalar());
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "DeleteDepot" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
      
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
    }
}