using FMDSS.Models.CitizenService.PermissionServices;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace FMDSS.Models.SWCSModel
{
    public class SWCSParentClass
    {
        public string SWSID { get; set; }
        public string ActID { get; set; }
        public string ActivityID { get; set; }
        public string IsNew { get; set; }
        public string RegNo { get; set; }
    }

    public class SWCSResponse
    {
        public SWCSResponse()
        {
            Appcode = 19;
            Activitytype = "1";
        }

        public SWCSResponse(int appServiceCode, string regNo, int statusCode, string remarks="", string actionDetails="", string actionUrl="", string activityType="")
        {
            Appcode = 19;
            AppServiceCode = appServiceCode;
            RegNo = regNo;
            Statuscode = statusCode;
            Remarks = remarks;
            Actiondetails = actionDetails;
            Actionurl = actionUrl;
            Activitytype = activityType; 
        }
        
        public int Appcode { get; set; }
        public int AppServiceCode { get; set; }
        public string RegNo { get; set; }
        public int Statuscode { get; set; }
        public string Remarks { get; set; }
        public string Actiondetails { get; set; }
        public string Actionurl { get; set; }
        public string Activitytype { get; set; }
    }

    public class SWCSModel : SWCSParentClass
    {
        public Int64 SWCS_TblID { get; set; }
        public string Userdetails { get; set; }
        public string Establishmentname { get; set; }
        public string Total_Employees { get; set; }
        public string ProposedInvestment { get; set; }
        public string Operational_Date { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CategoryofEstablishment { get; set; }
        public string NatureOfBusiness { get; set; }
        public string PlotNo { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string RuralUrban { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string Village { get; set; }
        public string Tehsil { get; set; }
        public string District { get; set; }
        public string PIN { get; set; }
        public string BusinessDetail { get; set; }
        public string PrimaryGroup { get; set; }
        public string BRN { get; set; }
        public string PAN { get; set; }
        public string TIN { get; set; }
        public string VAT { get; set; }
        public string STDCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string PostalAddress { get; set; }
        public string Est_PlotNo { get; set; }
        public string Est_Street { get; set; }
        public string Est_Area { get; set; }
        public string Est_RuralUrban { get; set; }
        public string Est_District { get; set; }
        public string Est_Tehsil { get; set; }
        public string Est_Ward { get; set; }
        public string Est_Village { get; set; }
        public string Est_PIN { get; set; }

    }

    public class SWCSRepository : DAL
    {
        public Boolean CRUDFromSWCS(FixedLandUsage model, ref long SWCS_TblID, string Action)
        {
            DataSet ds = new DataSet();
            Boolean flag = false;
            try
            {
                #region Serialzed Request SWCS
                string Request = Newtonsoft.Json.JsonConvert.SerializeObject(model._SWCSModel);
                string Response = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                #endregion
                #region Serialzed Response SWCS
                string SWCSRequest = Newtonsoft.Json.JsonConvert.SerializeObject(model._SWCSModel);
                string SWCSResponse = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                #endregion

                DALConn();

                SqlCommand cmd = new SqlCommand("SP_CRUD_SWCSLog", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@SWCSLogID", model._SWCSModel.SWCS_TblID);
                cmd.Parameters.AddWithValue("@RequestID", !string.IsNullOrEmpty(model.RequestedID) ? model.RequestedID : model.TransactionId);
                cmd.Parameters.AddWithValue("@SWSID", model._SWCSModel.SWSID);
                cmd.Parameters.AddWithValue("@ActID", model._SWCSModel.ActID);
                cmd.Parameters.AddWithValue("@ActivityID", model._SWCSModel.ActivityID);
                cmd.Parameters.AddWithValue("@IsNew", model._SWCSModel.IsNew);
                cmd.Parameters.AddWithValue("@RegNo", model._SWCSModel.RegNo);
                cmd.Parameters.AddWithValue("@Request", Request);
                cmd.Parameters.AddWithValue("@Response", Response);
                cmd.Parameters.AddWithValue("@SWCSRequest", SWCSRequest);
                cmd.Parameters.AddWithValue("@SWCSResponse", SWCSResponse);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@ExtraColumn", "");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && Convert.ToInt16(ds.Tables[0].Rows[0]["Status"]) > 0)
                {
                    SWCS_TblID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "CRUDFromSWCS" + "_" + "SWCSRepository", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return flag;
        }
    }

    public static class SWCSHelper
    {
        public static string posttopage(string URL, string userdetails, string AppName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body style='background-color:#F0F0F0;' onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", URL);
            sb.AppendFormat("<div style='float:left; width:100%; height:100%;'>");
            sb.AppendFormat("<div style='float:left; width:100%; height:100%; margin-top:10%;'>	");
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center; font-size:30px; color:#525252; margin:0 0 50px 0;'>Please wait while you are being redirected to <span style='font-weight:bold;'>{0}</span> Application.</div>", AppName.ToUpper());
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center;'>");
            sb.AppendFormat("<img src='/images/loading.gif'  width='350px'/>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("<input type='hidden' name='userdetails' value='{0}'>", userdetails);
            sb.AppendFormat("</div>");
            sb.AppendFormat("<div>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

    }

    public static class SWCSPostApplicationData
    {
        /// <summary>
        /// If you are Using Payment Gateway That time this method call
        /// </summary>
        /// <param name="model"></param>
        public static void SavePartialData(FixedLandUsage model)
        {
            #region SET Single Window Statgeing Credentials Developed By Rajveer
            string URL = string.Empty;
            EmitraData obj = new EmitraData();
            URL = "http://swcstest.rajasthan.gov.in:8887/statusupdate.asmx";
            // var client = new RestClient(BaseUrl + TokenNumber + "&SECRET_KEY=" + SECRET_KEY + "&MERCHANT_CODE=" + MERCHANT_CODE + "&SERVICE_ID=" + SERVICE_ID);
            var client1 = new RestClient(URL);

            var request1 = new RestRequest(Method.POST);
            string Regno = string.Empty;
            string SWSID = string.Empty;
            request1.AddParameter("SWSID", model._SWCSModel.SWSID);
            request1.AddParameter("Regno", model.TransactionId);
            request1.AddParameter("Statuscode", "1");//STatus 1 Means Pendeing the Request
            request1.AddHeader("cache-control", "no-cache");
            IRestResponse response1 = client1.Execute(request1);
            var Data = response1.Content;

            #endregion

        }

        public static void SaveDataOnSWCS(FixedLandUsage model)
        {

            //SingleWindow.STATUSUPDATESoapClient SWCSSTATUSUPDATE = new SingleWindow.STATUSUPDATESoapClient("STATUSUPDATESoap");
            //string result= SWCSSTATUSUPDATE.statusupdate(19, model.swcsResponse.AppServiceCode,  model.swcsResponse.RegNo,  model.swcsResponse.Statuscode, 
            //    model.swcsResponse.Remarks, model.swcsResponse.Actiondetails , model.swcsResponse.Actionurl,  model.swcsResponse.Activitytype);

            //SWCSSTATUSUPDATE.STATUSUPDATE statusupdate = new SWCSSTATUSUPDATE.STATUSUPDATE();
            //string svcoutput = statusupdate.statusupdate(13, SVCTYPE, APPID, 2, "License Fees Paid And Application Submitted", "", "", "");

            //#region SET Single Window Statgeing Credentials Developed By Rajveer
            //string URL = "http://swcstest.rajasthan.gov.in:8887/statusupdate.asmx";
            //EmitraData obj = new EmitraData();      
       
            //var client1 = new RestClient(URL);

            //var request1 = new RestRequest(Method.POST);
            //string Regno = string.Empty;
            //string SWSID = string.Empty;
            //request1.AddParameter("Appcode",);
            //request1.AddParameter("Appsvrcode", model.swcsResponse.AppServiceCode);
            //request1.AddParameter("Regno", model.swcsResponse.RegNo);
            //request1.AddParameter("Statuscode",model.swcsResponse.Statuscode);//Status 1 Means Pendeing the Request
            //request1.AddParameter("Remarks", model.swcsResponse.Remarks);
            //request1.AddParameter("Actiondetails", model.swcsResponse.Actiondetails);
            //request1.AddParameter("Actionurl", model.swcsResponse.Actionurl);
            //request1.AddParameter("Activitytype",model.swcsResponse.Activitytype);

            //request1.AddHeader("cache-control", "no-cache");
            //IRestResponse response1 = client1.Execute(request1);
            //var Data = response1.Content;

            //#endregion
        }

    }
}