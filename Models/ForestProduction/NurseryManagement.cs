//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : Manage Nursery and Forest Produce Model
//  Description  : File contains functions For Business Rules and DB
//  Date Created : 30-Dec-2015
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

namespace FMDSS.Models.ForestProduction
{
    [Serializable]
    public class CitizenOrDeptModel
    {
        #region Citizen Or Dept User

        public int ID { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        #endregion
    }

    [Serializable]
    public class NurseryManagement : DAL
    {
        #region Data Members
        public string ddlDistricts { get; set; }
        public string districtName { get; set; }
        public string ddlBlocks { get; set; }
        public string blkName { get; set; }
        public string ddlGPs { get; set; }
        public string gpName { get; set; }
        public string ddlVillages { get; set; }
        public string villName { get; set; }
        public string address { get; set; }
        public string landmark { get; set; }
        public string nurseryCode { get; set; }
        public string nurseryName { get; set; }
        public string product { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string nurseryType { get; set; }
        public string statusid { get; set; }
        public string statusDesc { get; set; }
        public int nurseryNumber { get; set; }
        public string ddlRange { get; set; }
        public string RangeName { get; set; }
        public string produceQty { get; set; }
        public string d_produceQty { get; set; }
        public string pur_produceQty { get; set; }
        public string NurseryInchargeSSOID { get; set; }
        public bool ActiveStatus { get; set; }
        public int IsCitizenOrDeptEndOpenNusery { get; set; }

        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }

        public List<CitizenOrDeptModel> ActiveStatusCitizenANDDeptUser { get; set; }
        #endregion



        #region Member Functions
        /// <summary>
        /// function responsible for add/edit nursery details
        /// </summary>
        /// <param name="actionFlag"></param>
        /// <returns>scope identity returned from DB</returns>
        public Int64 InsertUpdateNursery(string actionFlag, Boolean isActive = true)
        {
            Int64 result = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_ADD_EDIT_NURSERY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistCode", ddlDistricts);
                cmd.Parameters.AddWithValue("@BlockCode", ddlBlocks);
                cmd.Parameters.AddWithValue("@GPCode", ddlGPs);
                cmd.Parameters.AddWithValue("@RANGECODE", ddlRange);
                cmd.Parameters.AddWithValue("@VillageCode", ddlVillages);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Landmark", landmark);
                cmd.Parameters.AddWithValue("@NurseryCode", nurseryCode);
                cmd.Parameters.AddWithValue("@NurseryName", nurseryName);
                cmd.Parameters.AddWithValue("@NurseryType", nurseryType);
                cmd.Parameters.AddWithValue("@Latitude", latitude);
                cmd.Parameters.AddWithValue("@Longitude", longitude);
                cmd.Parameters.AddWithValue("@UpdatedBy", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.Parameters.AddWithValue("@ActiveStatus", isActive);
                cmd.Parameters.AddWithValue("@NurseryInchargeSSOID", NurseryInchargeSSOID);
                cmd.Parameters.AddWithValue("@IsCitizenOrDeptEndOpenNusery", IsCitizenOrDeptEndOpenNusery);

                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertUpdateNursery" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return result;
        }
        /// <summary>
        /// Function responsible to fetch all nurseries created by Logged in Person
        /// </summary>
        /// <param name="nurseryCode"></param>
        /// <returns>datatable for all Nursery result</returns>
        public DataTable FetchNurseries(string nurseryCode = null)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_ADD_EDIT_NURSERY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistCode", ddlDistricts);
                cmd.Parameters.AddWithValue("@BlockCode", ddlBlocks);
                cmd.Parameters.AddWithValue("@GPCode", ddlGPs);
                cmd.Parameters.AddWithValue("@VillageCode", ddlVillages);
                cmd.Parameters.AddWithValue("@NurseryCode", nurseryCode);
                cmd.Parameters.AddWithValue("@NurseryName", nurseryName);
                cmd.Parameters.AddWithValue("@NurseryType", nurseryType);
                cmd.Parameters.AddWithValue("@ActiveStatus", ActiveStatus);
                cmd.Parameters.AddWithValue("@UpdatedBy", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@ActionFlag", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "FetchNurseries" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable Select_NURSERY_LEVELWISE(Int64 UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@UserID", UserID) };
                Fill(dt, "SP_GETNURSERYFORESTLEVELWISE", parameters);
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
        /// Returns Division Code By Range Code 
        /// </summary>
        /// <param name="rangeCode">Represents Range Code</param>
        /// <returns></returns>
        public DataTable GetDivisionByRangeCode(string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters = { new SqlParameter("@RANGECODE", rangeCode) };
                Fill(dt, "SP_COMMON_SELECT_DIVISIONCODE_BY_RANGE", parameters);
                return dt;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return dt;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Get Circle Code By Range Code
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable GetCircleByRangeCode(string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters = { new SqlParameter("@RANGECODE", rangeCode) };
                Fill(dt, "SP_COMMON_SELECT_CIRCLECODE_BY_RANGE", parameters);
                return dt;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return dt;
            }
            finally
            {
                Conn.Close();
            }
        }




        #endregion
    }
}