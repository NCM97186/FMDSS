//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : DFODecisionController
//  Description  : File contains calling functions from UI
//  Date Created : 29-Mar-2016
//  History      : Add the Details of DFO Decision
//  Version      : 1.0
//  Author       : Ashok Yadav
//  Modified By  :  
//  History      :  
//  Modified On  :  
//  Reviewed By  : 
//  Reviewed On  : 
//bug no-476

using System;
using System.Data;
using System.Collections.Generic;
using FMDSS.Models;
using System.Web.Mvc;
using FMDSS.Filters;
using FMDSS.Models.ForestProtection;
using System.Web;
using System.IO;

namespace FMDSS.Controllers.ForestProtection
{
    [MyAuthorization]
    [MyExceptionHandler]
    public class DFODecisionController : BaseController
    {
        int ModuleID = 4;
        string actionName = string.Empty;
        string controllerName = string.Empty;
        /// <summary>
        /// Get DFO Decision list
        /// </summary>
        /// <returns></returns>
        public ActionResult DFODecision()
        {
            GetDropdown();
            return View();
        }
        /// <summary>
        /// Get Offense Category list
        /// </summary>
        public void GetDropdown()
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DFODecision dfo = new DFODecision();
            DataSet dsCategory = new DataSet();
            List<SelectListItem> lstCategory = new List<SelectListItem>();
            try
            {
                dfo.option = "1";
                dsCategory = dfo.GetOffenseCategory();
                foreach (System.Data.DataRow dr in dsCategory.Tables[0].Rows)
                {
                    lstCategory.Add(new SelectListItem { Text = @dr["FOCategory"].ToString(), Value = @dr["FOCatID"].ToString() });
                }
                ViewBag.OCategory = lstCategory;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
        }
        /// <summary>
        /// Get Offense Code for dfo decision
        /// </summary>
        /// <param name="OffenseCategory"></param>
        /// <returns></returns>
        public JsonResult GetOffenseCode(string OffenseCategory)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DFODecision dfo = null;
            List<SelectListItem> lstOffense = new List<SelectListItem>();
            try
            {
                dfo = new DFODecision();
                dfo.option = "2";
                dfo.OffenseCategory = OffenseCategory;
                DataSet dsOffense = new DataSet();
                dsOffense = dfo.GetOffenseCategory();
                foreach (System.Data.DataRow dr in dsOffense.Tables[0].Rows)
                {
                    lstOffense.Add(new SelectListItem { Text = @dr["OffenseCode"].ToString(), Value = @dr["OffenseCode"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(lstOffense, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get details on Offense Code
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOffenseDetails(string OffenseCode)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DFODecision dfode = null;
            DFODecision dfode1 = null;
            DFODecision dfode2 = null;
            DFODecision dfode3 = null;
            DFODecision dfoPunish = null;
            DFODecision dfo = new DFODecision();
            DataSet ds = new DataSet();
            List<DFODecision> lstOffender = new List<DFODecision>();
            List<DFODecision> lstWitness = new List<DFODecision>();
            List<DFODecision> lstdfo = new List<DFODecision>();
            List<DFODecision> lstdfo3 = new List<DFODecision>();
            List<DFODecision> Punishment = new List<DFODecision>();
            try
            {
                dfo.option = "3";
                dfo.OffenseCode = OffenseCode;
                ds = dfo.GetOffenseCategory();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dfode = new DFODecision();
                        dfode.SectionForest = ds.Tables[0].Rows[i]["Forest_Protection_Act"].ToString();
                        dfode.SectionWildlife = ds.Tables[0].Rows[i]["Wildlife_Protection_Act"].ToString();
                        dfode.District = ds.Tables[0].Rows[i]["District"].ToString();
                        dfode.OffenseDate = ds.Tables[0].Rows[i]["OffenseDate"].ToString();
                        dfode.OffenseTime = ds.Tables[0].Rows[i]["OffenseTime"].ToString();
                        dfode.OffensePlace = ds.Tables[0].Rows[i]["OffensePlace"].ToString();
                        dfode.Landmark = ds.Tables[0].Rows[i]["Landmark"].ToString();
                        dfode.DistanceFromNaka = ds.Tables[0].Rows[i]["DistanceFromNaka"].ToString();
                        dfode.OffenseName = ds.Tables[0].Rows[i]["OffenderName"].ToString();
                        dfode.OffenseStatement = ds.Tables[0].Rows[i]["OffenderStatement"].ToString();
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            dfode.FineAmount = ds.Tables[4].Rows[0]["PunishmentAmount"].ToString();
                        }
                        lstOffender.Add(dfode);
                    }
                    
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        dfode1 = new DFODecision();
                        dfode1.SeizedItem = ds.Tables[1].Rows[i]["SeizedItem"].ToString();
                        dfode1.RegistrationNo = ds.Tables[1].Rows[i]["RegistrationNo"].ToString();
                        dfode1.Name = ds.Tables[1].Rows[i]["Name"].ToString();
                        dfode1.FirstOfficer = ds.Tables[1].Rows[i]["FirstOfficer"].ToString();
                        dfode1.SecondOfficer = ds.Tables[1].Rows[i]["SecondOfficer"].ToString();
                        lstdfo.Add(dfode1);
                    }

                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        dfode2 = new DFODecision();
                        dfode2.WitnessName = ds.Tables[2].Rows[i]["WitnessName"].ToString();
                        dfode2.WitnessPhone = ds.Tables[2].Rows[i]["PhoneNo"].ToString();
                        dfode2.WitnessAddress = ds.Tables[2].Rows[i]["Address1"].ToString();
                        dfode2.WitnessVillage = ds.Tables[2].Rows[i]["VILL_NAME"].ToString();
                        dfode2.WitnessStatement = ds.Tables[2].Rows[i]["WitnessStatement"].ToString();
                        lstWitness.Add(dfode2);
                    }
                  
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        dfode3 = new DFODecision();
                        dfode3.OffenseCode = ds.Tables[3].Rows[i]["OffenseCode"].ToString();
                        dfode3.OffenseCategory = ds.Tables[3].Rows[i]["FOCategory"].ToString();
                        dfode3.ForestCategory = ds.Tables[3].Rows[i]["Forest_Protection_Act"].ToString();
                        dfode3.SectionForest = ds.Tables[3].Rows[i]["FOSubCategory"].ToString();
                        dfode3.WildlifeCategory = ds.Tables[3].Rows[i]["Wildlife_Protection_Act"].ToString();
                        dfode3.SectionWildlife = ds.Tables[3].Rows[i]["WOSubCategory"].ToString();
                        dfode3.DFODecisionTaken = ds.Tables[3].Rows[i]["DfoDecision"].ToString();
                        dfode3.CaseStatus = ds.Tables[3].Rows[i]["CaseStatus"].ToString();
                        dfode3.FineAmount = ds.Tables[3].Rows[i]["FineAmount"].ToString();
                        dfode3.ItemSeized = ds.Tables[3].Rows[i]["ItemSeized"].ToString();
                        dfode3.Compounding = ds.Tables[3].Rows[i]["Compounding"].ToString();
                        dfode3.OffenceDescription = ds.Tables[3].Rows[i]["Description"].ToString();
                        lstdfo3.Add(dfode3);                    
                    }                    
                }
                DataSet DS = new DataSet();
                DFODecision dfd = new DFODecision();
                dfd.OffenseCode = OffenseCode;
                DS = dfo.GetPunishment(dfd);
                if (DS.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                    {
                        DataTable DT = new DataTable();
                        DT = ds.Tables[0];
                        dfoPunish = new DFODecision();
                        dfoPunish.Category = DS.Tables[0].Rows[i]["Category"].ToString();
                        dfoPunish.Details = DS.Tables[0].Rows[i]["Details"].ToString();
                        dfoPunish.Punishment = DS.Tables[0].Rows[i]["Punishment"].ToString();
                        dfoPunish.Cognition = DS.Tables[0].Rows[i]["Cognition"].ToString();
                        dfoPunish.Bailable = DS.Tables[0].Rows[i]["Bailable"].ToString();
                        Punishment.Add(dfoPunish);
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(new { list1 = lstOffender, list2 = lstdfo, list3 = lstWitness, list4 = lstdfo3, list5 = Punishment }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Submit final data 
        /// </summary>
        /// <param name="dfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(DFODecision dfo, HttpPostedFileBase UploadOffenderStatement)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string UploadDoc = string.Empty;
            string FilePath = "~/ForestProtectionDocument/";
            var path = "";
            try
            {
                if (UploadOffenderStatement != null && UploadOffenderStatement.ContentLength > 0)
                {
                    UploadDoc = Path.GetFileName(UploadOffenderStatement.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + UploadDoc;
                    path = Path.Combine(FilePath, FileFullName);
                    dfo.UploadOffenderStatement = path;
                    UploadOffenderStatement.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    dfo.UploadOffenderStatement = "";
                }
                Int16 status = dfo.InsertDfoDecision();
                if (status == 1)
                {
                    TempData["Status"] = "Decision updated sucessfully";
                }
                else
                {
                    TempData["Status"] = "Details Not Found";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return RedirectToAction("DFODecision", "DFODecision");
        }

        public ActionResult Courthearingdetails()
        {
            List<CourtHearingDetail> List = new List<CourtHearingDetail>();
            if (Session["UserID"] != null)
            {
                DataSet dtf = new CourtHearingDetail().GetHearingDetails(Convert.ToInt64(Session["UserID"].ToString()));

                if (dtf.Tables.Count > 0)
                {
                    foreach (DataRow dr in dtf.Tables[0].Rows)
                        List.Add(
                            new CourtHearingDetail()

                            {
                                Index = dr["Index"].ToString(),
                                OffenseCode = dr["OffenseCode"].ToString(),
                                OffenderName = dr["OffenderName"].ToString(),
                                FatherName = dr["FatherName"].ToString(),
                                DIST_NAME = dr["DIST_NAME"].ToString(),
                                AppreanceDate = dr["AppreanceDate"].ToString(),
                            });
                }
                ViewData["CHearingdataList"] = List;

            }

            return View();
        }

    }
}
