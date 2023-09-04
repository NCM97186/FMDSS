using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.ComponentModel;

namespace FMDSS.Models
{
    public class EmitraReponse
    {
        public string message { get; set; }
        public string status { get; set; }
        public string errorMsg { get; set; }

    }


    public class ReconciliationUpdate
    {
        public string RequestID { get; set; }
        public string TicketID { get; set; }
        [Required(ErrorMessage = "The Comment field is required")]
        public string RefundComment { get; set; }
        public string TokenNO { get; set; }

        public long ServiceID { get; set; }
        public string Total_Amount { get; set; }
    }
    public class Reconciliation : ReconciliationUpdate
    {

        public string Fmdss_Status { get; set; }
        public string Emitra_Status { get; set; }


        public string ReceiptNo { get; set; }
        public int RefundStatus { get; set; }
    }

    public class ReconciliationModel
    {
        public ReconciliationModel()
        {
            ReconciliationMatch = new List<Reconciliation>();
            ReconciliationDiffrent = new List<Reconciliation>();
            RefundModel = new ReconciliationUpdate();
            EmritaListStatus = new List<Reconciliation>();
        }
        public List<Reconciliation> ReconciliationMatch { get; set; }
        public List<Reconciliation> ReconciliationDiffrent { get; set; }

        public ReconciliationUpdate RefundModel { get; set; }

        public List<Reconciliation> EmritaListStatus { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string ServiceID { get; set; }
    }


    public class ReconciliationRepo : DAL
    {
        public DataSet GetReconciliationData(ReconciliationModel model)
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("sp_GetReconciliationData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetReconciliationData");
                cmd.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(model.FromDate) ? null : model.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(model.ToDate) ? null : model.ToDate);
                cmd.Parameters.AddWithValue("@ServiceID", string.IsNullOrEmpty(model.ServiceID) ? null : model.ServiceID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                dt = new DataSet();
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public long InsertReconciliationLog(string ControllerActionName, string IpAddress, string Request, long UserID, string EmitraRequest, string EmitraResponse, long ReconciliationLogID, string InsertOrUpdated)
        {
            DataTable dt = new DataTable();
            long RequestID = 0;
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("sp_GetReconciliationData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (InsertOrUpdated.Trim().ToUpper() == "UPDATED")
                    cmd.Parameters.AddWithValue("@Action", "UpdateReconciliationLogs");
                else
                    cmd.Parameters.AddWithValue("@Action", "InsertReconciliationLogs");

                cmd.Parameters.AddWithValue("@Request", Request);
                cmd.Parameters.AddWithValue("@userID", UserID);
                cmd.Parameters.AddWithValue("@ControllerActionName", ControllerActionName);
                cmd.Parameters.AddWithValue("@IpAddress", IpAddress);

                cmd.Parameters.AddWithValue("@EmitraRequest", EmitraRequest);
                cmd.Parameters.AddWithValue("@EmitraResponse", EmitraResponse);
                cmd.Parameters.AddWithValue("@ReconciliationLogID", ReconciliationLogID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    RequestID = Convert.ToInt64(dt.Rows[0]["ReconciliationLogID"].ToString());
                }

            }
            catch (Exception ex)
            {
                dt = new DataTable();
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return RequestID;
        }
        public DataSet UpdateRefundTran(ReconciliationUpdate model, long UserID)
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("sp_GetReconciliationData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "UpdateRefundTran");
                cmd.Parameters.AddWithValue("@RequestID", model.RequestID);
                cmd.Parameters.AddWithValue("@TicketID", model.TicketID);
                cmd.Parameters.AddWithValue("@RefundStatus", 1);
                cmd.Parameters.AddWithValue("@RefundComment", model.RefundComment);
                cmd.Parameters.AddWithValue("@ServiceID", model.ServiceID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                #region Send Main and Email
                if (dt != null && dt.Tables.Count > 0)
                {
                    try
                    {
                        //string AllAdminEmail = string.Empty;
                        //if (dt.Tables[1] != null && dt.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Tables[1].Rows[0]["EmailID"])))
                        //{
                        //    AllAdminEmail = Convert.ToString(dt.Tables[1].Rows[0]["EmailID"]);
                        //}
                        #region Email and SMS
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RequestID, Convert.ToString(dt.Tables[0].Rows[0]["ServiceName"]));
                        string ApplicantEmail = string.Empty;
                        string ApplicantMobile = string.Empty;
                        if (DT != null && DT.Rows.Count > 0)
                        {
                            ApplicantEmail = Convert.ToString(DT.Rows[0]["ApplicantEmail"]);
                            ApplicantMobile = Convert.ToString(DT.Rows[0]["ApplicantMobile"]);

                        }
                        objSMSandEMAILtemplate.SendMailComman("ALL", "Reconciliation", model.RequestID, Convert.ToString(DT.Rows[0]["ApplicantName"]), ApplicantEmail, ApplicantMobile, "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                        #endregion
                        //SendSMSEmailForSuccessTransaction(model.RequestID, "GETUSERDETAILSFORSENDSMSANDEMAIL", Convert.ToString(model.RefundComment), AllAdminEmail);
                    }
                    catch (Exception ex)
                    {
                        new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "SubmitApplication" + "_" + "AmritaDeviAwardMail", 1, DateTime.Now, UserID);
                    }
                }
                #endregion
                return dt;

            }
            catch (Exception ex)
            {
                dt = new DataSet();
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public void SendSMSEmailForSuccessTransaction(string RequestId, string DatabaseAction, string Comments, string AdminEmailAddress)
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Rajveer Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable DT = objSMSandEMAILtemplate.GetUserDetails(RequestId, DatabaseAction);
            if (DT.Rows.Count > 0)
            {
                if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                {
                    body = string.Empty;

                    #region Send Email Submit Application
                    //string UserMailBody = UserMailSMSBody("Mail", ACTION, RequestId, IsApproveAndRejectStatus);
                    string UserMailBody = objSMSandEMAILtemplate.ReconciliationMail("Mail", RequestId, Convert.ToString(DT.Rows[0]["NAME"]), Comments, WebConfigurationManager.AppSettings["Reconciliation"].ToString());
                    string subject = "Reconciliation";
                    // objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ApplicantEmail"].ToString(), string.Empty);
                    objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ApplicantEmail"].ToString(), string.Empty);

                    if (!string.IsNullOrEmpty(AdminEmailAddress))
                    {
                        string AdminMailBody = objSMSandEMAILtemplate.ReconciliationMail("Mail", RequestId, Convert.ToString(DT.Rows[0]["NAME"]), Comments, WebConfigurationManager.AppSettings["ReconciliationAdmin"].ToString());
                        objSMS_EMail_Services.sendEMail(subject, UserMailBody, AdminEmailAddress, string.Empty);
                    }
                    #endregion
                }
            }
            #endregion
        }


        public void SendEmailFailureEmitraTransation(string ErrorMessage, string Request, string Token)
        {
            try
            {
                #region  after SUCCESS flag send SMS and Email to the user // code by Rajveer Sharma

                #region Send Email Submit Application
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                objSMSandEMAILtemplate.SendMailComman("ALL", "ReconciliationError", Token, string.Empty, string.Empty, string.Empty, "");


                //string UserMailBody = UserMailSMSBody("Mail", ACTION, RequestId, IsApproveAndRejectStatus);
                //string UserMailBody = objSMSandEMAILtemplate.ReconciliationMail("Mail", ErrorMessage, Request, Token, WebConfigurationManager.AppSettings["EmitraTransationError"].ToString());
                //string subject = "Reconciliation Payment Time";
                //// objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ApplicantEmail"].ToString(), string.Empty);
                //objSMS_EMail_Services.sendEMail(subject, UserMailBody, WebConfigurationManager.AppSettings["EmailAddress"].ToString(), string.Empty);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        public EmitraReponse EmitraCancelationProcess(string TokenNumber, string ServiceID, string MethodName, long UserID = 0)
        {
            EmitraReponse resp = new EmitraReponse();
            try
            {

                //var client1 = new RestClient("https://emitraapp.rajasthan.gov.in/webServicesRepository/refundPGTransaction");
                //var request1 = new RestRequest(Method.POST);
                //request1.AddHeader("content-type", "application/x-www-form-urlencoded");
                //request1.AddParameter("application/x-www-form-urlencoded", "TOKEN_NO=170000212551&SECRET_KEY=@FOREST#4*23N&MERCHANT_CODE=FOREST0117&SERVICE_ID=2239", ParameterType.RequestBody);
                //IRestResponse response1 = client1.Execute(request1);

                #region Get Emitra Live Credentials Developed By Rajveer
                DataSet DS = new DataSet();

                //string ReconsilationURL = string.Empty;
                //string merchantCode = string.Empty;
                //string serviceId = string.Empty;
                //string allDetails = string.Empty;
                //string URL = ReconsilationURL + "?fromDate=" + DateTime.Now.AddDays(-1).ToShortDateString() + "&toDate=" + DateTime.Now.AddDays(-1).ToShortDateString() + "&merchantCode=" + merchantCode + "&serviceId=" + serviceId + "&allDetails=" + allDetails;
                string URL = string.Empty;
                EmitraData obj = new EmitraData();
                DS = obj.GetEmitraServiceDetailsWithParameter("SetRefundProcessURL", Convert.ToInt64(ServiceID), MethodName, UserID);
                if (DS != null && DS.Tables[0].Rows.Count > 0)
                {
                    URL = Convert.ToString(DS.Tables[0].Rows[0]["URL"]).Replace("####", TokenNumber);
                    // var client = new RestClient(BaseUrl + TokenNumber + "&SECRET_KEY=" + SECRET_KEY + "&MERCHANT_CODE=" + MERCHANT_CODE + "&SERVICE_ID=" + SERVICE_ID);
                    var client1 = new RestClient(URL);

                    var request1 = new RestRequest(Method.POST);
                    request1.AddHeader("cache-control", "no-cache");
                    IRestResponse response1 = client1.Execute(request1);
                    resp = Newtonsoft.Json.JsonConvert.DeserializeObject<EmitraReponse>(response1.Content);
                }
                else
                {
                    resp.message = "No Service Url Find in Database Please Check It" + URL;
                    resp.errorMsg = "No Service Url Find in Database Please Check It" + URL;
                }
                #endregion

                //string str = string.Empty;

                //string BaseUrl = WebConfigurationManager.AppSettings["EmitraUrlReconsilation"].ToString();
                //string SECRET_KEY = WebConfigurationManager.AppSettings["SECRET_KEY"].ToString();
                //string MERCHANT_CODE = WebConfigurationManager.AppSettings["MERCHANT_CODE"].ToString();
                //string SERVICE_ID = WebConfigurationManager.AppSettings["SERVICE_ID"].ToString();
                //// var client = new RestClient(BaseUrl + TokenNumber + "&SECRET_KEY=" + SECRET_KEY + "&MERCHANT_CODE=" + MERCHANT_CODE + "&SERVICE_ID=" + SERVICE_ID);
                //var client = new RestClient(BaseUrl + TokenNumber + "&SECRET_KEY=" + SECRET_KEY + "&MERCHANT_CODE=" + MERCHANT_CODE + "&SERVICE_ID=" + SERVICE_ID);

                //var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                //IRestResponse response = client.Execute(request);
                //resp = Newtonsoft.Json.JsonConvert.DeserializeObject<EmitraReponse>(response.Content);


            }
            catch (Exception ex)
            {
            }
            return resp;
        }
    }

    #region Add Reconsilation Transation By Emitra API Developed By Rajveer
    public class EmitraResponseReconciliationProcessModel
    {
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string USEREMAIL { get; set; }
        public string MERCHANTCODE { get; set; }
        public string COMMTYPE { get; set; }
        public string TRANSACTIONID { get; set; }
        public string TRANSDATE { get; set; }
        public string OFFICECODE { get; set; }
        public string SERVICEID { get; set; }
        public string PAYMENTMODE { get; set; }
        public string PAYMENTMODEBID { get; set; }
        public string PAIDAMOUNT { get; set; }
        public string AMOUNT { get; set; }
        public string STATUS { get; set; }
        public string USERMOBILE { get; set; }
        public string USERNAME { get; set; }
        public string REVENUEHEAD { get; set; }
        public string PRN { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string RECEIPTNO { get; set; }
        public string PAYMENTMODETIMESTAMP { get; set; }
        public string REQTIMESTAMP { get; set; }
    }


    public class EmitraData : DAL
    {
        public string message { get; set; }
        public string status { get; set; }
        //public List<Datum> data { get; set; }
        public List<EmitraResponseReconciliationProcessModel> data { get; set; }
        public string errorMsg { get; set; }



        public void INSERTDATATOFMDSS(DataTable DT)
        {
            Int64 chk = 0;

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_EmitraPaymentReconciliation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TypeEmitraPaymentReconciliationData", DT);


                chk = Convert.ToInt64(cmd.ExecuteNonQuery());

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
            // return chk;
        }

        public DataTable GetEmitraReconciliation()
        {
            DataTable ds = new DataTable();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GetReconciliationData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "InsertReconciliationLogs");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
            return ds;
            // return chk;


        }

        public DataSet InsertDataRefundProcess(DataTable DT)
        {
            DataSet ds = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_EmitraPaymentReconciliation", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TypeEmitraPaymentReconciliationData", DT);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertDataRefundProcess", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        #region Get Emitra Live Credentials Developed By Rajveer
        public DataSet GetEmitraServiceDetailsWithParameter(string Action, long ServiceID, string ChildActionName, long UserID = 0)
        {
            DataSet ds = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GetEmitraServiceDetailsWithParameter", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@ServiceID", ServiceID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@ChildActionName", ChildActionName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetEmitraServiceDetailsWithParameter", 0, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        #endregion
    }

    public static class JsoneConvertToTable
    {

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
    #endregion
}