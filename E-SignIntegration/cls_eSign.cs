using FMDSS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace FMDSS.E_SignIntegration
{

    public class Logs : DAL
    {
        #region Request and Response Log
        public DataSet E_signRequestResponseLogs(string ModuleName, string Request, string Response, string RequestID)
        {
            DataSet DS = new DataSet();
            try
            {
                #region Get IP Address
                string IpAddress = string.Empty;
                #endregion

                DALConn();
                SqlCommand cmd = new SqlCommand("sp_E_SignIntegrationRequestLogs", Conn);
                cmd.Parameters.AddWithValue("@ModuleName", ModuleName);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.Parameters.AddWithValue("@Request", Request);
                cmd.Parameters.AddWithValue("@Response", Response);
                cmd.Parameters.AddWithValue("@IpAddress", IpAddress);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "E_SignIntegration" + "_" + "E_signRequestResponseLogs", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return DS;
        }
        #endregion
    }

    public class cls_eSign
    {

        #region E-Sign Immplementation
        public OtpResponce GetOTP(clsOTP OTPDetails, string RequestID = null)
        {
            OtpResponce OtpResponce = new OtpResponce();
            var postData = JsonConvert.SerializeObject(OTPDetails);
            try
            {

                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["OTPURL"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                OtpResponce = (OtpResponce)JsonConvert.DeserializeObject<OtpResponce>(responseString);

                #region Maintain Request and Response Logs
                Logs logs = new Logs();
                string Response = JsonConvert.SerializeObject(OtpResponce);
                logs.E_signRequestResponseLogs("GetOTP", postData, Response, RequestID);

                #endregion

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                    OtpResponce.ErrorMessage = text;
                    OtpResponce.Status = "0";

                    #region Maintain Request and Response Logs
                    Logs logs = new Logs();
                    string Response = JsonConvert.SerializeObject(OtpResponce);
                    logs.E_signRequestResponseLogs("GetOTP Error", postData, Response, RequestID);

                    #endregion
                }
            }
            return OtpResponce;
        }
        public clsVerifyOTPResponce VerifyOTP(clsVerifyOTP VerifyOTP, string RequestID)
        {
            clsVerifyOTPResponce VerifyOtpResponce = new clsVerifyOTPResponce();
            var postData = JsonConvert.SerializeObject(VerifyOTP);
            try
            {
                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["VerifyOTPURL"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);

                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                VerifyOtpResponce = (clsVerifyOTPResponce)JsonConvert.DeserializeObject<clsVerifyOTPResponce>(responseString);
                #region Maintain Request and Response Logs
                Logs logs = new Logs();
                string Response = JsonConvert.SerializeObject(VerifyOtpResponce);
                logs.E_signRequestResponseLogs("VerifyOTP ", postData, Response, RequestID);

                #endregion

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);


                    string text = reader.ReadToEnd();
                    VerifyOtpResponce.ErrorMessage = text;
                    VerifyOtpResponce.Status = "0";

                    #region Maintain Request and Response Logs
                    Logs logs = new Logs();
                    string Response = JsonConvert.SerializeObject(VerifyOtpResponce);
                    logs.E_signRequestResponseLogs("VerifyOTP Error", postData, Response, RequestID);

                    #endregion
                }
            }

            return VerifyOtpResponce;
        }
        public clsDocumentESignResponce GetDocumentESign(clsDocumentESign clsDocumentESignRequest, string RequestID)
        {
            clsDocumentESignResponce DocumentESignResponse = new clsDocumentESignResponce();
            var postData = JsonConvert.SerializeObject(clsDocumentESignRequest);
            try
            {
                //string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["DocumenteSignOnePage"].ToString();
                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["DocumenteSignAllPage"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);

                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                DocumentESignResponse = JsonConvert.DeserializeObject<clsDocumentESignResponce>(responseString);
                #region Maintain Request and Response Logs
                Logs logs = new Logs();
                string Response = JsonConvert.SerializeObject(DocumentESignResponse);
                logs.E_signRequestResponseLogs("GetDocumentESign", postData, Response, RequestID);

                #endregion

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);


                    string text = reader.ReadToEnd();
                    DocumentESignResponse.ErrorMessage = text;
                    DocumentESignResponse.Status = "0";

                    #region Maintain Request and Response Logs
                    Logs logs = new Logs();
                    string Response = JsonConvert.SerializeObject(DocumentESignResponse);
                    logs.E_signRequestResponseLogs("GetDocumentESign Error", postData, Response, RequestID);

                    #endregion
                }
            }

            return DocumentESignResponse;
        }

        public clsDocumentESignResponce GetDocumentESign(clsDocumentESign clsDocumentESignRequest, string RequestID, string type)
        {
            clsDocumentESignResponce DocumentESignResponse = new clsDocumentESignResponce();
            var postData = JsonConvert.SerializeObject(clsDocumentESignRequest);
            try
            {
                string serviceUrl = string.Empty;

                switch (type)
                {
                    case "MultipleLocation":
                        serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["DocumenteSignMultiplePosition"].ToString();
                        break;
                    default:
                        serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["DocumenteSignAllPage"].ToString();
                        break;
                }

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);

                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                DocumentESignResponse = JsonConvert.DeserializeObject<clsDocumentESignResponce>(responseString);
                #region Maintain Request and Response Logs
                Logs logs = new Logs();
                string Response = JsonConvert.SerializeObject(DocumentESignResponse);
                logs.E_signRequestResponseLogs("GetDocumentESign", postData, Response, RequestID);

                #endregion

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);


                    string text = reader.ReadToEnd();
                    DocumentESignResponse.ErrorMessage = text;
                    DocumentESignResponse.Status = "0";

                    #region Maintain Request and Response Logs
                    Logs logs = new Logs();
                    string Response = JsonConvert.SerializeObject(DocumentESignResponse);
                    logs.E_signRequestResponseLogs("GetDocumentESign Error", postData, Response, RequestID);

                    #endregion
                }
            }

            return DocumentESignResponse;
        }

        #endregion

        #region E-Sign Emitra Integration Refund Process in online zoo booking

        public OtpResponce GetOTPbyEmitra(clsOTPbyEmitra OTPDetails, string RequestID = null)
        {
            OtpResponce OtpResponce = new OtpResponce();
            var postData = JsonConvert.SerializeObject(OTPDetails);
            try
            {

                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["OTPURLUATEmitra"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                OtpResponce = (OtpResponce)JsonConvert.DeserializeObject<OtpResponce>(responseString);

                #region Maintain Request and Response Logs
                Logs logs = new Logs();
                string Response = JsonConvert.SerializeObject(OtpResponce);
                logs.E_signRequestResponseLogs("GetOTPbyEmitra", postData, Response, RequestID);

                #endregion

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                    OtpResponce.ErrorMessage = text;
                    OtpResponce.Status = "0";

                    #region Maintain Request and Response Logs
                    Logs logs = new Logs();
                    string Response = JsonConvert.SerializeObject(OtpResponce);
                    logs.E_signRequestResponseLogs("GetOTPbyEmitra Error", postData, Response, RequestID);

                    #endregion
                }
            }
            return OtpResponce;
        }

        public clsVerifyOTPResponce VerifyOTPbyEmitra(clsVerifyOTP VerifyOTP, string RequestID = null)
        {
            clsVerifyOTPResponce VerifyOtpResponce = new clsVerifyOTPResponce();
            var postData = JsonConvert.SerializeObject(VerifyOTP);
            try
            {
                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["VerifyOTPURLUATEmitra"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);

                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                VerifyOtpResponce = (clsVerifyOTPResponce)JsonConvert.DeserializeObject<clsVerifyOTPResponce>(responseString);
                #region Maintain Request and Response Logs
                Logs logs = new Logs();
                string Response = JsonConvert.SerializeObject(VerifyOtpResponce);
                logs.E_signRequestResponseLogs("VerifyOTPbyEmitra ", postData, Response, RequestID);

                #endregion

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);


                    string text = reader.ReadToEnd();
                    VerifyOtpResponce.ErrorMessage = text;
                    VerifyOtpResponce.Status = "0";

                    #region Maintain Request and Response Logs
                    Logs logs = new Logs();
                    string Response = JsonConvert.SerializeObject(VerifyOtpResponce);
                    logs.E_signRequestResponseLogs("VerifyOTPbyEmitra Error", postData, Response, RequestID);

                    #endregion
                }
            }

            return VerifyOtpResponce;
        }

        public clsDocumentESignByEmitraResponse GetDocumentESignByEmitra(clsDocumentESignByEmitra clsDocumentESignRequest, string RequestID = null)
        {
            clsDocumentESignByEmitraResponse DocumentESignResponse = new clsDocumentESignByEmitraResponse();
            var postData = JsonConvert.SerializeObject(clsDocumentESignRequest);
            try
            {
                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["UploadTextFileUATEmitra"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);

                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                DocumentESignResponse = JsonConvert.DeserializeObject<clsDocumentESignByEmitraResponse>(responseString);
                #region Maintain Request and Response Logs
                Logs logs = new Logs();
                string Response = JsonConvert.SerializeObject(DocumentESignResponse);
                logs.E_signRequestResponseLogs("GetDocumentESignByEmitra", postData, Response, RequestID);

                #endregion

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);


                    string text = reader.ReadToEnd();
                    DocumentESignResponse.ErrorMessage = text;
                    DocumentESignResponse.Status = "0";

                    #region Maintain Request and Response Logs
                    Logs logs = new Logs();
                    string Response = JsonConvert.SerializeObject(DocumentESignResponse);
                    logs.E_signRequestResponseLogs("GetDocumentESignByEmitra Error", postData, Response, RequestID);

                    #endregion
                }
            }

            return DocumentESignResponse;
        }

        public clsUploadTextFileResponse UploadTextFile(clsUploadTextFile TextFileDetails, string SSOID, string RequestID = null)
        {
            clsUploadTextFileResponse OtpResponce = new clsUploadTextFileResponse();
            var postData = JsonConvert.SerializeObject(TextFileDetails);
            try
            {

                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["UploadTextFile"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Basic " + SSOID);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                OtpResponce = (clsUploadTextFileResponse)JsonConvert.DeserializeObject<clsUploadTextFileResponse>(responseString);
                #region Maintain Request and Response Logs
                Logs logs = new Logs();
                string Response = JsonConvert.SerializeObject(responseString);
                logs.E_signRequestResponseLogs("UploadTextFile", postData, Response, RequestID);

                #endregion

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);


                    string text = reader.ReadToEnd();
                    OtpResponce.message = text;
                    OtpResponce.status = "0";

                    #region Maintain Request and Response Logs
                    Logs logs = new Logs();
                    string Response = JsonConvert.SerializeObject(OtpResponce);
                    logs.E_signRequestResponseLogs("UploadTextFile Error", postData, Response, RequestID);

                    #endregion
                }
            }
            return OtpResponce;
        }

        public clsGetTransationStatusResponse GetTransationStatus(clsGetTransationStatus Request, string SSOID, string RequestID = null)
        {
            clsGetTransationStatusResponse Responce = new clsGetTransationStatusResponse();
            try
            {
                string test = JsonConvert.SerializeObject(Responce);

                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["GetTranStatusUAT"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);
                var postData = JsonConvert.SerializeObject(Request);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.Headers.Add("Basic", SSOID);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Responce = JsonConvert.DeserializeObject<clsGetTransationStatusResponse>(responseString);


            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);


                    string text = reader.ReadToEnd();
                    Responce.message = text;
                    Responce.status = "0";
                }
            }
            return Responce;
        }

        public clsAllTransationStatusResponse GetALLTransationStatus(clsAllTransationStatusRequest Request, string SSOID, string RequestID = null)
        {
            clsAllTransationStatusResponse Responce = new clsAllTransationStatusResponse();
            try
            {
                string test = JsonConvert.SerializeObject(Responce);

                string serviceUrl = System.Configuration.ConfigurationSettings.AppSettings["GetTranStatusAllUAT"].ToString();

                var request = (HttpWebRequest)WebRequest.Create(serviceUrl);
                var postData = JsonConvert.SerializeObject(Request);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.Headers.Add("Basic", SSOID);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Responce = JsonConvert.DeserializeObject<clsAllTransationStatusResponse>(responseString);


            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);


                    string text = reader.ReadToEnd();
                    Responce.message = text;
                    Responce.status = "0";
                }
            }
            return Responce;
        }
        #endregion
    }
}