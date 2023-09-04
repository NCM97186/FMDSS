//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Publish Notice Number
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
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Admin
{
    public class Notice:DAL
    {
        #region data members
        public Int64 NoticeId { get; set; }
        public string NoticeNo { get; set; }
        public string RegionCode { get; set; }

        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string RangeCode { get; set; }
        public string VillageCode { get; set; }
        public Int64 DepotId { get; set; }
        public string ForestProduce { get; set; }
        public string Qty { get; set; }
        public Decimal ReservedPrice { get; set; }
        public Int64 CreatedBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public int IsActive { get; set; }


        #endregion

        #region Member Functions
        /// <summary>
        /// Function to Create new Notice Number into database
        /// </summary>
        /// <returns></returns>
        public Int64 CreateNotice()
        {
            Int64 noticeNo = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Insert_Notice_Number", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_NoticeId", NoticeId);
                cmd.Parameters.AddWithValue("@P_NoticeNo", NoticeNo);
                cmd.Parameters.AddWithValue("@P_RegionCode", RegionCode);
                cmd.Parameters.AddWithValue("@P_CircleCode", CircleCode);
                cmd.Parameters.AddWithValue("@P_DivisionCode", DivisionCode);
                cmd.Parameters.AddWithValue("@P_RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@P_VillageCode", VillageCode);
                cmd.Parameters.AddWithValue("@P_DepotId", DepotId);
                cmd.Parameters.AddWithValue("@P_ForestProduce", ForestProduce);
                cmd.Parameters.AddWithValue("@P_Qty", Qty);
                cmd.Parameters.AddWithValue("@P_ReservedPrice", ReservedPrice);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                  noticeNo = Convert.ToInt64(cmd.ExecuteScalar());
                
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "CreateNotice" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return noticeNo;
        }

        /// <summary>
        /// Function for fetching  Detail Information of Particular Notice No. from database
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public DataTable BindNoticeNo(Int64 noticeId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
              
                SqlCommand cmd = new SqlCommand("Sp_Select_Notice_Detail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_noticeId", noticeId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindNoticeNo" + "_" + "(byID)Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindNoticeNo()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Notice_Number", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindNoticeNo" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }




        #endregion
    }
}