//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : FPMReleaseController
//  Description  : File contains calling functions from UI
//  Date Created : 13-June-2016
//  History      : 
//  Version      : 1.0
//  Author       : Ashok Yadav
//  Modified By  : Ashok Yadav
//  Modified On  : 13-June-2016
//  Reviewed By  :  
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.ForestProtection;
using System.Data;
using System.Data.SqlClient;
using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionService;
using System.Configuration;
using System.IO;
namespace FMDSS.Controllers.ForestProtection
{
    public class FPMReleaseController : BaseController
    {
        List<ForestReleases> SeizedIteam = new List<ForestReleases>();
        List<ForestReleases> CompoundIteam = new List<ForestReleases>();
        Int64 UserID = 0;
        int ModuleID = 4;
        //
        // GET: /FPMRelease/
        /// <summary>
        /// Use for seized item details
        /// </summary>
        /// <returns></returns>
        public ActionResult SeizedItem()
        {
            ForestReleases obj = new ForestReleases();
            DataSet DS = obj.Get_SizedIteamList(Session["SSOID"].ToString());
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                SeizedIteam.Add(
                    new ForestReleases()
                    {
                        OffenceCategory = dr["FOCategory"].ToString(),
                        OffenseSubCategoryWildLife = dr["OffenseSubCategoryWildLife"].ToString(),
                        OffenseCode = dr["OffenseCode"].ToString(),
                        FirstOfficerID = dr["FirstOfficerID"].ToString(),
                        SecondOfficerID = dr["SecondOfficerID"].ToString(),
                        Total = dr["Total"].ToString(),
                        //StatusDesc = dr["StatusDesc"].ToString(),
                        //Status = dr["Status"].ToString(),
                    });

            }
            ViewData["SeizedIteam"] = SeizedIteam;

            return View();
        }

        /// <summary>
        /// Get all data by offence code
        /// </summary>
        /// <param name="OffenceCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetIteams(string OffenceCode)
        {
            ForestReleases obj = new ForestReleases();
            DataSet DS = obj.Get_SizedIteamDetails(OffenceCode);
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                SeizedIteam.Add(
                    new ForestReleases()
                    {
                        ArticalName = dr["ArticalName"].ToString(),
                        ArticalDetail = dr["Description"].ToString(),
                        Quantity = dr["Quantity"].ToString(),
                        Status = dr["Status"].ToString(),
                        chkStatus = checkedStatus(dr["Status"].ToString()),
                        ID = Convert.ToInt64(dr["SeizedanimalArticleId"].ToString()),
                        TableName = dr["TableName"].ToString(),
                    });

            }
            ViewData["IteamDetails"] = SeizedIteam;
            return Json(new { list2 = SeizedIteam }, JsonRequestBehavior.AllowGet);

            //   return View();
        }

        public bool checkedStatus(string str)
        {
            bool chk = false;
            if (str == "Not Applied")
            {
                chk = false;
            }
            else
            {
                chk = true;
            }
            return chk;
        }

        /// <summary>
        /// Used to request seized item
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitSeziedItemRequest(FormCollection fm, string Command, HttpPostedFileBase fileappseized, HttpPostedFileBase filedocOwnership)
        {
       
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = "~/ForestProtectionDocument/";
            string status = "";
            if (Command == "Submit")
            {
                
                ForestReleases obj = new ForestReleases();
                if (Session["UserId"] != null)
                {
                    obj.UserID = Convert.ToInt64(Session["UserID"]);
                }
                if (fm["hdSeized"].ToString() == "")
                {
                    obj.OffenseCode = "0";
                }
                else
                {
                    obj.OffenseCode = fm["hdSeized"].ToString();
                }
                if (fileappseized != null && fileappseized.ContentLength > 0)
                {
                    FileName = Path.GetFileName(fileappseized.FileName);
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    obj.AppOfSeized = path;
                    fileappseized.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    obj.AppOfSeized = "";
                }

                if (filedocOwnership != null && filedocOwnership.ContentLength > 0)
                {
                    FileName = Path.GetFileName(filedocOwnership.FileName);
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    obj.DocOfownership = path;
                    filedocOwnership.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    obj.DocOfownership = "";
                }
                if (Session["FPMSeizedItem"] != null)
                {
                    List<ForestReleases> list = (List<ForestReleases>)Session["FPMSeizedItem"];
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            obj.ID = list[0].ID;
                            obj.TableName = list[0].TableName;
                            obj.Status = list[0].Status.Trim();
                            if (obj.Status == "Not")
                            {
                                status = obj.SubmitSizedIteamRequest(obj);
                            }
                        }
                       //list[0].ID
                        
                    }
                }


                if (status != "")
                {
                    TempData["SizedIteamStatus"] = "Request submit Successfully";                   
                }
                else
                {
                    TempData["SizedIteamStatus"] = "Not inserted";
                }

            }
            if(Session["Role"].ToString()=="CITIZEN"){
            return RedirectToAction("SeizedItem", "FPMRelease");
            }
            else{
                return RedirectToAction("ReleaseSeizedItem", "FPMRelease");
            }
            
            
        }

        /// <summary>
        /// used to get compound iteam
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCompoundIteams()
        {
            ForestReleases obj = new ForestReleases();
            DataSet DS = obj.Get_Compoundlist(Session["SSOID"].ToString());
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                CompoundIteam.Add(
                    new ForestReleases()
                    {                     
                        OffenseCode = dr["OffenseCode"].ToString(),                        
                        SettlementAmount = dr["SettlementAmount"].ToString(),
                        AmountPaid = dr["AmountPaid"].ToString(),
                        CaseStatus = dr["CaseStatus"].ToString(),
                        FineAmount = dr["FineAmount"].ToString(),
                        DfoDecision = dr["DfoDecision"].ToString(),
                        StatusDesc = dr["StatusDesc"].ToString(),
                        Status = dr["Status"].ToString(),
                    });

            }
            ViewData["CompoundIteam"] = CompoundIteam;
            return View();
        }



        /// <summary>
        /// get item detail by code
        /// </summary>
        /// <param name="OffenceCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCompoundIteamDetails(string OffenceCode)
        {
            ForestReleases obj = null;
            ForestReleases obj1 = new ForestReleases();
            DataSet DS = obj1.Get_CompoundteamDetails(OffenceCode);
            if (DS.Tables[0].Rows.Count > 0)
            {
                obj = new ForestReleases();               
                obj.OffenseCode = DS.Tables[0].Rows[0]["OffenseCode"].ToString();             
                obj.SettlementAmount = DS.Tables[0].Rows[0]["SettlementAmount"].ToString();
                obj.AmountPaid = DS.Tables[0].Rows[0]["AmountPaid"].ToString();
                obj.CaseStatus = DS.Tables[0].Rows[0]["CaseStatus"].ToString();
                obj.FineAmount = DS.Tables[0].Rows[0]["FineAmount"].ToString();
                obj.DfoDecision = DS.Tables[0].Rows[0]["DfoDecision"].ToString();
                obj.StatusDesc = DS.Tables[0].Rows[0]["StatusDesc"].ToString();
                obj.Status = DS.Tables[0].Rows[0]["Status"].ToString();
            }
            //ViewData["IteamDetails"] = SeizedIteam;
            return Json(new { list1 = obj }, JsonRequestBehavior.AllowGet);
            //   return View();
        }
        /// <summary>
        /// Used to request for compound item
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitCompoundItemRequest(FormCollection fm, string Command, HttpPostedFileBase AppOfcompounding)
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = "~/ForestProtectionDocument/";
            string status = "";
            if (Command == "Submit")
            {
                ForestReleases obj = new ForestReleases();
                if (Session["UserId"] != null)
                {
                    obj.UserID = Convert.ToInt64(Session["UserID"]);
                }
                if (fm["hdCompound"].ToString() == "")
                {
                    obj.OffenseCode = "0";
                }
                else
                {
                    obj.OffenseCode = fm["hdCompound"].ToString();
                   
                }

                if (fm["CompAmount"].ToString() == "")
                {
                    obj.CompAmount = "0";
                }
                else
                {
                    obj.CompAmount = fm["CompAmount"].ToString();
                }
                if (AppOfcompounding != null && AppOfcompounding.ContentLength > 0)
                {
                    FileName = Path.GetFileName(AppOfcompounding.FileName);
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    obj.AppOfcompounding = path;
                    AppOfcompounding.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    obj.AppOfcompounding = "";
                }
                obj.RequestId = DateTime.Now.Ticks.ToString();
                Session["RequestId"] = obj.RequestId;                
                status = obj.SubmitCompoundIteamRequest(obj);
                if (status != "")
                {                   
                    #region payment
                    DataTable dtColmn = new DataTable();
                    if (dtColmn.Rows.Count == 0)
                    {
                        dtColmn.Columns.Add("Transaction_Id");
                        dtColmn.Columns.Add("TotalFees");
                        dtColmn.Columns.Add("FineAmount");
                        dtColmn.Columns.Add("Discount");
                        dtColmn.Columns.Add("EnterBy");
                        dtColmn.Columns.Add("Status");
                    }
                    decimal totalPrice = 0;
                    totalPrice = Convert.ToDecimal(fm["CompAmount"].ToString());
                    Session["totalprice"] = totalPrice;
                    DataRow dtrow = dtColmn.NewRow();                 
                    dtrow["Transaction_Id"] = Session["RequestId"].ToString();
                    dtrow["TotalFees"] = totalPrice;
                    dtrow["FineAmount"] = fm["hdnFineAmount"].ToString();
                    dtrow["Discount"] = "";
                    dtrow["EnterBy"] = Session["User"].ToString();
                    dtrow["Status"] = "Pending";
                    dtColmn.Rows.Add(dtrow);
                    ViewData.Model = dtColmn.AsEnumerable();
                    return View("PaymentCompound");
                    #endregion
                }
                else
                {
                    TempData["CompoundIteamStatus"] = "Not inserted";
                }
            }
            return RedirectToAction("GetCompoundIteams", "FPMRelease");
        }
        [HttpPost]
        public void Pay()
        {
            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
           
            //EM33172142@5488
            Payment pay = new Payment();
            string encrypt = pay.RequestString("EM33172142@5488", Session["RequestId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "FPMRelease/Payment", Session["User"].ToString(), "", "");
            Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
        }
        public ActionResult Payment()
        {
            if (Session["RequestId"] != null)
            {                
                Payment pay = new Payment();
                //ForestReleases obj = new ForestReleases();
                FMDSS.Models.OnlineBooking.TicketBooking cs = new FMDSS.Models.OnlineBooking.TicketBooking();
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
                    //dt.Columns.Add("TRANSACTION BANK BID");                    
                }
                #endregion
                string response = Request.QueryString["trnParams"].ToString();              
                #region Response decryption
                string ResponseResult = pay.ProcesTranscationresponce(response);
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
                        string[] status1 = item.Split('=');
                        rowstatus1 = status1[1].ToString();
                    }
                    if (item.Equals(checkSucess))
                    {
                        string[] status1 = item.Split('=');
                        rowstatus1 = status1[1].ToString();
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
                    cs.TransactionId = finalreqid;
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
                        cs.Trn_Status_Code = 0;
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
                    cs.TransactionId = finalreqid;
                    string rawtransamount = reqamt[1].ToString();
                    int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                    string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                    string rawusername = username[1].ToString();
                    int usernamelen = Convert.ToInt32(rawusername.Length);
                    string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);
                    string rawbank = bank[1].ToString();
                    int banklen = Convert.ToInt32(rawbank.Length);
                    string finalbank = rawbank.ToString().Substring(1, banklen - 2);
                    //string rawbankbid = bankbidno[1].ToString();
                    //int bankidlen = Convert.ToInt32(rawbankbid.Length);
                    //string finalbankid = rawbankbid.ToString().Substring(1, bankidlen - 2);
                    dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                    dtrow["REQUEST ID"] = finalreqid;
                    dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
                    dtrow["TRANSACTION TIME"] = transtime[1] + Responsearr[2];
                    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    dtrow["USER NAME"] = finalUserName;
                    dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                    //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                    if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                    {
                        cs.Trn_Status_Code = 1;
                    }
                    dt.Rows.Add(dtrow);
                }
                #endregion
                cs.UpdateTransactionStatus("4");
                ViewData.Model = dt.AsEnumerable();

            }
            return View("TransactionStatus");
        }
        /// <summary>
        /// use to get details of compounding
        /// </summary>
        /// <returns></returns>
        public ActionResult ReleaseItems()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());

                    List<SelectListItem> RangeName = new List<SelectListItem>();
                    List<SelectListItem> OffenseCode = new List<SelectListItem>();
                    DataTable dtRange = new Common().Select_Range(UserID);
                    foreach (DataRow dr in dtRange.Rows)
                    {
                        RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.ddlRange = RangeName;
                    ViewBag.ddlOffenseCode = OffenseCode;
                    return View();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetOffenseCode(string RangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataSet ds = new ForestReleases().Get_OffenseCode(RangeCode, "1");

                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["OffenseCode"].ToString(), Value = @dr["OffenseCode"].ToString() });
                    }

                    ViewBag.ddlOffenseCode = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }
        /// <summary>
        /// Save release data
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        public ActionResult SaveReleaseData(FormCollection fm, HttpPostedFileBase fileappcompound,HttpPostedFileBase filereceipt)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string status = "";
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = "~/ForestProtectionDocument/";
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    ForestReleases obj = new ForestReleases();
                    if (fm["CompAmount"].ToString() == "")
                    {

                        obj.CompAmount = "0";



                    }
                    else
                    {
                        obj.CompAmount = fm["CompAmount"].ToString();
                    }
                    if (fm["ddlOffenseCode"].ToString() != null)
                    {

                        obj.OffenseCode = fm["ddlOffenseCode"].ToString();
                    }

                    if (fileappcompound != null && fileappcompound.ContentLength > 0)
                    {                       
                        FileName = Path.GetFileName(fileappcompound.FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        obj.AppOfcompounding = path;
                        fileappcompound.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        obj.AppOfcompounding = "";
                    }

                    if (filereceipt != null && filereceipt.ContentLength > 0)
                    {
                        FileName = Path.GetFileName(filereceipt.FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        obj.ReceiptOfAmount = path;
                        filereceipt.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        obj.ReceiptOfAmount = "";
                    }

                    obj.EmitraTransactionID = "0";
                    obj.trn_Status_Code = "1";
                    obj.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    obj.RequestId = "0";
                    status = obj.SubmitCompoundIteamRequest(obj);
                    if (status != "")
                    {
                        TempData["SizedIteamStatus"] = "Request submitted Successfully";
                        //return RedirectToAction("SurveyBudgetEstimation", "SurveyBudgetEstimation");
                    }
                    else
                    {
                        TempData["SizedIteamStatus"] = "Not inserted";
                    }
                    return RedirectToAction("ReleaseItems", "FPMRelease");
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        public JsonResult SaveSeizedItemMapping(string rowID, string TableName, string Status)
        {

            List<ForestReleases> _objData = new List<ForestReleases>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["FPMSeizedItem"] != null)
                {
                    List<ForestReleases> list = (List<ForestReleases>)Session["FPMSeizedItem"];

                    if (list != null)
                    {
                        ForestReleases obj = new ForestReleases { ID = Convert.ToInt64(rowID), TableName = TableName, Status = Status };
                        list.Add(obj);
                        Session["FPMSeizedItem"] = list;
                    }

                }
                else
                {

                    ForestReleases obj = new ForestReleases { ID = Convert.ToInt64(rowID), TableName = TableName, Status = Status };
                    _objData.Add(obj);
                    Session["FPMSeizedItem"] = _objData;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now,Convert.ToInt64(Session["UserID"]) );
            }
            return null;
        }
        /// <summary>
        /// Delete the Mapping betweem Subactivity and Activity if use change prefences. 
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public JsonResult DeleteMapping(string rowID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string data = Common.Status.Success.ToString();
            try
            {
                List<ForestReleases> list = (List<ForestReleases>)Session["FPMSeizedItem"];
                Int64 Rowid = 0;

                for (int i = 0; i < list.Count; i++)
                {

                    if (list[i].ID == Convert.ToInt64(rowID))
                    {
                        Rowid = Convert.ToInt64(rowID);
                        list.RemoveAll(item => item.ID == Rowid);

                        

                    }
                }

                Session["FPMSeizedItem"] = list;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// use to get details of release item
        /// </summary>
        /// <returns></returns>
      
        public ActionResult ReleaseSeizedItem()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());

                    List<SelectListItem> RangeName = new List<SelectListItem>();
                    List<SelectListItem> OffenseCode = new List<SelectListItem>();
                    DataTable dtRange = new Common().Select_Range(UserID);
                    foreach (DataRow dr in dtRange.Rows)
                    {
                        RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.ddlRange = RangeName;
                  ViewBag.ddlOffenseCode = OffenseCode;
                   
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        /// <summary>
        /// Function to bind all offense code to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetOffenseCodeForSeized(string RangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataSet ds = new ForestReleases().Get_OffenseCode(RangeCode, "2");

                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["OffenseCode"].ToString(), Value = @dr["OffenseCode"].ToString() });
                    }

                    ViewBag.ddlOffenseCodes = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

    }
}
