using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterAction;
using FMDSS.Models.Master;
using FMDSS.Models.SWCSModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    //[MyAuthorization]
    public class NOCController : Controller
    {
        #region "Property Intialization"
        FixedLandUsage fl = new FixedLandUsage();
        UserProfile User = null;
        private FmdssContext dbContext;
        SMS_EMail_Services _objMailSMS = new SMS_EMail_Services();
        DAL dl = new DAL();

        int ModuleID = 1;
        Int64 UserID = 0;
        //double discount = 0;
        //double Amount = 0;
        //double Tax = 0;
        //double FinalAmount = 0;
        //double DiscountedAmmount = 0;
        //double TaxableAmount = 0;
        //private int PDFdocumentCount = 0;
        //Location _obj = new Location();
        //DataSet dsName = new DataSet();

        public NOCController()
        {
            dbContext = new FmdssContext();
        }
        #endregion

        public ActionResult Index()
        {
            NocData _objmodel;
            if (Session["NOCData"] != null)  // GIS return case
            {
                _objmodel = (NocData)Session["NOCData"];
            }
            else if (Request.QueryString["Req"] != null) // Re-Assign Case
            {
                _objmodel = new NocData();
                _objmodel.RequestedID = Request.QueryString["Req"].Trim();
                ReassignReq(_objmodel);
            }
            else // First time load 
            {
                _objmodel = new NocData();
            }
            BindPageData(_objmodel);
            if (_objmodel.IsSWCS)
            {
                ViewBag.UserDetail = _objmodel.Userdetails;
                return View("_CitizenNoc", _objmodel);
            }
            else
                return View(_objmodel);
        }

        [HttpPost]
        public ActionResult Index(string SWSID)
        {
            Session["NOCData"] = null;
            NocData _objmodel;
            #region SWCS Case


            if (Request.QueryString["SWSID"] != null && Request.QueryString["ActID"] != null && Request.QueryString["ActivityID"] != null)
            {
                #region Get DATA and Fill Model


                _objmodel = new NocData();
                _objmodel.SWSID = Convert.ToString(Request.QueryString["SWSID"]);
                _objmodel.NOCPurpose = Convert.ToInt32(Request.QueryString["ActivityID"]);
                _objmodel.NOCType = Convert.ToInt32(Request.QueryString["ActID"]);
                _objmodel.IsNew = Convert.ToString(Request.QueryString["IsNew"]);
                _objmodel.Userdetails = Convert.ToString(Request.Form["Userdetails"]);
                _objmodel.IsSWCS = true;
                _objmodel.Userdetails = Convert.ToString(Request.Form["Userdetails"]);
                if (Request.QueryString["RegNo"] != null)
                {
                    _objmodel.RequestedID = Convert.ToString(Request.QueryString["RegNo"]);
                    Session["RegNo"] = null;
                    Session["RegNo"] = _objmodel.RequestedID;
                }
                if (!string.IsNullOrEmpty(Request.Form["Userdetails"]))
                {
                    SetSSOSession(Convert.ToString(Request.Form["userdetail"]), "", "");
                }

                if (Request.QueryString["IsView"] != null)
                {
                    if (!String.IsNullOrEmpty(_objmodel.RequestedID))
                    {
                        _objmodel.IsView = true;
                        ReassignReq(_objmodel);
                    }
                    //_objmodel.IsView = Convert.ToBoolean(Convert.ToInt32(Request.QueryString["IsView"]));
                    //else if ((!String.IsNullOrEmpty(_objmodel.RequestedID)) && (! _objmodel.IsView))
                    //{
                    //    return View("Detail", _objmodel);
                    //}
                }
                else if (!string.IsNullOrEmpty(_objmodel.RequestedID))
                {
                    ReassignReq(_objmodel);
                }
                else
                {
                    #region Personal and Company Data
                    _objmodel.personInfo = new PersonSW();
                    _objmodel.personInfo.Establishmentname = Convert.ToString(Request.Form["Establishmentname"]);
                    _objmodel.personInfo.Total_Employees = Convert.ToString(Request.Form["Total_Employees"]);
                    _objmodel.personInfo.ProposedInvestment = Convert.ToString(Request.Form["ProposedInvestment"]);
                    _objmodel.personInfo.Operational_Date = Convert.ToString(Request.Form["Operational_Date"]);
                    _objmodel.personInfo.Mobile = Convert.ToString(Request.Form["Mobile"]);
                    _objmodel.personInfo.Email = Convert.ToString(Request.Form["Email"]);
                    _objmodel.personInfo.CategoryofEstablishment = Convert.ToString(Request.Form["CategoryofEstablishment"]);
                    _objmodel.personInfo.NatureOfBusiness = Convert.ToString(Request.Form["NatureOfBusiness"]);
                    _objmodel.personInfo.PlotNo = Convert.ToString(Request.Form["PlotNo"]);
                    _objmodel.personInfo.Street = Convert.ToString(Request.Form["Street"]);
                    _objmodel.personInfo.Area = Convert.ToString(Request.Form["Area"]);
                    _objmodel.personInfo.RuralUrban = Convert.ToString(Request.Form["RuralUrban"]);
                    _objmodel.personInfo.City = Convert.ToString(Request.Form["City"]);
                    _objmodel.personInfo.Ward = Convert.ToString(Request.Form["Ward"]);
                    _objmodel.personInfo.Village = Convert.ToString(Request.Form["Village"]);
                    _objmodel.personInfo.Tehsil = Convert.ToString(Request.Form["Tehsil"]);
                    _objmodel.personInfo.District = Convert.ToString(Request.Form["District"]);
                    _objmodel.personInfo.PIN = Convert.ToString(Request.Form["PIN"]);
                    _objmodel.personInfo.BusinessDetail = Convert.ToString(Request.Form["BusinessDetail"]);
                    _objmodel.personInfo.PrimaryGroup = Convert.ToString(Request.Form["PrimaryGroup"]);

                    _objmodel.personInfo.BRN = Convert.ToString(Request.Form["BRN"]);
                    _objmodel.personInfo.PAN = Convert.ToString(Request.Form["PAN"]);
                    _objmodel.personInfo.TIN = Convert.ToString(Request.Form["TIN"]);
                    _objmodel.personInfo.VAT = Convert.ToString(Request.Form["VAT"]);
                    _objmodel.personInfo.STDCode = Convert.ToString(Request.Form["STDCode"]);
                    _objmodel.personInfo.FirstName = Convert.ToString(Request.Form["FirstName"]);
                    _objmodel.personInfo.LastName = Convert.ToString(Request.Form["LastName"]);
                    _objmodel.personInfo.Gender = Convert.ToString(Request.Form["Gender"]);
                    _objmodel.personInfo.DOB = Convert.ToString(Request.Form["DOB"]);
                    _objmodel.personInfo.PostalAddress = Convert.ToString(Request.Form["PostalAddress"]);
                    _objmodel.personInfo.Est_PlotNo = Convert.ToString(Request.Form["Est_PlotNo"]);
                    _objmodel.personInfo.Est_Street = Convert.ToString(Request.Form["Est_Street"]);
                    _objmodel.personInfo.Est_Area = Convert.ToString(Request.Form["Est_Area"]);
                    _objmodel.personInfo.Est_RuralUrban = Convert.ToString(Request.Form["Est_RuralUrban"]);
                    _objmodel.personInfo.Est_District = Convert.ToString(Request.Form["Est_District"]);
                    _objmodel.personInfo.Est_Tehsil = Convert.ToString(Request.Form["Est_Tehsil"]);
                    _objmodel.personInfo.Est_Ward = Convert.ToString(Request.Form["Est_Ward"]);
                    _objmodel.personInfo.Est_Village = Convert.ToString(Request.Form["Est_Village"]);
                    _objmodel.personInfo.Est_PIN = Convert.ToString(Request.Form["Est_PIN"]);

                    _objmodel.personStr = JsonConvert.SerializeObject(_objmodel.personInfo);
                    #endregion
                }

                Session["NOCData"] = _objmodel;

                #endregion
            }
            else
            { _objmodel = new NocData(); }
            #endregion

            BindPageData(_objmodel);
            ViewBag.UserDetail = _objmodel.Userdetails;
            return View("_CitizenNoc", _objmodel);
        }

        private void SetSSOSession(string UserDetail, string Mobile, string Email)
        {
            #region Set Session
            RAJSSO.SSO SSO = new RAJSSO.SSO();
            Session["SSOTOKEN"] = UserDetail;
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
                if ((!String.IsNullOrEmpty(Email)) && Email != ssouser.mailPersonal)
                {
                    User.EmailId = Email;
                }

                if ((!String.IsNullOrEmpty(Mobile)) && Mobile != ssouser.mobile)
                {
                    User.MobileNumber = Mobile;
                }

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
            #endregion
        }

        public ActionResult getGISData(FormCollection form)
        {
            NocData _objmodel = new NocData();
            try
            {
                if (Convert.ToString(form["successFlag"]).ToLower() == "true")
                {
                    List<clsPermission> myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));

                    #region "Muliple List"
                    List<clsPermission> fixedlandlist = new List<clsPermission>();
                    foreach (var dr in myDeserializedObj)
                    {
                        fixedlandlist.Add(new clsPermission()
                        {
                            Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd,
                            Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd,
                            Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd,
                            Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd,
                            Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd,
                            Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM,
                            Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM,
                            Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM,
                            Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM,
                            Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM,
                            areaName = dr.areaName == "NA" ? "N/A" : dr.areaName,
                            FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE,
                            Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd,
                            Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM
                        });
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
                    _objmodel.GISID = form["gisid"].ToString();
                    _objmodel.Area_Size = Convert.ToString(form["shapeArea"]);

                    _objmodel.KML_Path = form["filePath"].ToString() == "NA" ? "" : form["filePath"].ToString();
                    if (form["locCentroid"].ToString() != "" && form["locCentroid"].ToString().Contains(","))
                    {
                        string[] locCentroid = form["locCentroid"].ToString().Split(',');
                        _objmodel.GPSLat = locCentroid[1] == "NA" ? "" : locCentroid[1];
                        _objmodel.GPSLong = locCentroid[0] == "NA" ? "" : locCentroid[0];
                    }
                    #endregion


                    _objmodel.gislist = fixedlandlist;
                    _objmodel.NOCPurpose = Convert.ToInt32(Request.QueryString["nocpurpose"].ToString());
                    _objmodel.NOCType = Convert.ToInt32(Request.QueryString["noctype"].ToString());

                    _objmodel.NOCPurposeStr = Request.QueryString["nocpurposestr"];
                    _objmodel.NOCTypeStr = Request.QueryString["noctypestr"];
                    _objmodel.SWSID = Convert.ToString(Request.QueryString["swcsid"]);
                    _objmodel.IsSWCS = Convert.ToBoolean(Request.QueryString["isswcs"]);
                    _objmodel.Userdetails = Convert.ToString(Request.QueryString["userdetail"]);

                    _objmodel.Revenue_Record_Path = Convert.ToString(Request.QueryString["Revenue_Record_Path"]);
                    _objmodel.Revenue_Map_Path = Convert.ToString(Request.QueryString["Revenue_Map_Path"]);
                    _objmodel.Additional_Document = Convert.ToString(Request.QueryString["Additional_Document"]);
                    _objmodel.RequestedID = Convert.ToString(Request.QueryString["RequestedID"]);
                    if (_objmodel.IsSWCS)
                    {
                        _objmodel.personStr = Convert.ToString(Request.QueryString["personStr"]);
                        _objmodel.personInfo = JsonConvert.DeserializeObject<PersonSW>(_objmodel.personStr);
                    }

                    if (!(_objmodel.RequestedID != null && _objmodel.RequestedID.Length > 2))
                    {
                        _objmodel.RequestedID = DateTime.Now.Ticks.ToString();
                    }

                    Session["NOCData"] = _objmodel;
                }
                else
                {
                    List<clsPermission> fixedlandlist = new List<clsPermission>(); ViewData["fixedlandlist"] = fixedlandlist;
                }
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("Index");
        }

        private void ReassignReq(NocData _nc)
        {
            DataSet ds = _nc.GetReAssignReq(_nc.RequestedID);
            if (ds != null && ds.Tables[0] != null)
            {
                _nc.PerposedArea = Convert.ToString(ds.Tables[0].Rows[0]["PerposedArea"]) == "" ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["PerposedArea"]);
                _nc.OtherPermission = Convert.ToString(ds.Tables[0].Rows[0]["OtherPermission"]);
                _nc.Tree_species = Convert.ToString(ds.Tables[0].Rows[0]["Tree_species"]);
                _nc.Citizen_Comment = Convert.ToString(ds.Tables[0].Rows[0]["Citizen_Comment"]);
                _nc.GISID = Convert.ToString(ds.Tables[0].Rows[0]["GISID"]);
                _nc.NOCPurpose = Convert.ToInt32(ds.Tables[0].Rows[0]["NOC_Purpose"]);
                _nc.NOCType = Convert.ToInt32(ds.Tables[0].Rows[0]["P_ID"]);
                _nc.ApplicantType = Convert.ToInt32(ds.Tables[0].Rows[0]["ApplicantType"]);

                _nc.Revenue_Record_Path = Convert.ToString(ds.Tables[0].Rows[0]["Revenue_Record_Path"]);
                _nc.Revenue_Map_Path = Convert.ToString(ds.Tables[0].Rows[0]["Revenue_Map_Path"]);
                _nc.Additional_Document = Convert.ToString(ds.Tables[0].Rows[0]["Additional_Document"]);

                #region MiningPermission
                _nc.Nearest_WaterSource = Convert.ToString(ds.Tables[0].Rows[0]["Nearest_WaterSource"]);
                _nc.WaterSource_Distance = Convert.ToString(ds.Tables[0].Rows[0]["WaterSource_Distance"]);
                _nc.Forest_Distance = Convert.ToString(ds.Tables[0].Rows[0]["Forest_Distance"]);
                _nc.Wildlife_Distance = Convert.ToString(ds.Tables[0].Rows[0]["Wildlife_Distance"]);
                _nc.AravalliHills = Convert.ToString(ds.Tables[0].Rows[0]["AravalliHills"]);
                _nc.ForestLand = Convert.ToString(ds.Tables[0].Rows[0]["ForestLand"]);
                _nc.Plantation_Area = Convert.ToString(ds.Tables[0].Rows[0]["Plantation_Area"]);
                #endregion

                #region "KML and Lat-Long"
                _nc.GISID = Convert.ToString(ds.Tables[0].Rows[0]["GISID"]);
                _nc.Area_Size = Convert.ToString(ds.Tables[0].Rows[0]["Area_Size"]);
                _nc.KML_Path = Convert.ToString(ds.Tables[0].Rows[0]["KML_Path"]);

                _nc.GPSLat = Convert.ToString(ds.Tables[0].Rows[0]["GPSLat"]);
                _nc.GPSLong = Convert.ToString(ds.Tables[0].Rows[0]["GPSLong"]);
                #endregion

                #region "Gis List"
                _nc.gislist = new List<clsPermission>();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    _nc.gislist.Add(new clsPermission()
                    {
                        Div_Cd = Convert.ToString(dr["DIV_CODE"]),
                        Dist_Cd = Convert.ToString(dr["DIST_CODE"]),
                        Blk_Cd = Convert.ToString(dr["BLK_CODE"]),
                        Gp_Cd = Convert.ToString(dr["GP_CODE"]),
                        Vlg_Cd = Convert.ToString(dr["VILL_CODE"]),
                        Div_NM = Convert.ToString(dr["DIV_NAME"]),
                        Dist_NM = Convert.ToString(dr["DIST_NAME"]),
                        Block_NM = Convert.ToString(dr["BLK_NAME"]),
                        Gp_NM = Convert.ToString(dr["GP_NAME"]),
                        Village_NM = Convert.ToString(dr["VILL_NAME"]),
                        areaName = Convert.ToString(dr["Area"]),
                        FOREST_DIVCODE = Convert.ToString(dr["FOREST_DIVCODE"]),
                        Tehsil_Cd = Convert.ToString(dr["DIV_CODE"]),
                        Tehsil_NM = Convert.ToString(dr["DIV_CODE"]),
                        KhasraNo = Convert.ToString(dr["KhasraNo"])
                    });
                }
                #endregion

                #region "Plant List"
                _nc.plantList = new List<PlantFixedPermission>();
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    _nc.plantList.Add(new PlantFixedPermission()
                    {
                        PlantID = Convert.ToInt64(dr["PlantID"]),
                        PlantCount = Convert.ToString(dr["Number_Trees"])
                    });
                }
                #endregion

                #region Personal and Company Data
                // myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));

                _nc.personInfo = new PersonSW();
                _nc.personStr = Convert.ToString(ds.Tables[0].Rows[0]["SWCS_PersonInfo"]);
                _nc.personInfo = JsonConvert.DeserializeObject<PersonSW>(_nc.personStr);
                #endregion
            }
        }

        [HttpPost]
        public ActionResult Submit(NocData objModel)
        {
            int PDFdocumentCount = 1;
            var path = "";
            string FileName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
            try
            {
                DataTable dtGis = new DataTable("GisTable");
                dtGis.Columns.Add("Div_Cd", typeof(String));
                dtGis.Columns.Add("Dist_Cd", typeof(String));
                dtGis.Columns.Add("Blk_Cd", typeof(String));
                dtGis.Columns.Add("Gp_Cd", typeof(String));
                dtGis.Columns.Add("Vlg_Cd", typeof(String));
                dtGis.Columns.Add("areaName", typeof(String));
                dtGis.Columns.Add("khasraNo", typeof(String));
                dtGis.Columns.Add("FOREST_DIVCODE", typeof(String));
                dtGis.Columns.Add("Tehsil_Cd", typeof(String));

                if (objModel.gislist != null)
                {
                    foreach (clsPermission gis in objModel.gislist)
                    {
                        DataRow dr = dtGis.NewRow();
                        dr["Div_Cd"] = gis.Div_Cd;
                        dr["Dist_Cd"] = gis.Dist_Cd;
                        dr["Blk_Cd"] = gis.Blk_Cd;
                        dr["Gp_Cd"] = gis.Gp_Cd;
                        dr["Vlg_Cd"] = gis.Vlg_Cd;
                        dr["areaName"] = gis.areaName;
                        dr["khasraNo"] = gis.KhasraNo;
                        dr["FOREST_DIVCODE"] = gis.FOREST_DIVCODE;
                        dr["Tehsil_Cd"] = gis.Tehsil_Cd;
                        dtGis.Rows.Add(dr);
                    }
                }

                DataTable dtPlant = new DataTable("Plants");
                dtPlant.Columns.Add("ID", typeof(string));
                dtPlant.Columns.Add("Number_Trees", typeof(string));

                if (objModel.plantList != null)
                {
                    foreach (PlantFixedPermission item in objModel.plantList)
                    {
                        if (item.PlantCount != null && Convert.ToInt32(item.PlantCount) > 0)
                        {
                            DataRow dr = dtPlant.NewRow();
                            dr["ID"] = item.PlantID;
                            dr["Number_Trees"] = item.PlantCount;
                            dtPlant.Rows.Add(dr);
                        }
                    }
                }

                if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    FileName = Path.GetFileName(Request.Files[0].FileName);
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    objModel.Revenue_Record_Path = path;
                    Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));
                    PDFdocumentCount++;
                }
                if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                {
                    FileName = Path.GetFileName(Request.Files[1].FileName);
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    objModel.Revenue_Map_Path = path;
                    Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                    PDFdocumentCount++;
                }
                if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                {
                    FileName = Path.GetFileName(Request.Files[2].FileName);  ///// Amit Change (22-02-2016)
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    objModel.Additional_Document = path;
                    Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));
                    PDFdocumentCount++;
                }

                objModel.UserID = Convert.ToInt32(Session["Userid"].ToString());

                //  objModel.personInfo = JsonConvert.DeserializeObject<PersonSW>(objModel.personStr);
                if (objModel.personInfo == null)
                    objModel.personInfo = new PersonSW();

                objModel.personInfo.Mobile = objModel.personInfo.Mobile;
                objModel.personInfo.Email = objModel.personInfo.Email;
                objModel.personStr = JsonConvert.SerializeObject(objModel.personInfo);

                int id = fl.SubmitNocData(objModel, dtGis, dtPlant);

               // SetSSOSession(objModel.Userdetails, objModel.personInfo.Mobile, objModel.personInfo.Email); this case used in single window

                if (id > 0)
                {
                    objModel.Status = id;
                    #region File Operation
                    //if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml")) == true)
                    //{
                    //    System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["DistInfo"].ToString() + ".xml"));
                    //    Session["DistInfo"] = null;
                    //}
                    if (objModel.Amount > 0)
                    {
                        if (objModel.IsSWCS)
                        {
                            STATUSUPDATE.STATUSUPDATE statusupdate = new STATUSUPDATE.STATUSUPDATE();
                            string result = statusupdate.RSPCBREGNUMUPDATE(Convert.ToInt32(objModel.SWSID), objModel.RequestedID, 1);
                        }
                        if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                        {
                            #region Kiosk User
                            KioskPaymentDetails _obj = new KioskPaymentDetails();
                            _obj.ModuleId = 1;
                            _obj.ServiceTypeId = 1;
                            _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                            _obj.SubPermissionId = objModel.NOCType;
                            _obj.RequestedId = objModel.RequestedID;
                            DataTable dtKiosk = _obj.FetchKisokValue(_obj);

                            if (dtKiosk.Rows.Count > 0)
                            {
                                Session["EmitraDivCode"] = "DIV003";//dtGis.Rows[0]["FOREST_DIVCODE"].ToString();
                                _obj.RequestedId = objModel.RequestedID;
                                _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                return PartialView("KioskPaymentNOC", _obj);
                                //Session["EmitraDivCode"] = "DIV003";//dtGis.Rows[0]["FOREST_DIVCODE"].ToString();
                                //_obj.RequestedId = objModel.RequestedID;
                                //_obj.RevenueHead = "820-1000.00|840-53.00";//Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                //_obj.MerchantCode = "FOREST0716";//Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                //_obj.DepartmantalFees = 1000; //Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                //_obj.KMLCharges = 15;//Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                //_obj.DataEntryAndDocUploadFees = 35;//Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                //_obj.TotalAmount = 53;//Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                //return PartialView("KioskPaymentDetail", _obj);
                            }
                            #endregion
                        }
                        else if (Session["IsDepartmentalKioskUser"] != null && Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                        {
                            #region Department User
                            // Added by Arvind Kumar Sharma
                            PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();
                            _obj.ModuleId = 1;
                            _obj.ServiceTypeId = 1;
                            _obj.PermissionId = 1;
                            _obj.SubPermissionId = objModel.NOCType;
                            _obj.RequestedId = objModel.RequestedID;
                            _obj.PaidBy = objModel.kioskuserid;
                            _obj.PaidForCitizen = objModel.UserID;
                            _obj.PaidAmount = objModel.Final_Amount;
                            _obj.PaidOn = Convert.ToString(DateTime.Now);
                            return PartialView("PaymentByDepartmentalKioskUsers", _obj);
                            #endregion
                        }
                        else
                        {
                            // Session["FRequestId"] = objModel.RequestedID;
                            Session["Ftotalprice"] = objModel.Final_Amount;
                            Session["PermissionType"] = objModel.NOCTypeStr;
                            return View("NocPayment", objModel);
                        }
                    }
                    else
                    {
                        DataSet ds = fl.UpdatePaymentStatus(0, 1, "payUpdate", objModel.RequestedID, Convert.ToInt64(Session["UserId"].ToString()));
                        if (objModel.IsSWCS)
                        {
                            CallSWCS(objModel);
                        }
                    }
                    #endregion
                }

                #region Unknown Case (may be dept user)
                //{
                //    #region Else Part of Submit Command
                //    if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                //    {
                //        FileName = Path.GetFileName(Request.Files[1].FileName);
                //        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                //        path = Path.Combine(FilePath, FileFullName);
                //        _objmodel.Revenue_Record_Path = path;
                //        Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));
                //    }
                //    else
                //    { _objmodel.Revenue_Record_Path = FCollection["hdRevRecPath"].ToString(); }

                //    if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                //    {
                //        FileName = Path.GetFileName(Request.Files[2].FileName);
                //        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                //        path = Path.Combine(FilePath, FileFullName);
                //        _objmodel.Revenue_Map_Path = path;
                //        Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));
                //    }
                //    else
                //    { _objmodel.Revenue_Map_Path = FCollection["hdRevMapPath"].ToString(); }

                //    _objmodel.UserID = Convert.ToInt32(Session["UserId"]);
                //    _objmodel.RequestedID = command;
                //    _objmodel.UpdateData(_objmodel, "reassigned");
                //    return RedirectToAction("dashboard", "dashboard", new { messagetype = "3" });
                //    #endregion
                //} 
                #endregion

                return RedirectToAction("Nocs");
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        private ActionResult PaymentUpdate(NocData objModel)
        {
            if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
            {
                KioskPaymentDetails _obj = new KioskPaymentDetails();
                _obj.ModuleId = 1;
                _obj.ServiceTypeId = 1;
                _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                _obj.SubPermissionId = objModel.NOCType;
                _obj.RequestedId = objModel.RequestedID;
                DataTable dtKiosk = _obj.FetchKisokValue(_obj);

                if (dtKiosk.Rows.Count > 0)
                {
                    _obj.RequestedId = objModel.RequestedID;
                    _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                    _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                    _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                    _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                    _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                    _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                    return PartialView("KioskPaymentNoc", _obj);
                }
            }
            else if (Session["IsDepartmentalKioskUser"] != null && Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
                // Added by Arvind Kumar Sharma
                PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();
                _obj.ModuleId = 1;
                _obj.ServiceTypeId = 1;
                _obj.PermissionId = 1;
                _obj.SubPermissionId = objModel.NOCType;
                _obj.RequestedId = objModel.RequestedID;
                _obj.PaidBy = objModel.kioskuserid;
                _obj.PaidForCitizen = objModel.UserID;
                _obj.PaidAmount = objModel.Final_Amount;
                _obj.PaidOn = Convert.ToString(DateTime.Now);
                return PartialView("PaymentByDepartmentalKioskUsers", _obj);
            }
            else
            {
                //Session["FRequestId"] = objModel.RequestedID;
                Session["Ftotalprice"] = objModel.Final_Amount;
                Session["PermissionType"] = objModel.NOCTypeStr;

                return View("NocPayment", objModel);
            }
            return null;
        }

        public ActionResult List()
        {
            NocData nd = new NocData();
            DataSet ds = nd.GetAllRequests(Session["ssoId"].ToString());
            ViewBag.NOCMyAction = ToNocList(ds.Tables[2]);
            ViewBag.NOCPending = ToNocList(ds.Tables[1]);
            ViewBag.NOCToBeAssign = ToNocList(ds.Tables[0]);
            return View();
        }

        public ActionResult Nocs()
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            NocData nd = new NocData();
            DataSet ds = nd.GetAllRequests(null, UserId);
            ViewBag.NOCMyAction = ToNocList(ds.Tables[2]);
            ViewBag.NOCPending = ToNocList(ds.Tables[1]);
            ViewBag.NOCToBeAssign = ToNocList(ds.Tables[0]);
            return View();
            return View();
        }

        public ActionResult VerifyNoc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyNoc(string ReqId)
        {
            string origin = System.Configuration.ConfigurationManager.AppSettings["liveWeburl"].ToString();
            DataSet ds = new DataSet();
            SqlParameter[] param = { new SqlParameter("@RequestId", ReqId) };
            dl.Fill(ds, "KN_ValidateReq", param);

            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                sb.Append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                sb.Append("<th>Requested ID</th><th>Noc Type</th><th>Noc Purpose </th><th>Status</th> <th>Requested On</th><th>Status</th>");
                sb.Append("</tr></thead><tbody><tr>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["RequestedID"] + "</td>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["NocType"] + "</td>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["NocPurpose"] + "</td>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["Status"] + "</td>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["RequestOn"] + "</td>");
                var lnk = "<a href='" + ds.Tables[0].Rows[0]["Pdf"].ToString().Replace("~/", origin) + "' target='_blank' rel = 'noopener noreferrer'> Download </a>";
                sb.Append("<td>" + lnk + "</td>");
                sb.Append("</tr></tbody></table>");
            }
            else
            {
                sb.Append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                sb.Append("<td> No Record Found </td>");
                sb.Append("</tr></tbody></table>");
            }

            ViewBag.tbl = sb.ToString();

            return View();
        }

        public ActionResult VerifyPay()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyPay(string ReqId)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = { new SqlParameter("@RequestId", ReqId) };
            dl.Fill(ds, "KN_ValidatePayment", param);

            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                string payStatus = "Fail";
                if (ds.Tables[0].Rows[0]["Trn_Status_Code"].ToString() == "1" && Convert.ToInt32(ds.Tables[0].Rows[0]["Final_Amount"]) > 0)
                {
                    payStatus = "Success";
                }
                sb.Append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                sb.Append("<th>Requested ID</th><th>Noc Type</th><th>Amount</th><th>Status</th> <th>Requested On</th>");
                sb.Append("</tr></thead><tbody><tr>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["RequestedID"] + "</td>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["NocType"] + "</td>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["Final_Amount"] + "</td>");
                sb.Append("<td>" + payStatus + "</td>");
                // sb.Append("<td>" + ds.Tables[0].Rows[0]["RequestOn"] + "</td>");
                sb.Append("<td>" + ds.Tables[0].Rows[0]["RequestOn"] + "</td>");
                sb.Append("</tr></tbody></table>");
            }
            else
            {
                sb.Append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                sb.Append("<td> No Record Found </td>");
                sb.Append("</tr></tbody></table>");
            }

            ViewBag.tbl = sb.ToString();

            return View();
        }

        private List<NocList> ToNocList(DataTable dt)
        {
            List<NocList> lst = new List<NocList>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    NocList nobj = new NocList();
                    nobj.RequestId = (string)(dr["RequestId"] ?? "");
                    nobj.NocType = (string)(dr["NocType"] ?? "");
                    nobj.Status = (string)(dr["Status"] ?? "");
                    nobj.StatusId = Convert.ToInt32(dr["StatusId"]);
                    nobj.ReqDate = (string)(dr["ReqDate"] ?? "");
                    lst.Add(nobj);
                }
            }
            return lst;
            //return dt.AsEnumerable().Select(x => new NocList
            //{
            //    RequestId = (string)(x["RequestId"] ?? ""),
            //    NocType = (string)(x["NocType"] ?? ""),
            //    Status = (string)(x["Status"] ?? ""),
            //    StatusId = (int)(x["StatusId"] ?? 0),
            //    ReqDate = (string)(x["ReqDate"] ?? ""),
            //}).ToList();
        }

        public JsonResult getDetail(string requestId)
        {
            //NocData nd = new NocData();

            //var result = FMDSS.Models.Utility.DynamicSqlQuery(dbContext.Database, "spEncrocherDetailById @EN_Code", param1); 
            //return Json(result, JsonRequestBehavior.AllowGet);

            var param = new SqlParameter("@RequestId", requestId);
            var result = FMDSS.Models.Utility.DynamicSqlQuery(dbContext.Database, "KN_GetReqDetail @RequestId", param);
            //var result = dbContext.Database.SqlQuery<object>("spEncrocherDetailById @EN_Code", param1).ToList();

            //foreach (dynamic a in result)
            //{
            //    PropertyInfo[] properties = a.GetType().GetProperties();
            //    foreach (PropertyInfo property in properties)
            //    {
            //        string key = property.Name;
            //    }
            //    //foreach (Dictionary<string, string> kvp in ((List<Dictionary<string, string>>)a))
            //    //{
            //    //    Console.WriteLine("Key = {0}, Value = {1}", kvp.Keys, kvp.Values);
            //    //}
            //    string s = a[0].Status;
            //}
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public void getDataRows(string requestId)
        {
            string origin = System.Configuration.ConfigurationManager.AppSettings["liveWeburl"].ToString();

            DataSet ds = new DataSet();
            SqlParameter[] param = { new SqlParameter("@RequestId", requestId) };
            dl.Fill(ds, "KN_GetReqDetail", param);
            DataTable tbl = ds.Tables[0];
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn dc in tbl.Columns)
            {
                string colVal = Convert.ToString(tbl.Rows[0][dc.ColumnName]);
                if (!string.IsNullOrEmpty(colVal))
                {
                    if (colVal.Contains("~/"))
                    {
                        var lnk = "<a href='" + colVal.Replace("~/", origin) + "' target='_blank' rel = 'noopener noreferrer'> Download </a>";
                        sb.Append("<tr><td>" + dc.ColumnName + "</td><td>" + lnk + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td>" + dc.ColumnName + "</td><td>" + colVal + "</td></tr>");
                    }
                }
            }
            ViewBag.ReqData = sb.ToString();
        }

        public ActionResult Detail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AssignReq(string reqid, string ReqStatus, string cmd)
        {
            NocData noc = new NocData();
            noc.ReviewApprove(reqid, Session["ssoId"].ToString(), Convert.ToInt16(cmd), "", "SurveyDoc");
            UpdateSWCS(reqid, Convert.ToInt16(cmd));
            return RedirectToAction("List");

            //int ReqState = Convert.ToInt32(ReqStatus);
            //}
            //else if (ReqState == 11)
            //{
            //    noc.ReviewApprove(reqid, Session["ssoId"].ToString(), 7, "", "SurveyDoc");
            //}
            //else if (ReqState == 7)
            //{
            //    noc.ReviewApprove(reqid, Session["ssoId"].ToString(), 2, "", "SurveyDoc");
            //} 
        }

        public void UpdateSWCS(string reqId, int newStatus)
        {
            DAL dl = new DAL();

            SqlParameter[] param = { new SqlParameter("@RequestId", reqId) };
            DataSet dsReq = new DataSet();
            dl.Fill(dsReq, "spGetFixedPermissionData", param);
            if (dsReq != null && dsReq.Tables[0] != null && dsReq.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToBoolean(dsReq.Tables[0].Rows[0]["IsSWCS"]))
                {
                    STATUSUPDATE.STATUSUPDATE statusupdate = new STATUSUPDATE.STATUSUPDATE();
                    int AppServiceCode = Convert.ToInt32(dsReq.Tables[0].Rows[0]["P_Id"]);
                    //int newStatus = Convert.ToInt32(dsReq.Tables[0].Rows[0]["status"]);
                    //string result = statusupdate.statusupdate(19,AppServiceCode, reqId, newStatus, "Updated by " + name, "", "", "2");
                    string result = statusupdate.statusupdate(19, AppServiceCode, reqId, newStatus, "", "", "", "");
                }
            }
        }

        public void BindPageData(NocData _objmodel)
        {
            // ListPlant = new List<PlantFixedPermission>();
            DataSet dsPlant = fl.GetPlantList();
            List<PlantFixedPermission> ListPlant = dsPlant.Tables[0].AsEnumerable().Select(x => new PlantFixedPermission()
            {
                PlantID = Convert.ToInt64(x["ID"].ToString()),
                PlantName = x["Plant_Name"].ToString()
            }).ToList();

            if (_objmodel.plantList != null && _objmodel.plantList.Count() > 0)
            {
                foreach (PlantFixedPermission plt in _objmodel.plantList)
                    ListPlant.Find(x => x.PlantID == plt.PlantID).PlantCount = plt.PlantCount;
            }

            ViewBag.PlantList = ListPlant;
            if (_objmodel.ApplicantType > 0)
            {
                _objmodel.ApplicantTypeStr = Common.GetApplicantType().Where(x => Convert.ToInt32(x.Value) == _objmodel.ApplicantType).FirstOrDefault().Text;
            }
            else
                ViewBag.ApplicantType = new SelectList(Common.GetApplicantType(), "Value", "Text");

            BindMasterData _ObjMaster = new BindMasterData();
            List<SelectListItem> NOCPurposes = new List<SelectListItem>();
            DataTable dtNOCPurpose = _ObjMaster.GetFixedLandNOCPurpose();

            if (_objmodel.NOCPurpose > 0 && string.IsNullOrEmpty(_objmodel.NOCPurposeStr))
            {
                _objmodel.NOCPurposeStr = dtNOCPurpose.AsEnumerable().Where(x => x.Field<int>("NOCTypeId") == _objmodel.NOCPurpose).FirstOrDefault().Field<string>("NOCTypeName").ToString();
                _objmodel.NOCTypeStr = fl.GetPermissionTypes(_objmodel.NOCType).Tables[0].Rows[0]["Name"].ToString();
            }
            else
            {
                foreach (DataRow dr in dtNOCPurpose.Rows)
                {
                    NOCPurposes.Add(new SelectListItem { Text = @dr["NOCTypeName"].ToString(), Value = @dr["NOCTypeId"].ToString() });
                }
                ViewBag.NOCPurposes = NOCPurposes;
            }

            SawmillType obj1 = new SawmillType();
            IndustryType obj = new IndustryType();
            if ((!String.IsNullOrEmpty(_objmodel.Sawmill_Type)) && Convert.ToInt32(_objmodel.Sawmill_Type) > 0)
            {
                ViewBag.Sawmill_Types = new SelectList(obj1.GetSawmillType(), "Value", "Text", _objmodel.Sawmill_Type);
            }
            else
            {
                ViewBag.Sawmill_Types = new SelectList(obj1.GetSawmillType(), "Value", "Text");
            }

            if ((!String.IsNullOrEmpty(_objmodel.Industrial_Type)) && Convert.ToInt32(_objmodel.Industrial_Type) > 0)
            {
                ViewBag.Industrial_Types = new SelectList(obj.GetIndustryType(), "Value", "Text", _objmodel.Industrial_Type);
            }
            else
            {
                ViewBag.Industrial_Types = new SelectList(obj.GetIndustryType(), "Value", "Text");
            }

            DataSet Payds = new DataSet();
            Payds = fl.GetFinalAmount(1, _objmodel.NOCType);
            double discount = 0;
            double Amount = 0;
            double Tax = 0;
            double FinalAmount = 0;
            double DiscountedAmmount = 0;
            double TaxableAmount = 0;
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
        }

        public JsonResult GetOfficers()
        {
            List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
            ActionRequest actionRequest = new ActionRequest();
            DataTable dtOfficerDesignation = actionRequest.GetOfficerDesignation();
            if (dtOfficerDesignation.Rows.Count > 0)
            {
                foreach (System.Data.DataRow dr in dtOfficerDesignation.Rows)
                {
                    lstOfficerDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["EmpDesignation"].ToString() });
                }
            }
            else
            {
                lstOfficerDesignation.Add(new SelectListItem { Text = "NA", Value = "0" });
            }
            return Json(lstOfficerDesignation, JsonRequestBehavior.AllowGet);
        }

        #region Pay Button
        [HttpPost]
        public void Pay(NocData obj)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Session["FRequestId"] = null;
            Session["FRequestId"] = obj.RequestedID;
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    // string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();


                    ////EM33172142@5488
                    //Payment pay = new Payment();
                    //string encrypt = pay.RequestString("EM33172142@5488", Session["FRequestId"].ToString(), Session["Ftotalprice"].ToString(), ReturnUrl + "FixedPermission/ResponsePay", Session["User"].ToString(), "", "");
                    //Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
                    BookOnTicket OBJ = new BookOnTicket();
                    DataSet DS = new DataSet();
                    DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("CitizenNOCPermission", obj.RequestedID);

                    string REVENUEHEAD = Convert.ToString(DS.Tables[0].Rows[0]["REVENUEHEAD"]);

                    string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                    EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();


                    string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["FRequestId"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["ChecksumKey"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["EncryptionKey"]),
                        //ReturnUrl + "FixedPermission/ResponsePay", ReturnUrl + "FixedPermission/ResponsePay",
                         ReturnUrl + "NOC/Payment", ReturnUrl + "NOC/Payment",
                        Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]), Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]), REVENUEHEAD, Session["User"].ToString());


                    Response.Write(forms);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }
        #endregion

        public ActionResult Payment()
        {
            NocData _objmodel = new NocData();
            if (Session["NOCData"] != null)
            {
                _objmodel = Session["NOCData"] as NocData;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int fmdssStatus = 0;
            Session["UserDetailSingleWindow"] = null;
            Session["UserDetailSingleWindow"] = Session["SSOTOKEN"];
            if (Session["FRequestId"] != null)
            {
                try
                {
                    string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";
                    if (Request.Form["MERCHANTCODE"] != null)
                        MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
                    if (Request.Form["PRN"] != null)
                        PRN = Request.Form["PRN"].ToString();
                    if (Request.Form["STATUS"] != null)
                        STATUS = Request.Form["STATUS"].ToString();
                    if (Request.Form["ENCDATA"] != null)
                        ENCDATA = Request.Form["ENCDATA"].ToString();

                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    //  DecryptedData = "{'MERCHANTCODE':'FOREST0117','REQTIMESTAMP':'20170810001655000','PRN':'636379209711021628','AMOUNT':'10044.00','PAIDAMOUNT':'10050.00','SERVICEID':'2239','TRANSACTIONID':'170055011924','RECEIPTNO':'17053223840','EMITRATIMESTAMP':'20170810001853257','STATUS':'SUCCESS','PAYMENTMODE':'Kotak Bank Payment Gateway(All Banks)','PAYMENTMODEBID':'CTX170809184817579982','PAYMENTMODETIMESTAMP':'20170810001715000','RESPONSECODE':'200','RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.','UDF1':'CKNDR HASHIM','UDF2':'udf2','CHECKSUM':'03263f854d49bcdbf966ce9ffe494d26'}";
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    int status1 = 0;
                    BookOnTicket cs = new BookOnTicket();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    cs.UpdateEmitraResponse(Session["FRequestId"].ToString(), "CitizenNOCPermission", DecryptedData);

                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("EMITRA AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion

                    //****************************** for test only

                    //ObjPGResponse.STATUS = "SUCCESS";
                    ////ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                    ////ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                    ////ObjPGResponse.AMOUNT = Convert.ToString(Session["totalprice"]);
                    //ObjPGResponse.PAIDAMOUNT = Convert.ToString(Convert.ToDecimal(ObjPGResponse.AMOUNT) + Convert.ToDecimal(5));
                    //ObjPGResponse.TRANSACTIONID = DateTime.Now.Ticks.ToString();

                    //****************************** for test only;

                    string steps = string.Empty;
                    #region Response Status
                    if (ObjPGResponse.STATUS != "SUCCESS")
                    {
                        DataRow dtrow = dt.NewRow();
                        cs.EmitraTransactionId = "0";
                        cs.RequestId = ObjPGResponse.PRN;

                        if (Session["KioskUserId"] == "" || Session["KioskUserId"] == null)
                        {
                            cs.KioskUserId = "0";
                        }
                        else
                        {
                            cs.KioskUserId = Session["KioskUserId"].ToString();
                        }

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = "0";
                        dtrow["EMITRA AMOUNT"] = "0";
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name

                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED" || dtrow["TRANSACTION STATUS"].ToString() == "PENDING")
                        {
                            _objmodel.Trn_Status_Code = 0;
                        }
                        dt.Rows.Add(dtrow);

                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
                    {
                        DataRow dtrow = dt.NewRow();
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        cs.RequestId = ObjPGResponse.PRN;
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;
                        //dtrow["TRANSACTION BANK BID"] = finalbankid;   

                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            _objmodel.Trn_Status_Code = 1;
                        }
                        dt.Rows.Add(dtrow);
                    }
                    #endregion

                    ViewData.Model = dt.AsEnumerable();
                    _objmodel.UserName = Session["User"].ToString();
                    FixedLandUsage fl = new FixedLandUsage();
                    if (_objmodel.Trn_Status_Code == 1)
                    {

                        //ds = _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), _objmodel.Trn_Status_Code, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()), Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT));
                        DataSet ds = fl.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), _objmodel.Trn_Status_Code, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()), Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT));

                        #region "User Send Email"
                        string UserMailBody = Common.GenerateBody(_objmodel.Final_Amount, ds.Tables[0].Rows[0]["Name"].ToString(), ObjPGResponse.PRN, Session["PermissionType"].ToString() + " Permission");
                        string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), ObjPGResponse.PRN, Session["PermissionType"].ToString() + " Permission");
                        _objMailSMS.sendEMail("Payment Details for " + Session["PermissionType"].ToString() + " Permission", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

                        SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        #endregion
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            //#region "Is Reviwer Mail"
                            //string ReviwerMailBody = Common.GenerateReviwerBody(ds.Tables[1].Rows[0]["Name"].ToString(), _objmodel.TransactionId, ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission");
                            //_objMailSMS.sendEMail("Request for " + ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission", ReviwerMailBody, ds.Tables[1].Rows[0]["EmailId"].ToString(), string.Empty);
                            //#endregion
                        }
                        if (_objmodel.IsSWCS)
                        {
                            TempData["IsShowOrNotSingleWindow"] = 1;
                        }
                        return View("TransationStatusForNOC");
                        //return View("TransactionStatus");
                    }
                    else
                    {
                        TempData["IsShowOrNotSingleWindow"] = 0;
                        fl.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), status1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));
                        return View("TransationStatusForNOC");

                    }

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                return View("TransationStatusForNOC");
            }
            return View();
        }

        public void CallSWCSAfterSuccessandFailNOC()
        {
            NocData _objmodel = new NocData();
            if (Session["NOCData"] != null)
            {
                _objmodel = Session["NOCData"] as NocData;
                _objmodel.Status = 1;

                CallSWCS(_objmodel);
            }


            //String appName = "SWCS";
            //int reqStatus = _objmodel.Status == null ? 1 : _objmodel.Status == 8 ? 8 : 1;
            //String url = System.Configuration.ConfigurationManager.AppSettings["SWCSPath"].ToString() + "/servicelanding.aspx?SWSID=" + _objmodel.SWSID + "&ActID=" + _objmodel.NOCPurpose
            //    + "&ActivityID=" + _objmodel.NOCType + "&IsNew=" + _objmodel.IsNew + "&RegNo=" + Convert.ToString(Session["RegNo"]) + "&status=" + _objmodel.Status;

            //string poststring = SWCSHelper.posttopage(url, _objmodel.Userdetails, appName);
            //Response.Write(poststring);
            //Response.End();

            //STATUSUPDATE.STATUSUPDATE statusupdate = new STATUSUPDATE.STATUSUPDATE();
            //string result = statusupdate.statusupdate(19, 1, "565498", 2, "License Fees Paid And Application Submitted", "", "", "");  
        }

        //#region "Pay"

        //[HttpPost]
        //public void Pay(NocData obj)
        //{
        //    try
        //    {
        //        if (Session["UserID"] != null)
        //        {
        //            UserID = Convert.ToInt64(Session["UserID"].ToString());
        //            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
        //            Session["FRequestId"] = obj.RequestedID;

        //            //EM33172142@5488
        //            Payment pay = new Payment();
        //            string encrypt = pay.RequestString("EM33172142@5488", obj.RequestedID, obj.Final_Amount.ToString(), ReturnUrl + "Noc/ResponsePay", Session["User"].ToString(), "", "");
        //            Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }
        //}

        ///// <summary>
        ///// For Fetching the reponse from Payment gateway and save the status in Database
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ResponsePay()
        //{
        //    Payment pay = new Payment();
        //    int status1 = 0;
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

        //    try
        //    {
        //        if (Session["UserID"] != null)
        //        {
        //            UserID = Convert.ToInt64(Session["UserID"].ToString());

        //            DataTable dt = new DataTable();
        //            #region Datarow defination
        //            if (dt.Rows.Count == 0)
        //            {
        //                dt.Columns.Add("TRANSACTION STATUS");
        //                dt.Columns.Add("REQUEST ID");
        //                dt.Columns.Add("EMITRA TRANSACTION ID");
        //                dt.Columns.Add("TRANSACTION TIME");
        //                dt.Columns.Add("TRANSACTION AMOUNT");
        //                dt.Columns.Add("USER NAME");
        //                dt.Columns.Add("TRANSACTION BANK DETAILS");
        //                //dt.Columns.Add("TRANSACTION BANK BID");                    
        //            }
        //            #endregion
        //            string response = Request.QueryString["trnParams"].ToString();

        //            #region Response decryption
        //            string ResponseResult = pay.ProcesTranscationresponce(response);
        //            // string ResponseResult = "<RESPONSE RCPT_NO='0' TRN_TIME='' REQUEST_ID='636064207456763666' AMOUNT='488.0' OTHER_PARAM_1='MOHD ASIF' OTHER_PARAM_2='null' OTHER_PARAM_3='null' STATUS='FAILED'></RESPONSE>|9856EDAED93AE7FAACA16B490DBFFAFC0929C8EB5BFE14E14584A7D38DC483CA";
        //            string str1, str2;
        //            str1 = ResponseResult.Replace("<RESPONSE ", "");
        //            str2 = str1.Replace("></RESPONSE>", " ");
        //            //string[] Responsearr = str2.Split(' ');
        //            string[] Responsearr = System.Text.RegularExpressions.Regex.Split(str2, "' ");
        //            string checkFail = "STATUS='FAILED";
        //            string checkSucess = "STATUS='SUCCESS";
        //            string rowstatus1 = "";
        //            foreach (var item in Responsearr)
        //            {
        //                if (item.Equals(checkFail))
        //                {
        //                    string[] statusx = item.Split('=');
        //                    rowstatus1 = statusx[1].ToString();
        //                }
        //                if (item.Equals(checkSucess))
        //                {
        //                    string[] status2 = item.Split('=');
        //                    rowstatus1 = status2[1].ToString();
        //                }
        //            }
        //            int status1len = Convert.ToInt32(rowstatus1.ToString().Length);
        //            string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 1);
        //            #endregion

        //            #region Response Status
        //            if (finalstatus1 == "FAILED")
        //            {
        //                string[] emitratransid = Responsearr[0].Split('=');
        //                string[] transtime = Responsearr[1].Split('=');
        //                string[] reqid = Responsearr[2].Split('=');
        //                string[] reqamt = Responsearr[3].Split('=');
        //                string[] username = Responsearr[4].Split('=');
        //                string[] status = Responsearr[7].Split('=');


        //                DataRow dtrow = dt.NewRow();
        //                string rowstatus = status[1].ToString();
        //                int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
        //                string finalstatus = rowstatus.ToString().Substring(1, statuslen - 1);
        //                string rowreqid = reqid[1].ToString();
        //                int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
        //                string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 1);
        //                string rawemitra = emitratransid[1].ToString();
        //                int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
        //                string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 1);
        //                fl.TransactionId = finalemitraid;
        //                string rawtransamount = reqamt[1].ToString();
        //                int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
        //                string finalamount = rawtransamount.ToString().Substring(1, amountlen - 1);
        //                string rawusername = username[1].ToString();
        //                int usernamelen = Convert.ToInt32(rawusername.Length);
        //                string finalUserName = rawusername.ToString().Substring(1, usernamelen - 1);

        //                dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
        //                dtrow["REQUEST ID"] = finalreqid;
        //                dtrow["EMITRA TRANSACTION ID"] = "";
        //                dtrow["TRANSACTION TIME"] = "";//transtime[1];
        //                dtrow["TRANSACTION AMOUNT"] = finalamount;
        //                dtrow["USER NAME"] = finalUserName;

        //                if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
        //                {
        //                    fl.Trn_Status_Code = 0;
        //                }
        //                dt.Rows.Add(dtrow);
        //            }
        //            else if (finalstatus1 == "SUCCESS")
        //            {
        //                string[] emitratransid = Responsearr[0].Split('=');
        //                string[] transtime = Responsearr[1].Split('=');
        //                string[] reqid = Responsearr[2].Split('=');
        //                string[] reqamt = Responsearr[3].Split('=');
        //                string[] username = Responsearr[4].Split('=');
        //                string[] status = Responsearr[7].Split('=');
        //                string[] bank = Responsearr[8].Split('=');
        //                string[] bankbidno = Responsearr[10].Split('=');

        //                DataRow dtrow = dt.NewRow();
        //                //string rowstatus = status[1].ToString();
        //                int statuslen = Convert.ToInt32(status[1].ToString().Length);
        //                string finalstatus = status[1].ToString().Substring(1, statuslen - 1);
        //                //string rowreqid = reqid[1].ToString();
        //                int reqidlen = Convert.ToInt32(reqid[1].ToString().Length);
        //                string finalreqid = reqid[1].ToString().Substring(1, reqidlen - 1);
        //                //string rawemitra = emitratransid[1].ToString();
        //                int emitralen = Convert.ToInt32(emitratransid[1].ToString().Length);
        //                string finalemitraid = emitratransid[1].ToString().Substring(1, emitralen - 1);
        //                fl.TransactionId = finalemitraid;
        //                //string rawtransamount = reqamt[1].ToString();
        //                int amountlen = Convert.ToInt32(reqamt[1].ToString().Length);
        //                string finalamount = reqamt[1].ToString().Substring(1, amountlen - 1);

        //                int timelen = Convert.ToInt32(transtime[1].ToString().Length);
        //                string datetime = transtime[1].ToString().Substring(1, timelen - 1);
        //                //  string rawbank = bank[1].ToString();
        //                int banklen = Convert.ToInt32(bank[1].ToString().Length);
        //                string finalbank = bank[1].ToString().Substring(1, banklen - 1);

        //                int userlen = Convert.ToInt32(username[1].ToString().Length);
        //                string finalUsername = username[1].ToString().Substring(1, userlen - 1);

        //                //string rawbankbid = bankbidno[1].ToString();
        //                //int bankidlen = Convert.ToInt32(rawbankbid.Length);
        //                //string finalbankid = rawbankbid.ToString().Substring(1, bankidlen - 2);
        //                dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
        //                dtrow["REQUEST ID"] = finalreqid;
        //                dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
        //                dtrow["TRANSACTION TIME"] = datetime;
        //                dtrow["TRANSACTION AMOUNT"] = finalamount;
        //                dtrow["USER NAME"] = finalUsername;
        //                dtrow["TRANSACTION BANK DETAILS"] = finalbank;
        //                //dtrow["TRANSACTION BANK BID"] = finalbankid;   
        //                if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
        //                {
        //                    fl.Trn_Status_Code = 1;
        //                }
        //                dt.Rows.Add(dtrow);
        //            }
        //            #endregion


        //            ViewData.Model = dt.AsEnumerable();


        //            fl.UserName = Session["User"].ToString();

        //            if (fl.Trn_Status_Code == 1)
        //            {
        //                DataSet ds = new DataSet();

        //                ds = fl.UpdatePaymentStatus(Convert.ToInt64(fl.TransactionId), fl.Trn_Status_Code, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));

        //                #region "User Send Email"
        //                string UserMailBody = Common.GenerateBody(fl.Final_Amount, ds.Tables[0].Rows[0]["Name"].ToString(), fl.TransactionId, Session["PermissionType"].ToString() + " Permission");
        //                string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), fl.TransactionId, Session["PermissionType"].ToString() + " Permission");
        //                _objMailSMS.sendEMail("Payment Details for " + Session["PermissionType"].ToString() + " Permission", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

        //                SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
        //                #endregion
        //                if (ds.Tables[1].Rows.Count > 0)
        //                {
        //                    #region "Is Reviwer Mail"
        //                    string ReviwerMailBody = Common.GenerateReviwerBody(ds.Tables[1].Rows[0]["Name"].ToString(), fl.TransactionId, ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission");
        //                    _objMailSMS.sendEMail("Request for " + ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission", ReviwerMailBody, ds.Tables[1].Rows[0]["EmailId"].ToString(), string.Empty);
        //                    #endregion
        //                }
        //                return View("TransactionStatus");
        //            }
        //            else
        //            {
        //                fl.UpdatePaymentStatus(Convert.ToInt64(fl.TransactionId), status1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));
        //                return View("NOCFIlmORGTransactionStatus");

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }
        //    return null;


        //}

        //#endregion

        #region SWCS Actions
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

        [HttpPost]
        public JsonResult GetNOCType(int NOCPuproseId)
        {
            //string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
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
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(NOCPurpose, "Value", "Text"));

        }

        public void CallSWCS(NocData _objmodel)
        {
            Session["NOCData"] = null;
            String appName = "SWCS";
            int reqStatus = _objmodel.Status == 8 ? 8 : 1;
            String url = System.Configuration.ConfigurationManager.AppSettings["SWCSPath"].ToString() + "/servicelanding.aspx?SWSID=" + _objmodel.SWSID + "&ActID=" + _objmodel.NOCPurpose
                + "&ActivityID=" + _objmodel.NOCType + "&IsNew=" + _objmodel.IsNew + "&RegNo=" + _objmodel.RequestedID + "&status=" + _objmodel.Status;

            string poststring = SWCSHelper.posttopage(url, _objmodel.Userdetails, appName);
            Response.Write(poststring);
            Response.End();

            //STATUSUPDATE.STATUSUPDATE statusupdate = new STATUSUPDATE.STATUSUPDATE();
            //string result = statusupdate.statusupdate(19, 1, "565498", 2, "License Fees Paid And Application Submitted", "", "", "");  
        }

        [HttpPost]
        public void BackToSWCS(string UserDetail)
        {
            Session["NOCData"] = null;
            String appName = "SWCS";
            String url = System.Configuration.ConfigurationManager.AppSettings["SWCSPath"].ToString() + "/ssolanding.aspx";
            string poststring = SWCSHelper.posttopage(url, UserDetail, appName);
            Response.Write(poststring);
            Response.End();
        }

        [HttpPost]
        public void CancelSWCS(string Usrdetails)
        {
            //NocData swcsModel = new NocData();
            //if (Session["NOCData"] != null)
            //{
            //    swcsModel = Session["NOCData"] as NocData;
            //}
            Session["NOCData"] = null;
            String appName = "SWCS";
            String url = System.Configuration.ConfigurationManager.AppSettings["SWCSPath"].ToString() + "/ssolanding.aspx";
            string poststring = SWCSHelper.posttopage(url, Usrdetails, appName);
            Response.Write(poststring);
            Response.End();
        }


        #endregion


       
    }
}
