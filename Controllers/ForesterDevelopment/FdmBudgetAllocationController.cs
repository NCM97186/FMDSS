using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.ForesterDevelopment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class FdmBudgetAllocationController : BaseController
    {
        //
        // GET: /FdmBudgetAllocation/
        List<FdmBudgetAllocation> ModelList = new List<FdmBudgetAllocation>();
        List<FdmBudgetAllocation> ModelList1 = new List<FdmBudgetAllocation>();
        FdmBudgetAllocation _objModel = new FdmBudgetAllocation();
        List<SelectListItem> FinancialYear = new List<SelectListItem>();
        Location _obj = new Location();
        int ModuleID = 0;

        public ActionResult FdmBudgetAllocation()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            List<SelectListItem> items = new List<SelectListItem>();
            List<SelectListItem> CircleCode = new List<SelectListItem>();
            List<SelectListItem> DivisionName = new List<SelectListItem>();
            List<SelectListItem> RangeName = new List<SelectListItem>();
            Activity _objActivityModel = new Activity();
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.ConditionalGridShow = true;
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    _objModel.SSOID = Session["SSOid"].ToString();

                    DataTable dt = _obj.BindFinancialYear();

                    ViewBag.fname1 = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                    {
                        FinancialYear.Add(new SelectListItem { Text = @dr["FinancialYear"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.ddlFinancialYear = FinancialYear;

                    dt = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "CCF");
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        CircleCode.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
                    }
                    ViewBag.CircleCode = CircleCode;

                    dt = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "DCF/DFO");
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        DivisionName.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }
                    ViewBag.ddlDivision = DivisionName;

                    DataTable dtRange = new Common().Select_Range(_objModel.UserID);
                    foreach (DataRow dr in dtRange.Rows)
                    {
                        RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.ddlRange = RangeName;


                    ViewData["BudgetList"] = ModelList;



                    return View("FdmBudgetAllocation", _objModel);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return RedirectToAction("FdmBudgetAllocation", _objModel);
        }

        public JsonResult GetHQBudgetData(string financialYear)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            FdmBudgetAllocation _obj = null;
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataSet ds = _objModel.GetHQAllRecords(Convert.ToInt64(financialYear));
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            _obj = new FdmBudgetAllocation()
                            {

                                Index = Convert.ToInt64(dr["Index"].ToString()),
                                CIRCLE_CODE = dr["CIRCLE_CODE"].ToString(),
                                CIRCLE_NAME = dr["CIRCLE_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                Budget_Head = dr["BudgetHead"].ToString(),
                                Edit_Mode = dr["Level"].ToString()
                            };
                            ModelList.Add(_obj);
                        }

                    }

                    if (ds != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            _obj = new FdmBudgetAllocation()
                            {

                                Index = Convert.ToInt64(dr["Index"].ToString()),
                                STATE_CODE = dr["STATE_CODE"].ToString(),
                                STATE_NAME = dr["STATE_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                Budget_Head = dr["BudgetHead"].ToString(),
                                Budget_HeadID = Convert.ToInt64(dr["BudgetHeadID"].ToString())
                            };
                            ModelList1.Add(_obj);
                        }

                    }
                }

                return Json(new { Model = ModelList, Model1 = ModelList1 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public JsonResult GetCCFBudgetData(string financialYear, string Circle_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            FdmBudgetAllocation _obj = null;
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.SSOID = Session["SSOid"].ToString();
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataSet ds = _objModel.GetCCFAllRecords(Convert.ToInt64(financialYear), Circle_Code, "CCF");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            _obj = new FdmBudgetAllocation()
                            {

                                Index = Convert.ToInt64(dr["Index"].ToString()),
                                DIV_CODE = dr["DIV_CODE"].ToString(),
                                DIV_Name = dr["DIV_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                ApprovedTotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString()),
                                Edit_Mode = dr["Level"].ToString(),
                                Budget_Head = dr["BudgetHead"].ToString()

                            };
                            ModelList.Add(_obj);
                        }

                    }
                    if (ds != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            _obj = new FdmBudgetAllocation()
                            {

                                Index = Convert.ToInt64(dr["Index"].ToString()),
                                CIRCLE_CODE = dr["CIRCLE_CODE"].ToString(),
                                CIRCLE_NAME = dr["CIRCLE_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                Budget_Head = dr["BudgetHead"].ToString(),
                                Budget_HeadID = Convert.ToInt64(dr["BudgetHeadID"].ToString())
                            };
                            ModelList1.Add(_obj);
                        }

                    }
                    else
                    {
                        _obj = new FdmBudgetAllocation()
                            {
                                Index = 0,
                                CIRCLE_CODE = "",
                                CIRCLE_NAME = "",
                                Estimated_Amount = 0,
                                Allocated_Amount = 0,
                                Budget_Head = "",
                                Budget_HeadID = 0
                            };
                        ModelList1.Add(_obj);
                    }
                    return Json(new { Model = ModelList, Model1 = ModelList1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public JsonResult GetDCFBudgetData(string financialYear, string Div_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            FdmBudgetAllocation _obj = null;

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataSet ds = _objModel.GetDCFAllRecords(Convert.ToInt64(financialYear), Div_Code, "DCF/DFO");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            _obj = new FdmBudgetAllocation()
                            {

                                Index = Convert.ToInt64(dr["Index"].ToString()),
                                RANGE_CODE = dr["RANGE_CODE"].ToString(),
                                RANGE_NAME = dr["RANGE_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                Edit_Mode = dr["Level"].ToString(),
                                ApprovedTotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString()),
                                Budget_Head = dr["BudgetHead"].ToString()
                            };
                            ModelList.Add(_obj);
                        }
                        
                    }
                    if (ds != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            _obj = new FdmBudgetAllocation()
                            {

                                Index = Convert.ToInt64(dr["Index"].ToString()),
                                DIV_CODE = dr["DIV_CODE"].ToString(),
                                DIV_Name = dr["DIV_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                Budget_Head = dr["BudgetHead"].ToString(),
                                Budget_HeadID = Convert.ToInt64(dr["BudgetHeadID"].ToString())
                            };
                            ModelList1.Add(_obj);
                        }

                    }
                    else
                    {
                        _obj = new FdmBudgetAllocation()
                        {
                            Index = 0,
                            DIV_CODE = "",
                            DIV_Name = "",
                            Estimated_Amount = 0,
                            Allocated_Amount = 0,
                            Budget_Head = "",
                            Budget_HeadID = 0
                        };
                        ModelList1.Add(_obj);
                    }
                    return Json(new { Model = ModelList, Model1 = ModelList1 }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }


        public JsonResult GetROBudgetData(string financialYear, string Range_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            FdmBudgetAllocation _obj = null;

            //decimal TotalAllocAmount = 0;
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataSet ds = _objModel.GetROAllRecords(Convert.ToInt64(financialYear), Range_Code, "RNG");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            _obj = new FdmBudgetAllocation()
                            {


                                Index = Convert.ToInt64(dr["Index"].ToString()),
                                VIll_Name = dr["VILL_NAME"].ToString(),
                                VILL_CODE = dr["VILL_CODE"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                ApprovedTotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString()),
                                Edit_Mode = dr["Level"].ToString(),
                                Budget_Head = dr["BudgetHead"].ToString()
                            };
                            ModelList.Add(_obj);
                        }
                        
                    }
                    if (ds != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            _obj = new FdmBudgetAllocation()
                            {

                                Index = Convert.ToInt64(dr["Index"].ToString()),
                                RANGE_CODE = dr["RANGE_CODE"].ToString(),
                                RANGE_NAME = dr["RANGE_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                Budget_Head = dr["BudgetHead"].ToString(),
                                Budget_HeadID = Convert.ToInt64(dr["BudgetHeadID"].ToString())
                            };
                            ModelList1.Add(_obj);
                        }

                    }
                    else
                    {
                        _obj = new FdmBudgetAllocation()
                        {
                            Index = 0,
                            DIV_CODE = "",
                            DIV_Name = "",
                            Estimated_Amount = 0,
                            Allocated_Amount = 0,
                            Budget_Head = "",
                            Budget_HeadID = 0
                        };
                        ModelList1.Add(_obj);
                    }
                    return Json(new { Model = ModelList, Model1 = ModelList1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public ActionResult SaveHQBudgetData(FormCollection fCollection)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    string[] circlecode = fCollection["hdCircleCode"].Split(char.Parse(","));
                    string[] allotAmt = fCollection["txtAllocatedAmount"].Split(char.Parse(","));
                    string[] estimatedAmt = fCollection["hdestimatedAmt"].Split(char.Parse(","));
                    string[] circlecode1 = fCollection["hdStCode"].Split(char.Parse(","));
                    string[] allotAmt1 = fCollection["txtAllocatedAmount1"].Split(char.Parse(","));
                    string[] estimatedAmt1 = fCollection["hdestimatedAmt1"].Split(char.Parse(","));
                    string[] budgetHeadID = fCollection["hdbudgetHeadID"].Split(char.Parse(","));
                    string[] editMode = fCollection["hdEditMode"].Split(char.Parse(","));




                    _objModel = new FdmBudgetAllocation();

                    DataTable table = new DataTable();
                    DataTable table1 = new DataTable();

                    table1.Columns.Add("FinancialYear", typeof(int));
                    table1.Columns.Add("Budget_Head", typeof(int));
                    table1.Columns.Add("STATE_CODE", typeof(string));
                    table1.Columns.Add("CIRCLE_CODE", typeof(string));
                    table1.Columns.Add("DIV_CODE", typeof(string));
                    table1.Columns.Add("RANGE_CODE", typeof(string));
                    table1.Columns.Add("VILL_CODE", typeof(string));
                    table1.Columns.Add("Allocated_Amount", typeof(decimal));
                    table1.Columns.Add("Estimated_Amount", typeof(decimal));



                    table.Columns.Add("FinancialYear", typeof(int));
                    table.Columns.Add("CIRCLE_CODE", typeof(string));
                    table.Columns.Add("DIV_CODE", typeof(string));
                    table.Columns.Add("RANGE_CODE", typeof(string));
                    table.Columns.Add("VILL_CODE", typeof(string));
                    table.Columns.Add("Allocated_Amount", typeof(decimal));
                    table.Columns.Add("Estimated_Amount", typeof(decimal));
                    table.Columns.Add("EditMode", typeof(string));

                    for (var i = 0; i < circlecode.Length; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["FinancialYear"] = fCollection["ddlFinancialYear"].ToString();
                        dr["CIRCLE_CODE"] = circlecode[i];
                        dr["Allocated_Amount"] = allotAmt[i];
                        dr["Estimated_Amount"] = estimatedAmt[i];
                        dr["EditMode"] = editMode[i];
                        table.Rows.Add(dr);

                    }
                    for (var i = 0; i < circlecode1.Length; i++)
                    {
                        DataRow dr = table1.NewRow();
                        dr["FinancialYear"] = fCollection["ddlFinancialYear"].ToString();
                        dr["STATE_CODE"] = circlecode1[i];
                        dr["Allocated_Amount"] = allotAmt1[i];
                        dr["Estimated_Amount"] = estimatedAmt1[i];
                        dr["Budget_Head"] = budgetHeadID[i];
                        table1.Rows.Add(dr);

                    }

                    for (int i = table.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = table.Rows[i];
                        if (dr["EditMode"].ToString() == "1")
                            dr.Delete();
                    }
                    table.Columns.Remove("EditMode");

                    //list.ForEach(t => t.RequestedID = Convert.ToString(id));


                    Int64 id = _objModel.SaveBudgetAllocationData(table, table1, Convert.ToInt64(Session["UserID"]), "HQ");
                    if (id > 0)
                    {
                        TempData["ViewSuccessMessage"] = "Record Updated Successfully!!";
                        return RedirectToAction("FdmBudgetAllocation", "FdmBudgetAllocation");
                    }
                    else
                    {
                        TempData["ViewErrorMessage"] = "error occurred during updating records!!";
                        return RedirectToAction("FdmBudgetAllocation", "FdmBudgetAllocation");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public ActionResult SaveCCFBudgetData(FormCollection fCollection)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    string[] divcode = fCollection["hdDivCode"].Split(char.Parse(","));
                    string[] allotAmt = fCollection["txtAllocatedAmount"].Split(char.Parse(","));
                    string[] estimatedAmt = fCollection["hdestimatedAmt"].Split(char.Parse(","));

                    string[] circlecode1 = fCollection["hdCircleCode11"].Split(char.Parse(","));
                    string[] allotAmt1 = fCollection["txtAllocatedAmount1"].Split(char.Parse(","));
                    string[] estimatedAmt1 = fCollection["hdestimatedAmt1"].Split(char.Parse(","));
                    string[] budgetHeadID = fCollection["hdbudgetHeadID"].Split(char.Parse(","));

                    _objModel = new FdmBudgetAllocation();

                    DataTable table = new DataTable();
                    DataTable table1 = new DataTable();

                    table1.Columns.Add("FinancialYear", typeof(int));
                    table1.Columns.Add("Budget_Head", typeof(int));
                    table1.Columns.Add("STATE_CODE", typeof(string));
                    table1.Columns.Add("CIRCLE_CODE", typeof(string));
                    table1.Columns.Add("DIV_CODE", typeof(string));
                    table1.Columns.Add("RANGE_CODE", typeof(string));
                    table1.Columns.Add("VILL_CODE", typeof(string));
                    table1.Columns.Add("Allocated_Amount", typeof(decimal));
                    table1.Columns.Add("Estimated_Amount", typeof(decimal));

                    table.Columns.Add("FinancialYear", typeof(int));
                    table.Columns.Add("CIRCLE_CODE", typeof(string));
                    table.Columns.Add("DIV_CODE", typeof(string));
                    table.Columns.Add("RANGE_CODE", typeof(string));
                    table.Columns.Add("VILL_CODE", typeof(string));
                    table.Columns.Add("Allocated_Amount", typeof(decimal));
                    table.Columns.Add("Estimated_Amount", typeof(decimal));
                    //table.Columns.Add("Allocated_ID", typeof(decimal));

                    for (var i = 0; i < divcode.Length; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["FinancialYear"] = fCollection["ddlFinancialYear"].ToString();
                        dr["DIV_CODE"] = divcode[i];
                        dr["Allocated_Amount"] = allotAmt[i];
                        dr["Estimated_Amount"] = estimatedAmt[i];

                        table.Rows.Add(dr);

                    }
                    for (var i = 0; i < circlecode1.Length; i++)
                    {
                        DataRow dr = table1.NewRow();
                        dr["FinancialYear"] = fCollection["ddlFinancialYear"].ToString();
                        dr["CIRCLE_CODE"] = circlecode1[i];
                        dr["Allocated_Amount"] = allotAmt1[i];
                        dr["Estimated_Amount"] = estimatedAmt1[i];
                        dr["Budget_Head"] = budgetHeadID[i];
                        table1.Rows.Add(dr);

                    }

                    Int64 id = _objModel.SaveCCFBudgetAllocationData(table,table1, Convert.ToInt64(Session["UserID"]), fCollection["CircleCode"].ToString(), "CCF");
                    if (id > 0)
                    {
                        TempData["ViewSuccessMessage"] = "Record Updated Successfully!!";
                        return RedirectToAction("FdmBudgetAllocation", "FdmBudgetAllocation");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public ActionResult SaveDCFAllocatedBudgetData(FormCollection fCollection)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    string[] Rangecode = fCollection["hdRangeCode"].Split(char.Parse(","));
                    string[] allotAmt = fCollection["txtAllocatedAmount"].Split(char.Parse(","));
                    string[] estimatedAmt = fCollection["hdestimatedAmt"].Split(char.Parse(","));

                    string[] circlecode1 = fCollection["hdDivCode11"].Split(char.Parse(","));
                    string[] allotAmt1 = fCollection["txtAllocatedAmount1"].Split(char.Parse(","));
                    string[] estimatedAmt1 = fCollection["hdestimatedAmt1"].Split(char.Parse(","));
                    string[] budgetHeadID = fCollection["hdbudgetHeadID"].Split(char.Parse(","));
                    _objModel = new FdmBudgetAllocation();

                    DataTable table = new DataTable();
                    DataTable table1 = new DataTable();

                    table1.Columns.Add("FinancialYear", typeof(int));
                    table1.Columns.Add("Budget_Head", typeof(int));
                    table1.Columns.Add("STATE_CODE", typeof(string));
                    table1.Columns.Add("CIRCLE_CODE", typeof(string));
                    table1.Columns.Add("DIV_CODE", typeof(string));
                    table1.Columns.Add("RANGE_CODE", typeof(string));
                    table1.Columns.Add("VILL_CODE", typeof(string));
                    table1.Columns.Add("Allocated_Amount", typeof(decimal));
                    table1.Columns.Add("Estimated_Amount", typeof(decimal));

                    table.Columns.Add("FinancialYear", typeof(int));
                    table.Columns.Add("CIRCLE_CODE", typeof(string));
                    table.Columns.Add("DIV_CODE", typeof(string));
                    table.Columns.Add("RANGE_CODE", typeof(string));
                    table.Columns.Add("VILL_CODE", typeof(string));
                    table.Columns.Add("Allocated_Amount", typeof(decimal));
                    table.Columns.Add("Estimated_Amount", typeof(decimal));
                    //table.Columns.Add("Allocated_ID", typeof(decimal));

                    for (var i = 0; i < Rangecode.Length; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["FinancialYear"] = fCollection["ddlFinancialYear"].ToString();
                        dr["RANGE_CODE"] = Rangecode[i];
                        dr["Allocated_Amount"] = allotAmt[i];
                        dr["Estimated_Amount"] = estimatedAmt[i];

                        table.Rows.Add(dr);

                    }
                    for (var i = 0; i < circlecode1.Length; i++)
                    {
                        DataRow dr = table1.NewRow();
                        dr["FinancialYear"] = fCollection["ddlFinancialYear"].ToString();
                        dr["DIV_CODE"] = circlecode1[i];
                        dr["Allocated_Amount"] = allotAmt1[i];
                        dr["Estimated_Amount"] = estimatedAmt1[i];
                        dr["Budget_Head"] = budgetHeadID[i];
                        table1.Rows.Add(dr);

                    }

                    Int64 id = _objModel.SaveDCFBudgetAllocationData(table,table1, Convert.ToInt64(Session["UserID"]), fCollection["ddlDivision"].ToString(), "DCF/DFO");
                    if (id > 0)
                    {
                        TempData["ViewSuccessMessage"] = "Record Updated Successfully!!";
                        return RedirectToAction("FdmBudgetAllocation", "FdmBudgetAllocation");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public ActionResult SaveROAllocatedBudgetData(FormCollection fCollection)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    string[] Villcode = fCollection["hdVillCode"].Split(char.Parse(","));
                    string[] allotAmt = fCollection["txtAllocatedAmount"].Split(char.Parse(","));
                    string[] estimatedAmt = fCollection["hdestimatedAmt"].Split(char.Parse(","));

                    string[] circlecode1 = fCollection["hdRangeCode"].Split(char.Parse(","));
                    string[] allotAmt1 = fCollection["txtAllocatedAmount1"].Split(char.Parse(","));
                    string[] estimatedAmt1 = fCollection["hdestimatedAmt1"].Split(char.Parse(","));
                    string[] budgetHeadID = fCollection["hdbudgetHeadID"].Split(char.Parse(","));

                    _objModel = new FdmBudgetAllocation();

                    DataTable table = new DataTable();
                    DataTable table1 = new DataTable();

                    table1.Columns.Add("FinancialYear", typeof(int));
                    table1.Columns.Add("Budget_Head", typeof(int));
                    table1.Columns.Add("STATE_CODE", typeof(string));
                    table1.Columns.Add("CIRCLE_CODE", typeof(string));
                    table1.Columns.Add("DIV_CODE", typeof(string));
                    table1.Columns.Add("RANGE_CODE", typeof(string));
                    table1.Columns.Add("VILL_CODE", typeof(string));
                    table1.Columns.Add("Allocated_Amount", typeof(decimal));
                    table1.Columns.Add("Estimated_Amount", typeof(decimal));

                    table.Columns.Add("FinancialYear", typeof(int));
                    table.Columns.Add("CIRCLE_CODE", typeof(string));
                    table.Columns.Add("DIV_CODE", typeof(string));
                    table.Columns.Add("RANGE_CODE", typeof(string));
                    table.Columns.Add("VILL_CODE", typeof(string));
                    table.Columns.Add("Allocated_Amount", typeof(decimal));
                    table.Columns.Add("Estimated_Amount", typeof(decimal));
                    //table.Columns.Add("Allocated_ID", typeof(decimal));

                    for (var i = 0; i < Villcode.Length; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["FinancialYear"] = fCollection["ddlFinancialYear"].ToString();
                        dr["VILL_CODE"] = Villcode[i];
                        dr["Allocated_Amount"] = allotAmt[i];
                        dr["Estimated_Amount"] = estimatedAmt[i];

                        table.Rows.Add(dr);

                    }
                    for (var i = 0; i < circlecode1.Length; i++)
                    {
                        DataRow dr = table1.NewRow();
                        dr["FinancialYear"] = fCollection["ddlFinancialYear"].ToString();
                        dr["RANGE_CODE"] = circlecode1[i];
                        dr["Allocated_Amount"] = allotAmt1[i];
                        dr["Estimated_Amount"] = estimatedAmt1[i];
                        dr["Budget_Head"] = budgetHeadID[i];
                        table1.Rows.Add(dr);

                    }


                    Int64 id = _objModel.SaveROBudgetAllocationData(table,table1, Convert.ToInt64(Session["UserID"]), fCollection["ddlRange"].ToString(), "RNG");
                    if (id > 0)
                    {
                        TempData["ViewSuccessMessage"] = "Record Updated Successfully!!";
                        return RedirectToAction("FdmBudgetAllocation", "FdmBudgetAllocation");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }
        public JsonResult EditHQBudgetData(string BdgID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"]);
                    DataSet ds = _objModel.EditHQBudget(Convert.ToInt64(BdgID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {


                        return Json(new
                        {

                            Div_Name = ds.Tables[0].Rows[0]["DIV_NAME"].ToString(),
                            AllocAmount = ds.Tables[0].Rows[0]["Allocated_Amount"].ToString(),
                            ID = Convert.ToInt64(BdgID)

                        });

                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }




    }
}
