using FMDSS.Entity.Protection.ViewModel;
using FMDSS.Entity.ViewModel;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class WaterResourceController : Controller
    {

        private Int64 UserID
        {
            get
            {
                if (Session["UserID"] != null)
                    return Convert.ToInt64(Session["UserID"].ToString());
                else
                    return 0;
            }
        }

        public ActionResult waterresource()
        {
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult waterresource(WaterFireModel model)
        {
            SetDropdownData();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            dt = objWF.SaveWaterResouceData(model);
            if (dt.Rows.Count > 0)
            {
                TempData["ReturnMsg"] = dt.Rows[0]["msg"];
                TempData["IsError"] = dt.Rows[0]["isError"];
            }
            return RedirectToAction("waterresource");
        }
        private void SetDropdownData()
        {
            List<SelectListItem> lstRange = new List<SelectListItem>();
            List<SelectListItem> lstNaka = new List<SelectListItem>();
            ViewBag.Range = lstRange;
            ViewBag.Naka = lstNaka;
            WaterFireModel objWF = new WaterFireModel();
            DataTable dt = objWF.GetDivisionOnUser(Convert.ToString(Session["SSOID"].ToString()));
            ViewBag.Division = GetDropdownData(1, dt);
        }
        [HttpPost]
        public ActionResult UploadFiles(int docTypeID)
        {
            var isError = false;
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = "Documents/WaterResorceFireLine/";
            List<CommonDocument> lstDoc = new List<CommonDocument>();
            if (Request.Files.Count > 0)
            {
                try
                {
                    var docType = "";
                    switch (docTypeID)
                    {
                        case 1: docType = "UploadKMLFile"; break;
                        case 2: docType = "UploadSourceKMLFile"; break;
                        case 3: docType = "UpKMLFile_ForestFireLine"; break;
                        case 4: docType = "UpSourceKMLFile_ForestFireLine"; break;

                    }
                    HttpFileCollectionBase files = Request.Files;
                    string strDoc = string.Empty;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            FileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            FileName = file.FileName;
                        }
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        file.SaveAs(Server.MapPath("~/" + FilePath + FileFullName));
                        if (docTypeID == 1)
                        {
                            Session["WaterPointImagePath"] = path;
                        }
                        else if (docTypeID == 2)
                        {
                            Session["WaterSourceImagePath"] = path;
                        }
                        else if (docTypeID == 3)
                        {
                            Session["FFLinePointImagePath"] = path;
                        }
                        else if (docTypeID == 4)
                        {
                            Session["FFLineSourceImagePath"] = path;
                        }
                        if (Session["UploadFile"] == null)
                        {
                            lstDoc.Add(new CommonDocument
                            {
                                TempID = Guid.NewGuid().ToString(),
                                IsNew = true,
                                DocumentName = FileName,
                                DocumentPath = path,
                                DocumentTypeID = docTypeID
                            });
                            Session["UploadFile"] = lstDoc;
                        }
                        else
                        {
                            lstDoc = (List<CommonDocument>)Session["UploadFile"];
                            lstDoc.Add(new CommonDocument
                            {
                                TempID = Guid.NewGuid().ToString(),
                                IsNew = true,
                                DocumentName = FileName,
                                DocumentPath = path,
                                DocumentTypeID = docTypeID
                            });
                            Session["UploadFile"] = lstDoc;
                        }
                    }
                }
                catch (Exception)
                {
                    isError = true;
                }
                return Json(new { isError = isError, data = lstDoc, returnMsg = "File Uploaded Successfully!" });
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public JsonResult GetRange(string parentID)
        {
            try
            {
                WaterFireModel objWF = new WaterFireModel();
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = objWF.GetRange(parentID);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"));
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetNaka(string parentID)
        {
            try
            {
                WaterFireModel objWF = new WaterFireModel();
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = objWF.GetNaka(parentID);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["NakaName"].ToString(), Value = @dr["NakaID"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"));
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        private EnumerableRowCollection<SelectListItem> GetDropdownData(int actionCode, DataTable dtDropdownData)
        {
            EnumerableRowCollection<SelectListItem> data = null;
            switch (actionCode)
            {
                case 1:
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("DIV_CODE"),
                        Text = x.Field<string>("DIV_NAME")
                    });
                    return data;


            }
            return null;
        }

        public ActionResult ForestFireLine()
        {
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult ForestFireLine(WaterFireModel model)
        {
            SetDropdownData();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            dt = objWF.SaveForestFireLineData(model);
            if (dt.Rows.Count > 0)
            {
                TempData["ReturnMsg"] = dt.Rows[0]["msg"];
                TempData["IsError"] = dt.Rows[0]["isError"];
            }
            return RedirectToAction("ForestFireLine");
        }

        public ActionResult WaterResFFLineDetails()
        {
            DataSet ds = new DataSet();
            WaterFireModel objWF = new WaterFireModel();
            List<WaterFireModel> WRList = new List<WaterFireModel>();
            List<WaterFireModel> WaterRSummaryList = new List<WaterFireModel>();
            List<WaterFireModel> FFLineList = new List<WaterFireModel>();
            ds = objWF.WaterResFFLineDetails();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    WaterFireModel w = new WaterFireModel();
                  
                    w.SNO = Convert.ToInt32(ds.Tables[0].Rows[i]["SNO"]);
                    w.DivisionName = Convert.ToString(ds.Tables[0].Rows[i]["DIV_NAME"]);
                    w.RangeName = Convert.ToString(ds.Tables[0].Rows[i]["RANGE_NAME"]);
                    w.NakaName = Convert.ToString(ds.Tables[0].Rows[i]["NakaName"]);
                    w.BlockName = Convert.ToString(ds.Tables[0].Rows[i]["BlockName"]);
                    w.WaterPoint_LatLong = Convert.ToString(ds.Tables[0].Rows[i]["WaterPoint_LatLong"]);
                    w.WaterSource_LatLong = Convert.ToString(ds.Tables[0].Rows[i]["WaterSource_LatLong"]);
                    w.Distance = Convert.ToString(ds.Tables[0].Rows[i]["Distance"]);
                    w.EnteredOn = Convert.ToString(ds.Tables[0].Rows[i]["EnteredOn"]);
                    w.Enteredby = Convert.ToString(ds.Tables[0].Rows[i]["EnteredBy"]);
                    w.WaterPointImagePath = Convert.ToString(ds.Tables[0].Rows[i]["WaterPointImagePath"]);
                    w.WaterSourceImagePath = Convert.ToString(ds.Tables[0].Rows[i]["WaterSourceImagePath"]);

                    w.SourceName = Convert.ToString(ds.Tables[0].Rows[i]["SourceName"]);
                    w.DestinationName = Convert.ToString(ds.Tables[0].Rows[i]["DestinationName"]);

                    w.SourceLatLong = Convert.ToString(ds.Tables[0].Rows[i]["SourceLatLong"]);
                    w.DestinationLatLong = Convert.ToString(ds.Tables[0].Rows[i]["DestinationLatLong"]);
                    
                    WRList.Add(w);
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                {
                    WaterFireModel f = new WaterFireModel();
                    f.SNO = Convert.ToInt32(ds.Tables[1].Rows[j]["SNO"]);
                    f.DivisionName = Convert.ToString(ds.Tables[1].Rows[j]["DIV_NAME"]);
                    f.RangeName = Convert.ToString(ds.Tables[1].Rows[j]["RANGE_NAME"]);
                    f.NakaName = Convert.ToString(ds.Tables[1].Rows[j]["NakaName"]);
                    f.BlockName = Convert.ToString(ds.Tables[1].Rows[j]["BlockName"]);
                    f.StartLatitude = Convert.ToString(ds.Tables[1].Rows[j]["StartLatitude"]);
                    f.StartLongitude = Convert.ToString(ds.Tables[1].Rows[j]["StartLongitude"]);
                    f.EndLatitude = Convert.ToString(ds.Tables[1].Rows[j]["EndLatitude"]);
                    f.EndLongitude = Convert.ToString(ds.Tables[1].Rows[j]["EndLongitude"]);
                    f.Source_StartLatitude = Convert.ToString(ds.Tables[1].Rows[j]["Source_StartLatitude"]);
                    f.Source_StartLongitude = Convert.ToString(ds.Tables[1].Rows[j]["Source_StartLatitude"]);
                    f.EnteredOn = Convert.ToString(ds.Tables[1].Rows[j]["EnteredOn"]);
                    f.Enteredby = Convert.ToString(ds.Tables[1].Rows[j]["EnteredBy"]);
                    f.WaterPointImagePath = Convert.ToString(ds.Tables[1].Rows[j]["WaterPointImagePath"]);
                    f.WaterSourceImagePath = Convert.ToString(ds.Tables[1].Rows[j]["WaterSourceImagePath"]);
                    FFLineList.Add(f);
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                {
                    WaterFireModel WS = new WaterFireModel();
                    WS.SNO = Convert.ToInt32(ds.Tables[2].Rows[j]["SNO"]);
                    WS.DivisionName = Convert.ToString(ds.Tables[2].Rows[j]["DIV_NAME"]);
                    WS.RangeName = Convert.ToString(ds.Tables[2].Rows[j]["RANGE_NAME"]);
                    WS.WaterPoint_LatLong = Convert.ToString(ds.Tables[2].Rows[j]["WaterPoint_LatLong"]);
                    WS.SourceLatLong = Convert.ToString(ds.Tables[2].Rows[j]["SourceLatLong"]);
                    WS.DestinationLatLong = Convert.ToString(ds.Tables[2].Rows[j]["DestinationLatLong"]);
                    WaterRSummaryList.Add(WS);
                }
            }
            ViewBag.WRSummaryList = WaterRSummaryList;
            ViewBag.FFLineList = FFLineList;
            ViewBag.WRList = WRList;
            return View("WaterResFFLine");
        }


        public ActionResult WRDetials(string WaterPoint_LatLong, string fromDate, string toDate)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }
            DataSet ds = new DataSet();
            WaterFireModel objWF = new WaterFireModel();
            List<WaterFireModel> WRList = new List<WaterFireModel>();
            ds = objWF.WaterResFFLineDetails(WaterPoint_LatLong, fromDate, toDate);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    WaterFireModel w = new WaterFireModel();
                    w.SNO = Convert.ToInt32(ds.Tables[0].Rows[i]["SNO"]);
                    w.DivisionName = Convert.ToString(ds.Tables[0].Rows[i]["DIV_NAME"]);
                    w.RangeName = Convert.ToString(ds.Tables[0].Rows[i]["RANGE_NAME"]);
                    w.NakaName = Convert.ToString(ds.Tables[0].Rows[i]["NakaName"]);
                    w.BlockName = Convert.ToString(ds.Tables[0].Rows[i]["BlockName"]);
                    w.WaterPoint_LatLong = Convert.ToString(ds.Tables[0].Rows[i]["WaterPoint_LatLong"]);
                    w.WaterSource_LatLong = Convert.ToString(ds.Tables[0].Rows[i]["WaterSource_LatLong"]);
                    w.Distance = Convert.ToString(ds.Tables[0].Rows[i]["Distance"]);
                    w.EnteredOn = Convert.ToString(ds.Tables[0].Rows[i]["EnteredOn"]);
                    w.Enteredby = Convert.ToString(ds.Tables[0].Rows[i]["EnteredBy"]);
                    w.WaterPointImagePath = Convert.ToString(ds.Tables[0].Rows[i]["WaterPointImagePath"]);
                    w.WaterSourceImagePath = Convert.ToString(ds.Tables[0].Rows[i]["WaterSourceImagePath"]);
                    WRList.Add(w);
                }
            }
            ViewBag.WRList = WRList;
            return PartialView("_WRDetials");
        }

        public ActionResult _GetWaterRefillList(string sourceLatLong, string destinationLatLong)
        {
            
            WaterFireModel wf = new WaterFireModel();
            List<WaterSourceDetail> lst = new List<WaterSourceDetail>();
            DataTable dt = wf.GetWaterRefillList("GetWaterRefillList", sourceLatLong, destinationLatLong);
            lst = Globals.Util.GetListFromTable<WaterSourceDetail>(dt);
            return PartialView(lst);
        }



    }
}
