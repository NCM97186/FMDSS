using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using FMDSS.Models.FmdssContext;
using FMDSS.Repository;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using FMDSS.Models.Encroachment.DomainModel;
using System.Data.SqlClient;
using AutoMapper;
using System.Data.Entity;
using FMDSS.Models;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class BudgetAllocationtoSubBudgetHeadController : Controller
    {


        public FmdssContext fmdsscontext;

        public BudgetAllocationtoSubBudgetHeadController()
        {
            fmdsscontext = new FmdssContext();
        }

        public ActionResult BudgetAllocationToBudgetHead()
        {
            try
            {
                Session.Remove("HeadList");
                List<SelectListItem> lstFinancialYear = new List<SelectListItem>();
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                List<SelectListItem> lstScheme = new List<SelectListItem>();

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

                ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                ViewBag.Scheme = lstScheme;
                ViewBag.FinancialYear = lstFinancialYear;
                ViewBag.BudgetHead = lstBudgetHead;
                ViewData["BudgetAllocationBudgetHead"] = GetBudgetAllocationHeadList();
            }
            catch (Exception)
            {

                throw;
            }
            return View();


        }

        public List<View_BudgetHead_Allocation> GetBudgetAllocationHeadList()
        {
            List<View_BudgetHead_Allocation> lstBudgetAllocationCircle = new List<View_BudgetHead_Allocation>();
            try
            {
                var param1 = new SqlParameter("@Option", "BUDGETHEAD");
                var result = fmdsscontext.Database.SqlQuery<View_BudgetHead_Allocation>("BA_getBudgetAllocationList @Option", param1).ToList();
                foreach (var item in result)
                {
                    lstBudgetAllocationCircle.Add(new View_BudgetHead_Allocation
                    {
                        FinancialYear = item.FinancialYear,
                        SchemeName = item.SchemeName,
                        ActivityName = item.ActivityName + "/" + item.SubActivityName,
                        BudgetHead = item.BudgetHead + "/" + item.SubBudgetHead,
                        AllocatedAmount = item.AllocatedAmount,
                        isActive = item.isActive
                    });
                }
                return lstBudgetAllocationCircle;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public JsonResult GetActivity(string SchemeID)
        {
            try
            {
                List<SelectListItem> lstActivity = new List<SelectListItem>();

                var param1 = new SqlParameter("@SchemeID", SchemeID);
                var result = fmdsscontext.Database.SqlQuery<View_BudgetHead_Allocation>("BA_GetActivity @SchemeID", param1).ToList();
                foreach (var item in result)
                {
                    lstActivity.Add(new SelectListItem { Text = item.ActivityName, Value = Convert.ToString(item.ActivityID) });
                }
                return Json(lstActivity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }


        public JsonResult GetSubActivity(string ActivityID)
        {
            try
            {
                List<SelectListItem> lstSubActivity = new List<SelectListItem>();

                var param1 = new SqlParameter("@ActvityID", ActivityID);
                var result = fmdsscontext.Database.SqlQuery<View_BudgetHead_Allocation>("BA_GetSubActivity @ActvityID", param1).ToList();
                foreach (var item in result)
                {
                    lstSubActivity.Add(new SelectListItem { Text = item.SubActivityName, Value = Convert.ToString(item.SubActivityID) });
                }
                return Json(lstSubActivity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        public JsonResult GetBudgetSubHead(string budgetHead)
        {
            try
            {
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();

                var query = (from a in fmdsscontext.tbl_mst_SubBudgetHead
                             select new
                             {
                                 a.BudgetHeadID,
                                 a.SubBudgetHead,
                                 a.ID
                             }).ToList();
                var subBudgetHead = query.Where(i => i.BudgetHeadID == Convert.ToInt64(budgetHead));
                foreach (var item in subBudgetHead)
                {

                    lstBudgetHead.Add(new SelectListItem { Text = item.SubBudgetHead, Value = Convert.ToString(item.ID) });
                }
                return Json(lstBudgetHead, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }


        public ActionResult GetAllocatedAmtDetails(View_BudgetHead_Allocation objCircle)
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

                var result = fmdsscontext.Database.SqlQuery<View_BudgetHead_Allocation>("BA_getAVailableAmountforHead @BudgetHeadID,@SubBudgetHeadID,@FY_ID,@SchemeID,@ActivityID,@SubActivityID", param1, param2, param3, param4, param5, param6).ToList();

                var fresult = result.FirstOrDefault();
                decimal totalAvaliableAmount = fresult.AvailableAmount;
                decimal totalAllocatedAmount = fresult.TotalAllocatedAmount;
                var json = Json(new {totalAvaliableAmount=totalAvaliableAmount,totalAllocatedAmount= totalAllocatedAmount});
                return json;

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }



        }



        public ActionResult Submit()
        {
            try
            {
                if (Session["HeadList"] != null)
                {
                    List<View_BudgetHead_Allocation> listDivisionView = (List<View_BudgetHead_Allocation>)Session["HeadList"];
                    foreach (View_BudgetHead_Allocation objItems in listDivisionView)
                    {
                        objItems.EnteredBy = Convert.ToInt64(Session["UserId"]);
                        tbl_BudgetHead_Allocation tblBudgDivUpdate = new tbl_BudgetHead_Allocation();
                        if (objItems.SubActivityID != 0)
                        {
                            tblBudgDivUpdate = fmdsscontext.tbl_BudgetHead_Allocation.FirstOrDefault(i => i.FY_ID == objItems.FY_ID && i.BudgetHeadID == objItems.BudgetHeadID && i.SchemeID == objItems.SchemeID && i.ActivityID == objItems.ActivityID && i.SubActivityID == objItems.SubActivityID && i.isActive == true);
                        }
                        else if (objItems.SubBudgetHeadID != 0)
                        {
                            tblBudgDivUpdate = fmdsscontext.tbl_BudgetHead_Allocation.FirstOrDefault(i => i.FY_ID == objItems.FY_ID && i.BudgetHeadID == objItems.BudgetHeadID && i.SchemeID == objItems.SchemeID && i.ActivityID == objItems.ActivityID && i.SubBudgetHeadID == objItems.SubBudgetHeadID && i.isActive == true);
                        }
                        else if (objItems.SubBudgetHeadID != 0 && objItems.SubBudgetHeadID != 0)
                        {
                            tblBudgDivUpdate = fmdsscontext.tbl_BudgetHead_Allocation.FirstOrDefault(i => i.FY_ID == objItems.FY_ID && i.BudgetHeadID == objItems.BudgetHeadID && i.SchemeID == objItems.SchemeID && i.ActivityID == objItems.ActivityID && i.SubActivityID == objItems.SubActivityID && i.SubBudgetHeadID == objItems.SubBudgetHeadID && i.isActive == true);

                        }

                        else
                        {
                            tblBudgDivUpdate = fmdsscontext.tbl_BudgetHead_Allocation.FirstOrDefault(i => i.FY_ID == objItems.FY_ID && i.BudgetHeadID == objItems.BudgetHeadID && i.SchemeID == objItems.SchemeID && i.ActivityID == objItems.ActivityID && i.isActive == true);
                        }


                        if (tblBudgDivUpdate == null)
                        {
                            Mapper.CreateMap<View_BudgetHead_Allocation, tbl_BudgetHead_Allocation>();
                            tbl_BudgetHead_Allocation ObjDivisionView = Mapper.Map<View_BudgetHead_Allocation, tbl_BudgetHead_Allocation>(objItems);
                            ObjDivisionView.isActive = true;
                            this.fmdsscontext.tbl_BudgetHead_Allocation.Add(ObjDivisionView);
                            fmdsscontext.Entry(ObjDivisionView).State = EntityState.Added;
                        }
                        else
                        {

                            tblBudgDivUpdate.AllocatedAmount = objItems.AllocatedAmount;
                            tblBudgDivUpdate.isActive = true;
                            this.fmdsscontext.tbl_BudgetHead_Allocation.Add(tblBudgDivUpdate);
                            fmdsscontext.Entry(tblBudgDivUpdate).State = EntityState.Modified;
                        }
                    }
                    var status = fmdsscontext.SaveChanges();
                    if (status > 0)
                    {
                        TempData["Msg"] = "Data inserted sucessfully";
                    }
                    else if (status == 0)
                    {

                        TempData["Msg"] = "Data already inserted please check!!!";
                    }
                    else
                    {
                        TempData["Msg"] = "Something wrong please check after sometime!!!";
                    }
                }
                else
                {
                    TempData["Msg"] = "Add details into list to proceed!!!";

                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("BudgetAllocationToBudgetHead", "BudgetAllocationtoSubBudgetHead");
        }


        public JsonResult AdddHeadDetails(View_BudgetHead_Allocation objModel)
        {
            List<View_BudgetHead_Allocation> objData = new List<View_BudgetHead_Allocation>();
            List<View_BudgetHead_Allocation> listDivision = new List<View_BudgetHead_Allocation>();
            try
            {
                if (objModel.TotalAllocatedAmount < objModel.AllocatedAmount)
                {
                    throw new Exception("Release amount not greater then to Total Allocated Amount.");
                }

                if (Session["HeadList"] != null)
                {
                    listDivision = (List<View_BudgetHead_Allocation>)Session["HeadList"];
                    foreach (var item in listDivision)
                    {
                        if (item.BudgetHeadID == objModel.BudgetHeadID && item.SubBudgetHeadID == objModel.SubBudgetHeadID && item.SchemeID == objModel.SchemeID && item.ActivityID == objModel.ActivityID && item.SubActivityID == objModel.SubActivityID)
                        {
                            objModel.rowid = "D";
                        }
                        else
                        {
                            if (item.rowid == objModel.rowid)
                            {
                                objData.Add(objModel);
                            }
                            else
                            {
                                objData.Add(item);
                            }
                        }
                    }
                    if (objModel.rowid == null)
                    {
                        objModel.rowid = Guid.NewGuid().ToString();
                        objData.Add(objModel);
                        Session["HeadList"] = null;
                        Session["HeadList"] = objData;
                    }
                }
                else
                {
                    objModel.rowid = Guid.NewGuid().ToString();
                    objData.Add(objModel);
                    Session["HeadList"] = objData;
                }

            }
            catch (Exception ex)
            {
                objModel.rowid = "0";
                objModel.ActivityName = ex.Message.ToString();
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteHeadDetails(string Id)
        {
            List<View_BudgetHead_Allocation> lstDivision = new List<View_BudgetHead_Allocation>();
            try
            {
                if (Session["HeadList"] != null)
                {
                    lstDivision = (List<View_BudgetHead_Allocation>)Session["HeadList"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        View_BudgetHead_Allocation ench = lstDivision.Single(a => a.rowid == Id);
                        tbl_BudgetHead_Allocation tbl = fmdsscontext.tbl_BudgetHead_Allocation.FirstOrDefault(i => i.FY_ID == ench.FY_ID && i.ActivityID == ench.ActivityID && i.BudgetHeadID == ench.BudgetHeadID && i.SchemeID == ench.SchemeID && i.SubActivityID == ench.SubActivityID && i.SubBudgetHeadID == ench.SubBudgetHeadID && i.isActive == true);
                        if (tbl != null)
                        {
                            tbl.isActive = false;
                            fmdsscontext.Entry(tbl).State = System.Data.Entity.EntityState.Modified;
                            fmdsscontext.SaveChanges();
                        }
                        lstDivision.Remove(ench);

                    }
                    Session["HeadList"] = lstDivision;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(lstDivision, JsonRequestBehavior.AllowGet);
        }



    }
}
