using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FMDSS.Repository;
using System.Data.SqlClient;
using System.Data.Entity;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using AutoMapper;
using System.Globalization;
using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionServices;
using Newtonsoft.Json;
using System.Data;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class BudgetExpenditureController : Controller
    {
        FmdssContext dbContext;
        public BudgetExpenditureController()
        {
            dbContext = new FmdssContext();
        }
        public ActionResult BudgetExpenditure()
        {


            TempData.Remove("BudgetExpenditure");
            TempData.Remove("ExpendList");
            List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
            List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
            List<SelectListItem> lstScheme = new List<SelectListItem>();
            List<SelectListItem> lstCircle = new List<SelectListItem>();
            List<SelectListItem> lstMonths = new List<SelectListItem>();

            var financialYear = dbContext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).ToList();
            foreach (var item in financialYear)
            {
                lstFinancialYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
            }

            var budgetHead = dbContext.tbl_mst_BudgetHead.Select(i => new { i.BudgetHead, i.ID });
            foreach (var item in budgetHead)
            {

                lstBudgetHead.Add(new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) });
            }

            var Scheme = dbContext.tbl_FDM_SchemeForWidelife.Select(i => new { i.Scheme_Name, i.ID });
            foreach (var item in Scheme)
            {

                lstScheme.Add(new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) });
            }

            var Circle = dbContext.tbl_mst_Forest_WildLifeCircles.Where(s => s.ISWILDLIFECIRCLE == true).Select(i => new { i.CIRCLE_NAME, i.CIRCLE_CODE });
            foreach (var item in Circle)
            {

                lstCircle.Add(new SelectListItem { Text = item.CIRCLE_NAME, Value = Convert.ToString(item.CIRCLE_CODE) });
            }

            string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            foreach (string item in monthNames) // writing out
            {
                if (Convert.ToString(item) != string.Empty)
                    lstMonths.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }
            ViewBag.Circle = lstCircle;
            ViewBag.Scheme = lstScheme;
            ViewBag.FinancialYear = lstFinancialYear;
            ViewBag.BudgetHead = lstBudgetHead;
            ViewBag.Months = lstMonths;
            ViewData["ExpendList"] = new View_Budget_Expenditure().GetExpenditureList(Convert.ToInt32(Session["UserID"]));
            TempData["ExpendList"] = new View_Budget_Expenditure().GetExpenditureList(Convert.ToInt32(Session["UserID"]));
            return View();
        }
        public JsonResult AjaxSubmit(List<View_Budget_Expenditure> lstExpenditure)
        {
            long status = 0;
            try
            {
                if (TempData["BudgetExpenditure"] != null)
                {
                    List<View_Budget_Expenditure> lstBudgetExpenditure = (List<View_Budget_Expenditure>)TempData.Peek("BudgetExpenditure");

                    var query = (from a in dbContext.tbl_Budget_Expenditure
                                 select new
                                 {
                                     a.Id,
                                     a.FY_ID,
                                     a.SchemeID,
                                     a.BudgetHeadID,
                                     a.SubBudgetHeadID,
                                     a.ActivityID,
                                     a.SubActivityID,
                                     a.CIRCLE_CODE,
                                     a.Division,
                                     a.ISCircleDivision,
                                     a.ExpenditureMonths,
                                     a.IsActive
                                 }).ToList();

                    var fQuery = query.Where(i => lstExpenditure.Any(e => e.ID == i.Id));

                    var lstbudgetExpenditure = fQuery.Where(i => lstBudgetExpenditure.Any(e => e.FY_ID == i.FY_ID && e.SchemeID == i.SchemeID && i.BudgetHeadID == e.BudgetHeadID && i.SubBudgetHeadID == e.SubBudgetHeadID && i.CIRCLE_CODE == e.CIRCLE_CODE && i.Division == e.Division && i.ISCircleDivision == e.ISCircleDivision));

                    foreach (var items in lstbudgetExpenditure)
                    {
                        tbl_Budget_Expenditure tblBudgExpenditure = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.Id == items.Id);

                        if (tblBudgExpenditure != null)
                        {
                            tblBudgExpenditure.IsActive = true;
                            this.dbContext.tbl_Budget_Expenditure.Add(tblBudgExpenditure);
                            dbContext.Entry(tblBudgExpenditure).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    status = dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdddExpenditureDetails(View_Budget_Expenditure objModel)
        {
            try
            {
                bool duplicate = CheckDuplicateRecords(objModel);
                TempData.Keep("BudgetExpenditure");
                if (duplicate == false)
                {
                    if (objModel != null && (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle"))
                    {
                        if (objModel != null && objModel.ISCircleDivision == "HQ")
                        {
                            objModel.CIRCLE_CODE = "HQ";
                            objModel.CIRCLE_NAME = "HQ";
                        }
                        tbl_Budget_Expenditure tblExpandCircle = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.ISCircleDivision == objModel.ISCircleDivision && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ"));
                        if (tblExpandCircle == null)
                        {
                            Mapper.CreateMap<View_Budget_Expenditure, tbl_Budget_Expenditure>();
                            tbl_Budget_Expenditure ObjbudgetExpand = Mapper.Map<View_Budget_Expenditure, tbl_Budget_Expenditure>(objModel);
                            ObjbudgetExpand.EnteredOn = DateTime.Now;
                            ObjbudgetExpand.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetExpand.IsActive = false;
                            this.dbContext.tbl_Budget_Expenditure.Add(ObjbudgetExpand);
                            dbContext.Entry(ObjbudgetExpand).State = System.Data.Entity.EntityState.Added;
                            dbContext.SaveChanges();
                            long Id = ObjbudgetExpand.Id;
                            objModel.rowid = Convert.ToString(Id);
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    if (objModel != null && objModel.ISCircleDivision == "Division")
                    {
                        tbl_Budget_Expenditure tblExpandDiv = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == objModel.ISCircleDivision && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && i.ISCircleDivision == "Division");
                        if (tblExpandDiv == null)
                        {
                            Mapper.CreateMap<View_Budget_Expenditure, tbl_Budget_Expenditure>();
                            tbl_Budget_Expenditure ObjbudgetExpand = Mapper.Map<View_Budget_Expenditure, tbl_Budget_Expenditure>(objModel);
                            ObjbudgetExpand.EnteredOn = DateTime.Now;
                            ObjbudgetExpand.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetExpand.IsActive = false;
                            this.dbContext.tbl_Budget_Expenditure.Add(ObjbudgetExpand);
                            dbContext.Entry(ObjbudgetExpand).State = System.Data.Entity.EntityState.Added;
                            dbContext.SaveChanges();
                            long Id = ObjbudgetExpand.Id;
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

        public JsonResult DeleteExpenditure(long Id, string ISCircleDivision)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_Budget_Expenditure tblExpenditure = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.Id == Id);

                    if (tblExpenditure != null)
                    {
                        if (ISCircleDivision == "Circle" || ISCircleDivision == "HQ" || tblExpenditure.ISCircleDivision == "Circle" || tblExpenditure.ISCircleDivision == "HQ")
                        {
                            List<View_Budget_Expenditure> lstExpenditure = (List<View_Budget_Expenditure>)TempData.Peek("BudgetExpenditure");

                            if (lstExpenditure != null)
                            {

                                View_Budget_Expenditure result = lstExpenditure.FirstOrDefault(i => i.FY_ID == tblExpenditure.FY_ID && i.SchemeID == tblExpenditure.SchemeID && i.BudgetHeadID == tblExpenditure.BudgetHeadID && i.SubBudgetHeadID == tblExpenditure.SubBudgetHeadID && i.ActivityID == tblExpenditure.ActivityID && i.SubActivityID == tblExpenditure.SubActivityID && i.CIRCLE_CODE == tblExpenditure.CIRCLE_CODE && i.ExpenditureMonths == tblExpenditure.ExpenditureMonths && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ"));
                                if (result != null)
                                {
                                    lstExpenditure.Remove(result);
                                }
                                TempData["BudgetExpenditure"] = null;
                                TempData["BudgetExpenditure"] = lstExpenditure;
                            }
                        }
                        if (ISCircleDivision == "Division" || tblExpenditure.ISCircleDivision == "Division")
                        {

                            List<View_Budget_Expenditure> lstExpenditure = (List<View_Budget_Expenditure>)TempData.Peek("BudgetExpenditure");

                            if (lstExpenditure != null)
                            {
                                View_Budget_Expenditure result = lstExpenditure.FirstOrDefault(i => i.FY_ID == tblExpenditure.FY_ID && i.SchemeID == tblExpenditure.SchemeID && i.BudgetHeadID == tblExpenditure.BudgetHeadID && i.SubBudgetHeadID == tblExpenditure.SubBudgetHeadID && i.ActivityID == tblExpenditure.ActivityID && i.SubActivityID == tblExpenditure.SubActivityID && i.CIRCLE_CODE == tblExpenditure.CIRCLE_CODE && i.Division == tblExpenditure.Division && i.ExpenditureMonths == tblExpenditure.ExpenditureMonths && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ"));

                                if (result != null)
                                {
                                    lstExpenditure.Remove(result);
                                }
                                TempData["BudgetExpenditure"] = null;
                                TempData["BudgetExpenditure"] = lstExpenditure;
                            }
                        }
                        tblExpenditure.IsActive = false;
                        dbContext.Entry(tblExpenditure).State = System.Data.Entity.EntityState.Modified;
                        status = dbContext.SaveChanges();
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
        public ActionResult BudgetAllocationFilterCircleList(View_Budget_Expenditure obj)
        {

            List<View_Budget_Expenditure> lstBudgetAllocationFilterCircle = new List<View_Budget_Expenditure>();
            try
            {

                TempData["ExpendList"] = new View_Budget_Expenditure().GetExpenditureList(Convert.ToInt32(Session["UserID"]));
                TempData.Remove("BudgetExpenditure");
                if (TempData["ExpendList"] != null)
                {
                    List<View_Budget_Expenditure> lstBudgetAllocationCircle = (List<View_Budget_Expenditure>)TempData.Peek("ExpendList");
                    if (obj.FY_ID != 0 && obj.SchemeID == 0 && obj.BudgetHeadID == 0 && obj.SubBudgetHeadID == 0 && obj.ActivityID == 0 && obj.SubActivityID == 0)
                    {
                        lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID).ToList();
                    }
                    else if (obj.FY_ID != 0 && obj.SchemeID != 0 && obj.BudgetHeadID == 0 && obj.SubBudgetHeadID == 0 && obj.ActivityID == 0 && obj.SubActivityID == 0)
                    {
                        lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID).ToList();
                    }
                    //else if (obj.FY_ID != 0 && obj.SchemeID != 0 && obj.ActivityID != 0 && obj.SubActivityID == 0 && obj.BudgetHeadID == 0 && obj.SubBudgetHeadID == 0)
                    else if (obj.FY_ID != 0 && obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID == 0 && obj.ActivityID == 0 && obj.SubActivityID == 0)
                    {
                        //lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.ActivityID == obj.ActivityID).ToList();
                        lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.BudgetHeadID == obj.BudgetHeadID).ToList();
                    }
                    else if (obj.FY_ID != 0 && obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID != 0 && obj.ActivityID == 0 && obj.SubActivityID == 0)
                    {
                        //lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.ActivityID == obj.ActivityID).ToList();
                        lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.BudgetHeadID == obj.BudgetHeadID && s.SubBudgetHeadID == obj.SubBudgetHeadID).ToList();
                    }

                    else if (obj.FY_ID != 0 && obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID != 0 && obj.ActivityID != 0 && obj.SubActivityID == 0)
                    {
                        //lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.ActivityID == obj.ActivityID).ToList();
                        lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.BudgetHeadID == obj.BudgetHeadID && s.SubBudgetHeadID == obj.SubBudgetHeadID && s.ActivityID == obj.ActivityID).ToList();
                    }

                    else if (obj.FY_ID != 0 && obj.SchemeID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID != 0 && obj.ActivityID != 0 && obj.SubActivityID != 0 && obj.CIRCLE_CODE == null)
                    {
                        //lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.ActivityID == obj.ActivityID).ToList();
                        lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.BudgetHeadID == obj.BudgetHeadID && s.SubBudgetHeadID == obj.SubBudgetHeadID && s.ActivityID == obj.ActivityID && s.SubActivityID == obj.SubActivityID).ToList();
                    }

                    //else if (obj.FY_ID != 0 && obj.SchemeID != 0 && obj.ActivityID != 0 && obj.SubActivityID != 0 && obj.BudgetHeadID == 0 && obj.SubBudgetHeadID == 0)
                    //{
                    //    lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.ActivityID==obj.ActivityID && s.SubActivityID== obj.SubActivityID  ).ToList();
                    //}
                    //else if ((obj.FY_ID != 0 && obj.SchemeID != 0 && obj.ActivityID != 0 && obj.SubActivityID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID == 0 ))
                    //{
                    //    lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.ActivityID == obj.ActivityID && s.SubActivityID == obj.SubActivityID && s.BudgetHeadID==obj.BudgetHeadID).ToList();
                    //}
                    //else if ((obj.FY_ID != 0 && obj.SchemeID != 0 && obj.ActivityID != 0 && obj.SubActivityID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID != 0))
                    //{
                    //    lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.ActivityID == obj.ActivityID && s.SubActivityID == obj.SubActivityID && s.BudgetHeadID == obj.BudgetHeadID && s.SubBudgetHeadID == obj.SubBudgetHeadID).ToList();
                    //}
                    else if ((obj.FY_ID != 0 && obj.SchemeID != 0 && obj.ActivityID != 0 && obj.SubActivityID != 0 && obj.BudgetHeadID != 0 && obj.SubBudgetHeadID != 0 && obj.CIRCLE_CODE != null))
                    {
                        lstBudgetAllocationCircle = lstBudgetAllocationCircle.Where(s => s.FY_ID == obj.FY_ID && s.SchemeID == obj.SchemeID && s.ActivityID == obj.ActivityID && s.SubActivityID == obj.SubActivityID && s.BudgetHeadID == obj.BudgetHeadID && s.SubBudgetHeadID == obj.SubBudgetHeadID && s.CIRCLE_CODE == obj.CIRCLE_CODE).ToList();
                    }

                    foreach (View_Budget_Expenditure objItems in lstBudgetAllocationCircle)
                    {
                        lstBudgetAllocationFilterCircle.Add(new View_Budget_Expenditure
                        {
                            BudgetHeadAllocationID = objItems.BudgetHeadAllocationID,
                            rowid = objItems.rowid,
                            CIRCLE_CODE = objItems.CIRCLE_CODE,
                            CIRCLE_NAME = objItems.CIRCLE_NAME + "/" + objItems.Division,
                            SchemeName = objItems.SchemeName,
                            SchemeID = objItems.SchemeID,
                            ActivityName = objItems.ActivityName,
                            ActivityID = objItems.ActivityID,
                            SubActivityID = objItems.SubActivityID,
                            SubActivityName = objItems.SubActivityName,
                            BudgetHead = objItems.BudgetHead + "/" + objItems.SubBudgetHead,
                            BudgetHeadID = objItems.BudgetHeadID,
                            SubBudgetHeadID = objItems.SubBudgetHeadID,
                            AllocatedAmount = objItems.AllocatedAmount,
                            IsActive = objItems.IsActive,
                            ExpenditureTilldate = objItems.ExpenditureTilldate,
                            ExpenditureMonths = objItems.ExpenditureMonths,
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
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult BudgetAllocationExpenditureList()
        {
            List<View_Budget_Expenditure> lstBudgetExpendlist = new List<View_Budget_Expenditure>();
            try
            {
                lstBudgetExpendlist = new View_Budget_Expenditure().GetExpenditureList(Convert.ToInt32(Session["UserID"]));
                return Json(lstBudgetExpendlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAllocatedAmtDetails(View_Budget_Expenditure objCircle)
        {
            try
            {
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                object[] xparams ={
                          new SqlParameter("@BudgetHeadID", objCircle.BudgetHeadID),
                          new SqlParameter("@SubBudgetHeadID", objCircle.SubBudgetHeadID),
                          new SqlParameter("@FY_ID", objCircle.FY_ID),
                          new SqlParameter("@SchemeID", objCircle.SchemeID),
                          new SqlParameter("@ActivityID", objCircle.ActivityID),
                          new SqlParameter("@SubActivityID", objCircle.SubActivityID),
                          new SqlParameter("@Circle_Code", objCircle.CIRCLE_CODE),
                          new SqlParameter("@Div_Code", objCircle.Division),
                          new SqlParameter("@Option", objCircle.ISCircleDivision)};

                var result = dbContext.Database.SqlQuery<View_Budget_Expenditure>("BA_getAVailableAmount @BudgetHeadID,@SubBudgetHeadID,@FY_ID,@SchemeID,@ActivityID,@SubActivityID,@Circle_Code,@Div_Code,@Option", xparams).ToList();
                var fresult = result.FirstOrDefault();
                decimal AvaliableAmount = fresult.AvailableAmount;
                decimal AllocatedAmount = fresult.AllocatedAmount;
                decimal RemaningAmount = fresult.RemaningAmount;
                long BudgetAllocationHeadId = fresult.BudgetHeadAllocationID;
                var json = Json(new { AvaliableAmount, AllocatedAmount, BudgetAllocationHeadId });
                return json;
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }


        public bool CheckDuplicateRecords(View_Budget_Expenditure objModel)
        {
            List<View_Budget_Expenditure> lstChkDup = new List<View_Budget_Expenditure>();
            bool duplicate = false;
            if (TempData["BudgetExpenditure"] == null)
            {
                if (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
                {

                    tbl_Budget_Expenditure tblExpandCircle = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ"));
                    if (tblExpandCircle == null)
                    {
                        lstChkDup.Add(new View_Budget_Expenditure
                        {
                            FY_ID = objModel.FY_ID,
                            SchemeID = objModel.SchemeID,
                            BudgetHeadID = objModel.BudgetHeadID,
                            SubBudgetHeadID = objModel.SubBudgetHeadID,
                            ActivityID = objModel.ActivityID,
                            SubActivityID = objModel.SubActivityID,
                            CIRCLE_CODE = objModel.CIRCLE_CODE,
                            Division = objModel.Division,
                            ExpenditureMonths = objModel.ExpenditureMonths,
                            ISCircleDivision = objModel.ISCircleDivision
                        });
                        TempData["BudgetExpenditure"] = lstChkDup;
                    }
                    else
                    {
                        objModel.rowid = "D";
                    }

                }
                if (objModel != null && objModel.ISCircleDivision == "Division")
                {
                    tbl_Budget_Expenditure tblExpandDiv = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && i.ISCircleDivision == "Division");
                    if (tblExpandDiv == null)
                    {
                        lstChkDup.Add(new View_Budget_Expenditure
                        {
                            FY_ID = objModel.FY_ID,
                            SchemeID = objModel.SchemeID,
                            BudgetHeadID = objModel.BudgetHeadID,
                            SubBudgetHeadID = objModel.SubBudgetHeadID,
                            ActivityID = objModel.ActivityID,
                            SubActivityID = objModel.SubActivityID,
                            CIRCLE_CODE = objModel.CIRCLE_CODE,
                            Division = objModel.Division,
                            ExpenditureMonths = objModel.ExpenditureMonths,
                            ISCircleDivision = objModel.ISCircleDivision
                        });
                        TempData["BudgetExpenditure"] = lstChkDup;
                    }
                    else
                    {
                        objModel.rowid = "D";
                    }
                }

            }
            else
            {
                List<View_Budget_Expenditure> lstBudgetAllocationCircle = (List<View_Budget_Expenditure>)TempData.Peek("BudgetExpenditure");
                foreach (var item in lstBudgetAllocationCircle)
                {
                    if ((item.ISCircleDivision == "Circle" && objModel.ISCircleDivision == "Circle") || item.ISCircleDivision == "HQ" && objModel.ISCircleDivision == "HQ")
                    {

                        if (item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && item.ExpenditureMonths == objModel.ExpenditureMonths && (item.ISCircleDivision == "Circle" || item.ISCircleDivision == "HQ"))
                        {

                            duplicate = true;
                        }
                    }
                    if (item.ISCircleDivision == "Division" && objModel.ISCircleDivision == "Division")
                    {
                        if (item.FY_ID == objModel.FY_ID && item.SchemeID == objModel.SchemeID && item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID && item.CIRCLE_CODE == objModel.CIRCLE_CODE && item.Division == objModel.Division && item.ExpenditureMonths == objModel.ExpenditureMonths && item.ISCircleDivision == "Division")
                        {
                            duplicate = true;

                        }
                    }
                }

                if (duplicate == false)
                {
                    lstBudgetAllocationCircle.Add(new View_Budget_Expenditure
                    {
                        FY_ID = objModel.FY_ID,
                        SchemeID = objModel.SchemeID,
                        BudgetHeadID = objModel.BudgetHeadID,
                        SubBudgetHeadID = objModel.SubBudgetHeadID,
                        ActivityID = objModel.ActivityID,
                        SubActivityID = objModel.SubActivityID,
                        CIRCLE_CODE = objModel.CIRCLE_CODE,
                        Division = objModel.Division,
                        ExpenditureMonths = objModel.ExpenditureMonths,
                        ISCircleDivision = objModel.ISCircleDivision
                    });
                    TempData["BudgetExpenditure"] = null;
                    TempData["BudgetExpenditure"] = lstBudgetAllocationCircle;
                }
            }
            return duplicate;
        }

        public JsonResult GetDivision(string circleCode)
        {
            try
            {
                List<SelectListItem> lstDivision = new List<SelectListItem>();
                var result = dbContext.tbl_mst_Forest_Divisions.Where(i => i.CIRCLE_CODE == circleCode && i.ForBudgetModuleDist == 1).Select(i => new { i.DIV_CODE, i.DIV_NAME }).OrderBy(s => s.DIV_NAME);
                foreach (var item in result)
                {
                    lstDivision.Add(new SelectListItem { Text = item.DIV_NAME, Value = item.DIV_CODE });
                }
                return Json(lstDivision, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetActivity()
        {
            try
            {
                List<SelectListItem> lstActivity = new List<SelectListItem>();
                var result = dbContext.tbl_mst_ActivityForWidelife.Where(i => i.IsActive == 1).Select(i => new { i.Activity_Name, i.ID }).OrderBy(s => s.Activity_Name);

                foreach (var item in result)
                {
                    lstActivity.Add(new SelectListItem { Text = item.Activity_Name, Value = Convert.ToString(item.ID) });
                }
                return Json(lstActivity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        public JsonResult GetSubActivity(long ActivityID)
        {
            try
            {
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();

                var result = dbContext.tbl_mst_SUBActivityForWidelife.Where(i => i.IsActive == 1 && i.ActivityID == ActivityID).Select(i => new { i.SUBActivity_Name, i.ID }).OrderBy(s => s.SUBActivity_Name);
                if (result.Count() > 0)
                {

                    foreach (var item in result)
                    {
                        lstSubActivity.Add(new SelectListItem { Text = item.SUBActivity_Name, Value = Convert.ToString(item.ID) });
                    }

                }
                else
                {
                    lstSubActivity.Add(new SelectListItem { Text = "none", Value = "0" });
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
                var query = (from a in dbContext.tbl_mst_SubBudgetHead
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

        public void FillDropDownMaster()
        {
            List<SelectListItem> FinanceYear = new List<SelectListItem>();
            List<SelectListItem> SchemeList = new List<SelectListItem>();
            List<SelectListItem> BudgetHead = new List<SelectListItem>();
            List<SelectListItem> Activity = new List<SelectListItem>();
            List<SelectListItem> Circle = new List<SelectListItem>();
            try
            {
                #region Circle Bind
                var circle = dbContext.tbl_mst_Forest_WildLifeCircles.Where(e => e.isBOTH == 1).Select(i => new { i.CIRCLE_CODE, i.CIRCLE_NAME }).Distinct().ToList().OrderBy(d => d.CIRCLE_NAME);
                Circle.Add(new SelectListItem { Text = "HQ", Value = "HQ" });
                foreach (var item in circle)
                {
                    Circle.Add(new SelectListItem { Text = item.CIRCLE_NAME, Value = item.CIRCLE_CODE });
                }


                ViewBag.Circle = Circle;
                #endregion

                #region FinanceYear Bind
                var financialYear = dbContext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).Distinct().ToList();
                foreach (var item in financialYear)
                {
                    FinanceYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }
                ViewBag.Financial = FinanceYear;
                #endregion

                #region Scheme Bind
                var SchemeLists = dbContext.tbl_FDM_SchemeForWidelife.Select(i => new { i.Scheme_Name, i.ID }).Distinct().ToList().OrderBy(d => d.Scheme_Name);
                foreach (var item in SchemeLists)
                {
                    SchemeList.Add(new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) });
                }
                ViewBag.SchemeList = SchemeList;
                #endregion

                #region Activity Bind
                var ActivityList = dbContext.tbl_mst_ActivityForWidelife.Select(i => new { i.Activity_Name, i.ID }).Distinct().ToList().OrderBy(d => d.Activity_Name);
                foreach (var item in ActivityList)
                {
                    Activity.Add(new SelectListItem { Text = item.Activity_Name, Value = Convert.ToString(item.ID) });
                }
                ViewBag.Activity = Activity;
                #endregion

                #region Budget Head Bind
                var BudgetHeadList = dbContext.tbl_mst_BudgetHead.Select(i => new { i.BudgetHead, i.ID }).Distinct().ToList().OrderBy(d => d.BudgetHead);
                foreach (var item in BudgetHeadList)
                {
                    BudgetHead.Add(new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) });
                }
                ViewBag.BudgetHead = BudgetHead;
                #endregion

            }
            catch (Exception ex)
            {
                ViewBag.Financial = new List<SelectListItem>();
                ViewBag.Circle = new List<SelectListItem>();
                ViewBag.Activity = new List<SelectListItem>();
                ViewBag.BudgetHead = new List<SelectListItem>();
            }
        }

        #region BudgetExpenditure Developed by Rajveer
        public ActionResult BudgetExpenditureAllocation()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            BudgetExpenditureModel model = new BudgetExpenditureModel();
            Session["AddBudgetExpenditureAllocationList"] = null;
            Session["BudgetAllocationList"] = null;
            Session["AddBudgetExpenditureAllocationListFilter"] = null;
            Session["BudgetAllocationListFilter"] = null;
            Session["BudgetExenditureGISinformation"] = null;
            try
            {


                ViewBag.DivisionList = new List<SelectListItem>();
                ViewBag.SantauryList = new List<SelectListItem>();
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();

                #region Fill the month
                string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                List<SelectListItem> lstMonths = new List<SelectListItem>();
                foreach (string item in monthNames) // writing out
                {
                    if (Convert.ToString(item) != string.Empty)
                        lstMonths.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                }
                ViewBag.Months = lstMonths;
                #endregion


                #region Budget Expenditure List
                model.BudgetExpenditureList = new View_Budget_Expenditure().GetExpenditureList(Convert.ToInt32(Session["UserID"]));
                Session["AddBudgetExpenditureAllocationListFilter"] = model.BudgetExpenditureList;
                #endregion

                #region Budget Allocated List
                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                model.BudgetAllocationList = repo.GetBudgetAllocationCircleList(Convert.ToInt32(Session["UserID"]));
                Session["BudgetAllocationList"] = model.BudgetAllocationList;
                Session["BudgetAllocationListFilter"] = model.BudgetAllocationList;
                #endregion

                #region Show Pop up partial view Budget expenditute module
                try
                {

                    var param2 = new SqlParameter("@UserID", Convert.ToInt32(Session["UserID"]));
                    model.BudgetExpenditureModels.HeaderPopUpShow = Convert.ToInt32(dbContext.Database.SqlQuery<int>("SP_GetDesignationOfficePartialViewShowHideBudgetModule @UserID", param2).FirstOrDefault());

                }
                catch (Exception ex)
                {
                }
                #endregion

                #region Fill Dropdown Accoding to the Budget Allocation
                FillDropDownMasterWithUserID(model.BudgetAllocationList);
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult BudgetExpenditureAllocation(BudgetExpenditureModel model)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            BudgetExpenditureModel modeldata = new BudgetExpenditureModel();

            try
            {
                long status = 0;
                try
                {

                   

                    ViewBag.DivisionList = new List<SelectListItem>();
                    ViewBag.SantauryList = new List<SelectListItem>();
                    ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                    ViewBag.SubActivityList = new List<SelectListItem>();
                    #region Fill the month
                    string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                    List<SelectListItem> lstMonths = new List<SelectListItem>();
                    foreach (string item in monthNames) // writing out
                    {
                        if (Convert.ToString(item) != string.Empty)
                            lstMonths.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                    }
                    ViewBag.Months = lstMonths;
                    #endregion

                    #region Maintain the Budget Allocation List
                    List<View_BudgetAllocation_Circle> BudgetAllocationLists = new List<View_BudgetAllocation_Circle>();
                    if (Session["BudgetAllocationListFilter"] == null)
                    {
                        Session["BudgetAllocationListFilter"] = BudgetAllocationLists;
                    }
                    else
                    {
                        BudgetAllocationLists = (List<View_BudgetAllocation_Circle>)Session["BudgetAllocationListFilter"];
                    }

                    #region Fill Dropdown Accoding to the Budget Allocation
                    FillDropDownMasterWithUserID(BudgetAllocationLists);
                    #endregion

                    if (BudgetAllocationLists!=null)
                    {
                        BudgetAllocationLists = BudgetAllocationLists.Where(d => d.FY_ID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.FinancialYear) ? Convert.ToInt32(model.BudgetExpenditureModels.FinancialYear) : d.FY_ID) && d.SchemeID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SchemeName) ? Convert.ToInt32(model.BudgetExpenditureModels.SchemeName) : d.SchemeID) && d.BudgetHeadID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.BudgetHead) ? Convert.ToInt32(model.BudgetExpenditureModels.BudgetHead) : d.BudgetHeadID) && d.SubBudgetHeadID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SubBudgetHead) ? Convert.ToInt32(model.BudgetExpenditureModels.SubBudgetHead) : d.SubBudgetHeadID) && d.ActivityID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.ActivityName) ? Convert.ToInt32(model.BudgetExpenditureModels.ActivityName) : d.ActivityID) && d.SubActivityID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SubActivityName) ? Convert.ToInt32(model.BudgetExpenditureModels.SubActivityName) : d.SubActivityID) && d.CIRCLE_CODE == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.CIRCLE_CODE) ? Convert.ToString(model.BudgetExpenditureModels.CIRCLE_CODE) : d.CIRCLE_CODE) && d.Div_Code == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.Division) ? Convert.ToString(model.BudgetExpenditureModels.Division) : d.Div_Code) && d.SanctuaryCode == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SanctuaryCode) ? Convert.ToString(model.BudgetExpenditureModels.SanctuaryCode.Trim()) : d.SanctuaryCode)).ToList();
                    }


                        modeldata.BudgetAllocationList = BudgetAllocationLists;
                    #endregion

                   
                    #region Maintain the Budget Allocation Expenditure List
                    List<View_Budget_Expenditure> BudgetExpenditureAllocationLists = new List<View_Budget_Expenditure>();
                    if (Session["AddBudgetExpenditureAllocationListFilter"] == null)
                    {
                        Session["AddBudgetExpenditureAllocationListFilter"] = BudgetExpenditureAllocationLists;
                    }
                    else
                    {
                        BudgetExpenditureAllocationLists = (List<View_Budget_Expenditure>)Session["AddBudgetExpenditureAllocationListFilter"];
                        BudgetExpenditureAllocationLists = BudgetExpenditureAllocationLists.Where(d => d.FY_ID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.FinancialYear) ? Convert.ToInt32(model.BudgetExpenditureModels.FinancialYear) : d.FY_ID) && d.SchemeID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SchemeName) ? Convert.ToInt32(model.BudgetExpenditureModels.SchemeName) : d.SchemeID) && d.BudgetHeadID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.BudgetHead) ? Convert.ToInt32(model.BudgetExpenditureModels.BudgetHead) : d.BudgetHeadID) && d.SubBudgetHeadID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SubBudgetHead) ? Convert.ToInt32(model.BudgetExpenditureModels.SubBudgetHead) : d.SubBudgetHeadID) && d.ActivityID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.ActivityName) ? Convert.ToInt32(model.BudgetExpenditureModels.ActivityName) : d.ActivityID) && d.SubActivityID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SubActivityName) ? Convert.ToInt32(model.BudgetExpenditureModels.SubActivityName) : d.SubActivityID) && d.CIRCLE_CODE == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.CIRCLE_CODE) ? Convert.ToString(model.BudgetExpenditureModels.CIRCLE_CODE) : d.CIRCLE_CODE) && d.Division == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.Division) ? Convert.ToString(model.BudgetExpenditureModels.Division) : d.Division) && d.SanctuaryCode == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SanctuaryCode) ? Convert.ToString(model.BudgetExpenditureModels.SanctuaryCode.Trim()) : d.SanctuaryCode)).ToList();
                    }
                    modeldata.BudgetExpenditureList = BudgetExpenditureAllocationLists;
                    #endregion

                    #region Fill Division and Santaury And
                    #region Fill Division
                    List<SelectListItem> lstDivision = new List<SelectListItem>();

                    try
                    {

                        var result = dbContext.tbl_mst_Forest_Divisions.Where(i => i.CIRCLE_CODE == model.BudgetExpenditureModels.CIRCLE_CODE && i.ForBudgetModuleDist == 1).Select(i => new { i.DIV_CODE, i.DIV_NAME }).OrderBy(s => s.DIV_NAME);
                        foreach (var item in result)
                        {
                            lstDivision.Add(new SelectListItem { Text = item.DIV_NAME, Value = item.DIV_CODE });
                        }
                        ViewBag.DivisionList = lstDivision;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DivisionList = new List<SelectListItem>();

                    }
                    #endregion

                    #region Fill Santaury
                    try
                    {
                        List<SelectListItem> SanctuaryList = new List<SelectListItem>();

                        var param1 = new SqlParameter("@Action", "Detail");
                        var param2 = new SqlParameter("@DIV_CODE", model.BudgetExpenditureModels.Division);
                        ViewBag.SantauryList = dbContext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();

                    }
                    catch (Exception ex)
                    {
                        ViewBag.SantauryList = new List<SelectListItem>();
                    }
                    #endregion

                    #region Fill SubBudgethead
                    long BudgetHeadID = Convert.ToInt64(model.BudgetExpenditureModels.BudgetHead);
                    ViewBag.SubBudgetHeadList = dbContext.tbl_mst_SubBudgetHead.Where(s => s.BudgetHeadID == BudgetHeadID).OrderBy(d => d.SubBudgetHead).ToList().Select(d => new SelectListItem() { Text = d.SubBudgetHead, Value = d.ID.ToString() }).ToList();
                    #endregion

                    #region Fill SubActivityhead
                    int ActivityID = !string.IsNullOrEmpty(model.BudgetExpenditureModels.ActivityName) ? Convert.ToInt32(model.BudgetExpenditureModels.ActivityName) : 0;
                    ViewBag.SubActivityList = dbContext.tbl_mst_SUBActivityForWidelife.Where(i => i.IsActive == 1 && i.ActivityID == ActivityID).OrderBy(d => d.SUBActivity_Name).ToList().Select(item => new SelectListItem { Text = item.SUBActivity_Name, Value = Convert.ToString(item.ID) }).ToList();
                    #endregion
                    #endregion

                    #region Show Pop up partial view Budget expenditute module
                    try
                    {
                        var param2 = new SqlParameter("@UserID", Convert.ToInt32(Session["UserID"]));
                        modeldata.BudgetExpenditureModels.HeaderPopUpShow = Convert.ToInt32(dbContext.Database.SqlQuery<int>("SP_GetDesignationOfficePartialViewShowHideBudgetModule @UserID", param2).FirstOrDefault());

                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(modeldata);
        }


        public ActionResult BudgetExpenditureAllocationSave(BudgetExpenditureModel model)
        {

            try
            {
                long status = 0;
                try
                {
                    if (Session["AddBudgetExpenditureAllocationList"] != null)
                    {

                        List<View_Budget_Expenditure> BudgetExpenditureList = (List<View_Budget_Expenditure>)Session["AddBudgetExpenditureAllocationList"];

                        //var lstFbudget = fQuery.Where(i => lstBudgetAllocationCircle.Any(e => e.FY_ID == i.FY_ID && e.SchemeID == i.SchemeID && i.BudgetHeadID == e.BudgetHeadID && i.SubBudgetHeadID == e.SubBudgetHeadID && i.CIRCLE_CODE == e.CIRCLE_CODE && i.Division == e.Division && i.ISCircleDivision == e.ISCircleDivision));

                        foreach (var items in BudgetExpenditureList)
                        {
                            long id = Convert.ToInt64(items.rowid);
                            tbl_Budget_Expenditure tblBudgExpUpdate = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.Id == id);

                            if (tblBudgExpUpdate != null)
                            {
                                tblBudgExpUpdate.IsActive = true;
                                this.dbContext.tbl_Budget_Expenditure.Add(tblBudgExpUpdate);
                                dbContext.Entry(tblBudgExpUpdate).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        status = dbContext.SaveChanges();
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
            return RedirectToAction("BudgetExpenditureAllocation");
        }
        public JsonResult BudgetExpenditureAllocationDetails(string ID)
        {
            List<View_BudgetAllocation_Circle> BudgetAllocationList = new List<View_BudgetAllocation_Circle>();
            View_BudgetAllocation_Circle BudgetAllocationDetails = new View_BudgetAllocation_Circle();

            Session["AddBudgetExpenditureAllocationList"] = new List<View_Budget_Expenditure>();
            try
            {
                BudgetAllocationList = (List<View_BudgetAllocation_Circle>)Session["BudgetAllocationList"];
                if (BudgetAllocationList.Count > 0)
                {
                    BudgetAllocationDetails = BudgetAllocationList.FirstOrDefault(s => s.ID == Convert.ToInt64(ID));
                }

            }
            catch (Exception ex)
            {

            }
            return Json(BudgetAllocationDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddExpenditureDetails(View_Budget_Expenditure objModel)
        {
            try
            {
                #region Maintain the BudgetAllocation List
                List<View_Budget_Expenditure> BudgetExpenditureAllocationLists = new List<View_Budget_Expenditure>();
                if (Session["AddBudgetExpenditureAllocationList"] == null)
                {
                    Session["AddBudgetExpenditureAllocationList"] = new List<View_Budget_Expenditure>();
                }
                else
                {
                    BudgetExpenditureAllocationLists = (List<View_Budget_Expenditure>)Session["AddBudgetExpenditureAllocationList"];
                }
                #endregion


                bool duplicate = CheckAddBudgetExpenditureDuplicate(objModel, BudgetExpenditureAllocationLists);
                if (duplicate == false)
                {
                    if (string.IsNullOrEmpty(objModel.SanctuaryCode))
                    {
                        objModel.SanctuaryCode = string.Empty;
                    }

                    #region Circle and HQ
                    if (objModel != null && (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle"))
                    {
                        if (objModel != null && objModel.ISCircleDivision == "HQ")
                        {
                            objModel.CIRCLE_CODE = "HQ";
                            objModel.CIRCLE_NAME = "HQ";
                        }
                        tbl_Budget_Expenditure tblExpandCircle = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.ISCircleDivision == objModel.ISCircleDivision && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim()));
                        if (tblExpandCircle == null)
                        {
                            Mapper.CreateMap<View_Budget_Expenditure, tbl_Budget_Expenditure>();
                            tbl_Budget_Expenditure ObjbudgetExpand = Mapper.Map<View_Budget_Expenditure, tbl_Budget_Expenditure>(objModel);
                            ObjbudgetExpand.EnteredOn = DateTime.Now;
                            ObjbudgetExpand.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetExpand.IsActive = false;
                            ObjbudgetExpand.BudgetAllocation_CircleID = objModel.BudgetHeadAllocationID;
                            this.dbContext.tbl_Budget_Expenditure.Add(ObjbudgetExpand);
                            dbContext.Entry(ObjbudgetExpand).State = System.Data.Entity.EntityState.Added;
                            dbContext.SaveChanges();
                            long Id = ObjbudgetExpand.Id;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetExpenditureAllocationLists.Add(objModel);
                            Session["AddBudgetExpenditureAllocationList"] = BudgetExpenditureAllocationLists;
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
                        tbl_Budget_Expenditure tblExpandDiv = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == objModel.ISCircleDivision && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && i.ISCircleDivision == "Division" && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim()));
                        if (tblExpandDiv == null)
                        {
                            Mapper.CreateMap<View_Budget_Expenditure, tbl_Budget_Expenditure>();
                            tbl_Budget_Expenditure ObjbudgetExpand = Mapper.Map<View_Budget_Expenditure, tbl_Budget_Expenditure>(objModel);
                            ObjbudgetExpand.EnteredOn = DateTime.Now;
                            ObjbudgetExpand.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetExpand.IsActive = false;
                            ObjbudgetExpand.BudgetAllocation_CircleID = objModel.BudgetHeadAllocationID;
                            this.dbContext.tbl_Budget_Expenditure.Add(ObjbudgetExpand);
                            dbContext.Entry(ObjbudgetExpand).State = System.Data.Entity.EntityState.Added;
                            dbContext.SaveChanges();
                            long Id = ObjbudgetExpand.Id;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetExpenditureAllocationLists.Add(objModel);
                            Session["AddBudgetExpenditureAllocationList"] = BudgetExpenditureAllocationLists;
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
                        tbl_Budget_Expenditure tblExpandDiv = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == objModel.ISCircleDivision && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim()));
                        if (tblExpandDiv == null)
                        {
                            Mapper.CreateMap<View_Budget_Expenditure, tbl_Budget_Expenditure>();
                            tbl_Budget_Expenditure ObjbudgetExpand = Mapper.Map<View_Budget_Expenditure, tbl_Budget_Expenditure>(objModel);
                            ObjbudgetExpand.EnteredOn = DateTime.Now;
                            ObjbudgetExpand.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetExpand.BudgetAllocation_CircleID = objModel.BudgetHeadAllocationID;
                            ObjbudgetExpand.IsActive = false;
                            this.dbContext.tbl_Budget_Expenditure.Add(ObjbudgetExpand);
                            dbContext.Entry(ObjbudgetExpand).State = System.Data.Entity.EntityState.Added;
                            dbContext.SaveChanges();
                            long Id = ObjbudgetExpand.Id;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetExpenditureAllocationLists.Add(objModel);
                            Session["AddBudgetExpenditureAllocationList"] = BudgetExpenditureAllocationLists;
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

        public bool CheckAddBudgetExpenditureDuplicate(View_Budget_Expenditure objModel, List<View_Budget_Expenditure> List)
        {
            List<View_Budget_Expenditure> lstChkDup = new List<View_Budget_Expenditure>();
            bool duplicate = false;

            #region Check In Table
            if (objModel != null && objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
            {
                #region Check In Current Session
                int count = List.Where(i =>i.IsCoreOrBuffer==objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.ExpenditureMonths == objModel.ExpenditureMonths && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim())).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    //tbl_Budget_Expenditure tbl = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ"));
                    int dbCount = dbContext.tbl_Budget_Expenditure.Where(i => i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim())).Count();
                    if (dbCount > 0)
                    {
                        duplicate = true;
                    }
                }

            }
            else if (objModel != null && objModel.ISCircleDivision == "Division")
            {
                #region Check In Current Session
                int count = List.Where(i => i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ExpenditureMonths == objModel.ExpenditureMonths && i.ISCircleDivision == "Division" && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim())).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    //tbl_Budget_Expenditure tbl = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && i.ISCircleDivision == "Division");
                    int DBCount = dbContext.tbl_Budget_Expenditure.Where(i => i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && i.ISCircleDivision == "Division" && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim())).Count();
                    if (DBCount > 0)
                    {
                        duplicate = true;
                    }
                }
            }
            else if (objModel != null && objModel.ISCircleDivision == "Sanctuary")
            {
                #region Check In Current Session
                int count = List.Where(i => i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ExpenditureMonths == objModel.ExpenditureMonths && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim())).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_Budget_Expenditure tbl = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.IsCoreOrBuffer == objModel.IsCoreOrBuffer && i.FY_ID == objModel.FY_ID && i.SchemeID == objModel.SchemeID && i.ActivityID == objModel.ActivityID && i.SubActivityID == objModel.SubActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ExpenditureMonths == objModel.ExpenditureMonths && i.IsActive == true && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && true == (string.IsNullOrEmpty(objModel.SiteName) ? true : i.SiteName.Trim() == objModel.SiteName.Trim()));
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }
            }
            #endregion




            return duplicate;
        }

        public JsonResult DeleteExpenditureDetailsCurrentSession(long Id)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    #region Maintain the BudgetAllocation List
                    List<View_Budget_Expenditure> BudgetExpenditureAllocationLists = new List<View_Budget_Expenditure>();
                    if (Session["AddBudgetExpenditureAllocationList"] == null)
                    {
                        Session["AddBudgetExpenditureAllocationList"] = new List<View_Budget_Expenditure>();
                    }
                    else
                    {
                        BudgetExpenditureAllocationLists = (List<View_Budget_Expenditure>)Session["AddBudgetExpenditureAllocationList"];
                    }
                    #endregion

                    #region Delete Data Current Session
                    if (BudgetExpenditureAllocationLists.Count > 0)
                    {
                        View_Budget_Expenditure result = BudgetExpenditureAllocationLists.FirstOrDefault(i => Convert.ToInt64(i.rowid) == Id);
                        if (result != null)
                        {
                            BudgetExpenditureAllocationLists.Remove(result);
                            status = 1;
                        }
                        TempData["AddBudgetExpenditureAllocationList"] = null;
                        TempData["AddBudgetExpenditureAllocationList"] = BudgetExpenditureAllocationLists;
                    }
                    else
                    {
                        status = 0;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExpAmtDetails(View_Budget_Expenditure objCircle)
        {
            try
            {
                if (string.IsNullOrEmpty(objCircle.SanctuaryCode))
                {
                    objCircle.SanctuaryCode = string.Empty;
                }
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                object[] xparams ={
                          new SqlParameter("@BudgetHeadID", objCircle.BudgetHeadID),
                          new SqlParameter("@SubBudgetHeadID", objCircle.SubBudgetHeadID),
                          new SqlParameter("@FY_ID", objCircle.FY_ID),
                          new SqlParameter("@SchemeID", objCircle.SchemeID),
                          new SqlParameter("@ActivityID", objCircle.ActivityID),
                          new SqlParameter("@SubActivityID", objCircle.SubActivityID),
                          new SqlParameter("@Circle_Code", objCircle.CIRCLE_CODE),
                          new SqlParameter("@Div_Code", objCircle.Division),
                           new SqlParameter("@SanctuaryCode", objCircle.SanctuaryCode),
                            new SqlParameter("@SiteName", objCircle.SiteName),
                             new SqlParameter("@IsCoreOrBuffer", objCircle.IsCoreOrBuffer),
                          new SqlParameter("@Option", "BudgetAllocationExpenditureTotalAmount")};

                var result = dbContext.Database.SqlQuery<View_Budget_Expenditure>("SP_GetExpentitureToatlAmount @BudgetHeadID,@SubBudgetHeadID,@FY_ID,@SchemeID,@ActivityID,@SubActivityID,@Circle_Code,@Div_Code,@SanctuaryCode,@SiteName,@IsCoreOrBuffer,@Option", xparams).ToList();
                if (result.Count > 0)
                {
                    var fresult = result.FirstOrDefault();
                    //decimal AvaliableAmount = fresult.AvailableAmount;
                    decimal AllocatedAmount = fresult.AllocatedAmount;
                    decimal RemaningAmount = fresult.RemaningAmount;
                    //long BudgetAllocationHeadId = fresult.BudgetHeadAllocationID;
                    string RemainingFlagforContainZero = "T";
                    var json = Json(new { AllocatedAmount, RemaningAmount, RemainingFlagforContainZero });
                    return json;
                }
                else
                {
                    string AllocatedAmount = string.Empty;
                    string RemaningAmount = string.Empty;
                    string RemainingFlagforContainZero = "F";
                    var json = Json(new { AllocatedAmount, RemaningAmount, RemainingFlagforContainZero });
                    return json;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult DeleteBudgetExpenditureEntry(long Id)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_Budget_Expenditure tblBudgetExpenditure = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.Id == Id);

                    if (tblBudgetExpenditure != null)
                    {
                        tblBudgetExpenditure.IsActive = false;
                        dbContext.Entry(tblBudgetExpenditure).State = System.Data.Entity.EntityState.Modified;
                        status = dbContext.SaveChanges();
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
        #endregion


        #region New Budget Screen Developed by Rajveer
        public ActionResult BudgetExpenditureAllocationMaster()
        {
            BudgetExpenditureModel model = new BudgetExpenditureModel();
            Session["AddBudgetExpenditureAllocationList"] = null;
            Session["BudgetAllocationList"] = null;
            try
            {
                #region Fill the month
                string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                List<SelectListItem> lstMonths = new List<SelectListItem>();
                foreach (string item in monthNames) // writing out
                {
                    if (Convert.ToString(item) != string.Empty)
                        lstMonths.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                }
                ViewBag.Months = lstMonths;
                #endregion


                #region Budget Expenditure List
                model.BudgetExpenditureList = new View_Budget_Expenditure().GetExpenditureList(Convert.ToInt32(Session["UserID"]));
                #endregion

                #region Budget Allocated List
                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                model.BudgetAllocationList = repo.GetBudgetAllocationCircleList(Convert.ToInt32(Session["UserID"]));
                Session["BudgetAllocationList"] = model.BudgetAllocationList;
                #endregion

                #region Set Month

                model.DictionaryList.Add("January", "0");
                model.DictionaryList.Add("February", "0");
                model.DictionaryList.Add("March", "0");

                model.DictionaryList.Add("April", "0");
                model.DictionaryList.Add("May", "0");
                model.DictionaryList.Add("June", "0");

                model.DictionaryList.Add("July", "0");
                model.DictionaryList.Add("August", "0");
                model.DictionaryList.Add("September", "0");

                model.DictionaryList.Add("October", "0");
                model.DictionaryList.Add("November", "0");
                model.DictionaryList.Add("December", "0");

                #endregion
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult BudgetExpenditureAllocationMaster(BudgetExpenditureModel model)
        {
            try
            {
                long status = 0;
                try
                {
                    if (Session["AddBudgetExpenditureAllocationList"] != null)
                    {

                        List<View_Budget_Expenditure> BudgetExpenditureList = (List<View_Budget_Expenditure>)Session["AddBudgetExpenditureAllocationList"];

                        //var lstFbudget = fQuery.Where(i => lstBudgetAllocationCircle.Any(e => e.FY_ID == i.FY_ID && e.SchemeID == i.SchemeID && i.BudgetHeadID == e.BudgetHeadID && i.SubBudgetHeadID == e.SubBudgetHeadID && i.CIRCLE_CODE == e.CIRCLE_CODE && i.Division == e.Division && i.ISCircleDivision == e.ISCircleDivision));

                        foreach (var items in BudgetExpenditureList)
                        {
                            long id = Convert.ToInt64(items.rowid);
                            tbl_Budget_Expenditure tblBudgExpUpdate = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.Id == id);

                            if (tblBudgExpUpdate != null)
                            {
                                tblBudgExpUpdate.IsActive = true;
                                this.dbContext.tbl_Budget_Expenditure.Add(tblBudgExpUpdate);
                                dbContext.Entry(tblBudgExpUpdate).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        status = dbContext.SaveChanges();
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
            return RedirectToAction("BudgetExpenditureAllocationMaster");
        }

        [HttpPost]
        public ActionResult BudgetExpenditureAllocationSaveMonthly(BudgetExpenditureModel objModel)
        {

            long status = 0;
            try
            {
                if (objModel.BudgetExpenditureModels != null && objModel.DictionaryList != null)
                {
                    string Month = string.Empty;
                    decimal MonthValue = 0;
                    foreach (var itm in objModel.DictionaryList.ToList().Where(s => Convert.ToDecimal(s.Value) > 0))
                    {
                        Month = Convert.ToString(itm.Key);
                        MonthValue = Convert.ToDecimal(itm.Value);


                        #region Circle and HQ
                        if (objModel.BudgetExpenditureModels != null && (objModel.BudgetExpenditureModels.ISCircleDivision == "HQ" || objModel.BudgetExpenditureModels.ISCircleDivision == "Circle"))
                        {
                            if (objModel != null && objModel.BudgetExpenditureModels.ISCircleDivision == "HQ")
                            {
                                objModel.BudgetExpenditureModels.CIRCLE_CODE = "HQ";
                                objModel.BudgetExpenditureModels.CIRCLE_NAME = "HQ";
                            }
                            tbl_Budget_Expenditure tblExpandCircle = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.BudgetExpenditureModels.FY_ID && i.SchemeID == objModel.BudgetExpenditureModels.SchemeID && i.ActivityID == objModel.BudgetExpenditureModels.ActivityID && i.SubActivityID == objModel.BudgetExpenditureModels.SubActivityID && i.BudgetHeadID == objModel.BudgetExpenditureModels.BudgetHeadID && i.SubBudgetHeadID == objModel.BudgetExpenditureModels.SubBudgetHeadID && i.CIRCLE_CODE == objModel.BudgetExpenditureModels.CIRCLE_CODE && i.ISCircleDivision == objModel.BudgetExpenditureModels.ISCircleDivision && i.ExpenditureMonths == Month && i.IsActive == true && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ"));
                            if (tblExpandCircle == null)
                            {
                                Mapper.CreateMap<View_Budget_Expenditure, tbl_Budget_Expenditure>();
                                tbl_Budget_Expenditure ObjbudgetExpand = Mapper.Map<View_Budget_Expenditure, tbl_Budget_Expenditure>(objModel.BudgetExpenditureModels);
                                ObjbudgetExpand.EnteredOn = DateTime.Now;
                                ObjbudgetExpand.EnteredBy = Convert.ToInt64(Session["UserId"]);
                                ObjbudgetExpand.IsActive = true;
                                ObjbudgetExpand.ExpenditureMonths = Month;
                                ObjbudgetExpand.ExpenditureTilldate = MonthValue;
                                this.dbContext.tbl_Budget_Expenditure.Add(ObjbudgetExpand);
                                dbContext.Entry(ObjbudgetExpand).State = System.Data.Entity.EntityState.Added;
                                dbContext.SaveChanges();
                                long Id = ObjbudgetExpand.Id;
                                objModel.BudgetExpenditureModels.rowid = Convert.ToString(Id);


                            }
                            else
                            {
                                tblExpandCircle.ExpenditureTilldate = MonthValue;
                                tblExpandCircle.IsActive = true;
                                this.dbContext.tbl_Budget_Expenditure.Add(tblExpandCircle);
                                dbContext.Entry(tblExpandCircle).State = System.Data.Entity.EntityState.Modified;
                                status = dbContext.SaveChanges();
                            }
                        }
                        #endregion
                        #region Division
                        if (objModel != null && objModel.BudgetExpenditureModels.ISCircleDivision == "Division")
                        {
                            tbl_Budget_Expenditure tblExpandDiv = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.BudgetExpenditureModels.FY_ID && i.SchemeID == objModel.BudgetExpenditureModels.SchemeID && i.ActivityID == objModel.BudgetExpenditureModels.ActivityID && i.SubActivityID == objModel.BudgetExpenditureModels.SubActivityID && i.BudgetHeadID == objModel.BudgetExpenditureModels.BudgetHeadID && i.SubBudgetHeadID == objModel.BudgetExpenditureModels.SubBudgetHeadID && i.CIRCLE_CODE == objModel.BudgetExpenditureModels.CIRCLE_CODE && i.Division == objModel.BudgetExpenditureModels.Division && i.ISCircleDivision == objModel.BudgetExpenditureModels.ISCircleDivision && i.ExpenditureMonths == Month && i.IsActive == true && i.ISCircleDivision == "Division");
                            if (tblExpandDiv == null)
                            {
                                Mapper.CreateMap<View_Budget_Expenditure, tbl_Budget_Expenditure>();
                                tbl_Budget_Expenditure ObjbudgetExpand = Mapper.Map<View_Budget_Expenditure, tbl_Budget_Expenditure>(objModel.BudgetExpenditureModels);
                                ObjbudgetExpand.EnteredOn = DateTime.Now;
                                ObjbudgetExpand.EnteredBy = Convert.ToInt64(Session["UserId"]);
                                ObjbudgetExpand.IsActive = true;
                                ObjbudgetExpand.ExpenditureMonths = Month;
                                ObjbudgetExpand.ExpenditureTilldate = MonthValue;
                                this.dbContext.tbl_Budget_Expenditure.Add(ObjbudgetExpand);
                                dbContext.Entry(ObjbudgetExpand).State = System.Data.Entity.EntityState.Added;
                                dbContext.SaveChanges();
                                long Id = ObjbudgetExpand.Id;
                                objModel.BudgetExpenditureModels.rowid = Convert.ToString(Id);

                            }

                            else
                            {
                                tblExpandDiv.ExpenditureTilldate = MonthValue;
                                this.dbContext.tbl_Budget_Expenditure.Add(tblExpandDiv);
                                dbContext.Entry(tblExpandDiv).State = System.Data.Entity.EntityState.Modified;
                                status = dbContext.SaveChanges();
                            }

                        }
                        #endregion
                        #region Sanctuary
                        if (objModel != null && objModel.BudgetExpenditureModels.ISCircleDivision == "Sanctuary")
                        {
                            tbl_Budget_Expenditure tblExpandDiv = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i => i.FY_ID == objModel.BudgetExpenditureModels.FY_ID && i.SchemeID == objModel.BudgetExpenditureModels.SchemeID && i.ActivityID == objModel.BudgetExpenditureModels.ActivityID && i.SubActivityID == objModel.BudgetExpenditureModels.SubActivityID && i.BudgetHeadID == objModel.BudgetExpenditureModels.BudgetHeadID && i.SubBudgetHeadID == objModel.BudgetExpenditureModels.SubBudgetHeadID && i.CIRCLE_CODE == objModel.BudgetExpenditureModels.CIRCLE_CODE && i.Division == objModel.BudgetExpenditureModels.Division && i.ISCircleDivision == objModel.BudgetExpenditureModels.ISCircleDivision && i.ExpenditureMonths == Month && i.IsActive == true && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.BudgetExpenditureModels.SanctuaryCode);
                            if (tblExpandDiv == null)
                            {
                                Mapper.CreateMap<View_Budget_Expenditure, tbl_Budget_Expenditure>();
                                tbl_Budget_Expenditure ObjbudgetExpand = Mapper.Map<View_Budget_Expenditure, tbl_Budget_Expenditure>(objModel.BudgetExpenditureModels);
                                ObjbudgetExpand.EnteredOn = DateTime.Now;
                                ObjbudgetExpand.EnteredBy = Convert.ToInt64(Session["UserId"]);
                                ObjbudgetExpand.IsActive = true;
                                ObjbudgetExpand.ExpenditureMonths = Month;
                                ObjbudgetExpand.ExpenditureTilldate = MonthValue;
                                this.dbContext.tbl_Budget_Expenditure.Add(ObjbudgetExpand);
                                dbContext.Entry(ObjbudgetExpand).State = System.Data.Entity.EntityState.Added;
                                dbContext.SaveChanges();
                                long Id = ObjbudgetExpand.Id;
                                objModel.BudgetExpenditureModels.rowid = Convert.ToString(Id);

                            }

                            else
                            {
                                tblExpandDiv.ExpenditureTilldate = MonthValue;
                                this.dbContext.tbl_Budget_Expenditure.Add(tblExpandDiv);
                                dbContext.Entry(tblExpandDiv).State = System.Data.Entity.EntityState.Modified;
                                status = dbContext.SaveChanges();

                            }

                        }
                        #endregion

                    }


                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("BudgetExpenditureAllocationMaster");
        }
        #endregion

        #region Filter in Budget Expentiture
        public JsonResult FilterBudgetExpenditure(View_Budget_Expenditure model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<object> lst = new List<object>();
            try
            {
                #region Maintain the Budget Allocation List
                List<View_BudgetAllocation_Circle> BudgetAllocationLists = new List<View_BudgetAllocation_Circle>();
                if (Session["BudgetAllocationListFilter"] == null)
                {
                    Session["BudgetAllocationListFilter"] = BudgetAllocationLists;
                }
                else
                {
                    BudgetAllocationLists = (List<View_BudgetAllocation_Circle>)Session["BudgetAllocationListFilter"];
                    BudgetAllocationLists = BudgetAllocationLists.Where(d => d.FY_ID == (!string.IsNullOrEmpty(model.FinancialYear) ? Convert.ToInt32(model.FinancialYear) : d.FY_ID) && d.SchemeID == (!string.IsNullOrEmpty(model.SchemeName) ? Convert.ToInt32(model.SchemeName) : d.SchemeID) && d.BudgetHeadID == (!string.IsNullOrEmpty(model.BudgetHead) ? Convert.ToInt32(model.BudgetHead) : d.BudgetHeadID) && d.SubBudgetHeadID == (!string.IsNullOrEmpty(model.SubBudgetHead) ? Convert.ToInt32(model.SubBudgetHead) : d.SubBudgetHeadID) && d.ActivityID == (!string.IsNullOrEmpty(model.ActivityName) ? Convert.ToInt32(model.ActivityName) : d.ActivityID) && d.SubActivityID == (!string.IsNullOrEmpty(model.SubActivityName) ? Convert.ToInt32(model.SubActivityName) : d.SubActivityID) && d.CIRCLE_CODE == (!string.IsNullOrEmpty(model.CIRCLE_CODE) ? Convert.ToString(model.CIRCLE_CODE) : d.CIRCLE_CODE) && d.Div_Code == (!string.IsNullOrEmpty(model.Division) ? Convert.ToString(model.Division) : d.Div_Code) && d.SanctuaryCode == (!string.IsNullOrEmpty(model.SanctuaryCode.Trim()) ? Convert.ToString(model.SanctuaryCode.Trim()) : d.SanctuaryCode)).ToList();
                }

                lst.Add(BudgetAllocationLists);
                #endregion

                #region Maintain the Budget Allocation Expenditure List
                List<View_Budget_Expenditure> BudgetExpenditureAllocationLists = new List<View_Budget_Expenditure>();
                if (Session["AddBudgetExpenditureAllocationListFilter"] == null)
                {
                    Session["AddBudgetExpenditureAllocationListFilter"] = BudgetExpenditureAllocationLists;
                }
                else
                {
                    BudgetExpenditureAllocationLists = (List<View_Budget_Expenditure>)Session["AddBudgetExpenditureAllocationListFilter"];
                    BudgetExpenditureAllocationLists = BudgetExpenditureAllocationLists.Where(d => d.FY_ID == (!string.IsNullOrEmpty(model.FinancialYear) ? Convert.ToInt32(model.FinancialYear) : d.FY_ID) && d.SchemeID == (!string.IsNullOrEmpty(model.SchemeName) ? Convert.ToInt32(model.SchemeName) : d.SchemeID) && d.BudgetHeadID == (!string.IsNullOrEmpty(model.BudgetHead) ? Convert.ToInt32(model.BudgetHead) : d.BudgetHeadID) && d.SubBudgetHeadID == (!string.IsNullOrEmpty(model.SubBudgetHead) ? Convert.ToInt32(model.SubBudgetHead) : d.SubBudgetHeadID) && d.ActivityID == (!string.IsNullOrEmpty(model.ActivityName) ? Convert.ToInt32(model.ActivityName) : d.ActivityID) && d.SubActivityID == (!string.IsNullOrEmpty(model.SubActivityName) ? Convert.ToInt32(model.SubActivityName) : d.SubActivityID) && d.CIRCLE_CODE == (!string.IsNullOrEmpty(model.CIRCLE_CODE) ? Convert.ToString(model.CIRCLE_CODE) : d.CIRCLE_CODE) && d.Division == (!string.IsNullOrEmpty(model.Division) ? Convert.ToString(model.Division) : d.Division) && d.SanctuaryCode == (!string.IsNullOrEmpty(model.SanctuaryCode.Trim()) ? Convert.ToString(model.SanctuaryCode.Trim()) : d.SanctuaryCode)).ToList();
                }
                lst.Add(BudgetExpenditureAllocationLists);
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get GIS Information

        public ActionResult GetBudgetExpenditureDetailsForGIS(string ID)
        {
            List<View_BudgetAllocation_Circle> BudgetAllocationList = new List<View_BudgetAllocation_Circle>();
            BudgetAllocationGISInformation model = new BudgetAllocationGISInformation();
            Session["AddBudgetExpenditureAllocationList"] = new List<View_Budget_Expenditure>();
            TempData["ID"] = null;
            TempData["ID"] = ID;
            model.ID = Convert.ToInt64(ID);
            try
            {

                #region Check GIS Information not Null
                if (Session["BudgetExenditureGISinformation"] != null)
                {
                    model = (BudgetAllocationGISInformation)Session["BudgetExenditureGISinformation"];
                }
                #endregion

                BudgetAllocationList = (List<View_BudgetAllocation_Circle>)Session["BudgetAllocationList"];
                if (BudgetAllocationList.Count > 0)
                {
                    model.BudgetAllocationDetails = BudgetAllocationList.FirstOrDefault(s => s.ID == Convert.ToInt64(ID));

                }

            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetBudgetExpenditureDetailsForGIS(BudgetAllocationGISInformation model)
        {
            try
            {
                long ID = model.ID;
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                #region Check GIS Information not Null
                if (Session["BudgetExenditureGISinformation"] != null)
                {
                    model = (BudgetAllocationGISInformation)Session["BudgetExenditureGISinformation"];
                }
                #endregion
                string JSONString = JsonConvert.SerializeObject(model.GISInformationList);
                DataTable GISTable = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));

                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                DataTable dt = repo.InsertGisInformationBudgetExpenditure("Insert", ID.ToString(), Convert.ToInt64(Session["UserId"]), GISTable);
                if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["Status"]) == 1)
                {
                    TempData["Message"] = "<div>" + Convert.ToString(dt.Rows[0]["message"]) + " </div>";

                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later " + Convert.ToString(dt.Rows[0]["message"]) + ")</div>";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            Session["BudgetExenditureGISinformation"] = null;
            return RedirectToAction("BudgetExpenditureAllocation");
        }

        public ActionResult GISDataBudgetModule(FormCollection form)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            BudgetAllocationGISInformation model = new BudgetAllocationGISInformation();
            Session["BudgetExenditureGISinformation"] = null;
            string BudgetExpenditureID = "0";
            string aid = string.Empty;
            try
            {
                if (TempData["ID"] != null)
                {
                    BudgetExpenditureID = Convert.ToString(TempData["ID"]);
                }

                BindMasterData _ObjMaster = new BindMasterData();
                if (Convert.ToString(Session["PermissionTypeID"]) != "") { aid = Convert.ToString(Session["PermissionTypeID"]); }
                if (Convert.ToString(form["successFlag"]).ToLower() == "true")
                {


                    List<clsPermission> myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));
                    model.GISID = form["gisid"].ToString();
                    model.AreaShape = Convert.ToDecimal(form["shapeArea"].ToString());
                    #region "Muliple List"
                    string LAT = string.Empty, Long = string.Empty;
                    foreach (var dr in myDeserializedObj)
                    {
                        #region "KML and Lat-Long"
                        if (form["locCentroid"].ToString() != "")
                        {
                            if (form["locCentroid"].ToString().Contains(","))
                            {
                                string[] locCentroid = form["locCentroid"].ToString().Split(',');
                                LAT = locCentroid[1] == "NA" ? "" : locCentroid[1];
                                Long = locCentroid[0] == "NA" ? "" : locCentroid[0];
                            }
                        }

                        #endregion

                        clsPermission cls = new clsPermission();
                        cls.Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        cls.Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        cls.Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        cls.Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        cls.Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        cls.Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM;
                        cls.Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM;
                        cls.Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM;
                        cls.Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM;
                        cls.Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM;
                        cls.areaName = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        cls.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE;
                        cls.Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd;
                        cls.Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM;
                        model.GISInformationName.Add(cls);

                        GISDataBaseModel gisModel = new GISDataBaseModel();
                        gisModel.DIV_CODE = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        gisModel.DIST_CODE = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        gisModel.BLK_CODE = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        gisModel.GP_CODE = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        gisModel.VILL_CODE = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        gisModel.Area = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        gisModel.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : string.IsNullOrEmpty(dr.FOREST_DIVCODE) ? "N/A" : dr.FOREST_DIVCODE;
                        gisModel.GPSLat = LAT;
                        gisModel.GPSLong = Long;
                        gisModel.GISFilePath = Convert.ToString(form["filePath"]);
                        gisModel.GISOrignalFilePath = Convert.ToString(form["originalFileName"]);
                        gisModel.GISID = model.GISID;
                        gisModel.AreaShapeInHactor = model.AreaShape;

                        model.Latitude = LAT;
                        model.Longitude = Long;
                        model.GISInformationList.Add(gisModel);

                    }

                    #endregion

                    Session["BudgetExenditureGISinformation"] = model;

                }
                else
                {
                    Session["BudgetExenditureGISinformation"] = null;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, 0);
            }
            return RedirectToAction("GetBudgetExpenditureDetailsForGIS", "BudgetExpenditure", new { ID = BudgetExpenditureID });
        }



        #endregion

        #region Expenditure Report
        public ActionResult ExpenditureReport()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            BudgetExpenditureModel model = new BudgetExpenditureModel();
            Session["ExpenditureReports"] = null;
            Session["BudgetListFilter"] = null;
            try
            {
               
                ViewBag.DivisionList = new List<SelectListItem>();
                ViewBag.SantauryList = new List<SelectListItem>();
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();

                #region Fill the month
                string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                List<SelectListItem> lstMonths = new List<SelectListItem>();
                foreach (string item in monthNames) // writing out
                {
                    if (Convert.ToString(item) != string.Empty)
                        lstMonths.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                }
                ViewBag.Months = lstMonths;
                #endregion

                #region Budget Expenditure List
                model.BudgetExpenditureList = new View_Budget_Expenditure().GetExpenditureList(Convert.ToInt32(Session["UserID"]));
                Session["ExpenditureReports"] = model.BudgetExpenditureList;
                #endregion

                #region Budget Allocated List
                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                model.BudgetAllocationList = repo.GetBudgetAllocationCircleList(Convert.ToInt32(Session["UserID"]));
                Session["BudgetListFilter"] = model.BudgetAllocationList;
                #endregion

                #region Fill Dropdown Accoding to the Budget Allocation
                FillDropDownMasterWithUserID(model.BudgetAllocationList);
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ExpenditureReport(BudgetExpenditureModel model)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            BudgetExpenditureModel modeldata = new BudgetExpenditureModel();
            try
            {
                long status = 0;
                try
                {
                    List<View_BudgetAllocation_Circle> BudgetList = new List<View_BudgetAllocation_Circle>();
                    FillDropDownMasterWithUserID(BudgetList);
                    ViewBag.DivisionList = new List<SelectListItem>();
                    ViewBag.SantauryList = new List<SelectListItem>();
                    ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                    ViewBag.SubActivityList = new List<SelectListItem>();
                    #region Fill the month
                    string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                    List<SelectListItem> lstMonths = new List<SelectListItem>();
                    foreach (string item in monthNames) // writing out
                    {
                        if (Convert.ToString(item) != string.Empty)
                            lstMonths.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                    }
                    ViewBag.Months = lstMonths;
                    #endregion

                    #region Maintain the Budget Allocation Expenditure List
                    List<View_Budget_Expenditure> BudgetExpenditureAllocationLists = new List<View_Budget_Expenditure>();
                    if (Session["ExpenditureReports"] == null)
                    {
                        Session["ExpenditureReports"] = BudgetExpenditureAllocationLists;
                    }
                    else
                    {
                        string san = model.BudgetExpenditureModels.SanctuaryCode;
                        BudgetExpenditureAllocationLists = (List<View_Budget_Expenditure>)Session["ExpenditureReports"];
                        BudgetExpenditureAllocationLists = BudgetExpenditureAllocationLists.Where(d => d.FY_ID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.FinancialYear) ? Convert.ToInt32(model.BudgetExpenditureModels.FinancialYear) : d.FY_ID) && d.SchemeID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SchemeName) ? Convert.ToInt32(model.BudgetExpenditureModels.SchemeName) : d.SchemeID) && d.BudgetHeadID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.BudgetHead) ? Convert.ToInt32(model.BudgetExpenditureModels.BudgetHead) : d.BudgetHeadID) && d.SubBudgetHeadID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SubBudgetHead) ? Convert.ToInt32(model.BudgetExpenditureModels.SubBudgetHead) : d.SubBudgetHeadID) && d.ActivityID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.ActivityName) ? Convert.ToInt32(model.BudgetExpenditureModels.ActivityName) : d.ActivityID) && d.SubActivityID == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SubActivityName) ? Convert.ToInt32(model.BudgetExpenditureModels.SubActivityName) : d.SubActivityID) && d.CIRCLE_CODE == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.CIRCLE_CODE) ? Convert.ToString(model.BudgetExpenditureModels.CIRCLE_CODE) : d.CIRCLE_CODE) && d.Division == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.Division) ? Convert.ToString(model.BudgetExpenditureModels.Division) : d.Division) && d.SanctuaryCode == (!string.IsNullOrEmpty(model.BudgetExpenditureModels.SanctuaryCode) ? Convert.ToString(model.BudgetExpenditureModels.SanctuaryCode.Trim()) : d.SanctuaryCode)).ToList();
                    }
                    modeldata.BudgetExpenditureList = BudgetExpenditureAllocationLists;
                    #endregion

                    #region Fill Division and Santaury And
                    //#region Fill Division
                    //List<SelectListItem> lstDivision = new List<SelectListItem>();

                    //try
                    //{

                    //    var result = dbContext.tbl_mst_Forest_Divisions.Where(i => i.CIRCLE_CODE == model.BudgetExpenditureModels.CIRCLE_CODE && i.ForBudgetModuleDist == 1).Select(i => new { i.DIV_CODE, i.DIV_NAME }).OrderBy(s => s.DIV_NAME);
                    //    foreach (var item in result)
                    //    {
                    //        lstDivision.Add(new SelectListItem { Text = item.DIV_NAME, Value = item.DIV_CODE });
                    //    }
                    //    ViewBag.DivisionList = lstDivision;
                    //}
                    //catch (Exception ex)
                    //{
                    //    ViewBag.DivisionList = new List<SelectListItem>();

                    //}
                    //#endregion

                    //#region Fill Santaury
                    //try
                    //{
                    //    List<SelectListItem> SanctuaryList = new List<SelectListItem>();

                    //    var param1 = new SqlParameter("@Action", "Detail");
                    //    var param2 = new SqlParameter("@DIV_CODE", model.BudgetExpenditureModels.Division);
                    //    ViewBag.SantauryList = dbContext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();

                    //}
                    //catch (Exception ex)
                    //{
                    //    ViewBag.SantauryList = new List<SelectListItem>();
                    //}
                    //#endregion

                    //#region Fill SubBudgethead
                    //long BudgetHeadID = Convert.ToInt64(model.BudgetExpenditureModels.BudgetHead);
                    //ViewBag.SubBudgetHeadList = dbContext.tbl_mst_SubBudgetHead.Where(s => s.BudgetHeadID == BudgetHeadID).OrderBy(d => d.SubBudgetHead).ToList().Select(d => new SelectListItem() { Text = d.SubBudgetHead, Value = d.ID.ToString() }).ToList();
                    //#endregion

                    //#region Fill SubActivityhead
                    //int ActivityID = !string.IsNullOrEmpty(model.BudgetExpenditureModels.ActivityName) ? Convert.ToInt32(model.BudgetExpenditureModels.ActivityName) : 0;
                    //ViewBag.SubActivityList = dbContext.tbl_mst_SUBActivityForWidelife.Where(i => i.IsActive == 1 && i.ActivityID == ActivityID).OrderBy(d => d.SUBActivity_Name).ToList().Select(item => new SelectListItem { Text = item.SUBActivity_Name, Value = Convert.ToString(item.ID) }).ToList();
                    //#endregion
                    #endregion

                    #region Budget Allocated List
                    model.BudgetAllocationList = (List<View_BudgetAllocation_Circle>)Session["BudgetListFilter"];
                    #endregion

                    #region Fill Dropdown Accoding to the Budget Allocation
                    FillDropDownMasterWithUserID(model.BudgetAllocationList);
                    #endregion

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(modeldata);
        }



        #endregion

        #region Get Scheme Budget Circle division all master according the record Developed by Rajveer
        public void FillDropDownMasterWithUserID(List<View_BudgetAllocation_Circle> BudgetList)
        {
            List<SelectListItem> FinanceYear = new List<SelectListItem>();
            List<SelectListItem> SchemeList = new List<SelectListItem>();
            List<SelectListItem> BudgetHead = new List<SelectListItem>();
            List<SelectListItem> Activity = new List<SelectListItem>();
            List<SelectListItem> Circle = new List<SelectListItem>();
            Session["BudgetListFilter"] = null;
            try
            {
                Session["BudgetListFilter"] = BudgetList;

                #region Get Circle and Division with userID
                DataSet MasterCircleDivision = new DataSet();
                MasterCircleDivisionWithUserID repomaster = new MasterCircleDivisionWithUserID();
                MasterCircleDivision = repomaster.Select_CircleDivisionWithUserID("ALL", Convert.ToInt64(Session["UserID"]));
                ViewBag.Circle = new SelectList(MasterCircleDivision.Tables[0].AsDataView(), "Value", "Text");
                ViewBag.DivisionLists = new SelectList(MasterCircleDivision.Tables[1].AsDataView(), "Value", "Text");
                #endregion

                #region FinanceYear Bind
                var financialYear = dbContext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).Distinct().ToList();
                foreach (var item in financialYear)
                {
                    FinanceYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }
                ViewBag.Financial = FinanceYear;
                #endregion

                #region Scheme Bind


                List<long> SchemeIds = BudgetList.Select(s => s.SchemeID).Distinct().ToList();
                SchemeList = dbContext.tbl_FDM_SchemeForWidelife.Where(s => SchemeIds.Contains(s.ID)).OrderBy(d => d.Scheme_Name).ToList().Select(item => new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) }).ToList();
                ViewBag.SchemeList = SchemeList;
                #endregion

                #region Activity Bind
                List<long> ActivityIds = BudgetList.Select(s => s.ActivityID).Distinct().ToList();
                Activity = dbContext.tbl_mst_ActivityForWidelife.Where(s => ActivityIds.Contains(s.ID)).OrderBy(d => d.Activity_Name).ToList().Select(item => new SelectListItem { Text = item.Activity_Name, Value = Convert.ToString(item.ID) }).ToList();
                ViewBag.Activity = Activity;
                #endregion

                #region Budget Head Bind
                List<long> BudgetHeadIds = BudgetList.Select(s => s.BudgetHeadID).Distinct().ToList();
                BudgetHead = dbContext.tbl_mst_BudgetHead.Where(s => BudgetHeadIds.Contains(s.ID)).OrderBy(d => d.BudgetHead).ToList().Select(item => new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) }).ToList();
                ViewBag.BudgetHead = BudgetHead;
                #endregion

            }
            catch (Exception ex)
            {
                ViewBag.Financial = new List<SelectListItem>();
                ViewBag.Circle = new List<SelectListItem>();
                ViewBag.Activity = new List<SelectListItem>();
                ViewBag.BudgetHead = new List<SelectListItem>();
            }
        }


        public JsonResult GetSubActivityAccordingAllocation(long ActivityID)
        {
            try
            {
                #region Contain Budget List According User ID
                List<View_BudgetAllocation_Circle> BudgetList=(List<View_BudgetAllocation_Circle>)Session["BudgetListFilter"];
                #endregion

                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                List<long> SubActivityIds = BudgetList.Where(s=>s.ActivityID==ActivityID).Select(s => s.SubActivityID).Distinct().ToList();
                lstSubActivity = dbContext.tbl_mst_SUBActivityForWidelife.Where(s => SubActivityIds.Contains(s.ID)).OrderBy(d => d.SUBActivity_Name).ToList().Select(a => new SelectListItem { Text = a.SUBActivity_Name, Value = Convert.ToString(a.ID) }).ToList();

                return Json(lstSubActivity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        public JsonResult GetBudgetSubHeadAccordingAllocation(long budgetHead)
        {
            try
            {

                #region Contain Budget List According User ID
                List<View_BudgetAllocation_Circle> BudgetList = (List<View_BudgetAllocation_Circle>)Session["BudgetListFilter"];
                #endregion

                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<long> SubBudgetHeadIds = BudgetList.Where(s=>s.BudgetHeadID == budgetHead).Select(s => s.SubBudgetHeadID).Distinct().ToList();
                lstBudgetHead = dbContext.tbl_mst_SubBudgetHead.Where(s => SubBudgetHeadIds.Contains(s.ID)).OrderBy(d => d.SubBudgetHead).ToList().Select(a => new SelectListItem { Text = a.SubBudgetHead, Value = Convert.ToString(a.ID) }).ToList();
                lstBudgetHead.Add(new SelectListItem { Text = "none", Value = "0" });
               
                return Json(lstBudgetHead, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        #endregion
    }
}
