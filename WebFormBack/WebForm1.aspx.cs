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
//  Modified By  :
//  Modified On  :
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

namespace FMDSS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        #region Data Members

        UserProfile UserInfo = new UserProfile();
        UserProfile User = null;
        bool flag = false;

        #endregion

        #region Member Functions

        /// <summary>
        /// Method responsible to connect with SSO and to fetch details of logged in User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["val"] == "Staging")
            {
                string aa = Session["User"].ToString();
                if (Session["SSODetail"] != null)
                {
                    User = (UserProfile)Session["SSODetail"];
                    string option = "Insert";
                    DataTable dt = User.InsertUpdateUserInfo(option);
                    bool flag = Convert.ToBoolean(dt.Rows[0][0]);
                    Session["UserId"] = Convert.ToInt64(dt.Rows[0][1]); 
                    Session["DesignationId"] = Convert.ToInt64(dt.Rows[0][2]);
                    Session["DesignationDes"] = dt.Rows[0][3].ToString();
                    Session["AlertList"] = Get_LatestRequest();
                   
                    //Commneted by Vandana Gupta as Update Profile is to be hidden
                    //if (flag)
                    //{
                    //    Response.Redirect("~/UserProfiling/UserProfiling", false);
                    //}
                    //else
                    //{
                    //    if (Session["Role"].ToString() == "RANGER" || Session["Role"].ToString() == "FORESTER" || Session["Role"].ToString() == "2")
                    //        Response.Redirect("~/ForesterAction/ForesterAction", false);
                    //    else if (Session["Role"].ToString() == "ADMIN")
                    //        Response.Redirect("~/SystemAdmin/SystemAdmin", false);
                    //    else
                    //        Response.Redirect("~/Dashboard/Dashboard", false);
                    //}

                    if (Session["Role"].ToString() == "RANGER" || Session["Role"].ToString() == "FORESTER" || Session["Role"].ToString() == "2")
                        Response.Redirect("~/ForesterAction/ForesterAction", false);
                    else if (Session["Role"].ToString() == "ADMIN")
                        Response.Redirect("~/SystemAdmin/SystemAdmin", false);
                    else
                        Response.Redirect("~/Dashboard/Dashboard", false);
                }
            }
            else
            {
                if (Request.QueryString["val"] != null)
                {
                    string ssoToken = Request.QueryString["ssoToken"].ToString();
                    string value = Request.QueryString["val"];
                    if (value == "backtosso")
                    {
                        BackToSSO();
                    }
                    else if (value == "logout")
                    {
                        Logout();
                        // RAJSSO.SSO user = new RAJSSO.SSO();
                        // string UserToken=Convert.ToString(Session["SSOTOKEN"]);
                        // user.SSOSignout(UserToken);

                    }
                    else if (value == "Grievance" && ssoToken != null)
                    {
                        BackToSSO();
                      //  posttopage("http://10.68.5.236/index.aspx", ssoToken, "Sampark");
                        //Response.Redirect("http://164.100.222.107/index.aspx? value=" + Session["SSOID"].ToString()+Session["Role"].ToString());
                    }
                    else if (value == "RTI" && ssoToken != null)
                    {
                        BackToSSO();
                        //posttopage("http://rtitest.rajasthan.gov.in/sso_landing_page.aspx", ssoToken, "RTI");
                    }
                }
                RAJSSO.SSO SSO = new RAJSSO.SSO();
                if (!IsPostBack)
                {
                    if (Request.Form["userdetails"] != null)
                    {
                        Session["SSOTOKEN"] = Request.Form["userdetails"].ToString();
                        SSO.CreateSSOSession();
                        SSO.ClientSideSession();

                        RAJSSO.SSOWS.SSOTokenDetail detail = SSO.GetSessionValueXml();
                        Session["SSOID"] = detail.sAMAccountName;
                        Session["Role"] = string.Join(",", detail.Roles).ToUpperInvariant();
                        Session["loggedin"] = true;
                        RAJSSO.SSOWS.SSOUserDetail ssouser = SSO.GetUserDetailXML(Session["SSOID"].ToString(), ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[0], ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[1]);
                        string userdetails = string.Empty;

                        userdetails += "aadhaarId =" + ssouser.aadhaarId + "<br/>";
                        userdetails += "bhamashahId =" + ssouser.bhamashahId + "<br/>";
                        userdetails += "bhamashahMemberId =" + ssouser.bhamashahMemberId + "<br/>";
                        userdetails += "dateOfBirth =" + ssouser.dateOfBirth + "<br/>";
                        userdetails += "displayName =" + ssouser.displayName + "<br/>";
                        userdetails += "gender =" + ssouser.gender + "<br/>";
                        userdetails += "jpegPhoto =" + ssouser.jpegPhoto + "<br/>";
                        userdetails += "mailPersonal =" + ssouser.mailPersonal + "<br/>";
                        userdetails += "mobile =" + ssouser.mobile + "<br/>";
                        userdetails += "telephoneNumber =" + ssouser.telephoneNumber + "<br/>";
                        userdetails += "postalAddress =" + ssouser.postalAddress + "," + ssouser.l + "," + ssouser.st + ",PIN -" + ssouser.postalCode + "<br/>";
                        Session["User"] = ssouser.displayName;
                        
                        User = new UserProfile()
                        {
                            SSOId = detail.sAMAccountName,
                            FullName = ssouser.displayName,
                            AadharId = ssouser.aadhaarId,
                            BhamashahId = ssouser.bhamashahId,
                            DatOFBirth = ssouser.dateOfBirth,
                            Gender = ssouser.gender,
                            PhotURL = ssouser.jpegPhoto,
                            EmailId = ssouser.mailPersonal,
                            MobileNumber = ssouser.mobile,
                            Designation = string.Empty,
                            Address1 = ssouser.postalAddress,
                            PINCode1 = ssouser.postalCode,
                            District1 = ssouser.l,
                            Roles = Session["Role"].ToString(),
                        };
                        Session["SSODetail"] = User;
                        User = (UserProfile)Session["SSODetail"];
                        string option = "Insert";
                        DataTable dt = User.InsertUpdateUserInfo(option);
                        bool flag = false;
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                flag = Convert.ToBoolean(dt.Rows[0][0]);
                                Session["UserId"] = Convert.ToInt64(dt.Rows[0][1]);
                                Session["DesignationId"] = Convert.ToInt64(dt.Rows[0][2]);
                                Session["DesignationDes"] = dt.Rows[0][3].ToString();
                            }
                        }

                        //Commneted by Vandana Gupta as Update Profile is to be hidden
                        //if (flag)
                        //{
                        //    Response.Redirect("~/UserProfiling/UserProfiling", false);
                        //}
                        //else
                        //{
                        //    if (Session["Role"].ToString() == "RANGER" || Session["Role"].ToString() == "FORESTER" || Session["Role"].ToString() == "2")
                        //        Response.Redirect("~/ForesterAction/ForesterAction", false);
                        //    else if (Session["Role"].ToString() == "ADMIN" || Session["Role"].ToString() == "PCCF")
                        //        Response.Redirect("~/SystemAdmin/SystemAdmin", false);
                        //    else
                        //        Response.Redirect("~/Dashboard/Dashboard", false);
                        //}
                        Session["AlertList"] = Get_LatestRequest();
                        if (Session["Role"].ToString() == "RANGER" || Session["Role"].ToString() == "FORESTER" || Session["Role"].ToString() == "2")
                            Response.Redirect("~/ForesterAction/ForesterAction", false);
                        else if (Session["Role"].ToString() == "ADMIN" || Session["Role"].ToString() == "PCCF")
                            Response.Redirect("~/SystemAdmin/SystemAdmin", false);
                        else
                            Response.Redirect("~/Dashboard/Dashboard", false);
                    }
                }
            }
        }

        /// <summary>
        /// Method responsible to redirect the user to sso login page
        /// </summary>
        public string BackToSSO()
        {
            string SSOID = HttpContext.Current.Request.Cookies["RAJSSO"].Value;
            //HttpContext.Current.Session.Clear();//clear session
            //HttpContext.Current.Session.Abandon();//Abandon session  
            RAJSSO.SSO SSO = new RAJSSO.SSO();
            SSO.BackToSSO(SSOID);
            //SSO.BackToSSO(Convert.ToString(Session["TOKEN"]));
            return "Success";

        }

        /// <summary>
        /// Method responsible to logout 
        /// </summary>
        public void Logout()
        {
            string Token = "";
            //string SSOID = Session["SSOid"].ToString();
            if (HttpContext.Current.Request.Cookies["RAJSSO"] != null)
                Token = HttpContext.Current.Request.Cookies["RAJSSO"].Value;

            Response.Write(Token);

            Session.Clear();//clear session
            Session.Abandon();//Abandon session              
            RAJSSO.SSO SSO = new RAJSSO.SSO();
            SSO.SSOSignout(Token);

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
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", URL,false);
            sb.AppendFormat("<div style='float:left; width:100%; height:100%;'>");
            sb.AppendFormat("<div style='float:left; width:100%; height:100%; margin-top:10%;'>	");
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center; font-size:30px; color:#525252; margin:0 0 50px 0;'>Please wait while you are being redirected to <span style='font-weight:bold;'>{0}</span> Application.</div>", AppName.ToUpper());
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center;'>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("<input type='hidden' name='userdetails' value='{0}'>", userdetails);
            sb.AppendFormat("</div>");
            sb.AppendFormat("<div>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();
            Session["SSOTOKEN"] = userdetails;
        }

        /// <summary>
        /// For left pannel alert
        /// </summary>
        /// <returns></returns>
        public string Get_LatestRequest()
        {
            string str = "";
            string UserID = Session["UserId"].ToString();
            DataTable DT = new CitizenDashboard().Get_LatestRequest(UserID);
            if (DT.Rows.Count > 0)
            {
                str = "Your latest request no is " + DT.Rows[0]["RequestedId"].ToString() + " registered on " + DT.Rows[0]["EnteredOn"].ToString() + ".";
            }
            else
            {
                str = "Welcome in FMDSS portal.";
            }
            return str;

        }
        #endregion
    }
}
