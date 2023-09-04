using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace FMDSS.Models
{
    public class EmitraPayRequest:DAL
    {
        public string PayRequest(bool IsMobileApp, string REQUESTID, string MERCHANTCODE, string ChecksumKey, string EncryptionKey, string SUCCESSURL, string FAILUREURL, string OFFICECODE, string SERVICEID, string TotalAmount, string REVENUEHEAD, string User, string BASEURL = "", string COMMTYPE = "")
        {

            string destinationUrl = "";
            if (!string.IsNullOrWhiteSpace(BASEURL))
                destinationUrl = BASEURL;
            else
            {              
                destinationUrl = ConfigurationManager.AppSettings["emitraPostUrlUATServer"].ToString();//UAT
            }

            string ENCDATA = "Khan"; // fixed Value
           
            MERCHANTCODE = "FOREST0716";
            ChecksumKey = "EmitraNew@2016";
            EncryptionKey = "E-m!tr@2016 ";
           
            SERVICEID = "7926"; //Nursery Module UAT
            REVENUEHEAD = "900-1.00|901-0.00|1262-0.00";

            //SERVICEID =  "3870";//UAT-Nahargarh Zoological Park
            // SERVICEID = "2925";// Jhalana Leapord Santury
            //REVENUEHEAD =  "820-0.00|840-1.00"; //UAT-Nahargarh Zoological Park
            COMMTYPE = "3";//UAT- Jhalana Leapord Santury
                           //COMMTYPE = "3";//UAT-Nahargarh Zoological Park


            //destinationUrl = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA ";
            //END

            PGRequest data = new PGRequest();
            data.AMOUNT = "1.00";

            if (!string.IsNullOrWhiteSpace(COMMTYPE))
                data.COMMTYPE = COMMTYPE;
            else
                data.COMMTYPE = "1";

            data.FAILUREURL = FAILUREURL; //HttpContext.Current.Request.Url.AbsoluteUri.Replace("Default.aspx", "PGResult.aspx");
            data.MERCHANTCODE = MERCHANTCODE;
            data.OFFICECODE = OFFICECODE;// "FORESTHQ"; // OFFICECODE; // "RSOSHQ";

            data.OFFICECODE = "DIV045"; // OFFICECODE; // "RSOSHQ"; UAT CHANGES
            //data.OFFICECODE = "DIV003";// UAT CHANGES

            data.PRN = REQUESTID; // DateTime.Now.ToString("yyyyMMddHHmmss");
            data.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmss") + "000";
            data.REVENUEHEAD = REVENUEHEAD;
            data.SERVICEID = SERVICEID; // SERVICEID which service is this line wild life booking / zoo booking / camp booking / film shooting booking 
            data.SUCCESSURL = SUCCESSURL;
            data.UDF1 = User; // Not Required
            data.UDF2 = "udf2"; // Not Required
            data.USEREMAIL = ""; // Not Required
            data.USERMOBILE = ""; // Not Required
            data.USERNAME = ""; // Not Required
            data.CHECKSUM = EncodingDecoding.CreateMD5(data.PRN + "|" + data.AMOUNT + "|" + ChecksumKey);

            EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(EncryptionKey);
            SaveEmitraSentRequestDetail(REQUESTID, JsonConvert.SerializeObject(data));
            ENCDATA = objEncryptDecrypt.encrypt(JsonConvert.SerializeObject(data));

            //NameValueCollection frmData = new NameValueCollection();
            //frmData.Add("MERCHANTCODE", MERCHANTCODE);
            //frmData.Add("SERVICEID", SERVICEID);
            //frmData.Add("ENCDATA", ENCDATA);
            return HTMLHelper.RedirectAndPOST(IsMobileApp, destinationUrl, MERCHANTCODE, ENCDATA, SERVICEID);




        }
        public string PayRequestOld(bool IsMobileApp, string REQUESTID, string MERCHANTCODE, string ChecksumKey, string EncryptionKey, string SUCCESSURL, string FAILUREURL, string OFFICECODE, string SERVICEID, string TotalAmount, string REVENUEHEAD, string User, string BASEURL = "", string COMMTYPE = "")
        {

            string destinationUrl = "";
            if (!string.IsNullOrWhiteSpace(BASEURL))
                destinationUrl = BASEURL;
            else
            {
                //if (FMDSS.Globals.Util.GetAppSettings("WebsiteStatus").Equals("Live"))
                //    destinationUrl = ConfigurationManager.AppSettings["emitraPostUrl"].ToString();//LIVE UAT CHANGES
                //else
                destinationUrl = ConfigurationManager.AppSettings["emitraPostUrlUATServer"].ToString();//UAT
            }


            //destinationUrl = ConfigurationManager.AppSettings["emitraPostUrlUATServer"].ToString();//UAT CHANGES



            //string MERCHANTCODE = "FOREST0716"; // fixed Value
            string ENCDATA = "Khan"; // fixed Value
            //SERVICEID = "2349";


            //#Test on UAT 

            MERCHANTCODE = "FOREST0716";
            ChecksumKey = "EmitraNew@2016";
            EncryptionKey = "EmitraNew@2016 ";
            //OFFICECODE = "FORESTHQ";

            SERVICEID = "2349"; //Nursery Module UAT
            REVENUEHEAD = "1060-0.00|900-0.00|901-0.00|920-1.00";

            //SERVICEID =  "3870";//UAT-Nahargarh Zoological Park
           // SERVICEID = "2925";// Jhalana Leapord Santury
            //REVENUEHEAD =  "820-0.00|840-1.00"; //UAT-Nahargarh Zoological Park
            COMMTYPE = "3";//UAT- Jhalana Leapord Santury
            //COMMTYPE = "3";//UAT-Nahargarh Zoological Park
            

            //destinationUrl = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA ";
            //END

            PGRequest data = new PGRequest();
            data.AMOUNT = "1.00";

            if (!string.IsNullOrWhiteSpace(COMMTYPE))
                data.COMMTYPE = COMMTYPE;
            else
                data.COMMTYPE = "1";

            data.FAILUREURL = FAILUREURL; //HttpContext.Current.Request.Url.AbsoluteUri.Replace("Default.aspx", "PGResult.aspx");
            data.MERCHANTCODE = MERCHANTCODE;
            data.OFFICECODE = OFFICECODE;// "FORESTHQ"; // OFFICECODE; // "RSOSHQ";

            data.OFFICECODE = "FORESTHQ"; // OFFICECODE; // "RSOSHQ"; UAT CHANGES
            //data.OFFICECODE = "DIV003";// UAT CHANGES

            data.PRN = REQUESTID; // DateTime.Now.ToString("yyyyMMddHHmmss");
            data.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmss") + "000";
            data.REVENUEHEAD = REVENUEHEAD;
            data.SERVICEID = SERVICEID; // SERVICEID which service is this line wild life booking / zoo booking / camp booking / film shooting booking 
            data.SUCCESSURL = SUCCESSURL;
            data.UDF1 = User; // Not Required
            data.UDF2 = "udf2"; // Not Required
            data.USEREMAIL = ""; // Not Required
            data.USERMOBILE = ""; // Not Required
            data.USERNAME = ""; // Not Required
            data.CHECKSUM = EncodingDecoding.CreateMD5(data.PRN + "|" + data.AMOUNT + "|" + ChecksumKey);

            EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(EncryptionKey);
            SaveEmitraSentRequestDetail(REQUESTID, JsonConvert.SerializeObject(data));
            ENCDATA = objEncryptDecrypt.encrypt(JsonConvert.SerializeObject(data));

            //NameValueCollection frmData = new NameValueCollection();
            //frmData.Add("MERCHANTCODE", MERCHANTCODE);
            //frmData.Add("SERVICEID", SERVICEID);
            //frmData.Add("ENCDATA", ENCDATA);
            return HTMLHelper.RedirectAndPOST(IsMobileApp, destinationUrl, MERCHANTCODE, ENCDATA, SERVICEID);




        }
        public string PayRequestLive(bool IsMobileApp, string REQUESTID, string MERCHANTCODE, string ChecksumKey, string EncryptionKey, string SUCCESSURL, string FAILUREURL, string OFFICECODE, string SERVICEID, string TotalAmount, string REVENUEHEAD, string User, string BASEURL = "", string COMMTYPE = "")
        {

            string destinationUrl = "";
            if (!string.IsNullOrWhiteSpace(BASEURL))
                destinationUrl = BASEURL;
            else
            {
                //if (FMDSS.Globals.Util.GetAppSettings("WebsiteStatus").Equals("Live"))
                //    destinationUrl = ConfigurationManager.AppSettings["emitraPostUrl"].ToString();//LIVE UAT CHANGES
                //else
                destinationUrl = ConfigurationManager.AppSettings["emitraPostUrlUATServer"].ToString();//UAT
            }


            //destinationUrl = ConfigurationManager.AppSettings["emitraPostUrlUATServer"].ToString();//UAT CHANGES



            //string MERCHANTCODE = "FOREST0716"; // fixed Value
            string ENCDATA = "Khan"; // fixed Value
            //SERVICEID = "2349";


            //#Test on UAT 

            //MERCHANTCODE = "FOREST0716";
            //ChecksumKey = "EmitraNew@2016";
            //EncryptionKey = "EmitraNew@2016 ";
            OFFICECODE = "FORESTHQ";
            //SERVICEID = "2349"; //Nursery Module UAT
            //REVENUEHEAD = "1060-0.00|900-0.00|901-0.00|920-1.00";
            //destinationUrl = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA ";
            //END

            PGRequest data = new PGRequest();
            //data.AMOUNT = "1.00";

            if (!string.IsNullOrWhiteSpace(COMMTYPE))
                data.COMMTYPE = COMMTYPE;
            else
                data.COMMTYPE = "1";

            data.FAILUREURL = FAILUREURL; //HttpContext.Current.Request.Url.AbsoluteUri.Replace("Default.aspx", "PGResult.aspx");
            data.MERCHANTCODE = MERCHANTCODE;
            data.OFFICECODE = OFFICECODE;// "FORESTHQ"; // OFFICECODE; // "RSOSHQ";
            //data.OFFICECODE = "FORESTHQ"; // OFFICECODE; // "RSOSHQ"; UAT CHANGES
            //data.OFFICECODE = "DIV003";// UAT CHANGES
            data.PRN = REQUESTID; // DateTime.Now.ToString("yyyyMMddHHmmss");
            data.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmss") + "000";
            data.REVENUEHEAD = REVENUEHEAD;
            data.SERVICEID = SERVICEID; // SERVICEID which service is this line wild life booking / zoo booking / camp booking / film shooting booking 
            data.SUCCESSURL = SUCCESSURL;
            data.UDF1 = User; // Not Required
            data.UDF2 = "udf2"; // Not Required
            data.USEREMAIL = ""; // Not Required
            data.USERMOBILE = ""; // Not Required
            data.USERNAME = ""; // Not Required
            data.AMOUNT = TotalAmount;
            data.CHECKSUM = EncodingDecoding.CreateMD5(data.PRN + "|" + data.AMOUNT + "|" + ChecksumKey);

            EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(EncryptionKey);
            SaveEmitraSentRequestDetail(REQUESTID, JsonConvert.SerializeObject(data));
            ENCDATA = objEncryptDecrypt.encrypt(JsonConvert.SerializeObject(data));

            //NameValueCollection frmData = new NameValueCollection();
            //frmData.Add("MERCHANTCODE", MERCHANTCODE);
            //frmData.Add("SERVICEID", SERVICEID);
            //frmData.Add("ENCDATA", ENCDATA);
            return HTMLHelper.RedirectAndPOST(IsMobileApp, destinationUrl, MERCHANTCODE, ENCDATA, SERVICEID);




        }

        public string EncreptData(PGRequest data, string URL)
        {
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            //var client1 = new RestClient(URL);
            //var request1 = new RestRequest(Method.POST);
            //request1.AddHeader("cache-control", "no-cache");

            //IRestResponse response1 = client1.Execute(request1);
            string resp = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            string responsebody = "";
            using (var client = new System.Net.WebClient()) //WebClient  
            {

                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("toBeEncrypt", resp);
                byte[] responsebytes = client.UploadValues("http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/emitraAESEncryption", "POST", reqparm);
                responsebody = Encoding.UTF8.GetString(responsebytes);
            }
            return responsebody;//HTMLHelper.RedirectAndPOST(IsMobileApp, destinationUrl, MERCHANTCODE, ENCDATA, SERVICEID);



        }

        public void SaveEmitraSentRequestDetail(string requestId, string EmitraSentDetail)
        {            
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();
                cmd = new SqlCommand("SP_EmitraSentRequest", Conn);    
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.Parameters.AddWithValue("@EmitraSentDetail", EmitraSentDetail);              
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_EmitraSentRequest" , 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}