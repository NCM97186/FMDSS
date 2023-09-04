using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Repository;
using System.Data.SqlClient;
using System.Data.Entity;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System.Data;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class BudgetMnthProgReportController : Controller
    {


        FmdssContext dbContext;
        public BudgetMnthProgReportController()
        {
            dbContext = new FmdssContext();
        }
        public ActionResult BudgetMnthProgReport()
        {
            FillDropdown();
            return View();
        }
        public void FillDropdown()
        {
            try
            {
                List<SelectListItem> lstFinancial = new List<SelectListItem>();
                List<SelectListItem> lstCircle = new List<SelectListItem>();

                var circle = dbContext.tbl_mst_Forest_WildLifeCircles.Where(e => e.ISWILDLIFECIRCLE == true).Select(i => new { i.CIRCLE_CODE, i.CIRCLE_NAME }).Distinct().ToList();
                foreach (var item in circle)
                {
                    lstCircle.Add(new SelectListItem { Text = item.CIRCLE_NAME, Value = item.CIRCLE_CODE });
                }

                var financialYear = dbContext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).Distinct().ToList();
                foreach (var item in financialYear)
                {
                    lstFinancial.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }

                ViewBag.Financial = lstFinancial;
                ViewBag.Circle = lstCircle;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult GetBudgetActivity(View_BudgetMnthProgReport viewObj)
        {
            try
            {

                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                List<SelectListItem> lstBudget = new List<SelectListItem>();
                List<SelectListItem> lstActivity = new List<SelectListItem>();
                var budget = (from a in dbContext.tbl_Budget_Expenditure
                              join b in dbContext.tbl_mst_BudgetHead on a.BudgetHeadID equals b.ID
                              select new
                              {
                                  a.BudgetHeadID,
                                  b.BudgetHead,
                                  a.FY_ID,
                                  a.IsActive
                              }).Where(a => a.FY_ID == viewObj.FinancialYearId && a.IsActive == true).Distinct();

                foreach (var item in budget)
                {
                    lstBudget.Add(new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.BudgetHeadID) });
                }

                var activity = (from a in dbContext.tbl_Budget_Expenditure
                                join b in dbContext.tbl_mst_ActivityForWidelife on a.ActivityID equals b.ID
                                select new
                                {
                                    a.ActivityID,
                                    b.Activity_Name,
                                    a.FY_ID,
                                    a.IsActive
                                }).Where(a => a.FY_ID == viewObj.FinancialYearId && a.IsActive == true).Distinct(); ;
                foreach (var item in activity)
                {
                    lstActivity.Add(new SelectListItem { Text = item.Activity_Name, Value = Convert.ToString(item.ActivityID) });
                }

                lstDetails = new View_BudgetMnthProgReport().GetMontlyProgressReport(viewObj);

                return Json(new { list1 = lstBudget, list2 = lstActivity, list3 = lstDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetSubBudgetHead(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                List<SelectListItem> lstSubBudgetHead = new List<SelectListItem>();

                var subBudgetHead = (from a in dbContext.tbl_Budget_Expenditure
                                     join b in dbContext.tbl_mst_SubBudgetHead on a.SubBudgetHeadID equals b.ID
                                     select new
                                     {
                                         a.BudgetHeadID,
                                         a.SubBudgetHeadID,
                                         b.SubBudgetHead,
                                         a.FY_ID,
                                         a.IsActive
                                     }).Where(a => a.FY_ID == obj.FinancialYearId && a.BudgetHeadID == obj.BudgetHeadId && a.IsActive == true).Distinct();

                foreach (var item in subBudgetHead)
                {
                    lstSubBudgetHead.Add(new SelectListItem { Text = item.SubBudgetHead, Value = Convert.ToString(item.SubBudgetHeadID) });
                }

                lstDetails = new View_BudgetMnthProgReport().GetMontlyProgressReport(obj);

                return Json(new { list1 = lstSubBudgetHead, list2 = lstDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }


        public JsonResult GetSubBudgetHeadReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                lstDetails = new View_BudgetMnthProgReport().GetMontlyProgressReport(obj);
                return Json(new { list1 = lstDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetSubActivityReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                lstDetails = new View_BudgetMnthProgReport().GetMontlyProgressReport(obj);

                var subActivity = (from a in dbContext.tbl_Budget_Expenditure
                                   join b in dbContext.tbl_mst_SUBActivityForWidelife on a.SubActivityID equals b.ID
                                   select new
                                   {
                                       a.BudgetHeadID,
                                       a.SubBudgetHeadID,
                                       a.ActivityID,
                                       a.SubActivityID,
                                       b.SUBActivity_Name,
                                       a.FY_ID,
                                       a.IsActive
                                   }).Where(a => a.FY_ID == obj.FinancialYearId && a.BudgetHeadID == obj.BudgetHeadId && a.SubBudgetHeadID == obj.SubBudgetHeadId && a.ActivityID == obj.ActivityID && a.IsActive == true).Distinct();

                foreach (var item in subActivity)
                {
                    lstSubActivity.Add(new SelectListItem { Text = item.SUBActivity_Name, Value = Convert.ToString(item.SubActivityID) });
                }
                return Json(new { list1 = lstSubActivity, list2 = lstDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetSubActivityWiseReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                lstDetails = new View_BudgetMnthProgReport().GetMontlyProgressReport(obj);
                return Json(new { list1 = lstDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetCircleWiseReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                List<SelectListItem> lstDivision = new List<SelectListItem>();
                lstDetails = new View_BudgetMnthProgReport().GetMontlyProgressReport(obj);


                var subActivity = (from a in dbContext.tbl_Budget_Expenditure
                                   join b in dbContext.tbl_mst_Forest_Divisions on a.Division equals b.DIV_CODE
                                   select new
                                   {
                                       a.BudgetHeadID,
                                       a.SubBudgetHeadID,
                                       a.ActivityID,
                                       a.SubActivityID,
                                       a.Division,
                                       b.DIV_NAME,
                                       a.FY_ID,
                                       a.IsActive
                                   }).Where(a => a.FY_ID == obj.FinancialYearId && a.BudgetHeadID == obj.BudgetHeadId && a.SubBudgetHeadID == obj.SubBudgetHeadId && a.ActivityID == obj.ActivityID && a.IsActive == true).Distinct();

                foreach (var item in subActivity)
                {
                    lstDivision.Add(new SelectListItem { Text = item.DIV_NAME, Value = item.Division });
                }

                return Json(new { list1 = lstDivision, list2 = lstDetails }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetDivisionWiseReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                lstDetails = new View_BudgetMnthProgReport().GetMontlyProgressReport(obj);
                return Json(new { list1 = lstDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetDateWiseReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                lstDetails = new View_BudgetMnthProgReport().GetMontlyProgressReport(obj);
                return Json(new { list1 = lstDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetMonthlyReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<MonthlyProgressReport> lstDetails = new List<MonthlyProgressReport>();
                lstDetails = new MonthlyProgressReport().GetMontlyProgressReport(obj);
                return Json(new { list1 = lstDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        #region Budget Report Developed by Rajveer

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
                var circle = dbContext.tbl_mst_Forest_WildLifeCircles.Where(e => e.isBOTH == 1).Select(i => new { i.CIRCLE_CODE, i.CIRCLE_NAME }).Distinct().OrderBy(d=>d.CIRCLE_NAME).ToList();
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
                var SchemeLists = dbContext.tbl_FDM_SchemeForWidelife.OrderBy(s => s.Scheme_Name).Select(i => new { i.Scheme_Name, i.ID }).Distinct().ToList().OrderBy(s => s.Scheme_Name);
                foreach (var item in SchemeLists)
                {
                    SchemeList.Add(new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) });
                }
                ViewBag.SchemeList = SchemeList;
                #endregion

                #region Activity Bind
                var ActivityList = dbContext.tbl_mst_ActivityForWidelife.Select(i => new { i.Activity_Name, i.ID }).Distinct().ToList().OrderBy(s => s.Activity_Name);
                foreach (var item in ActivityList)
                {
                    Activity.Add(new SelectListItem { Text = item.Activity_Name, Value = Convert.ToString(item.ID) });
                }
                ViewBag.Activity = Activity;
                #endregion

                #region Budget Head Bind
                var BudgetHeadList = dbContext.tbl_mst_BudgetHead.Select(i => new { i.BudgetHead, i.ID }).Distinct().ToList().OrderBy(s => s.BudgetHead);
                foreach (var item in BudgetHeadList)
                {
                    BudgetHead.Add(new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) });
                }
                ViewBag.BudgetHead = BudgetHead;
                #endregion

                #region Santuary
                var param1 = new SqlParameter("@Action", "ALL");
                var param2 = new SqlParameter("@DIV_CODE", "");
                ViewBag.SantauryList = dbContext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();
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

        public ActionResult BudgetReport()
        {
            MonthlyProgressReportExpenditureModel model = new MonthlyProgressReportExpenditureModel();
            MonthlyProgressReportExpenditure repo = new MonthlyProgressReportExpenditure();
            try
            {
                model.List = repo.GetMontlyProgressReport(model.model, "ALL");
                TempData["BudgetReportList"] = model.List;
                FillDropDownMaster();
            }
            catch (Exception ex)
            {
                model.List = new List<MonthlyProgressReportModel>();
            }
            return View(model.model);
        }

        [HttpPost]
        public ActionResult BudgetReport(MonthlyProgressReportModel model)
        {
            MonthlyProgressReportExpenditure repo = new MonthlyProgressReportExpenditure();
            try
            {
                if (Session["BudgetReportList"] == null)
                {
                    TempData["BudgetReportList"] = repo.GetMontlyProgressReport(model, "ALL");
                }
                else
                {
                    TempData["BudgetReportList"] = new List<MonthlyProgressReportModel>();
                }

                FillDropDownMaster();

            }
            catch (Exception ex)
            {

            }
            return View(model);
        }


        public JsonResult GetMonthStatusReport(MonthlyProgressReportModel obj)
        {
            try
            {
                MonthlyProgressReportExpenditure repo = new MonthlyProgressReportExpenditure();
                List<MonthlyProgressReportModel> lstDetails = new List<MonthlyProgressReportModel>();
                lstDetails = repo.GetMontlyProgressReport(obj, "MONTHLY");
                return Json(new { list1 = lstDetails.ToList().Select(d => d.MonthlyReport) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        #endregion


        #region Budget Report Some Required Fields
        public ActionResult BudgetReportMonth()
        {
            MonthlyProgressReportExpenditureModel model = new MonthlyProgressReportExpenditureModel();
            MonthlyProgressReportExpenditure repo = new MonthlyProgressReportExpenditure();
            try
            {
                model.List = repo.GetMontlyProgressReport(model.model, "ALL");
                TempData["BudgetReportList"] = model.List;
                FillDropDownMaster();
                ViewBag.DivisionList = new List<SelectListItem>();
                #region Fill Sub Budget Head and Sub Activity
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();
                #endregion
            }
            catch (Exception ex)
            {
                model.List = new List<MonthlyProgressReportModel>();
            }
            return View(model.model);
        }

        [HttpPost]
        public ActionResult BudgetReportMonth(MonthlyProgressReportModel model, string Circle_Names = "", string Division_Names = "", string SantuaryNames = "")
        {
            MonthlyProgressReportExpenditure repo = new MonthlyProgressReportExpenditure();
            try
            {
                #region Fill If Export the Excel

                TempData["Circle_Names"] = !string.IsNullOrEmpty(Circle_Names) ? Circle_Names : "N/A";
                TempData["Division_Names"] = !string.IsNullOrEmpty(Division_Names) ? Division_Names : "N/A";
                TempData["SantuaryNames"] = !string.IsNullOrEmpty(SantuaryNames) ? SantuaryNames : "N/A";
                TempData["Circle_Namess"] = !string.IsNullOrEmpty(Circle_Names) ? Circle_Names : "N/A";

                #region Fill Division and Santaury And 
                #region Fill Division
                List<SelectListItem> lstDivision = new List<SelectListItem>();

                try
                {

                    var result = dbContext.tbl_mst_Forest_Divisions.Where(i => i.CIRCLE_CODE == model.Circle_Code && i.ForBudgetModuleDist == 1).OrderByDescending(s => s.DIV_NAME).Select(i => new { i.DIV_CODE, i.DIV_NAME });
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
                    var param2 = new SqlParameter("@DIV_CODE", model.Division_Code??string.Empty);
                    ViewBag.SantauryList = dbContext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();

                }
                catch (Exception ex)
                {
                    ViewBag.SantauryList = new List<SelectListItem>();
                }
                #endregion
                #endregion
                #endregion

                if (Session["BudgetReportList"] == null)
                {
                    TempData["BudgetReportList"] = repo.GetMontlyProgressReport(model, "ALL");
                }
                else
                {
                    TempData["BudgetReportList"] = new List<MonthlyProgressReportModel>();
                }

                FillDropDownMaster();

                #region Fill Sub Budget Head and Sub Activity
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();

                if(!string.IsNullOrEmpty(model.SubBudgetHead) && !string.IsNullOrEmpty(model.BudgetHead))
                {
                    int BudgetHead = Convert.ToInt32(model.BudgetHead);
                    ViewBag.SubBudgetHeadList = dbContext.tbl_mst_SubBudgetHead.Where(i => i.BudgetHeadID == BudgetHead).ToList().Select(i => new SelectListItem { Text = i.SubBudgetHead, Value = i.ID.ToString() }).OrderBy(s => s.Text);
                }
                if (!string.IsNullOrEmpty(model.SubActivity_Name) && !string.IsNullOrEmpty(model.Activity_Name))
                {
                    int Activity_Name = Convert.ToInt32(model.Activity_Name);
                    ViewBag.SubActivityList = dbContext.tbl_mst_SUBActivityForWidelife.Where(i => i.IsActive == 1 && i.ActivityID == Activity_Name).ToList().Select(i => new SelectListItem {Text= i.SUBActivity_Name,Value= i.ID.ToString() }).OrderBy(s => s.Text);
                }

                #endregion
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        #endregion


        #region Analisyt Report
        public ActionResult BudgetAnalystReport()
        {
            MonthlyProgressReportExpenditureModel model = new MonthlyProgressReportExpenditureModel();
            MonthlyProgressAnalystReport obj = new MonthlyProgressAnalystReport();
            try
            {
                TempData["BudgetAnalystReport"] = obj.GetMontlyProgressAnalystReport(model.model, "ALL");
                FillDropDownMaster();
                ViewBag.DivisionList = new List<SelectListItem>();
                List<SelectListItem> Provinces = new List<SelectListItem>();
                Provinces.Add(new SelectListItem() { Text = "Circle Name", Value = "C.CIRCLE_NAME" });
                Provinces.Add(new SelectListItem() { Text = "Division Name", Value = "Div.DIV_NAME" });
                Provinces.Add(new SelectListItem() { Text = "Sanctuary Name", Value = "SanctuaryData.Place_Name" });
                Provinces.Add(new SelectListItem() { Text = "Scheme Name", Value = "Sch.Scheme_Name" });
                Provinces.Add(new SelectListItem() { Text = "Budget Head", Value = "B.BudgetHead" });
                Provinces.Add(new SelectListItem() { Text = "Sub Budget Head", Value = "S.SubBudgetHead" });
                Provinces.Add(new SelectListItem() { Text = "Recurring Or Non Recurring", Value = "Cir.IsRecurring" });
                Provinces.Add(new SelectListItem() { Text = "Activity Name", Value = "Act.Activity_Name" });
                Provinces.Add(new SelectListItem() { Text = "Sub-Activity Name", Value = "SAct.SUBActivity_Name" });
                Provinces.Add(new SelectListItem() { Text = "Site Name", Value = "Cir.SiteName" });
                Provinces.Add(new SelectListItem() { Text = "IsCoreOrBuffer", Value = "Cir.IsCoreOrBuffer" });



                ViewBag.mySkills = Provinces;

                #region Fill Sub Budget Head and Sub Activity
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();
                #endregion

            }
            catch (Exception ex)
            {
                model.List = new List<MonthlyProgressReportModel>();
            }
            return View(model.model);
        }


        [HttpPost]
        public ActionResult BudgetAnalystReport(MonthlyProgressReportModel model, string Circle_Names = "", string Division_Names = "", string SantuaryNames = "")
        {
            MonthlyProgressAnalystReport obj = new MonthlyProgressAnalystReport();
            try
            {
                #region Fill If Export the Excel

                TempData["Circle_Names"] = !string.IsNullOrEmpty(Circle_Names) ? Circle_Names : "N/A";
                TempData["Division_Names"] = !string.IsNullOrEmpty(Division_Names) ? Division_Names : "N/A";
                TempData["SantuaryNames"] = !string.IsNullOrEmpty(SantuaryNames) ? SantuaryNames : "N/A";
                TempData["Circle_Namess"] = !string.IsNullOrEmpty(Circle_Names) ? Circle_Names : "N/A";

                #region Fill Division and Santaury And
                #region Fill Division
                List<SelectListItem> lstDivision = new List<SelectListItem>();

                try
                {

                    var result = dbContext.tbl_mst_Forest_Divisions.Where(i => i.CIRCLE_CODE == model.Circle_Code && i.ForBudgetModuleDist == 1).OrderByDescending(s => s.DIV_NAME).Select(i => new { i.DIV_CODE, i.DIV_NAME });
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

                List<SelectListItem> Provinces = new List<SelectListItem>();
                Provinces.Add(new SelectListItem() { Text = "Circle Name", Value = "C.CIRCLE_NAME" });
                Provinces.Add(new SelectListItem() { Text = "Division Name", Value = "Div.DIV_NAME" });
                Provinces.Add(new SelectListItem() { Text = "Sanctuary Name", Value = "SanctuaryData.Place_Name" });
                Provinces.Add(new SelectListItem() { Text = "Scheme Name", Value = "Sch.Scheme_Name" });
                Provinces.Add(new SelectListItem() { Text = "Budget Head", Value = "B.BudgetHead" });
                Provinces.Add(new SelectListItem() { Text = "Sub Budget Head", Value = "S.SubBudgetHead" });
                Provinces.Add(new SelectListItem() { Text = "Recurring Or Non Recurring", Value = "Cir.IsRecurring" });
                Provinces.Add(new SelectListItem() { Text = "Activity Name", Value = "Act.Activity_Name" });
                Provinces.Add(new SelectListItem() { Text = "Sub-Activity Name", Value = "SAct.SUBActivity_Name" });
                Provinces.Add(new SelectListItem() { Text = "Site Name", Value = "Cir.SiteName" });
                 Provinces.Add(new SelectListItem() { Text = "IsCoreOrBuffer", Value = "Cir.IsCoreOrBuffer" });
				ViewBag.mySkills = Provinces;

                #region Fill Santaury
                try
                {
                    List<SelectListItem> SanctuaryList = new List<SelectListItem>();

                    var param1 = new SqlParameter("@Action", "Detail");
                    var param2 = new SqlParameter("@DIV_CODE", model.Division_Code);
                    ViewBag.SantauryList = dbContext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();

                }
                catch (Exception ex)
                {
                    ViewBag.SantauryList = new List<SelectListItem>();
                }
                #endregion
                #endregion
                #endregion
                #region Fill Sub Budget Head and Sub Activity
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();

                if (!string.IsNullOrEmpty(model.SubBudgetHead) && !string.IsNullOrEmpty(model.BudgetHead))
                {
                    int BudgetHead = Convert.ToInt32(model.BudgetHead);
                    ViewBag.SubBudgetHeadList = dbContext.tbl_mst_SubBudgetHead.Where(i => i.BudgetHeadID == BudgetHead).ToList().Select(i => new SelectListItem { Text = i.SubBudgetHead, Value = i.ID.ToString() }).OrderBy(s => s.Text);
                }
                if (!string.IsNullOrEmpty(model.SubActivity_Name) && !string.IsNullOrEmpty(model.Activity_Name))
                {
                    int Activity_Name = Convert.ToInt32(model.Activity_Name);
                    ViewBag.SubActivityList = dbContext.tbl_mst_SUBActivityForWidelife.Where(i => i.IsActive == 1 && i.ActivityID == Activity_Name).ToList().Select(i => new SelectListItem { Text = i.SUBActivity_Name, Value = i.ID.ToString() }).OrderBy(s => s.Text);
                }

                #endregion

                TempData["BudgetAnalystReport"] = obj.GetMontlyProgressAnalystReport(model, "ALL");
                FillDropDownMaster();
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        #endregion

        #region Report With Pie Chart
        public ActionResult BudgetReportChart()
        {
            return View();
        }
        #endregion

        #region Budget Summary Report For Division 

        public ActionResult BudgetSummaryReports()
        {
            MonthlyProgressReportExpenditureModel model = new MonthlyProgressReportExpenditureModel();
            MonthlyProgressAnalystReport obj = new MonthlyProgressAnalystReport();
            Session["BudgetSummaryReportFilter"] = null;
            try
            {
                DataSet ReportList = new DataSet();
                TempData["BudgetSummaryReport"] = obj.GetMontlyProgressSummaryeport(model.model, "ALL",ref ReportList);
                Session["BudgetSummaryReportFilter"] = ReportList.Tables[0];
                FillDropDownMasterWithUserIDReport(ReportList.Tables[0]);
                ViewBag.SantauryList = new List<SelectListItem>();
                #region Fill Sub Budget Head and Sub Activity
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();
                #endregion
                
            }
            catch (Exception ex)
            {
                model.List = new List<MonthlyProgressReportModel>();
            }
            return View(model.model);
        }


        [HttpPost]
        public ActionResult BudgetSummaryReports(MonthlyProgressReportModel model, string Circle_Names = "", string Division_Names = "", string SantuaryNames = "")
        {
            MonthlyProgressAnalystReport obj = new MonthlyProgressAnalystReport();
            try
            {

                DataSet ReportList = new DataSet();
                DataTable dt = new DataTable();
                TempData["BudgetSummaryReport"] = obj.GetMontlyProgressSummaryeport(model, "ALL", ref ReportList);
                dt = (DataTable)Session["BudgetSummaryReportFilter"];
                FillDropDownMasterWithUserIDReport(dt);
               

                #region Fill Division
                List<SelectListItem> lstDivision = new List<SelectListItem>();

                try
                {

                    var result = dbContext.tbl_mst_Forest_Divisions.Where(i => i.CIRCLE_CODE == model.Circle_Code && i.ForBudgetModuleDist == 1).OrderByDescending(s => s.DIV_NAME).Select(i => new { i.DIV_CODE, i.DIV_NAME });
                    foreach (var item in result)
                    {
                        lstDivision.Add(new SelectListItem { Text = item.DIV_NAME, Value = item.DIV_CODE });
                    }
                    ViewBag.DivisionLists = lstDivision;
                }
                catch (Exception ex)
                {
                    ViewBag.DivisionList = new List<SelectListItem>();

                }
                #endregion


                #region Fill Santaury
                try
                {
                    ViewBag.SantauryList = new List<SelectListItem>();
                    if (!string.IsNullOrEmpty(model.Division_Code))
                    {
                        List<SelectListItem> SanctuaryList = new List<SelectListItem>();
                        var param1 = new SqlParameter("@Action", "Detail");
                        var param2 = new SqlParameter("@DIV_CODE", model.Division_Code);
                        ViewBag.SantauryList = dbContext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.SantauryList = new List<SelectListItem>();
                }
                #endregion

                #region Fill Sub Budget Head and Sub Activity
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();

                if (!string.IsNullOrEmpty(model.SubBudgetHead) && !string.IsNullOrEmpty(model.BudgetHead))
                {
                    int BudgetHead = Convert.ToInt32(model.BudgetHead);
                    ViewBag.SubBudgetHeadList = dbContext.tbl_mst_SubBudgetHead.Where(i => i.BudgetHeadID == BudgetHead).ToList().Select(i => new SelectListItem { Text = i.SubBudgetHead, Value = i.ID.ToString() }).OrderBy(s => s.Text);
                }
                if (!string.IsNullOrEmpty(model.SubActivity_Name) && !string.IsNullOrEmpty(model.Activity_Name))
                {
                    int Activity_Name = Convert.ToInt32(model.Activity_Name);
                    ViewBag.SubActivityList = dbContext.tbl_mst_SUBActivityForWidelife.Where(i => i.IsActive == 1 && i.ActivityID == Activity_Name).ToList().Select(i => new SelectListItem { Text = i.SUBActivity_Name, Value = i.ID.ToString() }).OrderBy(s => s.Text);
                }

                #endregion
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        #endregion

        #region Get Scheme Budget Circle division all master according the record Developed by Rajveer
        public void FillDropDownMasterWithUserIDReport(DataTable BudgetList)
        {
            List<SelectListItem> FinanceYear = new List<SelectListItem>();
            List<SelectListItem> SchemeList = new List<SelectListItem>();
            List<SelectListItem> BudgetHead = new List<SelectListItem>();
            List<SelectListItem> Activity = new List<SelectListItem>();
            List<SelectListItem> Circle = new List<SelectListItem>();
            try
            {

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


                List<long> SchemeIds = BudgetList.AsEnumerable().Where(x=>!string.IsNullOrEmpty(x["SchemeID"].ToString())).Select(s =>Convert.ToInt64(s["SchemeID"])).Distinct().ToList();
                SchemeList = dbContext.tbl_FDM_SchemeForWidelife.Where(s => SchemeIds.Contains(s.ID)).OrderBy(d => d.Scheme_Name).ToList().Select(item => new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) }).ToList();
                ViewBag.SchemeList = SchemeList;
                #endregion

                #region Activity Bind
                List<long> ActivityIds = BudgetList.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["ActivityID"].ToString())).Select(s => Convert.ToInt64(s["ActivityID"])).Distinct().ToList();
                Activity = dbContext.tbl_mst_ActivityForWidelife.Where(s => ActivityIds.Contains(s.ID)).OrderBy(d => d.Activity_Name).ToList().Select(item => new SelectListItem { Text = item.Activity_Name, Value = Convert.ToString(item.ID) }).ToList();
                ViewBag.Activity = Activity;
                #endregion

                #region Budget Head Bind
                List<long> BudgetHeadIds = BudgetList.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["BudgetHeadId"].ToString())).Select(s => Convert.ToInt64(s["BudgetHeadId"])).Distinct().ToList();
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
                DataTable BudgetList = (DataTable)Session["BudgetSummaryReportFilter"];
                #endregion

                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                List<long> SubActivityIds = BudgetList.AsEnumerable().Where(x => !string.IsNullOrEmpty(x["SubActivityID"].ToString()) && !string.IsNullOrEmpty(x["ActivityID"].ToString()) && Convert.ToInt64(x["ActivityID"])== ActivityID).Select(s => Convert.ToInt64(s["SubActivityID"])).Distinct().ToList();
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
                DataTable BudgetList = (DataTable)Session["BudgetSummaryReportFilter"];
                #endregion

                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<long> SubBudgetHeadIds = BudgetList.AsEnumerable().Where(s => !string.IsNullOrEmpty(s["SubBudgetHeadID"].ToString()) && !string.IsNullOrEmpty(s["BudgetHeadID"].ToString()) && Convert.ToInt64(s["BudgetHeadID"]) == budgetHead).Select(s => Convert.ToInt64(s["SubBudgetHeadID"])).Distinct().ToList();
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
