//*********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS)
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : SSO Integration file
//  Description  : Code File added for FMDSS integration with SSO system 
//  Date Created : 24-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  : Arvind Kumar Sharma
//  Modified On  : 02-Feb-2017
//  Modified Description  : After Update this file User Access and Layout Redirect  Compeltely Dynamice Now
//  Reviewed By  : 
//  Reviewed On  :
//*********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Services;
using System.Text;
using FMDSS.Models;
using System.Data;
using Newtonsoft.Json;
using FMDSS.Models.ForestProduction;
using FMDSS.App_Start;

namespace FMDSS
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        #region Data Members
        UserProfile UserInfo = new UserProfile();
        string[] qryStr = null;
        UserProfile User = null;
        bool flag = false;
        string msg = "";
        #endregion
        #region Member Functions
        /// <summary>
        /// Method responsible to connect with SSO and to fetch details of logged in User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] qryStr = null;
            string[] rtnUrl = null;
            string ipaddress = string.Empty;
            string returnUrl = string.Empty;
            if (Convert.ToString(Request.Headers["Host"]) == "localhost")
            {
                ipaddress = Convert.ToString(Request.Headers["Host"]);
                // Do what you want for localhost
            }
            else
            {
                if (ipaddress == "" || ipaddress == null)
                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            int MaxTimeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["UrlIdleMaxTimeLimit"].ToString());
            FmdssStaticData.RemoveIdleURL(MaxTimeLimit);
            bool isInCondition = false;

            if (Request.QueryString["returnURL"] != null)
            {
                if (Request.QueryString["returnURL"] != "")
                {
                    rtnUrl = Request.QueryString["returnURL"].ToString().Split('~');
                    Session["returnURL"] = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(rtnUrl[0]);
                    FmdssStaticData.AddReturnURL(ipaddress, FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(rtnUrl[0]));
                    isInCondition = true;
                }
            }
            
            if (Session["SSODetail"] == null && Request.QueryString["returnURL"] != null)
            {
                //if (Convert.ToBoolean(Session["IsLogged"]) == false)
                //{
                //Session["IsLogged"] = null;
                rtnUrl = Request.QueryString["returnURL"].ToString().Split('~');
                if (rtnUrl[1] == "y")
                {
                    if (Convert.ToInt32(ConfigurationManager.AppSettings["IsDevelopment"].ToString()) == 1)
                    {
                        Response.Redirect("~/Login/Login?returnURL=" + Request.QueryString["returnURL"]);
                    }
                    else
                        Response.Redirect("https://ssotest.rajasthan.gov.in/signin?encq=D1RZ1LiY9YiA4+sGzbkUhImAUqAaWlAl3eKvmWwulpY=", false);                    
                }
                else
                    Response.Redirect("https://ssotest.rajasthan.gov.in/register", false);

            }
           

            if (isInCondition == false)
            {
                returnUrl = FmdssStaticData.GetReturnURLByIp(ipaddress);
                Session["returnURL"] = returnUrl;
            }
            try
            {
                DataSet DS = new DataSet();
                UserRolesMenusDetails OBJUserRolesMenusDetails = new UserRolesMenusDetails();
                List<ROLEGROUPS> ROLEGROUPS = new List<ROLEGROUPS>();
                List<Menus> Menus = new List<Menus>();

                #region Call Random No And Ip_Address
                string Create16Digit = Create16DigitString();
                DataTable UserProfileSessionMaintainDataset = new DataTable();
                Session["Create16DigitSession"] = Create16Digit;

                //string ipaddress;
                //ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                //if (ipaddress == "" || ipaddress == null)
                //    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                #endregion
                string rtnQry = "";
                qryStr = new string[1];
                if (Request.QueryString["val"] == null)
                {
                    rtnQry = "";
                }
                else
                {
                    if (Request.QueryString["val"].ToString().ToLower().IndexOf('&') != -1)
                    {
                        qryStr = Request.QueryString["val"].ToString().Split('&');
                        rtnQry = qryStr[0];
                    }
                    else
                    {
                        rtnQry = Request.QueryString["val"];
                    }                   
                }



                if (rtnQry == "Staging")
                {

                    Session["PlaceSelectTime"] = null;      //add for place select before 10 am
                    #region Staging Case
                    //string aa = Session["User"].ToString();
                    if (Session["SSODetail"] != null)
                    {
                        User = (UserProfile)Session["SSODetail"];

                        if (User.isRedirected == false && User.Designation != "11" &&  User.Designation != "12"  && (Session["returnURL"] == null || Session["returnURL"].ToString() ==""))
                        {
                            Response.Redirect("~/MiddleWare/MiddleWareMainModules");
                        }

                        //Check Blocked User 13-12-2021 Mukesh Kumar Jangid
                        bool flgBlockUser = false;
                        string ip = GetIp();
                        //-----------------------------Single Browser and Single Ip Login-----------------------------------
                        if (FmdssStaticData.IsAlreadyLogged(ref msg, User.SSOId, ip, User.MobileNumber) == true)
                        {
                            int ResetTimeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["ResetLoginTimeInterval"].ToString());
                            var diffInSeconds1 = (DateTime.Now - FmdssStaticData.GetLoginTime(User.SSOId, ip)).TotalSeconds;
                            int diffMinuts = (diffInSeconds1 > 0 ? (int)diffInSeconds1 / 60 : 0);
                            
                            if (diffMinuts > ResetTimeLimit)
                            {                              
                                FmdssStaticData.RemoveLoggedUsers(User.SSOId,ip);                               
                            }
                        }

                        if (FmdssStaticData.GetIPAddressCount(ip) > 1)
                        {
                            FmdssStaticData.RemoveLoggedUsers(User.SSOId,"");
                            Response.Cookies.Clear();
                            Session.Clear();//clear session
                            Session.Abandon();
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                            Response.Cache.SetNoStore();
                            Response.Redirect("~/LoginStatus.html", true);
                        }

                        if (FmdssStaticData.IsAlreadyLogged(ref msg, User.SSOId, ip, User.MobileNumber) == false)
                        {

                            FmdssStaticData.AddLoggedUsers(User.SSOId, ip, User.MobileNumber);
                        }
                        else
                        {
                           
                            if (FmdssStaticData.GetBrowserName(User.SSOId, ip, User.MobileNumber) == FmdssStaticData.GetCurrentBrowser() && FmdssStaticData.GetLoggedIPAddress(User.SSOId, User.MobileNumber)== ip)
                            {
                                FmdssStaticData.RemoveLoggedUsers(User.SSOId);
                                FmdssStaticData.AddLoggedUsers(User.SSOId, ip, User.MobileNumber);
                            }
                            else
                            {
                                Response.Cookies.Clear();
                                Session.Clear();//clear session
                                Session.Abandon();
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                                Response.Cache.SetNoStore();
                                Response.Redirect("~/LoginStatus.html", true);
                            }
                        }
                        if (FmdssStaticData.GetIPAddressCount(ip) > 1 || FmdssStaticData.GetUserLoggedCount(User.SSOId, User.MobileNumber) > 1)
                        {
                            FmdssStaticData.RemoveLoggedUsers(User.SSOId);
                            Response.Cookies.Clear();
                            Session.Clear();//clear session
                            Session.Abandon();
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                            Response.Cache.SetNoStore();
                            Response.Redirect("~/LoginStatus.html", true);
                        }
                        
                            //-----------------------------Single Browser and Single Ip Login-----------------------------------
                            DataSet dataSet = User.FindBlockedStatus(User.SSOId, ip, User.MobileNumber, User.EmailId);
                        if (dataSet != null)
                        {
                            if (dataSet.Tables[0] != null)
                            {
                                if (dataSet.Tables[0].Rows.Count > 0)
                                {
                                    if (dataSet.Tables[0].Rows[0][0].ToString().Trim().Length > 0)
                                        flgBlockUser = true;
                                }
                            }
                            if (dataSet.Tables[1] != null)
                            {
                                if (dataSet.Tables[1].Rows.Count > 0)
                                {
                                    if (dataSet.Tables[1].Rows[0][0].ToString().Trim().Length > 0)
                                        flgBlockUser = true;
                                }
                            }
                            if (dataSet.Tables[2] != null)
                            {
                                if (dataSet.Tables[2].Rows.Count > 0)
                                {
                                    if (dataSet.Tables[2].Rows[0][0].ToString().Trim().Length > 0)
                                        flgBlockUser = true;
                                }
                            }
                            if (dataSet.Tables[3] != null)
                            {
                                if (dataSet.Tables[3].Rows.Count > 0)
                                {
                                    if (dataSet.Tables[3].Rows[0][0].ToString().Trim().Length > 0)
                                        flgBlockUser = true;
                                }
                            }
                        }
                        if (flgBlockUser == true)
                        {
                            Response.Cookies.Clear();
                            Session.Clear();//clear session
                            Session.Abandon();
                            Response.Redirect("~/HtmlBlock.html", true);
                        }

                        User.IsSSO = true;
                        User.IsBhamashah = false;

                        #region Multiple Office Check
                        DataTable dt = User.GetMultipleOffice(User.SSOId);
                        if (dt != null && dt.Rows.Count > 1)
                        {
                            Session["IsStaging"] = "Staging";
                            List<MultiOffice> lstOfc = new List<MultiOffice>();
                            string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                            lstOfc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MultiOffice>>(str);
                            Session["lstOfc"] = null;
                            Session["lstOfc"] = lstOfc;
                            Response.Redirect("~/Office/Index", false);
                        }
                        #endregion
                        else
                        {

                            //#region Check Duplicate User Not login other PC
                            //RAJSSO.SSO sso = new RAJSSO.SSO();
                            //string strings = sso.GetSessionValue();
                            ////RAJSSO.SSOWS.GetTokenDetailJSONCompletedEventArgs abc=new RAJSSO.SSOWS.GetTokenDetailJSONCompletedEventArgs("");

                            //#endregion

                            #region new code added by mukesh sir 6/2/2022
                            if (Session["From_2_0"] != null)
                            {
                                if (Session["From_2_0"].ToString() == "Yes")
                                {
                                    //New User Info from FMDSS 2.0 Detail.
                                    //Check User Info in tbl_Userprofile if Not Exist then
                                    //Insert Identity On Identity Off
                                    UserProfile user = Session["SSODetail"] as UserProfile;
                                    User.FatchedUserInfo_FMDSS2_0_Insert_Update(user);

                                }

                            }
                            #endregion

                            DS = User.InsertUpdateUserInfo();
                            

                            UserProfileSessionMaintainDataset = User.UserProfileSessionMaintain("INSERT", User.SSOId, Create16Digit, ipaddress);//Add by Rajveer
                            Session["Create16DigitSession"] = Create16Digit;
                            bool flag = Convert.ToBoolean(DS.Tables[0].Rows[0]["FIRSTTIMELOGIN"]);
                            Session["UserId"] = Convert.ToInt64(DS.Tables[0].Rows[0]["USERID"]);
                            Session["DesignationId"] = Convert.ToInt64(DS.Tables[0].Rows[0]["DESIGNATION"]);
                            Session["DesignationDes"] = DS.Tables[0].Rows[0]["DESIG_NAME"].ToString();
                            Session["AadharID"] = Convert.ToString(DS.Tables[0].Rows[0]["AadharID"]);
                            Session["AlertList"] = Get_LatestRequest();
                            //Session["AlertListNotice"] = Get_LatestPublishedno();
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
                            // Change By Arvind Kumar Sharma // For Dynamic Access Selections
                            if (DS.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow dr in DS.Tables[1].Rows)
                                {
                                    ROLEGROUPS.Add(
                                    new ROLEGROUPS()
                                    {
                                        RoleId = Convert.ToInt16(dr["RoleId"]),
                                        RoleName = Convert.ToString(dr["RoleName"]),
                                        RolePriority = Convert.ToInt16(dr["RolePriority"]),
                                        DefaultPage = Convert.ToString(dr["DefaultPage"]),
                                        DefaultLayout = Convert.ToString(dr["Layout"]),
                                    });
                                }
                                foreach (DataRow dr in DS.Tables[2].Rows)
                                {
                                    Menus.Add(
                                    new Menus()
                                    {
                                        UserID = Convert.ToInt64(dr["UserID"]),
                                        SSOID = Convert.ToString(dr["SSOID"]),
                                        RoleId = Convert.ToInt16(dr["RoleId"]),
                                        RoleName = Convert.ToString(dr["RoleName"]),
                                        PageTitle = Convert.ToString(dr["PageTitle"]),
                                        PageURL = Convert.ToString(dr["PageURL"]),
                                        IsActive = Convert.ToString(dr["IsActive"]),
                                        IconClass = Convert.ToString(dr["IconClass"]),
                                        isIcon = Convert.ToString(dr["isIcon"]),
                                        Layout = Convert.ToString(dr["Layout"]),
                                        PageID = Convert.ToInt64(dr["PageID"]),
                                        ParentID = Convert.ToInt64(dr["ParentID"]),
                                        IsNested = Convert.ToBoolean(dr["IsNested"]),
                                        IsTargetBlank = Convert.ToBoolean(dr["IsTargetBlank"]),
                                    });
                                }
                                StringBuilder SB = new StringBuilder();
                                if (DS.Tables[3].Rows.Count > 0)
                                {
                                    SB.Append("<li><div class='col-lg-12'><div style='font-size: 12px; border-top: #337ab7 3px solid;'>");
                                    SB.Append("<section class='panel' style='font-size:12px;'>");
                                    SB.Append("<div class='panel-body'style='padding:0px;'><div class='task-thumb-details' style='text-align: center; '>");
                                    SB.Append("<div style=' text-align :center; font-size :18px; color :#337ab7;'>" + DS.Tables[3].Rows[0]["Name"] + "</div>");
                                    SB.Append("<span  style ='text-align: center; color: #337ab7;'>" + DS.Tables[3].Rows[0]["Desig_Name"] + "</div></div></div>");
                                    SB.Append("<table class='table'><tbody>");
                                    SB.Append("<tr><td>SSO ID</td><td>" + DS.Tables[3].Rows[0]["Ssoid"] + "</td></tr>");
                                    SB.Append("<tr><td>Department</td><td>" + DS.Tables[3].Rows[0]["Department"] + "</td></tr>");
                                    SB.Append("<tr><td>Office</td><td>" + DS.Tables[3].Rows[0]["OfficeName"] + "</td></tr>");
                                    SB.Append("<tr><td>Role</td><td>" + DS.Tables[3].Rows[0]["Roles"] + "</td></tr>");
                                    SB.Append("<tr><td>Mobile No</td><td>" + DS.Tables[3].Rows[0]["Mobile"] + "</td></tr>");
                                    SB.Append("<tr><td>Email Id</td><td>" + DS.Tables[3].Rows[0]["EmailId"] + "</td></tr>");
                                    SB.Append("</tbody></table></section></div></div> </li>");
                                    Session["UserProfile"] = SB.ToString();
                                }
                                else
                                {
                                    Session["UserProfile"] = null;
                                }
                                OBJUserRolesMenusDetails.ROLEGROUPS = ROLEGROUPS;
                                OBJUserRolesMenusDetails.Menus = Menus;
                                Session["CURRENT_Menus"] = null;
                                Session["ROLEGROUPS"] = ROLEGROUPS;
                                Session["Menus"] = Menus;
                                Session["CURRENT_ROLE"] = Convert.ToString(DS.Tables[1].Rows[0]["RoleId"]);

                                ///// Check User Session 07/11/2019
                                #region "User Session check "
                                cls_UserSession oUserSession = new cls_UserSession();
                                FMDSS.Models.cls_UserSession.clsUserSession oS = new FMDSS.Models.cls_UserSession.clsUserSession();
                                oS.IPAddress = ipaddress;
                                oS.SessionId = Session.SessionID;
                                oS.SessionTimeOut = Session.Timeout.ToString();
                                oS.SSOId = Convert.ToString(Session["SSOID"]);
                                string Status = Convert.ToString(oUserSession.SaveUserLogs(oS));

                                if (Status == "True")
                                    Response.Redirect(Convert.ToString(DS.Tables[1].Rows[0]["DefaultPage"]), false);
                                else
                                    Response.Redirect("errorbySSO.html");
                                #endregion
                                ////// End User Session 
                               

                               //Response.Redirect(Convert.ToString(DS.Tables[1].Rows[0]["DefaultPage"]), false);
                            }
                            else
                            {
                                Response.Redirect("~/Office/Index", false);
                            }
                        }
                        // end
                        // Change By Arvind Kumar Sharma // For Dynamic Access Selections
                    } 
                    #endregion
                }
                else
                {
                    #region Production
                    if (Request.QueryString["val"] != null)
                    {
                        string value = "";
                        qryStr = new string[1];
                        if (Request.QueryString["val"] == null)
                        {
                            value = "";
                        }
                        else
                        {
                            if (Request.QueryString["val"].ToString().ToLower().IndexOf('&') != -1)
                            {
                                qryStr = Request.QueryString["val"].ToString().Split('&');
                                value = qryStr[0];
                            }
                            else
                            {
                                value = Request.QueryString["val"];
                            }                           
                        }

                        Session["PlaceSelectTime"] = null;
                       // string value =  Request.QueryString["val"];
                        if (value == "backtosso")
                        {
                            BackToSSO();
                        }
                        else if (value == "logout")
                        {
                            Logout();
                        }
                        else if (value == "Grievance" && Session["SSOTOKEN"] != null)
                        {
                            //posttopage("http://10.68.5.236/index.aspx", Encryption.encrypt(Session["SSOTOKEN"].ToString()), "Sampark");
                            posttopage(Convert.ToString(ConfigurationManager.AppSettings["Sampark"]), Encryption.encrypt(Session["SSOTOKEN"].ToString()), "Sampark");
                        }
                        else if (value == "RTI" && Session["SSOTOKEN"] != null)
                        {
                            posttopage(Convert.ToString(ConfigurationManager.AppSettings["RTI"]), Encryption.encrypt(Session["SSOTOKEN"].ToString()), "RTI");
                        } 
                    }
                    if (!IsPostBack)
                    {
                        Session["encDataMultiOffice"]=null;
                        Session["encDataMultiOffice"]=Convert.ToString(Request.Form["encData"]);
                        Session.Clear();

                        if (isInCondition == false && returnUrl!="" && returnUrl.Trim().Length>0)
                        {
                            Session["returnURL"] = returnUrl;
                        }
                          

                        dynamic oObj;
                        if (Request.Form != null && Convert.ToString(Request.Form.Keys[0]).Equals("encData"))
                        {
                            string decryptString = string.Empty;
                            string sData = HttpUtility.UrlDecode(Convert.ToString(Request.Form["encData"]), Encoding.UTF8);
                            decryptString = Common.Decrypt(sData.Replace(" ", "+"), "E-m!tr@2016");
                            FMDSS.Models.UserProfile.eMitraObject _objEmitra = null;
                            if (decryptString != null)
                                _objEmitra = JsonConvert.DeserializeObject<FMDSS.Models.UserProfile.eMitraObject>(decryptString);

                            Session["SSOTOKEN"] = _objEmitra.SSOTOKEN;
                            Session["EmitrServiceId"] = _objEmitra.SERVICEID;
                            Session["SSOID"] = _objEmitra.SSOID;
                            Session["Role"] = "KIOSK";
                            Session["loggedin"] = true;
                            Session["User"] = _objEmitra.KIOSKNAME;
                            Session["EmitraRETURNURL"] = _objEmitra.RETURNURL;
                            User = new UserProfile()
                            {
                                SSOId = _objEmitra.SSOID,
                                FullName = _objEmitra.KIOSKNAME,
                                EmailId = _objEmitra.EMAIL,
                                MobileNumber = _objEmitra.MOBILE,
                                Designation = "11",
                                Address1 = _objEmitra.WARD + "," + _objEmitra.VILLAGE + "," + _objEmitra.TEHSIL,
                                PINCode1 = _objEmitra.PINCODE,
                                District1 = _objEmitra.DISTRICT,
                                Roles = Session["Role"].ToString(),
                                IsSSO = false,
                                IsBhamashah = false
                            };
                        }
                        else
                        {
                            RAJSSO.SSO SSO = new RAJSSO.SSO();
                            if (!string.IsNullOrEmpty(Request.Form["userdetails"]))
                            {
                                Session["SSOTOKEN"] = Convert.ToString(Request.Form["userdetails"]);
                                Session["SSOTOKENMultOffices"]=null;
                                Session["SSOTOKENMultOffices"] = Convert.ToString(Request.Form["userdetails"]);
                                //// Response.Cookies["RAJSSO"]=RAJSSO
                                //RAJSSO.SSOWS.SSOTokenDetail detail = SSO.GetSessionValueXml();
                               
                                //#region Check User Already Login Or Not
                                //if (!string.IsNullOrEmpty(detail.sAMAccountName))
                                //{
                                //    Response.Redirect("~/Office/UserLogin");
                                //}
                                //#endregion

                                SSO.CreateSSOSession();
                                SSO.ClientSideSession();
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
                                }
                            }
                        }
                        Session["SSODetail"] = User;
                        User = (UserProfile)Session["SSODetail"];

                       
                        //Check Blocked User 13-12-2021 Mukesh Kumar Jangid
                        bool flgBlockUser = false;
                        string ip = GetIp();
                        
                        //-----------------------------Single Browser and Single Ip Login-----------------------------------
                        if (FmdssStaticData.IsAlreadyLogged(ref msg, User.SSOId, ip, User.MobileNumber) == true)
                        {
                            int ResetTimeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["ResetLoginTimeInterval"].ToString());
                            var diffInSeconds1 = (DateTime.Now - FmdssStaticData.GetLoginTime(User.SSOId, ip)).TotalSeconds;
                            int diffMinuts = (diffInSeconds1 > 0 ? (int)diffInSeconds1 / 60 : 0);

                            if (diffMinuts > ResetTimeLimit)
                            {                              
                                FmdssStaticData.RemoveLoggedUsers(User.SSOId, ip);
                            }
                        }

                        if (FmdssStaticData.GetIPAddressCount(ip) > 1)
                        {
                            FmdssStaticData.RemoveLoggedUsers(User.SSOId);
                            Response.Cookies.Clear();
                            Session.Clear();//clear session
                            Session.Abandon();
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                            Response.Cache.SetNoStore();
                            Response.Redirect("~/LoginStatus.html", true);
                        }

                        if (FmdssStaticData.IsAlreadyLogged(ref msg, User.SSOId, ip, User.MobileNumber) == false)
                        {

                            FmdssStaticData.AddLoggedUsers(User.SSOId, ip, User.MobileNumber);
                        }
                        else
                        {

                            if (FmdssStaticData.GetBrowserName(User.SSOId, ip, User.MobileNumber) == FmdssStaticData.GetCurrentBrowser() && FmdssStaticData.GetLoggedIPAddress(User.SSOId, User.MobileNumber) == ip)
                            {
                                FmdssStaticData.RemoveLoggedUsers(User.SSOId);
                                FmdssStaticData.AddLoggedUsers(User.SSOId, ip, User.MobileNumber);
                            }
                            else
                            {
                                Response.Cookies.Clear();
                                Session.Clear();//clear session
                                Session.Abandon();
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                                Response.Cache.SetNoStore();
                                Response.Redirect("~/LoginStatus.html", true);
                            }
                        }
                        if (FmdssStaticData.GetIPAddressCount(ip) > 1 || FmdssStaticData.GetUserLoggedCount(User.SSOId, User.MobileNumber) > 1)
                        {
                            FmdssStaticData.RemoveLoggedUsers(User.SSOId);
                            Response.Cookies.Clear();
                            Session.Clear();//clear session
                            Session.Abandon();
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                            Response.Cache.SetNoStore();
                            Response.Redirect("~/LoginStatus.html", true);
                        }

                        //-----------------------------Single Browser and Single Ip Login-----------------------------------

                        
                        DataSet dataSet = User.FindBlockedStatus(User.SSOId, ip, User.MobileNumber, User.EmailId);
                        if (dataSet != null)
                        {
                            if (dataSet.Tables[0] != null)
                            {
                                if (dataSet.Tables[0].Rows.Count > 0)
                                {
                                    if (dataSet.Tables[0].Rows[0][0].ToString().Trim().Length > 0)
                                        flgBlockUser = true;
                                }
                            }
                            if (dataSet.Tables[1] != null)
                            {
                                if (dataSet.Tables[1].Rows.Count > 0)
                                {
                                    if (dataSet.Tables[1].Rows[0][0].ToString().Trim().Length > 0)
                                        flgBlockUser = true;
                                }
                            }
                            if (dataSet.Tables[2] != null)
                            {
                                if (dataSet.Tables[2].Rows.Count > 0)
                                {
                                    if (dataSet.Tables[2].Rows[0][0].ToString().Trim().Length > 0)
                                        flgBlockUser = true;
                                }
                            }
                            if (dataSet.Tables[3] != null)
                            {
                                if (dataSet.Tables[3].Rows.Count > 0)
                                {
                                    if (dataSet.Tables[3].Rows[0][0].ToString().Trim().Length > 0)
                                        flgBlockUser = true;
                                }
                            }
                        }
                        if (flgBlockUser == true)
                        {

                            Response.Cookies.Clear();
                            Session.Clear();//clear session
                            Session.Abandon();
                            Response.Redirect("~/HtmlBlock.html", true);
                            //Response.Redirect("~/error.html", true);
                            //Response.Redirect("~/Office/Index", false);
                        }
                        //Check Blocked User 13-12-2021 Mukesh Kumar Jangid
                        #region Multiple Office Check
                        DataTable dt = User.GetMultipleOffice(User.SSOId);
                        if (dt != null && dt.Rows.Count > 1)
                        {
                            
                            if (User.isRedirected == false && User.Designation != "11" &&  User.Designation != "12"  && (Session["returnURL"] == null || Session["returnURL"].ToString() == ""))
                            {
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (dt.Rows[j]["AnotherSSOID"].ToString().ToLower() == User.SSOId.ToLower())
                                    {
                                        Session["UserId"] = dt.Rows[j]["UserID"];
                                        Session["SSOId"] = User.SSOId;                                        
                                    }
                                }
                           
                                Response.Redirect("~/MiddleWare/MiddleWareMainModules");
                            }
                            else
                            {
                                Session["IsStaging"] = "Production";
                                List<MultiOffice> lstOfc = new List<MultiOffice>();
                                string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                                lstOfc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MultiOffice>>(str);
                                Session["lstOfc"] = null;
                                Session["lstOfc"] = lstOfc;
                                Response.Redirect("~/Office/Index", false);
                            }
                            
                        }
                        #endregion
                        else
                        {
                            #region new code added by mukesh sir 6/2/2022
                            if (Session["From_2_0"] != null)
                            {
                                if (Session["From_2_0"].ToString() == "Yes")
                                {
                                    //New User Info from FMDSS 2.0 Detail.
                                    //Check User Info in tbl_Userprofile if Not Exist then
                                    //Insert Identity On Identity Off
                                    UserProfile user = Session["SSODetail"] as UserProfile;
                                    User.FatchedUserInfo_FMDSS2_0_Insert_Update(user);

                                }

                            }
                            #endregion
                            DS = User.InsertUpdateUserInfo();
                            UserProfileSessionMaintainDataset = User.UserProfileSessionMaintain("INSERT", User.SSOId, Create16Digit, ipaddress);//Add by Rajveer
                            
                            Session["Create16DigitSession"] = Create16Digit;
                            bool flag = Convert.ToBoolean(DS.Tables[0].Rows[0]["FIRSTTIMELOGIN"]);
                            Session["UserId"] = Convert.ToInt64(DS.Tables[0].Rows[0]["USERID"]);
                            Session["DesignationId"] = Convert.ToInt64(DS.Tables[0].Rows[0]["DESIGNATION"]);
                            Session["DesignationDes"] = DS.Tables[0].Rows[0]["DESIG_NAME"].ToString();
                            Session["AadharID"] = Convert.ToString(DS.Tables[0].Rows[0]["AadharID"]);
                            Session["AlertList"] = Get_LatestRequest();
                            //Session["AlertListNotice"] = Get_LatestPublishedno();
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
                            // Change By Arvind Kumar Sharma // For Dynamic Access Selections
                            if (DS.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow dr in DS.Tables[1].Rows)
                                {
                                    ROLEGROUPS.Add(
                                    new ROLEGROUPS()
                                    {
                                        RoleId = Convert.ToInt16(dr["RoleId"]),
                                        RoleName = Convert.ToString(dr["RoleName"]),
                                        RolePriority = Convert.ToInt16(dr["RolePriority"]),
                                        DefaultPage = Convert.ToString(dr["DefaultPage"]),
                                        DefaultLayout = Convert.ToString(dr["Layout"]),
                                    });
                                }
                                foreach (DataRow dr in DS.Tables[2].Rows)
                                {
                                    Menus.Add(
                                    new Menus()
                                    {
                                        UserID = Convert.ToInt64(dr["UserID"]),
                                        SSOID = Convert.ToString(dr["SSOID"]),
                                        RoleId = Convert.ToInt16(dr["RoleId"]),
                                        RoleName = Convert.ToString(dr["RoleName"]),
                                        PageTitle = Convert.ToString(dr["PageTitle"]),
                                        PageURL = Convert.ToString(dr["PageURL"]),
                                        IsActive = Convert.ToString(dr["IsActive"]),
                                        IconClass = Convert.ToString(dr["IconClass"]),
                                        isIcon = Convert.ToString(dr["isIcon"]),
                                        Layout = Convert.ToString(dr["Layout"]),
                                        PageID = Convert.ToInt64(dr["PageID"]),
                                        ParentID = Convert.ToInt64(dr["ParentID"]),
                                        IsNested = Convert.ToBoolean(dr["IsNested"]),
                                        IsTargetBlank = Convert.ToBoolean(dr["IsTargetBlank"]),
                                    });
                                }
                                string FinalImage = "data:image/png;base64," + User.PhotURL;
                                StringBuilder SB = new StringBuilder();
                                if (DS.Tables[3].Rows.Count > 0)
                                {
                                    SB.Append("<li><div class='col-lg-12'><div style='font-size: 12px; border-top: #337ab7 3px solid;'>");
                                    SB.Append("<section class='panel' style='font-size:12px;'>");
                                    SB.Append("	<center><div class='profile-userpic'  ><img class='img-circle' src=" + FinalImage + " width='80' height='80' class='img-responsive' alt=''></center></div>");
                                    SB.Append("<div class='panel-body'style='padding:0px;'><div class='task-thumb-details' style='text-align: center; '>");
                                    SB.Append("<div style=' text-align :center; font-size :18px; color :#337ab7;'>" + DS.Tables[3].Rows[0]["Name"] + "</div>");
                                    SB.Append("<span  style ='text-align: center; color: #337ab7;'>" + DS.Tables[3].Rows[0]["Desig_Name"] + "</div></div></div>");
                                    SB.Append("<table class='table'><tbody>");
                                    SB.Append("<tr><td>SSO ID</td><td>" + DS.Tables[3].Rows[0]["Ssoid"] + "</td></tr>");
                                    ////SB.Append("<tr><td>Department</td><td>" + DS.Tables[3].Rows[0]["Department"] + "</td></tr>");
                                    if (Convert.ToString(DS.Tables[3].Rows[0]["OfficeName"]) != string.Empty)
                                    {
                                        SB.Append("<tr><td>Office</td><td>" + DS.Tables[3].Rows[0]["OfficeName"] + "</td></tr>");
                                    }
                                    SB.Append("<tr><td>Role</td><td>" + DS.Tables[3].Rows[0]["Roles"] + "</td></tr>");
                                    SB.Append("<tr><td>Mobile No</td><td>" + DS.Tables[3].Rows[0]["Mobile"] + "</td></tr>");
                                    SB.Append("<tr><td>Email Id</td><td>" + DS.Tables[3].Rows[0]["EmailId"] + "</td></tr>");
                                    SB.Append("</tbody></table></section></div></div> </li>");
                                    Session["UserProfile"] = SB.ToString();
                                }
                                else
                                {
                                    Session["UserProfile"] = null;
                                }
                                OBJUserRolesMenusDetails.ROLEGROUPS = ROLEGROUPS;
                                OBJUserRolesMenusDetails.Menus = Menus;
                                Session["CURRENT_Menus"] = null;
                                Session["ROLEGROUPS"] = ROLEGROUPS;
                                Session["Menus"] = Menus;
                                Session["CURRENT_ROLE"] = Convert.ToString(DS.Tables[1].Rows[0]["RoleId"]);
                                Response.Redirect(Convert.ToString(DS.Tables[1].Rows[0]["DefaultPage"]), false);


                                ///// Check User Session 07/11/2019
                                #region "User Session check "
                                cls_UserSession oUserSession = new cls_UserSession();
                                FMDSS.Models.cls_UserSession.clsUserSession oS = new FMDSS.Models.cls_UserSession.clsUserSession();
                                oS.IPAddress = ipaddress;
                                oS.SessionId = Session.SessionID;
                                oS.SessionTimeOut = Session.Timeout.ToString();
                                oS.SSOId = Convert.ToString(Session["SSOID"]);
                                string Status = Convert.ToString(oUserSession.SaveUserLogs(oS));

                                User.UserId= Convert.ToInt64(DS.Tables[0].Rows[0]["USERID"]);
                                User.Designation = Convert.ToString(DS.Tables[0].Rows[0]["DESIGNATION"]);   
                                
                                if (User.isRedirected == false && User.Designation != "11" &&  User.Designation != "12"  && (Session["returnURL"] == null || Session["returnURL"].ToString()==""))
                                {
                                    Response.Redirect("~/MiddleWare/MiddleWareMainModules");
                                }

                                if (Status == "True")
                                    Response.Redirect(Convert.ToString(DS.Tables[1].Rows[0]["DefaultPage"]), false);
                                else
                                    Response.Redirect("errorbySSO.html");
                                #endregion
                                ////// End User Session 





                                // end
                                // Change By Arvind Kumar Sharma // For Dynamic Access Selections
                            }
                            else
                            {
                                if (User.isRedirected == false && User.Designation != "11" &&  User.Designation != "12"  && (Session["returnURL"] == null || Session["returnURL"].ToString() == ""))
                                {
                                    Session["UserId"] = User.UserId;
                                    Session["SSOId"] = User.SSOId;
                                    Session["Designation"] = User.Designation;
                                    Response.Redirect("~/MiddleWare/MiddleWareMainModules");
                                }
                                else
                                {
                                    Response.Redirect("~/Dashboard/Dashboard", false);
                                }
                                
                            }
                        }
                    }
                    else
                    {
                        Response.Write("SSO user is null");
                        //SMSandEMAILtemplate obj = new SMSandEMAILtemplate();
                        //obj.SendMailComman("ALL", "LoginTimeError", DateTime.Now.ToString(), "SSO user is null", string.Empty, string.Empty, string.Empty);
                    } 
                    #endregion
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "FMDSS Entry" + "_" + "User Validation", 0, DateTime.Now, 0);
                //SMSandEMAILtemplate obj = new SMSandEMAILtemplate();
                //obj.SendMailComman("ALL", "LoginTimeError", DateTime.Now.ToString(), ex.Message, string.Empty, string.Empty, string.Empty);
                //Response.Redirect(ConfigurationManager.AppSettings["ErrorBySSO"].ToString());
            }
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
        /// <summary>
        /// Method responsible to redirect the user to sso login page
        /// </summary>
        public string BackToSSO()
        {
            // string SSOID = HttpContext.Current.Request.Cookies["RAJSSO"].Value;
            string SSOID = Convert.ToString(Session["SSOTOKEN"]);
            HttpContext.Current.Session.Clear();//clear session
            HttpContext.Current.Session.Abandon();//Abandon session  
            RAJSSO.SSO SSO = new RAJSSO.SSO();           
            SSO.BackToSSO(SSOID);       
            return "Success";
        }
        /// <summary>
        /// Method responsible to logout 
        /// </summary>
        public void Logout()
        {
            if (Session["MemberInfo"] != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml")) == true)
                {
                    System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                    Session["MemberInfo"] = null;
                }
            }

            #region "User Session check "
            cls_UserSession oUserSession = new cls_UserSession();
            string ssoId = Convert.ToString(Session["SSOID"]);
            oUserSession.LogoutUser(ssoId);
            FmdssStaticData.RemoveLoggedUsers(ssoId);
            Session["SSODetail"] = null;
            Session["IsLogged"] = null;
            #endregion

            string SSOID = HttpContext.Current.Request.Cookies["RAJSSO"].Value;
            HttpContext.Current.Request.Cookies["RAJSSO"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Clear();
            Session.Clear();//clear session
            Session.Abandon();//Abandon session  
            RAJSSO.SSO SSO = new RAJSSO.SSO();
            Response.Redirect("https://ssotest.rajasthan.gov.in/signout", false);
            //SSO.SSOSignout(SSOID);
            //Response.Redirect("https://ssotest.rajasthan.gov.in/signout", false);
            

        }
        /// <summary>
        /// Method responsible to increase session 
        /// </summary>
        public void IncreaseSession()
        {
            RAJSSO.SSO SSO = new RAJSSO.SSO();
            string str = SSO.IncreaseSession().ToString();
        }
        /// <summary>
        /// Method responsible to redirect to specific page
        /// </summary>
        private void posttopage(string URL, string userdetails, string AppName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body style='background-color:#F0F0F0;' onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", URL, false);
            sb.AppendFormat("<div style='float:left; width:100%; height:100%;'>");
            sb.AppendFormat("<div style='float:left; width:100%; height:100%; margin-top:10%;'>	");
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center; font-size:30px; color:#525252; margin:0 0 50px 0;'>Please wait while you are being redirected to <span style='font-weight:bold;'>{0}</span> Application.</div>", AppName.ToUpper());
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center;'>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("<input type='hidden' name='userdetails' value='{0}'>", Encryption.decrypt(userdetails));
            sb.AppendFormat("</div>");
            sb.AppendFormat("<div>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();
        }
        public string Get_LatestRequest()
        {
            string str = "";
            string UserID = Session["UserID"].ToString();
            DataSet DT = new CitizenDashboard().Get_LatestRequest(UserID);
            if (DT.Tables[0].Rows.Count > 0)
            {
                str = "<table><tr><td style='width:100%' colspan='2'><b>Noc Information </b></td></tr>";
                str += "Your latest request no is " + DT.Tables[0].Rows[0]["RequestedId"].ToString() + " registered on " + DT.Tables[0].Rows[0]["EnteredOn"].ToString();
                str += "</td></tr></table>";
            }
            else
            {
                str += "<table><tr><td><b>Citizen </b></td></tr> <tr><td style='width:100px;'>";
                str = "Welcome in FMDSS portal";
                str += "</td></tr></table> </br>";
            }
            if (DT.Tables[1].Rows.Count > 0)
            {
                str += "<table><tr><td colspan='2' style='width:width:100px;'><b>Latest Public Notices </b></td></tr> <tr><td style='width:100px;'>";
                int count = 1;
                str += "<tr><td><b>#</b></td><td>Notice Number</td> </tr>";
                for (int i = 0; i < DT.Tables[1].Rows.Count; i++)
                {
                    str += string.Format("<tr><td><b>" + count + " </b></td><td><a data-toggle='modal' data-target='#diologNotice' onclick='PostPublishNotice({0})'>" + Convert.ToString(DT.Tables[1].Rows[i]["NOTICE_NUMBER"]) + "</a></td> </tr>", DT.Tables[1].Rows[i]["Id"]);
                    count += 1;
                }
            }
            str += "</table>";
            return str;
        }
        //public string Get_LatestPublishedno()
        //{
        //    string str = "";
        //    string UserID = Session["UserID"].ToString();
        //    DataTable dtpublished = new NoticeManagement().BindPublishednoticealert(Convert.ToInt64(UserID));
        //    if (dtpublished != null && dtpublished.Rows.Count > 0)
        //    {
        //        str = "Your latest Notice is " + Convert.ToString(dtpublished.Rows[0]["Notice_Number"]) + "is published on " + Convert.ToString(dtpublished.Rows[0]["EnteredOn"]);
        //    }
        //    else
        //    {
        //        str = "Welcome in FMDSS portal";
        //    }
        //    return str;
        //}
        #endregion

        #region generate 16 digit number by rajveer
        private static Random RNG = new Random();

        public string Create16DigitString()
        {
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(RNG.Next(10).ToString());
            }
            return builder.ToString();
        }
        #endregion
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
    }
}
