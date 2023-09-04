//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Work order, work order milestone, work order progress
//  Date Created : 24-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Durgesh N Sharma
//  Modified By  : Durgesh N Sharma  
//  Modified On  : 10-Mar-2016
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@



using System;
using System.Collections.Generic;
using System.Linq;
using FMDSS.Models;
using FMDSS.Models.ForestDevelopment;
using FMDSS.Models.ForesterDevelopment;
using FMDSS.Models.ForestProduction;
using FMDSS.Models.CitizenService.PermissionServices;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using FMDSS.Models.Admin;
using System.Data;
using FMDSS.GenericClass;
using System.IO;
using FMDSS.Filters;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;

namespace FMDSS.Controllers.ForesterDevelopment
{
    [MyAuthorization]
    public class WorkOrderController : Controller
    {
        //surve
        Int64 UserID;//= Convert.ToInt64(Session["UserId"]);
        Location _objLocation = new Location();
        Workorder _obj = new Workorder();
        List<SelectListItem> District = new List<SelectListItem>();
        List<Workorder> WOList = new List<Workorder>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        List<SelectListItem> ddlProduceType = new List<SelectListItem>();
        List<SelectListItem> ddlProduct = new List<SelectListItem>();
        List<SelectListItem> Division = new List<SelectListItem>();
        List<SelectListItem> ddlIFMSWorkOrder = new List<SelectListItem>();
        List<SelectListItem> ddlBudgetHead = new List<SelectListItem>();
        List<SelectListItem> WorkOrderList = new List<SelectListItem>();
        List<SelectListItem> ddlModel = new List<SelectListItem>();
        List<SelectListItem> ddlActivity = new List<SelectListItem>();
        List<SelectListItem> ddlsubActivity = new List<SelectListItem>();
        List<SelectListItem> RangeName = new List<SelectListItem>();
        List<SelectListItem> ddlAssetType = new List<SelectListItem>();
        List<SelectListItem> ddlAsset = new List<SelectListItem>();
        List<SelectListItem> JFMCorContractAgency = new List<SelectListItem>();
        // GET: /WorkOrder/

        public ActionResult Index()
        {
            try
            {
                _obj.UserID = Convert.ToInt64(Session["UserId"]);
                DataTable dtf = _obj.Select_WorkOrder();

                foreach (DataRow dr in dtf.Rows)
                {
                    WOList.Add(new Workorder()
                    {
                        RowID = dr["RowID"].ToString(),
                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        WorkOrder_Code = dr["WorkOrder_Code"].ToString(),
                        WorkOrder_Name = dr["WorkOrder_Name"].ToString(),
                        IFMC_WorkOrder_Code = dr["IFMC_WorkOrder_Code"].ToString(),
                        AdminApprovedOrderNo = dr["AdminApprovedOrderNo"].ToString(),
                        FinanceApprovedOrderNo = dr["FinanceApprovedOrderNo"].ToString(),
                        ContractAgencyType = dr["ContractAgencyType"].ToString(),
                        WorkOrderType = dr["WorkOrderType"].ToString(),
                        EnteredOn = dr["EnteredOn"].ToString(),
                        StatusDesc = dr["StatusDesc"].ToString()
                    });
                }
                //return dtf;
                return View("index", WOList);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        //
        // GET: /WorkOrder/Details/5

        public ActionResult Details(int id)
        {

            return View();
        }
        //Milestone entry for workorder added for defect log id 246 and 257 solved by Durgesh N Sharma
        [HttpPost]
        public ActionResult MilestoneEntry(WorkOrderMilestone woMile)
        {
            try
            {

                DataTable dtMiles = new DataTable("Table");
                //  dtMiles.Columns.Add("WorkOrderID", typeof(String));
                dtMiles.Columns.Add("MilestoneID", typeof(String));
                dtMiles.Columns.Add("ActivityID", typeof(String));
                dtMiles.Columns.Add("SubActivityID", typeof(String));
                dtMiles.Columns.Add("PercentageActivitycompletion", typeof(String));
                dtMiles.AcceptChanges();
                List<MilestoneActivity> ma = new List<MilestoneActivity>();
                if (Session["DTWOMA"] != null)
                {
                    ma = (List<MilestoneActivity>)Session["DTWOMA"];
                }
                foreach (MilestoneActivity objma in ma)
                {
                    DataRow dr = dtMiles.NewRow();
                    //  dr["WorkOrderID"] = objma.WorkorderID;
                    dr["MilestoneID"] = objma.MilestoneID;
                    dr["ActivityID"] = objma.ActivityID;
                    dr["SubActivityID"] = objma.SubActivityID;
                    dr["PercentageActivitycompletion"] = objma.PercentageActivitycompletion;
                    dtMiles.Rows.Add(dr);
                    dtMiles.AcceptChanges();
                }
                _obj.SubmitMilestone(woMile.WorkOrderID, dtMiles);
                return RedirectToAction("MilestoneEntry");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }

        }
        //Milestone entry for workorder added for defect log id 246 and 257 solved by Durgesh N Sharma
        public ActionResult MilestoneEntry(string WorkOrderID, string WorkOrderName)
        {
            WorkOrderID = Encryption.decrypt(WorkOrderID);
            WorkOrderName = Encryption.decrypt(WorkOrderName);
            try
            {
                if (string.IsNullOrEmpty(WorkOrderID))
                    return RedirectToAction("Index");
                else
                {
                    Session["WorkOrderID"] = WorkOrderID;
                    WorkOrderMilestone womil = new WorkOrderMilestone();
                    womil.WorkOrderID = Session["WorkOrderID"].ToString();
                    Session["DTWOMA"] = null;
                    Session["dtWOMilestone"] = null;

                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();
                    _obj = new Workorder();
                    DataTable dt = new FixedLandUsage().Division();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        Division.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }
                    GetWorkOrderMilestone(Session["WorkOrderID"].ToString());
                    GetMilestoneActivity(Session["WorkOrderID"].ToString());
                    ViewBag.ddlDivision1 = Division;
                    ViewBag.ddlDistrict1 = District;
                    ViewBag.ddlBlockName1 = BlockName;
                    ViewBag.ddlGPName1 = GPName;
                    ViewBag.ddlVillName1 = VillageName;
                    ViewBag.ddlProduceType1 = ddlProduceType;
                    ViewBag.ddlProduct1 = ddlProduct;
                    ViewBag.WorkOrderList1 = WorkOrderList;
                    ViewBag.ddlModel1 = ddlModel;
                    ViewBag.ddlActivity1 = ddlActivity;
                    ViewBag.ddlsubActivity1 = ddlsubActivity;
                    // ViewBag.WorkOrderID = Session["WorkOrderID"].ToString();
                    ViewData["WorkOrderName"] = WorkOrderName;
                    return View();//"MilestoneEntry",womil);
                }
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        //
        // GET: /WorkOrder/Create
        /// <summary>
        /// to return view for add and edit purpose
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Create(string WorkOrderID)
        {

            string ID = Encryption.decrypt(WorkOrderID);
            try
            {
                Session["DTWOMP"] = null;
                Session["WorkOrderID"] = ID;
                Session["dtWOMilestone"] = null;
                UserID = Convert.ToInt64(Session["UserID"].ToString());
                List<SelectListItem> items = new List<SelectListItem>();
                _obj = new Workorder();
                DataTable dt = _objLocation.District();
                //ViewBag.fname = dt;
                //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                //{
                //    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                //} 
                dt = new FixedLandUsage().Division();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Division.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                }
                DataTable dtRange = new Common().Select_Range(_obj.UserID);
                foreach (DataRow dr in dtRange.Rows)
                {
                    RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.ddlRange = RangeName;

                List<IFMSWorkOrder> lst = GetIFMSWorkorder("19329"); // _obj.GetActiveIFMSWorkOrder();
                ViewBag.fname = lst;
                foreach (IFMSWorkOrder wo in lst)
                {
                    ddlIFMSWorkOrder.Add(new SelectListItem { Text = wo.WorkOrderName, Value = wo.WorkOrderId });
                }
                if (!string.IsNullOrEmpty(ID))
                {
                    // GetWorkOrderMilestone
                    GenericClasses<Workorder> genMP = new GenericClasses<Workorder>();
                    GenericClasses<WorkOrderMicroplan> genWOMP = new GenericClasses<WorkOrderMicroplan>();
                    GenericClasses<WorkOrderMilestone> genwoMilestone = new GenericClasses<WorkOrderMilestone>();
                    DataTable dtf = _obj.Select_WorkOrder(ID);

                    // dt = _obj.SelectBudgetHeadByProject();
                    //ViewBag.fname = dt;
                    //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    //{
                    //    ddlBudgetHead.Add(new SelectListItem { Text = @dr["ID"].ToString(), Value = @dr["BudgetHead"].ToString() });
                    //}
                    ddlBudgetHead.Add(new SelectListItem { Text = dtf.Rows[0]["BudgetHead"].ToString(), Value = dtf.Rows[0]["BudgetHead"].ToString() });
                    _obj = genMP.model(dtf);

                    if (Convert.ToString(dtf.Rows[0]["ContractAgencyType"]) == "JFMC")
                    {
                        dt = new MicroPlan().GetJFMCbyDivCode(Convert.ToString(dtf.Rows[0]["Div_CODE"]));
                        ViewBag.fname = dt;
                        foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                        {
                            JFMCorContractAgency.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["ID"].ToString() });
                        }
                    }
                    else
                    {
                        List<IFMSVendor> lstv = GetIFMSVendors("19329");
                        foreach (IFMSVendor v in lstv)
                        {
                            JFMCorContractAgency.Add(new SelectListItem { Text = v.name, Value = v.VendorCode });
                        }
                    }
                    Session["DTWOMP"] = genWOMP.lstmodel(_obj.getWorkOrderMicroplan(ID));


                    // Session["DTWOMilestone"] = genwoMilestone.lstmodel(_obj.getWorkOrderMilestone(ID));

                }
                ViewBag.ddlDivision1 = Division; ViewBag.JFMCorContractAgency1 = JFMCorContractAgency;
                ViewBag.ddlDistrict1 = District;
                ViewBag.ddlBlockName1 = BlockName;
                ViewBag.ddlGPName1 = GPName;
                ViewBag.ddlVillName1 = VillageName;
                ViewBag.ddlIFMSWorkOrder1 = ddlIFMSWorkOrder;
                ViewBag.ddlBudgetHead1 = ddlBudgetHead;

                return View("Create", _obj);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        /// <summary>
        ///to return view for work order progress grid data
        /// </summary>
        /// <returns></returns>
        public ActionResult ProgressIndex()
        {
            try
            {
                _obj.UserID = Convert.ToInt64(Session["UserId"]);
                DataTable dtf = _obj.Select_WorkOrderProgress();

                foreach (DataRow dr in dtf.Rows)
                {
                    WOList.Add(new Workorder()
                    {
                        RowID = dr["RowID"].ToString(),
                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        WorkOrder_Name = dr["WorkOrder_Name"].ToString(),
                        WorkOrderType = dr["WorkOrderType"].ToString(),
                        FieldName = dr["FieldName"].ToString(),
                        Model_Name = dr["Model_Name"].ToString(),
                        Activity_Name = dr["Activity_Name"].ToString(),
                        Sub_Activity_Name = dr["Sub_Activity_Name"].ToString(),
                        EnteredOn = dr["EnteredOn"].ToString(),
                        ProgressStatus = dr["ProgressStatus"].ToString(),
                        ProgressImage = dr["ProgressImage"].ToString()
                    });
                }
                //return dtf;
                return View("Progressindex", WOList);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            //return View();
        }
        /// <summary>
        /// to return view for add or edit work order progress
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ProgressEntry(string WPID)
        {
            string ID = Encryption.decrypt(WPID);
            try
            {
                UserID = Convert.ToInt64(Session["UserID"].ToString());
                List<SelectListItem> items = new List<SelectListItem>();
                _obj = new Workorder();
                DataTable dt = new FixedLandUsage().Division();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Division.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                }
                //dt = _objLocation.District();
                //ViewBag.fname = dt;
                //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                //{
                //    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                //}
                dt = _obj.SelectProduceType();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    ddlProduceType.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
                }
                dt = _obj.SelectAssetCategoryType();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    ddlAssetType.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["AssetCategoryID"].ToString() });
                }
                DataTable dtRange = new Common().Select_Range(_obj.UserID);
                foreach (DataRow dr in dtRange.Rows)
                {
                    RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.ddlRange = RangeName;
                if (!string.IsNullOrEmpty(ID))
                {
                    GenericClasses<Workorder> genMP = new GenericClasses<Workorder>();
                    DataTable dtf = _obj.Select_WorkOrderProgress(ID);

                    dt = _obj.SelectWorkOrderByDivisionCode(Convert.ToString(dtf.Rows[0]["Div_Code"]));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        WorkOrderList.Add(new SelectListItem { Text = @dr["WorkOrder_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    //dt = _obj.SelectModelByWorkOrderID(Convert.ToString(dtf.Rows[0]["WorkOrderID"]));
                    //ViewBag.fname = dt;
                    //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    //{
                    //    ddlModel.Add(new SelectListItem { Text = @dr["Model_Name"].ToString(), Value = @dr["ID"].ToString() });
                    //}
                    dt = _obj.SelectSurveyByWorkOrderID(Convert.ToString(dtf.Rows[0]["WorkOrderID"]));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        ddlModel.Add(new SelectListItem { Text = @dr["AreaName"].ToString(), Value = @dr["SurveyID"].ToString() });
                    }
                    dt = _obj.SelectActivityBySurveyID(Convert.ToString(dtf.Rows[0]["WorkOrderID"]), Convert.ToString(dtf.Rows[0]["SurveyID"]));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        ddlActivity.Add(new SelectListItem { Text = @dr["Activity_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    dt = _obj.SelectSubActivityByActivityID(Convert.ToString(dtf.Rows[0]["Activity"]));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        ddlsubActivity.Add(new SelectListItem { Text = @dr["Sub_Activity_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    _obj = genMP.model(dtf);
                }
                ViewBag.ddlDivision1 = Division;
                ViewBag.ddlDistrict1 = District;
                ViewBag.ddlBlockName1 = BlockName;
                ViewBag.ddlGPName1 = GPName;
                ViewBag.ddlVillName1 = VillageName;
                ViewBag.ddlProduceType1 = ddlProduceType;
                ViewBag.ddlProduct1 = ddlProduct;
                ViewBag.ddlAssetType1 = ddlAssetType;
                ViewBag.ddlAsset1 = ddlAsset;
                ViewBag.WorkOrderList1 = WorkOrderList;
                ViewBag.ddlModel1 = ddlModel;
                ViewBag.ddlActivity1 = ddlActivity;
                ViewBag.ddlsubActivity1 = ddlsubActivity;
                return View("ProgressEntry", _obj);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        //
        // POST: /WorkOrder/Create
        /// <summary>
        /// to save data of work order progress
        /// </summary>
        /// <param name="CurrentStatusUrl"></param>
        /// <param name="objWo"></param>
        /// <param name="frm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProgressEntry(HttpPostedFileBase CurrentStatusUrl, Workorder objWo, FormCollection frm)
        {
            try
            {
                string CurrentStatusFile = string.Empty;
                var path = "";
                objWo.IsActive = true;
                objWo.Status = 1;
                //if (CurrentStatusUrl != null && CurrentStatusUrl.ContentLength > 0)
                //{
                //    CurrentStatusFile = Path.GetFileName(CurrentStatusUrl.FileName);
                //    String FileFullName = "WOP_"+DateTime.Now.Ticks + "_" + CurrentStatusUrl;
                //    path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                //    //fs.IDProofUrl = path;
                //    objWo.ProgressImage = @"../PermissionDocument/" + FileFullName.Trim();
                //    CurrentStatusUrl.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                //}
                ////if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                ////{

                ////    CurrentStatusFile = Path.GetFileName(Request.Files[0].FileName);



                ////    String FileFullName = "WOP_" + DateTime.Now.Ticks + "_" + CurrentStatusFile;
                ////    path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                ////    objWo.ProgressImage = @"../PermissionDocument/" + FileFullName.Trim();
                ////    Request.Files[0].SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);

                ////}
                ////else
                ////{ objWo.ProgressImage = ""; }
                objWo.BillVoucherDate = "01/01/2016";
                if (!string.IsNullOrEmpty(frm["MilestoneSelected"]))
                    objWo.CompletedMilestoneActivity = Convert.ToString(frm["MilestoneSelected"]);
                else
                    objWo.CompletedMilestoneActivity = "0";
                objWo.SubmitWorkOrderProgress(objWo);
                return RedirectToAction("ProgressIndex");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        /// <summary>
        /// to save work order in DB
        /// </summary>
        /// <param name="objWO"></param>
        /// <param name="frm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Workorder objWO, FormCollection frm)
        {
            try
            {
                string woname = objWO.WorkOrder_Name;
                bool sendtomilestone = false;//
                if (objWO.ID == 0) sendtomilestone = true;// ? 1 : 0;
                string CurrentStatusFile = string.Empty;
                var path = "";
                // TODO: Add insert logic here
                // Workorder objWO = new Workorder();

                // objWO.ID = Convert.ToInt64(frm["hdn_id"].ToString());
                /*objWO.Project_Name = frm["ProjectName"].ToString();
                objWO.Scheme_Id = Convert.ToInt64(frm["Dropscheme"].ToString());
                objWO.Model_Code = frm["Dropmodel"].ToString();
                objWO.AreaofRolloutinSQKM = Convert.ToDecimal(frm["ArRollout"]);
                objWO.ReferenceNo = frm["RefNodoc"].ToString();*/
                objWO.StartDate = frm["StartDate"];
                objWO.EndDate = frm["EndDate"];
                objWO.AdminApprovedDate = "01/01/2016";
                objWO.FinanceApprovedDate = "01/01/2016";
                objWO.Status = 1;
                objWO.IsActive = true;

                DataTable dtwomp = new DataTable("Table");
                dtwomp.Columns.Add("WorkorderID", typeof(String));
                dtwomp.Columns.Add("Village_Code", typeof(String));
                dtwomp.Columns.Add("MicroPlanID", typeof(String));
                dtwomp.Columns.Add("ProjectID", typeof(String));
                dtwomp.Columns.Add("ModelIDs", typeof(String));
                dtwomp.Columns.Add("ActivityIDs", typeof(String));
                dtwomp.AcceptChanges();
                List<WorkOrderMicroplan> womp = new List<WorkOrderMicroplan>();
                if (Session["DTWOMP"] != null)
                {
                    womp = (List<WorkOrderMicroplan>)Session["DTWOMP"];
                }
                foreach (WorkOrderMicroplan objwomp in womp)
                {
                    DataRow dr = dtwomp.NewRow();
                    dr["WorkorderID"] = objwomp.WorkorderID;
                    dr["Village_Code"] = objwomp.Village_Code;
                    dr["MicroPlanID"] = objwomp.MicroPlanID;
                    dr["ProjectID"] = objwomp.ProjectID;
                    dr["ModelIDs"] = objwomp.ModelIDs;
                    dr["ActivityIDs"] = objwomp.ActivityIDs;
                    dtwomp.Rows.Add(dr);
                    dtwomp.AcceptChanges();
                }

                DataTable dtWOMilestone = new DataTable("Table");
                //  dtWOMilestone.Columns.Add("WorkorderID", typeof(String));
                dtWOMilestone.Columns.Add("MilestoneName", typeof(String));
                dtWOMilestone.Columns.Add("MilestonePaymentPercentage", typeof(Int32));
                dtwomp.AcceptChanges();
                List<WorkOrderMilestone> WOMilestone = new List<WorkOrderMilestone>();
                if (Session["DTWOMilestone"] != null)
                {
                    WOMilestone = (List<WorkOrderMilestone>)Session["DTWOMilestone"];
                }
                foreach (WorkOrderMilestone objwomp in WOMilestone)
                {
                    DataRow dr = dtWOMilestone.NewRow();
                    //  dr["WorkorderID"] = objwomp.WorkorderID;
                    dr["MilestoneName"] = objwomp.MilestoneName;
                    dr["MilestonePaymentPercentage"] = objwomp.MilestonePaymentPercentage;
                    dtWOMilestone.Rows.Add(dr);
                    dtWOMilestone.AcceptChanges();
                }
                // Ref doc and ref no added for defect log id 244 solved by Durgesh N Sharma
                if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    CurrentStatusFile = Path.GetFileName(Request.Files[0].FileName);
                    String FileFullName = "WOP_" + DateTime.Now.Ticks + "_" + CurrentStatusFile;
                    path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    objWO.refDocument = @"../PermissionDocument/" + FileFullName.Trim();
                    Request.Files[0].SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);

                }
                //Milestone removed from here and made a new page for the same for defect log id 256 solved by Durgesh N Sharma-->
                Session["WorkOrderID"] = objWO.SubmitWorkOrder(objWO, dtwomp, dtWOMilestone);
                if (sendtomilestone)
                {
                    TempData["ViewMessage"] = "Work order saved successfully.please enter milestone dtails for this work order!!";
                    return RedirectToAction("MilestoneEntry", new { WorkOrderID = Encryption.encrypt(Session["WorkOrderID"].ToString()), WorkOrderName = Encryption.encrypt(woname) });
                }
                else
                    return RedirectToAction("Index");

            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        //
        // GET: /WorkOrder/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /WorkOrder/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /WorkOrder/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    _obj.ID = Convert.ToInt64(id);
                    _obj.UpdatedBy = Convert.ToInt64(Session["UserID"]);
                    Int64 i = _obj.DeleteWorkOrder();
                }
                //Int64 i = cst.DeleteMasterFeesEntry();
                return RedirectToAction("index", "WorkOrder");
                // return View();
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        //
        // GET: /WorkOrder/Delete/5

        public ActionResult DeleteProgressEntry(int id)
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    _obj.ID = Convert.ToInt64(id);
                    _obj.UpdatedBy = Convert.ToInt64(Session["UserID"]);
                    Int64 i = _obj.DeleteWorkOrderProgress();
                }
                //Int64 i = cst.DeleteMasterFeesEntry();
                return RedirectToAction("Progressindex", "WorkOrder");
                // return View();
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }


        //
        // POST: /WorkOrder/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        public JsonResult AddWorkOrderMicroplan(WorkOrderMicroplan WOMP)
        {
            List<WorkOrderMicroplan> lstWOMP = new List<WorkOrderMicroplan>();
            #region Comment
            /*
            DataTable dtMP;
            if (Session["DTWOMP"] == null)
            {
                dtMP = new DataTable("Table");
                dtMP.Columns.Add("rowID", typeof(String));
                dtMP.Columns.Add("Village_Code", typeof(String));
                dtMP.Columns.Add("Village_Name", typeof(String));
                dtMP.Columns.Add("MicroPlanID", typeof(String));
                dtMP.Columns.Add("MicroPlanName", typeof(String));
                dtMP.Columns.Add("ProjectID", typeof(String));
                dtMP.Columns.Add("ProjectName", typeof(String));
                dtMP.Columns.Add("ModelIDs", typeof(String));
                dtMP.Columns.Add("ModelName", typeof(String));
                dtMP.Columns.Add("ActivityIDs", typeof(String));
                dtMP.Columns.Add("ActivityName", typeof(String));
                dtMP.AcceptChanges();
            }
            else
                dtMP = (DataTable)Session["DTWOMP"];
            try 
            {
                DataRow dr = dtMP.NewRow();
                dr["rowID"] = Guid.NewGuid();
                dr["Village_Code"] = WOMP.Village_Code;
                dr["Village_Name"] = WOMP.Village_Name;
                dr["MicroPlanID"] = WOMP.MicroPlanID;
                dr["MicroPlanName"] = WOMP.MicroPlanName;
                dr["ProjectID"] = WOMP.ProjectID;
                dr["ProjectName"] = WOMP.ProjectName;
                dr["ModelIDs"] = WOMP.ModelIDs;
                dr["ModelName"] = WOMP.ModelName;
                dr["ActivityIDs"] = WOMP.ActivityIDs;
                dr["ActivityName"] = WOMP.ActivityName;
                dtMP.Rows.Add(dr);
                dtMP.AcceptChanges();
            }*/
            #endregion
            try
            {
                if (Session["DTWOMP"] != null)
                {
                    lstWOMP = (List<WorkOrderMicroplan>)Session["DTWOMP"];

                }
                WOMP.rowID = Guid.NewGuid().ToString();
                if (lstWOMP.Any(a => a.ProjectID == WOMP.ProjectID))
                    WOMP.projectArea = 0;
                else
                {
                    WOMP.projectArea = Convert.ToDouble(_obj.Select_ProjectArea(WOMP.ProjectID));// new Random().Next(1, 25);
                }

                if (lstWOMP.Any(a => a.ActivityIDs == WOMP.ActivityIDs && a.ModelIDs == WOMP.ModelIDs))
                    WOMP.ActivityCost = 0;
                else
                {
                    WOMP.ActivityCost = Convert.ToDouble(_obj.Select_Activity_BSRAmount(WOMP.ActivityIDs).Rows[0]["TotalAmount"]);
                }
                lstWOMP.Add(WOMP);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            finally { }
            Session["DTWOMP"] = lstWOMP;
            return Json(lstWOMP, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddWorkOrderMilestone(WorkOrderMilestone WOMilestone)
        {
            List<WorkOrderMilestone> lstWOMilestone = new List<WorkOrderMilestone>();
            GenericClasses<WorkOrderMilestone> genwoMilestone = new GenericClasses<WorkOrderMilestone>();
            try
            {
                lstWOMilestone = genwoMilestone.lstmodel(_obj.saveWorkOrderMilestone(WOMilestone));
                Session["DTWOMilestone"] = lstWOMilestone;

            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            finally { }
            // Session["DTWOMilestone"] = lstWOMilestone;
            return Json(lstWOMilestone, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkOrderMilestone(string WorkOrderID)
        {
            try
            {
                GenericClasses<WorkOrderMilestone> genwoMilestone = new GenericClasses<WorkOrderMilestone>();
                List<WorkOrderMilestone> lstWOMilestone = new List<WorkOrderMilestone>();
                lstWOMilestone = genwoMilestone.lstmodel(_obj.getWorkOrderMilestone(WorkOrderID));
                Session["DTWOMilestone"] = lstWOMilestone;
                return Json(lstWOMilestone, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        public JsonResult GetMilestoneActivity(string WorkOrderID)
        {
            try
            {
                GenericClasses<MilestoneActivity> genwoMilestone = new GenericClasses<MilestoneActivity>();
                List<MilestoneActivity> lstWOMilestone = new List<MilestoneActivity>();
                lstWOMilestone = genwoMilestone.lstmodel(_obj.GetMilestoneActivity(WorkOrderID));
                Session["DTWOMA"] = lstWOMilestone;
                return Json(lstWOMilestone, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        public JsonResult AddMilestoneActivity(MilestoneActivity WOMA)
        {
            List<MilestoneActivity> lstWOMA = new List<MilestoneActivity>();

            try
            {
                if (Session["DTWOMA"] != null)
                {
                    lstWOMA = (List<MilestoneActivity>)Session["DTWOMA"];

                }
                WOMA.rowID = Guid.NewGuid().ToString();
                lstWOMA.Add(WOMA);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            finally { }
            Session["DTWOMA"] = lstWOMA;
            return Json(lstWOMA, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteMilestoneActivity(string Id)
        {
            DataTable dtMP = new DataTable();
            List<MilestoneActivity> lstWOMA = new List<MilestoneActivity>();

            try
            {

                if (Session["DTWOMA"] != null)
                {
                    lstWOMA = (List<MilestoneActivity>)Session["DTWOMA"];
                    // DataRow dr = (DataRow)dtMP.Select("rowID=" + Id).First();
                    if (Id != "0" && Id.Length > 0)
                    {
                        MilestoneActivity wwmp = lstWOMA.Single(a => a.rowID == Id);
                        lstWOMA.Remove(wwmp);
                    }
                    // dtMP.Rows.Remove((DataRow)dtMP.Select("rowID=" + Id).First());
                    Session["DTWOMA"] = lstWOMA;
                }
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            return Json(lstWOMA, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSurveyDetailbyID(string SurveyId)
        {
            GenericClasses<WorkOrderSurvey> genSR = new GenericClasses<WorkOrderSurvey>();
            List<WorkOrderSurvey> lstSurveyReport = new List<WorkOrderSurvey>();

            try
            {
                lstSurveyReport = genSR.lstmodel(_obj.SelectSurvey(SurveyId));
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            finally { }
            return Json(lstSurveyReport, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_SubActivity_BSRAmount(string SubActivityID)
        {
            string stramt = "";


            try
            {
                stramt = _obj.Select_SubActivity_BSRAmount(SubActivityID).Rows[0]["TotalAmount"].ToString();

            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            finally { }
            return Json(stramt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_Activity_BSRAmount(string ActivityID)
        {
            string stramt = "";


            try
            {
                stramt = _obj.Select_Activity_BSRAmount(ActivityID).Rows[0]["TotalAmount"].ToString();

            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            finally { }
            return Json(stramt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteWorkOrderMicroplan(string Id)
        {
            DataTable dtMP = new DataTable();
            List<WorkOrderMicroplan> WOMP = new List<WorkOrderMicroplan>();

            try
            {

                if (Session["DTWOMP"] != null)
                {
                    WOMP = (List<WorkOrderMicroplan>)Session["DTWOMP"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        // DataRow dr = (DataRow)dtMP.Select("rowID=" + Id).First();
                        WorkOrderMicroplan wwmp = WOMP.Single(a => a.rowID == Id);
                        WOMP.Remove(wwmp);
                    }
                    // dtMP.Rows.Remove((DataRow)dtMP.Select("rowID=" + Id).First());
                    Session["DTWOMP"] = WOMP;
                }
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            return Json(WOMP, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteWorkOrderMilestone(string Id)
        {
            string WorkOrderID = Session["WorkOrderID"].ToString();
            DataTable dtMP = new DataTable();
            List<WorkOrderMilestone> lstWOMilestone = new List<WorkOrderMilestone>();
            GenericClasses<WorkOrderMilestone> genwoMilestone = new GenericClasses<WorkOrderMilestone>();
            try
            {
                if (Id != "0" && Id.Length > 0)
                    lstWOMilestone = genwoMilestone.lstmodel(_obj.DeleteWorkOrderMilestone(Id, WorkOrderID));
                else
                    lstWOMilestone = (List<WorkOrderMilestone>)Session["DTWOMilestone"];
                Session["DTWOMilestone"] = lstWOMilestone;
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            return Json(lstWOMilestone, JsonRequestBehavior.AllowGet);
        }
        // Show mile stones  while complete a activity and select mile stone added for defect log id 260 solved by Durgesh N Sharma-->
        public JsonResult SelectMilestoneByActivitySubActivityID(string WorkOrderID, string ActivityID, string SubActivityID)
        {
            DataTable dtMP = new DataTable();
            List<WorkOrderMilestone> lstWOMilestone = new List<WorkOrderMilestone>();
            GenericClasses<WorkOrderMilestone> genwomile = new GenericClasses<WorkOrderMilestone>();
            try
            {
                lstWOMilestone = genwomile.lstmodel(_obj.SelectMilestoneByActivitySubActivityID(WorkOrderID, ActivityID, SubActivityID));
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
            return Json(lstWOMilestone, JsonRequestBehavior.AllowGet);
        }
        # region Combofilling
        [HttpPost]
        public JsonResult SelectMicroPlanByVilageCode(string Village_Code)
        {
            try
            {
                DataTable dt = _obj.SelectMicroPlanByVilageCode(Village_Code);
                return dtToViewBagJSON(dt, "MicroPlanName", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        [HttpPost]
        public JsonResult SelectWorkOrderByMicroPlanID(string MicroPlanID)
        {
            try
            {
                DataTable dt = _obj.SelectWorkOrderByMicroPlanID(MicroPlanID);
                return dtToViewBagJSON(dt, "WorkOrder_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        [HttpPost]
        public JsonResult SelectWorkOrderByDivisionCode(string Division)
        {
            try
            {
                DataTable dt = _obj.SelectWorkOrderByDivisionCode(Division);
                return dtToViewBagJSON(dt, "WorkOrder_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        [HttpPost]
        public JsonResult SelectProjectByMicroplan(string MicroplanID)
        {
            try
            {
                DataTable dt = _obj.SelectProjectByMicroplan(MicroplanID);
                return dtToViewBagJSON(dt, "Project_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        [HttpPost]
        public JsonResult SelectBudgetHeadByProject()
        {
            try
            {
                string ProjectIDs = "0";
                List<WorkOrderMicroplan> WOMP = new List<WorkOrderMicroplan>();
                if (Session["DTWOMP"] != null)
                {
                    WOMP = (List<WorkOrderMicroplan>)Session["DTWOMP"];
                    foreach (WorkOrderMicroplan ww in WOMP)
                        ProjectIDs += "," + ww.ProjectID.ToString();
                }
                DataTable dt = _obj.SelectBudgetHeadByProject(ProjectIDs);
                return dtToViewBagJSON(dt, "BudgetHead", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        [HttpPost]
        public JsonResult SelectModelByProjectID(string ProjectID)
        {
            try
            {
                DataTable dt = _obj.SelectModelByProjectID(ProjectID);
                return dtToViewBagJSON(dt, "Model_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        [HttpPost]
        public JsonResult SelectModelByWorkOrderID(string WorkOrderID)
        {
            try
            {
                DataTable dt = _obj.SelectModelByWorkOrderID(WorkOrderID);
                return dtToViewBagJSON(dt, "Model_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        //Survey details(Date of survey,Photo survey,lat and long,Area of survey, etc.) should be visible while we select the survey added for defect log id 258 solved by Durgesh N Sharma-->
        [HttpPost]
        public JsonResult SelectSurveyByWorkOrderID(string WorkOrderID)
        {
            try
            {
                DataTable dt = _obj.SelectSurveyByWorkOrderID(WorkOrderID);
                return dtToViewBagJSON(dt, "AreaName", "SurveyID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        [HttpPost]
        public JsonResult SelectActivityBySurveyID(string WorkOrderID, string SurveyID)
        {
            try
            {
                DataTable dt = _obj.SelectActivityBySurveyID(WorkOrderID, SurveyID);
                return dtToViewBagJSON(dt, "Activity_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        [HttpPost]
        public JsonResult SelectProductByProduceType(string ProduceTypeID)
        {
            try
            {
                DataTable dt = _obj.SelectProductByProduceType(ProduceTypeID);
                return dtToViewBagJSON(dt, "ProductName", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        public JsonResult SelectAssetbyCategoryType(string AssetCategoryID)
        {
            try
            {
                DataTable dt = _obj.SelectAssetbyCategoryType(AssetCategoryID);
                return dtToViewBagJSON(dt, "ASSETNAME", "ASSETID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        [HttpPost]
        public JsonResult SelectActivityByModelID(string ModelID)
        {
            try
            {
                DataTable dt = _obj.SelectActivityByModelID(ModelID);
                return dtToViewBagJSON(dt, "Activity_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        public JsonResult SelectActivityByWorkOrderID(string WorkOrderID)
        {
            try
            {
                DataTable dt = _obj.SelectActivityByWorkOrderID(WorkOrderID);
                return dtToViewBagJSON(dt, "Activity_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        [HttpPost]
        public JsonResult SelectSubActivityByActivityID(string ActivityID)
        {
            try
            {
                DataTable dt = _obj.SelectSubActivityByActivityID(ActivityID);
                return dtToViewBagJSON(dt, "Sub_Activity_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        [HttpPost]
        public JsonResult SelectSubActivityByWorkOrderID(string WorkOrderID)
        {
            try
            {
                DataTable dt = _obj.SelectSubActivityByWorkOrderID(WorkOrderID);
                return dtToViewBagJSON(dt, "Sub_Activity_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        public JsonResult SelectNurseryByVilageCode(string Village_Code)
        {
            try
            {
                DataTable dt = _obj.SelectNurseryByVilageCode(Village_Code);
                return dtToViewBagJSON(dt, "NURSERY_NAME", "NURSERY_CODE");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }


        public JsonResult dtToViewBagJSON(DataTable dt, string TextField, string ValueField)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    //DataTable dt = _obj.SelectMicroPlanByVilageCode(Village_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr[TextField].ToString(), Value = @dr[ValueField].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }


        /// <summary>
        /// This function is used to get IFMS Work order data according to office id.
        /// </summary>
        /// <param name="OfficeId"></param>      
        /// <returns></returns>
        public List<IFMSWorkOrder> GetIFMSWorkorder(string OfficeId)
        {
            // private string URL = "https://sub.domain.com/objects.json?api_key=123";
            Forest_Ven_Workorder.forestSoapClient obj = new Forest_Ven_Workorder.forestSoapClient();
            string json = obj.GetWorkOrders("forest", "For@Rest", OfficeId);
            List<IFMSWorkOrder> lstIFMSWO = new JavaScriptSerializer().Deserialize<List<IFMSWorkOrder>>(json);
            //  lstIFMSWO.Sort();
            return lstIFMSWO.OrderBy(o => o.WorkOrderName.Trim()).ToList(); ;


            // return responseFromServer;
        }

        /// <summary>
        /// This function is used to get IFMS Work order data according to office id.
        /// </summary>
        /// <param name="OfficeId"></param>      
        /// <returns></returns>
        public List<IFMSVendor> GetIFMSVendors(string OfficeId)
        {
            // private string URL = "https://sub.domain.com/objects.json?api_key=123";
            Forest_Ven_Workorder.forestSoapClient obj = new Forest_Ven_Workorder.forestSoapClient();
            string json = obj.GetVendors("forest", "For@Rest", OfficeId);
            List<IFMSVendor> lstIfmsVen = new JavaScriptSerializer().Deserialize<List<IFMSVendor>>(json);

            // lstIfmsVen.Sort();
            return lstIfmsVen.OrderBy(o => o.name.Trim()).ToList();
            // return responseFromServer;
        }

        /// <summary>
        /// This function is used to get IFMS Work order data according to office id.
        /// </summary>
        /// <param name="OfficeId"></param>      
        /// <returns></returns>
        public JsonResult GetIFMSVendorsJson(string OfficeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            // private string URL = "https://sub.domain.com/objects.json?api_key=123";
            Forest_Ven_Workorder.forestSoapClient obj = new Forest_Ven_Workorder.forestSoapClient();
            string json = obj.GetVendors("forest", "For@Rest", OfficeId);
            List<IFMSVendor> lstIfmsVen = new JavaScriptSerializer().Deserialize<List<IFMSVendor>>(json);
            //  lstIfmsVen.Sort();
            lstIfmsVen = lstIfmsVen.OrderBy(o => o.name.Trim()).ToList();
            foreach (IFMSVendor v in lstIfmsVen)
            {
                items.Add(new SelectListItem { Text = v.name, Value = v.VendorCode });
            }
            // ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
            return Json(new SelectList(items, "Value", "Text"));
            // return responseFromServer;
        }

        #endregion

    }
}
