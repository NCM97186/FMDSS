using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    [Serializable]//add by rajveer load balanceing
    public class AuctionRevenueMaster : DAL
    {
        #region data members
        public Int64 RowID { get; set; }
        public string RangeCode { get; set; }
        public string RangeName { get; set; }
        public Int64 DepotId { get; set; }
        public string DepotName { get; set; }
        public Int64 ForestProduceID { get; set; }
        public string ForestProducename { get; set; }
        public Int64 ForestProductID { get; set; }
        public string ForestProductName { get; set; }
        public string Qty { get; set; }
        public string RevenueYear { get; set; }
        public string RevinueAmount { get; set; }
        public decimal ProductRate { get; set; }
        public string No_Status { get; set; }
        public string ProduceUnit { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public int IsActive { get; set; }
        public Int64 EnteredBy { get; set; }

        public string DocMOM { get; set; }
        public string DocMOMfilepath { get; set; }
        public DateTime MeatingDate { get; set; }
        public string MeatingDates { get; set; }
        public string CollectionRate { get; set; }

        #endregion

        #region Member Functions

        /// <summary>
        /// function responsible for add/edit nursery details
        /// </summary>
        /// <param name="actionFlag"></param>
        /// <returns>scope identity returned from DB</returns>
        public Int64 InsertUpdateAucRevenueDetail(string actionFlag)
        {

            Int64 result = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_ADD_EDIT_AUCTION_REVENUE", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AuctionRevenueID", RowID);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@DepotId", DepotId);
                cmd.Parameters.AddWithValue("@ForestProduceID", ForestProduceID);
                cmd.Parameters.AddWithValue("@ForestProductID", ForestProductID);
                cmd.Parameters.AddWithValue("@ProduceUnit", ProduceUnit);
                cmd.Parameters.AddWithValue("@Qty", Qty);
                cmd.Parameters.AddWithValue("@RevenueYear", RevenueYear);
                cmd.Parameters.AddWithValue("@RevinueAmount", RevinueAmount);
                cmd.Parameters.AddWithValue("@UpdatedBy", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "InsertUpdateAucRevenueDetail" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return result;
        }

        /// <summary>
        /// function responsible for add/edit nursery details
        /// </summary>
        /// <param name="actionFlag"></param>
        /// <returns>scope identity returned from DB</returns>
        public Int64 InsertUpdateCollectionRate(string actionFlag)
        {

            Int64 result = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_ADD_EDIT_Meating_CollectionRate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MeatingCollectionID", RowID);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@DepotId", DepotId);
                cmd.Parameters.AddWithValue("@ForestProduceID", ForestProduceID);
                cmd.Parameters.AddWithValue("@ForestProductID", ForestProductID);
                cmd.Parameters.AddWithValue("@ProduceUnit", ProduceUnit);
                cmd.Parameters.AddWithValue("@CollectionRate", CollectionRate);
                cmd.Parameters.AddWithValue("@MeatingDate", MeatingDate);
                cmd.Parameters.AddWithValue("@DocMOMfilepath", DocMOMfilepath);
                cmd.Parameters.AddWithValue("@DocMOM", DocMOM);
                cmd.Parameters.AddWithValue("@UpdatedBy", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "InsertUpdateCollectionRate" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionFlag"></param>
        /// <returns></returns>
        public DataTable Select_AuctionRevenue(Int64 id,  string actionFlag, Int64 enteredBy)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Master_Select_Auction_Revenue", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AuctionRevenueID", id);
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.Parameters.AddWithValue("@UserId", enteredBy);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "InsertUpdateCollectionRate" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionFlag"></param>
        /// <returns></returns>
        public DataTable Select_MeationgCollectionRate(Int64 collectionID, string actionFlag)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Master_Select_CollectionRate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MeatingCollectionID", collectionID);
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "InsertUpdateCollectionRate" + "_" + "Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
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


        #endregion


    }
}