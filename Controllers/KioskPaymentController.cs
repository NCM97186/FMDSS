using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using FMDSS.Models.Master;
using FMDSS.App_Start;
using System.Security.Cryptography;
using RestSharp;
using System.Diagnostics;

namespace FMDSS.Controllers
{
    public class KioskPaymentController : BaseController
    {
        List<SelectListItem> districts = new List<SelectListItem>();
        int ModuleID = 1;
        [HttpGet]
        public ActionResult CreateKioskUser()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                GetDistricts();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateKioskUser(KioskUserDetail UserDetails)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (ModelState.IsValid)
                {
                    UserProfile User = new UserProfile()
                    {
                        IsAgency = (UserDetails.UserType == "Individual") ? false : true,
                        AgencyName = UserDetails.OrganizationName,
                        AgencyAddress = UserDetails.OrganizationAddress,
                        AgencyContact = UserDetails.OrganizationContact,
                        AgencySPOC = UserDetails.OrganizationSPOC,
                        EmailId = UserDetails.Email,
                        SSOId = "",
                        FullName = UserDetails.DisplayName,
                        DatOFBirth = (string)UserDetails.DOB,
                        Gender = UserDetails.Gender,
                        MobileNumber = UserDetails.MobileNumber,
                        Designation = "10",
                        Address1 = UserDetails.PostalAddress,
                        PINCode1 = UserDetails.PinCode,
                        District1 = UserDetails.State,
                        Roles = "CITIZEN",
                        IsSSO = true,
                        IsBhamashah = false,
                        KioskSSOId = (string)Session["SSOID"]
                    };

                    DataTable dt = User.InsertUpdateUserInfo().Tables[0];
                    bool flag = false;
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            flag = Convert.ToBoolean(dt.Rows[0]["FIRSTTIMELOGIN"]);
                            Session["UserId"] = Convert.ToInt64(dt.Rows[0]["USERID"]);
                            Session["KioskCtznName"] = UserDetails.DisplayName;
                            SMS_EMail_Services.sendSingleSMS(dt.Rows[0]["Mobile"].ToString(), "Welcome to FMDDS, You have been registered successfully in FMDSS system with the User Id " + dt.Rows[0]["Ssoid"] + ". Kindly keep the same for future reference.");
                            //SMS_EMail_Services.sendSingleSMS(dt.Rows[0]["Mobile"].ToString(), "You have been registred successfully with User Id " + dt.Rows[0]["Ssoid"]);
                        }
                        ModelState.Clear();
                        return RedirectToAction("Dashboard", "Dashboard", false);
                    }
                    else
                    { ModelState.Clear(); Session["Register"] = "false"; return View(); }

                }
                else
                { Session["Register"] = "false"; return View(); }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }

        }



        //[HttpPost]
        //public ActionResult Pay(FormCollection frmCollection)
        //{

        //    KioskUserDetail kud = new KioskUserDetail();

        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
        //    try
        //    {
        //        //var baseAddress = System.Configuration.ConfigurationManager.AppSettings["EmitraBacktoBackURL"];

        //        var baseAddress = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "GETSERVICEURL");

        //        eMitraObjForPayment _objKioskPayment = new eMitraObjForPayment();
        //        //_objKioskPayment.MERCHANTCODE = System.Configuration.ConfigurationManager.AppSettings["MerchantCode"].ToUpper();
        //        _objKioskPayment.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
        //        _objKioskPayment.REQUESTID = frmCollection["RequestId"].ToString();
        //        _objKioskPayment.REQTIMESTAMP = DateTime.Now.Ticks.ToString();
        //        //_objKioskPayment.SERVICEID = System.Configuration.ConfigurationManager.AppSettings["EMitraServiceId"].ToUpper();
        //        _objKioskPayment.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
        //        // _objKioskPayment.SUBSERVICEID = Convert.ToString(frmCollection["SubServiceId"]).ToUpper();
        //        _objKioskPayment.SUBSERVICEID = "";
        //        //_objKioskPayment.REVENUEHEAD = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RevenueHead"]).ToUpper() + "-" + frmCollection["KioskCharges"].ToString();
        //        _objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
        //        //_objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
        //        // _objKioskPayment.CONSUMERKEY = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"]).ToUpper();
        //        _objKioskPayment.CONSUMERKEY = frmCollection["RequestId"].ToString();
        //        _objKioskPayment.CONSUMERNAME = Convert.ToString(Session["KioskCtznName"]).ToUpper();
        //        _objKioskPayment.SSOID = Convert.ToString(Session["KioskSSOId"]).ToUpper();
        //        // _objKioskPayment.OFFICECODE = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["OfficeCode"]).ToUpper();

        //        if (Session["EmitraDivCode"] != null && Convert.ToString(Session["EmitraDivCode"]) != "")
        //        {
        //            _objKioskPayment.OFFICECODE = Convert.ToString(Session["EmitraDivCode"]).ToUpper(); // "FORESTHQ"; //
        //        }
        //        else
        //        {
        //            TempData["EmitraDivCode"] = "Office Code Not Found";
        //            return RedirectToAction("KioskDashboard", "KioskDashboard");
        //        }

        //        _objKioskPayment.COMMTYPE = "2";
        //        _objKioskPayment.SSOTOKEN = Convert.ToString(Session["SSOTOKEN"]);
        //        eMitraObjectForPaymentChecksum _csum = new eMitraObjectForPaymentChecksum();
        //        _objKioskPayment.CHECKSUM = _csum.GetCheckSum(_objKioskPayment);


        //        string encData = FMDSS.Models.EncodingDecoding.Encrypt(JsonConvert.SerializeObject(_objKioskPayment), "E-m!tr@2016");
        //        var client = new RestClient(baseAddress);
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        //        request.AddParameter("application/x-www-form-urlencoded", "encData='" + encData + "'", ParameterType.RequestBody);

        //        Stopwatch timer = new Stopwatch();
        //        timer.Start();

        //        IRestResponse response = client.Execute(request);

        //        string decVal = FMDSS.Models.EncodingDecoding.Decrypt(response.Content.ToString(), "E-m!tr@2016");

        //        kud.EmitraLOGJsone(decVal, _objKioskPayment.REQUESTID, Convert.ToString(UserID));
        //        eMitraObjForPayment _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(decVal);


        //        timer.Stop();
        //        TimeSpan timeTaken = timer.Elapsed;

        //        if (timeTaken.Seconds > Convert.ToInt16(kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "SERVICE_RESPONSE_TIME")))
        //        {
        //            #region Email and SMS
        //            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
        //            string name = Convert.ToString(timeTaken.Seconds) + " sec for Request ID " + _objKiosk.REQUESTID;

        //            objSMSandEMAILtemplate.SendMailComman("ALL", "EmitrakioskbacktobackserviceDeley", "", name, "", "", "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

        //            #endregion
        //        }



        //        if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
        //        {
        //            DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");

        //            if (DT.Rows.Count > 0)
        //            {
        //                if (Convert.ToString(DT.Rows[0][0]) == "1")
        //                {
        //                    DataTable DT2 = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "2", "1");
        //                    if (DT2.Rows.Count > 0)
        //                    {
        //                        _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
        //                        _objKiosk.COMMTYPE = "True";
        //                        _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
        //                    }
        //                    else
        //                    {
        //                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                    }
        //                }
        //                else
        //                {
        //                    _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                }
        //            }
        //            else
        //            {
        //                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                _objKiosk.COMMTYPE = "True";

        //            }
        //        }
        //        else if (_objKiosk.TRANSACTIONSTATUS == "FAILURE")
        //        {

        //            // var TransactionVerificationURL = System.Configuration.ConfigurationManager.AppSettings["TransactionVerificationURL"]; //string.Empty; //
        //            //TransactionVerificationURL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/getTokenVerifyNewProcessByRequestId";
        //            var TransactionVerificationURL = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackTransactionVerificationService", "GETSERVICEURL");

        //            VerifyTransaction _objVerifyTransaction = new VerifyTransaction();

        //            _objVerifyTransaction.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
        //            _objVerifyTransaction.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
        //            _objVerifyTransaction.REQUESTID = frmCollection["RequestId"].ToString();
        //            _objVerifyTransaction.SSOTOKEN = "0";

        //            eMitraObjectForPaymentChecksum _csum2 = new eMitraObjectForPaymentChecksum();
        //            _objVerifyTransaction.CHECKSUM = _csum2.GetCheckSumForVerifyTrans(_objVerifyTransaction);

        //            var Data = JsonConvert.SerializeObject(_objVerifyTransaction);
        //            var client2 = new RestClient(TransactionVerificationURL + "?data=" + Data + "");
        //            var request2 = new RestRequest(Method.POST);

        //            IRestResponse response2 = client2.Execute(request2);
        //            _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(response2.Content.ToString());
        //            _objKiosk.TRANSAMT = _objKiosk.AMT;

        //            if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
        //            {
        //                DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");

        //                if (DT.Rows.Count > 0)
        //                {
        //                    if (Convert.ToString(DT.Rows[0][0]) == "1")
        //                    {

        //                        DataTable DT2 = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "2", "1");
        //                        if (DT2.Rows.Count > 0)
        //                        {
        //                            _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
        //                            _objKiosk.COMMTYPE = "True";
        //                            _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
        //                        }
        //                        else
        //                        {
        //                            _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                    }
        //                }
        //                else
        //                {

        //                    _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";

        //                }
        //            }
        //            else
        //            {
        //                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //            }

        //        }
        //        else if (_objKiosk.TRANSACTIONSTATUS == "ERROR")
        //        {
        //            _objKiosk.TRANSACTIONSTATUS = _objKiosk.MSG;

        //            #region Email and SMS
        //            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
        //            string name = "[ " + _objKiosk.REQUESTID + " ] " + _objKiosk.MSG;

        //            objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskEndError", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

        //            #endregion

        //        }


        //        return PartialView("KioskTransactionStatus", _objKiosk);
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //        return null;
        //    }
        //}

        [HttpPost]
        public ActionResult PayExtraInventory(FormCollection frmCollection)
        {

            KioskUserDetail kud = new KioskUserDetail();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                //var baseAddress = System.Configuration.ConfigurationManager.AppSettings["EmitraBacktoBackURL"];//UAT Changes

                var baseAddress = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "GETSERVICEURL");

                eMitraObjForPayment _objKioskPayment = new eMitraObjForPayment();
                //_objKioskPayment.MERCHANTCODE = System.Configuration.ConfigurationManager.AppSettings["MerchantCode"].ToUpper();
                _objKioskPayment.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
                _objKioskPayment.REQUESTID = frmCollection["RequestId"].ToString();
                _objKioskPayment.REQTIMESTAMP = DateTime.Now.Ticks.ToString();
                //_objKioskPayment.SERVICEID = System.Configuration.ConfigurationManager.AppSettings["EMitraServiceId"].ToUpper();
                _objKioskPayment.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
                // _objKioskPayment.SUBSERVICEID = Convert.ToString(frmCollection["SubServiceId"]).ToUpper();
                _objKioskPayment.SUBSERVICEID = "";
                //_objKioskPayment.REVENUEHEAD = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RevenueHead"]).ToUpper() + "-" + frmCollection["KioskCharges"].ToString();
                _objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
                //_objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
                // _objKioskPayment.CONSUMERKEY = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"]).ToUpper();
                _objKioskPayment.CONSUMERKEY = frmCollection["RequestId"].ToString();
                _objKioskPayment.CONSUMERNAME = Convert.ToString(Session["KioskCtznName"]).ToUpper();
                _objKioskPayment.SSOID = Convert.ToString(Session["KioskSSOId"]).ToUpper();
                // _objKioskPayment.OFFICECODE = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["OfficeCode"]).ToUpper();

                if (Session["EmitraDivCode"] != null && Convert.ToString(Session["EmitraDivCode"]) != "")
                {
                    _objKioskPayment.OFFICECODE = Convert.ToString(Session["EmitraDivCode"]).ToUpper(); // "FORESTHQ"; // //UAT Changes
                   //  _objKioskPayment.OFFICECODE = "DIV003";
                }
                else
                {
                    TempData["EmitraDivCode"] = "Office Code Not Found";
                    return RedirectToAction("KioskDashboard", "KioskDashboard");
                }

                //_objKioskPayment.COMMTYPE = "2";
                _objKioskPayment.COMMTYPE = "3"; ////Change by Amit for change emitra latter number REF No : F11(135)/doit/project 2012 / pt-3-03716 (27-08-2020)
                _objKioskPayment.SSOTOKEN = Convert.ToString(Session["SSOTOKEN"]);
                eMitraObjectForPaymentChecksum _csum = new eMitraObjectForPaymentChecksum();
                _objKioskPayment.CHECKSUM = _csum.GetCheckSum(_objKioskPayment);


                string encData = FMDSS.Models.EncodingDecoding.Encrypt(JsonConvert.SerializeObject(_objKioskPayment), "E-m!tr@2016");
                var client = new RestClient(baseAddress);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "encData='" + encData + "'", ParameterType.RequestBody);

                Stopwatch timer = new Stopwatch();
                timer.Start();

                IRestResponse response = client.Execute(request);

                string decVal = FMDSS.Models.EncodingDecoding.Decrypt(response.Content.ToString(), "E-m!tr@2016");

                kud.EmitraLOGJsone(decVal, _objKioskPayment.REQUESTID, Convert.ToString(UserID));
                eMitraObjForPayment _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(decVal);


                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;

                if (timeTaken.Seconds > Convert.ToInt16(kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "SERVICE_RESPONSE_TIME")))
                {
                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    string name = Convert.ToString(timeTaken.Seconds) + " sec for Request ID " + _objKiosk.REQUESTID;

                    objSMSandEMAILtemplate.SendMailComman("ALL", "EmitrakioskbacktobackserviceDeley", "", name, "", "", "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                    #endregion
                }

                //DataTable DTIsTicketBooking = kud.IsTicketBooking(Convert.ToString(Session["EmitrServiceId"]));

                //Boolean IsTicketBooking = false;

                //if (DTIsTicketBooking.Rows.Count > 0)
                //{
                //    IsTicketBooking = Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]);
                //}

                // ViewBag.IsTicketBooking = IsTicketBooking;
                if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
                {

                    //if (Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]) == true)
                    //{

                    DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");

                    if (DT.Rows.Count > 0)
                    {
                        if (Convert.ToString(DT.Rows[0][0]) == "1")
                        {
                            DataTable DT2 = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "2", "1");
                            if (DT2.Rows.Count > 0)
                            {
                                _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                                _objKiosk.COMMTYPE = "True";
                                _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
                            }
                            else
                            {
                                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                            }
                        }
                        else
                        {
                            _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                        }
                    }


                    //}


                }
                else if (_objKiosk.TRANSACTIONSTATUS == "FAILURE")
                {

                    // var TransactionVerificationURL = System.Configuration.ConfigurationManager.AppSettings["TransactionVerificationURL"]; //string.Empty; //
                    //  var TransactionVerificationURL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/getTokenVerifyNewProcessByRequestId";
                    var TransactionVerificationURL = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackTransactionVerificationService", "GETSERVICEURL");

                    VerifyTransaction _objVerifyTransaction = new VerifyTransaction();

                    _objVerifyTransaction.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
                    _objVerifyTransaction.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
                    _objVerifyTransaction.REQUESTID = frmCollection["RequestId"].ToString();
                    _objVerifyTransaction.SSOTOKEN = "0";

                    eMitraObjectForPaymentChecksum _csum2 = new eMitraObjectForPaymentChecksum();
                    _objVerifyTransaction.CHECKSUM = _csum2.GetCheckSumForVerifyTrans(_objVerifyTransaction);

                    var Data = JsonConvert.SerializeObject(_objVerifyTransaction);
                    var client2 = new RestClient(TransactionVerificationURL + "?data=" + Data + "");
                    var request2 = new RestRequest(Method.POST);

                    IRestResponse response2 = client2.Execute(request2);
                    _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(response2.Content.ToString());
                    _objKiosk.TRANSAMT = _objKiosk.AMT;

                    //if (Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]) == true)
                    //{
                    if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
                    {
                        DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");

                        if (DT.Rows.Count > 0)
                        {
                            if (Convert.ToString(DT.Rows[0][0]) == "1")
                            {

                                DataTable DT2 = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "2", "1");
                                if (DT2.Rows.Count > 0)
                                {
                                    _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                                    _objKiosk.COMMTYPE = "True";
                                    _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
                                }
                                else
                                {
                                    _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                                }
                            }
                            else
                            {
                                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                            }
                        }
                    }
                    else
                    {
                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                    }

                    // }

                }
                else if (_objKiosk.TRANSACTIONSTATUS == "ERROR")
                {
                    _objKiosk.TRANSACTIONSTATUS = _objKiosk.MSG;

                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    string name = "[ " + _objKiosk.REQUESTID + " ] " + _objKiosk.MSG;

                    objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskEndError", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                    #endregion

                }

                return PartialView("KioskTransactionStatusExtraInventory", _objKiosk);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }

        //// UAT Section BELOW
        //[HttpPost]
        //public ActionResult Pay(FormCollection frmCollection)
        //{

        //    KioskUserDetail kud = new KioskUserDetail();

        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
        //    try
        //    {


        //        //added  by shaan for testing
        //        var baseAddress = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA";  //UAT
        //        eMitraObjForPayment _objKioskPayment = new eMitraObjForPayment();
        //        //_objKioskPayment.MERCHANTCODE = System.Configuration.ConfigurationManager.AppSettings["MerchantCode"].ToUpper();
        //        _objKioskPayment.MERCHANTCODE = frmCollection["MerchantCode"].ToString();

        //        _objKioskPayment.REQUESTID = frmCollection["RequestId"].ToString();
        //        _objKioskPayment.REQTIMESTAMP = DateTime.Now.Ticks.ToString();
        //        //_objKioskPayment.SERVICEID = System.Configuration.ConfigurationManager.AppSettings["EMitraServiceId"].ToUpper();
        //        _objKioskPayment.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
        //        // _objKioskPayment.SUBSERVICEID = Convert.ToString(frmCollection["SubServiceId"]).ToUpper();
        //        _objKioskPayment.SUBSERVICEID = "";
        //        // 900 - 0.00 | 840 - 05.00
        //        //_objKioskPayment.REVENUEHEAD = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RevenueHead"]).ToUpper() + "-" + frmCollection["KioskCharges"].ToString();
        //        //var strArrAmt = frmCollection["RevenueHead"].ToString().Split('|');
        //        //_objKioskPayment.REVENUEHEAD = "900-0.00|"+ strArrAmt[2];
        //        _objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
        //        //_objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
        //        // _objKioskPayment.CONSUMERKEY = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"]).ToUpper();
        //        _objKioskPayment.CONSUMERKEY = frmCollection["RequestId"].ToString();
        //        _objKioskPayment.CONSUMERNAME = Convert.ToString(Session["KioskCtznName"]).ToUpper();
        //        _objKioskPayment.SSOID = Convert.ToString(Session["KioskSSOId"]).ToUpper();
        //        // _objKioskPayment.OFFICECODE = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["OfficeCode"]).ToUpper();

        //        if (Session["EmitraDivCode"] != null && Convert.ToString(Session["EmitraDivCode"]) != "")
        //        {
        //            _objKioskPayment.OFFICECODE = "FORESTHQ"; // //UAT Changes
        //            //_objKioskPayment.OFFICECODE = "DIV003";  //added  by shaan for testing
        //        }
        //        else
        //        {
        //            TempData["EmitraDivCode"] = "Office Code Not Found";
        //            return RedirectToAction("KioskDashboard", "KioskDashboard");
        //        }

        //        _objKioskPayment.COMMTYPE = "3";   //added  by shaan for testing UAT
        //        _objKioskPayment.SSOTOKEN = Convert.ToString(Session["SSOTOKEN"]);
        //        eMitraObjectForPaymentChecksum _csum = new eMitraObjectForPaymentChecksum();
        //        _objKioskPayment.CHECKSUM = _csum.GetCheckSum(_objKioskPayment);


        //        string encData = FMDSS.Models.EncodingDecoding.Encrypt(JsonConvert.SerializeObject(_objKioskPayment), "E-m!tr@2016");


        //        var client = new RestClient(baseAddress);
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        //        request.AddParameter("application/x-www-form-urlencoded", "encData='" + encData + "'", ParameterType.RequestBody);

        //        Stopwatch timer = new Stopwatch();
        //        timer.Start();

        //        IRestResponse response = client.Execute(request);

        //        string decVal = FMDSS.Models.EncodingDecoding.Decrypt(response.Content.ToString(), "E-m!tr@2016");


        //        kud.EmitraLOGJsone(decVal, _objKioskPayment.REQUESTID, Convert.ToString(UserID));
        //        eMitraObjForPayment _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(decVal);

        //        kud.SaveKioskEmitraResponse(_objKiosk);
        //        timer.Stop();
        //        TimeSpan timeTaken = timer.Elapsed;

        //        if (timeTaken.Seconds > Convert.ToInt16(kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "SERVICE_RESPONSE_TIME")))
        //        {
        //            #region Email and SMS
        //            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
        //            string name = Convert.ToString(timeTaken.Seconds) + " sec for Request ID " + _objKiosk.REQUESTID;

        //            objSMSandEMAILtemplate.SendMailComman("ALL", "EmitrakioskbacktobackserviceDeley", "", name, "", "", "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

        //            #endregion
        //        }

        //        //DataTable DTIsTicketBooking = kud.IsTicketBooking(Convert.ToString(Session["EmitrServiceId"]));

        //        //Boolean IsTicketBooking = false;

        //        //if (DTIsTicketBooking.Rows.Count > 0)
        //        //{
        //        //    IsTicketBooking = Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]);
        //        //}

        //        // ViewBag.IsTicketBooking = IsTicketBooking;
        //        if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
        //        {

        //            //if (Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]) == true)
        //            //{

        //            DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");

        //            if (DT.Rows.Count > 0)
        //            {
        //                if (Convert.ToString(DT.Rows[0][0]) == "1")
        //                {
        //                    DataTable DT2 = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "2", "1");
        //                    if (DT2.Rows.Count > 0)
        //                    {
        //                        _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
        //                        _objKiosk.COMMTYPE = "True";
        //                        _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
        //                    }
        //                    else
        //                    {
        //                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                    }
        //                }
        //                else
        //                {
        //                    _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                }
        //            }
        //            kud.SaveKioskEmitraResponse(_objKiosk);
        //        }
        //        else if (_objKiosk.TRANSACTIONSTATUS == "FAILURE")
        //        {

        //            // var TransactionVerificationURL = System.Configuration.ConfigurationManager.AppSettings["TransactionVerificationURL"]; //string.Empty; //
        //            //  var TransactionVerificationURL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/getTokenVerifyNewProcessByRequestId";
        //            var TransactionVerificationURL = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackTransactionVerificationService", "GETSERVICEURL");

        //            VerifyTransaction _objVerifyTransaction = new VerifyTransaction();

        //            _objVerifyTransaction.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
        //            _objVerifyTransaction.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
        //            _objVerifyTransaction.REQUESTID = frmCollection["RequestId"].ToString();
        //            _objVerifyTransaction.SSOTOKEN = "0";

        //            eMitraObjectForPaymentChecksum _csum2 = new eMitraObjectForPaymentChecksum();
        //            _objVerifyTransaction.CHECKSUM = _csum2.GetCheckSumForVerifyTrans(_objVerifyTransaction);

        //            var Data = JsonConvert.SerializeObject(_objVerifyTransaction);
        //            var client2 = new RestClient(TransactionVerificationURL + "?data=" + Data + "");
        //            var request2 = new RestRequest(Method.POST);

        //            IRestResponse response2 = client2.Execute(request2);
        //            _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(response2.Content.ToString());
        //            _objKiosk.TRANSAMT = _objKiosk.AMT;

        //            //if (Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]) == true)
        //            //{
        //            Session["_objKioskPayment"] = _objKioskPayment;/////Change by Amit on 05-02-2021
        //            if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
        //            {
        //                DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");

        //                if (DT.Rows.Count > 0)
        //                {
        //                    if (Convert.ToString(DT.Rows[0][0]) == "1")
        //                    {

        //                        DataTable DT2 = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "2", "1");
        //                        if (DT2.Rows.Count > 0)
        //                        {
        //                            _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
        //                            _objKiosk.COMMTYPE = "True";
        //                            _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
        //                        }
        //                        else
        //                        {
        //                            _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
        //            }
        //            kud.SaveKioskEmitraResponse(_objKiosk);                   
        //        }
        //        else if (_objKiosk.TRANSACTIONSTATUS == "ERROR")
        //        {
        //            _objKiosk.TRANSACTIONSTATUS = _objKiosk.MSG;
        //            kud.SaveKioskEmitraResponse(_objKiosk);

        //            #region Email and SMS
        //            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
        //            string name = "[ " + _objKiosk.REQUESTID + " ] " + _objKiosk.MSG;

        //            objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskEndError", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

        //            #endregion

        //        }

        //        return PartialView("KioskTransactionStatus", _objKiosk);
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //        return null;
        //    }
        //}
        //// UAT Section ABOVE

        //////Live Section Below ON When Go Live
        [HttpPost]
        public ActionResult Pay(FormCollection frmCollection)
        {

            KioskUserDetail kud = new KioskUserDetail();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                // var baseAddress = System.Configuration.ConfigurationManager.AppSettings["EmitraBacktoBackURL"];//UAT Changes

                var baseAddress = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "GETSERVICEURL"); //live

                //added  by shaan for testing
                // var baseAddress = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA";  //UAT
                eMitraObjForPayment _objKioskPayment = new eMitraObjForPayment();
                //_objKioskPayment.MERCHANTCODE = System.Configuration.ConfigurationManager.AppSettings["MerchantCode"].ToUpper();
                _objKioskPayment.MERCHANTCODE = frmCollection["MerchantCode"].ToString();

                _objKioskPayment.REQUESTID = frmCollection["RequestId"].ToString();
                _objKioskPayment.REQTIMESTAMP = DateTime.Now.Ticks.ToString();
                //_objKioskPayment.SERVICEID = System.Configuration.ConfigurationManager.AppSettings["EMitraServiceId"].ToUpper();
                _objKioskPayment.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
                // _objKioskPayment.SUBSERVICEID = Convert.ToString(frmCollection["SubServiceId"]).ToUpper();
                _objKioskPayment.SUBSERVICEID = "";
                // 900 - 0.00 | 840 - 05.00
                //_objKioskPayment.REVENUEHEAD = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RevenueHead"]).ToUpper() + "-" + frmCollection["KioskCharges"].ToString();
                //var strArrAmt = frmCollection["RevenueHead"].ToString().Split('|');
                //_objKioskPayment.REVENUEHEAD = "900-0.00|"+ strArrAmt[2];
                _objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
                //_objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
                // _objKioskPayment.CONSUMERKEY = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"]).ToUpper();
                _objKioskPayment.CONSUMERKEY = frmCollection["RequestId"].ToString();
                _objKioskPayment.CONSUMERNAME = Convert.ToString(Session["KioskCtznName"]).ToUpper();
                _objKioskPayment.SSOID = Convert.ToString(Session["KioskSSOId"]).ToUpper();
                // _objKioskPayment.OFFICECODE = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["OfficeCode"]).ToUpper();

                if (Session["EmitraDivCode"] != null && Convert.ToString(Session["EmitraDivCode"]) != "")
                {
                    _objKioskPayment.OFFICECODE = Convert.ToString(Session["EmitraDivCode"]).ToUpper();  // Live Office Code
                                                                                                         //_objKioskPayment.OFFICECODE = "FORESTHQ"; // //UAT Changes
                                                                                                         //_objKioskPayment.OFFICECODE = "DIV003";  //added  by shaan for testing
                }
                else
                {
                    TempData["EmitraDivCode"] = "Office Code Not Found";
                    return RedirectToAction("KioskDashboard", "KioskDashboard");
                }


                _objKioskPayment.COMMTYPE = "2"; //Live
                                                 //_objKioskPayment.COMMTYPE = "3";   //added  by shaan for testing UAT
                _objKioskPayment.SSOTOKEN = Convert.ToString(Session["SSOTOKEN"]);
                eMitraObjectForPaymentChecksum _csum = new eMitraObjectForPaymentChecksum();
                _objKioskPayment.CHECKSUM = _csum.GetCheckSum(_objKioskPayment);


                string encData = FMDSS.Models.EncodingDecoding.Encrypt(JsonConvert.SerializeObject(_objKioskPayment), "E-m!tr@2016"); //Live


                var client = new RestClient(baseAddress);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "encData='" + encData + "'", ParameterType.RequestBody);

                Stopwatch timer = new Stopwatch();
                timer.Start();

                IRestResponse response = client.Execute(request);

                string decVal = FMDSS.Models.EncodingDecoding.Decrypt(response.Content.ToString(), "E-m!tr@2016");


                kud.EmitraLOGJsone(decVal, _objKioskPayment.REQUESTID, Convert.ToString(UserID));
                eMitraObjForPayment _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(decVal);
                kud.SaveKioskEmitraResponse(_objKiosk);

                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;

                if (timeTaken.Seconds > Convert.ToInt16(kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "SERVICE_RESPONSE_TIME")))
                {
                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    string name = Convert.ToString(timeTaken.Seconds) + " sec for Request ID " + _objKiosk.REQUESTID;

                    objSMSandEMAILtemplate.SendMailComman("ALL", "EmitrakioskbacktobackserviceDeley", "", name, "", "", "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                    #endregion
                }

                //DataTable DTIsTicketBooking = kud.IsTicketBooking(Convert.ToString(Session["EmitrServiceId"]));

                //Boolean IsTicketBooking = false;

                //if (DTIsTicketBooking.Rows.Count > 0)
                //{
                //    IsTicketBooking = Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]);
                //}

                // ViewBag.IsTicketBooking = IsTicketBooking;
                if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
                {

                    //if (Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]) == true)
                    //{

                    DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");

                    if (DT.Rows.Count > 0)
                    {
                        if (Convert.ToString(DT.Rows[0][0]) == "1")
                        {
                            DataTable DT2 = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "2", "1");
                            if (DT2.Rows.Count > 0)
                            {
                                _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                                _objKiosk.COMMTYPE = "True";
                                _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
                            }
                            else
                            {
                                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                            }
                        }
                        else
                        {
                            _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                        }
                    }
                    kud.SaveKioskEmitraResponse(_objKiosk);
                }
                else if (_objKiosk.TRANSACTIONSTATUS == "FAILURE")
                {

                    // var TransactionVerificationURL = System.Configuration.ConfigurationManager.AppSettings["TransactionVerificationURL"]; //string.Empty; //
                    //  var TransactionVerificationURL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/getTokenVerifyNewProcessByRequestId";
                    var TransactionVerificationURL = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackTransactionVerificationService", "GETSERVICEURL");

                    VerifyTransaction _objVerifyTransaction = new VerifyTransaction();

                    _objVerifyTransaction.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
                    _objVerifyTransaction.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
                    _objVerifyTransaction.REQUESTID = frmCollection["RequestId"].ToString();
                    _objVerifyTransaction.SSOTOKEN = "0";

                    eMitraObjectForPaymentChecksum _csum2 = new eMitraObjectForPaymentChecksum();
                    _objVerifyTransaction.CHECKSUM = _csum2.GetCheckSumForVerifyTrans(_objVerifyTransaction);

                    var Data = JsonConvert.SerializeObject(_objVerifyTransaction);
                    var client2 = new RestClient(TransactionVerificationURL + "?data=" + Data + "");
                    var request2 = new RestRequest(Method.POST);

                    IRestResponse response2 = client2.Execute(request2);
                    _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(response2.Content.ToString());
                    _objKiosk.TRANSAMT = _objKiosk.AMT;

                    //if (Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]) == true)
                    //{
                    Session["_objKioskPayment"] = _objKioskPayment;/////Change by Amit on 05-02-2021
                    if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
                    {
                        DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");

                        if (DT.Rows.Count > 0)
                        {
                            if (Convert.ToString(DT.Rows[0][0]) == "1")
                            {

                                DataTable DT2 = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "2", "1");
                                if (DT2.Rows.Count > 0)
                                {
                                    _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                                    _objKiosk.COMMTYPE = "True";
                                    _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
                                }
                                else
                                {
                                    _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                                }
                            }
                            else
                            {
                                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                            }
                        }
                    }
                    else
                    {
                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                    }

                    kud.SaveKioskEmitraResponse(_objKiosk);

                }
                else if (_objKiosk.TRANSACTIONSTATUS == "ERROR")
                {

                    _objKiosk.TRANSACTIONSTATUS = _objKiosk.MSG;
                    kud.SaveKioskEmitraResponse(_objKiosk);
                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    string name = "[ " + _objKiosk.REQUESTID + " ] " + _objKiosk.MSG;

                    objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskEndError", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                    #endregion

                }

                return PartialView("KioskTransactionStatus", _objKiosk);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }
        //// Live Payment Section Above



        #region Pay Wildlife Kiosk User
        [HttpPost]
        public ActionResult PayWildlife(FormCollection frmCollection)
        {

            KioskUserDetail kud = new KioskUserDetail();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
               // var baseAddress = System.Configuration.ConfigurationManager.AppSettings["EmitraBacktoBackURL"];//UAT UAT CHANGES

                var baseAddress = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "GETSERVICEURL");

                eMitraObjForPayment _objKioskPayment = new eMitraObjForPayment();
                //_objKioskPayment.MERCHANTCODE = System.Configuration.ConfigurationManager.AppSettings["MerchantCode"].ToUpper();
                _objKioskPayment.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
                _objKioskPayment.REQUESTID = frmCollection["RequestId"].ToString();
                _objKioskPayment.REQTIMESTAMP = DateTime.Now.Ticks.ToString();
                //_objKioskPayment.SERVICEID = System.Configuration.ConfigurationManager.AppSettings["EMitraServiceId"].ToUpper();
                _objKioskPayment.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
                // _objKioskPayment.SUBSERVICEID = Convert.ToString(frmCollection["SubServiceId"]).ToUpper();
                _objKioskPayment.SUBSERVICEID = "";
                //_objKioskPayment.REVENUEHEAD = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["RevenueHead"]).ToUpper() + "-" + frmCollection["KioskCharges"].ToString();
                _objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
                //_objKioskPayment.REVENUEHEAD = Convert.ToString(frmCollection["RevenueHead"].ToString());
                // _objKioskPayment.CONSUMERKEY = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"]).ToUpper();
                _objKioskPayment.CONSUMERKEY = frmCollection["RequestId"].ToString();
                _objKioskPayment.CONSUMERNAME = Convert.ToString(Session["KioskCtznName"]).ToUpper();
                _objKioskPayment.SSOID = Convert.ToString(Session["KioskSSOId"]).ToUpper();
                // _objKioskPayment.OFFICECODE = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["OfficeCode"]).ToUpper();

                if (Session["EmitraDivCode"] != null && Convert.ToString(Session["EmitraDivCode"]) != "")
                {
                    _objKioskPayment.OFFICECODE = Convert.ToString(Session["EmitraDivCode"]).ToUpper(); // "FORESTHQ"; //
                  //  _objKioskPayment.OFFICECODE = "FORESTHQ"; // UAT CHANGES
                }
                else
                {
                    TempData["EmitraDivCode"] = "Office Code Not Found";
                    return RedirectToAction("KioskDashboard", "KioskDashboard");
                }

                _objKioskPayment.COMMTYPE = "2";
                _objKioskPayment.SSOTOKEN = Convert.ToString(Session["SSOTOKEN"]);
                eMitraObjectForPaymentChecksum _csum = new eMitraObjectForPaymentChecksum();
                _objKioskPayment.CHECKSUM = _csum.GetCheckSum(_objKioskPayment);


                string encData = FMDSS.Models.EncodingDecoding.Encrypt(JsonConvert.SerializeObject(_objKioskPayment), "E-m!tr@2016");
                var client = new RestClient(baseAddress);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "encData='" + encData + "'", ParameterType.RequestBody);

                Stopwatch timer = new Stopwatch();
                timer.Start();

                IRestResponse response = client.Execute(request);

                string decVal = FMDSS.Models.EncodingDecoding.Decrypt(response.Content.ToString(), "E-m!tr@2016");

                kud.EmitraLOGJsone(decVal, _objKioskPayment.REQUESTID, Convert.ToString(UserID));
                eMitraObjForPayment _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(decVal);


                //****************************** for test only

                //_objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                //_objKiosk.REQUESTID = Convert.ToString(Session["RequestId"]);
                //_objKiosk.REQUESTID = Convert.ToString(Session["RequestId"]);
                //_objKiosk.TRANSAMT = Convert.ToString(Session["totalprice"]);
                //_objKiosk.TRANSACTIONID = DateTime.Now.Ticks.ToString();

                //****************************** for test only;


                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;

                if (timeTaken.Seconds > Convert.ToInt16(kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "SERVICE_RESPONSE_TIME")))
                {
                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    string name = Convert.ToString(timeTaken.Seconds) + " sec for Request ID " + _objKiosk.REQUESTID;

                    objSMSandEMAILtemplate.SendMailComman("ALL", "EmitrakioskbacktobackserviceDeley", "", name, "", "", "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                    #endregion
                }

                //DataTable DTIsTicketBooking = kud.IsTicketBooking(Convert.ToString(Session["EmitrServiceId"]));

                //Boolean IsTicketBooking = false;

                //if (DTIsTicketBooking.Rows.Count > 0)
                //{
                //    IsTicketBooking = Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]);
                //}

                // ViewBag.IsTicketBooking = IsTicketBooking;
                if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
                {

                    //if (Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]) == true)
                    //{

                    DataTable DT = kud.UpdateEmitraKioskTransactionStatusforWildlife(_objKiosk, "1", "1", Convert.ToInt64(Session["USERID"]), _objKioskPayment.REVENUEHEAD);

                    if (DT.Rows.Count > 0)
                    {
                        if (Convert.ToString(DT.Rows[0][0]) == "1")
                        {
                            DataTable DT2 = kud.UpdateEmitraKioskTransactionStatusforWildlife(_objKiosk, "2", "1", Convert.ToInt64(Session["USERID"]), _objKioskPayment.REVENUEHEAD);
                            if (DT2.Rows.Count > 0)
                            {
                                _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                                _objKiosk.COMMTYPE = "True";
                                _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
                            }
                            else
                            {
                                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                            }
                        }
                        else
                        {
                            _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                        }
                    }


                    //}


                }
                else if (_objKiosk.TRANSACTIONSTATUS == "FAILURE")
                {

                    // var TransactionVerificationURL = System.Configuration.ConfigurationManager.AppSettings["TransactionVerificationURL"]; //string.Empty; //
                    //  var TransactionVerificationURL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/getTokenVerifyNewProcessByRequestId";
                    var TransactionVerificationURL = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackTransactionVerificationService", "GETSERVICEURL");

                    VerifyTransaction _objVerifyTransaction = new VerifyTransaction();

                    _objVerifyTransaction.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
                    _objVerifyTransaction.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
                    _objVerifyTransaction.REQUESTID = frmCollection["RequestId"].ToString();
                    _objVerifyTransaction.SSOTOKEN = "0";

                    eMitraObjectForPaymentChecksum _csum2 = new eMitraObjectForPaymentChecksum();
                    _objVerifyTransaction.CHECKSUM = _csum2.GetCheckSumForVerifyTrans(_objVerifyTransaction);

                    var Data = JsonConvert.SerializeObject(_objVerifyTransaction);
                    var client2 = new RestClient(TransactionVerificationURL + "?data=" + Data + "");
                    var request2 = new RestRequest(Method.POST);

                    IRestResponse response2 = client2.Execute(request2);
                    _objKiosk = JsonConvert.DeserializeObject<eMitraObjForPayment>(response2.Content.ToString());
                    _objKiosk.TRANSAMT = _objKiosk.AMT;

                    //if (Convert.ToBoolean(DTIsTicketBooking.Rows[0]["IsTicketBooking"]) == true)
                    //{
                    if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
                    {
                        DataTable DT = kud.UpdateEmitraKioskTransactionStatusforWildlife(_objKiosk, "1", "1", Convert.ToInt64(Session["USERID"]), _objKioskPayment.REVENUEHEAD);

                        if (DT.Rows.Count > 0)
                        {
                            if (Convert.ToString(DT.Rows[0][0]) == "1")
                            {

                                DataTable DT2 = kud.UpdateEmitraKioskTransactionStatusforWildlife(_objKiosk, "2", "1", Convert.ToInt64(Session["USERID"]), _objKioskPayment.REVENUEHEAD);
                                if (DT2.Rows.Count > 0)
                                {
                                    _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                                    _objKiosk.COMMTYPE = "True";
                                    _objKiosk.CHECKSUM = Convert.ToString(DT2.Rows[0][0]);
                                }
                                else
                                {
                                    _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                                }
                            }
                            else
                            {
                                _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                            }
                        }
                    }
                    else
                    {
                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                    }

                    // }

                }
                else if (_objKiosk.TRANSACTIONSTATUS == "ERROR")
                {
                    _objKiosk.TRANSACTIONSTATUS = _objKiosk.MSG;

                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    string name = "[ " + _objKiosk.REQUESTID + " ] " + _objKiosk.MSG;

                    objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskEndError", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                    #endregion

                }

                return PartialView("KioskTransactionStatusWildlife", _objKiosk);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }
        #endregion


        public void verifytransaction()
        {

        }




        public string postt(string URL, string userdetails)
        {
            var webAddr = URL;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = userdetails;

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
        private void postto_page(string URL, string userdetails, string AppName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body style='background-color:#F0F0F0;' onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", URL, false);
            sb.AppendFormat("<div style='float:left; width:100%; height:100%;'>");
            sb.AppendFormat("<div style='float:left; width:100%; height:100%; margin-top:10%;'>	");
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center; font-size:30px; color:#525252; margin:0 0 50px 0;'>Please wait <span style='font-weight:bold;'>{0}</span> Application.</div>", AppName.ToUpper());
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center;'>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("<input type='hidden' name='data' value='{0}'>", userdetails);
            sb.AppendFormat("</div>");
            sb.AppendFormat("<div>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();
        }


        //private string MakeTransactions()
        //{
        //    string data = "{'MERCHANTCODE':'BHAM0616','REQUESTID':'1234453','REQTIMESTAMP':'20160430013622123','SERVICEID': '1214','SUBSERVICEID':'1111', 'REVENUEHEAD':'212-0.00|213-5.00','CONSUMERKEY': '123456-3342-Y-332-2-ADDED','CONSUMERNAME':'xyz', 'SSOID': 'SSOTESTKIOSK', 'OFFICECODE':'STATE123'}";
        //    try
        //    {
        //        //Base String
        //        string baseAddress = "http://103.203.136.34/webServicesRepositoryUat/backtobacktransaction";
        //        //Post Parameters
        //        StringBuilder postData = new StringBuilder();
        //        postData.Append("data=" + HttpUtility.UrlEncode(data));

        //        //Create Web Request
        //        var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
        //        http.Method = "POST";
        //        http.Accept = "application/json";
        //        http.ContentType = "application/x-www-form-urlencoded";

        //        //Start Writing Post parameters to request object
        //        string parsedContent = postData.ToString();
        //        ASCIIEncoding encoding = new ASCIIEncoding();
        //        Byte[] bytes = encoding.GetBytes(parsedContent);
        //        Stream newStream = http.GetRequestStream();
        //        newStream.Write(bytes, 0, bytes.Length);
        //        newStream.Close();


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void GetDistricts()
        {
            Models.Admin.Location _objDistricts = new Models.Admin.Location();
            DataTable dt = new DataTable();
            dt = _objDistricts.District();
            ViewBag.fname = dt;
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                districts.Add(new SelectListItem { Text = @dr["Dist_Name"].ToString(), Value = @dr["Dist_Code"].ToString() });
            }
            ViewBag.districts = districts;


        }


        public ActionResult PaymentByDepartmentalKioskUser(PaymentByDepartmentalKioskUserDetails UserDetails)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                return View(UserDetails);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }



        public JsonResult ADDPaymentByDepartmentalKioskUser(PaymentByDepartmentalKioskUserDetails OBJ)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                OBJ.RequestedId = Encryption.decrypt(OBJ.RequestedId);
                PaymentByDepartmentalKioskUserDetails OBJCT = new PaymentByDepartmentalKioskUserDetails();
                DataTable DT = OBJCT.ADDPaymentByDepartmentalKioskUser(OBJ);

                if (DT.Rows.Count > 0)
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("PrintBoardingPass", "TicketBooking"),
                        isRedirect = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("PrintBoardingPass", "TicketBooking"),
                        isRedirect = false
                    });

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }




        public ActionResult ADDPaymentByDepartmentalKioskUserDetails(PaymentByDepartmentalKioskUserDetails OBJ)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            PaymentByDepartmentalKioskUserDetails OBJCT = new PaymentByDepartmentalKioskUserDetails();
            try
            {
                DataTable DT = OBJCT.ADDPaymentByDepartmentalKioskUser(OBJ);
                //if (Convert.ToString(Session["IsDepartmentalKioskUser"].ToString()) == "True")
                //{
                //    string ss = string.Empty;

                //    ss = Encryption.encrypt(OBJ.RequestedId.ToString());

                //    return RedirectToAction("IssueBoardingPass", "BoardingMaster", new { id = ss.ToString() });
                //}
                //else
                //{

                if (DT != null && DT.Rows.Count > 0)
                {

                    OBJCT.TransactionID = Convert.ToString(DT.Rows[0]["ReceiptNumber"]);
                    OBJCT.RequestedId = Convert.ToString(DT.Rows[0]["RequestedId"]);

                    OBJCT.ModuleName = Convert.ToString(DT.Rows[0]["ModuleDesc"]);
                    OBJCT.ServiceTypeName = Convert.ToString(DT.Rows[0]["ServiceTypeDesc"]);
                    OBJCT.PermissionName = Convert.ToString(DT.Rows[0]["PermissionDesc"]);
                    OBJCT.SubPermissioName = Convert.ToString(DT.Rows[0]["SubPermissionDesc"]);
                    OBJCT.PaidForCitizenName = Convert.ToString(DT.Rows[0]["PAIDFOR"]);
                    OBJCT.PaidBy = Convert.ToString(DT.Rows[0]["PAIDBY"]);
                    OBJCT.PaidOn = Convert.ToString(DT.Rows[0]["PaidOn"]);
                    OBJCT.PaidAmount = Convert.ToDecimal(DT.Rows[0]["PaidAmount"]);
                    OBJCT.PaymentMode = Convert.ToString(DT.Rows[0]["PaymentMode"]);

                    OBJCT.BankName = Convert.ToString(DT.Rows[0]["BankName"]);
                    OBJCT.IFSCCode = Convert.ToString(DT.Rows[0]["IFSCCode"]);
                    OBJCT.ChequeNumber = Convert.ToString(DT.Rows[0]["ChequeNumber"]);
                    OBJCT.ChequeDate = Convert.ToString(DT.Rows[0]["ChequeDate"]);
                    return PartialView("_partialPaymentByDepartmentalKioskUserLISTS", OBJCT);
                }
                else
                    return PartialView("_partialPaymentByDepartmentalKioskUserLISTS", null);
                // }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }


        public ActionResult ADDZooPaymentByDepartmentalKioskUserDetails(PaymentByDepartmentalKioskUserDetails OBJ)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            PaymentByDepartmentalKioskUserDetails OBJCT = new PaymentByDepartmentalKioskUserDetails();
            try
            {
                DataTable DT = OBJCT.ADDZOOPaymentByDepartmentalKioskUser(OBJ);
                ViewBag.TicketStatus = "failed";
                if (DT != null && DT.Rows.Count > 0)
                {

                    OBJCT.TransactionID = Convert.ToString(DT.Rows[0]["ReceiptNumber"]);
                    OBJCT.RequestedId = Convert.ToString(DT.Rows[0]["RequestedId"]);

                    OBJCT.ModuleName = Convert.ToString(DT.Rows[0]["ModuleDesc"]);
                    OBJCT.ServiceTypeName = Convert.ToString(DT.Rows[0]["ServiceTypeDesc"]);
                    OBJCT.PermissionName = Convert.ToString(DT.Rows[0]["PermissionDesc"]);
                    OBJCT.SubPermissioName = Convert.ToString(DT.Rows[0]["SubPermissionDesc"]);
                    OBJCT.PaidForCitizenName = Convert.ToString(DT.Rows[0]["PAIDFOR"]);
                    OBJCT.PaidBy = Convert.ToString(DT.Rows[0]["PAIDBY"]);
                    OBJCT.PaidOn = Convert.ToString(DT.Rows[0]["PaidOn"]);
                    OBJCT.PaidAmount = Convert.ToDecimal(DT.Rows[0]["PaidAmount"]);
                    OBJCT.PaymentMode = Convert.ToString(DT.Rows[0]["PaymentMode"]);

                    OBJCT.BankName = Convert.ToString(DT.Rows[0]["BankName"]);
                    OBJCT.IFSCCode = Convert.ToString(DT.Rows[0]["IFSCCode"]);
                    OBJCT.ChequeNumber = Convert.ToString(DT.Rows[0]["ChequeNumber"]);
                    OBJCT.ChequeDate = Convert.ToString(DT.Rows[0]["ChequeDate"]);
                    OBJCT.GuideName = Convert.ToString(DT.Rows[0]["ZooBookingId"]); // GuideName as ZooBookingId
                    ViewBag.TicketStatus = "SUCCESS";

                    return PartialView("_partialPaymentByDepartmentalKioskUserLISTS", OBJCT);

                }
                else
                    return PartialView("_partialPaymentByDepartmentalKioskUserLISTS", null);
                // }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }


        public ActionResult ADDNurseryPaymentByDepartmentalKioskUserDetails(PaymentByDepartmentalKioskUserDetails OBJ)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            PaymentByDepartmentalKioskUserDetails OBJCT = new PaymentByDepartmentalKioskUserDetails();
            try
            {

                DataTable DT = OBJCT.ADDNurseryPaymentByDepartmentalKioskUser(OBJ);
                ViewBag.TicketStatus = "failed";
                if (DT != null && DT.Rows.Count > 0)
                {

                    OBJCT.TransactionID = Convert.ToString(DT.Rows[0]["ReceiptNumber"]);
                    OBJCT.RequestedId = Convert.ToString(DT.Rows[0]["RequestedId"]);

                    OBJCT.ModuleName = Convert.ToString(DT.Rows[0]["ModuleDesc"]);
                    OBJCT.ServiceTypeName = Convert.ToString(DT.Rows[0]["ServiceTypeDesc"]);
                    OBJCT.PermissionName = Convert.ToString(DT.Rows[0]["PermissionDesc"]);
                    OBJCT.SubPermissioName = Convert.ToString(DT.Rows[0]["SubPermissionDesc"]);
                    OBJCT.PaidForCitizenName = Convert.ToString(DT.Rows[0]["PAIDFOR"]);
                    OBJCT.PaidBy = Convert.ToString(DT.Rows[0]["PAIDBY"]);
                    OBJCT.PaidOn = Convert.ToString(DT.Rows[0]["PaidOn"]);
                    OBJCT.PaidAmount = Convert.ToDecimal(DT.Rows[0]["PaidAmount"]);
                    OBJCT.PaymentMode = Convert.ToString(DT.Rows[0]["PaymentMode"]);

                    OBJCT.BankName = Convert.ToString(DT.Rows[0]["BankName"]);
                    OBJCT.IFSCCode = Convert.ToString(DT.Rows[0]["IFSCCode"]);
                    OBJCT.ChequeNumber = Convert.ToString(DT.Rows[0]["ChequeNumber"]);
                    OBJCT.ChequeDate = Convert.ToString(DT.Rows[0]["ChequeDate"]);
                    OBJCT.GuideName = Convert.ToString(DT.Rows[0]["RequestedId"]); // GuideName as ZooBookingId
                    ViewBag.TicketStatus = "SUCCESS";

                    return PartialView("_partialPaymentByDepartmentalKioskUserNurseryLISTS", OBJCT);

                }
                else
                    return PartialView("_partialPaymentByDepartmentalKioskUserLISTS", null);
                // }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }


        #region Payment Dept User Developed by Rajveer 
        public ActionResult ADDNurseryPaymentByDepartmentalUserDetailsDeptUser(PaymentByDepartmentalKioskUserDetails OBJ, HttpPostedFileBase fileUpload)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            PaymentByDepartmentalKioskUserDetails OBJCT = new PaymentByDepartmentalKioskUserDetails();
            try
            {
                DataTable DT = OBJCT.ADDNurseryPaymentByDepartmentalKioskUserDeptUsers(OBJ);
                ViewBag.TicketStatus = "failed";
                if (DT != null && DT.Rows.Count > 0)
                {
                    OBJCT.TransactionID = Convert.ToString(DT.Rows[0]["ReceiptNumber"]);
                    OBJCT.RequestedId = Convert.ToString(DT.Rows[0]["RequestedId"]);

                    OBJCT.ModuleName = Convert.ToString(DT.Rows[0]["ModuleDesc"]);
                    OBJCT.ServiceTypeName = Convert.ToString(DT.Rows[0]["ServiceTypeDesc"]);
                    OBJCT.PermissionName = Convert.ToString(DT.Rows[0]["PermissionDesc"]);
                    OBJCT.SubPermissioName = Convert.ToString(DT.Rows[0]["SubPermissionDesc"]);
                    OBJCT.PaidForCitizenName = Convert.ToString(DT.Rows[0]["PAIDFOR"]);
                    OBJCT.PaidBy = Convert.ToString(DT.Rows[0]["PAIDBY"]);
                    OBJCT.PaidOn = Convert.ToString(DT.Rows[0]["PaidOn"]);
                    OBJCT.PaidAmount = Convert.ToDecimal(DT.Rows[0]["PaidAmount"]);
                    OBJCT.PaymentMode = Convert.ToString(DT.Rows[0]["PaymentMode"]);

                    OBJCT.BankName = Convert.ToString(DT.Rows[0]["BankName"]);
                    OBJCT.IFSCCode = Convert.ToString(DT.Rows[0]["IFSCCode"]);
                    OBJCT.ChequeNumber = Convert.ToString(DT.Rows[0]["ChequeNumber"]);
                    OBJCT.ChequeDate = Convert.ToString(DT.Rows[0]["ChequeDate"]);
                    OBJCT.GuideName = Convert.ToString(DT.Rows[0]["RequestedId"]); // GuideName as ZooBookingId
                    ViewBag.TicketStatus = "SUCCESS";

                    #region upload File
                    if (fileUpload != null)
                    {
                        int i = 0;
                        string FilePath = "/Documents/NurseryDocuments/";
                        {
                            if (!string.IsNullOrEmpty(fileUpload.FileName))
                            {
                                string FileFullName = DateTime.Now.Ticks + "_" + fileUpload.FileName;
                                string path = Path.Combine(FilePath, FileFullName);
                                Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
                                OBJ.DocumentsSSOUsers = path;
                                i++;
                            }

                        }
                    }
                    OBJ.RequestedId = OBJCT.RequestedId;
                    DataTable DT1 = OBJCT.SaveNursueyDocuments(OBJ);
                    #endregion

                    return PartialView("_partialPaymentByDepartmentalKioskUserNurseryLISTS", OBJCT);

                }
                else
                    return PartialView("_partialPaymentByDepartmentalKioskUserLISTS", null);
                // }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }

        #endregion

    }
}
