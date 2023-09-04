using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace FMDSS.Models
{
    public class HTMLHelper
    {
        private static String PreparePOSTForm(string url, NameValueCollection data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key +
                               "\" value=\"" + data[key] + "\">");
            }

            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();



             
        }

        public static string posttopage(bool IsMobileApp, string destinationUrl, string MERCHANTCODE, string ENCDATA, string SERVICEID)
        {
            string formID = "PostForm";

            StringBuilder sb = new StringBuilder();

            if (IsMobileApp == true)
            {
                sb.Append(destinationUrl + "?MERCHANTCODE=" + MERCHANTCODE + "&SERVICEID=" + SERVICEID + "&ENCDATA=" + ENCDATA);

            }
            else
            {

                sb.Append("<html>");
                sb.Append("<body>");
                sb.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + destinationUrl + "\" method=\"POST\">");
                sb.AppendFormat("<input type='hidden' name='MERCHANTCODE' value=" + MERCHANTCODE + ">");
                sb.AppendFormat("<input type='hidden' name='SERVICEID' value=" + SERVICEID + ">");
                sb.AppendFormat("<input type='hidden' name='ENCDATA' value=" + ENCDATA + ">");
                sb.Append("</form>");
                sb.AppendFormat("<script language='javascript'>var " + formID + " = document.PostForm; " + formID + ".submit();</script>");
                sb.Append("</body>");
                sb.Append("</html>");
            }


   

            return sb.ToString();
        }


        public static string RedirectAndPOST(bool IsMobileApp, string destinationUrl, string MERCHANTCODE, string ENCDATA, string SERVICEID)
        {
            //Prepare the Posting form

            string strForm = posttopage(IsMobileApp,destinationUrl, MERCHANTCODE, ENCDATA, SERVICEID);
            //Add a literal control the specified page holding 
            //the Post Form, this is to submit the Posting form with the request.
           // page.Controls.Add(new LiteralControl(strForm));
             return strForm;
        }
    }
}