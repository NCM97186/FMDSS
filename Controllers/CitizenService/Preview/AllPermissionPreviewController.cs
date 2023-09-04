using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.CitizenService.ProductionServices.EducationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Filters;

namespace FMDSS.Controllers.CitizenService.Preview
{
    [MyAuthorization]
    public class AllPermissionPreviewController : BaseController
    {
        //
        // GET: /AllPermissionPreview/

        public ActionResult PermissionPreview(string pertype)
        {
            Research research = new Research();

            Research researchobj = null;
            FilmShooting filmShooting = null;
            OrganisingCamps organisingCamps = null;

            if (!String.IsNullOrEmpty(pertype))
            {
                Session["Permission"] = null;

                Session["Permission"] = pertype;

                if (pertype == "Research")
                {
                    if (Session["Research"] != null)
                    {

                        researchobj = (Research)Session["Research"];

                    }
                }

                if (pertype == "Film Shooting")
                {
                    if (Session["FilmShooting"] != null)
                    {
                        filmShooting = (FilmShooting)Session["FilmShooting"];
                    }
                }

                if (pertype == "Organizing Camp")
                {

                    if (Session["OrcCamp"] != null)
                    {

                        organisingCamps = (OrganisingCamps)Session["OrcCamp"];


                    }

                }
            }

            return View(Tuple.Create(researchobj, filmShooting, organisingCamps));
        }

    }

}
