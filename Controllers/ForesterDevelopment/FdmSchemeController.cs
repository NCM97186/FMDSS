
// *****************************************************************************************
// Author : Rajkumar Singh
// Created : 29-Dec-2015
// *****************************************************************************************
// <summary>This Controller is Created for Add Scheme in Forest Development</summary>
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
using FMDSS.Models;
using FMDSS.Models.FmdssContext;

namespace FMDSS.Controllers.ForestDevelopment
{
    [MyAuthorization]
    public class FdmSchemeController : BaseController
    {
        /// <summary>
        /// Return ciew with Scheme details,district and program
        /// </summary>
        public FmdssContext fmdsscontext;

        public FdmSchemeController()
        {
            fmdsscontext = new FmdssContext();
        }
        public ActionResult fdmScheme()
        {
            List<FdmScheme> schemelst = new List<FdmScheme>();
            try
            {
                FdmScheme AS = new FdmScheme();
                List<SelectListItem> districtlst = new List<SelectListItem>();
                List<SelectListItem> Programlst = new List<SelectListItem>();
                List<SelectListItem> Agencylst = new List<SelectListItem>();
                List<SelectListItem> modellst = new List<SelectListItem>();
                List<SelectListItem> budgetheadlst = new List<SelectListItem>();
                DataSet dsdistrict = new DataSet();
                DataSet dsScheme = new DataSet();
                DataSet dsProgram = new DataSet();
                DataSet dsImpAgency = new DataSet();
                DataSet dsModel = new DataSet();
                DataSet dsBudgetHead = new DataSet();
                dsdistrict = AS.BindDistrict();
                ViewBag.dist = dsdistrict.Tables[0];
                foreach (System.Data.DataRow dr in ViewBag.dist.Rows)
                {
                    districtlst.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.district = districtlst;
                dsScheme = AS.BindScheme();
                for (int i = 0; i < dsScheme.Tables.Count; i++)
                {
                    foreach (DataRow dr in dsScheme.Tables[i].Rows)
                    {
                        schemelst.Add(
                           new FdmScheme()
                           {
                               ID = Convert.ToInt64(dr["ID"].ToString()),
                               Scheme_Name = dr["Scheme_Name"].ToString(),
                               // StartDate = dr["StartDate"].ToString(),
                               // EndDate = dr["EndDate"].ToString(),
                               // AreaofRolloutinSQKM = Convert.ToDecimal(dr["AreaofRolloutinSQKM"].ToString()),
                               // Programme = Convert.ToInt64(dr["ProgramId"].ToString()),
                               // District = dr["DIST_CODE"].ToString(),
                               // RefNoRelatedDocument = dr["RefNoRelatedDocument"].ToString()
                               ProgramName = dr["Program_Name"].ToString(),
                               Budget_Head = dr["BudgetHead"].ToString(),
                               Budget = Convert.ToDecimal(dr["Budget"].ToString()),
                               StatusDesc = dr["StatusDesc"].ToString(),
                           });
                    }
                }
                // ViewData["schemelist"] = schemelst;
                dsProgram = AS.BindProgram();
                foreach (System.Data.DataRow dr in dsProgram.Tables[0].Rows)
                {
                    Programlst.Add(new SelectListItem { Text = @dr["Program_Name"].ToString(), Value = @dr["ProgramId"].ToString() });
                }
                ViewBag.Program = Programlst;

                dsImpAgency = AS.BindImplementAgency();
                foreach (System.Data.DataRow dr in dsImpAgency.Tables[0].Rows)
                {
                    Agencylst.Add(new SelectListItem { Text = @dr["AgencyName"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.Agency = Agencylst;
                //Defect log Id-237 Done by Rajkumar
                dsModel = AS.BindModel();
                foreach (System.Data.DataRow dr in dsModel.Tables[0].Rows)
                {
                    modellst.Add(new SelectListItem { Text = dr["Model_Name"].ToString(), Value = dr["Model_Id"].ToString() });
                }
                ViewBag.Model = modellst;

                dsBudgetHead = AS.BindBudget();
                foreach (System.Data.DataRow dr in dsBudgetHead.Tables[0].Rows)
                {
                    budgetheadlst.Add(new SelectListItem { Text = @dr["BudgetHead"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.Budget = budgetheadlst;

            }
            catch (Exception ex)
            {
                Console.Write("Error " + ex);
            }
            return View(schemelst);
        }
        /// <summary>
        /// On button click function for data submit
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="Upload_Button"></param>
        /// <returns></returns>
        /// Defect log Id-242 Done by Rajkumar
        [HttpPost]
        public ActionResult submit(FormCollection frm, HttpPostedFileBase Upload_Button, HttpPostedFileBase AdminApproval_doc, HttpPostedFileBase FApproval_doc, string Command)
        {
            try
            {
                if (Command == "submit")
                {
                    FdmScheme AS = new FdmScheme();
                    string RefDocument = string.Empty;
                    string Admin_Approval_Doc = string.Empty;
                    string Financial_Approval_Doc = string.Empty;
                    string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
                    var path = "";
                    AS.ID = Convert.ToInt64(frm["hdn_id"].ToString());
                    if (frm["Dropdistrict"] == null)
                    {
                        AS.District = "";
                    }
                    else
                    {
                        AS.District = frm["Dropdistrict"];
                    }
                    AS.Scheme_Name = frm["SchemeName"];
                    AS.Programme = Convert.ToInt64(frm["Dropprogram"]);
                    if (frm["Dropmodel"] == null)
                    {
                        AS.Model_Code = "";
                    }
                    else
                    {
                        AS.Model_Code = frm["Dropmodel"].ToString();
                    }
                    AS.AreaofRolloutinSQKM = Convert.ToDecimal(frm["ArRollout"]);
                    AS.RefNoRelatedDocument = frm["RefNodoc"];
                    if (Upload_Button != null && Upload_Button.ContentLength > 0)
                    {
                        RefDocument = Path.GetFileName(Upload_Button.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + RefDocument;
                        path = Path.Combine(FilePath, FileFullName);
                        AS.RefDocument = path;
                        Upload_Button.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        AS.RefDocument = "";
                    }
                    if (AdminApproval_doc != null && AdminApproval_doc.ContentLength > 0)
                    {
                        Admin_Approval_Doc = Path.GetFileName(AdminApproval_doc.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + Admin_Approval_Doc;
                        path = Path.Combine(FilePath, FileFullName);
                        AS.Administrative_Approval_Document = path;
                        AdminApproval_doc.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        AS.Administrative_Approval_Document = "";
                    }
                    if (FApproval_doc != null && FApproval_doc.ContentLength > 0)
                    {
                        Financial_Approval_Doc = Path.GetFileName(FApproval_doc.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + Financial_Approval_Doc;
                        path = Path.Combine(FilePath, FileFullName);
                        AS.Financial_Approval_Document = path;
                        FApproval_doc.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        AS.Financial_Approval_Document = "";
                    }
                    AS.StartDate1 = DateTime.ParseExact(frm["Startdate"].ToString(), "dd/MM/yyyy", null);
                    AS.EndDate1 = DateTime.ParseExact(frm["Enddate"].ToString(), "dd/MM/yyyy", null);
                    AS.ImpAgency = frm["dropImpAgency"];
                    //Defect log Id-225 Done by Rajkumar
                    AS.Budget_Head = frm["DropBudgethead"].ToString();
                    AS.Budget = Convert.ToDecimal(frm["txtBudget"].ToString());
                    AS.Administrative_Approval = frm["AdminApproval"].ToString();
                    AS.Financial_Approval = frm["FinancialApproval"].ToString();
                    if (frm["Aapprovaldate"].ToString() != "" && frm["Aapprovaldate"].ToString() != null)
                    {
                        AS.Administrative_Approval_Date = frm["Aapprovaldate"].ToString();
                    }
                    AS.Financial_Approval = frm["FinancialApproval"].ToString();
                    if (frm["Fapprovaldate"].ToString() != "" && frm["Fapprovaldate"].ToString() != null)
                    {
                        AS.Financial_Approval_Date = frm["Fapprovaldate"].ToString();
                    }
                    AS.EnteredBy = Convert.ToInt64(Session["UserId"]);
                    AS.UpdatedBy = Convert.ToInt64(Session["UserId"]);
                    AS.IsActive = true;
                    AS.Status = 1;
                    Int64 sts = AS.InsertScheme();
                    AS.SCHEME_ID = sts;
                    AS.InsertSchemeDistrict();
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error " + ex);
            }
            return RedirectToAction("fdmScheme");
        }
        [HttpPost]
        public JsonResult Edit(string SchemeId)
        {
            FdmScheme AS = null;
            FdmScheme ASObj = new FdmScheme();
            DataSet ds = new DataSet();
            ASObj.ID = Convert.ToInt64(SchemeId);
            ds = ASObj.EditScheme();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    AS = new FdmScheme();
                    AS.Scheme_Name = ds.Tables[0].Rows[0]["Scheme_Name"].ToString();
                    DateTime _date1 = DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    DateTime _date2 = DateTime.Parse(ds.Tables[0].Rows[0]["EndDate"].ToString());
                    AS.StartDate = _date1.ToString("dd/MM/yyyy");
                    AS.EndDate = _date2.ToString("dd/MM/yyyy");
                    AS.AreaofRolloutinSQKM = Convert.ToDecimal(ds.Tables[0].Rows[0]["AreaofRolloutinSQKM"].ToString());
                    AS.ImpAgency = ds.Tables[0].Rows[0]["ImplementingAgency"].ToString();
                    AS.Keybeneficial = ds.Tables[0].Rows[0]["Keybeneficial"].ToString();
                    AS.Keyactivity = ds.Tables[0].Rows[0]["Keyactivity"].ToString();
                    AS.RefDocument = ds.Tables[0].Rows[0]["RefDocument"].ToString();
                    AS.RefNoRelatedDocument = ds.Tables[0].Rows[0]["RefNoRelatedDocument"].ToString();
                    AS.District = ds.Tables[0].Rows[0]["DIST_CODE"].ToString();
                    AS.Model_Code = ds.Tables[0].Rows[0]["Model_Code"].ToString();
                    AS.ProgramName = ds.Tables[0].Rows[0]["Program_Name"].ToString();
                    AS.Budget_Head = ds.Tables[0].Rows[0]["BudgetHead"].ToString();
                    AS.Budget = Convert.ToDecimal(ds.Tables[0].Rows[0]["Budget"].ToString());
                    AS.Administrative_Approval = ds.Tables[0].Rows[0]["Administrative_Approval"].ToString();

                    if (ds.Tables[0].Rows[0]["Administrative_Approval_date"].ToString() != "" && ds.Tables[0].Rows[0]["Administrative_Approval_date"].ToString() != null)
                    {
                        DateTime _date3 = DateTime.Parse(ds.Tables[0].Rows[0]["Administrative_Approval_date"].ToString());
                        AS.Administrative_Approval_Date1 = _date3.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        AS.Administrative_Approval_Date1 = "";
                    }
                    if (ds.Tables[0].Rows[0]["Financial_Approval_Date"].ToString() != "" && ds.Tables[0].Rows[0]["Financial_Approval_Date"].ToString() != null)
                    {

                        DateTime _date4 = DateTime.Parse(ds.Tables[0].Rows[0]["Financial_Approval_Date"].ToString());
                        AS.Financial_Approval_Date1 = _date4.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        AS.Financial_Approval_Date1 = "";
                    }

                    AS.Administrative_Approval_Document = ds.Tables[0].Rows[0]["Administrative_Approval_Document"].ToString();
                    AS.Financial_Approval = ds.Tables[0].Rows[0]["Financial_Approval"].ToString();
                    AS.Financial_Approval_Document = ds.Tables[0].Rows[0]["Financial_Approval_Document"].ToString();
                    AS.Status = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString());
                }
            }
            return Json(AS, JsonRequestBehavior.AllowGet);
        }

        #region New WideLife Scheme Developed by Rajveer
        public ActionResult AddScheme()
        {
            Session["SchemeMappingModelFilter"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SchemeModel model = new SchemeModel();
            try
            {
                model.StartDate = Convert.ToString("01/04/" + DateTime.Now.Year);
                model.EndDate = Convert.ToString("31/03/" + DateTime.Now.AddYears(1).Year);
                SchemeRepo repo = new SchemeRepo();
                DataSet dsdistrict = new DataSet();
                dsdistrict = repo.BindDistrict();
                ViewBag.district = new SelectList(dsdistrict.Tables[0].AsDataView(), "DIST_CODE", "DIST_NAME");


                ViewBag.BudgetHeadList = fmdsscontext.tbl_mst_BudgetHead.OrderBy(s => s.BudgetHead).Select(i => new SelectListItem() { Text = i.BudgetHead, Value = i.ID.ToString() }).ToList();
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.ActivityList = fmdsscontext.tbl_mst_ActivityForWidelife.OrderBy(s => s.Activity_Name).Select(i => new SelectListItem() { Text = i.Activity_Name, Value = i.ID.ToString() }).ToList();
                ViewBag.SubActivityList = new List<SelectListItem>();

                #region Fill Circle Division and Santatury
                ViewBag.Circle = fmdsscontext.tbl_mst_Forest_WildLifeCircles.Where(s => s.isBOTH == 1).OrderBy(s => s.CIRCLE_NAME).Select(i => new SelectListItem() { Text = i.CIRCLE_NAME, Value = i.CIRCLE_CODE });
                ViewBag.DivisionList = new List<SelectListItem>();
                ViewBag.SanctuaryList = new List<SelectListItem>();
                #endregion

                DataSet ds = new DataSet();
                ds = repo.InsertScheme(model, "LIST", Convert.ToInt64(Session["UserID"]));

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    #region Marge Data
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Scheme>>(str);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddScheme(SchemeModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                SchemeRepo repo = new SchemeRepo();
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(model.ID))
                {
                    ds = repo.InsertScheme(model, "UPDATE", Convert.ToInt64(Session["UserID"]));
                }
                else
                {
                    ds = repo.InsertScheme(model, "INSERT", Convert.ToInt64(Session["UserID"]));
                }

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>" + Convert.ToString(ds.Tables[0].Rows[0]["Status"]) + "</div>";

                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return RedirectToAction("AddScheme");
        }


        [HttpPost]
        public JsonResult MapScheme(SchemeMapping model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SchemeMapping> list = new List<SchemeMapping>();
            try
            {
                SchemeRepo repo = new SchemeRepo();
                DataSet ds = new DataSet();

                #region Maintain Duplicate Report
                if (Session["SchemeMappingModelFilter"] != null)
                {
                    list = (List<SchemeMapping>)Session["SchemeMappingModelFilter"];
                }
                else
                {
                    Session["SchemeMappingModelFilter"] = new List<SchemeMapping>();
                }
                #endregion


                int count = list.Where(s => s.ID == model.ID && s.BudgetHead == model.BudgetHead && s.Activity == model.Activity && s.SubActivity == model.SubActivity && s.ISCircleDivision == model.ISCircleDivision && s.CIRCLE_CODE == model.CIRCLE_CODE && s.Division == model.Division && s.SanctuaryCode == model.SanctuaryCode).Count();
                if (count == 0)
                {
                    ds = repo.InsertSchemeMapping(model, "INSERT", Convert.ToInt64(Session["UserID"]), string.Empty);
                    model.MappingId = Convert.ToInt32(ds.Tables[0].Rows[0]["MappingID"]);
                    list.Add(model);
                    Session["SchemeMappingModelFilter"] = list;
                }
                else
                {
                    model.MappingId = -1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GETMapScheme(string SchemeID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SchemeMapping> list = new List<SchemeMapping>();
            SchemeModel model = new SchemeModel();
            try
            {
                SchemeRepo repo = new SchemeRepo();
                DataSet ds = new DataSet();
                model.ID = Convert.ToString(SchemeID);
                ds = repo.InsertSchemeMapping(model, "GETMAPPEDSCHEME", Convert.ToInt64(Session["UserID"]), string.Empty);

                string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SchemeMapping>>(str);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult CreateSchemeMapping(string ApprovedIds)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SchemeMapping model = new SchemeMapping();
            try
            {
                SchemeRepo repo = new SchemeRepo();
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(ApprovedIds))
                {
                    ApprovedIds = ApprovedIds.Remove(0, 1);
                    ds = repo.InsertSchemeMapping(model, "Approved", Convert.ToInt64(Session["UserID"]), ApprovedIds);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>" + Convert.ToString(ds.Tables[0].Rows[0]["Status"]) + "</div>";

                    }
                    else
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                    }
                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return RedirectToAction("AddScheme");
        }

        [HttpPost]
        public JsonResult DeleteSchemeMapping(int MappingID)
        {
            long status = 0;
            List<SchemeMapping> list = new List<SchemeMapping>();
            SchemeRepo repo = new SchemeRepo();
            DataSet ds = new DataSet();
            SchemeMapping model = new SchemeMapping();
            try
            {
                if (MappingID != 0)
                {
                    #region Maintain Duplicate Report
                    if (Session["SchemeMappingModelFilter"] != null)
                    {
                        list = (List<SchemeMapping>)Session["SchemeMappingModelFilter"];
                        list.RemoveAll(s => s.MappingId == MappingID);
                    }
                    #endregion
                    model.MappingId = MappingID;
                    ds = repo.InsertSchemeMapping(model, "DELETE", Convert.ToInt64(Session["UserID"]), string.Empty);
                    status = Convert.ToInt32(ds.Tables[0].Rows[0]["MappingID"]);// (-2 Means Deleted)
                }
                else
                {
                    status = 0;
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult EDITMapping(SchemeMapping model)
        {
            long status = 0;
            List<SchemeMapping> list = new List<SchemeMapping>();
            SchemeRepo repo = new SchemeRepo();
            DataSet ds = new DataSet();
            try
            {
                if (model.MappingId != 0)
                {
                    ds = repo.InsertSchemeMapping(model, "EDITMAPPING", Convert.ToInt64(Session["UserID"]), string.Empty);
                    status = Convert.ToInt32(ds.Tables[0].Rows[0]["MappingID"]);// (-2 Means Deleted and -3 Means Updated)
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>" + Convert.ToString(ds.Tables[0].Rows[0]["msg"]) + "</div>";
                }
                else
                {
                    status = 0;
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
