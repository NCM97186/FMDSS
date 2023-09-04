/////////////////////////////
///////Bug no 385 & 386



using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.ForestDevelopment;
using FMDSS.Models.ForesterDevelopment;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class FdmBudgetEstimationController : BaseController
    {
        //
        // GET: /FdmBudgetEstimation/
        FdmBudgetEstimation _objModel = new FdmBudgetEstimation();
        FdmBudgetFOfficeEstimation _objFOfficeModel = new FdmBudgetFOfficeEstimation();
        List<SelectListItem> BudgerServey = new List<SelectListItem>();
        List<SelectListItem> FinancialYear = new List<SelectListItem>();
        List<SelectListItem> District = new List<SelectListItem>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        List<SelectListItem> DefineModel = new List<SelectListItem>();
        SubActivity _objSubActivity = new SubActivity();
        List<SelectListItem> Activity = new List<SelectListItem>();
        Activity _objActivityModel = new Activity();
        List<FdmBudgetEstimation> ModelList = new List<FdmBudgetEstimation>();
        List<SelectListItem> RangeName = new List<SelectListItem>();
        Location _obj = new Location();
        Int64 UserID = 0;
        int ModuleID = 2;
        private int DesignationID = 0;
        private string RegionType = string.Empty;
        //History: Code Update with ref. to bug ID: 281
        public ActionResult FdmBudgetEstimation()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());

                    DataTable dt = _obj.District("District");

                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }

                    dt = _obj.BindFinancialYear();

                    ViewBag.fname1 = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                    {
                        FinancialYear.Add(new SelectListItem { Text = @dr["FinancialYear"].ToString(), Value = @dr["ID"].ToString() });
                    }


                    ViewBag.fname = dt;



                    DataTable dtRange = new Common().Select_Range(_objModel.UserID);
                    foreach (DataRow dr in dtRange.Rows)
                    {
                        RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.ddlRange = RangeName;

                    _objModel.SSOID = Session["SSOid"].ToString();
                    string Code = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "RANGER").Rows[0]["RANGE_CODE"].ToString();

                    ViewBag.ddlFinancialYear = FinancialYear;
                    ViewBag.ddlBudgerServey = BudgerServey;
                    ViewBag.ddlVillName = VillageName;
                    DataSet dtf = _objModel.GetAllRecords(Code);
                    for (int i = 0; i < dtf.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf.Tables[0].Rows)
                            ModelList.Add(
                                new FdmBudgetEstimation()

                                {
                                    Index = Convert.ToInt64(dr["Index"].ToString()),
                                    ID = Convert.ToInt64(dr["ID"].ToString()),
                                    FinancialYeartext = dr["FinancialYear"].ToString(),
                                    VILL_CODE_Name = dr["VILL_NAME"].ToString(),
                                    EstimatedBudget = Convert.ToDecimal(dr["EstimatedBudget"].ToString()),
                                    Status=Convert.ToInt32(dr["Status"].ToString())

                                });

                    }
                    ViewData["BudgetList"] = ModelList;
                    return View();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        [HttpPost]
        public JsonResult GetActivity(string Model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());


                    DataTable dt = _objSubActivity.BindDDlActivity(Convert.ToInt64(Model));
                    ViewBag.fname1 = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                    {
                        Activity.Add(new SelectListItem { Text = @dr["Activity_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.Activity_ID = new SelectList(Activity, "Value", "Text");


                    return Json(new SelectList(Activity, "Value", "Text"));
                }
                else
                {
                    //Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;

        }
        //History: Code Update with ref. to bug ID: 282 by gaurav
        /// <summary>
        /// SaveBudget
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="FCollection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveBudget(FdmBudgetEstimation Model, FormCollection FCollection, string command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    if (command == "")
                    {
                        _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                        _objModel.FinancialYear = string.IsNullOrEmpty(FCollection["ddlFinancialYear"].ToString()) ? 0 : Convert.ToInt32(FCollection["ddlFinancialYear"].ToString());
                        _objModel.VILL_CODE = string.IsNullOrEmpty(FCollection["ddlVillName"].ToString()) ? "" : FCollection["ddlVillName"].ToString();
                        _objModel.RANGE_CODE = string.IsNullOrEmpty(FCollection["ddlRange"].ToString()) ? "" : FCollection["ddlRange"].ToString();
                        _objModel.ServeyID = string.IsNullOrEmpty(FCollection["ddlServey"].ToString()) ? "" : FCollection["ddlServey"].ToString();
                        _objModel.EstimatedBudget = Convert.ToDecimal(FCollection["EstimatedBudget"].ToString());

                        Int64 id = _objModel.SubmitBudget(_objModel);
                        if (id == -2)
                        {
                            TempData["ViewErrorMessage"] = "Record already Exist!!";
                            return RedirectToAction("FdmBudgetEstimation", "FdmBudgetEstimation");

                        }
                        else if (id > 0 && id != -2)
                        {
                            TempData["ViewSuccessMessage"] = "Record Save Successfully!!";
                            return RedirectToAction("FdmBudgetEstimation", "FdmBudgetEstimation");

                        }
                        else
                        {

                            return View("Error");
                        }

                    }
                    else
                    {
                        _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                        _objModel.EstimatedBudget = Convert.ToDecimal(FCollection["EstimatedBudget"].ToString());
                        _objModel.ID = Convert.ToInt64(command);
                        Int64 id = _objModel.UpdateBudget(_objModel);
                        if (id > 0)
                        {
                            TempData["ViewSuccessMessage"] = "Record Update Successfully!!";
                            return RedirectToAction("FdmBudgetEstimation", "FdmBudgetEstimation");

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

        public JsonResult EditBudgetData(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataSet ds = _objModel.GetAllRecords(Convert.ToInt64(ID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _objModel.FinancialYear = Convert.ToInt64(ds.Tables[0].Rows[0]["FinancialYearID"].ToString());
                        _objModel.ServeyID = ds.Tables[0].Rows[0]["ServeyID"].ToString();
                        _objModel.RANGE_CODE = ds.Tables[0].Rows[0]["RANGE_CODE"].ToString();
                        _objModel.RANGE_NAME = ds.Tables[0].Rows[0]["RANGE_NAME"].ToString();
                        _objModel.VILL_CODE = ds.Tables[0].Rows[0]["VILL_CODE"].ToString();
                        _objModel.EstimatedBudget = Convert.ToDecimal(ds.Tables[0].Rows[0]["EstimatedBudget"].ToString());



                    }
                    return Json(new
                    {

                        FinancialYear = _objModel.FinancialYear,
                        RANGE_CODE = _objModel.RANGE_CODE,
                        ServeyID = _objModel.ServeyID,
                        VILL_CODE = _objModel.VILL_CODE,
                        ID = Convert.ToInt64(ID),
                        EstimatedBudget = _objModel.EstimatedBudget

                    });


                }
                else
                {

                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }



        /// <summary>
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetVillageNamebyRange(string RangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = new Common().Select_VillagesbyRange(RangeCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }

                    ViewBag.ddlVillName = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;

        }

        /// <summary>
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetDDLServeyDetails(string VILL_CODE)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> BudgerServey = new List<SelectListItem>();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    BudgerServey.Add(new SelectListItem { Text = "--Select--", Value = "0" });


                    DataTable dt = new Common().GetDDLBudgetServey(VILL_CODE);
                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.fname = dt;
                        foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                        {
                            BudgerServey.Add(new SelectListItem { Text = @dr["BudgetEsSurvey"].ToString(), Value = @dr["ID"].ToString() });
                        }

                        ViewBag.ddlBudgerServey = new SelectList(BudgerServey, "Value", "Text");
                    }



                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return Json(new SelectList(BudgerServey, "Value", "Text"));

        }

        /// <summary>
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetBudgetServeyDetails(string ServeyID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SurveyBudget> SurveyList = new List<SurveyBudget>();

            try
            {
                if (Session["UserID"] != null)
                {
                    SurveyBudget _obj = new SurveyBudget();
                    DataSet ds = _objModel.GetBudgetServeyRecords(Convert.ToInt64(ServeyID), "LowerLevel");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            SurveyList.Add(
                                new SurveyBudget()

                                {

                                    SDate = dr["SurveyDate"].ToString(),
                                    EstimatedAmt = Convert.ToDecimal(dr["EstimatedAmt"].ToString()),
                                    //Project = dr["Project_Name"].ToString(),
                                    SchemeName = dr["Scheme_Name"].ToString(),
                                    ModelName = dr["Model_Name"].ToString(),
                                    Activity = dr["Activity_Name"].ToString(),
                                    BudgetHead = dr["BudgetHead"].ToString()

                                });

                        //}


                        return Json(SurveyList, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        public ActionResult FDMBudgetEsimationUpper()
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> DivisionName = new List<SelectListItem>();
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    _objModel.SSOID = Session["SSOid"].ToString();
                    DataTable dt = _obj.BindFinancialYear();

                    ViewBag.fname1 = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                    {
                        FinancialYear.Add(new SelectListItem { Text = @dr["FinancialYear"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.fname = dt;

                    ViewBag.ddlFinancialYear = FinancialYear;

                    return View();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        public JsonResult GetEstimateData(string financialYear)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<FDMDCFBudgetEstimation> ModelList = new List<FDMDCFBudgetEstimation>();
            List<FDMDCFBudgetEstimation> ModelList1 = new List<FDMDCFBudgetEstimation>();
            string Amount = string.Empty;

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


                                DataSet ds = _objModel.GetEstBudget(Convert.ToInt32(financialYear), "ST001", DesignationID);
                                if (ds.Tables[1].Rows.Count > 0)
                                    Amount = ds.Tables[1].Rows[0]["Amount"].ToString();
                                if (ds != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                        ModelList.Add(new FDMDCFBudgetEstimation()
                                        {
                                            ID = DesignationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            STATE_CODE = dr["STATE_CODE"].ToString(),
                                            STATE_NAME = dr["STATE_NAME"].ToString(),
                                            Budget_Head = dr["BudgetHead"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString()),
                                            FofficeEstAmount = Convert.ToDecimal(Amount)
                                        });

                                }

                                if (ds != null && ds.Tables[2].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[2].Rows)
                                        ModelList1.Add(new FDMDCFBudgetEstimation()
                                        {
                                            ID = DesignationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            STATE_CODE = dr["STATE_CODE"].ToString(),
                                            STATE_NAME = dr["STATE_NAME"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString())

                                        });

                                }

                                break;

                            case 4:

                                string Circle_Code = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "CCF").Rows[0]["CIRCLE_CODE"].ToString();
                                ds = _objModel.GetEstBudget(Convert.ToInt32(financialYear), Circle_Code, DesignationID);
                                if (ds.Tables[1].Rows.Count > 0)
                                    Amount = ds.Tables[1].Rows[0]["Amount"].ToString();
                                if (ds != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                        ModelList.Add(new FDMDCFBudgetEstimation()
                                        {
                                            ID = DesignationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            CIRCLE_CODE = dr["CIRCLE_CODE"].ToString(),
                                            CIRCLE_NAME = dr["CIRCLE_NAME"].ToString(),
                                            Budget_Head = dr["BudgetHead"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString()),
                                            FofficeEstAmount = Convert.ToDecimal(Amount)
                                        });

                                }

                                if (ds != null && ds.Tables[2].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[2].Rows)
                                        ModelList1.Add(new FDMDCFBudgetEstimation()
                                        {
                                            ID = DesignationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            CIRCLE_CODE = dr["CIRCLE_CODE"].ToString(),
                                            CIRCLE_NAME = dr["CIRCLE_NAME"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString())

                                        });

                                }

                                break;

                            case 7:
                            case 6:

                                string Div_Code = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "DCF/DFO").Rows[0]["DIV_CODE"].ToString();
                                ds = _objModel.GetEstBudget(Convert.ToInt32(financialYear), Div_Code, DesignationID);
                                if (ds.Tables[1].Rows.Count > 0)
                                    Amount = ds.Tables[1].Rows[0]["Amount"].ToString();
                                if (ds != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                        ModelList.Add(new FDMDCFBudgetEstimation()
                                        {
                                            ID = DesignationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            DIV_CODE = dr["DIV_CODE"].ToString(),
                                            DIV_NAME = dr["DIV_NAME"].ToString(),
                                            Budget_Head = dr["BudgetHead"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString()),
                                            FofficeEstAmount = Convert.ToDecimal(Amount)
                                        });

                                }
                                if (ds != null && ds.Tables[2].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[2].Rows)
                                        ModelList1.Add(new FDMDCFBudgetEstimation()
                                        {
                                            ID = DesignationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            DIV_CODE = dr["DIV_CODE"].ToString(),
                                            DIV_NAME = dr["DIV_NAME"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString())

                                        });

                                }

                                break;
                            default:
                                ModelList.FirstOrDefault().ID = 0;

                                break;

                        }

                    }


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }

            return Json(new { data = ModelList, data1 = ModelList1, data2 = DesignationID }, JsonRequestBehavior.AllowGet);


        }


        public JsonResult GetBudgetByUserDesignation(string financialYear, string Code, int designationID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<FDMDCFBudgetEstimation> ModelList = new List<FDMDCFBudgetEstimation>();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    if (Session["DesignationId"] != null)
                    {
                        //DesignationID = Convert.ToInt32(Session["DesignationId"].ToString());
                        switch (designationID)
                        {
                            case 1:
                            case 2:

                                DataTable dt = _objModel.GetBudgetByDesignation(Convert.ToInt32(financialYear), "HOF", "");

                                if (dt != null && dt.Rows.Count > 0)
                                {

                                    foreach (DataRow dr in dt.Rows)
                                        ModelList.Add(new FDMDCFBudgetEstimation()
                                        {

                                            ID = designationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            CIRCLE_CODE = dr["CIRCLE_CODE"].ToString(),
                                            CIRCLE_NAME = dr["CIRCLE_NAME"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString())


                                        });



                                }
                                break;

                            case 4:

                                dt = _objModel.GetBudgetByDesignation(Convert.ToInt32(financialYear), "CCF", Code);

                                if (dt != null && dt.Rows.Count > 0)
                                {

                                    foreach (DataRow dr in dt.Rows)
                                        ModelList.Add(new FDMDCFBudgetEstimation()
                                        {

                                            ID = designationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            DIV_CODE = dr["DIV_CODE"].ToString(),
                                            DIV_NAME = dr["DIV_NAME"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString())


                                        });



                                }
                                break;

                            case 6:
                            case 7:

                                dt = _objModel.GetBudgetByDesignation(Convert.ToInt32(financialYear), "DCF", Code);

                                if (dt != null && dt.Rows.Count > 0)
                                {

                                    foreach (DataRow dr in dt.Rows)
                                        ModelList.Add(new FDMDCFBudgetEstimation()
                                        {

                                            ID = designationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            RANGE_CODE = dr["RANGE_CODE"].ToString(),
                                            RANGE_NAME = dr["RANGE_NAME"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString()),
                                            BudgetRowID = Convert.ToInt64(dr["ID"].ToString())


                                        });



                                }
                                break;
                            case 8:


                                dt = _objModel.GetBudgetByDesignation(Convert.ToInt32(financialYear), "RANGER", Code);

                                if (dt != null && dt.Rows.Count > 0)
                                {

                                    foreach (DataRow dr in dt.Rows)
                                        ModelList.Add(new FDMDCFBudgetEstimation()
                                        {

                                            ID = designationID,
                                            Index = Convert.ToInt64(dr["Index"].ToString()),
                                            Vill_NAME = dr["VILL_NAME"].ToString(),
                                            Vill_CODE = dr["VILL_CODE"].ToString(),
                                            ApprovedAmount = Convert.ToDecimal(dr["Amount"].ToString()),
                                            BudgetRowID = Convert.ToInt64(dr["ID"].ToString())


                                        });



                                }
                                break;
                        }
                        return Json(ModelList, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return Json(ModelList, JsonRequestBehavior.AllowGet);


        }

        public ActionResult SaveDCFBudgetData(FormCollection fCollection)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            FDMDCFBudgetEstimation _obj = null;
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    string[] Villcode = fCollection["hdVillCode"].Split(char.Parse(","));
                    string[] Bdgcode = fCollection["hdBudgetRowID"].Split(char.Parse(","));
                    string[] estimatedAmt = fCollection["hdEstimatedAmount"].Split(char.Parse(","));

                    _obj = new FDMDCFBudgetEstimation();

                    DataTable table = new DataTable();


                    table.Columns.Add("Vill_CODE", typeof(string));
                    table.Columns.Add("APPROVEDAMOUT", typeof(decimal));
                    table.Columns.Add("BudgetRowID", typeof(int));


                    for (var i = 0; i < Villcode.Length; i++)
                    {
                        DataRow dr = table.NewRow();

                        dr["Vill_CODE"] = Villcode[i];
                        dr["BudgetRowID"] = Bdgcode[i];
                        dr["APPROVEDAMOUT"] = estimatedAmt[i];
                        table.Rows.Add(dr);

                    }

                    if (Session["DesignationId"] != null)
                    {
                        DesignationID = Convert.ToInt32(Session["DesignationId"].ToString());
                        switch (DesignationID)
                        {
                            case 4:
                                RegionType = "CIRCLE"; break;
                            case 6:
                            case 7:
                                RegionType = "DIVISION"; break;
                        }
                    }
                    Int64 id = _objModel.SaveDCFBudgetEstimationData(table, Convert.ToInt64(Session["UserID"]), Convert.ToInt32(fCollection["ddlFinancialYear"].ToString()), fCollection["hdCode"], Convert.ToDecimal(fCollection["hdLevelAmount"]), RegionType);

                    if (id == 1)
                    {
                        TempData["ViewSuccessMessage"] = "Record Updated Successfully!!";
                        return RedirectToAction("FDMBudgetEsimationUpper", "FdmBudgetEstimation");
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

        /// <summary>
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetBudgetServeyUpperDetails(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SurveyBudget> SurveyList = new List<SurveyBudget>();

            try
            {
                if (Session["UserID"] != null)
                {
                    SurveyBudget _obj = new SurveyBudget();
                    DataSet ds = _objModel.GetBudgetServeyRecords(Convert.ToInt64(ID), "UpperLevel");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            SurveyList.Add(
                                new SurveyBudget()

                                {

                                    SDate = dr["SurveyDate"].ToString(),
                                    EstimatedAmt = Convert.ToDecimal(dr["EstimatedAmt"].ToString()),
                                    Project = dr["Project_Name"].ToString(),
                                    SchemeName = dr["Scheme_Name"].ToString(),
                                    ModelName = dr["Model_Name"].ToString(),
                                    Activity = dr["Activity_Name"].ToString(),
                                    BudgetHead = dr["BudgetHead"].ToString()

                                });

                        //}


                        return Json(SurveyList, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        public ActionResult AddBudgetForestOffice()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> CircleCode = new List<SelectListItem>();
            List<SelectListItem> DivisionName = new List<SelectListItem>();
            FdmScheme AS = new FdmScheme();
            List<SelectListItem> budgetheadlst = new List<SelectListItem>();
            List<SelectListItem> items = new List<SelectListItem>();

            DataSet dsBudgetHead = new DataSet();
            try
            {
                if (Session["UserID"] != null)
                {

                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    _objModel.SSOID = Session["SSOid"].ToString();

                    DataTable dt = _obj.BindFinancialYear();

                    ViewBag.fname1 = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                    {
                        FinancialYear.Add(new SelectListItem { Text = @dr["FinancialYear"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.ddlFinancialYear = FinancialYear;

                    DataTable dt1 = new FdmBudgetFOfficeEstimation().BindCircle();
                    ViewBag.fname = dt1;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
                    }
                    ViewBag.CircleCode1 = items;


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

                    dsBudgetHead = AS.BindBudget();
                    foreach (System.Data.DataRow dr in dsBudgetHead.Tables[0].Rows)
                    {
                        budgetheadlst.Add(new SelectListItem { Text = @dr["BudgetHead"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.Budget = budgetheadlst;

                    DataTable dtRange = new Common().Select_Range(_objModel.UserID);
                    foreach (DataRow dr in dtRange.Rows)
                    {
                        RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.ddlRange = RangeName;

                    return View("AddBudgetForestOffice", _objModel);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

        /// <summary>
        /// SaveBudget
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="FCollection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveFOfficeBudget(FormCollection FCollection)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string Code = string.Empty;
            string StateCode = "ST001";
            try
            {
                if (Session["UserID"] != null)
                {

                    _objFOfficeModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    _objFOfficeModel.FinancialYear = string.IsNullOrEmpty(FCollection["ddlFinancialYear"].ToString()) ? 0 : Convert.ToInt64(FCollection["ddlFinancialYear"].ToString());
                    if (Session["DesignationId"] != null)
                    {
                        _objFOfficeModel.Designation = Convert.ToInt32(Session["DesignationId"].ToString());
                        switch (_objFOfficeModel.Designation)
                        {
                            case 1:
                            case 2:
                                _objFOfficeModel.CODE = StateCode;
                                _objFOfficeModel.Region_Type = "STATE";
                                // _objFOfficeModel.CODE = string.IsNullOrEmpty(FCollection["CircleCode1"].ToString()) ? "" : FCollection["CircleCode1"].ToString();
                                break;
                            case 4: _objFOfficeModel.CODE = string.IsNullOrEmpty(FCollection["CircleCode"].ToString()) ? "" : FCollection["CircleCode"].ToString();
                                _objFOfficeModel.Region_Type = "CIRCLE";
                                break;
                            case 6:
                            case 7: _objFOfficeModel.CODE = string.IsNullOrEmpty(FCollection["ddlDivision"].ToString()) ? "" : FCollection["ddlDivision"].ToString();
                                _objFOfficeModel.Region_Type = "DIVISION";
                                break;

                            case 8: _objFOfficeModel.CODE = string.IsNullOrEmpty(FCollection["ddlRange"].ToString()) ? "" : FCollection["ddlRange"].ToString();
                                _objFOfficeModel.Region_Type = "RANGE";
                                break;
                            default: break;
                        }
                    }

                    _objFOfficeModel.EstimatedAmount = Convert.ToDecimal(FCollection["EstimatedBudget"].ToString());
                    _objFOfficeModel.BudgetHead = Convert.ToInt64(FCollection["DropBudgethead"].ToString());
                    _objFOfficeModel.Action = "SaveValue";

                    Int64 id = new FdmBudgetFOfficeEstimation().SubmitFOfficeBudget(_objFOfficeModel);
                    if (id > 0)
                    {
                        TempData["ViewSuccessMessage"] = "Record Save Successfully!!";
                        return RedirectToAction("AddBudgetForestOffice", "FdmBudgetEstimation");

                    }
                    else
                    {

                        return View("Error");
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
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetBudgetFOfficeDetails(string Code, string Fyear, string BdgHead)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    _objFOfficeModel.CODE = Code;
                    _objFOfficeModel.FinancialYear = Convert.ToInt64(Fyear);
                    _objFOfficeModel.BudgetHead = Convert.ToInt64(BdgHead);
                    _objFOfficeModel.Designation = Convert.ToInt32(Session["DesignationId"].ToString());
                    _objFOfficeModel.Action = "GetValue";

                    DataSet ds = new FdmBudgetFOfficeEstimation().GetBudgetFOfficeRecords(_objFOfficeModel);
                    if (ds != null)
                    {
                        string estAmt = ds.Tables[0].Rows[0]["EstimatedAmt"].ToString();
                        return Json(estAmt, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetSurveyLevelDetails(string Code, string Design)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SurveyBudget> SurveyList = new List<SurveyBudget>();

            try
            {
                if (Session["UserID"] != null)
                {
                    SurveyBudget _obj = new SurveyBudget();
                    DataSet ds = _objModel.GetServeyRecords(Code, Design);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            SurveyList.Add(
                                new SurveyBudget()

                                {


                                    EstimatedAmt = Convert.ToDecimal(dr["Proposal Budget"].ToString()),
                                    //Project = dr["Project Name"].ToString(),
                                    SchemeName = dr["Scheme Name"].ToString(),
                                    ModelName = dr["Model Name"].ToString(),
                                    Activity = dr["Activity Name"].ToString(),
                                    BudgetHead = dr["Budget Head"].ToString()

                                });

                        //}


                        return Json(SurveyList, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        public ActionResult ExportData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string Code = string.Empty;

            try
            {
                if (Session["UserID"] != null)
                {
                    if (Session["DesignationId"] != null)
                    {
                        DesignationID = Convert.ToInt32(Session["DesignationId"].ToString());
                        _objModel.SSOID = Session["SSOid"].ToString();
                        switch (DesignationID)
                        {
                            case 4:
                                Code = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "CCF").Rows[0]["CIRCLE_CODE"].ToString(); break;

                            case 6:
                            case 7:
                                Code = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "DCF/DFO").Rows[0]["DIV_CODE"].ToString(); break;
                            case 8:
                                Code = new FdmBudgetEstimation().Officedetails(_objModel.SSOID, "RANGER").Rows[0]["RANGE_CODE"].ToString(); break;

                        }
                    }



                    DataSet ds = new FdmBudgetFOfficeEstimation().ExportDataRecords(Code, Convert.ToInt32(Session["DesignationId"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        GridView gv = new GridView();
                        gv.DataSource = ds.Tables[0];
                        gv.DataBind();
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment; filename=" + "Excel" + "_" + DateTime.Now.ToString("ddMMYYYY") + ".xls");
                        Response.ContentType = "application/ms-excel";
                        Response.Charset = "";
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter htw = new HtmlTextWriter(sw);
                        gv.RenderControl(htw);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
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
