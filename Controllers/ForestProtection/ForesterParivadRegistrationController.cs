//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         :  ForesterParivadRegistration
//  Description  : File contains registration for forest protection by forester and related activity
//  Date Created : 20-09-2016
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
using FMDSS.Models;
using System.Web.Mvc;
using FMDSS.Models.ForestProtection;
using FMDSS.Models.Admin;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;

namespace FMDSS.Controllers.ForestProtection
{
    public class ForesterParivadRegistrationController : BaseController
    {

        #region "Declare the Page level Variables"
        int ModuleID = 4;
        ForesterParivad _objModel = new ForesterParivad();
        #endregion
        /// <summary>
        /// Global declaration of userid
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
        /// Function to return regstration view and related controls
        /// </summary>
        /// <returns></returns>
        public ActionResult FParivadRegistration()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> lstOfficer = new List<SelectListItem>();
            DataTable dtOfficer = new DataTable();
            try
            {
                _objModel.UserID = UserID;         
                DataTable dt = new DataTable();
                dt = _objModel.GetCircle_Div_by_Member();
                if (dt != null) 
                {
                    if (dt.Rows.Count > 0)
                    {
                        _objModel.CircleCode = dt.Rows[0]["CIRCLE_CODE"].ToString();
                        _objModel.DivisionCode = dt.Rows[0]["DIV_CODE"].ToString();
                        _objModel.DistrictCode = dt.Rows[0]["DIST_CODE"].ToString();
                        ViewBag.circle = dt.Rows[0]["CIRCLE_NAME"].ToString();
                        ViewBag.division = dt.Rows[0]["DIV_NAME"].ToString();
                        ViewBag.district = dt.Rows[0]["DIST_NAME"].ToString();
                    }
                }
                dtOfficer = _objModel.GetOfficerDesignation();
                lstOfficer.Add(new SelectListItem { Text = "Self", Value = Convert.ToString(_objModel.UserID) });
                foreach (System.Data.DataRow dr in dtOfficer.Rows)
                {
                    lstOfficer.Add(new SelectListItem { Text = @dr["SSO_ID"].ToString(), Value = @dr["ROWID"].ToString() });
                }
                ViewBag.ForestOfficer = lstOfficer;
                DDLList();
                return View(_objModel);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// Function to get details of offense for print fir
        /// </summary>
        /// <returns></returns>
        public ActionResult OffenseDetails()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ForesterParivad FP = new ForesterParivad();
            List<ForesterParivad> lstCase = new List<ForesterParivad>();
            DataTable dtOffense = new DataTable();
            try
            {
                dtOffense = FP.GetOffenseDetails();
                if (dtOffense.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dr in dtOffense.Rows)
                    {
                        lstCase.Add(new ForesterParivad
                        {
                            ApplicantName=dr["AccusedName"].ToString(),
                            OffenseCode = dr["Offense_code"].ToString(),
                            OffensePlace = dr["Place_of_offense"].ToString(),
                            OffenseDate = dr["OffenseDate"].ToString(),
                            Offence_Description = dr["Description_of_Offense"].ToString(),
                            ComplaintFound = dr["Complaint_Found"].ToString(),
                            OffenseStatus = dr["Satus"].ToString(),
                        });
                    }
                }
                ViewBag.Offenselist = lstCase;
                return View();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
       
       
        /// <summary>
        /// Use for Save the details of Form 1 (Forestor Ragister Parivad)
        /// </summary>
        /// <param name="fcollection"></param>
        /// <param name="Model"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitDetails(FormCollection fcollection, ForesterParivad Model, string Command, List<HttpPostedFileBase> fileUpload)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                     _objModel.UserID = UserID;                                                              
                    _objModel.CircleCode = string.IsNullOrEmpty(Model.CircleCode) ? "" : Model.CircleCode;
                    _objModel.DivisionCode = string.IsNullOrEmpty(Model.DivisionCode) ? "" : Model.DivisionCode;
                    _objModel.DistrictCode = string.IsNullOrEmpty(Model.DistrictCode) ? "" : Model.DistrictCode;
                    _objModel.RangeCode = string.IsNullOrEmpty(Model.RangeCode) ? "" : Model.RangeCode;
                    _objModel.Tehsil = string.IsNullOrEmpty(Model.Tehsil) ? "" : Model.Tehsil;
                    _objModel.Naka = string.IsNullOrEmpty(Model.Naka) ? "" : Model.Naka;
                    _objModel.ForestBlock = string.IsNullOrEmpty(Model.ForestBlock) ? "" : Model.ForestBlock;
                    _objModel.Compartment = string.IsNullOrEmpty(Model.Compartment) ? "" : Model.Compartment;
                    _objModel.OffensePlace = string.IsNullOrEmpty(Model.OffensePlace) ? "" : Model.OffensePlace;
                    _objModel.Latitude = string.IsNullOrEmpty(Model.Latitude) ? "" : Model.Latitude;
                    _objModel.Longitude = string.IsNullOrEmpty(Model.Longitude) ? "" : Model.Longitude;
                    _objModel.LandMark = string.IsNullOrEmpty(Model.LandMark) ? "" : Model.LandMark;
                    _objModel.OffenseDate = string.IsNullOrEmpty(Model.OffenseDate) ? "" : Model.OffenseDate;
                    _objModel.OffenseTime = string.IsNullOrEmpty(Model.OffenseTime) ? "" : Model.OffenseTime;
                    _objModel.NakaDistance = string.IsNullOrEmpty(Model.NakaDistance) ? "" : Model.NakaDistance;
                    _objModel.OffenceCategory = string.IsNullOrEmpty(Model.OffenceCategory) ? "" : Model.OffenceCategory;
                    _objModel.Offence_Description = string.IsNullOrEmpty(Model.Offence_Description) ? "" : Model.Offence_Description;
                    _objModel.ApplicantName = string.IsNullOrEmpty(Model.ApplicantName) ? "" : Model.ApplicantName;
                    _objModel.ForestType = Model.ForestType;
                    _objModel.ForestOfficer = string.IsNullOrEmpty(Model.ForestOfficer) ? "" : Model.ForestOfficer;
                    var path = "";
                    string FileName = string.Empty;
                    string FileFullName = string.Empty;
                    string FilePath = "~/ForestProtectionDocument/CrimeSceneDetails/";

                    System.Text.StringBuilder strbldr = new System.Text.StringBuilder();
                    int count = 0;
                    foreach (HttpPostedFileBase item in fileUpload)
                    {

                        if (item != null)
                        {
                            if (Array.Exists(Model.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
                            {
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = item.FileName.Split(new char[] { '\\' });
                                    FileName = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    FileName = item.FileName;
                                }
                                FileFullName = DateTime.Now.Ticks + "_" + FileName;
                                path = System.IO.Path.Combine(FilePath, FileFullName);

                                strbldr.Append(path + ",");
                                item.SaveAs(Server.MapPath(path));
                                count++;
                            }
                        }
                    }
                    _objModel.FilesToBeUploaded = strbldr.ToString().TrimEnd(',');
                    Int64 id = _objModel.SubmitForm1(_objModel);

                    if (id != 0)
                    {
                        TempData["ForesterParivad"] = "Record save successfully";
                        return RedirectToAction("ForesterAction", "ForesterAction");
                    }
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// Global method for bind drop down
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
                DataTable dtr = _objModel.Get_Range_for_LoginUser();
                foreach (System.Data.DataRow dr in dtr.Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.RangeCode1 = new SelectList(Range, "Value", "Text");

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
     
        public ActionResult ViewDetails(string OffenseCode)
        {
            try
            {
                #region Details
                ViewDetails OFA = new ViewDetails();
                ForesterParivad OFA1 = new ForesterParivad();              
                DataSet ds = new DataSet();
                ds = OFA1.ViewOffenseDetails(OffenseCode);
                if (ds.Tables.Count > 0)
                {                                 
                    if (ds.Tables[0].Columns.Contains("DIST_NAME"))
                    {
                        OFA.District = ds.Tables[0].Rows[0]["DIST_NAME"].ToString();
                        OFA.District = OFA.District == null ? "N/A" : OFA.District;
                    }
                    else
                    {
                        OFA.District = OFA.District == null ? "N/A" : OFA.District;
                    }
                    if (ds.Tables[0].Columns.Contains("BLK_NAME"))
                    {
                        OFA.ForestBlock = ds.Tables[0].Rows[0]["BLK_NAME"].ToString();
                        OFA.ForestBlock = OFA.ForestBlock == null ? "N/A" : OFA.ForestBlock;
                    }
                    else
                    {
                        OFA.ForestBlock = OFA.ForestBlock == null ? "N/A" : OFA.ForestBlock;
                    }
                    if (ds.Tables[0].Columns.Contains("Place_of_offense"))
                    {
                        OFA.OffensePlace = ds.Tables[0].Rows[0]["Place_of_offense"].ToString();
                        OFA.OffensePlace = OFA.OffensePlace == null ? "N/A" : OFA.OffensePlace;
                    }
                    else
                    {
                        OFA.OffensePlace = OFA.OffensePlace == null ? "N/A" : OFA.OffensePlace;
                    }

                    if (ds.Tables[0].Columns.Contains("OffenseDate"))
                    {
                        OFA.OffenseDate = ds.Tables[0].Rows[0]["OffenseDate"].ToString();
                        OFA.OffenseDate = OFA.OffenseDate == null ? "N/A" : OFA.OffenseDate;
                    }
                    else
                    {
                        OFA.OffenseDate = OFA.OffenseDate == null ? "N/A" : OFA.OffenseDate;
                    }

                    if (ds.Tables[0].Columns.Contains("FOCategory"))
                    {
                        OFA.OffenceCategory = ds.Tables[0].Rows[0]["FOCategory"].ToString();
                        OFA.OffenceCategory = OFA.OffenceCategory == null ? "N/A" : OFA.OffenceCategory;
                    }
                    else
                    {
                        OFA.OffenceCategory = OFA.OffenceCategory == null ? "N/A" : OFA.OffenceCategory;
                    }

                    if (ds.Tables[0].Columns.Contains("Forest_Type"))
                    {
                        OFA.TypeoFForest = ds.Tables[0].Rows[0]["Forest_Type"].ToString();
                        OFA.TypeoFForest = OFA.TypeoFForest == null ? "N/A" : OFA.TypeoFForest;
                    }
                    else
                    {
                        OFA.TypeoFForest = OFA.TypeoFForest == null ? "N/A" : OFA.TypeoFForest;
                    }
                    
                    if (ds.Tables[0].Columns.Contains("Description_of_Offense"))
                    {
                        OFA.Offence_Description = ds.Tables[0].Rows[0]["Description_of_Offense"].ToString();
                        OFA.Offence_Description = OFA.Offence_Description == null ? "N/A" : OFA.Offence_Description;
                    }
                    else
                    {
                        OFA.Offence_Description = OFA.Offence_Description == null ? "N/A" : OFA.Offence_Description;
                    }
                    if (ds.Tables[0].Columns.Contains("AssignTo"))
                    {
                        OFA.AssignTo = ds.Tables[0].Rows[0]["AssignTo"].ToString();
                        OFA.AssignTo = OFA.AssignTo == null ? "N/A" : OFA.AssignTo;
                    }
                    else
                    {
                        OFA.AssignTo = OFA.AssignTo == null ? "N/A" : OFA.AssignTo;
                    }
                    if (ds.Tables[0].Columns.Contains("AssignDate"))
                    {
                        OFA.AssignDate = ds.Tables[0].Rows[0]["AssignDate"].ToString();
                        OFA.AssignDate = OFA.AssignDate == null ? "N/A" : OFA.AssignDate;
                    }
                    else
                    {
                        OFA.AssignDate = OFA.AssignDate == null ? "N/A" : OFA.AssignDate;
                    }

                    if (ds.Tables[0].Columns.Contains("Complaint_Found"))
                    {
                        OFA.Complaint_Found = ds.Tables[0].Rows[0]["Complaint_Found"].ToString();
                        OFA.Complaint_Found = OFA.Complaint_Found == null ? "N/A" : OFA.Complaint_Found;
                    }
                    else
                    {
                        OFA.Complaint_Found = OFA.Complaint_Found == null ? "N/A" : OFA.Complaint_Found;
                    }
                    if (ds.Tables[0].Columns.Contains("Mokapunchnama"))
                    {                                          
                        //OFA.Mokapunchnama = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Mokapunchnama"].ToString()) || ds.Tables[0].Rows[0]["Mokapunchnama"].ToString()=="~" ? "N/A" : "<a  href='" + @Url.Content(ds.Tables[0].Rows[0]["Mokapunchnama"].ToString()) + "' target='_blank'><img src='../images/jpeg.png' Width='30' /></a>";
                        OFA.Mokapunchnama = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Mokapunchnama"].ToString()) || ds.Tables[0].Rows[0]["Mokapunchnama"].ToString()=="~" ? "N/A" : ds.Tables[0].Rows[0]["Mokapunchnama"].ToString();
                    }
                    else
                    {
                        OFA.Mokapunchnama = "N/A";
                    }
                    if (ds.Tables[0].Columns.Contains("Najri_Naksha"))
                    {
                       // OFA.Najri_Naksha = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Najri_Naksha"].ToString()) || ds.Tables[0].Rows[0]["Najri_Naksha"].ToString()=="~" ? "N/A" : "<a  href='" + @Url.Content(ds.Tables[0].Rows[0]["Najri_Naksha"].ToString()) + "' target='_blank'><img src='../images/jpeg.png' Width='30' /></a>";
                        OFA.Najri_Naksha = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Najri_Naksha"].ToString()) || ds.Tables[0].Rows[0]["Najri_Naksha"].ToString() == "~" ? "N/A" :  ds.Tables[0].Rows[0]["Najri_Naksha"].ToString();                       
                    }
                    else
                    {
                        OFA.Najri_Naksha ="N/A";
                    }
                    if (ds.Tables[0].Columns.Contains("Witness_Recorded1"))
                    {
                      //  OFA.Witness_Recorded1 = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Witness_Recorded1"].ToString()) || ds.Tables[0].Rows[0]["Witness_Recorded1"].ToString() == "~" ? "N/A" : "<a  href='" + @Url.Content(ds.Tables[0].Rows[0]["Witness_Recorded1"].ToString()) + "' target='_blank'><img src='../images/jpeg.png' Width='30' /></a>";                                            
                       OFA.Witness_Recorded1 =string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Witness_Recorded1"].ToString()) || ds.Tables[0].Rows[0]["Witness_Recorded1"].ToString() == "~" ? "N/A" :  ds.Tables[0].Rows[0]["Witness_Recorded1"].ToString();     
                    }
                    else
                    {
                        OFA.Witness_Recorded1 ="N/A";
                    }
                    if (ds.Tables[0].Columns.Contains("Witness_Recorded2"))
                    {
                        OFA.Witness_Recorded2 = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Witness_Recorded2"].ToString()) || ds.Tables[0].Rows[0]["Witness_Recorded2"].ToString()=="~" ? "N/A" :ds.Tables[0].Rows[0]["Witness_Recorded2"].ToString();                                                                  
                   
                    }
                    else
                    {
                        OFA.Witness_Recorded2 = "N/A";
                    }
                    if (ds.Tables[0].Columns.Contains("Witness_Recorded3"))
                    {
                        OFA.Witness_Recorded3 = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Witness_Recorded3"].ToString()) || ds.Tables[0].Rows[0]["Witness_Recorded3"].ToString() == "~" ? "N/A" :ds.Tables[0].Rows[0]["Witness_Recorded3"].ToString();                                                                  
                    }
                    else
                    {
                        OFA.Witness_Recorded3 = "N/A";
                    }
                    if (ds.Tables[0].Columns.Contains("List_of_ArticalSeized"))
                    {
                        OFA.List_of_ArticalSeized = ds.Tables[0].Rows[0]["List_of_ArticalSeized"].ToString();
                        OFA.List_of_ArticalSeized = OFA.List_of_ArticalSeized == null ? "N/A" : OFA.List_of_ArticalSeized;
                    }
                    else
                    {
                        OFA.List_of_ArticalSeized = OFA.List_of_ArticalSeized == null ? "N/A" : OFA.List_of_ArticalSeized;
                    }
                    if (ds.Tables[0].Columns.Contains("Recommendation"))
                    {
                        OFA.Recommendation = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Recommendation"].ToString()) || ds.Tables[0].Rows[0]["Recommendation"].ToString()=="~" ? "N/A" : ds.Tables[0].Rows[0]["Recommendation"].ToString();                                                                                          
                    }
                    else
                    {
                        OFA.Recommendation = "N/A";
                    }
                    if (ds.Tables[0].Columns.Contains("FieldInspection"))
                    {
                        OFA.FieldInspection = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FieldInspection"].ToString()) || ds.Tables[0].Rows[0]["FieldInspection"].ToString()=="~" ? "N/A" :ds.Tables[0].Rows[0]["FieldInspection"].ToString();                                                                                        
                    }
                    else
                    {
                        OFA.FieldInspection ="N/A";
                    }
                    if (ds.Tables[0].Columns.Contains("InvestigationCompleteDate"))
                    {
                        OFA.InvestigationCompleteDate = ds.Tables[0].Rows[0]["InvestigationCompleteDate"].ToString();
                        OFA.InvestigationCompleteDate = OFA.InvestigationCompleteDate == null ? "N/A" : OFA.InvestigationCompleteDate;
                    }
                    else
                    {
                        OFA.InvestigationCompleteDate = OFA.InvestigationCompleteDate == null ? "N/A" : OFA.InvestigationCompleteDate;
                    }
                    if (ds.Tables[0].Columns.Contains("DispatchNo"))
                    {
                        OFA.DispatchNo = ds.Tables[0].Rows[0]["DispatchNo"].ToString();
                        OFA.DispatchNo = OFA.DispatchNo == null ? "N/A" : OFA.DispatchNo;
                    }
                    else
                    {
                        OFA.DispatchNo = OFA.DispatchNo == null ? "N/A" : OFA.DispatchNo;
                    }
                    if (ds.Tables[0].Columns.Contains("VehicleSeized"))
                    {
                        OFA.VehicleSeized = ds.Tables[0].Rows[0]["VehicleSeized"].ToString();
                        OFA.VehicleSeized = OFA.VehicleSeized == null ? "N/A" : OFA.VehicleSeized;
                    }
                    else
                    {
                        OFA.VehicleSeized = OFA.VehicleSeized == null ? "N/A" : OFA.VehicleSeized;
                    }

                    if (OFA.VehicleSeized == "Yes") {
                        if (ds.Tables[0].Columns.Contains("CategoryName"))
                        {
                            OFA.VechileType = ds.Tables[0].Rows[0]["CategoryName"].ToString();
                            OFA.VechileType = OFA.VechileType == null ? "N/A" : OFA.VechileType;
                        }
                        else
                        {
                            OFA.VechileType = OFA.VechileType == null ? "N/A" : OFA.VechileType;
                        }
                        if (ds.Tables[0].Columns.Contains("VehicleRegistrationNo"))
                        {
                            OFA.VechileRegistrationNo = ds.Tables[0].Rows[0]["VehicleRegistrationNo"].ToString();
                            OFA.VechileRegistrationNo = OFA.VechileRegistrationNo == null ? "N/A" : OFA.VechileRegistrationNo;
                        }
                        else
                        {
                            OFA.VechileRegistrationNo = OFA.VechileRegistrationNo == null ? "N/A" : OFA.VechileRegistrationNo;
                        }
                        if (ds.Tables[0].Columns.Contains("VehicleMake"))
                        {
                            OFA.VechileMake = ds.Tables[0].Rows[0]["VehicleMake"].ToString();
                            OFA.VechileMake = OFA.VechileMake == null ? "N/A" : OFA.VechileMake;
                        }
                        else
                        {
                            OFA.VechileMake = OFA.VechileMake == null ? "N/A" : OFA.VechileMake;
                        }
                        if (ds.Tables[0].Columns.Contains("VehicleModel"))
                        {
                            OFA.VechileModel = ds.Tables[0].Rows[0]["VehicleModel"].ToString();
                            OFA.VechileModel = OFA.VechileModel == null ? "N/A" : OFA.VechileModel;
                        }
                        else
                        {
                            OFA.VechileModel = OFA.VechileModel == null ? "N/A" : OFA.VechileModel;
                        }
                        if (ds.Tables[0].Columns.Contains("ChassisNo"))
                        {
                            OFA.VechileChassisNo = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
                            OFA.VechileChassisNo = OFA.VechileChassisNo == null ? "N/A" : OFA.VechileChassisNo;
                        }
                        else
                        {
                            OFA.VechileChassisNo = OFA.VechileChassisNo == null ? "N/A" : OFA.VechileChassisNo;
                        }
                        if (ds.Tables[0].Columns.Contains("EngineNo"))
                        {
                            OFA.VechileEngineNo = ds.Tables[0].Rows[0]["EngineNo"].ToString();
                            OFA.VechileEngineNo = OFA.VechileEngineNo == null ? "N/A" : OFA.VechileEngineNo;
                        }
                        else
                        {
                            OFA.VechileEngineNo = OFA.VechileEngineNo == null ? "N/A" : OFA.VechileEngineNo;
                        } 
                    }
                   
                }
                var OffenderPartialView2 = RenderRazorViewToString(this.ControllerContext, "OffenseDetail2", OFA);
                //var VehiclePartialView = RenderRazorViewToString(this.ControllerContext, "ZooVehicleInfo", lstVehicle);
                var json = Json(new { OffenderPartialView2 });
                return json;
                #endregion
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            return null;
       }


        /// <summary>
        /// Function for getting details of offense and offender on partial view
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        public ActionResult GetParivadeDetails(string OffenseCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ForesterParivad FP = new ForesterParivad();
            List<ForesterParivad> lstDetails = new List<ForesterParivad>();
            List<CitizenParivad> lstVehicalDtl = new List<CitizenParivad>();
            List<CitizenParivad> lstOffenderDtl = new List<CitizenParivad>();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            try {
                FP.OffenseCode = OffenseCode;
                DataSet DS = FP.GetParivadeDetails(FP);
                DT1 = DS.Tables[0];
                DT2 = DS.Tables[1];
                DT3 = DS.Tables[2];

                if (DT3.Rows.Count > 0)
                {
                    for (int i = 0; i < DT3.Rows.Count; i++)
                    {
                        lstVehicalDtl.Add(new CitizenParivad
                        {
                            VechileRegistrationNo = DT3.Rows[i]["VehicleRegistrationNo"].ToString(),
                            VechileOwnerName = DT3.Rows[i]["OwnerName"].ToString(),
                            VechileType = DT3.Rows[i]["Name"].ToString(),
                            VechileMake = DT3.Rows[i]["VehicleMake"].ToString(),
                            VechileModel = DT3.Rows[i]["VehicleModel"].ToString(),
                            VechileChassisNo = DT3.Rows[i]["ChassisNo"].ToString(),
                            VechileEngineNo = DT3.Rows[i]["EngineNo"].ToString(),

                        });
                    }
                }
                ViewData["lstVehicle"] = lstVehicalDtl;
                if (DT2.Rows.Count > 0)
                {
                    for (int i = 0; i < DT2.Rows.Count; i++)
                    {
                        lstOffenderDtl.Add(new CitizenParivad
                        {
                            OffenderName = DT2.Rows[i]["Name"].ToString(),
                            OFatherName = DT2.Rows[i]["Father_Name"].ToString(),
                            OAddress1 = DT2.Rows[i]["Address"].ToString(),
                            OffenderStatement = DT2.Rows[i]["Statement"].ToString(),
                            //VechileModel = DT2.Rows[0]["Statementdocument"].ToString(),
                        });
                    }
                }

                ViewData["lstOffender"] = lstOffenderDtl;
                if (DT1.Rows.Count > 0)
                {
                    lstDetails.Add(new ForesterParivad
                    {

                        OffenseCode = DT1.Rows[0]["Offense_code"].ToString(),
                        ApplicantName = DT1.Rows[0]["ApplicantName"].ToString(),
                        Circle = DT1.Rows[0]["CIRCLE_NAME"].ToString(),
                        Division = DT1.Rows[0]["DIV_NAME"].ToString(),
                        District = DT1.Rows[0]["DIST_NAME"].ToString(),
                        ForestBlock = DT1.Rows[0]["BLK_NAME"].ToString(),
                        GP_Name = DT1.Rows[0]["GP_NAME"].ToString(),
                        Vill_Name = DT1.Rows[0]["VILL_NAME"].ToString(),
                        Range_Name = DT1.Rows[0]["RANGE_NAME"].ToString(),
                        Tehsil = DT1.Rows[0]["Tehsil"].ToString(),
                        Naka = DT1.Rows[0]["Naka"].ToString(),
                        Beat = DT1.Rows[0]["Beat"].ToString(),
                        OffensePlace = DT1.Rows[0]["Place_of_offense"].ToString(),
                        OffenseDate = DT1.Rows[0]["Date_of_offense"].ToString(),
                        OffenseTime = DT1.Rows[0]["Time_of_offense"].ToString(),
                        OffenceCategory = DT1.Rows[0]["FOCategory"].ToString(),
                        TypeoFForest = DT1.Rows[0]["Forest_Type"].ToString(),
                        Offence_Description = DT1.Rows[0]["Description_of_Offense"].ToString(),
                        No_of_offender = Convert.ToInt32(DT1.Rows[0]["No_of_offender"].ToString()),
                        Description_of_offenders = DT1.Rows[0]["Description_of_offenders"].ToString(),
                        ComplaintOnBhalfOf = DT1.Rows[0]["Applicantname1"].ToString(),
                        AssignTo = DT1.Rows[0]["AssignTo"].ToString(),
                        OffenseSeverity = DT1.Rows[0]["Offense_Severity"].ToString(),
                        WildlifeProtectionSection = DT1.Rows[0]["Wildlife_Protection_Act"].ToString(),
                        OffenseSubCategoryWildLife = DT1.Rows[0]["Wildlife_Sub_Section"].ToString(),
                        ForestProtectionSection = DT1.Rows[0]["Forest_Protection_Act"].ToString(),
                        OffenseSubCategoryForest = DT1.Rows[0]["Forest_Sub_Section"].ToString(),
                        VisitDate = DT1.Rows[0]["VisitDate"].ToString(),
                        VisitPlace = DT1.Rows[0]["VisitPlace"].ToString(),
                        Complaint_Found = DT1.Rows[0]["Complaint_Found"].ToString(),
                        Mokapunchnama = string.IsNullOrEmpty(DT1.Rows[0]["Mokapunchnama"].ToString()) || DT1.Rows[0]["Mokapunchnama"].ToString() == "~" ? "N/A" : DT1.Rows[0]["Mokapunchnama"].ToString(),
                        Najri_Naksha = string.IsNullOrEmpty(DT1.Rows[0]["Najri_Naksha"].ToString()) || DT1.Rows[0]["Najri_Naksha"].ToString() == "~" ? "N/A" : DT1.Rows[0]["Najri_Naksha"].ToString(),
                        Witness_Recorded1 = string.IsNullOrEmpty(DT1.Rows[0]["Witness_Recorded1"].ToString()) || DT1.Rows[0]["Witness_Recorded1"].ToString() == "~" ? "N/A" : DT1.Rows[0]["Witness_Recorded1"].ToString(),
                        Witness_Recorded2 = string.IsNullOrEmpty(DT1.Rows[0]["Witness_Recorded2"].ToString()) || DT1.Rows[0]["Witness_Recorded2"].ToString() == "~" ? "N/A" : DT1.Rows[0]["Witness_Recorded2"].ToString(),
                        Witness_Recorded3 = string.IsNullOrEmpty(DT1.Rows[0]["Witness_Recorded3"].ToString()) || DT1.Rows[0]["Witness_Recorded3"].ToString() == "~" ? "N/A" : DT1.Rows[0]["Witness_Recorded3"].ToString(),
                        List_of_ArticalSeized = DT1.Rows[0]["List_of_ArticalSeized"].ToString(),
                        Recommendation = string.IsNullOrEmpty(DT1.Rows[0]["Recommendation"].ToString()) || DT1.Rows[0]["Recommendation"].ToString() == "~" ? "N/A" : DT1.Rows[0]["Recommendation"].ToString(),
                        FieldInspection = string.IsNullOrEmpty(DT1.Rows[0]["FieldInspection"].ToString()) || DT1.Rows[0]["FieldInspection"].ToString() == "~" ? "N/A" : DT1.Rows[0]["FieldInspection"].ToString(),
                        VehicleSeized = DT1.Rows[0]["VehicleSeized"].ToString(),
                        InvestigationCompleteDate = DT1.Rows[0]["InvestigationCompleteDate"].ToString(),
                        DispatchNo = DT1.Rows[0]["DispatchNo"].ToString(),
                    });
                }
                ViewData["lstOffenseDetails"] = lstDetails;
                var OffenderPartialView = RenderRazorViewToString(this.ControllerContext, "OffenseDetail1", lstDetails);
                var OffenderListPartialView = RenderRazorViewToString(this.ControllerContext, "OffenderListInfo", ViewData["lstOffender"]);
                // var VehicleListPartialView = RenderRazorViewToString(this.ControllerContext, "VehicleListInfo", lstVehicalDtl);
                var json = Json(new { OffenderPartialView, OffenderListPartialView });
                return json;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        
        }


        /// <summary>
        /// Return entire partial view 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static String RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model)
        {           
            controllerContext.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Final submission of case
        /// </summary>
        /// <param name="CaseAction"></param>
        /// <param name="OffenseCode"></param>
        /// <param name="Remarks"></param>
        /// <returns></returns>

        public ActionResult SubmitCaseAction(string CaseAction, string OffenseCode, string Remarks)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ForesterParivad FP = new ForesterParivad();
            Int32 status=0;
            try
            {
                if (CaseAction != "" && OffenseCode != "")
                {
                    status = FP.SubmitCaseAction(CaseAction, Remarks, OffenseCode);
                }
                if (status > 0)
                {
                    return Json("Submit");
                }
                else
                {
                    return Json("NotSubmit");
                }
            }             
           catch (Exception ex)
           {
               new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
           }
           return null;
        }

        /// <summary>
        /// Get compounding list details
        /// </summary>
        /// <returns></returns>
        public ActionResult CompoundingDetails()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ForesterParivad FP = new ForesterParivad();
            DataSet dsComp = new DataSet();
            List<ForesterParivad> lstCompounding = new List<ForesterParivad>();
            List<SelectListItem> budgetheadlst = new List<SelectListItem>();
            DataSet dsBudgetHead = new DataSet();
            try
            {
                dsComp = FP.GetCompoundDetails();
                if (dsComp.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dr in dsComp.Tables[0].Rows)
                    {
                        lstCompounding.Add(new ForesterParivad
                        {

                            OffenseCode = dr["Offense_code"].ToString(),
                            ApplicantName = dr["ApplicantName"].ToString(),
                            OffensePlace = dr["Place"].ToString(),
                            OffenseDate = dr["OffenseDate"].ToString(),
                            ComplaintFound = dr["Complaint_Found"].ToString(),
                            OffenseStatus = dr["Satus"].ToString(),
                        });
                    }
                }

                dsBudgetHead = FP.BindBudget();
                foreach (System.Data.DataRow dr in dsBudgetHead.Tables[0].Rows)
                {
                    budgetheadlst.Add(new SelectListItem { Text = @dr["BudgetHead"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.Budget = budgetheadlst;

                ViewBag.Compoundlist = lstCompounding;
                return View();
            }            
           catch (Exception ex)
           {
               new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
           }
           return null;
        }

    /// <summary>
    /// Final submission of compounding details
    /// </summary>
    /// <param name="frm"></param>
    /// <returns></returns>
       [HttpPost]
        public ActionResult SubmitCompoundingDetails(FormCollection frm)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ForesterParivad FP = new ForesterParivad();
            try { 
            if (Convert.ToString(frm["txtAmount"]) != null && Convert.ToString(frm["txtAmount"]) !="") {

                FP.CompoundAmount = Convert.ToString(frm["txtAmount"]);
            }
            if (Convert.ToString(frm["txtReceipt"]) != null && Convert.ToString(frm["txtReceipt"]) != "")
            {
                FP.CompoundReceipt = Convert.ToString(frm["txtReceipt"]);
            }
            if (Convert.ToString(frm["txtDate"]) != null && Convert.ToString(frm["txtDate"]) != "")
            {
                FP.CompoundDate = Convert.ToString(frm["txtDate"]);
            }
            if (Convert.ToString(frm["ddlBudgethead"]) != null && Convert.ToString(frm["ddlBudgethead"]) != "")
            {
                FP.CompoundBudgetHead = Convert.ToString(frm["ddlBudgethead"]);
            }
            if (Convert.ToString(frm["hdOffenseCode"]) != null && Convert.ToString(frm["hdOffenseCode"]) != "")
            {
                FP.OffenseCode = Convert.ToString(frm["hdOffenseCode"]);
            }
            if (Convert.ToString(frm["txtChallanNo"]) != null && Convert.ToString(frm["txtChallanNo"]) != "")
            {
                FP.ChallanNo = Convert.ToString(frm["txtChallanNo"]);
            }
            if (Convert.ToString(frm["txtBank"]) != null && Convert.ToString(frm["txtBank"]) != "")
            {
                FP.BankName = Convert.ToString(frm["txtBank"]);
            } 
              
            Int32 status=FP.SubmitCompounding(FP);
            if (status > 0)
            {             
                return RedirectToAction("CompoundingDetails", "ForesterParivadRegistration");
            }
            else {             
                return RedirectToAction("CompoundingDetails", "ForesterParivadRegistration");
            }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return null;

        }

        /// <summary>
        /// Getting list file court case
        /// </summary>
        ///        
       //public ActionResult FileCourtCase(string OffenseCode)        
       //{

       //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
       //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
       //    try
       //    {
       //        List<ForesterParivad> OffenceFinal = new List<ForesterParivad>();
       //        ForesterParivad objReg = new ForesterParivad();
       //        DataSet DT = new DataSet();
       //        objReg.OffenseCode = Encryption.decrypt(OffenseCode);
       //        DT = objReg.GetCourtCaseDetails("3");
       //        foreach (DataRow dr in DT.Tables[0].Rows)
       //            OffenceFinal.Add(
       //                new ForesterParivad()
       //                {
       //                    OffenseCode = dr["OffenseCode"].ToString(),
       //                    CourtName = dr["CourtName"].ToString(),
       //                    CourtCaseNo = dr["CourtCaseNo"].ToString(),
       //                    CourtType = dr["CourtType"].ToString(),
       //                    ProsecutionDate = dr["ProsecutionDate"].ToString(),
       //                    DecisionTaken = dr["DecisionTaken"].ToString(),
       //                    DateOfDecisionTaken = dr["DateOfDecisionTaken"].ToString(),
       //                    ReasonOfCaseFailed = dr["ReasonOfCaseFailed"].ToString(),
       //                });
       //        ViewData["CourtCaseDetail"] = OffenceFinal;
       //        TempData["OffenseCode"] = Encryption.decrypt(OffenseCode);
       //        return View("FileCourtCase");
       //    }
       //    catch (Exception ex)
       //    {
       //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
       //    }
       //    return null;
       // }


       public ActionResult FileCourtCase(string OffenseCode)
       {

           string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
           string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

           List<SelectListItem> DecisionTaken = new List<SelectListItem>();
           List<SelectListItem> ReasonOfCaseFailed = new List<SelectListItem>();
           DataTable dtfDec = new OffenseReg().Get_DecisionTaken();
           foreach (System.Data.DataRow dr in dtfDec.Rows)
           {
               DecisionTaken.Add(new SelectListItem { Text = @dr["DName"].ToString(), Value = @dr["DID"].ToString() });
           }
           ViewBag.DecisionTaken = new SelectList(DecisionTaken, "Value", "Text");

           DataTable dtfRsn = new OffenseReg().Get_ReasonOfCaseFailed();
           foreach (System.Data.DataRow dr in dtfRsn.Rows)
           {
               ReasonOfCaseFailed.Add(new SelectListItem { Text = @dr["RName"].ToString(), Value = @dr["RID"].ToString() });
           }
           ViewBag.ReasonOfCaseFailed = new SelectList(ReasonOfCaseFailed, "Value", "Text");
           try
           {
               List<ForesterParivad> OffenceFinal = new List<ForesterParivad>();
               ForesterParivad objReg = new ForesterParivad();
               DataSet DT = new DataSet();
               objReg.OffenseCode = Encryption.decrypt(OffenseCode);
               DT = objReg.GetCourtCaseDetails("3");
               foreach (DataRow dr in DT.Tables[0].Rows)
                   OffenceFinal.Add(
                       new ForesterParivad()
                       {
                           OffenseCode = dr["OffenseCode"].ToString(),
                           CourtName = dr["CourtName"].ToString(),
                           CourtCaseNo = dr["CourtCaseNo"].ToString(),
                           CourtType = dr["CourtType"].ToString(),
                           ProsecutionDate = dr["ProsecutionDate"].ToString(),
                           DecisionTaken = dr["DecisionTaken"].ToString(),
                           DateOfDecisionTaken = dr["DateOfDecisionTaken"].ToString(),
                           ReasonOfCaseFailed = dr["ReasonOfCaseFailed"].ToString(),
                       });
               ViewData["CourtCaseDetail"] = OffenceFinal;
               TempData["OffenseCode"] = Encryption.decrypt(OffenseCode);
               return View("FileCourtCase");
           }
           catch (Exception ex)
           {
               new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
           }
           return null;
       }





       /// <summary>
       /// Get List of file court case
       /// </summary>
       /// <returns></returns>
       public ActionResult CourtCaselist()
       {
          string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
          string  controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
          List<ForesterParivad> Offenselst = new List<ForesterParivad>();
           try
           {
               DataSet dsOffense = new DataSet();
               ForesterParivad orf = new ForesterParivad();
               dsOffense = orf.GetCourtCaseDetails("1");
               foreach (System.Data.DataRow dr in dsOffense.Tables[0].Rows)
               {
                   Offenselst.Add(new ForesterParivad
                   {
                       OffenseCode = dr["OffenseCode"].ToString(),
                       OffenseDate = dr["OffenseDate"].ToString(),
                       OffensePlace = dr["OffensePlace"].ToString(),
                       District = dr["DIST_NAME"].ToString(),
                       ComplaintFound = dr["Complaint_Found"].ToString(),
                   });
               }
               ViewData["CourtCaseDetail"] = Offenselst;
           }
           catch (Exception ex)
           {
               new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
           }
           return View();
       }
       /// <summary>
       /// Submit file court case details
       /// </summary>
       /// <param name="ObjCourtCaseItem"></param>
       /// <param name="frm"></param>
       /// <param name="InterimOrder"></param>
       /// <param name="FinalJudgmentOrder"></param>
       /// <returns></returns>
       [HttpPost]
       public ActionResult SubmitFileCourtCase(ForesterParivad ObjCourtCaseItem, FormCollection frm, HttpPostedFileBase InterimOrder, HttpPostedFileBase FinalJudgmentOrder)
       {
           ForesterParivad OR = new ForesterParivad();
           string Document = string.Empty;
           var path = "";
           string Document1 = string.Empty;
           var path1 = "";
          string  actionName = this.ControllerContext.RouteData.Values["action"].ToString();
          string  controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
          var OffenseCode = string.Empty;
           try
           {
               if (InterimOrder != null && InterimOrder.ContentLength > 0)
               {
                   Document = Path.GetFileName(InterimOrder.FileName);
                   String FileFullName = DateTime.Now.Ticks + "_" + Document;
                   path = Path.Combine(Server.MapPath("~/ForestProtectionDocument/"), FileFullName);
                   ObjCourtCaseItem.InterimOrder = path;
                   InterimOrder.SaveAs(Server.MapPath("~/ForestProtectionDocument/") + FileFullName);
               }
               else
               {
                   ObjCourtCaseItem.InterimOrder = "";
               }
               if (FinalJudgmentOrder != null && FinalJudgmentOrder.ContentLength > 0)
               {
                   Document1 = Path.GetFileName(FinalJudgmentOrder.FileName);
                   String FileFullName = DateTime.Now.Ticks + "_" + Document1;
                   path1 = Path.Combine(Server.MapPath("~/ForestProtectionDocument/"), FileFullName);
                   ObjCourtCaseItem.FinalJudgmentOrder = path1;
                   FinalJudgmentOrder.SaveAs(Server.MapPath("~/ForestProtectionDocument/") + FileFullName);
               }
               else
               {
                   ObjCourtCaseItem.FinalJudgmentOrder = "";
               }
               if (frm["CaseId"].ToString() == "")
               {
                   ObjCourtCaseItem.CaseId = Convert.ToInt64("0");
               }
               else
               {
                   ObjCourtCaseItem.CaseId = Convert.ToInt64(frm["CaseId"].ToString());
               }
               if (frm["txtOffenseCode"] != null && frm["txtOffenseCode"] != "")
               {
                   ObjCourtCaseItem.OffenseCode = frm["txtOffenseCode"].ToString();
                   OffenseCode= Encryption.encrypt(ObjCourtCaseItem.OffenseCode);
               }
               Int64 status = ObjCourtCaseItem.InsertFileCourtCase();
               if (status != 0)
               {
                   TempData["status"] = "Record save successfully";                 
               }
               else
               {
                   TempData["status"] = "Court Case detail already exists";                
               }             
           }
           catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }

           return RedirectToAction("FileCourtCase", "ForesterParivadRegistration", new { OffenseCode });
          // return View("FileCourtCase", OR);
       }

/// <summary>
/// For FIR print
/// </summary>
/// <param name="OffenseCode"></param>
/// <returns></returns>

       public ActionResult PdfFIR(string OffenseCode)
       {
           ForesterParivad FP = new ForesterParivad();
           List<ForesterParivad> lstDetails = new List<ForesterParivad>();
           List<CitizenParivad> lstVehicalDtl = new List<CitizenParivad>();
           List<CitizenParivad> lstOffenderDtl = new List<CitizenParivad>();
           DataTable DT1 = new DataTable();
           DataTable DT2 = new DataTable();
           DataTable DT3 = new DataTable();
           FP.OffenseCode = OffenseCode;
           DataSet DS = FP.GetParivadeDetails(FP);
           DT1 = DS.Tables[0];
           DT2 = DS.Tables[1];
           DT3 = DS.Tables[2];

           //DataTable dt = new DataTable();
           //ActionRequest ar = new ActionRequest();
           //DataSet DS = new DataSet();
           //Status = "FIR";
           //DS = ar.GetFIRDetails(RequestId, Status, TableName);

           //dt = ar.GetFIRDetails(RequestId, Status, TableName);
           string filepath = string.Empty;
           filepath = "~/ForestProtectionDocument/FIR_" + DateTime.Now.Ticks.ToString() + ".pdf";
           Document doc = new Document(PageSize.A4, 50, 50f, 50f, 50f);
           PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
           var FontColour = new BaseColor(0, 0, 0);
           Paragraph tableheading = null;
           var MyFontU = FontFactory.GetFont("Arial", 12, FontColour);
           var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
           var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
           var MyFont1 = FontFactory.GetFont("Arial", 10, FontColour);
           PdfPCell cell;
           Phrase colHeading;
           PdfPTable pdfTable = null;
           doc.Open();
           doc.NewPage();
           tableheading = new Paragraph("Government of Rajasthan,Forest Department", MyFont);
           tableheading.Font.Size = 13;
           tableheading.Alignment = (Element.ALIGN_CENTER);
           doc.Add(tableheading);

           tableheading = new Paragraph("First Information Report", MyFont);
           tableheading.Font.Size = 12;
           tableheading.Alignment = (Element.ALIGN_CENTER);
           doc.Add(tableheading);

           doc.Add(new Paragraph(Environment.NewLine));
           doc.Add(new Paragraph(Environment.NewLine));
           tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_RIGHT);
           doc.Add(tableheading);
           DataTable DT0 = DS.Tables[0];

           tableheading = new Paragraph("S.No. of FIR: :" + OffenseCode, MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);


           //tableheading = new Paragraph("Date of FIR: :" + DT0.Rows[0]["FIRDATE"].ToString(), MyFont);
           //tableheading.Font.Size = 10;
           //tableheading.Alignment = (Element.ALIGN_LEFT);
           //doc.Add(tableheading);

           tableheading = new Paragraph("Place of Offense :" + DT0.Rows[0]["Place_of_offense"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);

           tableheading = new Paragraph("Date of Offense :  " + DT0.Rows[0]["Date_of_offense"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);
           tableheading = new Paragraph("Time of Offense :  " + DT0.Rows[0]["Time_of_offense"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);
           tableheading = new Paragraph("Description of Offense :  " + DT0.Rows[0]["Description_of_Offense"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);

           tableheading = new Paragraph("Category of Offense :  " + DT0.Rows[0]["FOCategory"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);

           tableheading = new Paragraph("Type of Offense :  " + DT0.Rows[0]["Forest_Type"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);
           tableheading = new Paragraph("Investigating Officer :  " + DT0.Rows[0]["AssignTo"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);

           tableheading = new Paragraph("Offense Severity  :  " + DT0.Rows[0]["Offense_Severity"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);


           tableheading = new Paragraph("Forest Protection Act  :  " + DT0.Rows[0]["Forest_Protection_Act"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);
           tableheading = new Paragraph("Forest Protection Sub Act  :  " + DT0.Rows[0]["Forest_Sub_Section"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);

           tableheading = new Paragraph("Wildlife Protection Act  :  " + DT0.Rows[0]["Wildlife_Protection_Act"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);
           tableheading = new Paragraph("Wildlife Protection Sub Act  :  " + DT0.Rows[0]["Wildlife_Sub_Section"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);

           tableheading = new Paragraph("Investigating Officer Visit Date :  " + DT0.Rows[0]["VisitDate"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);
           tableheading = new Paragraph("Investigating Officer Visit Place  :  " + DT0.Rows[0]["VisitPlace"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);


           tableheading = new Paragraph("Complaint Found :  " + DT0.Rows[0]["Complaint_Found"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);

           tableheading = new Paragraph("List of Articles seized :  " + DT0.Rows[0]["List_of_ArticalSeized"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);

           tableheading = new Paragraph("Investigation Completion Date :  " + DT0.Rows[0]["InvestigationCompleteDate"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);


           tableheading = new Paragraph("Dispatch no :  " + DT0.Rows[0]["DispatchNo"].ToString(), MyFont);
           tableheading.Font.Size = 10;
           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);


           tableheading = new Paragraph("Offender Detail :- ");
           tableheading.Font.Size = 10;


           tableheading.Alignment = (Element.ALIGN_LEFT);
           doc.Add(tableheading);
           doc.Add(new Paragraph(Environment.NewLine));
  
           DT2 = DS.Tables[1];
           int count2 = DT2.Rows.Count;
           pdfTable = new PdfPTable(4);
           pdfTable.DefaultCell.Padding = 1;
           pdfTable.WidthPercentage = 95;
           pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

           if (count2 > 0)
           {


               string[,] arrPdfData = new string[count2, 4];
               arrPdfData[0, 0] = "Offender Name";
               arrPdfData[0, 1] = "Father Name";
               arrPdfData[0, 2] = "Address";
               arrPdfData[0, 3] = "Statement";
 
               pdfTable.AddCell(new PdfPCell(new Paragraph("Name", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("Father_Name", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("Address", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("Statement", MyFont1)));
 
               for (int xid = 0; xid < count2; xid++)
               {
                   for (int yid = 0; yid <4; yid++)
                   {
                       arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                       colHeading = new Phrase(arrPdfData[xid, yid]);
                       colHeading.Font.Size = 9;
                       cell = new PdfPCell(new Phrase(colHeading));
                       cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                       cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                       pdfTable.AddCell(cell);
                   }
               }
               doc.Add(pdfTable);
           }

     



    
    
           int count = DT3.Rows.Count;
           pdfTable = new PdfPTable(7);
           pdfTable.DefaultCell.Padding = 1;
           pdfTable.WidthPercentage = 95;
           pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
           if (count > 0)
           {
               tableheading = new Paragraph("Seized Vehicle Detail  :- ");
               tableheading.Font.Size = 10;
               tableheading.Font.IsUnderlined();
               tableheading.Alignment = (Element.ALIGN_LEFT);
               doc.Add(tableheading);
               doc.Add(new Paragraph(Environment.NewLine));

               string[,] arrPdfData = new string[count, 7];
               arrPdfData[0, 0] = "Registration No";
               arrPdfData[0, 1] = "Owner Name";
               arrPdfData[0, 2] = "Name";
               arrPdfData[0, 3] = "VehicleMake";
               arrPdfData[0, 4] = "VehicleModel";
               arrPdfData[0, 5] = "ChassisNo";
               arrPdfData[0, 6] = "EngineNo";
               pdfTable.AddCell(new PdfPCell(new Paragraph("Registration No", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("Owner Name", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("Name", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("VehicleMake", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("VehicleModel", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("ChassisNo", MyFont1)));
               pdfTable.AddCell(new PdfPCell(new Paragraph("EngineNo", MyFont1)));
               for (int xid = 0; xid < count; xid++)
               {
                   for (int yid = 0; yid < 7; yid++)
                   {
                       arrPdfData[xid, yid] = DT3.Rows[xid][yid].ToString();
                       colHeading = new Phrase(arrPdfData[xid, yid]);
                       colHeading.Font.Size = 9;
                       cell = new PdfPCell(new Phrase(colHeading));
                       cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                       cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                       pdfTable.AddCell(cell);
                   }
               }
               doc.Add(pdfTable);

           }




            
           doc.Add(new Paragraph(Environment.NewLine));
           doc.Add(new Paragraph(Environment.NewLine));
           tableheading = new Paragraph("Signature of officer");
           tableheading.Font.Size = 10;
           tableheading.Font.IsUnderlined();
           tableheading.Alignment = (Element.ALIGN_RIGHT);
           doc.Add(tableheading);

           doc.Close();
           if (System.IO.File.Exists(Server.MapPath(filepath)))
           {
               string FilePath = Server.MapPath(filepath);
               WebClient User = new WebClient();
               Byte[] FileBuffer = User.DownloadData(FilePath);
               if (FileBuffer != null)
               {
                   Response.ContentType = "application/pdf";
                   Response.AddHeader("content-length", FileBuffer.Length.ToString());
                   Response.BinaryWrite(FileBuffer);
                   Response.End();
               }
           }

           return RedirectToAction("OffenseDetails", "ForesterParivadRegistration");
       }

    }
}
