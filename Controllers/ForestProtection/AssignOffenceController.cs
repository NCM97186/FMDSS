
//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         :  ForesterParivadRegistration
//  Description  : File contains assinging of offense by forester
//  Date Created : 24-09-2016
//  History      :
//  Version      : 1.0
//  Author       : Rajkumar
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using FMDSS.Models.ForestProtection;
using System.Data.SqlClient;
using System.IO;

namespace FMDSS.Controllers.ForestProtection
{
    public class AssignOffenceController : BaseController
    {
        //
        // GET: /AssignOffence/
        int ModuleID = 4;
        /// <summary>
        /// Return view for assign offense
        /// </summary>
        /// <returns></returns>
        public ActionResult AssignOffence()
        {
            DataTable dtOfficerDesignation = new DataTable();
            List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
            AssignOffence _objModel = new AssignOffence();
            List<AssignOffence> Offenderdata = new List<AssignOffence>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                DataSet dtf = _objModel.GetViewExistingRecords();

                foreach (DataRow dr in dtf.Tables[0].Rows)
                    Offenderdata.Add(
                        new AssignOffence()
                        {
                            District = dr["District"].ToString(),
                            OffenseCode = dr["OffenseCode"].ToString(),
                            OffensePlace = dr["OffensePlace"].ToString(),
                            OffenseDate = dr["OffenseDate"].ToString(),
                           // OffenseTime = dr["OffenseTime"].ToString(),
                            AssignTo = dr["AssignTo"].ToString(),
                            AssignDate=dr["AssignDate"].ToString(),
                            OffenseDescription = dr["Description"].ToString()
                        });
                ViewData["OffenderList"] = Offenderdata;

                dtOfficerDesignation = _objModel.GetOfficerDesignation();
                foreach (System.Data.DataRow dr in dtOfficerDesignation.Rows)
                {
                    lstOfficerDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["EmpDesignation"].ToString() });
                }
                ViewBag.OfficerDesignation = lstOfficerDesignation;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View();
        }

        /// <summary>
        /// function to bind forest officer in drop down
        /// </summary>
        /// <param name="designation"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getForestOfficer(string designation)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> lstOfficer = new List<SelectListItem>();
            try
            {
                DAL dal = new DAL();
                DataSet dsOfficerDesig = new DataSet();
                SqlParameter[] paramBlock = { new SqlParameter("@option", "2"), 
                                             new SqlParameter("@ssoid", Session["SSOid"]), 
                                             new SqlParameter("@EmpDesig", designation), 
                                            };
                dal.Fill(dsOfficerDesig, "Sp_FPM_GetFOfficerDesig", paramBlock);
                ViewBag.fname = dsOfficerDesig.Tables[0];

                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    if (@dr["SSO_ID"].ToString() != "--Select--")
                    {
                        lstOfficer.Add(new SelectListItem { Text = @dr["SSO_ID"].ToString(), Value = @dr["SSO_ID"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(new SelectList(lstOfficer, "Value", "Text"));
        }
        /// <summary>
        /// function to forward offense to forester
        /// </summary>
        /// <param name="form"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(FormCollection form, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            AssignOffence _objModel = new AssignOffence();
            try
            {
                if (Command == "Forward")
                {
                    DataSet ds = new DataSet();
                    _objModel.OffenseCode = form["hdnOffenseCode"].ToString();
                    if (form["AssignDescription"] != null)
                    {
                        _objModel.AssignDescription = form["AssignDescription"].ToString();
                    }
                    else
                    {
                        _objModel.AssignDescription = "";
                    }
                    ds = _objModel.SubmitDFO_Forward(form["dropForester"].ToString());
                }
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return RedirectToAction("AssignOffence");
        }




    }
}
