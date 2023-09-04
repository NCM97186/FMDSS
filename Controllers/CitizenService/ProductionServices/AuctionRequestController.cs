using FMDSS.Entity.DOD.ViewModel;
using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.CitizenService.ProductionServices
{
    public class AuctionRequestController : BaseController
    {
        #region [Properties & Variables]
        private IAuctionRequestRepository _repository;
        private ICommonRepository _commonRepository;
        SMS_EMail_Services _objMailSMS = new SMS_EMail_Services();
        //Models.CitizenService.PermissionServices.FixedLandUsage _objmodel = new Models.CitizenService.PermissionServices.FixedLandUsage();
        int ModuleID = 3;
        #endregion

        #region [Constructor]
        public AuctionRequestController()
        {
            _repository = new AuctionRequestRepository();
            _commonRepository = new CommonRepository();
        }
        #endregion

        #region [Customer Part]
        public ActionResult Index()
        {
            var model = _repository.GetAuctionDetailsForCustomer();
            return View(model);
        }

        public ActionResult ApplyNewAuction()
        {
            var model = _repository.GetNoticeDetailsForAuction();
            ViewBag.AppliedAuctionList = _repository.GetAuctionDetailsForCustomer();
            return View(model);
        }

        public ActionResult AuctionRequest(long noticeID)
        {
            AuctionVM model = _repository.GetNoticeDetails(noticeID);
            return View(model);
        }

        public ActionResult AuctionPayment(long auctionID)
        {
            AuctionVM model = _repository.GetNoticeDetails(auctionID, "5");
            return View(model);
        }

        [HttpPost]
        public ActionResult AuctionPayment(AuctionVM model, string CaptchaValue, string captchaPrefix)
        {
            if (!CaptchaValue.Equals(Convert.ToString(Session["Captcha" + captchaPrefix])))
            {
                TempData["ReturnMsg"] = "Captcha value is not valid.";
                TempData["IsError"] = true;
                return Json(new { ReturnMsg = "Captcha value is not valid.", IsError = true, ObjID = model.NoticeID }, JsonRequestBehavior.AllowGet);
            }

            Session["NRequestID"] = string.Format("AucPendingAmt/{0}/{1}", model.AuctionID, DateTime.Now.Ticks);
            Session["NBiddingAmt"] = model.PendingAmount;
            Session["AuctionRequest"] = model;
            return PartialView("_AuctionPayment", model);
        }

        [HttpPost]
        public ActionResult AuctionRequest_Old(AuctionVM model, string CaptchaValue, string captchaPrefix)
        {
            if (model.DODProductList == null || model.DODProductList.Count == 0)
            {
                TempData["ReturnMsg"] = "There is no item available for auction.";
                TempData["IsError"] = true;
                return Json(new { ReturnMsg = "There is no item available for auction.", IsError = true, ObjID = model.NoticeID }, JsonRequestBehavior.AllowGet);
            }
            else if (!CaptchaValue.Equals(Convert.ToString(Session["Captcha" + captchaPrefix])))
            {
                TempData["ReturnMsg"] = "Captcha value is not valid.";
                TempData["IsError"] = true;
                return Json(new { ReturnMsg = "Captcha value is not valid.", IsError = true, ObjID = model.NoticeID }, JsonRequestBehavior.AllowGet);
            }
            var msg = _repository.ValidateUser(model.NoticeID);
            if (msg.IsError)
            {
                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                return Json(new { ReturnMsg = "Captcha value is not valid.", IsError = true, ObjID = model.NoticeID }, JsonRequestBehavior.AllowGet);
            }

            Session["NRequestID"] = string.Format("Auc/{0}/{1}", model.NoticeID, DateTime.Now.Ticks);
            Session["NBiddingAmt"] = model.EarnMoneyDeposit;
            Session["AuctionRequest"] = model;
            return PartialView("_AuctionPayment", model);
        }

        [HttpPost]
        public ActionResult AuctionRequest(AuctionVM model, string CaptchaValue, string captchaPrefix)
        {
            if (model.DODProductList == null || model.DODProductList.Count == 0)
            {
                TempData["ReturnMsg"] = "There is no item available for auction.";
                TempData["IsError"] = true;
                return Json(new { ReturnMsg = "There is no item available for auction.", IsError = true, ObjID = model.NoticeID }, JsonRequestBehavior.AllowGet);
            }
            else if (!CaptchaValue.Equals(Convert.ToString(Session["Captcha" + captchaPrefix])))
            {
                TempData["ReturnMsg"] = "Captcha value is not valid.";
                TempData["IsError"] = true;
                return Json(new { ReturnMsg = "Captcha value is not valid.", IsError = true, ObjID = model.NoticeID }, JsonRequestBehavior.AllowGet);
            }
            var msg = _repository.ValidateUser(model.NoticeID);
            if (msg.IsError)
            {
                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                return Json(new { ReturnMsg = "Captcha value is not valid.", IsError = true, ObjID = model.NoticeID }, JsonRequestBehavior.AllowGet);
            }

            model.RequestedId = string.Format("Auc/{0}/{1}", model.NoticeID, DateTime.Now.Ticks);
            _repository.SaveAuctionDetailsWithoutPayment(model);
            TempData["ReturnMsg"] = "Auction applied successfully, Request No is #" + model.RequestedId;
            TempData["IsError"] = false;
            return Json(new { ReturnMsg = "Auction applied successfully, Request No is #" + model.RequestedId, IsError = false, ObjID = model.NoticeID }, JsonRequestBehavior.AllowGet);
        }

        #region [Pay]

        [HttpPost]
        public void Pay()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = _commonRepository.GetServiceDetails("DOD Auction");

                    if (Globals.Util.isValidDataTable(dt, true))
                    {
                        AuctionVM data = (AuctionVM)Session["AuctionRequest"];
                        Models.CommanModels.PaymentViewModel payModel = new Models.CommanModels.PaymentViewModel();
                        payModel.emitraserviceid = Convert.ToString(dt.Rows[0]["EmitraServiceCode"]);
                        payModel.requestid = Convert.ToString(Session["NRequestID"]);
                        payModel.PayAmt = Convert.ToDecimal(Session["NBiddingAmt"]);
                        payModel.parentid = string.Join(",", data.DODProductList.Where(x => x.IsSelected == true).Select(x => x.AuctionWinnerID));
                        Session["ParentID"] = payModel.parentid;
                        payModel.ActionCode = "DOD";
                        payModel.officecode = data.DIV_CODE;
                        payModel.EmitraHeadCode = data.EmitraHeadCode;
                        var paymentResponse = Models.CommanModels.PaymentRepository.Pay(BookingType.OnlineCitizenBooking, payModel);
                        Response.Write(paymentResponse.OnlinePaymentResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
        }

        public ActionResult Payment()
        {
            AuctionRequest _objmodel = new AuctionRequest();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            EncryptDecrypt3DES objEncryptDecrypt = null;

            if (Session["NRequestID"] != null)
            {
                try
                {
                    string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";

                    if (Request.Form["MERCHANTCODE"] != null)
                        MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
                    if (Request.Form["PRN"] != null)
                        PRN = Request.Form["PRN"].ToString();
                    if (Request.Form["STATUS"] != null)
                        STATUS = Request.Form["STATUS"].ToString();
                    if (Request.Form["ENCDATA"] != null)
                        ENCDATA = Request.Form["ENCDATA"].ToString();

                    if (Globals.Util.GetAppSettings("WebsiteStatus") == "Dev")
                    {
                        objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016");//Staging
                    }
                    else
                    {
                        objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");//Production
                    }

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    //  DecryptedData = "{'MERCHANTCODE':'FOREST0117','REQTIMESTAMP':'20170810001655000','PRN':'636379209711021628','AMOUNT':'10044.00','PAIDAMOUNT':'10050.00','SERVICEID':'2239','TRANSACTIONID':'170055011924','RECEIPTNO':'17053223840','EMITRATIMESTAMP':'20170810001853257','STATUS':'SUCCESS','PAYMENTMODE':'Kotak Bank Payment Gateway(All Banks)','PAYMENTMODEBID':'CTX170809184817579982','PAYMENTMODETIMESTAMP':'20170810001715000','RESPONSECODE':'200','RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.','UDF1':'CKNDR HASHIM','UDF2':'udf2','CHECKSUM':'03263f854d49bcdbf966ce9ffe494d26'}";
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    BookOnTicket cs = new BookOnTicket();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    cs.UpdateEmitraResponse(Session["NRequestID"].ToString(), "DODAuction", DecryptedData);

                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("EMITRA AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion

                    string steps = string.Empty;
                    #region Response Status
                    if (ObjPGResponse.STATUS == "SUCCESS")
                    {
                        DataRow dtrow = dt.NewRow();
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        cs.RequestId = ObjPGResponse.PRN;
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;
                        //dtrow["TRANSACTION BANK BID"] = finalbankid;   

                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            #region Model data
                            _objmodel.Trn_Status_Code = 1;
                            _objmodel.NoticeId = Convert.ToInt64(Session["NRequestID"].ToString().Split('/')[1]);
                            _objmodel.RequestedId = Session["NRequestID"].ToString();
                            _objmodel.ParentID = Session["ParentID"].ToString();
                            _objmodel.EmitraTransactionID = ObjPGResponse.TRANSACTIONID;
                            _objmodel.Comments = ObjPGResponse.RESPONSEMESSAGE;
                            _objmodel.PayableAmount = Convert.ToDecimal(ObjPGResponse.AMOUNT);
                            _objmodel.EmitraAmount = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                            #endregion
                        }
                        dt.Rows.Add(dtrow);

                    }
                    else
                    {

                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = "0";
                        cs.RequestId = ObjPGResponse.PRN;

                        if (Session["KioskUserId"] == null || string.IsNullOrWhiteSpace(Convert.ToString(Session["KioskUserId"])))
                        {
                            cs.KioskUserId = "0";
                        }
                        else
                        {
                            cs.KioskUserId = Convert.ToString(Session["KioskUserId"]);
                        }

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = "0";
                        dtrow["EMITRA AMOUNT"] = "0";
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name


                        if (dtrow["TRANSACTION STATUS"].ToString() != "SUCCESS")
                        {
                            #region Model data
                            _objmodel.Trn_Status_Code = 0;
                            _objmodel.NoticeId = Convert.ToInt64(Session["NRequestID"].ToString().Split('/')[1]);
                            _objmodel.RequestedId = Session["NRequestID"].ToString();
                            _objmodel.EmitraTransactionID = "";
                            _objmodel.Comments = ObjPGResponse.RESPONSEMESSAGE;
                            _objmodel.PayableAmount = 0;
                            _objmodel.EmitraAmount = 0;
                            #endregion
                        }
                        dt.Rows.Add(dtrow);
                    }
                    #endregion

                    ViewData.Model = dt.AsEnumerable();




                    if (_objmodel.Trn_Status_Code == 1)
                    {
                        DataSet ds = new DataSet();

                        //ds = _objmodel1.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), _objmodel.Trn_Status_Code, "payUpdate", Session["NRequestID"].ToString(), Convert.ToInt64(Session["UserId"].ToString()), Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT));

                        ds = _repository.SaveAuctionDetails(_objmodel);

                        #region "User Send Email"
                        string UserMailBody = Common.GenerateBody(Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT), ds.Tables[0].Rows[0]["Name"].ToString(), ObjPGResponse.PRN, "DOD Auction");
                        string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), ObjPGResponse.PRN, "DOD Auction");
                        _objMailSMS.sendEMail("Payment Details for " + "DOD Auction", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

                        SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        #endregion
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            //#region "Is Reviwer Mail"
                            //string ReviwerMailBody = Common.GenerateReviwerBody(ds.Tables[1].Rows[0]["Name"].ToString(), _objmodel.TransactionId, ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission");
                            //_objMailSMS.sendEMail("Request for " + ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission", ReviwerMailBody, ds.Tables[1].Rows[0]["EmailId"].ToString(), string.Empty);
                            //#endregion
                        }
                        return RedirectToAction("dashboard", "dashboard", new { messagetype = "2" });
                        //return View("TransactionStatus");
                    }
                    else
                    {
                        //_objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), status1, "payUpdate", Session["NRequestID"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));
                        _repository.SaveAuctionDetails(_objmodel);
                        return View("NOCFIlmORGTransactionStatus");

                    }

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserId"]));
                }
                return View("TransactionStatus");
            }
            return View();
        }
        #endregion
        [HttpPost]
        public JsonResult GetCaptchaValue(string captchaPrefix)
        {
            return Json(new { CaptchaValue = Session["Captcha" + captchaPrefix] }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region [Admin Part]

        #region Manage AuctionRequests
        public ActionResult AppliedAuction()
        {
            if (TempData["ReturnMsg"] != null)
            {
                ViewBag.IsError = TempData["IsError"];
                ViewBag.ReturnMsg = TempData["ReturnMsg"];
                TempData["ReturnMsg"] = null; TempData["IsError"] = null;
            }
            var model = _repository.GetAuctionDetails();
            return View(model);

        }

        public ActionResult ViewDetailsCommon(string parentID, string actionType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dynamic obj;
            ViewBag.ActionType = actionType;
            ActionResult actionResult = null;
            try
            {
                if (!String.IsNullOrEmpty(parentID))
                {
                    switch (actionType)
                    {
                        case "NoticeDetails":
                            obj = _repository.GetNoticeDetails(Convert.ToInt64(parentID), "5");
                            actionResult = PartialView("_EditNotice", obj);
                            break;
                        case "WinnerDetails":
                            obj = _repository.GetNoticeDetails(Convert.ToInt64(parentID), "5");
                            actionResult = PartialView("_AuctionWinnerPayment", obj);
                            break;
                        case "Payment":
                            obj = _repository.GetPaymentDetails(Convert.ToInt64(parentID), "6");
                            actionResult = PartialView("_ViewPaymentDetails", obj);
                            break;
                        case "AuctionClearance":
                            SetDropdownData();
                            actionResult = PartialView("_AddAuctionClearance", new AuctionClearanceVM());
                            break;
                    }
                }
            }
            catch (Exception ex) { }
            return actionResult;

        }

        [HttpPost]
        public ActionResult UpdateAuctionWinner(AuctionVM model, string mode = "Citizen")
        {
            Entity.ResponseMsg msg = null;
            if (mode.Equals("Dept"))
            {
                msg = _repository.UpdateAuctionWinnerDept(model);
            }
            else
            {
                msg = _repository.UpdateAuctionWinner(model);
            }
            TempData["ReturnMsg"] = msg.ReturnMsg;
            TempData["IsError"] = msg.IsError;
            return Json(new { IsError = msg.IsError, ReturnMsg = msg.ReturnMsg, redirectURL = "AppliedAuction" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Manage Auction Clearance

        public ActionResult GetDODProductDetails(string parentID, string childID)
        {
            AuctionClearanceVM model = new AuctionClearanceVM();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {

                if (!string.IsNullOrEmpty(parentID))
                {
                    var dt = _repository.GetDetailsByInventory(parentID, childID);
                    model.DODProductList = Globals.Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(dt);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserId"]));
            }
            return PartialView("_ACProductDetails", model);
        }
        public ActionResult ManageAuctionClearance()
        {
            if (TempData["ReturnMsg"] != null)
            {
                ViewBag.IsError = TempData["IsError"];
                ViewBag.ReturnMsg = TempData["ReturnMsg"];
                TempData["ReturnMsg"] = null; TempData["IsError"] = null;
            }
            var model = _repository.GetAuctionDetailsForClearance();
            return View(model);

        }
        [HttpPost]
        public ActionResult SaveAuctionClearance(AuctionClearanceVM model)
        {
            var msg = _repository.SaveAuctionClearance(model);
            TempData["ReturnMsg"] = msg.ReturnMsg;
            TempData["IsError"] = msg.IsError;
            return PartialView("_AddAuctionClearance", model);
        }

        [HttpPost]
        public JsonResult BindAuctionNotice(string inventoryID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> itms = new List<SelectListItem>();
            try
            {
                itms = _repository.GetAuctionNoticeListForAuctionClearance(inventoryID).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = Convert.ToString(x.Field<long>("AuctionWinnerID")),
                    Text = x.Field<string>("Notice_Number")
                }).ToList();
            }
            catch (Exception ex) { }


            return Json(new { data = itms }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerateReport(string RequestID, string Command, string OTP, string TransationID)
        {
            try
            {
                var rootPath = Server.MapPath("~/");
                E_SignIntegration.clsVerifyOTP request = new E_SignIntegration.clsVerifyOTP();
                request.otp = OTP;
                request.transactionid = TransationID;

                var otpResponse = FMDSS.App_Start.cls_ESignIntegrationByFRA.VerifyOTPData(request, RequestID);
                var msg = _repository.GenerateReport(RequestID, Command, rootPath, otpResponse);

                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                return Json(new { IsError = msg.IsError, ReturnMsg = msg.ReturnMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { }
            return Json(new { IsError = true, ReturnMsg = "Something went wrong, please try again after some time." }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Private Methods
        private void SetDropdownData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var data = _repository.GetNoticeDataForAuctionClearance();
                ViewBag.LotList = GetDropdownData(1, data.Tables[0]);
                ViewBag.TransportMode = new FMDSS.Models.ForestDevelopment.TransitPermit().SetDropdownData(2, string.Empty);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
        }

        private EnumerableRowCollection<SelectListItem> GetDropdownData(int actionCode, DataTable dtDropdownData)
        {
            EnumerableRowCollection<SelectListItem> data = null;
            switch (actionCode)
            {
                case 1:
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("InventoryID")),
                        Text = x.Field<string>("DisplayLotNumber")
                    });
                    return data;
            }
            return null;
        }
        #endregion
    }
}
