//////////////// Bug no-387,432,439




using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.CitizenService.ProductionServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.CitizenService.ProductionServices
{
    public class TransitPermissionController : BaseController
    {
        //
        // GET: /TransitPermission/
        TransitPermission obj_Tp = new TransitPermission();
        List<TransitPermission> lstTp = new List<TransitPermission>();
        List<SelectListItem> items = new List<SelectListItem>();
        Int64 UserID = 0;
        int ModuleID = 1;
        public ActionResult TransitPermission()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                ViewBag.Applicant_Type = new SelectList(Common.GetApplicantType(), "Value", "Text");

                if (Session["User"] != null)
                {

                    ViewData["Name"] = Session["User"];
                }
                if (Session["UserId"] != null)
                {
                    #region Bind All Request Transaction
                    DataTable dtp = obj_Tp.Select_All_TransactionReq_ByUserID(Convert.ToInt64(Session["UserId"].ToString()));
                    ViewBag.fname = dtp;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["ReqID"].ToString() , Value = @dr["TransFrom"].ToString() });
                    }
                    
                    ViewBag.ReqID = items;

                    #endregion


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

                return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FetchAllTransactionDetail(string reqType, string reqID)
        {
           
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<TransitPermission> lstTp = new List<TransitPermission>();
          
            try
            {

                if (!string.IsNullOrEmpty(reqID))
                {

                    DataTable dtp = obj_Tp.Select_All_Transaction_ByUserID(Convert.ToInt64(Session["UserId"].ToString()),reqType, reqID);
                    foreach (DataRow dr in dtp.Rows)
                    {
                        lstTp.Add(new TransitPermission()
                        {
                            RowID = dr["RowID"].ToString(),
                            ReqID = dr["ReqID"].ToString(),
                            DistNAME = dr["DistNAME"].ToString(),
                            VillRange = dr["VillRange"].ToString(),
                            Location = dr["Location"].ToString(),
                            Product = dr["Product"].ToString(),
                            QTy = dr["QTy"].ToString(),
                            Unit = dr["Unit"].ToString(),
                            PaidAMT = Convert.ToDecimal(dr["PaidAMT"].ToString())

                        });
                    }
                  

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(lstTp, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Bind District dropdown
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public JsonResult vehicleCategoryMode(string TransportModel)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                string[] TobeDistinct = { "vehicleCatID", "Vehicle_Type" };

                if (!String.IsNullOrEmpty(TransportModel) && TransportModel=="By Road")
                {

                    DataTable dt = obj_Tp.Select_VehicleForTp();
                   //dt = dt.Select("CategoryID in (1,2)").CopyToDataTable();
                   //var dt1= dt.AsEnumerable().Select(t => new { id = t.Field<Int64>("ID"), name = t.Field<string>("Vehicle_Type") }).Distinct().ToList();

                   DataTable dtUniqRecords = new DataTable();
                   dtUniqRecords = dt.DefaultView.ToTable(true, TobeDistinct);


                   return dtToViewBagJSON(dtUniqRecords, "Vehicle_Type", "vehicleCatID");

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }

            return null;
        }

        /// <summary>
        /// Bind District dropdown
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public JsonResult Bindvehicle(string VehicleType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                string[] TobeDistinct = {"vehicleCatID", "vehicleID", "Vehicle_Name" };

                if (!String.IsNullOrEmpty(VehicleType))
                {

                    DataTable dt = obj_Tp.Select_VehicleForTp();
                    //dt = dt.Select("CategoryID in (1,2)").CopyToDataTable();
                    //var dt1= dt.AsEnumerable().Select(t => new { id = t.Field<Int64>("ID"), name = t.Field<string>("Vehicle_Type") }).Distinct().ToList();

                    DataTable dtUniqRecords = new DataTable();
                    dtUniqRecords = dt.DefaultView.ToTable(true, TobeDistinct).Select("vehicleCatID='" + VehicleType + "'").CopyToDataTable();


                    return dtToViewBagJSON(dtUniqRecords, "Vehicle_Name", "vehicleID");

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }

            return null;
        }


        [HttpPost]
        public void Pay()
        {
            //EM33172142@5488
            Payment pay = new Payment();
            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
            string encrypt = pay.RequestString("EM33172142@5488", Session["requestId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "Auction/Payment", Session["User"].ToString(), "", "");
            Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
        }

        /// <summary>
        /// check payment status
        /// </summary>
        /// <returns></returns>
        public ActionResult Payment()
        {
            if (Session["requestId"] != null)
            {
                //TicketBooking cs = new TicketBooking();
                int status1 = 0;
                TransitPermission au = new TransitPermission();
                Payment pay = new Payment();
                DataTable dt = new DataTable();
                #region Datarow defination
                if (dt.Rows.Count == 0)
                {
                    dt.Columns.Add("TRANSACTION STATUS");
                    dt.Columns.Add("REQUEST ID");
                    dt.Columns.Add("EMITRA TRANSACTION ID");
                    dt.Columns.Add("TRANSACTION TIME");
                    dt.Columns.Add("TRANSACTION AMOUNT");
                    dt.Columns.Add("USER NAME");
                    dt.Columns.Add("TRANSACTION BANK DETAILS");
                }
                #endregion

                string response = Request.QueryString["trnParams"].ToString();
                string ResponseResult = pay.ProcesTranscationresponce(response);

                #region Response decryption

                string str1, str2;
                str1 = ResponseResult.Replace("<RESPONSE ", "");
                str2 = str1.Replace("></RESPONSE>", "");
                string[] Responsearr = str2.Split(' ');
                string checkFail = "STATUS='FAILED'";
                string checkSucess = "STATUS='SUCCESS'";
                string rowstatus1 = "";
                foreach (var item in Responsearr)
                {
                    if (item.Equals(checkFail))
                    {
                        string[] status2 = item.Split('=');
                        rowstatus1 = status2[1].ToString();
                    }
                    if (item.Equals(checkSucess))
                    {
                        string[] status2 = item.Split('=');
                        rowstatus1 = status2[1].ToString();
                    }
                }
                int status1len = Convert.ToInt32(rowstatus1.ToString().Length);
                string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 2);
                #endregion
                #region Response Status
                if (finalstatus1 == "FAILED")
                {
                    string[] emitratransid = Responsearr[0].Split('=');
                    string[] transtime = Responsearr[1].Split('=');
                    string[] reqid = Responsearr[2].Split('=');
                    string[] reqamt = Responsearr[3].Split('=');
                    string[] username = Responsearr[4].Split('=');
                    string[] status = Responsearr[7].Split('=');


                    DataRow dtrow = dt.NewRow();
                    string rowstatus = status[1].ToString();
                    int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                    string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
                    string rowreqid = reqid[1].ToString();
                    int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                    string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
                    string rawemitra = emitratransid[1].ToString();
                    int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                    string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
                    au.TransactionID = "0";
                    au.RequestId = finalreqid;
                    string rawtransamount = reqamt[1].ToString();
                    int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                    string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                    string rawusername = username[1].ToString();
                    int usernamelen = Convert.ToInt32(rawusername.Length);
                    string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);

                    dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                    dtrow["REQUEST ID"] = finalreqid;
                    dtrow["EMITRA TRANSACTION ID"] = "";
                    dtrow["TRANSACTION TIME"] = "";//transtime[1];
                    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    dtrow["USER NAME"] = finalUserName;

                    if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                    {
                        au.Trn_Status_Code = 0;
                    }
                    dt.Rows.Add(dtrow);
                }
                else if (finalstatus1 == "SUCCESS")
                {
                    string[] emitratransid = Responsearr[0].Split('=');
                    string[] transtime = Responsearr[1].Split('=');
                    string[] reqid = Responsearr[3].Split('=');
                    string[] reqamt = Responsearr[4].Split('=');
                    string[] username = Responsearr[5].Split('=');
                    string[] status = Responsearr[8].Split('=');
                    string[] bank = Responsearr[9].Split('=');
                    string[] bankbidno = Responsearr[13].Split('=');

                    DataRow dtrow = dt.NewRow();
                    string rowstatus = status[1].ToString();
                    int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                    string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
                    string rowreqid = reqid[1].ToString();
                    int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                    string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
                    string rawemitra = emitratransid[1].ToString();
                    int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                    string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
                    au.TransactionID = finalreqid;
                    string rawtransamount = reqamt[1].ToString();
                    int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                    string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                    string rawusername = username[1].ToString();
                    int usernamelen = Convert.ToInt32(rawusername.Length);
                    string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);
                    string rawbank = bank[1].ToString();
                    int banklen = Convert.ToInt32(rawbank.Length);
                    string finalbank = rawbank.ToString().Substring(1, banklen - 2);
                    dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                    dtrow["REQUEST ID"] = finalreqid;
                    dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
                    au.TransactionID = finalemitraid;
                    dtrow["TRANSACTION TIME"] = transtime[1] + Responsearr[2];
                    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    dtrow["USER NAME"] = finalUserName;
                    dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                    if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                    {
                        au.Trn_Status_Code = 1;
                        status1 = 1;
                    }
                    dt.Rows.Add(dtrow);
                    if (Session["requestId"] != null)
                    {
                        if (Session["UserId"] != null)
                        {
                            au.CreatedBy = Convert.ToInt64(Session["UserID"]);
                        }
                        au.RequestId = Session["requestId"].ToString();
                       // au.UpdateTransactionStatus();
                    }
                }
                #endregion
                //SMS_EMail_Services SE = new SMS_EMail_Services();

                //if (Session["PaymentType"].ToString() == "FilmShooting")
                //{                 
                //    DataTable dtf = new DataTable();
                //    dtf=fs.UpdateTransactionStatus("1");
                //    if (dtf != null)
                //    {
                //        if (dt.Rows.Count > 0)
                //        {
                //            string subject = "Request for Film Shooting Permission Review";
                //            string body = Common.GenerateReviwerBody(dtf.Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), "Film Shooting Permission");
                //            SE.sendEMail(subject, body, dtf.Rows[0]["EmailId"].ToString(), "");
                //            // SMS_EMail_Services.sendBulkSMS(dt.Rows[0]["Mobile"].ToString(), "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                //            if (Session["SSODetail"] != null)
                //            {
                //                UserProfile up = (UserProfile)Session["SSODetail"];
                //                SMS_EMail_Services.sendSingleSMS(up.MobileNumber, "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                //            }
                //        }
                //    }
                //}

                //cs.UpdateTransactionStatus("3");

                ViewData.Model = dt.AsEnumerable();
                //return View("TransactionStatus");
                return View("dashboard", "TransitPermitCitizen");
            }
            return View("dashboard", "TransitPermitCitizen");
        }


          /// <summary>
        /// Save data into database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FormCollection fm, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            TransitPermission tObj = new TransitPermission();
            ActionResult actionResult = null;
            try
            {
                DateTime now = DateTime.Now;
                string requesteId = now.Ticks.ToString();
             
                if (Session["UserID"] != null)
                {

                    tObj.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (requesteId == "")
                {
                    tObj.RequestId = "";
                }
                else
                {
                    tObj.RequestId = requesteId;
                }

                if (fm["Applicant_type"].ToString() == "")
                {
                    tObj.Applicant_Type = "";
                }
                else
                {
                    tObj.Applicant_Type = fm["Applicant_type"].ToString();

                }
                if (fm["TransID"].ToString() == "")
                {
                    tObj.ReqID = "";
                }
                else
                {
                    tObj.ReqID = fm["TransID"].ToString();

                }

                if (fm["Echalan"].ToString() == "")
                {
                    tObj.Echalan = "";
                }
                else
                {
                    tObj.Echalan = fm["Echalan"].ToString();

                }
                if (fm["Paid_amount"].ToString() == "")
                {
                    tObj.Paid_amount = 0;
                }
                else
                {
                    tObj.Paid_amount =Convert.ToDecimal(fm["Paid_amount"].ToString());

                }
                if (fm["ToLocation"].ToString() == "")
                {
                    tObj.ToLocation = "";
                }
                else
                {
                    tObj.ToLocation = fm["ToLocation"].ToString();

                }
                if (fm["TransferQty"].ToString() == "")
                {
                    tObj.TransferQty = "";
                }
                else
                {
                    tObj.TransferQty = fm["TransferQty"].ToString();

                }
                

                if (fm["TransportMode"].ToString() == "")
                {
                    tObj.TransportMode = "";
                }
                else
                {
                    tObj.TransportMode = fm["TransportMode"].ToString();

                }
                if (fm["VehicleNo"].ToString() == "")
                {
                    tObj.VehicleNo = "";
                }
                else
                {
                    tObj.VehicleNo = fm["VehicleNo"].ToString();

                }
                if (fm["VehicleType"].ToString() == "")
                {
                    tObj.VehicleType = 0;
                }
                else
                {
                    tObj.VehicleType =Convert.ToInt64(fm["VehicleType"].ToString());

                }
                if (fm["Vehicle"].ToString() == "")
                {
                    tObj.Vehicle =0;
                }
                else
                {
                    tObj.Vehicle =Convert.ToInt64(fm["Vehicle"].ToString());

                }
                if (fm["DriverLicense"].ToString() == "")
                {
                    tObj.DriverLicense = "";
                }
                else
                {
                    tObj.DriverLicense = fm["DriverLicense"].ToString();

                }
                if (fm["DriverName"].ToString() == "")
                {
                    tObj.DriverName = "";
                }
                else
                {
                    tObj.DriverName = fm["DriverName"].ToString();

                }
                if (fm["DriverMobileno"].ToString() == "")
                {
                    tObj.DriverMobileno = "";
                }
                else
                {
                    tObj.DriverMobileno = fm["DriverMobileno"].ToString();

                }
               
                if (fm["Durationfrom"].ToString() == "")
                {
                    tObj.Durationfrom = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    tObj.Durationfrom = DateTime.ParseExact(fm["DurationFrom"].ToString(), "dd/MM/yyyy", null);

                }
                if (fm["Durationto"].ToString() == "")
                {
                    tObj.Durationto = Convert.ToDateTime(SqlDateTime.Null);

                }
                else
                {
                    tObj.Durationto = DateTime.ParseExact(fm["Durationto"].ToString(), "dd/MM/yyyy", null);

                }

                if (fm["Amounttobepaid"].ToString() == "")
                {
                    tObj.Amounttobepaid = 0;
                }
                else
                {
                    tObj.Amounttobepaid = Convert.ToDecimal(fm["Amounttobepaid"].ToString());

                }
                if (Command == "Submit")
                {
                    string status = tObj.CreateTransitPermission();
                    if (!String.IsNullOrEmpty(status.ToString()))
                    {


                        if(tObj.Amounttobepaid != null)
                        {
                            if (Session["KioskUserId"] != null && Session["KioskCtznName"] != null)
                            {
                                tObj.kioskId = Session["KioskUserId"].ToString();
                                KioskPaymentDetails _obj = new KioskPaymentDetails();
                                _obj.ModuleId = 1;
                                _obj.ServiceTypeId = 2;
                                _obj.PermissionId = 1;
                                _obj.SubPermissionId = 1;
                                DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                                if (dtKiosk.Rows.Count > 0)
                                {
                                    _obj.Fee = Convert.ToDecimal(Session["totalprice"]);
                                    _obj.KioskCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KisokCharges"].ToString());
                                    _obj.TotalAmount = _obj.Fee + _obj.KioskCharges + _obj.PDFDocCharges + _obj.KMLCharges;
                                    return PartialView("KioskPaymentDetail", _obj);
                                }
                            }
                            else
                            {

                                #region payment
                                DataTable dtColmn = new DataTable();
                                if (dtColmn.Rows.Count == 0)
                                {
                                    dtColmn.Columns.Add("Request_Id");
                                    dtColmn.Columns.Add("Name");
                                    dtColmn.Columns.Add("PaidAmt");
                                    dtColmn.Columns.Add("Status");
                                }
                                //decimal biddingAmt = 0;

                                DataRow dtrow = dtColmn.NewRow();

                                dtrow["Request_Id"] = tObj.RequestId;
                                Session["requestId"] = tObj.RequestId;
                                dtrow["Name"] = Session["User"].ToString(); ;
                                dtrow["PaidAmt"] = tObj.Amounttobepaid;
                                Session["totalprice"] = tObj.Amounttobepaid;
                                dtrow["Status"] = "Pending";

                                dtColmn.Rows.Add(dtrow);
                                ViewData.Model = dtColmn.AsEnumerable();

                                #endregion

                                actionResult = View("TransitPayment");
                            }
                        }
                    }
                    else
                    {
                        actionResult = RedirectToAction("TransitPermission", "TransitPermission");
                    }



                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return actionResult;
        }



        /// <summary>
        /// dtToViewBagJSON
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        /// <returns></returns>
        public JsonResult dtToViewBagJSON(DataTable dt, string TextField, string ValueField)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    //DataTable dt = _obj.SelectMicroPlanByVilageCode(Village_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr[TextField].ToString(), Value = @dr[ValueField].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }


    }
}
