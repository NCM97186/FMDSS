//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : FixedPermissionController
//  Description  : File contains calling functions from UI
//  Date Created : 26-Nov-2015
//  History      : Add the Dist mapping data,Implementing GIS integartion 
//  Version      : 1.0
//  Author       : Gaurav Pandey
//  Modified By  : Gaurav Pandey
//  Modified On  : 05-Feb-2016
//  Reviewed By  : Amar Swain 
//  Reviewed On  : 06-Feb-2016
//********************************************************************************************************

using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using FMDSS.Models.SWCSModel;
using FMDSS.Models.BookOnlineTicket;
using Newtonsoft.Json;
using RestSharp;
namespace FMDSS.Controllers.CitizenService.FixedPermission
{
    //[MyAuthorization]
    public class FixedPermissionController : BaseController
    {
        //
        // GET: /FixedPermission/
        #region "Property Intialization"
        FixedLandUsage _objmodel = new FixedLandUsage();
        SMS_EMail_Services _objMailSMS = new SMS_EMail_Services();
        List<SelectListItem> Division = new List<SelectListItem>();
        List<SelectListItem> District = new List<SelectListItem>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        int ModuleID = 1;
        Int64 UserID = 0;
        double discount = 0;
        double Amount = 0;
        double Tax = 0;
        double FinalAmount = 0;
        double DiscountedAmmount = 0;
        double TaxableAmount = 0;
        private int PDFdocumentCount = 0;
        Location _obj = new Location();
        DataSet dsName = new DataSet();
        #endregion


        // GET: /FixedPermission/
        /// <summary>
        /// For Load the UI Default values first time
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="messagetype"></param>
        /// <returns></returns>
        public ActionResult FixedPermission(string aid, string messagetype, string NOCType = "", string NOCName = "", int NOCTypeId = 0)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {

                if (Session["UserID"] != null)
                {
                    if (messagetype == null)
                        aid = Encryption.decrypt(aid);

                    if (aid == null)
                    {
                        return View("error");
                    }
                    else
                    {
                        UserID = Convert.ToInt64(Session["UserID"].ToString());
                        if (Session["SSOid"] != null) { _objmodel.SSOID = Session["SSOid"].ToString(); }
                        if (aid == null || aid == "")
                        {
                            //string aid1 = Encryption.encrypt("1");
                            // aid = (aid1;

                        }
                        List<clsPermission> fixedlandlist = new List<clsPermission>();
                        ViewData["fixedlandlist"] = fixedlandlist;

                        Session["NOCPurposeId"] = NOCTypeId;
                        Session["NOCPurpose"] = NOCType;
                        Session["NOCType"] = NOCName;
                        Session.Remove("EmitraDivCode");
                        Session["PermissionTypeID"] = "";
                        Session["PermissionTypeID"] = aid;
                        _objmodel = BindPageData(aid);
                        if (messagetype == "1")
                        {
                            ViewData["Message"] = "Error occur while saving records!";
                        }

                        //changes done by Vandana Gupta for the points received on 12-aug-2016 
                        BindMasterData _ObjMaster = new BindMasterData();
                        List<SelectListItem> NOCPurpose = new List<SelectListItem>();
                        DataTable dtNOCPurpose = new DataTable();
                        dtNOCPurpose = _ObjMaster.GetFixedLandNOCPurpose();
                        foreach (System.Data.DataRow dr in dtNOCPurpose.Rows)
                        {
                            NOCPurpose.Add(new SelectListItem { Text = @dr["NOCTypeName"].ToString(), Value = @dr["NOCTypeId"].ToString() });
                        }
                        ViewBag.NOCPurpose = NOCPurpose;

                        return View(_objmodel);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        // Post: /GetDistrictName/
        /// <summary>
        /// For Bind the District 
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>

        [HttpPost]
       
        public JsonResult GetDistrictName(string Division)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _objmodel.BindDistrict(Division);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }

                    ViewBag.ddlDistrict1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        // Post: /GetBlockName/
        /// <summary>
        /// For Bind the Block
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>

        [HttpPost]
        
        public JsonResult GetBlockName(string District)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _obj.BindBlockName(District);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                    }

                    ViewBag.ddlBlockName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        ///  For Bind the GPanchyat Name
        /// </summary>
        /// <param name="district"></param>
        /// <param name="blockName"></param>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult GetGramPName(string District, string BlockName)
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = _obj.BindGramPanchayatName(District, BlockName);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                    ViewBag.ddlGPName1 = new SelectList(items, "Value", "Text");

                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// For Bind the Village Name
        /// 
        /// </summary>
        /// <param name="district"></param>
        /// <param name="blockName"></param>
        /// <param name="gpName"></param>
        /// <returns></returns>
        [HttpPost]
         
        public JsonResult GetVillageName(string District, string BlockName, string GPName)
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = _obj.BindVillageName(District, BlockName, GPName);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        /// For save the all records in database
        /// </summary>
        /// <param name="fCollection"></param>
        /// <param name="model"></param>
        /// <param name="command"></param>
        /// <returns></returns>


        [HttpPost]
       
        public ActionResult FixedLandPermissions(FormCollection FCollection, FixedLandUsage Model, string command, HttpPostedFileBase uploadFile)
        {
            var path = "";
            string FileName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());

                    if (command == "")
                    {


                        _objmodel.UserID = Convert.ToInt32(Session["UserId"]);
                        _objmodel.PerposedArea = Convert.ToDecimal(FCollection["PerposedArea"]);
                        if (FCollection["hdMenuId"].ToString() == "")
                        {
                            _objmodel.PermissionId = 0;
                        }
                        else
                        {
                            _objmodel.PermissionId = Convert.ToInt32(FCollection["hdMenuId"]);
                        }
                        if (FCollection["Applicant_type"].ToString() == "")
                        {
                            _objmodel.ApplicantType = 0;
                        }
                        else
                        {
                            _objmodel.ApplicantType = Convert.ToInt32(FCollection["Applicant_type"]);
                        }


                        if (Model.Area_Size == null || Model.Area_Size.ToString() == "")
                        {
                            _objmodel.Area_Size = "";
                        }
                        else
                        {
                            _objmodel.Area_Size = Model.Area_Size.ToString();
                        }
                        if (Model.GPSLat == null || Model.GPSLat.ToString() == "")
                        {
                            _objmodel.GPSLat = "";
                        }
                        else
                        {
                            _objmodel.GPSLat = Model.GPSLat.ToString();
                        }
                        if (Model.GPSLong == null || Model.GPSLong.ToString() == "")
                        {
                            _objmodel.GPSLat = "";
                        }
                        else
                        {
                            _objmodel.GPSLong = Model.GPSLong.ToString();
                        }
                        if (FCollection["Industrial_Type"].ToString() == "")
                        {
                            _objmodel.Industrial_Type = "";
                        }
                        else
                        {
                            _objmodel.Industrial_Type = FCollection["Industrial_Type"].ToString();
                        }
                        if (FCollection["Revenue_Map_Signed"] == "")
                        {
                            _objmodel.Revenue_Map_Signed = "";
                        }
                        else
                        {
                            _objmodel.Revenue_Map_Signed = FCollection["Revenue_Map_Signed"].ToString();
                        }

                        _objmodel.IsGTSheetAvailable = "1";
                        _objmodel.Duration_From = DateTime.Now.ToShortDateString();
                        _objmodel.Duration_To = DateTime.Now.ToShortDateString();

                        //if (Model.Duration_From == null || Model.Duration_From.ToString() == "")
                        //{
                        //    _objmodel.Duration_From = "";
                        //}
                        //else
                        //{
                        //    _objmodel.Duration_From = Model.Duration_From.ToString();
                        //}
                        //if (Model.Duration_To == null || Model.Duration_To.ToString() == "")
                        //{
                        //    _objmodel.Duration_To = Model.Duration_From.ToString();
                        //}
                        //else
                        //{
                        //    _objmodel.Duration_To = Model.Duration_To.ToString();
                        //}

                        _objmodel.KML_Path = FCollection["hdKMLFile"] == "" ? "" : FCollection["hdKMLFile"];
                        _objmodel.GISID = FCollection["hdGISID"] == "" ? "" : FCollection["hdGISID"];


                        if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[0].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Revenue_Record_Path = path;
                            Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));
                            PDFdocumentCount = PDFdocumentCount + 1;

                        }
                        else
                        { _objmodel.Revenue_Record_Path = ""; }

                        if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[1].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Revenue_Map_Path = path;
                            Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                            PDFdocumentCount = PDFdocumentCount + 1;

                        }
                        else
                        { _objmodel.Revenue_Map_Path = ""; }

                        if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                        {


                            // FileName = Path.GetFileName(Request.Files[3].FileName);
                            FileName = Path.GetFileName(Request.Files[2].FileName);  ///// Amit Change (22-02-2016)
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Additional_Document = path;
                            Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));
                            PDFdocumentCount = PDFdocumentCount + 1;

                        }
                        else
                        { _objmodel.Additional_Document = ""; }

                        if (Model.Citizen_Comment == null || Model.Citizen_Comment.ToString() == "")
                        {
                            _objmodel.Citizen_Comment = "";
                        }
                        else
                        {
                            _objmodel.Citizen_Comment = Model.Citizen_Comment.ToString();
                        }

                        #region MiningPermission


                        if (FCollection["Nearest_WaterSource"].ToString() == "")
                        {
                            _objmodel.Nearest_WaterSource = "";
                        }
                        else
                        {
                            _objmodel.Nearest_WaterSource = FCollection["Nearest_WaterSource"].ToString();
                        }
                        if (Model.WaterSource_Distance == null || Model.WaterSource_Distance.ToString() == "")
                        {
                            _objmodel.WaterSource_Distance = "";
                        }
                        else
                        {
                            _objmodel.WaterSource_Distance = Model.WaterSource_Distance.ToString();
                        }
                        if (Model.Forest_Distance == null || Model.Forest_Distance.ToString() == "")
                        {
                            _objmodel.Forest_Distance = "";
                        }
                        else
                        {
                            _objmodel.Forest_Distance = Model.Forest_Distance.ToString();
                        }
                        if (Model.Wildlife_Distance == null || Model.Wildlife_Distance.ToString() == "")
                        {
                            _objmodel.Wildlife_Distance = "";
                        }
                        else
                        {
                            _objmodel.Wildlife_Distance = Model.Wildlife_Distance.ToString();
                        }
                        if (Model.Tree_species == null || Model.Tree_species.ToString() == "")
                        {
                            _objmodel.Tree_species = "";
                        }
                        else
                        {
                            _objmodel.Tree_species = Model.Tree_species.ToString();
                        }

                        if (Model.AravalliHills == null || Model.AravalliHills.ToString() == "")
                        {
                            _objmodel.AravalliHills = "";
                        }
                        else
                        {
                            _objmodel.AravalliHills = Model.AravalliHills.ToString();
                        }
                        if (Model.ForestLand == null || Model.ForestLand.ToString() == "")
                        {
                            _objmodel.ForestLand = "";
                        }
                        else
                        {
                            _objmodel.ForestLand = Model.ForestLand.ToString();
                        }
                        if (Model.Plantation_Area == null || Model.Plantation_Area.ToString() == "")
                        {
                            _objmodel.Plantation_Area = "";
                        }
                        else
                        {
                            _objmodel.Plantation_Area = Model.Plantation_Area.ToString();
                        }

                        #endregion

                        #region "Sawmills"

                        if (FCollection["Sawmill_Type"].ToString() == "")
                        {
                            _objmodel.Sawmill_Type = "";
                        }
                        else
                        {
                            _objmodel.Sawmill_Type = FCollection["Sawmill_Type"].ToString();
                        }

                        if (Model.Sawmill_Size == null || Model.Sawmill_Size.ToString() == "")
                        {
                            _objmodel.Sawmill_Size = "";
                        }
                        else
                        {
                            _objmodel.Sawmill_Size = Model.Sawmill_Size.ToString();
                        }

                        #endregion
                        if (Model.OtherPermission == null || Model.OtherPermission.ToString() == "")
                        {
                            _objmodel.OtherPermission = "";
                        }
                        else
                        {
                            _objmodel.OtherPermission = Model.OtherPermission.ToString();
                        }
                        if (Model.Amount == null || Model.Amount.ToString() == "")
                        {
                            _objmodel.Amount = 0;
                        }
                        else
                        {
                            _objmodel.Amount = Convert.ToDecimal(Model.Amount);
                        }
                        if (Model.Discount == null || Model.Discount.ToString() == "")
                        {
                            _objmodel.Discount = 0;
                        }
                        else
                        {
                            _objmodel.Discount = Convert.ToDecimal(Model.Discount);
                        }

                        if (Model.FOREST_DIVCODE == null || Model.FOREST_DIVCODE.ToString() == "")
                        {
                            _objmodel.FOREST_DIVCODE = "0";
                        }
                        else
                        {
                            _objmodel.FOREST_DIVCODE = Model.FOREST_DIVCODE;
                        }

                        if (Model != null && _objmodel.Amount > 0)
                        {
                            _objmodel.Final_Amount = Model.Final_Amount + Convert.ToDecimal(0); ;
                            Session["Ftotalprice"] = _objmodel.Final_Amount.ToString();
                        }
                        _objmodel.TransactionId = DateTime.Now.Ticks.ToString();
                        Session["FRequestId"] = _objmodel.TransactionId.ToString();
                        _objmodel.PayStatus = "Pending";
                        if (Session["KioskUserId"] != null)
                        {
                            _objmodel.kioskuserid = Session["KioskUserId"].ToString();
                        }
                        else
                        {
                            _objmodel.kioskuserid = "0";
                        }
                        string[] plantcount = FCollection["plantcount"].Split(char.Parse(","));
                        string[] plantId = FCollection["hdplant"].Split(char.Parse(","));

                        DataTable planttable = new DataTable();
                        planttable.Columns.Add("ID", typeof(string));
                        planttable.Columns.Add("Number_Trees", typeof(string));

                        for (var i = 0; i < plantcount.Length; i++)
                        {
                            DataRow dr = planttable.NewRow();
                            if (plantcount[i].ToString() != "0")
                            {
                                dr["ID"] = plantId[i];
                                dr["Number_Trees"] = plantcount[i];
                                planttable.Rows.Add(dr);
                            }
                        }
                        DataSet dsm = new DataSet();
                        if (Session["DistInfo"] != null)
                        {
                            dsm.ReadXml(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                        }
                        else
                        {
                            dsm.Clear();
                            DataTable objDt2 = new DataTable("Table");
                            objDt2.Columns.Add("Div_Cd", typeof(String));
                            objDt2.Columns.Add("Dist_Cd", typeof(String));
                            objDt2.Columns.Add("Blk_Cd", typeof(String));
                            objDt2.Columns.Add("Gp_Cd", typeof(String));
                            objDt2.Columns.Add("Vlg_Cd", typeof(String));
                            objDt2.Columns.Add("areaName", typeof(String));
                            objDt2.Columns.Add("khasraNo", typeof(String));
                            objDt2.Columns.Add("FOREST_DIVCODE", typeof(String));
                            objDt2.Columns.Add("Tehsil_Cd", typeof(String));
                            objDt2.AcceptChanges();
                            dsm.Tables.Add(objDt2);
                            objDt2.Clear();
                            objDt2.AcceptChanges();
                        }

                        //Added by Vandana Gupta on 26-Aug-2016
                        _objmodel.txtplantOthers = Model.txtplantOthers;
                        _objmodel.txtplantOthersNo = Model.txtplantOthersNo;

                        Int64 id = _objmodel.SubmitFixedLandUsage(_objmodel, dsm.Tables[0], planttable, Convert.ToInt16(Session["NOCPurposeId"]));
                        Session["NOCPurpose"] = null;
                        Session["NOCType"] = null;


                        ////DMS Code ---- 07-04-2016

                        //UploadResponce responce = new UploadResponce();
                        //DMSAttribute dMSAttribute = new DMSAttribute();


                        //dMSAttribute.ModuleId = "1";
                        //dMSAttribute.PermissionId = "1";
                        //dMSAttribute.RequestedID = _objmodel.TransactionId;
                        //dMSAttribute.ServiceTypeId = "1";
                        //dMSAttribute.SubPermissionId = _objmodel.PermissionId.ToString();
                        //string Filefor = "Revenue_Record_Path";
                        //responce = DMS_Service.DMSPushDocument(Server.MapPath(_objmodel.Revenue_Record_Path), _objmodel.Revenue_Record_Path, DMS_Service.DocumentTypeClass.ProofOfIdentity, dMSAttribute, "tbl_FixedPermissions", Filefor);
                        //Filefor = "Revenue_Map_Path";
                        //responce = DMS_Service.DMSPushDocument(Server.MapPath(_objmodel.Revenue_Map_Path), _objmodel.Revenue_Map_Path, DMS_Service.DocumentTypeClass.ProofOfIdentity, dMSAttribute, "tbl_FixedPermissions", Filefor);
                        //if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                        //{
                        //    Filefor = "OtherDoc";
                        //    responce = DMS_Service.DMSPushDocument(Server.MapPath(_objmodel.Additional_Document), _objmodel.Additional_Document, DMS_Service.DocumentTypeClass.ProofOfIdentity, dMSAttribute, "tbl_FixedPermissions", Filefor);

                        //}

                        if (id > 0)
                        {
                            if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml")) == true)
                            {
                                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                                Session["DistInfo"] = null;
                            }
                            if (_objmodel.Amount > 0)
                            {
                                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]))//&& Session["KioskCtznName"] != null
                                {
                                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                                    _obj.ModuleId = 1;
                                    _obj.ServiceTypeId = 1;
                                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                                    _obj.SubPermissionId = _objmodel.PermissionId;
                                    _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);

                                    if (dtKiosk.Rows.Count > 0)
                                    {
                                        _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                        return PartialView("KioskPaymentNOC", _obj);
                                    }
                                }
                                //  else if (Session["IsDepartmentalKioskUser"] != null && Convert.ToBoolean(Session["IsDepartmentalKioskUser"]) && Session["KioskCtznName"] != null)
                                else if (Session["IsDepartmentalKioskUser"] != null && Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                                {
                                    // Added by Arvind Kumar Sharma
                                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();
                                    _obj.ModuleId = 1;
                                    _obj.ServiceTypeId = 1;
                                    _obj.PermissionId = 1;
                                    _obj.SubPermissionId = Convert.ToInt32(_objmodel.PermissionId);
                                    _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                    _obj.PaidBy = _objmodel.kioskuserid;
                                    _obj.PaidForCitizen = _objmodel.UserID;
                                    _obj.PaidAmount = _objmodel.Final_Amount;
                                    _obj.PaidOn = Convert.ToString(DateTime.Now);
                                    return PartialView("PaymentByDepartmentalKioskUsers", _obj);
                                }
                                else
                                {
                                    return View("FixedPermissionPayment", _objmodel);
                                }
                            }
                            else
                            {
                                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                                {
                                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                                    _obj.ModuleId = 1;
                                    _obj.ServiceTypeId = 1;
                                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                                    _obj.SubPermissionId = _objmodel.PermissionId;
                                    _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);

                                    if (dtKiosk.Rows.Count > 0)
                                    {
                                        _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                        return PartialView("PayKioskNOC", _obj);
                                    }
                                }
                                //  else if (Session["IsDepartmentalKioskUser"] != null && Convert.ToBoolean(Session["IsDepartmentalKioskUser"]) && Session["KioskCtznName"] != null)
                                else if (Session["IsDepartmentalKioskUser"] != null && Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                                {
                                    // Added by Arvind Kumar Sharma
                                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();
                                    _obj.ModuleId = 1;
                                    _obj.ServiceTypeId = 1;
                                    _obj.PermissionId = 1;
                                    _obj.SubPermissionId = Convert.ToInt32(_objmodel.PermissionId);
                                    _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                    _obj.PaidBy = _objmodel.kioskuserid;
                                    _obj.PaidForCitizen = _objmodel.UserID;
                                    _obj.PaidAmount = _objmodel.Final_Amount;
                                    _obj.PaidOn = Convert.ToString(DateTime.Now);
                                    return PartialView("PaymentByDepartmentalKioskUsers", _obj);
                                }
                                else
                                {

                                    DataSet ds = new DataSet();
                                    ds = _objmodel.UpdatePaymentStatus(0, 1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));

                                    #region "User Send Email"
                                    string UserMailBody = Common.GenerateBody(_objmodel.Final_Amount, ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                                    string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                                    //Thread email = new Thread(delegate()
                                    //{


                                    _objMailSMS.sendEMail("Payment Details for " + Convert.ToString(Session["PermissionType"]) + " Permission", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

                                    SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);

                                    // Calling Esanchar Service

                                    //  string eSanchar = Convert.ToString(Session["PermissionType"]) + "आवेदन करने के लिए धन्यवाद. आपका अनुरोध आईडी है " + _objmodel.TransactionId + "";
                                    //  eSanchar += "अपने आवेदन की स्थिति को देखने के लिए  कृपया  www डॉट fmdss डॉट forest डॉट rajasthan डॉट gov डॉट in पर लॉग इन करें. धन्यवाद";
                                    ////  bool flag = FMDSS.App_Start.cls_Esanchar.SendeSancharMessage(Convert.ToString(Session["PermissionType"]), Convert.ToString(ds.Tables[0].Rows[0]["Mobile"]), eSanchar);

                                    #endregion
                                    return RedirectToAction("dashboard", "dashboard", new { messagetype = "2" });
                                }


                            }


                        }
                        else
                        {
                            return View("Error");
                        }

                    }
                    else
                    {

                        if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[1].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Revenue_Record_Path = path;
                            Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.Revenue_Record_Path = FCollection["hdRevRecPath"].ToString(); }

                        if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[2].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Revenue_Map_Path = path;
                            Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.Revenue_Map_Path = FCollection["hdRevMapPath"].ToString(); }


                        _objmodel.UserID = Convert.ToInt32(Session["UserId"]);
                        _objmodel.RequestedID = command;


                        _objmodel.UpdateData(_objmodel, "reassigned");

                        return RedirectToAction("dashboard", "dashboard", new { messagetype = "3" });

                    }
                }
            }
            catch (Exception ex)
            {
                Session["NOCPurpose"] = null;
                Session["NOCType"] = null;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        #region "Pay"

        [HttpPost]
        public void Pay()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    // string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();


                    ////EM33172142@5488
                    //Payment pay = new Payment();
                    //string encrypt = pay.RequestString("EM33172142@5488", Session["FRequestId"].ToString(), Session["Ftotalprice"].ToString(), ReturnUrl + "FixedPermission/ResponsePay", Session["User"].ToString(), "", "");
                    //Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
                    BookOnTicket OBJ = new BookOnTicket();
                    DataSet DS = new DataSet();
                    DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("CitizenNOCPermission", Convert.ToString(Session["FRequestId"]));

                    string REVENUEHEAD = Convert.ToString(DS.Tables[0].Rows[0]["REVENUEHEAD"]);

                    string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                    EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();


                    string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["FRequestId"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["ChecksumKey"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["EncryptionKey"]),
                        //ReturnUrl + "FixedPermission/ResponsePay", ReturnUrl + "FixedPermission/ResponsePay",
                         ReturnUrl + "FixedPermission/Payment", ReturnUrl + "FixedPermission/Payment",
                        Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]), Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]), REVENUEHEAD, Session["User"].ToString());


                    Response.Write(forms);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }
        #endregion

        public ActionResult Payment()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int fmdssStatus = 0;
            if (Session["FRequestId"] != null)
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

                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    //  DecryptedData = "{'MERCHANTCODE':'FOREST0117','REQTIMESTAMP':'20170810001655000','PRN':'636379209711021628','AMOUNT':'10044.00','PAIDAMOUNT':'10050.00','SERVICEID':'2239','TRANSACTIONID':'170055011924','RECEIPTNO':'17053223840','EMITRATIMESTAMP':'20170810001853257','STATUS':'SUCCESS','PAYMENTMODE':'Kotak Bank Payment Gateway(All Banks)','PAYMENTMODEBID':'CTX170809184817579982','PAYMENTMODETIMESTAMP':'20170810001715000','RESPONSECODE':'200','RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.','UDF1':'CKNDR HASHIM','UDF2':'udf2','CHECKSUM':'03263f854d49bcdbf966ce9ffe494d26'}";
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    int status1 = 0;
                    BookOnTicket cs = new BookOnTicket();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    cs.UpdateEmitraResponse(Session["FRequestId"].ToString(), "CitizenNOCPermission", DecryptedData);

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
                    if (ObjPGResponse.STATUS == "FAILED")
                    {
                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = "0";
                        cs.RequestId = ObjPGResponse.PRN;

                        if (Session["KioskUserId"] == "" || Session["KioskUserId"] == null)
                        {
                            cs.KioskUserId = "0";
                        }
                        else
                        {
                            cs.KioskUserId = Session["KioskUserId"].ToString();

                        }

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = "0";
                        dtrow["EMITRA AMOUNT"] = "0";
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name


                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                        {
                            _objmodel.Trn_Status_Code = 0;
                        }
                        dt.Rows.Add(dtrow);

                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
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
                            _objmodel.Trn_Status_Code = 1;
                        }
                        dt.Rows.Add(dtrow);

                    }



                    #endregion


                    ViewData.Model = dt.AsEnumerable();


                    _objmodel.UserName = Session["User"].ToString();

                    if (_objmodel.Trn_Status_Code == 1)
                    {
                        DataSet ds = new DataSet();

                        ds = _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), _objmodel.Trn_Status_Code, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()),Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT));

                        #region "User Send Email"
                        string UserMailBody = Common.GenerateBody(_objmodel.Final_Amount, ds.Tables[0].Rows[0]["Name"].ToString(), ObjPGResponse.PRN, Session["PermissionType"].ToString() + " Permission");
                        string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), ObjPGResponse.PRN, Session["PermissionType"].ToString() + " Permission");
                        _objMailSMS.sendEMail("Payment Details for " + Session["PermissionType"].ToString() + " Permission", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

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
                        _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), status1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));
                        return View("NOCFIlmORGTransactionStatus");

                    }

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                return View("TransactionStatus");
            }
            return View();
        }




        /// <summary>
        /// For Fetching the reponse from Payment gateway and save the status in Database
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult ResponsePay()
        {

            Payment pay = new Payment();
            int status1 = 0;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());

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
                    // string ResponseResult = "<RESPONSE RCPT_NO='0' TRN_TIME='' REQUEST_ID='636064207456763666' AMOUNT='488.0' OTHER_PARAM_1='MOHD ASIF' OTHER_PARAM_2='null' OTHER_PARAM_3='null' STATUS='FAILED'></RESPONSE>|9856EDAED93AE7FAACA16B490DBFFAFC0929C8EB5BFE14E14584A7D38DC483CA";
                    string str1, str2;
                    str1 = ResponseResult.Replace("<RESPONSE ", "");
                    str2 = str1.Replace("></RESPONSE>", " ");
                    //string[] Responsearr = str2.Split(' ');
                    string[] Responsearr = System.Text.RegularExpressions.Regex.Split(str2, "' ");
                    string checkFail = "STATUS='FAILED";
                    string checkSucess = "STATUS='SUCCESS";
                    string rowstatus1 = "";
                    foreach (var item in Responsearr)
                    {
                        if (item.Equals(checkFail))
                        {
                            string[] statusx = item.Split('=');
                            rowstatus1 = statusx[1].ToString();
                        }
                        if (item.Equals(checkSucess))
                        {
                            string[] status2 = item.Split('=');
                            rowstatus1 = status2[1].ToString();
                        }
                    }
                    int status1len = Convert.ToInt32(rowstatus1.ToString().Length);
                    string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 1);
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
                        string finalstatus = rowstatus.ToString().Substring(1, statuslen - 1);
                        string rowreqid = reqid[1].ToString();
                        int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                        string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 1);
                        string rawemitra = emitratransid[1].ToString();
                        int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                        string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 1);
                        _objmodel.TransactionId = finalemitraid;
                        string rawtransamount = reqamt[1].ToString();
                        int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                        string finalamount = rawtransamount.ToString().Substring(1, amountlen - 1);
                        string rawusername = username[1].ToString();
                        int usernamelen = Convert.ToInt32(rawusername.Length);
                        string finalUserName = rawusername.ToString().Substring(1, usernamelen - 1);

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                        dtrow["REQUEST ID"] = finalreqid;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";//transtime[1];
                        dtrow["TRANSACTION AMOUNT"] = finalamount;
                        dtrow["USER NAME"] = finalUserName;

                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                        {
                            _objmodel.Trn_Status_Code = 0;
                        }
                        dt.Rows.Add(dtrow);
                    }
                    else if (finalstatus1 == "SUCCESS")
                    {
                        string[] emitratransid = Responsearr[0].Split('=');
                        string[] transtime = Responsearr[1].Split('=');
                        string[] reqid = Responsearr[2].Split('=');
                        string[] reqamt = Responsearr[3].Split('=');
                        string[] username = Responsearr[4].Split('=');
                        string[] status = Responsearr[7].Split('=');
                        string[] bank = Responsearr[8].Split('=');
                        string[] bankbidno = Responsearr[10].Split('=');

                        DataRow dtrow = dt.NewRow();
                        //string rowstatus = status[1].ToString();
                        int statuslen = Convert.ToInt32(status[1].ToString().Length);
                        string finalstatus = status[1].ToString().Substring(1, statuslen - 1);
                        //string rowreqid = reqid[1].ToString();
                        int reqidlen = Convert.ToInt32(reqid[1].ToString().Length);
                        string finalreqid = reqid[1].ToString().Substring(1, reqidlen - 1);
                        //string rawemitra = emitratransid[1].ToString();
                        int emitralen = Convert.ToInt32(emitratransid[1].ToString().Length);
                        string finalemitraid = emitratransid[1].ToString().Substring(1, emitralen - 1);
                        _objmodel.TransactionId = finalemitraid;
                        //string rawtransamount = reqamt[1].ToString();
                        int amountlen = Convert.ToInt32(reqamt[1].ToString().Length);
                        string finalamount = reqamt[1].ToString().Substring(1, amountlen - 1);

                        int timelen = Convert.ToInt32(transtime[1].ToString().Length);
                        string datetime = transtime[1].ToString().Substring(1, timelen - 1);
                        //  string rawbank = bank[1].ToString();
                        int banklen = Convert.ToInt32(bank[1].ToString().Length);
                        string finalbank = bank[1].ToString().Substring(1, banklen - 1);

                        int userlen = Convert.ToInt32(username[1].ToString().Length);
                        string finalUsername = username[1].ToString().Substring(1, userlen - 1);

                        //string rawbankbid = bankbidno[1].ToString();
                        //int bankidlen = Convert.ToInt32(rawbankbid.Length);
                        //string finalbankid = rawbankbid.ToString().Substring(1, bankidlen - 2);
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                        dtrow["REQUEST ID"] = finalreqid;
                        dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
                        dtrow["TRANSACTION TIME"] = datetime;
                        dtrow["TRANSACTION AMOUNT"] = finalamount;
                        dtrow["USER NAME"] = finalUsername;
                        dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                        //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            _objmodel.Trn_Status_Code = 1;
                        }
                        dt.Rows.Add(dtrow);
                    }
                    #endregion


                    ViewData.Model = dt.AsEnumerable();


                    _objmodel.UserName = Session["User"].ToString();

                    if (_objmodel.Trn_Status_Code == 1)
                    {
                        DataSet ds = new DataSet();

                        ds = _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), _objmodel.Trn_Status_Code, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));

                        #region "User Send Email"
                        string UserMailBody = Common.GenerateBody(_objmodel.Final_Amount, ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                        string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                        _objMailSMS.sendEMail("Payment Details for " + Session["PermissionType"].ToString() + " Permission", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

                        SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        #endregion
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            #region "Is Reviwer Mail"
                            string ReviwerMailBody = Common.GenerateReviwerBody(ds.Tables[1].Rows[0]["Name"].ToString(), _objmodel.TransactionId, ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission");
                            _objMailSMS.sendEMail("Request for " + ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission", ReviwerMailBody, ds.Tables[1].Rows[0]["EmailId"].ToString(), string.Empty);
                            #endregion
                        }
                        return View("TransactionStatus");
                    }
                    else
                    {
                        _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), status1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));
                        return View("NOCFIlmORGTransactionStatus");

                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;


        }

        /// <summary>
        /// For Fetching the Values from DB based on Requested and bind the data on UI
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdateFixedPermission(string id)
        {
            IndustryType obj = new IndustryType();
            SawmillType obj1 = new SawmillType();
            ViewBag.Industrial_Type = obj.GetIndustryType();//new SelectList(Common.GetSawmillType(), "Value", "Text", _objds.Tables[0].Rows[0]["Sawmill_Type"].ToString());
            ViewBag.Sawmill_Type = obj1.GetSawmillType(); //new SelectList(Common.GetIndustrialType(), "Value", "Text", _objds.Tables[0].Rows[0]["Industrial_Type"].ToString());
            ViewData["disablecontrols"] = true;
            DataSet _objds = new DataSet();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<PlantFixedPermission> ListPlant = new List<PlantFixedPermission>();

            try
            {
                if (Session["UserID"] != null)
                {
                    id = Encryption.decrypt(id);
                    if (id == null)
                    {
                        return View("Error");
                    }
                    else
                    {
                        UserID = Convert.ToInt64(Session["UserID"].ToString());
                        _objds = _objmodel.GetFixedLandValues(id);

                        if (_objds.Tables[0] != null)
                        {

                            _objmodel.RequestedID = id;
                            _objmodel.PermissionId = Convert.ToInt32(_objds.Tables[0].Rows[0]["P_ID"].ToString());



                            DataSet dtf = _objmodel.GetFixedDistMap(id);
                            List<clsPermission> fixedlandlist = new List<clsPermission>();
                            //for (int i = 0; i < dtf.Tables.Count; i++)
                            //{
                            //    foreach (DataRow dr in dtf.Tables[0].Rows)
                            //        fixedlandlist.Add(
                            //            new clsPermission()

                            //            {
                            //                DIV_CODE = dr["DIV_NAME"].ToString(),
                            //                DIST_CODE = dr["DIST_NAME"].ToString(),
                            //                BLK_CODE = dr["BLK_NAME"].ToString(),
                            //                GP_CODE = dr["GP_NAME"].ToString(),
                            //                VILL_CODE = dr["VILL_NAME"].ToString(),
                            //                KhasraNo = GetkasraValue(dr["KhasraNo"].ToString()),
                            //                Area = dr["Area"].ToString()
                            //            });

                            //}
                            ViewData["fixedlandlist"] = fixedlandlist;
                            ViewData["PlantList"] = ListPlant;
                            ViewBag.ApplicantType = new SelectList(Common.GetApplicantType(), "Value", "Text", _objds.Tables[0].Rows[0]["ApplicantType"].ToString());
                            ViewBag.NearestWaterSource = new SelectList(Common.GetNearestWaterSource(), "Value", "Text", _objds.Tables[0].Rows[0]["Nearest_WaterSource"].ToString());



                            _objmodel.ConditionFileEditMode = true;
                            //_objmodel.Area = _objds.Tables[0].Rows[0]["Area"].ToString();
                            _objmodel.Area_Size = _objds.Tables[0].Rows[0]["Area_Size"].ToString();
                            _objmodel.GPSLat = _objds.Tables[0].Rows[0]["GPSLat"].ToString();
                            _objmodel.GPSLong = _objds.Tables[0].Rows[0]["GPSLong"].ToString();
                            _objmodel.Revenue_Map_Signed = _objds.Tables[0].Rows[0]["Revenue_Map_Signed"].ToString();
                            _objmodel.IsGTSheetAvailable = _objds.Tables[0].Rows[0]["IsGTSheetAvaliable"].ToString();

                            _objmodel.KML_Path = _objds.Tables[0].Rows[0]["KML_Path"].ToString();
                            _objmodel.Revenue_Map_Path = _objds.Tables[0].Rows[0]["Revenue_Map_Path"].ToString();
                            _objmodel.Revenue_Record_Path = _objds.Tables[0].Rows[0]["Revenue_Record_Path"].ToString();
                            DateTime datefrom = new DateTime();
                            datefrom = Convert.ToDateTime(_objds.Tables[0].Rows[0]["DurationFrom"].ToString());
                            DateTime dateto = new DateTime();
                            dateto = Convert.ToDateTime(_objds.Tables[0].Rows[0]["DurationTo"].ToString());
                            _objmodel.Duration_From = datefrom.ToString("dd/MM/yyyy");

                            _objmodel.Duration_To = dateto.ToString("dd/MM/yyyy");
                            _objmodel.WaterSource_Distance = _objds.Tables[0].Rows[0]["WaterSource_Distance"].ToString();
                            _objmodel.Forest_Distance = _objds.Tables[0].Rows[0]["Forest_Distance"].ToString();
                            _objmodel.Wildlife_Distance = _objds.Tables[0].Rows[0]["Wildlife_Distance"].ToString();
                            _objmodel.Tree_species = _objds.Tables[0].Rows[0]["Tree_species"].ToString();
                            _objmodel.AravalliHills = _objds.Tables[0].Rows[0]["AravalliHills"].ToString();
                            _objmodel.ForestLand = _objds.Tables[0].Rows[0]["ForestLand"].ToString();
                            _objmodel.Plantation_Area = _objds.Tables[0].Rows[0]["Plantation_Area"].ToString();
                            _objmodel.Sawmill_Size = _objds.Tables[0].Rows[0]["Sawmill_Size"].ToString();
                            _objmodel.OtherPermission = _objds.Tables[0].Rows[0]["OtherPermission"].ToString();
                            _objmodel.Amount = Convert.ToDecimal(_objds.Tables[0].Rows[0]["Amount"]);
                            _objmodel.Discount = Convert.ToDecimal(_objds.Tables[0].Rows[0]["Discount"]);
                            _objmodel.Tax = Convert.ToDecimal(_objds.Tables[0].Rows[0]["Tax"]);
                            _objmodel.Final_Amount = Convert.ToDecimal(_objds.Tables[0].Rows[0]["Final_Amount"]);
                            if (Convert.ToBoolean(_objds.Tables[0].Rows[0]["Revenue_Map_Signed"])) { _objmodel.ConditionRevenueMapSigned = "Yes"; } else { _objmodel.ConditionRevenueMapSigned = "No"; }
                            if (Convert.ToBoolean(_objds.Tables[0].Rows[0]["IsGTSheetAvaliable"])) { _objmodel.ConditionIsGTSheetAvailable = "Yes"; } else { _objmodel.ConditionIsGTSheetAvailable = "No"; }
                            _objmodel.Additional_Document = _objds.Tables[0].Rows[0]["Additional_Document"].ToString();
                            _objmodel.Citizen_Comment = _objds.Tables[0].Rows[0]["Citizen_Comment"].ToString();
                        }
                        return View("FixedPermission", _objmodel);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// To display the Saved documnets/picture etc..
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ActionResult ViewFile(string filePath)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    if (System.IO.File.Exists(Server.MapPath(filePath)))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            Arguments = Server.MapPath(filePath),
                            FileName = "explorer.exe"
                        };
                        Process.Start(startInfo);
                    }
                }

                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]
        /// <summary>
        /// Use for save mapping between District to Village
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>      

        public JsonResult SaveDistMapping(string DIV_CODE, string DIST_CODE, string BLK_CODE, string GP_CODE, string VILL_CODE, string areaName, string KhasraNo, string FOREST_DIVCODE, string tehsilCode)
        {
            DataSet ds = new DataSet();
            clsPermission objModel = new clsPermission();
            objModel.DIV_CODE = DIV_CODE;
            objModel.DIST_CODE = DIST_CODE;
            objModel.BLK_CODE = BLK_CODE;
            objModel.GP_CODE = GP_CODE;
            objModel.VILL_CODE = VILL_CODE;
            objModel.areaName = areaName;
            objModel.KhasraNo = KhasraNo;
            objModel.FOREST_DIVCODE = FOREST_DIVCODE;
            Session["EmitraDivCode"] = objModel.FOREST_DIVCODE;
            objModel.Tehsil_Cd = tehsilCode;
            if (objModel != null)
            {
                try
                {

                    #region comment
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = "FXD_" + DateTime.Now.Ticks.ToString();

                    if (Session["DistInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("DistRoot");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["DistInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                    }
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["Vlg_Cd"].ToString() == objModel.Vlg_Cd)
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }


                            }
                            ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                        }
                    }
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));

                    XmlElement Dist_Map = xmldoc.CreateElement("Dist_Map");
                    XmlElement Div_Cd = xmldoc.CreateElement("Div_Cd");
                    XmlElement Dist_Cd = xmldoc.CreateElement("Dist_Cd");
                    XmlElement Blk_Cd = xmldoc.CreateElement("Blk_Cd");
                    XmlElement Gp_Cd = xmldoc.CreateElement("Gp_Cd");
                    XmlElement Vlg_Cd = xmldoc.CreateElement("Vlg_Cd");
                    XmlElement AreaName = xmldoc.CreateElement("areaName");
                    XmlElement khasraNo = xmldoc.CreateElement("khasraNo");
                    XmlElement FOREST_DivCode = xmldoc.CreateElement("FOREST_DIVCODE");
                    XmlElement Tehsil_Cd = xmldoc.CreateElement("Tehsil_Cd");


                    Div_Cd.InnerText = objModel.DIV_CODE;
                    Dist_Cd.InnerText = objModel.DIST_CODE;
                    Blk_Cd.InnerText = objModel.BLK_CODE;
                    Gp_Cd.InnerText = objModel.GP_CODE;
                    Vlg_Cd.InnerText = objModel.VILL_CODE;
                    AreaName.InnerText = objModel.areaName;
                    khasraNo.InnerText = objModel.KhasraNo;
                    FOREST_DivCode.InnerText = objModel.FOREST_DIVCODE;
                    Tehsil_Cd.InnerText = objModel.Tehsil_Cd;

                    Dist_Map.AppendChild(Div_Cd);
                    Dist_Map.AppendChild(Dist_Cd);
                    Dist_Map.AppendChild(Blk_Cd);
                    Dist_Map.AppendChild(Gp_Cd);
                    Dist_Map.AppendChild(Vlg_Cd);
                    Dist_Map.AppendChild(AreaName);
                    Dist_Map.AppendChild(khasraNo);
                    Dist_Map.AppendChild(FOREST_DivCode);
                    Dist_Map.AppendChild(Tehsil_Cd);

                    xmldoc.DocumentElement.AppendChild(Dist_Map);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                    #endregion
                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }
            }

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// To Parse the Kahsra no xml into plain text
        /// </summary>
        /// <param name="KhasraNo"></param>
        /// <returns></returns>
        public string GetkasraValue(string KhasraNo)
        {
            var doc = new XmlDocument();
            doc.LoadXml(KhasraNo.ToString());


            XmlNodeList xnList = doc.SelectNodes("/KhasraRoot/KhasraValue");
            StringBuilder sb = new StringBuilder();
            List<String> list = new List<String>();
            for (int i = 0; i < xnList.Count; i++)
            {
                string values = xnList[i].InnerText;
                sb.Append(values + ",");
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Use to bind the data to page controls which come from GIS Service
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult getGISData(FormCollection form)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string aid = string.Empty;
            try
            {
                if (Convert.ToString(Session["PermissionTypeID"]) != "") { aid = Convert.ToString(Session["PermissionTypeID"]); }
                if (Convert.ToString(form["successFlag"]).ToLower() == "true")
                {
                    List<clsPermission> myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));


                    _objmodel = BindPageData(aid);
                    _objmodel.GISID = form["gisid"].ToString();
                    _objmodel.Area_Size = Convert.ToString(form["shapeArea"]);
                    #region "Muliple List"
                    List<clsPermission> fixedlandlist = new List<clsPermission>();
                    foreach (var dr in myDeserializedObj)
                        fixedlandlist.Add(
                            new clsPermission()

                            {
                                Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd,
                                Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd,
                                Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd,
                                Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd,
                                Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd,
                                Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM,
                                Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM,
                                Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM,
                                Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM,
                                Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM,
                                areaName = dr.areaName == "NA" ? "N/A" : dr.areaName,
                                FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE,
                                Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd,
                                Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM
                            });

                    #endregion

                    #region MiningPermission

                    _objmodel.Nearest_WaterSource = form["nearbywaterbody"].ToString() == "NA" ? "N/A" : form["nearbywaterbody"].ToString();
                    _objmodel.WaterSource_Distance = form["nearbywaterbodydistance"].ToString() == "NA" ? "N/A" : form["nearbywaterbodydistance"].ToString();
                    _objmodel.Forest_Distance = form["nearbyforestdistance"].ToString() == "NA" ? "N/A" : form["nearbyforestdistance"].ToString();
                    _objmodel.Wildlife_Distance = form["nearbywildlifedistance"].ToString() == "NA" ? "" : form["nearbywildlifedistance"].ToString();
                    _objmodel.AravalliHills = form["iswithinaravali"].ToString() == "NA" ? "0" : "1";
                    _objmodel.ForestLand = form["iswithinforest"].ToString() == "NA" ? "0" : "1";
                    _objmodel.Plantation_Area = form["iswithinplantation"].ToString() == "NA" ? "0" : "1";

                    #endregion

                    #region "KML and Lat-Long"
                    _objmodel.KML_Path = form["filePath"].ToString() == "NA" ? "" : form["filePath"].ToString();

                    if (form["locCentroid"].ToString() != "")
                    {
                        if (form["locCentroid"].ToString().Contains(","))
                        {

                            string[] locCentroid = form["locCentroid"].ToString().Split(',');
                            _objmodel.GPSLat = locCentroid[1] == "NA" ? "" : locCentroid[1];
                            _objmodel.GPSLong = locCentroid[0] == "NA" ? "" : locCentroid[0];

                        }
                    }
                    #endregion

                    _objmodel.ConditionGISMode = true;
                    ViewData["fixedlandlist"] = fixedlandlist;
                }
                else
                {
                    List<clsPermission> fixedlandlist = new List<clsPermission>(); ViewData["fixedlandlist"] = fixedlandlist;
                    _objmodel = BindPageData(aid);
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.InnerException + "_" + ex.StackTrace);
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("FixedPermission", _objmodel);
        }
        /// <summary>
        /// Bind the page load events
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public FixedLandUsage BindPageData(string aid)
        {
            List<PlantFixedPermission> ListPlant = new List<PlantFixedPermission>();
            if (!string.IsNullOrEmpty(aid))
            {
                _objmodel.PermissionType = _objmodel.GetPermissionTypes(Convert.ToInt32(aid)).Tables[0].Rows[0]["Name"].ToString();
                Session["PermissionType"] = "";

                Session["PermissionType"] = _objmodel.PermissionType;

                DataSet Payds = new DataSet();

                List<FixedLandUsage> FixedLandList = new List<FixedLandUsage>();
                DataTable dt = null;
                dt = _objmodel.Division();

                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Division.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                }
                SawmillType obj1 = new SawmillType();
                IndustryType obj = new IndustryType();
                ViewBag.ddlDivision1 = Division;
                ViewBag.ddlDistrict1 = District;
                ViewBag.ddlBlockName1 = BlockName;
                ViewBag.ddlGPName1 = GPName;
                ViewBag.ddlVillName1 = VillageName;
                ViewBag.ApplicantType = new SelectList(Common.GetApplicantType(), "Value", "Text");
                ViewBag.NearestWaterSource = new SelectList(Common.GetNearestWaterSource(), "Value", "Text");
                ViewBag.Sawmill_Type = new SelectList(obj1.GetSawmillType(), "Value", "Text");
                ViewBag.Industrial_Type = new SelectList(obj.GetIndustryType(), "Value", "Text");
                _objmodel.ConditionRevenueMapSigned = "Both";
                _objmodel.Tree_species = "0";
                Payds = _objmodel.GetFinalAmount(1, Convert.ToInt32(aid));

                if (Payds.Tables[0].Rows.Count > 0)
                {
                    Amount = Convert.ToDouble(Payds.Tables[0].Rows[0]["Amount"]);
                    discount = Convert.ToDouble(Payds.Tables[0].Rows[0]["Discount"]);
                    Tax = Convert.ToDouble(Payds.Tables[0].Rows[0]["Tax"]);
                    DiscountedAmmount = (Amount * discount) / 100.0;
                    TaxableAmount = (Amount * Tax) / 100.0;
                    FinalAmount = (Amount + TaxableAmount) - DiscountedAmmount;
                    _objmodel.Amount = Convert.ToDecimal(Amount);
                    _objmodel.Discount = Convert.ToDecimal(discount);
                    _objmodel.Tax = Convert.ToDecimal(Tax);
                    _objmodel.Final_Amount = Convert.ToDecimal(FinalAmount);
                }

                DataSet dsPlant = _objmodel.GetPlantList();

                for (int i = 0; i < dsPlant.Tables.Count; i++)
                {
                    foreach (DataRow dr in dsPlant.Tables[0].Rows)
                        ListPlant.Add(
                            new PlantFixedPermission()

                            {
                                PlantID = Convert.ToInt64(dr["ID"].ToString()),
                                PlantName = dr["Plant_Name"].ToString()
                            });

                }
                ViewData["PlantList"] = ListPlant;
                _objmodel.PermissionId = Convert.ToInt32(aid);
                return _objmodel;
            }
            else
            {
                return _objmodel;


            }

        }

        [HttpPost]
        
        public JsonResult GetNOCType(int NOCPuproseId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> NOCPurpose = new List<SelectListItem>();
            try
            {
                //changes done by Vandana Gupta for the points received on 12-aug-2016 
                BindMasterData _ObjMaster = new BindMasterData();
                DataTable dtNOCPurpose = new DataTable();
                dtNOCPurpose = _ObjMaster.GetFixedLandNOCNames(NOCPuproseId);
                foreach (System.Data.DataRow dr in dtNOCPurpose.Rows)
                {
                    NOCPurpose.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["P_ID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(NOCPurpose, "Value", "Text"));

        }

        #region Payment For Kisok User for NOC
        [HttpPost]
        public ActionResult PayKioskNOC(FormCollection frmCollection)
        {
            KioskUserDetail kud = new KioskUserDetail();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                var baseAddress = kud.GetEmitraServiceUrlDirectOrBypass("EmitraKioksBacktoBackService", "GETSERVICEURL");
               // var baseAddress = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA";

                eMitraObjForPayment _objKioskPayment = new eMitraObjForPayment();
                _objKioskPayment.MERCHANTCODE = frmCollection["MerchantCode"].ToString();
                _objKioskPayment.REQUESTID = frmCollection["RequestId"].ToString();
                _objKioskPayment.REQTIMESTAMP = DateTime.Now.Ticks.ToString();
                _objKioskPayment.SERVICEID = Convert.ToString(Session["EmitrServiceId"]);
               // _objKioskPayment.SERVICEID = "3509";
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
                if (_objKiosk.TRANSACTIONSTATUS == "SUCCESS")
                {

                    DataSet DT2 = _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objKiosk.TRANSACTIONID), 1, "payUpdate", _objKiosk.REQUESTID, Convert.ToInt64(Session["UserId"].ToString()), Convert.ToDecimal(_objKiosk.TRANSAMT));
                    if (DT2.Tables[0].Rows.Count > 0)
                    {
                        _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                        _objKiosk.COMMTYPE = "True";
                       // _objKiosk.CHECKSUM = Convert.ToString(DT2.Tables[0].Rows[0][0]);
                    }
                    else
                    {
                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                    }
                }
                else if (_objKiosk.TRANSACTIONSTATUS != "SUCCESS")
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
                        // DataTable DT = kud.UpdateEmitraKioskTransactionStatus(_objKiosk, "1", "1");
                        DataSet DT = _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objKiosk.TRANSACTIONID), 1, "payUpdate", _objKiosk.REQUESTID, Convert.ToInt64(Session["UserId"].ToString()), Convert.ToDecimal(_objKiosk.AMT));

                        if (DT.Tables[0].Rows.Count > 0)
                        {
                            _objKiosk.TRANSACTIONSTATUS = "SUCCESS";
                            _objKiosk.COMMTYPE = "True";
                           // _objKiosk.CHECKSUM = Convert.ToString(DT.Tables[0].Rows[0][0]);
                        }
                        else
                        {
                            _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                            #region Email and SMS
                            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                            string name = "[ " + _objKiosk.REQUESTID + " ] " + _objKiosk.MSG;

                            objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskEndError", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                            #endregion
                        }
                    }
                    else
                    {
                        DataSet DT = _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objKiosk.TRANSACTIONID), 0, "payUpdate", _objKiosk.REQUESTID, Convert.ToInt64(Session["UserId"].ToString()), Convert.ToDecimal(_objKiosk.AMT));
                        _objKiosk.TRANSACTIONSTATUS = "Failed at FMDSS";
                        #region Email and SMS
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        string name = "[ " + _objKiosk.REQUESTID + " ] " + _objKiosk.MSG;

                        objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskEndError", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                        #endregion
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

                return PartialView("KioskTransactionStatus", _objKiosk);
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
