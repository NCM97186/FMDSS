using FMDSS.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace FMDSS
{
    public class EmitraKisokPayment : DAL
    {
        public EmitraKioskResponse ProcessPayment(EmitraKioskRequest paymentRequest, int isChoice = 0, string RequestDesc = "")
        {
            string responsString = string.Empty;
            try
            {
                var client = new RestClient(paymentRequest.BASEURL);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

                //SET CHECKSUM
                paymentRequest.CHECKSUM = GetCheckSum(new EmitraKioskChecksum { SSOID = paymentRequest.SSOID, REQTIMESTAMP = paymentRequest.REQTIMESTAMP, REQUESTID = paymentRequest.REQUESTID, SSOTOKEN = paymentRequest.SSOTOKEN });

                SaveEmitraRequest(paymentRequest.REQUESTID, JsonConvert.SerializeObject(paymentRequest), paymentRequest.SERVICEID, isChoice, RequestDesc);

                string encData = FMDSS.Models.EncodingDecoding.Encrypt(JsonConvert.SerializeObject(paymentRequest), "E-m!tr@2016");

                request.AddParameter("application/x-www-form-urlencoded", "encData='" + encData + "'", ParameterType.RequestBody);

                Stopwatch timer = new Stopwatch();
                timer.Start();

                IRestResponse response = client.Execute(request);
                responsString = FMDSS.Models.EncodingDecoding.Decrypt(response.Content.ToString(), "E-m!tr@2016");

                EmitraKioskResponse emitraResponse = JsonConvert.DeserializeObject<EmitraKioskResponse>(responsString);
                emitraResponse.RESPONSE = responsString;
                timer.Stop();

                if (emitraResponse.TRANSACTIONSTATUS == "FAILURE")
                {
                    emitraResponse = VerifyTransaction(paymentRequest);
                }
                emitraResponse.TIMEELAPSED = timer.Elapsed.Seconds;
                SaveEmitraResponse(paymentRequest.REQUESTID, responsString, emitraResponse.TRANSACTIONSTATUS, paymentRequest.SERVICEID, emitraResponse.TRANSACTIONID, isChoice, RequestDesc);
                return emitraResponse;
            }
            catch (Exception ex)
            {
                EmitraKioskResponse emitraResponse2 = JsonConvert.DeserializeObject<EmitraKioskResponse>(responsString);

                SaveEmitraResponse(paymentRequest.REQUESTID, responsString, emitraResponse2.TRANSACTIONSTATUS, paymentRequest.SERVICEID, emitraResponse2.TRANSACTIONID, isChoice, RequestDesc);

                emitraResponse2.REQUESTID = paymentRequest.REQUESTID;
                emitraResponse2.TIMEELAPSED = 0;
                emitraResponse2.TRANSACTIONSTATUS = "FAILURE";
                emitraResponse2.RESPONSE = "ERROR: -" + (ex.InnerException == null ? (ex.Message == null ? "on varification error occured" : ex.Message.ToString()) : (ex.InnerException == null ? "on varification error occured" : ex.InnerException.StackTrace.ToString()));
                emitraResponse2.TRANSAMT = "0.00";
                emitraResponse2.TRANSACTIONID = "0";


                return emitraResponse2;
            }
        }
        private void SaveEmitraRequest(string RequestId, string EmitraRequest, string ServiceId, int isChoice, string RequestDesc)
        {
            SqlParameter[] param = {
                                        new SqlParameter("@RequestId",RequestId),
                                        new SqlParameter("@EmitraRequests",EmitraRequest),
                                        new SqlParameter("@ServiceId",ServiceId),
                                        new SqlParameter("@IsChoiceVehicleBoat",isChoice),
                                        new SqlParameter("@RequestDesc",RequestDesc),
                                   };
            ExecuteNonQuery("spNP_KioskEmitraRequestResponse", param);
        }
        private void SaveEmitraResponse(string RequestId, string EmitraResponse, string ResponseStatus, string ServiceId, string Transactionid, int isChoice, string RequestDesc)
        {
            SqlParameter[] param = {
                                        new SqlParameter("@RequestId",RequestId),
                                        new SqlParameter("@EmitraResponse",EmitraResponse),
                                        new SqlParameter("@ResponseStatus",ResponseStatus),
                                        new SqlParameter("@ServiceId",ServiceId),
                                        new SqlParameter("@TransactionId",Transactionid),
                                        new SqlParameter("@IsChoiceVehicleBoat",isChoice),
                                        new SqlParameter("@RequestDesc",RequestDesc),
                                   };
            ExecuteNonQuery("spNP_KioskEmitraRequestResponse", param);
        }
        public EmitraKioskResponse VerifyTransaction(EmitraKioskRequest paymentRequest)
        {
            var TransactionVerificationURL = paymentRequest.VERIFICAION_URL; 

            EmitraKioskVerifyTransactionRequest verifyRequest = new EmitraKioskVerifyTransactionRequest
            {
                 MERCHANTCODE = paymentRequest.MERCHANTCODE,
                 REQUESTID = paymentRequest.REQUESTID,
                 SERVICEID = paymentRequest.SERVICEID,
                 SSOTOKEN = "0",
                 CHECKSUM  = GetCheckSum(new EmitraKioskVerifyTransactionChecksum{  MERCHANTCODE=paymentRequest.MERCHANTCODE, REQUESTID = paymentRequest.REQUESTID, SSOTOKEN = "0"})
            };
            var Data = JsonConvert.SerializeObject(verifyRequest);
            var client = new RestClient(TransactionVerificationURL + "?data=" + Data + "");
            var request = new RestRequest(Method.POST);

            IRestResponse response = client.Execute(request);
            EmitraKioskResponse emitraResponse = JsonConvert.DeserializeObject<EmitraKioskResponse>(response.Content.ToString());
            emitraResponse.TRANSAMT = emitraResponse.AMT;
            emitraResponse.RESPONSE = response.Content.ToString();
            return emitraResponse;
        }


        private string GetCheckSum(object checksum)
        {
            string retVal = CreateMD5(JsonConvert.SerializeObject(checksum));
            return retVal;
        }

        private static string CreateMD5(string input)
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
}