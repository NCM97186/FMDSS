using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace FMDSS.Models.ForestProtection
{
    public class ForestReleases :DAL
    {
        #region Global Variable
        public string OffenceCategory { get; set; }
        public string RequestId { get; set; }
        public string OffenseSubCategoryWildLife { get; set; }
        public Int64 UserID { get; set; }
        public string EmitraTransactionID { get; set; }
        public string trn_Status_Code { get; set; }
        public string CompAmount { get; set; }
        public string OffenseCode { get; set; }
        public string FirstOfficerID { get; set; }
        public string SecondOfficerID { get; set; }
        public string Total { get; set; }
        public string SSOID { get; set; }
        public string Status { get; set; }
        public string StatusDesc { get; set; }
        public string ArticalName { get; set; }
        public string ArticalDetail { get; set; }
        public string Quantity { get; set; }

        public long ID { get; set; }
        public bool chkStatus { get; set; }
        public string IsCompoundable { get; set; }
        public string TableName { get; set; }
        public string SettlementAmount { get; set; }
        public string AmountPaid { get; set; }
        public string CaseStatus { get; set; }
        public string FineAmount { get; set; }
        public string DfoDecision { get; set; }
        public string TransactionId { get; set; }
        public int Trn_Status_Code { get; set; }
        public string AppOfcompounding { get; set; }
        public string ReceiptOfAmount { get; set; }
        public string AppOfSeized { get; set; }
        public string DocOfownership { get; set; }
        public string fileAppCompound { get; set; }

        #endregion

        #region Member Functions

        /// <summary>
        /// To get dashboard reuest details
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ActionID"></param>
        /// <returns></returns>
        public DataSet Get_SizedIteamList(String SSOID)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_SizedIteamDetails", Conn);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.Parameters.AddWithValue("@Status", 1);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OffenceCode"></param>
        /// <returns></returns>
        public DataSet Get_SizedIteamDetails(string OffenceCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_SizedIteamDetails", Conn);
                cmd.Parameters.AddWithValue("@OffenceCode", OffenceCode);
                cmd.Parameters.AddWithValue("@Status", 2);
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

        /// <summary>
        /// to insert the seized iteam details.
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public string SubmitSizedIteamRequest(ForestReleases _objmodel)
        {

            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_FPM_SubmitSeizedItemRequest", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TableName", _objmodel.TableName);
                cmd.Parameters.AddWithValue("@OffenceCode", _objmodel.OffenseCode);
                cmd.Parameters.AddWithValue("@ID", _objmodel.ID);
                cmd.Parameters.AddWithValue("@Ownership_Document", _objmodel.DocOfownership);
                cmd.Parameters.AddWithValue("@Application_SeizedItem", _objmodel.AppOfSeized);      
                cmd.Parameters.AddWithValue("@EnteredBy", _objmodel.UserID);


                string chId = cmd.ExecuteNonQuery().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitSizedIteam" + "_" + "SubmitSizedIteamRequest", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";
        }


        public DataSet Get_Compoundlist(String SSOID)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_GetCompountDetails", Conn);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.Parameters.AddWithValue("@Status", 1);
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

        public DataSet Get_CompoundteamDetails(string OffenceCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_GetCompountDetails", Conn);
                cmd.Parameters.AddWithValue("@OffenceCode", OffenceCode);
                cmd.Parameters.AddWithValue("@Status", 2);
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


        public string SubmitCompoundIteamRequest(ForestReleases _objmodel)
        {
            string chId = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_SubmitCompoundItemRequest", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestFor", "CompoundItem");
                cmd.Parameters.AddWithValue("@OffenceCode", _objmodel.OffenseCode);
                cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(_objmodel.CompAmount));
                cmd.Parameters.AddWithValue("@Application_Compounding",  _objmodel.AppOfcompounding);
                cmd.Parameters.AddWithValue("@Reciept_Compounding",  _objmodel.ReceiptOfAmount);
                cmd.Parameters.AddWithValue("@EnteredBy", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@RequestId",Convert.ToInt64(_objmodel.RequestId));
                chId = cmd.ExecuteNonQuery().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitCompoundIteam" + "_" + "SubmitCompoundIteamRequest", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }

        public DataSet Get_OffenseCode(string RangeCode, string Option)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetSeizedCompoudingdetails", Conn);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@option", Option);
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

        #endregion
    }
     
}