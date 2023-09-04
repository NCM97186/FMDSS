//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Apply Research Permission
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class Depot:DAL
    {
        #region Global Variables
        public Int64 DepotId { get; set; }

        public string DepotType { get; set; }
        public string RegionCode { get; set; }
        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string RangeCode { get; set; }
        public string DepotName { get; set; }
        public string DepotIncharge { get; set; }
        public string EnteredOn { get; set; }
        public Int64 EnteredBy { get; set; }
        public int IsActive { get; set; }
        public int Status { get; set; }
        public bool IsRead { get; set; }

        #endregion

        /// <summary>
        /// Used for save Depot detail into database
        /// </summary>
        /// <returns></returns>
        public Int64 AddDepotData()
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
                cmd.Parameters.AddWithValue("@P_EnteredBy", EnteredBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                  chk = Convert.ToInt64(cmd.ExecuteScalar());
           
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "AddDepotData" + "_" + "production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
    }
}