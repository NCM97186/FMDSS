using FMDSS.Models;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Master
{
    public class TermAndConditionMasterController : BaseController
    {
        //
        // GET: /TermAndConditionMaster/
        TermAndCondition obj = new TermAndCondition();

        List<TermAndCondition> TermsConditionLst = new List<TermAndCondition>();

        List<ListTermAndCondition> ListTC = new List<ListTermAndCondition>();

        List<SelectListItem> lstPlaces = new List<SelectListItem>();

        List<SelectListItem> lstISactive = new List<SelectListItem>();
        
      
        #region "TermsCondition"
        public ActionResult TermsCondition(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> TermsCondition = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            TermAndCondition obj = new TermAndCondition();
            try
            {

                DataTable dtf = obj.Select_TermsConditions();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    TermsConditionLst.Add(
                        new TermAndCondition()
                        {
                            Index = count,
                            ID = Convert.ToInt32(dr["ID"]),
                            TermAndCondition_Text = Convert.ToString(dr["TermAndCondition_Text"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(TermsConditionLst);
        }
         [ValidateInput(false)]
        public ActionResult AddUpdateTermsCondition(TermAndCondition  oTermsCondition)
        {
            List<SelectListItem> TermsCondition = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                //TermsCondition obj = new TermsCondition();

                DataTable dtf = obj.AddUpdateTermsCondition(oTermsCondition);
                //oTicker.LastUpdatedBy = UserID;
                //status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("TermsCondition", new { RecordStatus = status });
        }
        public ActionResult GetTermsCondition(string ID)
        {

            //TermsCondition obj = new TermsCondition();
            List<SelectListItem> TermsCondition = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (ID == "0" ? "Add Term And Condition" : "Edit Term And Condition");


                DataTable dtf = obj.Select_TermsCondition(Convert.ToInt32(ID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new TermAndCondition 
                    {
                        ID = Convert.ToInt32(dr["ID"].ToString()),
                        TermAndCondition_Text = Convert.ToString(dr["TermAndCondition_Text"].ToString()),
                        IsActive = Convert.ToInt32(dr["IsActive"]),
                        OperationType = "Edit TermsCondition"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialTermsCondition", obj);
        }


        #endregion



        [HttpGet]
        public ActionResult MappingTermAndCondition()
        {

            DataTable DT = obj.GetPlaces();
            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstPlaces.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
            }
            ViewBag.placeIDs = lstPlaces;

            obj.LstTermAndCondition = ListTC;

            return View(obj);

        }

        [HttpPost]
        public ActionResult MappingTermAndCondition(TermAndCondition model,string command)
        {
            DataTable DT = obj.GetPlaces();

            foreach (System.Data.DataRow dr in DT.Rows)
            {
                lstPlaces.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
            }
            ViewBag.placeIDs = lstPlaces;

            if (command.Equals("View"))
            {


                DataTable dtf = obj.GetTermAndConditionDataOnPlaces(model.PlaceID);
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ListTC.Add(
                        new ListTermAndCondition()
                        {
                            Index = count,
                            ID = Convert.ToInt32(dr["ID"].ToString()),
                            TermAndConditionID = Convert.ToInt32(dr["TermAndConditionID"].ToString()),
                            PlaceID = Convert.ToInt32(dr["PlaceID"].ToString()),
                            TermAndCondition_Text = Convert.ToString(dr["TermAndCondition_Text"].ToString()),
                            IsActive = Convert.ToBoolean(dr["IsActive"]),
                            SetDisplayOrder = Convert.ToInt32(dr["SetDisplayOrder"]),

                        });
                    count += 1;
                }
                model.LstTermAndCondition = ListTC;
            }
            else if (command.Equals("Submit"))
            {


                DataTable dtf = obj.GetTermAndConditionDataOnPlaces(model.PlaceID);
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ListTC.Add(
                        new ListTermAndCondition()
                        {
                            Index = count,
                            ID = Convert.ToInt32(dr["ID"].ToString()),
                            TermAndConditionID = Convert.ToInt32(dr["TermAndConditionID"].ToString()),
                            PlaceID = Convert.ToInt32(dr["PlaceID"].ToString()),
                            TermAndCondition_Text = Convert.ToString(dr["TermAndCondition_Text"].ToString()),
                            IsActive = Convert.ToBoolean(dr["IsActive"]),
                        });
                    count += 1;
                }
                model.LstTermAndCondition = ListTC;
            }
            return View(model);
        }

        public JsonResult Mapping(int IDs, int PIDs, int TCIDs, bool STATUS, int SetDisplayOrder = 0)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = "0";
            try
            {
                obj.ID = IDs;
                obj.PlaceID = PIDs;
                obj.TermAndConditionID = TCIDs;
                obj.IsactiveView = STATUS;
                obj.SetDisplayOrder = SetDisplayOrder;


              status = obj.AddUpdateTandC(obj);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

    }
}
