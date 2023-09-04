//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Apply Auction
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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FMDSS.Models.CitizenService.ProductionServices
{
    public class Auction : DAL
    {
        #region data members
        public Int64 AuctionId { get; set; }
        public Int64 NoticeId { get; set; }
        public string RequestId { get; set; }
        public string NoticeNo { get; set; }

        public string Applicant_Type { get; set; }
        public string BidderName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DurationFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime DurationTo { get; set; }

        public string RegionCode { get; set; }
        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string DistrictCode { get; set; }
        public string RangeCode { get; set; }
        public string RangeName { get; set; }
        public string VillageCode { get; set; }

        public string VillageName { get; set; }
        public Int64 DepotId { get; set; }

        public string PlaceofAuction { get; set; }
        public string ForestProduce { get; set; }
        public string Qty { get; set; }
        public Decimal ReservedPrice { get; set; }
        public Decimal BiddingAmount { get; set; }
        public string DropOutReason { get; set; }

        public string Status { get; set; }

        public Int64 CreatedBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public int IsActive { get; set; }

        public string TransactionID { get; set; }
        public int Trn_Status_Code { get; set; }
        public string StockQuantity { get; set; }
        public string ProduceFor { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountToBePaid { get; set; }
        public decimal FinalAmount { get; set; }
        public string AvailStatus { get; set; }

        //Bank Details Member Data

        public string PaymentMode { get; set; }
        public string OfflinePaymentMode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }

        public string DdchkIssuesDate { get; set; }
        public string DdChkNumber { get; set; }
        public string DdchkFile { get; set; }
        public string DdchkFilepth { get; set; }
        public decimal EmdPaybleAmount { get; set; }
        public decimal PsPaybleAmount { get; set; }
        public string PaymentType { get; set; }

        public Int64 Bidder_ID { get; set; }


        public string ReceiptDoc { get; set; }

        public string ReceipDocpath { get; set; }

        public string DepotIncharge { get; set; }
        public string ProductUnit { get; set; }
        #endregion


        #region TenduPattaOnlineAuction

        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string BiddingPrice { get; set; }
        public decimal TotalReservePrice { get; set; }
        public DataTable Select_TenduPattaOnlineAuction(string Option, string Range = "")
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_TenduPattaOnlineAuctione_Getdropdown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Option", Option);
                cmd.Parameters.AddWithValue("@Range", Range);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitAuctionWinnerDetail" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }



        public DataTable Select_NoticeDetails(string NoticeId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_GetTanduPattaNoticeDetail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NoticeID", NoticeId);
                // cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitAuctionWinnerDetail" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public Int64 SubmitTenduPattaOnline(DataTable dt)
        {
            Int64 chId = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_InsertTenduPattaOnlineAuction", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@ApplicantType", Applicant_Type);
                cmd.Parameters.AddWithValue("@NoticeNumber", NoticeNo);
                cmd.Parameters.AddWithValue("@BidderName", BidderName);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@BiddingAmount", BiddingAmount);
                cmd.Parameters.AddWithValue("@P_PaymentMode", PaymentMode);
                cmd.Parameters.AddWithValue("@P_OfflinePaymentMode", OfflinePaymentMode);
                cmd.Parameters.AddWithValue("@P_BankName", BankName);
                cmd.Parameters.AddWithValue("@P_BranchName", BranchName);
                cmd.Parameters.AddWithValue("@P_DDChkIssueDate", DdchkIssuesDate);
                cmd.Parameters.AddWithValue("@P_DDChkNumber", DdChkNumber);
                cmd.Parameters.AddWithValue("@P_DDChkFileName", DdchkFile);
                cmd.Parameters.AddWithValue("@P_DdchkFilepth", DdchkFilepth);
                cmd.Parameters.AddWithValue("@P_EmdPaybleAmt", EmdPaybleAmount);
                //cmd.Parameters.AddWithValue("@P_PsPaybleAmt", PsPaybleAmount);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@TanduPattaOnline", dt);

                chId = Convert.ToInt64(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitAuction" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }


        #endregion

        #region Member Functions
        /// <summary>
        /// Function to Insert new Action Application data into database
        /// </summary>
        /// <returns></returns>
        public Int64 SubmitAuction()
        {
            Int64 chId = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Insert_Auction_Detail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_AuctionId", AuctionId);
                cmd.Parameters.AddWithValue("@P_NoticeId", NoticeId);
                cmd.Parameters.AddWithValue("@P_RequestId", RequestId);
                cmd.Parameters.AddWithValue("@P_Applicant_Type", Applicant_Type);
                cmd.Parameters.AddWithValue("@P_BidderName", BidderName);
                cmd.Parameters.AddWithValue("@P_BiddingAmount", BiddingAmount);
                cmd.Parameters.AddWithValue("@P_BidOpeningDate", DurationFrom);
                cmd.Parameters.AddWithValue("@P_BiddClosingDate", DurationTo);
                cmd.Parameters.AddWithValue("@P_PaymentMode", PaymentMode);
                cmd.Parameters.AddWithValue("@P_OfflinePaymentMode", OfflinePaymentMode);
                cmd.Parameters.AddWithValue("@P_BankName", BankName);
                cmd.Parameters.AddWithValue("@P_BranchName", BranchName);
                cmd.Parameters.AddWithValue("@P_DDChkIssueDate", DdchkIssuesDate);
                cmd.Parameters.AddWithValue("@P_DDChkNumber", DdChkNumber);
                cmd.Parameters.AddWithValue("@P_DDChkFileName", DdchkFile);
                cmd.Parameters.AddWithValue("@P_DdchkFilepth", DdchkFilepth);
                cmd.Parameters.AddWithValue("@P_EmdPaybleAmt", EmdPaybleAmount);
                cmd.Parameters.AddWithValue("@P_PsPaybleAmt", PsPaybleAmount);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);

                chId = Convert.ToInt64(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitAuction" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }

        public Int64 SubmitAuctionWinnerDetail(string auctionType, string action)
        {
            Int64 chId = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Insert_AuctionWinner_Detail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_AuctionwinId", AuctionId);
                cmd.Parameters.AddWithValue("@P_NoticeId", NoticeId);
                cmd.Parameters.AddWithValue("@P_BuyerID", Bidder_ID);
                cmd.Parameters.AddWithValue("@P_RequestId", RequestId);
                cmd.Parameters.AddWithValue("@P_BiddingAmount", BiddingAmount);
                cmd.Parameters.AddWithValue("@P_ReceiptDoc", ReceiptDoc);
                cmd.Parameters.AddWithValue("@P_ReceipDocpath", ReceipDocpath);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@AuctionType", auctionType);
                chId = Convert.ToInt64(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitAuctionWinnerDetail" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }


        public DataTable FetchWinners(string auctionType, string action)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Insert_AuctionWinner_Detail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_AuctionwinId", AuctionId);
                cmd.Parameters.AddWithValue("@P_NoticeId", NoticeId);
                cmd.Parameters.AddWithValue("@P_BuyerID", Bidder_ID);
                cmd.Parameters.AddWithValue("@P_RequestId", RequestId);
                cmd.Parameters.AddWithValue("@P_BiddingAmount", BiddingAmount);
                cmd.Parameters.AddWithValue("@P_ReceiptDoc", ReceiptDoc);
                cmd.Parameters.AddWithValue("@P_ReceipDocpath", ReceipDocpath);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@AuctionType", auctionType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitAuctionWinnerDetail" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public Int64 DropoutAuction()
        {
            Int64 chId = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Forest_Dropout_Auction_Detail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_AuctionId", AuctionId);
                cmd.Parameters.AddWithValue("@P_NoticeNo", NoticeNo);
                cmd.Parameters.AddWithValue("@P_Drop_Out_Reasons", DropOutReason);
                cmd.Parameters.AddWithValue("@P_UpdatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 0);

                chId = Convert.ToInt64(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "DropoutAuction" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }

        /// <summary>
        /// Encription of Bidder Name And Bidding Amount
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        public string Encrypt(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Decryption of Bidder Name And Bidding Amount
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Decrypt(string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        /// <summary>
        /// Function for fetching  Detail Information of Applyed Auction from database
        /// </summary>
        /// <returns></returns>
        public DataTable BindAuctionDetail(Int64 noticeId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDP_Select_Auction", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_NoticeId", noticeId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindAuctionDetail" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindAuctionWinner(Int64 userId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_AuctionWinner", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", NoticeId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindAuctionWinner" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable IsApplyedAuction(Int64 UserId, Int64 NoticeId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Citizen_Check_ApplyByUser", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_UserId", UserId);
                cmd.Parameters.AddWithValue("@P_NoticeId", NoticeId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "IsApplyedAuction" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }



        public void UpdateTransactionStatus()
        {
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {              
            
            new SqlParameter("@TransactionID",TransactionID),
            new SqlParameter("@TransactionStatus", Trn_Status_Code),
            new SqlParameter("@RequestID", RequestId),
            new SqlParameter("@userID", CreatedBy),
             new SqlParameter("@PaymentType", PaymentType)
            };
                Int32 chk = Convert.ToInt32(ExecuteNonQuery("sp_FDP_UpdateAuctionEmdStatus", parameters));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateTransactionStatus" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
        }


        #endregion




    }
}