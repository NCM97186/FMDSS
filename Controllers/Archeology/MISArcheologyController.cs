using FMDSS.Models.Archeology;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Web.Mvc;

namespace FMDSS.Controllers.Archeology
{
    public class MISArcheologyController : Controller
    {
        //
        // GET: /MISArcheology/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MISData()
        {
            GetDistrict();
            return View();
        }

        public void GetDistrict()
        {
            var obj = new ArcheologyFunctions(); DataTable dt = new DataTable(); List<SelectListItem> lst = new List<SelectListItem>();
            var lstplace = new List<SelectListItem>();
            try
            {
                dt = obj.GetDistrictMaster();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lst.Add(new SelectListItem { Text = Convert.ToString(@dr["DistrictName"]), Value = Convert.ToString(@dr["PK_Id"]) });
                }
                ViewBag.District = lst;
                // lstplace.Add(new SelectListItem { Text = "select", Value = "0" });
                ViewBag.Place = lstplace;// null;// lstplace;
            }
            catch (Exception ex)
            {

            }

        }

        [System.Web.Http.HttpPost]
        public ActionResult FinalSubmit([FromBody] ArcheologyMIS input)
        {
            var data = input;

            var obj = new ArcheologyFunctions(); DataTable dt = new DataTable();
            try
            {
                dt = obj.GetArcheologyMISData(input);

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }


            return View();
        }

        [System.Web.Http.HttpGet]
        public JsonResult GetPlacesByDistrictId(int Districtid)
        {
            var obj = new ArcheologyFunctions(); DataTable dt = new DataTable(); List<SelectListItem> lst = new List<SelectListItem>();
            try
            {
                dt = obj.GetArcheologyareaByDistrict(Districtid);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lst.Add(new SelectListItem { Text = Convert.ToString(@dr["AreaName"]), Value = Convert.ToString(@dr["PK_Id"]) });
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
