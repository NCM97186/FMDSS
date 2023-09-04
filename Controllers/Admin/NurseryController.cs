using FMDSS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Admin
{
    public class NurseryController : BaseController
    {
        //
        // GET: /Nursery/
        List<SelectListItem> distrct = new List<SelectListItem>();
        Location location = new Location();
        public ActionResult Nursery()
        {

            DataTable dt = location.District();
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    distrct.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.DistrictCode = distrct;
            }

            return View();
        }

    }
}
