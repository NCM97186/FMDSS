using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FMDSS.Filters;
using System.IO;
using log4net;
using System.Web.SessionState;
using System.Threading;
using FMDSS.Models;
using System.Timers;
using FMDSS.Repository.Interface;
using FMDSS.Repository;
using System.Web.Security;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Configuration;
using FMDSS.Models.EMitraReFund;

namespace FMDSS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

           // MvcHandler.DisableMvcResponseHeader = true;
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalFilters.Filters.Add(new MyExceptionHandler());
            //log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            GloblaUserModel.EnteredUserList = new List<string>();

            log4net.Config.XmlConfigurator.Configure();
            ILog ErrorLog = LogManager.GetLogger("DBLogger");
            MvcHandler.DisableMvcResponseHeader = true;

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //----start----- add by amrit barotia on 10-06-2021  for get Forest Fire data from API
            // for api hit time intervel 2 hour
            System.Timers.Timer timer = new System.Timers.Timer(7200000);
            //System.Timers.Timer timer = new System.Timers.Timer(60000);
            timer.Enabled = true;
            // Setup Event Handler for Timer Elapsed Event
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            //----end----- add by amrit barotia

            //System.Timers.Timer emailMessageService = new System.Timers.Timer(900000);
            ////System.Timers.Timer emailMessageService = new System.Timers.Timer(20000);
            //emailMessageService.Elapsed += new ElapsedEventHandler(EMailMessageService);
            //emailMessageService.Start();


            //System.Timers.Timer waitingMessageService = new System.Timers.Timer(3600000);
            ////System.Timers.Timer waitingMessageService = new System.Timers.Timer(20000);
            //waitingMessageService.Elapsed += new ElapsedEventHandler(WaitingTicketsRefundService);
            //waitingMessageService.Start();

            
        }
       
        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null &&
                app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
                Response.Headers.Remove("Server");
                Response.Headers.Remove("X-AspNet-Version");
                Response.Headers.Remove("X-AspNetMvc-Version");
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        //----start----- add by amrit barotia on 10-06-2021  for get Forest Fire data from API
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IProtectionRepository protectionRepository = new ProtectionRepository();
            TimeSpan currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan stopFromTime = new TimeSpan(9, 45, 0);
            TimeSpan stopToTime = new TimeSpan(11, 15, 0);
            if (!(currentTime.CompareTo(stopFromTime) > 0 && currentTime.CompareTo(stopToTime) < 0))
            {
                var msg = protectionRepository.UpdateForestFireDataFromAPI();

            }
        }
        //----end----- add by amrit barotia 
        //start for checking place select before 10am gaurab
        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                TimeSpan currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                TimeSpan stopFromTime = new TimeSpan(10, 00, 00);
                //TimeSpan stopToTime = new TimeSpan(12, 23, 59);

                if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["UserID"] != null && Session["PlaceSelectTime"] != null)
                {
                    TimeSpan placeSelectTime = (TimeSpan)Session["PlaceSelectTime"];
                    if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1")
                    {
                        if (Convert.ToInt32(Session["PlaceIdCurrentAdvanceSession"].ToString()) != 53 && Convert.ToInt32(Session["PlaceIdCurrentAdvanceSession"].ToString()) != 57)
                        {
                            if ((currentTime.CompareTo(stopFromTime) > -1 && placeSelectTime.CompareTo(stopFromTime) <= -1))
                            {
                                Session.Clear();
                                Session.Abandon();
                                Response.Cookies.Remove("__RequestVerificationToken");
                                Response.Cookies.Clear();
                                Session.RemoveAll();
                                FormsAuthentication.SignOut();
                                Response.Redirect("https://sso.rajasthan.gov.in/");
                                base.Context.ApplicationInstance.CompleteRequest();
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {

               // new Common().ErrorLog(ex.Message, "Current Booking", 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
        }
        static void WaitingTicketsRefundService(object sender, System.Timers.ElapsedEventArgs e)
        {

            TimeSpan currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan stopFromTime = new TimeSpan(10, 0, 0);
            TimeSpan stopToTime = new TimeSpan(10, 30, 0);
            if ((currentTime.CompareTo(stopFromTime) > 0 && currentTime.CompareTo(stopToTime) < 0))
            {
                int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
                bool IsLiveOrUAT = (liveUat == 0 ? false : true);
                EmitraReFund refundService = new EmitraReFund();
                refundService.WaitingRefundService(IsLiveOrUAT);
            }
        }
        static void EMailMessageService(object sender, System.Timers.ElapsedEventArgs e)
        {

            //TimeSpan currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            //TimeSpan stopFromTime = new TimeSpan(15, 46, 0);
            //TimeSpan stopToTime = new TimeSpan(15, 46, 05);
            //if ((currentTime.CompareTo(stopFromTime) > 0 && currentTime.CompareTo(stopToTime) < 0))
            //{
                ///TestMessage();
                Booking_WaitingConfirmationSMSMail();
            //}
        }

        static bool Booking_WaitingConfirmationSMSMail()
        {
            bool flg = false;
            string testEmailId = "mukeshjangid100@gmail.com";
            string testMobileNo = "7014114229";

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataSet DT = new DataSet();
            string IsDevelopment= ConfigurationManager.AppSettings["IsDevelopment"].ToString();
           
            DT = objSMSandEMAILtemplate.GetWaitingCNFUsersDetails("Get_CNF_TO_Cancelled_AND_WL_To_CNF_Details");
            if (DT.Tables[0].Rows.Count > 0)
            {
                bool canEmail = true;bool canMobile = true;
                bool cnfEmail = true;bool cnfMobile = true;
                foreach (DataRow dataRow in DT.Tables[0].Rows) // Message for Cancelliation
                {
                    if (Convert.ToString(dataRow["EmailId"]) != string.Empty)
                    {
                        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(dataRow["UserName"]), Convert.ToString(dataRow["PlaceName"]), Convert.ToString(dataRow["CanRequestId"]), Convert.ToString(dataRow["VisitDate"]), System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["WaitingTicketCancelliationTemplate"].ToString()));
                        if (IsDevelopment=="0")
                            objSMS_EMail_Services.sendEMail("WildLife Ticket Booking for RequestID : " + Convert.ToString(dataRow["CanRequestId"]), body, Convert.ToString(dataRow["EmailId"]), ConfigurationManager.AppSettings["WildLifeTicketEmail_CC"].ToString());
                        else if (IsDevelopment == "1" && canEmail == true)
                        {
                            canEmail = false;
                            objSMS_EMail_Services.sendEMail("WildLife Ticket Booking for RequestID : " + Convert.ToString(dataRow["CanRequestId"]), body, testEmailId, ConfigurationManager.AppSettings["WildLifeTicketEmail_CC"].ToString());
                        }
                            

                        body = string.Empty;

                    }

                    if (Convert.ToString(dataRow["Mobile"]) != string.Empty)
                    {
                        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(dataRow["UserName"]), Convert.ToString(dataRow["PlaceName"]), Convert.ToString(dataRow["CanRequestId"]), Convert.ToString(dataRow["VisitDate"]), System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["WaitingTicketCancelliationSMSTemplate"].ToString()));

                        if (IsDevelopment == "0")
                            SendMobileSMS(Convert.ToString(dataRow["Mobile"]), body, "1507166782064690804");
                        else if (IsDevelopment == "1" && canMobile==true)
                        {
                            canMobile = false;
                            SendMobileSMS(testMobileNo, body, "1507166782064690804");
                        }
                            

                        body = string.Empty;
                    }
                    objSMSandEMAILtemplate.UpdatetWaitingCNFMessageSentInfo(Convert.ToString(dataRow["CanRequestId"]), "UPDATE_WL_To_CNF_MessageSentStaus");
                }
                foreach (DataRow dataRow in DT.Tables[1].Rows) // Message for Waiting Confirmation
                {


                    if (Convert.ToString(dataRow["EmailId"]) != string.Empty)
                    {
                        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(dataRow["UserName"]), Convert.ToString(dataRow["PlaceName"]), Convert.ToString(dataRow["WaitingCNFRequestId"]), Convert.ToString(dataRow["VisitDate"]), System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["WaitingTicketConfirmationTemplate"].ToString()));
                        if (IsDevelopment == "0")
                            objSMS_EMail_Services.sendEMail("WildLife Ticket Booking for RequestID : " + Convert.ToString(dataRow["WaitingCNFRequestId"]), body, Convert.ToString(dataRow["EmailId"]), ConfigurationManager.AppSettings["WildLifeTicketEmail_CC"].ToString());
                        else if (IsDevelopment == "1" && cnfEmail == true)
                        {
                            cnfEmail = false;
                            objSMS_EMail_Services.sendEMail("WildLife Ticket Booking for RequestID : " + Convert.ToString(dataRow["WaitingCNFRequestId"]), body, testEmailId, ConfigurationManager.AppSettings["WildLifeTicketEmail_CC"].ToString());
                        }
                            

                        body = string.Empty;

                    }

                    if (Convert.ToString(dataRow["Mobile"]) != string.Empty)
                    {
                        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(dataRow["UserName"]), Convert.ToString(dataRow["PlaceName"]), Convert.ToString(dataRow["WaitingCNFRequestId"]), Convert.ToString(dataRow["VisitDate"]), System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["WaitingTicketConfirmationSMSTemplate"].ToString()));
                        if (IsDevelopment == "0")
                            SendMobileSMS(Convert.ToString(dataRow["Mobile"]), body, "1507166782062436486");
                        else if (IsDevelopment == "1" && cnfMobile==true)
                        {
                            cnfMobile = false;
                            SendMobileSMS(testMobileNo, body, "1507166782062436486");                            
                        }
                            

                        body = string.Empty;
                    }
                    objSMSandEMAILtemplate.UpdatetWaitingCNFMessageSentInfo(Convert.ToString(dataRow["WaitingCNFRequestId"]), "UPDATE_WL_To_CNF_MessageSentStaus");
                }
                flg = true;
            }
            return flg;
        }
       
        public static void SendMobileSMS(String mobileNo, String message,String templateid)
        {
            try
            {
                List<string> mobileList = new List<string> { mobileNo,"9667355740" };
                SMS_EMail_Services.SendSMS_Ver_2_0(mobileList, message, templateid);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Send Single_SMS" + "_" + "SMS_Email_Services", 0, DateTime.Now, 219);
            }

        }
        public static void TestMessage()
        {
            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            string  body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate("Mukesh", "Ranthambore","test123456789","01/01/2000", System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["WaitingTicketCancelliationSMSTemplate"].ToString()));           
            SendMobileSMS("9251659750", body, "1507166782064690804");          
            body = string.Empty;
            body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate("Mukesh", "Ranthambore", "test123456790", "01/01/2000", System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["WaitingTicketConfirmationSMSTemplate"].ToString()));
            SendMobileSMS("9251659750", body, "1507166782062436486");
            body = string.Empty;
        }

        //end for checking place select before 10am gaurab
        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    Thread td = new Thread(new ThreadStart(refreshPage));
        //    td.IsBackground = true;
        //    td.Start();
        //}

        //private void refreshPage()
        //{
        //    DateTime CurrentTime=DateTime.Now;
        //    DateTime t1 = Convert.ToDateTime("01:20:00 PM");
        //    if (CurrentTime == t1)
        //    {
        //        Session.Clear();
        //        Session.Abandon();
        //        Context.Response.Redirect("sso.rajasthan.gov.in");
        //    }
        //}
    }
}