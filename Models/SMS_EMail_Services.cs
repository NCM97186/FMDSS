using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace FMDSS.Models
{
    public class SMS_EMail_Services
    {
        public string connection = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString();
        public SqlConnection Conn;

        //static String username = "username";
        //static String password = "password";
        //static String senderid = "senderid";
        //static String message = "message";
        //static String mobileNo = "9856XXXXX";
        //static String mobileNos = "9856XXXXX, 9856XXXXX ";

        static String scheduledTime = "20110819 13:26:00";
        static HttpWebRequest request;
        static Stream dataStream;

        //public static void Main(String[] args)
        //{
        //    request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
        //    request.ProtocolVersion = HttpVersion.Version10;
        //    //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
        //    ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
        //    request.Method = "POST";
        //    Console.WriteLine("Before Calling Method");
        //    sendSingleSMS(username, password, senderid, mobileNo, message);
        //    sendBulkSMS(username, password, senderid, mobileNos, message);

        //}

        // Method for sending single SMS.
        public static void sendSingleSMS_OTP(String mobileNo, String message)
        {
            String username = System.Configuration.ConfigurationManager.AppSettings["SMSUserName"]; //"rajmsdg-forest";
            String password = System.Configuration.ConfigurationManager.AppSettings["SMSPassword"];
            String senderid = "RAJSMS";
            String NewSecureKey = System.Configuration.ConfigurationManager.AppSettings["SMSSecureKey"];

            string Responce = sendSingleSMS_OTP(username, password, senderid, mobileNo, message, NewSecureKey);
        }
        public static void sendSingleSMS(String mobileNo, String message)
        {
            try
            {
                String username = System.Configuration.ConfigurationManager.AppSettings["SMSUserName"]; //"rajmsdg-forest";
                String password = System.Configuration.ConfigurationManager.AppSettings["SMSPassword"];
                String senderid = "RAJSMS";
                String NewSecureKey = System.Configuration.ConfigurationManager.AppSettings["SMSSecureKey"];

                string Responce = sendSingleSMSNew(username, password, senderid, mobileNo, message, NewSecureKey);



                //implement the secure key functionality
                //Stream dataStream;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                //request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
                //request.ProtocolVersion = HttpVersion.Version10;
                //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
                //((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                //request.Method = "POST";
                //String smsservicetype = "singlemsg"; //For single message.
                //String query = "username=" + HttpUtility.UrlEncode(username) +
                //    "&password=" + HttpUtility.UrlEncode(password) +
                //    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                //    "&content=" + HttpUtility.UrlEncode(message) +
                //    "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +
                //    "&senderid=" + HttpUtility.UrlEncode(senderid);

                //byte[] byteArray = Encoding.ASCII.GetBytes(query);
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = byteArray.Length;
                //dataStream = request.GetRequestStream();
                //dataStream.Write(byteArray, 0, byteArray.Length);
                //dataStream.Close();
                //WebResponse response = request.GetResponse();
                //String Status = ((HttpWebResponse)response).StatusDescription;
                //dataStream = response.GetResponseStream();
                //StreamReader reader = new StreamReader(dataStream);
                //string responseFromServer = reader.ReadToEnd();
                //reader.Close();
                //dataStream.Close();
                //response.Close();
                //int i = insert_SMS_History(mobileNo, message, Responce);
            }
             catch (Exception ex) 
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Send Single_SMS" + "_" + "SMS_Email_Services", 0, DateTime.Now, 219); 
             }

        }
        // method for sending bulk SMS
        public static void sendBulkSMS(String mobileNos, String message)
        {
            //String username = "rajmsdg-forest";
            //String password = "Forest@2015#";
            //String senderid = "RAJSMS";
            //request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
            //request.ProtocolVersion = HttpVersion.Version10;
            ////((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            //((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
            //request.Method = "POST";
            //String smsservicetype = "bulkmsg"; // for bulk msg
            //String query = "username=" + HttpUtility.UrlEncode(username) +
            // "&password=" + HttpUtility.UrlEncode(password) +
            // "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
            // "&content=" + HttpUtility.UrlEncode(message) +
            // "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) +
            // "&senderid=" + HttpUtility.UrlEncode(senderid);
            //byte[] byteArray = Encoding.ASCII.GetBytes(query);
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = byteArray.Length;
            //dataStream = request.GetRequestStream();
            //dataStream.Write(byteArray, 0, byteArray.Length);
            //dataStream.Close();
            //WebResponse response = request.GetResponse();
            //String Status = ((HttpWebResponse)response).StatusDescription;
            //dataStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(dataStream);
            //string responseFromServer = reader.ReadToEnd();
            //reader.Close();
            //dataStream.Close();
            //response.Close();
            int i = insert_SMS_History(mobileNos, message);
        }


        public void sendEMail(string Subject, string Body, string To_Address, string CC_Address)
        {
            try
            {
               
                CC_Address = "helpdesk.fmdss@rajasthan.gov.in";
                string BCC = "amit17rajput@gmail.com";
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("MAIL.RAJ.GOV.IN");
                if (!string.IsNullOrEmpty(CC_Address))
                {
                    // mail.Bcc.Add(BCC );
                    mail.CC.Add(CC_Address);
                }
                mail.From = new MailAddress("donotreply.forest@raj.gov.in");
                mail.To.Add(To_Address);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("donotreply.forest@raj.gov.in", "Rajasthan@08");
                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);
                int i = insert_Email_History(Subject, Body, To_Address, CC_Address);
            }
            catch
            {

            }
            //catch (Exception ex)
            //{
            //    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "sendEMail" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            //}


        }


      


        public static int insert_SMS_History(string MobileNo, string MsgText, string ResponceText = "")
        {
            int chk = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[SP_Common_Insert_SMS_History]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("@Msg_Text", MsgText);
                cmd.Parameters.AddWithValue("@ResponceText", ResponceText);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));

                chk = Convert.ToInt32(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "insert_SMS_History" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                con.Close();
            }

            return chk;
        }

        public static int insert_Email_History(string Subject, string Body, string To_Address, string CC_Address)
        {
            int chk = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[SP_Common_Insert_EMail_History]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email_From", "MAIL.RAJ.GOV.IN");
                cmd.Parameters.AddWithValue("@Email_To", To_Address);
                cmd.Parameters.AddWithValue("@Email_CC", CC_Address);
                cmd.Parameters.AddWithValue("@Email_Text", Body);
                cmd.Parameters.AddWithValue("@Email_Suject", Subject);

                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));

                chk = Convert.ToInt32(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "insert_Email_History" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                con.Close();
            }
            return chk;
        }



        #region
        ////implement the secure key functionality

        //public static String sendSingleSMSNew(String username, String password, String senderid, String mobileNo, String message, String secureKey)
        //{

        //    //Latest Generated Secure Key
        //    Stream dataStream;
        //    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest");
        //    request.ProtocolVersion = HttpVersion.Version10;
        //    request.KeepAlive = false;
        //    request.ServicePoint.ConnectionLimit = 1;
        //    //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";

        //    ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
        //    request.Method = "POST";

        //    System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

        //    //MyPolicy policy = new MyPolicy();
        //    //System.Net.ServicePointManager.ServerCertificateValidationCallback = policy.CheckValidationResult;

        //    String encryptedPassword = encryptedPasswod(password);  ////New Method
        //    String NewsecureKey = hashGenerator(username, senderid, message, secureKey); //// New Method
        //    String smsservicetype = "singlemsg"; //For single message.
        //    String query = "username=" + HttpUtility.UrlEncode(username) +
        //        "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
        //        "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
        //        "&content=" + HttpUtility.UrlEncode(message) +
        //        "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +
        //        "&senderid=" + HttpUtility.UrlEncode(senderid) +
        //      "&key=" + HttpUtility.UrlEncode(NewsecureKey);
        //    byte[] byteArray = Encoding.ASCII.GetBytes(query);
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = byteArray.Length;
        //    dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    WebResponse response = request.GetResponse();
        //    String Status = ((HttpWebResponse)response).StatusDescription;
        //    dataStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    String responseFromServer = reader.ReadToEnd();
        //    reader.Close();
        //    dataStream.Close();
        //    response.Close();
        //    return responseFromServer;
        //}

        public static String sendSingleSMS_OTP(String username, String password, String senderid, String mobileNo, String message, String secureKey)
        {
            ////string templateid = "1007566632229575729";// this templateid is registerd at sevadwar here is no need 
            List<string> mobileList = new List<string>();
            mobileList.Add(mobileNo);
            System.Net.ServicePointManager.SecurityProtocol =
            System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 |
            System.Net.SecurityProtocolType.Tls;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.sewadwaar.rajasthan.gov.in/app/live/eSanchar/Prod/");
            client.DefaultRequestHeaders.Add("username", "FmdssSms");
            client.DefaultRequestHeaders.Add("password", "mdF$$_07fD_0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            MultipartFormDataContent form = new MultipartFormDataContent();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (
           //SMS Integration Process
           Object obj, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
           System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors errors)
            {
                return (true);
            };
            var inputparams = new ExternalSMSApiInfo();
            inputparams.UniqueID = "FMDSS_SMS";
            inputparams.serviceName = "eSanchar Send SMS Request";
            inputparams.language = "ENG";
            //inputparams.message = "123456 is the One Time Password(OTP) to process, expires in 2 mins. Verify now.- Forest Department";
            inputparams.message = message;
            inputparams.mobileNo = mobileList;
            var response = client.PostAsJsonAsync("api/OBD/CreateSMS/Request?client_id=6c4f9996-cebc-45d3-9fd7-babd69fab94e", inputparams).Result;
            var asyncResponse = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JObject.Parse(asyncResponse);
            string status = "Response Code: " + jsonResponse["responseCode"] + "\n\nResponse Message - " + jsonResponse["responseMessage"];
            return status;
            //Stream dataStream;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");
            //request.ProtocolVersion = HttpVersion.Version10;
            //request.KeepAlive = false;
            //request.ServicePoint.ConnectionLimit = 1;
            ////((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            //((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
            //request.Method = "POST";
            //System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
            //String encryptedPassword = encryptedPasswod(password);
            //String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
            //String smsservicetype = "singlemsg"; //For single message.
            //String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +
            //    "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
            //    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
            //    "&content=" + HttpUtility.UrlEncode(message.Trim()) +
            //    "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +
            //    "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
            //    "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
            //    "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());
            //byte[] byteArray = Encoding.ASCII.GetBytes(query);
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = byteArray.Length;
            //dataStream = request.GetRequestStream();
            //dataStream.Write(byteArray, 0, byteArray.Length);
            //dataStream.Close();
            //WebResponse response = request.GetResponse();
            //String Status = ((HttpWebResponse)response).StatusDescription;
            //dataStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(dataStream);
            //String responseFromServer = reader.ReadToEnd();
            //reader.Close();
            //dataStream.Close();
            //response.Close();
            //return responseFromServer;
        }
        public static String sendSingleSMSNew(String username, String password, String senderid, String mobileNo, String message, String secureKey)
        {
            Stream dataStream;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest");
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;
            //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
            request.Method = "POST";
            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
            String encryptedPassword = encryptedPasswod(password);
            String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
            String smsservicetype = "singlemsg"; //For single message.
            String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +
                "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                "&content=" + HttpUtility.UrlEncode(message.Trim()) +
                "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +
                "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
                "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim());
            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
        public static void SendSMS_Ver_2_0(List<string> MobileNo, string MessageTemplate, string TemplateID)
        {
            System.Net.ServicePointManager.SecurityProtocol =
           System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 |
           System.Net.SecurityProtocolType.Tls;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.sewadwaar.rajasthan.gov.in/app/live/eSanchar/Prod/");
            client.DefaultRequestHeaders.Add("username", "FmdssSms");
            client.DefaultRequestHeaders.Add("password", "mdF$$_07fD_0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            MultipartFormDataContent form = new MultipartFormDataContent();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (
           //SMS Integration Process
           Object obj, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
           System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors errors)
            {
                return (true);
            };
            var inputparams = new ExternalSMSApiInfo();
            inputparams.UniqueID = "FMDSS_SMS";
            inputparams.serviceName = "eSanchar Send SMS Request";
            inputparams.language = "ENG";
            //inputparams.message = "123456 is the One Time Password(OTP) to process, expires in 2 mins. Verify now.- Forest Department";
            inputparams.message = MessageTemplate;
            inputparams.mobileNo = MobileNo;
            var response = client.PostAsJsonAsync("api/OBD/CreateSMS/Request?client_id=6c4f9996-cebc-45d3-9fd7-babd69fab94e", inputparams).Result;
            var asyncResponse = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(asyncResponse);
            string status = "Response Code: " + jsonResponse["responseCode"] + "\n\nResponse Message - " + jsonResponse["responseMessage"];
        }

        /// <summary> 
        /// Method to get Encrypted the password 
        /// </summary> 
        /// <param name="password"> password as String"
        protected static String encryptedPasswod(String password)
         {
             byte[] encPwd = Encoding.UTF8.GetBytes(password);
             //static byte[] pwd = new byte[encPwd.Length];
             HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
             byte[] pp = sha1.ComputeHash(encPwd);
             // static string result = System.Text.Encoding.UTF8.GetString(pp);
             StringBuilder sb = new StringBuilder();
             foreach (byte b in pp)
             {
                 sb.Append(b.ToString("x2"));
             }
	         return sb.ToString();
         } 
  
 
         /// <summary>
         /// Method to Generate hash code 
         /// </summary>
         /// <param name="secure_key">your last generated Secure_key
         protected static String hashGenerator(String Username, String sender_id, String message, String secure_key)
         {
             StringBuilder sb = new StringBuilder();
             sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
             byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
             //static byte[] pwd = new byte[encPwd.Length];
             HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
             byte[] sec_key = sha1.ComputeHash(genkey);
             StringBuilder sb1 = new StringBuilder();
             for (int i = 0; i < sec_key.Length; i++)
             {
                 sb1.Append(sec_key[i].ToString("x2"));
             }
             return sb1.ToString();
         }
 
	    }

        #endregion


    }

public class ExternalSMSApiInfo
{
    public string UniqueID { get; set; }
    public string serviceName { get; set; }
    public string language { get; set; }
    public string message { get; set; }
    public List<string> mobileNo { get; set; }
    public string templateID { get; set; }
}
public class MyPolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint srvPoint, System.Security.Cryptography.X509Certificates.X509Certificate certificate, WebRequest request, int certificateProblem)
        {
           // throw new NotImplementedException();
            Console.WriteLine(certificateProblem); 
            return true; 
        }
    }

 