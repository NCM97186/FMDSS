

// *****************************************************************************************
// Author : Rajkumar Singh
// Created : 30-Dec-2015
// *****************************************************************************************
// <summary>This Controller is Created for Add Project in Forest Development</summary>
// *****************************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.ForestDevelopment;
using System.IO;
using FMDSS.Filters;
using System.Configuration;

namespace FMDSS.Controllers.ForestDevelopment
{
     [MyAuthorization]
    public class FdmAddProjectController : BaseController
    {
      /// <summary>
      /// Return add project view with project,Scheme and model
      /// </summary>
      /// <returns></returns>
        public ActionResult FdmAddProject()
        {
            List<FdmAddProject> Projlst = new List<FdmAddProject>();
            try
            {
                FdmAddProject AP = new FdmAddProject();
                DataSet dsScheme = new DataSet();
                DataSet dsProject = new DataSet();
                DataSet dsModel = new DataSet();
                DataSet dsDivision = new DataSet();           
                List<SelectListItem> Schemelst = new List<SelectListItem>();              
                List<SelectListItem> modellst = new List<SelectListItem>();
                List<SelectListItem> divisionlst = new List<SelectListItem>();  
                dsProject = AP.BindProject();
                foreach (System.Data.DataRow dr in dsProject.Tables[0].Rows)
                {
                    Projlst.Add(new FdmAddProject
                    {
                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        Project_Name = dr["Project_Name"].ToString(),
                        AreaofRolloutinSQKM = Convert.ToDecimal(dr["AreaofRolloutinSQKM"].ToString()),
                        Scheme_Id = Convert.ToInt64(dr["Scheme_Id"].ToString()),
                        ReferenceNo = dr["ReferenceNo"].ToString(),
                        StartDate1 = dr["StartDate"].ToString(),
                        EndDate1 = dr["EndDate"].ToString(),
                        EstimatedBudget = Convert.ToDecimal(dr["EstimatedBudget"].ToString()),
                        Model_Code = dr["Model_Code"].ToString(),
                        StatusDesc = dr["StatusDesc"].ToString(),
                    });
                }
                //ViewData["projectlist"] = Projlst;
                dsScheme = AP.BindScheme();
                foreach (System.Data.DataRow dr in dsScheme.Tables[0].Rows)
                {
                    Schemelst.Add(new SelectListItem { Text = dr["Scheme_Name"].ToString(), Value = dr["Scheme_Id"].ToString() });
                }
                ViewBag.Scheme = Schemelst;

                dsModel = AP.BindModel();
                foreach (System.Data.DataRow dr in dsModel.Tables[0].Rows)
                {
                    modellst.Add(new SelectListItem { Text = dr["Model_Name"].ToString(), Value = dr["Model_Id"].ToString() });
                }
                ViewBag.Model = modellst;

                dsDivision = AP.BindDivision();
                foreach (System.Data.DataRow dr in dsDivision.Tables[0].Rows)
                {
                    divisionlst.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["ADMIN_DIST_MAPPED"].ToString() });
                }
                ViewBag.Division = divisionlst;
            }
            catch (Exception ex) { Console.Write("Error " + ex); }

            return View(Projlst);
        }

        [HttpPost]
        public ActionResult submit(FormCollection frm,string Command, HttpPostedFileBase Upload_Button, HttpPostedFileBase Upload_DPR)
        {
            try {
                if (Command == "submit")
                {
                    FdmScheme AS = new FdmScheme();
                    string RefDocument = string.Empty;
                    string DPRDocument = string.Empty;
                    string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
                    var path = "";
                    FdmAddProject ADP = new FdmAddProject();
                    ADP.ID = Convert.ToInt64(frm["hdn_id"].ToString());
                    ADP.Project_Name = frm["ProjectName"].ToString();
                    ADP.Scheme_Id = Convert.ToInt64(frm["Dropscheme"].ToString());
                   // ADP.Model_Code = frm["Dropmodel"].ToString();
                    if (frm["Dropdivision"] == null)
                    {
                        ADP.Dist_Code = "";
                    }
                    else
                    {
                        ADP.Dist_Code = frm["Dropdivision"];
                    }
                    if (frm["Dropmodel"] == null)
                    {
                        ADP.Model_Code = "";
                    }
                    else
                    {
                        ADP.Model_Code = frm["Dropmodel"].ToString();
                    } 
                    ADP.AreaofRolloutinSQKM = Convert.ToDecimal(frm["ArRollout"]);
                    ADP.ReferenceNo = frm["RefNodoc"].ToString();
                    if (Upload_Button != null && Upload_Button.ContentLength > 0)
                    {
                        RefDocument = Path.GetFileName(Upload_Button.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + RefDocument;
                        path = Path.Combine(FilePath, FileFullName);
                        ADP.RefDocument = path;
                        Upload_Button.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        ADP.RefDocument = "";
                    }
                    if (Upload_DPR != null && Upload_DPR.ContentLength > 0)
                    {
                        DPRDocument = Path.GetFileName(Upload_DPR.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + DPRDocument;
                        path = Path.Combine(FilePath, FileFullName);
                        ADP.DPRDocument = path;
                        Upload_Button.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        ADP.DPRDocument = "";
                    }
                    ADP.EstimatedBudget = Convert.ToDecimal(frm["EBudget"].ToString());
                    ADP.StartDate = DateTime.ParseExact(frm["Startdate"].ToString(), "dd/MM/yyyy", null);
                    ADP.EndDate = DateTime.ParseExact(frm["Enddate"].ToString(), "dd/MM/yyyy", null);
                    //ADP.Keybeneficial = frm["KeyBenefical"].ToString();
                    //ADP.Keyactivity = frm["KeyActivities"].ToString();
                    ADP.EnteredBy = Convert.ToInt64(Session["UserId"]);
                    ADP.UpdatedBy = Convert.ToInt64(Session["UserId"]);
                    ADP.IsActive = true;
                    ADP.Status = 1;
                    Int64 sts = ADP.InsertProject();
                }
            }
            catch (Exception ex) { Console.Write("Error " + ex); }            
            return RedirectToAction("FdmAddProject");
        }
        [HttpPost]
        public JsonResult Edit(string ProjectId)
        {
            FdmAddProject AP = null;
            FdmAddProject APObj = new FdmAddProject();
            DataSet ds = new DataSet();
            APObj.ID = Convert.ToInt64(ProjectId);
            ds = APObj.EditProject();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    AP = new FdmAddProject();
                    AP.Project_Name = ds.Tables[0].Rows[0]["Project_Name"].ToString();
                    DateTime _date1 = DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    DateTime _date2 = DateTime.Parse(ds.Tables[0].Rows[0]["EndDate"].ToString());
                    AP.StartDate1 = _date1.ToString("dd/MM/yyyy");
                    AP.EndDate1 = _date2.ToString("dd/MM/yyyy");
                    AP.AreaofRolloutinSQKM = Convert.ToDecimal(ds.Tables[0].Rows[0]["AreaofRolloutinSQKM"].ToString());
                    AP.Scheme_Id = Convert.ToInt64(ds.Tables[0].Rows[0]["Scheme_Id"].ToString());
                    //AP.Keybeneficial = ds.Tables[0].Rows[0]["Keybeneficial"].ToString();
                    //AP.Keyactivity = ds.Tables[0].Rows[0]["Keyactivity"].ToString();
                    AP.RefDocument = ds.Tables[0].Rows[0]["RefDocument"].ToString();
                    AP.ReferenceNo = ds.Tables[0].Rows[0]["ReferenceNo"].ToString();
                    //AP.Model_Code = ds.Tables[0].Rows[0]["Model_Code"].ToString();
                    //AP.Dist_Code = ds.Tables[0].Rows[0]["Dist_Code"].ToString();
                    AP.Model_Code = ds.Tables[0].Rows[0]["Model"].ToString();
                    AP.Dist_Code = ds.Tables[0].Rows[0]["Dist"].ToString();
                    AP.EstimatedBudget = Convert.ToDecimal(ds.Tables[0].Rows[0]["EstimatedBudget"].ToString());
                    AP.DPRDocument = ds.Tables[0].Rows[0]["DPRDocument"].ToString();
                    AP.SchemeBudget = Convert.ToDecimal(ds.Tables[0].Rows[0]["Budget"].ToString());
                    AP.Status = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString()); 
                }
            }
            return Json(AP, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetModel(string SchemeId)
        {
            FdmAddProject AP = null;
            FdmAddProject APObj = new FdmAddProject();
            DataSet ds = new DataSet();
            List<SelectListItem> lstModel = new List<SelectListItem>();
            List<SelectListItem> lstDist = new List<SelectListItem>();
            APObj.ID = Convert.ToInt64(SchemeId);
            ds = APObj.GetModel(APObj.ID);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    //AP.Model_Code = ds.Tables[0].Rows[0]["Model_Code"].ToString();
                    //AP.Dist_Code = ds.Tables[0].Rows[0]["Dist_Code"].ToString();
                    //AP.SchemeBudget = Convert.ToDecimal(ds.Tables[0].Rows[0]["Budget"].ToString());

                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                    {

                        lstModel.Add(new SelectListItem { Text = @dr["Model_Name"].ToString(), Value = @dr["Model_Id"].ToString() });
                    }

                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dr in ds.Tables[1].Rows)
                    {
                        lstDist.Add(new SelectListItem { Text = @dr["DIST_Name"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    AP = new FdmAddProject();
                    AP.SchemeBudget = Convert.ToDecimal(ds.Tables[2].Rows[0][0].ToString());
                }
            }
            return Json(new { list1 = lstModel, list2 = lstDist, list3 = AP.SchemeBudget }, JsonRequestBehavior.AllowGet);     
        }
        [HttpPost]
        public JsonResult ToolTip(string ModelId)
        {
            FdmAddProject AP = null;
            FdmAddProject APObj = new FdmAddProject();
            DataSet ds = new DataSet();
            List<SelectListItem> lsttool = new List<SelectListItem>();
            ds = APObj.GetToolTipDetail(Convert.ToInt64(ModelId));
            if (ds.Tables[0].Rows.Count > 0)
            {
                AP = new FdmAddProject();
                AP.ActivityName = ds.Tables[0].Rows[0][0].ToString();
                AP.ActivityType = ds.Tables[0].Rows[0][1].ToString();
                AP.ActivityTotalCost = ds.Tables[0].Rows[0][2].ToString();

            }
            return Json(AP, JsonRequestBehavior.AllowGet);
        }

    }
}
