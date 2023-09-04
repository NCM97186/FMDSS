using AutoMapper;
using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using Newtonsoft.Json;
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
    public class BudgetAllocationPerposalController : Controller
    {
        //
        // GET: /BudgetAllocationPerposal/
        public FmdssContext fmdsscontext;
        public BudgetAllocationPerposalController()
        {
            fmdsscontext = new FmdssContext();
        }


        #region  Budget Perposal Module

        public ActionResult AddBudgetPerposal()
        {
            try
            {
                Session["AddBudgetPerposalList"] = null;
                List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<SelectListItem> lstScheme = new List<SelectListItem>();
                List<SelectListItem> lstCircle = new List<SelectListItem>();
                var financialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).ToList();
                foreach (var item in financialYear)
                {
                    lstFinancialYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }

                var budgetHead = fmdsscontext.tbl_mst_BudgetHead.Select(i => new { i.BudgetHead, i.ID });
                foreach (var item in budgetHead)
                {

                    lstBudgetHead.Add(new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) });
                }
                var Scheme = fmdsscontext.tbl_FDM_SchemeForWidelife.Select(i => new { i.Scheme_Name, i.ID });
                foreach (var item in Scheme)
                {
                    lstScheme.Add(new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) });
                }
                var activity = fmdsscontext.tbl_mst_ActivityForWidelife.Select(i => new { i.Activity_Name, i.ID });

                #region Get Circle and Division with userID
                DataSet MasterCircleDivision = new DataSet();
                MasterCircleDivisionWithUserID repomaster = new MasterCircleDivisionWithUserID();
                MasterCircleDivision = repomaster.Select_CircleDivisionWithUserID("ALL", Convert.ToInt64(Session["UserID"]));
                ViewBag.Circle = new SelectList(MasterCircleDivision.Tables[0].AsDataView(), "Value", "Text");
                ViewBag.Division = new SelectList(MasterCircleDivision.Tables[1].AsDataView(), "Value", "Text");
                TempData["PlaceArea"] = Convert.ToString(MasterCircleDivision.Tables[3].Rows[0]["OfficeArea"]);
                #endregion

                ViewBag.Scheme = lstScheme;
                ViewBag.FinancialYear = lstFinancialYear;
                ViewBag.BudgetHead = lstBudgetHead;
                ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                BudgetPerposalRepo repo = new BudgetPerposalRepo();
                Session["AddBudgetPerposalList"] = repo.GetBudgetPerposalCircleList(Convert.ToInt32(Session["UserID"]));
                ViewData["BudgetPerposalList"] = Session["AddBudgetPerposalList"];



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, 0);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddBudgetPerposal(ViewBudgetAllocationPerposalModel model)
        {
            #region Active Status

            long status = 0;
            try
            {
                if (Session["AddBudgetPerposalList"] != null)
                {

                    List<ViewBudgetAllocationPerposalModel> lstBudgetAllocationCircle = (List<ViewBudgetAllocationPerposalModel>)Session["AddBudgetPerposalList"];

                    //var lstFbudget = fQuery.Where(i => lstBudgetAllocationCircle.Any(e => e.FY_ID == i.FY_ID && e.SchemeID == i.SchemeID && i.BudgetHeadID == e.BudgetHeadID && i.SubBudgetHeadID == e.SubBudgetHeadID && i.CIRCLE_CODE == e.CIRCLE_CODE && i.Division == e.Division && i.ISCircleDivision == e.ISCircleDivision));

                    foreach (var items in lstBudgetAllocationCircle)
                    {
                        long id = Convert.ToInt64(items.rowid);
                        tbl_BudgetAllocationPerposal tblBudgCirUpdate = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ID == id);

                        if (tblBudgCirUpdate != null)
                        {
                            tblBudgCirUpdate.isActive = true;
                            this.fmdsscontext.tbl_BudgetAllocationPerposal.Add(tblBudgCirUpdate);
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
            return RedirectToAction("AddBudgetPerposal");
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
                        string Fullfname = Path.Combine(Server.MapPath("~/BudgetPerposalFiles/"), fname);
                        file.SaveAs(Fullfname);

                        #region Upload Files.
                        tbl_BudgetPerposalFilesUpload Val = new tbl_BudgetPerposalFilesUpload();
                        Val.BudgetPerposalId = Convert.ToInt64(BudgetID);
                        Val.FilesName = "~/BudgetPerposalFiles/" + fname;
                        Val.CreatedDate = DateTime.Now.ToShortDateString();
                        Val.Createdby = Convert.ToInt64(Session["UserId"]);
                        Val.Status = 1;


                        fmdsscontext.tbl_BudgetPerposalFilesUpload.Add(Val);
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



        [HttpPost]
        public JsonResult AddBudgetPerposalDetailsMaster(ViewBudgetAllocationPerposalModel objModel, List<HttpPostedFileBase> fileUpload1)
        {
            try
            {

                #region Maintain the BudgetAllocation List
                List<ViewBudgetAllocationPerposalModel> BudgetAllocationLists = new List<ViewBudgetAllocationPerposalModel>();
                if (Session["AddBudgetPerposalList"] == null)
                {
                    Session["AddBudgetPerposalList"] = new List<ViewBudgetAllocationPerposalModel>();
                }
                else
                {
                    BudgetAllocationLists = (List<ViewBudgetAllocationPerposalModel>)Session["AddBudgetPerposalList"];
                }
                #endregion

                bool Duplicate = CheckDuplicateRecordsMasterPerposal(objModel, BudgetAllocationLists);

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

                        tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>();
                            tbl_BudgetAllocationPerposal ObjbudgetCircle = Mapper.Map<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);




                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetPerposalList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Division
                    else if (objModel != null && objModel.ISCircleDivision == "Division")
                    {

                        tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>();
                            tbl_BudgetAllocationPerposal ObjbudgetCircle = Mapper.Map<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetPerposalList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Sanctuary
                    else if (objModel != null && objModel.ISCircleDivision == "Sanctuary")
                    {

                        tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>();
                            tbl_BudgetAllocationPerposal ObjbudgetCircle = Mapper.Map<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetPerposalList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Range
                    else if (objModel != null && objModel.ISCircleDivision == "Range")
                    {

                        tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Range" && i.SanctuaryCode == objModel.SanctuaryCode && i.RangeCode==objModel.RangeCode && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>();
                            tbl_BudgetAllocationPerposal ObjbudgetCircle = Mapper.Map<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetPerposalList"] = BudgetAllocationLists;
                            #endregion
                        }
                        else
                        {
                            objModel.rowid = "D";
                        }
                    }
                    #endregion

                    #region Naka
                    else if (objModel != null && objModel.ISCircleDivision == "Naka")
                    {

                        tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Range" && i.SanctuaryCode == objModel.SanctuaryCode && i.RangeCode == objModel.RangeCode && i.NakaID == objModel.NakaID && i.isActive == true);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>();
                            tbl_BudgetAllocationPerposal ObjbudgetCircle = Mapper.Map<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>(objModel);
                            ObjbudgetCircle.EnteredOn = DateTime.Now;
                            ObjbudgetCircle.EnteredBy = Convert.ToInt64(Session["UserId"]);
                            ObjbudgetCircle.isActive = false;
                            fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                            fmdsscontext.SaveChanges();
                            long Id = ObjbudgetCircle.ID;
                            objModel.rowid = Convert.ToString(Id);

                            #region Add Data in List
                            BudgetAllocationLists.Add(objModel);
                            Session["AddBudgetPerposalList"] = BudgetAllocationLists;
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


        public bool CheckDuplicateRecordsMasterPerposal(ViewBudgetAllocationPerposalModel objModel, List<ViewBudgetAllocationPerposalModel> List)
        {
            List<ViewBudgetAllocationPerposalModel> lstChkDup = new List<ViewBudgetAllocationPerposalModel>();
            bool duplicate = false;

            #region Check In Table
            if (objModel != null && objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
            {
                #region Check In Current Session
                int count = List.Where(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ")).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true);
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }

            }
            else if (objModel != null && objModel.ISCircleDivision == "Division")
            {
                #region Check In Current Session
                int count = List.Where(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division").Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true);
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }
            }

            else if (objModel != null && objModel.ISCircleDivision == "Sanctuary")
            {
                #region Check In Current Session
                int count = List.Where(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.FY_ID == objModel.FY_ID && i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true);
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }
            }
            #endregion




            return duplicate;
        }


        public ActionResult GetAllocatedPerposalAmtDetails(ViewBudgetAllocationPerposalModel objCircle)
        {
            try
            {
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                var param1 = new SqlParameter("@BudgetHeadID", objCircle.BudgetHeadID);
                var param2 = new SqlParameter("@SubBudgetHeadID", objCircle.SubBudgetHeadID);
                var param3 = new SqlParameter("@FY_ID", objCircle.FY_ID);
                var param4 = new SqlParameter("@SchemeID", objCircle.SchemeID);
                var param5 = new SqlParameter("@ActivityID", objCircle.ActivityID);
                var param6 = new SqlParameter("@SubActivityID", objCircle.SubActivityID);
                var result = fmdsscontext.Database.SqlQuery<ViewBudgetAllocationPerposalModel>("BA_getAVailableAmountforPerposalBudget @BudgetHeadID,@SubBudgetHeadID,@FY_ID,@SchemeID,@ActivityID,@SubActivityID", param1, param2, param3, param4, param5, param6).ToList();
                var fresult = result.FirstOrDefault();
                decimal AvaliableAmount = fresult.TotalAmount;
                decimal AllocatedAmount = fresult.AllocatedAmount;
                decimal RemaningAmount = fresult.RemaningAmount;
                long BudgetAllocationHeadId = fresult.BudgetHeadAllocationID;
                var json = Json(new { AvaliableAmount, AllocatedAmount, RemaningAmount, BudgetAllocationHeadId });
                return json;

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult DeleteBudgetAllocationPerposal(long Id, string ISCircleDivision)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_BudgetAllocationPerposal tblAllocationCircle = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ID == Id);

                    if (tblAllocationCircle != null)
                    {
                        #region Circle and HQ
                        if (ISCircleDivision == "Circle" || ISCircleDivision == "HQ")
                        {

                            List<ViewBudgetAllocationPerposalModel> lstBudgetAllocationCircle = (List<ViewBudgetAllocationPerposalModel>)Session["AddBudgetPerposalList"];

                            if (lstBudgetAllocationCircle != null)
                            {

                                ViewBudgetAllocationPerposalModel result = lstBudgetAllocationCircle.FirstOrDefault(i => i.FY_ID == tblAllocationCircle.FY_ID && i.SchemeID == tblAllocationCircle.SchemeID && i.BudgetHeadID == tblAllocationCircle.BudgetHeadID && i.SubBudgetHeadID == tblAllocationCircle.SubBudgetHeadID && i.ActivityID == tblAllocationCircle.ActivityID && i.SubActivityID == tblAllocationCircle.SubActivityID && i.CIRCLE_CODE == tblAllocationCircle.CIRCLE_CODE && (tblAllocationCircle.ISCircleDivision == "Circle" || tblAllocationCircle.ISCircleDivision == "HQ"));
                                if (result != null)
                                {
                                    lstBudgetAllocationCircle.Remove(result);
                                }
                                Session["AddBudgetPerposalList"] = null;
                                Session["AddBudgetPerposalList"] = lstBudgetAllocationCircle;
                            }
                        }
                        #endregion
                        #region Division
                        if (ISCircleDivision == "Division")
                        {

                            List<ViewBudgetAllocationPerposalModel> lstBudgetAllocationCircle = (List<ViewBudgetAllocationPerposalModel>)Session["AddBudgetPerposalList"];

                            if (lstBudgetAllocationCircle != null)
                            {
                                ViewBudgetAllocationPerposalModel result = lstBudgetAllocationCircle.FirstOrDefault(i => i.FY_ID == tblAllocationCircle.FY_ID && i.SchemeID == tblAllocationCircle.SchemeID && i.BudgetHeadID == tblAllocationCircle.BudgetHeadID && i.SubBudgetHeadID == tblAllocationCircle.SubBudgetHeadID && i.ActivityID == tblAllocationCircle.ActivityID && i.SubActivityID == tblAllocationCircle.SubActivityID && i.CIRCLE_CODE == tblAllocationCircle.CIRCLE_CODE && i.Division == tblAllocationCircle.Division && tblAllocationCircle.ISCircleDivision == "Division");

                                if (result != null)
                                {
                                    lstBudgetAllocationCircle.Remove(result);
                                }
                                Session["AddBudgetPerposalList"] = null;
                                Session["AddBudgetPerposalList"] = lstBudgetAllocationCircle;
                            }
                        }
                        #endregion

                        #region Sanctuary
                        if (ISCircleDivision == "Sanctuary")
                        {

                            List<ViewBudgetAllocationPerposalModel> lstBudgetAllocationCircle = (List<ViewBudgetAllocationPerposalModel>)Session["AddBudgetPerposalList"];

                            if (lstBudgetAllocationCircle != null)
                            {
                                ViewBudgetAllocationPerposalModel result = lstBudgetAllocationCircle.FirstOrDefault(i => i.FY_ID == tblAllocationCircle.FY_ID && i.SchemeID == tblAllocationCircle.SchemeID && i.BudgetHeadID == tblAllocationCircle.BudgetHeadID && i.SubBudgetHeadID == tblAllocationCircle.SubBudgetHeadID && i.ActivityID == tblAllocationCircle.ActivityID && i.SubActivityID == tblAllocationCircle.SubActivityID && i.CIRCLE_CODE == tblAllocationCircle.CIRCLE_CODE && i.Division == tblAllocationCircle.Division && tblAllocationCircle.ISCircleDivision == "Sanctuary" && tblAllocationCircle.SanctuaryCode == tblAllocationCircle.SanctuaryCode);

                                if (result != null)
                                {
                                    lstBudgetAllocationCircle.Remove(result);
                                }
                                Session["AddBudgetPerposalList"] = null;
                                Session["AddBudgetPerposalList"] = lstBudgetAllocationCircle;
                            }
                        }
                        #endregion
                        tblAllocationCircle.isActive = false;
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

        public JsonResult DeleteBudgetAllocatedEntryForPerposal(long Id)
        {
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_BudgetAllocationPerposal tblAllocationCircle = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ID == Id);

                    if (tblAllocationCircle != null)
                    {
                        tblAllocationCircle.isActive = false;
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
        #endregion

        #region Budget Perposal API

        #region Finances Years
        public JsonResult GetFinancesYear()
        {
            List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
            try
            {
                lstFinancialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new SelectListItem() { Text = i.FinancialYear, Value = i.ID.ToString() }).ToList();

            }
            catch (Exception ex)
            {
                lstFinancialYear = new List<SelectListItem>();
            }
            return Json(lstFinancialYear, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Budget Head
        public JsonResult GetBudgetHead()
        {
            List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
            try
            {
                lstBudgetHead = fmdsscontext.tbl_mst_BudgetHead.Select(i => new SelectListItem() { Text = i.BudgetHead, Value = i.ID.ToString() }).ToList();
            }
            catch (Exception ex)
            {
                lstBudgetHead = new List<SelectListItem>();
            }
            return Json(lstBudgetHead, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Scheme List
        public JsonResult GetSchemeList()
        {
            List<SelectListItem> lstScheme = new List<SelectListItem>();
            try
            {
                lstScheme = fmdsscontext.tbl_FDM_SchemeForWidelife.Select(i => new SelectListItem() { Text = i.Scheme_Name, Value = i.ID.ToString() }).ToList();
            }

            catch (Exception ex)
            {
                lstScheme = new List<SelectListItem>();
            }
            return Json(lstScheme, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Circle List
        public JsonResult GetCircleList()
        {
            List<SelectListItem> lstCircle = new List<SelectListItem>();
            try
            {
                lstCircle = fmdsscontext.tbl_mst_Forest_WildLifeCircles.Select(i => new SelectListItem() { Text = i.CIRCLE_NAME, Value = i.CIRCLE_CODE.ToString() }).ToList();
            }

            catch (Exception ex)
            {
                lstCircle = new List<SelectListItem>();
            }
            return Json(lstCircle, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Sub Unit List
        public JsonResult GetUnitList()
        {
            SelectList lstUnit = null;
            try
            {
                lstUnit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
            }

            catch (Exception ex)
            {
                lstUnit = null;
            }
            return Json(lstUnit, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Get Perposal List List
        public JsonResult GetPerposalList(int UserID)
        {
            List<ViewBudgetAllocationPerposalModel> PerposalLst = new List<ViewBudgetAllocationPerposalModel>();
            try
            {
                BudgetPerposalRepo repo = new BudgetPerposalRepo();
                PerposalLst = repo.GetBudgetPerposalCircleList(UserID);
            }

            catch (Exception ex)
            {
                PerposalLst = new List<ViewBudgetAllocationPerposalModel>();
            }
            return Json(PerposalLst, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #endregion


        #region Get GIS Information

        public ActionResult GetBudgetPerposalDetailsForGIS(string ID)
        {
            List<View_BudgetAllocation_Circle> BudgetAllocationList = new List<View_BudgetAllocation_Circle>();
            BudgetAllocationGISInformation model = new BudgetAllocationGISInformation();
            TempData["ID"] = ID;
            model.ID = Convert.ToInt64(ID);
            try
            {

                #region Check GIS Information not Null
                if (Session["BudgetPerposalGISinformation"] != null)
                {
                    model = (BudgetAllocationGISInformation)Session["BudgetPerposalGISinformation"];
                }
                #endregion
                string str = JsonConvert.SerializeObject(Session["AddBudgetPerposalList"]);
                BudgetAllocationList = JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);
                if (BudgetAllocationList.Count > 0)
                {
                    model.BudgetAllocationDetails = BudgetAllocationList.FirstOrDefault(s => s.ID == Convert.ToInt64(ID));

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, 0);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetBudgetPerposalDetailsForGIS(BudgetAllocationGISInformation model)
        {
            try
            {
                long ID = model.ID;
                TempData["ID"] = model.ID;
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                #region Check GIS Information not Null
                if (Session["BudgetPerposalGISinformation"] != null)
                {
                    model = (BudgetAllocationGISInformation)Session["BudgetPerposalGISinformation"];
                }
                #endregion
                string JSONString = JsonConvert.SerializeObject(model.GISInformationList);
                DataTable GISTable = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));

                BudgetAllocationRepo repo = new BudgetAllocationRepo();
                DataTable dt = repo.InsertGisInformationBudgetExpenditure("Insert_BudgetPerposal", ID.ToString(), Convert.ToInt64(Session["UserId"]), GISTable);
                if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["Status"]) == 1)
                {
                    TempData["Message"] = "<div>" + Convert.ToString(dt.Rows[0]["message"]) + " </div>";

                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later " + Convert.ToString(dt.Rows[0]["message"]) + ")</div>";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            Session["BudgetPerposalGISinformation"] = null;
            return RedirectToAction("AddBudgetPerposal");
        }

        public ActionResult GISDataBudgetModule(FormCollection form)
        {
            BudgetAllocationGISInformation model = new BudgetAllocationGISInformation();
            Session["BudgetPerposalGISinformation"] = null;
            string ID = "0";
            string aid = string.Empty;
            try
            {
                if (TempData["ID"] != null)
                {
                    ID = Convert.ToString(TempData["ID"]);
                }

                BindMasterData _ObjMaster = new BindMasterData();
                if (Convert.ToString(Session["PermissionTypeID"]) != "") { aid = Convert.ToString(Session["PermissionTypeID"]); }
                if (Convert.ToString(form["successFlag"]).ToLower() == "true")
                {


                    List<clsPermission> myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));
                    model.GISID = form["gisid"].ToString();
                    model.AreaShape = Convert.ToDecimal(form["shapeArea"].ToString());
                    #region "Muliple List"
                    string LAT = string.Empty, Long = string.Empty;
                    foreach (var dr in myDeserializedObj)
                    {
                        #region "KML and Lat-Long"
                        if (form["locCentroid"].ToString() != "")
                        {
                            if (form["locCentroid"].ToString().Contains(","))
                            {
                                string[] locCentroid = form["locCentroid"].ToString().Split(',');
                                LAT = locCentroid[1] == "NA" ? "" : locCentroid[1];
                                Long = locCentroid[0] == "NA" ? "" : locCentroid[0];
                            }
                        }

                        #endregion

                        clsPermission cls = new clsPermission();
                        cls.Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        cls.Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        cls.Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        cls.Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        cls.Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        cls.Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM;
                        cls.Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM;
                        cls.Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM;
                        cls.Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM;
                        cls.Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM;
                        cls.areaName = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        cls.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE;
                        cls.Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd;
                        cls.Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM;
                        model.GISInformationName.Add(cls);

                        GISDataBaseModel gisModel = new GISDataBaseModel();
                        gisModel.DIV_CODE = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        gisModel.DIST_CODE = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        gisModel.BLK_CODE = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        gisModel.GP_CODE = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        gisModel.VILL_CODE = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        gisModel.Area = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        gisModel.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : string.IsNullOrEmpty(dr.FOREST_DIVCODE) ? "N/A" : dr.FOREST_DIVCODE;
                        gisModel.GPSLat = LAT;
                        gisModel.GPSLong = Long;
                        gisModel.GISFilePath = Convert.ToString(form["filePath"]);
                        gisModel.GISOrignalFilePath = Convert.ToString(form["originalFileName"]);
                        gisModel.GISID = model.GISID;
                        gisModel.AreaShapeInHactor = model.AreaShape;

                        model.Latitude = LAT;
                        model.Longitude = Long;
                        model.GISInformationList.Add(gisModel);

                    }

                    #endregion

                    Session["BudgetPerposalGISinformation"] = model;

                }
                else
                {
                    Session["BudgetPerposalGISinformation"] = null;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, 0);
            }
            return RedirectToAction("GetBudgetPerposalDetailsForGIS", "BudgetAllocationPerposal", new { ID = ID });
        }



        #endregion

        #region Get Budget Perposal Data

        public ActionResult GetBudgetPerposalInformation()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBudgetAllocationPerposalModel model = new ViewBudgetAllocationPerposalModel();
            try
            {
                ViewBag.DivisionList = new List<SelectListItem>();
                ViewBag.SantauryList = new List<SelectListItem>();
                ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                ViewBag.SubActivityList = new List<SelectListItem>();
                BudgetPerposalRepo repo = new BudgetPerposalRepo();
                List<ViewBudgetAllocationPerposalModel> List = new List<ViewBudgetAllocationPerposalModel>();
                List = repo.GetBudgetPerposalCircleList(Convert.ToInt32(Session["UserID"]));
                ViewData["BudgetPerposalListInfo"] = List;
                Session["BudgetPerposalListFilter"] = List;
                #region Fill Dropdown Accoding to the Budget Allocation
                FillDropDownMasterWithUserID(List);
                #endregion
            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult GetBudgetPerposalInformation(ViewBudgetAllocationPerposalModel model)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBudgetAllocationPerposalModel modeldata = new ViewBudgetAllocationPerposalModel();
            try
            {
                long status = 0;
                try
                {
                    ViewBag.DivisionList = new List<SelectListItem>();
                    ViewBag.SantauryList = new List<SelectListItem>();
                    ViewBag.SubBudgetHeadList = new List<SelectListItem>();
                    ViewBag.SubActivityList = new List<SelectListItem>();

                    #region Maintain the Budget Perposal List
                    List<ViewBudgetAllocationPerposalModel> BudgetPerposalLists = new List<ViewBudgetAllocationPerposalModel>();
                    if (Session["BudgetPerposalListFilter"] == null)
                    {
                        Session["BudgetPerposalListFilter"] = BudgetPerposalLists;
                    }
                    else
                    {
                        BudgetPerposalLists = (List<ViewBudgetAllocationPerposalModel>)Session["BudgetPerposalListFilter"];
                    }

                    #region Fill Dropdown Accoding to the Budget Allocation
                    FillDropDownMasterWithUserID(BudgetPerposalLists);
                    #endregion

                    if (BudgetPerposalLists != null)
                    {
                        BudgetPerposalLists = BudgetPerposalLists.Where(d => d.FY_ID == (!string.IsNullOrEmpty(model.FinancialYear) ? Convert.ToInt32(model.FinancialYear) : d.FY_ID) && d.SchemeID == (!string.IsNullOrEmpty(model.SchemeName) ? Convert.ToInt32(model.SchemeName) : d.SchemeID) && d.BudgetHeadID == (!string.IsNullOrEmpty(model.BudgetHead) ? Convert.ToInt32(model.BudgetHead) : d.BudgetHeadID) && d.SubBudgetHeadID == (!string.IsNullOrEmpty(model.SubBudgetHead) ? Convert.ToInt32(model.SubBudgetHead) : d.SubBudgetHeadID) && d.ActivityID == (!string.IsNullOrEmpty(model.ActivityName) ? Convert.ToInt32(model.ActivityName) : d.ActivityID) && d.SubActivityID == (!string.IsNullOrEmpty(model.SubActivityName) ? Convert.ToInt32(model.SubActivityName) : d.SubActivityID) && d.CIRCLE_CODE == (!string.IsNullOrEmpty(model.CIRCLE_CODE) ? Convert.ToString(model.CIRCLE_CODE) : d.CIRCLE_CODE) && d.Div_Code == (!string.IsNullOrEmpty(model.Division) ? Convert.ToString(model.Division) : d.Div_Code) && d.SanctuaryCode == (!string.IsNullOrEmpty(model.SanctuaryCode) ? Convert.ToString(model.SanctuaryCode.Trim()) : d.SanctuaryCode)).ToList();
                    }


                    ViewData["BudgetPerposalListInfo"] = BudgetPerposalLists;
                    #endregion
                    #region Fill Division and Santaury And
                    #region Fill Division
                    List<SelectListItem> lstDivision = new List<SelectListItem>();

                    try
                    {

                        var result = fmdsscontext.tbl_mst_Forest_Divisions.Where(i => i.CIRCLE_CODE == model.CIRCLE_CODE && i.ForBudgetModuleDist == 1).Select(i => new { i.DIV_CODE, i.DIV_NAME }).OrderBy(s => s.DIV_NAME);
                        foreach (var item in result)
                        {
                            lstDivision.Add(new SelectListItem { Text = item.DIV_NAME, Value = item.DIV_CODE });
                        }
                        ViewBag.DivisionList = lstDivision;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.DivisionList = new List<SelectListItem>();

                    }
                    #endregion

                    #region Fill Santaury
                    try
                    {
                        List<SelectListItem> SanctuaryList = new List<SelectListItem>();

                        var param1 = new SqlParameter("@Action", "Detail");
                        var param2 = new SqlParameter("@DIV_CODE", model.Division);
                        ViewBag.SantauryList = fmdsscontext.Database.SqlQuery<SelectListItem>("SP_GetSanctuaryName @Action,@DIV_CODE", param1, param2).ToList();

                    }
                    catch (Exception ex)
                    {
                        ViewBag.SantauryList = new List<SelectListItem>();
                    }
                    #endregion

                    #region Fill SubBudgethead
                    long BudgetHeadID = Convert.ToInt64(model.BudgetHead);
                    ViewBag.SubBudgetHeadList = fmdsscontext.tbl_mst_SubBudgetHead.Where(s => s.BudgetHeadID == BudgetHeadID).OrderBy(d => d.SubBudgetHead).ToList().Select(d => new SelectListItem() { Text = d.SubBudgetHead, Value = d.ID.ToString() }).ToList();
                    #endregion

                    #region Fill SubActivityhead
                    int ActivityID = !string.IsNullOrEmpty(model.ActivityName) ? Convert.ToInt32(model.ActivityName) : 0;
                    ViewBag.SubActivityList = fmdsscontext.tbl_mst_SUBActivityForWidelife.Where(i => i.IsActive == 1 && i.ActivityID == ActivityID).OrderBy(d => d.SUBActivity_Name).ToList().Select(item => new SelectListItem { Text = item.SUBActivity_Name, Value = Convert.ToString(item.ID) }).ToList();
                    #endregion
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
        public ActionResult GetBudgetPerposalDetails(string ID)
        {
            List<ViewBudgetAllocationPerposalModel> BudgetPerposalLists = new List<ViewBudgetAllocationPerposalModel>();
            ViewBudgetAllocationPerposalModel model = new ViewBudgetAllocationPerposalModel();
            try
            {
                #region Maintain the Budget Perposal List

                if (Session["BudgetPerposalListFilter"] == null)
                {
                    Session["BudgetPerposalListFilter"] = BudgetPerposalLists;
                }
                else
                {
                    BudgetPerposalLists = (List<ViewBudgetAllocationPerposalModel>)Session["BudgetPerposalListFilter"];
                }
                if (BudgetPerposalLists != null && !string.IsNullOrEmpty(ID))
                {
                    model = BudgetPerposalLists.Where(d => d.ID == Convert.ToInt64(ID) && d.isActive == true).FirstOrDefault();
                    BudgetPerposalRepo repo = new BudgetPerposalRepo();
                    model.UploadFilesList = new List<string>();
                    model.UploadFilesList = repo.GetBudgetPerposalUploadFiles(Convert.ToInt32(ID), "GetUploadedFilesBudgetPerposal");
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return PartialView("_BudgetPerposalDetails", model);
        }


        #region Get Scheme Budget Circle division all master according the record Developed by Rajveer
        public void FillDropDownMasterWithUserID(List<ViewBudgetAllocationPerposalModel> BudgetList)
        {
            List<SelectListItem> FinanceYear = new List<SelectListItem>();
            List<SelectListItem> SchemeList = new List<SelectListItem>();
            List<SelectListItem> BudgetHead = new List<SelectListItem>();
            List<SelectListItem> Activity = new List<SelectListItem>();
            List<SelectListItem> Circle = new List<SelectListItem>();
            Session["BudgetListFilter"] = null;
            try
            {
                Session["BudgetListFilter"] = BudgetList;

                #region Get Circle and Division with userID
                DataSet MasterCircleDivision = new DataSet();
                MasterCircleDivisionWithUserID repomaster = new MasterCircleDivisionWithUserID();
                MasterCircleDivision = repomaster.Select_CircleDivisionWithUserID("ALL", Convert.ToInt64(Session["UserID"]));
                ViewBag.Circle = new SelectList(MasterCircleDivision.Tables[0].AsDataView(), "Value", "Text");
                ViewBag.DivisionLists = new SelectList(MasterCircleDivision.Tables[1].AsDataView(), "Value", "Text");
                #endregion

                #region FinanceYear Bind
                var financialYear = fmdsscontext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).Distinct().ToList();
                foreach (var item in financialYear)
                {
                    FinanceYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                }
                ViewBag.Financial = FinanceYear;
                #endregion

                #region Scheme Bind


                List<long> SchemeIds = BudgetList.Select(s => s.SchemeID).Distinct().ToList();
                SchemeList = fmdsscontext.tbl_FDM_SchemeForWidelife.Where(s => SchemeIds.Contains(s.ID)).OrderBy(d => d.Scheme_Name).ToList().Select(item => new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) }).ToList();
                ViewBag.SchemeList = SchemeList;
                #endregion

                #region Activity Bind
                List<long> ActivityIds = BudgetList.Select(s => s.ActivityID).Distinct().ToList();
                Activity = fmdsscontext.tbl_mst_ActivityForWidelife.Where(s => ActivityIds.Contains(s.ID)).OrderBy(d => d.Activity_Name).ToList().Select(item => new SelectListItem { Text = item.Activity_Name, Value = Convert.ToString(item.ID) }).ToList();
                ViewBag.Activity = Activity;
                #endregion

                #region Budget Head Bind
                List<long> BudgetHeadIds = BudgetList.Select(s => s.BudgetHeadID).Distinct().ToList();
                BudgetHead = fmdsscontext.tbl_mst_BudgetHead.Where(s => BudgetHeadIds.Contains(s.ID)).OrderBy(d => d.BudgetHead).ToList().Select(item => new SelectListItem { Text = item.BudgetHead, Value = Convert.ToString(item.ID) }).ToList();
                ViewBag.BudgetHead = BudgetHead;
                #endregion

            }
            catch (Exception ex)
            {
                ViewBag.Financial = new List<SelectListItem>();
                ViewBag.Circle = new List<SelectListItem>();
                ViewBag.Activity = new List<SelectListItem>();
                ViewBag.BudgetHead = new List<SelectListItem>();
            }
        }


        public JsonResult GetSubActivityAccordingAllocation(long ActivityID)
        {
            try
            {
                #region Contain Budget List According User ID
                List<ViewBudgetAllocationPerposalModel> BudgetList = (List<ViewBudgetAllocationPerposalModel>)Session["BudgetPerposalListFilter"];
                #endregion

                List<SelectListItem> lstSubActivity = new List<SelectListItem>();
                List<long> SubActivityIds = BudgetList.Where(s => s.ActivityID == ActivityID).Select(s => s.SubActivityID).Distinct().ToList();
                lstSubActivity = fmdsscontext.tbl_mst_SUBActivityForWidelife.Where(s => SubActivityIds.Contains(s.ID)).OrderBy(d => d.SUBActivity_Name).ToList().Select(a => new SelectListItem { Text = a.SUBActivity_Name, Value = Convert.ToString(a.ID) }).ToList();

                return Json(lstSubActivity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        public JsonResult GetBudgetSubHeadAccordingAllocation(long budgetHead)
        {
            try
            {

                #region Contain Budget List According User ID
                List<ViewBudgetAllocationPerposalModel> BudgetList = (List<ViewBudgetAllocationPerposalModel>)Session["BudgetPerposalListFilter"];
                #endregion

                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<long> SubBudgetHeadIds = BudgetList.Where(s => s.BudgetHeadID == budgetHead).Select(s => s.SubBudgetHeadID).Distinct().ToList();
                lstBudgetHead = fmdsscontext.tbl_mst_SubBudgetHead.Where(s => SubBudgetHeadIds.Contains(s.ID)).OrderBy(d => d.SubBudgetHead).ToList().Select(a => new SelectListItem { Text = a.SubBudgetHead, Value = Convert.ToString(a.ID) }).ToList();
                lstBudgetHead.Add(new SelectListItem { Text = "none", Value = "0" });

                return Json(lstBudgetHead, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        #endregion
        #endregion

    }
}
