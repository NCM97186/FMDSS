//---------------------------------------------------------------
//  Project      : FMDSS 
//  Project Code : IEISL_SSD_2015_16_ENV_004
//  Copyright (C): IEISL 
//  File         : BudgetAllocationDivisionController
//  Description  : File contains details of Budget allocation at division
//  Date Created : 01-09-2017
//  Author       : Ashok Yadav
//---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System.Data.SqlClient;
using FMDSS.Repository;
using System.IO;
using AutoMapper;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.Encroachment.DomainModel;
using System.Data.Entity;
using FMDSS.Models.ForesterDevelopment;
using System.Data;
using FMDSS.Models;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class BudgetAllocationCircleController : Controller
    {

        public FmdssContext fmdsscontext;
        public BudgetAllocationCircleController()
        {
            fmdsscontext = new FmdssContext();
        }
        public ActionResult BudgetAllocationTocircle()
        {
            try
            {
                TempData.Remove("CircleList");
                TempData.Remove("BudgetAllocation");
                List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<SelectListItem> lstScheme = new List<SelectListItem>();
                List<SelectListItem> lstCircle = new List<SelectListItem>();
                var financialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).ToList();
                foreach (var item in financialYear)
                {
                    lstFinancialYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }

                var budgetHead = fmdsscontext.tbl_mst_BudgetHead.Select(i => new { i.BudgetHead, i.ID });
                foreach (var item in budgetHead)
                {

                    lstBudgetHead.Add(new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) });
                }
                var Scheme = fmdsscontext.tbl_FDM_SchemeForWidelife.Select(i => new { i.Scheme_Name, i.ID });
                foreach (var item in Scheme)
                {
                    lstScheme.Add(new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) });
                }
                var activity = fmdsscontext.tbl_mst_ActivityForWidelife.Select(i => new { i.Activity_Name, i.ID });
                var Circle = fmdsscontext.tbl_mst_Forest_WildLifeCircles.Where(d => d.ISWILDLIFECIRCLE == true).Select(i => new { i.CIRCLE_NAME, i.CIRCLE_CODE });
                foreach (var item in Circle)
                {
                    lstCircle.Add(new SelectListItem { Text = item.CIRCLE_NAME, Value = Convert.ToString(item.CIRCLE_CODE) });
                }

                ViewBag.Circle = lstCircle;
                ViewBag.Scheme = lstScheme;
                ViewBag.FinancialYear = lstFinancialYear;
                ViewBag.Division = new View_BudgetAllocation_Division().GetDivisionList();
                ViewBag.BudgetHead = lstBudgetHead;

                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                ViewData["BudgetAllocationCircleList"] = repo.GetBudgetAllocationCircleList(Convert.ToInt32(Session["UserId"]));
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }



        public JsonResult GetSubActivity(long ActivityID)
        {
            try
            {
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();

                var result = fmdsscontext.tbl_mst_SUBActivityForWidelife.Where(i => i.ActivityID == ActivityID).Select(i => new { i.SUBActivity_Name, i.ID }).OrderBy(s => s.SUBActivity_Name);

                if (result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        lstSubActivity.Add(new SelectListItem { Text = item.SUBActivity_Name, Value = Convert.ToString(item.ID) });
                    }
                }
                else
                {
                    lstSubActivity.Add(new SelectListItem { Text = "None", Value = "0" });
                }
                return Json(lstSubActivity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetBudgetSubHead(string budgetHead)
        {
            try
            {
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                var query = (from a in fmdsscontext.tbl_mst_SubBudgetHead
                             select new
                             {
                                 a.BudgetHeadID,
                                 a.SubBudgetHead,
                                 a.ID
                             }).ToList();
                var subBudgetHead = query.Where(i => i.BudgetHeadID == Convert.ToInt64(budgetHead)).OrderBy(s => s.SubBudgetHead);

                if (subBudgetHead.Count() > 0)
                {
                    foreach (var item in subBudgetHead)
                    {
                        lstBudgetHead.Add(new SelectListItem { Text = item.SubBudgetHead, Value = Convert.ToString(item.ID) });
                    }
                }
                else
                {
                    lstBudgetHead.Add(new SelectListItem { Text = "none", Value = "0" });
                }
                return Json(lstBudgetHead, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public ActionResult GetAllocatedAmtDetails(View_BudgetAllocation_Circle objCircle)
        {
            try
            {
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                var param1 = new SqlParameter("@BudgetHeadID", objCircle.BudgetHeadID);
                var param2 = new SqlParameter("@SubBudgetHeadID", objCircle.SubBudgetHeadID);
                var param3 = new SqlParameter("@FY_ID", objCircle.FY_ID);
                var param4 = new SqlParameter("@SchemeID", objCircle.SchemeID);
                var param5 = new SqlParameter("@ActivityID", objCircle.ActivityID);
                var param6 = new SqlParameter("@SubActivityID", objCircle.SubActivityID);
                var result = fmdsscontext.Database.SqlQuery<View_BudgetAllocation_Circle>("BA_getAVailableAmountforCircle @BudgetHeadID,@SubBudgetHeadID,@FY_ID,@SchemeID,@ActivityID,@SubActivityID", param1, param2, param3, param4, param5, param6).ToList();
                var fresult = result.FirstOrDefault();
                decimal AvaliableAmount = fresult.TotalAmount;
                decimal AllocatedAmount = fresult.AllocatedAmount;
                decimal RemaningAmount = fresult.RemaningAmount;
                long BudgetAllocationHeadId = fresult.BudgetHeadAllocationID;
                var json = Json(new { AvaliableAmount, AllocatedAmount, RemaningAmount, BudgetAllocationHeadId });
                return json;

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public ActionResult BudgetAllocationCircleList()
        {
            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = new List<View_BudgetAllocation_Circle>();
            try
            {
                var param1 = new SqlParameter("@Option", "CIRCLELIST");
                var result = fmdsscontext.Database.SqlQuery<View_BudgetAllocation_Circle>("BA_getBudgetAllocationList @Option", param1).ToList();
                foreach (var item in result)
                {
                    lstBudgetAllocationCircle.Add(new View_BudgetAllocation_Circle
                    {
                        rowid = Convert.ToString(item.ID),
                        CIRCLE_CODE = item.CIRCLE_CODE,
                        CIRCLE_NAME = item.CIRCLE_NAME + "/" + item.Division,
                        SchemeName = item.SchemeName,
                        SchemeID = item.SchemeID,
                        isActive = item.isActive,
                        ActivityName = item.ActivityName + "/" + item.SubActivityName,
                        ActivityID = item.ActivityID,
                        SubActivityID = item.SubActivityID,
                        BudgetHead = item.BudgetHead + "/" + item.SubBudgetHead,
                        BudgetHeadID = item.BudgetHeadID,
                        SubBudgetHeadID = item.SubBudgetHeadID,
                        AllocatedAmount = item.AllocatedAmount,
                        TotalAmount = item.TotalAmount,
                        BudgetHeadAllocationID = item.BudgetHeadAllocationID,
                        ISCircleDivision = item.ISCircleDivision,
                        SanctuaryCode = item.SanctuaryCode,
                        SanctuaryName = item.SanctuaryName
                    });
                }
                TempData["CircleList"] = lstBudgetAllocationCircle;
                return Json(lstBudgetAllocationCircle, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult BudgetAllocationFilterCircleList(View_BudgetAllocation_Circle obj)
        {

            BudgetAllocationCircleList();
            TempData.Remove("BudgetAllocation");
            List<View_BudgetAllocation_Circle> lstBudgetAllocationFilterCircle = new List<View_BudgetAllocation_Circle>();
            if (TempData["CircleList"] != null)
            {
                List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)TempData.Peek("CircleList");
                if (obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID == 0 && obj.ActivityID == 0 && obj.SubActivityID == 0)
                {
                    lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.BudgetHeadID == obj.BudgetHeadID && s.SchemeID == obj.SchemeID).ToList();
                }
                else if (obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID != 0 && obj.ActivityID == 0 && obj.SubActivityID == 0)
                {
                    lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.BudgetHeadID == obj.BudgetHeadID && s.SchemeID == obj.SchemeID && s.SubBudgetHeadID == obj.SubBudgetHeadID).ToList();
                }
                else if (obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.ActivityID != 0 && obj.SubBudgetHeadID != 0 && obj.SubActivityID == 0)
                {
                    lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.BudgetHeadID == obj.BudgetHeadID && s.SchemeID == obj.SchemeID && s.SubBudgetHeadID == obj.SubBudgetHeadID && s.ActivityID == obj.ActivityID).ToList();
                }
                else if (obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID == 0 && obj.ActivityID != 0 && obj.SubActivityID == 0)
                {
                    lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.BudgetHeadID == obj.BudgetHeadID && s.SchemeID == obj.SchemeID && s.ActivityID == obj.ActivityID).ToList();
                }
                else if (obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.ActivityID != 0 && obj.SubActivityID != 0)
                {
                    lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.BudgetHeadID == obj.BudgetHeadID && s.SchemeID == obj.SchemeID && s.SubBudgetHeadID == obj.SubBudgetHeadID && s.ActivityID == obj.ActivityID && s.SubActivityID == obj.SubActivityID).ToList();
                }
                foreach (View_BudgetAllocation_Circle objItems in lstBudgetAllocationCircle)
                {

                    lstBudgetAllocationFilterCircle.Add(new View_BudgetAllocation_Circle
                    {
                        BudgetHeadAllocationID = objItems.BudgetHeadAllocationID,
                        rowid = objItems.rowid,
                        CIRCLE_CODE = objItems.CIRCLE_CODE,
                        CIRCLE_NAME = objItems.CIRCLE_NAME + "/" + objItems.Division,
                        SchemeName = objItems.SchemeName,
                        SchemeID = objItems.SchemeID,
                        ActivityName = objItems.ActivityName + "/" + objItems.SubBudgetHead,
                        ActivityID = objItems.ActivityID,
                        SubActivityID = objItems.SubActivityID,
                        BudgetHead = objItems.BudgetHead + "/" + objItems.SubBudgetHead,
                        BudgetHeadID = objItems.BudgetHeadID,
                        SubBudgetHeadID = objItems.SubBudgetHeadID,
                        AllocatedAmount = objItems.AllocatedAmount,
                        TotalAmount = objItems.TotalAmount,
                        isActive = objItems.isActive,
                        ISCircleDivision = objItems.ISCircleDivision
                    });
                }
            }
            if (lstBudgetAllocationFilterCircle.Count > 0)
            {

                return Json(lstBudgetAllocationFilterCircle, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AjaxSubmit(List<View_BudgetAllocation_Circle> lstBodgetCircle)
        {
            long status = 0;
            try
            {
                if (TempData["BudgetAllocation"] != null)
                {

                    List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)TempData.Peek("BudgetAllocation");

                    var query = (from a in fmdsscontext.tbl_BudgetAllocation_Circle
                                 select new
                                 {
                                     a.ID,
                                     a.FY_ID,
                                     a.SchemeID,
                                     a.BudgetHeadID,
                                     a.SubBudgetHeadID,
                                     a.ActivityID,
                                     a.SubActivityID,
                                     a.CIRCLE_CODE,
                                     a.Division,
                                     a.ISCircleDivision,
                                     a.isActive
                                 }).ToList();

                    var fQuery = query.Where(i => lstBodgetCircle.Any(e => e.ID == i.ID));

                    var lstFbudget = fQuery.Where(i => lstBudgetAllocationCircle.Any(e => e.FY_ID == i.FY_ID && e.SchemeID == i.SchemeID && i.BudgetHeadID == e.BudgetHeadID && i.SubBudgetHeadID == e.SubBudgetHeadID && i.CIRCLE_CODE == e.CIRCLE_CODE && i.Division == e.Division && i.ISCircleDivision == e.ISCircleDivision));

                    foreach (var items in lstFbudget)
                    {
                        tbl_BudgetAllocation_Circle tblBudgCirUpdate = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == items.ID);

                        if (tblBudgCirUpdate != null)
                        {
                            tblBudgCirUpdate.isActive = true;
                            this.fmdsscontext.tbl_BudgetAllocation_Circle.Add(tblBudgCirUpdate);
                            fmdsscontext.Entry(tblBudgCirUpdate).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    status = fmdsscontext.SaveChanges();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdddCircleDetails(View_BudgetAllocation_Circle objModel)
        {
            try
            {
                bool duplicate = CheckDuplicateRecords(objModel);
                TempData.Keep("BudgetAllocation");
                if (duplicate == false)
                {
                    if (objModel != null && (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle"))
                    {
                        if (objModel.ISCircleDivision == "HQ")
                        {
                            objModel.CIRCLE_CODE = "HQ";
                            objModel.CIRCLE_NAME = "HQ";
                        }

                        tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>();
                            tbl_BudgetAllocation_Circle ObjbudgetCircle = Mapper.Map<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    if (objModel != null && objModel.ISCircleDivision == "Division")
                    {

                        tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>();
                            tbl_BudgetAllocation_Circle ObjbudgetCircle = Mapper.Map<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                }
                else
                {
                    objModel.rowid = "D";
                }

            }
            catch (Exception)
            {
                throw;
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        public bool CheckDuplicateRecords(View_BudgetAllocation_Circle objModel)
        {
            List<View_BudgetAllocation_Circle> lstChkDup = new List<View_BudgetAllocation_Circle>();
            bool duplicate = false;
            if (TempData["BudgetAllocation"] == null)
            {
                if (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
                {

                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true);
                    if (tbl == null)
                    {

                        lstChkDup.Add(new View_BudgetAllocation_Circle
                        {
                            FY_ID = objModel.FY_ID,
                            SchemeID = objModel.SchemeID,
                            BudgetHeadID = objModel.BudgetHeadID,
                            SubBudgetHeadID = objModel.SubBudgetHeadID,
                            ActivityID = objModel.ActivityID,
                            SubActivityID = objModel.SubActivityID,
                            CIRCLE_CODE = objModel.CIRCLE_CODE,
                            Division = objModel.Division,
                            ISCircleDivision = objModel.ISCircleDivision
                        });
                        TempData["BudgetAllocation"] = lstChkDup;
                    }
                    else
                    {
                        duplicate = true;
                    }
                }


                if (objModel != null && objModel.ISCircleDivision == "Division")
                {

                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true);
                    if (tbl == null)
                    {

                        lstChkDup.Add(new View_BudgetAllocation_Circle
                        {
                            FY_ID = objModel.FY_ID,
                            SchemeID = objModel.SchemeID,
                            BudgetHeadID = objModel.BudgetHeadID,
                            SubBudgetHeadID = objModel.SubBudgetHeadID,
                            ActivityID = objModel.ActivityID,
                            SubActivityID = objModel.SubActivityID,
                            CIRCLE_CODE = objModel.CIRCLE_CODE,
                            Division = objModel.Division,
                            ISCircleDivision = objModel.ISCircleDivision
                        });
                        TempData["BudgetAllocation"] = lstChkDup;

                    }
                    else
                    {
                        duplicate = true;
                    }

                }

                if (objModel.ISCircleDivision == "Sanctuary")
                {

                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true);
                    if (tbl == null)
                    {

                        lstChkDup.Add(new View_BudgetAllocation_Circle
                        {
                            FY_ID = objModel.FY_ID,
                            SchemeID = objModel.SchemeID,
                            BudgetHeadID = objModel.BudgetHeadID,
                            SubBudgetHeadID = objModel.SubBudgetHeadID,
                            ActivityID = objModel.ActivityID,
                            SubActivityID = objModel.SubActivityID,
                            CIRCLE_CODE = objModel.CIRCLE_CODE,
                            Division = objModel.Division,
                            SanctuaryCode = objModel.SanctuaryCode,
                            ISCircleDivision = objModel.ISCircleDivision
                        });
                        TempData["BudgetAllocation"] = lstChkDup;

                    }
                }
                else
                {
                    duplicate = true;
                }
            }
            else
            {
                List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)TempData.Peek("BudgetAllocation");
                foreach (var item in lstBudgetAllocationCircle)
                {
                    if ((item.ISCircleDivision == "Circle" && objModel.ISCircleDivision == "Circle") || item.ISCircleDivision == "HQ" && objModel.ISCircleDivision == "HQ")
                    {

                        if (item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && (item.ISCircleDivision == "Circle" || item.ISCircleDivision == "HQ"))
                        {

                            duplicate = true;
                        }
                    }
                    if (item.ISCircleDivision == "Division" && objModel.ISCircleDivision == "Division")
                    {
                        if (item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && item.Division == objModel.Division && item.ISCircleDivision == "Division")
                        {
                            duplicate = true;

                        }
                    }

                    if (item.ISCircleDivision == "Sanctuary" && objModel.ISCircleDivision == "Sanctuary")
                    {
                        if (item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && item.Division == objModel.Division && item.ISCircleDivision == "Sanctuary" && item.SanctuaryCode == objModel.SanctuaryCode)
                        {
                            duplicate = true;

                        }
                    }
                }

                if (duplicate == false)
                {
                    lstBudgetAllocationCircle.Add(new View_BudgetAllocation_Circle
                    {
                        FY_ID = objModel.FY_ID,
                        SchemeID = objModel.SchemeID,
                        BudgetHeadID = objModel.BudgetHeadID,
                        SubBudgetHeadID = objModel.SubBudgetHeadID,
                        ActivityID = objModel.ActivityID,
                        SubActivityID = objModel.SubActivityID,
                        CIRCLE_CODE = objModel.CIRCLE_CODE,
                        Division = objModel.Division,
                        ISCircleDivision = objModel.ISCircleDivision
                    });
                    TempData["BudgetAllocation"] = null;
                    TempData["BudgetAllocation"] = lstBudgetAllocationCircle;
                }
            }
            return duplicate;
        }

        public bool CheckDuplicateRecordsEditCase(View_BudgetAllocation_Circle objModel)
        {
            List<View_BudgetAllocation_Circle> lstChkDup = new List<View_BudgetAllocation_Circle>();
            bool duplicate = false;
            #region If Site Name Null Or Empty then Set n/a
            if (string.IsNullOrEmpty(objModel.SiteName))
            {
                objModel.SiteName = "n/a";
            }
            #endregion
            if (TempData["BudgetAllocation"] == null)
            {
                if (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
                {
                    int tblCount = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => i.FY_ID == objModel.FY_ID && i.ID != objModel.BudgetHeadAllocationID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                    //tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => (i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true) && i.ID != objModel.BudgetHeadAllocationID).FirstOrDefault();
                    //if (tbl != null)
                    if (tblCount > 0)
                    {
                        duplicate = true;
                    }
                }
                if (objModel != null && objModel.ISCircleDivision == "Division")
                {
                    int tblCount = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => i.FY_ID == objModel.FY_ID && i.ID != objModel.BudgetHeadAllocationID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.ISCircleDivision == "Division" && i.Division == objModel.Division && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();

                    // tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => (i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true) && i.ID != objModel.BudgetHeadAllocationID);
                    //if (tbl != null)
                    if (tblCount > 0)
                    {
                        duplicate = true;
                    }

                }
                if (objModel.ISCircleDivision == "Sanctuary")
                {
                    int tblCount = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => i.FY_ID == objModel.FY_ID && i.ID != objModel.BudgetHeadAllocationID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                    // tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => (i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true) && i.ID != objModel.BudgetHeadAllocationID);
                    if (tblCount > 0)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)TempData.Peek("BudgetAllocation");
                //foreach (var item in lstBudgetAllocationCircle)
                //{
                if ((objModel.ISCircleDivision == "Circle") || objModel.ISCircleDivision == "HQ")
                {

                    int count = lstBudgetAllocationCircle.Where(item => item.FY_ID == objModel.FY_ID && item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && (item.ISCircleDivision == "Circle" || item.ISCircleDivision == "HQ") && item.ID != objModel.BudgetHeadAllocationID && true == ((string.IsNullOrEmpty(objModel.SiteName) ? item.SiteName == objModel.SiteName : item.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                    if (count > 0)
                    {
                        duplicate = true;
                    }

                    //if ((item.IsCoreOrBuffer == objModel.IsCoreOrBuffer && item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && (item.ISCircleDivision == "Circle" || item.ISCircleDivision == "HQ")) && item.ID != objModel.BudgetHeadAllocationID)
                    //{
                    //    duplicate = true;
                    //}
                }
                if (objModel.ISCircleDivision == "Division")
                {

                    int count = lstBudgetAllocationCircle.Where(item => item.FY_ID == objModel.FY_ID && item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && item.Division == objModel.Division && item.ISCircleDivision == "Division" && item.ID != objModel.BudgetHeadAllocationID && true == ((string.IsNullOrEmpty(objModel.SiteName) ? item.SiteName == objModel.SiteName : item.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                    if (count > 0)
                    {
                        duplicate = true;
                    }
                }

                if (objModel.ISCircleDivision == "Sanctuary")
                {

                    int count = lstBudgetAllocationCircle.Where(item => item.FY_ID == objModel.FY_ID && item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && item.Division == objModel.Division && item.ISCircleDivision == "Sanctuary" && item.SanctuaryCode == objModel.SanctuaryCode && item.ID != objModel.BudgetHeadAllocationID && true == ((string.IsNullOrEmpty(objModel.SiteName) ? item.SiteName == objModel.SiteName : item.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                    if (count > 0)
                    {
                        duplicate = true;
                    }
                }
                //}

            }
            return duplicate;
        }

        public JsonResult DeleteCircle(long Id, string ISCircleDivision)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_BudgetAllocation_Circle tblAllocationCircle = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == Id);

                    if (tblAllocationCircle != null)
                    {
                        if (ISCircleDivision == "Circle" || ISCircleDivision == "HQ")
                        {

                            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)TempData.Peek("BudgetAllocation");

                            if (lstBudgetAllocationCircle != null)
                            {

                                View_BudgetAllocation_Circle result = lstBudgetAllocationCircle.FirstOrDefault(i => i.FY_ID == tblAllocationCircle.FY_ID && i.SchemeID == tblAllocationCircle.SchemeID && i.BudgetHeadID == tblAllocationCircle.BudgetHeadID && i.SubBudgetHeadID == tblAllocationCircle.SubBudgetHeadID && i.ActivityID == tblAllocationCircle.ActivityID && i.SubActivityID == tblAllocationCircle.SubActivityID && i.CIRCLE_CODE == tblAllocationCircle.CIRCLE_CODE && (tblAllocationCircle.ISCircleDivision == "Circle" || tblAllocationCircle.ISCircleDivision == "HQ"));
                                if (result != null)
                                {
                                    lstBudgetAllocationCircle.Remove(result);
                                }
                                TempData["BudgetAllocation"] = null;
                                TempData["BudgetAllocation"] = lstBudgetAllocationCircle;
                            }
                        }
                        if (ISCircleDivision == "Division")
                        {

                            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)TempData.Peek("BudgetAllocation");

                            if (lstBudgetAllocationCircle != null)
                            {
                                View_BudgetAllocation_Circle result = lstBudgetAllocationCircle.FirstOrDefault(i => i.FY_ID == tblAllocationCircle.FY_ID && i.SchemeID == tblAllocationCircle.SchemeID && i.BudgetHeadID == tblAllocationCircle.BudgetHeadID && i.SubBudgetHeadID == tblAllocationCircle.SubBudgetHeadID && i.ActivityID == tblAllocationCircle.ActivityID && i.SubActivityID == tblAllocationCircle.SubActivityID && i.CIRCLE_CODE == tblAllocationCircle.CIRCLE_CODE && i.Division == tblAllocationCircle.Division && tblAllocationCircle.ISCircleDivision == "Division");

                                if (result != null)
                                {
                                    lstBudgetAllocationCircle.Remove(result);
                                }
                                TempData["BudgetAllocation"] = null;
                                TempData["BudgetAllocation"] = lstBudgetAllocationCircle;
                            }
                        }
                        tblAllocationCircle.isActive = false;
                        fmdsscontext.Entry(tblAllocationCircle).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();
                    }
                    else
                    {
                        status = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        #region Budget Module Developed By Rajveer
        [HttpPost]
        public JsonResult GetMasterActivity(string Action, string ID)
        {
            try
            {
                SubActivityRepo repo = new SubActivityRepo();

                DataTable ActivityDataset = new DataTable();
                ActivityDataset = repo.BindDDlBudgetMasterModel(Action, Convert.ToInt32(ID));
                IEnumerable<SelectListItem> lstActivity = (IEnumerable<SelectListItem>)new SelectList(ActivityDataset.AsDataView(), "value", "text");
                return Json(lstActivity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        [HttpGet]
        public JsonResult GetMasterSchemeWise(string SchemeID, string BudgetHeadId, string ActivityID, string ActionName)
        {
            try
            {
                SubActivityRepo repo = new SubActivityRepo();
                DataSet ActivityDataset = new DataSet();
                ActivityDataset = repo.BindDDlBudgetMasterWithSchemeWise(Convert.ToInt32(SchemeID), Convert.ToInt32(BudgetHeadId), Convert.ToInt32(ActivityID), ActionName);
                IEnumerable<SelectListItem> lstobj = (IEnumerable<SelectListItem>)new SelectList(ActivityDataset.Tables[0].AsDataView(), "value", "text");
                return Json(lstobj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        public ActionResult BudgetAllocation()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            BudgetAllocationModel model = new BudgetAllocationModel();
            Session["AddBudgetAllocationList"] = null;
            Session["BudgetMasterList"] = null;
            try
            {
                #region Budget Head Master List
                var param1 = new SqlParameter("@Option", "BUDGETHEAD");
                var result = fmdsscontext.Database.SqlQuery<View_BudgetHead_Allocation>("BA_getBudgetAllocationList @Option", param1).ToList();

                #region Serialized Object to Model

                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                model.BudgetHeadMasterList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetHead_Allocation>>(str);
                Session["BudgetMasterList"] = model.BudgetHeadMasterList;
                #endregion

                #endregion

                #region Budget Allocated List
                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                model.BudgetAllocationList = repo.GetBudgetAllocationCircleList(Convert.ToInt32(Session["UserID"]));
                #endregion

                #region Fill Circle and Division
                List<SelectListItem> lstCircle = new List<SelectListItem>();
                var Circle = fmdsscontext.tbl_mst_Forest_WildLifeCircles.Where(d => d.isBOTH == 1).Select(i => new { i.CIRCLE_NAME, i.CIRCLE_CODE }).OrderBy(s => s.CIRCLE_NAME);
                foreach (var item in Circle)
                {
                    lstCircle.Add(new SelectListItem { Text = item.CIRCLE_NAME, Value = Convert.ToString(item.CIRCLE_CODE) });
                }
                ViewBag.Circle = lstCircle;
                ViewBag.Division = new View_BudgetAllocation_Division().GetDivisionList();
                #endregion

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult BudgetAllocation(BudgetAllocationModel model)
        {
            #region Active Status
            try
            {
                long status = 0;
                try
                {
                    if (Session["AddBudgetAllocationList"] != null)
                    {

                        List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];

                        //var lstFbudget = fQuery.Where(i => lstBudgetAllocationCircle.Any(e => e.FY_ID == i.FY_ID && e.SchemeID == i.SchemeID && i.BudgetHeadID == e.BudgetHeadID && i.SubBudgetHeadID == e.SubBudgetHeadID && i.CIRCLE_CODE == e.CIRCLE_CODE && i.Division == e.Division && i.ISCircleDivision == e.ISCircleDivision));

                        foreach (var items in lstBudgetAllocationCircle)
                        {
                            long id = Convert.ToInt64(items.rowid);
                            tbl_BudgetAllocation_Circle tblBudgCirUpdate = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == id);

                            if (tblBudgCirUpdate != null)
                            {
                                tblBudgCirUpdate.isActive = true;
                                this.fmdsscontext.tbl_BudgetAllocation_Circle.Add(tblBudgCirUpdate);
                                fmdsscontext.Entry(tblBudgCirUpdate).State = System.Data.Entity.EntityState.Modified;


                            }
                        }
                        status = fmdsscontext.SaveChanges();
                    }

                }
                catch (Exception)
                {
                    throw;
                }

            }

            catch (Exception ex)

            {

            }
            #endregion
            return RedirectToAction("BudgetAllocation");
        }

        public JsonResult AddBudgetAllocation(View_BudgetAllocation_Circle objModel)
        {
            try
            {
                #region Maintain the BudgetAllocation List
                List<View_BudgetAllocation_Circle> BudgetAllocationLists = new List<View_BudgetAllocation_Circle>();
                if (Session["AddBudgetAllocationList"] == null)
                {
                    Session["AddBudgetAllocationList"] = new List<View_BudgetAllocation_Circle>();
                }
                else
                {
                    BudgetAllocationLists = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];
                }
                #endregion

                bool Duplicate = CheckAddBudgetAllocationDuplicateRecords(objModel, BudgetAllocationLists);

                if (Duplicate == false)
                {


                    #region Circle and HQ
                    if (objModel != null && (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle"))
                    {
                        if (objModel.ISCircleDivision == "HQ")
                        {
                            objModel.CIRCLE_CODE = "HQ";
                            objModel.CIRCLE_NAME = "HQ";
                        }

                        tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>();
                            tbl_BudgetAllocation_Circle ObjbudgetCircle = Mapper.Map<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetAllocationList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Division
                    if (objModel != null && objModel.ISCircleDivision == "Division")
                    {

                        tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>();
                            tbl_BudgetAllocation_Circle ObjbudgetCircle = Mapper.Map<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetAllocationList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion
                }
                else
                {
                    objModel.rowid = "D";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteBudgetAllocation(long Id, string ISCircleDivision)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_BudgetAllocation_Circle tblAllocationCircle = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == Id);

                    if (tblAllocationCircle != null)
                    {
                        #region Circle and HQ
                        if (ISCircleDivision == "Circle" || ISCircleDivision == "HQ")
                        {

                            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];

                            if (lstBudgetAllocationCircle != null)
                            {

                                View_BudgetAllocation_Circle result = lstBudgetAllocationCircle.FirstOrDefault(i => i.FY_ID == tblAllocationCircle.FY_ID && i.SchemeID == tblAllocationCircle.SchemeID && i.BudgetHeadID == tblAllocationCircle.BudgetHeadID && i.SubBudgetHeadID == tblAllocationCircle.SubBudgetHeadID && i.ActivityID == tblAllocationCircle.ActivityID && i.SubActivityID == tblAllocationCircle.SubActivityID && i.CIRCLE_CODE == tblAllocationCircle.CIRCLE_CODE && (tblAllocationCircle.ISCircleDivision == "Circle" || tblAllocationCircle.ISCircleDivision == "HQ"));
                                if (result != null)
                                {
                                    lstBudgetAllocationCircle.Remove(result);
                                }
                                Session["AddBudgetAllocationList"] = null;
                                Session["AddBudgetAllocationList"] = lstBudgetAllocationCircle;
                            }
                        }
                        #endregion
                        #region Division
                        if (ISCircleDivision == "Division")
                        {

                            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];

                            if (lstBudgetAllocationCircle != null)
                            {
                                View_BudgetAllocation_Circle result = lstBudgetAllocationCircle.FirstOrDefault(i => i.FY_ID == tblAllocationCircle.FY_ID && i.SchemeID == tblAllocationCircle.SchemeID && i.BudgetHeadID == tblAllocationCircle.BudgetHeadID && i.SubBudgetHeadID == tblAllocationCircle.SubBudgetHeadID && i.ActivityID == tblAllocationCircle.ActivityID && i.SubActivityID == tblAllocationCircle.SubActivityID && i.CIRCLE_CODE == tblAllocationCircle.CIRCLE_CODE && i.Division == tblAllocationCircle.Division && tblAllocationCircle.ISCircleDivision == "Division");

                                if (result != null)
                                {
                                    lstBudgetAllocationCircle.Remove(result);
                                }
                                Session["AddBudgetAllocationList"] = null;
                                Session["AddBudgetAllocationList"] = lstBudgetAllocationCircle;
                            }
                        }
                        #endregion

                        #region Sanctuary
                        if (ISCircleDivision == "Sanctuary")
                        {

                            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];

                            if (lstBudgetAllocationCircle != null)
                            {
                                View_BudgetAllocation_Circle result = lstBudgetAllocationCircle.FirstOrDefault(i => i.FY_ID == tblAllocationCircle.FY_ID && i.SchemeID == tblAllocationCircle.SchemeID && i.BudgetHeadID == tblAllocationCircle.BudgetHeadID && i.SubBudgetHeadID == tblAllocationCircle.SubBudgetHeadID && i.ActivityID == tblAllocationCircle.ActivityID && i.SubActivityID == tblAllocationCircle.SubActivityID && i.CIRCLE_CODE == tblAllocationCircle.CIRCLE_CODE && i.Division == tblAllocationCircle.Division && tblAllocationCircle.ISCircleDivision == "Sanctuary" && tblAllocationCircle.SanctuaryCode == tblAllocationCircle.SanctuaryCode);

                                if (result != null)
                                {
                                    lstBudgetAllocationCircle.Remove(result);
                                }
                                Session["AddBudgetAllocationList"] = null;
                                Session["AddBudgetAllocationList"] = lstBudgetAllocationCircle;
                            }
                        }
                        #endregion
                        tblAllocationCircle.isActive = false;
                        fmdsscontext.Entry(tblAllocationCircle).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();
                    }
                    else
                    {
                        status = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public bool CheckAddBudgetAllocationDuplicateRecords(View_BudgetAllocation_Circle objModel, List<View_BudgetAllocation_Circle> List)
        {
            List<View_BudgetAllocation_Circle> lstChkDup = new List<View_BudgetAllocation_Circle>();
            bool duplicate = false;

            #region Check In Table
            if (objModel != null && objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
            {
                #region Check In Current Session
                int count = List.Where(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ")).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true);
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }

            }
            else if (objModel != null && objModel.ISCircleDivision == "Division")
            {
                #region Check In Current Session
                int count = List.Where(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division").Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true);
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }
            }
            #endregion




            return duplicate;
        }

        public JsonResult DeleteBudgetAllocatedEntry(long Id)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_BudgetAllocation_Circle tblAllocationCircle = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == Id);

                    if (tblAllocationCircle != null)
                    {
                        tblAllocationCircle.isActive = false;
                        fmdsscontext.Entry(tblAllocationCircle).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();

                        #region Update Allocation Amount on Circle Level
                        long userID = Convert.ToInt64(Session["UserID"]);
                        var ChkCirclelevelLog = fmdsscontext.tbl_BudgetAllocation_CircleLevels.Where(i => i.tbl_BudgetAllocation_CircleID == Id && i.Status == 1).OrderByDescending(d => d.ID).FirstOrDefault();



                        var tbl_BudgetAllocationParent = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => i.ID == ChkCirclelevelLog.tbl_BudgetAllocation_ParentID && i.isActive == true).OrderByDescending(d => d.ID).FirstOrDefault();

                        if (ChkCirclelevelLog != null && tbl_BudgetAllocationParent != null)
                        {
                            tbl_BudgetAllocationParent.AllocatedAmount = tbl_BudgetAllocationParent.AllocatedAmount + ChkCirclelevelLog.AllocatedAmountDistribute;
                        }
                        ChkCirclelevelLog.Status = 0;
                        fmdsscontext.Entry(ChkCirclelevelLog).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();

                        fmdsscontext.Entry(tbl_BudgetAllocationParent).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();
                        #endregion

                        #region Email and SMS
                        SMS_EMail_Services SE = new SMS_EMail_Services();
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        DataTable DT = objSMSandEMAILtemplate.GetUserDetails(Convert.ToString(Session["UserID"]), "BudgetAllocationDeleteingPersonInfo");
                        DataTable budgetInfo = objSMSandEMAILtemplate.GetUserDetails(Convert.ToString(Id), "BudgetAllocationDeleteingInfo");

                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(budgetInfo);
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);
                        View_BudgetAllocation_Circle model = data.FirstOrDefault();
                        if (budgetInfo != null && budgetInfo.Rows.Count > 0)
                            objSMSandEMAILtemplate.SendMailComman("ALL", "BudgetAllocationDeleteModuleByPCCF", string.Empty, Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "Deleted", null, model.FinancialYear, model.SchemeName, model.BudgetHead, model.SubBudgetHead, model.ActivityName, model.SubActivityName, Convert.ToString(model.AllocatedAmount), model.CIRCLE_NAME, model.Division, model.SanctuaryName, model.ISCircleDivision);
                        #endregion
                    }
                    else
                    {
                        status = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BudgetAllocationDetails(string ID)
        {
            List<View_BudgetHead_Allocation> BudgetAllocationHeadList = new List<View_BudgetHead_Allocation>();
            View_BudgetHead_Allocation BudgetAllocationHeadDetails = new View_BudgetHead_Allocation();

            Session["AddBudgetAllocationList"] = new List<View_BudgetAllocation_Circle>();
            try
            {
                BudgetAllocationHeadList = (List<View_BudgetHead_Allocation>)Session["BudgetMasterList"];
                if (BudgetAllocationHeadList.Count > 0)
                {
                    BudgetAllocationHeadDetails = BudgetAllocationHeadList.FirstOrDefault(s => s.ID == Convert.ToInt64(ID));
                }

            }
            catch (Exception ex)
            {

            }
            return Json(BudgetAllocationHeadDetails, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GIS Services Developed by Rajveer

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqFor">ISCircleDivision</param>
        /// <param name="reqType">IsForestOrWildLife</param>
        /// <param name="Code">CircleCode/Division/HQ COde</param>
        /// <returns></returns>
        public JsonResult GetBudgetAllocation(string reqFor = "", string reqType = "", string Code = "")
        {
            List<View_BudgetAllocation_CircleForGIS> lstBudgetAllocationCircle = new List<View_BudgetAllocation_CircleForGIS>();
            try
            {
                var param1 = new SqlParameter("@Option", "BudgetAllocation");
                var param2 = new SqlParameter("@ISCircleDivision", reqFor);
                var param3 = new SqlParameter("@IsForestOrWideLife", reqType);
                var param4 = new SqlParameter("@ISCircleOrDivisionCode", Code);
                var result = fmdsscontext.Database.SqlQuery<View_BudgetAllocation_Circle>("sp_BudgetModuleWithGIS @Option,@ISCircleDivision,@IsForestOrWideLife,@ISCircleOrDivisionCode", param1, param2, param3, param4).ToList();

                //string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                //lstBudgetAllocationCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);
                lstBudgetAllocationCircle = result.ToList().Select(d => new View_BudgetAllocation_CircleForGIS
                {
                    CIRCLE_NAME = d.CIRCLE_NAME,
                    HeadType = d.HeadType,
                    SubBudgetHead = d.SubBudgetHead,
                    Division = d.Division,
                    FinancialYear = d.FinancialYear,
                    SchemeName = d.SchemeName,
                    ActivityName = d.ActivityName,
                    SubActivityName = d.SubActivityName,
                    AllocatedAmount = d.AllocatedAmount,
                    TotalAmount = d.TotalAmount,
                    ISCircleDivision = d.ISCircleDivision,
                    IsWildlifeOrForest = d.IsWildlifeOrForest

                }).ToList();

            }
            catch (Exception ex)
            {
                lstBudgetAllocationCircle = new List<View_BudgetAllocation_CircleForGIS>();
            }
            return Json(lstBudgetAllocationCircle, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqFor">ISCircleDivision</param>
        /// <param name="reqType">IsForestOrWildLife</param>
        /// <param name="Code">CircleCode/Division/HQ COde</param>
        /// <returns></returns>
        public JsonResult GetBudgetAllocationExpenditure(string reqFor = "", string reqType = "", string Code = "")
        {
            List<View_BudgetAllocationExpemditureForGIS> lstBudgetAllocationCircle = new List<View_BudgetAllocationExpemditureForGIS>();
            try
            {
                var param1 = new SqlParameter("@Option", "BudgetAllocationExpenditure");
                var param2 = new SqlParameter("@ISCircleDivision", reqFor);
                var param3 = new SqlParameter("@IsForestOrWideLife", reqType);
                var param4 = new SqlParameter("@ISCircleOrDivisionCode", Code);
                var result = fmdsscontext.Database.SqlQuery<View_Budget_Expenditure>("sp_BudgetModuleWithGIS @Option,@ISCircleDivision,@IsForestOrWideLife,@ISCircleOrDivisionCode", param1, param2, param3, param4).ToList();

                //string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                //lstBudgetAllocationCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);

                lstBudgetAllocationCircle = result.ToList().Select(d => new View_BudgetAllocationExpemditureForGIS
                {
                    CIRCLE_NAME = d.CIRCLE_NAME,
                    Division_Name = d.Division_Name,
                    BudgetHead = d.BudgetHead,
                    SubBudgetHead = d.SubBudgetHead,
                    FinancialYear = d.FinancialYear,
                    SchemeName = d.SchemeName,
                    ActivityName = d.ActivityName,
                    SubActivityName = d.SubActivityName,
                    AllocatedAmount = d.AllocatedAmount,
                    ISCircleDivision = d.ISCircleDivision,
                    IsWildlifeOrForest = d.IsWildlifeOrForest,
                    RemaningAmount = d.RemaningAmount,
                    ExpenditureTilldate = d.ExpenditureTilldate,



                }).ToList();
            }
            catch (Exception ex)
            {
                lstBudgetAllocationCircle = new List<View_BudgetAllocationExpemditureForGIS>();
            }
            return Json(lstBudgetAllocationCircle, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Budget Module New Changes Marge Budget Master And Allocation

        public ActionResult AddBudgetInformation()
        {
            try
            {
                Session["AddBudgetAllocationList"] = null;
                List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<SelectListItem> lstScheme = new List<SelectListItem>();
                List<SelectListItem> lstCircle = new List<SelectListItem>();
                var financialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).ToList();
                foreach (var item in financialYear)
                {
                    lstFinancialYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }

                var budgetHead = fmdsscontext.tbl_mst_BudgetHead.Select(i => new { i.BudgetHead, i.ID }).OrderBy(s => s.BudgetHead);
                foreach (var item in budgetHead)
                {

                    lstBudgetHead.Add(new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) });
                }
                var Scheme = fmdsscontext.tbl_FDM_SchemeForWidelife.Select(i => new { i.Scheme_Name, i.ID }).OrderBy(s => s.Scheme_Name);
                foreach (var item in Scheme)
                {
                    lstScheme.Add(new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) });
                }
                var activity = fmdsscontext.tbl_mst_ActivityForWidelife.Select(i => new { i.Activity_Name, i.ID }).OrderBy(s => s.Activity_Name);
                var Circle = fmdsscontext.tbl_mst_Forest_WildLifeCircles.Where(s => s.isBOTH == 1).Select(i => new { i.CIRCLE_NAME, i.CIRCLE_CODE }).OrderBy(s => s.CIRCLE_NAME);
                foreach (var item in Circle)
                {
                    lstCircle.Add(new SelectListItem { Text = item.CIRCLE_NAME, Value = Convert.ToString(item.CIRCLE_CODE) });
                }

                ViewBag.Circle = lstCircle;
                ViewBag.Scheme = lstScheme;
                ViewBag.FinancialYear = lstFinancialYear;
                ViewBag.Division = new View_BudgetAllocation_Division().GetDivisionList();
                ViewBag.BudgetHead = lstBudgetHead;
                ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                Session["AddBudgetAllocationList"] = repo.GetBudgetAllocationCircleList(Convert.ToInt32(Session["UserID"]));
                ViewData["BudgetAllocationCircleList"] = Session["AddBudgetAllocationList"];



            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult AddBudgetInformation(View_BudgetAllocation_Circle model)
        {
            #region Active Status

            long status = 0;
            try
            {
                if (Session["AddBudgetAllocationList"] != null)
                {

                    List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];

                    //var lstFbudget = fQuery.Where(i => lstBudgetAllocationCircle.Any(e => e.FY_ID == i.FY_ID && e.SchemeID == i.SchemeID && i.BudgetHeadID == e.BudgetHeadID && i.SubBudgetHeadID == e.SubBudgetHeadID && i.CIRCLE_CODE == e.CIRCLE_CODE && i.Division == e.Division && i.ISCircleDivision == e.ISCircleDivision));

                    foreach (var items in lstBudgetAllocationCircle)
                    {
                        long id = Convert.ToInt64(items.rowid);
                        tbl_BudgetAllocation_Circle tblBudgCirUpdate = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == id);

                        if (tblBudgCirUpdate != null)
                        {
                            tblBudgCirUpdate.isActive = true;
                            this.fmdsscontext.tbl_BudgetAllocation_Circle.Add(tblBudgCirUpdate);
                            fmdsscontext.Entry(tblBudgCirUpdate).State = System.Data.Entity.EntityState.Modified;

                            #region Maintain Budget Allocation Log UserWise

                            tbl_BudgetAllocation_CircleLevels AllocationCircleLevelelog = new tbl_BudgetAllocation_CircleLevels();
                            AllocationCircleLevelelog.tbl_BudgetAllocation_ParentID = id;
                            AllocationCircleLevelelog.tbl_BudgetAllocation_CircleID = id;
                            AllocationCircleLevelelog.AllocatedAmountCircleLevel = tblBudgCirUpdate.AllocatedAmount;
                            AllocationCircleLevelelog.AllocatedAmountDistribute = tblBudgCirUpdate.AllocatedAmount;

                            AllocationCircleLevelelog.UpdatedBy = Convert.ToInt64(Session["UserId"]);
                            AllocationCircleLevelelog.CreatedBy = Convert.ToInt64(Session["UserId"]);

                            AllocationCircleLevelelog.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                            AllocationCircleLevelelog.UpdatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                            AllocationCircleLevelelog.Status = 1;

                            this.fmdsscontext.tbl_BudgetAllocation_CircleLevels.Add(AllocationCircleLevelelog);
                            fmdsscontext.Entry(AllocationCircleLevelelog).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            #endregion
                        }
                    }
                    status = fmdsscontext.SaveChanges();
                }

            }
            catch (Exception)
            {
                throw;
            }


            #endregion
            return RedirectToAction("AddBudgetInformation");
        }

        public JsonResult AddCircleDetailsMaster(View_BudgetAllocation_Circle objModel)
        {
            try
            {

                #region Maintain the BudgetAllocation List
                List<View_BudgetAllocation_Circle> BudgetAllocationLists = new List<View_BudgetAllocation_Circle>();
                if (Session["AddBudgetAllocationList"] == null)
                {
                    Session["AddBudgetAllocationList"] = new List<View_BudgetAllocation_Circle>();
                }
                else
                {
                    BudgetAllocationLists = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];
                }
                #endregion

                bool Duplicate = CheckDuplicateRecordsMaster(objModel, BudgetAllocationLists);

                if (Duplicate == false)
                {


                    #region Circle and HQ
                    if (objModel != null && (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle"))
                    {
                        if (objModel.ISCircleDivision == "HQ")
                        {
                            objModel.CIRCLE_CODE = "HQ";
                            objModel.CIRCLE_NAME = "HQ";
                        }

                        tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName.Trim().ToLower() == objModel.SiteName.Trim().ToLower())));
                        if (tbl == null)
                        {
                            Mapper.CreateMap<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>();
                            tbl_BudgetAllocation_Circle ObjbudgetCircle = Mapper.Map<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            ObjbudgetCircle.SiteName = !string.IsNullOrEmpty(objModel.SiteName) ? objModel.SiteName.Trim().ToLower() : "n/a";
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetAllocationList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Division
                    if (objModel != null && objModel.ISCircleDivision == "Division")
                    {

                        tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName.Trim().ToLower() == objModel.SiteName.Trim().ToLower())));
                        if (tbl == null)
                        {
                            Mapper.CreateMap<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>();
                            tbl_BudgetAllocation_Circle ObjbudgetCircle = Mapper.Map<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            ObjbudgetCircle.SiteName = !string.IsNullOrEmpty(objModel.SiteName) ? objModel.SiteName.Trim().ToLower() : "n/a";
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetAllocationList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Sanctuary
                    if (objModel != null && objModel.ISCircleDivision == "Sanctuary")
                    {

                        tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName.Trim().ToLower() == objModel.SiteName.Trim().ToLower())));
                        if (tbl == null)
                        {
                            Mapper.CreateMap<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>();
                            tbl_BudgetAllocation_Circle ObjbudgetCircle = Mapper.Map<View_BudgetAllocation_Circle, tbl_BudgetAllocation_Circle>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            ObjbudgetCircle.SiteName = !string.IsNullOrEmpty(objModel.SiteName) ? objModel.SiteName.Trim().ToLower() : "n/a";
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetAllocationList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion
                }
                else
                {
                    objModel.rowid = "D";
                }




            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSanctuaryList(string Div_Code)
        {
            try
            {
                List<SelectListItem> SanctuaryList = new List<SelectListItem>();

                var param1 = new SqlParameter("@Action", "Detail");
                var param2 = new SqlParameter("@DIV_CODE", Div_Code);
                SanctuaryList = fmdsscontext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();
                return Json(SanctuaryList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetDropdownDataByDivision(string Div_Code)
        {
            try
            {
                List<SelectListItem> SanctuaryList = new List<SelectListItem>();

                var param1 = new SqlParameter("@Action", "Detail");
                var param2 = new SqlParameter("@DIV_CODE", Div_Code);
                SanctuaryList = fmdsscontext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();

                List<SelectListItem> lstRange = new List<SelectListItem>();
                var rangeList = fmdsscontext.tbl_mst_Forest_Ranges.AsQueryable().Where(x=>x.DIV_CODE==Div_Code).Select(i => new { i.RANGE_NAME, i.RANGE_CODE }).ToList();
                foreach (var item in rangeList)
                {
                    lstRange.Add(new SelectListItem { Text = item.RANGE_NAME, Value = item.RANGE_CODE });
                }

                return Json(new { SanctuaryList= SanctuaryList, rangeList = lstRange }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public bool CheckDuplicateRecordsMaster(View_BudgetAllocation_Circle objModel, List<View_BudgetAllocation_Circle> List)
        {
            List<View_BudgetAllocation_Circle> lstChkDup = new List<View_BudgetAllocation_Circle>();
            bool duplicate = false;

            #region If Site Name Null Or Empty then Set n/a
            if (string.IsNullOrEmpty(objModel.SiteName))
            {
                objModel.SiteName = "n/a";
            }
            #endregion

            #region Check In Table
            if (objModel != null && objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
            {
                #region Check In Current Session
                int count = List.Where(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower())));
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }

            }
            else if (objModel != null && objModel.ISCircleDivision == "Division")
            {
                #region Check In Current Session
                int count = List.Where(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower())));
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }
            }

            else if (objModel != null && objModel.ISCircleDivision == "Sanctuary")
            {
                #region Check In Current Session
                int count = List.Where(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && true == ((string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower())));
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }
            }
            #endregion




            return duplicate;
        }

        #endregion

        #region Edit Budget Allocation Module
        public ActionResult EditBudgetInformation(string BudgetAllocationID)
        {
            View_BudgetAllocation_Circle model = new View_BudgetAllocation_Circle();
            BudgetAllocationRepo repo = new BudgetAllocationRepo();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                #region Maintain the BudgetAllocation List
                List<View_BudgetAllocation_Circle> BudgetAllocationLists = new List<View_BudgetAllocation_Circle>();
                if (Session["AddBudgetAllocationList"] == null)
                {
                    Session["AddBudgetAllocationList"] = new List<View_BudgetAllocation_Circle>();
                    Session["AddBudgetAllocationList"] = repo.GetBudgetAllocationCircleList(Convert.ToInt32(Session["UserID"]));
                }
                else
                {
                    BudgetAllocationLists = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];
                }


                #endregion

                #region Get BudgetAllocation Details
                List<View_BudgetAllocation_Circle> List = new List<View_BudgetAllocation_Circle>();
                List = (List<View_BudgetAllocation_Circle>)Session["AddBudgetAllocationList"];
                model = List.FirstOrDefault(d => d.ID == Convert.ToInt64(BudgetAllocationID));
                model.Division = model.Div_Code;
                model.AllocatedAmount = model.AllocatedAmount - model.ExtraAllocatedAmount;
                #endregion

                #region Fill DropDown
                List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
                List<SelectListItem> lstScheme = new List<SelectListItem>();
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<SelectListItem> lstSubBudgetHead = new List<SelectListItem>();
                List<SelectListItem> lstActivity = new List<SelectListItem>();
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();

                List<SelectListItem> lstCircle = new List<SelectListItem>();
                List<SelectListItem> lstDivision = new List<SelectListItem>();
                var financialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).ToList();
                foreach (var item in financialYear)
                {
                    lstFinancialYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }

                var budgetHead = fmdsscontext.tbl_mst_BudgetHead.Select(i => new { i.BudgetHead, i.ID }).OrderBy(s => s.BudgetHead);
                foreach (var item in budgetHead)
                {
                    lstBudgetHead.Add(new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) });
                }

                var SubbudgetHead = fmdsscontext.tbl_mst_SubBudgetHead.Where(i => i.BudgetHeadID == model.BudgetHeadID).ToList().Select(i => new { i.SubBudgetHead, i.ID }).OrderBy(s => s.SubBudgetHead);
                foreach (var item in SubbudgetHead)
                {
                    lstSubBudgetHead.Add(new SelectListItem { Text = item.SubBudgetHead, Value = Convert.ToString(item.ID) });
                }

                var Scheme = fmdsscontext.tbl_FDM_SchemeForWidelife.Select(i => new { i.Scheme_Name, i.ID }).OrderBy(s => s.Scheme_Name);
                foreach (var item in Scheme)
                {
                    lstScheme.Add(new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) });
                }
                var activity = fmdsscontext.tbl_mst_ActivityForWidelife.Select(i => new { i.Activity_Name, i.ID }).OrderBy(s => s.Activity_Name);
                foreach (var item in activity)
                {
                    lstActivity.Add(new SelectListItem { Text = item.Activity_Name, Value = Convert.ToString(item.ID) });
                }

                var Subactivity = fmdsscontext.tbl_mst_SUBActivityForWidelife.Where(d => d.ActivityID == model.ActivityID).Select(i => new { i.SUBActivity_Name, i.ID }).OrderBy(s => s.SUBActivity_Name);
                foreach (var item in Subactivity)
                {
                    lstSubActivity.Add(new SelectListItem { Text = item.SUBActivity_Name, Value = Convert.ToString(item.ID) });
                }
                var Circle = fmdsscontext.tbl_mst_Forest_WildLifeCircles.Where(s => s.isBOTH == 1).Select(i => new { i.CIRCLE_NAME, i.CIRCLE_CODE }).OrderBy(s => s.CIRCLE_NAME);
                foreach (var item in Circle)
                {
                    lstCircle.Add(new SelectListItem { Text = item.CIRCLE_NAME, Value = Convert.ToString(item.CIRCLE_CODE) });
                }
                CitizenModel Model = new CitizenModel();
                DataTable dt = Model.GetDivision(Convert.ToString(model.CIRCLE_CODE));
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lstDivision.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                }

                List<SelectListItem> SanctuaryList = new List<SelectListItem>();
                var param1 = new SqlParameter("@Action", "Detail");
                var param2 = new SqlParameter("@DIV_CODE", model.Div_Code);
                ViewBag.SanctuaryList = fmdsscontext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();


                ViewBag.Circle = lstCircle;
                ViewBag.Scheme = lstScheme;
                ViewBag.FinancialYear = lstFinancialYear;
                ViewBag.BudgetHead = lstBudgetHead;
                ViewBag.SubbudgetHead = lstSubBudgetHead;
                ViewBag.Activity = lstActivity;
                ViewBag.SubActivity = lstSubActivity;
                ViewBag.Divisionlst = lstDivision;
                ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditBudgetInformation(View_BudgetAllocation_Circle objModel)
        {
            try
            {
                bool duplicate = CheckDuplicateRecordsEditCase(objModel);
                if (duplicate == false)
                {
                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == objModel.BudgetHeadAllocationID);
                    if (tbl != null)
                    {
                        // string str = Newtonsoft.Json.JsonConvert.SerializeObject(objModel);
                        // tbl = Newtonsoft.Json.JsonConvert.DeserializeObject<tbl_BudgetAllocation_Circle>(str);
                        tbl.BudgetHeadID = objModel.BudgetHeadID;
                        tbl.FY_ID = objModel.FY_ID;
                        tbl.SchemeID = objModel.SchemeID;
                        tbl.SubBudgetHeadID = objModel.SubBudgetHeadID;
                        tbl.ActivityID = objModel.ActivityID;
                        tbl.SubActivityID = objModel.SubActivityID;
                        tbl.SiteName = objModel.SiteName;
                        tbl.IsRecurring = objModel.IsRecurring;
                        tbl.Unit = objModel.Unit;
                        tbl.RatePerUnit = objModel.RatePerUnit;
                        tbl.NumberPerUnit = objModel.NumberPerUnit;

                        tbl.AllocatedAmount = objModel.AllocatedAmount + objModel.ExtraAllocatedAmount;

                        tbl.TotalAmount = objModel.TotalAmount;

                        tbl.ISCircleDivision = objModel.ISCircleDivision;
                        tbl.CIRCLE_CODE = objModel.CIRCLE_CODE;
                        tbl.IsCoreOrBuffer = objModel.IsCoreOrBuffer;
                        if (objModel.ISCircleDivision.Trim().ToLower() == "division")
                            tbl.Division = objModel.Division;
                        else
                        {
                            tbl.Division = "0";
                        }
                        if (objModel.ISCircleDivision.Trim().ToLower() == "sanctuary")
                            tbl.SanctuaryCode = objModel.SanctuaryCode;
                        else
                        {
                            tbl.SanctuaryCode = "0";
                        }

                        tbl.EnteredOn = DateTime.Now;
                        tbl.EnteredBy = Convert.ToInt64(Session["UserId"]);
                        //this.fmdsscontext.tbl_BudgetAllocation_Circle.Add(ObjbudgetCircle);
                        //this.fmdsscontext.tbl_BudgetAllocation_Circle.Add(tbl);
                        fmdsscontext.Entry(tbl).State = System.Data.Entity.EntityState.Modified;
                        fmdsscontext.SaveChanges();

                        #region Maintain Budget Allocation Log UserWise
                        tbl_BudgetAllocation_CircleLevels log = fmdsscontext.tbl_BudgetAllocation_CircleLevels.FirstOrDefault(i => i.tbl_BudgetAllocation_CircleID == objModel.BudgetHeadAllocationID && i.Status == 1);

                        if (log != null)
                        {
                            log.Status = 0;
                            this.fmdsscontext.tbl_BudgetAllocation_CircleLevels.Add(log);
                            fmdsscontext.Entry(log).State = System.Data.Entity.EntityState.Modified;
                            fmdsscontext.SaveChanges();
                        }
                        else
                        {
                            log = new tbl_BudgetAllocation_CircleLevels();
                        }

                        tbl_BudgetAllocation_CircleLevels AllocationCircleLevelelog = new tbl_BudgetAllocation_CircleLevels();
                        AllocationCircleLevelelog.tbl_BudgetAllocation_ParentID = log.tbl_BudgetAllocation_ParentID;
                        AllocationCircleLevelelog.tbl_BudgetAllocation_CircleID = objModel.BudgetHeadAllocationID;
                        AllocationCircleLevelelog.AllocatedAmountDistribute = objModel.AllocatedAmount;
                        AllocationCircleLevelelog.ExtraAllocatedAmount = objModel.ExtraAllocatedAmount;
                        AllocationCircleLevelelog.AllocatedAmountCircleLevel = log.AllocatedAmountCircleLevel;

                        AllocationCircleLevelelog.UpdatedBy = Convert.ToInt64(Session["UserId"]);
                        AllocationCircleLevelelog.CreatedBy = Convert.ToInt64(Session["UserId"]);

                        AllocationCircleLevelelog.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        AllocationCircleLevelelog.UpdatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        AllocationCircleLevelelog.Status = 1;

                        this.fmdsscontext.tbl_BudgetAllocation_CircleLevels.Add(AllocationCircleLevelelog);
                        fmdsscontext.Entry(AllocationCircleLevelelog).State = System.Data.Entity.EntityState.Added;
                        fmdsscontext.SaveChanges();
                        #endregion


                        TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> Budget form has been updated successfully !!! </div>";
                        return RedirectToAction("AddBudgetInformation", "BudgetAllocationCircle");
                    }
                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>This record already Exists !!!</div>";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            return RedirectToAction("EditBudgetInformation", "BudgetAllocationCircle", new { @BudgetAllocationID = objModel.BudgetHeadAllocationID });
        }
        #endregion

        #region Allocation In Circle Level Developed by Rajveer
        public ActionResult AllocationCircle()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            BudgetAllocationCircleLevelModel Model = new BudgetAllocationCircleLevelModel();
            try
            {
                #region Budget Allocated List
                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                Model = repo.GetBudgetAllocationCircleLevel(Convert.ToInt32(Session["UserID"]), Model);
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(Model);
        }
        public ActionResult EditBudgetInformationCircleLevel(string BudgetAllocationID)
        {

            View_BudgetAllocation_Circle model = new View_BudgetAllocation_Circle();
            BudgetAllocationRepo repo = new BudgetAllocationRepo();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                #region Maintain the BudgetAllocation List
                List<View_BudgetAllocation_Circle> BudgetAllocationLists = new List<View_BudgetAllocation_Circle>();
                BudgetAllocationLists = repo.GetBudgetAllocationCircleList(Convert.ToInt32(Session["UserID"]));
                #endregion

                #region Get BudgetAllocation Details
                model = BudgetAllocationLists.FirstOrDefault(d => d.ID == Convert.ToInt64(BudgetAllocationID));
                model.Division = model.Div_Code;
                #endregion

                #region Fill DropDown


                #region Get Circle and Division with userID
                DataSet MasterCircleDivision = new DataSet();
                MasterCircleDivisionWithUserID repomaster = new MasterCircleDivisionWithUserID();
                MasterCircleDivision = repomaster.Select_CircleDivisionWithUserID("ALL", Convert.ToInt64(Session["UserID"]));
                ViewBag.Circle = new SelectList(MasterCircleDivision.Tables[0].AsDataView(), "Value", "Text");
                ViewBag.Divisionlst = new SelectList(MasterCircleDivision.Tables[1].AsDataView(), "Value", "Text");

                List<SelectListItem> SanctuaryList = new List<SelectListItem>();
                var param1 = new SqlParameter("@Action", "Detail");
                var param2 = new SqlParameter("@DIV_CODE", model.Div_Code);
                ViewBag.SanctuaryList = fmdsscontext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();
                #endregion





                ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                #endregion


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult EditBudgetInformationCircleLevel(View_BudgetAllocation_Circle objModel)
        {

            try
            {
                bool duplicate = CheckDuplicateRecordsCircleLevel(objModel);
                if (duplicate == false)
                {
                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == objModel.BudgetHeadAllocationID);
                    Decimal TotalAllocatedAmountCircleLevel = 0;
                    if (tbl != null)
                    {
                        TotalAllocatedAmountCircleLevel = tbl.AllocatedAmount;
                        tbl.AllocatedAmount = tbl.AllocatedAmount - objModel.AllocatedAmount;

                        if (tbl.AllocatedAmount < 0)
                        {
                            throw new Exception("Allocated amount not greter then available amount");
                        }

                        fmdsscontext.Entry(tbl).State = System.Data.Entity.EntityState.Modified;
                        fmdsscontext.SaveChanges();


                        tbl = new tbl_BudgetAllocation_Circle();
                        tbl.BudgetHeadID = objModel.BudgetHeadID;
                        tbl.FY_ID = objModel.FY_ID;
                        tbl.SchemeID = objModel.SchemeID;
                        tbl.SubBudgetHeadID = objModel.SubBudgetHeadID;
                        tbl.ActivityID = objModel.ActivityID;
                        tbl.SubActivityID = objModel.SubActivityID;
                        tbl.SiteName = objModel.SiteName;
                        tbl.IsRecurring = objModel.IsRecurring;
                        tbl.Unit = objModel.Unit;
                        tbl.RatePerUnit = objModel.RatePerUnit;
                        tbl.NumberPerUnit = objModel.NumberPerUnit;

                        tbl.AllocatedAmount = objModel.AllocatedAmount + objModel.ExtraAllocatedAmount;
                        tbl.TotalAmount = objModel.TotalAmount;

                        tbl.ISCircleDivision = objModel.ISCircleDivision;
                        tbl.CIRCLE_CODE = objModel.CIRCLE_CODE;
                        tbl.IsCoreOrBuffer = objModel.IsCoreOrBuffer;
                        if (objModel.ISCircleDivision.Trim().ToLower() == "division")
                            tbl.Division = objModel.Division;
                        else
                        {
                            tbl.Division = "0";
                        }
                        if (objModel.ISCircleDivision.Trim().ToLower() == "sanctuary")
                            tbl.SanctuaryCode = objModel.SanctuaryCode;
                        else
                        {
                            tbl.SanctuaryCode = "0";
                        }

                        tbl.isActive = true;
                        tbl.EnteredOn = DateTime.Now;
                        tbl.EnteredBy = Convert.ToInt64(Session["UserId"]);
                        //this.fmdsscontext.tbl_BudgetAllocation_Circle.Add(ObjbudgetCircle);
                        this.fmdsscontext.tbl_BudgetAllocation_Circle.Add(tbl);
                        fmdsscontext.Entry(tbl).State = System.Data.Entity.EntityState.Added;
                        fmdsscontext.SaveChanges();


                        var ChkCirclelevelLog = fmdsscontext.tbl_BudgetAllocation_CircleLevels.FirstOrDefault(i => i.tbl_BudgetAllocation_ParentID == objModel.BudgetHeadAllocationID && i.Status == 1);
                        if (ChkCirclelevelLog != null)
                        {
                            TotalAllocatedAmountCircleLevel = Convert.ToDecimal(fmdsscontext.tbl_BudgetAllocation_CircleLevels.FirstOrDefault(i => i.tbl_BudgetAllocation_ParentID == objModel.BudgetHeadAllocationID).AllocatedAmountCircleLevel);
                        }

                        tbl_BudgetAllocation_CircleLevels AllocationCircleLevelelog = new tbl_BudgetAllocation_CircleLevels();
                        AllocationCircleLevelelog.tbl_BudgetAllocation_ParentID = objModel.BudgetHeadAllocationID;
                        AllocationCircleLevelelog.tbl_BudgetAllocation_CircleID = tbl.ID;
                        AllocationCircleLevelelog.AllocatedAmountCircleLevel = TotalAllocatedAmountCircleLevel;
                        AllocationCircleLevelelog.AllocatedAmountDistribute = objModel.AllocatedAmount;
                        AllocationCircleLevelelog.UpdatedBy = Convert.ToInt64(Session["UserId"]);
                        AllocationCircleLevelelog.CreatedBy = Convert.ToInt64(Session["UserId"]);
                        AllocationCircleLevelelog.ExtraAllocatedAmount = objModel.ExtraAllocatedAmount;

                        AllocationCircleLevelelog.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        AllocationCircleLevelelog.UpdatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        AllocationCircleLevelelog.Status = 1;

                        this.fmdsscontext.tbl_BudgetAllocation_CircleLevels.Add(AllocationCircleLevelelog);
                        fmdsscontext.Entry(AllocationCircleLevelelog).State = System.Data.Entity.EntityState.Added;
                        fmdsscontext.SaveChanges();

                    }


                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> Budget form has been updated successfully !!! </div>";
                    return RedirectToAction("AllocationCircle", "BudgetAllocationCircle");
                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>This record already Exists !!!</div>";
                }
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later) " + ex.Message + "</div>";
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return RedirectToAction("AllocationCircle", "BudgetAllocationCircle", new
            {
                @BudgetAllocationID = objModel.BudgetHeadAllocationID
            });
        }
        public JsonResult DeleteBudgetAllocationCircleLevel(long Id, long AllocationParentID)
        {
            long status = 0;
            try
            {
                if (Id != 0 && AllocationParentID != 0)
                {
                    tbl_BudgetAllocation_Circle tbl_BudgetAllocation_Circle = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == Id);
                    tbl_BudgetAllocation_Circle tbl_BudgetAllocationParent = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ID == AllocationParentID);

                    if (tbl_BudgetAllocation_Circle != null && tbl_BudgetAllocationParent != null)
                    {
                        #region IsActive Row
                        tbl_BudgetAllocation_Circle.isActive = false;
                        fmdsscontext.Entry(tbl_BudgetAllocation_Circle).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();
                        #endregion

                        #region Update Allocation Amount on Circle Level
                        long userID = Convert.ToInt64(Session["UserID"]);
                        var ChkCirclelevelLog = fmdsscontext.tbl_BudgetAllocation_CircleLevels.Where(i => i.tbl_BudgetAllocation_ParentID == AllocationParentID && i.tbl_BudgetAllocation_CircleID == Id && i.CreatedBy == userID).OrderByDescending(d => d.ID).FirstOrDefault();
                        if (ChkCirclelevelLog != null)
                        {
                            tbl_BudgetAllocationParent.AllocatedAmount = tbl_BudgetAllocationParent.AllocatedAmount + ChkCirclelevelLog.AllocatedAmountDistribute;
                        }
                        else
                        {
                            tbl_BudgetAllocationParent.AllocatedAmount = tbl_BudgetAllocationParent.AllocatedAmount;
                        }
                        ChkCirclelevelLog.Status = 0;
                        fmdsscontext.Entry(ChkCirclelevelLog).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();

                        fmdsscontext.Entry(tbl_BudgetAllocationParent).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();
                        #endregion

                        #region Email and SMS
                        SMS_EMail_Services SE = new SMS_EMail_Services();
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        DataTable DT = objSMSandEMAILtemplate.GetUserDetails(Convert.ToString(Session["UserID"]), "BudgetAllocationDeleteingPersonInfo");
                        DataTable budgetInfo = objSMSandEMAILtemplate.GetUserDetails(Convert.ToString(Id), "BudgetAllocationDeleteingInfo");

                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(budgetInfo);
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);
                        View_BudgetAllocation_Circle model = data.FirstOrDefault();
                        if (budgetInfo != null && budgetInfo.Rows.Count > 0)
                            objSMSandEMAILtemplate.SendMailComman("ALL", "BudgetAllocationDeleteModuleByPCCF", string.Empty, Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "Deleted", null, model.FinancialYear, model.SchemeName, model.BudgetHead, model.SubBudgetHead, model.ActivityName, model.SubActivityName, Convert.ToString(model.AllocatedAmount), model.CIRCLE_NAME, model.Division, model.SanctuaryName, model.ISCircleDivision);
                        #endregion
                    }
                    else
                    {
                        status = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public bool CheckDuplicateRecordsCircleLevel(View_BudgetAllocation_Circle objModel)
        {
            List<View_BudgetAllocation_Circle> lstChkDup = new List<View_BudgetAllocation_Circle>();
            bool duplicate = false;
            #region If Site Name Null Or Empty then Set n/a
            if (string.IsNullOrEmpty(objModel.SiteName))
            {
                objModel.SiteName = "n/a";
            }
            #endregion

            if (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
            {
                int tblCount = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                //tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => (i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true) && i.ID != objModel.BudgetHeadAllocationID).FirstOrDefault();
                //if (tbl != null)
                if (tblCount > 0)
                {
                    duplicate = true;
                }
            }
            if (objModel != null && objModel.ISCircleDivision == "Division")
            {
                int tblCount = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.ISCircleDivision == "Division" && i.Division == objModel.Division && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();

                // tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => (i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true) && i.ID != objModel.BudgetHeadAllocationID);
                //if (tbl != null)
                if (tblCount > 0)
                {
                    duplicate = true;
                }

            }
            if (objModel.ISCircleDivision == "Sanctuary")
            {
                int tblCount = fmdsscontext.tbl_BudgetAllocation_Circle.Where(i => i.FY_ID == objModel.FY_ID && i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true && true == ((string.IsNullOrEmpty(objModel.SiteName) ? i.SiteName == objModel.SiteName : i.SiteName == objModel.SiteName.Trim().ToLower()))).Count();
                // tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => (i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true) && i.ID != objModel.BudgetHeadAllocationID);
                if (tblCount > 0)
                {
                    duplicate = true;
                }
            }


            return duplicate;
        }

        [HttpPost]
        public JsonResult GetAllocationCircleListLog(int Id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<BudgetAllocationLogModel> List = new List<BudgetAllocationLogModel>();
            try
            {
                #region Budget Allocated List
                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                List = repo.GetBudgetAllocationCircleLevelLog(Id, Convert.ToInt64(Session["UserID"]));

                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        #endregion 

    }

}