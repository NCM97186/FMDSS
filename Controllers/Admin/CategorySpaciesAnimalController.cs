using FMDSS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Filters;

namespace FMDSS.Controllers.Admin
{
      [MyAuthorization]
    public class CategorySpaciesAnimalController : BaseController
    {
        //
        // GET: /CategorySpaciesAnimal/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCategorySpeciesAnimal()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depot"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult SubmitCategorySpaciesanimal(FormCollection fm, string Command)
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
                if (fm["Name"].ToString() == "")
                {
                    categorySpeciesAnimal.Name = "";
                }
                else
                {
                    categorySpeciesAnimal.Name = fm["Name"].ToString();
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
                    Int64 status = categorySpeciesAnimal.AddCategorySpeciesAnimal();
                    if (status > 0)
                    {
                        Session["Status"] = "Inserted Sucessfully";
                    }
                    else
                    {
                        Session["Status"] = "Not inserted";
                    }


                }
                return RedirectToAction("AddCategorySpeciesAnimal", "CategorySpaciesAnimal");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
