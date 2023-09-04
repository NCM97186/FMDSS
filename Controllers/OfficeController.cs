using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using System.Data;
using FMDSS.Filters;
using System.Text;

using FMDSS.Models.ForesterAction;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using System.Configuration;
using iTextSharp.text.pdf.draw;
using FMDSS.Models.Home;
using Newtonsoft.Json;

namespace FMDSS.Controllers
{
    public class OfficeController : Controller
    {
        //
        // GET: /Office/ 
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


        UserProfile UserInfo = new UserProfile();
        UserProfile User = null;
        bool flag = false;

        public ActionResult Index()
        {
            ViewBag.isStaging =Convert.ToString(Session["IsStaging"]);
            List<MultiOffice> lstOfc = (List<MultiOffice>)Session["lstOfc"];
             
            return View(lstOfc);
        }

        [HttpPost]
        public ActionResult Index(string IsStagingOrProduction, string AnotherSSOId)
        {
            List<MultiOffice> lstOfc = new List<MultiOffice>();
            string aa = Session["User"].ToString();

            #region Call Random No And Ip_Address
            string Create16Digit = Create16DigitString();
            DataTable UserProfileSessionMaintainDataset = new DataTable();
            Session["Create16DigitSession"] = Create16Digit;

            string ipaddress = string.Empty;
            if (Request.Url!=null && Request.Url.Host == "localhost")
            {
                ipaddress = HttpContext.Request.UrlReferrer.Host;
                // Do what you want for localhost
            }
            else
            {
                ipaddress = Request.UserHostAddress;
            }
            //string ipaddress;
            //ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (ipaddress == "" || ipaddress == null)
            //    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            #endregion

            IsStagingOrProduction = "Staging";
            try
            {
                DataSet DS = new DataSet();
                UserRolesMenusDetails OBJUserRolesMenusDetails = new UserRolesMenusDetails();
                List<ROLEGROUPS> ROLEGROUPS = new List<ROLEGROUPS>();
                List<Menus> Menus = new List<Menus>();
                if (IsStagingOrProduction == "Staging")
                {
                    #region Staging Case                    
                    if (Session["SSODetail"] != null)
                    {
                        User = (UserProfile)Session["SSODetail"];
                        User.IsSSO = true;
                        User.IsBhamashah = false;

                        User.SSOId = AnotherSSOId;
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
                            Response.Redirect(Convert.ToString(DS.Tables[1].Rows[0]["DefaultPage"]), false);
                        }
                        else
                        {
                            Response.Redirect("~/Dashboard/Dashboard", false);
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
                        string value = Request.QueryString["val"];
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

                    Session.Clear();
                    dynamic oObj;
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["encDataMultiOffice"])) && Convert.ToString(Session["encDataMultiOffice"]).Equals("encData"))
                    {
                        string decryptString = string.Empty;
                        string sData = HttpUtility.UrlDecode(Convert.ToString(Session["encDataMultiOffice"]), Encoding.UTF8);
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
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["SSOTOKENMultOffices"])))
                        {
                            Session["SSOTOKEN"] = Convert.ToString(Session["SSOTOKENMultOffices"]);
                            // Response.Cookies["RAJSSO"]=RAJSSO
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
                        // end
                        // Change By Arvind Kumar Sharma // For Dynamic Access Selections
                    }
                    else
                    {
                        Response.Redirect("~/Dashboard/Dashboard", false);
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
            return View(lstOfc);
        }


        public string UserLogin()
        {
           return BackToSSO();
            
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
           Session.Clear();//clear session
          Session.Abandon();//Abandon session  
            RAJSSO.SSO SSO = new RAJSSO.SSO();
            SSO.SSOSignout(SSOID);
          //  SSO.BackToSSO(SSOID);
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
            string SSOID = Request.Cookies["RAJSSO"].Value;
            Request.Cookies["RAJSSO"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Clear();
            Session.Clear();//clear session
            Session.Abandon();//Abandon session  
            RAJSSO.SSO SSO = new RAJSSO.SSO();
            SSO.SSOSignout(SSOID);
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
                    str += "<tr><td><b>" + count + " </b></td><td>" + Convert.ToString(DT.Tables[1].Rows[i]["NOTICE_NUMBER"]) + "</td> </tr>";
                    count += 1;
                }
            }
            str += "</table>";
            return str;
        }

    }
}
