using FMDSS.Models;
using FMDSS.Models.ForestDevelopment;
using FMDSS.Models.ForesterDevelopment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForestDevelopment
{
    public class ModelController : Controller
    {

        int ModuleID = 1;
        Int64 UserID = 0;
        DefineModel _objModel = new DefineModel();
        //
        // GET: /Model/
        [HttpGet]
        public ActionResult Model()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    ViewBag.Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                    return View();
                } 
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
           
        }

        [HttpPost]
        public ActionResult SaveModelData(DefineModel Model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    

                    if (Model.Model_Code == null || Model.Model_Code.ToString() == "")
                    {
                        _objModel.Model_Code = "";
                    }
                    else { _objModel.Model_Code = Model.Model_Code; }

                    if (Model.Model_Name == null || Model.Model_Name.ToString() == "")
                    {
                        _objModel.Model_Name = "";
                    }
                    else { _objModel.Model_Name = Model.Model_Name; }

                    if (Model.LaborPerDay == null)
                    {
                        _objModel.LaborPerDay = 0;
                    }
                    else { _objModel.LaborPerDay = Model.LaborPerDay; }

                    if (Model.Unit == null || Model.Unit.ToString() == "")
                    {
                        _objModel.Unit = "";
                    }
                    else { _objModel.Unit = Model.Unit; }

                    if (Model.Perimeter == null || Model.Perimeter.ToString() == "")
                    {
                        _objModel.Perimeter = "";
                    }
                    else { _objModel.Perimeter = Model.Perimeter; }

                    if (Model.AveragePerimeter == null || Model.AveragePerimeter.ToString() == "")
                    {
                        _objModel.AveragePerimeter = "";
                    }
                    else { _objModel.AveragePerimeter = Model.AveragePerimeter; }

                    if (Model.Specing == null || Model.Specing.ToString() == "")
                    {
                        _objModel.Specing = "";
                    }
                    else { _objModel.Specing = Model.Specing; }

                    if (Model.PerRKM == null || Model.PerRKM.ToString() == "")
                    {
                        _objModel.PerRKM = "";
                    }
                    else { _objModel.PerRKM = Model.PerRKM; }

                    if (Model.Model_FromDate == null || Model.Model_FromDate.ToString() == "")
                    {
                        _objModel.Model_FromDate = "";
                    }
                    else { _objModel.Model_FromDate = Model.Model_FromDate; }

                    if (Model.Model_ToDate == null || Model.Model_ToDate.ToString() == "")
                    {
                        _objModel.Model_ToDate = "";
                    }
                    else { _objModel.Model_ToDate = Model.Model_ToDate; }

                    Int64 id = _objModel.SubmitDefineModel(_objModel);
                    if (id > 0)
                    {
                        return View("Model");

                    }
                    else
                    {
                        return View("Error");
                    }

                }
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;

            
        }

    }
}
