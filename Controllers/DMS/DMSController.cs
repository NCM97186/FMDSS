using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.Common;
using System.Web.UI.WebControls;
using FMDSS.Models;
using System.Data;
using System.Collections;
using System.Configuration;
using System.IO;



namespace FMDSS.Controllers.DMS
{
    public class DMSController : BaseController
    {
        // GET api/dms
        int ModuleID = 1;
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        DMSmodel Model = new DMSmodel();


        [NonAction]
        public void ModuleData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                List<SelectListItem> lstModule = new List<SelectListItem>();

                DataTable dt = Model.GetModuleForDMS();
                lstModule.Add(new SelectListItem { Text = "--Select--", Value = "0" });

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lstModule.Add(new SelectListItem { Text = dr["TEXT"].ToString(), Value = dr["VELUE"].ToString() });
                }
                ViewBag.Module = lstModule;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        public JsonResult ServiceTypeData(string ModuleCode)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(ModuleCode)))
                {
                    items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    DataTable dt = Model.GetServiceTypeForDMS(Convert.ToString(ModuleCode));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = dr["TEXT"].ToString(), Value = dr["VELUE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public JsonResult PermissionData(string ModuleCode, string ServiceTypeCode)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(ModuleCode) && !String.IsNullOrEmpty(ServiceTypeCode))
                {
                    items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    DataTable dt = Model.GetPermissionForDMS(ModuleCode, ServiceTypeCode);
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = dr["TEXT"].ToString(), Value = dr["VELUE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public JsonResult SubPermissionData(string ModuleCode, string ServiceTypeCode, string PermissionCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(ModuleCode) && !String.IsNullOrEmpty(ServiceTypeCode) && !String.IsNullOrEmpty(PermissionCode))
                {
                    items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    DataTable dt = Model.GetSubPermissionForDMS(ModuleCode, ServiceTypeCode, PermissionCode);
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = dr["TEXT"].ToString(), Value = dr["VELUE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public string FetchRepoData(string ModuleCode, string ServiceTypeCode, string PermissionCode, string SubPermissionCode, string FromDate, string ToDate)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<ListDMSFetchData> items = new List<ListDMSFetchData>();

            try
            {
                DataTable dt = Model.GetFetchRepoData(ModuleCode, ServiceTypeCode, PermissionCode, SubPermissionCode, FromDate, ToDate);
                FetchDMSResponce Responce;

                foreach (DataRow DR in dt.Rows)
                {



                    items.Add(new ListDMSFetchData
                        {
                            RequestId = Convert.ToString(DR["RequestedID"]),
                            FileExtension = Convert.ToString(Path.GetExtension(Convert.ToString(DR["FileName"]))).Replace(".",""),
                            FileType = Convert.ToString(DR["FileType"]),
                            DownloadDocument =  Convert.ToString(DR["RepoDocID"]) + Path.GetExtension(Convert.ToString(DR["FileName"])),
                        });
                   
                }

                var requestId = from ii in items
                          group ii.RequestId by ii.RequestId into g
                          select new { requestId = g.Key };

                List<oListDMSFetchData> oItem = new List<oListDMSFetchData>();
                foreach(var o in requestId)
                {
                    oListDMSFetchData dms = new oListDMSFetchData();
                    var itemsList = items.Where(t => t.RequestId == o.requestId);
                    dms.RequestId = o.requestId;
                    dms.DMSFields = new List<DMSdata>();
                    foreach(var oI in itemsList)
                    {
                        DMSdata data = new DMSdata { FileType = oI.FileType, FileExtension = oI.FileExtension, DownloadDocument = oI.DownloadDocument };

                        dms.DMSFields.Add(data);
                    }
                    oItem.Add(dms);
                }

                string sTr=string.Empty;
                foreach (var tr in oItem)
                {
                    sTr += "<tr><td width='25%;'>" + tr.RequestId + "</td><td colspan='3'><table class='table table-striped table-bordered table-hover'>";
                    // sTr += "<tr><td><b>File Type</b></td><td><b>File Extension</b></td><td><b>Action</b></td>";
                    foreach (var inner in tr.DMSFields)
                    {
                        sTr += "<tr><td width='25%;'>" + inner.FileType + "</td><td width='25%;'>" + inner.FileExtension + "</td> <td width='25%;' style='text-align:center'><a rel='noopener noreferrer' class='btn btn-info btn-circle' title='Download File'  target='_blank' href=Download?name=" + inner.DownloadDocument + " ><i class='fa fa-download'></i></a></td> </tr> ";
                    }
                    sTr += "</table></td></tr>";
                }
                  

                 return sTr;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return "";
        }


        public string FetchRepoDataforWildLifeAndZoo(string WildLifeAndZoo, string DateType, string FromDate, string ToDate, string PlaceID, string RequestID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<ListDMSFetchData> items = new List<ListDMSFetchData>();

            try
            {
                DataTable dt = Model.FetchRepoDataforWildLifeAndZoo(WildLifeAndZoo, DateType, FromDate, ToDate, PlaceID, RequestID);
                FetchDMSResponce Responce;

                foreach (DataRow DR in dt.Rows)
                {



                    items.Add(new ListDMSFetchData
                    {
                        RequestId = Convert.ToString(DR["RequestedID"]),
                        FileExtension = Convert.ToString(Path.GetExtension(Convert.ToString(DR["FileName"]))).Replace(".", ""),
                        FileType = Convert.ToString(DR["FileType"]),
                        DownloadDocument = Convert.ToString(DR["RepoDocID"]) + Path.GetExtension(Convert.ToString(DR["FileName"])),
                    });

                }

                var requestId = from ii in items
                                group ii.RequestId by ii.RequestId into g
                                select new { requestId = g.Key };

                List<oListDMSFetchData> oItem = new List<oListDMSFetchData>();
                foreach (var o in requestId)
                {
                    oListDMSFetchData dms = new oListDMSFetchData();
                    var itemsList = items.Where(t => t.RequestId == o.requestId);
                    dms.RequestId = o.requestId;
                    dms.DMSFields = new List<DMSdata>();
                    foreach (var oI in itemsList)
                    {
                        DMSdata data = new DMSdata { FileType = oI.FileType, FileExtension = oI.FileExtension, DownloadDocument = oI.DownloadDocument };

                        dms.DMSFields.Add(data);
                    }
                    oItem.Add(dms);
                }

                string sTr = string.Empty;
                foreach (var tr in oItem)
                {
                    sTr += "<tr><td>" + tr.RequestId + "</td><td colspan='3'><table class='table table-striped table-bordered table-hover'>";
                    sTr += "<tr>";
                    foreach (var inner in tr.DMSFields)
                    {
                        sTr += "<tr><td >" + inner.FileType + "</td><td>" + inner.FileExtension + "</td> <td style='text-align:center'><a class='btn btn-info btn-circle' title='Download File'  target='_blank' href=Download?name=" + inner.DownloadDocument + "><i class='fa fa-download'></i></a></td> </tr> ";
                    }
                    sTr += "</table></td></tr>";
                }

                return sTr;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return "";
        }


        public JsonResult GETPLACES(string TABLETYPE)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(TABLETYPE)))
                {
                    items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    DataTable dt = Model.FetchPLACES(Convert.ToString(TABLETYPE));

                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = dr["TEXT"].ToString(), Value = dr["VELUE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public FileResult Download(string Name)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                try
                {
                    
                    FetchDMSResponce Responce;
                    Responce = DMS_Service.DMSGetDocument(Convert.ToString(Path.GetFileNameWithoutExtension(Name)), DMS_Service.DocumentTypeClass.ProofOfIdentity);

                    if (Responce.File != null)
                    {

                        byte[] fileBytes = Convert.FromBase64String(Responce.File);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Name);
                    }
                }
                catch
                {
                }
            }

            return null;
        }


        [HttpGet]
        public ActionResult Index()
        {
            ModuleData();
            return View();
        }

        [HttpGet]
        public ActionResult WildLifeAndZooDMS()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                List<SelectListItem> lst1 = new List<SelectListItem>();
                List<SelectListItem> lst2 = new List<SelectListItem>();
                

               
                lst1.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                lst1.Add(new SelectListItem { Text = "WildLifeBooking", Value = "WildLifeBooking" });
                lst1.Add(new SelectListItem { Text = "ZooBooking", Value = "ZooBooking" });
                ViewBag.lst1 = lst1;

                lst2.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                lst2.Add(new SelectListItem { Text = "Date Of Visit", Value = "DATEOFVISIT" });
                lst2.Add(new SelectListItem { Text = "Date Of Booking", Value = "DATEOFBOOKING" });
                ViewBag.lst2 = lst2;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View();
        }


    }
}
