using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.SWCSModel;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Models;
using System.Data;
using FMDSS.Models.Admin;
using FMDSS.Models.Master;
using System.Configuration;
using System.IO;
using System.Xml;
using FMDSS.Models.CitizenService.PermissionService;
namespace FMDSS.Controllers.SWCS
{
    public class SWCSController : Controller
    {
        //
        // GET: /SWCS/

        #region "Property Intialization"
        FixedLandUsage _objmodel = new FixedLandUsage();
        SMS_EMail_Services _objMailSMS = new SMS_EMail_Services();
        List<SelectListItem> Division = new List<SelectListItem>();
        List<SelectListItem> District = new List<SelectListItem>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        int ModuleID = 1;
        Int64 UserID = 0;
        double discount = 0;
        double Amount = 0;
        double Tax = 0;
        double FinalAmount = 0;
        double DiscountedAmmount = 0;
        double TaxableAmount = 0;
        private int PDFdocumentCount = 0;
        Location _obj = new Location();
        DataSet dsName = new DataSet();
        #endregion

        public ActionResult Index()
        {
            #region Check SWCS Data Developed by Rajveer
            SWCSModel model = new SWCSModel();
            UserProfile User = null;

            try
            {
                if (Request.QueryString["SWSID"] != null && Request.QueryString["ActID"] != null && Request.QueryString["ActivityID"] != null && Request.QueryString["IsNew"] != null)
                {
                    #region Get DATA and Fill Model
                    model.SWSID = Convert.ToString(Request.QueryString["SWSID"]);
                    model.ActID = Convert.ToString(Request.QueryString["ActID"]);
                    model.ActivityID = Convert.ToString(Request.QueryString["ActivityID"]);
                    model.IsNew = Convert.ToString(Request.QueryString["IsNew"]);

                    model.Userdetails = Convert.ToString(Request.Form["Userdetails"]);
                    model.Establishmentname = Convert.ToString(Request.Form["Establishmentname"]);
                    model.Total_Employees = Convert.ToString(Request.Form["Total_Employees"]);
                    model.ProposedInvestment = Convert.ToString(Request.Form["ProposedInvestment"]);
                    model.Operational_Date = Convert.ToString(Request.Form["Operational_Date"]);
                    model.Mobile = Convert.ToString(Request.Form["Mobile"]);
                    model.Email = Convert.ToString(Request.Form["Email"]);
                    model.CategoryofEstablishment = Convert.ToString(Request.Form["CategoryofEstablishment"]);
                    model.NatureOfBusiness = Convert.ToString(Request.Form["NatureOfBusiness"]);
                    model.PlotNo = Convert.ToString(Request.Form["PlotNo"]);
                    model.Street = Convert.ToString(Request.Form["Street"]);
                    model.Area = Convert.ToString(Request.Form["Area"]);
                    model.RuralUrban = Convert.ToString(Request.Form["RuralUrban"]);
                    model.City = Convert.ToString(Request.Form["City"]);
                    model.Ward = Convert.ToString(Request.Form["Ward"]);
                    model.Village = Convert.ToString(Request.Form["Village"]);
                    model.Tehsil = Convert.ToString(Request.Form["Tehsil"]);
                    model.District = Convert.ToString(Request.Form["District"]);
                    model.PIN = Convert.ToString(Request.Form["PIN"]);
                    model.BusinessDetail = Convert.ToString(Request.Form["BusinessDetail"]);
                    model.PrimaryGroup = Convert.ToString(Request.Form["PrimaryGroup"]);

                    model.BRN = Convert.ToString(Request.Form["BRN"]);
                    model.PAN = Convert.ToString(Request.Form["PAN"]);
                    model.TIN = Convert.ToString(Request.Form["TIN"]);
                    model.VAT = Convert.ToString(Request.Form["VAT"]);
                    model.STDCode = Convert.ToString(Request.Form["STDCode"]);
                    model.FirstName = Convert.ToString(Request.Form["FirstName"]);
                    model.LastName = Convert.ToString(Request.Form["LastName"]);
                    model.Gender = Convert.ToString(Request.Form["Gender"]);
                    model.DOB = Convert.ToString(Request.Form["DOB"]);
                    model.PostalAddress = Convert.ToString(Request.Form["PostalAddress"]);
                    model.Est_PlotNo = Convert.ToString(Request.Form["Est_PlotNo"]);
                    model.Est_Street = Convert.ToString(Request.Form["Est_Street"]);
                    model.Est_Area = Convert.ToString(Request.Form["Est_Area"]);
                    model.Est_RuralUrban = Convert.ToString(Request.Form["Est_RuralUrban"]);
                    model.Est_District = Convert.ToString(Request.Form["Est_District"]);
                    model.Est_Tehsil = Convert.ToString(Request.Form["Est_Tehsil"]);
                    model.Est_Ward = Convert.ToString(Request.Form["Est_Ward"]);
                    model.Est_Village = Convert.ToString(Request.Form["Est_Village"]);
                    model.Est_PlotNo = Convert.ToString(Request.Form["Userdetails"]);
                    model.Est_PIN = Convert.ToString(Request.Form["Est_PIN"]);


                    #region Set Session
                    RAJSSO.SSO SSO = new RAJSSO.SSO();
                    if (!string.IsNullOrEmpty(Request.Form["Userdetails"]))
                    {
                        Session["SSOTOKEN"] = Convert.ToString(Request.Form["userdetails"]);
                        SSO.CreateSSOSession();

                        RAJSSO.SSOWS.SSOTokenDetail detail = SSO.GetSessionValueXml();
                        Session["SSOID"] = detail.sAMAccountName;
                        Session["Role"] = string.Join(",", detail.Roles).ToUpperInvariant();
                        Session["loggedin"] = true;
                        RAJSSO.SSOWS.SSOUserDetail ssouser = SSO.GetUserDetailXML(Session["SSOID"].ToString(), ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[0], ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[1]);

                        if (ssouser != null)
                        {
                            Session["User"] = ssouser.displayName;
                            string designationId = string.Empty;
                            if (Session["Role"].Equals("CITIZEN"))
                                designationId = "10";

                            User = new UserProfile()
                            {
                                SSOId = Remove_hazardous_Character(detail.sAMAccountName),
                                FullName = Remove_hazardous_Character(ssouser.displayName),
                                AadharId = Remove_hazardous_Character(ssouser.aadhaarId),
                                BhamashahId = Remove_hazardous_Character(ssouser.bhamashahId),
                                DatOFBirth = Remove_hazardous_Character(ssouser.dateOfBirth),
                                Gender = Remove_hazardous_Character(ssouser.gender),
                                PhotURL = ssouser.jpegPhoto,
                                EmailId = ssouser.mailPersonal,
                                MobileNumber = Remove_hazardous_Character(ssouser.mobile),
                                Designation = Remove_hazardous_Character(designationId),
                                Address1 = Remove_hazardous_Character(ssouser.postalAddress),
                                PINCode1 = Remove_hazardous_Character(ssouser.postalCode),
                                District1 = Remove_hazardous_Character(ssouser.l),
                                Roles = Remove_hazardous_Character(Session["Role"].ToString()),
                                IsSSO = true,
                                IsBhamashah = false
                            };
                            Session["SSODetail"] = User;
                            User = (UserProfile)Session["SSODetail"];
                            DataSet DS = User.InsertUpdateUserInfo();
                            bool flag = Convert.ToBoolean(DS.Tables[0].Rows[0]["FIRSTTIMELOGIN"]);
                            Session["UserId"] = Convert.ToInt64(DS.Tables[0].Rows[0]["USERID"]);
                            Session["DesignationId"] = Convert.ToInt64(DS.Tables[0].Rows[0]["DESIGNATION"]);
                            Session["DesignationDes"] = DS.Tables[0].Rows[0]["DESIG_NAME"].ToString();

                            Session["IsKioskUser"] = Convert.ToBoolean(DS.Tables[0].Rows[0]["ISKIOSKUSER"]);
                            Session["IsDepartmentalKioskUser"] = Convert.ToBoolean(DS.Tables[0].Rows[0]["ISDEPARTMENTALKIOSKUSER"]);
                            Session["Role"] = Convert.ToString(DS.Tables[0].Rows[0]["Role"]);
                            if (Convert.ToBoolean(DS.Tables[0].Rows[0]["IsKioskUser"]) || Convert.ToBoolean(DS.Tables[0].Rows[0]["ISDEPARTMENTALKIOSKUSER"]))
                            {
                                Session["KioskUserId"] = Session["UserId"];
                                Session["KioskSSOId"] = Session["SSOID"];
                                Session["Role"] = "KIOSK";
                            }
                            Session["SSOID"] = User.SSOId;
                        }
                    }

                    #endregion

                    #endregion

                    #region Fill Session from SWCSModel
                    Session["SWCSModel"] = null;
                    Session["SWCSModel"] = model;
                    #endregion

                    return RedirectToAction("FixedPermission", "SWCS", new { aid = Encryption.encrypt(model.ActID), NOCType = "", NOCName = "", NOCTypeId = 0, SWCS = "true" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            #endregion
            return View();
        }

        // BACK TO SWCS Use following code to post values
        [HttpPost]
        public void BackToSWCS(string Userdetails)
        {
            String appName = "SWCS";
            String url = "http://swcstest.rajasthan.gov.in/ssolanding.aspx";
            string poststring = SWCSHelper.posttopage(url, Userdetails, appName);
            Response.Write(poststring);
            Response.End();
        }

        [HttpPost]
        public void CancelSWCS(string Usrdetails)
        {
            SWCSModel swcsModel = (SWCSModel)Session["SWCSModel"];

            String appName = "SWCS";
            String url = "http://swcstest.rajasthan.gov.in/servicelanding.aspx?SWSID=" + swcsModel.SWSID + "&ActID=" + swcsModel.ActID + "&ActivityID=" + swcsModel.ActivityID + "&IsNew=" + swcsModel.IsNew + "&RegNo=4321&status=0";
            string poststring = SWCSHelper.posttopage(url, Usrdetails, appName);
            Response.Write(poststring);
            Response.End();
        }

        public string Remove_hazardous_Character(string myString)
        {
            if (!string.IsNullOrEmpty(myString))
            {
                myString = myString.Replace("/", "");
                myString = myString.Replace("<", "");
                myString = myString.Replace(">", "");
                myString = myString.Replace("'", "");
                myString = myString.Replace("%", "");
                myString = myString.Replace(";", "");
                myString = myString.Replace("&", "");
                myString = myString.Replace("*", "");
                myString = myString.Replace("(", "");
                myString = myString.Replace(")", "");
                myString = myString.Replace("@", "");
                myString = myString.Replace("#", "");
                myString = myString.Replace("+", "");

                return myString;
            }
            else
                return string.Empty;
        }

        public ActionResult FixedPermission(string aid, string messagetype, string NOCType = "", string NOCName = "", int NOCTypeId = 0, string SWCS = "")
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                Boolean FlagSWCS = false;
                #region If SWCS TRUE Mean Calling this page from Single window in SSO Developed By Rajveer

                #region Fill Model In Session
                if (!string.IsNullOrEmpty(SWCS) && SWCS.ToLower().Trim() == "true" && Session["SWCSModel"] != null)
                {
                    SWCSRepository swcsRepo = new SWCSRepository();
                    _objmodel._SWCSModel = new SWCSModel();
                    _objmodel._SWCSModel = (SWCSModel)Session["SWCSModel"];

                    long SWCS_TblID = 0;
                    FlagSWCS = swcsRepo.CRUDFromSWCS(_objmodel, ref SWCS_TblID, "insert");
                    _objmodel._SWCSModel.SWCS_TblID = SWCS_TblID;
                    if (FlagSWCS == false)
                    {
                        // Error On Page
                    }
                }
                #endregion

                #endregion

                if (messagetype == null)
                { aid = Encryption.decrypt(aid); }

                if (aid == null)
                {
                    return View("error");
                }
                else
                {
                    // UserID = Convert.ToInt64(Session["UserID"].ToString());
                    if (Session["SSOid"] != null) { _objmodel.SSOID = Session["SSOid"].ToString(); }
                    if (aid == null || aid == "")
                    {
                        //string aid1 = Encryption.encrypt("1");
                        // aid = (aid1; 
                    }
                    #region If GIS Returned Request
                    List<clsPermission> fixedlandlist;
                    if (Session["clsPermission"] != null)
                    {
                        ViewData["fixedlandlist"] = (List<clsPermission>)Session["clsPermission"];
                        Session["clsPermission"] = null;
                    }
                    else
                    {
                        fixedlandlist = new List<clsPermission>();
                        ViewData["fixedlandlist"] = fixedlandlist;
                    }
                    if (Session["objModel"] != null)
                    {
                        FixedLandUsage gidModel = (FixedLandUsage)Session["objModel"];

                        _objmodel.Nearest_WaterSource = gidModel.Nearest_WaterSource;
                        _objmodel.WaterSource_Distance = gidModel.WaterSource_Distance;
                        _objmodel.Forest_Distance = gidModel.Forest_Distance;
                        _objmodel.Wildlife_Distance = gidModel.Wildlife_Distance;
                        _objmodel.AravalliHills = gidModel.AravalliHills;
                        _objmodel.ForestLand = gidModel.ForestLand;
                        _objmodel.Plantation_Area = gidModel.Plantation_Area;

                        _objmodel.KML_Path = gidModel.KML_Path;
                        _objmodel.GPSLat = gidModel.GPSLat;
                        _objmodel.GPSLong = gidModel.GPSLong;

                        _objmodel.GISID = gidModel.GISID;
                        _objmodel.Area_Size = gidModel.Area_Size;
                        _objmodel.ConditionGISMode = gidModel.ConditionGISMode;
                        Session["objModel"] = null;
                    }
                    #endregion

                    Session["NOCPurposeId"] = NOCTypeId;
                    Session["NOCPurpose"] = NOCType;
                    Session["NOCType"] = NOCName;
                    Session.Remove("EmitraDivCode");
                    Session["PermissionTypeID"] = "";
                    Session["PermissionTypeID"] = aid;
                    _objmodel = BindPageData(aid);
                    if (messagetype == "1")
                    {
                        ViewData["Message"] = "Error occur while saving records!";
                    }

                    //changes done by Vandana Gupta for the points received on 12-aug-2016 
                    BindMasterData _ObjMaster = new BindMasterData();
                    List<SelectListItem> NOCPurpose = new List<SelectListItem>();
                    DataTable dtNOCPurpose = new DataTable();
                    dtNOCPurpose = _ObjMaster.GetFixedLandNOCPurpose();
                    foreach (System.Data.DataRow dr in dtNOCPurpose.Rows)
                    {
                        NOCPurpose.Add(new SelectListItem { Text = @dr["NOCTypeName"].ToString(), Value = @dr["NOCTypeId"].ToString() });
                    }
                    ViewBag.NOCPurpose = NOCPurpose;
                    return View(_objmodel);
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt32(Session["UserId"].ToString()));
            }
            return null;
        }

        public FixedLandUsage BindPageData(string aid)
        {
            List<PlantFixedPermission> ListPlant = new List<PlantFixedPermission>();
            if (!string.IsNullOrEmpty(aid))
            {
                _objmodel.PermissionType = _objmodel.GetPermissionTypes(Convert.ToInt32(aid)).Tables[0].Rows[0]["Name"].ToString();
                Session["PermissionType"] = "";
                Session["PermissionType"] = _objmodel.PermissionType;
                DataSet Payds = new DataSet();
                List<FixedLandUsage> FixedLandList = new List<FixedLandUsage>();
                DataTable dt = null;
                dt = _objmodel.Division();

                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Division.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                }
                SawmillType obj1 = new SawmillType();
                IndustryType obj = new IndustryType();
                ViewBag.ddlDivision1 = Division;
                ViewBag.ddlDistrict1 = District;
                ViewBag.ddlBlockName1 = BlockName;
                ViewBag.ddlGPName1 = GPName;
                ViewBag.ddlVillName1 = VillageName;
                ViewBag.ApplicantType = new SelectList(Common.GetApplicantType(), "Value", "Text");
                ViewBag.NearestWaterSource = new SelectList(Common.GetNearestWaterSource(), "Value", "Text");
                ViewBag.Sawmill_Type = new SelectList(obj1.GetSawmillType(), "Value", "Text");
                ViewBag.Industrial_Type = new SelectList(obj.GetIndustryType(), "Value", "Text");
                _objmodel.ConditionRevenueMapSigned = "Both";
                _objmodel.Tree_species = "0";
                Payds = _objmodel.GetFinalAmount(1, Convert.ToInt32(aid));

                if (Payds.Tables[0].Rows.Count > 0)
                {
                    Amount = Convert.ToDouble(Payds.Tables[0].Rows[0]["Amount"]);
                    discount = Convert.ToDouble(Payds.Tables[0].Rows[0]["Discount"]);
                    Tax = Convert.ToDouble(Payds.Tables[0].Rows[0]["Tax"]);
                    DiscountedAmmount = (Amount * discount) / 100.0;
                    TaxableAmount = (Amount * Tax) / 100.0;
                    FinalAmount = (Amount + TaxableAmount) - DiscountedAmmount;
                    _objmodel.Amount = Convert.ToDecimal(Amount);
                    _objmodel.Discount = Convert.ToDecimal(discount);
                    _objmodel.Tax = Convert.ToDecimal(Tax);
                    _objmodel.Final_Amount = Convert.ToDecimal(FinalAmount);
                }

                DataSet dsPlant = _objmodel.GetPlantList();

                for (int i = 0; i < dsPlant.Tables.Count; i++)
                {
                    foreach (DataRow dr in dsPlant.Tables[0].Rows)
                    {
                        ListPlant.Add(new PlantFixedPermission()
                        {
                            PlantID = Convert.ToInt64(dr["ID"].ToString()),
                            PlantName = dr["Plant_Name"].ToString()
                        });
                    }
                }
                ViewData["PlantList"] = ListPlant;
                _objmodel.PermissionId = Convert.ToInt32(aid);
                return _objmodel;
            }
            else
            {
                return _objmodel;
            }
        }

        [HttpPost]
        public ActionResult FixedLandPermissions(FormCollection FCollection, FixedLandUsage Model, string command)
        {
            var path = "";
            string FileName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (command == "")
                {
                    #region Model Data
                    _objmodel.UserID = Convert.ToInt32(Session["UserId"].ToString());
                    _objmodel.PerposedArea = Convert.ToDecimal(FCollection["PerposedArea"]);

                    if (FCollection["hdMenuId"].ToString() == "")
                    {
                        _objmodel.PermissionId = 0;
                    }
                    else
                    {
                        _objmodel.PermissionId = Convert.ToInt32(FCollection["hdMenuId"]);
                    }
                    if (FCollection["Applicant_type"].ToString() == "")
                    {
                        _objmodel.ApplicantType = 0;
                    }
                    else
                    {
                        _objmodel.ApplicantType = Convert.ToInt32(FCollection["Applicant_type"]);
                    }

                    if (Model.Area_Size == null || Model.Area_Size.ToString() == "")
                    {
                        _objmodel.Area_Size = "";
                    }
                    else
                    {
                        _objmodel.Area_Size = Model.Area_Size.ToString();
                    }
                    if (Model.GPSLat == null || Model.GPSLat.ToString() == "")
                    {
                        _objmodel.GPSLat = "";
                    }
                    else
                    {
                        _objmodel.GPSLat = Model.GPSLat.ToString();
                    }
                    if (Model.GPSLong == null || Model.GPSLong.ToString() == "")
                    {
                        _objmodel.GPSLat = "";
                    }
                    else
                    {
                        _objmodel.GPSLong = Model.GPSLong.ToString();
                    }
                    if (FCollection["Industrial_Type"].ToString() == "")
                    {
                        _objmodel.Industrial_Type = "";
                    }
                    else
                    {
                        _objmodel.Industrial_Type = FCollection["Industrial_Type"].ToString();
                    }
                    if (FCollection["Revenue_Map_Signed"] == "")
                    {
                        _objmodel.Revenue_Map_Signed = "";
                    }
                    else
                    {
                        _objmodel.Revenue_Map_Signed = FCollection["Revenue_Map_Signed"].ToString();
                    }

                    _objmodel.IsGTSheetAvailable = "1";
                    _objmodel.Duration_From = DateTime.Now.ToShortDateString();
                    _objmodel.Duration_To = DateTime.Now.ToShortDateString();

                    _objmodel.KML_Path = FCollection["hdKMLFile"] == "" ? "" : FCollection["hdKMLFile"];
                    _objmodel.GISID = FCollection["hdGISID"] == "" ? "" : FCollection["hdGISID"];

                    if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                    {
                        FileName = Path.GetFileName(Request.Files[0].FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        _objmodel.Revenue_Record_Path = path;
                        Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));
                        PDFdocumentCount = PDFdocumentCount + 1;
                    }
                    else
                    { _objmodel.Revenue_Record_Path = ""; }

                    if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                    {
                        FileName = Path.GetFileName(Request.Files[1].FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        _objmodel.Revenue_Map_Path = path;
                        Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                        PDFdocumentCount = PDFdocumentCount + 1;
                    }
                    else
                    { _objmodel.Revenue_Map_Path = ""; }

                    if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                    {
                        FileName = Path.GetFileName(Request.Files[2].FileName);  ///// Amit Change (22-02-2016)
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        _objmodel.Additional_Document = path;
                        Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));
                        PDFdocumentCount = PDFdocumentCount + 1;
                    }
                    else
                    { _objmodel.Additional_Document = ""; }

                    if (Model.Citizen_Comment == null || Model.Citizen_Comment.ToString() == "")
                    {
                        _objmodel.Citizen_Comment = "";
                    }
                    else
                    {
                        _objmodel.Citizen_Comment = Model.Citizen_Comment.ToString();
                    }

                    #region MiningPermission
                    if (FCollection["Nearest_WaterSource"].ToString() == "")
                    {
                        _objmodel.Nearest_WaterSource = "";
                    }
                    else
                    {
                        _objmodel.Nearest_WaterSource = FCollection["Nearest_WaterSource"].ToString();
                    }
                    if (Model.WaterSource_Distance == null || Model.WaterSource_Distance.ToString() == "")
                    {
                        _objmodel.WaterSource_Distance = "";
                    }
                    else
                    {
                        _objmodel.WaterSource_Distance = Model.WaterSource_Distance.ToString();
                    }
                    if (Model.Forest_Distance == null || Model.Forest_Distance.ToString() == "")
                    {
                        _objmodel.Forest_Distance = "";
                    }
                    else
                    {
                        _objmodel.Forest_Distance = Model.Forest_Distance.ToString();
                    }
                    if (Model.Wildlife_Distance == null || Model.Wildlife_Distance.ToString() == "")
                    {
                        _objmodel.Wildlife_Distance = "";
                    }
                    else
                    {
                        _objmodel.Wildlife_Distance = Model.Wildlife_Distance.ToString();
                    }
                    if (Model.Tree_species == null || Model.Tree_species.ToString() == "")
                    {
                        _objmodel.Tree_species = "";
                    }
                    else
                    {
                        _objmodel.Tree_species = Model.Tree_species.ToString();
                    }

                    if (Model.AravalliHills == null || Model.AravalliHills.ToString() == "")
                    {
                        _objmodel.AravalliHills = "";
                    }
                    else
                    {
                        _objmodel.AravalliHills = Model.AravalliHills.ToString();
                    }
                    if (Model.ForestLand == null || Model.ForestLand.ToString() == "")
                    {
                        _objmodel.ForestLand = "";
                    }
                    else
                    {
                        _objmodel.ForestLand = Model.ForestLand.ToString();
                    }
                    if (Model.Plantation_Area == null || Model.Plantation_Area.ToString() == "")
                    {
                        _objmodel.Plantation_Area = "";
                    }
                    else
                    {
                        _objmodel.Plantation_Area = Model.Plantation_Area.ToString();
                    }

                    #endregion

                    #region "Sawmills"

                    if (FCollection["Sawmill_Type"].ToString() == "")
                    {
                        _objmodel.Sawmill_Type = "";
                    }
                    else
                    {
                        _objmodel.Sawmill_Type = FCollection["Sawmill_Type"].ToString();
                    }

                    if (Model.Sawmill_Size == null || Model.Sawmill_Size.ToString() == "")
                    {
                        _objmodel.Sawmill_Size = "";
                    }
                    else
                    {
                        _objmodel.Sawmill_Size = Model.Sawmill_Size.ToString();
                    }

                    #endregion
                    if (Model.OtherPermission == null || Model.OtherPermission.ToString() == "")
                    {
                        _objmodel.OtherPermission = "";
                    }
                    else
                    {
                        _objmodel.OtherPermission = Model.OtherPermission.ToString();
                    }
                    if (Model.Amount == null || Model.Amount.ToString() == "")
                    {
                        _objmodel.Amount = 0;
                    }
                    else
                    {
                        _objmodel.Amount = Convert.ToDecimal(Model.Amount);
                    }
                    if (Model.Discount == null || Model.Discount.ToString() == "")
                    {
                        _objmodel.Discount = 0;
                    }
                    else
                    {
                        _objmodel.Discount = Convert.ToDecimal(Model.Discount);
                    }

                    if (Model.FOREST_DIVCODE == null || Model.FOREST_DIVCODE.ToString() == "")
                    {
                        _objmodel.FOREST_DIVCODE = "0";
                    }
                    else
                    {
                        _objmodel.FOREST_DIVCODE = Model.FOREST_DIVCODE;
                    }

                    if (Model != null && _objmodel.Amount > 0)
                    {
                        _objmodel.Final_Amount = Model.Final_Amount + Convert.ToDecimal(5); ;
                        Session["Ftotalprice"] = _objmodel.Final_Amount.ToString();
                    }
                    _objmodel.TransactionId = DateTime.Now.Ticks.ToString();
                    Session["FRequestId"] = _objmodel.TransactionId.ToString();
                    _objmodel.PayStatus = "Pending";

                    string[] plantcount = FCollection["plantcount"].Split(char.Parse(","));
                    string[] plantId = FCollection["hdplant"].Split(char.Parse(","));

                    DataTable planttable = new DataTable();
                    planttable.Columns.Add("ID", typeof(string));
                    planttable.Columns.Add("Number_Trees", typeof(string));

                    for (var i = 0; i < plantcount.Length; i++)
                    {
                        DataRow dr = planttable.NewRow();
                        if (plantcount[i].ToString() != "0")
                        {
                            dr["ID"] = plantId[i];
                            dr["Number_Trees"] = plantcount[i];
                            planttable.Rows.Add(dr);
                        }
                    }
                    DataSet dsm = new DataSet();
                    if (Session["DistInfo"] != null)
                    {
                        dsm.ReadXml(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                    }
                    else
                    {
                        dsm.Clear();
                        DataTable objDt2 = new DataTable("Table");
                        objDt2.Columns.Add("Div_Cd", typeof(String));
                        objDt2.Columns.Add("Dist_Cd", typeof(String));
                        objDt2.Columns.Add("Blk_Cd", typeof(String));
                        objDt2.Columns.Add("Gp_Cd", typeof(String));
                        objDt2.Columns.Add("Vlg_Cd", typeof(String));
                        objDt2.Columns.Add("areaName", typeof(String));
                        objDt2.Columns.Add("khasraNo", typeof(String));
                        objDt2.Columns.Add("FOREST_DIVCODE", typeof(String));
                        objDt2.Columns.Add("Tehsil_Cd", typeof(String));
                        objDt2.AcceptChanges();
                        dsm.Tables.Add(objDt2);
                        objDt2.Clear();
                        objDt2.AcceptChanges();
                    }

                    //Added by Vandana Gupta on 26-Aug-2016
                    _objmodel.txtplantOthers = Model.txtplantOthers;
                    _objmodel.txtplantOthersNo = Model.txtplantOthersNo;

                    if (Session["SWCSModel"] != null)
                    {
                        _objmodel._SWCSModel = (SWCSModel)Session["SWCSModel"];                        
                    }
                    
                    Int64 id = _objmodel.SubmitFixedLandUsage(_objmodel, dsm.Tables[0], planttable, Convert.ToInt16( _objmodel._SWCSModel.ActID));
                    Session["NOCPurpose"] = null;
                    Session["NOCType"] = null;
                    #endregion

                    if (id > 0)
                    {
                        #region File Operation
                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml")) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                            Session["DistInfo"] = null;
                        }
                        if (_objmodel.Amount > 0)
                        {
                            STATUSUPDATE.STATUSUPDATE statusupdate = new STATUSUPDATE.STATUSUPDATE();
                            string result = statusupdate.RSPCBREGNUMUPDATE(Convert.ToInt32(_objmodel._SWCSModel.SWSID), _objmodel.TransactionId.ToString(), 1);
                            if (result == "Success")
                            {
                                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                                {
                                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                                    _obj.ModuleId = 1;
                                    _obj.ServiceTypeId = 1;
                                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                                    _obj.SubPermissionId = _objmodel.PermissionId;
                                    _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);

                                    if (dtKiosk.Rows.Count > 0)
                                    {
                                        _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                        return PartialView("KioskPaymentDetail", _obj);
                                    }
                                }
                                else if (Session["IsDepartmentalKioskUser"] != null && Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                                {
                                    // Added by Arvind Kumar Sharma
                                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();
                                    _obj.ModuleId = 1;
                                    _obj.ServiceTypeId = 1;
                                    _obj.PermissionId = 1;
                                    _obj.SubPermissionId = Convert.ToInt32(_objmodel.PermissionId);
                                    _obj.RequestedId = Convert.ToString(Session["FRequestId"]);
                                    _obj.PaidBy = _objmodel.kioskuserid;
                                    _obj.PaidForCitizen = _objmodel.UserID;
                                    _obj.PaidAmount = _objmodel.Final_Amount;
                                    _obj.PaidOn = Convert.ToString(DateTime.Now);
                                    return PartialView("PaymentByDepartmentalKioskUsers", _obj);
                                }
                                else
                                { 
                                    return View("FixedPermissionPayment", _objmodel);
                                }
                            }
                        }
                        else
                        {
                            DataSet ds = new DataSet();
                            ds = _objmodel.UpdatePaymentStatus(0, 1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));

                            CallSWCS(_objmodel);
                        }
                        #endregion
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    #region Else Part of Submit Command
                    if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                    {
                        FileName = Path.GetFileName(Request.Files[1].FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        _objmodel.Revenue_Record_Path = path;
                        Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    { _objmodel.Revenue_Record_Path = FCollection["hdRevRecPath"].ToString(); }

                    if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                    {
                        FileName = Path.GetFileName(Request.Files[2].FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        _objmodel.Revenue_Map_Path = path;
                        Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    { _objmodel.Revenue_Map_Path = FCollection["hdRevMapPath"].ToString(); }

                    _objmodel.UserID = Convert.ToInt32(Session["UserId"]);
                    _objmodel.RequestedID = command;
                    _objmodel.UpdateData(_objmodel, "reassigned");
                    return RedirectToAction("dashboard", "dashboard", new { messagetype = "3" });
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Session["NOCPurpose"] = null;
                Session["NOCType"] = null;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }
            return null;
        }

        public void CallSWCS(FixedLandUsage _objmodel)
        {
            Boolean FlagSWCS = false;
            SWCSRepository swcsRepo = new SWCSRepository();

            long SWCS_TblID = 0;

            FlagSWCS = swcsRepo.CRUDFromSWCS(_objmodel, ref SWCS_TblID, "update");
            _objmodel._SWCSModel.SWCS_TblID = SWCS_TblID;

            String appName = "SWCS";
            String url = "http://swcstest.rajasthan.gov.in/servicelanding.aspx?SWSID=" + _objmodel._SWCSModel.SWSID + "&ActID=" + _objmodel._SWCSModel.ActID
                + "&ActivityID=" + _objmodel._SWCSModel.ActivityID + "&IsNew=" + _objmodel._SWCSModel.IsNew + "&RegNo="+_objmodel.TransactionId+"&status=1";

            string poststring = SWCSHelper.posttopage(url, _objmodel._SWCSModel.Userdetails, appName);
            Response.Write(poststring);
            Response.End();

            //STATUSUPDATE.STATUSUPDATE statusupdate = new STATUSUPDATE.STATUSUPDATE();
            //string result = statusupdate.statusupdate(19, 1, "565498", 2, "License Fees Paid And Application Submitted", "", "", "");  
        }

        #region "Pay"

        [HttpPost]
        public void Pay()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                    Payment pay = new Payment();
                    string encrypt = pay.RequestString("EM33172142@5488", Session["FRequestId"].ToString(), Session["Ftotalprice"].ToString(), ReturnUrl + "SWCS/ResponsePay", Session["User"].ToString(), "", "");
                    Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }
        #endregion

        /// <summary>
        /// For Fetching the reponse from Payment gateway and save the status in Database
        /// </summary>
        /// <returns></returns>
        public ActionResult ResponsePay()
        {
            Payment pay = new Payment();
            int status1 = 0;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = new DataTable();
                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                    }
                    #endregion
                    string response = Request.QueryString["trnParams"].ToString();

                    #region Response decryption
                    string ResponseResult = pay.ProcesTranscationresponce(response);
                    string str1, str2;
                    str1 = ResponseResult.Replace("<RESPONSE ", "");
                    str2 = str1.Replace("></RESPONSE>", " ");
                    string[] Responsearr = System.Text.RegularExpressions.Regex.Split(str2, "' ");
                    string checkFail = "STATUS='FAILED";
                    string checkSucess = "STATUS='SUCCESS";
                    string rowstatus1 = "";
                    foreach (var item in Responsearr)
                    {
                        if (item.Equals(checkFail))
                        {
                            string[] statusx = item.Split('=');
                            rowstatus1 = statusx[1].ToString();
                        }
                        if (item.Equals(checkSucess))
                        {
                            string[] status2 = item.Split('=');
                            rowstatus1 = status2[1].ToString();
                        }
                    }
                    int status1len = Convert.ToInt32(rowstatus1.ToString().Length);
                    string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 1);
                    #endregion

                    #region Response Status
                    if (finalstatus1 == "FAILED")
                    {
                        string[] emitratransid = Responsearr[0].Split('=');
                        string[] transtime = Responsearr[1].Split('=');
                        string[] reqid = Responsearr[2].Split('=');
                        string[] reqamt = Responsearr[3].Split('=');
                        string[] username = Responsearr[4].Split('=');
                        string[] status = Responsearr[7].Split('=');

                        DataRow dtrow = dt.NewRow();
                        string rowstatus = status[1].ToString();
                        int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                        string finalstatus = rowstatus.ToString().Substring(1, statuslen - 1);
                        string rowreqid = reqid[1].ToString();
                        int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                        string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 1);
                        string rawemitra = emitratransid[1].ToString();
                        int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                        string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 1);
                        _objmodel.TransactionId = finalemitraid;
                        string rawtransamount = reqamt[1].ToString();
                        int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                        string finalamount = rawtransamount.ToString().Substring(1, amountlen - 1);
                        string rawusername = username[1].ToString();
                        int usernamelen = Convert.ToInt32(rawusername.Length);
                        string finalUserName = rawusername.ToString().Substring(1, usernamelen - 1);

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                        dtrow["REQUEST ID"] = finalreqid;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";//transtime[1];
                        dtrow["TRANSACTION AMOUNT"] = finalamount;
                        dtrow["USER NAME"] = finalUserName;

                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                        {
                            _objmodel.Trn_Status_Code = 0;
                        }
                        dt.Rows.Add(dtrow);
                    }
                    else if (finalstatus1 == "SUCCESS")
                    {
                        string[] emitratransid = Responsearr[0].Split('=');
                        string[] transtime = Responsearr[1].Split('=');
                        string[] reqid = Responsearr[2].Split('=');
                        string[] reqamt = Responsearr[3].Split('=');
                        string[] username = Responsearr[4].Split('=');
                        string[] status = Responsearr[7].Split('=');
                        string[] bank = Responsearr[8].Split('=');
                        string[] bankbidno = Responsearr[10].Split('=');

                        DataRow dtrow = dt.NewRow();
                        int statuslen = Convert.ToInt32(status[1].ToString().Length);
                        string finalstatus = status[1].ToString().Substring(1, statuslen - 1);
                        int reqidlen = Convert.ToInt32(reqid[1].ToString().Length);
                        string finalreqid = reqid[1].ToString().Substring(1, reqidlen - 1);
                        int emitralen = Convert.ToInt32(emitratransid[1].ToString().Length);
                        string finalemitraid = emitratransid[1].ToString().Substring(1, emitralen - 1);
                        _objmodel.TransactionId = finalemitraid;
                        int amountlen = Convert.ToInt32(reqamt[1].ToString().Length);
                        string finalamount = reqamt[1].ToString().Substring(1, amountlen - 1);

                        int timelen = Convert.ToInt32(transtime[1].ToString().Length);
                        string datetime = transtime[1].ToString().Substring(1, timelen - 1);
                        int banklen = Convert.ToInt32(bank[1].ToString().Length);
                        string finalbank = bank[1].ToString().Substring(1, banklen - 1);

                        int userlen = Convert.ToInt32(username[1].ToString().Length);
                        string finalUsername = username[1].ToString().Substring(1, userlen - 1);

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                        dtrow["REQUEST ID"] = finalreqid;
                        dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
                        dtrow["TRANSACTION TIME"] = datetime;
                        dtrow["TRANSACTION AMOUNT"] = finalamount;
                        dtrow["USER NAME"] = finalUsername;
                        dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            _objmodel.Trn_Status_Code = 1;
                        }
                        dt.Rows.Add(dtrow);
                    }
                    #endregion
                    ViewData.Model = dt.AsEnumerable();
                    _objmodel.UserName = Session["User"].ToString();
                    if (_objmodel.Trn_Status_Code == 1)
                    {
                        DataSet ds = new DataSet();
                        ds = _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), _objmodel.Trn_Status_Code, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));
                        #region "User Send Email"
                        string UserMailBody = Common.GenerateBody(_objmodel.Final_Amount, ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                        string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                        _objMailSMS.sendEMail("Payment Details for " + Session["PermissionType"].ToString() + " Permission", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

                        SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        #endregion
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            #region "Is Reviwer Mail"
                            string ReviwerMailBody = Common.GenerateReviwerBody(ds.Tables[1].Rows[0]["Name"].ToString(), _objmodel.TransactionId, ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission");
                            _objMailSMS.sendEMail("Request for " + ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission", ReviwerMailBody, ds.Tables[1].Rows[0]["EmailId"].ToString(), string.Empty);
                            #endregion
                        }
                        if (Session["SWCSModel"] != null)
                        {
                            _objmodel._SWCSModel = (SWCSModel)Session["SWCSModel"];
                        } 
                        CallSWCS(_objmodel);
                        //return View("TransactionStatus");
                    }
                    else
                    {
                        _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), status1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));
                        if (Session["SWCSModel"] != null)
                        {
                            _objmodel._SWCSModel = (SWCSModel)Session["SWCSModel"];
                        }
                        CallSWCS(_objmodel);
                        //return View("NOCFIlmORGTransactionStatus");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        public ActionResult getGISData(FormCollection form)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string aid = string.Empty;
            try
            {
                if (Convert.ToString(Session["PermissionTypeID"]) != "") { aid = Convert.ToString(Session["PermissionTypeID"]); }
                if (Convert.ToString(form["successFlag"]).ToLower() == "true")
                {
                    List<clsPermission> myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));

                    #region "Muliple List"
                    List<clsPermission> fixedlandlist = new List<clsPermission>();
                    string LAT = string.Empty, Long = string.Empty;
                    foreach (var dr in myDeserializedObj)
                    {
                        #region Old Code Commented
                        //fixedlandlist.Add(
                        //    new clsPermission()

                        //    {
                        //        Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd,
                        //        Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd,
                        //        Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd,
                        //        Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd,
                        //        Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd,
                        //        Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM,
                        //        Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM,
                        //        Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM,
                        //        Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM,
                        //        Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM,
                        //        areaName = dr.areaName == "NA" ? "N/A" : dr.areaName,
                        //        FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE,
                        //        Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd,
                        //        Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM
                        //    }); 
                        #endregion

                        clsPermission cls = new clsPermission();
                        cls.Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        cls.Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        cls.Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        cls.Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        cls.Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        cls.Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM;
                        cls.Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM;
                        cls.Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM;
                        cls.Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM;
                        cls.Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM;
                        cls.areaName = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        cls.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE;
                        cls.Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd;
                        cls.Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM;
                        fixedlandlist.Add(cls);

                        //GISDataBaseModel gisModel = new GISDataBaseModel();
                        //gisModel.DIV_CODE = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        //gisModel.DIST_CODE = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        //gisModel.BLK_CODE = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        //gisModel.GP_CODE = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        //gisModel.VILL_CODE = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        //gisModel.Area = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        //gisModel.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : string.IsNullOrEmpty(dr.FOREST_DIVCODE) ? "N/A" : dr.FOREST_DIVCODE;
                        //gisModel.GPSLat = LAT;
                        //gisModel.GPSLong = Long;
                        //gisModel.GISFilePath = Convert.ToString(form["filePath"]);
                        //gisModel.GISOrignalFilePath = Convert.ToString(form["originalFileName"]);
                        //gisModel.GISID = _objmodel.GISID;
                        // gisModel.AreaShapeInHactor = _objmodel.Area_Size;

                        //_objmodel.Area_Size = 
                        //GISInformationList.Add(gisModel);
                    }

                    #endregion

                    #region MiningPermission

                    _objmodel.Nearest_WaterSource = form["nearbywaterbody"].ToString() == "NA" ? "N/A" : form["nearbywaterbody"].ToString();
                    _objmodel.WaterSource_Distance = form["nearbywaterbodydistance"].ToString() == "NA" ? "N/A" : form["nearbywaterbodydistance"].ToString();
                    _objmodel.Forest_Distance = form["nearbyforestdistance"].ToString() == "NA" ? "N/A" : form["nearbyforestdistance"].ToString();
                    _objmodel.Wildlife_Distance = form["nearbywildlifedistance"].ToString() == "NA" ? "" : form["nearbywildlifedistance"].ToString();
                    _objmodel.AravalliHills = form["iswithinaravali"].ToString() == "NA" ? "0" : "1";
                    _objmodel.ForestLand = form["iswithinforest"].ToString() == "NA" ? "0" : "1";
                    _objmodel.Plantation_Area = form["iswithinplantation"].ToString() == "NA" ? "0" : "1";

                    #endregion

                    #region "KML and Lat-Long"
                    _objmodel.KML_Path = form["filePath"].ToString() == "NA" ? "" : form["filePath"].ToString();

                    if (form["locCentroid"].ToString() != "")
                    {
                        if (form["locCentroid"].ToString().Contains(","))
                        {

                            string[] locCentroid = form["locCentroid"].ToString().Split(',');
                            _objmodel.GPSLat = locCentroid[1] == "NA" ? "" : locCentroid[1];
                            _objmodel.GPSLong = locCentroid[0] == "NA" ? "" : locCentroid[0];

                        }
                    }
                    #endregion

                    //_objmodel = BindPageData(aid);
                    _objmodel.GISID = form["gisid"].ToString();
                    _objmodel.Area_Size = Convert.ToString(form["shapeArea"]);
                    _objmodel.ConditionGISMode = true;
                    //ViewData["fixedlandlist"] = fixedlandlist;
                    //Session["GISModel"] = GISInformationList;
                    Session["clsPermission"] = fixedlandlist;
                    Session["objModel"] = _objmodel;
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.InnerException + "_" + ex.StackTrace);
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }
            //return View("FixedPermission", _objmodel);
            return RedirectToAction("FixedPermission", "SWCS", new { aid = Encryption.encrypt(aid), NOCType = "", NOCName = "", NOCTypeId = 0, SWCS = "true" });
        }


        [HttpPost]
        /// <summary>
        /// Use for save mapping between District to Village
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>   
        public JsonResult SaveDistMapping(string DIV_CODE, string DIST_CODE, string BLK_CODE, string GP_CODE, string VILL_CODE, string areaName, string KhasraNo, string FOREST_DIVCODE, string tehsilCode)
        {
            DataSet ds = new DataSet();
            clsPermission objModel = new clsPermission();
            objModel.DIV_CODE = DIV_CODE;
            objModel.DIST_CODE = DIST_CODE;
            objModel.BLK_CODE = BLK_CODE;
            objModel.GP_CODE = GP_CODE;
            objModel.VILL_CODE = VILL_CODE;
            objModel.areaName = areaName;
            objModel.KhasraNo = KhasraNo;
            objModel.FOREST_DIVCODE = FOREST_DIVCODE;
            Session["EmitraDivCode"] = objModel.FOREST_DIVCODE;
            objModel.Tehsil_Cd = tehsilCode;
            if (objModel != null)
            {
                try
                {

                    #region comment
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = "FXD_" + DateTime.Now.Ticks.ToString();

                    if (Session["DistInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("DistRoot");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["DistInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                    }
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["Vlg_Cd"].ToString() == objModel.Vlg_Cd)
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }


                            }
                            ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                        }
                    }
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));

                    XmlElement Dist_Map = xmldoc.CreateElement("Dist_Map");
                    XmlElement Div_Cd = xmldoc.CreateElement("Div_Cd");
                    XmlElement Dist_Cd = xmldoc.CreateElement("Dist_Cd");
                    XmlElement Blk_Cd = xmldoc.CreateElement("Blk_Cd");
                    XmlElement Gp_Cd = xmldoc.CreateElement("Gp_Cd");
                    XmlElement Vlg_Cd = xmldoc.CreateElement("Vlg_Cd");
                    XmlElement AreaName = xmldoc.CreateElement("areaName");
                    XmlElement khasraNo = xmldoc.CreateElement("khasraNo");
                    XmlElement FOREST_DivCode = xmldoc.CreateElement("FOREST_DIVCODE");
                    XmlElement Tehsil_Cd = xmldoc.CreateElement("Tehsil_Cd");


                    Div_Cd.InnerText = objModel.DIV_CODE;
                    Dist_Cd.InnerText = objModel.DIST_CODE;
                    Blk_Cd.InnerText = objModel.BLK_CODE;
                    Gp_Cd.InnerText = objModel.GP_CODE;
                    Vlg_Cd.InnerText = objModel.VILL_CODE;
                    AreaName.InnerText = objModel.areaName;
                    khasraNo.InnerText = objModel.KhasraNo;
                    FOREST_DivCode.InnerText = objModel.FOREST_DIVCODE;
                    Tehsil_Cd.InnerText = objModel.Tehsil_Cd;

                    Dist_Map.AppendChild(Div_Cd);
                    Dist_Map.AppendChild(Dist_Cd);
                    Dist_Map.AppendChild(Blk_Cd);
                    Dist_Map.AppendChild(Gp_Cd);
                    Dist_Map.AppendChild(Vlg_Cd);
                    Dist_Map.AppendChild(AreaName);
                    Dist_Map.AppendChild(khasraNo);
                    Dist_Map.AppendChild(FOREST_DivCode);
                    Dist_Map.AppendChild(Tehsil_Cd);

                    xmldoc.DocumentElement.AppendChild(Dist_Map);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                    #endregion
                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }
            }

            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNOCType(int NOCPuproseId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> NOCPurpose = new List<SelectListItem>();
            try
            {
                //changes done by Vandana Gupta for the points received on 12-aug-2016 
                BindMasterData _ObjMaster = new BindMasterData();
                DataTable dtNOCPurpose = new DataTable();
                dtNOCPurpose = _ObjMaster.GetFixedLandNOCNames(NOCPuproseId);
                foreach (System.Data.DataRow dr in dtNOCPurpose.Rows)
                {
                    NOCPurpose.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["P_ID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }
            return Json(new SelectList(NOCPurpose, "Value", "Text"));

        }

    }
}
