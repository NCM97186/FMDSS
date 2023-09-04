//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Apply Visit Services
//  Date Created : 06-Aug-2016
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//******

using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.CitizenService.PermissionService.EducationService;
using FMDSS.Models.CitizenService.ProductionServices.EducationService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FMDSS.Controllers.CitizenService.PermissionService.EducationService
{
    public class EducationVisitController : BaseController
    {
        //
        // GET: /EducationVisit/
        Int64 UserID = 0;
        int ModuleID = 1;
        public ActionResult EducationVisit(string aid)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> coordinator = new List<SelectListItem>();


            try
            {
                Session.Remove("EmitraDivCode");
                aid = Encryption.decrypt(aid);
                if (aid == "1")
                    ViewData["VisitType"] = "Visit Permission in Wildlife";
                else
                    ViewData["VisitType"] = "Visit Permission in Forest";

                ViewBag.Applicant_Type = new SelectList(Common.GetApplicantType(), "Value", "Text");
                ViewBag.Category = new SelectList(Common.GetCategory(), "Value", "Text");

                FilmShooting cs = new FilmShooting();
                List<SelectListItem> vehicleCategory = new List<SelectListItem>();
                DataTable dt = new DataTable();
                dt = cs.GetVehicleType();
                ViewBag.Vehiclecat = dt;
                foreach (System.Data.DataRow dr in ViewBag.Vehiclecat.Rows)
                {
                    vehicleCategory.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["CategoryID"].ToString() });
                }
                ViewBag.Vehiclecat = vehicleCategory;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View();
        }

        /// <summary>
        /// Get District by category
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        /// <summary>
        /// Get District by category
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public JsonResult DistrictbyCategory(string Category, string ReaserchType)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Research rObj = new Research();


            List<SelectListItem> District = new List<SelectListItem>();

            try
            {
                DataTable dtd = new DataTable();

                dtd = rObj.Get_CategorywiseDistrict(Category, ReaserchType);

                if (ReaserchType == "Visit Services in Wildlife")
                    return dtToViewBagJSON(dtd, "PlaceID", "PLACE_NAME");
                else
                    return dtToViewBagJSON(dtd, "DIST_CODE", "DIST_NAME");


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }

        }

        /// <summary>
        /// Bind District dropdown
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public JsonResult getDistrict(string district)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                if (!String.IsNullOrEmpty(district))
                {
                    Location location = new Location();
                    location.DistrictID = Convert.ToInt64(district);
                    DataTable dt = location.Select_Places_ByDistrict();

                    return dtToViewBagJSON(dt, "PlaceName", "PlaceId");

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }

            return null;
        }

        [HttpPost]
        
        public JsonResult GetEmitraDivCode(string LocationId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (!String.IsNullOrEmpty(LocationId))
                {
                    EducationTours edu = new EducationTours();
                    edu.Location = Convert.ToInt64(LocationId);
                    DataTable dt = edu.GetEmitraDivCode(edu.Location, "1");
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                        {

                            Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                        }

                    }
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
        /// Function to bind vehicle by category like vehicle,boat,Electra Van 
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <returns> Json result for return vehicle</returns>
        [HttpPost]
        
        public JsonResult vehicleByCategory(int vehicleCatID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            FilmShooting cs = new FilmShooting();
            List<SelectListItem> vehicle = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();
                dt = cs.Select_vehicle(Convert.ToInt64(vehicleCatID));
                ViewBag.vname = dt;
                foreach (System.Data.DataRow dr in ViewBag.vname.Rows)
                {
                    vehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["VehicleID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(vehicle, "Value", "Text"));
        }

        //Note: Code Updated with Ref. to bug ID 76
        /// <summary>
        /// Function to get fees on the basis of vehicle 
        /// </summary>
        /// <param name="VehicleCatID"></param>
        /// <param name="VehicleID"></param>
        /// <returns>Json result vehicle fees</returns>
        [HttpPost]
        
        public JsonResult feesByVehicle(int VehicleCatID, int VehicleID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<FilmShooting> fees = new List<FilmShooting>();
            try
            {
                FilmShooting cs = new FilmShooting();
                DataTable dt = new DataTable();
                cs.VehicleCatID = Convert.ToInt64(VehicleCatID);
                cs.VehicleID = Convert.ToInt64(VehicleID);
                dt = cs.Select_Fees_Per_Vehicle();

                foreach (DataRow dr in dt.Rows)
                {
                    fees.Add(new FilmShooting()
                    {
                        VehicleCatID = Int64.Parse(dr["CategoryID"].ToString()),
                        VehicleID = Convert.ToInt64(dr["VehicleID"].ToString()),
                        VehicleFees = Convert.ToDecimal(dr["Fees"].ToString()),
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(fees);
        }

        /// <summary>
        /// Function to create xml file of all vehicle details
        /// </summary>
        /// <param name="FS"></param>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult PostVehicleData(FilmShooting FS)
        {
            if (FS != null)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = DateTime.Now.Ticks.ToString();
                    if (Session["Vehicle"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("VehicleFees");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["Vehicle"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                    }
                    XmlElement Veh_TYPE = xmldoc.CreateElement("Veh_TYPE");
                    XmlElement CatID = xmldoc.CreateElement("CatID");
                    //XmlElement Category = xmldoc.CreateElement("Category");
                    XmlElement VehicleID = xmldoc.CreateElement("VehicleID");
                   // XmlElement Vehicle = xmldoc.CreateElement("Vehicle");
                    XmlElement TotalVehicle = xmldoc.CreateElement("TotalVehicle");
                    //XmlElement VehicleFees = xmldoc.CreateElement("VehicleFees");



                    CatID.InnerText = Convert.ToString(FS.VehicleCatID);
                   // Category.InnerText = FS.VehicleCategory;
                    VehicleID.InnerText = Convert.ToString(FS.VehicleID);
                    //Vehicle.InnerText = FS.Vehicle;
                    TotalVehicle.InnerText = Convert.ToString(FS.TotalVehicle);
                    //VehicleFees.InnerText = Convert.ToString(FS.VehicleFees);


                    Veh_TYPE.AppendChild(CatID);
                   // Veh_TYPE.AppendChild(Category);
                    Veh_TYPE.AppendChild(VehicleID);
                    //Veh_TYPE.AppendChild(Vehicle);
                    Veh_TYPE.AppendChild(TotalVehicle);
                   // Veh_TYPE.AppendChild(VehicleFees);

                    xmldoc.DocumentElement.AppendChild(Veh_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }

            }

            return Json(FS, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Use for Delete safari Data
        /// </summary>
        /// <param name="member"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult deletesafariData(FilmShooting member, string Id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Session["Vehicle"] != null)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    DataSet ds = new DataSet();
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (i + 1 == Convert.ToInt32(Id))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                                }
                                else
                                {
                                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml")) == true)
                                    {
                                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                                        Session["Vehicle"] = null;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(member, JsonRequestBehavior.AllowGet);
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
                        items.Add(new SelectListItem { Text = @dr[ValueField].ToString(), Value = @dr[TextField].ToString() });
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
        /// call for save data in database
        /// </summary>
        /// <param name="etour"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        
        public ActionResult CreateEducationtour(EducationTours etour,FormCollection fc, HttpPostedFileBase MemberListFilename, HttpPostedFileBase DocEducationalToueReq)
        {
            ActionResult actionResult = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string MemberListFilenameorg = string.Empty;
            string DocEducationalToueReqorg = string.Empty;
            var memberlistpath = "";
            var educationpath = "";
            try
            {

                if (MemberListFilename != null && MemberListFilename.ContentLength > 0)
                {
                    MemberListFilenameorg = Path.GetFileName(MemberListFilename.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + MemberListFilenameorg;
                    memberlistpath = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    etour.MemberListFilename = FileFullName;

                    etour.MemberListPath = @"~/PermissionDocument/" + FileFullName.Trim();
                    MemberListFilename.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    etour.MemberListFilename = "";
                    etour.MemberListPath = "";
                }

                if (DocEducationalToueReq != null && DocEducationalToueReq.ContentLength > 0)
                {
                    DocEducationalToueReqorg = Path.GetFileName(DocEducationalToueReq.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + DocEducationalToueReqorg;
                    educationpath = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    etour.DocEducationalToueReq = FileFullName;

                    etour.DocEducationalToueReqPath = @"~/PermissionDocument/" + FileFullName.Trim();
                    DocEducationalToueReq.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    etour.DocEducationalToueReq = "";
                    etour.DocEducationalToueReqPath = "";
                }


                etour.UserID = Convert.ToInt64(Session["UserID"].ToString());
                etour.EducationTourID = 0;
                DateTime now = DateTime.Now;
                string requesteId = now.Ticks.ToString();

                etour.RequestedId = requesteId;

                if (fc["Durationfrom"].ToString() == "")
                {
                    etour.DurationFrom = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    etour.DurationFrom = DateTime.ParseExact(fc["Durationfrom"].ToString(), "dd/MM/yyyy", null);



                }
                if (fc["Durationto"].ToString() == "")
                {
                    etour.DurationTo = Convert.ToDateTime(SqlDateTime.Null);

                }
                else
                {
                    etour.DurationTo = DateTime.ParseExact(fc["Durationto"].ToString(), "dd/MM/yyyy", null);


                }

              
                EducationTours _obj = new EducationTours();
                string status = _obj.addEducationtr(etour);
               
                if (!String.IsNullOrEmpty(status))
                {

                    if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                    {
                        KioskPaymentDetails _obj1 = new KioskPaymentDetails();
                        _obj1.ModuleId = 1;
                        _obj1.ServiceTypeId = 1;
                        _obj1.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                        _obj1.SubPermissionId = 1;
                        decimal totalPrice = 0;
                        _obj1.RequestedId = status;
                        DataTable dtKiosk = _obj1.FetchKisokValue(_obj1);

                        if (dtKiosk.Rows.Count > 0)
                        {
                            _obj1.RequestedId = status;
                            _obj1.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                            _obj1.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                            _obj1.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                            _obj1.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                            _obj1.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                            _obj1.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                            return PartialView("KioskPaymentDetail", _obj1);
                        }
                    }
                    else
                    {

                        TempData["ET_Status"] = "Your Request For Education Visit Services Saved Successfully! and  Requested Id:" + status;

                        // SMS code 
                        SendSMSEmailForSuccessTransaction("GETUSERDETAILSFORSENDSMSANDEMAILforEducationTour", etour.RequestedId);


                        actionResult = RedirectToAction("EducationVisit", "EducationVisit");
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return actionResult;
        }

        public void SendSMSEmailForSuccessTransaction(string ACTION, string RequestId)
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable DT = objSMSandEMAILtemplate.GetUserDetails(RequestId, ACTION);
            if (DT.Rows.Count > 0)
            {
                if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                {
                    //body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketEmailTemplate"].ToString()));

                    //objSMS_EMail_Services.sendEMail("Zoo Ticket Booking for RequestID : " + Convert.ToString(DT.Rows[0]["RequestID"]), body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["ZooTicketEmail_CC"].ToString());

                    body = string.Empty;

                    #region SMS Email
                    string UserMailBody = Common.Citizen_RequestApprovalEmailBody(DT.Rows[0]["Name"].ToString(), RequestId, DT.Rows[0]["ResearchType"].ToString());
                     string subject = "Regarding " + DT.Rows[0]["ResearchType"];
                    objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["EmailId"].ToString(), string.Empty);
                    #endregion

                }

                if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
                {
                    //body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketSMSTemplate"].ToString()));

                    //SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["Mobile"]), body);

                    //body = string.Empty;

                    string UserSmsBody = Common.Citizen_RequestApproval_SMSBody(DT.Rows[0]["Name"].ToString(), RequestId, DT.Rows[0]["ResearchType"].ToString());
                    SMS_EMail_Services.sendSingleSMS(DT.Rows[0]["Mobile"].ToString(), UserSmsBody);
                }

            }


            #endregion




        }


    }
}
