using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FMDSS.Controllers.ForestProduction
{
    public class ManageAucschedulerController : BaseController
    {
        //
        // GET: /ManageNotice/
        Location location = new Location();
        SMS_EMail_Services _objMail = new SMS_EMail_Services();
        List<SelectListItem> items = new List<SelectListItem>();
        List<SelectListItem> produce = new List<SelectListItem>();
        NoticeManagement obj_notice = new NoticeManagement();
        Int64 UserID = 0;
        int ModuleID = 3;
        /// <summary>
        /// Call when request come from ManageNotice view Bind Region  dropdown
        /// </summary>
        /// <returns></returns>
        
        public ActionResult ManageAucscheduler()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            List<NoticeManagement> result = new List<NoticeManagement>();

            try
            {
                if (Session["AuProductschedularInfo"] != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml")) == true)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml"));
                        Session["AuProductschedularInfo"] = null;
                    }
                }
                if (Session["AuDateschedularInfo"] != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml")) == true)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml"));
                        Session["AuDateschedularInfo"] = null;
                    }
                }
                if (Session["AuDepotschedularInfo"] != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml")) == true)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml"));
                        Session["AuDepotschedularInfo"] = null;
                    }
                }

                #region Product Type
                DataTable dtp = new DataTable();
                dtp = obj_notice.BindProductType();
                ViewBag.fname1 = dtp;
                foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                {
                    produce.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
                }

                ViewBag.ForestProduceID = produce;
                // ViewBag.ToLocation = items;
                #endregion


                #region Region
                DataTable dt = new DataTable();
                dt = location.BindRegion();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["REG_NAME"].ToString(), Value = @dr["REG_CODE"].ToString() });

                }

                ViewBag.RegionCode = items;



                // ViewBag.ToLocation = items;
                #endregion



            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View(result);
        }

        /// <summary>
        /// bind dropdown of circle by reagion Id
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BindRegionCircle()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string regionCode = "";
            string circleCode = "";
            try
            {
                DataTable circledt = location.BindRegionByUserID(Convert.ToInt64(Session["UserId"]));
                if (circledt != null)
                {
                    regionCode = circledt.Rows[0]["REG_CODE"].ToString();
                    circleCode = circledt.Rows[0]["CIRCLE_CODE"].ToString();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(regionCode + "," + circleCode, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<NoticeManagement> listAu = new List<NoticeManagement>();
            try
            {
                obj_notice.CreatedBy = Convert.ToInt64(Session["UserId"]);
                DataTable dtf = obj_notice.Select_AuctionScheduler();

                foreach (DataRow dr in dtf.Rows)
                {
                    listAu.Add(new NoticeManagement()
                    {
                        RowID = Convert.ToInt64(dr["RowID"].ToString()),
                        NoticeId = Convert.ToInt64(dr["AucScdID"].ToString()),
                        SchedulerNo = dr["Auction_SchedularNo"].ToString(),
                        SchedulerPeriod = dr["SchedulerPeriod"].ToString(),
                        EmdAmount = dr["EmdAmount"].ToString(),
                        Description = dr["Description"].ToString(),
                        IsActive = Convert.ToInt16(dr["IsActive"].ToString()),

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
        /// Viewscheduler
        /// </summary>
        /// <param name="transitID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Viewscheduler(string transitID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            string notice = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                if (!String.IsNullOrEmpty(transitID))
                {
                    NoticeManagement noticeManagement = new NoticeManagement();


                    DataTable dnt = noticeManagement.PublishNoticeview(Convert.ToInt64(transitID), "AUCSHE");
                    if (dnt != null && dnt.Rows.Count > 0)
                    {
                        notice = dnt.Rows[0][0].ToString();
                    }


                    DataSet dsDepotSchedulers = noticeManagement.BindScheduler(Convert.ToInt64(transitID));
                    DataView dv = null; DataTable dtMonths = null; DataTable dtSchedulerData = null;

                    foreach (DataTable dtDepotSchedular in dsDepotSchedulers.Tables)
                    {
                        dv = new DataView(dtDepotSchedular);
                        dtMonths = dv.ToTable(true, new string[] { "Month" });
                        dtSchedulerData = new DataTable();
                        dtSchedulerData.Columns.Add("depot", typeof(string));
                        dtSchedulerData.Columns.Add("product", typeof(string));

                        Dictionary<int, string> strMonths = new Dictionary<int, string>();
                        string item = string.Empty;

                        foreach (DataRow dr in dtMonths.Rows)
                        {
                            item = string.Empty;
                            dtSchedulerData.Columns.Add(dr["Month"].ToString(), typeof(string));

                            DataRow[] dr1 = dtDepotSchedular.Select("Month ='" + dr["Month"].ToString() + "'");
                            foreach (DataRow drs in dr1)
                            { item = item + "," + drs["Day"].ToString(); }

                            item = item + " " + dr["Month"].ToString();
                            strMonths.Add(strMonths.Count, item.TrimStart(','));
                        }
                        dtSchedulerData.Columns.Add("MobileNo", typeof(string));

                        DataRow drNewRow = dtSchedulerData.NewRow();
                        drNewRow["depot"] = dtDepotSchedular.Rows[0][0].ToString();
                        drNewRow["product"] = dtDepotSchedular.Rows[0][1].ToString();
                        drNewRow["MobileNo"] = dtDepotSchedular.Rows[0]["MobileNo"].ToString();

                        for (int i = 0; i < strMonths.Count; i++)
                        {
                            drNewRow[i + 2] = strMonths.Values.ToList()[i].ToString();
                        }
                        dtSchedulerData.Rows.Add(drNewRow);


                        sb.Append("<tr>");
                        foreach (DataColumn dc in dtSchedulerData.Columns)
                        {
                            sb.Append("<td col-lg-3>" + dtSchedulerData.Rows[0][dc] + "</td>");
                        }
                        sb.Append("</tr>");
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new { list1 = notice, list2 = sb.ToString(), list3 = items }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete Notice Number.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteNoticeData(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            NoticeManagement obj = new NoticeManagement();
            Int64 UpdatedBy = 0;

            try
            {
                if (Session["UserID"] != null)
                {

                    UpdatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (!String.IsNullOrEmpty(id))
                {

                    string status = obj.DeactivateNoticeNo(Convert.ToInt64(id), UpdatedBy, "Auction");

                    if (!String.IsNullOrEmpty(status.ToString()))
                    {
                        TempData["AUSCH_Status"] = "Auction Scheduler No:#" + status + " Deleted Successfully";
                    }
                    else
                    {
                        TempData["AUSCH_Status"] = "Not Deleted";
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return RedirectToAction("Index", "ManageAucscheduler");
        }


        /// <summary>
        /// Bind Product Dropdown 
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="producetype"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getForesProduct(string producetype)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(producetype))
                {
                    NoticeManagement noticeManagement = new NoticeManagement();
                    DataTable dt = noticeManagement.BindProduct(Convert.ToInt64(producetype));
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["ProductName"].ToString(), Value = @dr["PRODUCE_ID"].ToString() });
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new SelectList(items, "Value", "Text"));
        }

        [HttpPost]
        public JsonResult getForesProduceqty(string product)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            NoticeManagement noticeManagement = null;
            NoticeManagement notice = new NoticeManagement();
            try
            {
                if (!String.IsNullOrEmpty(product))
                {
                    noticeManagement = new NoticeManagement();
                    DataTable dt = notice.BindProductUnitRate(Convert.ToInt64(product));
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {

                            noticeManagement.ProduceUnit = dr["UnitType"].ToString();
                            noticeManagement.ProductRate = Convert.ToDecimal(dr["RatePerUnit"].ToString());
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(noticeManagement, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindproductDetail(NoticeManagement notice)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (notice != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = "Auction_Product_Schedular" + DateTime.Now.Ticks.ToString();

                    if (Session["AuProductschedularInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("AuProductschedularInfo");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["AuProductschedularInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml"));
                    }

                    XmlElement schedularInfo = xmldoc.CreateElement("SchedularDetails");
                    XmlElement ID = xmldoc.CreateElement("ID");
                    XmlElement forestProduceID = xmldoc.CreateElement("ForestProduceID");
                    XmlElement forestProductID = xmldoc.CreateElement("ForestProductID");
                    XmlElement qty = xmldoc.CreateElement("Qty");
                    XmlElement unit = xmldoc.CreateElement("Unit");




                    ID.InnerText = notice.ID.ToString();
                    forestProduceID.InnerText = Convert.ToString(notice.ForestProduceID);
                    forestProductID.InnerText = Convert.ToString(notice.ForestProductID);
                    qty.InnerText = notice.Qty;
                    unit.InnerText = notice.ProduceUnit;


                    schedularInfo.AppendChild(ID);
                    schedularInfo.AppendChild(forestProduceID);
                    schedularInfo.AppendChild(forestProductID);
                    schedularInfo.AppendChild(qty);
                    schedularInfo.AppendChild(unit);



                    xmldoc.DocumentElement.AppendChild(schedularInfo);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(notice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProductFromList(string ProductID)
        {
            NoticeManagement noticeManagement = new NoticeManagement();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<NoticeManagement>();
            if (ProductID != null)
            {
                try
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml")) == true)
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml"));
                        ds.Tables[0].Rows.RemoveAt(Convert.ToInt16(ProductID) - 1);
                        ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml"));

                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                NoticeManagement pp = new NoticeManagement();
                                string productName = "";

                                pp.ID = Convert.ToInt16(dr["ID"].ToString());
                                pp.ForestProduceID = Convert.ToInt64(dr["ForestProduceID"].ToString());

                                DataTable dt = noticeManagement.BindProductdeletee(Convert.ToInt64(dr["ForestProduceID"].ToString()));
                                foreach (DataRow drs in dt.Rows)
                                {
                                    productName = drs["ProductName"].ToString();
                                }

                                pp.prodName = productName;
                                pp.ForestProductID = Convert.ToInt64(dr["ForestProductID"].ToString());
                                pp.ProduceUnit = dr["Unit"].ToString();
                                pp.Qty = dr["Qty"].ToString();

                                result.Add(pp);


                            }
                        }

                    }



                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult DeleteDateFromList(string DateID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<NoticeManagement>();
            if (DateID != null)
            {
                try
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml")) == true)
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml"));
                        ds.Tables[0].Rows.RemoveAt(Convert.ToInt16(DateID) - 1);
                        ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml"));

                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                NoticeManagement pp = new NoticeManagement();


                                pp.ID = Convert.ToInt16(dr["ID"].ToString());
                                pp.ScheduleDay = dr["Day"].ToString();
                                pp.ScheduleMonth = dr["Month"].ToString();
                                pp.Scheduleyear = dr["Year"].ToString();


                                result.Add(pp);


                            }
                        }

                    }



                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDepotFromList(string DepotID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<NoticeManagement>();
            if (DepotID != null)
            {
                try
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml")) == true)
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml"));
                        ds.Tables[0].Rows.RemoveAt(Convert.ToInt16(DepotID) - 1);
                        ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml"));

                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                NoticeManagement pp = new NoticeManagement();



                                pp.ID = Convert.ToInt16(dr["ID"].ToString());
                                pp.RegionCode = dr["RegionCode"].ToString();
                                pp.CircleCode = dr["CircleCode"].ToString();
                                pp.RegionCode = dr["RegionCode"].ToString();
                                pp.DivisionCode = dr["DivisionCode"].ToString();
                                pp.RangeCode = dr["RangeCode"].ToString();
                                pp.DepotId = Convert.ToInt64(dr["DepotId"].ToString());
                                pp.ScheduleTime = dr["Time"].ToString();

                                pp.Mobile = dr["Mobile"].ToString();

                                result.Add(pp);


                            }
                        }

                    }



                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddMultipleDate(string dt, string dID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (dt != null)
            {
                try
                {
                    string[] dateformt = dt.Split(',');
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = "Auction_Date_Scheduler" + DateTime.Now.Ticks.ToString();

                    if (Session["AuDateschedularInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("AuDatechedularInfo");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["AuDateschedularInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml"));
                    }

                    XmlElement dtschedularInfo = xmldoc.CreateElement("DateSchedularDetails");
                    XmlElement ID = xmldoc.CreateElement("ID");
                    XmlElement day = xmldoc.CreateElement("Day");
                    XmlElement month = xmldoc.CreateElement("Month");
                    XmlElement years = xmldoc.CreateElement("Year");



                    ID.InnerText = dID;
                    day.InnerText = dateformt[0].ToString();
                    month.InnerText = dateformt[1].ToString();
                    years.InnerText = dateformt[2].ToString();



                    dtschedularInfo.AppendChild(ID);
                    dtschedularInfo.AppendChild(day);
                    dtschedularInfo.AppendChild(month);
                    dtschedularInfo.AppendChild(years);


                    xmldoc.DocumentElement.AppendChild(dtschedularInfo);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(dt + "," + dID, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindDepotDetail(NoticeManagement notice)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (notice != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = "Auction_DepotNotice_Schedular" + DateTime.Now.Ticks.ToString();

                    if (Session["AuDepotschedularInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("AuDepotschedularInfo");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["AuDepotschedularInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml"));
                    }

                    XmlElement depotschedularInfo = xmldoc.CreateElement("DepotSchedularDetails");
                    XmlElement ID = xmldoc.CreateElement("ID");
                    XmlElement regionCode = xmldoc.CreateElement("RegionCode");
                    XmlElement circleCode = xmldoc.CreateElement("CircleCode");
                    XmlElement divisionCode = xmldoc.CreateElement("DivisionCode");
                    XmlElement rangeCode = xmldoc.CreateElement("RangeCode");
                    XmlElement depotId = xmldoc.CreateElement("DepotId");
                    XmlElement time = xmldoc.CreateElement("Time");
                    XmlElement mobile = xmldoc.CreateElement("Mobile");
                    XmlElement finalDate = xmldoc.CreateElement("FinalDate");


                    ID.InnerText = notice.ID.ToString();
                    regionCode.InnerText = notice.RegionCode;
                    circleCode.InnerText = notice.CircleCode;
                    divisionCode.InnerText = notice.DivisionCode;
                    rangeCode.InnerText = notice.RangeCode;
                    depotId.InnerText = Convert.ToString(notice.DepotId);
                    time.InnerText = notice.Time;
                    mobile.InnerText = notice.Mobile;
                    finalDate.InnerText = notice.FinalDate;

                    depotschedularInfo.AppendChild(ID);
                    depotschedularInfo.AppendChild(regionCode);
                    depotschedularInfo.AppendChild(circleCode);
                    depotschedularInfo.AppendChild(divisionCode);
                    depotschedularInfo.AppendChild(rangeCode);
                    depotschedularInfo.AppendChild(depotId);
                    depotschedularInfo.AppendChild(time);
                    depotschedularInfo.AppendChild(mobile);
                    depotschedularInfo.AppendChild(finalDate);

                    xmldoc.DocumentElement.AppendChild(depotschedularInfo);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml"));

                    //DataSet ds = new DataSet();
                    //if (Session["AuProductschedularInfo"] != null)
                    //{
                    //    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml"));

                    //    foreach (DataRow dr in ds.Tables[0].Rows)
                    //    {
                    //        Int64 
                    //    }
                    //}

                    if (Session["AuDateschedularInfo"] != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml")) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["AuDateschedularInfo"].ToString() + ".xml"));
                            Session["AuDateschedularInfo"] = null;
                        }
                    }

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(notice, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DepotInchargeContactByID(string depotId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            NoticeManagement notice = new NoticeManagement();
            string contactno = "";
            try
            {
                if (!String.IsNullOrEmpty(depotId))
                {
                    DataTable dt = notice.GetContactBYDepotID(Convert.ToInt64(depotId));
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            contactno = dr["Mobile"].ToString();

                        }
                    }


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(contactno, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult SubmitAuctionschedularForm(FormCollection fm, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ActionResult actionResult = null;
            TempData["AUSCH_Status"] = null;
            NoticeManagement notice = new NoticeManagement();
            string status = "";
            try
            {
                DateTime now = DateTime.Now;
                string id = now.Ticks.ToString();
                notice.RequestId = id;

                if (Session["UserID"] != null)
                {

                    notice.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }
                if (fm["SchedulerPeriod"].ToString() == "")
                {
                    notice.SchedulerPeriod = "";
                }
                else
                {
                    notice.SchedulerPeriod = fm["SchedulerPeriod"].ToString();
                }

                if (fm["EmdAmount"].ToString() == "")
                {
                    notice.EmdAmount = "";
                }
                else
                {
                    notice.EmdAmount = fm["EmdAmount"].ToString();
                }
                if (fm["Description"].ToString() == "")
                {
                    notice.Description = "";
                }
                else
                {
                    notice.Description = fm["Description"].ToString();
                }

                if (Command == "Submit")
                {
                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();
                    if (Session["AuProductschedularInfo"] != null && Session["AuDepotschedularInfo"] != null)
                    {
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml"));
                        ds2.ReadXml(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml"));
                        status = notice.AddAuctionSchedularDetails(ds.Tables[0], ds2.Tables[0]);

                        if (status != "")
                        {
                            TempData["AUSCH_Status"] = "Scheduler:#" + status + " Created Successfully";

                            if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml")) == true)
                            {
                                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["AuProductschedularInfo"].ToString() + ".xml"));
                                Session["AuProductschedularInfo"] = null;
                            }

                            if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml")) == true)
                            {
                                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["AuDepotschedularInfo"].ToString() + ".xml"));
                                Session["AuDepotschedularInfo"] = null;
                            }
                            actionResult = RedirectToAction("ManageAucscheduler", "ManageAucscheduler");
                        }
                        else
                        {
                            TempData["AUSCH_Status"] = "No inserted";
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return actionResult;
        }

    }
}
