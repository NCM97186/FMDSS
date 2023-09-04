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


using FMDSS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForestProduction
{
    public class NoticeManagement : DAL
    {
        #region Properties & Variables
        private FMDSS.Models.DAL _db = new Models.DAL();
        #endregion
        #region data members
        public Int64 NoticeId { get; set; }

        public Int64 SchedulerId { get; set; }
        public string SchedulerType { get; set; }
        public Int64 RowID { get; set; }
        public int ID { get; set; }
        public string NoticeNo { get; set; }
        public string RequestId { get; set; }
        public string SchedulerPeriod { get; set; }
        public string EmdAmount { get; set; }
        public string PaidAmount { get; set; }
        public string Description { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string CircleCode { get; set; }
        public string CircleName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string DistrictCode { get; set; }
        public string RangeCode { get; set; }
        public string Rangename { get; set; }
        public string VillageCode { get; set; }
        public Int64 DepotId { get; set; }
        public string DepotName { get; set; }
        public string ForestProduce { get; set; }
        public string ForestProduct { get; set; }

        public string BiddOpeningDate { get; set; }
        public string BidClosingDate { get; set; }
        public string ForestProduceName { get; set; }
        public Int64 ForestProduceID { get; set; }
        public Int64 ForestProductID { get; set; }
        public string Qty { get; set; }
        public decimal ProductRate { get; set; }
        public string No_Status { get; set; }
        public string ProduceUnit { get; set; }

        [DataType(DataType.Date)]
        public DateTime DurationFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime DurationTo { get; set; }
        public Decimal ReservedPrice { get; set; }
        public Int64 CreatedBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public int IsActive { get; set; }

        public string Durations { get; set; }

        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string BidderName { get; set; }
        public string BiddingAmount { get; set; }
        public string ReqAction { get; set; }

        public string ActionFlag { get; set; }
        public string NoticeView { get; set; }

        public string prodName { get; set; }
        public string DepName { get; set; }
        public string DtYear { get; set; }
        public string DurationFrm { get; set; }
        public string DurationT { get; set; }
        public string ViNot { get; set; }
        public string Time { get; set; }
        public string Mobile { get; set; }
        public string FinalDate { get; set; }
        public string SchedulerNo { get; set; }
        public string ScheduleDay { get; set; }
        public string ScheduleMonth { get; set; }
        public string Scheduleyear { get; set; }
        public string ScheduleTime { get; set; }
        public string Mobileno { get; set; }

        public Int64 SiteID { get; set; }
        public string SiteName { get; set; }
        public string Villagename { get; set; }

        public string SitePrice { get; set; }
        public bool IsCheckedSite { get; set; }
        public string InventoryID { get; set; }
        public string InventoryLotNumber { get; set; }
        public virtual List<ForestDevelopment.DODProductDetails> DODProductList { get; set; }
        #endregion

        #region Member Functions
        /// <summary>
        /// Function to Create new Notice Number into database
        /// </summary>
        /// <returns></returns>
        public string CreateNotice(NoticeManagement notice = null)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Insert_Notice_Number", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ActionFlag", ActionFlag);
                cmd.Parameters.AddWithValue("@P_NoticeId", NoticeId);
                cmd.Parameters.AddWithValue("@P_NoticeNo", RequestId);
                cmd.Parameters.AddWithValue("@P_RequestId", RequestId);
                cmd.Parameters.AddWithValue("@P_RegionCode", RegionCode == null ? (object)DBNull.Value : RegionCode);
                cmd.Parameters.AddWithValue("@P_CircleCode", CircleCode == null ? (object)DBNull.Value : CircleCode);
                cmd.Parameters.AddWithValue("@P_DivisionCode", DivisionCode == null ? (object)DBNull.Value : DivisionCode);
                cmd.Parameters.AddWithValue("@P_RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@P_VillageCode", "");
                cmd.Parameters.AddWithValue("@InventoryID", InventoryID);
                cmd.Parameters.AddWithValue("@P_DepotId", DepotId);
                cmd.Parameters.AddWithValue("@P_ForestProduce", ForestProduceID);
                cmd.Parameters.AddWithValue("@P_ForestProductID", ForestProductID);
                cmd.Parameters.AddWithValue("@P_Qty", Qty);
                cmd.Parameters.AddWithValue("@P_Unit", ProduceUnit);
                cmd.Parameters.AddWithValue("@P_DurationFrom", Globals.Util.GetDate(Convert.ToString(DurationFrom)));
                cmd.Parameters.AddWithValue("@P_DurationTo", Globals.Util.GetDate(Convert.ToString(DurationTo)));
                cmd.Parameters.AddWithValue("@P_ReservedPrice", ReservedPrice);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                cmd.Parameters.AddWithValue("@auctionDate", FinalDate == null ? (object)DBNull.Value : FinalDate);
                cmd.Parameters.AddWithValue("@schedulerType", SchedulerType == null ? (object)DBNull.Value : SchedulerType);
                cmd.Parameters.AddWithValue("@xmlFile", new FMDSS.Models.ForestDevelopment.TransitPermit().GetRequestInXML(notice));

                string noticeNo = cmd.ExecuteScalar().ToString();
                return noticeNo;
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

        public string SaveNotice(NoticeDetails model)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Insert_Notice_Number", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ActionFlag", "INSERT");
                cmd.Parameters.AddWithValue("@P_RangeCode", model.RangeCode);
                cmd.Parameters.AddWithValue("@InventoryID", model.InventoryID);
                cmd.Parameters.AddWithValue("@P_DepotId", model.DepotId);
                cmd.Parameters.AddWithValue("@P_DurationFrom", Globals.Util.GetDate(model.DurationFrom));
                cmd.Parameters.AddWithValue("@P_DurationTo", Globals.Util.GetDate(model.DurationTo));
                cmd.Parameters.AddWithValue("@P_CreatedBy", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                cmd.Parameters.AddWithValue("@xmlFile", new FMDSS.Models.ForestDevelopment.TransitPermit().GetRequestInXML(model));

                string noticeNo = cmd.ExecuteScalar().ToString();
                return noticeNo;
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
        /// Function to Create new Tendupatta Notice Number into database
        /// </summary>
        /// <returns></returns>
        public string CreateNotice(DataTable siteDetail)
        {
            try
            {


                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPD_Insert_Tendupatta", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_NoticeId", NoticeId);
                cmd.Parameters.AddWithValue("@P_RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@P_DurationFrom", DurationFrom);
                cmd.Parameters.AddWithValue("@P_DurationTo", DurationTo);
                cmd.Parameters.AddWithValue("@P_ReservedPrice", ReservedPrice);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_ActionFlag", ActionFlag);
                cmd.Parameters.AddWithValue("@SiteDetails", siteDetail);

                string noticeNo = cmd.ExecuteScalar().ToString();
                return noticeNo;

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

        public string AddAuctionSchedularDetails(DataTable dt, DataTable dt2)
        {
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPD_Insert_Auction_Schedulars", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SchedulerPeriod", SchedulerPeriod == null ? (object)DBNull.Value : SchedulerPeriod);
                cmd.Parameters.AddWithValue("@EmdAmt", EmdAmount == null ? (object)DBNull.Value : EmdAmount);
                cmd.Parameters.AddWithValue("@Description", Description == null ? (object)DBNull.Value : Description);
                cmd.Parameters.AddWithValue("@EnteredBy", CreatedBy == null ? (object)DBNull.Value : CreatedBy);
                cmd.Parameters.AddWithValue("@ProductSchedularDetail", dt);

                cmd.Parameters.AddWithValue("@DepotSchedularDetail", dt2);
                //cmd.Parameters.AddWithValue("@option", option);
                string chk = cmd.ExecuteScalar().ToString();
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

        public DataTable PublishNoticeview(Int64 NoticeId, string actionFlag)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_Notice_Format", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_NoticeId", NoticeId);
                cmd.Parameters.AddWithValue("@P_actionFlag", actionFlag);
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


        public DataTable AmmedmentNoticeview(Int64 NoticeId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FETCHNOTICEAMENDMENTS", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOTICENO", NoticeId);

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
        /// find site detail by range code
        /// </summary>
        /// <param name="NoticeId"></param>
        /// <returns></returns>
        public DataTable GetSiteDetail(string rangeCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FETCHDENDUPATTANOTICE", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rangeCode", rangeCode);

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


        public DataTable Select_AuctionScheduler()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forester_Select_Notice_Scheduler", Conn);
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

        public DataSet BindScheduler(Int64 depotID)
        {
            try
            {
                DALConn();
                DataSet dsDepotSchedulers = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Select_Notice_Scheduler", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_depotID", depotID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsDepotSchedulers);
                return dsDepotSchedulers;
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


        public DataTable BindDateScheduler(Int64 SID, Int64 depotID)
        {
            try
            {
                DALConn();
                DataTable dsDepotSchedulers = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GETAUCTIONDATES_DEPOTWISE", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AUCTNSCHEDULRID", SID);
                cmd.Parameters.AddWithValue("@DEPOTID", depotID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsDepotSchedulers);
                return dsDepotSchedulers;
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



        public DataTable GetContactBYDepotID(Int64 depotID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_UserContactBYDepotID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_depotID", depotID);
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



        public DataTable BindDropdownNoticeNotice(string auctionType, string rangeCode, Int64 depotId, Int64 producetype, Int64 product, bool isAuctionClosed = false)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_Notice_Number_Detailview", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AUCTIONTYPE", auctionType);
                cmd.Parameters.AddWithValue("@rangeCode", rangeCode);
                cmd.Parameters.AddWithValue("@depotId", depotId);
                cmd.Parameters.AddWithValue("@producetype", producetype);
                cmd.Parameters.AddWithValue("@product", product);
                cmd.Parameters.AddWithValue("@AUCTIONCLOSED", isAuctionClosed);
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
        /// Bind publish notice for tenduupatta and Timber
        /// </summary>
        /// <param name="auctionType"></param>
        /// <returns></returns>
        public DataTable BindPublishedNoticeNo(string auctionType = null)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_SELECT_NOTICE_NUMBER", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AUCTIONTYPE", auctionType);
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
        /// 
        /// </summary>
        /// <param name="auctionType"></param>
        /// <returns></returns>
        public DataTable BindDropdownNoticeNo(string auctionType, Int64 Userid, bool isAuctionClosed = false)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_Notice_Number", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AUCTIONTYPE", auctionType);
                cmd.Parameters.AddWithValue("@AUCTIONCLOSED", isAuctionClosed);
                cmd.Parameters.AddWithValue("@UserId", Userid);
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



        public DataTable BindBidder(string registrationFor)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_Bidder", Conn);
                cmd.Parameters.AddWithValue("@RegistrationFor", registrationFor);
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

        public DataTable BindProductType()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Comman_Select_auproduceType", Conn);
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

        public DataTable BindPublishednoticealert(Int64 userID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FPD_Select_Published_Notice", Conn);
                cmd.Parameters.AddWithValue("@userID", userID);

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

        public DataTable BindForestProduce()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Comman_Select_produceType", Conn);
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

        public ResponseMsg RemoveNotice(Int64 noticeId)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("ParentID", noticeId), 
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_DOD_Insert_Notice", prms);

            if (Globals.Util.isValidDataTable(dtData, true))
            {
                msg = Globals.Util.GetListFromTable<ResponseMsg>(dtData).FirstOrDefault();
            }

            return msg;
        }

        public string DeactivateNoticeNo(Int64 noticeId, Int64 updatedBy, string ActionFlag)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDP_delete_Notice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_NoticeId", noticeId);
                cmd.Parameters.AddWithValue("@P_UpdatedBy", updatedBy);
                cmd.Parameters.AddWithValue("@P_ActionFlag", ActionFlag);
                string noticeNo = cmd.ExecuteScalar().ToString();
                return noticeNo;

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
        /// Function for fetching  Detail Information of Particular Notice No. from database
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public DataTable BindNoticeData(string ActionFlag)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_Notice_Detail", Conn);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@P_ActionFlag", ActionFlag);
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
        /// Function for fetching  Detail Information of Particular Notice No. from database
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public DataTable BindNoticeNo(Int64 noticeId, string action)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_Notice_Detail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_noticeId", noticeId);
                cmd.Parameters.AddWithValue("@P_ActionFlag", action);
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

        public DataSet GetNoticeDetails(Int64 noticeId, string actionCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDetailsByNoticeID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ParentID", noticeId);
                cmd.Parameters.AddWithValue("@ActionCode", actionCode);
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

        /// <summary>
        /// Function for fetching  Detail Information of Particular Notice No. from database
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public DataTable LastThreeYearReveneu(string rangeCode, Int64 depotId, Int64 producetype, Int64 product)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_Revenue_Detail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rangeCode", rangeCode);
                cmd.Parameters.AddWithValue("@depotId", depotId);
                cmd.Parameters.AddWithValue("@producetype", producetype);
                cmd.Parameters.AddWithValue("@product", product);
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

        public DataSet GetSchedularDetails(Int64 SchedulerID)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Select_SchedularDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SchedulerID", SchedulerID);
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


        /// <summary>
        /// Function for fetching  Detail Information of Particular Notice No. from database
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public DataTable BindProducestock(Int64 depotId, Int64 produceId, Int64 productId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_StockByDepot", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_depotId", depotId);
                cmd.Parameters.AddWithValue("@P_produceId", produceId);
                cmd.Parameters.AddWithValue("@P_productId", productId);
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

        public DataTable BindTP(Int64 depotID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetTPList_Notice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@DepotID", depotID);
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
        /// Function for fetching  Detail Information of Particular Notice No. from database
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public DataTable BindProductUnitRate(Int64 productId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_UnitRate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_productId", productId);
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

        public DataTable BindForestProduct(Int64 depotId, Int64 forestProduceTypeID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_Product", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_depotId", depotId);
                cmd.Parameters.AddWithValue("@P_forestProduceTypeID", forestProduceTypeID);
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

        public DataTable BindForestProductforauction(Int64 depotId, Int64 forestproductId, string reqType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_Product_Auction", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@depotId", depotId);
                cmd.Parameters.AddWithValue("@P_forestproductId", forestproductId);
                cmd.Parameters.AddWithValue("@AUCTIONTYPE", reqType);
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



        public DataTable BindProduct(Int64 forestproductId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_auProduct", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_forestproductId", forestproductId);
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


        public DataTable BindProductdeletee(Int64 forestproductId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_auProduct_Delete", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_forestproductId", forestproductId);
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
        public DataTable BindProduceforauction(Int64 depotId, string reqType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_Produce_Auction", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@depotId", depotId);
                cmd.Parameters.AddWithValue("@AUCTIONTYPE", reqType);
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

        public DataTable BindProduce(Int64 depotId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_Produce", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_depotId", depotId);
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

        public DataTable BindProducetp(Int64 depotId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Select_ProduceDepotIncharge", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_depotId", depotId);
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



        public DataTable BindNoticeNo()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_Notice_Number", Conn);
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

        public DataTable GetDetailsByTP(Int64 tpID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDetailsByTPID", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                cmd.Parameters.AddWithValue("@TPID", tpID);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
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
        #endregion
    }

    public class NoticeDetails
    {
        [Required]
        [Display(Name = "Range Name")]
        public string RangeCode { get; set; }
        [Required]
        [Display(Name = "Place/Depot of Auction")]
        public string DepotId { get; set; }
        [Required]
        [Display(Name = "Inventory Lot Number")]
        public string InventoryID { get; set; }
        [Required]
        [Display(Name = "Start Date of EMD")]
        public string DurationFrom { get; set; }
        [Required]
        [Display(Name = "Date of Auction")]
        public string DurationTo { get; set; }
        [Required]
        [Display(Name = "Product Type")]
        public string ProductTypeID { get; set; }
        [Required]
        [Display(Name = "Product")]
        public string ProductID { get; set; }
        public virtual List<ForestDevelopment.DODProductDetails> DODProductList { get; set; }
    }
}