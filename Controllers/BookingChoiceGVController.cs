using FMDSS.LIBS;
using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.GVChoice;
using FMDSS.Models.GVChoice.IGVChoice;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.BookingChoiceGV
{
    public class BookingChoiceGVController : BaseController
    {
        //
        // GET: /BookingChoiceGV/
        FMDSS.Models.DAL dl = new Models.DAL();
        private BookingType BookingType
        {
            get
            {
                if (Session["IsDepartmentalKioskUser"] != null && Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
                {
                    return BookingType.DepartmentBooking;
                }
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]))
                {
                    return BookingType.EmitraKioskBooking;
                }
                else
                {
                    return BookingType.OnlineCitizenBooking;
                }
            }
        }
        public ActionResult Index(string ticketId = "")
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (ticketId != "")
                ticketId = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(ticketId);

            int bookingTyp = (BookingType == BookingType.OnlineCitizenBooking ? 1 : (BookingType == BookingType.DepartmentBooking ? 2 : 3));

            INTF_GVChoice gvChoice = new GVChoiceService();
            GV_ChoiceView gvChoiceView = new GV_ChoiceView();
            gvChoiceView.gvChoiceList = new List<GVChoice>();
            gvChoiceView.gvChoiceList = gvChoice.GetGVChoiceTransactionList(UserID, ref ticketId, bookingTyp);           
            return View(gvChoiceView);          
        }
        #region Choice Guide/Vehicle 
        
        public ActionResult ChoiceGuideVehicle(string ticketId="")
        {
            if(ticketId!="")
                ticketId = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(ticketId);
            int bookingTyp = (BookingType == BookingType.OnlineCitizenBooking ? 1 : (BookingType == BookingType.DepartmentBooking ? 2 : 3));

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            INTF_GVChoice gvChoice = new GVChoiceService();
            GV_ChoiceView gvChoiceView = new GV_ChoiceView();
            gvChoiceView.gvChoiceList = new List<GVChoice>();
            gvChoiceView.gvChoiceList = gvChoice.GetGVChoiceTransactionList(UserID,ref ticketId, bookingTyp);
            ViewBag.RequestedId = ticketId;

            ViewBag.PayMsg = "";
            ViewBag.PayStatus = -1;
            if(TempData["PayStatus"]!=null)
                ViewBag.PayStatus = Convert.ToInt16( TempData["PayStatus"].ToString());

            if (TempData["PayMsg"] != null)
                ViewBag.PayMsg = TempData["PayMsg"].ToString(); 

            return View(gvChoiceView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChoiceGuideVehicle(GVChoice gvChoice)
        {
            DataSet ds = null;
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            Session["GVChoice"] = gvChoice;
            if (TempData["PayStatus"] != null)
                ViewBag.PayStatus =Convert.ToInt16(TempData["PayStatus"]);

            if (TempData["PayMsg"]!=null)
                ViewBag.PayStatus = TempData["PayMsg"].ToString();
            //GV_PayStatus GV_PayStatus = new GV_PayStatus();
            if (ModelState.IsValid == true && (gvChoice.ChoiceType == 1 && gvChoice.GuideId > 0) || (gvChoice.ChoiceType == 2 && gvChoice.VehicleOrBoatId > 0) || (gvChoice.ChoiceType == 3 && gvChoice.GuideId > 0 && gvChoice.VehicleOrBoatId > 0))
            {
                if (Session["gvChoice"] == null)
                {
                    Session["gvChoice"] = gvChoice;
                }
                
                if (!string.IsNullOrEmpty(gvChoice.RequestId))
                {
                    if (BookingType == BookingType.OnlineCitizenBooking)
                    {
                        RedirectPGEmitra(gvChoice);

                        return new EmptyResult();
                    }
                    if (BookingType == BookingType.DepartmentBooking)
                    {

                        INTF_GVChoice gv_Choice = new GVChoiceService();
                        ds = gv_Choice.UpdateChoiceDetails(gvChoice, "", "", 1, true, 2, liveUat);
                        TempData["PayStatus"] = ds.Tables[0].Rows[0]["MsgStatus"].ToString() ;
                        TempData["PayMsg"] = ds.Tables[0].Rows[0]["Msg"].ToString();
                        return RedirectToAction("ChoiceGuideVehicle", "BookingChoiceGV");
                    }
                }
                return new EmptyResult();
            }
            else
            {
                return View(gvChoice);
            }
        }

        [HttpGet]
        public FileResult GetChoiceReceipt(string strid)
        {
            int bookingTyp = (BookingType == BookingType.OnlineCitizenBooking ? 1 : (BookingType == BookingType.DepartmentBooking ? 2 : 3));
            string strId = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(strid);
            string[] spl = null;
            spl = strId.Split('|');
            string requestId = spl[0];
            string choiceRequestId = spl[1];

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                INTF_GVChoice gvChoice = new GVChoiceService();
                GV_ChoiceView gvChoiceView = new GV_ChoiceView();
                gvChoiceView.gvChoiceList = new List<GVChoice>();
                gvChoiceView.gvChoiceList = gvChoice.GetGVChoiceForReceiptList(UserID, bookingTyp, requestId, choiceRequestId);

                string filepath = TicketPDFGenerate.GV_GenerateGuideOrVehicleReceipt(gvChoiceView.gvChoiceList);
                if (System.IO.File.Exists(filepath))
                {
                    // string FilePath = Server.MapPath(filepath);
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(filepath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetRequestDetails(string RequestId, int ChoiceType)
        {
            GV_ChoiceView gV = new GV_ChoiceView();
            INTF_GVChoice gvChoice = new GVChoiceService();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int bookingType = (BookingType == BookingType.OnlineCitizenBooking ? 1 : (BookingType == BookingType.DepartmentBooking ? 2 : 3)); 
           
            DataSet ds = gvChoice.GetChoiceDetails(RequestId, UserID, bookingType);
            string vehicleType = "";
            if (ds.Tables.Count > 0)
            {
                DataColumnCollection columns = ds.Tables[0].Columns;
                if (columns.Contains("validStatus"))
                {
                    var res1 = new { status = Convert.ToInt16(ds.Tables[0].Rows[0]["validStatus"]), respone = ds.Tables[0].Rows[0]["validMsg"].ToString() };
                    return Json(res1, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    gV.gvChoiceList = new List<GVChoice>();
                    GVChoice gvObj ;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        gvObj = new GVChoice();
                        gvObj.TicketId =Convert.ToInt32(dr["TicketId"]);
                        gvObj.RequestId = dr["RequestId"].ToString();
                        gvObj.VisitDate = Convert.ToDateTime(dr["VisitingDate"]).ToShortDateString();
                        gvObj.BookingDate = Convert.ToDateTime(dr["BookingDateTime"]).ToShortDateString();
                        gvObj.TotalMember = Convert.ToInt32(dr["TotalMember"]);
                        gvObj.PlaceId = Convert.ToInt32(dr["PlaceId"]);
                        gvObj.PlaceName = dr["PlaceName"].ToString();
                        gvObj.ZoneId = Convert.ToInt32(dr["ZoneId"]);
                        gvObj.ZoneName = dr["ZoneName"].ToString();
                        gvObj.ShiftId = Convert.ToInt32(dr["ShiftId"]);
                        gvObj.ShiftName = dr["ShiftName"].ToString();
                        gvObj.VehicleId = Convert.ToInt32(dr["VehicleId"]);
                        gvObj.VehicleName = dr["VehicleName"].ToString(); 
                        gvObj.GuideName = (String.IsNullOrEmpty(dr["GuideName"].ToString()) ? "" : dr["GuideName"].ToString()) ;
                        gvObj.GuideChoiceAmt = Convert.ToDecimal(dr["GuideChoiceAmt"]);
                        gvObj.VehicleNumber= (String.IsNullOrEmpty(dr["VehicleNumber"].ToString()) ? "" : dr["VehicleNumber"].ToString());
                        gvObj.VehileChoiceAmt=Convert.ToDecimal(dr["VehileChoiceAmt"]);
                        gvObj.IsVehicleChoice = (ChoiceType==3 || ChoiceType == 2?true:false);
                        gvObj.IsGuideChoice = (ChoiceType == 3 || ChoiceType == 1 ?true :false);
                        gvObj.GuideChoiceGSTAmt= Convert.ToDecimal(dr["GuideChoiceGSTAmt"]);
                        gvObj.VehileChoiceGSTAmt= Convert.ToDecimal(dr["VehileChoiceGSTAmt"]);
                        gvObj.status = 1;
                        gvObj.respone = "Valid";
                        gvObj.ChoiceType = ChoiceType;

                        vehicleType= dr["VehicleName"].ToString();

                        gV.gvChoiceList.Add(gvObj);
                    }
                    

                    gV.GuideSelectList = new List<SelectListItem>();
                    gV.VehicleSelectList = new List<VehicleProp>();

                    var obChoice = gV.gvChoiceList.ToList().FirstOrDefault();
                    gV.GuideSelectList = gvChoice.GetGvGuideList(obChoice.PlaceId);
                    gV.VehicleSelectList = gvChoice.GetVehicleNumberList(obChoice.PlaceId, vehicleType);
                    return PartialView("_GVChoice", gV);
                }


            }
            else
            {
                var res1 = new { status = 0, respone = "No Data Found" };
                return Json(res1, JsonRequestBehavior.AllowGet);
            }

            // return null;
        }
        
        
        public JsonResult CheckGuideBookedStatus(int GuideId, int ShiftId, string VisitDate)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);         
            try
            {
                INTF_GVChoice gvChoice = new GVChoiceService();                              
                DataTable dt = new DataTable();
                dt = gvChoice.CheckGuideBookedStatus(GuideId, ShiftId, VisitDate);
                var responseStatus = new { MsgStatus = Convert.ToInt16(dt.Rows[0]["MsgStatus"].ToString()), Msg = dt.Rows[0]["Msg"].ToString() };
                return Json(responseStatus, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                var responseStatus = new { MsgStatus = 0, Msg = ex.Message.ToString() };
                return Json(responseStatus, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult CheckVehicleBookedStatus(int VehicleId, int ShiftId, string VisitDate)
        {
           
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            
            try
            {
                INTF_GVChoice gvChoice = new GVChoiceService();
                DataTable dt = new DataTable();
                dt = gvChoice.CheckVehicleBookedStatus(VehicleId, ShiftId, VisitDate);
                var responseStatus = new { MsgStatus = Convert.ToInt16(dt.Rows[0]["MsgStatus"].ToString()), Msg = dt.Rows[0]["Msg"].ToString() };
                return Json(responseStatus, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                var responseStatus = new { MsgStatus = 0, Msg = ex.Message.ToString() };
                return Json(responseStatus, JsonRequestBehavior.AllowGet);
            }

        }
        /////Emitra Payment Section Start
        private void RedirectPGEmitra(GVChoice model)
        {
            string forms = "";
            decimal AmountToBePay = 0;
            string ChoiceRequestId = string.Empty;
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            string REVENUEHEAD = string.Empty;
            string CommonNames =(model.VehicleNumber==null?"":(model.VehicleNumber.Length > 0 ? model.VehicleName : ""));
            CommonNames += (CommonNames.Length > 0 ? "," + (model.GuideId > 0 ? "GUIDE" : "") : (model.GuideId > 0 ? "GUIDE" : ""));
            DataSet DS = GetEmitraHeadForOnLineChoiceGV(model.PlaceId, liveUat, CommonNames, out REVENUEHEAD); //Service ID to be decided in stored procedure
            AmountToBePay = model.GuideChoiceAmt + model.GuideChoiceGSTAmt + model.VehileChoiceAmt + model.VehileChoiceGSTAmt;
            if (DS != null && DS.Tables.Count > 0)
            {
                if (DS.Tables[0].Rows.Count > 0 )
                {               
                    string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                    EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();
                    INTF_GVChoice gvChoice = new GVChoiceService();
                    DataTable DT = gvChoice.SaveAndGetPGChoiceRequestId(model.RequestId);
                    ChoiceRequestId = DT.Rows[0]["ChoiceRequestId"].ToString();
                    Session["ChoiceRequestId"] = ChoiceRequestId;                 
                    if (liveUat == 1)
                    {
                        //model.RequestId
                       forms = ObjEmitraPayRequest.PayRequestLive(false, ChoiceRequestId,
                       Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                       Convert.ToString(DS.Tables[0].Rows[0]["ChecksumKey"]),
                       Convert.ToString(DS.Tables[0].Rows[0]["EncryptionKey"]),
                       ReturnUrl + "BookingChoiceGV/GVBPayment", ReturnUrl + "BookingChoiceGV/GVBPayment",
                       Convert.ToString(DS.Tables[0].Rows[0]["CDR_CODE"]), Convert.ToString(DS.Tables[0].Rows[0]["EmitraServiceId"]),
                       Convert.ToString(AmountToBePay), REVENUEHEAD, Session["User"].ToString(), "", Convert.ToString(DS.Tables[0].Rows[0]["ComType"]));
                    }
                    else
                    { // UAT
                        forms = ObjEmitraPayRequest.PayRequest(false, ChoiceRequestId,
                        Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["ChecksumKey"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["EncryptionKey"]),
                        ReturnUrl + "BookingChoiceGV/GVBPayment", ReturnUrl + "BookingChoiceGV/GVBPayment",
                        Convert.ToString(DS.Tables[0].Rows[0]["CDR_CODE"]), Convert.ToString(DS.Tables[0].Rows[0]["EmitraServiceId"]),
                        Convert.ToString(AmountToBePay), REVENUEHEAD, Session["User"].ToString(), "", Convert.ToString(DS.Tables[0].Rows[0]["ComType"]));
                    }
                    Response.Write(forms);
                }
            }
        }
        public ActionResult GVBPayment()
        {
            int IsLiveOrUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
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


                string ResponseStr = MERCHANTCODE + "|" + PRN + "|" + STATUS + "|" + ENCDATA;

                
                EncryptDecrypt3DES objEncryptDecrypt = (IsLiveOrUat == 1 ? new EncryptDecrypt3DES("N@FOREST#4*23") : new EncryptDecrypt3DES("EmitraNew@2016"));

                string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                GV_PayStatus updateBooking = new GV_PayStatus
                {
                    UserId = UserID,
                    EmitraResponse = DecryptedData,
                    TransactionId = ObjPGResponse.TRANSACTIONID,
                    PaymentMode = (int)PaymentMode.Online,
                    RequestId = PRN,
                    EmitraAmount = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) > 0 ? (Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT)) : 0
                };

                //****************************** for test only

                //ObjPGResponse.STATUS = "SUCCESS";
                //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                //ObjPGResponse.AMOUNT = Convert.ToString(Session["totalprice"]);
                //ObjPGResponse.PAIDAMOUNT = Convert.ToString(Convert.ToDecimal(Session["totalprice"]) + Convert.ToDecimal(5));
                //ObjPGResponse.TRANSACTIONID = DateTime.Now.Ticks.ToString();

                //****************************** for test only;

                if (ObjPGResponse.STATUS == "SUCCESS") //PAYMENT SUCCESSFUL
                {
                    updateBooking.ResponseStaus = (int)TransactionStatus.Paid;
                    updateBooking.isValidResponse = true;
                }
                else //PAYMENT FAILED
                {
                    updateBooking.isValidResponse = false;
                    updateBooking.ResponseStaus = (int)TransactionStatus.Failed;
                }


                Session["eMitraPGResponse"] = updateBooking;


                //Something went wrong.
                return RedirectToAction("UpdatePGResponse", "BookingChoiceGV");
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View();
        }
        [HttpGet]
        public ActionResult UpdatePGResponse()
        {
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            DataSet ds=null;
            GV_PayStatus eMitraPGResponse = Session["eMitraPGResponse"] as GV_PayStatus;
            GVChoice gvChoice = Session["GVChoice"] as GVChoice;
            eMitraPGResponse.ChoiceRequestId = Session["ChoiceRequestId"].ToString();
            if (eMitraPGResponse.isValidResponse == true)
            {
                INTF_GVChoice gv_Choice = new GVChoiceService();
                ds = gv_Choice.UpdateChoiceDetails(gvChoice, eMitraPGResponse.ChoiceRequestId, eMitraPGResponse.EmitraResponse, eMitraPGResponse.ResponseStaus, eMitraPGResponse.isValidResponse,1, liveUat);
            }

            if (eMitraPGResponse != null)
            {
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {                        
                        //TempData["PayStatus"] = Convert.ToInt16(ds.Tables[0].Rows[0]["MsgStatus"]); ;
                        //TempData["PayMsg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                        TempData["PayStatus"] = Convert.ToInt16(ds.Tables[0].Rows[0]["MsgStatus"]); 
                        TempData["PayMsg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    }
                }
                else
                {
                    TempData["PayStatus"] = eMitraPGResponse.ResponseStaus;
                    TempData["PayMsg"] = eMitraPGResponse.ResponseMsg;
                    //TempData["PayStatus"] = 0;
                    //TempData["PayMsg"] = eMitraKioskResponse;
                }
            }
           
            return RedirectToAction("ChoiceGuideVehicle", "BookingChoiceGV");
        }
        private GV_PayStatus EmitraKioskPayment(GVChoice model)
        {
            decimal AmountToBePay = 0;
            string ChoiceRequestId = string.Empty;
            GV_PayStatus gv_PayStatus = new GV_PayStatus();
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            if (Session["EmitrServiceId"] != null)
            {
                DataSet DS = GetEmitraHeadForChoiceVehicleOrBoat(model.RequestId, Convert.ToInt32(Session["EmitrServiceId"]), model.ChoiceType, liveUat);
                if (DS != null && DS.Tables.Count > 0)
                {
                    if (DS.Tables[0].Rows.Count > 0 && DS.Tables[0].Rows[0]["TicketId"].ToString().Length > 0 && DS.Tables[2].Rows.Count > 0)
                    {
                        //decimal TotalAmount = Convert.ToDecimal(DS.Tables[0].Rows[0]["TotalAmountBePay"]);
                        EmitraKisokPayment emitraKisokPayment = new EmitraKisokPayment();
                        ChoiceRequestId = DS.Tables[0].Rows[0]["TicketId"].ToString();

                        string REVENUEHEAD = string.Empty;
                        foreach (DataRow dr in DS.Tables[1].Rows)
                        {
                            REVENUEHEAD = REVENUEHEAD + dr["EmitraHeadCode"] + "-" + dr["HeadAmount"] + "|";
                            AmountToBePay = AmountToBePay + Convert.ToDecimal(dr["HeadAmount"]);
                        }
                        REVENUEHEAD = REVENUEHEAD + Convert.ToString(DS.Tables[0].Rows[0]["ZeroAmtHead"]) + "|";
                        REVENUEHEAD = REVENUEHEAD.Trim('|');
                        decimal TotalAmount = AmountToBePay;

                        EmitraKioskRequest request = new EmitraKioskRequest
                        {
                            BASEURL = Convert.ToString(DS.Tables[2].Rows[0]["BaseUrl"]),
                            VERIFICAION_URL = Convert.ToString(DS.Tables[2].Rows[0]["VerificationUrl"]),
                            SERVICERESPONSETIME = Convert.ToInt16(DS.Tables[2].Rows[0]["MAX_RESPONSE_TIME_SEC"]),
                            MERCHANTCODE = Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                            REQUESTID = DS.Tables[0].Rows[0]["TicketId"].ToString(),//model.RequestId,
                            REQTIMESTAMP = DateTime.Now.Ticks.ToString(),
                            SERVICEID = Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                            SUBSERVICEID = "",
                            REVENUEHEAD = REVENUEHEAD,
                            CONSUMERKEY = DS.Tables[0].Rows[0]["TicketId"].ToString(),//,model.RequestId,
                            CONSUMERNAME = Convert.ToString(Session["KioskCtznName"]).ToUpper(),
                            SSOID = Convert.ToString(Session["KioskSSOId"]).ToUpper(),
                            OFFICECODE = Convert.ToString(DS.Tables[0].Rows[0]["DivCode"]),
                            //COMMTYPE = "2",
                            COMMTYPE = "3", ////Change by Amit on 02/09/2020 for Ematra changes 
                            SSOTOKEN = Convert.ToString(Session["SSOTOKEN"])
                        };
                        if (liveUat == 0)
                        {

                            request.BASEURL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA";
                            request.VERIFICAION_URL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/getTokenVerifyNewProcessByRequestIdWithEncryption";
                            request.OFFICECODE = "FORESTHQ";
                        }
                        if (!string.IsNullOrEmpty(request.OFFICECODE))
                        {
                            EmitraKioskResponse emitraKisokResponse = emitraKisokPayment.ProcessPayment(request, 1, "Choice Boat Service Call");

                            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

                            gv_PayStatus.UserId = UserID;
                            gv_PayStatus.EmitraResponse = emitraKisokResponse.RESPONSE;
                            gv_PayStatus.TransactionId = emitraKisokResponse.TRANSACTIONID;
                            gv_PayStatus.PaymentMode = (int)PaymentMode.Online;
                            gv_PayStatus.RequestId = emitraKisokResponse.REQUESTID;
                            gv_PayStatus.ChoiceRequestId = ChoiceRequestId;
                            gv_PayStatus.EmitraAmount = Convert.ToDecimal(emitraKisokResponse.TRANSAMT) > 0 ? (Convert.ToDecimal(emitraKisokResponse.TRANSAMT) - Convert.ToDecimal(TotalAmount)) : 0;

                            if (emitraKisokResponse.TRANSACTIONSTATUS == "SUCCESS") //PAYMENT SUCCESSFUL
                            {
                                gv_PayStatus.ResponseStaus = (int)TransactionStatus.Paid;
                                gv_PayStatus.isValidResponse = true;
                            }
                            else //PAYMENT FAILED
                            {
                                gv_PayStatus.ResponseStaus = (int)TransactionStatus.Failed;
                                gv_PayStatus.isValidResponse = false;
                            }

                        }
                    }
                }
            }
            return gv_PayStatus;
        }
        private DataSet GetEmitraHeadForChoiceVehicleOrBoat(string RequestId, int serviceId, int ChoiceGV, int liveUat)
        {
            SqlParameter[] param = {new SqlParameter("@RequestId",RequestId),
                                            new SqlParameter("@UserID",Convert.ToInt32(Convert.ToString(Session["UserId"]))),
                                            new SqlParameter("@ServiceId",serviceId),
                                            new SqlParameter("@ChoiceGV",ChoiceGV),
                                            new SqlParameter("@IsLive ",liveUat)
                                            };
            DataSet DS = new DataSet();
            dl.Fill(DS, "spGV_GetEmitraHeadForChoiceVehicle", param);
            return DS;
        }
        private DataSet GetEmitraHeadForOnLineChoiceGV(long PlaceId,int IsLiveOrUAT,string CommonNameStr, out string REVENUEHEAD)
        {
            REVENUEHEAD = "";
            SqlParameter[] param = {new SqlParameter("@ActionName","GetChoiceGV_EmitraHeads"),
                                            new SqlParameter("@PlaceId",PlaceId),                                           
                                            new SqlParameter("@IsLiveOrUAT",(IsLiveOrUAT==0?0:1)),
                                            new SqlParameter("@CommonNameStr",CommonNameStr)
                                            };
            DataSet DS = new DataSet();
            dl.Fill(DS, "sp_EmitraFunctions", param);

            foreach(DataRow dr in DS.Tables[0].Rows)
            {
                REVENUEHEAD += (REVENUEHEAD.Length == 0 ? dr["RevenueHeadCode"].ToString() + "-" + dr["FeeAmount"].ToString() : "|" + dr["RevenueHeadCode"].ToString() + "-" + dr["FeeAmount"].ToString());
            }

           
            return DS;
        }
        /// Emitra Payment Section End
        #endregion
       
    }
}
