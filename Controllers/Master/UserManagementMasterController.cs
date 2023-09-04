//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : TicketBookingController
//  Description  : Manage and mapping user designation /  Manage and mapping user with forest office /  Manage and mapping user with multipal roles /  Manage and mapping user with Page access permission
//  Date Created : 21-12-2016
//  History      :
//  Version      : 1.0
//  Author       : Arvind Kumar Sharma
//  Modified By  : Arvind Kumar Sharma
//  Modified On  : 17-01-2017
//  Reviewed By  : Amit singh rajput 
//  Reviewed On  : 17-01-2017
//********************************************************************************************************
using FMDSS.Models;
using FMDSS.Models.ForesterAction;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FMDSS.Controllers.Master
{
    public class UserManagementMasterController : BaseController
    {
        List<SelectListItem> lstPLACES = new List<SelectListItem>();
        List<SelectListItem> lstISactive = new List<SelectListItem>();
        List<SelectListItem> lstisReviewerApprover = new List<SelectListItem>();
        List<Designations> DesignationLst = new List<Designations>();
        List<SelectListItem> lstofc = new List<SelectListItem>();

        List<SelectListItem> lstDesignation = new List<SelectListItem>();

        List<UnMappedOfficeDetails> lstUnMappedOfficeDetails = new List<UnMappedOfficeDetails>();
        List<MappedOfficeDetails> lstMappedOfficeDetails = new List<MappedOfficeDetails>();

        List<MappedUserDetails> lstMappedUserDetails = new List<MappedUserDetails>();
        List<UnMappedUserDetails> lstUnMappedUserDetails = new List<UnMappedUserDetails>();

        List<MappedPageAccessPermission> lstMappedPageAccessPermission = new List<MappedPageAccessPermission>();
        List<UnMappedPageAccessPermission> lstUnMappedPageAccessPermission = new List<UnMappedPageAccessPermission>();

        List<UserRole> lstUserRole = new List<UserRole>();


        List<UserProfileDetails> lstUserProfileDetails = new List<UserProfileDetails>();
        List<SelectListItem> LSTDesignation = new List<SelectListItem>();
        List<SelectListItem> lstGender = new List<SelectListItem>();

        List<SelectListItem> lstisSSo = new List<SelectListItem>();


        #region "SSO DETAILS"
        [HttpGet]
        public ActionResult SSODETAILS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SSODETAILS(USERDETAILS UOBJ)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Designations obj = new Designations();
            try
            {

                DataTable dtf = obj.Select_SSODETAILS(UOBJ.Ssoid);
                ViewBag.Rstatus = false;
                if (dtf.Rows.Count > 0)
                {
                    ViewBag.Rstatus = true;
                    UOBJ.Name = Convert.ToString(dtf.Rows[0]["Name"]);
                    UOBJ.Desig_Name = Convert.ToString(dtf.Rows[0]["Desig_Name"]);
                    UOBJ.Department = Convert.ToString(dtf.Rows[0]["Department"]);
                    UOBJ.OfficeName = Convert.ToString(dtf.Rows[0]["OfficeName"]);
                    UOBJ.Roles = Convert.ToString(dtf.Rows[0]["Roles"]);
                    UOBJ.Mobile = Convert.ToString(dtf.Rows[0]["Mobile"]);
                    UOBJ.Ssoid = Convert.ToString(dtf.Rows[0]["Ssoid"]);
                    UOBJ.OldSingleRole = Convert.ToString(dtf.Rows[0]["OldSingleRole"]);

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(UOBJ);
        }

        #endregion

        #region "DesignationsMappingForOfficeUser"

        public ActionResult DesignationsMappingForOfficeUser(string RecordStatus = "-1")
        {

            ViewBag.RecordStatus = RecordStatus;

            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Designations obj = new Designations();
            try
            {

                DataTable dtf = obj.Select_AllOfficeEmps();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    DesignationLst.Add(
                        new Designations()
                        {
                            Index = count,
                            SSOID = Convert.ToString(dr["SSOID"]),
                            Desig_Name = Convert.ToString(dr["Desig_Name"].ToString()),
                        });
                    count += 1;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(DesignationLst);
        }

        public ActionResult AddUpdateDesignationsMappingForOfficeUser(Designations oDesignations)
        {
            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                Designations obj = new Designations();

                DataTable dtf = obj.AddUpdateUserDesignation(oDesignations);
                oDesignations.LastUpdatedBy = UserID;
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("DesignationsMappingForOfficeUser", new { RecordStatus = status });
        }

        public ActionResult GetDesignationsMappingForOfficeUser(string SSOID)
        {

            Designations obj = new Designations();
            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = "Update Designations";


                DataSet dtf = new DataSet();
                dtf = obj.Select_AllOfficeEmps(SSOID);

                foreach (DataRow dr in dtf.Tables[0].Rows)
                {
                    obj = new Designations
                    {
                        SSOID = Convert.ToString(dr["SSOID"]),
                        DesigId = Convert.ToInt32(dr["DesigId"].ToString()),
                        Desig_Name = Convert.ToString(dr["Desig_Name"].ToString()),

                        OperationType = "Update Designations"
                    };

                }

                foreach (System.Data.DataRow dr in dtf.Tables[1].Rows)
                {
                    lstDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["DesigId"].ToString() });
                }

                ViewBag.lstDesignation = lstDesignation;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialDesignationsMappingForOfficeUser", obj);
        }

        #endregion


        #region "Designations"
        public ActionResult Designations(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Designations obj = new Designations();
            try
            {

                DataTable dtf = obj.Select_Designations();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    DesignationLst.Add(
                        new Designations()
                        {
                            Index = count,
                            DesigId = Convert.ToInt32(dr["DesigId"]),
                            Desig_Name = Convert.ToString(dr["Desig_Name"].ToString()),
                            Desig_Alias = Convert.ToString(dr["Desig_Alias"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(DesignationLst);
        }
        public ActionResult AddUpdateDesignation(Designations oDesignations)
        {
            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                Designations obj = new Designations();

                DataTable dtf = obj.AddUpdateDesignation(oDesignations);
                oDesignations.LastUpdatedBy = UserID;
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Designations", new { RecordStatus = status });
        }
        public ActionResult GetDesignations(string DesigId)
        {

            Designations obj = new Designations();
            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (DesigId == "0" ? "Add Designation" : "Edit Designations");


                DataTable dtf = obj.Select_Designation(Convert.ToInt32(DesigId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new Designations
                    {
                        DesigId = Convert.ToInt32(dr["DesigId"].ToString()),
                        Desig_Name = Convert.ToString(dr["Desig_Name"].ToString()),
                        Desig_Alias = Convert.ToString(dr["Desig_Alias"].ToString()),
                        IsActive = Convert.ToInt32(dr["IsActive"]),
                        IsReviewer = Convert.ToInt32(dr["IsReviewer"]),
                        IsApprover = Convert.ToInt32(dr["IsApprover"]),
                        ApplicationAssigner = Convert.ToInt32(dr["IsApplicationAssigner"]),

                        OperationType = "Edit Designations"
                    };

                }
                lstisReviewerApprover.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstisReviewerApprover.Add(new SelectListItem { Text = "No", Value = "0" });

                ViewBag.ISReviewerlst = lstisReviewerApprover;
                ViewBag.ISApproverlst = lstisReviewerApprover;

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialDesignation", obj);
        }



        public JsonResult Mapping(int IDs, int DesigIds, int FMDSSPermissionsTypesIDs, bool STATUS)
        {
            Designations Designation = new Designations();


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = "0";
            try
            {
                Designation.ID = IDs;
                Designation.DesigId = DesigIds;
                Designation.FMDSSPermissionsTypesID = FMDSSPermissionsTypesIDs;
                Designation.IsactiveView = STATUS;


                status = Designation.AddUpdateDesignationWithObjectLinking(Designation);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDesignationWithObjectLinking(string DesigId)
        {

            Designations obj = new Designations();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                DataTable dtf = obj.SelectDesignationWithObjectLinking(Convert.ToInt32(DesigId));
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    DesignationLst.Add(
                        new Designations()
                        {
                            Index = count,
                            ID = Convert.ToInt32(dr["ID"]),
                            DesigId = Convert.ToInt32(dr["DesigId"]),
                            Desig_Name = Convert.ToString(dr["Desig_Name"].ToString()),
                            FMDSSPermissionsTypesID = Convert.ToInt32(dr["FMDSSPermissionsTypesID"].ToString()),
                            ModuleDesc = Convert.ToString(dr["ModuleDesc"].ToString()),
                            ServiceTypeDesc = Convert.ToString(dr["ServiceTypeDesc"].ToString()),
                            PermissionDesc = Convert.ToString(dr["PermissionDesc"].ToString()),
                            SubPermissionDesc = Convert.ToString(dr["SubPermissionDesc"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialDesignationWithObjectLinking", DesignationLst);
        }



        #endregion

        #region "MappingDesignationsForPA"

        [HttpGet]
        public ActionResult MappingDesignationsForPA()
        {
            Designations obj = new Designations();

            DataTable DT = obj.Select_Designations();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstDesignation.Add(new SelectListItem { Text = "( " + @dr["Desig_Alias"].ToString() + " ) " + @dr["Desig_Name"].ToString(), Value = @dr["DesigId"].ToString() });
            }

            ViewBag.lstDesignations = lstDesignation;

            obj.MappedUserLIST = lstMappedUserDetails;
            obj.UnMappedUserLIST = lstUnMappedUserDetails;

            return View(obj);

        }

        [HttpPost]
        public ActionResult MappingDesignationsForPA(Designations model)
        {
            Designations obj = new Designations();

            DataTable DT = obj.Select_Designations();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstDesignation.Add(new SelectListItem { Text = "( " + @dr["Desig_Alias"].ToString() + " ) " + @dr["Desig_Name"].ToString(), Value = @dr["DesigId"].ToString() });
            }
            ViewBag.lstDesignations = lstDesignation;

            DataSet DS = new DataSet();
            DS = obj.GetMapUnmapDesignationsForPA(model.DesigId);
            int count = 1;
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                lstUnMappedUserDetails.Add(
                    new UnMappedUserDetails()
                    {
                        Index = count,
                        USERID = Convert.ToInt64(dr["UserID"].ToString()),
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.UnMappedUserLIST = lstUnMappedUserDetails;

            count = 1;
            foreach (DataRow dr in DS.Tables[1].Rows)
            {
                lstMappedUserDetails.Add(
                    new MappedUserDetails()
                    {
                        Index = count,
                        USERID = Convert.ToInt64(dr["UserID"].ToString()),
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.MappedUserLIST = lstMappedUserDetails;

            return View(model);
        }

        public JsonResult MappingForPA(Int64 USERIDs, int DESIGNATIONIDs, bool STATUS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            Designations obj = new Designations();

            string status = "0";
            try
            {
                status = obj.MappingForPA(USERIDs, DESIGNATIONIDs, STATUS, LOGINSSOID);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "MappingUserForOffice"
        BindMasterData masterObj = new BindMasterData();
        public JsonResult GetForestBoundaries(string OfficeLevel)
        {
            DataTable dt = masterObj.GetForestBoundaries(OfficeLevel);
            ViewBag.ForestOffices = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["CODE"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        public JsonResult GetForestBoundariesUserWise(string OfficeLevel)
        {
            DataTable dt = masterObj.GetForestBoundariesUserWise(OfficeLevel);
            List<SelectListItem> items = new List<SelectListItem>();
            if (Globals.Util.isValidDataTable(dt))
            {
                switch (OfficeLevel)
                {
                    case "CIR":
                        items = dt.AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = x.Field<string>("CIRCLE_CODE"),
                            Text = x.Field<string>("CIRCLE_NAME")
                        }).ToList();
                        break;
                    case "DIV":
                        items = dt.AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = x.Field<string>("DIV_CODE"),
                            Text = x.Field<string>("DIV_NAME")
                        }).ToList();
                        break;
                    case "RNG":
                        items = dt.AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = x.Field<string>("RANGE_CODE"),
                            Text = x.Field<string>("RANGE_NAME")
                        }).ToList();
                        break;
                }
            }
            return Json(new SelectList(items, "Value", "Text"));
        }


        [HttpPost]
        public JsonResult GetForestOffices(string ForestCode)
        {
            DataTable dt = masterObj.getForestOffices(ForestCode);
            ViewBag.ForestOffices = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["OfficeName"].ToString(), Value = @dr["Office_ID"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }




        [HttpGet]
        public ActionResult MappingUserForOffice(string id = "")
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            OfficeMapping obj = new OfficeMapping();
            try
            {


                if (id == "")
                {
                    ViewBag.lstForestBoundaries = lstDesignation;
                    ViewBag.lstOfficeID = lstDesignation;

                    obj.MappedOfficeLIST = lstMappedOfficeDetails;
                    obj.UnMappedOfficeLIST = lstUnMappedOfficeDetails;

                }
                else
                {

                    id = Encryption.decrypt(id);
                    obj.OffcLevel = id.ToString().Split('_')[0].ToString();
                    obj.ForestBoundaries = id.ToString().Split('_')[1].ToString();
                    obj.OfficeID = id.ToString().Split('_')[2].ToString();

                    obj = GETDataOfficeUser(obj);

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);
            }


            return View(obj);
        }

        [HttpPost]
        public ActionResult MappingUserForOffice(OfficeMapping model)
        {
            model = GETDataOfficeUser(model);
            return View(model);
        }

        public OfficeMapping GETDataOfficeUser(OfficeMapping model)
        {
            OfficeMapping obj = new OfficeMapping();
            SuperAdminOperations obj1 = new SuperAdminOperations();
            List<SelectListItem> items = new List<SelectListItem>();
            List<SelectListItem> items1 = new List<SelectListItem>();

            if (model.OffcLevel == "ST")
            {
                lstDesignation.Add(new SelectListItem { Text = "ST001", Value = "State HQ" });
                ViewBag.lstForestBoundaries = lstDesignation;
            }
            else
            {
                DataTable dt = masterObj.GetForestBoundaries(model.OffcLevel);
                ViewBag.ForestOffices = dt;

                foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["CODE"].ToString() });
                }

            }
            ViewBag.lstForestBoundaries = items;

            DataTable dt1 = masterObj.getForestOffices(model.ForestBoundaries);
            ViewBag.ForestOffices = dt1;

            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items1.Add(new SelectListItem { Text = @dr["OfficeName"].ToString(), Value = @dr["Office_ID"].ToString() });
            }

            ViewBag.lstOfficeID = items1;


            DataSet DS = new DataSet();
            DS = obj.GetMapUnmapDesignationsForPA(model.OfficeID);
            int count = 1;
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                lstUnMappedOfficeDetails.Add(
                    new UnMappedOfficeDetails()
                    {
                        Index = count,
                        USERID = Convert.ToInt64(dr["UserID"].ToString()),
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.UnMappedOfficeLIST = lstUnMappedOfficeDetails;

            count = 1;
            foreach (DataRow dr in DS.Tables[1].Rows)
            {
                lstMappedOfficeDetails.Add(
                    new MappedOfficeDetails()
                    {
                        Index = count,
                        USERID = Convert.ToInt64(dr["UserID"].ToString()),
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.MappedOfficeLIST = lstMappedOfficeDetails;

            return model;
        }

        public JsonResult MappingOffice(Int64 USERIDs, string ForestOffices, bool STATUS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            OfficeMapping obj = new OfficeMapping();

            string status = "0";
            try
            {
                status = obj.MappingForPA(USERIDs, ForestOffices, STATUS, LOGINSSOID);
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);
            }

            return Json(status, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetREPORTINGTO(string USERID, string Office, string ForestBoundaries, string OffcLevel)
        {

            OfficeMapping obj = new OfficeMapping();
            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = "Mapping Forest User For Office";
                DataSet DS = new DataSet();
                DS = obj.GetREPORTINGTO(USERID);

                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    obj = new OfficeMapping
                    {
                        OfficeID = Office,
                        ForestBoundaries = ForestBoundaries,
                        OffcLevel = OffcLevel,
                        USERID = USERID,
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        Designation = Convert.ToString(dr["Designation"].ToString()),
                        Designation_Name = Convert.ToString(dr["Desig_Name"].ToString()),
                        REPORTINGTO = "0",

                    };

                }

                foreach (System.Data.DataRow dr in DS.Tables[1].Rows)
                {
                    lstDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["DesigId"].ToString() });
                }

                ViewBag.LSTREPORTINGTO = lstDesignation;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialMapUserOfficeEmp", obj);
        }

        [HttpPost]
        public ActionResult SubmitREPORTINGTO(OfficeMapping model)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            OfficeMapping obj = new OfficeMapping();

            string status = "0";
            try
            {
                status = obj.SubmitREPORTINGTO(model, Convert.ToString(LOGINSSOID));
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);
            }

            return RedirectToAction("MappingUserForOffice", new { id = Encryption.encrypt(model.OffcLevel + "_" + model.ForestBoundaries + "_" + model.OfficeID) });
        }

        public ActionResult MappingPlaceWithForestOffice(string USERID, string OfficeID, string ForestBoundaries, string OffcLevel)
        {
            Designations obj = new Designations();
            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                DataSet dtf = new DataSet();
                dtf = obj.Select_Place(USERID, OfficeID);
                obj.USERID = Convert.ToInt64(USERID);
                obj.OfficeID = OfficeID;
                obj.PLCAES = "";
                obj.ForestBoundaries = ForestBoundaries;
                obj.OffcLevel = OffcLevel;

                string x = "";
                if (dtf.Tables[1].Rows.Count > 0)
                {
                    x = Convert.ToString(dtf.Tables[1].Rows[0][0]);

                    int[] xx = new int[x.Split(',').Length];
                    int row = 0;
                    foreach (var a in x.Split(','))
                    {

                        xx[row] = Convert.ToInt16(a);
                        row = row + 1;
                    }
                    obj.PLCAEIDss = xx;
                }


                foreach (DataRow dr in dtf.Tables[0].Rows)
                {
                    lstPLACES.Add(new SelectListItem { Text = @dr["PLACENAME"].ToString(), Value = @dr["PLACEID"].ToString() });
                }

                obj.PLCAEIDs = lstPLACES;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_partialMappingPlaceWithForestOffice", obj);
        }

        public ActionResult AddUpdatePlaceWithForestOffice(Designations obj)
        {

            DataSet dtf = new DataSet();

            dtf = obj.AddPlacewiseEmpAndOfficeMapping(obj.USERID, obj.OfficeID, string.Join(",", obj.PLCAEIDss));


            return RedirectToAction("MappingUserForOffice", new { id = Encryption.encrypt(obj.OffcLevel + "_" + obj.ForestBoundaries + "_" + obj.OfficeID) });
        }

        #endregion


        //#region "Add New User ROLE"
        //public ActionResult UserRole(bool? RecordStatus)
        //{
        //    if (RecordStatus == null)
        //    {
        //        ViewBag.RecordStatus = -1;
        //    }
        //    else
        //    {
        //        if (Convert.ToBoolean(RecordStatus) == true)
        //            ViewBag.RecordStatus = 1;
        //        else
        //            ViewBag.RecordStatus = 0;
        //    }

        //    List<SelectListItem> Places = new List<SelectListItem>();
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"]);
        //    UserRole obj = new UserRole();
        //    try
        //    {

        //        DataTable dtf = obj.Select_UserRoles();

        //        int count = 1;
        //        foreach (DataRow dr in dtf.Rows)
        //        {
        //            lstUserRole.Add(
        //                new UserRole()
        //                {
        //                    Index = count,
        //                    RoleId = Convert.ToInt32(dr["CategoryID"].ToString()),
        //                    RoleName = Convert.ToString(dr["RoleName"].ToString()),
        //                    Desc = Convert.ToString(dr["Desc"].ToString()),
        //                    IsactiveView = Convert.ToBoolean(dr["Isactive"]),

        //                });
        //            count += 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

        //    }
        //    return View(lstUserRole);
        //}
        //public ActionResult ADDUpdateUserRole(UserRole oUserRole)
        //{
        //    List<SelectListItem> Places = new List<SelectListItem>();
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"]);
        //    string status = null;
        //    try
        //    {

        //        UserRole obj = new UserRole();
        //        DataTable dtf = obj.AddUpdateUserRole(oUserRole);
        //        status = dtf.Rows[0][0].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

        //    }
        //    return RedirectToAction("UserRole", new { RecordStatus = status });
        //}
        //public ActionResult GetUserRole(string CategoryID)
        //{

        //    VehicleEquipment obj = new VehicleEquipment();
        //    List<SelectListItem> Places = new List<SelectListItem>();
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"]);
        //    try
        //    {
        //        List<SelectListItem> District = new List<SelectListItem>();

        //        List<SelectListItem> items = new List<SelectListItem>();

        //        ViewBag.OpType = (CategoryID == "0" ? "Add Vehicle Equipment" : "Edit Vehicle Equipment");


        //        DataTable dtf = obj.Select_VehicleEquipment(Convert.ToInt32(CategoryID));

        //        foreach (DataRow dr in dtf.Rows)
        //        {
        //            obj = new VehicleEquipment
        //            {
        //                CategoryID = Convert.ToInt64(dr["CategoryID"].ToString()),
        //                CategoryName = Convert.ToString(dr["CategoryName"].ToString()),
        //                Isactive = Convert.ToInt32(dr["Isactive"]),



        //                OperationType = "Edit Vehicle Equipment"
        //            };

        //        }

        //        lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
        //        lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

        //        ViewBag.ISactivelst = lstISactive;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

        //    }
        //    return PartialView("_partialVehicleEquipment", obj);
        //}



        //#endregion

        #region "Mapping ROLE with User"


        [HttpGet]
        public ActionResult MappingROLEwithUser()
        {
            UserRole obj = new UserRole();

            DataTable DT = obj.Select_UseRoles();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstDesignation.Add(new SelectListItem { Text = @dr["RoleName"].ToString() + " ( " + @dr["Desc"].ToString() + " ) ", Value = @dr["RoleId"].ToString() });
            }

            ViewBag.lstDesignations = lstDesignation;

            obj.MappedUserLIST = lstMappedUserDetails;
            obj.UnMappedUserLIST = lstUnMappedUserDetails;

            return View(obj);

        }

        [HttpPost]
        public ActionResult MappingROLEwithUser(UserRole model)
        {
            UserRole obj = new UserRole();

            DataTable DT = obj.Select_UseRoles();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstDesignation.Add(new SelectListItem { Text = "( " + @dr["RoleName"].ToString() + " ) " + @dr["Desc"].ToString(), Value = @dr["RoleId"].ToString() });
            }
            ViewBag.lstDesignations = lstDesignation;

            DataSet DS = new DataSet();
            DS = obj.GetMapUnmapROLEwithUser(model.RoleId);
            int count = 1;
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                lstUnMappedUserDetails.Add(
                    new UnMappedUserDetails()
                    {
                        Index = count,
                        USERID = Convert.ToInt64(dr["UserID"].ToString()),
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.UnMappedUserLIST = lstUnMappedUserDetails;

            count = 1;
            foreach (DataRow dr in DS.Tables[1].Rows)
            {
                lstMappedUserDetails.Add(
                    new MappedUserDetails()
                    {
                        Index = count,
                        USERID = Convert.ToInt64(dr["UserID"].ToString()),
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.MappedUserLIST = lstMappedUserDetails;

            return View(model);
        }

        public JsonResult VaildationsMappingUserWithRoles(Int64 USERIDs, int RoleIds, bool STATUS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            UserRole obj = new UserRole();

            string status = "0";
            try
            {
                status = obj.VaildationsMappingUserWithRoles(USERIDs, RoleIds, STATUS, LOGINSSOID);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        public JsonResult MappingForROLEwithUser(Int64 USERIDs, int RoleIds, bool STATUS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            UserRole obj = new UserRole();

            string status = "0";
            try
            {
                status = obj.MappingForROLEwithUser(USERIDs, RoleIds, STATUS, LOGINSSOID);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region "Page Access Permissions"


        [HttpGet]
        public ActionResult PageAccessPermissions()
        {
            PageAccessPermission obj = new PageAccessPermission();

            DataTable DT = obj.Select_UseRoles();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstDesignation.Add(new SelectListItem { Text = @dr["RoleName"].ToString() + " ( " + @dr["Desc"].ToString() + " ) ", Value = @dr["RoleId"].ToString() });
            }

            ViewBag.lstDesignations = lstDesignation;

            obj.MappedPageAccessLIST = lstMappedPageAccessPermission;
            obj.UnMappedPageAccessLIST = lstUnMappedPageAccessPermission;

            return View(obj);

        }

        [HttpPost]
        public ActionResult PageAccessPermissions(PageAccessPermission model)
        {
            PageAccessPermission obj = new PageAccessPermission();

            DataTable DT = obj.Select_UseRoles();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstDesignation.Add(new SelectListItem { Text = "( " + @dr["RoleName"].ToString() + " ) " + @dr["Desc"].ToString(), Value = @dr["RoleId"].ToString() });
            }
            ViewBag.lstDesignations = lstDesignation;

            DataSet DS = new DataSet();
            DS = obj.GetMapUnmapPageAccessPermission(model.RoleId);
            int count = 1;
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                lstUnMappedPageAccessPermission.Add(
                    new UnMappedPageAccessPermission()
                    {
                        Index = count,
                        PageID = Convert.ToString(dr["ID"]),
                        PageTitle = Convert.ToString(dr["PageTitle"]),
                        // PageURL = Convert.ToString(dr["PageURL"]),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.UnMappedPageAccessLIST = lstUnMappedPageAccessPermission;

            count = 1;
            foreach (DataRow dr in DS.Tables[1].Rows)
            {
                lstMappedPageAccessPermission.Add(
                    new MappedPageAccessPermission()
                    {
                        Index = count,
                        PageID = Convert.ToString(dr["ID"]),
                        PageTitle = Convert.ToString(dr["PageTitle"]),
                        //  PageURL = Convert.ToString(dr["PageURL"]),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }

            model.MappedPageAccessLIST = lstMappedPageAccessPermission;

            return View(model);
        }

        public JsonResult MappingForPageAccessPermissions(string PageIDs, int RoleIds, bool STATUS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            PageAccessPermission obj = new PageAccessPermission();

            string status = "0";
            try
            {
                status = obj.MappingForPageAccessPermission(PageIDs, RoleIds, STATUS, LOGINSSOID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region "One Click Access Rights For SSOID"

        [HttpGet]
        public ActionResult OneClickAccessPermission(Int16? RecordStatus)
        {

            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                ViewBag.RecordStatus = RecordStatus;
            }

            OneClickAccessRights obj = new OneClickAccessRights();
            ViewBag.lstForestBoundaries = lstofc;

            DataSet dtf = new DataSet();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                dtf = obj.GetOneClickAccessData("GetMasterDataWithRestriction");
                foreach (DataRow dr in dtf.Tables[0].Rows)
                {
                    lstPLACES.Add(new SelectListItem { Text = @dr["PLACENAME"].ToString(), Value = @dr["PLACEID"].ToString() });
                }

                obj.ListPLACEIDs = lstPLACES;

                foreach (DataRow dr in dtf.Tables[1].Rows)
                {
                    lstDesignation.Add(new SelectListItem { Text = @dr["DesigName"].ToString(), Value = @dr["DesigId"].ToString() });
                }

                obj.ListDESIGNATIONs = lstDesignation;

                foreach (DataRow dr in dtf.Tables[2].Rows)
                {
                    lstISactive.Add(new SelectListItem { Text = @dr["RoleName"].ToString(), Value = @dr["RoleId"].ToString() });
                }

                obj.ListRoleIds = lstISactive;

                foreach (DataRow dr in dtf.Tables[3].Rows)
                {
                    lstofc.Add(new SelectListItem { Text = @dr["OfficeLevelName"].ToString(), Value = @dr["OfficeLevelCode"].ToString() });
                }
                ViewBag.lstOfficeID = lstofc;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(obj);

        }

        [HttpPost]
        public ActionResult OneClickAccessPermission(OneClickAccessRights model)
        {
            //PageAccessPermission obj = new PageAccessPermission();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                model.USERID = UserID;
                model.PLACEIDstr = model.PLACEIDs == null ? "" : string.Join(",", model.PLACEIDs);
                model.RoleIdstr = model.RoleIds == null ? "" : string.Join(",", model.RoleIds);
                status = Convert.ToString(model.UPDATESSOID(model).Rows[0][0]);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return RedirectToAction("OneClickAccessPermission", new { RecordStatus = status });
        }

        [HttpGet]
        public ActionResult OneClickAccessRights(Int16? RecordStatus)
        {

            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                ViewBag.RecordStatus = RecordStatus;
            }






            OneClickAccessRights obj = new OneClickAccessRights();
            ViewBag.lstForestBoundaries = lstofc;
            ViewBag.lstOfficeID = lstofc;

            DataSet dtf = new DataSet();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                dtf = obj.Select_AllPlaceDesignationsRoles();
                foreach (DataRow dr in dtf.Tables[0].Rows)
                {
                    lstPLACES.Add(new SelectListItem { Text = @dr["PLACENAME"].ToString(), Value = @dr["PLACEID"].ToString() });
                }

                obj.ListPLACEIDs = lstPLACES;

                foreach (DataRow dr in dtf.Tables[1].Rows)
                {
                    lstDesignation.Add(new SelectListItem { Text = @dr["DesigName"].ToString(), Value = @dr["DesigId"].ToString() });
                }

                obj.ListDESIGNATIONs = lstDesignation;

                foreach (DataRow dr in dtf.Tables[2].Rows)
                {
                    lstISactive.Add(new SelectListItem { Text = @dr["RoleName"].ToString(), Value = @dr["RoleId"].ToString() });
                }

                obj.ListRoleIds = lstISactive;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(obj);

        }

        [HttpPost]
        public ActionResult OneClickAccessRights(OneClickAccessRights model)
        {
            //PageAccessPermission obj = new PageAccessPermission();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                model.USERID = UserID;
                model.PLACEIDstr = model.PLACEIDs == null ? "" : string.Join(",", model.PLACEIDs);
                model.RoleIdstr = model.RoleIds == null ? "" : string.Join(",", model.RoleIds);
                status = Convert.ToString(model.UPDATESSOID(model).Rows[0][0]);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return RedirectToAction("OneClickAccessRights", new { RecordStatus = status });
        }


        public JsonResult GetOldSSOID(string OldSSOID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            PageAccessPermission obj = new PageAccessPermission();

            string ssoid = "0";
            string DesigId = "0";
            string ReportingTo = "0";
            string RoleIDs = "";

            try
            {
                if (OldSSOID == string.Empty)
                {
                    ssoid = "0";
                }
                else
                {
                    Designations obj1 = new Designations();
                    DataTable dtf = obj1.Select_SSODETAILS(OldSSOID);
                    if (dtf.Rows.Count > 0)
                    {
                        ssoid = Convert.ToString(dtf.Rows[0]["ssoid"]);
                        DesigId = Convert.ToString(dtf.Rows[0]["DesigId"]);
                        ReportingTo = Convert.ToString(dtf.Rows[0]["ReportingTo"]);
                        RoleIDs = Convert.ToString(dtf.Rows[0]["RoleIDs"]);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);

            }
            return Json(new { ssoid = ssoid, designation = DesigId, ReportingTo = ReportingTo, RoleIDs = RoleIDs }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetREPORTINGTOData(string DESIGNATION)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            OneClickAccessRights obj = new OneClickAccessRights();

            string DESIGNATIONs = "0";
            try
            {
                if (DESIGNATIONs == string.Empty)
                {
                    DESIGNATIONs = "0";
                }
                else
                {

                    DataTable dtf = obj.Select_REPORTINGTO(DESIGNATION);
                    if (dtf.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtf.Rows)
                        {
                            lstDesignation.Add(new SelectListItem { Text = @dr["DesigName"].ToString(), Value = @dr["DesigId"].ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);

            }


            return Json(new SelectList(lstDesignation, "Value", "Text"));
        }


        #endregion


        #region "UserProfileDetails"


        public ActionResult CitizenProfileDetails(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
            try
            {
                UserProfileDetails obj = new UserProfileDetails();
                DataTable dtf = obj.Select_CitizenUserProfileDetailss();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstUserProfileDetails.Add(
                        new UserProfileDetails()
                        {
                            Index = count,
                            UserID = Convert.ToInt64(dr["UserID"].ToString()),
                            Ssoid = Convert.ToString(dr["Ssoid"]),
                            Name = Convert.ToString(dr["Name"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),



                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);

            }

            TempData["actionName"] = "CitizenProfileDetails";

            return View(lstUserProfileDetails);
        }



        public ActionResult GetCitizenUserProfileDetailsSSOWise(string SsoId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
            UserProfileDetails obj = new UserProfileDetails();
            try
            {

                // ViewBag.OpType = (UserID == "0" ? "Add User" : "Edit User");

                DataTable dtf = obj.Select_UserProfileDetails(SsoId);

                lstGender.Add(new SelectListItem { Text = "Male", Value = "Male" });
                lstGender.Add(new SelectListItem { Text = "Female", Value = "Female" });

                ViewBag.GenderType = lstGender;



                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



                lstisSSo.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstisSSo.Add(new SelectListItem { Text = "No", Value = "0" });

                ViewBag.Bhamashalst = lstisSSo;
                ViewBag.ISavilableSSolst = lstisSSo;
                ViewBag.Kioskuserlst = lstisSSo;
                ViewBag.DeptKioskUserlst = lstisSSo;


                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new UserProfileDetails
                    {

                        UserID = Convert.ToInt64(dr["UserID"].ToString()),
                        Name = Convert.ToString(dr["Name"]),
                        Ssoid = Convert.ToString(dr["Ssoid"]),
                        EmailId = Convert.ToString(dr["EmailId"]),
                        Mobile = Convert.ToString(dr["Mobile"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        DOB = Convert.ToString(dr["DOB"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Postal_Address1 = Convert.ToString(dr["Postal_Address1"]),
                        Postal_Code1 = Convert.ToString(dr["Postal_Code1"]),
                        IsKioskUser = Convert.ToInt32(dr["IsKioskUser"]),
                        Isactive = Convert.ToInt32(dr["Isactive"] == "" ? 0 : dr["Isactive"]),
                        //IsSSO = Convert.ToInt32(dr["IsSSO"]),
                        //IsBhamashah = Convert.ToInt32(dr["IsBhamashah"]),
                        IsDepartmentalKioskUser = Convert.ToInt32(dr["IsDepartmentalKioskUser"]),
                        Bhamashah_Id = Convert.ToString(dr["Bhamashah_Id"]),
                        Aadhar_ID = Convert.ToString(dr["Aadhar_ID"]),
                        OperationType = "Edit UserProfileDetails"
                    };

                }
                DataTable dtf2 = obj.DesignationName();

                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTDesignation.Add(new SelectListItem { Text = @dr1["Desig_Name"].ToString(), Value = @dr1["DesigId"].ToString() });
                }

                ViewBag.ddlDesignation = LSTDesignation;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);

            }
            return PartialView("_partialUserProfileDetails", obj);
        }



        public ActionResult GetCitizenUserProfileDetails(string UserID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
            UserProfileDetails obj = new UserProfileDetails();
            try
            {

                ViewBag.OpType = (UserID == "0" ? "Add User" : "Edit User");

                DataTable dtf = obj.Select_UserProfileDetails(Convert.ToInt32(UserID));

                lstGender.Add(new SelectListItem { Text = "Male", Value = "Male" });
                lstGender.Add(new SelectListItem { Text = "Female", Value = "Female" });

                ViewBag.GenderType = lstGender;



                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



                lstisSSo.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstisSSo.Add(new SelectListItem { Text = "No", Value = "0" });

                ViewBag.Bhamashalst = lstisSSo;
                ViewBag.ISavilableSSolst = lstisSSo;
                ViewBag.Kioskuserlst = lstisSSo;
                ViewBag.DeptKioskUserlst = lstisSSo;


                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new UserProfileDetails
                    {

                        UserID = Convert.ToInt64(dr["UserID"].ToString()),
                        Name = Convert.ToString(dr["Name"]),
                        Ssoid = Convert.ToString(dr["Ssoid"]),
                        EmailId = Convert.ToString(dr["EmailId"]),
                        Mobile = Convert.ToString(dr["Mobile"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        DOB = Convert.ToString(dr["DOB"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Postal_Address1 = Convert.ToString(dr["Postal_Address1"]),
                        Postal_Code1 = Convert.ToString(dr["Postal_Code1"]),
                        IsKioskUser = Convert.ToInt32(dr["IsKioskUser"]),
                        Isactive = Convert.ToInt32(dr["Isactive"] == "" ? 0 : dr["Isactive"]),
                        //IsSSO = Convert.ToInt32(dr["IsSSO"]),
                        //IsBhamashah = Convert.ToInt32(dr["IsBhamashah"]),
                        IsDepartmentalKioskUser = Convert.ToInt32(dr["IsDepartmentalKioskUser"]),
                        Bhamashah_Id = Convert.ToString(dr["Bhamashah_Id"]),
                        Aadhar_ID = Convert.ToString(dr["Aadhar_ID"]),
                        OperationType = "Edit UserProfileDetails"
                    };

                }
                DataTable dtf2 = obj.DesignationName();

                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTDesignation.Add(new SelectListItem { Text = @dr1["Desig_Name"].ToString(), Value = @dr1["DesigId"].ToString() });
                }

                ViewBag.ddlDesignation = LSTDesignation;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);

            }
            return PartialView("_partialUserProfileDetails", obj);
        }


        public ActionResult UserProfileDetails(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
            try
            {
                UserProfileDetails obj = new UserProfileDetails();
                DataTable dtf = obj.Select_UserProfileDetailss();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstUserProfileDetails.Add(
                        new UserProfileDetails()
                        {
                            Index = count,
                            UserID = Convert.ToInt64(dr["UserID"].ToString()),
                            Ssoid = Convert.ToString(dr["Ssoid"]),
                            Name = Convert.ToString(dr["Name"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);

            }
            TempData["actionName"] = "UserProfileDetails";
            return View(lstUserProfileDetails);
        }

        public ActionResult RemoveDepartmentUser(string id)
        {
            UserProfileDetails obj = new UserProfileDetails();

            DataTable dtUser = obj.GetUser(id);

            string ssoId = Convert.ToString(dtUser.Rows[0]["Ssoid"]);
            string Name = Convert.ToString(dtUser.Rows[0]["Name"]);
            string EmailId = Convert.ToString(dtUser.Rows[0]["EmailId"]);
            string Mobile = Convert.ToString(dtUser.Rows[0]["Mobile"]);

            string userDetails = Name + '(' + ssoId + '-' + Mobile + ')' + Name;

            DataTable dtf = obj.RemoveDepartmentRole(id);

            #region Email and SMS
            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            string Roles = string.Empty;
            foreach (DataRow dr in dtf.Rows)
            {
                Roles += Convert.ToString(dr["RoleName"]);
            }
            objSMSandEMAILtemplate.SendMailComman("ALL", "Remove Department Role", userDetails, Roles, "", "", "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
            #endregion

            return RedirectToAction("UserProfileDetails", "UserManagementMaster");

        }

        public ActionResult ADDUpdateUserProfileDetails(UserProfileDetails oUserProfileDetails)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                UserProfileDetails obj = new UserProfileDetails();
                //  obj.Placeauto();

                oUserProfileDetails.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateUserProfileDetails(oUserProfileDetails);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }


            return RedirectToAction(Convert.ToString(TempData["actionName"]), new { RecordStatus = status });
            //return RedirectToAction("UserProfileDetails", new { RecordStatus = status });

        }
        public ActionResult GetUserProfileDetails(string UserID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
            UserProfileDetails obj = new UserProfileDetails();
            try
            {

                ViewBag.OpType = (UserID == "0" ? "Add User" : "Edit User");

                DataTable dtf = obj.Select_UserProfileDetails(Convert.ToInt32(UserID));

                lstGender.Add(new SelectListItem { Text = "Male", Value = "Male" });
                lstGender.Add(new SelectListItem { Text = "Female", Value = "Female" });

                ViewBag.GenderType = lstGender;



                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



                lstisSSo.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstisSSo.Add(new SelectListItem { Text = "No", Value = "0" });

                ViewBag.Bhamashalst = lstisSSo;
                ViewBag.ISavilableSSolst = lstisSSo;
                ViewBag.Kioskuserlst = lstisSSo;
                ViewBag.DeptKioskUserlst = lstisSSo;


                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new UserProfileDetails
                    {

                        UserID = Convert.ToInt64(dr["UserID"].ToString()),
                        Name = Convert.ToString(dr["Name"]),
                        Ssoid = Convert.ToString(dr["Ssoid"]),
                        EmailId = Convert.ToString(dr["EmailId"]),
                        Mobile = Convert.ToString(dr["Mobile"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        DOB = Convert.ToString(dr["DOB"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Postal_Address1 = Convert.ToString(dr["Postal_Address1"]),
                        Postal_Code1 = Convert.ToString(dr["Postal_Code1"]),
                        IsKioskUser = Convert.ToInt32(dr["IsKioskUser"]),
                        Isactive = Convert.ToInt32(dr["Isactive"] == "" ? 0 : dr["Isactive"]),
                        //IsSSO = Convert.ToInt32(dr["IsSSO"]),
                        //IsBhamashah = Convert.ToInt32(dr["IsBhamashah"]),
                        IsDepartmentalKioskUser = Convert.ToInt32(dr["IsDepartmentalKioskUser"]),
                        Bhamashah_Id = Convert.ToString(dr["Bhamashah_Id"]),
                        Aadhar_ID = Convert.ToString(dr["Aadhar_ID"]),
                        OperationType = "Edit UserProfileDetails"
                    };

                }
                DataTable dtf2 = obj.DesignationName();

                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTDesignation.Add(new SelectListItem { Text = @dr1["Desig_Name"].ToString(), Value = @dr1["DesigId"].ToString() });
                }

                ViewBag.ddlDesignation = LSTDesignation;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);

            }
            return PartialView("_partialUserProfileDetails", obj);
        }


        public JsonResult CheckDuplicateForUser(int UserID, string Ssoid)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                UserProfileDetails Obj = new UserProfileDetails();
                Obj.UserID = UserID;
                Obj.Ssoid = Ssoid;



                status = Obj.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);

            }
            return Json(status);
        }



        #endregion

        #region "AddNewOffice"
        [HttpGet]
        public ActionResult AddNewOffice(string Boundary, string BoundaryName)
        {
            //string url = string.Format("https://emitraapp.rajasthan.gov.in/webServicesRepository/getPGDetails?fromDate=01/02/2017&toDate=03/05/2017&merchantCode=FOREST0117&serviceId=2239&ALL_DETAILS=0");
            //using (WebClient client = new WebClient())
            //{
            //    string json = client.DownloadString(url);

            //    object objResponse = (new JavaScriptSerializer()).Deserialize<object>(json);

            //}


            UserProfileDetails obj = new UserProfileDetails();

            if (Boundary == null)
            {

            }
            else
            {

                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
                try
                {

                    obj.Boundary = Boundary;
                    obj.Name = BoundaryName;
                    obj.OfficeName = "";

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);

                }

            }

            return PartialView("_partialAddNewOffice", obj);

        }

        [HttpPost]
        public ActionResult AddNewOffice(UserProfileDetails obj)
        {



            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
            try
            {
                DataTable dt = obj.AddNewOffice(obj);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);
            }
            return RedirectToAction("OneClickAccessRights", "UserManagementMaster");
        }

        #endregion


        List<SelectListItem> lstStatus = new List<SelectListItem>();

        #region "AssignUserRoleforADAward"


        [HttpGet]
        public ActionResult AssignUserRoleforADAward()
        {
            UseRoleADAward obj = new UseRoleADAward();

            DataTable DT = obj.Select_UseRoles();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstDesignation.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
            }

            ViewBag.lstDesignations = lstDesignation;

            DataTable DT1 = obj.Select_Status();
            foreach (System.Data.DataRow dr in DT1.Rows)
            {
                lstStatus.Add(new SelectListItem { Text = @dr["StatusDesc"].ToString(), Value = @dr["StatusID"].ToString() });
            }

            ViewBag.lstStatus = lstStatus;

            obj.MappedUserLIST = lstMappedUserDetails;
            obj.UnMappedUserLIST = lstUnMappedUserDetails;

            return View(obj);

        }

        [HttpPost]
        public ActionResult AssignUserRoleforADAward(UseRoleADAward model)
        {
            UseRoleADAward obj = new UseRoleADAward();

            DataTable DT = obj.Select_UseRoles();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstDesignation.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
            }

            ViewBag.lstDesignations = lstDesignation;

            DataTable DT1 = obj.Select_Status();
            foreach (System.Data.DataRow dr in DT1.Rows)
            {
                lstStatus.Add(new SelectListItem { Text = @dr["StatusDesc"].ToString(), Value = @dr["StatusID"].ToString() });
            }

            ViewBag.lstStatus = lstStatus;

            DataSet DS = new DataSet();
            DS = obj.GetMapUnmapROLEwithUser(model);
            int count = 1;
            foreach (DataRow dr in DS.Tables[0].Rows)
            {
                lstUnMappedUserDetails.Add(
                    new UnMappedUserDetails()
                    {
                        Index = count,
                        USERID = Convert.ToInt64(dr["UserID"].ToString()),
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.UnMappedUserLIST = lstUnMappedUserDetails;

            count = 1;
            foreach (DataRow dr in DS.Tables[1].Rows)
            {
                lstMappedUserDetails.Add(
                    new MappedUserDetails()
                    {
                        Index = count,
                        USERID = Convert.ToInt64(dr["UserID"].ToString()),
                        SSOID = Convert.ToString(dr["Ssoid"].ToString()),
                        IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                    });
                count += 1;
            }
            model.MappedUserLIST = lstMappedUserDetails;

            return View(model);
        }



        public JsonResult MappingForROLEwithUserADAward(Int64 USERIDs, string ApprovalLevelID, string UserStatusID, bool STATUS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);
            UseRoleADAward obj = new UseRoleADAward();

            string status = "0";
            try
            {
                status = obj.MappingForROLEwithUserADAward(USERIDs, ApprovalLevelID, UserStatusID, STATUS, LOGINSSOID);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region add by sunny for multiple offie mapping
        [HttpGet]
        public ActionResult Get_SSODetails()
        {
            TempData["msg"] = TempData["msg1"];
            TempData["isError1"] = TempData["isError"];
            ViewBag.ReturnMsg = TempData["msg"];
            ViewBag.IsError = TempData["isError1"];
            return View();
        }

        [HttpPost]
        public ActionResult Get_SSODetails(USERDETAILS_MultipleOffice UOBJ)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Designations obj = new Designations();
            try
            {

                DataTable dtf = obj.Select_SSODETAILS_MultiOffice(UOBJ.Ssoid);
                ViewBag.Rstatus = false;
                if (dtf.Rows.Count > 0)
                {
                    for (int i = 0; i < dtf.Rows.Count; i++)
                    {
                        if (Convert.ToString(dtf.Rows[0]["msg_status"]) == "1")
                        {
                            UOBJ.UserID = Convert.ToInt64(dtf.Rows[i]["UserID"]);
                            UOBJ.Name = Convert.ToString(dtf.Rows[i]["Name"]);
                            UOBJ.EmailId = Convert.ToString(dtf.Rows[i]["EmailId"]);
                            UOBJ.Mobile = Convert.ToString(dtf.Rows[i]["Mobile"]);
                            UOBJ.Gender = Convert.ToString(dtf.Rows[i]["Gender"]);
                            UOBJ.Aadhar_ID = Convert.ToString(dtf.Rows[i]["Aadhar_ID"]);
                            UOBJ.RoleId = Convert.ToString(dtf.Rows[i]["RoleId"]);
                            UOBJ.Desig_Name = Convert.ToString(dtf.Rows[i]["Desig_Name"]);
                            UOBJ.OfficeName = Convert.ToString(dtf.Rows[i]["OfficeName"]);
                            UOBJ.Ssoid = Convert.ToString(dtf.Rows[i]["Ssoid"]);
                            Session["SSOID"] = Convert.ToString(dtf.Rows[i]["Ssoid"]);
                        }
                        else
                        {
                            TempData["msg1"] = Convert.ToString(dtf.Rows[i]["msg"]);
                            TempData["isError"] = Convert.ToBoolean(Convert.ToString(dtf.Rows[i]["msg_status"]));
                            UOBJ = new USERDETAILS_MultipleOffice();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(UOBJ);
        }

        public ActionResult MultipleOfficeMapping()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            OneClickAccessRights obj = new OneClickAccessRights();
            ViewBag.lstForestBoundaries = lstofc;
            ViewBag.lstOfficeID = lstofc;

            DataSet dtf = new DataSet();
            try
            {
                dtf = obj.Select_AllPlaceDesignationsRoles();
                foreach (DataRow dr in dtf.Tables[0].Rows)
                {
                    lstPLACES.Add(new SelectListItem { Text = @dr["PLACENAME"].ToString(), Value = @dr["PLACEID"].ToString() });
                }

                obj.ListPLACEIDs = lstPLACES;

                foreach (DataRow dr in dtf.Tables[1].Rows)
                {
                    lstDesignation.Add(new SelectListItem { Text = @dr["DesigName"].ToString(), Value = @dr["DesigId"].ToString() });
                }

                obj.ListDESIGNATIONs = lstDesignation;

                foreach (DataRow dr in dtf.Tables[2].Rows)
                {
                    lstISactive.Add(new SelectListItem { Text = @dr["RoleName"].ToString(), Value = @dr["RoleId"].ToString() });
                }

                obj.ListRoleIds = lstISactive;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_MultipleOfficeMapping", obj);

        }

        [HttpPost]
        public ActionResult SubmitMultipleOffice(OneClickAccessRights model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                model.OLDSSOID = Convert.ToString(Session["SSOID"]);
                model.USERID = UserID;
                model.PLACEIDstr = model.PLACEIDs == null ? "" : string.Join(",", model.PLACEIDs);
                model.RoleIdstr = model.RoleIds == null ? "" : string.Join(",", model.RoleIds);
                status = Convert.ToString(model.InsertMultipleOfficeMapping(model).Rows[0][0]);
                TempData["msg1"] = status;
                TempData["isError"] = Convert.ToBoolean(0);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Get_SSODetails");
        }

        public ActionResult _SSODetailMultipleOffice(string ssoid)
        {
            Designations obj = new Designations();
            DataTable dtf = obj.Select_SSODETAILS_MultiOffice(ssoid);

            List<USERDETAILS_MultipleOffice> UOBJ = new List<USERDETAILS_MultipleOffice>();
            if (dtf.Rows.Count > 0)
            {
                int j = 1;
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    if (Convert.ToString(dtf.Rows[0]["msg_status"]) == "0")
                    {
                        ViewBag.Rstatus = Convert.ToString(dtf.Rows[0]["Ssoid"]);
                        Session["SSOID"] = Convert.ToString(dtf.Rows[0]["Ssoid"]);
                        UOBJ.Add(new USERDETAILS_MultipleOffice
                        {
                            SNO = j++,
                            UserID = Convert.ToInt64(dtf.Rows[i]["UserID"]),
                            Name = Convert.ToString(dtf.Rows[i]["Name"]),
                            EmailId = Convert.ToString(dtf.Rows[i]["EmailId"]),
                            Mobile = Convert.ToString(dtf.Rows[i]["Mobile"]),
                            Gender = Convert.ToString(dtf.Rows[i]["Gender"]),
                            Aadhar_ID = Convert.ToString(dtf.Rows[i]["Aadhar_ID"]),
                            RoleId = Convert.ToString(dtf.Rows[i]["RoleId"]),
                            Desig_Name = Convert.ToString(dtf.Rows[i]["Desig_Name"]),
                            OfficeName = Convert.ToString(dtf.Rows[i]["OfficeName"]),
                            Ssoid = Convert.ToString(dtf.Rows[i]["Ssoid"])
                        });

                    }
                    else
                    {
                        Session.Remove("SSOID");
                        TempData["msg"] = Convert.ToString(dtf.Rows[i]["msg"]);
                        TempData["isError1"] = Convert.ToBoolean(dtf.Rows[i]["msg_status"]);
                        ViewBag.ReturnMsg = TempData["msg"];
                        ViewBag.IsError = TempData["isError1"];
                    }
                }
            }
            return PartialView(UOBJ);
        }

        public ActionResult RemoveTmpSSOID(OneClickAccessRights model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                model.OLDSSOID = Convert.ToString(Session["SSOID"]);
                model.USERID = UserID;
                model.PLACEIDstr = model.PLACEIDs == null ? "" : string.Join(",", model.PLACEIDs);
                model.RoleIdstr = model.RoleIds == null ? "" : string.Join(",", model.RoleIds);
                status = Convert.ToString(model.RemoveTmpSSOID(model).Rows[0][0]);
                TempData["msg1"] = status;
                TempData["isError"] = Convert.ToBoolean(0);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View("Get_SSODetails");
        }
        #endregion
    }
}
