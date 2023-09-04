//*********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS)
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : System Administration Controller
//  Description  : File contains calling functions from UI for System Administration
//  Date Created : 24-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  :
//  Modified On  :
//  Reviewed By  :
//  Reviewed On  :
//*********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Filters;


namespace FMDSS.Controllers.Admin
{
    [MyAuthorization]
    public class SystemAdminController : BaseController
    {
        #region Data Members

        // GET: /SystemAdmin/
        BindMasterData masterObj = new BindMasterData();
        UserApproval uAObj = new UserApproval();

        #endregion

        #region Member Functions

        /// <summary>
        /// Renders UI for System Admin
        /// </summary>
        /// <returns>View for  System Admin</returns>
        public ActionResult SystemAdmin()
        {
            return View();
        }

        /// <summary>
        /// Method responsisble for fetching Permissions
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPermission(int moduleId, int ServiceId)
        {
            DataTable dt = masterObj.GetPermission(moduleId, ServiceId);
            ViewBag.ForestOffices = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["CODE"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Method responsisble for fetching SubPermissions
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <param name="PermissionId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSubPermission(int moduleId, int ServiceId, int PermissionId)
        {
            DataTable dt = masterObj.GetSubPermission(moduleId, ServiceId, PermissionId);
            ViewBag.ForestOffices = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["CODE"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Method responsisble for fetching Office Levels
        /// </summary>
        /// <param name="OfficeLevel"></param>
        /// <returns></returns>
        [HttpPost]
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

        /// <summary>
        /// Method responsisble for fetching forest offices
        /// </summary>
        /// <param name="ForestCode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method responsisble for fetching forest Employees for particular office
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <param name="PermissionId"></param>
        /// <param name="SubPermissionId"></param>
        /// <param name="officeCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getForestEmployees(int moduleId, int ServiceId, int PermissionId, int SubPermissionId, string officeCode)
        {
            List<UserApproval> uA = new List<UserApproval>();
            DataTable dt = masterObj.getForestEmployees(moduleId, ServiceId, PermissionId, SubPermissionId, officeCode);
            ViewBag.ForestOfficers = dt;
            foreach (DataRow dr in dt.Rows)
            {
                uA.Add(new UserApproval()
                {
                    UserId = dr["UserID"].ToString(),
                    Designation = dr["Designation"].ToString(),
                    EMPNAME = dr["EMPNAME"].ToString(),
                    EMPSSOID = dr["SSO_ID"].ToString(),
                    EMPDESIGNATION = dr["EMPDESIGNATION"].ToString(),
                    IsApprover = Convert.ToBoolean(dr["IsApprover"]),
                    IsReviewer = Convert.ToBoolean(dr["IsReviewer"])
                });
            }
            return Json(uA, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method responsisble to save reviewing / approving authority into DB
        /// </summary>
        /// <param name="uA"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveUserApproval(UserApproval uA)
        {
            Int64 result = uAObj.SaveUserApproval(uA);
            if (result > 0)
            {
                Session["ActionStatus"] = "Reviewer/Approver for selected permission has been updated sucessfully!!";
                //return RedirectToAction("SystemAdmin", "SystemAdmin", false);
            }
            else
            {
                Session["ActionStatus"] = "failed";
                // return null;
            }
            return Session["ActionStatus"].ToString();
            //return Json(uA, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

}
