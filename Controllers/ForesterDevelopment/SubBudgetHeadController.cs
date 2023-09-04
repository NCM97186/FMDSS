using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System.Data.SqlClient;
using FMDSS.Repository;
using System.IO;
using AutoMapper;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.Encroachment.DomainModel;
using System.Data.Entity;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class SubBudgetHeadController : Controller
    {
        
        public FmdssContext dbContext;
        public SubBudgetHeadController()
        {
            dbContext = new FmdssContext();
        }
        public ActionResult SubBudgetHead()
        {
            try
            {
                List<View_Mst_SubBudgetHead> lstSubBudgetList = new List<View_Mst_SubBudgetHead>();
                List<SelectListItem> lstBudgetHead = new List<SelectListItem>();
                var result = (from a in dbContext.tbl_mst_SubBudgetHead 
                              join b in dbContext.tbl_mst_BudgetHead on a.BudgetHeadID equals b.ID
                              select new { a.BudgetHeadID, b.BudgetHead, a.SubBudgetHead, a.ID, a.Descriptions });

                foreach (var item in result)
                {
                    lstSubBudgetList.Add(
                        new View_Mst_SubBudgetHead()
                        {
                            ID = item.ID,
                            BudgetHead = item.BudgetHead,
                            BudgetHeadID = item.BudgetHeadID,
                            SubBudgetHead = item.SubBudgetHead,
                            Descriptions = item.Descriptions
                        });
                }

                var budgetHead = dbContext.tbl_mst_BudgetHead.Select(i => new { i.ID, i.BudgetHead, i.HaveSubBudgetHead }).Where(i => i.HaveSubBudgetHead == true).ToList();

                foreach (var item in budgetHead) {

                    lstBudgetHead.Add(new SelectListItem { Text=item.BudgetHead,Value= Convert.ToString(item.ID)});
                }

                ViewData["SubBudgetHeadlst"] = lstSubBudgetList;
                ViewBag.BudgetHead = lstBudgetHead;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
            return View();
        }


        public ActionResult Submit(View_Mst_SubBudgetHead objSubBudget) {

            try
            {

                string msg = string.Empty;
                Mapper.CreateMap<View_Mst_SubBudgetHead, tbl_mst_SubBudgetHead>();
                tbl_mst_SubBudgetHead objSubBudgetInsert = Mapper.Map<View_Mst_SubBudgetHead, tbl_mst_SubBudgetHead>(objSubBudget);

                tbl_mst_SubBudgetHead tbl = dbContext.tbl_mst_SubBudgetHead.FirstOrDefault(i => i.ID == objSubBudget.ID);
                objSubBudgetInsert.EnterBy = Convert.ToInt64(Session["UserId"]);

                if (tbl == null)
                {

                     tbl_mst_SubBudgetHead duplicateChk = dbContext.tbl_mst_SubBudgetHead.FirstOrDefault(i => i.SubBudgetHead == objSubBudget.SubBudgetHead && i.BudgetHeadID == objSubBudget.BudgetHeadID);
                     if (duplicateChk == null)
                     {
                         dbContext.Entry(objSubBudgetInsert).State = EntityState.Added;
                         int status = dbContext.SaveChanges();
                         if (status > 0)
                         {
                             TempData["Message"] = "Data inserted sucessfully";
                         }
                         else
                         {
                             TempData["Message"] = "Something wrong please check after sometime!!!";
                         }
                     }
                     else
                     {
                         TempData["Message"] = "Data already exists please check!!!";
                     }

                    //dbContext.Entry(objSubBudgetInsert).State = EntityState.Added;
                    //msg = "Data inserted sucessfully";
                }
                else {
                    //tbl.BudgetHeadID = objSubBudgetInsert.BudgetHeadID;
                    //tbl.SubBudgetHead = objSubBudgetInsert.SubBudgetHead;
                    //dbContext.Entry(tbl).State = EntityState.Modified;
                    //msg = "Data updated sucessfully";


                    tbl.BudgetHeadID = objSubBudgetInsert.BudgetHeadID;
                    tbl.SubBudgetHead = objSubBudgetInsert.SubBudgetHead;
                    tbl.Descriptions = objSubBudgetInsert.Descriptions;
                    dbContext.Entry(tbl).State = EntityState.Modified;
                    int status = dbContext.SaveChanges();
                    if (status > 0)
                    {
                        TempData["Message"] = "Data Updated sucessfully";
                    }
                    else if (status == 0)
                    {

                        TempData["Message"] = "Data not updated!!!";
                    }
                    else
                    {
                        TempData["Message"] = "Something wrong please check after sometime!!!";
                    }                   

                }
                //int status = dbContext.SaveChanges();
                //if (status > 0)
                //{
                //    TempData["Message"] = msg;
                //}                
                //else
                //{
                //    TempData["Message"] = "Something wrong please check after sometime!!!";
                //}
            }
            catch (Exception) {
                throw;
            }
            
            return RedirectToAction("SubBudgetHead", "SubBudgetHead");
        
        }

        public ActionResult EditBudgetHead(int Id)
        {
            try
            {
                List<View_Mst_SubBudgetHead> lstSubBudgetHead = new List<View_Mst_SubBudgetHead>();               
                var result = (from a in dbContext.tbl_mst_SubBudgetHead
                              join b in dbContext.tbl_mst_BudgetHead on a.BudgetHeadID equals b.ID
                              select new { a.BudgetHeadID, b.BudgetHead, a.SubBudgetHead, a.ID, a.Descriptions });
                var fresult = result.Where(i => i.ID == Id);
                foreach (var item in fresult)
                {
                    lstSubBudgetHead.Add(
                        new View_Mst_SubBudgetHead()
                        {
                            ID = item.ID,
                            BudgetHead = item.BudgetHead,
                            BudgetHeadID = item.BudgetHeadID,
                            SubBudgetHead = item.SubBudgetHead,
                            Descriptions = item.Descriptions
                        });
                }
                return Json(lstSubBudgetHead, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
            return null;

        }

    }
}
