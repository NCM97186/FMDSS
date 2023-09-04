//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Asset Management
//  Date Created : 06-Jan-2016
//  History      :
//  Version      : 1.0
//  Author       : Manoj Kumar
//  Modified By  : Manoj Kumar
//  Modified On  : 06-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForestProduction
{
    public class AssetManagement:DAL
    {
        #region Global Variables
        public Int64 RowID { get; set; }
        public Int64 AssetID { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string BlockCode { get; set; }
        public string BlockName { get; set; }
        public string PanchayatCode { get; set; }
        public string PanchayatName { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public string JfmcID { get; set; }
        public string JfmcName { get; set; }
        public string JFMCAgencyType { get; set; }
        public string PlanID { get; set; }
        public string PlanName { get; set; }
        public string AssetName { get; set; }
        public string AssetCategoryName { get; set; }
        public string AssetCategoryID { get; set; }

        public Int64 AssetCatID { get; set; }
        public int TotalAsset { get; set; }
        public Int64 EnteredBy { get; set; }
        public Int64 UpdatedBy { get; set; }

        public Int64 WorkOrder { set; get; }

        public string WorkOrder_Code { set; get; }
        #endregion


        /// <summary>
        /// Function for fetching  Asset Category from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable BindAssetCategory()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_AssetCategory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
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

        /// <summary>
        /// Used for save Asset detail in database
        /// </summary>
        /// <returns></returns>
        public Int64 AddAsset(string option)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Insert_Update_Asset", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AssetID", AssetID == null ? (object)DBNull.Value : AssetID);
                cmd.Parameters.AddWithValue("@DistrictCode", DistrictCode == null ? (object)DBNull.Value : DistrictCode);
                cmd.Parameters.AddWithValue("@BlockCode", BlockCode == null ? (object)DBNull.Value : BlockCode);
                cmd.Parameters.AddWithValue("@PanchayatCode", PanchayatCode == null ? (object)DBNull.Value : PanchayatCode);
                cmd.Parameters.AddWithValue("@VillageCode", VillageCode == null ? (object)DBNull.Value : VillageCode);
                cmd.Parameters.AddWithValue("@JfmcID", JfmcID == null ? (object)DBNull.Value : JfmcID);
                cmd.Parameters.AddWithValue("@PlanID", PlanID == null ? (object)DBNull.Value : PlanID);
                cmd.Parameters.AddWithValue("@AssetName", AssetName == null ? (object)DBNull.Value :Convert.ToInt64(AssetName));
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrder == null ? (object)DBNull.Value : WorkOrder);
                cmd.Parameters.AddWithValue("@AssetCategoryID", AssetCategoryID == null ? (object)DBNull.Value : AssetCategoryID);
                cmd.Parameters.AddWithValue("@TotalAsset", TotalAsset == null ? (object)DBNull.Value : TotalAsset);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy == null ? (object)DBNull.Value : EnteredBy);
                cmd.Parameters.AddWithValue("@option", option);
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
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
        /// <summary>
        /// Function is used to Select all asset
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Asset()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FDP_Select_Asset", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
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

        /// <summary>
        /// Function for fetching  Asset details from database by ID
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable Select_AssetByID(string assetID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FDP_Select_Asset_byID", Conn);
                cmd.Parameters.AddWithValue("@assetID", Convert.ToInt64(assetID));
                cmd.CommandType = CommandType.StoredProcedure;
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

        /// <summary>
        /// Function is used to delete asset by ID
        /// </summary>
        /// <returns></returns>
        public Int64 DeleteAsset()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDP_delete_Asset", Conn);
                cmd.Parameters.AddWithValue("@AssetID", AssetID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
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

        public DataTable Select_AssetDetail(Int64 workOrderID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FPD_Select_AssetByworkorderID", Conn);
                cmd.Parameters.AddWithValue("@workOrderID", Convert.ToInt64(workOrderID));
                cmd.CommandType = CommandType.StoredProcedure;
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
    }
}