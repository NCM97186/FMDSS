using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Master
{
    public class RevenueDataManageController : BaseController
    {
        //
        // GET: /RevenueManage/
        #region global variables
       
        Location location = new Location();
        AuctionRevenueMaster _objAuRev = new AuctionRevenueMaster();
        List<SelectListItem> items = new List<SelectListItem>();
        List<SelectListItem> revinueItem = new List<SelectListItem>();
        List<SelectListItem> rangeCode = new List<SelectListItem>();
        int ModuleID = 3;
        Int64 UserID = 0;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RevenueDataManage()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
             UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dt = new DataTable();
             List<AuctionRevenueMaster> listAu = new List<AuctionRevenueMaster>();
            try
            {
                
                DataTable dtf = _objAuRev.Select_AuctionRevenue(0, "VIEW", UserID);

                foreach (DataRow dr in dtf.Rows)
                {
                    listAu.Add(new AuctionRevenueMaster()
                    {
                        RowID = Convert.ToInt64(dr["RowID"].ToString()),
                        RangeCode = dr["RANGE_NAME"].ToString(),
                        DepotName = dr["Depot_Name"].ToString(),
                        ForestProducename = dr["ProduceType"].ToString(),
                        ForestProductName = dr["ProductName"].ToString(),
                        Qty = dr["Revenue_Qty"].ToString(),
                        RevenueYear = dr["Revenue_Year"].ToString(),
                        RevinueAmount = dr["Revenue_Amount"].ToString(),
                        ProduceUnit = dr["Product_Unit"].ToString(),
                        // IsActive = Convert.ToInt16(dr["IsActive"].ToString()),

                    });
                }


                dt = new Common().Select_Range(UserID);
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }

                ViewBag.RangeCode = items;

                ViewBag.RevenueYear = new SelectList(Common.GetLastThreeYear(), "Value", "Text");
                
             
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(listAu);
        }



        /// <summary>
        /// bind data for update notice Number
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult EditDetails(string collectionID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            AuctionRevenueMaster obj_no = null;

            try
            {


                if (!String.IsNullOrEmpty(collectionID))
                {

                    _objAuRev.EnteredBy = Convert.ToInt64(Session["UserId"]);
                    DataTable dtf = _objAuRev.Select_AuctionRevenue(Convert.ToInt64(collectionID), "EDIT", _objAuRev.EnteredBy);

                   

                    foreach (DataRow dr in dtf.Rows)
                    {
                        obj_no = new AuctionRevenueMaster();

                        obj_no.RowID = Convert.ToInt64(dr["RowID"].ToString());
                        obj_no.RangeName = dr["RANGE_NAME"].ToString();
                        obj_no.RangeCode = dr["RANGE_CODE"].ToString();
                        obj_no.DepotId = Convert.ToInt64(dr["Depot_Id"].ToString());
                        obj_no.DepotName = dr["Depot_Name"].ToString();
                        obj_no.ForestProduceID = Convert.ToInt64(dr["ID"].ToString());
                        obj_no.ForestProducename = dr["ProduceType"].ToString();
                        obj_no.ForestProductID = Convert.ToInt64(dr["productID"].ToString());
                        obj_no.ForestProductName = dr["ProductName"].ToString();
                        obj_no.Qty = dr["Revenue_Qty"].ToString();
                        obj_no.RevenueYear = dr["Revenue_Year"].ToString();
                        obj_no.RevinueAmount = dr["Revenue_Amount"].ToString();
                        obj_no.ProduceUnit = dr["Product_Unit"].ToString();


                    }


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(obj_no, JsonRequestBehavior.AllowGet);
        }





        /// <summary>
        /// Bind Depot dropdown
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public JsonResult getDepot(string rangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                if (!String.IsNullOrEmpty(rangeCode))
                {
                    Location location = new Location();
                  
                    DataTable dt = location.BindDepotbyRangeCode(rangeCode);

                    return dtToViewBagJSON(dt, "Depot_Name", "Depot_Id");

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }

            return null;
        }



        /// <summary>
        /// Delete Revenue Data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeletRevenueeData(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

          
            Int64 UpdatedBy = 0;

            try
            {
                if (Session["UserID"] != null)
                {

                    UpdatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (!String.IsNullOrEmpty(id))
                {

                    string status = _objAuRev.DeactivateNoticeNo(Convert.ToInt64(id), UpdatedBy, "Revenue");

                    if (!String.IsNullOrEmpty(status.ToString()))
                    {
                        TempData["Revenue_Status"] = "Revenue Deleted Sucessfully";
                    }
                    else
                    {
                        TempData["Revenue_Status"] = "Not Deleted";
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return RedirectToAction("RevenueDataManage", "RevenueDataManage");
        }


        /// <summary>
        /// dtToViewBagJSON
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        /// <returns></returns>
        public JsonResult dtToViewBagJSON(DataTable dt, string TextField, string ValueField)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    //DataTable dt = _obj.SelectMicroPlanByVilageCode(Village_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr[TextField].ToString(), Value = @dr[ValueField].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<AuctionRevenueMaster> listAu = new List<AuctionRevenueMaster>();
            try
            {
                _objAuRev.EnteredBy = Convert.ToInt64(Session["UserId"]);
                DataTable dtf = _objAuRev.Select_AuctionRevenue(0,"VIEW", _objAuRev.EnteredBy);

                foreach (DataRow dr in dtf.Rows)
                {
                    listAu.Add(new AuctionRevenueMaster()
                    {
                        RowID = Convert.ToInt64(dr["RowID"].ToString()),
                        RangeCode = dr["RANGE_NAME"].ToString(),
                        DepotName = dr["Depot_Name"].ToString(),
                        ForestProducename = dr["ProduceType"].ToString(),
                        ForestProductName = dr["ProductName"].ToString(),
                        Qty = dr["Revenue_Qty"].ToString(),
                        RevenueYear = dr["Revenue_Year"].ToString(),
                        RevinueAmount = dr["Revenue_Amount"].ToString(),
                        ProduceUnit = dr["Product_Unit"].ToString(),
                       // IsActive = Convert.ToInt16(dr["IsActive"].ToString()),

                    });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View("Index", listAu);

        }


        //History: Code Update with ref. to bug ID: 231 Arvind
        /// <summary>
        /// Call when request come from Transit Permit  view Band Bind District and Region dropdown
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public ActionResult UpdateRevenue(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {

                _objAuRev.EnteredBy = Convert.ToInt64(Session["UserId"]);
                DataTable dtf = _objAuRev.Select_AuctionRevenue(Convert.ToInt64(id), "EDIT", _objAuRev.EnteredBy);

                DataTable dt = location.BindRangeByUserID(Convert.ToInt64(Session["UserId"]));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }

                ViewBag.RangeCode = new SelectList(items, "Value", "Text", dtf.Rows[0]["RANGE_CODE"].ToString());

              

                DataTable dtrng = location.BindDepotbyRangeCode(dtf.Rows[0]["RANGE_CODE"].ToString());

                ViewBag.fname1 = dtrng;
                foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["Depot_Name"].ToString(), Value = @dr["Depot_Id"].ToString() });
                }
                ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text", dtf.Rows[0]["Depot_Id"].ToString());


                ViewBag.RevenueYear = new SelectList(Common.GetLastThreeYear(), "Value", "Text", dtf.Rows[0]["Revenue_Year"].ToString());
                _objAuRev.Qty = dtf.Rows[0]["Revenue_Qty"].ToString();
                _objAuRev.RevinueAmount = dtf.Rows[0]["Revenue_Amount"].ToString();
               

             
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View("RevenueDataManage", _objAuRev);
        }

        /// <summary>
        /// Function to Save depot details to database 
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns>Saved depot ID</returns>
        [HttpPost]
        public ActionResult SubmitAuctionRevenueform(FormCollection fm, string Command)
        {
            Session["Depot_Status"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                AuctionRevenueMaster _objAuRevenue = new AuctionRevenueMaster();

                if (fm["CollectionID"].ToString() == "")
                {
                    _objAuRevenue.RowID = 0;
                }
                else
                {
                    _objAuRevenue.RowID = Convert.ToInt64(fm["CollectionID"].ToString());
                }

                if (Session["UserId"] != null)
                {
                    _objAuRevenue.EnteredBy = Convert.ToInt64(Session["UserID"]);
                }

                if (fm["RangeCode"].ToString() == "")
                {
                    _objAuRevenue.RangeCode = "";
                }
                else
                {
                    _objAuRevenue.RangeCode = fm["RangeCode"].ToString();
                }

                if (fm["DepotId"].ToString() == "")
                {
                    _objAuRevenue.DepotId = 0;
                }
                else
                {
                    _objAuRevenue.DepotId =Convert.ToInt64(fm["DepotId"].ToString());
                }

                if (fm["ForestProduceID"].ToString() == "")
                {
                    _objAuRevenue.ForestProduceID = 0;
                }
                else
                {
                    _objAuRevenue.ForestProduceID =Convert.ToInt64(fm["ForestProduceID"].ToString());
                }

                if (fm["ForestProductID"].ToString() == "")
                {
                    _objAuRevenue.ForestProductID = 0;
                }
                else
                {
                    _objAuRevenue.ForestProductID =Convert.ToInt64(fm["ForestProductID"].ToString());
                }

                if (fm["ProduceUnit"].ToString() == "")
                {
                    _objAuRevenue.ProduceUnit = "";
                }
                else
                {
                    _objAuRevenue.ProduceUnit = fm["ProduceUnit"].ToString();
                }

                if (fm["Qty"].ToString() == "")
                {
                    _objAuRevenue.Qty = "";
                }
                else
                {
                    _objAuRevenue.Qty = fm["Qty"].ToString();
                }

                if (fm["RevenueYear"].ToString() == "")
                {
                    _objAuRevenue.RevenueYear = "";
                }
                else
                {
                    _objAuRevenue.RevenueYear = fm["RevenueYear"].ToString();
                }

                if (fm["RevinueAmount"].ToString() == "")
                {
                    _objAuRevenue.RevinueAmount = "";
                }
                else
                {
                    _objAuRevenue.RevinueAmount = fm["RevinueAmount"].ToString();
                }


                string option = string.Empty;
                if (Command == "Submit")
                {
                    option = "INSERT";
                    _objAuRevenue.RowID = 0;
                    Int64 status = _objAuRevenue.InsertUpdateAucRevenueDetail(option);
                    if (status > 0)
                    {
                        TempData["Revenue_Status"] = "Revenue Created Sucessfully";
                    }
                    else
                    {
                        TempData["Revenue_Status"] = "Respective Record already exists, Please go for update the same!";
                    }


                }
                else if (Command == "Update")
                {
                    option = "Update";
                    Int64 status = _objAuRevenue.InsertUpdateAucRevenueDetail(option);
                    if (status > 0)
                    {
                        TempData["Revenue_Status"] = "Revenue Updated Sucessfully!";
                    }
                    else
                    {
                        TempData["Revenue_Status"] = "There is some Techincal Issue!";
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("RevenueDataManage", "RevenueDataManage");
        }



    }
}
