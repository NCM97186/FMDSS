﻿
//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : OffenseRegistrationfinalController
//  Description  : File contains calling functions from UI
//  Date Created : 29-Mar-2016
//  History      : Add the Details of 3 tabs (Warrant, Seized item and Crime scene )
//  Version      : 1.0
//  Author       : Raj kumar
//  Modified By  : Raj kumar
//  History      : Code change for add the new form File court case and edit functionlity for all tabs
//  Modified On  : 19-Apr-2016
//  Reviewed By  : 
//  Reviewed On  : 
//Bug No : 463,472,473,475

//********************************************************************************************************

using FMDSS.Filters;
using FMDSS.Models;
using FMDSS.Models.ForestProtection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace FMDSS.Controllers.ForestProtection
{
    [MyAuthorization]
    [MyExceptionHandler]
    public class OffenseRegistrationfinalController : BaseController
    {
        /// <summary>
        /// Load the All the Form UI controls with default value 
        /// </summary>
        /// <param name="Mode"></param>
        ///  Exception No-360
        /// <returns></returns>        
        int ModuleID = 4;
        string actionName = string.Empty;
        string controllerName = string.Empty;
        List<OffenseRegistrationfinal> OffenceFinal = new List<OffenseRegistrationfinal>();
        public ActionResult OffenseRegistrationfinal(string Mode,string OCode)
        {
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            try
            {
                Session.Remove("AddCrime");
                Session.Remove("AddVechile");
                Session.Remove("AddProduce");
                Session.Remove("AddEquipment");
                Session.Remove("AddAnimalArticle");
                Session.Remove("AddWitness");
                Session.Remove("AddWarrant");
                Session.Remove("AddAnimal");
                
                OR.TabShow1 = OR.TabShowInfo("1");
                if (OR.TabShow1 == "F" || OR.TabShow2 == "F")
                {
                    getDropdown();
                    OffenseList();
                    BindOffence();
                    OR = Edit();
                    OR.Iscomplete = OR.TabShowInfo("3");
                    OR.ApproveStatus = OR.TabShowInfo("4");
                    OR.DfoApproveStatus = OR.TabShowInfo("5");
                    return View(OR);
                }
                else
                {
                    getDropdown();
                    OffenseList();
                    BindOffence();
                    OR = getdataview();
                    OR.Iscomplete = OR.TabShowInfo("3");
                    OR.ApproveStatus = OR.TabShowInfo("4");
                    OR.DfoApproveStatus = OR.TabShowInfo("5");
                    return View(OR);
                }           
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
                return View(OR);
            }                     
        }
        public OffenseRegistrationfinal getdataview()
        {
            DataSet dsOffense = new DataSet();
            OffenseRegistrationfinal orf = new OffenseRegistrationfinal();
            dsOffense = orf.GetApplicantDeatils(Session["FPMOffenseCode"].ToString());
            if (dsOffense.Tables[0].Rows.Count > 0)
            {
                orf.OffenseCode = dsOffense.Tables[0].Rows[0]["OffenseCode"].ToString();
                orf.OffenseDate = dsOffense.Tables[0].Rows[0]["OffenseDate"].ToString();
                orf.OffensePlace = dsOffense.Tables[0].Rows[0]["OffensePlace"].ToString();
                orf.District = dsOffense.Tables[0].Rows[0]["DIST_NAME"].ToString();
                orf.UserRole = dsOffense.Tables[0].Rows[0]["UserRole"].ToString();
            }
            return orf;
        }
        /// <summary>
        /// function use to bind file court case list
        /// </summary>
        public void BindOffence()
        {
            OffenseRegistrationfinal objReg = new OffenseRegistrationfinal();
                    DataTable DT = new DataTable();
                    DT = objReg.GetCourtCaseDetailByOffenceCode();                                        
                        foreach (DataRow dr in DT.Rows)
                            OffenceFinal.Add(
                                new OffenseRegistrationfinal()

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
        }
        /// <summary>
        /// Fetch Offense List
        /// </summary>        
        /// <returns></returns>
        public ActionResult OffenseList()
        {
            List<OffenseRegistrationfinal> Offenselst = new List<OffenseRegistrationfinal>();
             actionName = this.ControllerContext.RouteData.Values["action"].ToString();
             controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                DataSet dsOffense = new DataSet();
                OffenseRegistrationfinal orf = new OffenseRegistrationfinal();
                dsOffense = orf.GetOffenseDetails();
                foreach (System.Data.DataRow dr in dsOffense.Tables[0].Rows)
                {
                    Offenselst.Add(new OffenseRegistrationfinal
                    {
                        OffenseCode = dr["OffenseCode"].ToString(),
                        OffenseDate = dr["OffenseDate"].ToString(),
                        OffensePlace = dr["OffensePlace"].ToString(),
                        DIST_NAME = dr["DIST_NAME"].ToString(),
                        UserRole = dr["UserRole"].ToString(), 

                    });
                }
                ViewData["Warrantlst"] = Offenselst;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View();
        }
        /// <summary>
        /// Get List of file court case
        /// </summary>
        /// <returns></returns>
        public ActionResult FileCourtCase()
        {
             actionName = this.ControllerContext.RouteData.Values["action"].ToString();
             controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> Offenselst = new List<OffenseRegistrationfinal>();
            try
            {
                DataSet dsOffense = new DataSet();
                OffenseRegistrationfinal orf = new OffenseRegistrationfinal();
                dsOffense = orf.GetCourtCaseDetails();

                foreach (System.Data.DataRow dr in dsOffense.Tables[0].Rows)
                {
                    Offenselst.Add(new OffenseRegistrationfinal
                    {
                        OffenseCode = dr["OffenseCode"].ToString(),
                        OffenseDate = dr["OffenseDate"].ToString(),
                        OffensePlace = dr["OffensePlace"].ToString(),
                        DIST_NAME = dr["DIST_NAME"].ToString(),
                        UserRole = dr["UserRole"].ToString(), 
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View(Offenselst);
        }
        /// <summary>
        /// Get list of list come under in issue jammant
        /// </summary>
        /// <returns></returns>
        public ActionResult IssueJammant()
        {
             actionName = this.ControllerContext.RouteData.Values["action"].ToString();
             controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> Offenselst = new List<OffenseRegistrationfinal>();
            try
            {
                DataSet dsOffense = new DataSet();
                OffenseRegistrationfinal orf = new OffenseRegistrationfinal();
                dsOffense = orf.GetIssueJammantDetails();
                foreach (System.Data.DataRow dr in dsOffense.Tables[0].Rows)
                {
                    Offenselst.Add(new OffenseRegistrationfinal
                    {
                        OffenseCode = dr["OffenseCode"].ToString(),
                        OffenseDate = dr["OffenseDate"].ToString(),
                        OffensePlace = dr["OffensePlace"].ToString(),
                        DIST_NAME = dr["DIST_NAME"].ToString(),
                        UserRole = dr["UserRole"].ToString(), 
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View(Offenselst);
        }       
        /// <summary>
        /// Bind the Drop down controls
        /// </summary>
        public void getDropdown()
        {
             actionName = this.ControllerContext.RouteData.Values["action"].ToString();
             controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                #region Dropdown Bind
                OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
                DataTable dtforestofficer = new DataTable();
                DataTable dtVechileType = new DataTable();
                DataTable dtProduceType = new DataTable();
                DataTable dtEquipmentType = new DataTable();
                DataTable dtVillage = new DataTable();
                DataTable dtDistrict = new DataTable();
                DataTable dtOffender = new DataTable();
                DataTable dtAnimal = new DataTable();
                DataTable dtSurvey = new DataTable();
                DataTable dtOfficePlace = new DataTable();
                List<SelectListItem> lstfirst = new List<SelectListItem>();
                List<SelectListItem> lstVechile = new List<SelectListItem>();
                List<SelectListItem> lstProduce = new List<SelectListItem>();
                List<SelectListItem> lstEquipmentType = new List<SelectListItem>();
                List<SelectListItem> lstVilllage = new List<SelectListItem>();
                List<SelectListItem> lstDistrict = new List<SelectListItem>();
                List<SelectListItem> lstOffender = new List<SelectListItem>();
                List<SelectListItem> lstAnimal = new List<SelectListItem>();
                List<SelectListItem> lstCaste = new List<SelectListItem>();
                List<SelectListItem> lstOfficePalce = new List<SelectListItem>();
                List<OffenseRegistrationfinal> lstSurvey = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstfile = new List<OffenseRegistrationfinal>();
                DataTable dtfile = new DataTable();               
                dtSurvey = OR.GetSurveyDetails();
                dtVechileType = OR.GetDropdown("1");
                dtProduceType = OR.GetDropdown("2");
                dtEquipmentType = OR.GetDropdown("3");
                dtVillage = OR.GetDropdown("4");
                dtDistrict = OR.GetDropdown("5");
                dtAnimal = OR.GetDropdown("6");
                dtOfficePlace = OR.GetWarrantOfficePlace();
                if (Session["FPMOffenseID"] != null)
                {
                    dtfile = OR.GetFileInfo(Session["FPMOffenseID"].ToString());
                    dtforestofficer = OR.GetForestOfficers(Session["FPMOffenseID"].ToString());
                    dtOffender = OR.GetOffenderName();
                    if (dtOffender.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in dtOffender.Rows)
                        {
                            lstOffender.Add(new SelectListItem { Text = @dr["OffenderName"].ToString(), Value = @dr["OffenderName"].ToString() });
                        }
                    }
                    else
                    {
                        lstOffender.Add(new SelectListItem { Text = "Test", Value = "Test" });
                    }
                }
                else
                {
                    dtforestofficer = OR.GetForestOfficers("");
                    lstOffender.Add(new SelectListItem { Text = "Test", Value = "Test" });
                }
                ViewBag.OffenderName = lstOffender;

                foreach (System.Data.DataRow dr in dtforestofficer.Rows)
                {
                    lstfirst.Add(new SelectListItem { Text = @dr["EmpName"].ToString(), Value = @dr["ROWID"].ToString() });
                }
                ViewBag.FOfficers = lstfirst;
                foreach (System.Data.DataRow dr in dtVechileType.Rows)
                {
                    lstVechile.Add(new SelectListItem { Text = @dr["VechileName"].ToString(), Value = @dr["VechileId"].ToString() });
                }
                ViewBag.VechileType = lstVechile;
                foreach (System.Data.DataRow dr in dtProduceType.Rows)
                {
                    lstProduce.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ProduceId"].ToString() });
                }
                ViewBag.ProduceType = lstProduce;
                foreach (System.Data.DataRow dr in dtEquipmentType.Rows)
                {
                    lstEquipmentType.Add(new SelectListItem { Text = @dr["EquipmentType"].ToString(), Value = @dr["EquipmentId"].ToString() });
                }
                ViewBag.EquipmentType = lstEquipmentType;
                foreach (System.Data.DataRow dr in dtVillage.Rows)
                {
                    lstVilllage.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                }
                ViewBag.Village = lstVilllage;
                foreach (System.Data.DataRow dr in dtDistrict.Rows)
                {
                    lstDistrict.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.District = lstDistrict;
                foreach (System.Data.DataRow dr in dtAnimal.Rows)
                {
                    lstAnimal.Add(new SelectListItem { Text = @dr["AnimalName"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.AnimalName = lstAnimal;

                DataTable dtCast = new BindMasterData().GetCastDetails();
                foreach (System.Data.DataRow dr in dtCast.Rows)
                {
                    lstCaste.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.ddlOCaste = new SelectList(lstCaste, "Value", "Text");

                foreach (System.Data.DataRow dr in dtSurvey.Rows)
                {                    
                    lstSurvey.Add(new OffenseRegistrationfinal {                                               
                                                VisitDate = @dr["Date_Of_Visit"].ToString(),
                                                VisitTime = @dr["Time_Of_Visit"].ToString(),
                                                VisitPlace = @dr["PlaceOfVisit"].ToString(),
                                                Description = @dr["Description_of_Crime"].ToString(),                                                                                               
                                                Village = @dr["VILL_NAME"].ToString(),
                                                Range = @dr["RANGE_NAME"].ToString(),
                                            });
                }
                ViewBag.SurveyDetail = lstSurvey;
                foreach (System.Data.DataRow dr in dtfile.Rows)
                {
                    lstfile.Add(new OffenseRegistrationfinal { OffenseCode = @dr["OffenseCode"].ToString(), FileView = @dr["FilePath"].ToString() });
                }
                ViewData["filelist"] = lstfile;

                foreach (System.Data.DataRow dr in dtOfficePlace.Rows) {
                    lstOfficePalce.Add(new SelectListItem { Text = @dr["OfficeName"].ToString(), Value = @dr["Office_ID"].ToString() });
                }
                ViewBag.ddlPlace = lstOfficePalce;
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }

        }
        /// <summary>
        /// Showing the existing data in edit mode
        /// </summary>
        /// <returns></returns>
        public OffenseRegistrationfinal Edit()
        {
             actionName = this.ControllerContext.RouteData.Values["action"].ToString();
             controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            try
            {
                #region FormEdit
                DataSet dsForesterDetails = new DataSet();
                List<OffenseRegistrationfinal> lstCrime = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstVechile = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstProduce = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstAnimal = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstEquipment = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstAnimalArticle = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstWitness = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstWarrant = new List<OffenseRegistrationfinal>();
                DataSet dsOffense = new DataSet();
                dsOffense = OR.GetApplicantDeatils(Session["FPMOffenseCode"].ToString());
                foreach (System.Data.DataRow dr in dsOffense.Tables[0].Rows)
                {
                    OR.OffenseCode = dr["OffenseCode"].ToString();
                    OR.OffenseDate = dr["OffenseDate"].ToString();
                    OR.OffensePlace = dr["OffensePlace"].ToString();
                    OR.District = dr["DIST_NAME"].ToString();
                    OR.UserRole = dr["UserRole"].ToString();
                }
                dsForesterDetails = OR.GetForestDetails();
                if (dsForesterDetails.Tables[0].Rows.Count > 0)
                {
                    OffenseRegistrationfinal ORC = null;
                    for (int i = 0; i < dsForesterDetails.Tables[0].Rows.Count; i++)
                    {
                        ORC = new OffenseRegistrationfinal();
                        ORC.CrimeSceneID = Convert.ToInt64(dsForesterDetails.Tables[0].Rows[i]["CrimeSceneID"].ToString());
                        ORC.Crimerowid = Guid.NewGuid().ToString();
                        ORC.VisitDate = dsForesterDetails.Tables[0].Rows[i]["VisitDate"].ToString();
                        ORC.VisitPlace = dsForesterDetails.Tables[0].Rows[i]["VisitPlace"].ToString();
                        ORC.VisitTime = dsForesterDetails.Tables[0].Rows[i]["VisitTime"].ToString();
                        ORC.Description = dsForesterDetails.Tables[0].Rows[i]["Description"].ToString();
                        ORC.PhotoURL = dsForesterDetails.Tables[0].Rows[i]["PhotoURL"].ToString();
                        lstCrime.Add(ORC);
                    }
                    Session["AddCrime"] = lstCrime;
                }
                if (dsForesterDetails.Tables[1].Rows.Count > 0)
                {

                    OR.FirstOfficer = dsForesterDetails.Tables[1].Rows[0]["FirstOfficerID"].ToString();
                    OR.FirstOfficerDesig = dsForesterDetails.Tables[1].Rows[0]["DesignationFirstOfficer"].ToString();
                    OR.SecondOfficer = dsForesterDetails.Tables[1].Rows[0]["SecondOfficerID"].ToString();
                    OR.SecondOfficerDesig = dsForesterDetails.Tables[1].Rows[0]["DesignationSecondOfficer"].ToString();
                    OR.ThirdOfficer = dsForesterDetails.Tables[1].Rows[0]["ThirdOfficerId"].ToString();
                    OR.ThirdOfficerDesig = dsForesterDetails.Tables[1].Rows[0]["DesignationThirdOfficer"].ToString();
                    OR.FourthOfficer = dsForesterDetails.Tables[1].Rows[0]["FourthOfficerId"].ToString();
                    OR.FourthOfficerDesig = dsForesterDetails.Tables[1].Rows[0]["DesignationFourthOfficer"].ToString();
                }

                if (dsForesterDetails.Tables[2].Rows.Count > 0)
                {
                    OffenseRegistrationfinal OR1 = null;
                    for (int i = 0; i < dsForesterDetails.Tables[2].Rows.Count; i++)
                    {

                        OR1 = new OffenseRegistrationfinal();
                        OR1.Vechilerowid = Guid.NewGuid().ToString();
                        OR1.SeizedItemId = Convert.ToInt64(dsForesterDetails.Tables[2].Rows[i]["TeamID"].ToString());
                        OR1.VechileRegistrationNo = dsForesterDetails.Tables[2].Rows[i]["VehicleRegistrationNo"].ToString();
                        OR1.VechileOwnerName = dsForesterDetails.Tables[2].Rows[i]["OwnerName"].ToString();
                        OR1.VechileType = dsForesterDetails.Tables[2].Rows[i]["VehicleType"].ToString();
                        OR1.VechileMake = dsForesterDetails.Tables[2].Rows[i]["VehicleMake"].ToString();
                        OR1.VechileModel = dsForesterDetails.Tables[2].Rows[i]["VehicleModel"].ToString();
                        OR1.VechileChassisNo = dsForesterDetails.Tables[2].Rows[i]["ChassisNo"].ToString();
                        OR1.VechileEngineNo = dsForesterDetails.Tables[2].Rows[i]["EngineNo"].ToString();
                        OR1.PastOffensesByVechile = dsForesterDetails.Tables[2].Rows[i]["PastOffenses"].ToString();
                        lstVechile.Add(OR1);
                    }
                    Session["AddVechile"] = lstVechile;
                }
                if (dsForesterDetails.Tables[3].Rows.Count > 0)
                {
                    OffenseRegistrationfinal OR2 = null;
                    for (int i = 0; i < dsForesterDetails.Tables[3].Rows.Count; i++)
                    {
                        OR2 = new OffenseRegistrationfinal();
                        OR2.Producerowid = Guid.NewGuid().ToString();
                       OR2.SeizedItemId = Convert.ToInt64(dsForesterDetails.Tables[3].Rows[i]["TeamID"].ToString());
                        OR2.ProduceType = dsForesterDetails.Tables[3].Rows[i]["ProduceTypeId"].ToString();
                        OR2.SpeciesName = dsForesterDetails.Tables[3].Rows[i]["SpeciesName"].ToString();
                        OR2.QuantityOfProduce = dsForesterDetails.Tables[3].Rows[i]["Quantity"].ToString();
                        lstProduce.Add(OR2);
                    }
                    Session["AddProduce"] = lstProduce;
                }
                if (dsForesterDetails.Tables[4].Rows.Count > 0)
                {
                    OffenseRegistrationfinal OR3 = null;
                    for (int i = 0; i < dsForesterDetails.Tables[4].Rows.Count; i++)
                    {
                        OR3 = new OffenseRegistrationfinal();
                        OR3.Animalrowid = Guid.NewGuid().ToString();
                        OR3.SeizedItemId = Convert.ToInt64(dsForesterDetails.Tables[4].Rows[i]["TeamID"].ToString());
                        OR3.AnimalScientificName = dsForesterDetails.Tables[4].Rows[i]["AnimalScientificName"].ToString();
                        OR3.AnimalCommanName = dsForesterDetails.Tables[4].Rows[i]["AnimalCommanName"].ToString();
                        OR3.AnimalDescription = dsForesterDetails.Tables[4].Rows[i]["AnimalDescription"].ToString();
                        OR3.AnimalWeight = dsForesterDetails.Tables[4].Rows[i]["AnimalWeight"].ToString();
                        lstAnimal.Add(OR3);
                    }
                    Session["AddAnimal"] = lstAnimal;
                }
                if (dsForesterDetails.Tables[5].Rows.Count > 0)
                {
                    OffenseRegistrationfinal OR4 = null;
                    for (int i = 0; i < dsForesterDetails.Tables[5].Rows.Count; i++)
                    {
                        OR4 = new OffenseRegistrationfinal();
                        OR4.Equipmentrowid = Guid.NewGuid().ToString();
                        OR4.SeizedItemId = Convert.ToInt64(dsForesterDetails.Tables[5].Rows[i]["TeamID"].ToString());
                        OR4.EquipmentType = dsForesterDetails.Tables[5].Rows[i]["EquipmentTypeId"].ToString();
                        OR4.EquipmentMake = dsForesterDetails.Tables[5].Rows[i]["Make"].ToString();
                        OR4.EquipmentModel = dsForesterDetails.Tables[5].Rows[i]["Model"].ToString();
                        OR4.EquipmentCaliber = dsForesterDetails.Tables[5].Rows[i]["Caliber"].ToString();
                        OR4.EquipmentIdentificationNo = dsForesterDetails.Tables[5].Rows[i]["IdentificationNo"].ToString();
                        OR4.EquipmentSize = dsForesterDetails.Tables[5].Rows[i]["size"].ToString();
                        lstEquipment.Add(OR4);
                    }
                    Session["AddEquipment"] = lstEquipment;
                }

                if (dsForesterDetails.Tables[6].Rows.Count > 0)
                {
                    OffenseRegistrationfinal OR5 = null;
                    for (int i = 0; i < dsForesterDetails.Tables[6].Rows.Count; i++)
                    {
                        OR5 = new OffenseRegistrationfinal();
                        OR5.Equipmentrowid = Guid.NewGuid().ToString();
                        OR5.SeizedItemId = Convert.ToInt64(dsForesterDetails.Tables[6].Rows[i]["TeamID"].ToString());
                        OR5.ArticleAnimalScientificName = dsForesterDetails.Tables[6].Rows[i]["ScientificName"].ToString();
                        OR5.ArticleAnimalCommanName = dsForesterDetails.Tables[6].Rows[i]["CommanName"].ToString();
                        OR5.NameOfAnimalArticle = dsForesterDetails.Tables[6].Rows[i]["AnimalArticleName"].ToString();
                        OR5.DescriptionOfAnimalArticle = dsForesterDetails.Tables[6].Rows[i]["AnimalArticleDescription"].ToString();
                        OR5.ArticleQuantity = dsForesterDetails.Tables[6].Rows[i]["Quantity"].ToString();
                        lstAnimalArticle.Add(OR5);
                    }
                    Session["AddAnimalArticle"] = lstAnimalArticle;
                }

                if (dsForesterDetails.Tables[7].Rows.Count > 0)
                {
                    OffenseRegistrationfinal OR6 = null;
                    for (int i = 0; i < dsForesterDetails.Tables[7].Rows.Count; i++)
                    {
                        OR6 = new OffenseRegistrationfinal();
                        OR6.Witnessrowid = Guid.NewGuid().ToString();
                        OR6.WitnessId = Convert.ToInt64(dsForesterDetails.Tables[7].Rows[i]["WitnessID"].ToString());
                        OR6.WitnessName = dsForesterDetails.Tables[7].Rows[i]["WitnessName"].ToString();
                        OR6.FatherName = dsForesterDetails.Tables[7].Rows[i]["FatherName"].ToString();
                        OR6.SpouseName = dsForesterDetails.Tables[7].Rows[i]["SpouseName"].ToString();
                        OR6.Category = dsForesterDetails.Tables[7].Rows[i]["Category"].ToString();
                        OR6.Caste = dsForesterDetails.Tables[7].Rows[i]["Caste"].ToString();
                        OR6.ResidentialAddress1 = dsForesterDetails.Tables[7].Rows[i]["Address1"].ToString();
                        OR6.ResidentialAddress2 = dsForesterDetails.Tables[7].Rows[i]["Address2"].ToString();
                        OR6.Pincode = dsForesterDetails.Tables[7].Rows[i]["Pincode"].ToString();
                        OR6.Village = dsForesterDetails.Tables[7].Rows[i]["VillageCode"].ToString();
                        OR6.District = dsForesterDetails.Tables[7].Rows[i]["DistrictCode"].ToString();
                        OR6.State = dsForesterDetails.Tables[7].Rows[i]["StateCode"].ToString();
                        OR6.PhoneNo = dsForesterDetails.Tables[7].Rows[i]["PhoneNo"].ToString();
                        OR6.PhotoId = dsForesterDetails.Tables[7].Rows[i]["IDType"].ToString();
                        OR6.UploadId = dsForesterDetails.Tables[7].Rows[i]["IDProofURL"].ToString();
                        OR6.WitnessAge = dsForesterDetails.Tables[7].Rows[i]["WitnessAge"].ToString();
                        OR6.StatementDate = dsForesterDetails.Tables[7].Rows[i]["StatementDate1"].ToString();
                        OR6.WitnessStatement = dsForesterDetails.Tables[7].Rows[i]["WitnessStatement"].ToString();
                        OR6.UploadSignedStatement = dsForesterDetails.Tables[7].Rows[i]["SignedStatementURL"].ToString();
                        lstWitness.Add(OR6);
                    }
                    Session["AddWitness"] = lstWitness;
                }

                if (dsForesterDetails.Tables[8].Rows.Count > 0)
                {
                    OffenseRegistrationfinal OR7 = null;
                    for (int i = 0; i < dsForesterDetails.Tables[8].Rows.Count; i++)
                    {
                        OR7 = new OffenseRegistrationfinal();
                        OR7.Warrantrowid = Guid.NewGuid().ToString();                       
                        OR7.NameOfOffender = dsForesterDetails.Tables[8].Rows[i]["OffenderName"].ToString();
                        OR7.ClothesWorn = dsForesterDetails.Tables[8].Rows[i]["ClothesWorn"].ToString();
                        OR7.ColorOfClothes = dsForesterDetails.Tables[8].Rows[i]["ClothesColor"].ToString();
                        OR7.PhysicalAppearance = dsForesterDetails.Tables[8].Rows[i]["PhysicalAppearance"].ToString();
                        OR7.Height = dsForesterDetails.Tables[8].Rows[i]["Height"].ToString();
                        OR7.OtherSpecialDetails = dsForesterDetails.Tables[8].Rows[i]["OtherSpecialDetails"].ToString();
                        OR7.Appearancedate = dsForesterDetails.Tables[8].Rows[i]["AppreanceDate"].ToString();
                        OR7.Appearancetime = dsForesterDetails.Tables[8].Rows[i]["AppearanceTime"].ToString();
                        OR7.AppearancePlace = dsForesterDetails.Tables[8].Rows[i]["AppearancePlace"].ToString();
                        lstWarrant.Add(OR7);
                    }
                    Session["AddWarrant"] = lstWarrant;
                }              
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return OR;
        }


        // Defect log Id-89 Done by Rajkumar 
        /// <summary>
        /// Bind the Designation details
        /// </summary>
        /// <param name="FirstOfficer"></param>
        ///         /// Exception No-360
        /// <returns></returns>
        public JsonResult getDesignation(string FirstOfficer)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal ORF = null;
            try
            {
                ORF = new OffenseRegistrationfinal();
                DataTable dtdesig = new DataTable();
                dtdesig = ORF.GetForestOfficersDesignation(FirstOfficer);
                ORF.FirstOfficerDesig = dtdesig.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(ORF.FirstOfficerDesig, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// function to return forest officer with designation
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFirstForestOfficer()
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal ORF = null;
            try
            {
                ORF = new OffenseRegistrationfinal();
                DataTable dtOfficer = new DataTable();
                dtOfficer = ORF.GetFirstForestOfficer(Session["FPMOffenseID"].ToString());
                if (dtOfficer.Rows.Count > 0)
                {
                    ORF.FirstOfficer = dtOfficer.Rows[0]["EmpId"].ToString();
                    ORF.FirstOfficerDesig = dtOfficer.Rows[0]["Desig_Name"].ToString();
                }
                else {
                    ORF.FirstOfficer = "";
                    ORF.FirstOfficerDesig = "";
                }
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(ORF.FirstOfficer+"#"+ORF.FirstOfficerDesig,JsonRequestBehavior.AllowGet);
        }

        
        /// <summary>
        /// Function return animal scientific name
        /// </summary>
        /// <param name="AnimalName"></param>
        /// <returns></returns>
        public JsonResult getAnimalScientficName(string AnimalName)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal ORF = null;
            try
            {
                ORF = new OffenseRegistrationfinal();
                DataTable dtAnimal = new DataTable();
                dtAnimal = ORF.GetAnimalScientificName(AnimalName);
                ORF.AnimalScientificName = dtAnimal.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(ORF.AnimalScientificName, JsonRequestBehavior.AllowGet);
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
        public JsonResult AddVechile(string VechileRegistrationNo, string VechileOwnerName, string VechileType, string VechileMake, string VechileModel, string VechileChassisNo, string VechileEngineNo, string PastOffensesByVechile, string VechileUploadDoc)
        {
             actionName = this.ControllerContext.RouteData.Values["action"].ToString();
             controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
            List<OffenseRegistrationfinal> lstVechile = new List<OffenseRegistrationfinal>();
            try
            {
                OffenseRegist.VechileRegistrationNo = VechileRegistrationNo;
                OffenseRegist.VechileOwnerName = VechileOwnerName;
                OffenseRegist.VechileType = VechileType;
                OffenseRegist.VechileMake = VechileMake;
                OffenseRegist.VechileModel = VechileModel;
                OffenseRegist.VechileChassisNo = VechileChassisNo;
                OffenseRegist.VechileEngineNo = VechileEngineNo;
                OffenseRegist.PastOffensesByVechile = PastOffensesByVechile;
                OffenseRegist.VechileUploadDoc = VechileUploadDoc;
                if (Session["AddVechile"] != null)
                {
                    lstVechile = (List<OffenseRegistrationfinal>)Session["AddVechile"];
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
        /// get the existing records of vechile
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetVechil()
        {
            List<OffenseRegistrationfinal> lstVechile = new List<OffenseRegistrationfinal>();
            if (Session["AddVechile"] != null)
            {
                lstVechile = (List<OffenseRegistrationfinal>)Session["AddVechile"];
            }            
            return Json(lstVechile, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Delete the vechile details based on ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteVechile(string Id)
        {
             actionName = this.ControllerContext.RouteData.Values["action"].ToString();
             controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> lstVechile = new List<OffenseRegistrationfinal>();
            try
            {
                if (Session["AddVechile"] != null)
                {
                    lstVechile = (List<OffenseRegistrationfinal>)Session["AddVechile"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        OffenseRegistrationfinal ofreg = lstVechile.Single(a => a.Vechilerowid == Id);
                        lstVechile.Remove(ofreg);
                    }
                    Session["AddVechile"] = lstVechile;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstVechile, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Add the Product produce details
        /// </summary>
        /// <param name="ProduceType"></param>
        /// <param name="SpeciesName"></param>
        /// <param name="QuantityOfProduce"></param>
        /// <returns></returns>
        public JsonResult AddProduce(string ProduceType, string SpeciesName, string QuantityOfProduce, string ProduceUploadDoc)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
            List<OffenseRegistrationfinal> lstProduce = new List<OffenseRegistrationfinal>();
            try
            {
                OffenseRegist.ProduceType = ProduceType;
                OffenseRegist.SpeciesName = SpeciesName;
                OffenseRegist.QuantityOfProduce = QuantityOfProduce;
                OffenseRegist.ProduceUploadDoc = ProduceUploadDoc;
                if (Session["AddProduce"] != null)
                {
                    lstProduce = (List<OffenseRegistrationfinal>)Session["AddProduce"];
                    if (!lstProduce.Any(element => element.Producerowid == OffenseRegist.Producerowid))
                    {
                        OffenseRegist.Producerowid = Guid.NewGuid().ToString();
                        lstProduce.Add(OffenseRegist);
                    }
                }
                else
                {
                    OffenseRegist.Producerowid = Guid.NewGuid().ToString();
                    lstProduce.Add(OffenseRegist);
                    Session["AddProduce"] = lstProduce;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
           
            return Json(lstProduce, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Fetch the records of Product Produce
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetProduce()
        {
            List<OffenseRegistrationfinal> lstProduce = new List<OffenseRegistrationfinal>();
            if (Session["AddProduce"] != null)
            {
                lstProduce = (List<OffenseRegistrationfinal>)Session["AddProduce"];
            }          
            return Json(lstProduce, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Delete the older entries of produce
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteProduce(string Id)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> lstProduce = new List<OffenseRegistrationfinal>();
            try
            {
                if (Session["AddProduce"] != null)
                {
                    lstProduce = (List<OffenseRegistrationfinal>)Session["AddProduce"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        OffenseRegistrationfinal ofreg = lstProduce.Single(a => a.Producerowid == Id);
                        lstProduce.Remove(ofreg);
                    }
                    Session["AddProduce"] = lstProduce;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstProduce, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Add the equipment details
        /// </summary>
        /// <param name="EquipmentType"></param>
        /// <param name="EquipmentMake"></param>
        /// <param name="EquipmentModel"></param>
        /// <param name="EquipmentCaliber"></param>
        /// <param name="EquipmentIdentificationNo"></param>
        /// <param name="EquipmentSize"></param>
        /// <returns></returns>
        public JsonResult AddEquipment(string EquipmentType, string EquipmentMake, string EquipmentModel, string EquipmentCaliber, string EquipmentIdentificationNo, string EquipmentSize, string EquipmentUploadDoc)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
            List<OffenseRegistrationfinal> lstEquipment = new List<OffenseRegistrationfinal>();
            try
            {
                OffenseRegist.EquipmentType = EquipmentType;
                OffenseRegist.EquipmentMake = EquipmentMake;
                OffenseRegist.EquipmentModel = EquipmentModel;
                OffenseRegist.EquipmentCaliber = EquipmentCaliber;
                OffenseRegist.EquipmentIdentificationNo = EquipmentIdentificationNo;
                OffenseRegist.EquipmentSize = EquipmentSize;
                OffenseRegist.EquipmentUploadDoc = EquipmentUploadDoc;
                if (Session["AddEquipment"] != null)
                {
                    lstEquipment = (List<OffenseRegistrationfinal>)Session["AddEquipment"];
                    if (!lstEquipment.Any(element => element.Equipmentrowid == OffenseRegist.Equipmentrowid))
                    {
                        OffenseRegist.Equipmentrowid = Guid.NewGuid().ToString();
                        lstEquipment.Add(OffenseRegist);
                    }
                }
                else
                {
                    OffenseRegist.Equipmentrowid = Guid.NewGuid().ToString();
                    lstEquipment.Add(OffenseRegist);
                    Session["AddEquipment"] = lstEquipment;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
          
            return Json(lstEquipment, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get the equipment details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEquipment()
        {
            List<OffenseRegistrationfinal> lstEquipment = new List<OffenseRegistrationfinal>();
            if (Session["AddEquipment"] != null)
            {
                lstEquipment = (List<OffenseRegistrationfinal>)Session["AddEquipment"];
            }           
            return Json(lstEquipment, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Delete the Eqipment details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteEquipment(string Id)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> lstEquipment = new List<OffenseRegistrationfinal>();
            try
            {
                if (Session["AddEquipment"] != null)
                {
                    lstEquipment = (List<OffenseRegistrationfinal>)Session["AddEquipment"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        OffenseRegistrationfinal ofreg = lstEquipment.Single(a => a.Equipmentrowid == Id);
                        lstEquipment.Remove(ofreg);
                    }
                    Session["AddEquipment"] = lstEquipment;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstEquipment, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Add the Animal Details
        /// </summary>
        /// <param name="AnimalScientificName"></param>
        /// <param name="AnimalCommanName"></param>
        /// <param name="AnimalDescription"></param>
        /// <param name="AnimalWeight"></param>
        /// <returns></returns>
        public JsonResult AddAnimal(string AnimalScientificName, string AnimalCommanName, string AnimalDescription, string AnimalWeight, string AnimalUploadDoc)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
            List<OffenseRegistrationfinal> lstAnimal = new List<OffenseRegistrationfinal>();
            try
            {
                OffenseRegist.AnimalScientificName = AnimalScientificName;
                OffenseRegist.AnimalCommanName = AnimalCommanName;
                OffenseRegist.AnimalDescription = AnimalDescription;
                OffenseRegist.AnimalWeight = AnimalWeight;
                OffenseRegist.AnimalUploadDoc = AnimalUploadDoc;
                if (Session["AddAnimal"] != null)
                {
                    lstAnimal = (List<OffenseRegistrationfinal>)Session["AddAnimal"];
                    if (!lstAnimal.Any(element => element.Animalrowid == OffenseRegist.Animalrowid))
                    {
                        OffenseRegist.Animalrowid = Guid.NewGuid().ToString();
                        lstAnimal.Add(OffenseRegist);
                    }
                }
                else
                {
                    OffenseRegist.Animalrowid = Guid.NewGuid().ToString();
                    lstAnimal.Add(OffenseRegist);
                    Session["AddAnimal"] = lstAnimal;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            finally { }
            return Json(lstAnimal, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get the animal details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAnimal()
        {
            List<OffenseRegistrationfinal> lstAnimal = new List<OffenseRegistrationfinal>();
            if (Session["AddAnimal"] != null)
            {
                lstAnimal = (List<OffenseRegistrationfinal>)Session["AddAnimal"];
            }            
            return Json(lstAnimal, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// delete the exiting records by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteAnimal(string Id)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> lstAnimal = new List<OffenseRegistrationfinal>();
            try
            {
                if (Session["AddAnimal"] != null)
                {
                    lstAnimal = (List<OffenseRegistrationfinal>)Session["AddAnimal"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        OffenseRegistrationfinal ofreg = lstAnimal.Single(a => a.Animalrowid == Id);
                        lstAnimal.Remove(ofreg);
                    }
                    Session["AddAnimal"] = lstAnimal;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstAnimal, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Add animal article
        /// </summary>
        /// <param name="ArticleAnimalScientificName"></param>
        /// <param name="ArticleAnimalCommanName"></param>
        /// <param name="NameOfAnimalArticle"></param>
        /// <param name="DescriptionOfAnimalArticle"></param>
        /// <param name="ArticleQuantity"></param>
        /// <returns></returns>
        public JsonResult AddAnimalArticle(string ArticleAnimalScientificName, string ArticleAnimalCommanName, string NameOfAnimalArticle, string DescriptionOfAnimalArticle, string ArticleQuantity, string AnimalArticleUploadDoc)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
            List<OffenseRegistrationfinal> lstAnimalArticle = new List<OffenseRegistrationfinal>();
            try
            {
                OffenseRegist.ArticleAnimalScientificName = ArticleAnimalScientificName;
                OffenseRegist.ArticleAnimalCommanName = ArticleAnimalCommanName;
                OffenseRegist.NameOfAnimalArticle = NameOfAnimalArticle;
                OffenseRegist.DescriptionOfAnimalArticle = DescriptionOfAnimalArticle;
                OffenseRegist.ArticleQuantity = ArticleQuantity;
                OffenseRegist.AnimalArticleUploadDoc = AnimalArticleUploadDoc;
                if (Session["AddAnimalArticle"] != null)
                {
                    lstAnimalArticle = (List<OffenseRegistrationfinal>)Session["AddAnimalArticle"];
                    if (!lstAnimalArticle.Any(element => element.ArticleAnimalrowid == OffenseRegist.ArticleAnimalrowid))
                    {
                        OffenseRegist.ArticleAnimalrowid = Guid.NewGuid().ToString();
                        lstAnimalArticle.Add(OffenseRegist);
                    }
                }
                else
                {
                    OffenseRegist.ArticleAnimalrowid = Guid.NewGuid().ToString();
                    lstAnimalArticle.Add(OffenseRegist);
                    Session["AddAnimalArticle"] = lstAnimalArticle;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            finally { }
            return Json(lstAnimalArticle, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get the animal article details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAnimalArticle()
        {
            List<OffenseRegistrationfinal> lstAnimalArticle = new List<OffenseRegistrationfinal>();
            if (Session["AddAnimalArticle"] != null)
            {
                lstAnimalArticle = (List<OffenseRegistrationfinal>)Session["AddAnimalArticle"];
            }          
            return Json(lstAnimalArticle, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// delete the exiting records by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteAnimalArticle(string Id)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> lstAnimalArticle = new List<OffenseRegistrationfinal>();
            try
            {
                if (Session["AddAnimalArticle"] != null)
                {
                    lstAnimalArticle = (List<OffenseRegistrationfinal>)Session["AddAnimalArticle"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        OffenseRegistrationfinal ofreg = lstAnimalArticle.Single(a => a.ArticleAnimalrowid == Id);
                        lstAnimalArticle.Remove(ofreg);
                    }

                    Session["AddAnimalArticle"] = lstAnimalArticle;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstAnimalArticle, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Add the witness details
        /// </summary>
        /// <param name="WitnessName"></param>
        /// <param name="FatherName"></param>
        /// <param name="SpouseName"></param>
        /// <param name="Caste"></param>
        /// <param name="ResidentialAddress1"></param>
        /// <param name="ResidentialAddress2"></param>
        /// <param name="Pincode"></param>
        /// <param name="Village"></param>
        /// <param name="District"></param>
        /// <param name="State"></param>
        /// <param name="PhoneNo"></param>
        /// <param name="PhotoId"></param>
        /// <param name="UploadId"></param>
        /// <param name="WitnessAge"></param>
        /// <param name="StatementDate"></param>
        /// <param name="WitnessStatement"></param>
        /// <param name="UploadSignedStatement"></param>
        /// <returns></returns>
        public JsonResult AddWitness(string Witnessrowid, string EmailId, string WitnessName, string FatherName, string SpouseName,string Category, string Caste, string ResidentialAddress1, string ResidentialAddress2, string Pincode, string Village, string District, string State, string PhoneNo, string PhotoId, string UploadId, string WitnessAge, string StatementDate, string WitnessStatement, string UploadSignedStatement)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
            List<OffenseRegistrationfinal> lstWitness = new List<OffenseRegistrationfinal>();
            try
            {
                OffenseRegist.Witnessrowid = Witnessrowid;
                OffenseRegist.WitnessName = WitnessName;
                OffenseRegist.FatherName = FatherName;
                OffenseRegist.SpouseName = SpouseName;
                OffenseRegist.Category = Category;
                OffenseRegist.Caste = Caste;
                OffenseRegist.ResidentialAddress1 = ResidentialAddress1;
                OffenseRegist.ResidentialAddress2 = ResidentialAddress2;
                OffenseRegist.Pincode = Pincode;
                OffenseRegist.Village = Village;
                OffenseRegist.District = District;
                OffenseRegist.State = State;
                OffenseRegist.PhoneNo = PhoneNo;
                OffenseRegist.PhotoId = PhotoId;
                OffenseRegist.UploadId = UploadId;
                OffenseRegist.EmailId = EmailId;
                OffenseRegist.WitnessAge = WitnessAge;
                OffenseRegist.StatementDate = StatementDate;
                OffenseRegist.WitnessStatement = WitnessStatement;
                OffenseRegist.UploadSignedStatement = UploadSignedStatement;
                if (Session["AddWitness"] != null)
                {                                     
                    List<OffenseRegistrationfinal> list = (List<OffenseRegistrationfinal>)Session["AddWitness"];
                    if (list != null && list.Count>0)
                    {
                        foreach (var item in list)
                        {
                            if (item.Witnessrowid == OffenseRegist.Witnessrowid)
                            {
                                lstWitness.Add(OffenseRegist);
                            }
                            else
                            {
                                lstWitness.Add(item);
                            }
                        }
                    }
                    if (OffenseRegist.Witnessrowid == null || OffenseRegist.Witnessrowid=="")
                    {
                        OffenseRegist.Witnessrowid = Guid.NewGuid().ToString();
                        lstWitness.Add(OffenseRegist);
                    }
                    Session["AddWitness"] = null;
                    Session["AddWitness"] = lstWitness;
                }
                else
                {
                    OffenseRegist.Witnessrowid = Guid.NewGuid().ToString();
                    lstWitness.Add(OffenseRegist);
                    Session["AddWitness"] = lstWitness;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
         
            return Json(lstWitness, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// get the Witness details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetWitness()
        {
            List<OffenseRegistrationfinal> lstWitness = new List<OffenseRegistrationfinal>();
            if (Session["AddWitness"] != null)
            {
                lstWitness = (List<OffenseRegistrationfinal>)Session["AddWitness"];

            }          
            return Json(lstWitness, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Delete the Witness details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteWitness(string Id)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> lstWitness = new List<OffenseRegistrationfinal>();
            try
            {
                if (Session["AddWitness"] != null)
                {
                    lstWitness = (List<OffenseRegistrationfinal>)Session["AddWitness"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        OffenseRegistrationfinal ofreg = lstWitness.Single(a => a.Witnessrowid == Id);
                        lstWitness.Remove(ofreg);
                    }
                    Session["AddWitness"] = lstWitness;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstWitness, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Add the warrant details
        /// </summary>
        /// <param name="NameOfOffender"></param>
        /// <param name="ClothesWorn"></param>
        /// <param name="ColorOfClothes"></param>
        /// <param name="PhysicalAppearance"></param>
        /// <param name="Height"></param>
        /// <param name="OtherSpecialDetails"></param>
        /// <returns></returns>
        public JsonResult AddWarrant(string NameOfOffender, string ClothesWorn, string ColorOfClothes, string PhysicalAppearance, string Height, string OtherSpecialDetails, string Appearancedate, string Appearancetime, string AppearancePlace, string Warrantrowid)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
            List<OffenseRegistrationfinal> lstWarrant = new List<OffenseRegistrationfinal>();
            try
            {
                OffenseRegist.NameOfOffender = NameOfOffender;
                OffenseRegist.ClothesWorn = ClothesWorn;
                OffenseRegist.ColorOfClothes = ColorOfClothes;
                OffenseRegist.PhysicalAppearance = PhysicalAppearance;
                OffenseRegist.Height = Height;
                OffenseRegist.OtherSpecialDetails = OtherSpecialDetails;
                OffenseRegist.Appearancedate = Appearancedate;
                OffenseRegist.Appearancetime = Appearancetime;
                OffenseRegist.AppearancePlace = AppearancePlace;
                OffenseRegist.Warrantrowid = Warrantrowid;
                if (Session["AddWarrant"] != null)
                {
                    List<OffenseRegistrationfinal> lstWarrant1 = (List<OffenseRegistrationfinal>)Session["AddWarrant"];
                    if (lstWarrant1 != null && lstWarrant1.Count > 0)
                    {
                        foreach (var item in lstWarrant1)
                        {
                            if (item.Warrantrowid == OffenseRegist.Warrantrowid)
                            {
                                if (lstWarrant.Any(a => a.Warrantrowid == OffenseRegist.Warrantrowid))
                                {
                                    OffenseRegistrationfinal ofreg = lstWarrant.Single(a => a.Warrantrowid == OffenseRegist.Warrantrowid);
                                    lstWarrant.Remove(ofreg);
                                    lstWarrant.Add(OffenseRegist);
                                }
                                else
                                {
                                    lstWarrant.Add(OffenseRegist);
                                }
                            }
                            else
                            {
                                lstWarrant.Add(item);
                            }
                        }
                    }
                    Session["AddWarrant"] = null;
                    Session["AddWarrant"] = lstWarrant;
                }
                else
                {
                    OffenseRegist.Warrantrowid = Guid.NewGuid().ToString();
                    lstWarrant.Add(OffenseRegist);
                    Session["AddWarrant"] = lstWarrant;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstWarrant, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Fetch the existing records
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetWarrant()
        {
            List<OffenseRegistrationfinal> lstWarrant = new List<OffenseRegistrationfinal>();
            if (Session["AddWarrant"] != null)
            {
                lstWarrant = (List<OffenseRegistrationfinal>)Session["AddWarrant"];
            }           
            return Json(lstWarrant, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// delete the older records by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteWarrant(string Id)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<OffenseRegistrationfinal> lstWarrant = new List<OffenseRegistrationfinal>();
            try
            {
                if (Session["AddWarrant"] != null)
                {
                    lstWarrant = (List<OffenseRegistrationfinal>)Session["AddWarrant"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        OffenseRegistrationfinal ofreg = lstWarrant.Single(a => a.Warrantrowid == Id);
                        lstWarrant.Remove(ofreg);
                    }
                    Session["AddWarrant"] = lstWarrant;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstWarrant, JsonRequestBehavior.AllowGet);
        }
        /// function use to add warrant details
        /// </summary>
        /// <param name="OffenderName"></param>
        /// <returns></returns>
        public JsonResult GetWarrantDetail(string OffenderName)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OffenseRegistrationfinal OffenseRegist1 = new OffenseRegistrationfinal();
            OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
            List<OffenseRegistrationfinal> lstWarrant = new List<OffenseRegistrationfinal>();
            List<OffenseRegistrationfinal> lstWarrant1 = new List<OffenseRegistrationfinal>();
            DataTable dt = new DataTable();
            dt = OffenseRegist1.GetOffenderWarrantDetail(OffenderName);
            try
            {
                OffenseRegist.NameOfOffender = dt.Rows[0]["OffenderName"].ToString();
                OffenseRegist.ClothesWorn = dt.Rows[0]["ClothesWorn"].ToString();
                OffenseRegist.ColorOfClothes = dt.Rows[0]["ClothesColor"].ToString();
                OffenseRegist.PhysicalAppearance = dt.Rows[0]["PhysicalAppearance"].ToString();
                OffenseRegist.Height = dt.Rows[0]["height"].ToString();
                OffenseRegist.OtherSpecialDetails = dt.Rows[0]["OtherSpecialDetails"].ToString();

                #region MultipleOffender
                if (Session["AddWarrant"] != null)
                {
                    lstWarrant1 = (List<OffenseRegistrationfinal>)Session["AddWarrant"];
                    if (!lstWarrant1.Any(a => a.NameOfOffender == OffenseRegist.NameOfOffender && a.ClothesWorn == OffenseRegist.ClothesWorn && a.ColorOfClothes == OffenseRegist.ColorOfClothes && a.PhysicalAppearance == OffenseRegist.PhysicalAppearance && a.Height == OffenseRegist.Height && a.OtherSpecialDetails == OffenseRegist.OtherSpecialDetails))
                    {
                        foreach (var item in lstWarrant1)
                        {
                            if (item.Warrantrowid != null && item.Warrantrowid != "")
                            {
                                lstWarrant.Add(item);
                            }
                        }
                        if (OffenseRegist.Warrantrowid == null || OffenseRegist.Warrantrowid == "")
                        {
                            OffenseRegist.Warrantrowid = Guid.NewGuid().ToString();
                            lstWarrant.Add(OffenseRegist);
                        }
                    }
                    else
                    {
                        foreach (var item in lstWarrant1)
                        {
                            if (item.Warrantrowid != null && item.Warrantrowid != "")
                            {
                                OffenseRegist.Warrantrowid = item.Warrantrowid;
                                lstWarrant.Add(OffenseRegist);
                            }
                        }
                    }
                    Session["AddWarrant"] = null;
                    Session["AddWarrant"] = lstWarrant;
                }
                else
                {
                    OffenseRegist.Warrantrowid = Guid.NewGuid().ToString();
                    lstWarrant.Add(OffenseRegist);
                    Session["AddWarrant"] = lstWarrant;
                }
                #endregion

            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstWarrant, JsonRequestBehavior.AllowGet);
        }    
        /// <summary>
        /// Save the records of Seized item tab
        /// </summary>
        /// <param name="ObjSeizedItem"></param>
        /// <param name="frm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitSeizedItem(OffenseRegistrationfinal ObjSeizedItem, FormCollection frm)
        {
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            try
            {
                actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                DataTable dtVechile = VechileDescription();
                DataTable dtProduce = ProduceDescription();
                DataTable dtEquipment = EquipmentDescription();
                DataTable dtAnimal = AnimalDescription();
                DataTable dtAnimalDescription = AnimalArticleDescription();
                if (dtVechile.Rows.Count > 0 || dtProduce.Rows.Count > 0 || dtEquipment.Rows.Count > 0 || dtAnimal.Rows.Count > 0 || dtAnimalDescription.Rows.Count > 0)
                {
                    //if (frm["SeizedItemId"].ToString() == "")
                    //{
                    //    ObjSeizedItem.SeizedItemId = Convert.ToInt64("0");
                    //}
                    //else
                    //{
                    //    ObjSeizedItem.SeizedItemId = Convert.ToInt64(frm["SeizedItemId"].ToString());
                    //}

                    Int64 status = ObjSeizedItem.InsertSeizedItems(dtVechile, dtProduce, dtEquipment, dtAnimal, dtAnimalDescription);
                    if (status > 0)
                    {
                        TempData["status"] = "Record save sucessfully";
                        ViewBag.TabInfo = "#tab3default";
                    }
                    else {
                        TempData["status"] = "No data inserted add team detail first!!!";
                        ViewBag.TabInfo = "#tab2default";
                    }                    
                }
                else
                {
                    TempData["status"] = "Add atleast one seized item to save";
                    ViewBag.TabInfo = "#tab2default";
                }
                       
                getDropdown();
                OR = getdataview();
                OR.Iscomplete = OR.TabShowInfo("3");
                OR.ApproveStatus = OR.TabShowInfo("4");
                OR.DfoApproveStatus = OR.TabShowInfo("5");   
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return View("OffenseRegistrationfinal",OR);
        }
        /// <summary>
        /// Save the records of Seized item tab
        /// </summary>
        /// <param name="ObjSeizedItem"></param>
        /// <param name="frm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitSeizedItemTeam(OffenseRegistrationfinal ObjSeizedTeam)
        {
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            try
            {
                actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                 Int64 status = ObjSeizedTeam.InsertSeizedItemTeam();
                    if (status > 0)
                    {
                        TempData["status"] = "Record save sucessfully";
                        ViewBag.TabInfo = "#tab2default";
                    }
                    else
                    {
                        TempData["status"] = "No data inserted";
                        ViewBag.TabInfo = "#tab1default";
                    }                 
                getDropdown();
                OR = getdataview();
                OR.Iscomplete = OR.TabShowInfo("3");
                OR.ApproveStatus = OR.TabShowInfo("4");
                OR.DfoApproveStatus = OR.TabShowInfo("5");
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return View("OffenseRegistrationfinal", OR);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjWitnessItem"></param>
        /// <param name="frm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitWitnessDetails(OffenseRegistrationfinal ObjWitnessItem, FormCollection frm)
        {
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            try
            {
                actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                DataTable dtWitnessDetails = WitnessDescription();
                if (dtWitnessDetails.Rows.Count > 0)
                {
                    if (frm["WitnessId"].ToString() == "")
                    {
                        ObjWitnessItem.WitnessId = Convert.ToInt64("0");
                    }
                    else
                    {
                        ObjWitnessItem.WitnessId = Convert.ToInt64(frm["WitnessId"].ToString());
                    }

                    Int64 status = ObjWitnessItem.InsertWitnessDetails(dtWitnessDetails);
                    TempData["status"] = "Record save sucessfully";
                    ViewBag.TabInfo = "#tab3default";        
                }
                else
                {
                     ViewBag.TabInfo = "#tab3default";
                    TempData["status"] = "Add atleast one witness details to save!!!";
                }                       
                getDropdown();
                OR = getdataview();
                OR.Iscomplete = OR.TabShowInfo("3");
                OR.ApproveStatus = OR.TabShowInfo("4");
                OR.DfoApproveStatus = OR.TabShowInfo("5");     
            }                       
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            
            return View("OffenseRegistrationfinal",OR);
        }
        /// <summary>
        /// Save the Warrant details
        /// </summary>
        /// <param name="ObjWarrantItem"></param>
        /// <param name="frm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitWarrantDetails(OffenseRegistrationfinal ObjWarrantItem, FormCollection frm)
        {
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            try
            {
                actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                DataTable dtWarrantDetails = WarrantDescription();
                if (dtWarrantDetails.Rows.Count > 0)
                {                   
                    Int64 status = ObjWarrantItem.InsertWarrantDetails(dtWarrantDetails);
                    if (status != 0)
                    {
                        TempData["status"] = "Record save sucessfully";
                    }
                    else
                    {
                        TempData["status"] = "Warrant detail already exists";
                    }
                    ViewBag.TabInfo = "#tab5default1";
                }
                else
                {
                    TempData["status"] = "Add atleast one warrant details to save!!!";
                    ViewBag.TabInfo = "#tab5default1";

                }                             
                getDropdown();
                OR = getdataview();
                OR.Iscomplete = OR.TabShowInfo("3");
                OR.ApproveStatus = OR.TabShowInfo("4");
                OR.DfoApproveStatus = OR.TabShowInfo("5");
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return View("OffenseRegistrationfinal",OR);
        }

        [HttpPost]
        public ActionResult SubmitWarrantDelivery(OffenseRegistrationfinal _objDelivery)
        {
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                string status = _objDelivery.InsertWarrantDelivery();
                if (status == "UP")
                {
                    TempData["status"] = "Record save sucessfully";
                    ViewBag.TabInfo = "#tab5default2";
                }
                if (status == "NA")
                {
                    TempData["status"] = "Record not save as warrant approval is pending!!!";
                    ViewBag.TabInfo = "#tab5default2";
                }
                if (status == "NF")
                {
                    TempData["status"] = "Record not save as Offense code not exists!!!";
                    ViewBag.TabInfo = "#tab5default2";
                }
                getDropdown();
                OR = getdataview();
                OR.Iscomplete = OR.TabShowInfo("3");
                OR.ApproveStatus = OR.TabShowInfo("4");
                OR.DfoApproveStatus = OR.TabShowInfo("5");
                OR.WarrantApproveStatus = OR.TabShowInfo("6");
            }
            catch (Exception ex) { new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return View("OffenseRegistrationfinal", OR);
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
        public ActionResult SubmitFileCourtCase(OffenseRegistrationfinal ObjCourtCaseItem, FormCollection frm, HttpPostedFileBase InterimOrder, HttpPostedFileBase FinalJudgmentOrder)
        {
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            string Document = string.Empty;
            var path = "";
            string Document1 = string.Empty;
            var path1 = "";
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
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
                Int64 status = ObjCourtCaseItem.InsertFileCourtCase();
                if (status != 0)
                {
                    TempData["status"] = "Record save sucessfully";
                    ViewBag.TabInfo = "#tab6default";
                }
                else
                {
                    TempData["status"] = "Court Case detail already exists";
                     ViewBag.TabInfo = "#tab6default";
                }                                     
                getDropdown();
                BindOffence();
                OffenseList();               
                OR = getdataview();
                OR.Iscomplete = OR.TabShowInfo("3");
                OR.ApproveStatus = OR.TabShowInfo("4");
                OR.DfoApproveStatus = OR.TabShowInfo("5");      
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return View("OffenseRegistrationfinal",OR);
        }
        /// <summary>
        /// Submit issue jamannt details
        /// </summary>
        /// <param name="ObjJamanatItem"></param>
        /// <param name="frm"></param>
        /// <param name="IdProof"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitIssueJamanat(OffenseRegistrationfinal ObjJamanatItem, FormCollection frm, HttpPostedFileBase IdProof)
        {
            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            string Document = string.Empty;
            var path = "";
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (IdProof != null && IdProof.ContentLength > 0)
                {
                    Document = Path.GetFileName(IdProof.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(Server.MapPath("~/ForestProtectionDocument/"), FileFullName);
                    ObjJamanatItem.IdProof = path;
                    IdProof.SaveAs(Server.MapPath("~/ForestProtectionDocument/") + FileFullName);
                }
                else
                {
                    ObjJamanatItem.IdProof = "";
                }


                if (frm["JamanatId"].ToString() == "")
                {
                    ObjJamanatItem.JamanatId = Convert.ToInt64("0");
                }
                else
                {
                    ObjJamanatItem.JamanatId = Convert.ToInt64(frm["JamanatId"].ToString());
                }
                Int64 status = ObjJamanatItem.InsertIssueJamanat();
                if (status != 0)
                {
                    TempData["status"] = "Record save sucessfully";
                    ViewBag.TabInfo = "#tab7default";
                }
                else
                {
                    TempData["status"] = "Jammant details already exists";
                      ViewBag.TabInfo = "#tab7default";
                }                              
                getDropdown();
                BindOffence();
                OffenseList();       
                OR = getdataview();
                OR.Iscomplete = OR.TabShowInfo("3");
                OR.ApproveStatus = OR.TabShowInfo("4");
                OR.DfoApproveStatus = OR.TabShowInfo("5");        
            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return View("OffenseRegistrationfinal",OR);
        }
        /// <summary>
        /// Submit final details
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FinalSubmit()
        {


            OffenseRegistrationfinal OR = new OffenseRegistrationfinal();
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                string status = OR.InsertFinalDetails();
                if (status == "STNF")
                {
                    getDropdown();
                    BindOffence();
                    OffenseList();
                    OR = getdataview();
                    OR.Iscomplete = OR.TabShowInfo("3");
                    OR.ApproveStatus = OR.TabShowInfo("4");
                    OR.DfoApproveStatus = OR.TabShowInfo("5");
                    ViewBag.TabInfo = "#tab1default";
                    TempData["status"] = "Fill assisting team details to proceed";
                }
                if (status == "WDNF")
                {
                    getDropdown();
                    BindOffence();
                    OffenseList();
                    OR = getdataview();
                    OR.Iscomplete = OR.TabShowInfo("3");
                    OR.ApproveStatus = OR.TabShowInfo("4");
                    OR.DfoApproveStatus = OR.TabShowInfo("5");
                    ViewBag.TabInfo = "#tab3default";
                    TempData["status"] = "Fill Witness details to proceed";
                }
                if (status != "STNF" && status != "WDNF")
                {
                    ModelState.Clear();
                    Session["AddCrime"] = null;
                    Session["AddVechile"] = null;
                    Session["AddProduce"] = null;
                    Session["AddEquipment"] = null;
                    Session["AddAnimal"] = null;
                    Session["AddAnimalArticle"] = null;
                    Session["AddWitness"] = null;
                    Session["AddWarrant"] = null;
                    OR = getdataview();
                    TempData["ForesterParivad"] = "Record save sucessfully";
                    Session["Servicetype"] = "Protection";
                    return RedirectToAction("ForesterAction", "ForesterAction");
                }

            }
            catch (Exception ex) { new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return View("OffenseRegistrationfinal", OR);
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
            dtVechile.Columns.Add("PastOffenses", typeof(String));
            dtVechile.Columns.Add("UploadDoc", typeof(String));
            dtVechile.AcceptChanges();
            List<OffenseRegistrationfinal> lstSeized = new List<OffenseRegistrationfinal>();
            if (Session["AddVechile"] != null)
            {
                lstSeized = (List<OffenseRegistrationfinal>)Session["AddVechile"];
            }
            foreach (OffenseRegistrationfinal objSeized in lstSeized)
            {
                DataRow dr = dtVechile.NewRow();
                dr["VehicleRegistrationNo"] = objSeized.VechileRegistrationNo;
                dr["OwnerName"] = objSeized.VechileOwnerName;
                dr["VehicleType"] = objSeized.VechileType;
                dr["VehicleMake"] = objSeized.VechileMake;
                dr["VehicleModel"] = objSeized.VechileModel;
                dr["ChassisNo"] = objSeized.VechileChassisNo;
                dr["EngineNo"] = objSeized.VechileEngineNo;
                dr["PastOffenses"] = objSeized.PastOffensesByVechile;
                dr["UploadDoc"] = objSeized.VechileUploadDoc;
                dtVechile.Rows.Add(dr);
                dtVechile.AcceptChanges();
            }
            return dtVechile;
        }
        /// <summary>
        /// Get production description
        /// </summary>
        /// <returns></returns>
        public DataTable ProduceDescription()
        {
            DataTable dtProduce = new DataTable("Table");
            dtProduce.Columns.Add("ProduceTypeId", typeof(String));
            dtProduce.Columns.Add("SpeciesName", typeof(String));
            dtProduce.Columns.Add("Quantity", typeof(String));
            dtProduce.Columns.Add("UploadDoc", typeof(String));
            dtProduce.AcceptChanges();
            List<OffenseRegistrationfinal> lstProduce = new List<OffenseRegistrationfinal>();
            if (Session["AddProduce"] != null)
            {
                lstProduce = (List<OffenseRegistrationfinal>)Session["AddProduce"];
            }
            foreach (OffenseRegistrationfinal objProduce in lstProduce)
            {
                DataRow dr = dtProduce.NewRow();
                dr["ProduceTypeId"] = objProduce.ProduceType;
                dr["SpeciesName"] = objProduce.SpeciesName;
                dr["Quantity"] = objProduce.QuantityOfProduce;
                dr["UploadDoc"] = objProduce.ProduceUploadDoc;
                dtProduce.Rows.Add(dr);
                dtProduce.AcceptChanges();
            }
            return dtProduce;
        }
        /// <summary>
        /// Get equipment description
        /// </summary>
        /// <returns></returns>
        public DataTable EquipmentDescription()
        {
            DataTable dtEquipment = new DataTable("Table");
            dtEquipment.Columns.Add("EquipmentTypeId", typeof(String));
            dtEquipment.Columns.Add("Make", typeof(String));
            dtEquipment.Columns.Add("Model", typeof(String));
            dtEquipment.Columns.Add("Caliber", typeof(String));
            dtEquipment.Columns.Add("IdentificationNo", typeof(String));
            dtEquipment.Columns.Add("size", typeof(String));
            dtEquipment.Columns.Add("UploadDoc", typeof(String));
            dtEquipment.AcceptChanges();
            List<OffenseRegistrationfinal> lstEquipment = new List<OffenseRegistrationfinal>();
            if (Session["AddEquipment"] != null)
            {
                lstEquipment = (List<OffenseRegistrationfinal>)Session["AddEquipment"];
            }
            foreach (OffenseRegistrationfinal objEquipment in lstEquipment)
            {
                DataRow dr = dtEquipment.NewRow();
                dr["EquipmentTypeId"] = objEquipment.EquipmentType;
                dr["Make"] = objEquipment.EquipmentMake;
                dr["Model"] = objEquipment.EquipmentModel;
                dr["Caliber"] = objEquipment.EquipmentCaliber;
                dr["IdentificationNo"] = objEquipment.EquipmentIdentificationNo;
                dr["size"] = objEquipment.EquipmentSize;
                dr["UploadDoc"] = objEquipment.EquipmentUploadDoc;
                dtEquipment.Rows.Add(dr);
                dtEquipment.AcceptChanges();
            }
            return dtEquipment;
        }
        /// <summary>
        /// Get amimal description
        /// </summary>
        /// <returns></returns>
        public DataTable AnimalDescription()
        {
            DataTable dtAnimal = new DataTable("Table");
            dtAnimal.Columns.Add("AnimalScientificName", typeof(String));
            dtAnimal.Columns.Add("AnimalCommanName", typeof(String));
            dtAnimal.Columns.Add("AnimalDescription", typeof(String));
            dtAnimal.Columns.Add("AnimalWeight", typeof(String));
            dtAnimal.Columns.Add("UploadDoc", typeof(String));
            dtAnimal.AcceptChanges();
            List<OffenseRegistrationfinal> lstAnimal = new List<OffenseRegistrationfinal>();
            if (Session["AddAnimal"] != null)
            {
                lstAnimal = (List<OffenseRegistrationfinal>)Session["AddAnimal"];
            }
            foreach (OffenseRegistrationfinal objAnimal in lstAnimal)
            {
                DataRow dr = dtAnimal.NewRow();
                dr["AnimalScientificName"] = objAnimal.AnimalScientificName;
                dr["AnimalCommanName"] = objAnimal.AnimalCommanName;
                dr["AnimalDescription"] = objAnimal.AnimalDescription;
                dr["AnimalWeight"] = objAnimal.AnimalWeight;
                dr["UploadDoc"] = objAnimal.AnimalUploadDoc;
                dtAnimal.Rows.Add(dr);
                dtAnimal.AcceptChanges();
            }
            return dtAnimal;
        }
        /// <summary>
        /// Get animal article description
        /// </summary>
        /// <returns></returns>
        public DataTable AnimalArticleDescription()
        {
            DataTable dtAnimalArticle = new DataTable("Table");
            dtAnimalArticle.Columns.Add("ScientificName", typeof(String));
            dtAnimalArticle.Columns.Add("CommanName", typeof(String));
            dtAnimalArticle.Columns.Add("AnimalArticleName", typeof(String));
            dtAnimalArticle.Columns.Add("AnimalArticleDescription", typeof(String));
            dtAnimalArticle.Columns.Add("Quantity", typeof(String));
            dtAnimalArticle.Columns.Add("UploadDoc", typeof(String));
            dtAnimalArticle.AcceptChanges();
            List<OffenseRegistrationfinal> lstAnimalArticle = new List<OffenseRegistrationfinal>();
            if (Session["AddAnimalArticle"] != null)
            {
                lstAnimalArticle = (List<OffenseRegistrationfinal>)Session["AddAnimalArticle"];
            }
            foreach (OffenseRegistrationfinal objAnimalArticle in lstAnimalArticle)
            {
                DataRow dr = dtAnimalArticle.NewRow();
                dr["ScientificName"] = objAnimalArticle.ArticleAnimalScientificName;
                dr["CommanName"] = objAnimalArticle.ArticleAnimalCommanName;
                dr["AnimalArticleName"] = objAnimalArticle.NameOfAnimalArticle;
                dr["AnimalArticleDescription"] = objAnimalArticle.DescriptionOfAnimalArticle;
                dr["Quantity"] = objAnimalArticle.ArticleQuantity;
                dr["UploadDoc"] = objAnimalArticle.AnimalArticleUploadDoc;
                dtAnimalArticle.Rows.Add(dr);
                dtAnimalArticle.AcceptChanges();
            }
            return dtAnimalArticle;
        }
        /// <summary>
        /// Get witness description
        /// </summary>
        /// <returns></returns>
        public DataTable WitnessDescription()
        {
            DataTable dtWitness = new DataTable("Table");
            dtWitness.Columns.Add("WitnessName", typeof(String));
            dtWitness.Columns.Add("FatherName", typeof(String));
            dtWitness.Columns.Add("SpouseName", typeof(String));
            dtWitness.Columns.Add("Category", typeof(String));
            dtWitness.Columns.Add("Caste", typeof(String));
            dtWitness.Columns.Add("ResidentialAddress1", typeof(String));
            dtWitness.Columns.Add("ResidentialAddress2", typeof(String));
            dtWitness.Columns.Add("State", typeof(String));
            dtWitness.Columns.Add("Pincode", typeof(String));
            dtWitness.Columns.Add("Village", typeof(String));
            dtWitness.Columns.Add("District", typeof(String));           
            dtWitness.Columns.Add("PhoneNo", typeof(String));
            dtWitness.Columns.Add("EmailId", typeof(String));
            dtWitness.Columns.Add("PhotoId", typeof(String));
            dtWitness.Columns.Add("UploadId", typeof(String));
            dtWitness.Columns.Add("WitnessAge", typeof(String));
            dtWitness.Columns.Add("StatementDate", typeof(String));
            dtWitness.Columns.Add("WitnessStatement", typeof(String));
            dtWitness.Columns.Add("UploadSignedStatement", typeof(String));
            dtWitness.AcceptChanges();
            List<OffenseRegistrationfinal> lstWitness = new List<OffenseRegistrationfinal>();
            if (Session["AddWitness"] != null)
            {
                lstWitness = (List<OffenseRegistrationfinal>)Session["AddWitness"];
            }
            foreach (OffenseRegistrationfinal objWitness in lstWitness)
            {
                DataRow dr = dtWitness.NewRow();
                dr["WitnessName"] = objWitness.WitnessName;
                dr["FatherName"] = objWitness.FatherName;
                dr["SpouseName"] = objWitness.SpouseName;
                dr["Category"] = objWitness.Category;
                dr["Caste"] = objWitness.Caste;
                dr["ResidentialAddress1"] = objWitness.ResidentialAddress1;
                dr["ResidentialAddress2"] = objWitness.ResidentialAddress2;
                dr["Pincode"] = objWitness.Pincode;
                dr["Village"] = objWitness.Village;
                dr["District"] = objWitness.District;
                dr["State"] = objWitness.State;
                dr["PhoneNo"] = objWitness.PhoneNo;
                dr["EmailId"] = objWitness.EmailId;
                dr["PhotoId"] = objWitness.PhotoId;
                dr["UploadId"] = objWitness.UploadId;
                dr["WitnessAge"] = objWitness.WitnessAge;
                dr["StatementDate"] = objWitness.StatementDate;
                dr["WitnessStatement"] = objWitness.WitnessStatement;
                dr["UploadSignedStatement"] = objWitness.UploadSignedStatement;
                dtWitness.Rows.Add(dr);
                dtWitness.AcceptChanges();
            }
            return dtWitness;

        }
        /// <summary>
        /// Get warrant description
        /// </summary>
        /// <returns></returns>
        public DataTable WarrantDescription()
        {
            DataTable dtWarrant = new DataTable("Table");
            dtWarrant.Columns.Add("NameOfOffender", typeof(String));
            dtWarrant.Columns.Add("ClothesWorn", typeof(String));
            dtWarrant.Columns.Add("ColorOfClothes", typeof(String));
            dtWarrant.Columns.Add("PhysicalAppearance", typeof(String));
            dtWarrant.Columns.Add("Height", typeof(String));
            dtWarrant.Columns.Add("OtherSpecialDetails", typeof(String));
            dtWarrant.Columns.Add("Appearancedate", typeof(String));
            dtWarrant.Columns.Add("Appearancetime", typeof(String));
            dtWarrant.Columns.Add("AppearancePlace", typeof(String));
            dtWarrant.AcceptChanges();
            List<OffenseRegistrationfinal> lstWarrant = new List<OffenseRegistrationfinal>();
            if (Session["AddWarrant"] != null)
            {
                lstWarrant = (List<OffenseRegistrationfinal>)Session["AddWarrant"];
            }
            foreach (OffenseRegistrationfinal objWarrant in lstWarrant)
            {
                DataRow dr = dtWarrant.NewRow();
                dr["NameOfOffender"] = objWarrant.NameOfOffender;
                dr["ClothesWorn"] = objWarrant.ClothesWorn;
                dr["ColorOfClothes"] = objWarrant.ColorOfClothes;
                dr["PhysicalAppearance"] = objWarrant.PhysicalAppearance;
                dr["Height"] = objWarrant.Height;
                dr["OtherSpecialDetails"] = objWarrant.OtherSpecialDetails;
                dr["Appearancedate"] = objWarrant.Appearancedate;
                dr["Appearancetime"] = objWarrant.Appearancetime;
                dr["AppearancePlace"] = objWarrant.AppearancePlace;
                dtWarrant.Rows.Add(dr);
                dtWarrant.AcceptChanges();
            }
            return dtWarrant;
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
                    //  Get all files from Request object  
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
        /// function use for edit witness details
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditDetails(string ID)
        {
            try
            {
                OffenseRegistrationfinal OffenseRegist = new OffenseRegistrationfinal();
                List<OffenseRegistrationfinal> lstWitness = new List<OffenseRegistrationfinal>();
                List<OffenseRegistrationfinal> lstWitnessEdit = new List<OffenseRegistrationfinal>();
                if (Session["AddWitness"] != null)
                {
                    lstWitness = (List<OffenseRegistrationfinal>)Session["AddWitness"];

                    if (ID != "0" && ID.Length > 0)
                    {
                        OffenseRegistrationfinal witness = lstWitness.Single(a => a.Witnessrowid == ID);
                        lstWitnessEdit.Add(witness);
                    }
                }
                foreach (var item in lstWitnessEdit)
                {
                    OffenseRegist.WitnessName = item.WitnessName;
                    OffenseRegist.FatherName = item.FatherName;
                    OffenseRegist.SpouseName = item.SpouseName;
                    OffenseRegist.Category = item.Category;
                    OffenseRegist.Caste = item.Caste;
                    OffenseRegist.ResidentialAddress1 = item.ResidentialAddress1;
                    OffenseRegist.ResidentialAddress2 = item.ResidentialAddress2;
                    OffenseRegist.Pincode = item.Pincode;
                    OffenseRegist.Village = item.Village;
                    OffenseRegist.District = item.District;
                    OffenseRegist.State = item.State;
                    OffenseRegist.PhoneNo = item.PhoneNo;
                    OffenseRegist.PhotoId = item.PhotoId;
                    OffenseRegist.EmailId = item.EmailId;
                    OffenseRegist.UploadId = item.UploadId;
                    OffenseRegist.WitnessAge = item.WitnessAge;
                    OffenseRegist.StatementDate = item.StatementDate;
                    OffenseRegist.WitnessStatement = item.WitnessStatement;
                    OffenseRegist.UploadSignedStatement = item.UploadSignedStatement;
                }
                return Json(OffenseRegist, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }




        public JsonResult GetRTOVechileRDetails(string VechileRegistrationNumber)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {

                cls_RTODetails.VehicleDetails RTOVechileXmlData;
                 RTOVechileXmlData =  cls_RTODetails.GetRTO(VechileRegistrationNumber);
                return Json(RTOVechileXmlData);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return null;
        }


    }
   
}

