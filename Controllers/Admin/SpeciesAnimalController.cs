using FMDSS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Filters;

namespace FMDSS.Controllers.Admin
{
       [MyAuthorization]
    public class SpeciesAnimalController : BaseController
    {
        //
        // GET: /SpeciesAnimal/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddSpeciesAnimal()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetSpeciesAnimal(string category)
        {
            CategorySpeciesAnimal objCategory = new CategorySpeciesAnimal();
            List<SelectListItem> items = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(category))
            {

                DataTable dt = objCategory.BindCategorySpanimal(category);
                ViewBag.fname = dt;


            }

            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AddSpaciesanimal(FormCollection fm, string Command)
        {

            try
            {

                CategorySpeciesAnimal categorySpeciesAnimal = new CategorySpeciesAnimal();
                categorySpeciesAnimal.CategoryId = 0;
                //if (Session["SSODetail"] != null)
                //{
                //    User user = (User)Session["SSODetail"];
                //    categorySpeciesAnimal.CreatedBy = user.UserId;
                //}

                if (fm["Category"].ToString() == "")
                {
                    categorySpeciesAnimal.Category = "";
                }
                else
                {
                    categorySpeciesAnimal.Category = fm["Category"].ToString();
                }
                if (fm["CategoryspanimalId"].ToString() == "")
                {
                    categorySpeciesAnimal.CategoryspanimalId = 0;
                }
                else
                {
                    categorySpeciesAnimal.CategoryspanimalId = Convert.ToInt64(fm["CategoryspanimalId"].ToString());
                }

                if (fm["Name"].ToString() == "")
                {
                    categorySpeciesAnimal.Name = "";
                }
                else
                {
                    categorySpeciesAnimal.Name = fm["Name"].ToString();
                }

                if (fm["Snospanimal"].ToString() == "")
                {
                    categorySpeciesAnimal.SnoSpanimal = "";
                }
                else
                {
                    categorySpeciesAnimal.SnoSpanimal = fm["Snospanimal"].ToString();
                }

                if (fm["Description"].ToString() == "")
                {
                    categorySpeciesAnimal.Description = "";
                }
                else
                {
                    categorySpeciesAnimal.Description = fm["Description"].ToString();
                }

                if (Command == "Save")
                {
                    Int64 status = categorySpeciesAnimal.AddSpeciesAnimal();
                    if (status > 0)
                    {
                        Session["Status"] = "Inserted Sucessfully";
                    }
                    else
                    {
                        Session["Status"] = "Not inserted";
                    }
                }
                return RedirectToAction("AddSpeciesAnimal", "SpeciesAnimal");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
