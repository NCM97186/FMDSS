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
    public class BudgetHeadController : Controller
    {
         public FmdssContext fmdsscontext;

         public BudgetHeadController()
        {
            fmdsscontext = new FmdssContext();
        }

        public ActionResult InsertBudgetHead()
         {
             try
             {
                 var Scheme = (from a in fmdsscontext.tbl_mst_BudgetHead select new { a.BudgetHead,a.HaveSubBudgetHead,a.ID,a.TypeOfHead});

                 List<View_Mst_BudgetHead> BudgetList = new List<View_Mst_BudgetHead>();
                 foreach (var item in Scheme)
                     BudgetList.Add(
                         new View_Mst_BudgetHead()
                         {

                             BudgetHead = item.BudgetHead,
                             ID = item.ID,
                             HaveSubBudgetHead = item.HaveSubBudgetHead,
                                         TypeOfHead = item.TypeOfHead,
                             
                 

                         });
                 ViewBag.BudgetHeadlst = BudgetList;
             }
             catch (Exception ex)
             {
                 throw ex;
             }

           
            return View();
        }

        //
        // GET: /BudgetHead/Details/5

        public ActionResult Submit(View_Mst_BudgetHead objBudgetHead)
        {
              try
            {
            tbl_mst_BudgetHead tbl = fmdsscontext.tbl_mst_BudgetHead.FirstOrDefault(i => i.ID == objBudgetHead.ID);

            if (tbl == null)
            {               
                objBudgetHead.EnterBy = Convert.ToInt64(Session["UserId"]);
                Mapper.CreateMap<View_Mst_BudgetHead, tbl_mst_BudgetHead>();
                tbl_mst_BudgetHead ObjDivisionView = Mapper.Map<View_Mst_BudgetHead, tbl_mst_BudgetHead>(objBudgetHead);

                  tbl_mst_BudgetHead duplicatechk = fmdsscontext.tbl_mst_BudgetHead.FirstOrDefault(i => i.BudgetHead == objBudgetHead.BudgetHead && i.TypeOfHead == objBudgetHead.TypeOfHead && i.HaveSubBudgetHead == objBudgetHead.HaveSubBudgetHead);
                  if (duplicatechk == null)
                  {
                      this.fmdsscontext.tbl_mst_BudgetHead.Add(ObjDivisionView);
                      fmdsscontext.Entry(ObjDivisionView).State = EntityState.Added;
                      var status = fmdsscontext.SaveChanges();
                      if (status > 0)
                      {
                          TempData["Message"] = "Data inserted sucessfully";
                      }
                      else if (status == 0)
                      {

                          TempData["Message"] = "Data already inserted please check!!!";
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
            }
            else {
                tbl.BudgetHead = objBudgetHead.BudgetHead;
                tbl.HaveSubBudgetHead = objBudgetHead.HaveSubBudgetHead;
                tbl.TypeOfHead = objBudgetHead.TypeOfHead;
                this.fmdsscontext.tbl_mst_BudgetHead.Add(tbl);
                fmdsscontext.Entry(tbl).State = EntityState.Modified;
                int status = fmdsscontext.SaveChanges();              
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
          

            }
              catch (Exception)
              {

                  throw;
              }



              return RedirectToAction("InsertBudgetHead", "BudgetHead");
        }


        public ActionResult EditBudgetHead(int Id)
        {



            try
            {
                List<View_Mst_BudgetHead> lstBudgetHead = new List<View_Mst_BudgetHead>();
                var query = (from a in fmdsscontext.tbl_mst_BudgetHead
                             select new
                             {
                                 a.ID,
                                 a.BudgetHead,
                                 a.TypeOfHead,
                                 a.HaveSubBudgetHead,
                             }).ToList();
                var BudgetHead = query.Where(i => i.ID == Convert.ToInt64(Id));

                foreach (var item in BudgetHead)
                {
                    lstBudgetHead.Add(new View_Mst_BudgetHead()
                    {
                        ID = item.ID,
                        BudgetHead=item.BudgetHead,
                        TypeOfHead=item.TypeOfHead,
                        HaveSubBudgetHead=item.HaveSubBudgetHead,
                    });
                    return Json(lstBudgetHead, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
            return null;
      
        }

        //
        // POST: /BudgetHead/Edit/5

      
        public ActionResult UpdateBudgetHead(View_Mst_BudgetHead objBudgetHead)
        {
            try
            {
                var result = (from p in fmdsscontext.tbl_mst_BudgetHead
                              where p.ID == objBudgetHead.ID
                                 select p).SingleOrDefault();

 
                tbl_mst_BudgetHead tbl = fmdsscontext.tbl_mst_BudgetHead.FirstOrDefault(i => i.ID == objBudgetHead.ID);
                if (tbl != null)
                {
                    tbl.BudgetHead = objBudgetHead.BudgetHead;
                    tbl.HaveSubBudgetHead = objBudgetHead.HaveSubBudgetHead;
                    tbl.TypeOfHead = objBudgetHead.TypeOfHead;
                    int status = fmdsscontext.SaveChanges();
                    if (status > 0)
                    {
                        TempData["Message1"] = "Data Updated sucessfully";
                    }
                    else if (status == 0)
                    {

                        TempData["Message1"] = "Data not updated!!!";
                    }
                    else
                    {
                        TempData["Message1"] = "Something wrong please check after sometime!!!";
                    }

                }
                else
                {
                    TempData["Message1"] = "Data does not exist!!!";
                }
               

                // TODO: Add update logic here

                return RedirectToAction("InsertBudgetHead", "BudgetHead");
            }
            catch
            {
                return View();
            }
        }

       
    }
}
