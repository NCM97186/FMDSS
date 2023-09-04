using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Master
{
    public class MeatingCollectionController : BaseController
    {
        //
        // GET: /MeatingCollection/
        #region global variables

        Location location = new Location();
        AuctionRevenueMaster _objAuRev = new AuctionRevenueMaster();
        List<SelectListItem> items = new List<SelectListItem>();
        List<SelectListItem> rangeCode = new List<SelectListItem>();
        int ModuleID = 3;
        Int64 UserID = 0;
        #endregion
        public ActionResult MeatingCollection()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<AuctionRevenueMaster> listAu = new List<AuctionRevenueMaster>();
            DataTable dt = new DataTable();
            try
            {
                dt = new Common().Select_Range(UserID);
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }

                ViewBag.RangeCode = items;


                _objAuRev.EnteredBy = Convert.ToInt64(Session["UserId"]);
                DataTable dtf = _objAuRev.Select_MeationgCollectionRate(0,"VIEW");

                foreach (DataRow dr in dtf.Rows)
                {
                    listAu.Add(new AuctionRevenueMaster()
                    {
                        RowID = Convert.ToInt64(dr["RowID"].ToString()),
                        RangeCode = dr["RANGE_NAME"].ToString(),
                        DepotName = dr["Depot_Name"].ToString(),
                        ForestProducename = dr["ProduceType"].ToString(),
                        ForestProductName = dr["ProductName"].ToString(),
                        CollectionRate = dr["Collection_Rate"].ToString(),
                        MeatingDates = dr["Meating_Date"].ToString(),
                        DocMOMfilepath = dr["Meating_Minute_Filepath"].ToString(),
                        ProduceUnit = dr["Product_Unit"].ToString(),
                        // IsActive = Convert.ToInt16(dr["IsActive"].ToString()),

                    });
                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(listAu);
        }

        /// <summary>
        /// Delete Meating Data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeletMeatingData(string id)
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

                    string status = _objAuRev.DeactivateNoticeNo(Convert.ToInt64(id), UpdatedBy, "Meating");

                    if (!String.IsNullOrEmpty(status.ToString()))
                    {
                        TempData["Revenue_Status"] = "MOM for collection rate has been deleted!";
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

            return RedirectToAction("MeatingCollection", "MeatingCollection");
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
                    DataTable dtf = _objAuRev.Select_MeationgCollectionRate(Convert.ToInt64(collectionID), "EDIT");

                    foreach (DataRow dr in dtf.Rows)
                    {
                          obj_no=new AuctionRevenueMaster();

                          obj_no.RowID =Convert.ToInt64(dr["MeatingCollectionID"].ToString());
                          obj_no.RangeName = dr["RANGE_NAME"].ToString();
                          obj_no.RangeCode = dr["RANGE_CODE"].ToString();
                          obj_no.DepotId = Convert.ToInt64(dr["Depot_Id"].ToString());
                          obj_no.DepotName = dr["Depot_Name"].ToString();
                          obj_no.ForestProduceID = Convert.ToInt64(dr["ID"].ToString());
                          obj_no.ForestProducename = dr["ProduceType"].ToString();
                          obj_no.ForestProductID = Convert.ToInt64(dr["productID"].ToString());
                          obj_no.ForestProductName = dr["ProductName"].ToString();
                          obj_no.CollectionRate = dr["Collection_Rate"].ToString();
                          //obj_no.MeatingDates = dr["Meating_Date"].ToString();
                          DateTime _date2 = DateTime.Parse(dr["Meating_Date"].ToString());
                          obj_no.MeatingDates = _date2.ToString("dd/MM/yyyy");
                          obj_no.DocMOMfilepath = dr["Meating_Minute_Filepath"].ToString();
                          obj_no.ProduceUnit = dr["Product_Unit"].ToString();
                            // IsActive = Convert.ToInt16(dr["IsActive"].ToString()),
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
                DataTable dtf = _objAuRev.Select_MeationgCollectionRate(0,"VIEW");

                foreach (DataRow dr in dtf.Rows)
                {
                    listAu.Add(new AuctionRevenueMaster()
                    {
                        RowID = Convert.ToInt64(dr["RowID"].ToString()),
                        RangeCode = dr["RANGE_NAME"].ToString(),
                        DepotName = dr["Depot_Name"].ToString(),
                        ForestProducename = dr["ProduceType"].ToString(),
                        ForestProductName = dr["ProductName"].ToString(),
                        CollectionRate = dr["Collection_Rate"].ToString(),
                        MeatingDates = dr["Meating_Date"].ToString(),
                        DocMOMfilepath = dr["Meating_Minute_Filepath"].ToString(),
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



        /// <summary>
        /// Function to Save depot details to database 
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns>Saved depot ID</returns>
        [HttpPost]
        public ActionResult SubmitMeatingCollection(FormCollection fm, HttpPostedFileBase DocMOM, string Command)
        {
            Session["Revenue_Status"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            string DocMOMfile = string.Empty;
            var DocMOMfilepath = "";
          
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
                    _objAuRevenue.DepotId = Convert.ToInt64(fm["DepotId"].ToString());
                }

                if (fm["ForestProduceID"].ToString() == "")
                {
                    _objAuRevenue.ForestProduceID = 0;
                }
                else
                {
                    _objAuRevenue.ForestProduceID = Convert.ToInt64(fm["ForestProduceID"].ToString());
                }

                if (fm["ForestProductID"].ToString() == "")
                {
                    _objAuRevenue.ForestProductID = 0;
                }
                else
                {
                    _objAuRevenue.ForestProductID = Convert.ToInt64(fm["ForestProductID"].ToString());
                }

                if (fm["ProduceUnit"].ToString() == "")
                {
                    _objAuRevenue.ProduceUnit = "";
                }
                else
                {
                    _objAuRevenue.ProduceUnit = fm["ProduceUnit"].ToString();
                }

                if (fm["CollectionRate"].ToString() == "")
                {
                    _objAuRevenue.CollectionRate = "";
                }
                else
                {
                    _objAuRevenue.CollectionRate = fm["CollectionRate"].ToString();
                }

                if (fm["MeatingDate"].ToString() == "")
                {
                    _objAuRevenue.MeatingDate = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    _objAuRevenue.MeatingDate = DateTime.ParseExact(fm["MeatingDate"].ToString(), "dd/MM/yyyy", null);

                }

                if (DocMOM != null && DocMOM.ContentLength > 0)
                {
                    DocMOMfile = Path.GetFileName(DocMOM.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + DocMOMfile;
                    DocMOMfilepath = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    _objAuRevenue.DocMOM = FileFullName;

                    _objAuRevenue.DocMOMfilepath = @"~/PermissionDocument/" + FileFullName.Trim();
                    DocMOM.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    _objAuRevenue.DocMOM = "";
                    _objAuRevenue.DocMOMfilepath = "";
                }
             

                string option = string.Empty;
                if (Command == "Submit")
                {
                    option = "INSERT";
                    _objAuRevenue.RowID = 0;
                    Int64 status = _objAuRevenue.InsertUpdateCollectionRate(option);
                    if (status > 0)
                    {
                        TempData["Revenue_Status"] = "MOM for collection rate has been inserted!";
                    }
                    else
                    {
                        TempData["Revenue_Status"] = "MOM for collection rate already exists!";
                    }


                }
                else if (Command == "Update")
                {
                    option = "UPDATE";
                    

                    Int64 status = _objAuRevenue.InsertUpdateCollectionRate(option);
                    if (status > 0)
                    {
                        TempData["Revenue_Status"] = "MOM for collection rate has been updated!";
                    }
                    else
                    {
                        TempData["Revenue_Status"] = "Error Occured";
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("MeatingCollection", "MeatingCollection");
        }


    }
}
