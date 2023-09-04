using AutoMapper;
using FMDSS.Models;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class BudgetPreSurveyController : Controller
    {
        //
        // GET: /BudgetPreSurvey/

        public FmdssContext fmdsscontext;
        public BudgetPreSurveyController()
        {
            fmdsscontext = new FmdssContext();
        }

        #region Add Pre Survey

        public ActionResult AddPreSurvey()
        {
            ViewBudgetPreSurveyModel model = new ViewBudgetPreSurveyModel();
            try
            {
                Session["AddBudgetPreSurveyList"] = null;
                List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<SelectListItem> lstActivity = new List<SelectListItem>();
                var financialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).ToList();
                foreach (var item in financialYear)
                {
                    lstFinancialYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }

                #region Get Circle and Division with userID
                DataSet MasterCircleDivision = new DataSet();
                MasterCircleDivisionWithUserID repomaster = new MasterCircleDivisionWithUserID();
                MasterCircleDivision = repomaster.Select_CircleDivisionWithUserID("ALL", Convert.ToInt64(Session["UserID"]));
                ViewBag.Circle = new SelectList(MasterCircleDivision.Tables[0].AsDataView(), "Value", "Text");
                ViewBag.Division = new SelectList(MasterCircleDivision.Tables[1].AsDataView(), "Value", "Text");
                ViewBag.RangeList = new SelectList(MasterCircleDivision.Tables[2].AsDataView(), "Value", "Text");
                TempData["PlaceArea"] = Convert.ToString(MasterCircleDivision.Tables[3].Rows[0]["OfficeArea"]);
                #endregion

                #region Get Scheme According to Division
                MasterCircleDivision = repomaster.Select_SchemeDivisionWise("SCHEME", Convert.ToString(MasterCircleDivision.Tables[0].Rows[0]["Value"]), Convert.ToString(MasterCircleDivision.Tables[1].Rows[0]["Value"]), Convert.ToInt64(Session["UserID"]));
                ViewBag.Scheme= new SelectList(MasterCircleDivision.Tables[0].AsDataView(), "Value", "Text");
                #endregion


                ViewBag.FinancialYear = lstFinancialYear;
                ViewBag.BudgetHead = lstBudgetHead;
                ViewBag.Activity = lstActivity;
                
                ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                BudgetPreSurveyRepo repo = new BudgetPreSurveyRepo();
                Session["AddBudgetPreSurveyList"] = repo.GetBudgetPreSurveyList(Convert.ToInt32(Session["UserID"]),"Range");
                ViewData["PreSurveyList"] = Session["AddBudgetPreSurveyList"];



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, 0);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddPreSurvey(ViewBudgetPreSurveyModel model)
        {
            #region Active Status

            long status = 0;
            try
            {
                if (Session["AddBudgetPreSurveyList"] != null)
                {

                    List<ViewBudgetPreSurveyModel> lstBudgetAllocationCircle = (List<ViewBudgetPreSurveyModel>)Session["AddBudgetPreSurveyList"];

                    //var lstFbudget = fQuery.Where(i => lstBudgetAllocationCircle.Any(e => e.FY_ID == i.FY_ID && e.SchemeID == i.SchemeID && i.BudgetHeadID == e.BudgetHeadID && i.SubBudgetHeadID == e.SubBudgetHeadID && i.CIRCLE_CODE == e.CIRCLE_CODE && i.Division == e.Division && i.ISCircleDivision == e.ISCircleDivision));

                    foreach (var items in lstBudgetAllocationCircle)
                    {
                        long id = Convert.ToInt64(items.rowid);
                        tbl_BudgetPreSurvey tblBudgCirUpdate = fmdsscontext.tbl_BudgetPreSurvey.Where(i => i.ID == id).FirstOrDefault();

                        if (tblBudgCirUpdate != null)
                        {
                            tblBudgCirUpdate.isActive = 1;
                            tblBudgCirUpdate.UpdatedBy = Convert.ToInt32(Session["UserID"]);
                            tblBudgCirUpdate.UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                            this.fmdsscontext.tbl_BudgetPreSurvey.Add(tblBudgCirUpdate);
                            fmdsscontext.Entry(tblBudgCirUpdate).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    status = fmdsscontext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, 0);
            }


            #endregion
            return RedirectToAction("AddPreSurvey");
        }


        [HttpPost]
        public JsonResult AddBudgetPreSurvey(ViewBudgetPreSurveyModel objModel)
        {
            try
            {

                objModel.ISCircleDivision = "Range"; //This is hard coded bcz that time only range level person is fill this form.In future this form open Division level or circle level ISCircleDivision paramenter pass with View Page

                #region Maintain the Pre Survey List
                List<ViewBudgetPreSurveyModel> BudgetAllocationLists = new List<ViewBudgetPreSurveyModel>();
                if (Session["AddBudgetPreSurveyList"] == null)
                {
                    Session["AddBudgetPreSurveyList"] = new List<ViewBudgetPreSurveyModel>();
                }
                else
                {
                    BudgetAllocationLists = (List<ViewBudgetPreSurveyModel>)Session["AddBudgetPreSurveyList"];
                }
                #endregion

                bool Duplicate = CheckDuplicateRecordsPreSurvey(objModel, BudgetAllocationLists);

                if (Duplicate == false)
                {


                    #region Circle and HQ
                    if (objModel != null && (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle"))
                    {
                        if (objModel.ISCircleDivision == "HQ")
                        {
                            objModel.CIRCLE_CODE = "HQ";
                            objModel.CIRCLE_NAME = "HQ";
                        }

                        tbl_BudgetPreSurvey tbl = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == 1);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>();
                            tbl_BudgetPreSurvey ObjbudgetCircle = Mapper.Map<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>(objModel);
                            ObjbudgetCircle.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                            ObjbudgetCircle.CreatedBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = 0;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);




                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetPreSurveyList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Division
                    if (objModel != null && objModel.ISCircleDivision == "Division")
                    {

                        tbl_BudgetPreSurvey tbl = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.DivisionCode == objModel.DivisionCode && i.ISCircleDivision == "Division" && i.isActive == 1);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>();
                            tbl_BudgetPreSurvey ObjbudgetCircle = Mapper.Map<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>(objModel);
                            ObjbudgetCircle.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                            ObjbudgetCircle.CreatedBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = 0;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetPreSurveyList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Sanctuary
                    if (objModel != null && objModel.ISCircleDivision == "Range")
                    {

                        tbl_BudgetPreSurvey tbl = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.DivisionCode == objModel.DivisionCode && i.ISCircleDivision == "Range" && i.RangeCode == objModel.RangeCode && i.isActive == 1 && i.SiteName.ToLower().Trim()==objModel.SiteName.ToLower().Trim());
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>();
                            tbl_BudgetPreSurvey ObjbudgetCircle = Mapper.Map<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>(objModel);
                            ObjbudgetCircle.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                            ObjbudgetCircle.CreatedBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = 0;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetPreSurveyList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion
                }
                else
                {
                    objModel.rowid = "D";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }


        public bool CheckDuplicateRecordsPreSurvey(ViewBudgetPreSurveyModel objModel, List<ViewBudgetPreSurveyModel> List)
        {
            List<ViewBudgetPreSurveyModel> lstChkDup = new List<ViewBudgetPreSurveyModel>();
            bool duplicate = false;
            #region If Site Name Null Or Empty then Set n/a
            if (string.IsNullOrEmpty(objModel.SiteName))
            {
                objModel.SiteName = "n/a";
            }
            #endregion

            #region Check In Table
           if (objModel != null && objModel.ISCircleDivision == "Range")
            {
                #region Check In Current Session
                int count = List.Where(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.DivisionCode == objModel.DivisionCode && i.ISCircleDivision == "Range" && i.RangeCode == objModel.RangeCode && i.SiteName.ToLower().Trim()==objModel.SiteName.ToLower().Trim()).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetPreSurvey tbl = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.DivisionCode == objModel.DivisionCode && i.ISCircleDivision == "Range" && i.RangeCode == objModel.RangeCode && i.isActive == 1 && i.SiteName.ToLower().Trim() == objModel.SiteName.ToLower().Trim());
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }
            }
            #endregion




            return duplicate;
        }
        #endregion

        public JsonResult DeleteBudgetPreSurvey(long Id)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_BudgetPreSurvey tblAllocationCircle = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ID == Id);

                    if (tblAllocationCircle != null)
                    {
                        tblAllocationCircle.isActive = 0;
                        fmdsscontext.Entry(tblAllocationCircle).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();
                    }
                    else
                    {
                        status = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UploadFiles(string BudgetID)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        string Fullfname = Path.Combine(Server.MapPath("~/BudgetPreSurveyFiles/"), fname);
                        file.SaveAs(Fullfname);

                        #region Upload Files.
                        tbl_BudgetPreSurveyFilesUpload Val = new tbl_BudgetPreSurveyFilesUpload();
                        Val.BudgetPreSurveyId = Convert.ToInt64(BudgetID);
                        Val.FilesName = "~/BudgetPreSurveyFiles/" + fname;
                        Val.CreatedDate = DateTime.Now.ToShortDateString();
                        Val.Createdby = Convert.ToInt64(Session["UserId"]);
                        Val.Status = 1;


                        fmdsscontext.tbl_BudgetPreSurveyFilesUpload.Add(Val);
                        fmdsscontext.SaveChanges();

                        #endregion
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred in File Uploaded time . Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }



        #region Get Budget Perposal Data

        public ActionResult GetPreSurveyBudgetInformation()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBudgetPreSurveyModel model = new ViewBudgetPreSurveyModel();
            try
            {
               
                BudgetPreSurveyRepo repo = new BudgetPreSurveyRepo();
                List<ViewBudgetPreSurveyModel> List = new List<ViewBudgetPreSurveyModel>();
                List = repo.GetBudgetPreSurveyList(Convert.ToInt32(Session["UserID"]), "DIVISION");
                ViewData["BudgetPreSurveyListInfo"] = List;
                Session["BudgetPreSurveyListFilter"] = List;

                #region Get Circle and Division with userID
                List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                var financialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).ToList();
                foreach (var item in financialYear)
                {
                    lstFinancialYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }

                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();
                ViewBag.Activity = new List<SelectListItem>();
                DataSet MasterCircleDivision = new DataSet();
                MasterCircleDivisionWithUserID repomaster = new MasterCircleDivisionWithUserID();
                MasterCircleDivision = repomaster.Select_CircleDivisionWithUserID("ALL", Convert.ToInt64(Session["UserID"]));
                
                TempData["PlaceArea"] = Convert.ToString(MasterCircleDivision.Tables[3].Rows[0]["OfficeArea"]);

                #region Get Scheme According to Division
                MasterCircleDivision = repomaster.Select_SchemeDivisionWise("GetPreSurveyBudgetInformation", Convert.ToString(MasterCircleDivision.Tables[0].Rows[0]["Value"]), Convert.ToString(MasterCircleDivision.Tables[1].Rows[0]["Value"]), Convert.ToInt64(Session["UserID"]));
                ViewBag.SchemeList = new SelectList(MasterCircleDivision.Tables[0].AsDataView(), "Value", "Text");
                ViewBag.RangeList = new SelectList(MasterCircleDivision.Tables[1].AsDataView(), "Value", "Text");
                #endregion
                ViewBag.FinancialYear = lstFinancialYear;
                ViewBag.BudgetHead = lstBudgetHead;
                #endregion

            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult GetPreSurveyBudgetInformation(ViewBudgetPreSurveyModel model)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBudgetPreSurveyModel modeldata = new ViewBudgetPreSurveyModel();
            try
            {
                long status = 0;
                try
                {

                    #region Maintain the Budget Perposal List
                    List<ViewBudgetPreSurveyModel> BudgetPerposalLists = new List<ViewBudgetPreSurveyModel>();
                    if (Session["BudgetPreSurveyListFilter"] == null)
                    {
                        Session["BudgetPreSurveyListFilter"] = BudgetPerposalLists;
                    }
                    else
                    {
                        BudgetPerposalLists = (List<ViewBudgetPreSurveyModel>)Session["BudgetPreSurveyListFilter"];
                    }

                    #region Get Circle and Division with userID
                    List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
                    List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                    var financialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).ToList();
                    foreach (var item in financialYear)
                    {
                        lstFinancialYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                    }

                    ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                    ViewBag.SubActivityList = new List<SelectListItem>();
                    ViewBag.Activity = new List<SelectListItem>();
                    DataSet MasterCircleDivision = new DataSet();
                    MasterCircleDivisionWithUserID repomaster = new MasterCircleDivisionWithUserID();
                    MasterCircleDivision = repomaster.Select_CircleDivisionWithUserID("ALL", Convert.ToInt64(Session["UserID"]));
                    TempData["PlaceArea"] = Convert.ToString(MasterCircleDivision.Tables[3].Rows[0]["OfficeArea"]);

                    #region Get Scheme According to Division
                    MasterCircleDivision = repomaster.Select_SchemeDivisionWise("GetPreSurveyBudgetInformation", Convert.ToString(MasterCircleDivision.Tables[0].Rows[0]["Value"]), Convert.ToString(MasterCircleDivision.Tables[1].Rows[0]["Value"]), Convert.ToInt64(Session["UserID"]));
                    ViewBag.SchemeList = new SelectList(MasterCircleDivision.Tables[0].AsDataView(), "Value", "Text");
                    ViewBag.RangeList = new SelectList(MasterCircleDivision.Tables[1].AsDataView(), "Value", "Text");
                    #endregion
                    ViewBag.FinancialYear = lstFinancialYear;
                    ViewBag.BudgetHead = lstBudgetHead;
                    #endregion

                    if (BudgetPerposalLists != null)
                    {
                        BudgetPerposalLists = BudgetPerposalLists.Where(d => d.FY_ID == (!string.IsNullOrEmpty(model.FinancialYear) ? Convert.ToInt32(model.FinancialYear) : d.FY_ID) && d.SchemeID == (!string.IsNullOrEmpty(model.SchemeName) ? Convert.ToInt32(model.SchemeName) : d.SchemeID) && d.BudgetHeadID == (!string.IsNullOrEmpty(model.BudgetHead) ? Convert.ToInt32(model.BudgetHead) : d.BudgetHeadID) && d.SubBudgetHeadID == (!string.IsNullOrEmpty(model.SubBudgetHead) ? Convert.ToInt32(model.SubBudgetHead) : d.SubBudgetHeadID) && d.ActivityID == (!string.IsNullOrEmpty(model.ActivityName) ? Convert.ToInt32(model.ActivityName) : d.ActivityID) && d.SubActivityID == (!string.IsNullOrEmpty(model.SubActivityName) ? Convert.ToInt32(model.SubActivityName) : d.SubActivityID) && d.CIRCLE_CODE == (!string.IsNullOrEmpty(model.CIRCLE_CODE) ? Convert.ToString(model.CIRCLE_CODE) : d.CIRCLE_CODE) && d.DivisionCode == (!string.IsNullOrEmpty(model.DivisionCode) ? Convert.ToString(model.DivisionCode) : d.DivisionCode) && d.RangeCode == (!string.IsNullOrEmpty(model.RangeCode) ? Convert.ToString(model.RangeCode.Trim()) : d.RangeCode)).ToList();
                    }

                    ViewData["BudgetPreSurveyListInfo"] = BudgetPerposalLists;
                    #endregion
                    


                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(modeldata);
        }

        [HttpGet]
        public ActionResult GetPreSurveyDetails(string ID)
        {
            List<ViewBudgetPreSurveyModel> BudgetPerposalLists = new List<ViewBudgetPreSurveyModel>();
            ViewBudgetPreSurveyModel model = new ViewBudgetPreSurveyModel();
            try
            {
                #region Maintain the Budget Perposal List

                if (Session["BudgetPreSurveyListFilter"] == null)
                {
                    Session["BudgetPreSurveyListFilter"] = BudgetPerposalLists;
                }
                else
                {
                    BudgetPerposalLists = (List<ViewBudgetPreSurveyModel>)Session["BudgetPreSurveyListFilter"];
                }
                if (BudgetPerposalLists != null && !string.IsNullOrEmpty(ID))
                {
                    model = BudgetPerposalLists.Where(d => d.ID == Convert.ToInt64(ID) && d.isActive == 1).FirstOrDefault();
                    BudgetPerposalRepo repo = new BudgetPerposalRepo();
                    model.UploadFilesList = new List<string>();
                    model.UploadFilesList = repo.GetBudgetPerposalUploadFiles(Convert.ToInt32(ID), "GetBudgetPreSurveyFilesUpload");
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return PartialView("_BudgetPreSurveyDetails", model);
        }


        
        #endregion
    }
}
