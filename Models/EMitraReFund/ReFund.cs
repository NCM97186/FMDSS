using FMDSS.Models.BookOnlineTicket;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace FMDSS.Models.EMitraReFund
{
    public class EmitraReFund : DAL
    {
        //Request Parameter Value
        //{
        //"MERCHANTCODE": "mCode",
        //"REQUESTID": "reqId",
        //"REQTIMESTAMP": "reqTime",
        //"SERVICEID": "srvId",
        //"SUBSERVICEID": "subsrvId",
        //"REVENUEHEAD": "revnuId1 - revnuValue1 | revnuId2 - revnuValue2",
        //"CONSUMERKEY": "consKey",
        //"CONSUMERNAME": "consName",
        //"COMMTYPE": "commissionType",
        //"SSOID": "ssoID",
        //"OFFICECODE": "officeCode",
        //"SSOTOKEN": "ssotoken",
        //"CHECKSUM": "checksum",
        //"PAYMODE":"",
        //"BANKREFNUMBER":""
        //}


        //Response String
        //        {
        //"REQUESTID": "reqId",
        //"TRANSACTIONSTATUSCODE": "statusCode",
        //"RECEIPTNO": "receiptNo",
        //"TRANSACTIONID": "transId",
        //"TRANSAMT": "transactionAmt",
        //"REMAININGWALLET": "remainingWallet",
        //"EMITRATIMESTAMP": "responseTime",
        //"TRANSACTIONSTATUS": "status",
        //"MSG": "Msg",
        //"CHECKSUM": "checksum"
        //}
        //CHECKSUM Calculation
        //Checksum will be calculated on below parameters / string–
        //{
        //    "SSOID": "SSOTESTKIOSK",
        //    "REQUESTID": "1228",
        //    "REQTIMESTAMP": "20160617165442681",
        //    "SSOTOKEN": "158442"
        //}
        ////Checksum is generated using MD5 algorithm and append it with request string
        //public string RefundRequest(bool IsMobileApp, string REQUESTID, string MERCHANTCODE, string ChecksumKey, string EncryptionKey, string SUCCESSURL, string FAILUREURL, string OFFICECODE, string SERVICEID, string TotalAmount, string REVENUEHEAD, string User, string BASEURL = "", string COMMTYPE = "")
        public string RefundRequestOldType(string UserName, string UserId, string UserSSOId, string RequestId, string REVENUEHEAD, string eMitraTransactioId, string PAYMENTMODEBID, Int64 ticketId, decimal TotalRefundableAmt, bool IsLiveOrUAT,string OldReceiptNo,string OldServiceId,string OfficeDivCode, out string EmitraRefundResponse)
        {
            string strResult ="|";
            EmitraRefundResponse = "";
            RefundServices refundServerices = new RefundServices();
            refundServerices = Get_RefundService(IsLiveOrUAT); //IsLive =1 or 0 for UAT

            //Int64 intId = Convert.ToInt64(Encryption.decrypt(Id));
            //RefundServices refundServerices = new RefundServices();
            EmitraReFund obj = new EmitraReFund();
            //BookOnTicket OBJ = new BookOnTicket();
            ///DataTable data = obj.BookingCancelledMarkedRecord(intId);
            //string RequestId = "";
            //string TicketId = "";
            DataSet DS = new DataSet();

            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrlForLocal"].ToString();

            ////////KioskUserDetail kud = new KioskUserDetail();
            KioskRefundDetail kud = new KioskRefundDetail();

            Int64 UserID = Convert.ToInt64(UserId);
            try
            {

                var baseAddress = refundServerices.KIOSK_Back_to_back_Trans_URL;  //UAT
                eMitraObjForRefund _objKioskRefund = new eMitraObjForRefund();
               
                _objKioskRefund.MERCHANTCODE =  refundServerices.MerchantCode; //"FOREST0716";//

                _objKioskRefund.REQUESTID = RequestId;
                _objKioskRefund.REQTIMESTAMP = DateTime.Now.Ticks.ToString();
                _objKioskRefund.SERVICEID = refundServerices.e_MitraServiceId;
                _objKioskRefund.SUBSERVICEID = "";
                _objKioskRefund.REVENUEHEAD = REVENUEHEAD;
                //_objKioskRefund.CONSUMERKEY = eMitraTransactioId;// RequestId;
                _objKioskRefund.CONSUMERKEY = eMitraTransactioId + "-" + OldReceiptNo + "-" + OldServiceId + "-" + refundServerices.PayMode;// RequestId;  //ConsumerKey=EmtiraTokenNumber-ReceiptNumber-ServiceNumber-PaymentMode
                _objKioskRefund.CONSUMERNAME = UserName;
                _objKioskRefund.SSOID =  UserSSOId;
                _objKioskRefund.OFFICECODE = OfficeDivCode; ///refundServerices.OfficeCode; //"FORESTHQ"; //
                _objKioskRefund.TRANSAMT = Convert.ToString(TotalRefundableAmt);
               

                _objKioskRefund.COMMTYPE = refundServerices.ComType; //"3";   //added  by shaan for testing UAT
                _objKioskRefund.SSOTOKEN = "0"; /// Convert.ToString(Session["SSOTOKEN"]);
				eMitraObjectForRefundChecksum _csum = new eMitraObjectForRefundChecksum();
                _objKioskRefund.PAYMODE = refundServerices.PayMode; //"0"; 0 will be send on live case and 173 will be send on uat
                _objKioskRefund.BANKREFNUMBER = PAYMENTMODEBID + "_R";

                // sending extra parameters as discussed with emitra team (Chetan, Abhishek) -- 17-10-2022


                eMitraObjForRefund checksum = new eMitraObjForRefund() { SSOID = UserSSOId, REQUESTID = RequestId, REQTIMESTAMP = _objKioskRefund.REQTIMESTAMP, SSOTOKEN = "0" };


                _objKioskRefund.CHECKSUM = _csum.GetCheckSum(checksum);


                //string encData = FMDSS.Models.EncodingDecoding.Encrypt(JsonConvert.SerializeObject(_objKioskRefund), "E-m!tr@2016");
                string encData = FMDSS.Models.EncodingDecoding.Encrypt(JsonConvert.SerializeObject(_objKioskRefund), refundServerices.Encryption_Key);


                var client = new RestClient(baseAddress);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "encData='" + encData + "'", ParameterType.RequestBody);

                Stopwatch timer = new Stopwatch();
                timer.Start();

                IRestResponse response = client.Execute(request);

                string decVal = FMDSS.Models.EncodingDecoding.Decrypt(response.Content.ToString(), refundServerices.Encryption_Key); //"E-m!tr@2016"
                EmitraRefundResponse = decVal;

                kud.EmitraLOGJsone(decVal, _objKioskRefund.REQUESTID, Convert.ToString(UserID));
                eMitraObjForRefund _objKiosk = JsonConvert.DeserializeObject<eMitraObjForRefund>(decVal);

                kud.SaveKioskEmitraResponse(_objKiosk);
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;

                //if (timeTaken.Seconds > Convert.ToInt16(kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "SERVICE_RESPONSE_TIME")))
                //{
                //	#region Email and SMS
                //	SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                //	string name = Convert.ToString(timeTaken.Seconds) + " sec for Request ID " + _objKiosk.REQUESTID;

                //	objSMSandEMAILtemplate.SendMailComman("ALL", "EmitrakioskbacktobackserviceDeley", "", name, "", "", "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                //	#endregion
                //}

                if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
                {
                    strResult = _objKiosk.TRANSACTIONSTATUS +"|"+"1";
                }
                else if (_objKiosk.TRANSACTIONSTATUS == "FAILURE")
                {
                    strResult = _objKiosk.TRANSACTIONSTATUS + "|" + "-1";
                }
                else if (_objKiosk.TRANSACTIONSTATUS == "ERROR")
                {
                    strResult = _objKiosk.TRANSACTIONSTATUS + "|" + "-2";
                    _objKiosk.TRANSACTIONSTATUS = _objKiosk.MSG;
                    kud.SaveKioskEmitraResponse(_objKiosk);

                    #region Email and SMS
                    //SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    //string name = "[ " + _objKiosk.REQUESTID + " ] " + _objKiosk.MSG;

                    //objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskEndError", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                    #endregion
                }

                //return PartialView("KioskTransactionStatus", _objKiosk);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Refund_RefundPayment", 22, DateTime.Now, UserID);
                strResult = ex.Message.ToString() + "|" + "-3";
            }

            return strResult;
        }


        //public PGBackToBackResponse RefundRequest(string UserSSOId, string RequestId, string REVENUEHEAD, string eMitraTransactioId, string PAYMENTMODEBID, Int64 ticketId)
        //{
        //    PGBackToBackResponse response = new PGBackToBackResponse();

        //    RefundServices refundServerices = new RefundServices();
        //    refundServerices = Get_RefundService(false); //IsLive =1 or 0 for UAT

        //    //PGRequest data = new PGRequest();
        //    PGBackToBackRequest data = new PGBackToBackRequest();
        //    string RequestTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss") + "000";
        //    data.MERCHANTCODE = refundServerices.MerchantCode;
        //    data.REQUESTID = RequestId;// new request id for refund generation           
        //    data.REQTIMESTAMP = RequestTimeStamp;
        //    data.SERVICEID = refundServerices.e_MitraServiceId;
        //    data.SUBSERVICEID = "";
        //    data.REVENUEHEAD = REVENUEHEAD;
        //    data.CONSUMERKEY = eMitraTransactioId; //RequestId;
        //    data.CONSUMERNAME = "NA";
        //    data.COMMTYPE = refundServerices.ComType;
        //    data.SSOID = UserSSOId;
        //    data.OFFICECODE = refundServerices.OfficeCode;
        //    data.SSOTOKEN = "0";
        //    data.PAYMODE = refundServerices.PayMode; //"0"; 0 will be send on live case and 173 will be send on uat
        //    data.BANKREFNUMBER = PAYMENTMODEBID;


        //    var checksum = new PGBackToBackCheckSum() { SSOID = UserSSOId, REQUESTID = RequestId, REQTIMESTAMP = RequestTimeStamp, SSOTOKEN = "0" };

        //    string jsonCheckSum = JsonConvert.SerializeObject(checksum);

        //    //var stringjsonCheckSum = new StringContent(jsonCheckSum, UnicodeEncoding.UTF8, "application/json");

        //    data.CHECKSUM = EncodingDecoding.CreateMD5(jsonCheckSum);



        //    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(refundServerices.Encryption_Key);

        //    var refundJsonData = JsonConvert.SerializeObject(data);

        //    string ENCDATA = objEncryptDecrypt.encrypt(refundJsonData.ToString());
        //    string destinationUrl = refundServerices.KIOSK_Back_to_back_Trans_URL + "?encData=" + ENCDATA;


        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(refundServerices.KIOSK_Back_to_back_Trans_URL);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(refundJsonData);
        //        var byteContent = new ByteArrayContent(buffer);

        //        var postTask = client.PostAsync(destinationUrl, byteContent);
        //        postTask.Wait();



        //        string JSONString = JsonConvert.SerializeObject(postTask.Result);

        //        DataTable dt = new DataTable("PlantTypeListStock");
        //        dt = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
        //        response = Globals.Util.GetListFromTable<PGBackToBackResponse>(dt).ToList().FirstOrDefault();
        //        if (response.TRANSACTIONSTATUSCODE == "200")
        //        {
        //            string msg = "200";
        //        }
        //        UpdateRefundResponse(ticketId, JSONString, response.TRANSACTIONSTATUSCODE);
        //    }



        //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
        //    //request.Method = "POST";
        //    //request.ContentType = "application/json";
        //    //request.ContentLength = refundJsonData.Length;
        //    //StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
        //    //requestWriter.Write(refundJsonData);
        //    //requestWriter.Close();
        //    //try
        //    //{
        //    //    WebResponse webResponse = request.GetResponse();
        //    //    Stream webStream = webResponse.GetResponseStream();
        //    //    StreamReader responseReader = new StreamReader(webStream);
        //    //    string response1 = responseReader.ReadToEnd();
        //    //    List<PGBackToBackResponse> objList = new List<PGBackToBackResponse>();
        //    //    objList = JsonConvert.DeserializeObject<List<PGBackToBackResponse>>(response1);
        //    //    response = objList.FirstOrDefault();
        //    //   // UpdateRefundResponse(ticketId, JsonConvert.SerializeObject(response), response.TRANSACTIONSTATUSCODE);
        //    //    Console.Out.WriteLine(response);
        //    //    responseReader.Close();
        //    //    Console.WriteLine("Process Started....");
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    Console.Out.WriteLine("-----------------");
        //    //    Console.Out.WriteLine(e.Message);
        //    //}
        //    return response;
        //}

        public RefundServices Get_RefundService(bool IsLive)
        {
            RefundServices refundServices = new RefundServices();
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_eMitraRefundFun", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Get_eMitraRefundServices");

                cmd.Parameters.AddWithValue("@IsLiveOrUAT", (IsLive == true ? 1 : 0)); //0 fro UAT and 1 for Live                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                refundServices = Globals.Util.GetListFromTable<RefundServices>(dt).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "Get_RefundService", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return refundServices;
        }
        public List<RefundHeads> Get_RefundHeads(int SId)
        {
            List<RefundHeads> refundHeadList = new List<RefundHeads>();

            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_eMitraRefundFun", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Get_eMitraRefundHeads");

                cmd.Parameters.AddWithValue("@SId", SId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                refundHeadList = Globals.Util.GetListFromTable<RefundHeads>(dt).ToList();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "Get_RefundHeads", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }

            return refundHeadList;
        }
        public DataTable Get_CancellationReasonList()
        {

            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_eMitraRefundFun", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Get_BookingCancellationReasons");

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "Get_CancellationReasonList", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }
        public DataSet Get_RefundAmount(string RequestId, string TicketId)
        {

            DataSet dt = new DataSet();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_eMitraRefundFun", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Get_RefundAmount");

                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@TicketId", TicketId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "Get_RefundAmount", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }
        public DataTable Save_RefundAmount(BookOnTicket cs, bool IsLive, Int64 UserId)
        {

            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_eMitraRefundFun", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Save_RefundAmount");

                cmd.Parameters.AddWithValue("@RequestId", cs.RequestId);
                cmd.Parameters.AddWithValue("@IsLiveOrUAT", (IsLive == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@IsFullRefund", (cs.IsFullRefund == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@TypeOfActions", cs.TypeOfActions);
                cmd.Parameters.AddWithValue("@CancellationReason", cs.CancellationReason);
                cmd.Parameters.AddWithValue("@CancellationRemarks", cs.CancellationRemarks);
                cmd.Parameters.AddWithValue("@IsForcefullyAppliedByAdmin", (cs.IsForcefullyAppliedByAdmin == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@MemberID", cs.MemberSLNo);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "Save_RefundAmount", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }

        public DataSet BookingCancelledMarkedList(WildLifeBookingFilterModel Model)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_BookingCancelledMarked", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Get_BookingCancelledMarkedList");
                cmd.Parameters.AddWithValue("@FromDate", Model.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", Model.ToDate);
                cmd.Parameters.AddWithValue("@Place", Model.Place);
                cmd.Parameters.AddWithValue("@TypeOfBooking", Model.TypeOfBooking);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.Parameters.AddWithValue("@StartRow", Model.StartRow);
                cmd.Parameters.AddWithValue("@FetchRowsNext", Model.FetchRowsNext);
                cmd.Parameters.AddWithValue("@Search", Model.search);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_BookingCancelledMarked" + "_" + "BookingCancelledMarkedList", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataTable BookingCancelledMarkedRecord(Int64 id)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_BookingCancelledMarked", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Get_BookingCancelledMarkedRecord");

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "Get_RefundHeads", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }
        public DataTable Download_SavedRefundDetails(int PlaceId, string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_eMitraRefundFun", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Get_SavedRefundDetails");
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "Get_RefundHeads", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }

        public DataTable GetPaymentModeBId(Int64 ticketId)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_eMitraRefundFun", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Get_PaymentModeBId");

                cmd.Parameters.AddWithValue("@TicketId", ticketId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "GetPaymentModeBId", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }
        public void UpdateRefundResponse(Int64 ticketId, string EmitraResponse, string eResponseStatus, string e_MitraRequestId,string e_MitraResponseString)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_eMitraRefundFun", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "Update_RefundEmitraResponse");

                cmd.Parameters.AddWithValue("@TicketId", ticketId);
                cmd.Parameters.AddWithValue("@OnCanEmitraResponse", EmitraResponse);
                cmd.Parameters.AddWithValue("@eResponseStatus", eResponseStatus);
                cmd.Parameters.AddWithValue("@e_MitraRequestId", e_MitraRequestId);
                cmd.Parameters.AddWithValue("@e_MitraResponseString", e_MitraResponseString);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "UpdateRefundResponse", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
        }
        public void WaitingRefundService(bool IsLiveOrUat)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                cmd = new SqlCommand("SP_WaitingTicketsRefundService", Conn);
              
                cmd.Parameters.AddWithValue("@IsLiveOrUAT", (IsLiveOrUat==true?1:0));
               
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_eMitraRefundFun" + "_" + "UpdateRefundResponse", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
        }
    }
    public class RefundServices
    {
        public int SId { get; set; }
        public string e_MitraServiceId { get; set; }
        public string ServiceName { get; set; }
        public string MerchantCode { get; set; }
        public string OfficeCode { get; set; }
        public string ComType { get; set; }
        public string Encryption_Key { get; set; }
        public string KIOSK_Back_to_back_Trans_URL { get; set; }
        public string KIOSK_Transaction_Verification_URL { get; set; }
        public string KIOSK_PG_Transaction_Cancellation_URL { get; set; }
        public bool IsLiveOrUAT { get; set; }
        public string PayMode { get; set; }
    }
    public class RefundHeads
    {
        public int RefundHeadId { get; set; }
        public string HeadName { get; set; }
        public string HeadDescription { get; set; }
        public string HeadValue { get; set; }
        public string ServiceId { get; set; }
    }
}