using FMDSS.Models;
using FMDSS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Admin
{
    public class CoordinatorRegistrationController : BaseController
    {
        //
        // GET: /CoordinatorRegistration/
        Int64 UserID = 0;
        int ModuleID = 1;
        Location location = new Location();
        List<SelectListItem> items = new List<SelectListItem>();
        CordinatorManagement obj = new CordinatorManagement();
        public ActionResult CoordinatorRegistration()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                #region district
                DataTable dt = new DataTable();
                dt = location.District();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }

                ViewBag.DistrictCode = items;
                // ViewBag.ToLocation = items;
                #endregion

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View();
        }

        public ActionResult Index()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<CordinatorManagement> result = new List<CordinatorManagement>();
            try
            {

                #region Bind Coordinator Detail

                CordinatorManagement CordinatorManagement = null;
                DataTable corddt = obj.BindCordinator(Convert.ToInt64(Session["UserId"]));
                if (corddt != null)
                {
                    if (corddt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in corddt.Rows)
                        {


                            CordinatorManagement = new CordinatorManagement()
                            {

                                CoordinatorId = Convert.ToInt64(dr["ID"].ToString()),
                                CoordinatorNo = dr["CoordinatorId"].ToString(),
                                CoordinatorName = dr["Name"].ToString(),
                                Address = dr["Address"].ToString() + "," + dr["DIST_NAME"].ToString() + "-" + dr["Pincode"].ToString(),
                               


                            };
                            result.Add(CordinatorManagement);
                        }

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("Index", result);
            //return View();
        }

        /// <summary>
        /// call for save data in database
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitCoordinatorform(CordinatorManagement cID, FormCollection fc)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                string msg=string.Empty;
                cID.EnteredBy = Convert.ToInt64(Session["UserId"]);
                if (cID.CoordinatorId == 0)
                {
                    cID.ActionFlag = "INSERT";
                    msg = " Created Successfully";
                }
                else
                {
                    cID.ActionFlag = "UPDATE";
                    msg = " Updated Successfully";
                }

                string tpNo = obj.CreateCoorditor(cID);
                if (!String.IsNullOrEmpty(tpNo))
                {
                    TempData["CO_Status"] = "Coordinator ID:#" + tpNo + msg;
                }

               
            }
            catch(Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("CoordinatorRegistration");
        }

        /// <summary>
        /// for Edit Coordinator 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Int64 id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CordinatorManagement obj = new CordinatorManagement();
            try
            {
                UserID = Convert.ToInt64(Session["UserID"].ToString());
                DataTable cdt = obj.getCoordinatorVal(id);
                if (cdt != null)
                {
                    DataTable dt = location.District();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }

                    ViewBag.DistrictCode = new SelectList(items, "Value", "Text", cdt.Rows[0]["DIST_CODE"].ToString());

                    obj.CoordinatorName = cdt.Rows[0]["Name"].ToString();
                    obj.SSOID = cdt.Rows[0]["SSOID"].ToString();
                    obj.Address = cdt.Rows[0]["Address"].ToString();
                    obj.Pincode = cdt.Rows[0]["Pincode"].ToString();
                    obj.CoordinatorId = Convert.ToInt64(cdt.Rows[0]["ID"].ToString());
                    obj.DistrictCode = cdt.Rows[0]["DIST_CODE"].ToString();
                    //obj.ActionFlag = "UPDATE";

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
           

            return View("CoordinatorRegistration", obj);
        }


    }
}
