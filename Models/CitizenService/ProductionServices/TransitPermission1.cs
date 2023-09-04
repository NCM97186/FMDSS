using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CitizenService.ProductionServices
{
    public class TransitPermission:DAL
    {
        public string RowID { get; set; }
        public string RequestId { get; set; }
        public string ReqID { get; set; }
        public string DistNAME { get; set; }
        public string VillRange { get; set; }
        public string Location { get; set; }
        public string Product { get; set; }
        public string QTy { get; set; }
        public string Unit { get; set; }
        public decimal PaidAMT { get; set; }
        public string TransactionID { get; set; }
        public int Trn_Status_Code { get; set; }
        public string Applicant_Type { get; set; }
        public string ToLocation { get; set; }
        public string TransportMode { get; set; }
        public string VehicleNo { get; set; }
        public string DriverLicense { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileno { get; set; }
        public DateTime Durationfrom { get; set; }
        public DateTime Durationto { get; set; }
        public decimal Amounttobepaid { get; set; }
        public Int64 CreatedBy { get; set; }
        public string kioskId { get; set; }

        public string TransferQty { get; set; }
        public Int64 VehicleType { get; set; }
        public Int64 Vehicle { get; set; }
        public string Echalan { get; set; }

        public decimal Paid_amount { get; set; }

        public string CreateTransitPermission()
        {
            string RequesteID = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Insert_TPRequest", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_RequestId", RequestId);
                cmd.Parameters.AddWithValue("@P_Applicant", Applicant_Type);
                cmd.Parameters.AddWithValue("@P_ReqID", ReqID);
                cmd.Parameters.AddWithValue("@P_Echalan", Echalan);
                cmd.Parameters.AddWithValue("@P_Paid_amount", Paid_amount);
                cmd.Parameters.AddWithValue("@P_ToLocation", ToLocation);
                cmd.Parameters.AddWithValue("@P_TransferQty", TransferQty);
                cmd.Parameters.AddWithValue("@P_VehicleType", VehicleType);
                cmd.Parameters.AddWithValue("@P_Vehicle", Vehicle);
                cmd.Parameters.AddWithValue("@P_TransportMode", TransportMode);
                cmd.Parameters.AddWithValue("@P_VehicleNo", VehicleNo);
                cmd.Parameters.AddWithValue("@P_DriverLicense", DriverLicense);
                cmd.Parameters.AddWithValue("@P_DriverName", DriverName);
                cmd.Parameters.AddWithValue("@P_DriverMobileno", DriverMobileno);
                cmd.Parameters.AddWithValue("@P_Durationfrom", Durationfrom);
                cmd.Parameters.AddWithValue("@P_Durationto", Durationto);
                cmd.Parameters.AddWithValue("@P_Amounttobepaid", Amounttobepaid);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                 RequesteID = cmd.ExecuteScalar().ToString();
               

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "CreateTransitPermission" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return RequesteID;
        }
        public DataTable Select_All_Transaction_ByUserID(Int64 userID, string reqType, string reqID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_AllTransactions", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_userID", userID);
                cmd.Parameters.AddWithValue("@P_reqType", reqType);
                cmd.Parameters.AddWithValue("@P_reqID", reqID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
         
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_All_Transaction_ByUserID" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        
        public DataTable Select_All_TransactionReq_ByUserID(Int64 userID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();            
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_AllTransactionReqID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_userID", userID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_All_TransactionReq_ByUserID" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_VehicleForTp()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_VehicleForTP", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
             
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_All_TransactionReq_ByUserID" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public int UpdateTransactionStatus(decimal Amount, decimal EmitraAmount)
        {
            Int32 chk=0;

            try
            {
                DALConn();
                SqlParameter[] parameters =
            {  

            new SqlParameter("@TransactionID",TransactionID),
            new SqlParameter("@TransactionStatus", Trn_Status_Code),
            new SqlParameter("@RequestID", RequestId),
            new SqlParameter("@Amount", Amount),
            new SqlParameter("@EmitraAmount", EmitraAmount),
            new SqlParameter("@userID", CreatedBy), 

            };

                 chk = Convert.ToInt32(ExecuteNonQuery("sp_Citizen_UpdateTransitPermissionStatus", parameters));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateTransactionStatus" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }


    }
}