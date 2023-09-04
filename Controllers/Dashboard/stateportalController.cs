using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Dashboard
{
    public class stateportalController : Controller
    {
        //
        // GET: /stateportal/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /stateportal/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /stateportal/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /stateportal/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /stateportal/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /stateportal/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /stateportal/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /stateportal/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
