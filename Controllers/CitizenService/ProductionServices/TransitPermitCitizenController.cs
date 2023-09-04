using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.CitizenService.ProductionServices;
using FMDSS.Models.CitizenService.PermissionService;
using System.Configuration;
using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using Newtonsoft.Json;
using System.IO;
using FMDSS.Models.ForesterAction;
using FMDSS.App_Start;
using System.util;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using FMDSS.E_SignIntegration;

namespace FMDSS.Controllers.CitizenService.ProductionServices
{
    public class TransitPermitCitizenController : Controller
    {
        TransitPermit objtp = new TransitPermit();
        #region Survey Reports for TP by Rajveer
        public ActionResult SurveyReportForTP(string RequestID)
        {
            ActionRequest ar = new ActionRequest();
            SurveyReportTP model = new SurveyReportTP();
            model.PermitID = Encryption.decrypt(RequestID);
            //ViewBag.DIVISION_CODE = getDropdown("13");
            //ViewBag.STATE_CODE = getDropdown("3");
            //ViewBag.DISTRICT_CODE = getDropdown("4");
            ////ViewBag.DISTRICT_CODE2 = getDropdown("44");
            //ViewBag.APPLICANT_TEHSIL = getDropdown("0");
            //ViewBag.APPLICANT_VILLAGE = getDropdown("0");
            ViewBag.PRODUCE_NAME = getDropdown("10");

            objtp.Option = "13";//Get All DIvision, Distict,Tehasil,Village
            DataSet dtp = objtp.Fill_DropdownRangeAndDist();
            List<SelectListItem> lstDivision = new List<SelectListItem>();
            List<SelectListItem> lstDist = new List<SelectListItem>();
            List<SelectListItem> lstTahasil = new List<SelectListItem>();
            List<SelectListItem> lstVillage = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in dtp.Tables[0].Rows)
            {
                lstDivision.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
            }
            ViewBag.DIVISION_CODE = lstDivision;
            foreach (System.Data.DataRow dr in dtp.Tables[1].Rows)
            {
                lstDist.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
            }
            ViewBag.DISTRICT_CODE = lstDist;
            foreach (System.Data.DataRow dr in dtp.Tables[2].Rows)
            {
                lstTahasil.Add(new SelectListItem { Text = @dr["TEHSIL_Name"].ToString(), Value = @dr["TEHSIL_CODE"].ToString() });
            }
            ViewBag.APPLICANT_TEHSIL = lstTahasil;
            foreach (System.Data.DataRow dr in dtp.Tables[3].Rows)
            {
                lstVillage.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
            }
            ViewBag.APPLICANT_VILLAGE = lstVillage;
            DataSet dtdata = new DataSet();
            dtdata = ar.BindActionListWithDataset(model.PermitID, "Tbl_Citizen_TransitPermit");
            if (dtdata != null && dtdata.Tables[0] != null && dtdata.Tables[0].Rows.Count > 0)
            {
                model.AreaInKm = Convert.ToString(dtdata.Tables[0].Rows[0]["Land Holding Area"]);
                model.AreaName = Convert.ToString(dtdata.Tables[0].Rows[0]["Place Name where Produce Generate"]);
                model.SurveyDate = Convert.ToString(dtdata.Tables[0].Rows[0]["InspectionDate"]);

            }

            if (dtdata != null && dtdata.Tables[1] != null && dtdata.Tables[1].Rows.Count > 0)
            {
                model.ProduceListInString = Newtonsoft.Json.JsonConvert.SerializeObject(dtdata.Tables[1]);

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult SurveyReportForTP(SurveyReportTP model, string ProductDetails)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {


                #region Modify Product List
                if (!string.IsNullOrEmpty(ProductDetails))
                {
                    model.ProduceList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductDetails>>(ProductDetails);

                }
                #endregion

                string FileFullName = string.Empty;
                string path = string.Empty;
                string FilePath = "~/PDFFolder/CitizenTP/SurveyReport";
                if (model.fileUpload != null)
                {
                    FileFullName = DateTime.Now.Ticks + "_" + model.fileUpload.FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    model.fileUpload.SaveAs(Server.MapPath(FilePath + FileFullName));
                    model.UploadFile = FilePath + FileFullName;
                }

                if (model.fileShapeFile != null)
                {
                    FileFullName = DateTime.Now.Ticks + "_" + model.fileShapeFile.FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    model.fileShapeFile.SaveAs(Server.MapPath(FilePath + FileFullName));
                    model.UploadShapeFile = FilePath + FileFullName;
                }

                SurveyReportRepository repo = new SurveyReportRepository();
                string Message = repo.SaveServeyReportTP(model);
                TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>" + Message + "</div>";
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return RedirectToAction("ForesterAction", "ForesterAction", new { @ServiceType = "Q2l0aXplbg==", Tab = "VGFiMw==" });
        }
        #endregion

        #region Get Bhamasha Data from ID by Rajveer

        [HttpGet]

        public ActionResult GetBhamashaData(string BhamashaId)
        {
            // Encryption.encrypt()
            string sBhamashaId = AESEncrytDecry.DecryptStringAES(BhamashaId);
            BhamashaError BhamashaError = new BhamashaError();
            try
            {
                BhamashaRoot BhamashaData = cls_Bhamasha.GetBhamashaInfo(sBhamashaId.Trim().ToUpper());

                if (BhamashaData.Cmsg == "")
                {
                    return PartialView("_MemberDetails", BhamashaData);
                }
                else
                {
                    if (BhamashaData.Cmsg == "101")
                    {
                        BhamashaError.errorcode = "101";
                        BhamashaError.errorDescription = "Scheme name is null or not valid";
                    }
                    else if (BhamashaData.Cmsg == "110")
                    {
                        BhamashaError.errorcode = "110";
                        BhamashaError.errorDescription = "Family Id is not valid. It should have 7 characters";
                    }
                    else if (BhamashaData.Cmsg == "116")
                    {
                        BhamashaError.errorcode = "116";
                        BhamashaError.errorDescription = "Family Id is not valid.";
                    }
                    else if (BhamashaData.Cmsg == "107")
                    {
                        BhamashaError.errorcode = "107";
                        BhamashaError.errorDescription = "Aadhar number should be 12 digit number.";
                    }
                    else if (BhamashaData.Cmsg == "112")
                    {
                        BhamashaError.errorcode = "112";
                        BhamashaError.errorDescription = "fatal error occurs.";
                    }
                    return Json(BhamashaError, JsonRequestBehavior.AllowGet); //PartialView(BhamashaError);
                }

            }
            catch (Exception ex)
            {
                BhamashaError.errorcode = ex.Message;
                BhamashaError.errorDescription = ex.Message;
                return Json(BhamashaError, JsonRequestBehavior.AllowGet); //PartialView(BhamashaError);

            }


        }
        #endregion
        public ActionResult TransitPermitCitizen()
        {
            try
            {
                ViewBag.DIVISION_CODE = getDropdown("1");
                ViewBag.STATE_CODE = getDropdown("3");
                ViewBag.DISTRICT_CODE = getDropdown("4");
                ViewBag.DISTRICT_CODE2 = getDropdown("44");
                ViewBag.VechileType = getDropdown("5");
                ViewBag.PRODUCE_NAME = getDropdown("10");
                ViewBag.APPLICANT_TEHSIL = getDropdown("0");
                ViewBag.APPLICANT_VILLAGE = getDropdown("0");

                getUserDetails("11");
                objtp.TP_VALIDITY_DATE = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception)
            {
                throw;
            }
            return View(objtp);
        }
        public ActionResult getRange(string DIV_CODE)
        {
            try
            {
                List<List<SelectListItem>> obj = new List<List<SelectListItem>>();
                List<SelectListItem> lstRange = new List<SelectListItem>();
                List<SelectListItem> lstDist = new List<SelectListItem>();
                List<SelectListItem> lstState = new List<SelectListItem>();
                List<SelectListItem> lstTahasil = new List<SelectListItem>();
                List<SelectListItem> lstVillage = new List<SelectListItem>();
                objtp.UserId = Convert.ToInt64(Session["UserId"]);
                objtp.DIVISION_OFFICE = DIV_CODE;
                objtp.Option = "12";//Get both Range and Distict 
                DataSet dtp = objtp.Fill_DropdownRangeAndDist();
                foreach (System.Data.DataRow dr in dtp.Tables[0].Rows)
                {
                    lstRange.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                obj.Add(lstRange);
                foreach (System.Data.DataRow dr in dtp.Tables[1].Rows)
                {
                    lstDist.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                obj.Add(lstDist);
                foreach (System.Data.DataRow dr in dtp.Tables[2].Rows)
                {
                    lstState.Add(new SelectListItem { Text = @dr["StateName"].ToString(), Value = @dr["StateID"].ToString() });
                }
                obj.Add(lstState);

                foreach (System.Data.DataRow dr in dtp.Tables[3].Rows)
                {
                    lstTahasil.Add(new SelectListItem { Text = @dr["TEHSIL_Name"].ToString(), Value = @dr["TEHSIL_CODE"].ToString() });
                }
                obj.Add(lstTahasil);

                foreach (System.Data.DataRow dr in dtp.Tables[4].Rows)
                {
                    lstVillage.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                }
                obj.Add(lstVillage);

                return Json(Newtonsoft.Json.JsonConvert.SerializeObject(obj), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult CreatePermit(TransitPermit objTP, string ProductDetails)
        {

            try
            {
                ModelState.Remove("TP_VALIDITY_DATE");
                if (ModelState.IsValid)
                {
                    #region Modify Product List
                    if (!string.IsNullOrEmpty(ProductDetails))
                    {
                        //string[] str = ProductDetails.Split('|');
                        //for (int i = 0; i < str.Length - 1; i++)
                        //{
                        //    string[] val = str[i].Split(',');
                        //    ProductDetails itm = new Models.CitizenService.ProductionServices.ProductDetails();
                        //    itm.PRODUCE_VALUE =Convert.ToInt32(val[0]);
                        //    itm.PRODUCE_QUANTITY = val[1];
                        //    itm.PRODUCE_DESCRIPTION = val[2];
                        //    objTP.ProductList.Add(itm);
                        //}

                        objTP.ProductList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductDetails>>(ProductDetails);

                    }
                    #endregion


                    System.Text.StringBuilder sbProduceList = new System.Text.StringBuilder();
                    List<TransitPermit> lstSearch = new List<TransitPermit>();
                    //if (Request.Form["TP_VALIDITY_DATE"] != null)
                    //{
                    //    objTP.TP_VALIDITY_DATE = DateTime.ParseExact(Convert.ToString(Request.Form["TP_VALIDITY_DATE"]), "dd/MM/yyyy", null);
                    //}
                    //else
                    //{
                    //    objTP.TP_VALIDITY_DATE = null;
                    //}
                    string PermitNo = objTP.CREATE_TRANSITPERMIT();
                    if (Convert.ToString(PermitNo) != "")
                    {
                        objTP.PERMIT_NO = PermitNo;

                        Session["permitId"] = PermitNo;

                        #region payment
                        DataTable dtColmn = new DataTable();
                        if (dtColmn.Rows.Count == 0)
                        {
                            dtColmn.Columns.Add("PermitId");
                            dtColmn.Columns.Add("Name");
                            dtColmn.Columns.Add("PaidAmt");
                            dtColmn.Columns.Add("Status");
                        }
                        DataRow dtrow = dtColmn.NewRow();
                        dtrow["PermitId"] = objTP.PERMIT_NO;
                        Session["permitId"] = objTP.PERMIT_NO;
                        dtrow["Name"] = Session["User"].ToString(); ;
                        dtrow["PaidAmt"] = objTP.TP_FEES;
                        Session["totalprice"] = objTP.TP_FEES;
                        dtrow["Status"] = "Pending";
                        dtColmn.Rows.Add(dtrow);
                        ViewData.Model = dtColmn.AsEnumerable();


                        TempData["Message"] = TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>" +
                            " Applicant form submited successfully !</div>";
                        #endregion

                        return View("TransitPayment");
                    }
                }
                else
                {
                    TransitPermitCitizen();
                    return this.View("TransitPermitCitizen", objTP);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        [HttpPost]

        public void Pay()
        {


            ////EM33172142@5488
            //Payment pay = new Payment();
            //string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
            //string encrypt = pay.RequestString("EM33172142@5488", Session["permitId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "Auction/Payment", Session["User"].ToString(), "", "");
            //Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                // get different heads amount from DB
                BookOnTicket OBJ = new BookOnTicket();
                DataSet DS = new DataSet();
                DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("CitizenTransitPermit", Convert.ToString(Session["permitId"]));

                string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();

                string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["permitId"]),
                    Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                    Convert.ToString(DS.Tables[0].Rows[0]["ChecksumKey"]),
                    Convert.ToString(DS.Tables[0].Rows[0]["EncryptionKey"]),
                    ReturnUrl + "TransitPermitCitizen/Payment", ReturnUrl + "TransitPermitCitizen/Payment",
                    Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]), Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                    Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]), Convert.ToString(DS.Tables[0].Rows[0]["REVENUEHEAD"]), Session["User"].ToString());

                Response.Write(forms);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }



        }

        /// <summary>
        /// check payment status
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult Payment()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //Int64 UserID =   Convert.ToInt64(Session["UserID"].ToString());    // Comment for uat not working 
            Int64 UserID = 137667;// Convert.ToInt64(Session["UserID"].ToString());     // testing
            int fmdssStatus = 0;
            Session["permitId"] = Request.Form["PRN"].ToString();
            if (Session["permitId"] != null)
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


                    //EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");
                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016");


                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);


                    int status1 = 0;
                    TransitPermission tp = new TransitPermission();
                    BookOnTicket cs = new BookOnTicket();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    cs.UpdateEmitraResponse(Session["permitId"].ToString(), "CitizenTransitPermit", DecryptedData);
                   // cs.UpdateEmitraResponse("TPC001", "CitizenTransitPermit", DecryptedData);
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
                    DataRow dtrow = dt.NewRow();
                    #region Response Status
                    if (ObjPGResponse.STATUS == "FAILED")
                    {



                        tp.TransactionID = "0";
                        tp.RequestId = ObjPGResponse.PRN;
                        if (Session["KioskUserId"] == "" || Session["KioskUserId"] == null)
                        {
                            tp.kioskId = "0";
                        }
                        else
                        {
                            tp.kioskId = Session["KioskUserId"].ToString();

                        }

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = "0";
                        dtrow["EMITRA AMOUNT"] = "0";
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        tp.Trn_Status_Code = 0;
                        dt.Rows.Add(dtrow);
                        ViewData.Model = dt.AsEnumerable();
                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
                    {


                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        tp.RequestId = ObjPGResponse.PRN;
                        tp.TransactionID = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;
                       
                       
                        //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {

                            try
                            {
                                //if (Convert.ToString(Session["permitId"]).Equals(ObjPGResponse.PRN) && (Session["totalprice"] != null && Convert.ToDecimal(Session["totalprice"]) == Convert.ToDecimal(ObjPGResponse.AMOUNT)))
                                if (Convert.ToString(Session["permitId"]).Equals(ObjPGResponse.PRN))
                                {
                                    cs.Trn_Status_Code = 1;
                                    status1 = 1;
                                    fmdssStatus = tp.UpdateTransactionStatus(Convert.ToDecimal(ObjPGResponse.AMOUNT), Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT));
                                    DataTable Permitdetails = tp.GetTransactionReq_ByPermitNo(ObjPGResponse.PRN);

                                    TransitPermit objTP = new TransitPermit();
                                    objTP.PERMIT_NO = ObjPGResponse.PRN;
                                    objTP.DIVISION_OFFICE = Permitdetails.Rows[0]["DIVISION_OFFICE"].ToString();
                                    objTP.RANGE_OFFICE = Permitdetails.Rows[0]["RANGE_OFFICE"].ToString();
                                    objTP.TO_STATE = Convert.ToInt32(Permitdetails.Rows[0]["TO_STATE"].ToString());
                                    objTP.ENTEREDBY = UserID; // Convert.ToInt64(Session["UserId"].ToString());
                                    string PermitNo = objTP.CREATE_RequestPermit(objTP);
                                }
                                else // Added to save mismatch in payment
                                {
                                    cs.Trn_Status_Code = 0;
                                    status1 = 0;
                                    fmdssStatus = 0;
                                }
                            }
                            finally
                            {

                            }
                        }

                        if (fmdssStatus == 1)
                        {
                            dtrow["TRANSACTION STATUS"] = "SUCCESS";
                            // SendSMSEmailForSuccessTransaction();

                        }
                        else
                        {
                            dtrow["TRANSACTION STATUS"] = "FAILED";

                        }
                        dt.Rows.Add(dtrow);
                        ViewData.Model = dt.AsEnumerable();
                    }
                    #endregion


                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                }
               return View("TransactionStatus");
               // return View("dashboard", "TransitPermitCitizen");
            }
            return View();
        }


        public string sendRequest()
        {
            string PermitId = "";



            return PermitId;



        }

        public void SendSMSEmailForSuccessTransaction()
        {
            //#region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

            //SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            //SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            //string body = string.Empty;

            //DataTable DT = objSMSandEMAILtemplate.GetUserDetails(Session["permitId"].ToString(), "CitizenTransitPermit");
            //if (DT.Rows.Count > 0)
            //{
            //    if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
            //    {
            //        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["WildLifeTicketEmailTemplate"].ToString()));

            //        objSMS_EMail_Services.sendEMail("WildLife Ticket Booking for RequestID : " + Convert.ToString(DT.Rows[0]["RequestID"]), body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["WildLifeTicketEmail_CC"].ToString());

            //        body = string.Empty;

            //    }

            //    if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
            //    {
            //        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["WildLifeTicketSMSTemplate"].ToString()));

            //        SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["Mobile"]), body);

            //        body = string.Empty;
            //    }

            //}

            //#endregion

        }




        public List<SelectListItem> getDropdown(string option)
        {
            try
            {
                objtp.UserId = Convert.ToInt64(Session["UserId"]);
                objtp.Option = option;
                DataTable dtp = objtp.Fill_Dropdown();
                if (option == "0")
                {
                    List<SelectListItem> blnklist = new List<SelectListItem>();

                    blnklist.Add(new SelectListItem { Text = "--Select--", Value = "" });
                    return blnklist;
                }

                if (option == "1" || option == "13")
                {
                    List<SelectListItem> lstDivision = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstDivision.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }
                    return lstDivision;
                }
                else if (option == "3")
                {
                    List<SelectListItem> lstState = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstState.Add(new SelectListItem { Text = @dr["STATENAME"].ToString(), Value = @dr["StateID"].ToString() });
                    }
                    return lstState;
                }
                else if (option == "4")
                {
                    List<SelectListItem> lstDistrict = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstDistrict.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }
                    return lstDistrict;
                }
                else if (option == "5")
                {
                    List<SelectListItem> lstVehicle = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["TransportModeID"].ToString() });
                    }
                    return lstVehicle;
                }
                else if (option == "10")
                {
                    List<SelectListItem> lstVehicle = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstVehicle.Add(new SelectListItem { Text = @dr["ProductName"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    return lstVehicle;
                }

                else
                {
                    List<SelectListItem> lstVehicle = new List<SelectListItem>();
                    return lstVehicle;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public void getUserDetails(string option)
        {
            try
            {
                DataTable dtp = objtp.GetUserDetails("11");
                if (dtp != null && dtp.Rows.Count > 0)
                {

                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        objtp.APPLICANT_ADDRESS = Convert.ToString(@dr["Address"]);
                        objtp.APPLICANT_NAME = Convert.ToString(@dr["Name"]);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public ActionResult getTPFees(string VehicleId)
        {
            try
            {
                string TP_Fees = string.Empty;
                objtp.VEHICLE_TYPE = VehicleId;
                objtp.Option = "6";
                DataTable dtp = objtp.Fill_Dropdown();
                if (dtp.Rows.Count > 0)
                {

                    TP_Fees = dtp.Rows[0]["VehiclePrice"].ToString();
                }

                return Json(TP_Fees, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Function to show list of tagged file
        /// </summary>
        /// <param name="Id"> Id </param>
        /// <returns>list of json data</returns>
        public JsonResult ProduceList()
        {
            try
            {
                TransitPermit objProduce;
                List<TransitPermit> lstProduce = new List<TransitPermit>();
                objtp.Option = "7";
                DataTable dtp = objtp.Fill_Dropdown();
                if (dtp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtp.Rows)
                    {
                        objProduce = new TransitPermit();
                        lstProduce.Add(new TransitPermit
                        {
                            PRODUCE_ID = Convert.ToString(dr["ID"]),
                            PRODUCE_NAME = Convert.ToString(dr["ProductName"])
                        });
                    }
                }
                return this.Json(new { list1 = lstProduce, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return this.Json(new { Success = false, Message = "An error occurred, please try later! " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }


        public ActionResult dashboard()
        {
            CitizenTransmitpermitSummary obj = new CitizenTransmitpermitSummary();
            obj.UserId = Convert.ToInt64(Session["UserID"].ToString());

            DataSet data = new DataSet();
            data = obj.CitizenTransmitpermitSummaryCount(obj.UserId);
            if (data.Tables[0].Rows.Count > 0)
            {
                obj.CitizenTransmitpermitPending = data.Tables[0].Rows.Count.ToString();//  data.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                obj.CitizenTransmitpermitPending = "0";
            }
            if (data.Tables[1].Rows.Count > 0)
            {
                obj.CitizenTransmitpermitApproved = data.Tables[1].Rows.Count.ToString();//data.Tables[1].Rows[0][0].ToString();
            }
            else
            {
                obj.CitizenTransmitpermitApproved = "0";
            }
            if (data.Tables[2].Rows.Count > 0)
            {
                obj.CitizenTransmitpermitReject = data.Tables[2].Rows.Count.ToString();//data.Tables[2].Rows[0][0].ToString();
            }
            else
            {
                obj.CitizenTransmitpermitReject = "0";
            }
            return View(obj);
        }


        public ActionResult dashboardDetails(int listid)
        {
            CitizenTransmitpermitSummary obj = new CitizenTransmitpermitSummary();
            DataTable data = new DataTable();
            obj.UserId = Convert.ToInt64(Session["UserID"].ToString());
            string role = Convert.ToString(Session["CURRENT_ROLE"]);

            data = obj.GetdashboardDetails(obj.UserId);
            ViewBag.datalist = data;
            ViewBag.role = role;
            return View();
        }


        public ActionResult TransmitPermitTransation(string Id)
        {

            CitizenTransmitpermitSummary objmodel = new CitizenTransmitpermitSummary();
            try
            {
                string role = Convert.ToString(Session["CURRENT_ROLE"]);
                ViewBag.role = role;
                DataSet data = new DataSet();
                DataTable dataTable = new DataTable();
                data = objmodel.CitizenTransmitpermitApprove(Id);
                dataTable = data.Tables[0];
                //List<CitizenTransmitpermitSummary> datas = FMDSS.APIRepo.Util.GetListFromTable<CitizenTransmitpermitSummary>(dataTable);
                objmodel.PERMIT_NO = dataTable.Rows[0]["PERMIT_NO"].ToString();
                objmodel.DIVISION_OFFICE = dataTable.Rows[0]["DIVISION_OFFICE"].ToString();
                objmodel.RANGE_OFFICE = dataTable.Rows[0]["RANGE_OFFICE"].ToString();
                objmodel.APPLICANT_NAME = dataTable.Rows[0]["APPLICANT_NAME"].ToString();
                objmodel.APPLICANT_ADDRESS = dataTable.Rows[0]["APPLICANT_ADDRESS"].ToString();
                objmodel.APPLICANT_DISTRICTNAME = dataTable.Rows[0]["APPLICANT_DISTRICTNAME"].ToString();
                objmodel.APPLICANT_TEHSILNAME = dataTable.Rows[0]["APPLICANT_TEHSILNAME"].ToString();
                objmodel.APPLICANT_VILLAGENAME = dataTable.Rows[0]["APPLICANT_VILLAGENAME"].ToString();
                objmodel.AadharOrBhamasha = dataTable.Rows[0]["AadharOrBhamasha"].ToString();
                objmodel.AadharOrBhamashaNumber = dataTable.Rows[0]["AadharOrBhamashaNumber"].ToString();
                objmodel.LANDHOLDER_NAME = dataTable.Rows[0]["LANDHOLDER_NAME"].ToString();
                objmodel.TEHSIL = dataTable.Rows[0]["TEHSIL"].ToString();
                objmodel.VILLAGE_NAME = dataTable.Rows[0]["VILLAGE_NAME"].ToString();
                objmodel.KHASRA_NO = dataTable.Rows[0]["KHASRA_NO"].ToString();
                objmodel.PLACE_NAME = dataTable.Rows[0]["PLACE_NAME"].ToString();
                objmodel.LANDHOLDING_AREA = Convert.ToDecimal(dataTable.Rows[0]["LANDHOLDING_AREA"].ToString());
                objmodel.VEHICLE_NO = dataTable.Rows[0]["VEHICLE_NO"].ToString();
                objmodel.VEHICLE_TYPE = dataTable.Rows[0]["VEHICLE_TYPE"].ToString();
                objmodel.TP_FEES = Convert.ToInt32(dataTable.Rows[0]["TP_FEES"].ToString());
                objmodel.FROM_STATENAME = dataTable.Rows[0]["FROM_STATENAME"].ToString();
                objmodel.TO_STATENAME = dataTable.Rows[0]["TO_STATENAME"].ToString();
                objmodel.FROM_DISTRICTNAME = dataTable.Rows[0]["FROM_DISTRICTNAME"].ToString();
                objmodel.TO_DISTRICTNAME = dataTable.Rows[0]["TO_DISTRICTNAME"].ToString();
                objmodel.ROUTE_PLAN = dataTable.Rows[0]["ROUTE_PLAN"].ToString();
                objmodel.APPLICATIONSTATUS = Convert.ToInt32(dataTable.Rows[0]["APPLICATIONSTATUS"].ToString());
                ViewBag.productdeteils = data.Tables[1].Rows;
            }

            catch (Exception ex)
            {

            }
            return View(objmodel);
        }

        [HttpPost]
        public ActionResult TransmitPermitTransation(CitizenTransmitpermitSummary objmodel, string Command, string OTP, string TransationID)
        {

            try
            {
                objmodel.UserId = Convert.ToInt64(Session["UserID"].ToString());
                objmodel.ACTION = Convert.ToInt32(Command);
                string PermitNo = objmodel.CitizenTransmitpermitPermission(objmodel);
                if (Command  == "2")
                {
                    #region Call E-Sign API
                    clsVerifyOTP request = new clsVerifyOTP();
                    request.otp = OTP;
                    request.transactionid = TransationID;
                    clsVerifyOTPResponce response = FMDSS.App_Start.cls_ESignIntegration.VerifyOTPAndGenrateTransation(request, PermitNo, Command, "Tbl_Citizen_TransitPermit");
                    if (!string.IsNullOrEmpty(response.TransactionId))
                        TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " has been Approved Sucessfully and Genrated PDF with E-Sign";
                    else
                        TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " has been Approved Sucessfully but not Genrated PDF with E-Sign";
                    #endregion
                }
                DataSet data = new DataSet();
                data = objmodel.CitizenTransmitpermitSummaryCount(objmodel.UserId);
                objmodel.CitizenTransmitpermitPending = data.Tables[0].Rows[0][0].ToString();
                objmodel.CitizenTransmitpermitApproved = data.Tables[1].Rows[0][0].ToString();
                objmodel.CitizenTransmitpermitReject = data.Tables[2].Rows[0][0].ToString();
                objmodel.CitizenTransmitpermitTotal = data.Tables[3].Rows[0][0].ToString();
            }

            catch (Exception ex)
            {

            }

            return RedirectToAction("ForesterDashboard", "ForesterAction");
        }


        public ActionResult TransmitPermitTransationDetails(string Id)
        {

            CitizenTransmitpermitSummary objmodel = new CitizenTransmitpermitSummary();
            try
            {

                DataSet data = new DataSet();
                DataTable dataTable = new DataTable();
                data = objmodel.CitizenTransmitpermitApprove(Id);
                dataTable = data.Tables[0];
                //List<CitizenTransmitpermitSummary> datas = FMDSS.APIRepo.Util.GetListFromTable<CitizenTransmitpermitSummary>(dataTable);
                objmodel.PERMIT_NO = dataTable.Rows[0]["PERMIT_NO"].ToString();
                objmodel.DIVISION_OFFICE = dataTable.Rows[0]["DIVISION_OFFICE"].ToString();
                objmodel.RANGE_OFFICE = dataTable.Rows[0]["RANGE_OFFICE"].ToString();
                objmodel.APPLICANT_NAME = dataTable.Rows[0]["APPLICANT_NAME"].ToString();
                objmodel.APPLICANT_ADDRESS = dataTable.Rows[0]["APPLICANT_ADDRESS"].ToString();
                objmodel.APPLICANT_DISTRICTNAME = dataTable.Rows[0]["APPLICANT_DISTRICTNAME"].ToString();
                objmodel.APPLICANT_TEHSILNAME = dataTable.Rows[0]["APPLICANT_TEHSILNAME"].ToString();
                objmodel.APPLICANT_VILLAGENAME = dataTable.Rows[0]["APPLICANT_VILLAGENAME"].ToString();
                objmodel.AadharOrBhamasha = dataTable.Rows[0]["AadharOrBhamasha"].ToString();
                objmodel.AadharOrBhamashaNumber = dataTable.Rows[0]["AadharOrBhamashaNumber"].ToString();
                objmodel.LANDHOLDER_NAME = dataTable.Rows[0]["LANDHOLDER_NAME"].ToString();
                objmodel.TEHSIL = dataTable.Rows[0]["TEHSIL"].ToString();
                objmodel.VILLAGE_NAME = dataTable.Rows[0]["VILLAGE_NAME"].ToString();
                objmodel.KHASRA_NO = dataTable.Rows[0]["KHASRA_NO"].ToString();
                objmodel.PLACE_NAME = dataTable.Rows[0]["PLACE_NAME"].ToString();
                objmodel.LANDHOLDING_AREA = Convert.ToDecimal(dataTable.Rows[0]["LANDHOLDING_AREA"].ToString());
                objmodel.VEHICLE_NO = dataTable.Rows[0]["VEHICLE_NO"].ToString();
                objmodel.VEHICLE_TYPE = dataTable.Rows[0]["VEHICLE_TYPE"].ToString();
                objmodel.TP_FEES = Convert.ToInt32(dataTable.Rows[0]["TP_FEES"].ToString());
                objmodel.FROM_STATENAME = dataTable.Rows[0]["FROM_STATENAME"].ToString();
                objmodel.TO_STATENAME = dataTable.Rows[0]["TO_STATENAME"].ToString();
                objmodel.FROM_DISTRICTNAME = dataTable.Rows[0]["FROM_DISTRICTNAME"].ToString();
                objmodel.TO_DISTRICTNAME = dataTable.Rows[0]["TO_DISTRICTNAME"].ToString();
                objmodel.ROUTE_PLAN = dataTable.Rows[0]["ROUTE_PLAN"].ToString();
                objmodel.APPLICATIONSTATUS = Convert.ToInt32(dataTable.Rows[0]["APPLICATIONSTATUS"].ToString());
                objmodel.Reason = dataTable.Rows[0]["Reason"].ToString();
                ViewBag.productdeteils = data.Tables[1].Rows;
            }

            catch (Exception ex)
            {

            }
            return View(objmodel);
        }



        public void PrintTransmitPermit(string Id)
        {
            string filename = Server.MapPath("~/ApproveTransitPermit/TransitPermit_" + Id + ".pdf");
            FileInfo file = new FileInfo(filename);
            if (file.Exists)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=\"TransitPermit_" + Id + ".pdf");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "Application/pdf";
                Response.TransmitFile(filename);
                Response.End();
            }

        }



    }
}
