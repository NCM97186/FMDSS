using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.Rescue;
using FMDSS.Models.Master;
using FMDSS.Models.Admin;
using FMDSS.Models;
using System.Data;
using System.IO;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.Encroachment.DomainModel;
using System.Text.RegularExpressions;
using FMDSS.Models.CommanModels;
using FMDSS.Models.ForesterAction;
using System.Data.SqlClient;
using System.Web.Routing;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Net;
using FMDSS.Entity.Mob_BudgetVM;
using FMDSS.Repository.Budget_Mobile;
using System.Drawing;

namespace FMDSS.Controllers.Rescue
{
    public class RescueController : Controller
    {
        private IBudget_MobileRepository _repository;

        public RescueController()
        {
            _repository = new Budget_MobileRepository();
        }
        // GET: /Rescue/
        #region "Property Intialization"
        Location _objLocation = new Location();
        AnimalName _objAnimal = new AnimalName();
        int ModuleID = 2;
        Int64 UserID = 0;
        private FmdssContext dbContext;
        #endregion
        CordinatorManagement CordinatorManagement = new CordinatorManagement();

        List<SelectListItem> Range = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        List<SelectListItem> district = new List<SelectListItem>();


        [HttpPost]
        public JsonResult mobilelogin(string SsoId)
        {

            cls_mobileLogin oLogin = new cls_mobileLogin(); 
            DataSet oObjLoginData = oLogin.LoginMobileUser(SsoId); 
            RootObject tables = new RootObject(); 
            if (Globals.Util.isValidDataSet(oObjLoginData, 0))
            {
               
                tables.loginDetail = Globals.Util.GetListFromTable<LoginDetail>(oObjLoginData, 0);
                 var Status = GetOTP(SsoId);
                if (Status ==true)
                {
                    tables.Status = 1;
                    tables.Ssoid = SsoId;
                    tables.Message = "Otp Send Your mobile number";

                }
                else
                {

                    tables.Status = 0;
                    tables.Ssoid = SsoId;
                    tables.Message = "User invalid SSoid";
                    return Json(tables, JsonRequestBehavior.AllowGet);

                }

            } 
            else
            {
                
                tables.Status = 0;
                tables.Ssoid = SsoId;
                tables.Message = "User invalid SSoid"; 
                return Json(tables, JsonRequestBehavior.AllowGet);

            }
            RootObject obj = (RootObject)tables;
            return Json(tables, JsonRequestBehavior.AllowGet);
            
        }

       [HttpPost]
        public JsonResult CheckOTP(string SsoId, string OTP)
        {
            RootObject tables = new RootObject();
            if (Session["OTPCODE"].ToString() == OTP)
            {
                cls_mobileLogin oLogin = new cls_mobileLogin(); 
                DataSet oObjLoginData = oLogin.LoginMobileUser(SsoId); 
                if (Globals.Util.isValidDataSet(oObjLoginData, 0))
                {

                    tables.loginDetail = Globals.Util.GetListFromTable<LoginDetail>(oObjLoginData, 0);
                    tables.ModuleList = Globals.Util.GetListFromTable<ModuleList>(oObjLoginData, 1);
                    tables.Ssoid = SsoId;
                    tables.Message = "Otp Check successfully";
                    tables.Status = 1;

                }
            }
            else
            {
                var status ="Otp not valid";
                return Json(status, JsonRequestBehavior.AllowGet);

            }

            RootObject obj = (RootObject)tables;
            return Json(tables, JsonRequestBehavior.AllowGet);

            
        }


        [HttpPost]
        public JsonResult getRescueList(string SsoId)
        {
            DataSet RescueList1 = new DataSet();
            RootObject tables = new RootObject();
            cls_mobileLogin oLogin = new cls_mobileLogin();
               
                string returnMsg = string.Empty;
                DataTable oObjLoginData = oLogin.GetUseridbySsoid(SsoId);

                if(oObjLoginData!=null)
                {
                    RescueList1 = oLogin.getRescueList(oObjLoginData.Rows[0][0].ToString());
                   tables.RescueList = Globals.Util.GetListFromTable<RescueList>(RescueList1, 0);

            }

            RootObject obj = (RootObject)tables;
            return Json(tables, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public JsonResult getRescueList_new(string UserId)
        {
            DataSet RescueList1 = new DataSet();
            RootObject tables = new RootObject();
            cls_mobileLogin oLogin = new cls_mobileLogin(); 
 
            RescueList1 = oLogin.getRescueList(UserId);
            tables.RescueList = Globals.Util.GetListFromTable<RescueList>(RescueList1, 0);
 
            RootObject obj = (RootObject)tables;
            return Json(tables, JsonRequestBehavior.AllowGet);


        }


        //
        // GET: /MobileLogin/
        [System.Web.Http.HttpGet]
        public bool GetOTP(string SsoId)
        {
            cls_mobileLogin oLogin = new cls_mobileLogin();
            bool isError = false;
            cls_UserOTPResponceCode model = new cls_UserOTPResponceCode();
            string returnMsg = string.Empty;
            var oObjLoginData = oLogin.GetOTP(SsoId);

            if (oObjLoginData.Rows.Count > 0)
            {
                model.Mobile = Convert.ToString(oObjLoginData.Rows[0]["Mobile"]);
                model.Email = Convert.ToString(oObjLoginData.Rows[0]["EmailId"]);
                model.OTP = Convert.ToString(oObjLoginData.Rows[0]["OTP"]);
                model.Status = Convert.ToString(oObjLoginData.Rows[0]["Status"]);
                model.UserSmsBody = string.Format("{0} is the One Time Password(OTP) to process, expires in 2 mins. Verify now ", model.OTP);

                SMS_EMail_Services.sendSingleSMS(Convert.ToString(model.Mobile), model.UserSmsBody);
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();

                objSMSandEMAILtemplate.SendMailComman("ALL", "GET_OTP_FOR_MOBILE", model.UserSmsBody, model.UserSmsBody, model.Email, "", "");
                Session["OTPCODE"] = model.OTP;
                var STATUS = true;
                return STATUS;
            }
            else
            {
                 
                return isError;

            }

            // return Json(new { Status= Status }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult getForestOfficerWithNGO(string designation)
        {
            List<SelectListItem> lstOfficer = new List<SelectListItem>();
            try
            {
                DAL dal = new DAL();
                DataSet dsOfficerDesig = new DataSet();
                SqlParameter[] paramBlock = { new SqlParameter("@option", "2"),
                                             new SqlParameter("@ssoid", Session["SSOid"]),
                                              new SqlParameter("@EmpDesig", designation),
                                            };
                dal.Fill(dsOfficerDesig, "Sp_GetDesignatationForResuceModule", paramBlock);
                ViewBag.fname = dsOfficerDesig.Tables[0];
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    if (@dr["SSO_ID"].ToString() != "--Select--")
                    {
                        lstOfficer.Add(new SelectListItem { Text = @dr["SSO_ID"].ToString(), Value = @dr["SSO_ID"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error " + ex);
            }
            return Json(new SelectList(lstOfficer, "Value", "Text"));
        }
        [HttpPost]
        public JsonResult GetNgoDetail()
        {
            List<SelectListItem> lstNGO = new List<SelectListItem>();
            try
            {
                DAL dal = new DAL();
                DataSet dsOfficerDesig = new DataSet();
                SqlParameter[] paramBlock = { new SqlParameter("@option", "3"),
                                             new SqlParameter("@ssoid", ""),
                                              new SqlParameter("@EmpDesig", ""),
                                            };
                dal.Fill(dsOfficerDesig, "Sp_GetDesignatationForResuceModule", paramBlock);
                ViewBag.fname = dsOfficerDesig.Tables[0];
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    lstNGO.Add(new SelectListItem { Text = @dr["VendorSSOId"].ToString(), Value = @dr["WaterHoleVendorDetailsID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error " + ex);
            }
            return Json(new SelectList(lstNGO, "Value", "Text"));
        }
        public ActionResult Index(string RoleName = "Q2l0aXplbg==")
        {
            RescueModel models = new RescueModel();
            List<RescueModel> list = new List<RescueModel>();
            ViewBag.CitizenRolesEnc = RoleName;
            TempData["encRoleNameIndex"] = RoleName;
            RoleName = Encryption.decrypt(RoleName);
            models.CitizenRole = RoleName;//Convert.ToString(Session["Role"]);
            list = models.getAllRegistrations(models.CitizenRole, Convert.ToInt64(Session["UserID"]));
            ViewBag.Roles = GetRoleNames();
            ViewBag.CitizenRoles = models.CitizenRole;
            return View(list);
        }

        #region Needs Work Dipak
        public ActionResult RescueDetails(string RoleName = "Q2l0aXplbg==")
        {
            UserID = Convert.ToInt64(Session["UserId"]);
            RescueModel model = new RescueModel();
            if (UserID > 0)
            {
                dbContext = new FmdssContext();
                tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == UserID);
                if (tblUserProfile != null)
                {
                    model.CitizenName = tblUserProfile.Name;
                    model.CitizenMobileNo = tblUserProfile.Mobile;
                    model.CitizenEmailID = tblUserProfile.EmailId;
                }
            }
            model.Rural = true;
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            List<SelectListItem> lstChildAnimals = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(lstDistricts[0].Value).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(lstDistricts[0].Value, lstBlocks[1].Value).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(lstDistricts[0].Value, lstBlocks[1].Value, lstGPs[1].Value).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;

            #region Below code is commented by pooran on 01-08-2019 as city name and ward name are not being shwon in UI so not in use            
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            #endregion

            foreach (System.Data.DataRow dr in model.GetDropdownData(1).Tables[0].Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;


            //foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            //{
            //    lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            //}
            //ViewBag.ChildAnimals = lstChildAnimals;

            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Roles = GetRoleNames();
            ViewBag.RoleName = Convert.ToString(Session["Role"]); ;
            ViewBag.CitizenRoles = RoleName;
            return View(model);
        }

        [HttpPost]
        public ActionResult RescueDetails(RescueModel model, HttpPostedFileBase[] ImageDataRegistration, string RoleName = "Q2l0aXplbg==")
        {
            try
            {

                if (model.AssistanceTypeFirstAidModel != null && model.AssistanceTypeFirstAidModel.Where(d => d.IsChecked == true).Count() > 0)
                {
                    model.AssistanceTypeFirstAid = string.Join(",", model.AssistanceTypeFirstAidModel.Where(d => d.IsChecked == true).Select(d => d.Name));
                }
                DataTable dtImg = new DataTable();
                dtImg.Columns.Add("ID");
                dtImg.Columns.Add("ActiveStatus");
                dtImg.Columns.Add("ImagePath");
                dtImg.Columns.Add("Type");
                if (ImageDataRegistration != null)
                {
                    foreach (HttpPostedFileBase file in ImageDataRegistration)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {

                            string fileName = "Registration_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                            string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                            file.SaveAs(_FileName);
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = "~/RescueDocument/" + fileName;
                            dr["Type"] = "Registration";
                            dtImg.Rows.Add(dr);
                        }
                    }
                }
                if (dtImg.Rows.Count > 0)
                    model.Images = dtImg;

                //if (ModelState.IsValid)
                //{
                if (!model.Casualty)
                {
                    model.CasualtyType = string.Empty;
                    model.MediAssistRequired = false;
                }
                if (!model.MediAssistRequired)
                {
                    model.MediAssistType = string.Empty;
                    model.NoOfPersonInjured = "0";
                    model.CasualtyDescription = string.Empty;
                }
                model.RescueStatus = "New";
                model.UserID = Convert.ToInt32(Session["UserID"]);

                Entity.ResponseMsg msg = model.createRegistration(model);
                model.RegistrationID = Convert.ToInt32(msg.ReturnIDs);
                if (model.RegistrationID > 0)
                {
                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RegistrationID.ToString(), "Rescue");


                    if (DT != null && DT.Rows.Count > 0)
                    {
                        objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue Registration", model.RegistrationID.ToString(), Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));
                    }
                    #endregion
                    TempData["ReturnMsg"] = msg.ReturnMsg;
                    TempData["IsError"] = msg.IsError;
                    return RedirectToAction("Index", new { RoleName = RoleName });
                }
                //}
                TempData["ReturnMsg"] = "Something went wrong.Please try again.";
                TempData["IsError"] = true;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ReturnMsg"] = ex.Message;
                TempData["IsError"] = true;
                return RedirectToAction("Index");
            }
        }

        #endregion

        public ActionResult GetUserDetail()
        {
            UserID = Convert.ToInt64(Session["UserId"]);
            RescueModel model = new RescueModel();
            if (UserID > 0)
            {
                dbContext = new FmdssContext();
                tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == UserID);
                if (tblUserProfile != null)
                {
                    model.CitizenName = tblUserProfile.Name;
                    model.CitizenMobileNo = tblUserProfile.Mobile;
                    model.CitizenEmailID = tblUserProfile.EmailId;
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(RescueModel model, HttpPostedFileBase[] ImageDataRegistration)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.OtherAnimalName))
                {
                    //insert it in Animal Master Table

                }
                //HttpPostedFileBase file = Request.Files["ImageDataRegistration"];
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                if (string.IsNullOrWhiteSpace(model.CitizenName))
                {
                    ModelState.AddModelError("CitizenName", "Citizen Name is Required");
                }
                //if (string.IsNullOrWhiteSpace(model.CitizenEmailID))
                //{
                //    ModelState.AddModelError("CitizenEmailID", "Citizen Email ID is Required");
                //}
                if (string.IsNullOrWhiteSpace(model.CitizenMobileNo))
                {
                    ModelState.AddModelError("CitizenMobileNo", "Citizen Mobile No is Required");
                }
                else if (!(model.CitizenMobileNo.Replace("+", "").Replace("-", "").Length == 10 || model.CitizenMobileNo.Replace("+", "").Replace("-", "").Length == 12))
                {
                    ModelState.AddModelError("CitizenMobileNo", "Citizen Mobile No is not in proper format");
                }
                if (!string.IsNullOrWhiteSpace(model.CitizenEmailID))
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(model.CitizenEmailID);
                    if (!match.Success)
                    {
                        ModelState.AddModelError("CitizenEmailID", "Citizen Email ID is not in proper format");
                    }
                }
                if (string.IsNullOrWhiteSpace(model.DistrictID) || model.DistrictID == "0")
                {
                    ModelState.AddModelError("DistrictID", "District is Required");
                }
                if (model.Rural)
                {
                    if (string.IsNullOrWhiteSpace(model.BlockID) || model.BlockID == "0")
                    {
                        ModelState.AddModelError("BlockID", "Block is Required");
                    }
                    if (string.IsNullOrWhiteSpace(model.GPID) || model.GPID == "0")
                    {
                        ModelState.AddModelError("GPID", "Gram Panchayat is Required");
                    }
                    if (string.IsNullOrWhiteSpace(model.VillageID) || model.VillageID == "0")
                    {
                        ModelState.AddModelError("VillageID", "Village is Required");
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.CityID) || model.CityID == "0")
                    {
                        ModelState.AddModelError("CityID", "City is Required");
                    }
                    if (string.IsNullOrWhiteSpace(model.WardID) || model.WardID == "0")
                    {
                        ModelState.AddModelError("WardID", "Ward is Required");
                    }
                }
                if (string.IsNullOrWhiteSpace(model.Location))
                {
                    ModelState.AddModelError("Location", "Location is Required");
                }
                if (string.IsNullOrWhiteSpace(model.AnimalID) || model.AnimalID == "0")
                {
                    ModelState.AddModelError("AnimalID", "Animal Name is Required");
                }
                if (model.AssistanceTypeFirstAidModel != null && model.AssistanceTypeFirstAidModel.Where(d => d.IsChecked == true).Count() > 0)
                {
                    model.AssistanceTypeFirstAid = string.Join(",", model.AssistanceTypeFirstAidModel.Where(d => d.IsChecked == true).Select(d => d.Name));
                }
                DataTable dtImg = new DataTable();
                dtImg.Columns.Add("ID");
                dtImg.Columns.Add("ActiveStatus");
                dtImg.Columns.Add("ImagePath");
                dtImg.Columns.Add("Type");
                if (ImageDataRegistration != null)
                {
                    foreach (HttpPostedFileBase file in ImageDataRegistration)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {

                            string fileName = "Registration_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                            string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                            //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                            file.SaveAs(_FileName);
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = "~/RescueDocument/" + fileName;
                            dr["Type"] = "Registration";
                            dtImg.Rows.Add(dr);

                        }

                    }
                }
                if (dtImg.Rows.Count > 0)
                    model.Images = dtImg;

                if (ModelState.IsValid)
                {
                    if (!model.Casualty)
                    {
                        model.CasualtyType = string.Empty;
                        model.MediAssistRequired = false;
                    }
                    if (!model.MediAssistRequired)
                    {
                        model.MediAssistType = string.Empty;
                        model.NoOfPersonInjured = "0";
                        model.CasualtyDescription = string.Empty;
                    }
                    model.RescueStatus = "New";
                    model.UserID = Convert.ToInt32(Session["UserID"]);

                    Entity.ResponseMsg msg = model.createRegistration(model);
                    if (!msg.IsError)
                    {
                        #region Email and SMS
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RegistrationID.ToString(), "Rescue");
                        if (DT != null && DT.Rows.Count > 0)
                        {
                            objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue Registration", msg.ReturnIDs, Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                        }
                        #endregion
                        TempData["SuccessMsg"] = "Report submitted successfully.";
                        return RedirectToAction("Index");
                    }
                }
                List<SelectListItem> lstDistricts = new List<SelectListItem>();
                List<SelectListItem> lstBlocks = new List<SelectListItem>();
                List<SelectListItem> lstGPs = new List<SelectListItem>();
                List<SelectListItem> lstVillages = new List<SelectListItem>();
                List<SelectListItem> lstAnimals = new List<SelectListItem>();
                List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
                List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
                List<SelectListItem> lstCity = new List<SelectListItem>();
                List<SelectListItem> lstWard = new List<SelectListItem>();
                foreach (System.Data.DataRow dr in _objLocation.District().Rows)
                {
                    lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
                }
                ViewBag.Districts = lstDistricts;
                foreach (System.Data.DataRow dr in _objLocation.BindBlockName(model.DistrictID == null || model.DistrictID == "0" ? lstDistricts[0].Value : model.DistrictID).Rows)
                {
                    lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
                }
                ViewBag.Blocks = lstBlocks;
                foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(model.DistrictID == null || model.DistrictID == "0" ? lstDistricts[0].Value : model.DistrictID, model.BlockID == "0" ? lstBlocks[1].Value : model.BlockID).Rows)
                {
                    lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
                }
                ViewBag.GPs = lstGPs;
                foreach (System.Data.DataRow dr in _objLocation.BindVillageName(model.DistrictID == null || model.DistrictID == "0" ? lstDistricts[0].Value : model.DistrictID, model.BlockID == "0" ? lstBlocks[1].Value : model.BlockID, model.GPID == "0" ? lstGPs[1].Value : model.GPID).Rows)
                {
                    lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
                }
                ViewBag.Villages = lstVillages;
                foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
                {
                    lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
                }
                ViewBag.City = lstCity;
                foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstCity[0].Value).Rows)
                {
                    lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
                }
                ViewBag.Ward = lstWard;
                foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
                {
                    lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
                }
                ViewBag.Animals = lstAnimals;
                foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
                {
                    lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.MediAssistTypes = lstMediAssistType;
                foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
                {
                    lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.CasualtyTypes = lstCasualtyType;
                ViewBag.Roles = GetRoleNames();
                ViewBag.ErrorMsg = "Something went wrong.Please try again.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }
            return View(model);
        }

        public ActionResult Approve(int? registrationID, string RoleName = "Q2l0aXplbg==")
        {
            RescueModel model = new RescueModel();
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(model.DistrictID).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(model.DistrictID, model.BlockID).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(model.DistrictID, model.BlockID, model.GPID).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;
            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Roles = GetRoleNames();
            ViewBag.CitizenRoles = RoleName;
            model.CitizenRole = RoleName;
            return View(model);
        }

        [HttpPost]
        public ActionResult Approve(RescueModel model, string button)
        {
            if (ModelState.IsValid)
            {
                UserID = Convert.ToInt64(Session["UserId"]);
                if (UserID > 0)
                {
                    dbContext = new FmdssContext();
                    tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == UserID);
                    if (tblUserProfile != null)
                    {
                        model.RescueOfficerName = tblUserProfile.Ssoid;
                        model.RescueOfficerDesig = tblUserProfile.Designation;
                    }
                }

                if (button == "Approve")
                {
                    model.RegistrationApproved = true;
                    model.RescueStatus = "Approved";
                }
                else
                {
                    model.RegistrationApproved = false;
                    model.RescueStatus = "Rejected";
                }
                model.updateRegistrationApprove(model);
                #region Email and SMS
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RegistrationID.ToString(), "CWLW Approved Rescue");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", "Send TO CWLW TO DCF Rescue", model.RegistrationID.ToString(), Convert.ToString(model.CWLWRemarks), Convert.ToString(DT.Rows[0]["emailID"]), Convert.ToString(DT.Rows[0]["Mobile"]), Convert.ToString(model.RescueStatus));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion
                return RedirectToAction("Index", "Rescue", new { RoleName = model.CitizenRole });
            }
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(lstDistricts[0].Value).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(lstDistricts[0].Value, lstBlocks[1].Value).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(lstDistricts[0].Value, lstBlocks[1].Value, lstGPs[1].Value).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Animals = lstAnimals;
            ViewBag.Roles = GetRoleNames();
            return View(model);
        }

        public ActionResult Assign(int? registrationID)
        {
            RescueModel model = new RescueModel();
            ViewBag.CitizenRoles = TempData["encRoleNameIndex"];
            TempData["EncRole"] = TempData["encRoleNameIndex"];
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            List<SelectListItem> WLScheduleList = new List<SelectListItem>();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(model.DistrictID).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(model.DistrictID, model.BlockID).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(model.DistrictID, model.BlockID, model.GPID).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;
            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Roles = GetRoleNames();
            #region New Added
            DataTable dtWLPA = CordinatorManagement.GetWildlifeSchedule();
            var SelectedValues = dtWLPA.AsEnumerable().Where(d => d.Field<int>("RescueCWLWApproveStatus") == 1).Select(s => s.Field<int>("WildLifeScheduleId")).ToArray();
            model.WLScheduleListCWLWApproval = string.Join(",", SelectedValues);

            foreach (System.Data.DataRow dr in dtWLPA.Rows)
            {
                WLScheduleList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["WildLifeScheduleId"].ToString() });
            }

            ViewBag.WLSchedule_List = WLScheduleList;
            #endregion

            #region Get Designation
            DataTable dtOfficerDesignation = new DataTable();
            List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
            dtOfficerDesignation = model.GetOfficerDesignationRescue();
            if (dtOfficerDesignation.Rows.Count > 0)
            {

                foreach (System.Data.DataRow dr in dtOfficerDesignation.Rows)
                {
                    lstOfficerDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["EmpDesignation"].ToString() });
                }
                ViewBag.OfficerDesignation = lstOfficerDesignation;
            }
            else
            {
                lstOfficerDesignation.Add(new SelectListItem { Text = "NA", Value = "0" });
                ViewBag.OfficerDesignation = lstOfficerDesignation;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Assign(RescueModel model, string buttonType)
        {
            try
            {
                UserID = Convert.ToInt64(Session["UserId"]);
                if (UserID > 0)
                {
                    dbContext = new FmdssContext();
                    tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == UserID);
                    if (tblUserProfile != null)
                    {
                        model.RescueOfficerName = model.ForestStaffSSOID; //tblUserProfile.Ssoid;
                        model.RescueOfficerDesig = (model.SendToNGOOrSelf).ToString();
                    }
                }


                if (ModelState.IsValid)
                {
                    model.RescueStatus = "Assigned";
                    Entity.ResponseMsg msg = model.UpdateRegistrationAssigned(model);
                    if (!msg.IsError)
                    {
                        #region Email and SMS
                        string SMSModuleName = string.Empty;
                        string Status = string.Empty;
                        if (model.RescueStatus == "Assigned")
                            SMSModuleName = "Rescue Assign";

                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RegistrationID.ToString(), SMSModuleName);
                        if (DT != null && DT.Rows.Count > 0)
                        {
                            objSMSandEMAILtemplate.SendMailComman("ALL", SMSModuleName, model.RegistrationID.ToString(), model.RegistrationID.ToString(), Convert.ToString(DT.Rows[0]["emailID"]), Convert.ToString(DT.Rows[0]["Mobile"]), Convert.ToString(Status));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                        }
                        #endregion
                        TempData["ReturnMsg"] = msg.ReturnMsg;
                        TempData["IsError"] = msg.IsError;
                        return RedirectToAction("Index", "Rescue", new { RoleName = TempData["EncRole"] });
                    }
                }
                List<SelectListItem> lstDistricts = new List<SelectListItem>();
                List<SelectListItem> lstBlocks = new List<SelectListItem>();
                List<SelectListItem> lstGPs = new List<SelectListItem>();
                List<SelectListItem> lstVillages = new List<SelectListItem>();
                List<SelectListItem> lstAnimals = new List<SelectListItem>();
                List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
                List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
                List<SelectListItem> lstCity = new List<SelectListItem>();
                List<SelectListItem> lstWard = new List<SelectListItem>();
                List<SelectListItem> WLScheduleList = new List<SelectListItem>();
                foreach (System.Data.DataRow dr in _objLocation.District().Rows)
                {
                    lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
                }
                ViewBag.Districts = lstDistricts;
                foreach (System.Data.DataRow dr in _objLocation.BindBlockName(lstDistricts[0].Value).Rows)
                {
                    lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
                }
                ViewBag.Blocks = lstBlocks;
                foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(lstDistricts[0].Value, lstBlocks[1].Value).Rows)
                {
                    lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
                }
                ViewBag.GPs = lstGPs;
                foreach (System.Data.DataRow dr in _objLocation.BindVillageName(lstDistricts[0].Value, lstBlocks[1].Value, lstGPs[1].Value).Rows)
                {
                    lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
                }
                ViewBag.Villages = lstVillages;
                foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
                {
                    lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
                }
                ViewBag.City = lstCity;
                foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
                {
                    lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
                }
                ViewBag.Ward = lstWard;
                foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
                {
                    lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
                }
                ViewBag.Animals = lstAnimals;
                foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
                {
                    lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.MediAssistTypes = lstMediAssistType;
                foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
                {
                    lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.CasualtyTypes = lstCasualtyType;
                ViewBag.Roles = GetRoleNames();
                #region New Added
                DataTable dtWLPA = CordinatorManagement.GetWildlifeSchedule();
                foreach (System.Data.DataRow dr in dtWLPA.Rows)
                {
                    WLScheduleList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["WildLifeScheduleId"].ToString() });
                }
                ViewBag.WLSchedule_List = WLScheduleList;
                #endregion
                ViewBag.ErrorMsg = "Something went wrong.Please try again.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }
            return View(model);
        }

        public ActionResult Capture(int? registrationID)
        {
            RescueModel model = new RescueModel();
            ViewBag.CitizenRoles = TempData["encRoleNameIndex"];
            TempData["EncRole"] = TempData["encRoleNameIndex"];
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            List<SelectListItem> WLScheduleList = new List<SelectListItem>();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(model.DistrictID).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(model.DistrictID, model.BlockID).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(model.DistrictID, model.BlockID, model.GPID).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;
            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Roles = GetRoleNames();
            #region New Added
            DataTable dtWLPA = CordinatorManagement.GetWildlifeSchedule();
            var SelectedValues = dtWLPA.AsEnumerable().Where(d => d.Field<int>("RescueCWLWApproveStatus") == 1).Select(s => s.Field<int>("WildLifeScheduleId")).ToArray();
            model.WLScheduleListCWLWApproval = string.Join(",", SelectedValues);

            foreach (System.Data.DataRow dr in dtWLPA.Rows)
            {
                WLScheduleList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["WildLifeScheduleId"].ToString() });
            }

            ViewBag.WLSchedule_List = WLScheduleList;
            #endregion

            #region Get Designation
            DataTable dtOfficerDesignation = new DataTable();
            List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
            dtOfficerDesignation = model.GetOfficerDesignationRescue();
            if (dtOfficerDesignation.Rows.Count > 0)
            {

                foreach (System.Data.DataRow dr in dtOfficerDesignation.Rows)
                {
                    lstOfficerDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["EmpDesignation"].ToString() });
                }
                ViewBag.OfficerDesignation = lstOfficerDesignation;
            }
            else
            {
                lstOfficerDesignation.Add(new SelectListItem { Text = "NA", Value = "0" });
                ViewBag.OfficerDesignation = lstOfficerDesignation;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Capture(RescueModel model, string buttonType, HttpPostedFileBase[] ImageDataCapture, HttpPostedFileBase[] ImageDataCaptureStaff)
        {
            try
            {
                UserID = Convert.ToInt64(Session["UserId"]);
                if (UserID > 0)
                {
                    dbContext = new FmdssContext();
                    tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == UserID);
                    if (tblUserProfile != null)
                    {
                        model.RescueOfficerName = tblUserProfile.Ssoid;
                        model.RescueOfficerDesig = tblUserProfile.Designation;
                    }
                }

                if (buttonType == "Submit")
                {
                    DataTable dtImg = new DataTable();
                    dtImg.Columns.Add("ID");
                    dtImg.Columns.Add("ActiveStatus");
                    dtImg.Columns.Add("ImagePath");
                    dtImg.Columns.Add("Type");
                    if (ImageDataCapture != null && ImageDataCaptureStaff != null)
                    {
                        foreach (HttpPostedFileBase file in ImageDataCapture)
                        {
                            //Checking file is available to save.  
                            if (file != null)
                            {
                                string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                                string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                                //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                                file.SaveAs(_FileName);
                                DataRow dr = dtImg.NewRow();
                                dr["ImagePath"] = "~/RescueDocument/" + fileName;
                                dr["Type"] = "Capture";
                                dtImg.Rows.Add(dr);
                            }
                        }

                        foreach (HttpPostedFileBase file in ImageDataCaptureStaff)
                        {
                            //Checking file is available to save.  
                            if (file != null)
                            {
                                string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                                string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                                //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                                file.SaveAs(_FileName);
                                DataRow dr = dtImg.NewRow();
                                dr["ImagePath"] = "~/RescueDocument/" + fileName;
                                dr["Type"] = "CaptureStaff";
                                dtImg.Rows.Add(dr);
                            }
                        }

                        if (dtImg.Rows.Count > 0)
                            model.Images = dtImg;
                    }
                    else
                    {
                        ModelState.AddModelError("RescuePhotoPath", "Upload Picture is Required");
                    }

                    if (string.IsNullOrWhiteSpace(model.RescueRemarks))
                    {
                        ModelState.AddModelError("RescueRemarks", "Remarks is Required");
                    }
                }
                //if (ModelState.IsValid)
                //{
                if (buttonType == "Send TO CWLW Approval")
                {
                    model.RescueStatus = "Send TO CWLW Approval";
                }
                else if (buttonType == "Send TO NGO")
                {
                    model.RescueStatus = "Send TO NGO";
                }
                else
                {
                    model.RescueStatus = "Captured";

                }

                if (model.SendToNGOOrSelf == -1)
                {
                    model.SendToOfficerSSOID = model.ForestStaffSSOID;
                }

                if (model.AssistanceTypeFirstAidModel != null && model.AssistanceTypeFirstAidModel.Where(d => d.IsChecked == true).Count() > 0)
                {
                    model.AssistanceTypeFirstAid = string.Join(",", model.AssistanceTypeFirstAidModel.Where(d => d.IsChecked == true).Select(d => d.Name));
                }

                Entity.ResponseMsg msg = model.updateRegistrationCapture(model);
                if (!msg.IsError)
                {
                    #region Email and SMS
                    string SMSModuleName = string.Empty;
                    string Status = string.Empty;
                    if (model.RescueStatus == "Captured")
                        SMSModuleName = "Rescue Capture";
                    else if (model.RescueStatus == "Send TO NGO")
                    {
                        SMSModuleName = "Send TO NGO Rescue";
                        Status = model.SendTONGORemarks;
                    }
                    else if (model.RescueStatus == "Send TO CWLW Approval")
                    {
                        SMSModuleName = "Send TO CWLW Rescue";
                        Status = model.WildlifeScheduleRemarks;
                    }
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, SMSModuleName);
                    if (DT != null && DT.Rows.Count > 0)
                    {
                        objSMSandEMAILtemplate.SendMailComman("ALL", SMSModuleName, msg.ReturnIDs, msg.ReturnIDs, Convert.ToString(DT.Rows[0]["emailID"]), Convert.ToString(DT.Rows[0]["Mobile"]), Convert.ToString(Status));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                    }
                    #endregion
                    TempData["ReturnMsg"] = msg.ReturnMsg;
                    TempData["IsError"] = false;
                    return RedirectToAction("Index", "Rescue", new { RoleName = TempData["EncRole"] });
                }
                //}
                List<SelectListItem> lstDistricts = new List<SelectListItem>();
                List<SelectListItem> lstBlocks = new List<SelectListItem>();
                List<SelectListItem> lstGPs = new List<SelectListItem>();
                List<SelectListItem> lstVillages = new List<SelectListItem>();
                List<SelectListItem> lstAnimals = new List<SelectListItem>();
                List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
                List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
                List<SelectListItem> lstCity = new List<SelectListItem>();
                List<SelectListItem> lstWard = new List<SelectListItem>();
                List<SelectListItem> WLScheduleList = new List<SelectListItem>();
                foreach (System.Data.DataRow dr in _objLocation.District().Rows)
                {
                    lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
                }
                ViewBag.Districts = lstDistricts;
                foreach (System.Data.DataRow dr in _objLocation.BindBlockName(lstDistricts[0].Value).Rows)
                {
                    lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
                }
                ViewBag.Blocks = lstBlocks;
                foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(lstDistricts[0].Value, lstBlocks[1].Value).Rows)
                {
                    lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
                }
                ViewBag.GPs = lstGPs;
                foreach (System.Data.DataRow dr in _objLocation.BindVillageName(lstDistricts[0].Value, lstBlocks[1].Value, lstGPs[1].Value).Rows)
                {
                    lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
                }
                ViewBag.Villages = lstVillages;
                foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
                {
                    lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
                }
                ViewBag.City = lstCity;
                foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
                {
                    lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
                }
                ViewBag.Ward = lstWard;
                foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
                {
                    lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
                }
                ViewBag.Animals = lstAnimals;
                foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
                {
                    lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.MediAssistTypes = lstMediAssistType;
                foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
                {
                    lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.CasualtyTypes = lstCasualtyType;
                ViewBag.Roles = GetRoleNames();
                #region New Added
                DataTable dtWLPA = CordinatorManagement.GetWildlifeSchedule();
                foreach (System.Data.DataRow dr in dtWLPA.Rows)
                {
                    WLScheduleList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["WildLifeScheduleId"].ToString() });
                }
                ViewBag.WLSchedule_List = WLScheduleList;
                #endregion
                ViewBag.ErrorMsg = "Something went wrong.Please try again.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }
            return View(model);
        }

        public ActionResult Release(int? registrationID)
        {
            ViewBag.CitizenRoles = TempData["encRoleNameIndex"];
            TempData["EncRoleRelease"] = TempData["encRoleNameIndex"];
            RescueModel model = new RescueModel();
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            List<SelectListItem> WLScheduleList = new List<SelectListItem>();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(model.DistrictID).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(model.DistrictID, model.BlockID).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(model.DistrictID, model.BlockID, model.GPID).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;
            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Roles = GetRoleNames();
            #region New Added
            DataTable dtWLSchedule = CordinatorManagement.GetWildlifeSchedule();
            foreach (System.Data.DataRow dr in dtWLSchedule.Rows)
            {
                WLScheduleList.Add(new SelectListItem { Text = dr["NAME"].ToString(), Value = dr["WildLifeScheduleId"].ToString() });
            }

            ViewBag.WLSchedule_List = WLScheduleList;
            #endregion
            return View(model);
        }

        [HttpPost]
        public ActionResult Release(RescueModel model, HttpPostedFileBase[] ImageDataRelease, HttpPostedFileBase[] ImageDataReleaseStaff)
        {
            try
            {

                DataTable dtImg = new DataTable();
                dtImg.Columns.Add("ID");
                dtImg.Columns.Add("ActiveStatus");
                dtImg.Columns.Add("ImagePath");
                dtImg.Columns.Add("Type");
                if (ImageDataRelease != null && ImageDataReleaseStaff != null)
                {
                    foreach (HttpPostedFileBase file in ImageDataRelease)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {
                            string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                            string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                            //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                            file.SaveAs(_FileName);
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = "~/RescueDocument/" + fileName;
                            dr["Type"] = "Release";
                            dtImg.Rows.Add(dr);
                        }
                    }

                    foreach (HttpPostedFileBase file in ImageDataReleaseStaff)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {
                            string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                            string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                            //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                            file.SaveAs(_FileName);
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = "~/RescueDocument/" + fileName;
                            dr["Type"] = "ReleaseStaff";
                            dtImg.Rows.Add(dr);
                        }
                    }

                    if (dtImg.Rows.Count > 0)
                        model.Images = dtImg;
                }
                else
                {
                    ModelState.AddModelError("ReleasePhotoPath", "Upload Picture is Required");
                }

                if (string.IsNullOrWhiteSpace(model.ReleaseRemarks))
                {
                    ModelState.AddModelError("ReleaseRemarks", "Remarks is Required");
                }
                //if (ModelState.IsValid)
                //{
                if (!model.AnimalNeedTreatment)
                {
                    model.HospitalName = string.Empty;
                    model.HospitalAddress = string.Empty;
                }
                model.RescueStatus = "Released";
                Entity.ResponseMsg msg = model.updateRegistrationRelease(model);
                if (!msg.IsError)
                {

                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, "Rescue");
                    if (DT != null && DT.Rows.Count > 0)
                    {
                        objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue Release", msg.ReturnIDs, Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                    }
                    #endregion
                    TempData["ReturnMsg"] = msg.ReturnMsg;
                    TempData["IsError"] = msg.IsError;
                    return RedirectToAction("Index", "Rescue", new { RoleName = TempData["EncRoleRelease"] });
                }
                //}
                List<SelectListItem> lstDistricts = new List<SelectListItem>();
                List<SelectListItem> lstBlocks = new List<SelectListItem>();
                List<SelectListItem> lstGPs = new List<SelectListItem>();
                List<SelectListItem> lstVillages = new List<SelectListItem>();
                List<SelectListItem> lstAnimals = new List<SelectListItem>();
                List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
                List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
                List<SelectListItem> lstCity = new List<SelectListItem>();
                List<SelectListItem> lstWard = new List<SelectListItem>();
                foreach (System.Data.DataRow dr in _objLocation.District().Rows)
                {
                    lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
                }
                ViewBag.Districts = lstDistricts;
                foreach (System.Data.DataRow dr in _objLocation.BindBlockName(lstDistricts[0].Value).Rows)
                {
                    lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
                }
                ViewBag.Blocks = lstBlocks;
                foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(lstDistricts[0].Value, lstBlocks[1].Value).Rows)
                {
                    lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
                }
                ViewBag.GPs = lstGPs;
                foreach (System.Data.DataRow dr in _objLocation.BindVillageName(lstDistricts[0].Value, lstBlocks[1].Value, lstGPs[1].Value).Rows)
                {
                    lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
                }
                ViewBag.Villages = lstVillages;
                foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
                {
                    lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
                }
                ViewBag.City = lstCity;
                foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
                {
                    lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
                }
                ViewBag.Ward = lstWard;
                foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
                {
                    lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
                }
                ViewBag.Animals = lstAnimals;
                foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
                {
                    lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.MediAssistTypes = lstMediAssistType;
                foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
                {
                    lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.CasualtyTypes = lstCasualtyType;
                ViewBag.Roles = GetRoleNames();
                ViewBag.ErrorMsg = "Something went wrong.Please try again.";
                //return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }
            return View(model);
        }

        public ActionResult Officer(int? registrationID)
        {
            ViewBag.CitizenRoles = TempData["encRoleNameIndex"];
            TempData["EncRoleClose"] = TempData["encRoleNameIndex"];
            RescueModel model = new RescueModel();
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(model.DistrictID).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(model.DistrictID, model.BlockID).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(model.DistrictID, model.BlockID, model.GPID).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;
            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Roles = GetRoleNames();
            return View(model);
        }

        [HttpPost]
        public ActionResult Officer(RescueModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RescueOfficerDesig))
            {
                ModelState.AddModelError("RescueOfficerDesig", "Officer Desognation is Required");
            }
            if (string.IsNullOrWhiteSpace(model.RescueOfficerName))
            {
                ModelState.AddModelError("RescueOfficerName", "Officer Name is Required");
            }
            if (ModelState.IsValid || 1 == 1)
            {
                model.RescueStatus = "Closed";
                Entity.ResponseMsg msg = model.updateRegistrationOfficer(model);
                #region Email and SMS
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, "Rescue");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue", msg.ReturnIDs, Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion
                return RedirectToAction("Index", "Rescue", new { RoleName = TempData["EncRoleClose"] });
            }
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(lstDistricts[0].Value).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(lstDistricts[0].Value, lstBlocks[1].Value).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(lstDistricts[0].Value, lstBlocks[1].Value, lstGPs[1].Value).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;
            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Roles = GetRoleNames();
            return View(model);
        }

        //View detials from list 
        public ActionResult View(int? registrationID, string RoleName = "Q2l0aXplbg==")
        {
            RescueModel model = new RescueModel();
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstChildAnimals = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            TempData["encRoleNameView"] = TempData["encRoleNameIndex"];
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(model.DistrictID).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(model.DistrictID, model.BlockID).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(model.DistrictID, model.BlockID, model.GPID).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;

            foreach (System.Data.DataRow dr in _objAnimal.BindChildAnimalName(model.AnimalID).Rows)
            {
                lstChildAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.ChildAnimals = lstChildAnimals;


            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;

            ViewBag.Roles = GetRoleNames();
            //ViewBag.CitizenRoles = RoleName;
            ViewBag.CitizenRoles = TempData["encRoleNameIndex"];
            return View(model);
        }

        /// <summary>
        /// For Bind the Block
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBlockName(string District)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                //if (Session["UserID"] != null)
                //{
                //UserID = Convert.ToInt64(Session["UserID"].ToString());
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = new Repository.CommonRepository().GetDropdownData3(18, District);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"));
                //}
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        /// Bind Child Animal Based On Parent Animal
        /// </summary>
        /// <param name="parentId">Parent Animal Name</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetChildAnimalByParentAnimalId(string parentId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                List<SelectListItem> lstData = new List<SelectListItem>();
                foreach (System.Data.DataRow dr in new RescueModel().GetDropdownData(2, parentId).Tables[0].Rows)
                {
                    lstData.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
                }
                return Json(new SelectList(lstData, "Value", "Text"));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        [HttpPost]
        public JsonResult BindCityName(string District)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                //if (Session["UserID"] != null)
                //{
                //UserID = Convert.ToInt64(Session["UserID"].ToString());
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = _objLocation.BindCityName(District);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["CITY_NAME"].ToString(), Value = @dr["CITY_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"));
                //}
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        [HttpPost]
        public JsonResult BindWardName(string District)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                //if (Session["UserID"] != null)
                //{
                //UserID = Convert.ToInt64(Session["UserID"].ToString());
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = _objLocation.BindWardName(District);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["WARD_NAME"].ToString(), Value = @dr["WARD_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"));
                //}
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        ///  For Bind the GPanchyat Name
        /// </summary>
        /// <param name="district"></param>
        /// <param name="blockName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGramPName(string District, string BlockName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                DataTable dt = new Repository.CommonRepository().GetDropdownData3(19, BlockName);

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// For Bind the Village Name
        /// 
        /// </summary>
        /// <param name="district"></param>
        /// <param name="blockName"></param>
        /// <param name="gpName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetVillageName(string District, string BlockName, string GPName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                DataTable dt = new Repository.CommonRepository().GetDropdownData3(20, GPName);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// For Bind the Block
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBlockNames(string District)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = _objLocation.BindBlockName(District);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        [HttpGet]
        public JsonResult BindCityNames(string District)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = _objLocation.BindCityName(District);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["CITY_NAME"].ToString(), Value = @dr["CITY_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        [HttpGet]
        public JsonResult BindWardNames(string District)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = _objLocation.BindWardName(District);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["WARD_NAME"].ToString(), Value = @dr["WARD_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        ///  For Bind the GPanchyat Name
        /// </summary>
        /// <param name="district"></param>
        /// <param name="blockName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetGramPNames(string District, string BlockName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                DataTable dt = _objLocation.BindGramPanchayatName(District, BlockName);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// For Bind the Village Name
        /// 
        /// </summary>
        /// <param name="district"></param>
        /// <param name="blockName"></param>
        /// <param name="gpName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetVillageNames(string District, string BlockName, string GPName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                DataTable dt = _objLocation.BindVillageName(District, BlockName, GPName);
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                }
                return Json(new SelectList(items, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        public ActionResult Rescuedatalist(string RecordStatus)
        {
            List<SelectListItem> lstVillage = new List<SelectListItem>();
            ViewBag.Village = lstVillage;
            ViewBag.RecordStatus = RecordStatus;
            RescueData oDataRescue = new Models.Rescue.RescueData { Action = "SELECT" };
            var oRescue = _objLocation.CrudOperationRescueData(oDataRescue);

            var List = (from DataRow dr in oRescue.Rows

                        select new RescueData()
                        {
                            Index = Convert.ToString(dr["Srno"]),
                            RescueId = Convert.ToInt16(dr["RescueId"]),
                            RescueDateTime = Convert.ToString(dr["RescueDate"].ToString()),
                            AnimalName = dr["AnimalName"].ToString(),
                            DistrictName = dr["DIST_NAME"].ToString(),
                            VillageName = dr["VILL_NAME"].ToString(),
                            Lat = dr["Lat"].ToString(),
                            Long = dr["Long"].ToString(),
                            Status = dr["Status"].ToString(),
                            Processing = dr["ProcessingStatus"].ToString(),
                            PostMortemReportPath = dr["PostMortemReportPath"].ToString(),
                            FactualReportPath = dr["FactualReportPath"].ToString()
                        }).ToList();
            return View(List);
        }

        public ActionResult Rescuedata(string Id)
        {

            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstStatus = new List<SelectListItem>();
            List<SelectListItem> lstProcessing = new List<SelectListItem>();
            List<SelectListItem> lstVillage = new List<SelectListItem>();

            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            ViewBag.Village = lstVillage;
            ViewBag.Districts = lstDistricts;
            //lstStatus.Add(new SelectListItem { Text = "Dead", Value = "1" });
            //lstStatus.Add(new SelectListItem { Text = "Rescue", Value = "2" });
            //ViewBag.Status = lstStatus;
            //lstProcessing.Add(new SelectListItem { Text = "Post mortem done", Value = "1" });
            //lstProcessing.Add(new SelectListItem { Text = "Released", Value = "2" });
            //lstProcessing.Add(new SelectListItem { Text = "Retained Captivity", Value = "3" });
            //ViewBag.Processing = lstProcessing;

            RescueData RD = new RescueData { RescueId = Convert.ToInt32(Id), Action = "SELECTONE" };

            var oObj = _objLocation.CrudOperationRescueData(RD);

            RescueData List = new RescueData();

            string dis = string.Empty;
            if (oObj.Rows.Count > 0)
            {
                dis = oObj.Rows[0]["DistrictId"].ToString();

                foreach (System.Data.DataRow dr in _objLocation.BindVillageName(dis).Rows)
                {
                    lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
                }
                ViewBag.Village = lstVillages;
                List = (from DataRow dr in oObj.Rows
                        select new RescueData()
                        {
                            RescueId = Convert.ToInt16(dr["RescueId"]),
                            RescueDateTime = Convert.ToString(dr["RescueDate"].ToString()),
                            AnimalName = dr["AnimalName"].ToString(),
                            DistrictId = dr["DistrictId"].ToString(),
                            VillageId = dr["VillageID"].ToString(),
                            Lat = dr["Lat"].ToString(),
                            Long = dr["Long"].ToString(),
                            Status = dr["Status"].ToString(),
                            Processing = dr["ProcessingStatus"].ToString(),
                            Remarks = dr["Remarks"].ToString()

                        }).FirstOrDefault();
            }

            return View(List);
        }

        public JsonResult GetVillage(string DistrictId)
        {
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(DistrictId).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            //ViewBag.Villages = lstVillages;

            return Json(lstVillages, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitRescue(RescueData model)
        {
            HttpPostedFileBase file = Request.Files["ImageUploadFactualReport"];
            HttpPostedFileBase file1 = Request.Files["ImageUploadPostMortemReport"];
            if (!string.IsNullOrWhiteSpace(file.FileName))
            {
                string fileName = "FactualReport_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                model.FactualReportPath = "~/RescueDocument/" + fileName;
                file.SaveAs(_FileName);
            }
            else
            {
                model.FactualReportPath = string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(file1.FileName))
            {
                string fileName = "PostMortemReport_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file1.FileName);
                string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                model.PostMortemReportPath = "~/RescueDocument/" + fileName;
                file.SaveAs(_FileName);
            }
            else
            {
                model.PostMortemReportPath = string.Empty;
            }
            if (model.RescueId == 0)
            {
                model.Action = "Insert";
            }
            else
            {
                model.Action = "Update";
            }
            _objLocation.CrudOperationRescueData(model);

            return RedirectToAction("Rescuedatalist", new RouteValueDictionary(new { controller = "Rescue", action = "Rescuedatalist", RecordStatus = 1 }));


        }

        #region NGO Operation
        public ActionResult NGOIndex()
        {

            NGOModel ngoModel = new NGOModel();
            List<NGOModel> lstNgoModel = ngoModel.GetNGOList();
            return View(lstNgoModel);
        }

        [HttpGet]
        public JsonResult NGOIndexList()
        {

            NGOModel ngoModel = new NGOModel();
            List<NGOModel> lstNgoModel = ngoModel.GetNGOList();
            return Json(lstNgoModel, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCircelAll()
        {
            
            List<SelectListItem> lstCircel = new List<SelectListItem>();
            try
            {
                foreach (System.Data.DataRow dr in _objLocation.BindCircle().Rows)
                {
                    lstCircel.Add(new SelectListItem { Text = dr["CIRCLE_NAME"].ToString(), Value = dr["CIRCLE_CODE"].ToString() });
                }
            }
            catch (Exception ex)
            {
                lstCircel = new List<SelectListItem>();
            }

            
            return Json(lstCircel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNGO(string regNumber = "")
        {
            NGOModel ngModel = new NGOModel();
            NGOModel ngoModel = new NGOModel();
            List<SelectListItem> lstData = new List<SelectListItem>();

            Location location = new Location();
            DataTable dt = location.BindCircle();
            ViewBag.fname = dt;
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            Range = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                Range.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
            }
            ViewBag.CCODE = Range;
            lstDistricts = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;

            lstData = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in new RescueModel().GetDropdownData(1).Tables[0].Rows)
            {
                lstData.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstData;

            if (!string.IsNullOrEmpty(regNumber))
            {
                ngoModel = ngModel.GetNGODetails(regNumber);

                List<SelectListItem> lstDivision = new List<SelectListItem>();
                DataTable dtCircle = location.BindDivision(ngoModel.Circle_Code);
                ViewBag.fname = dtCircle;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    lstDivision.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                }
                ViewBag.DivisionCode = lstDivision;

                DataTable dtRange = location.BindRangeBydivisionCode(ngoModel.Division_Code);
                ViewBag.fname = dtRange;
                List<SelectListItem> lstRangeCode = new List<SelectListItem>();
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    lstRangeCode.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.RangeCode = lstRangeCode;

                lstData = new List<SelectListItem>();
                foreach (System.Data.DataRow dr in new RescueModel().GetDropdownData(2, ngoModel.ChildAnimalID).Tables[0].Rows)
                {
                    lstData.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
                }
                ViewBag.ChildAnimals = lstData;
            }

            return View(ngoModel);
        }
        [HttpPost]
        public ActionResult AddNGO(NGOModel ngoModel, HttpPostedFileBase[] UploadNGODocuments, HttpPostedFileBase[] UploadNGOAffidavit)
        {
            DataTable dtImg = new DataTable();
            dtImg.Columns.Add("ID");
            dtImg.Columns.Add("ActiveStatus");
            dtImg.Columns.Add("ImagePath");
            dtImg.Columns.Add("Type");
            if (UploadNGODocuments != null)
            {
                foreach (HttpPostedFileBase file in UploadNGODocuments)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string fileName = "NGODocument_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                        string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                        file.SaveAs(_FileName);
                        DataRow dr = dtImg.NewRow();
                        dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        dr["Type"] = "NGODocument";
                        dtImg.Rows.Add(dr);
                    }
                }
            }
            if (UploadNGOAffidavit != null)
            {
                foreach (HttpPostedFileBase file in UploadNGOAffidavit)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string fileName = "NGOAffidavit_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                        string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                        file.SaveAs(_FileName);
                        DataRow dr = dtImg.NewRow();
                        dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        dr["Type"] = "NGOAffidavit";
                        dtImg.Rows.Add(dr);
                    }
                }

                if (dtImg.Rows.Count > 0)
                    ngoModel.Images = dtImg;
            }

            ngoModel.UsedFor = "rescue";
            if (!string.IsNullOrEmpty(Convert.ToString(Session["SSOid"])))
                ngoModel.InsertedBy = Convert.ToString(Session["SSOid"]);

            Entity.ResponseMsg msg = null;

            if (string.IsNullOrEmpty(ngoModel.RegNumber))
            {
                msg = ngoModel.InsertUpdateNGODetails(ngoModel);
                TempData["ReturnMsg"] = msg.ReturnMsg;

            }
            else
            {
                msg = ngoModel.InsertUpdateNGODetails(ngoModel, "Update");
                TempData["ReturnMsg"] = msg.ReturnMsg;

            }
            try
            {
                #region Email and SMS 
                string SMSModuleName = "Rescue NGO Registration";
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, "Rescue NGO Registration");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", SMSModuleName, msg.ReturnIDs, msg.ReturnIDs, Convert.ToString(DT.Rows[0]["emailID"]), Convert.ToString(DT.Rows[0]["Mobile"]), Convert.ToString(12));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion 

            }
            catch { }
            TempData["IsError"] = false;
            return RedirectToAction("NGOIndex");
        }

        public ActionResult RejectByNGO(string ObjectID)
        {
            Entity.ResponseMsg msg = new RescueModel().UpdateRegistrationByNGO("2", ObjectID, Convert.ToString(Session["UserId"]));
            TempData["ReturnMsg"] = msg.ReturnMsg;
            TempData["IsError"] = msg.IsError;
            return RedirectToAction("Index", "Rescue", new { RoleName = TempData["EncRole"] });
        }

        public ActionResult DeActivateNGO(string ObjectID)
        {
            Entity.ResponseMsg msg = new NGOModel().UpdateVendorDetails(ObjectID, "DeActivateNGO", Convert.ToString(Session["UserId"]));
            TempData["ReturnMsg"] = msg.ReturnMsg;
            TempData["IsError"] = msg.IsError;
            try
            {
                #region Email and SMS 
                string SMSModuleName = "NGO DeActivate";
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, "NGO DeActivate");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", SMSModuleName, msg.ReturnIDs, msg.ReturnIDs, "", "", Convert.ToString(12));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion 

            }
            catch { }
            return RedirectToAction("NGOIndex");
        }

        [HttpGet]
        public JsonResult AddNGOMobile(string regNumber = "")
        {
            try
            {
                NGOModel ngoModel = new NGOModel();
                
                //string str = Newtonsoft.Json.JsonConvert.SerializeObject(ngoModel);

                dynamic oObj=null;

                if (!string.IsNullOrEmpty(regNumber))
                {
                    oObj = new NGOModel().GetNGODetails(regNumber);
                }
                // var result = JsonConvert.SerializeObject(oObj, Formatting.Indented,
                //      new JsonSerializerSettings
                 //     {
                 //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                  //    });

              

                string jsonData = JsonConvert.SerializeObject(oObj);
                
               
                JavaScriptSerializer j = new JavaScriptSerializer();
                object a = j.Deserialize(jsonData, typeof(object));


                return Json(a, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception ex)
            {
                return Json(new { ReturnMsg = ex.Message, IsError = true }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult AddNGOMobile_ByRangCode(string RangCode = "")
        {
            try
            {
                NGOModel ngoModel = new NGOModel();

                //string str = Newtonsoft.Json.JsonConvert.SerializeObject(ngoModel);

                dynamic oObj = null;

                if (!string.IsNullOrEmpty(RangCode))
                {
                    oObj = new NGOModel().GetNGODetails_RangCode(RangCode);
                }
                // var result = JsonConvert.SerializeObject(oObj, Formatting.Indented,
                //      new JsonSerializerSettings
                //     {
                //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //    });



                string jsonData = JsonConvert.SerializeObject(oObj);


                JavaScriptSerializer j = new JavaScriptSerializer();
                object a = j.Deserialize(jsonData, typeof(object));


                return Json(a, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { ReturnMsg = ex.Message, IsError = true }, JsonRequestBehavior.AllowGet);
            }

        }

        ///Current NGO registration and update

        [HttpPost]
        public JsonResult AddNGOMobile(NGOModel ngoModel)
        {
            
            DataTable dtImg = new DataTable("Tbl_RescueImg");
            dtImg.Columns.Add("ID");
            dtImg.Columns.Add("ActiveStatus");
            dtImg.Columns.Add("ImagePath");
            dtImg.Columns.Add("Type");
            if (ngoModel.UploadNGODocuments != null)
            {
                foreach (RescuePhoto file in ngoModel.UploadNGODocuments)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string fileName = "";
                        string _FileName = "";
                        DataRow dr = dtImg.NewRow();
                        if (file.Base64string != null && file.Base64string.Trim() != "" && file.Id==0 )
                        {
                            fileName = "NGODocument_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                            _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                            SaveByteArrayAsImage(_FileName, file.Base64string);
                            dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        }
                        else if (file.Base64string != null && file.Base64string.Trim() != "" && file.Id > 0 )
                        {
                            dr["ImagePath"] = file.Base64string;
                        }
                       
                        //file.SaveAs(_FileName);
                        dr["Type"] = "NGODocument";
                        dr["ID"] = file.Id;
                        dr["ActiveStatus"] = 1;
                        dtImg.Rows.Add(dr);
                    }
                }
            }
            if (ngoModel.UploadNGOAffidavit != null)
            {
                foreach (RescuePhoto file in ngoModel.UploadNGOAffidavit)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string fileName = "";
                        string _FileName = "";
                        DataRow dr = dtImg.NewRow();
                        if (file.Base64string != null && file.Base64string.Trim() != "" && file.Id == 0)
                        {
                             fileName = "NGOAffidavit_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                             _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                            SaveByteArrayAsImage(_FileName, file.Base64string);
                            dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        }
                        else if (file.Base64string != null && file.Base64string.Trim() != "" && file.Id > 0)
                        {
                            dr["ImagePath"] = file.Base64string;
                        }
                        //file.SaveAs(_FileName);

                       
                        dr["Type"] = "NGOAffidavit";
                        dr["ID"] = file.Id;
                        dr["ActiveStatus"] = 1;
                        dtImg.Rows.Add(dr);
                    }
                }

                if (dtImg.Rows.Count > 0)
                    ngoModel.Images = dtImg;
            }

            ngoModel.UsedFor = "rescue";
            ngoModel.InsertedBy = Convert.ToString(ngoModel.VendorSSOId);
            Entity.ResponseMsg msg = null;

            if (string.IsNullOrEmpty(ngoModel.RegNumber))
            {
                msg = ngoModel.InsertUpdateNGODetails(ngoModel);
            }
            else
            {
                msg = ngoModel.InsertUpdateNGODetails(ngoModel, "Update");
            }
            try
            {
                #region Email and SMS 
                string SMSModuleName = "Rescue NGO Registration";
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, "Rescue NGO Registration");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", SMSModuleName, msg.ReturnIDs, msg.ReturnIDs, Convert.ToString(DT.Rows[0]["emailID"]), Convert.ToString(DT.Rows[0]["Mobile"]), Convert.ToString(12));
                }
                #endregion 

            }
            catch { }
            return Json(new { IsError = false, ReturnMsg = msg.ReturnMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DivisionData(string circleCode)
        {
            Location location = new Location();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(circleCode)))
                {
                    DataTable dt = location.BindDivision(circleCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }
                    ViewBag.DivisionCode = items;
                }



            }
            catch (Exception ex)
            {
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        [HttpPost]
        public JsonResult DivisionBycircleCode(string circleCode)
        {

            List<SelectListItem> lstDivision = new List<SelectListItem>();
            try
            {
                foreach (System.Data.DataRow dr in _objLocation.BindDivision(circleCode).Rows)
                {
                    lstDivision.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                }
            }
            catch (Exception ex)
            {
                lstDivision = new List<SelectListItem>();
            }

            
            return Json(lstDivision, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RangeDataByDivision(string divisionCode)
        {

            List<SelectListItem> lstRange = new List<SelectListItem>();
            try
            {
                foreach (System.Data.DataRow dr in _objLocation.BindRangeBydivisionCode(divisionCode).Rows)
                {
                    lstRange.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                }
            }
            catch (Exception ex)
            {
                lstRange = new List<SelectListItem>();
            }


            return Json(lstRange, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RangeData(string divisionCode)
        {
            Location location = new Location();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(divisionCode)))
                {

                    DataTable dt = location.BindRangeBydivisionCode(divisionCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.RangeCode = items;
                }
            }
            catch (Exception ex)
            {
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public ActionResult AddNewRowForNGO(int currentRowIndex, long objectID)
        {
            NGOModel model = new NGOModel();
            StaffModel sm = new StaffModel();
            List<StaffModel> lst = new List<StaffModel>();
            lst.Add(sm);
            model.StaffList = lst;
            ViewBag.CurrentIndex = currentRowIndex;
            ViewBag.RowType = FMDSS.RowType.Rescue_Staff;
            return PartialView("_AddNewRow", model);
        }
        #endregion

        #region Mobile Application 
        public JsonResult DeActivateNGOByMobile(string objectID, string userID)
        {
            Entity.ResponseMsg msg = new NGOModel().UpdateVendorDetails(objectID, "DeActivateNGO", userID); 
            try
            {
                #region Email and SMS 
                string SMSModuleName = "NGO DeActivate";
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, "NGO DeActivate");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", SMSModuleName, msg.ReturnIDs, msg.ReturnIDs, "", "", Convert.ToString(12));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion 

            }
            catch { }
            return Json(new { IsError = msg.IsError, ReturnMsg = msg.ReturnMsg }, JsonRequestBehavior.AllowGet);
        } 



        public JsonResult IndexByMobile(string UserID, string RoleName)
        {
            RescueModel models = new RescueModel();
            List<RescueModel> list = new List<RescueModel>();
            list = models.getAllRegistrations(RoleName, Convert.ToInt64(UserID));
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IndexByMobileAndsearch(string UserID, int startRec, int pageSize, string Search)
        {
            RescueModel models = new RescueModel();
            List<RescueModel> list = new List<RescueModel>();
            int NextFetch = pageSize;
            if (startRec > 0)
            {
                NextFetch = ((startRec / pageSize) + 1) * pageSize;
            }

            list = models.getAllRegistrationsBySearch(Convert.ToInt64(UserID), startRec+1, NextFetch, Search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RescueDetails_Mobile_search(string UserID, int startRec, int pageSize, string Search, string Action, string actionCode = "1")
        {
            RescueModel models = new RescueModel();
            List<RescueModel> list = new List<RescueModel>();
            int NextFetch = pageSize;
            if (startRec > 0)
            {
                NextFetch = ((startRec / pageSize) + 1) * pageSize;
            }

            list = models.GetRescueDetails_Mobile_BySerach(actionCode, Action, Convert.ToInt64(UserID), startRec+1, NextFetch, Search);
            //list = models.getAllRegistrationsBySearch(Convert.ToInt64(UserID), startRec + 1, NextFetch, Search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RejectByNGOMobile(string ObjectID, string UserID)
        {
            Entity.ResponseMsg msg = new RescueModel().UpdateRegistrationByNGO("2", ObjectID, UserID);
            return Json(new { IsError = msg.IsError, ReturnMsg = msg.ReturnMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RescueDetails_Mobile(string UserID, string Action, string actionCode = "1")
        {
            RescueModel models = new RescueModel();
            List<RescueModel> list = new List<RescueModel>();
            list = models.GetRescueDetails_Mobile(actionCode, Action, Convert.ToInt64(UserID));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateByMobile(RescueModel model)
        {
            model.ReportingTime = DateTime.Now;
                if (string.IsNullOrWhiteSpace(model.CitizenName))
            {
                ModelState.AddModelError("CitizenName", "Citizen Name is Required");
            }
            if (string.IsNullOrWhiteSpace(model.CitizenMobileNo))
            {
                ModelState.AddModelError("CitizenMobileNo", "Citizen Mobile No is Required");
            }
            if (string.IsNullOrWhiteSpace(model.DistrictID) || model.DistrictID == "0")
            {
                ModelState.AddModelError("DistrictID", "District is Required");
            }
            if (model.Rural)
            {
                if (string.IsNullOrWhiteSpace(model.BlockID) || model.BlockID == "0")
                {
                    ModelState.AddModelError("BlockID", "Block is Required");
                }
                if (string.IsNullOrWhiteSpace(model.GPID) || model.GPID == "0")
                {
                    ModelState.AddModelError("GPID", "Gram Panchayat is Required");
                }
                if (string.IsNullOrWhiteSpace(model.VillageID) || model.VillageID == "0")
                {
                    ModelState.AddModelError("VillageID", "Village is Required");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.CityID) || model.CityID == "0")
                {
                    ModelState.AddModelError("CityID", "City is Required");
                }
                if (string.IsNullOrWhiteSpace(model.WardID) || model.WardID == "0")
                {
                    ModelState.AddModelError("WardID", "Ward is Required");
                }
            }
            if (string.IsNullOrWhiteSpace(model.Location))
            {
                ModelState.AddModelError("Location", "Location is Required");
            }
            if (string.IsNullOrWhiteSpace(model.AnimalID) || model.AnimalID == "0")
            {
                ModelState.AddModelError("AnimalID", "Animal Name is Required");
            }

            DataTable dtImg = new DataTable();
            //dtImg.Columns.Add("ID");
            //dtImg.Columns.Add("ActiveStatus");
            dtImg.Columns.Add("ImagePath");
            dtImg.Columns.Add("Type");
            if (model.ImageDataRegistration != null)
            {
                foreach (RescuePhoto file in model.ImageDataRegistration)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        //string fileName = "Registration_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                        //string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        //model.RegistrationPhotoPath = SaveByteArrayAsImage(_FileName, file.Base64string);
                        //DataRow dr = dtImg.NewRow();
                        //dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        string fileName = string.Empty;
                        fileName = Upload_CattleGuardImagesstamp(file.FileName, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), model.Latitude, model.Longitude, file.Base64string, "Registration");
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = fileName;
                            dr["Type"] = "Registration";
                            dtImg.Rows.Add(dr);
                        }
                    }
                }
            }
            if (dtImg.Rows.Count > 0)
                model.Images = dtImg;


            model.RescuePhotoPath = string.Empty;
            if (!string.IsNullOrWhiteSpace(model.ReleasePhotoPath) && !string.IsNullOrEmpty(model.ReleasePhotoPath))
            {
                string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(model.ReleasePhotoPath) + "." + GetFileExtensionFromBase64(model.ReleasePhotoPath);
                string _FileName = Server.MapPath("/RescueDocument/") + fileName;
                model.ReleasePhotoPath = SaveByteArrayAsImage(_FileName, model.ReleasePhotoPath);
                model.ReleasePhotoPath = "/RescueDocument/" + fileName;
            }
            else
            {
                model.ReleasePhotoPath = string.Empty;
            }

            ModelState.Remove("ImageDataRegistration");
            if (ModelState.IsValid)
            {
                if (!model.Casualty)
                {
                    model.CasualtyType = string.Empty;
                    model.MediAssistRequired = false;
                }
                if (!model.MediAssistRequired)
                {
                    model.MediAssistType = string.Empty;
                    model.NoOfPersonInjured = "0";
                    model.CasualtyDescription = string.Empty;
                }
                model.RescueStatus = "New";
                Entity.ResponseMsg msg = model.createRegistration(model);
                if (!msg.IsError)
                {
                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, "Rescue");
                    if (DT != null && DT.Rows.Count > 0)
                    {
                        objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue", msg.ReturnIDs, Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                    }
                    #endregion
                }
                return Json(new { ObjectID = msg.ReturnIDs, Result = "Success", IsError = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsError = true, Result = "Fail Amrit" }, JsonRequestBehavior.AllowGet);
        }

        //private string SaveByteArrayAsImage(string fullOutputPath, string base64String)
        //{
            
        //    if (base64String.IndexOf(',') > 0)
        //    {
        //        string[] arr = base64String.Split(',');
        //        base64String = arr[1];
        //    }

        //    byte[] bytes = Convert.FromBase64String(base64String);
        //    System.IO.File.WriteAllBytes(fullOutputPath, bytes);
        //    return fullOutputPath;
        //}

        //public static string GetFileExtensionFromBase64(string base64String)
        //{
        //    var data = base64String.Substring(0, 30);


        //    if (data.ToUpper() == "IVBOR" || data.ToUpper().Contains("IVBOR"))
        //        return "png";
        //    else if (data.ToUpper() == "/9J/4" || data.ToUpper().Contains("/9J/4"))
        //    {
        //        return "jpg";
        //    }

        //    else
        //    {
        //        return string.Empty;
        //    }
        //    //switch (data.ToUpper())
        //    //{
        //    //    case "IVBOR":
        //    //        return "png";
        //    //    case "/9J/4":
        //    //        return "jpg";
        //    //    case "AAAAF":
        //    //        return "mp4";
        //    //    case "JVBER":
        //    //        return "pdf";
        //    //    case "AAABA":
        //    //        return "ico";
        //    //    case "UMFYI":
        //    //        return "rar";
        //    //    case "E1XYD":
        //    //        return "rtf";
        //    //    case "U1PKC":
        //    //        return "txt";
        //    //    case "MQOWM":
        //    //    case "77U/M":
        //    //        return "srt";
        //    //    default:
        //    //        return string.Empty;
        //    //}
        //}
        public JsonResult ApproveByMobile(int? registrationID)
        {
            RescueModel model = new RescueModel();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveByMobile(RescueModel model, string button)
        {
            if (ModelState.IsValid)
            {
                if (model.RescueStatus == "1")
                {
                    model.RegistrationApproved = true;
                    model.RescueStatus = "Approved";
                }
                else
                {
                    model.RegistrationApproved = false;
                    model.RescueStatus = "Rejected";
                }
                model.updateRegistrationApprove(model);
                #region Email and SMS
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RegistrationID.ToString(), "Rescue");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue", model.RegistrationID.ToString(), Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion
                return Json(new { Result = "Success" });
            }
            return Json(new { Result = "Fail" });
        }


        public JsonResult AssignByMobile(int? registrationID)
        {
            RescueModel model = new RescueModel();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AssignByMobile(RescueModel model)
        {
            try
            {
                if (model.UserID > 0)
                {
                    dbContext = new FmdssContext();
                    tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == model.UserID);
                    if (tblUserProfile != null)
                    {
                        model.RescueOfficerName = tblUserProfile.Ssoid;
                        model.RescueOfficerDesig = tblUserProfile.Designation;
                    }
                }


                if (ModelState.IsValid)
                {
                    model.RescueStatus = "Assigned";
                    Entity.ResponseMsg msg = model.UpdateRegistrationAssigned(model);
                    if (!msg.IsError)
                    {
                        #region Email and SMS
                        string SMSModuleName = string.Empty;
                        string Status = string.Empty;
                        if (model.RescueStatus == "Assigned")
                            SMSModuleName = "Rescue Assign";

                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, SMSModuleName);
                        if (DT != null && DT.Rows.Count > 0)
                        {
                            objSMSandEMAILtemplate.SendMailComman("ALL", SMSModuleName, msg.ReturnIDs, model.RegistrationID.ToString(), Convert.ToString(DT.Rows[0]["emailID"]), Convert.ToString(DT.Rows[0]["Mobile"]), Convert.ToString(Status));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                        }
                        return Json(new { ObjectID = msg.ReturnIDs, Result = "Success", Message = "Success" });
                        #endregion
                    }
                    else
                    {
                        return Json(new { Result = "Failed", Message = "Failed" });
                    }

                }
                return Json(new { Result = "Failed", Message = "ModelState Invalid" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Fail", Message = ex.Message });
            }

        }
        [HttpGet]
        public JsonResult CaptureByMobile(int? registrationID)
        {
            RescueModel model = new RescueModel();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CaptureByMobile(RescueModel model)
        {
            //HttpPostedFileBase file = Request.Files["ImageDataCapture"];


            DataTable dtImg = new DataTable();
            //dtImg.Columns.Add("ID");
            //dtImg.Columns.Add("ActiveStatus");
            dtImg.Columns.Add("ImagePath");
            dtImg.Columns.Add("Type");
            if (model.ImageDataCapturesubmit != null && model.ImageDataCaptureStaff != null)
            {
                foreach (RescuePhoto file in model.ImageDataCapturesubmit)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        //string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                        //string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        ////model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                        //model.RescuePhotoPath = SaveByteArrayAsImage(_FileName, file.Base64string);
                        //DataRow dr = dtImg.NewRow();
                        //dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        //dr["Type"] = "Capture";
                        //dtImg.Rows.Add(dr);


                        //string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                        //string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                        //model.RescuePhotoPath = SaveByteArrayAsImage(_FileName, file.Base64string);
                        string fileName = string.Empty;
                        fileName = Upload_CattleGuardImagesstamp(file.FileName, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), model.Latitude, model.Longitude, file.Base64string, "Capture");
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = fileName;
                            //dr["ImagePath"] = "~/RescueDocument/" + fileName;
                            dr["Type"] = "Capture";
                            dtImg.Rows.Add(dr);
                        }
                    }
                }

                foreach (RescuePhoto file in model.ImageDataCaptureStaff)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        //string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                        //string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        ////model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                        //model.RescuePhotoPath = SaveByteArrayAsImage(_FileName, file.Base64string);
                        //DataRow dr = dtImg.NewRow();
                        //dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        //dr["Type"] = "CaptureStaff";
                        //dtImg.Rows.Add(dr);

                        string fileName = string.Empty;
                        //string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                        //string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        //model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                        //model.RescuePhotoPath = SaveByteArrayAsImage(_FileName, file.Base64string);
                        
                        fileName = Upload_CattleGuardImagesstamp(file.FileName, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), model.Latitude, model.Longitude, file.Base64string, "CaptureStaff");
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = fileName;
                            //dr["ImagePath"] = "~/RescueDocument/" + fileName;
                            dr["Type"] = "CaptureStaff";
                            dtImg.Rows.Add(dr);
                        }
                    }
                }

                if (dtImg.Rows.Count > 0)
                    model.Images = dtImg;
            }
            else
            {
                ModelState.AddModelError("RescuePhotoPath", "Upload Picture is Required");
            }

            //if (!string.IsNullOrWhiteSpace(model.RescuePhotoPath))
            //{
            //    string fileName = "Rescue_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(model.RescuePhotoPath) + "." + GetFileExtensionFromBase64(model.RescuePhotoPath);
            //    string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
            //    model.RescuePhotoPath = SaveByteArrayAsImage(_FileName, model.RescuePhotoPath);
            //    //model.RescuePhotoPath = System.Configuration.ConfigurationManager.AppSettings["SitePath"] + "/RescueDocument/" + fileName;
            //    model.RescuePhotoPath = "/RescueDocument/" + fileName;
            //}
            //else
            //{
            //    model.RescuePhotoPath = string.Empty;
            //}
            if (string.IsNullOrWhiteSpace(model.RescueRemarks))
            {
                ModelState.AddModelError("RescueRemarks", "Remarks is Required");
            }
            if (ModelState.IsValid)
            {
                if (model.AssistanceTypeFirstAidModel != null && model.AssistanceTypeFirstAidModel.Where(d => d.IsChecked == true).Count() > 0)
                {
                    model.AssistanceTypeFirstAid = string.Join(",", model.AssistanceTypeFirstAidModel.Where(d => d.IsChecked == true).Select(d => d.Name));
                }
                model.RescueStatus = "Captured";
                Entity.ResponseMsg msg = model.updateRegistrationCapture(model);
                #region Email and SMS
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RegistrationID.ToString(), "Rescue");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue", model.RegistrationID.ToString(), Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion
                return Json(new { ObjectID = msg.ReturnIDs, Result = "Success", Message = "Success" });
            }
            return Json(new { Result = "Fail" });
        }

        public JsonResult ReleaseByMobile(int? registrationID)
        {
            RescueModel model = new RescueModel();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReleaseByMobile(RescueModel model)
        {
            #region "Check Validation by amit on 09-09-2019"
            //if (string.IsNullOrEmpty(model.ForestStaffSSOID))
            //    return Json("Please enter ForestStaffSSOID", JsonRequestBehavior.AllowGet);
            if (string.IsNullOrEmpty(model.Gender))
                return Json("Please enter Gender", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.ActionTakenTime.ToString()))
            //    return Json("Please enter Action Taken Time", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.Age))
            //    return Json("Please enter Age", JsonRequestBehavior.AllowGet);
            else if (string.IsNullOrEmpty(model.AnimalDead.ToString()))
                return Json("Please enter animal Dead", JsonRequestBehavior.AllowGet);
            else if (string.IsNullOrEmpty(model.AnimalName))
                return Json("Please enter animal name", JsonRequestBehavior.AllowGet);
            else if (string.IsNullOrEmpty(model.AnimalNeedTreatment.ToString()))
                return Json("Please enter Animal Need Treatment", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.AnimalReleasedInto))
            //    return Json("Please enter Animal Released Into", JsonRequestBehavior.AllowGet);
            else if (string.IsNullOrEmpty(model.AnimalRescuedSuccessfully.ToString()))
                return Json("Please enter Animal Rescued Successfully", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.AnimalRescueTime.ToString()))
            //    return Json("Please enter Animal Rescue Time", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.AssistanceTypeFirstAid))
            //    return Json("Please enter Animal Rescue Time", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.Block))
            //    return Json("Please enter Block", JsonRequestBehavior.AllowGet);
            else if (string.IsNullOrEmpty(model.Casualty.ToString()))
                return Json("Please enter Casualty", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.CasualtyDescription))
            //    return Json("Please enter Casualty Description", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.CasualtyType))
            //    return Json("Please enter Casualty Type", JsonRequestBehavior.AllowGet);
            //else if (string.IsNullOrEmpty(model.ChildAnimalName))
            //    return Json("Please enter Child Animal Name", JsonRequestBehavior.AllowGet);
            else if (string.IsNullOrEmpty(model.CitizenEmailID))
                return Json("Please enter Child Animal Name", JsonRequestBehavior.AllowGet);
            else if (string.IsNullOrEmpty(model.CitizenMobileNo))
                return Json("Please enter CitizenMobileNo", JsonRequestBehavior.AllowGet);


            #endregion



            if (string.IsNullOrWhiteSpace(model.ReleaseRemarks))
            {
                ModelState.AddModelError("ReleaseRemarks", "Remarks is Required");
            }
            DataTable dtImg = new DataTable();
            //dtImg.Columns.Add("ID");
            //dtImg.Columns.Add("ActiveStatus");
            dtImg.Columns.Add("ImagePath");
            dtImg.Columns.Add("Type");
            if (model.ImageDataReleasesubmit != null && model.ImageDataReleaseStaff != null)
            {
                foreach (RescuePhoto file in model.ImageDataReleasesubmit)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        //string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                        //string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        //model.RescuePhotoPath = SaveByteArrayAsImage(_FileName, file.Base64string);
                        //DataRow dr = dtImg.NewRow();
                        //dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        string fileName = string.Empty;
                        fileName = Upload_CattleGuardImagesstamp(file.FileName, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), model.Latitude, model.Longitude, file.Base64string, "Release");
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = fileName;
                            dr["Type"] = "Release";
                            dtImg.Rows.Add(dr);
                        }
                    }
                }

                foreach (RescuePhoto file in model.ImageDataReleaseStaff)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        //string fileName = "Release_" + DateTime.Now.ToFileTime().ToString() + "_" + file.FileName + "." + file.Ext;
                        //string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
                        ////model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                        //model.RescuePhotoPath = SaveByteArrayAsImage(_FileName, file.Base64string);
                        //DataRow dr = dtImg.NewRow();
                        //dr["ImagePath"] = "~/RescueDocument/" + fileName;
                        string fileName = string.Empty;
                        fileName = Upload_CattleGuardImagesstamp(file.FileName, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), model.Latitude, model.Longitude, file.Base64string, "ReleaseStaff");
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            DataRow dr = dtImg.NewRow();
                            dr["ImagePath"] = fileName;
                            dr["Type"] = "ReleaseStaff";
                            dtImg.Rows.Add(dr);
                        }
                    }
                }

                if (dtImg.Rows.Count > 0)
                    model.Images = dtImg;
            }
            else
            {
                ModelState.AddModelError("ReleasePhotoPath", "Uoad Picture is Required");
            }

            if (ModelState.IsValid)
            {
                if (!model.AnimalNeedTreatment)
                {
                    model.HospitalName = string.Empty;
                    model.HospitalAddress = string.Empty;
                }
                model.RescueStatus = "Released";



                Entity.ResponseMsg msg = model.updateRegistrationRelease(model);
                #region Email and SMS
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RegistrationID.ToString(), "Rescue");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue", model.RegistrationID.ToString(), Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion
                return Json(new { ObjectID = msg.ReturnIDs, Result = "Success", Message = "Success" });
            }
            return Json(new { Result = "Fail" });
        }

        public JsonResult OfficerByMobile(int? registrationID)
        {
            RescueModel model = new RescueModel();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OfficerByMobile(RescueModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RescueOfficerDesig))
            {
                ModelState.AddModelError("RescueOfficerDesig", "Officer Desognation is Required");
            }
            if (string.IsNullOrWhiteSpace(model.RescueOfficerName))
            {
                ModelState.AddModelError("RescueOfficerName", "Officer Name is Required");
            }
            if (ModelState.IsValid)
            {
                model.RescueStatus = "Closed";
                model.updateRegistrationOfficer(model);
                #region Email and SMS
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                DataTable DT = objSMSandEMAILtemplate.GetUserDetails(model.RegistrationID.ToString(), "Rescue");
                if (DT != null && DT.Rows.Count > 0)
                {
                    objSMSandEMAILtemplate.SendMailComman("ALL", "Rescue", model.RegistrationID.ToString(), Convert.ToString(DT.Rows[0]["ApplicantName"]), Convert.ToString(DT.Rows[0]["ApplicantEmail"]), Convert.ToString(DT.Rows[0]["ApplicantMobile"]), Convert.ToString(DT.Rows[0]["Status"]));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                }
                #endregion
                return Json(new { Result = "Success" });
            }
            return Json(new { Result = "Fail" });
        }

        public JsonResult ViewByMobile(int? registrationID)
        {
            RescueModel model = new RescueModel();
            model = model.getRegistrationByID(registrationID.GetValueOrDefault());
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public string GetRoleNames()
        {
            string roles = string.Empty;
            List<ROLEGROUPS> OBJListROLE = new List<ROLEGROUPS>();
            OBJListROLE = (List<ROLEGROUPS>)Session["ROLEGROUPS"];
            if (OBJListROLE != null)
            {
                foreach (ROLEGROUPS grp in OBJListROLE)
                {
                    roles = roles + grp.RoleName + ",";
                }
            }
            return roles;
        }


        public JsonResult GetRoleNameByUserID(string SSOID)
        {
            UserRolesModelDetailsByRoles Model = new UserRolesModelDetailsByRoles();
            CommanRepo CommanRepo = new CommanRepo();
            try
            {
                Model = CommanRepo.GetUserRoleBySSOID(SSOID, "LIST");
            }
            catch (Exception ex)
            {
                Model = new UserRolesModelDetailsByRoles();
            }
            return Json(Model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DistrictList()
        {
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            try
            {
                foreach (System.Data.DataRow dr in _objLocation.DistrictBystatecode().Rows)
                {
                    lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
                }
            }
            catch (Exception ex)
            {
                lstDistricts = new List<SelectListItem>();
            }
            return Json(lstDistricts, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AnimalsList()
        {
            List<SelectListItem> lstAnimalsList = new List<SelectListItem>();
            try
            {
                foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
                {
                    lstAnimalsList.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
                }
            }
            catch (Exception ex)
            {
                lstAnimalsList = new List<SelectListItem>();
            }
            return Json(lstAnimalsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MediAssistType()
        {
            List<SelectListItem> MediAssistType = new List<SelectListItem>();
            try
            {
                foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
                {
                    MediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
            }
            catch (Exception ex)
            {
                MediAssistType = new List<SelectListItem>();
            }
            return Json(MediAssistType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CasualtyType()
        {
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            try
            {
                foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
                {
                    lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
            }
            catch (Exception ex)
            {
                lstCasualtyType = new List<SelectListItem>();
            }
            return Json(lstCasualtyType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Assigned(int? registrationID)
        {
            RescueModel model = new RescueModel();
            DataTable dtOfficerDesignation = new DataTable();
            List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
            dtOfficerDesignation = model.GetOfficerDesignationRescue();

            UserID = Convert.ToInt64(Session["UserId"]);
            if (UserID > 0)
            {
                dbContext = new FmdssContext();
                tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == UserID);
                if (tblUserProfile != null)
                {
                    model.CitizenName = tblUserProfile.Name;
                    model.CitizenMobileNo = tblUserProfile.Mobile;
                    model.CitizenEmailID = tblUserProfile.EmailId;
                }
            }


            if (dtOfficerDesignation.Rows.Count > 0)
            {

                foreach (System.Data.DataRow dr in dtOfficerDesignation.Rows)
                {
                    lstOfficerDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["EmpDesignation"].ToString() });
                }
                ViewBag.OfficerDesignation = lstOfficerDesignation;
            }
            else
            {
                lstOfficerDesignation.Add(new SelectListItem { Text = "NA", Value = "0" });
                ViewBag.OfficerDesignation = lstOfficerDesignation;
            }
            model.Rural = true;
            List<SelectListItem> lstDistricts = new List<SelectListItem>();
            List<SelectListItem> lstBlocks = new List<SelectListItem>();
            List<SelectListItem> lstGPs = new List<SelectListItem>();
            List<SelectListItem> lstVillages = new List<SelectListItem>();
            List<SelectListItem> lstAnimals = new List<SelectListItem>();
            List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
            List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            List<SelectListItem> lstWard = new List<SelectListItem>();
            List<SelectListItem> lstChildAnimals = new List<SelectListItem>();

            model = model.getRegistrationByID(registrationID.GetValueOrDefault());

            foreach (System.Data.DataRow dr in _objLocation.District().Rows)
            {
                lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = lstDistricts;
            foreach (System.Data.DataRow dr in _objLocation.BindBlockName(lstDistricts[0].Value).Rows)
            {
                lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
            }
            ViewBag.Blocks = lstBlocks;
            foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(lstDistricts[0].Value, lstBlocks[1].Value).Rows)
            {
                lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
            }
            ViewBag.GPs = lstGPs;
            foreach (System.Data.DataRow dr in _objLocation.BindVillageName(lstDistricts[0].Value, lstBlocks[1].Value, lstGPs[1].Value).Rows)
            {
                lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
            }
            ViewBag.Villages = lstVillages;

            #region Below code is commented by pooran on 01-08-2019 as city name and ward name are not being shwon in UI so not in use
            foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
            {
                lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
            }
            ViewBag.City = lstCity;
            foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
            {
                lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
            }
            ViewBag.Ward = lstWard;
            #endregion

            foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            {
                lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            }
            ViewBag.Animals = lstAnimals;


            //foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
            //{
            //    lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
            //}
            //ViewBag.ChildAnimals = lstChildAnimals;

            foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
            {
                lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.MediAssistTypes = lstMediAssistType;
            foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
            {
                lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
            }
            ViewBag.CasualtyTypes = lstCasualtyType;
            ViewBag.Roles = GetRoleNames();
            ViewBag.RoleName = Convert.ToString(Session["Role"]); ;
            //ViewBag.CitizenRoles = RoleName;

            return View(model);
        }

        [HttpPost]
        public ActionResult Assigned(RescueModel model, string buttonType, HttpPostedFileBase[] ImageDataCapture)
        {
            try
            {
                UserID = Convert.ToInt64(Session["UserId"]);
                if (UserID > 0)
                {
                    dbContext = new FmdssContext();
                    tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == UserID);
                    if (tblUserProfile != null)
                    {
                        model.RescueOfficerName = tblUserProfile.Ssoid;
                        model.RescueOfficerDesig = tblUserProfile.Designation;
                    }
                }

                if (buttonType == "Submit")
                {
                    //DataTable dtImg = new DataTable();
                    //dtImg.Columns.Add("ID");
                    //dtImg.Columns.Add("ActiveStatus");
                    //dtImg.Columns.Add("ImagePath");
                    //dtImg.Columns.Add("Type");
                    //if (ImageDataCapture != null)
                    //{
                    //    foreach (HttpPostedFileBase file in ImageDataCapture)
                    //    {
                    //        //Checking file is available to save.  
                    //        if (file != null)
                    //        {

                    //            string fileName = "Capture_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(file.FileName);
                    //            string _FileName = Server.MapPath("~/RescueDocument/") + fileName;//model.RegistrationPhotoPath = "~/RescueDocument/" + fileName;
                    //            file.SaveAs(_FileName);
                    //            DataRow dr = dtImg.NewRow();
                    //            dr["ImagePath"] = "~/RescueDocument/" + fileName;
                    //            dr["Type"] = "Capture";
                    //            dtImg.Rows.Add(dr);

                    //        }

                    //    }
                    //    if (dtImg.Rows.Count > 0)
                    //        model.Images = dtImg;
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("RescuePhotoPath", "Upload Picture is Required");
                    //}

                    if (string.IsNullOrWhiteSpace(model.RescueRemarks))
                    {
                        ModelState.AddModelError("RescueRemarks", "Remarks is Required");
                    }
                }
                if (ModelState.IsValid)
                {
                    if (buttonType == "Send TO CWLW Approval")
                    {
                        model.RescueStatus = "Send TO CWLW Approval";
                    }
                    else if (buttonType == "Send TO NGO")
                    {
                        model.RescueStatus = "Send TO NGO";
                    }
                    else
                    {
                        model.RescueStatus = "Assigned";

                    }

                    if (model.SendToNGOOrSelf == -1 || model.SendToNGOOrSelf == -2)
                    {
                        model.SendToOfficerSSOID = model.ForestStaffSSOID;
                    }
                    Entity.ResponseMsg msg = model.UpdateRegistrationAssigned(model);
                    if (!msg.IsError)
                    {
                        #region Email and SMS
                        string SMSModuleName = string.Empty;
                        string Status = string.Empty;
                        if (model.RescueStatus == "Captured")
                            SMSModuleName = "Rescue Capture";
                        else if (model.RescueStatus == "Send TO NGO")
                        {
                            SMSModuleName = "Send TO NGO Rescue";
                            Status = model.SendTONGORemarks;
                        }
                        else if (model.RescueStatus == "Send TO CWLW Approval")
                        {
                            SMSModuleName = "Send TO CWLW Rescue";
                            Status = model.WildlifeScheduleRemarks;
                        }
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        DataTable DT = objSMSandEMAILtemplate.GetUserDetails(msg.ReturnIDs, SMSModuleName);
                        if (DT != null && DT.Rows.Count > 0)
                        {
                            objSMSandEMAILtemplate.SendMailComman("ALL", SMSModuleName, msg.ReturnIDs, model.RegistrationID.ToString(), Convert.ToString(DT.Rows[0]["emailID"]), Convert.ToString(DT.Rows[0]["Mobile"]), Convert.ToString(Status));//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                        }
                        #endregion
                        TempData["ReturnMsg"] = msg.ReturnMsg;
                        TempData["IsError"] = msg.IsError;
                        return RedirectToAction("Index", "Rescue", new { RoleName = TempData["EncRole"] });
                    }
                }
                List<SelectListItem> lstDistricts = new List<SelectListItem>();
                List<SelectListItem> lstBlocks = new List<SelectListItem>();
                List<SelectListItem> lstGPs = new List<SelectListItem>();
                List<SelectListItem> lstVillages = new List<SelectListItem>();
                List<SelectListItem> lstAnimals = new List<SelectListItem>();
                List<SelectListItem> lstMediAssistType = new List<SelectListItem>();
                List<SelectListItem> lstCasualtyType = new List<SelectListItem>();
                List<SelectListItem> lstCity = new List<SelectListItem>();
                List<SelectListItem> lstWard = new List<SelectListItem>();
                List<SelectListItem> WLScheduleList = new List<SelectListItem>();
                foreach (System.Data.DataRow dr in _objLocation.District().Rows)
                {
                    lstDistricts.Add(new SelectListItem { Text = dr["DIST_NAME"].ToString(), Value = dr["DIST_CODE"].ToString() });
                }
                ViewBag.Districts = lstDistricts;
                foreach (System.Data.DataRow dr in _objLocation.BindBlockName(lstDistricts[0].Value).Rows)
                {
                    lstBlocks.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
                }
                ViewBag.Blocks = lstBlocks;
                foreach (System.Data.DataRow dr in _objLocation.BindGramPanchayatName(lstDistricts[0].Value, lstBlocks[1].Value).Rows)
                {
                    lstGPs.Add(new SelectListItem { Text = dr["GP_NAME"].ToString(), Value = dr["GP_CODE"].ToString() });
                }
                ViewBag.GPs = lstGPs;
                foreach (System.Data.DataRow dr in _objLocation.BindVillageName(lstDistricts[0].Value, lstBlocks[1].Value, lstGPs[1].Value).Rows)
                {
                    lstVillages.Add(new SelectListItem { Text = dr["VILL_NAME"].ToString(), Value = dr["VILL_CODE"].ToString() });
                }
                ViewBag.Villages = lstVillages;
                foreach (System.Data.DataRow dr in _objLocation.BindCityName(lstDistricts[0].Value).Rows)
                {
                    lstCity.Add(new SelectListItem { Text = dr["CITY_NAME"].ToString(), Value = dr["CITY_CODE"].ToString() });
                }
                ViewBag.City = lstCity;
                foreach (System.Data.DataRow dr in _objLocation.BindWardName(lstDistricts[0].Value).Rows)
                {
                    lstWard.Add(new SelectListItem { Text = dr["WARD_NAME"].ToString(), Value = dr["WARD_CODE"].ToString() });
                }
                ViewBag.Ward = lstWard;
                foreach (System.Data.DataRow dr in _objAnimal.BindAnimalName().Rows)
                {
                    lstAnimals.Add(new SelectListItem { Text = dr["Animal_NAME"].ToString(), Value = dr["Animal_CODE"].ToString() });
                }
                ViewBag.Animals = lstAnimals;
                foreach (System.Data.DataRow dr in _objAnimal.SelectMediAssistType().Rows)
                {
                    lstMediAssistType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.MediAssistTypes = lstMediAssistType;
                foreach (System.Data.DataRow dr in _objAnimal.SelectCasualtyType().Rows)
                {
                    lstCasualtyType.Add(new SelectListItem { Text = dr["Text"].ToString(), Value = dr["Value"].ToString() });
                }
                ViewBag.CasualtyTypes = lstCasualtyType;
                ViewBag.Roles = GetRoleNames();
                #region New Added
                DataTable dtWLPA = CordinatorManagement.GetWildlifeSchedule();
                foreach (System.Data.DataRow dr in dtWLPA.Rows)
                {
                    WLScheduleList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["WildLifeScheduleId"].ToString() });
                }
                ViewBag.WLSchedule_List = WLScheduleList;
                #endregion
                ViewBag.ErrorMsg = "Something went wrong.Please try again.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GETOfficerDesignationList(string SSOID)
        {
            RescueModel model = new RescueModel();
            List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
            try
            {
                #region Get Designation
                DataTable dtOfficerDesignation = new DataTable();

                dtOfficerDesignation = model.GetOfficerDesignationRescue(SSOID);
                if (dtOfficerDesignation.Rows.Count > 0)
                {

                    foreach (System.Data.DataRow dr in dtOfficerDesignation.Rows)
                    {
                        lstOfficerDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["EmpDesignation"].ToString() });
                    }
                    ViewBag.OfficerDesignation = lstOfficerDesignation;
                }
                else
                {
                    lstOfficerDesignation.Add(new SelectListItem { Text = "NA", Value = "0" });
                }
                #endregion
            }
            catch (Exception ex)
            {
                lstOfficerDesignation.Add(new SelectListItem { Text = "NA", Value = "0" });
            }
            return Json(lstOfficerDesignation, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWildlifeScheduleList()
        {
            RescueModel model = new RescueModel();
            List<SelectListItem> WLScheduleList = new List<SelectListItem>();
            try
            {
                #region New Added
                DataTable dtWLPA = CordinatorManagement.GetWildlifeSchedule();
                var SelectedValues = dtWLPA.AsEnumerable().Where(d => d.Field<int>("RescueCWLWApproveStatus") == 1).Select(s => s.Field<int>("WildLifeScheduleId")).ToArray();
                model.WLScheduleListCWLWApproval = string.Join(",", SelectedValues);

                foreach (System.Data.DataRow dr in dtWLPA.Rows)
                {
                    WLScheduleList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["WildLifeScheduleId"].ToString() });
                }

                #endregion
            }
            catch (Exception ex)
            {
                WLScheduleList.Add(new SelectListItem { Text = "NA", Value = "0" });
            }
            return Json(WLScheduleList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AssistanceTypeFirstAidList()
        {
            #region New Added
            List<AssistanceTypeFirstAidModel> model = new List<AssistanceTypeFirstAidModel>
            { new AssistanceTypeFirstAidModel { Name = "Energy Drink/Glucose", IsChecked = false },
            new AssistanceTypeFirstAidModel { Name = "PainKiller", IsChecked = false },
            new AssistanceTypeFirstAidModel { Name = "Antiseptic Spray", IsChecked = false },
            new AssistanceTypeFirstAidModel { Name = "Bandage (if possible on site)", IsChecked = false },
            new AssistanceTypeFirstAidModel { Name = "Other", IsChecked = false }};

            #endregion
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetURLPrefix()
        {
            return Json(new { URlPrefix = Globals.Util.GetAppSettings("websiteUrl") }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult testapi(string img)
        {
            return Json(new { data = "ok ji" }, JsonRequestBehavior.AllowGet);
        }
        public string Upload_CattleGuardImagesstamp(string FileNameUpload, string date, string latitude, string longitude, string imgFile, string ImgType)
        {
            var path = string.Empty;
            var tmpPhyPath = string.Empty;
            string FileName = string.Empty;
            string fileWithPhyPath = string.Empty;
            string FileFullName = string.Empty;
            if (!string.IsNullOrWhiteSpace(imgFile) && !string.IsNullOrEmpty(imgFile))
            {
                try
                {
                    string ext = GetFileExtensionFromBase64(imgFile);
                    if (ext == "" || ext == null)
                    {
                        path = imgFile;
                    }
                    else
                    {

                        var Saveimageurl = Server.MapPath("~/RescueDocument/");
                        //string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                        // string FilePath = System.Configuration.ConfigurationManager.AppSettings["ImgWaterDistributionPath"].ToString();

                        FileName = ImgType + "_" + DateTime.Now.ToFileTime().ToString() + "_" + FileNameUpload + "." + GetFileExtensionFromBase64(imgFile);
                        //var currentMapPath = System.Web.Hosting.HostingEnvironment.MapPath(FilePath);
                        fileWithPhyPath = Saveimageurl + FileName;

                        imgFile = imgFile.Replace("data:image/jpeg;base64,", "");

                        path = "~/RescueDocument/" + FileName;
                        string[] splArr = path.Split('/');
                        string Imaget = SaveByteArrayAsImage(fileWithPhyPath, Convert.ToString(imgFile));

                        DateTime dateTime = DateTime.Now;
                        // return Friday, August 05, 2016  
                        var shortTm = dateTime.ToLongTimeString();
                        var wm = date;
                        using (var bmp = Bitmap.FromFile(fileWithPhyPath))
                        using (var transparentBrush = new SolidBrush(Color.FromArgb(255, 0, 0)))
                        using (var g = Graphics.FromImage(bmp))
                        {
                            var font = new Font("Times New Roman", 30, FontStyle.Bold, GraphicsUnit.Pixel);
                            var dim = g.MeasureString(wm, font);
                            PointF loc1 = new PointF(50, bmp.Height - 150);
                            PointF loc2 = new PointF(50, bmp.Height - 100);
                            PointF loc3 = new PointF(50, bmp.Height - 50);
                            //PointF secondLocation = new PointF(10f, 50f);

                            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(fileWithPhyPath);//load the image file

                            using (Graphics graphics = Graphics.FromImage(bitmap))
                            {
                                using (Font arialFont = new Font("Arial", 28,FontStyle.Bold))
                                {
                                    graphics.DrawString("Latitude:" + latitude, arialFont, Brushes.Red, loc1);
                                    graphics.DrawString("Longitude:" + longitude, arialFont, Brushes.Red, loc2);
                                    graphics.DrawString("Date:" + wm, arialFont, Brushes.Red, loc3);
                                }
                            }
                            string[] phyPathArr = fileWithPhyPath.Split('\\');


                            g.Dispose();
                            tmpPhyPath = "";
                            string dbFilePathAndName = string.Empty;
                            for (int i = 0; i < splArr.Length - 1; i++)
                            {
                                if (i == 0)
                                    dbFilePathAndName = dbFilePathAndName + splArr[i];
                                else
                                    dbFilePathAndName = dbFilePathAndName + "/" + splArr[i];
                            }

                            for (int i = 0; i < phyPathArr.Length - 1; i++)
                            {
                                if (i == 0)
                                    tmpPhyPath = tmpPhyPath + phyPathArr[i];
                                else
                                    tmpPhyPath = tmpPhyPath + "/" + phyPathArr[i];
                            }

                            string filename = "New_" + splArr[splArr.Length - 1];
                            tmpPhyPath = tmpPhyPath + "/" + filename;
                            path = dbFilePathAndName + "/" + filename;
                            bitmap.Save(tmpPhyPath);
                            bitmap.Dispose();
                            bmp.Dispose();
                        }
                        System.IO.File.Delete(fileWithPhyPath);

                    }
                }
                catch (Exception ex)
                {
                    //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Upload_CattleGuard" + "_" + "UserAccountApi", 0, DateTime.Now, 0);
                    path = "";
                }

            }

            return path;
        }
        public static string GetFileExtensionFromBase64(string base64String)
        {
            var data = base64String.Substring(0, 39);


            if (data.ToUpper() == "IVBOR" || data.ToUpper().Contains("IVBOR"))
            {
                return "png";
            }
            else if (data.ToUpper() == "/9J/4" || data.ToUpper().Contains("/9J/4"))
            {
                return "jpeg";
            }
            else if (data.ToUpper() == "/9J/4" || data.ToUpper().Contains("/9J/4"))
            {
                return "jpg";
            }
            else if (data.ToUpper() == "JVBER" || data.ToUpper().Contains("JVBER"))
            {
                return "pdf";
            }
            else
            {
                return string.Empty;
            }

        }
        private string SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            try
            {
                if (base64String.IndexOf(',') > 0)
                {
                    string[] arr = base64String.Split(',');
                    base64String = arr[1];
                }

                byte[] bytes = Convert.FromBase64String(base64String);
                System.IO.File.WriteAllBytes(fullOutputPath, bytes);
                return fullOutputPath;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message.ToString());
                return "";
            }

        }
    }

}
