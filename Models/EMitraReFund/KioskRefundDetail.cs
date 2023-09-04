using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.EMitraReFund
{
    public class KioskRefundDetail
    : DAL
    {

        private string GetRanDom()
        {
            var rnd = new Random();
            var randomNumbers = rnd.Next();
            return randomNumbers.ToString().Substring(0, 4);
        }

        public DataTable SaveKisokUserDetails(KioskRefundDetail _objModel)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();

                DALConn();

                SqlParameter[] parameters =
                {
                new SqlParameter("@Action","SaveKioskUserDetails"),
                new SqlParameter("@UserId",GetRanDom()),
                new SqlParameter("@DisplayName",_objModel.DisplayName),
                new SqlParameter("@MobileNumber",_objModel.MobileNumber),
                new SqlParameter("@Gender",_objModel.Gender),

                new SqlParameter("@DOB",_objModel.DOB),
                new SqlParameter("@PostalAddress",_objModel.PostalAddress),
                new SqlParameter("@City",_objModel.City),
                new SqlParameter("@State",_objModel.State),

                new SqlParameter("@PinCode",_objModel.PinCode),
                new SqlParameter("@UserType",_objModel.UserType),
                new SqlParameter("@Designation",_objModel.Designation),

                new SqlParameter("@OrganizationName",_objModel.OrganizationName),
                new SqlParameter("@OrganizationAddress",_objModel.OrganizationAddress),
                new SqlParameter("@OrganizationContact",_objModel.OrganizationContact),
                new SqlParameter("@OrganizationSPOC",_objModel.OrganizationSPOC),
                new SqlParameter("@Email",_objModel.Email),
                new SqlParameter("@KioskUserId",HttpContext.Current.Session["KioskUserId"]),
                };
                Fill(dtKiosk, "spKioskUserDetails", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dtKiosk;
        }

        public DataTable IsTicketBooking(string ServiceID)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();

                SqlParameter[] parameters =
                {
                new SqlParameter("@ServiceID",ServiceID),


                };

                Fill(dtKiosk, "SP_EmitraKioskIsTicketBooking", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }

            return dtKiosk;

        }
        public DataTable UpdateEmitraKioskTransactionStatus(eMitraObjForRefund _objModel, string ACTION, string status)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();

                DALConn();

                SqlParameter[] parameters =
                {

                new SqlParameter("@RequestedId",_objModel.REQUESTID),
                new SqlParameter("@TransactionId",_objModel.TRANSACTIONID),
                new SqlParameter("@TransactionStatus",1),
                new SqlParameter("@option",ACTION),
                new SqlParameter("@Amount",_objModel.TRANSAMT),
                };
                Fill(dtKiosk, "SP_Zoo_UpdateEmitraKioskTransactionStatus", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dtKiosk;
        }
        public void SaveKioskEmitraResponse(eMitraObjForRefund _objModel)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();

                SqlParameter[] parameters =
                {
                    new SqlParameter("@REQUESTID",_objModel.REQUESTID),
                    new SqlParameter("@TRANSACTIONSTATUSCODE",_objModel.TRANSACTIONSTATUSCODE),
                    new SqlParameter("@RECEIPTNO",_objModel.RECEIPTNO),
                    new SqlParameter("@TRANSACTIONID",_objModel.TRANSACTIONID),
                    new SqlParameter("@TRANSAMT",_objModel.TRANSAMT),
                    new SqlParameter("@REMAININGWALLET",_objModel.REMAININGWALLET),
                    new SqlParameter("@EMITRATIMESTAMP",_objModel.EMITRATIMESTAMP),
                    new SqlParameter("@TRANSACTIONSTATUS",_objModel.TRANSACTIONSTATUS),
                    new SqlParameter("@MSG",_objModel.MSG),
                    new SqlParameter("@CHECKSUM",_objModel.CHECKSUM),
                };
                Fill(dtKiosk, "SP_SaveKioskEmitraResponse", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
        }
        #region For Wildlife Kiosk User
        public DataTable UpdateEmitraKioskTransactionStatusforWildlife(eMitraObjForPayment _objModel, string ACTION, string status, long userID, string REVENUEHEAD)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();

                DALConn();

                SqlParameter[] parameters =
                {

                new SqlParameter("@RequestedId",_objModel.REQUESTID),
                new SqlParameter("@TransactionId",_objModel.TRANSACTIONID),
                new SqlParameter("@TransactionStatus",1),
                new SqlParameter("@option",ACTION),
                new SqlParameter("@Amount",_objModel.TRANSAMT),
                new SqlParameter("@UserID",userID),
                new SqlParameter("@RevenueHead",REVENUEHEAD),
                };
                Fill(dtKiosk, "SP_Wildlife_UpdateEmitraKioskTransactionStatus", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dtKiosk;
        }
        #endregion

        public string GetEmitraServiceUrlDirectOrBypass(string ServiceName, string ACTION)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();

                DALConn();

                SqlParameter[] parameters =
                {
                  new SqlParameter("@ACTION",ACTION),
                new SqlParameter("@SERVICENAME",ServiceName),
                };
                Fill(dtKiosk, "Sp_EMITRAKIOSKLINKBYPASS", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return Convert.ToString(dtKiosk.Rows[0][0]);
        }


        public void EmitraLOGJsone(string LOGJsone, string REQUESTID, string UserId)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();

                SqlParameter[] parameters =
                {
                new SqlParameter("@LOGJsone",LOGJsone),
                new SqlParameter("@REQUESTID",REQUESTID),
                new SqlParameter("@UserId",UserId),

                };

                Fill(dtKiosk, "SP_EmitraKioskTransactionLog", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }

        }


        public string UserId { get; set; }
        [Required(ErrorMessage = "Display Name is Required")]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [StringLength(10)]
        [MaxLength(10)]
        // [Required(ErrorMessage = "Mobile Number is Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile Number must be number")]
        public string MobileNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        public string Gender { get; set; }
        //  [Required(ErrorMessage = "Date of Birth is Required")]
        public string DOB { get; set; }

        //   [Required(ErrorMessage = "Postal Address is Required")]
        [StringLength(500)]
        public string PostalAddress { get; set; }

        [StringLength(50)]
        // [Required(ErrorMessage = "City is Required")]
        public string City { get; set; }

        [StringLength(50)]
        //   [Required(ErrorMessage = "State is Required")]
        public string State { get; set; }

        [MaxLength(6)]
        //    [StringLength(6, ErrorMessage = "The field Postal Code must be a number with a maximum length of 6.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postal Code must be number")]
        [Display(Name = "Postal Code")]
        public string PinCode { get; set; }
        public string UserType { get; set; }
        [StringLength(50)]
        public string Designation { get; set; }
        [StringLength(100)]
        public string OrganizationName { get; set; }
        [StringLength(500)]
        public string OrganizationAddress { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Organization Contact must be number")]
        public string OrganizationContact { get; set; }
        public string OrganizationSPOC { get; set; }
        public string IsActive { get; set; }
        public DateTime InsertDate { get; set; }
        public string KioskUserId { get; set; }
    }
    public class eMitraObjForRefund : BaseModelSerializable
    {
        public string MERCHANTCODE { get; set; }

        public string REQTIMESTAMP { get; set; }
        public string SERVICEID { get; set; }
        public string SUBSERVICEID { get; set; }
        public string REVENUEHEAD { get; set; }
        public string CONSUMERKEY { get; set; }
        public string CONSUMERNAME { get; set; }
        public string SSOID { get; set; }
        public string OFFICECODE { get; set; }
        public string COMMTYPE { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }
        public string MSG { get; set; }
        public string TRANSACTIONID { get; set; }
        public string TRANSACTIONSTATUS { get; set; }
        public string REQUESTID { get; set; }
        public string TRANSACTIONSTATUSCODE { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string TRANSAMT { get; set; }
        public string RECEIPTNO { get; set; }
        public string AMT { get; set; } // while trans verifaction

        public string REMAININGWALLET { get; set; }

        public string PAYMODE { get; set; }
        public string BANKREFNUMBER { get; set; }

    }
    [Serializable]
    public class VerifyRefundTransactionForChecksum : BaseModelSerializable
    {
        public string MERCHANTCODE { get; set; }
        // public string SERVICEID { get; set; }
        public string REQUESTID { get; set; }
        public string SSOTOKEN { get; set; }
    }
    [Serializable]
    public class eMitraObjectForRefundChecksum : BaseModelSerializable
    {
        public string SSOID { get; set; }
        public string REQUESTID { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string SSOTOKEN { get; set; }
        public string GetCheckSum(eMitraObjForRefund req)
        {
            this.SSOID = req.SSOID;
            this.REQUESTID = req.REQUESTID;
            this.REQTIMESTAMP = req.REQTIMESTAMP;
            this.SSOTOKEN = req.SSOTOKEN;

            string retVal = CreateMD5(JsonConvert.SerializeObject(this));
            return retVal;
        }

        public string GetCheckSumForVerifyTrans(VerifyRefundTransaction _objVerifyTransaction)
        {
            VerifyRefundTransactionForChecksum obj = new VerifyRefundTransactionForChecksum();

            obj.MERCHANTCODE = _objVerifyTransaction.MERCHANTCODE;
            //obj.SERVICEID = _objVerifyTransaction.SERVICEID;
            obj.REQUESTID = _objVerifyTransaction.REQUESTID;
            obj.SSOTOKEN = _objVerifyTransaction.SSOTOKEN;

            string retVal = CreateMD5(JsonConvert.SerializeObject(obj));
            return retVal;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    //sb.Append(hashBytes[i].ToString("X2"));
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
    [Serializable]
    public class KioskRefundDetails : DAL
    {
        public string RequestedId { get; set; }
        public decimal Fee { get; set; }
        public decimal Tax { get; set; }
        public int ModuleId { get; set; }
        public int ServiceTypeId { get; set; }
        public int PermissionId { get; set; }
        public long SubPermissionId { get; set; }
        public decimal Discount { get; set; }
        public decimal KioskCharges { get; set; }
        public decimal KMLCharges { get; set; }
        public decimal PDFDocCharges { get; set; }
        public decimal ServiceTax { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OtherCharges { get; set; }
        public string PaymentStatus { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal DepartmantalFees { get; set; }
        public decimal DataEntryAndDocUploadFees { get; set; }
        public string RevenueHead { get; set; }
        public string MerchantCode { get; set; }

        #region eMitra Object Detail


        #endregion

        public DataTable FetchKisokValue1(KioskRefundDetails _objModel)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();

                DALConn();

                SqlParameter[] parameters =
                {
                new SqlParameter("@option","GetDetail"),
                new SqlParameter("@ModuleId",_objModel.ModuleId),
                new SqlParameter("@ServiceTypeId",_objModel.ServiceTypeId),
                new SqlParameter("@PermissionId",_objModel.PermissionId),
                new SqlParameter("@SubPermissionId",_objModel.SubPermissionId)
                };
                Fill(dtKiosk, "Sp_Common_Get_KioskDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dtKiosk;
        }

        public DataTable FetchKisokValue(KioskRefundDetails _objModel)
        {
            DataTable dtKiosk = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {
                new SqlParameter("@option","GetDetail"),
                new SqlParameter("@RequestedId",_objModel.RequestedId),
                new SqlParameter("@ServiceId",_objModel.PermissionId),
                };
                Fill(dtKiosk, "Sp_Common_Get_KioskServiceDetail", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtKiosk;
        }
    }
    //[Serializable]
    //public class RefundByDepartmentalKioskUserDetails : DAL
    //{
    //    public string RequestedId { get; set; }

    //    //=========
    //    // ADDED BY ARVIND 

    //    public string PlaceID { get; set; }
    //    public string PlaceName { get; set; }
    //    public string DateOfArrival { get; set; }
    //    public string ZoneID { get; set; }
    //    public string ZoneName { get; set; }
    //    public string VehicleID { get; set; }

    //    public string VehicleName { get; set; }
    //    public string ShiftTime { get; set; }
    //    public string ShiftName { get; set; }
    //    public string RequestedIdEn { get; set; }

    //    public string DocumentsSSOUsers { get; set; }

    //    public string UserName { get; set; }
    //    public string UserMobileNo { get; set; }
    //    public string UserEmailAddress { get; set; }
    //    public string UserAddress { get; set; }



    //    // ADDED BY ARVIND 
    //    //=========

    //    public string TransactionID { get; set; }
    //    public int ModuleId { get; set; }
    //    public string ModuleName { get; set; }
    //    public int ServiceTypeId { get; set; }
    //    public string ServiceTypeName { get; set; }
    //    public int PermissionId { get; set; }
    //    public string PermissionName { get; set; }
    //    public int SubPermissionId { get; set; }
    //    public string SubPermissioName { get; set; }



    //    public Int64 PaidForCitizen { get; set; }
    //    public string PaidForCitizenName { get; set; }

    //    public string PaidBy { get; set; }
    //    public string PaidOn { get; set; }
    //    public decimal PaidAmount { get; set; }
    //    public string PaymentMode { get; set; }
    //    public string BankName { get; set; }
    //    public string IFSCCode { get; set; }
    //    public string ChequeNumber { get; set; }
    //    public string ChequeDate { get; set; }

    //    public string VehcileName { get; set; }
    //    public string VehicalNumber { get; set; }
    //    public string GuideName { get; set; }

    //    public string OfficeName { get; set; }
    //    public string DesignationName { get; set; }

    //    public DataTable ADDPaymentByDepartmentalKioskUser(PaymentByDepartmentalKioskUserDetails _objModel)
    //    {
    //        DataTable dt = new DataTable();
    //        try
    //        {
    //            DALConn();
    //            SqlCommand cmd = new SqlCommand("Sp_PaymentByDepartmentalKioskUser", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@option", "1");
    //            cmd.Parameters.AddWithValue("@RequestedId", _objModel.RequestedId);
    //            cmd.Parameters.AddWithValue("@ModuleId", _objModel.ModuleId);
    //            cmd.Parameters.AddWithValue("@ServiceTypeId", _objModel.ServiceTypeId);
    //            cmd.Parameters.AddWithValue("@PermissionId", _objModel.PermissionId);
    //            cmd.Parameters.AddWithValue("@SubPermissionId", _objModel.SubPermissionId);

    //            cmd.Parameters.AddWithValue("@PaidForCitizen", _objModel.PaidForCitizen);
    //            cmd.Parameters.AddWithValue("@PaidBy", _objModel.PaidBy);
    //            cmd.Parameters.AddWithValue("@PaidOn", _objModel.PaidOn);
    //            cmd.Parameters.AddWithValue("@PaidAmount", _objModel.PaidAmount);

    //            cmd.Parameters.AddWithValue("@PaymentMode", _objModel.PaymentMode);
    //            cmd.Parameters.AddWithValue("@BankName", _objModel.BankName);
    //            cmd.Parameters.AddWithValue("@IFSCCode", _objModel.IFSCCode);
    //            cmd.Parameters.AddWithValue("@ChequeNumber", _objModel.ChequeNumber);
    //            cmd.Parameters.AddWithValue("@ChequeDate", _objModel.ChequeDate);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);


    //        }
    //        catch (Exception ex)
    //        {
    //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //        return dt;
    //    }

    //    public DataSet GetRequestIDDetails(string RequestID)
    //    {
    //        DataSet dt = new DataSet();
    //        try
    //        {
    //            DALConn();
    //            SqlCommand cmd = new SqlCommand("sp_KioskVehicleDetail", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@Action", "GetVehicle");
    //            cmd.Parameters.AddWithValue("@VehicleId", RequestID);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);
    //        }
    //        catch (Exception ex)
    //        {
    //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //        return dt;
    //    }

    //    public DataTable GetRequestIDDetailsForVIPSeats(string RequestID)
    //    {
    //        DataTable dt = new DataTable();
    //        try
    //        {
    //            DALConn();
    //            SqlCommand cmd = new SqlCommand("sp_KioskVehicleDetailForVIPSeats", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@Action", "GetVehicle");
    //            cmd.Parameters.AddWithValue("@VehicleId", RequestID);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);
    //        }
    //        catch (Exception ex)
    //        {
    //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //        return dt;
    //    }

    //    public DataTable GetZOORequestIDDetails(string RequestID)
    //    {
    //        DataTable dt = new DataTable();
    //        try
    //        {
    //            DALConn();
    //            SqlCommand cmd = new SqlCommand("sp_KioskZOODETAILS", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@Action", "KioskZOODETAILS");
    //            cmd.Parameters.AddWithValue("@RequestID", RequestID);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);
    //        }
    //        catch (Exception ex)
    //        {
    //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //        return dt;
    //    }

    //    public DataTable ADDZOOPaymentByDepartmentalKioskUser(PaymentByDepartmentalKioskUserDetails _objModel)
    //    {
    //        DataTable dt = new DataTable();
    //        try
    //        {
    //            DALConn();
    //            SqlCommand cmd = new SqlCommand("Sp_PaymentByDepartmentalKioskUser", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@option", "2");
    //            cmd.Parameters.AddWithValue("@RequestedId", _objModel.RequestedId);
    //            cmd.Parameters.AddWithValue("@ModuleId", _objModel.ModuleId);
    //            cmd.Parameters.AddWithValue("@ServiceTypeId", _objModel.ServiceTypeId);
    //            cmd.Parameters.AddWithValue("@PermissionId", _objModel.PermissionId);
    //            cmd.Parameters.AddWithValue("@SubPermissionId", _objModel.SubPermissionId);

    //            cmd.Parameters.AddWithValue("@PaidForCitizen", _objModel.PaidForCitizen);
    //            cmd.Parameters.AddWithValue("@PaidBy", _objModel.PaidBy);
    //            cmd.Parameters.AddWithValue("@PaidOn", _objModel.PaidOn);
    //            cmd.Parameters.AddWithValue("@PaidAmount", _objModel.PaidAmount);

    //            cmd.Parameters.AddWithValue("@PaymentMode", _objModel.PaymentMode);
    //            cmd.Parameters.AddWithValue("@BankName", _objModel.BankName);
    //            cmd.Parameters.AddWithValue("@IFSCCode", _objModel.IFSCCode);
    //            cmd.Parameters.AddWithValue("@ChequeNumber", _objModel.ChequeNumber);
    //            cmd.Parameters.AddWithValue("@ChequeDate", _objModel.ChequeDate);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);


    //        }
    //        catch (Exception ex)
    //        {
    //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //        return dt;
    //    }

    //    public DataTable ADDNurseryPaymentByDepartmentalKioskUser(PaymentByDepartmentalKioskUserDetails _objModel)
    //    {
    //        DataTable dt = new DataTable();
    //        try
    //        {
    //            DALConn();
    //            SqlCommand cmd = new SqlCommand("Sp_PaymentByDepartmentalUserNurseryModule", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@option", "3");
    //            cmd.Parameters.AddWithValue("@RequestedId", DateTime.Now.Ticks.ToString());
    //            cmd.Parameters.AddWithValue("@ModuleId", _objModel.ModuleId);
    //            cmd.Parameters.AddWithValue("@ServiceTypeId", _objModel.ServiceTypeId);
    //            cmd.Parameters.AddWithValue("@PermissionId", _objModel.PermissionId);
    //            cmd.Parameters.AddWithValue("@SubPermissionId", _objModel.SubPermissionId);

    //            cmd.Parameters.AddWithValue("@PaidForCitizen", _objModel.PaidForCitizen);
    //            cmd.Parameters.AddWithValue("@PaidBy", _objModel.PaidBy);
    //            cmd.Parameters.AddWithValue("@PaidOn", _objModel.PaidOn);
    //            cmd.Parameters.AddWithValue("@PaidAmount", _objModel.PaidAmount);

    //            cmd.Parameters.AddWithValue("@PaymentMode", _objModel.PaymentMode);
    //            cmd.Parameters.AddWithValue("@BankName", _objModel.BankName);
    //            cmd.Parameters.AddWithValue("@IFSCCode", _objModel.IFSCCode);
    //            cmd.Parameters.AddWithValue("@ChequeNumber", _objModel.ChequeNumber);
    //            cmd.Parameters.AddWithValue("@ChequeDate", _objModel.ChequeDate);
    //            cmd.Parameters.AddWithValue("@CartIDs", _objModel.RequestedIdEn);

    //            cmd.Parameters.AddWithValue("@UserName", _objModel.UserName);
    //            cmd.Parameters.AddWithValue("@UserMobileNo", _objModel.UserMobileNo);
    //            cmd.Parameters.AddWithValue("@UserEmailId", _objModel.UserEmailAddress);
    //            cmd.Parameters.AddWithValue("@UserAddress", _objModel.UserAddress);

    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);


    //        }
    //        catch (Exception ex)
    //        {
    //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //        return dt;
    //    }

    //    public DataTable ADDNurseryPaymentByDepartmentalKioskUserDeptUsers(PaymentByDepartmentalKioskUserDetails _objModel)
    //    {
    //        DataTable dt = new DataTable();
    //        try
    //        {
    //            DALConn();
    //            SqlCommand cmd = new SqlCommand("Sp_PaymentByDepartmentalUserNurseryModuleDeptUsers", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@option", "3");
    //            cmd.Parameters.AddWithValue("@RequestedId", DateTime.Now.Ticks.ToString());
    //            cmd.Parameters.AddWithValue("@ModuleId", _objModel.ModuleId);
    //            cmd.Parameters.AddWithValue("@ServiceTypeId", _objModel.ServiceTypeId);
    //            cmd.Parameters.AddWithValue("@PermissionId", _objModel.PermissionId);
    //            cmd.Parameters.AddWithValue("@SubPermissionId", _objModel.SubPermissionId);

    //            cmd.Parameters.AddWithValue("@PaidForCitizen", _objModel.PaidForCitizen);
    //            cmd.Parameters.AddWithValue("@PaidBy", _objModel.PaidBy);
    //            cmd.Parameters.AddWithValue("@PaidOn", _objModel.PaidOn);
    //            cmd.Parameters.AddWithValue("@PaidAmount", _objModel.PaidAmount);

    //            cmd.Parameters.AddWithValue("@PaymentMode", _objModel.PaymentMode);
    //            cmd.Parameters.AddWithValue("@BankName", _objModel.BankName);
    //            cmd.Parameters.AddWithValue("@IFSCCode", _objModel.IFSCCode);
    //            cmd.Parameters.AddWithValue("@ChequeNumber", _objModel.ChequeNumber);
    //            cmd.Parameters.AddWithValue("@ChequeDate", _objModel.ChequeDate);
    //            cmd.Parameters.AddWithValue("@CartIDs", _objModel.RequestedIdEn);

    //            cmd.Parameters.AddWithValue("@UserName", _objModel.UserName);
    //            cmd.Parameters.AddWithValue("@UserMobileNo", _objModel.UserMobileNo);
    //            cmd.Parameters.AddWithValue("@UserEmailId", _objModel.UserEmailAddress);
    //            cmd.Parameters.AddWithValue("@UserAddress", _objModel.UserAddress);

    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);


    //        }
    //        catch (Exception ex)
    //        {
    //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //        return dt;
    //    }

    //    public DataTable SaveNursueyDocuments(PaymentByDepartmentalKioskUserDetails model)
    //    {
    //        DataTable dt = new DataTable();
    //        try
    //        {
    //            DALConn();
    //            SqlCommand cmd = new SqlCommand("SP_InsertNURSERY_DEPT_DORUMENTS", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@Action", "NurseryDeptUserDocuments");
    //            cmd.Parameters.AddWithValue("@REQUESTID", model.RequestedId);
    //            cmd.Parameters.AddWithValue("@SSOID", model.UserName);
    //            cmd.Parameters.AddWithValue("@DOCUMENTS", model.DocumentsSSOUsers);
    //            cmd.Parameters.AddWithValue("@OfficeName", model.OfficeName);
    //            cmd.Parameters.AddWithValue("@DesignationName", model.DesignationName);
    //            cmd.Parameters.AddWithValue("@CREATEDBY", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);


    //        }
    //        catch (Exception ex)
    //        {
    //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "" + "_" + "", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //        return dt;
    //    }

    //}
    [Serializable]
    public class VerifyRefundTransaction : BaseModelSerializable
    {
        public string MERCHANTCODE { get; set; }
        public string SERVICEID { get; set; }
        public string REQUESTID { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }
    }
}