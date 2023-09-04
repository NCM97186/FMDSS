using FMDSS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class MoveOnFMDSS_V1Controller : Controller
    {
        //
        // GET: /MoveOnFMDSS_V1/

        public ActionResult Index()
        {

            //User.InsertTestComment(" Stagging " + ipaddress);
            //DS = User.InsertUpdateUserInfo();
            //UserProfileSessionMaintainDataset = User.UserProfileSessionMaintain("INSERT", User.SSOId, Create16Digit, ipaddress);//Add by Rajveer
            //Session["Create16DigitSession"] = Create16Digit;
            //bool flag = Convert.ToBoolean(DS.Tables[0].Rows[0]["FIRSTTIMELOGIN"]);
            //Session["UserId"] = Convert.ToInt64(DS.Tables[0].Rows[0]["USERID"]);
            //Session["DesignationId"] = Convert.ToInt64(DS.Tables[0].Rows[0]["DESIGNATION"]);
            //Session["DesignationDes"] = DS.Tables[0].Rows[0]["DESIG_NAME"].ToString();
            //Session["AadharID"] = Convert.ToString(DS.Tables[0].Rows[0]["AadharID"]);
            //Session["AlertList"] = Get_LatestRequest();
            ////Session["AlertListNotice"] = Get_LatestPublishedno();
            //Session["IsKioskUser"] = Convert.ToBoolean(DS.Tables[0].Rows[0]["ISKIOSKUSER"]);
            //Session["IsDepartmentalKioskUser"] = Convert.ToBoolean(DS.Tables[0].Rows[0]["ISDEPARTMENTALKIOSKUSER"]);
            //Session["Role"] = Convert.ToString(DS.Tables[0].Rows[0]["Role"]);
            //if (Convert.ToBoolean(DS.Tables[0].Rows[0]["IsKioskUser"]) || Convert.ToBoolean(DS.Tables[0].Rows[0]["ISDEPARTMENTALKIOSKUSER"]))
            //{
            //    Session["KioskUserId"] = Session["UserId"];
            //    Session["KioskSSOId"] = Session["SSOID"];
            //    Session["Role"] = "KIOSK";
            //}
            //Session["SSOID"] = User.SSOId;
            //// Change By Arvind Kumar Sharma // For Dynamic Access Selections
            //if (DS.Tables[1].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in DS.Tables[1].Rows)
            //    {
            //        ROLEGROUPS.Add(
            //        new ROLEGROUPS()
            //        {
            //            RoleId = Convert.ToInt16(dr["RoleId"]),
            //            RoleName = Convert.ToString(dr["RoleName"]),
            //            RolePriority = Convert.ToInt16(dr["RolePriority"]),
            //            DefaultPage = Convert.ToString(dr["DefaultPage"]),
            //            DefaultLayout = Convert.ToString(dr["Layout"]),
            //        });
            //    }
            //    foreach (DataRow dr in DS.Tables[2].Rows)
            //    {
            //        Menus.Add(
            //        new Menus()
            //        {
            //            UserID = Convert.ToInt64(dr["UserID"]),
            //            SSOID = Convert.ToString(dr["SSOID"]),
            //            RoleId = Convert.ToInt16(dr["RoleId"]),
            //            RoleName = Convert.ToString(dr["RoleName"]),
            //            PageTitle = Convert.ToString(dr["PageTitle"]),
            //            PageURL = Convert.ToString(dr["PageURL"]),
            //            IsActive = Convert.ToString(dr["IsActive"]),
            //            IconClass = Convert.ToString(dr["IconClass"]),
            //            isIcon = Convert.ToString(dr["isIcon"]),
            //            Layout = Convert.ToString(dr["Layout"]),
            //            PageID = Convert.ToInt64(dr["PageID"]),
            //            ParentID = Convert.ToInt64(dr["ParentID"]),
            //            IsNested = Convert.ToBoolean(dr["IsNested"]),
            //            IsTargetBlank = Convert.ToBoolean(dr["IsTargetBlank"]),
            //        });
            //    }
            //    StringBuilder SB = new StringBuilder();
            //    if (DS.Tables[3].Rows.Count > 0)
            //    {
            //        SB.Append("<li><div class='col-lg-12'><div style='font-size: 12px; border-top: #337ab7 3px solid;'>");
            //        SB.Append("<section class='panel' style='font-size:12px;'>");
            //        SB.Append("<div class='panel-body'style='padding:0px;'><div class='task-thumb-details' style='text-align: center; '>");
            //        SB.Append("<div style=' text-align :center; font-size :18px; color :#337ab7;'>" + DS.Tables[3].Rows[0]["Name"] + "</div>");
            //        SB.Append("<span  style ='text-align: center; color: #337ab7;'>" + DS.Tables[3].Rows[0]["Desig_Name"] + "</div></div></div>");
            //        SB.Append("<table class='table'><tbody>");
            //        SB.Append("<tr><td>SSO ID</td><td>" + DS.Tables[3].Rows[0]["Ssoid"] + "</td></tr>");
            //        SB.Append("<tr><td>Department</td><td>" + DS.Tables[3].Rows[0]["Department"] + "</td></tr>");
            //        SB.Append("<tr><td>Office</td><td>" + DS.Tables[3].Rows[0]["OfficeName"] + "</td></tr>");
            //        SB.Append("<tr><td>Role</td><td>" + DS.Tables[3].Rows[0]["Roles"] + "</td></tr>");
            //        SB.Append("<tr><td>Mobile No</td><td>" + DS.Tables[3].Rows[0]["Mobile"] + "</td></tr>");
            //        SB.Append("<tr><td>Email Id</td><td>" + DS.Tables[3].Rows[0]["EmailId"] + "</td></tr>");
            //        SB.Append("</tbody></table></section></div></div> </li>");
            //        Session["UserProfile"] = SB.ToString();
            //    }
            //    else
            //    {
            //        Session["UserProfile"] = null;
            //    }
            //    OBJUserRolesMenusDetails.ROLEGROUPS = ROLEGROUPS;
            //    OBJUserRolesMenusDetails.Menus = Menus;
            //    Session["CURRENT_Menus"] = null;
            //    Session["ROLEGROUPS"] = ROLEGROUPS;
            //    Session["Menus"] = Menus;

            //    Session["CURRENT_ROLE"] = Convert.ToString(DS.Tables[1].Rows[0]["RoleId"]);

            //    ///// Check User Session 07/11/2019
            //    #region "User Session check "
            //    cls_UserSession oUserSession = new cls_UserSession();
            //    FMDSS.Models.cls_UserSession.clsUserSession oS = new FMDSS.Models.cls_UserSession.clsUserSession();
            //    oS.IPAddress = ipaddress;
            //    oS.SessionId = Session.SessionID;
            //    oS.SessionTimeOut = Session.Timeout.ToString();
            //    oS.SSOId = Convert.ToString(Session["SSOID"]);
            //    string Status = Convert.ToString(oUserSession.SaveUserLogs(oS));

            //    if (Status == "True")
            //        Response.Redirect(Convert.ToString(DS.Tables[1].Rows[0]["DefaultPage"]), false);
            //    else
            //        Response.Redirect("errorbySSO.html");
            //    #endregion
                return View();
        }
        public ActionResult LoginInfo(FormCollection frm)
        {
            string colval = frm.ToString();
            UserProfile userObj = new UserProfile();
            userObj.SSOId = frm["SSOid"];
            userObj.Roles = frm["Role"];
            userObj.UserId =Convert.ToInt64(frm["UserId"]);
            userObj.Designation = frm["DesignationId"];            
            userObj.AadharId =frm["AadharID"];
            userObj.IsKioskUser =Convert.ToBoolean(frm["IsKioskUser"]);
            userObj.IsDepartmentalKioskUser =Convert.ToBoolean(frm["IsDepartmentalKioskUser"]);
            userObj.MobileNumber = frm["MOBILE"];
            userObj.EmailId = frm["EmailId"];
            userObj.IsAgency =Convert.ToBoolean(frm["IsAgency"]);
            userObj.BhamashahId = frm["BhamashahId"];
            userObj.DatOFBirth= frm["DatOFBirth"];
            userObj.Gender = frm["Gender"];
            userObj.Address1 = frm["Address1"];
            userObj.Address2 = frm["Address2"];
            userObj.PINCode1 = frm["PINCode1"];
            userObj.PINCode1 = frm["PINCode2"];
            userObj.District1 = frm["District1"];
            userObj.District2 = frm["District2"];
            userObj.City2 = frm["City2"];
            userObj.AgencyName= frm["AgencyName"];
            userObj.AgencyDistrict=frm["AgencyDistrict"];
            userObj.AgencyCity = frm["AgencyCity"];
            userObj.AgencyAddress= frm["AgencyAddress"];
            userObj.IsBhamashah=Convert.ToBoolean(frm["IsBhamashah"]);
            userObj.AgencySPOC = frm["AgencySPOC"];
            userObj.AgencyContact= frm["AgencyContact"];
            userObj.IsSSO = Convert.ToBoolean(frm["IsSSO"]);
           
            //userObj.isRedirected { get; set; }
            //userObj.TelephoneNumber { get; set; }
            //userObj.PhotURL { get; set; }
            //userObj.FullName { get; set; }
            //userObj.KioskSSOId { get; set; }            
            //userObj.IsDepartmentalKioskUser { get; set; }

            string name = frm["loggedin"];
        
            Session["SSODetail"] = userObj;

            Session["loggedin"] = frm["loggedin"].ToString();
            Session["SSOid"] = frm["SSOid"].ToString();
            Session["User"] = frm["User"].ToString();
            Session["Role"] = frm["Role"].ToString();
            Session["UserId"] = frm["UserId"].ToString();
            Session["DesignationId"] = frm["OrignalDesignationID"].ToString();
            Session["IsKioskUser"] = frm["IsKioskUser"].ToString();
            Session["UserCDR"] = frm["UserCDR"];
            Session["DESIGNATION"] = frm["DESIG_NAME"];
            Session["SSOToken"] = frm["SSOToken"];
            #region new code added by mukesh sir 6/2/2022
            Session["From_2_0"] = "Yes";
            #endregion
            string dftdashboard = (frm["DefaultDashboard"].ToString());
            //string[] spl = dftdashboard.Split('~');
            //string DefaultDashboard = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(spl[0]);
            //string makeUrl = FMDSS.Models.MySecurity.SecurityCode.EncodeUrl(DefaultDashboard);
            //MpaYqqIpYd % 20gDwQsxSdQ9Vx2BtwAxrP7sqJVRdvnrXlzlpmBRTFXBsRk4Q / lF2sc4oh7bFLhO60 = ~y
            //Request.QueryString["returnURL"] = dftdashboard;// 
            ////FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(frm["DefaultDashboard"]);
            if (dftdashboard == "" || dftdashboard == null)
            {
                Response.Redirect("~/WebForm1.aspx?val=Staging", false);
            }
            else
            {
                Response.Redirect("~/WebForm1.aspx?val=Staging&returnURL=" + dftdashboard, false);
            }
            
            return null;
        }
    }
}
