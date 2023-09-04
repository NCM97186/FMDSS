using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace FMDSS.Models
{
    public class EmitraPayRequest
    {

        public string PayRequest(bool IsMobileApp, string REQUESTID, string MERCHANTCODE, string ChecksumKey, string EncryptionKey, string SUCCESSURL, string FAILUREURL, string OFFICECODE, string SERVICEID, string TotalAmount, string REVENUEHEAD, string User, string BASEURL = "", string COMMTYPE = "")
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
            OFFICECODE = "FORESTHQ";
            SERVICEID = "2349"; //Nursery Module UAT
            REVENUEHEAD = "1060-0.00|900-0.00|901-0.00|920-1.00"; 
            //destinationUrl = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA ";

            //END

            PGRequest data = new PGRequest();
            data.AMOUNT = "1.00"; //TotalAmount; // "1.00"; 

            if (!string.IsNullOrWhiteSpace(COMMTYPE))
                data.COMMTYPE = COMMTYPE;
            else
                data.COMMTYPE = "2";

            data.FAILUREURL = "http://10.68.128.179/BookOnlineTicket/Payment";//FAILUREURL; //HttpContext.Current.Request.Url.AbsoluteUri.Replace("Default.aspx", "PGResult.aspx");
            data.MERCHANTCODE = MERCHANTCODE;
            data.OFFICECODE = OFFICECODE;// "FORESTHQ"; // OFFICECODE; // "RSOSHQ";
            //data.OFFICECODE = "FORESTHQ"; // OFFICECODE; // "RSOSHQ"; UAT CHANGES
            //data.OFFICECODE = "DIV003";// UAT CHANGES
            data.PRN = REQUESTID; // DateTime.Now.ToString("yyyyMMddHHmmss");
            data.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmss") + "000";
            data.REVENUEHEAD = REVENUEHEAD;
            data.SERVICEID = SERVICEID; // SERVICEID which service is this line wild life booking / zoo booking / camp booking / film shooting booking 
            data.SUCCESSURL = "http://10.68.128.179/BookOnlineTicket/Payment";//SUCCESSURL;
            data.UDF1 = User; // Not Required
            data.UDF2 = "udf2"; // Not Required
            data.USEREMAIL = ""; // Not Required
            data.USERMOBILE = ""; // Not Required
            data.USERNAME = ""; // Not Required
            data.CHECKSUM = EncodingDecoding.CreateMD5(data.PRN + "|" + data.AMOUNT + "|" + ChecksumKey);

            EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(EncryptionKey);
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
    }
}