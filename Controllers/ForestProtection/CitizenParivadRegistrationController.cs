//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : CitizenParivadRegistration
//  Description  : File contains registration for forest protection and related activity
//  Date Created : 17-09-2016
//  History      :
//  Version      : 1.0
//  Author       : Rajkumar
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.ForestProtection;
using FMDSS.App_Start;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.CitizenService.PermissionServices;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using FMDSS.Models.CitizenService.PermissionService.EducationService;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace FMDSS.Controllers.ForestProtection
{
    public class CitizenParivadRegistrationController : BaseController
    {
        int ModuleID = 4;       
        List<SelectListItem> Dist = new List<SelectListItem>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        List<SelectListItem> ForestOffense = new List<SelectListItem>();
        List<SelectListItem> lstCaste = new List<SelectListItem>();
        CitizenParivad _objModel = new CitizenParivad();
       
        /// <summary>
        /// Global UserId 
        /// </summary>
        private Int64 UserID
        {
            get
            {
                if (Session["UserID"] != null)
                    return Convert.ToInt64(Session["UserID"].ToString());
                else
                    return 0;
            }
        }

        /// <summary>
        /// Function for list of case details
        /// </summary>
        /// <returns></returns>
        public ActionResult CaseStatus() 
        {         
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CaseInvestigationStatus Cis= new CaseInvestigationStatus();
            List<CaseInvestigationStatus> lstCase = new List<CaseInvestigationStatus>();
            DataSet dsCase = new DataSet();
            try
            {
               dsCase= Cis.GetViewInvestigationStatus("1");
               if (dsCase.Tables[0].Rows.Count > 0) {
                   foreach (System.Data.DataRow dr in dsCase.Tables[0].Rows) {
                       lstCase.Add(new CaseInvestigationStatus
                       {
                           OffenseCode=dr["Offense_code"].ToString(),
                           OffensePlace = dr["Place_of_offense"].ToString(),
                           OffenseDate = dr["OffenseDate"].ToString(),
                           OffenseDescription = dr["Description_of_Offense"].ToString(),
                           ComplaintFound = dr["Complaint_Found"].ToString()
                       });                   
                   }               
               }
               ViewBag.Caselist = lstCase;
               TempData["CurrentDate"] = DateTime.Now.ToString("MM/dd/yyyy");
               return View();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// Submission of investigation details
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveInvestigationDetails(CaseInvestigationStatus Model)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CaseInvestigationStatus obj = new CaseInvestigationStatus();
            try
            {
                obj.InvestigationDate = Model.InvestigationDate;
                obj.DispatchNo = Model.DispatchNo;
                obj.OffenseCode = Model.OffenseCode;
                Int64 status = obj.SubmitInvestigationStatus("2");
                if (status > 1)
                {
                    TempData["Investigation"] = "Investigation details completed sucessfully";
                }
                else {
                    TempData["Investigation"] = "Investigation details not submitted";
                }
                return RedirectToAction("CaseStatus", "CitizenParivadRegistration");
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// Function to return citizen registration form with dropdown
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult CitizenParivad(FormCollection form)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        
            try
            {
                     Session.Remove("oKnownOffender");
                     Session.Remove("fpmOffenderData");
                     Session.Remove("AddVechile");
                     Session.Remove("EmitraDivCode");
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());

                    ViewBag.ddlOState = new SelectList(FPMParivadRegistrations.DDLState(), "Value", "Text");

                     DataTable dtoffense = new OffenseReg().Get_OffenseCategory();
                    foreach (System.Data.DataRow dr in dtoffense.Rows)
                    {
                        ForestOffense.Add(new SelectListItem { Text = @dr["FOCategory"].ToString(), Value = @dr["FOCatID"].ToString() });

                    }

                    DataTable dtCast = new BindMasterData().GetCastDetails();
                    foreach (System.Data.DataRow dr in dtCast.Rows)
                    {
                        lstCaste.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.ddlOCaste = new SelectList(lstCaste, "Value", "Text");

                    DataTable dt = new Location().District("District"); 
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        Dist.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }


                    ViewBag.ddlODistrict = new SelectList(Dist, "Value", "Text");


                    if (Convert.ToString(form["offenceData"]) != null)
                    {
                        string retData = Convert.ToString(form["retData"]);
                        if (retData != null)
                        {
                            string[] LatLong = retData.Split(',');
                            if (LatLong.Length == 2)
                            {
                                _objModel.Lattitude = Convert.ToDecimal(LatLong[0]);
                                _objModel.Longitude = Convert.ToDecimal(LatLong[1]);
                            }
                        }

                        string retDataGIs = Convert.ToString(form["offenceData"]);
                        Location _objLocation = new Location();                   
                        var oObj = JsonConvert.DeserializeObject<OffenderGISReturnDetails>(retDataGIs);                     
                        string OffenseCategoryGIS = oObj.OffenseCategoryID;
                        string DistrictGIS = oObj.DistrictID;
                        string BlocknameGIS = oObj.BlocknameID;
                        string GPNameGIS = oObj.GPNameID;
                        string VillageGIS = "";

                        VillageGIS = oObj.VillageID;

                        DataTable dtGIS;

                        dtGIS = _objLocation.BindBlockName(DistrictGIS);

                        foreach (System.Data.DataRow dr in dtGIS.Rows)
                        {
                            BlockName.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                        }

                        dtGIS = _objLocation.BindGramPanchayatName(DistrictGIS, BlocknameGIS);

                        foreach (System.Data.DataRow dr in dtGIS.Rows)
                        {
                            GPName.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                        }

                        dtGIS = _objLocation.BindVillageName(DistrictGIS, BlocknameGIS, GPNameGIS);

                        foreach (System.Data.DataRow dr in dtGIS.Rows)
                        {
                            VillageName.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                        }
                        ViewBag.OffenseCategory = new SelectList(ForestOffense, "Value", "Text", OffenseCategoryGIS);
                        ViewBag.ddlDistrict1 = new SelectList(Dist, "Value", "Text", DistrictGIS);
                        ViewBag.ddlBlockName1 = new SelectList(BlockName, "Value", "Text", BlocknameGIS);
                        ViewBag.ddlGPName1 = new SelectList(GPName, "Value", "Text", GPNameGIS);
                        ViewBag.ddlVillName1 = new SelectList(VillageName, "Value", "Text", VillageGIS);
                    }
                    else
                    {
                        ViewBag.OffenseCategory = new SelectList(ForestOffense, "Value", "Text");
                        ViewBag.ddlDistrict1 = new SelectList(Dist, "Value", "Text");
                        ViewBag.ddlBlockName1 = BlockName;
                        ViewBag.ddlGPName1 = GPName;
                        ViewBag.ddlVillName1 = VillageName;
                    }
                    CaseInvestigationStatus Cis = new CaseInvestigationStatus();
                    DataSet dsOffense = new DataSet();
                    List<CaseInvestigationStatus> lstcitizen = new List<CaseInvestigationStatus>();
                    dsOffense = Cis.GetViewInvestigationStatus("3");
                    if (dsOffense.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in dsOffense.Tables[0].Rows)
                        {
                            lstcitizen.Add(new CaseInvestigationStatus
                            {
                                OffenseCode = dr["Offense_code"].ToString(),
                                OffensePlace = dr["Place_of_offense"].ToString(),
                                OffenseDate = dr["OffenseDate"].ToString(),
                                OffenseDescription = dr["Description_of_Offense"].ToString(),
                                ComplaintFound = dr["Complaint_Found"].ToString()
                            });
                        }
                    }
                    ViewData["OffenseList"] = lstcitizen;
                    TempData["CurrentDate"] = DateTime.Now.ToString("MM/dd/yyyy");
                    getDropdown();
                    DDLList();

                    return View(_objModel);              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// Global fuction for bind of drop down 
        /// </summary>
        public void getDropdown()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();     
            try
            {
                #region Dropdown Bind
                OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
             
                DataTable dtVechileType = new DataTable();
                dtVechileType = OR.GetDropdown("1");
                List<SelectListItem> lstVechile = new List<SelectListItem>();
             
                DataTable dtfile = new DataTable();
               
                foreach (System.Data.DataRow dr in dtVechileType.Rows)
                {
                    lstVechile.Add(new SelectListItem { Text = @dr["VechileName"].ToString(), Value = @dr["VechileId"].ToString() });
                }
                ViewBag.VechileType = lstVechile;
                                          
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }

        }
        /// <summary>
        /// Function to save offender list in grid
        /// </summary>
        /// <param name="_objModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveOffenderList(KnownOffender _objModel)
        {
            string _data = string.Empty;
            KnownOffender obj = new KnownOffender();
            List<KnownOffender> _objData = new List<KnownOffender>();
            List<KnownOffender> list = new List<KnownOffender>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["oKnownOffender"] != null)
                {
                    list = (List<KnownOffender>)Session["oKnownOffender"];
                    foreach (var item in list)
                    {
                        if (item.OOffenderrowid == _objModel.OOffenderrowid)
                        {
                            _objData.Add(_objModel);
                        }
                        else
                        {
                            _objData.Add(item);
                        }
                    }
                    if (_objModel.OOffenderrowid == null)
                    {
                        _objModel.OOffenderrowid = Guid.NewGuid().ToString();
                        _objData.Add(_objModel);
                    }
                    Session["oKnownOffender"] = null;
                    Session["oKnownOffender"] = _objData;
                }
                else
                {
                    _objModel.OOffenderrowid = Guid.NewGuid().ToString();
                    _objData.Add(_objModel);
                    Session["oKnownOffender"] = _objData;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(_objModel, JsonRequestBehavior.AllowGet);
            
        }
        /// <summary>
        /// final submission of citizen registration form
        /// </summary>
        /// <param name="fcollection"></param>
        /// <param name="Model"></param>
        /// <param name="Command"></param>
        /// <param name="UploadEvidence"></param>
        /// <returns></returns>
        public ActionResult SaveOffenderDetails(FormCollection fcollection,CitizenParivad CP, FPMParivadRegistrations Model, string Command, HttpPostedFileBase UploadEvidence)
        {            
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string Document = string.Empty;
            var path = "";
            string FilePath = "~/ForestProtectionDocument/";
            try
            {

                if (Command == "submit")
                    {
                        _objModel.OffenderType = string.IsNullOrEmpty(fcollection["Offender"].ToString()) ? "" : fcollection["Offender"].ToString();                        
                        _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                        _objModel.OffenseCategory = string.IsNullOrEmpty(Model.OffenseCategory) ? "" : Model.OffenseCategory;
                        _objModel.DateOfOffense = string.IsNullOrEmpty(Model.DateOfOffense) ? "" : Model.DateOfOffense;
                        _objModel.TimeOfOffense = string.IsNullOrEmpty(Model.TimeOfOffense) ? "" : Model.TimeOfOffense;
                        _objModel.OffensePlace = string.IsNullOrEmpty(Model.OffensePlace) ? "" : Model.OffensePlace;
                       // _objModel.DistrictID = string.IsNullOrEmpty(fcollection["ddlDistrict"].ToString()) ? "" : fcollection["ddlDistrict"].ToString();

                        if (fcollection["hdnDistCode"] == null)
                        {
                            _objModel.DistrictID = "";
                        }
                        else
                        {
                            _objModel.DistrictID = fcollection["hdnDistCode"].ToString();
                            EducationTours edu = new EducationTours();
                            edu.Location = Convert.ToInt64(_objModel.DistrictID);
                            DataTable dt = edu.GetEmitraDivCode(edu.Location, "2");
                          
                            if (dt.Rows.Count > 0)
                            {
                                if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                                {
                                    Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                                }
                            }

                        }

                        if (fcollection["hdnBlockCode"] == null)
                        {
                            _objModel.BlockCode = "";
                        }
                        else
                        {
                            _objModel.BlockCode = CP.hdnBlockCode;
                        }
                        if (fcollection["hdnGPCode"] == null)
                        {
                            _objModel.GPCode = "";
                        }
                        else
                        {
                            //_objModel.GPCode = fcollection["ddlGPName"].ToString();
                            _objModel.GPCode = CP.hdnGPCode;
                        }
                        if (fcollection["hdnVillageCode"] == null)
                        {
                            _objModel.VillageCode = "";
                        }
                        else
                        {
                           // _objModel.VillageCode = fcollection["ddlVillName"].ToString();
                            _objModel.VillageCode = CP.hdnVillageCode;
                        }
                        if (fcollection["Longitude"] == null)
                        {
                            _objModel.Longitude = 0;
                        }
                        else
                        {
                            _objModel.Longitude = Convert.ToDecimal(fcollection["Longitude"]);
                        }
                        if (fcollection["Lattitude"] == null)
                        {
                            _objModel.Lattitude = 0;
                        }
                        else
                        {
                            _objModel.Lattitude = Convert.ToDecimal(fcollection["Lattitude"]);
                        }
                        _objModel.Description = string.IsNullOrEmpty(Model.Description) ? "" : Model.Description;

                        if (UploadEvidence != null && UploadEvidence.ContentLength > 0)
                        {
                             //New Code----
                        Random ran = new Random();
                        int firstNumber = ran.Next(200, 999);
                        int secondNumber = ran.Next(1, 9);
                        string text = firstNumber.ToString() + " + " + secondNumber.ToString() + " = ?";
                        Bitmap bitmap = new Bitmap(1, 1);
                        Font font = new Font("Arial", 25, FontStyle.Regular, GraphicsUnit.Pixel);
                        Graphics graphics = Graphics.FromImage(bitmap);
                        int width = (int)graphics.MeasureString(text, font).Width;
                        int height = (int)graphics.MeasureString(text, font).Height;
                        bitmap = new Bitmap(bitmap, new Size(width, height));
                        graphics = Graphics.FromImage(bitmap);
                        graphics.Clear(Color.Transparent);
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                        //graphics.DrawString(text, font, new SolidBrush(Color.FromArgb(255, 0, 0)), 0, 0);
                        graphics.DrawString(text, font, new SolidBrush(Color.FromArgb(255, 255, 0)), 0, 0);
                        graphics.Flush();
                        graphics.Dispose();
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(UploadEvidence.FileName);
                        path = Path.Combine(FilePath, fileName);
                        bitmap.Save(Server.MapPath(FilePath) + fileName, ImageFormat.Png);
                        _objModel.UploadEvidence = path;

                        //---Old Code
                        //Document = Path.GetFileName(UploadEvidence.FileName);
                        //String FileFullName = DateTime.Now.Ticks + "_" + Document;
                        //path = Path.Combine(FilePath, FileFullName);
                        //_objModel.UploadEvidence = path;
                        //UploadEvidence.SaveAs(Server.MapPath(FilePath + FileFullName));
                        }
                        else
                        {
                            _objModel.UploadEvidence = "";
                        }  
                        _objModel.ONumberOfOffender = Model.ONumberOfOffender;                       
                        _objModel.OffenderDescription = string.IsNullOrEmpty(Model.OffenderDescription) ? "" : Model.OffenderDescription;                      
                        
                        if (Session["KioskUserId"] != null)
                        {
                            _objModel.kioskuserid = Session["KioskUserId"].ToString();
                        }
                        else
                        {
                            _objModel.kioskuserid = "0";
                        }
                        if (Session["oKnownOffender"] != null)
                        {
                            List<KnownOffender> list = (List<KnownOffender>)Session["oKnownOffender"];
                            if (list != null)
                            {
                                DataTable dtOffender = ExtMethodCommon.AsDataTable(list);
                                dtOffender.Columns.Remove("OOffenderrowid");
                                dtOffender.Columns.Remove("OffenderStatement");
                                dtOffender.Columns.Remove("OffenderStatementDoc");
                                dtOffender.AcceptChanges();
                                Int64 id = _objModel.SubmitDetails(_objModel, dtOffender);
                                if (id > 0)
                                {
                                    Session["FPMOffenseID"] = id;
                                    Session["oKnownOffender"] = null;

                                    if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                                    {
                                        KioskPaymentDetails _obj = new KioskPaymentDetails();
                                        _obj.ModuleId = 1;
                                        _obj.ServiceTypeId = 1;
                                        _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                                        _obj.SubPermissionId = 1;
                                        _obj.RequestedId = Convert.ToString(Session["FPMOffenseID"]);
                                        DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                                       
                                        if (dtKiosk.Rows.Count > 0)
                                        {
                                            _obj.RequestedId = Convert.ToString(Session["FPMOffenseID"]);
                                            _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                            _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                            _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                            _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                            _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                            _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                            return PartialView("KioskPaymentDetail", _obj);
                                        }
                                    }
                                    else
                                    {
                                        TempData["Parivad"] = "Registred successfully with RequestId:" + id;
                                        return RedirectToAction("Dashboard", "Dashboard");
                                    }
                                }
                            }
                        }
                        else
                        {
                            List<KnownOffender> list = new List<KnownOffender>();
                            KnownOffender obj = new KnownOffender();
                            obj = new KnownOffender
                            {
                                OOffenderType = _objModel.OffenderType,
                                OffenderName = "",
                                OFatherName = "",                              
                                OAddress1 = "",                             
                                OStateCode = "",
                                ODistrictCode = "",
                                OVillageCode = "",
                                OffenderStatement=""                                                        
                            };
                            list.Add(obj);
                            DataTable dtOffender = ExtMethodCommon.AsDataTable(list);
                            dtOffender.Columns.Remove("OOffenderrowid");
                            dtOffender.Columns.Remove("OffenderStatement");
                            dtOffender.Columns.Remove("OffenderStatementDoc");
                            dtOffender.AcceptChanges();
                            Int64 id = _objModel.SubmitDetails(_objModel, dtOffender);
                            if (id > 0) {
                                Session["FPMOffenseID"] = id;
                                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                                {
                                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                                    _obj.ModuleId = 1;
                                    _obj.ServiceTypeId = 1;
                                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                                    _obj.SubPermissionId = 1;
                                    _obj.RequestedId = Convert.ToString(Session["FPMOffenseID"]);
                                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);

                                    if (dtKiosk.Rows.Count > 0)
                                    {
                                        _obj.RequestedId = Convert.ToString(Session["FPMOffenseID"]);
                                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                        return PartialView("KioskPaymentDetail", _obj);
                                    }
                                }
                                else
                                {
                                    TempData["Parivad"] = "Registred successfully";
                                    return RedirectToAction("Dashboard", "Dashboard");
                                }
                            }
                                                       
                        }
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Dashboard"); 
                    }               
            }

            catch (Exception ex)
            {
                Session["oKnownOffender"] = null;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View("");
        }   
    

        /// <summary>
        /// function for edit of offender details
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditDetails(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                KnownOffender OFA = new KnownOffender();
                List<KnownOffender> lstKnownOffender = new List<KnownOffender>();
                List<KnownOffender> lstKnownOffenderEdit = new List<KnownOffender>();
                if (Session["oKnownOffender"] != null)
                {
                    lstKnownOffender = (List<KnownOffender>)Session["oKnownOffender"];
                    if (ID != "0" && ID.Length > 0)
                    {
                        KnownOffender Offder = lstKnownOffender.Single(a => a.OOffenderrowid == ID);
                        lstKnownOffenderEdit.Add(Offder);
                    }
                }
                foreach (var item in lstKnownOffenderEdit)
                {
                    OFA.OffenderName = item.OffenderName;                  
                    OFA.OFatherName = item.OFatherName;               
                    OFA.OAddress1 = item.OAddress1;                  
                    OFA.OStateCode = item.OStateCode;                 
                    OFA.OVillageCode = item.OVillageCode;
                    OFA.ODistrictCode = item.ODistrictCode;                    
                    OFA.OffenderStatement = item.OffenderStatement;
                    OFA.OffenderStatementDoc = string.IsNullOrEmpty(item.OffenderStatementDoc) ? "N/A" : "<a  href='" + @Url.Content(item.OffenderStatementDoc) + "' target='_blank' rel = 'noopener noreferrer'><img src='../images/jpeg.png' Width='30' /></a>";                   
                }
                return Json(OFA, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return null;
        }

        /// <summary>
        /// Use for Page load data of Offense View
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewParivadOffense()
        {
            List<AssignOffence> Offenderdata = new List<AssignOffence>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
              
                DataSet dtf = _objModel.GetViewExistingRecords();
                if (dtf.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dtf.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf.Tables[0].Rows)
                            Offenderdata.Add(
                              new AssignOffence()
                              {
                                  District = dr["District"].ToString(),
                                 // UserRole = dr["UserRole"].ToString(),
                                  OffenseCode = dr["OffenseCode"].ToString(),
                                  OffensePlace = dr["OffensePlace"].ToString(),
                                  OffenseDate = dr["OffenseDate"].ToString(),
                                  OffenseTime = dr["OffenseTime"].ToString(),
                                //  Status = Convert.ToInt32(dr["Status"].ToString())
                              });
                    }
                    ViewData["OffenderList"] = Offenderdata;
                }
                else
                {
                    Offenderdata.Add(
                        new AssignOffence()
                        {
                            District = "",
                           // UserRole = "",
                            OffenseCode = "",
                            OffensePlace = "",
                            OffenseDate = "",
                            OffenseTime = "",
                           // Status = 0
                        });
                    ViewData["OffenderList"] = Offenderdata;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return View();
        }

        /// <summary>
        /// Use for Load the data based User role and Offense code
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <param name="UserRole"></param>
        /// <returns></returns>
        public ActionResult EditRecord(string OffenseCode, string UserRole)
        {
            FormCollection fm = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {                
                Session.Remove("AddVechile");
                _objModel.UserID = UserID;
                if (OffenseCode != null)
                {
                    Session["FPMOffenseCode"] = Encryption.decrypt(OffenseCode);
                    Session["FPMUserRole"] = Encryption.decrypt(UserRole);
                    _objModel.UserRole = Encryption.decrypt(UserRole);
                    DataSet dtf2 = _objModel.GetAllRecordsForm2(Session["FPMOffenseCode"].ToString());
                    
                    if (dtf2.Tables[0].Rows.Count > 0)
                    {
                        _objModel.OffenderType = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Offender_Known"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Offender_Known"].ToString();
                        _objModel.ONumberOfOffender = Convert.ToInt32(string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["No_of_offender"].ToString()) ? "0" : dtf2.Tables[0].Rows[0]["No_of_offender"].ToString());
                        _objModel.OffenderDescription = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Description_of_offenders"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Description_of_offenders"].ToString();
                        _objModel.OffenseSeverity = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Offense_Severity"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Offense_Severity"].ToString();
                        _objModel.ForestType = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Type_of_Forest"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Type_of_Forest"].ToString();
                        _objModel.OffenceCategory = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Category_of_Offense"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Category_of_Offense"].ToString();
                        _objModel.OffenseSubCategoryWildLife = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Wildlife_Sub_Section"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Wildlife_Sub_Section"].ToString();
                        _objModel.OffenseSubCategoryForest = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Forest_Sub_Section"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Forest_Sub_Section"].ToString();
                        _objModel.WildlifeProtectionSection = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Wildlife_Section"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Wildlife_Section"].ToString();
                        _objModel.ForestProtectionSection = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Forest_Section"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Forest_Section"].ToString();
                        _objModel.OffenseCode = Session["FPMOffenseCode"].ToString();
                        _objModel.VisitDate = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["DateOfVisit"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["DateOfVisit"].ToString();
                        _objModel.VisitPlace = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["VisitPlace"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["VisitPlace"].ToString();
                        _objModel.ComplainFound = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Complaint_Found"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["Complaint_Found"].ToString();
                        _objModel.MokaPunchnama = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Mokapunchnama"].ToString()) ? "~" : dtf2.Tables[0].Rows[0]["Mokapunchnama"].ToString();
                        _objModel.NagriNaka = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Najri_Naksha"].ToString()) ? "~" : dtf2.Tables[0].Rows[0]["Najri_Naksha"].ToString();
                        _objModel.WitnessRecord1 = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Witness_Recorded1"].ToString()) ? "~" : dtf2.Tables[0].Rows[0]["Witness_Recorded1"].ToString();
                        _objModel.WitnessRecord2 = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Witness_Recorded2"].ToString()) ? "~" : dtf2.Tables[0].Rows[0]["Witness_Recorded2"].ToString();
                        _objModel.WitnessRecord3 = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Witness_Recorded3"].ToString()) ? "~" : dtf2.Tables[0].Rows[0]["Witness_Recorded3"].ToString();
                        _objModel.FieldInspection = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["FieldInspection"].ToString()) ? "~" : dtf2.Tables[0].Rows[0]["FieldInspection"].ToString();
                        _objModel.Recomendation = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["Recommendation"].ToString()) ? "~" : dtf2.Tables[0].Rows[0]["Recommendation"].ToString();
                        _objModel.ListOfItemSeized = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["List_of_ArticalSeized"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["List_of_ArticalSeized"].ToString();
                        _objModel.VehicleSeized = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["VehicleSeized"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["VehicleSeized"].ToString();
                        if (Session["FPMUserRole"] != null)
                            _objModel.UserRole = Session["FPMUserRole"].ToString();
                    }

                }
                DDLList();
                getDropdown();
                _objModel.OffenseCode = Session["FPMOffenseCode"].ToString();                     
                return View("ParivadRegistration", _objModel);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

      

        /// <summary>
        /// Use for Bind the Existing Offender entries based on Offense Code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetMultiOffenderDetails(string id)
        {
            List<KnownOffender> list = new List<KnownOffender>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                _objModel.UserID = UserID;
                if (!String.IsNullOrEmpty(id))
                {
                    if (Session["FPMOffenseCode"] != null)
                    {
                      
                        DataSet dtf3 = _objModel.GetCitizenOffenderRecords("SelectCitizenOffenderDetails", id);
                        if (dtf3.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtf3.Tables[0].Rows)
                            {
                                if (dr["Name"].ToString() != "" && dr["State_Name"].ToString() != "") {
                                    list.Add(
                                        new KnownOffender()
                                        {
                                            OOffenderrowid = Guid.NewGuid().ToString(),
                                            OffenderName = dr["Name"].ToString(),
                                            OFatherName = dr["Father_Name"].ToString(),
                                            OAddress1 = dr["Address"].ToString(),
                                            OStateCode = dr["State_Name"].ToString(),
                                            ODistrictCode = dr["Dist_Code"].ToString(),
                                            OVillageCode = dr["Village_Code"].ToString(),
                                            OffenderStatement = dr["Statement"].ToString(),

                                        });
                                    Session["oKnownOffender"] = list;
                                }
                                else {
                                    list.Add(
                                           new KnownOffender()
                                           {
                                               OOffenderrowid = Guid.NewGuid().ToString(),
                                               OffenderName = dr["Name"].ToString(),
                                               OFatherName = dr["Father_Name"].ToString(),
                                               OAddress1 = dr["Address"].ToString(),
                                               OStateCode = dr["State_Name"].ToString(),
                                               ODistrictCode = dr["Dist_Code"].ToString(),
                                               OVillageCode = dr["Village_Code"].ToString(),
                                               OffenderStatement = dr["Statement"].ToString(),

                                           });
                                    Session["oKnownOffender"] = null;
                                }                                                                
                            }
                        }
                      
                        return Json(list, JsonRequestBehavior.AllowGet);

                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;

        }

        /// <summary>
        /// Function for fetching vehicle details
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVehicleDetails()
        {
            List<CitizenParivad> lstVechile = new List<CitizenParivad>();
            CitizenParivad OR = new CitizenParivad();    
             string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                DataSet dsForesterDetails = OR.GetSeizedVehicleDetails();
                if (dsForesterDetails.Tables[0].Rows.Count > 0)
                {
                    CitizenParivad OR1 = null;
                    for (int i = 0; i < dsForesterDetails.Tables[0].Rows.Count; i++)
                    {
                        OR1 = new CitizenParivad();
                        OR1.Vechilerowid = Guid.NewGuid().ToString();
                        OR1.VechileRegistrationNo = dsForesterDetails.Tables[0].Rows[i]["VehicleRegistrationNo"].ToString();
                        OR1.VechileOwnerName = dsForesterDetails.Tables[0].Rows[i]["OwnerName"].ToString();
                        OR1.VechileType = dsForesterDetails.Tables[0].Rows[i]["VehicleType"].ToString();
                        OR1.VechileMake = dsForesterDetails.Tables[0].Rows[i]["VehicleMake"].ToString();
                        OR1.VechileModel = dsForesterDetails.Tables[0].Rows[i]["VehicleModel"].ToString();
                        OR1.VechileChassisNo = dsForesterDetails.Tables[0].Rows[i]["ChassisNo"].ToString();
                        OR1.VechileEngineNo = dsForesterDetails.Tables[0].Rows[i]["EngineNo"].ToString();
                        lstVechile.Add(OR1);
                    }
                    Session["AddVechile"] = lstVechile;
                }
                return Json(lstVechile, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

        /// <summary>
        /// Delete the vechile details based on ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteVechile(string Id)
        {
           string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<CitizenParivad> lstVechile = new List<CitizenParivad>();
            try
            {
                if (Session["AddVechile"] != null)
                {
                    lstVechile = (List<CitizenParivad>)Session["AddVechile"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        CitizenParivad ofreg = lstVechile.Single(a => a.Vechilerowid == Id);
                        lstVechile.Remove(ofreg);
                    }
                    Session["AddVechile"] = lstVechile;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstVechile, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Bind village on dist code block code and gp code
        /// </summary>
        /// <param name="District_code"></param>
        /// <param name="Block_code"></param>
        /// <param name="GP_Code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getVillage(string dist_code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> lstVillage = new List<SelectListItem>();
            CitizenParivad or = new CitizenParivad();
            try
            {
                DataTable dtVillage = new DataTable();
                dtVillage = or.GetVillage(dist_code);
                foreach (System.Data.DataRow dr in dtVillage.Rows)
                {
                    if (@dr["VILL_NAME"].ToString() != "--Select--")
                    {
                        lstVillage.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 4, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(new SelectList(lstVillage, "Value", "Text"));
        }


        /// <summary>
        /// Use for Save the multiple Offender details
        /// </summary>
        /// <param name="offender"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OffenderData(KnownOffender offender)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                _objModel.UserID = UserID;
                if (offender.OffenderName != null && offender.OFatherName != null )
                {
                    List<KnownOffender> lstKnownOffender = new List<KnownOffender>();
                    if (Session["oKnownOffender"] != null)
                    {
                        List<KnownOffender> list = (List<KnownOffender>)Session["oKnownOffender"];
                        if (list != null && list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                if (item.OOffenderrowid == offender.OOffenderrowid)
                                {
                                    lstKnownOffender.Add(offender);
                                }
                                else
                                {
                                    lstKnownOffender.Add(item);
                                }
                            }
                        }
                        if (offender.OOffenderrowid == null || offender.OOffenderrowid == "")
                        {
                            offender.OOffenderrowid = Guid.NewGuid().ToString();
                            lstKnownOffender.Add(offender);
                        }
                        Session["oKnownOffender"] = null;
                        Session["oKnownOffender"] = lstKnownOffender;
                    }
                    else
                    {
                        offender.OOffenderrowid = Guid.NewGuid().ToString();
                        lstKnownOffender.Add(offender);
                        Session["oKnownOffender"] = lstKnownOffender;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return Json(offender, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Global function for binding dropdown
        /// </summary>
        private void DDLList()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> Range = new List<SelectListItem>();
            List<SelectListItem> District = new List<SelectListItem>();
            List<SelectListItem> OffensePlace = new List<SelectListItem>();
            List<SelectListItem> ForestType = new List<SelectListItem>();
            List<SelectListItem> WildlifeProtectionAct = new List<SelectListItem>();
            List<SelectListItem> OffenseSeverity = new List<SelectListItem>();
            List<SelectListItem> ForestProtectionAct = new List<SelectListItem>();
            List<SelectListItem> OPhotoIDType = new List<SelectListItem>();
            List<SelectListItem> WildLifeSubCategory = new List<SelectListItem>();
            List<SelectListItem> ForestSubCategory = new List<SelectListItem>();
            List<SelectListItem> ForestOffense = new List<SelectListItem>();
            Location cs = new Location();

            try
            {

                _objModel.UserID = UserID;
                ViewBag.ddlOState = new SelectList(FPMParivadRegistrations.DDLState(), "Value", "Text");
                DataTable dtoffense = new OffenseReg().Get_OffenseCategory();
                ViewBag.Offense = dtoffense;
                foreach (System.Data.DataRow dr in ViewBag.Offense.Rows)
                {
                    ForestOffense.Add(new SelectListItem { Text = @dr["FOCategory"].ToString(), Value = @dr["FOCatID"].ToString() });
                }
                ViewBag.OffenseCategory = ForestOffense;
                //DataTable dtr = _objModel.Get_Range_for_LoginUser();
                //foreach (System.Data.DataRow dr in dtr.Rows)
                //{
                //    Range.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                //}
                //ViewBag.RangeCode1 = new SelectList(Range, "Value", "Text");

                DataTable dtd = cs.District("District");
                foreach (System.Data.DataRow dr in dtd.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.ddlODistrict = new SelectList(District, "Value", "Text");

                DataTable dto = new OffenseReg().Get_OffensePlace();
                foreach (System.Data.DataRow dr in dto.Rows)
                {
                    OffensePlace.Add(new SelectListItem { Text = @dr["Offense_Place"].ToString(), Value = @dr["OPlaceID"].ToString() });
                }
                ViewBag.OffensePlace = new SelectList(OffensePlace, "Value", "Text");

                DataTable dtw = new OffenseReg().Get_WildlifeProtectionAct();
                foreach (System.Data.DataRow dr in dtw.Rows)
                {
                    WildlifeProtectionAct.Add(new SelectListItem { Text = @dr["Wildlife_Protection_Act"].ToString(), Value = @dr["WProtectionActID"].ToString() });
                }
                ViewBag.WildlifeProtectionAct = new SelectList(WildlifeProtectionAct, "Value", "Text");

                DataTable dtf = new OffenseReg().Get_ForestProtectionAct();
                foreach (System.Data.DataRow dr in dtf.Rows)
                {
                    ForestProtectionAct.Add(new SelectListItem { Text = @dr["Forest_Protection_Act"].ToString(), Value = @dr["FProtectionActID"].ToString() });
                }
                ViewBag.ForestProtectionAct = new SelectList(ForestProtectionAct, "Value", "Text");

                DataTable dtfype = new OffenseReg().Get_ForestType();
                foreach (System.Data.DataRow dr in dtfype.Rows)
                {
                    ForestType.Add(new SelectListItem { Text = @dr["Forest_Type"].ToString(), Value = @dr["FTypeID"].ToString() });
                }
                ViewBag.ForestType1 = new SelectList(ForestType, "Value", "Text");

                DataTable dtosev = new OffenseReg().Get_OffenseSeverity();
                foreach (System.Data.DataRow dr in dtosev.Rows)
                {
                    OffenseSeverity.Add(new SelectListItem { Text = @dr["Offense_Severity"].ToString(), Value = @dr["OffenseSeverityID"].ToString() });
                }
                ViewBag.OffenseSeverity1 = new SelectList(OffenseSeverity, "Value", "Text");

                DataTable dtoPhoto = new OffenseReg().Get_OPhotoIDType();
                foreach (System.Data.DataRow dr in dtoPhoto.Rows)
                {
                    OPhotoIDType.Add(new SelectListItem { Text = @dr["PhotoID_Name"].ToString(), Value = @dr["OPhotoIDTypeID"].ToString() });
                }
                ViewBag.OPhotoIDType = new SelectList(OPhotoIDType, "Value", "Text");

                DataTable dtWildlifeSubCat = new OffenseReg().Get_WildLifeSubCategory();
                foreach (System.Data.DataRow dr in dtWildlifeSubCat.Rows)
                {
                    WildLifeSubCategory.Add(new SelectListItem { Text = @dr["WOSubCategory"].ToString(), Value = @dr["WOSubCatID"].ToString() });
                }
                ViewBag.WildLifeSubCategory = new SelectList(WildLifeSubCategory, "Value", "Text");

                DataTable dtForestSubCat = new OffenseReg().Get_ForestSubCategory("");
                foreach (System.Data.DataRow dr in dtForestSubCat.Rows)
                {
                    ForestSubCategory.Add(new SelectListItem { Text = @dr["FOSubCategory"].ToString(), Value = @dr["FOSubCatID"].ToString() });
                }
                ViewBag.ForestSubCategory = new SelectList(ForestSubCategory, "Value", "Text");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
        }


        /// <summary>
        /// Function for upload file 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string FilePath = "~/ForestProtectionDocument/";

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  
                        HttpPostedFileBase file = files[i];
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            FileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            FileName = file.FileName;
                        }

                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        file.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    // Returns message that successfully uploaded  

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
                }
                return Json(new { list1 = "File Uploaded Successfully!", list2 = path });
            }
            else
            {
                return Json("No files selected.");
            }
        }

        /// <summary>
        /// Add the mutiple vechile details
        /// </summary>
        /// <param name="VechileRegistrationNo"></param>
        /// <param name="VechileOwnerName"></param>
        /// <param name="VechileType"></param>
        /// <param name="VechileMake"></param>
        /// <param name="VechileModel"></param>
        /// <param name="VechileChassisNo"></param>
        /// <param name="VechileEngineNo"></param>
        /// <param name="PastOffensesByVechile"></param>
        /// <returns></returns>
        public JsonResult AddVechile(string VechileRegistrationNo, string VechileOwnerName, string VechileType, string VechileMake, string VechileModel, string VechileChassisNo, string VechileEngineNo, string VechileUploadDoc)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CitizenParivad OffenseRegist = new CitizenParivad();
            List<CitizenParivad> lstVechile = new List<CitizenParivad>();
           
            try
            {
                OffenseRegist.VechileRegistrationNo = VechileRegistrationNo;
                OffenseRegist.VechileOwnerName = VechileOwnerName;
                OffenseRegist.VechileType = VechileType;
                OffenseRegist.VechileMake = VechileMake;
                OffenseRegist.VechileModel = VechileModel;
                OffenseRegist.VechileChassisNo = VechileChassisNo;
                OffenseRegist.VechileEngineNo = VechileEngineNo;              
                OffenseRegist.VechileUploadDoc = VechileUploadDoc;
                if (Session["AddVechile"] != null)
                {
                    lstVechile = (List<CitizenParivad>)Session["AddVechile"];
                    if (!lstVechile.Any(element => element.Vechilerowid == OffenseRegist.Vechilerowid))
                    {
                        OffenseRegist.Vechilerowid = Guid.NewGuid().ToString();
                        lstVechile.Add(OffenseRegist);
                    }
                }
                else
                {
                    OffenseRegist.Vechilerowid = Guid.NewGuid().ToString();
                    lstVechile.Add(OffenseRegist);
                    Session["AddVechile"] = lstVechile;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(lstVechile, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Final submission of parivad
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitParivad(FormCollection fcollection, CitizenParivad Model, string Command, HttpPostedFileBase MokaPunchnama, HttpPostedFileBase NagriNaka, HttpPostedFileBase WitnessRecord1, HttpPostedFileBase WitnessRecord2, HttpPostedFileBase WitnessRecord3, HttpPostedFileBase FieldInspection, HttpPostedFileBase Recomendation)
        {
            string Document = string.Empty;
            var path = "";
            string FilePath = "~/ForestProtectionDocument/";
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();           
            try {
                int existRecordCount = Convert.ToInt32(new OffenseReg().GetAllRecordsForm2(Session["FPMOffenseCode"].ToString(), "SelectForm2").Tables[0].Rows[0]["RowsCount"].ToString());
                if (existRecordCount > 0) { 
                
                
                }
                _objModel.OffenseCode = Session["FPMOffenseCode"].ToString();
                _objModel.OffenceCategory = string.IsNullOrEmpty(Model.OffenceCategory) ? "" : Model.OffenceCategory;
                _objModel.ForestType = string.IsNullOrEmpty(Model.ForestType) ? "" : Model.ForestType;
                _objModel.OffenseSeverity = string.IsNullOrEmpty(Model.OffenseSeverity) ? "" : Model.OffenseSeverity;
                _objModel.OffenderType = string.IsNullOrEmpty(fcollection["Offender"].ToString()) ? "" : fcollection["Offender"].ToString(); 
                _objModel.ONumberOfOffender = Model.ONumberOfOffender;
                _objModel.OffenderDescription = string.IsNullOrEmpty(Model.OffenderDescription) ? "" : Model.OffenderDescription;
                _objModel.OffenseSubCategoryWildLife = string.IsNullOrEmpty(fcollection["OffenseSubCategoryWildLife"].ToString()) ? "" : fcollection["OffenseSubCategoryWildLife"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.OffenseSubCategoryWildLife) ? "" : Model.OffenseSubCategoryWildLife;
                _objModel.OffenseSubCategoryForest = string.IsNullOrEmpty(fcollection["OffenseSubCategoryForest"].ToString()) ? "" : fcollection["OffenseSubCategoryForest"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.OffenseSubCategoryForest) ? "" : Model.OffenseSubCategoryForest;
                _objModel.WildlifeProtectionSection = string.IsNullOrEmpty(fcollection["WildlifeProtectionSection"].ToString()) ? "" : fcollection["WildlifeProtectionSection"].Replace(",", "").Trim().ToString();// string.IsNullOrEmpty(Model.WildlifeProtectionSection) ? "" : Model.WildlifeProtectionSection;
                _objModel.ForestProtectionSection = string.IsNullOrEmpty(fcollection["ForestProtectionSection"].ToString()) ? "" : fcollection["ForestProtectionSection"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.ForestProtectionSection) ? "" : Model.ForestProtectionSection;
                _objModel.VisitDate = string.IsNullOrEmpty(fcollection["VisitDate"].ToString()) ? "" : fcollection["VisitDate"].Replace(",", "").Trim().ToString();
                _objModel.VisitPlace = string.IsNullOrEmpty(fcollection["VisitPlace"].ToString()) ? "" : fcollection["VisitPlace"].Replace(",", "").Trim().ToString();
                _objModel.ComplainFound = string.IsNullOrEmpty(fcollection["ComplainFound"].ToString()) ? "" : fcollection["ComplainFound"].Replace(",", "").Trim().ToString();
              
                if (MokaPunchnama != null && MokaPunchnama.ContentLength > 0)
                {
                    Document = Path.GetFileName(MokaPunchnama.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    _objModel.MokaPunchnama = path;
                    MokaPunchnama.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    _objModel.MokaPunchnama = string.IsNullOrEmpty(fcollection["hdMokaPunchnama"].ToString()) ? "" : fcollection["hdMokaPunchnama"].ToString();  
                }
                if (NagriNaka != null && NagriNaka.ContentLength > 0)
                {
                    Document = Path.GetFileName(NagriNaka.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    _objModel.NagriNaka = path;
                    NagriNaka.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    _objModel.NagriNaka = string.IsNullOrEmpty(fcollection["hdNagriNaka"].ToString()) ? "" : fcollection["hdNagriNaka"].ToString();
                }

                if (WitnessRecord1 != null && WitnessRecord1.ContentLength > 0)
                {
                    Document = Path.GetFileName(WitnessRecord1.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    _objModel.WitnessRecord1 = path;
                    WitnessRecord1.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    _objModel.WitnessRecord1 = string.IsNullOrEmpty(fcollection["hdWitnessRecord1"].ToString()) ? "" : fcollection["hdWitnessRecord1"].ToString();
                }
                if (WitnessRecord2 != null && WitnessRecord2.ContentLength > 0)
                {
                    Document = Path.GetFileName(WitnessRecord2.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    _objModel.WitnessRecord2 = path;
                    WitnessRecord2.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    _objModel.WitnessRecord2 = string.IsNullOrEmpty(fcollection["hdWitnessRecord2"].ToString()) ? "" : fcollection["hdWitnessRecord2"].ToString();
                }

                if (WitnessRecord3 != null && WitnessRecord3.ContentLength > 0)
                {
                    Document = Path.GetFileName(WitnessRecord3.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    _objModel.WitnessRecord3 = path;
                    WitnessRecord3.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    _objModel.WitnessRecord3 = string.IsNullOrEmpty(fcollection["hdWitnessRecord3"].ToString()) ? "" : fcollection["hdWitnessRecord3"].ToString();
                }

                if (FieldInspection != null && FieldInspection.ContentLength > 0)
                {
                    Document = Path.GetFileName(FieldInspection.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    _objModel.FieldInspection = path;
                    FieldInspection.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    _objModel.FieldInspection = string.IsNullOrEmpty(fcollection["hdFieldInspection"].ToString()) ? "" : fcollection["hdFieldInspection"].ToString();
                }

                if (Recomendation != null && Recomendation.ContentLength > 0)
                {
                    Document = Path.GetFileName(Recomendation.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    _objModel.Recomendation = path;
                    Recomendation.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    _objModel.Recomendation = string.IsNullOrEmpty(fcollection["hdRecomendation"].ToString()) ? "" : fcollection["hdRecomendation"].ToString();
                }
                _objModel.ListOfItemSeized = string.IsNullOrEmpty(fcollection["ListOfItemSeized"].ToString()) ? "" : fcollection["ListOfItemSeized"].Replace(",", "").Trim().ToString();
                _objModel.VehicleSeized = string.IsNullOrEmpty(fcollection["VehicleSeized"].ToString()) ? "" : fcollection["VehicleSeized"].Replace(",", "").Trim().ToString();
                DataTable dtVechile = VechileDescription();
                if (_objModel.VehicleSeized == "Yes") {

                    if (dtVechile.Rows.Count == 0) {

                        return RedirectToAction("EditRecord", "CitizenParivadRegistration");
                    }
                }
                                             
                if (Session["oKnownOffender"] != null)
                {
                    List<KnownOffender> list = (List<KnownOffender>)Session["oKnownOffender"];
                    if (list != null)
                    {
                        DataTable DToffender = ExtMethodCommon.AsDataTable(list);
                        DToffender.Columns.Remove("OOffenderrowid");                     
                        DToffender.AcceptChanges();
                        string id = _objModel.FinalSubmission(_objModel, DToffender, dtVechile);
                        if (id != "0")
                        {                            
                            DDLList();
                            Session["oKnownOffender"] = null;
                            TempData["ForesterParivad"] = "Registred successfully with requestid:" + Session["FPMOffenseCode"].ToString();
                            Session["Servicetype"] = "Protection";
                            return RedirectToAction("ForesterAction", "ForesterAction");
                        }
                    }
                }
                else
                {
                    List<KnownOffender> list = new List<KnownOffender>();
                    KnownOffender obj = new KnownOffender();
                    obj = new KnownOffender
                    {
                        OOffenderType = _objModel.OffenderType,
                        OffenderName = "",
                        OFatherName = "",                       
                        OAddress1 = "",                       
                        OStateCode = "",
                        ODistrictCode = "",
                        OVillageCode = "",                      
                        OffenderStatement = "",                                        
                    };
                    list.Add(obj);
                    DataTable DToffender = ExtMethodCommon.AsDataTable(list);
                    DToffender.Columns.Remove("OOffenderrowid");
                    DToffender.AcceptChanges();
                    string id = _objModel.FinalSubmission(_objModel, DToffender, dtVechile);
                    if (id != "0")
                    {                        
                        DDLList();
                        Session["oKnownOffender"] = null;
                       // return RedirectToAction("EditRecord", "CitizenParivadRegistration");
                        TempData["ForesterParivad"] = "Registred Successfully with requestid:" + Session["FPMOffenseCode"].ToString();
                        Session["Servicetype"] = "Protection";
                        return RedirectToAction("ForesterAction", "ForesterAction");
                    }
                }            
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            TempData["ForesterParivad"] = "Record save successfully";
            Session["Servicetype"] = "Protection";
            return RedirectToAction("ForesterAction", "ForesterAction");
           
        }

        /// <summary>
        /// Get vehicle description
        /// </summary>
        /// <returns></returns>
        public DataTable VechileDescription()
        {
            DataTable dtVechile = new DataTable("Table");
            dtVechile.Columns.Add("VehicleRegistrationNo", typeof(String));
            dtVechile.Columns.Add("OwnerName", typeof(String));
            dtVechile.Columns.Add("VehicleType", typeof(String));
            dtVechile.Columns.Add("VehicleMake", typeof(String));
            dtVechile.Columns.Add("VehicleModel", typeof(String));
            dtVechile.Columns.Add("ChassisNo", typeof(String));
            dtVechile.Columns.Add("EngineNo", typeof(String));
            dtVechile.Columns.Add("UploadDoc", typeof(String));
            dtVechile.AcceptChanges();
            List<CitizenParivad> lstSeized = new List<CitizenParivad>();
            if (Session["AddVechile"] != null)
            {
                lstSeized = (List<CitizenParivad>)Session["AddVechile"];
            }
            foreach (CitizenParivad objSeized in lstSeized)
            {
                DataRow dr = dtVechile.NewRow();
                dr["VehicleRegistrationNo"] = objSeized.VechileRegistrationNo;
                dr["OwnerName"] = objSeized.VechileOwnerName;
                dr["VehicleType"] = objSeized.VechileType;
                dr["VehicleMake"] = objSeized.VechileMake;
                dr["VehicleModel"] = objSeized.VechileModel;
                dr["ChassisNo"] = objSeized.VechileChassisNo;
                dr["EngineNo"] = objSeized.VechileEngineNo;
                dr["UploadDoc"] = objSeized.VechileUploadDoc;
                dtVechile.Rows.Add(dr);
                dtVechile.AcceptChanges();
            }
            return dtVechile;
        }

        /// <summary>
        /// Web service for getting vehicle details
        /// </summary>
        /// <param name="VechileRegistrationNumber"></param>
        /// <returns></returns>
        [HttpPost]
       
        public JsonResult GetRTOVechileRDetails(string VechileRegistrationNumber)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                cls_RTODetails.VehicleDetails RTOVechileXmlData;
                RTOVechileXmlData = cls_RTODetails.GetRTO(VechileRegistrationNumber);
                return Json(RTOVechileXmlData);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return null;
        }
        
        /// CR No-31,CR Date-16-June-2016  
        /// Function for auto complete functionality
        [HttpPost]
        public JsonResult GetAutoComplete(string Division, string District, string Block, string Gram, string Range, string Village, string Option)
        {
            try
            {
                DataTable dt = new Common().AutoComplete(Division, District, Block, Gram, Range, Village, Option);
                if (Option == "DISTRICT")
                {
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        Dist.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }
                    return Json(new SelectList(Dist, "Value", "Text"));
                }
                if (Option == "BLOCK")
                {
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        BlockName.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                    }
                    return Json(new SelectList(BlockName, "Value", "Text"));
                }
                if (Option == "GRAM")
                {
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        GPName.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                    return Json(new SelectList(GPName, "Value", "Text"));
                }
                if (Option == "VILLAGE")
                {
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        VillageName.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    return Json(new SelectList(VillageName, "Value", "Text"));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult GethierarchyDetails(string Division, string District, string Block, string Gram, string Range, string Village, string Option)
        {
            try
            {
                CitizenParivad cp = new CitizenParivad();
                // DataSet dslst = new CitizenParivad().GethierarchyDetails(Village,Block,GP, option);
                DataTable dt = new Common().AutoComplete(Division, District, Block, Gram, Range, Village, Option);

                if (Option == "OVILLAGE")
                {
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        cp.DistrictID = dr["DIST_NAME"].ToString();
                        cp.hdnDistCode = dr["DIST_CODE"].ToString();
                        cp.BlockCode = dr["BLK_NAME"].ToString();
                        cp.hdnBlockCode = dr["BLK_CODE"].ToString();
                        cp.GPCode = dr["GP_NAME"].ToString();
                        cp.hdnGPCode = dr["GP_CODE"].ToString();
                    }
                }
                if (Option == "OGRAM")
                {
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        cp.DistrictID = dr["DIST_NAME"].ToString();
                        cp.hdnDistCode = dr["DIST_CODE"].ToString();
                        cp.BlockCode = dr["BLK_NAME"].ToString();
                        cp.hdnBlockCode = dr["BLK_CODE"].ToString();
                    }
                }
                if (Option == "OBLOCK")
                {
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        cp.DistrictID = dr["DIST_NAME"].ToString();
                        cp.hdnDistCode = dr["DIST_CODE"].ToString();
                    }
                }
                return Json(cp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    
    }
}
