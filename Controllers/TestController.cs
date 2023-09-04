using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            if (Session["HoldIpAddress"] == null)
            {
                Session["HoldIpAddress"] = Request.Url;
            }
          
            return View();
        }

        [HttpPost]
        public ActionResult Index(string str)
        {
            Session["HoldIpAddress"] = str;
            return View();
        }
    }
}
