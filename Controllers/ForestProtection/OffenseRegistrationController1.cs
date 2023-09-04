//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : OffenseRegistrationController
//  Description  : File contains calling functions from UI
//  Date Created : 28-Apr-2016
//  History      : Add the Dist the Forestor registration and Offense Category
//  Version      : 1.0
//  Author       : Gaurav Pandey
//  Modified By  : Gaurav Pandey
//  History      : Add the edit Functionlity for Citizen user and Forestor for All Forms Steps
//  Modified On  : 13-Apr-2016
//  Reviewed By  : 
//  Reviewed On  : 
//  Bug No-466,471
//********************************************************************************************************

using FMDSS.App_Start;
using FMDSS.Models;
using FMDSS.Models.ForestProtection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using FMDSS.Filters;
using System.Linq;

namespace FMDSS.Controllers.ForestProtection
{
    
    [MyAuthorization]
    public class OffenseRegistrationController : Controller
    {
        // 
        // GET: /OffenseRegistration/
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
        #region "Declare the Page level Variables"
        int ModuleID = 4;      
        OffenseReg _objModel = new OffenseReg();
        #endregion

        /// <summary>
        /// Page load Events
        /// </summary>
        /// <param name="_objModel"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public ActionResult OffenseRegistration(OffenseReg _objModel, string Mode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {               
                    if (Encryption.decrypt(Mode) == "Create")
                    {
                        Session["FPMuserMode"] = Encryption.decrypt(Mode);
                        Session["FPMUserRole"] = null;
                        Session["FPMOffenseCode"] = null;
                        Session["FPMOffenseID"] = null;                        
                    }
                    else
                    {
                        Session["FPMuserMode"] = null;
                    }
                    if (_objModel.RegFormNumber == 0)
                    {
                        _objModel.RegFormNumber = 1;
                    }                                  
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
        /// Use for Save the details of Form 1 (Forestor Ragister Parivad)
        /// </summary>
        /// <param name="fcollection"></param>
        /// <param name="Model"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        public ActionResult SubmitDetails(FormCollection fcollection, OffenseReg Model, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {               
                    _objModel.UserID = UserID;
                    if (Session["FPMOffenseCode"] != null)
                    {
                        getdataview(); 
                        int existRecordCount = Convert.ToInt32(new OffenseReg().GetAllRecordsForm1(Session["FPMOffenseCode"].ToString(), "SelectForm1").Tables[0].Rows[0]["RowsCount"].ToString());
                        if (existRecordCount > 0)
                        {
                            _objModel.IsEditMode = true;
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
                            _objModel.OffenseDate = string.IsNullOrEmpty(Model.OffenseDate) ? "" : Model.OffenseDate;
                            _objModel.OffenseTime = string.IsNullOrEmpty(Model.OffenseTime) ? "" : Model.OffenseTime;
                            _objModel.LandMark = string.IsNullOrEmpty(Model.LandMark) ? "" : Model.LandMark;
                            _objModel.NakaDistance = string.IsNullOrEmpty(Model.NakaDistance) ? "" : Model.NakaDistance;
                            _objModel.ForestType = Model.ForestType;
                            _objModel.OffenceCategory = string.IsNullOrEmpty(Model.OffenceCategory) ? "" : Model.OffenceCategory;
                            _objModel.Offence_Description = string.IsNullOrEmpty(Model.Offence_Description) ? "" : Model.Offence_Description;
                            _objModel.ApplicantName = string.IsNullOrEmpty(Model.ApplicantName) ? "" : Model.ApplicantName;        
                            _objModel.OffenseCode = Session["FPMOffenseCode"].ToString();
                            _objModel.UserRole = "FORESTER";
                            Session["FPMUserRole"] = _objModel.UserRole;
                            string id = _objModel.UpdateForm1(_objModel);
                            if (id != "0")
                            {
                                FormCollection fm = null;
                                SubmitDetails2(fm, _objModel, "Previous2");
                                //DDLList();
                                return View("OffenseRegistration", _objModel);
                            }
                        }
                    }
                    else
                    {
                        _objModel.IsEditMode = false;
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
                        _objModel.UserRole = "FORESTER";
                        Session["FPMUserRole"] = _objModel.UserRole;
                        string id = _objModel.SubmitForm1(_objModel);

                        if (id != "0")
                        {
                            //Session["FPMOffenseCode"] = id;
                            //_objModel.OffenseCode = id;
                            //_objModel.RegFormNumber = 2;
                            //DDLList();
                            TempData["ForesterParivad"] = "Record save sucessfully";
                            return RedirectToAction("ForesterAction", "ForesterAction");
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
        /// Use for Save and Upadate the details of Offense Category details
        /// </summary>
        /// <param name="fcollection"></param>
        /// <param name="Model"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        public ActionResult SubmitDetails1(FormCollection fcollection, OffenseReg Model, string Command, List<HttpPostedFileBase> fileUpload)
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string FilePath = "~/ForestProtectionDocument/OffenderDetails/";
            string id = string.Empty;
            Session.Remove("oKnownOffender");
            try
            {
               
                    _objModel.UserID = UserID;
                    if (Command == "Previous1")
                    {
                        if (Session["FPMOffenseCode"] != null)
                        {
                            getdataview(); 

                            DataSet dtf1 = _objModel.GetAllRecordsForm1(Session["FPMOffenseCode"].ToString());
                            if (dtf1.Tables[0].Rows.Count > 0)
                            {
                                _objModel.IsEditMode = true;
                                DDLList();
                                ViewBag.circle = dtf1.Tables[0].Rows[0]["CIRCLE_NAME"].ToString();
                                ViewBag.division = dtf1.Tables[0].Rows[0]["DIV_NAME"].ToString();
                                ViewBag.district = dtf1.Tables[0].Rows[0]["DIST_NAME"].ToString();
                                _objModel.CircleCode = dtf1.Tables[0].Rows[0]["Circle"].ToString();
                                _objModel.DivisionCode = dtf1.Tables[0].Rows[0]["Division"].ToString();
                                _objModel.DistrictCode = dtf1.Tables[0].Rows[0]["District"].ToString();
                                _objModel.Tehsil = dtf1.Tables[0].Rows[0]["Tehsil"].ToString();
                                _objModel.Naka = dtf1.Tables[0].Rows[0]["Naka"].ToString();
                                _objModel.ForestBlock = dtf1.Tables[0].Rows[0]["Block"].ToString();
                                _objModel.Compartment = dtf1.Tables[0].Rows[0]["Compartment"].ToString();
                                _objModel.Latitude = dtf1.Tables[0].Rows[0]["Latitude"].ToString();
                                _objModel.Longitude = dtf1.Tables[0].Rows[0]["Longitude"].ToString();
                                _objModel.LandMark = dtf1.Tables[0].Rows[0]["LandMark"].ToString();
                                _objModel.NakaDistance = dtf1.Tables[0].Rows[0]["DistanceFromNaka"].ToString();
                                _objModel.ForestType = Convert.ToInt32(dtf1.Tables[0].Rows[0]["ForestType"].ToString());
                                DateTime dateto = new DateTime();
                                dateto = Convert.ToDateTime(dtf1.Tables[0].Rows[0]["OffenseDate"].ToString());
                                _objModel.OffenseDate = dateto.ToString("dd/MM/yyyy");
                                _objModel.OffenseTime = dtf1.Tables[0].Rows[0]["OffenseTime"].ToString();
                                _objModel.RangeCode = dtf1.Tables[0].Rows[0]["Range"].ToString();
                                _objModel.OffensePlace = dtf1.Tables[0].Rows[0]["OffensePlace"].ToString();
                                _objModel.RegFormNumber = 1;
                                return View("OffenseRegistration", _objModel);
                            }
                        }
                    }
                    else
                    {
                        if (Session["FPMOffenseCode"] != null)
                        {
                            getdataview();                                                     
                            int existRecordCount = Convert.ToInt32(new OffenseReg().GetAllRecordsForm2(Session["FPMOffenseCode"].ToString(), "SelectForm2").Tables[0].Rows[0]["RowsCount"].ToString());
                            if (existRecordCount > 0)
                            {                                                             
                                _objModel.OffenseCode = Session["FPMOffenseCode"].ToString();
                                _objModel.OffenceCategory = string.IsNullOrEmpty(Model.OffenceCategory) ? "" : Model.OffenceCategory;
                                _objModel.OffenseSubCategoryWildLife = string.IsNullOrEmpty(fcollection["OffenseSubCategoryWildLife"].ToString()) ? "" : fcollection["OffenseSubCategoryWildLife"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.OffenseSubCategoryWildLife) ? "" : Model.OffenseSubCategoryWildLife;
                                _objModel.OffenseSubCategoryForest = string.IsNullOrEmpty(fcollection["OffenseSubCategoryForest"].ToString()) ? "" : fcollection["OffenseSubCategoryForest"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.OffenseSubCategoryForest) ? "" : Model.OffenseSubCategoryForest;
                                _objModel.WildlifeProtectionSection = string.IsNullOrEmpty(fcollection["WildlifeProtectionSection"].ToString()) ? "" : fcollection["WildlifeProtectionSection"].Replace(",", "").Trim().ToString();// string.IsNullOrEmpty(Model.WildlifeProtectionSection) ? "" : Model.WildlifeProtectionSection;
                                _objModel.ForestProtectionSection = string.IsNullOrEmpty(fcollection["ForestProtectionSection"].ToString()) ? "" : fcollection["ForestProtectionSection"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.ForestProtectionSection) ? "" : Model.ForestProtectionSection;
                                _objModel.OffenseSeverity = string.IsNullOrEmpty(Model.OffenseSeverity) ? "" : Model.OffenseSeverity;
                               // _objModel.CrimeScenePhoto1 = string.IsNullOrEmpty(fcollection["hdCrimePicUrlPath1"].ToString()) ? "" : fcollection["hdCrimePicUrlPath1"].ToString();
                               // _objModel.CrimeScenePhoto2 = string.IsNullOrEmpty(fcollection["hdCrimePicUrlPath2"].ToString()) ? "" : fcollection["hdCrimePicUrlPath2"].ToString();
                               // _objModel.CrimeScenePhoto3 = string.IsNullOrEmpty(fcollection["hdCrimePicUrlPath3"].ToString()) ? "" : fcollection["hdCrimePicUrlPath3"].ToString();
                                _objModel.OPoliceStation = string.IsNullOrEmpty(fcollection["OPoliceStation"].ToString()) ? "" : fcollection["OPoliceStation"].Replace(",", "").Trim().ToString();
                               // _objModel.IsCompoundable = string.IsNullOrEmpty(fcollection["compoundable"].ToString()) ? "" : fcollection["compoundable"].ToString();
                              //  _objModel.SettlementAmount = Model.SettlementAmount;
                                //_objModel.IsAmountPaid = string.IsNullOrEmpty(fcollection["Amount"].ToString()) ? "" : fcollection["Amount"].ToString();                                                                            
                                id = _objModel.UpdateForm2(_objModel);                               
                                if (id != "0")
                                {
                                    Session["FPMOffenseID"] = id;
                                    _objModel.RegFormNumber = 3;
                                    DDLList();
                                    if (Session["FPMUserRole"] != null)
                                    {
                                        _objModel.UserRole = Session["FPMUserRole"].ToString();
                                    }
                                    SubmitDetails3(Encryption.encrypt(id));
                                    return View("OffenseRegistration", _objModel);
                                }
                            }
                            else
                            {
                                _objModel.OffenderID = Convert.ToInt64(Session["FPMOffenseCode"].ToString());
                                _objModel.OffenceCategory = string.IsNullOrEmpty(Model.OffenceCategory) ? "" : Model.OffenceCategory;
                                _objModel.OffenseSubCategoryWildLife = string.IsNullOrEmpty(fcollection["OffenseSubCategoryWildLife"].ToString()) ? "" : fcollection["OffenseSubCategoryWildLife"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.OffenseSubCategoryWildLife) ? "" : Model.OffenseSubCategoryWildLife;
                                _objModel.OffenseSubCategoryForest = string.IsNullOrEmpty(fcollection["OffenseSubCategoryForest"].ToString()) ? "" : fcollection["OffenseSubCategoryForest"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.OffenseSubCategoryForest) ? "" : Model.OffenseSubCategoryForest;
                                _objModel.WildlifeProtectionSection = string.IsNullOrEmpty(fcollection["WildlifeProtectionSection"].ToString()) ? "" : fcollection["WildlifeProtectionSection"].Replace(",", "").Trim().ToString();// string.IsNullOrEmpty(Model.WildlifeProtectionSection) ? "" : Model.WildlifeProtectionSection;
                                _objModel.ForestProtectionSection = string.IsNullOrEmpty(fcollection["ForestProtectionSection"].ToString()) ? "" : fcollection["ForestProtectionSection"].Replace(",", "").Trim().ToString(); //string.IsNullOrEmpty(Model.ForestProtectionSection) ? "" : Model.ForestProtectionSection;
                                _objModel.OffenseSeverity = _objModel.OffenseSeverity = string.IsNullOrEmpty(Model.OffenseSeverity) ? "" : Model.OffenseSeverity;
                               // _objModel.CrimeScenePhoto1 = string.IsNullOrEmpty(fcollection["hdCrimePicUrlPath1"].ToString()) ? "" : fcollection["hdCrimePicUrlPath1"].ToString();
                               // _objModel.CrimeScenePhoto2 = string.IsNullOrEmpty(fcollection["hdCrimePicUrlPath2"].ToString()) ? "" : fcollection["hdCrimePicUrlPath2"].ToString();
                               // _objModel.CrimeScenePhoto3 = string.IsNullOrEmpty(fcollection["hdCrimePicUrlPath3"].ToString()) ? "" : fcollection["hdCrimePicUrlPath3"].ToString();
                                _objModel.OPoliceStation = string.IsNullOrEmpty(fcollection["OPoliceStation"].ToString()) ? "" : fcollection["OPoliceStation"].Replace(",", "").Trim().ToString();
                              //  _objModel.IsCompoundable = string.IsNullOrEmpty(fcollection["compoundable"].ToString()) ? "" : fcollection["compoundable"].ToString();
                               // _objModel.SettlementAmount = Model.SettlementAmount;
                             //   _objModel.IsAmountPaid = string.IsNullOrEmpty(fcollection["Amount"].ToString()) ? "" : fcollection["Amount"].ToString();
                                if (Session["FPMUserRole"] != null)
                                {
                                    _objModel.UserRole = Session["FPMUserRole"].ToString();
                                }
                                else
                                {
                                    _objModel.UserRole = "FORESTER";
                                }                              
                                id = _objModel.SubmitForm2(_objModel);

                                if (id != "0")
                                {
                                    Session["FPMOffenseID"] = id;
                                    _objModel.RegFormNumber = 3;
                                    DDLList();

                                    if (Session["FPMUserRole"] != null)
                                    {
                                        if (Session["FPMUserRole"].ToString() == "CITIZEN")
                                        {
                                            SubmitDetails3(Encryption.encrypt(id));

                                        }
                                    }

                                    return View("OffenseRegistration", _objModel);
                                }
                            }
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
        /// Use for Add and Upadate the functionlity of Offense Category and Offender Details
        /// </summary>
        /// <param name="fcollection"></param>
        /// <param name="Model"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        public ActionResult SubmitDetails2(FormCollection fcollection, OffenseReg Model, string Command)
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string FilePath = "~/ForestProtectionDocument/OffenderDetails/";
            try
            {
               _objModel.UserID = UserID;
                    if (Command == "Previous2")
                    {
                        if (Session["FPMOffenseCode"] != null)
                        {
                            getdataview(); 
                            DataSet dtf2 = _objModel.GetAllRecordsForm2(Session["FPMOffenseCode"].ToString());
                            if (dtf2.Tables[0].Rows.Count > 0)
                            {

                                _objModel.OffenceCategory = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["OffenceCategory"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["OffenceCategory"].ToString();
                                _objModel.OffenseSubCategoryWildLife = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["OffenseSubCategoryWildLife"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["OffenseSubCategoryWildLife"].ToString();
                                _objModel.OffenseSubCategoryForest = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["OffenseSubCategoryForest"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["OffenseSubCategoryForest"].ToString();
                                _objModel.WildlifeProtectionSection = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["SectionWildlife"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["SectionWildlife"].ToString();
                                _objModel.ForestProtectionSection = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["SectionForest"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["SectionForest"].ToString();
                                _objModel.OffenseSeverity = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["OffenseSeverity"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["OffenseSeverity"].ToString();
                                _objModel.OPoliceStation = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["PoliceStation"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["PoliceStation"].ToString();
                                //_objModel.CrimeScenePhoto1 = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["CrimePhotoURL1"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["CrimePhotoURL1"].ToString();
                                //_objModel.CrimeScenePhoto2 = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["CrimePhotoURL2"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["CrimePhotoURL2"].ToString();
                                //_objModel.CrimeScenePhoto3 = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["CrimePhotoURL3"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["CrimePhotoURL3"].ToString();

                                //if (_objModel.CrimeScenePhoto1 != "" || _objModel.CrimeScenePhoto2 != "" || _objModel.CrimeScenePhoto3 != "")
                                //{
                                //    _objModel.IsEditMode = true;
                                //}
                                //else { _objModel.IsEditMode = false; }
                               // _objModel.IsCompoundable = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["IsCompoundable"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["IsCompoundable"].ToString().Trim();
                               // _objModel.SettlementAmount = Convert.ToDecimal(dtf2.Tables[0].Rows[0]["SettlementAmount"].ToString());
                              //  _objModel.IsAmountPaid = string.IsNullOrEmpty(dtf2.Tables[0].Rows[0]["AmountPaid"].ToString()) ? "" : dtf2.Tables[0].Rows[0]["AmountPaid"].ToString().Trim();
                                _objModel.RegFormNumber = 2;
                                _objModel.OffenseCode = Session["FPMOffenseCode"].ToString();
                                if (Session["FPMUserRole"] != null)
                                    _objModel.UserRole = Session["FPMUserRole"].ToString();
                            }
                        }
                    }
                    else
                    {
                        int existRecordCount = Convert.ToInt32(new OffenseReg().GetAllRecordsForm2(Session["FPMOffenseCode"].ToString(), "SelectForm3").Tables[0].Rows[0]["RowsCount"].ToString());
                        if (existRecordCount > 0)
                        {
                            _objModel.OffenderType = string.IsNullOrEmpty(fcollection["Offender"].ToString()) ? "" : fcollection["Offender"].ToString();
                            _objModel.OffenseCode = Session["FPMOffenseID"].ToString();
                            _objModel.ONumberOfOffender = Model.ONumberOfOffender;
                            _objModel.OffenderDescription = string.IsNullOrEmpty(Model.OffenderDescription) ? "" : Model.OffenderDescription;
                           //_objModel.OPoliceStation = string.IsNullOrEmpty(Model.OPoliceStation) ? "" : Model.OPoliceStation;
                            _objModel.OffenseStatementDate = string.IsNullOrEmpty(fcollection["OffenseStatementDate"].ToString()) ? "" : fcollection["OffenseStatementDate"].ToString();
                          //  _objModel.OffenderStatement = string.IsNullOrEmpty(Model.OffenderStatement) ? "" : Model.OffenderStatement;
                            _objModel.OffenderComplainant = string.IsNullOrEmpty(Model.OffenderComplainant) ? "" : Model.OffenderComplainant;
                            //if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                            //{
                            //    FileName = Path.GetFileName(Request.Files[1].FileName);
                            //    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            //    path = Path.Combine(FilePath, FileFullName);
                            //    _objModel.OffenderStatementDoc = path;
                            //    Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                            //}
                            //else
                            //{                               
                            //    _objModel.OffenderStatementDoc = string.IsNullOrEmpty(fcollection["hdOffenderStatementDoc"].ToString()) ? "" : fcollection["hdOffenderStatementDoc"].ToString();                  
                            //}

                            if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                            {
                                FileName = Path.GetFileName(Request.Files[2].FileName);
                                FileFullName = DateTime.Now.Ticks + "_" + FileName;
                                path = Path.Combine(FilePath, FileFullName);
                                _objModel.ComplainantStatementDoc = path;
                                Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                            }
                            else
                            {
                                _objModel.ComplainantStatementDoc = string.IsNullOrEmpty(fcollection["hdComplainantStatementDoc"].ToString()) ? "" : fcollection["hdComplainantStatementDoc"].ToString();                                                    
                            }

                            if (Session["oKnownOffender"] != null)
                            {
                                List<KnownOffenderDetails> list = (List<KnownOffenderDetails>)Session["oKnownOffender"];
                                if (list != null)
                                {
                                    DataTable DT = ExtMethodCommon.AsDataTable(list);
                                    DT.Columns.Remove("OOffenderrowid");
                                    DT.AcceptChanges();
                                    string id = _objModel.UpdateForm3(_objModel, DT);

                                    if (id != "0")
                                    {
                                        _objModel.RegFormNumber = 4;
                                        DDLList();
                                        Session["oKnownOffender"] = null;
                                        return RedirectToAction("OffenseRegistrationfinal", "OffenseRegistrationfinal");
                                    }

                                }
                            }
                            else
                            {
                                List<KnownOffenderDetails> list = new List<KnownOffenderDetails>();
                                KnownOffenderDetails obj = new KnownOffenderDetails();
                                obj = new KnownOffenderDetails
                                {
                                    OffenderName = "",
                                    OFatherName = "",
                                    OSpouseName = "",
                                    OCategory="",
                                    OCaste = "",
                                    ClothesWorn="",
                                    ColorOfClothes="",
                                    PhysicalAppearance="",
                                    Height="",
                                    OtherSpecialDetails="",
                                    OAddress1 = "",
                                    OAddress2 = "",
                                    OPincode = "",
                                    OStateCode = "",
                                    ODistrictCode = "",
                                    OVillageCode = "",
                                    OPhoneNo = "",
                                    OEmailID = "",
                                    OPhotoIDURL = "",
                                    OPhotoIDType = 0,
                                    OffenderStatement = "",
                                    OffenderStatementDoc = "",
                                    OffenderAge = "0",
                                    ArrestedOrdetained = "",
                                    InformToOffenderRelative = "",
                                    CommunicationMode = "",
                                    CommunicationDate = "" ,
                                    FardGriftri = "",
                                    GriftariPunchnama = "",
                                    NagriNaka = "",
                                    JamaTalashi = "",
                                    MedicalReport = ""                                             
                                };
                                list.Add(obj);
                                DataTable DT = ExtMethodCommon.AsDataTable(list);
                                DT.Columns.Remove("OOffenderrowid");
                                DT.AcceptChanges();
                                string id = _objModel.SubmitForm3(_objModel, DT);
                                if (id != "0")
                                {
                                    _objModel.RegFormNumber = 4;
                                    DDLList();
                                    Session["oKnownOffender"] = null;
                                    return RedirectToAction("OffenseRegistrationfinal", "OffenseRegistrationfinal");
                                }
                            }

                        }
                        else
                        {
                            _objModel.OffenderType = string.IsNullOrEmpty(fcollection["Offender"].ToString()) ? "" : fcollection["Offender"].ToString();
                            _objModel.OffenseCode = Session["FPMOffenseID"].ToString();
                            _objModel.ONumberOfOffender = Model.ONumberOfOffender;
                            _objModel.OffenderDescription = string.IsNullOrEmpty(Model.OffenderDescription) ? "" : Model.OffenderDescription;
                            //_objModel.OPoliceStation = string.IsNullOrEmpty(Model.OPoliceStation) ? "" : Model.OPoliceStation;
                            _objModel.OffenseStatementDate = string.IsNullOrEmpty(fcollection["OffenseStatementDate"].ToString()) ? "" : fcollection["OffenseStatementDate"].ToString();
                           //_objModel.OffenderStatement = string.IsNullOrEmpty(Model.OffenderStatement) ? "" : Model.OffenderStatement;
                            _objModel.OffenderComplainant = string.IsNullOrEmpty(Model.OffenderComplainant) ? "" : Model.OffenderComplainant;

                            //if (Request.Files[1] != null && Request.Files[2].ContentLength > 0)
                            //{

                            //    FileName = Path.GetFileName(Request.Files[2].FileName);
                            //    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            //    path = Path.Combine(FilePath, FileFullName);
                            //    _objModel.OffenderStatementDoc = path;
                            //    Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                            //}
                            //else
                            //{ _objModel.OffenderStatementDoc = string.IsNullOrEmpty(fcollection["hdOffenderStatementDoc"].ToString()) ? "" : fcollection["hdOffenderStatementDoc"].ToString(); }

                            if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                            {

                                FileName = Path.GetFileName(Request.Files[2].FileName);
                                FileFullName = DateTime.Now.Ticks + "_" + FileName;
                                path = Path.Combine(FilePath, FileFullName);
                                _objModel.ComplainantStatementDoc = path;
                                Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                            }
                            else
                            { _objModel.ComplainantStatementDoc = string.IsNullOrEmpty(fcollection["hdComplainantStatementDoc"].ToString()) ? "" : fcollection["hdComplainantStatementDoc"].ToString(); }

                            if (Session["oKnownOffender"] != null)
                            {
                                List<KnownOffenderDetails> list = (List<KnownOffenderDetails>)Session["oKnownOffender"];
                                if (list != null)
                                {
                                    DataTable DT = ExtMethodCommon.AsDataTable(list);
                                    DT.Columns.Remove("OOffenderrowid");
                                    DT.AcceptChanges();
                                    string id = _objModel.UpdateForm3(_objModel, DT);
                                    if (id != "0")
                                    {
                                        _objModel.RegFormNumber = 4;
                                        DDLList();
                                        Session["oKnownOffender"] = null;
                                        return RedirectToAction("OffenseRegistrationfinal", "OffenseRegistrationfinal");
                                    }
                                }
                            }
                            else
                            {
                                List<KnownOffenderDetails> list = new List<KnownOffenderDetails>();
                                KnownOffenderDetails obj = new KnownOffenderDetails();
                                obj = new KnownOffenderDetails
                                {

                                    OffenderName = "",
                                    OFatherName = "",
                                    OSpouseName = "",
                                    OCategory="",
                                    OCaste = "",
                                    ClothesWorn = "",
                                    ColorOfClothes = "",
                                    PhysicalAppearance = "",
                                    Height = "",
                                    OtherSpecialDetails = "",
                                    OAddress1 = "",
                                    OAddress2 = "",
                                    OPincode = "",
                                    OStateCode = "",
                                    ODistrictCode = "",
                                    OVillageCode = "",
                                    OPhoneNo = "",
                                    OEmailID = "",
                                    OPhotoIDURL = "",
                                    OPhotoIDType = 0,
                                    OffenderStatement="",
                                    OffenderStatementDoc="",
                                    OffenderAge = "0",                                  
                                    ArrestedOrdetained = "",
                                    InformToOffenderRelative = "",
                                    CommunicationMode = "",
                                    CommunicationDate = "",
                                    FardGriftri = "",
                                    GriftariPunchnama = "",
                                    NagriNaka = "",
                                    JamaTalashi = "",
                                    MedicalReport = ""
                                };
                                list.Add(obj);
                                DataTable DT = ExtMethodCommon.AsDataTable(list);
                                DT.Columns.Remove("OOffenderrowid");
                                DT.AcceptChanges();
                                string id = _objModel.SubmitForm3(_objModel, DT);
                                if (id != "0")
                                {
                                    _objModel.RegFormNumber = 4;
                                    DDLList();
                                    Session["oKnownOffender"] = null;
                                    return RedirectToAction("OffenseRegistrationfinal", "OffenseRegistrationfinal");
                                }
                            }
                        }
                    }
                    DDLList();
                    return View("OffenseRegistration", _objModel);              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

        /// <summary>
        /// Use for Binding the Offender details in Edit mode 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult SubmitDetails3(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> District = new List<SelectListItem>();
            List<SelectListItem> OPhotoIDType = new List<SelectListItem>();
            try
            {              
                    _objModel.UserID = UserID;
                    if (Session["FPMOffenseCode"] != null)
                    {
                        getdataview(); 
                        int existRecordCount = Convert.ToInt32(new OffenseReg().GetAllRecordsForm2(Session["FPMOffenseCode"].ToString(), "SelectForm3").Tables[0].Rows[0]["RowsCount"].ToString());
                        if (existRecordCount == 0)
                        {
                            DataSet dtf3 = _objModel.GetCitizenOffenderRecords("SelectCitizenOffenderDetails", Encryption.decrypt(id));
                            if (dtf3.Tables[0].Rows.Count > 0)
                            {
                                ViewBag.ddlOState = new SelectList(FPMParivadRegistrations.DDLState(), "Value", "Text", dtf3.Tables[0].Rows[0]["StateCode"].ToString());
                                if (dtf3.Tables[0].Rows[0]["DistrictCode"].ToString() != "")
                                {
                                    DataTable dtd = new BindMasterData().getDistricts();
                                    foreach (System.Data.DataRow dr in dtd.Rows)
                                    {
                                        District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                                    }
                                    ViewBag.ddlODistrict = new SelectList(District, "Value", "Text", dtf3.Tables[0].Rows[0]["DistrictCode"].ToString());
                                }
                                _objModel.UserRole = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["UserRole"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["UserRole"].ToString();
                                _objModel.OffenderType = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["OffenderType"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["OffenderType"].ToString();                              
                                _objModel.ONumberOfOffender = Convert.ToInt32(dtf3.Tables[0].Rows[0]["NumberOfOffender"].ToString());
                                _objModel.OffenderDescription = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["OffenderDescription"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["OffenderDescription"].ToString();
                                if (dtf3.Tables[0].Rows[0]["StatementDate"].ToString() != "" && dtf3.Tables[0].Rows[0]["StatementDate"].ToString()!=null)
                                {
                                    DateTime dateto = new DateTime();
                                    dateto = Convert.ToDateTime(dtf3.Tables[0].Rows[0]["StatementDate"].ToString());
                                    _objModel.OffenseStatementDate = dateto.ToString("dd/MM/yyyy");                                
                                }
                               // _objModel.OffenderStatement = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["OffenderStatement"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["OffenderStatement"].ToString();
                                _objModel.OffenderComplainant = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["ComplainantStatement"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["ComplainantStatement"].ToString();
                              //  _objModel.OffenderStatementDoc = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["OffenderStatementDoc"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["OffenderStatementDoc"].ToString();
                                _objModel.ComplainantStatementDoc = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["ComplainantStatementDoc"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["ComplainantStatementDoc"].ToString();
                                _objModel.OffenseCode = Encryption.decrypt(id);
                                DDLList();
                                _objModel.RegFormNumber = 3;
                                return View("OffenseRegistration", _objModel);
                            }
                        }
                        else
                        {
                            DataSet dtf3 = _objModel.GetAllRecordsForm3(Encryption.decrypt(id));
                            if (dtf3.Tables[0].Rows.Count > 0)
                            {
                                ViewBag.ddlOState = new SelectList(FPMParivadRegistrations.DDLState(), "Value", "Text", dtf3.Tables[0].Rows[0]["StateCode"].ToString());
                                DataTable dtd = new BindMasterData().getDistricts();
                                foreach (System.Data.DataRow dr in dtd.Rows)
                                {
                                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                                }
                                ViewBag.ddlODistrict = new SelectList(District, "Value", "Text", dtf3.Tables[0].Rows[0]["DistrictCode"].ToString());
                                DataTable dtoPhoto = new OffenseReg().Get_OPhotoIDType();
                                foreach (System.Data.DataRow dr in dtoPhoto.Rows)
                                {
                                    OPhotoIDType.Add(new SelectListItem { Text = @dr["PhotoID_Name"].ToString(), Value = @dr["OPhotoIDTypeID"].ToString() });
                                }
                                ViewBag.OPhotoIDType = new SelectList(OPhotoIDType, "Value", "Text", dtf3.Tables[0].Rows[0]["OPhotoIDTypeID"].ToString());
                                _objModel.OffenderType = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["OffenderType"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["OffenderType"].ToString();                              
                                DateTime dateto = new DateTime();
                                dateto = Convert.ToDateTime(dtf3.Tables[0].Rows[0]["StatementDate"].ToString());
                                _objModel.OffenseStatementDate = dateto.ToString("dd/MM/yyyy");
                             //   _objModel.OffenderStatement = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["OffenderStatement"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["OffenderStatement"].ToString();
                                _objModel.OffenderComplainant = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["ComplainantStatement"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["ComplainantStatement"].ToString();
                             //   _objModel.OffenderStatementDoc = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["OffenderStatementDoc"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["OffenderStatementDoc"].ToString();
                                _objModel.ComplainantStatementDoc = string.IsNullOrEmpty(dtf3.Tables[0].Rows[0]["ComplainantStatementDoc"].ToString()) ? "" : dtf3.Tables[0].Rows[0]["ComplainantStatementDoc"].ToString();
                                _objModel.OffenseCode = Encryption.decrypt(id);
                                if (Session["FPMUserRole"] != null)
                                    _objModel.UserRole = Session["FPMUserRole"].ToString();
                                DDLList();
                                _objModel.RegFormNumber = 3;
                                return View("OffenseRegistration", _objModel);
                            }
                            else
                            {
                                return View("Error");
                            }
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
        /// Bind village on dist code block code and gp code
        /// </summary>
        /// <param name="District_code"></param>
        /// <param name="Block_code"></param>
        /// <param name="GP_Code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getVillage(string dist_code) {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> lstVillage = new List<SelectListItem>();
            OffenseReg or = new OffenseReg();
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
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 4, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }            
            return Json(new SelectList(lstVillage, "Value", "Text"));
        }

        /// <summary>
        /// use for Bind all the drop down controls
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
            List<SelectListItem> lstCaste = new List<SelectListItem>();
            List<SelectListItem> lstClothes = new List<SelectListItem>();
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

                    DataTable dtd = new BindMasterData().getDistricts();
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

                    DataTable dtCast = new BindMasterData().GetCastDetails();
                    foreach (System.Data.DataRow dr in dtCast.Rows)
                    {
                        lstCaste.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.ddlOCaste = new SelectList(lstCaste, "Value", "Text");

                    DataTable dtClothes = new OffenseReg().GetClothes();
                    foreach (System.Data.DataRow dr in dtClothes.Rows)
                    {
                        lstClothes.Add(new SelectListItem { Text = @dr["Clothes"].ToString(), Value = @dr["Clothes"].ToString() });
                    }
                    ViewBag.ddlOClothes = new SelectList(lstClothes, "Value", "Text");  
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
        }
        /// <summary>
        /// Use for Save the multiple Offender details
        /// </summary>
        /// <param name="offender"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OffenderData(KnownOffenderDetails offender)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {                
                _objModel.UserID = UserID;
                if (offender.OffenderName != null && offender.OFatherName != null && offender.ClothesWorn != null && offender.OVillageCode != null)
                {
                    List<KnownOffenderDetails> lstKnownOffender = new List<KnownOffenderDetails>();
                    if (Session["oKnownOffender"] != null)
                    {
                        List<KnownOffenderDetails> list = (List<KnownOffenderDetails>)Session["oKnownOffender"];                        
                        if (list != null && list.Count>0)
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
                        if (offender.OOffenderrowid == null || offender.OOffenderrowid=="")
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
        /// Use for delete the existing offender details based on email-id
        /// </summary>
        /// <param name="offender"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteOffenderData(string Id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<KnownOffenderDetails> listOffender = new List<KnownOffenderDetails>();
            try
            {                
                _objModel.UserID = UserID;
                if (Session["oKnownOffender"] != null)
                {
                    listOffender = (List<KnownOffenderDetails>)Session["oKnownOffender"];                    
                    if (Id != "0" && Id.Length > 0)
                    {
                        KnownOffenderDetails ofreg = listOffender.Single(a => a.OOffenderrowid == Id);
                        listOffender.Remove(ofreg);
                    }
                    Session["oKnownOffender"] = listOffender;                    
                }               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return Json(listOffender, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Use for Uplaod the Documents
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = "~/ForestProtectionDocument/OffenderDetails/";
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                
                    _objModel.UserID = UserID;
                    // Checking no of files injected in Request object  
                    if (Request.Files.Count > 0)
                    {

                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
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
                        return Json(new { list1 = "File Uploaded Successfully!", list2 = path });
                    }


                    else
                    {
                        return Json("No files selected.");
                    }
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// Use for Uplaod the Crime Scene Documents
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadCrimeSceneFiles()
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = "~/ForestProtectionDocument/CrimeSceneDetails/";
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                
                    _objModel.UserID = UserID;
                    // Checking no of files injected in Request object  
                    if (Request.Files.Count > 0)
                    {

                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
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
                        return Json(new { list1 = "File Uploaded Successfully!", list2 = path });
                    }
               
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
            List<KnownOffenderDetails> list = new List<KnownOffenderDetails>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {              
                    _objModel.UserID = UserID;
                    if (!String.IsNullOrEmpty(id))
                    {
                        if (Session["FPMOffenseCode"] != null)
                        {
                            int existRecordCount = Convert.ToInt32(new OffenseReg().GetAllRecordsForm2(Session["FPMOffenseCode"].ToString(), "SelectForm3").Tables[0].Rows[0]["RowsCount"].ToString());
                            if (existRecordCount == 0)
                            {
                                DataSet dtf3 = _objModel.GetCitizenOffenderRecords("SelectCitizenOffenderDetails", id);
                                if (dtf3.Tables[0].Rows.Count > 0)
                                {                                   
                                    foreach (DataRow dr in dtf3.Tables[0].Rows)
                                    {
                                        if (dr["OffenderType"].ToString() == "Known" || dr["OffenderType"].ToString() == "KNOWN")
                                        {
                                        list.Add(
                                           new KnownOffenderDetails()
                                           {
                                               OOffenderrowid = Guid.NewGuid().ToString(),
                                               OffenderName = dr["OffenderName"].ToString(),
                                               OFatherName = dr["FatherName"].ToString(),
                                               OSpouseName = dr["SpouseName"].ToString(),
                                               OCategory = dr["Category"].ToString(),
                                               OCaste = dr["Caste"].ToString(),
                                               ClothesWorn = dr["clothesWorn"].ToString(),
                                               ColorOfClothes = dr["ClothesColor"].ToString(),
                                               PhysicalAppearance = dr["PhysicalAppearance"].ToString(),
                                               Height = dr["Height"].ToString(),
                                               OtherSpecialDetails = dr["OtherSpecialDetails"].ToString(),
                                               OAddress1 = dr["Address1"].ToString(),
                                               OAddress2 = dr["Address2"].ToString(),
                                               OStateCode = dr["StateCode"].ToString(),
                                               ODistrictCode = dr["DistrictCode"].ToString(),
                                               OVillageCode = dr["VillageCode"].ToString(),
                                               OPincode = dr["Pincode"].ToString(),
                                               OPhoneNo = dr["PhoneNo"].ToString(),
                                               OEmailID = dr["EmailID"].ToString(),
                                               //OPhotoIDURL = dtf3.Tables[0].Rows[0]["IDProofURL"].ToString(),
                                               //OPhotoIDType = Convert.ToInt32(dtf3.Tables[0].Rows[0]["OPhotoIDTypeID"].ToString()),
                                               OPhotoIDURL="",
                                               OPhotoIDType=0,
                                               OffenderStatement = dr["OffenderStatement"].ToString(),
                                               OffenderStatementDoc = dr["OffenderStatementDoc"].ToString(),
                                               OffenderAge = dr["OffenderAge"].ToString(),
                                               ArrestedOrdetained = "",
                                               InformToOffenderRelative = "",
                                               CommunicationMode = "",
                                               CommunicationDate = "",
                                               FardGriftri = "",
                                               GriftariPunchnama = "",
                                               NagriNaka = "",
                                               JamaTalashi = "",
                                               MedicalReport = ""
                                           });
                                          Session["oKnownOffender"] = list;
                                        }                                       
                                    }
                                }
                            }
                            else
                            {
                                DataSet dtf3 = _objModel.GetAllRecordsForm3(id);
                                if (dtf3.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtf3.Tables[0].Rows) {
                                        if (dr["OffenderType"].ToString() == "Known" || dr["OffenderType"].ToString() == "KNOWN")
                                        {
                                            list.Add(
                                                   new KnownOffenderDetails()
                                                   {
                                                       OOffenderrowid = Guid.NewGuid().ToString(),
                                                       OffenderName = dr["OffenderName"].ToString(),
                                                       OFatherName = dr["FatherName"].ToString(),
                                                       OSpouseName = dr["SpouseName"].ToString(),
                                                       OCategory = dr["Category"].ToString(),
                                                       OCaste = dr["Caste"].ToString(),
                                                       ClothesWorn = dr["clothesWorn"].ToString(),
                                                       ColorOfClothes = dr["ClothesColor"].ToString(),
                                                       PhysicalAppearance = dr["PhysicalAppearance"].ToString(),
                                                       Height = dr["Height"].ToString(),
                                                       OtherSpecialDetails = dr["OtherSpecialDetails"].ToString(),
                                                       OAddress1 = dr["Address1"].ToString(),
                                                       OAddress2 = dr["Address2"].ToString(),
                                                       OStateCode = dr["StateCode"].ToString(),
                                                       ODistrictCode = dr["DistrictCode"].ToString(),
                                                       OVillageCode = dr["VillageCode"].ToString(),
                                                       OPincode = dr["Pincode"].ToString(),
                                                       OPhoneNo = dr["PhoneNo"].ToString(),
                                                       OEmailID = dr["EmailID"].ToString(),
                                                       OPhotoIDURL = dr["IDProofURL"].ToString(),
                                                       OPhotoIDType = Convert.ToInt32(dr["OPhotoIDTypeID"].ToString()),
                                                       OffenderStatement = dr["OffenderStatement"].ToString(),
                                                       OffenderStatementDoc = dr["OffenderStatementDoc"].ToString(),
                                                       OffenderAge = dr["OffenderAge"].ToString(),
                                                       ArrestedOrdetained = dr["ArrestedOrdetained"].ToString(),
                                                       InformToOffenderRelative = dr["InformToOffenderRelative"].ToString(),
                                                       CommunicationMode = dr["CommunicationMode"].ToString(),
                                                       CommunicationDate = dr["OffenderComDate"].ToString(),
                                                       FardGriftri = dr["FardGriftri"].ToString(),
                                                       GriftariPunchnama = dr["GriftariPunchnama"].ToString(),
                                                       NagriNaka = dr["NagriNaka"].ToString(),
                                                       JamaTalashi = dr["JamaTalashi"].ToString(),
                                                       MedicalReport = dr["MedicalReport"].ToString()
                                                   });
                                            Session["oKnownOffender"] = list;
                                        }
                                    }                                       
                                }
                            }
                        }
                        return Json(list, JsonRequestBehavior.AllowGet);

                    }
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;

        }
        /// <summary>
        /// Use for Page load data of Offense View
        /// </summary>
        /// <returns></returns>
        public ActionResult FPMViewOffense()
         {
            List<OffenseReg> Offenderdata = new List<OffenseReg>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
               
                    _objModel.UserID = UserID;
                    DataSet dtf = _objModel.GetViewExistingRecords();
                    if (dtf.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dtf.Tables.Count; i++)
                        {
                         foreach (DataRow dr in dtf.Tables[0].Rows)
                          Offenderdata.Add(
                            new OffenseReg()
                            {
                                District = dr["District"].ToString(),
                                UserRole = dr["UserRole"].ToString(),
                                OffenseCode = dr["OffenseCode"].ToString(),
                                OffensePlace = dr["OffensePlace"].ToString(),
                                OffenseDate = dr["OffenseDate"].ToString(),
                                OffenseTime = dr["OffenseTime"].ToString(),
                                Status = Convert.ToInt32(dr["Status"].ToString())
                            });
                        }
                        ViewData["OffenderList"] = Offenderdata;
                    }
                    else
                    {
                        Offenderdata.Add(
                            new OffenseReg()
                            {
                                District = "",
                                UserRole = "",
                                OffenseCode = "",
                                OffensePlace = "",
                                OffenseDate = "",
                                OffenseTime = "",
                                Status = 0
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

        public void getdataview() {
            DataSet dsOffense = new DataSet();
            OffenseRegistrationfinal orf = new OffenseRegistrationfinal();
            dsOffense = orf.GetApplicantDeatils(Session["FPMOffenseCode"].ToString());
            if (dsOffense.Tables[0].Rows.Count > 0)
            {
                _objModel.OffenseCode = dsOffense.Tables[0].Rows[0]["OffenseCode"].ToString();
                _objModel.OffenseDate = dsOffense.Tables[0].Rows[0]["OffenseDate"].ToString();
                _objModel.OffensePlace = dsOffense.Tables[0].Rows[0]["OffensePlace"].ToString();
                _objModel.District = dsOffense.Tables[0].Rows[0]["DIST_NAME"].ToString();
                _objModel.UserRole = dsOffense.Tables[0].Rows[0]["UserRole"].ToString();
                _objModel.ApplicantName = dsOffense.Tables[0].Rows[0]["Name"].ToString();
            }


            List<SelectListItem> lstPolicStation = new List<SelectListItem>();
            DataTable dtPoliceStation = new BindMasterData().GetPoliceStationDetails(Convert.ToString(_objModel.OffenseCode), _objModel.UserRole);
            foreach (System.Data.DataRow dr in dtPoliceStation.Rows)
            {
                lstPolicStation.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
            }

            ViewBag.ddlOPoliceStation = new SelectList(lstPolicStation, "Value", "Text");                                      
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
            _objModel.UserID = UserID;
            if (OffenseCode != null)
            {
                Session["FPMOffenseCode"] = Encryption.decrypt(OffenseCode);
                Session["FPMUserRole"] = Encryption.decrypt(UserRole);
                _objModel.UserRole = Encryption.decrypt(UserRole);
            }                                      
            SubmitDetails2(fm, _objModel, "Previous2");
            getdataview();                                                      
            return View("OffenseRegistration", _objModel);                   
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// fetch the data for Applicant (Citizen and Offender)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserRole"></param>
        /// <returns></returns>
        public JsonResult GetApplicantDetail(string id, string UserRole)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DataSet dtf3 = new DataSet();
            try
            {                
                    _objModel.UserID = UserID;
                    if (Session["FPMOffenseCode"] != null)
                    {
                        dtf3 = _objModel.GetApplicantDeatils(id, UserRole);
                        if (dtf3.Tables[0].Rows.Count > 0)
                        {
                            return Json(new
                            {
                                OffenseCode = dtf3.Tables[0].Rows[0]["OffenseCode"].ToString(),
                                 Category = dtf3.Tables[0].Rows[0]["FOCategory"].ToString(),
                                 Forest_Protection_Act = dtf3.Tables[0].Rows[0]["Forest_Protection_Act"].ToString(),
                                 FOSubCategory = dtf3.Tables[0].Rows[0]["FOSubCategory"].ToString(),
                                 Wildlife_Protection_Act = dtf3.Tables[0].Rows[0]["Wildlife_Protection_Act"].ToString(),
                                 WOSubCategory = dtf3.Tables[0].Rows[0]["WOSubCategory"].ToString(),

                                 DateOfOffense = dtf3.Tables[0].Rows[0]["DateOfOffense"].ToString(),
                                 TimeOfOffense = dtf3.Tables[0].Rows[0]["TimeOfOffense"].ToString(),
                                 OffenseDetail = dtf3.Tables[0].Rows[0]["Description"].ToString(),
                                 OffensePlace = dtf3.Tables[0].Rows[0]["OffensePlace"].ToString(),
                                 DIST_NAME = dtf3.Tables[0].Rows[0]["DIST_NAME"].ToString(),
                                 Ssoid = dtf3.Tables[0].Rows[0]["Ssoid"].ToString(),

                                 VisitDate = dtf3.Tables[0].Rows[0]["VisitDate"].ToString(),
                                 VisitPlace = dtf3.Tables[0].Rows[0]["VisitPlace"].ToString(),
                                 VisitTime = dtf3.Tables[0].Rows[0]["VisitTime"].ToString(),
                                 InvestiDescription = dtf3.Tables[0].Rows[0]["Description"].ToString(),
                               
                            });
                        }
                    }
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

        [HttpPost]
        public ActionResult UploadAction(OffenseReg model, List<HttpPostedFileBase> fileUpload)
        {
            // Your Code - / Save Model Details to DB
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = "~/ForestProtectionDocument/OffenderDetails/";
            // Handling Attachments -
            foreach (HttpPostedFileBase item in fileUpload)
            {
                if (item != null)
                {
                    if (Array.Exists(model.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
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
                        item.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                }
            }
            return View("OffenseRegistration");
        }

        [HttpPost]
        public JsonResult EditDetails(string ID)
        {
            try
            {
                OffenseReg OFA = new OffenseReg();
                List<KnownOffenderDetails> lstKnownOffender = new List<KnownOffenderDetails>();
                List<KnownOffenderDetails> lstKnownOffenderEdit = new List<KnownOffenderDetails>();
                if (Session["oKnownOffender"] != null)
                {                 
                    lstKnownOffender = (List<KnownOffenderDetails>)Session["oKnownOffender"];
                    if (ID != "0" && ID.Length > 0)
                    {
                        KnownOffenderDetails Offder = lstKnownOffender.Single(a => a.OOffenderrowid == ID);
                        lstKnownOffenderEdit.Add(Offder);                    
                    }
                }
                foreach (var item in lstKnownOffenderEdit)
                {                   
                    OFA.OffenderName = item.OffenderName;
                    OFA.OFatherName = item.OFatherName;
                    OFA.OSpouseName = item.OSpouseName;
                    OFA.OCategory = item.OCategory;
                    OFA.OCaste = item.OCaste;
                    OFA.OClothesWorn = item.ClothesWorn;
                    OFA.OColorOfClothes = item.ColorOfClothes;
                    OFA.OPhysicalAppearance = item.PhysicalAppearance;
                    OFA.OHeight = item.Height;
                    OFA.OOtherSpecialDetails = item.OtherSpecialDetails;
                    OFA.OAddress1=item.OAddress1;
                    OFA.OAddress2=item.OAddress2;
                    OFA.OStateCode=item.OStateCode;
                    OFA.OPincode =item.OPincode;
                    OFA.OVillageCode=item.OVillageCode;
                    OFA.ODistrictCode=item.ODistrictCode;
                    OFA.OEmailID =item.OEmailID;
                    OFA.OPhoneNo=item.OPhoneNo;
                    OFA.OPhotoIDURL=item.OPhotoIDURL;
                    OFA.OPhotoIDType=item.OPhotoIDType.ToString();
                    OFA.OffenderAge =item.OffenderAge;
                    OFA.ArrestedOrdetained = item.ArrestedOrdetained;
                    OFA.InformToOffenderRelative = item.InformToOffenderRelative;
                    OFA.CommunicationMode = item.CommunicationMode;
                    OFA.OffenderStatement = item.OffenderStatement;
                    OFA.OffenderStatementDoc = item.OffenderStatementDoc;
                    OFA.FardGriftri = item.FardGriftri;
                    OFA.GriftariPunchnama = item.GriftariPunchnama;
                    OFA.NagriNaka = item.NagriNaka;
                    OFA.JamaTalashi = item.JamaTalashi;
                    OFA.MedicalReport = item.MedicalReport;
                    if (item.CommunicationDate != "")
                    {                   
                        OFA.CommunicationDate = item.CommunicationDate;
                    }
                    else {
                        OFA.CommunicationDate = "";
                    }
                    
                }                                                                                                                                                                                                                                                                                                            
              return Json(OFA, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetforestSubCat(string FProtectionId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                OffenseReg ORF = new OffenseReg();
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = ORF.Get_ForestSubCategory(FProtectionId);
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["FOSubCategory"].ToString(), Value = @dr["FOSubCatID"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }

    }
}
