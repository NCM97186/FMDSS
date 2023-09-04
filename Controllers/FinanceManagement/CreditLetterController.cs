using FMDSS.GenericClass;
using FMDSS.Models;
using FMDSS.Models.FinanceManagement;
using FMDSS.Models.ForesterDevelopment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FMDSS.Controllers.FinanceManagement
{
    public class CreditLetterController : BaseController
    {
        //
        // GET: /CreditLetter/
        LCGenration _objModel = new LCGenration();
        LCAllocation _objAllocModel = new LCAllocation();
        List<SelectListItem> BudgetType = new List<SelectListItem>();
        List<SelectListItem> BudgetHead = new List<SelectListItem>();
        List<LCGenration> LCList = new List<LCGenration>();
        List<LCAllocation> ModelList = new List<LCAllocation>();
        private int DesignationID = 0;
        private int ModuleID = 5;
        public ActionResult CreditLetter()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DataSet ds, ds1, dsLCDetails = null;
            DataTable dtf1 = null;
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.SSOID = Session["SSOid"].ToString();
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());

                    ds = _objModel.BindDDL("BdgType");
                    ds1 = _objModel.BindDDL("BdgHead");
                    dsLCDetails = _objModel.GetLCDetails(_objModel.SSOID, "LCDetails");

                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                    {
                        BudgetType.Add(new SelectListItem { Text = @dr["BudgetType"].ToString(), Value = @dr["BudgetType_ID"].ToString() });
                    }
                    ViewBag.BudgetTypeID1 = BudgetType;

                    foreach (System.Data.DataRow dr in ds1.Tables[0].Rows)
                    {
                        BudgetHead.Add(new SelectListItem { Text = @dr["BudgetHead"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.BudgetHeadNumber = BudgetHead;


                    if (dsLCDetails.Tables[0].Rows.Count > 0)
                    {
                        _objModel.OfficeName = dsLCDetails.Tables[0].Rows[0]["OfficeName"].ToString();
                        _objModel.OfficeIDNumber = dsLCDetails.Tables[0].Rows[0]["Office_ID"].ToString();
                        _objModel.Approved_BudgetAmount = Convert.ToDecimal(dsLCDetails.Tables[0].Rows[0]["Allocated_Amount"].ToString());
                        _objModel.Div_Code = dsLCDetails.Tables[0].Rows[0]["DIV_CODE"].ToString();

                    }

                    dtf1 = _objModel.GetAllRecords(_objModel.UserID, "Select", 0);
                    if (dtf1.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtf1.Rows)
                            LCList.Add(
                                new LCGenration()
                                {
                                    LCreditID = Convert.ToInt64(dr["LCreditID"].ToString()),
                                    Index = Convert.ToInt32(dr["Index"].ToString()),
                                    OfficeName = dr["OfficeName"].ToString(),
                                    BankBranch_Name = dr["BankBranch_Name"].ToString(),
                                    Budget_HeadNumber = dr["BudgetHead"].ToString(),
                                    BudgetTypeName = dr["BudgetType"].ToString()



                                });
                    }

                    ViewData["LCdataList"] = LCList;
                    return View(_objModel);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 5, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

        public ActionResult SaveLCData(FormCollection fCollection, LCGenration _model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    _objModel.OfficeName = string.IsNullOrEmpty(_model.OfficeName) ? "" : _model.OfficeName;
                    _objModel.OfficeIDNumber = string.IsNullOrEmpty(_model.OfficeIDNumber) ? "" : _model.OfficeIDNumber;
                    _objModel.TOfficeName = string.IsNullOrEmpty(_model.TOfficeName) ? "" : _model.TOfficeName;
                    _objModel.TOfficeCode = string.IsNullOrEmpty(_model.TOfficeCode) ? "" : _model.TOfficeCode;
                    _objModel.BankBranch_Name = string.IsNullOrEmpty(_model.BankBranch_Name) ? "" : _model.BankBranch_Name;
                    _objModel.IFSC_Code = string.IsNullOrEmpty(_model.IFSC_Code) ? "" : _model.IFSC_Code;
                    _objModel.Month_Year = string.IsNullOrEmpty(_model.Month_Year) ? "" : _model.Month_Year;
                    _objModel.Budget_HeadNumber = string.IsNullOrEmpty(_model.Budget_HeadNumber) ? "" : _model.Budget_HeadNumber;
                    _objModel.BudgetTypeID = _model.BudgetTypeID;
                    _objModel.Assigned_BudgetAmount = Convert.ToDecimal(_model.Assigned_BudgetAmount);
                    _objModel.Requested_BudgetAmount = Convert.ToDecimal(_model.Requested_BudgetAmount);
                    _objModel.RemainingBudget_Amount = Convert.ToDecimal(_model.RemainingBudget_Amount);
                    _objModel.Past_Credit_Amount = Convert.ToDecimal(_model.Past_Credit_Amount);
                    _objModel.Remaining_Credit_Amount = Convert.ToDecimal(_model.Remaining_Credit_Amount);
                    _objModel.Offices_SpentAmount = Convert.ToDecimal(_model.Offices_SpentAmount);
                    _objModel.Div_Code = string.IsNullOrEmpty(fCollection["hdDivCode"].ToString()) ? "" : fCollection["hdDivCode"].ToString();


                    Int64 id = _objModel.InsertDFOLCDetails(_objModel, "Insert");
                    if (id > 0)
                    {

                        return RedirectToAction("CreditLetter", "CreditLetter");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 5, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public ActionResult LCEstimation()
        {
            return View();
        }

        public JsonResult GetEstimateData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            List<LCGenration> ModelList = new List<LCGenration>();


            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    if (Session["DesignationId"] != null)
                    {

                        DesignationID = Convert.ToInt32(Session["DesignationId"].ToString());
                        _objModel.SSOID = Session["SSOid"].ToString();
                        switch (DesignationID)
                        {

                            case 1:
                            case 2:


                                DataTable dt = _objModel.GetAllHOFRec();
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                        ModelList.Add(new LCGenration()
                                        {

                                            Index = Convert.ToInt32(dr["Index"].ToString()),
                                            STATE_CODE = dr["STATE_CODE"].ToString(),
                                            STATE_NAME = dr["STATE_NAME"].ToString(),
                                            EstAmount = Convert.ToDecimal(dr["estAmount"].ToString()),
                                            AllocAmount = Convert.ToDecimal(dr["AllocAmount"].ToString()),
                                            RequestedAmount = Convert.ToDecimal(dr["RequestedAmount"].ToString())
                                        });

                                }

                                break;

                            case 4:

                                string Circle_Code = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "CCF").Rows[0]["CIRCLE_CODE"].ToString();
                                dt = _objModel.GetAllCCFRec(Circle_Code);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                        ModelList.Add(new LCGenration()
                                        {

                                            Index = Convert.ToInt32(dr["Index"].ToString()),
                                            CIRCLE_CODE = dr["CIRCLE_CODE"].ToString(),
                                            CIRCLE_NAME = dr["CIRCLE_NAME"].ToString(),
                                            EstAmount = Convert.ToDecimal(dr["estAmount"].ToString()),
                                            AllocAmount = Convert.ToDecimal(dr["AllocAmount"].ToString()),
                                            RequestedAmount = Convert.ToDecimal(dr["RequestedAmount"].ToString())
                                        });

                                }

                                break;


                            default:
                                ModelList.FirstOrDefault().BudgetTypeID = 0;

                                break;

                        }

                    }


                }
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }

            return Json(new { data = ModelList, data2 = DesignationID }, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetBudgetByUserDesignation(int designationID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            List<LCGenration> ModelList = new List<LCGenration>();


            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    if (Session["DesignationId"] != null)
                    {

                        //DesignationID = Convert.ToInt32(Session["DesignationId"].ToString());
                        _objModel.SSOID = Session["SSOid"].ToString();
                        switch (designationID)
                        {

                            case 1:
                            case 2:

                                //ModelList.FirstOrDefault().BudgetTypeID = 1;
                                DataTable dt = _objModel.GetHOFCredit();
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                        ModelList.Add(new LCGenration()
                                        {

                                            Index = Convert.ToInt32(dr["Index"].ToString()),
                                            CIRCLE_CODE = dr["CIRCLE_CODE"].ToString(),
                                            CIRCLE_NAME = dr["CIRCLE_NAME"].ToString(),
                                            EstAmount = Convert.ToDecimal(dr["estAmount"].ToString()),
                                            AllocAmount = Convert.ToDecimal(dr["AllocAmount"].ToString()),
                                            RequestedAmount = Convert.ToDecimal(dr["RequestedAmount"].ToString()),
                                            ReleaseAmount = Convert.ToDecimal(dr["RequestedAmount"].ToString()),
                                        });

                                }

                                break;

                            case 4:

                                //ModelList.FirstOrDefault().BudgetTypeID = 4;
                                dt = _objModel.GetCCFCredit();
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                        ModelList.Add(new LCGenration()
                                        {

                                            Index = Convert.ToInt32(dr["Index"].ToString()),
                                            Div_Code = dr["DIV_CODE"].ToString(),
                                            DIV_NAME = dr["DIV_NAME"].ToString(),
                                            EstAmount = Convert.ToDecimal(dr["estAmount"].ToString()),
                                            AllocAmount = Convert.ToDecimal(dr["AllocAmount"].ToString()),
                                            RequestedAmount = Convert.ToDecimal(dr["RequestedAmount"].ToString()),
                                            ReleaseAmount = Convert.ToDecimal(dr["ReleaseAmount"].ToString())
                                        });

                                }

                                break;


                            default:
                                ModelList.FirstOrDefault().BudgetTypeID = 0;

                                break;

                        }

                    }


                }
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }

            return Json(ModelList, JsonRequestBehavior.AllowGet);


        }

        public ActionResult SaveDCFBudgetData(FormCollection fCollection)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LCGenration _obj = null;
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    string[] Villcode = fCollection["hdDivCode"].Split(char.Parse(","));
                    string[] estimatedAmt = fCollection["hdReleaseAmount"].Split(char.Parse(","));

                    _obj = new LCGenration();

                    DataTable table = new DataTable();


                    table.Columns.Add("Div_Code", typeof(string));
                    table.Columns.Add("ApprovedAmount", typeof(decimal));


                    for (var i = 0; i < Villcode.Length; i++)
                    {
                        DataRow dr = table.NewRow();

                        dr["Div_Code"] = Villcode[i];
                        dr["ApprovedAmount"] = estimatedAmt[i];
                        table.Rows.Add(dr);

                    }


                    Int64 id = _objModel.SaveDCFBudgetEstimationData(table, Convert.ToInt64(Session["UserID"]));

                    if (id == 1)
                    {
                        TempData["ViewSuccessMessage"] = "Record Updated Successfully!!";
                        return RedirectToAction("LCEstimation", "CreditLetter");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 5, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public ActionResult AllocationBudgetLOC()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> CircleCode = new List<SelectListItem>();
            try
            {
                if (Session["UserID"] != null)
                {

                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    _objModel.SSOID = Session["SSOid"].ToString();
                    DataTable dt = null;
                    dt = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "CCF");
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        CircleCode.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
                    }
                    ViewBag.CircleCode = CircleCode;
                    return View();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return RedirectToAction("AllocationBudgetLOC", _objModel);
        }

        public JsonResult GetHQBudgetData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LCAllocation _obj = null;

            try
            {
                if (Session["UserID"] != null)
                {
                    _objAllocModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = _objAllocModel.GetHOFLOCAllocation("HQ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _obj = new LCAllocation()
                            {

                                Index = Convert.ToInt32(dr["Index"].ToString()),
                                CIRCLE_CODE = dr["CIRCLE_CODE"].ToString(),
                                CIRCLE_NAME = dr["CIRCLE_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                Edit_Mode = dr["Level"].ToString()
                            };
                            ModelList.Add(_obj);
                        }
                        return Json(ModelList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 5, DateTime.Now, _objModel.UserID);
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
                    string[] editMode = fCollection["hdEditMode"].Split(char.Parse(","));




                    _objAllocModel = new LCAllocation();

                    DataTable table = new DataTable();


                    table.Columns.Add("CIRCLE_CODE", typeof(string));
                    table.Columns.Add("DIV_CODE", typeof(string));
                    table.Columns.Add("Allocated_Amount", typeof(decimal));
                    table.Columns.Add("Estimated_Amount", typeof(decimal));
                    table.Columns.Add("EditMode", typeof(string));

                    for (var i = 0; i < circlecode.Length; i++)
                    {
                        DataRow dr = table.NewRow();

                        dr["CIRCLE_CODE"] = circlecode[i];
                        dr["Allocated_Amount"] = allotAmt[i];
                        dr["Estimated_Amount"] = estimatedAmt[i];
                        dr["EditMode"] = editMode[i];
                        table.Rows.Add(dr);

                    }

                    for (int i = table.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = table.Rows[i];
                        if (dr["EditMode"].ToString() == "1")
                            dr.Delete();
                    }
                    table.Columns.Remove("EditMode");

                    //list.ForEach(t => t.RequestedID = Convert.ToString(id));


                    Int64 id = _objAllocModel.SaveLOCAllocationData(table, Convert.ToInt64(Session["UserID"]), "HQ");
                    if (id > 0)
                    {

                        return RedirectToAction("AllocationBudgetLOC", "CreditLetter");
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

        public JsonResult GetCCFBudgetData(string Circle_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LCAllocation _obj = null;
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.SSOID = Session["SSOid"].ToString();
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = _objAllocModel.GetCCFAllRecords(Circle_Code, "CCF");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _obj = new LCAllocation()
                            {

                                Index = Convert.ToInt32(dr["Index"].ToString()),
                                DIV_CODE = dr["DIV_CODE"].ToString(),
                                DIV_Name = dr["DIV_NAME"].ToString(),
                                Estimated_Amount = Convert.ToDecimal(dr["EstimatedAmount"].ToString()),
                                Allocated_Amount = Convert.ToDecimal(dr["allocatedamt"].ToString()),
                                ApprovedTotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString()),
                                Edit_Mode = dr["Level"].ToString(),

                            };
                            ModelList.Add(_obj);
                        }
                        return Json(ModelList, JsonRequestBehavior.AllowGet);
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

                    _objAllocModel = new LCAllocation();

                    DataTable table = new DataTable();


                    table.Columns.Add("CIRCLE_CODE", typeof(string));
                    table.Columns.Add("DIV_CODE", typeof(string));
                    table.Columns.Add("Allocated_Amount", typeof(decimal));
                    table.Columns.Add("Estimated_Amount", typeof(decimal));


                    for (var i = 0; i < divcode.Length; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["DIV_CODE"] = divcode[i];
                        dr["Allocated_Amount"] = allotAmt[i];
                        dr["Estimated_Amount"] = estimatedAmt[i];

                        table.Rows.Add(dr);

                    }


                    Int64 id = _objAllocModel.SaveCCFBudgetAllocationData(table, Convert.ToInt64(Session["UserID"]), fCollection["CircleCode"].ToString(), "CCF");
                    if (id > 0)
                    {

                        return RedirectToAction("AllocationBudgetLOC", "CreditLetter");
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

        public JsonResult ViewLOC(int ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
           
            GenericClasses<LCGenration> genLC = new GenericClasses<LCGenration>();
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.SSOID = Session["SSOid"].ToString();
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());

                    DataTable dt = _objModel.GetAllRecords(_objModel.UserID, "Select", ID);

                    
                  
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return Json(genLC.model(dt), JsonRequestBehavior.AllowGet);
                        
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
