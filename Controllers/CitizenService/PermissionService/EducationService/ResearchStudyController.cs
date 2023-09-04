//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Apply Auction
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.CitizenService.ProductionServices.EducationService;
using FMDSS.Models.ForesterAction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Filters;
using System.IO;
using FMDSS.Models.Master;
using System.Configuration;
using System.Data.SqlClient;
using FMDSS.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FMDSS.Controllers.CitizenService.PermissionService.EducationService
{
    [MyAuthorization]
    public class ResearchStudyController : BaseController
    {
        Common cm = new Common();
        List<SelectListItem> distrct = new List<SelectListItem>();
        List<SelectListItem> SpaciecCategory = new List<SelectListItem>();
        List<SelectListItem> AnimalCategory = new List<SelectListItem>();
        Location location = new Location();
        CategorySpeciesAnimal obj = new CategorySpeciesAnimal();
        Int64 UserID = 0;
        int ModuleID = 1;
        //User user = null;
        /// <summary>
        /// Used for  view page
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchStudy(string aid)
        {
            //Research research = new Research();
            //research.RequestedId = "6655489369698589";
            //GetPDFSamplefortable(research);

            Research model = new Research();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> coordinator = new List<SelectListItem>();
            List<SelectListItem> IUCN = new List<SelectListItem>();
            List<SelectListItem> WLScheduleList = new List<SelectListItem>();
            CordinatorManagement CordinatorManagement = new CordinatorManagement();
            try
            {
                Session.Remove("EmitraDivCode");
                //aid = Encryption.decrypt(aid);
                //if (aid == "1")
                //    model.ResearchType = "Research in Wildlife";
                //else
                //    model.ResearchType = "Research in Forest";
                DataTable dt = CordinatorManagement.BindCoordinatorName();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    coordinator.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["CoordinatorID"].ToString() });
                }

                ////IUCN Bind 12-07-2018
                DataTable dtIUCN = CordinatorManagement.IUCNcategory();
                foreach (System.Data.DataRow dr in dtIUCN.Rows)
                {
                    IUCN.Add(new SelectListItem { Text = @dr["IUCNName"].ToString(), Value = @dr["IUCNId"].ToString() });
                }
                ViewBag.IUCN_List = IUCN;

                DataTable dtWLPA = CordinatorManagement.GetWildlifeSchedule();
                foreach (System.Data.DataRow dr in dtWLPA.Rows)
                {
                    WLScheduleList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["WildLifeScheduleId"].ToString() });
                }
                ViewBag.WLSchedule_List = WLScheduleList;

                EducationQualification obj = new EducationQualification();
                ViewBag.C_name_List = coordinator;
                ViewBag.ApplicantType_List = new SelectList(Common.GetApplicantType(), "Value", "Text");
                ViewBag.Qualification_List = new SelectList(obj.GetEducationQualification(), "Value", "Text");
                ViewBag.Category_List = new SelectList(Common.GetCategory(), "Value", "Text");
                ViewBag.ResearchCategory_List = new SelectList(Common.GetResearchCategory(), "Value", "Text");
                //changes done for the points received on 06-aug-2016 
                FMDSS.Models.CitizenService.PermissionService.FilmShooting cs = new FMDSS.Models.CitizenService.PermissionService.FilmShooting();
                List<SelectListItem> vehicleCategory = new List<SelectListItem>();
                dt = new DataTable();
                dt = cs.GetVehicleType();
                dt = dt.Select("CategoryID in (1,2)").CopyToDataTable();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    vehicleCategory.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["CategoryID"].ToString() });
                }
                ViewBag.VehicleCats_List = vehicleCategory;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(model);
        }
        public SelectList GenSelectList(DataTable dt, string Selected = "0")
        {
            List<SelectListItem> sl = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sl.Add(new SelectListItem { Value = @dr[0].ToString(), Text = @dr[1].ToString() });
            }
            return new SelectList(sl, "Value", "Text", Selected);
        }
        public SelectList GenSelectListOneColumn(DataTable dt, string Selected = "0")
        {
            List<SelectListItem> sl = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sl.Add(new SelectListItem { Value = @dr[0].ToString(), Text = @dr[0].ToString() });
            }
            return new SelectList(sl, "Value", "Text", Selected);
        }
        public ActionResult EditRequest(string id)
        {
            Research research = new Research();
            List<SelectListItem> coordinator = new List<SelectListItem>();
            List<SelectListItem> IUCN = new List<SelectListItem>();
            List<SelectListItem> WLScheduleList = new List<SelectListItem>();
            CordinatorManagement CordinatorManagement = new CordinatorManagement();
            EducationQualification obj = new EducationQualification();

            if (Session["UserID"] != null)
            {
                id = Encryption.decrypt(id);
                if (id == null)
                {
                    return View("Error");
                }
                else
                {
                    var dsReq = research.GetResearchStudyData(id);

                    #region Fill Request Data
                    if (dsReq != null && dsReq.Tables[0].Rows.Count > 0)
                    {
                        research = Globals.Util.GetListFromTable<Research>(dsReq, 0).FirstOrDefault();
                        research.SpecimenDetailsList = Globals.Util.GetListFromTable<SpecimenDetailsModel>(dsReq, 1);
                        research.SampleDetailsList = Globals.Util.GetListFromTable<SampleDetailsModel>(dsReq, 2);


                        #region Dipak Test 
                        ViewBag.C_name_List = coordinator;
                        ViewBag.ApplicantType_List = new SelectList(Common.GetApplicantType(), "Value", "Text", research.ApplicationType);
                        ViewBag.Qualification_List = new SelectList(obj.GetEducationQualification(), "Value", "Text", research.QualificationID);
                        ViewBag.Category_List = new SelectList(Common.GetCategory(), "Value", "Text", research.AreaCategory);

                        if (research.ApplicationType == 1)
                            ViewData["ResearchType"] = "Research in Wildlife";
                        else
                            ViewData["ResearchType"] = "Research in Forest";

                        ////IUCN Bind 12-07-2018
                        DataTable dtIUCN = CordinatorManagement.IUCNcategory();
                        foreach (System.Data.DataRow dr in dtIUCN.Rows)
                        {
                            IUCN.Add(new SelectListItem { Text = @dr["IUCNName"].ToString(), Value = @dr["IUCNId"].ToString() });
                        }

                        ViewBag.IUCN_List = new SelectList(IUCN, "Value", "Text", research.IUCNCategoryID);

                        DataTable dtWLPA = CordinatorManagement.GetWildlifeSchedule();
                        foreach (System.Data.DataRow dr in dtWLPA.Rows)
                        {
                            WLScheduleList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["WildLifeScheduleId"].ToString() });
                        }
                        ViewBag.WLSchedule_List = WLScheduleList;

                        #endregion 
                    }
                    #endregion
                    return View("ResearchStudy", research);
                }
            }
            else
            {
                Response.Redirect("~/WebForm1.aspx?val=logout", false);
            }
            return View(research);
        }
        private string FileStore(HttpPostedFileBase fl)
        {
            string FileFullName = string.Empty;
            if (fl != null && fl.ContentLength > 0)
            {
                FileFullName = DateTime.Now.Ticks + "_" + Path.GetFileName(fl.FileName);
                fl.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
            }
            return FileFullName;
        }
        //[HttpPost]
        //public ActionResult SaveEdit(Research research, HttpPostedFileBase ResSynopsis, HttpPostedFileBase ResPresentation, HttpPostedFileBase AssistIDProof)
        //{
        //    if (Session["UserID"] != null)
        //    {
        //        research.UserID = Convert.ToInt64(Session["UserID"].ToString());
        //        string resSynop = FileStore(ResSynopsis);
        //        research.ResSynopsisName = resSynop.Length > 0 ? resSynop : research.ResSynopsisName;
        //        string resPresetation = FileStore(ResPresentation);
        //        research.ResPresentationName = resPresetation.Length > 0 ? resPresetation : research.ResPresentationName;
        //        string resIdProof = FileStore(AssistIDProof);
        //        research.AssistIDProofName = resIdProof.Length > 0 ? resIdProof : research.AssistIDProofName;
        //        if (research.EditResearch(research).Length > 0)
        //        {
        //            ViewBag.Message = "Permission Request Updated Successfully!";
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Record Updation Failed !";
        //        }
        //    }
        //    else
        //    {
        //        Response.Redirect("~/WebForm1.aspx?val=logout", false);
        //    }
        //    return RedirectToAction("dashboard", "dashboard", new { messagetype = "3" });
        //}
        private string GetAddressOfSuperwiser(string coordinatorName)
        {
            string Address = string.Empty;
            Research research = new Research();
            DataTable dt = research.GetCoordinatorDetail(coordinatorName);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Address = dr["Address"].ToString() + "," + dr["DIST_NAME"].ToString() + "-" + dr["Pincode"].ToString();
                }
            }
            return Address;
        }
        /// <summary>
        /// Bind District
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        
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
                if (ReaserchType == "Research in Wildlife")
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
        [HttpPost]
        
        public JsonResult GetCategoryPlantanimal(string RCategory, string Category, string LocationId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Research rObj = new Research();
            List<SelectListItem> items1 = new List<SelectListItem>();
            List<SelectListItem> items2 = new List<SelectListItem>();
            try
            {
                if (RCategory == "AnimalPlant")
                {
                    DataSet dta = rObj.Get_Animal_Plant_Category("Animal", Category, LocationId, "SPCAT");
                    DataSet dtp = rObj.Get_Animal_Plant_Category("Plant", Category, LocationId, "SPCAT");
                    ViewBag.fname = dta.Tables[0];
                    ViewBag.fname1 = dtp.Tables[0];
                    foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                    {
                        items1.Add(new SelectListItem { Text = @dr["SpecieCat"].ToString(), Value = @dr["SpecieCat"].ToString() });
                    }
                    if (dta.Tables[1].Rows.Count > 0)
                    {
                        if (Convert.ToString(dta.Tables[1].Rows[0]["DivCode"]) != "")
                        {
                            Session["EmitraDivCode"] = dta.Tables[1].Rows[0]["DivCode"].ToString();
                        }
                    }
                }
                else
                {
                    DataSet dt = rObj.Get_Animal_Plant_Category(RCategory, Category, LocationId, "SPCAT");
                    ViewBag.fname = dt.Tables[0];
                    if (dt.Tables[1].Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Tables[1].Rows[0]["DivCode"]) != "")
                        {
                            Session["EmitraDivCode"] = dt.Tables[1].Rows[0]["DivCode"].ToString();
                        }
                    }
                }
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items2.Add(new SelectListItem { Text = @dr["SpecieCat"].ToString(), Value = @dr["SpecieCat"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { item1 = items1, item2 = items2 });
        }
        /// <summary>
        /// Used for bind Animal Name
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult GetAnimalNameByCategory(string ARCategory, string ACategory, string ALocationId, string AAnimal_cat)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Research rObj = new Research();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                DataTable dt = rObj.Get_Animal_Plant_Category(ARCategory, ACategory, ALocationId, AAnimal_cat, "SPNAME");
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["SpecieName"].ToString(), Value = @dr["SPECIEID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(items, "Value", "Text"));
        }
        /// <summary>
        /// Used for bind Species Name
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult GetSpeciesNameByCategory(string category)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategorySpeciesAnimal objCategory = new CategorySpeciesAnimal();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!string.IsNullOrEmpty(category))
                {
                    DataTable dt = objCategory.BindSpanimal("Spaecies", Convert.ToInt64(category));
                    ViewBag.fname = dt;
                }
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(items, "Value", "Text"));
        }
        /// <summary>
        /// used for S.No of Species & Animal
        /// </summary>
        /// <param name="animalName"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        
        public JsonResult GetAnimalsno(string animalName)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategorySpeciesAnimal objCategory = new CategorySpeciesAnimal();
            string SpaNimalSNo = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(animalName))
                {
                    DataTable dt = objCategory.GetSpeciesanimalsno(Convert.ToInt64(animalName));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            SpaNimalSNo = dr["Sno_Species_Animal"].ToString();
                        }
                    }
                    else
                    {
                        SpaNimalSNo = "Sno not Available";
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(SpaNimalSNo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Used for Bind District
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        [HttpPost]
        
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
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceId"].ToString() });
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["DivCode"].ToString() != "")
                        {
                            Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(items, "Value", "Text"));
        }
        /// <summary>
        /// User for find Coordinator detail by Coordinator Id
        /// </summary>
        /// <param name="coordinatorId"></param>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult chkCoordinator(string coordinatorName)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Coordinator coordinator = new Coordinator();
            try
            {
                if (!String.IsNullOrEmpty(coordinatorName))
                {
                    Research research = new Research();
                    DataTable dt = research.GetCoordinatorDetail(coordinatorName);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            coordinator.CoordinatorName = dr["CoordinatorId"].ToString();
                            coordinator.Address = dr["Address"].ToString() + "," + dr["DIST_NAME"].ToString() + "-" + dr["Pincode"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(coordinator, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///  This  ActionResult Method will get all submit form data, call the model method  for insert data in database and finally return on view
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]        
        public ActionResult addResearch(Research research, FormCollection fm, HttpPostedFileBase Synopsis_Name, HttpPostedFileBase Presentation_Name, string command)
        {
            //if (1 != 0)
            //{
            //    return RedirectToAction("dashboard", "dashboard", new { messagetype = "4" });
            //}
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string filePath = string.Empty;
            // string status = "";
            ResponseMsg msg = null;

            try
            {
                if (command == "Cancel")
                {
                    return RedirectToAction("dashboard", "dashboard");
                }
                string DocFileName = string.Empty;
                if (Session["UserID"] != null)
                {
                    research.UserID = Convert.ToInt64(Session["UserID"]);
                }
                DateTime now = DateTime.Now;
                string id = research.ResearchID > 0 ? research.RequestedId : now.Ticks.ToString();
                research.RequestedId = id;
                research.Status = 1;


                if (Session["KioskUserId"] != null)
                {
                    research.KioskUserId = Session["KioskUserId"].ToString();
                }
                else
                {
                    research.KioskUserId = "0";
                }
                if (Synopsis_Name != null && Synopsis_Name.ContentLength > 0)
                {
                    research.Synopsis_Path = Path.GetFileName(Synopsis_Name.FileName);
                    String FileFullName = research.RequestedId + "_" + research.Synopsis_Path;
                    research.Synopsis_Path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    research.Synopsis_Name = FileFullName;
                    research.Synopsis_Path = @"~/PermissionDocument/" + FileFullName.Trim();
                    Synopsis_Name.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    research.Synopsis_Path = "";
                    research.Synopsis_Name = "";
                }
                if (Presentation_Name != null && Presentation_Name.ContentLength > 0)
                {
                    research.Presentation_Path = Path.GetFileName(Presentation_Name.FileName);
                    String FileFullName = research.RequestedId + "_" + research.Presentation_Path;
                    research.Presentation_Path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    research.Presentation_Name = FileFullName;
                    research.Presentation_Path = @"~/PermissionDocument/" + FileFullName.Trim();
                    Presentation_Name.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    research.Presentation_Path = "";
                    research.Presentation_Name = "";
                }

                #region New Code by dipak
                if (command == "Save")
                {
                    filePath = "~/ResearchDocument/CitizenAppliedForm/CitizenRequest_" + research.RequestedId + ".pdf";
                    research.ApplicationFilePath = filePath;
                    msg = research.addResearch(research);
                }
                if (msg != null)
                {

                    #region Create Citizen Application PDF
                    DataTable dtRequest = GetNocDetail(research.RequestedId);
                    if (dtRequest != null && dtRequest.Rows.Count > 0)
                    {
                        GetPDFSamplefortable(dtRequest, filePath);
                    }
                    #endregion

                    Session["ResearchStatuss"] = msg.ReturnMsg;
                    if (Session["SSODetail"] != null)
                    {
                        UserProfile user = (UserProfile)Session["SSODetail"];
                        if (user != null)
                        {
                            if (!String.IsNullOrEmpty(user.EmailId) && !String.IsNullOrEmpty(user.MobileNumber))
                            {
                                SendSMSEmailForSuccessTransaction("GETUSERDETAILSFORSENDSMSANDEMAILforResearchStudy", research.RequestedId);
                            }
                        }

                    }
                    DataSet ds1 = research.FindReviewer();
                    if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                    {
                        string permission = ds1.Tables[0].Rows[0]["SubPermissionDesc"].ToString();
                        string userName = ds1.Tables[0].Rows[0]["Name"].ToString();
                        string emailId = ds1.Tables[0].Rows[0]["EmailId"].ToString();
                        string mobileNo = ds1.Tables[0].Rows[0]["Mobile"].ToString();

                        string reqMessage = msg.ReturnMsg;
                    }
                    if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                    {
                        research.KioskUserId = Convert.ToString(Session["KioskUserId"]);
                        KioskPaymentDetails _obj = new KioskPaymentDetails();
                        _obj.ModuleId = 1;
                        _obj.ServiceTypeId = 1;
                        _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                        _obj.SubPermissionId = 1;
                        _obj.RequestedId = research.RequestedId;
                        DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                        if (dtKiosk.Rows.Count > 0)
                        {

                            _obj.RequestedId = research.RequestedId;
                            _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                            _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                            _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                            _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                            _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                            _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                            return PartialView("KioskPaymentDetail", _obj);
                        }
                    }
                    else
                    {
                        research.KioskUserId = "0";
                    }
                }
                else
                {
                    TempData["Statuss"] = "Not inserted";
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("dashboard", "dashboard", new { messagetype = "4" });
        }
        //public ActionResult UpdateResearchStudy(string id)
        //{
        //    ViewData["disablecontrols"] = true;
        //    Research _objModel = new Research();
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    try
        //    {
        //        if (Session["UserID"] != null)
        //        {
        //            UserID = Convert.ToInt64(Session["UserID"].ToString());
        //            DataTable objdt = _objModel.GetResearchValues(id);
        //            if (objdt != null)
        //            {
        //                _objModel.RequestedId = id;
        //                //_objmodel.PermissionId = Convert.ToInt32(_objds.Tables[0].Rows[0]["P_ID"].ToString());
        //                EducationQualification obje = new EducationQualification();
        //                ViewBag.ApplicantType_List = new SelectList(Common.GetApplicantType(), "Value", "Text", objdt.Rows[0]["Applicant_Type"].ToString());
        //                _objModel.FatherName = objdt.Rows[0]["FathersName"].ToString();
        //                ViewBag.Qualification_List = obje.GetEducationQualification();//new SelectList(Common.GetQualification(), "Value", "Text", objdt.Rows[0]["Qualification"].ToString());
        //                _objModel.CollegeName = objdt.Rows[0]["College"].ToString();
        //                _objModel.TitleOfResearch = objdt.Rows[0]["R_Subject"].ToString();
        //                _objModel.Procedure = objdt.Rows[0]["R_Procedure"].ToString();
        //                //  _objModel.DurationFrom = DateTime.ParseExact(objdt.Rows[0]["DurationFrom"].ToString(), "dd/MM/yyyy", null);
        //                // _objModel.DurationTo = DateTime.ParseExact(objdt.Rows[0]["DurationTo"].ToString(), "dd/MM/yyyy", null);
        //                ViewBag.ResearchCategory_List = new SelectList(Common.GetResearchCategory(), "Value", "Text", objdt.Rows[0]["Research_Type"].ToString());
        //                List<SelectListItem> items = new List<SelectListItem>();
        //                DataTable dt = location.District();
        //                if (dt != null && dt.Rows.Count > 0)
        //                {
        //                    foreach (System.Data.DataRow dr in dt.Rows)
        //                    {
        //                        distrct.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
        //                    }
        //                    ViewBag.ddlistrict_List = new SelectList(items, "Value", "Text", objdt.Rows[0]["DIST_CODE"].ToString());
        //                }
        //                location.DistrictID = Convert.ToInt64(objdt.Rows[0]["DIST_CODE"].ToString());
        //                DataTable dtplace = location.Select_Places_ByDistrict();
        //                foreach (System.Data.DataRow dr in dtplace.Rows)
        //                {
        //                    items.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceId"].ToString() });
        //                }
        //                ViewBag.Location_List = new SelectList(items, "Value", "Text", objdt.Rows[0]["PlaceID"].ToString());
        //                DataTable dtanimalcat = obj.BindCategorySpanimal("Animal");
        //                if (dtanimalcat != null && dtanimalcat.Rows.Count > 0)
        //                {
        //                    foreach (System.Data.DataRow dr in dtanimalcat.Rows)
        //                    {
        //                        AnimalCategory.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
        //                    }
        //                    ViewBag.Animal_cat_List = new SelectList(items, "Value", "Text", objdt.Rows[0]["Animal_cat"].ToString());
        //                }
        //                DataTable dtanimal = obj.BindSpanimal("Animal", Convert.ToInt64(objdt.Rows[0]["Animal_cat"].ToString())); // 1);
        //                foreach (System.Data.DataRow dr in dtanimal.Rows)
        //                {
        //                    items.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
        //                }
        //                ViewBag.AnimalName_List = new SelectList(items, "Value", "Text", objdt.Rows[0]["Animal_Name"].ToString());
        //                DataTable dtspecies = obj.BindCategorySpanimal("Spaecies");
        //                if (dtspecies != null && dtspecies.Rows.Count > 0)
        //                {
        //                    foreach (System.Data.DataRow dr in dtspecies.Rows)
        //                    {
        //                        SpaciecCategory.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
        //                    }
        //                    ViewBag.SpeciesCat_List = new SelectList(items, "Value", "Text", objdt.Rows[0]["Species_cat"].ToString());
        //                }
        //                DataTable dtspeciesname = obj.BindSpanimal("Spaecies", Convert.ToInt64(objdt.Rows[0]["Species_cat"].ToString())); // 2); 
        //                foreach (System.Data.DataRow dr in dtspeciesname.Rows)
        //                {
        //                    items.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
        //                }
        //                ViewBag.SpeciesName_List = new SelectList(items, "Value", "Text", objdt.Rows[0]["Species_Name"].ToString());

        //                ViewBag.Category_List = new SelectList(Common.GetCategory(), "Value", "Text", _objModel.ResearchCategory);

        //                //Fill dropdown related data 
        //                List<SelectListItem> coordinator = new List<SelectListItem>();
        //                CordinatorManagement CordinatorManagement = new CordinatorManagement();
        //                DataTable dtCo = CordinatorManagement.BindCoordinatorName();
        //                foreach (System.Data.DataRow dr in dtCo.Rows)
        //                {
        //                    coordinator.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
        //                }
        //                ViewBag.C_name_List = coordinator;

        //                FMDSS.Models.CitizenService.PermissionService.FilmShooting cs = new FMDSS.Models.CitizenService.PermissionService.FilmShooting();
        //                List<SelectListItem> vehicleCategory = new List<SelectListItem>();
        //                dt = new DataTable();
        //                dt = cs.GetVehicleType();
        //                dt = dt.Select("CategoryID in (1,2)").CopyToDataTable();
        //                foreach (System.Data.DataRow dr in dt.Rows)
        //                {
        //                    vehicleCategory.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["CategoryID"].ToString() });
        //                }
        //                ViewBag.VehicleCats_List = vehicleCategory;
        //                ViewBag.ResearchCategory_List = new SelectList(Common.GetResearchCategory(), "Value", "Text");

        //            }
        //            return View("ResearchStudy", _objModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }
        //    return null;
        //}

        /// <summary>
        /// Get Category list
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult GetCategory(string cat)
        {
            dynamic result;
            if (cat == "NON-PA")
                result = new SelectList(Common.GetCategory(), "Value", "Text");
            else
                result = new SelectList(Common.GetNONPACategory(), "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        
        public JsonResult BindScheduleSpecies(string wildlifeScheduleID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (!string.IsNullOrEmpty(wildlifeScheduleID))
                {
                    DataTable dt = new Research().GetResearchDropDownData(wildlifeScheduleID, 1);
                    if (Globals.Util.isValidDataTable(dt))
                    {
                        var data = dt.AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = Convert.ToString(x.Field<long>("ID")),
                            Text = x.Field<string>("SpeciesName")
                        });
                        return Json(new { isError = false, data = data }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { isError = true }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetVillage(string gpID)
        //{
        //    try
        //    {
        //        var villageList = dbContext.tbl_FRA_Village.AsEnumerable().Where(x => gpID.Contains(Convert.ToString(x.GPID))).OrderBy(x => x.VillageName).Select(x => new { Text = x.VillageName, Value = x.VillageCode });
        //        return Json(new { isError = false, data = villageList }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult AddNewRowForItemSeized(int currentRowIndex, long objectID, string objectType)
        {
            ViewBag.CurrentIndex = currentRowIndex;

            if (objectType == "Research_SpecimenDetails")
            {
                Research model = new Research();
                SpecimenDetailsModel sim = new SpecimenDetailsModel();
                List<SpecimenDetailsModel> lst = new List<SpecimenDetailsModel>();
                lst.Add(sim);
                model.SpecimenDetailsList = lst;
                ViewBag.RowType = FMDSS.RowType.Research_SpecimenDetails;
                return PartialView("_AddNewRow", model);
            }
            else if (objectType == "Research_SampleDetails")
            {
                Research model = new Research();
                SampleDetailsModel sim = new SampleDetailsModel();
                List<SampleDetailsModel> lst = new List<SampleDetailsModel>();
                lst.Add(sim);
                model.SampleDetailsList = lst;
                ViewBag.RowType = FMDSS.RowType.Research_SampleDetails;
                return PartialView("_AddNewRow", model);
            }
            return null;
        }

        #region private Methods
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
        #endregion

        public void GetPDFSamplefortable(DataTable dtresearch, string filePath)
        {
            Document doc = new Document();

            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filePath), FileMode.Create));

            string fontpaths1 = string.Format("C:\\Windows\\Fonts\\{0}", "Mangal.ttf");
            BaseFont dev1 = BaseFont.CreateFont(fontpaths1, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi2 = new iTextSharp.text.Font(dev1, 14, iTextSharp.text.Font.BOLD);

            string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Globals.Util.GetAppSettings("FontFileName"));
            BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 8);
            iTextSharp.text.Font hindi1 = new iTextSharp.text.Font(dev, 12);

            string fontpaths = string.Format("C:\\Windows\\Fonts\\{0}", Globals.Util.GetAppSettings("FontFileName"));
            BaseFont devbold = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //iTextSharp.text.Font verdana = FontFactory.GetFont("Krutidev", 16);
            iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font hindiboldsmall = new iTextSharp.text.Font(dev, 12, iTextSharp.text.Font.BOLD);

            var FontColour = new BaseColor(0, 0, 0);
            var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
            var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);

            var subheadfont = FontFactory.GetFont("Arial", 7, FontColour);
            var subheadfontData = FontFactory.GetFont("Arial", 6, FontColour);
            var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 6);


            doc.Open();
            #region header1 d1

            PdfPTable Header = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            Header.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cells = new PdfPCell() { Border = 4 };
            Header.TotalWidth = 120;
            Header.SetTotalWidth(new float[] { 20f, 20f, 0f, 0f });


            Phrase phraseL1 = new Phrase();
            phraseL1.Add(new Chunk("Specimen Collection Plan", Myfont));
            phraseL1.Add(Chunk.NEWLINE);
            Header.AddCell(phraseL1);
            doc.Add(phraseL1);

            #endregion

            PdfPTable Details = new PdfPTable(15) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellud = new PdfPCell() { Border = 15 };
            Details.TotalWidth = 120;
            Details.SetTotalWidth(new float[] { 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f });

            cellud = new PdfPCell(new Phrase("S.No.", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("Species", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("Sub Species", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);


            cellud = new PdfPCell(new Phrase("IUCN Category", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("Wildlife Protection Act Schedule in which listed", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);


            cellud = new PdfPCell(new Phrase("Status of endemism Endemic/Non Endemic", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);



            cellud = new PdfPCell(new Phrase("Date of field visit specimen collection (Date from .. to ..)", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);



            cellud = new PdfPCell(new Phrase("Quantity of specimen to be collected", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("Name of the Park/Sanctuary", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("Geographical location from where samples are to be of collected from", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            cellud.Colspan = 3;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("GPS Location", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("Researchers who would visit the field name and contact no.", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);


            cellud = new PdfPCell(new Phrase("Final fa of the specimen collection (In case of birds and animals)", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            //2 row
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);




            cellud = new PdfPCell(new Phrase("District or Forest Division", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("Tehsil or Forest Range", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("Village/Naka", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);
            cellud = new PdfPCell(new Phrase("", hindi));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);





            //3 row
            cellud = new PdfPCell(new Phrase("1", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("2", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("3", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("4", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);




            cellud = new PdfPCell(new Phrase("5", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("6", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("7", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("8", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);



            cellud = new PdfPCell(new Phrase("9", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("10", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("11", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("12", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);


            cellud = new PdfPCell(new Phrase("13", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("14", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("15", subheadfont));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);





            //data row
            cellud = new PdfPCell(new Phrase("1", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase(Convert.ToString(dtresearch.Rows[0]["Species"]), subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase(Convert.ToString(dtresearch.Rows[0]["IUCNName"]), subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase(Convert.ToString(dtresearch.Rows[0]["ScheduleCategories"]), subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);



            cellud = new PdfPCell(new Phrase(Convert.ToString(dtresearch.Rows[0]["Place Name"]), subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);


            cellud = new PdfPCell(new Phrase(Convert.ToString(dtresearch.Rows[0]["Coordinator Address"]), subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("N/A", subheadfontData));
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cellud);


            doc.Add(Details);


            PdfPTable BottomLine = new PdfPTable(1) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            BottomLine.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cellbl = new PdfPCell() { Border = 0 };
            BottomLine.TotalWidth = 20;
            BottomLine.SetTotalWidth(new float[] { 20f });

            Phrase phraseL2 = new Phrase();
            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(new Chunk("1. Objective of Study", subheadfont));
            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(new Chunk(".........................................................................................................................", subheadfont));
            phraseL2.Add(new Chunk("..........................................................................................................................................", subheadfont));
            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(new Chunk("2. Total time duration to be spent in the field............................................................................", subheadfont));
            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(new Chunk("3. Indicate if the specimen under study has any significant Market value/Medical value/Any other significance..............", subheadfont));

            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(Chunk.NEWLINE);
            phraseL2.Add(Chunk.NEWLINE);
            BottomLine.AddCell(phraseL2);
            doc.Add(BottomLine);

            PdfPTable Footer = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            Footer.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cellsF = new PdfPCell() { Border = 0 };
            Footer.TotalWidth = 120;
            Footer.SetTotalWidth(new float[] { 20f, 20f });

            cellsF = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cellsF.Colspan = 2;
            cellsF.HorizontalAlignment = Element.ALIGN_LEFT;
            Footer.AddCell(cellsF);

            cellsF = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cellsF.Colspan = 2;
            cellsF.HorizontalAlignment = Element.ALIGN_LEFT;
            Footer.AddCell(cellsF);


            cellsF = new PdfPCell(new Phrase("Date & Place...........", subheadfont)) { Border = 0 };
            cellsF.HorizontalAlignment = Element.ALIGN_LEFT;
            Footer.AddCell(cellsF);

            cellsF = new PdfPCell(new Phrase("Signature & Full Name of Signatory with Designation", subheadfont)) { Border = 0 };
            cellsF.HorizontalAlignment = Element.ALIGN_RIGHT;
            Footer.AddCell(cellsF);

            doc.Add(Footer);

            doc.Close();


        }

        public DataTable GetNocDetail(string requestId)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = {
                new SqlParameter("@RequestId", requestId),
                new SqlParameter("@UserID",Convert.ToInt64(Session["UserId"]))
            };
            cm.Fill(ds, "KN_GetResearchReqDetail", param);
            DataTable tbl = ds.Tables[0];
            return tbl;
        }


    }
}
