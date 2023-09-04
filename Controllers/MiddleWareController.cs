using FMDSS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.util;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class MiddleWareController : Controller
    {
		//
		// GET: /MiddleWare/
		public ActionResult MiddleWareMainModules()
		{
			UserProfile User = (UserProfile)Session["SSODetail"];
			
			MiddleWareModules middleWareModules = new MiddleWareModules();
			if (User != null)
			{
				User.isRedirected = true;
				Session["SSODetail"] = User;
				middleWareModules.MWMG_List = middleWareModules.GetMiddleWareModuleGroupList(User.SSOId, User.UserId);
			}
			else
			{
				middleWareModules.MWMG_List = middleWareModules.GetMiddleWareModuleGroupList("", 0);
			}			
			
			return View(middleWareModules);
		}
		public ActionResult MiddleWareModules(int MainGroupId)
        {
			UserProfile User = (UserProfile)Session["SSODetail"];
			MiddleWareModules middleWareModules = new MiddleWareModules();

			if (User != null)
			{
				User.isRedirected = true;
				Session["SSODetail"] = User;
				ViewBag.Roles = User.Roles;
				middleWareModules.MWM_List = middleWareModules.GetMiddleWareModulesList(User.SSOId, User.UserId, MainGroupId);
			}
			else
			{
				ViewBag.Roles = "CITIZEN";
				middleWareModules.MWM_List = middleWareModules.GetMiddleWareModulesList("", 0, MainGroupId);
			}									
			return View(middleWareModules);
        }
		public ActionResult GetSubModulePages(int ModuleId,string mainLink)
		{
			UserProfile User = (UserProfile)Session["SSODetail"];
			User.isRedirected = true;
			Session["SSODetail"] = User;
			MiddleWareModules middleWareModules = new MiddleWareModules();
			middleWareModules.MWSMG_List = middleWareModules.GetSubModulePageList(ModuleId);
			middleWareModules.MWSMG_List.ToList().ForEach(s => s.MainLink = mainLink + "&returnURL="+ FMDSS.Models.MySecurity.SecurityCode.EncodeUrl(s.PageLinkUrl));
			return PartialView("_PartialSubModulePageList", middleWareModules);
		}
		#region ConnectToFMDSS2.0
		[HttpGet]
        public ActionResult PostUserDetails(string DefaultDashboard)
        {

            //cls_userDetails oUser = new cls_userDetails { UserName = "Amit Singh", Address = "Railway Colony", mobileNumber = "07568246030", Pincode = "303313" };

            string Url = ConfigurationManager.AppSettings["FMDSS2_URL"];
            Response.Clear();
            StringBuilder sb = new StringBuilder();
            UserProfile userProfile = new UserProfile();
            DataSet ds = new DataSet();
            try
            {
                ds = userProfile.GetUserCDR(Convert.ToInt64(Session["UserId"].ToString()));
            }
            catch (Exception ex0)
            {
                new Common().ErrorLog(ex0.Message.ToString(), "MiddleWareController_PostUserDetails_0", 0, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }
            string UserCDR = "";

            DataTable dtCdr = ds.Tables[0];
            DataTable dtEmp = ds.Tables[1];
            try
            {
                foreach (DataRow dr in dtCdr.Rows)
                {
                    UserCDR = "" + dr["UserCDR"].ToString();
                }
            }
            catch (Exception ex1)
            {
                new Common().ErrorLog(ex1.Message.ToString(), "MiddleWareController_PostUserDetails_1", 0, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }

            long UserId = 0;
            string DESIGNATION = "";
            string DESIG_NAME = "";
            string MOBILE = "";
            string AadharID = "";
            string EmailId = "";
            string OfficeName = "";
            string UserName = "";
            bool ISKIOSKUSER = false;
            bool ISDEPARTMENTALKIOSKUSER = false;
            string SSOToken = null;
            try
            {
                if (Session["SSOTOKEN"] != null)
                {
                    SSOToken = Session["SSOTOKEN"].ToString();
                }
            }
            catch (Exception ex2)
            {
                new Common().ErrorLog(ex2.Message.ToString(), "MiddleWareController_PostUserDetails_2", 0, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }

            UserProfile user = new UserProfile();
            try
            {
                foreach (DataRow dr in dtEmp.Rows)
                {
                    UserId = (long)dr["UserId"];
                    UserName = "" + dr["Name"].ToString();
                    DESIGNATION = "" + dr["DESIGNATION"].ToString();
                    DESIG_NAME = "" + dr["DESIG_NAME"].ToString();
                    MOBILE = "" + dr["MOBILE"].ToString();
                    AadharID = "" + dr["AadharID"].ToString();
                    EmailId = "" + dr["EmailId"].ToString();
                    OfficeName = "" + dr["OfficeName"].ToString();
                    ISKIOSKUSER = (bool)dr["ISKIOSKUSER"];
                    ISDEPARTMENTALKIOSKUSER = (bool)dr["ISDEPARTMENTALKIOSKUSER"];

                    user.IsAgency = (bool)(dr["IsAgency"].ToString().Length == 0 ? false : dr["IsAgency"]);
                    user.BhamashahId = "" + dr["Bhamashah_Id"].ToString();
                    user.DatOFBirth = "" + dr["DOB"].ToString();
                    user.Gender = "" + dr["Gender"].ToString();
                    user.Address1 = "" + dr["Postal_Address1"].ToString();
                    user.PINCode1 = "" + dr["Postal_Code1"].ToString();
                    user.District1 = "" + dr["District1"].ToString();
                    //user.PhotURL = "" + dr["PhotURL"].ToString();
                    user.Address2 = "" + dr["Postal_Address2"].ToString();
                    user.PINCode2 = "" + dr["Postal_Code2"].ToString();
                    user.District2 = "" + dr["District2"].ToString();
                    user.City2 = "" + dr["City2"].ToString();
                    user.AgencyName = "" + dr["AgencyName"].ToString();
                    user.AgencyDistrict = "" + dr["AgencyDistrict"].ToString();
                    user.AgencyCity = "" + dr["AgencyCity"].ToString();
                    user.AgencyAddress = "" + dr["AgencyAddress"].ToString();
                    user.AgencySPOC = "" + dr["AgencySPOC"].ToString();
                    user.AgencyContact = "" + dr["AgencyContact"].ToString();
                    user.IsSSO = (bool)(dr["IsSSO"].ToString().Length == 0 ? false : dr["IsSSO"]);
                    user.IsBhamashah = (bool)(dr["IsBhamashah"].ToString().Length == 0 ? false : dr["IsBhamashah"]);

                }
            }
            catch (Exception ex3)
            {
                new Common().ErrorLog(ex3.Message.ToString(), "MiddleWareController_PostUserDetails_3", 0, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }
            bool loggedin = false;
            string SSOid = "";
            string role = "";
            try
            {
                userProfile = (UserProfile)Session["SSODetail"];
                loggedin = Convert.ToBoolean(Session["loggedin"]);
                SSOid = Session["SSOid"].ToString();
                role = Session["Role"].ToString();
            }
            catch (Exception ex4)
            {
                new Common().ErrorLog(ex4.Message.ToString(), "MiddleWareController_PostUserDetails_4", 0, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }

            try
            {
                sb.Append("<html>");
                sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
                //sb.AppendFormat("<form name='form' action='{0}' method='post'>", "http://10.68.128.179/home/UserData");

                //sb.AppendFormat("<form name='form' action='{0}' method='post'>", "http://10.70.231.190/Base/GetUserProfile");
                sb.AppendFormat("<form name='form' action='{0}' method='post'>", Url);

                //sb.AppendFormat("<form name='form' action='{0}' method='post' target='my-iframe'>", "http://10.70.231.190/Base/GetUserProfile");

                sb.AppendFormat("<input type='hidden' name='loggedin' value='{0}'>", loggedin);
                sb.AppendFormat("<input type='hidden' name='SSOid' value='{0}'>", SSOid);
                sb.AppendFormat("<input type='hidden' name='User' value='{0}'>", UserName);
                sb.AppendFormat("<input type='hidden' name='Role' value='{0}'>", role);
                sb.AppendFormat("<input type='hidden' name='UserId' value='{0}'>", Session["UserId"].ToString());
                sb.AppendFormat("<input type='hidden' name='DesignationId' value='{0}'>", (Session["DesignationId"] == null ? "0" : Session["DesignationId"].ToString()));
                sb.AppendFormat("<input type='hidden' name='DesignationDes' value='{0}'>", (Session["DesignationDes"] == null ? "" : Session["DesignationDes"].ToString()));
                ////sb.AppendFormat("<input type='hidden' name='AadharID' value='{0}'>", Session["AadharID"].ToString());
                sb.AppendFormat("<input type='hidden' name='AadharID' value='{0}'>", AadharID);

                //sb.AppendFormat("<input type='hidden' name='AlertList' value='{0}'>", Session["AlertList"].ToString());
                sb.AppendFormat("<input type='hidden' name='IsKioskUser' value='{0}'>", ISKIOSKUSER);
                sb.AppendFormat("<input type='hidden' name='IsDepartmentalKioskUser' value='{0}'>", ISDEPARTMENTALKIOSKUSER);
                sb.AppendFormat("<input type='hidden' name='UserCDR' value='{0}'>", UserCDR);
                sb.AppendFormat("<input type='hidden' name='DESIGNATION' value='{0}'>", DESIGNATION);
                sb.AppendFormat("<input type='hidden' name='DESIG_NAME' value='{0}'>", DESIG_NAME);
                sb.AppendFormat("<input type='hidden' name='MOBILE' value='{0}'>", MOBILE);
                sb.AppendFormat("<input type='hidden' name='EmailId' value='{0}'>", EmailId);
                sb.AppendFormat("<input type='hidden' name='OfficeName' value='{0}'>", OfficeName);
                sb.AppendFormat("<input type='hidden' name='DefaultDashboard' value='{0}'>", DefaultDashboard);

                sb.AppendFormat("<input type='hidden' name='IsAgency' value='{0}'>", user.IsAgency);
                sb.AppendFormat("<input type='hidden' name='BhamashahId' value='{0}'>", user.BhamashahId);
                sb.AppendFormat("<input type='hidden' name='DatOFBirth' value='{0}'>", user.DatOFBirth);
                sb.AppendFormat("<input type='hidden' name='Gender' value='{0}'>", user.Gender);
                //sb.AppendFormat("<input type='hidden' name='TelephoneNumber' value='{0}'>", user.TelephoneNumber );
                sb.AppendFormat("<input type='hidden' name='Address1' value='{0}'>", user.Address1);
                sb.AppendFormat("<input type='hidden' name='PINCode1' value='{0}'>", user.PINCode1);
                sb.AppendFormat("<input type='hidden' name='District1' value='{0}'>", user.District1);
                //sb.AppendFormat("<input type='hidden' name='PhotURL' value='{0}'>", user.PhotURL );
                sb.AppendFormat("<input type='hidden' name='Address2' value='{0}'>", user.Address2);
                sb.AppendFormat("<input type='hidden' name='PINCode2' value='{0}'>", user.PINCode2);
                sb.AppendFormat("<input type='hidden' name='District2' value='{0}'>", user.District2);
                sb.AppendFormat("<input type='hidden' name='City2' value='{0}'>", user.City2);
                sb.AppendFormat("<input type='hidden' name='AgencyName' value='{0}'>", user.AgencyName);
                sb.AppendFormat("<input type='hidden' name='AgencyDistrict' value='{0}'>", user.AgencyDistrict);
                sb.AppendFormat("<input type='hidden' name='AgencyCity' value='{0}'>", user.AgencyCity);
                sb.AppendFormat("<input type='hidden' name='AgencyAddress' value='{0}'>", user.AgencyAddress);
                sb.AppendFormat("<input type='hidden' name='AgencySPOC' value='{0}'>", user.AgencySPOC);
                sb.AppendFormat("<input type='hidden' name='AgencyContact' value='{0}'>", user.AgencyContact);
                sb.AppendFormat("<input type='hidden' name='IsSSO' value='{0}'>", user.IsSSO);
                sb.AppendFormat("<input type='hidden' name='IsBhamashah' value='{0}'>", user.IsBhamashah);
                sb.AppendFormat("<input type='hidden' name='SSOToken' value='{0}'>", SSOToken);

                //sb.AppendFormat("<input type='hidden' name='KioskUserId' value='{0}'>", Session["KioskUserId"].ToString());
                //sb.AppendFormat("<input type='hidden' name='KioskSSOId' value='{0}'>", Session["KioskSSOId"].ToString());
                //sb.AppendFormat("<input type='hidden' name='SSOID' value='{0}'>", Session["SSOID"].ToString());
                // Other params go here
                sb.Append("</form>");

                // sb.Append("<iframe name='my-iframe' src='fmdss.html'></iframe>");
                //<iframe name="my-iframe" src="iframe.php"></iframe>

                sb.Append("</body>");
                sb.Append("</html>");
                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex5)
            {
                new Common().ErrorLog(ex5.Message.ToString(), "MiddleWareController_PostUserDetails_5", 0, DateTime.Now, Convert.ToInt64(Session["UserId"].ToString()));
            }

            return null;
        }
        //public ActionResult PostUserDetails(string DefaultDashboard)
        //{

        //	//cls_userDetails oUser = new cls_userDetails { UserName = "Amit Singh", Address = "Railway Colony", mobileNumber = "07568246030", Pincode = "303313" };

        //	string Url = ConfigurationManager.AppSettings["FMDSS2_URL"]; 
        //	Response.Clear();
        //	StringBuilder sb = new StringBuilder();
        //	UserProfile userProfile = new UserProfile();
        //	DataSet ds = userProfile.GetUserCDR(Convert.ToInt64(Session["UserId"].ToString()));
        //	string UserCDR = "";
        //	DataTable dtCdr = ds.Tables[0];
        //	DataTable dtEmp = ds.Tables[1];
        //	foreach (DataRow dr in dtCdr.Rows)
        //	{
        //		UserCDR = "" + dr["UserCDR"].ToString();
        //	}
        //	long UserId = 0;
        //	string DESIGNATION = "";
        //	string DESIG_NAME = "";
        //	string MOBILE = "";
        //	string AadharID = "";
        //	string EmailId = "";
        //	string OfficeName = "";
        //	string UserName = "";
        //	bool ISKIOSKUSER = false;
        //	bool ISDEPARTMENTALKIOSKUSER = false;
        //	string SSOToken = null;
        //	if (Session["SSOTOKEN"] != null)
        //	{
        //		SSOToken = Session["SSOTOKEN"].ToString();
        //	}
        //	UserProfile user = new UserProfile();
        //	foreach (DataRow dr in dtEmp.Rows)
        //	{
        //		UserId = (long)dr["UserId"];
        //		UserName = "" + dr["Name"].ToString();
        //		DESIGNATION = "" + dr["DESIGNATION"].ToString();
        //		DESIG_NAME = "" + dr["DESIG_NAME"].ToString();
        //		MOBILE = "" + dr["MOBILE"].ToString();
        //		AadharID = "" + dr["AadharID"].ToString();
        //		EmailId = "" + dr["EmailId"].ToString();
        //		OfficeName = "" + dr["OfficeName"].ToString();
        //		ISKIOSKUSER = (bool)dr["ISKIOSKUSER"];
        //		ISDEPARTMENTALKIOSKUSER = (bool)dr["ISDEPARTMENTALKIOSKUSER"];

        //		user.IsAgency = (bool)(dr["IsAgency"].ToString().Length == 0 ? false : dr["IsAgency"]);
        //		user.BhamashahId = "" + dr["Bhamashah_Id"].ToString();
        //		user.DatOFBirth = "" + dr["DOB"].ToString();
        //		user.Gender = "" + dr["Gender"].ToString();
        //		user.Address1 = "" + dr["Postal_Address1"].ToString();
        //		user.PINCode1 = "" + dr["Postal_Code1"].ToString();
        //		user.District1 = "" + dr["District1"].ToString();
        //		//user.PhotURL = "" + dr["PhotURL"].ToString();
        //		user.Address2 = "" + dr["Postal_Address2"].ToString();
        //		user.PINCode2 = "" + dr["Postal_Code2"].ToString();
        //		user.District2 = "" + dr["District2"].ToString();
        //		user.City2 = "" + dr["City2"].ToString();
        //		user.AgencyName = "" + dr["AgencyName"].ToString();
        //		user.AgencyDistrict = "" + dr["AgencyDistrict"].ToString();
        //		user.AgencyCity = "" + dr["AgencyCity"].ToString();
        //		user.AgencyAddress = "" + dr["AgencyAddress"].ToString();
        //		user.AgencySPOC = "" + dr["AgencySPOC"].ToString();
        //		user.AgencyContact = "" + dr["AgencyContact"].ToString();
        //		user.IsSSO = (bool)(dr["IsSSO"].ToString().Length == 0 ? false : dr["IsSSO"]);
        //		user.IsBhamashah = (bool)(dr["IsBhamashah"].ToString().Length == 0 ? false : dr["IsBhamashah"]);

        //	}
        //	userProfile = (UserProfile)Session["SSODetail"];
        //	bool loggedin = Convert.ToBoolean(Session["loggedin"]);
        //	string SSOid = Session["SSOid"].ToString();
        //	string role = Session["Role"].ToString();
        //	sb.Append("<html>");
        //	sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
        //	//sb.AppendFormat("<form name='form' action='{0}' method='post'>", "http://10.68.128.179/home/UserData");

        //	//sb.AppendFormat("<form name='form' action='{0}' method='post'>", "http://10.70.231.190/Base/GetUserProfile");
        //	sb.AppendFormat("<form name='form' action='{0}' method='post'>", Url);

        //	//sb.AppendFormat("<form name='form' action='{0}' method='post' target='my-iframe'>", "http://10.70.231.190/Base/GetUserProfile");

        //	sb.AppendFormat("<input type='hidden' name='loggedin' value='{0}'>", loggedin);
        //	sb.AppendFormat("<input type='hidden' name='SSOid' value='{0}'>", SSOid);
        //	sb.AppendFormat("<input type='hidden' name='User' value='{0}'>", UserName);
        //	sb.AppendFormat("<input type='hidden' name='Role' value='{0}'>", role);
        //	sb.AppendFormat("<input type='hidden' name='UserId' value='{0}'>", Session["UserId"].ToString());
        //	sb.AppendFormat("<input type='hidden' name='DesignationId' value='{0}'>",(Session["DesignationId"]==null?"0": Session["DesignationId"].ToString()));
        //	sb.AppendFormat("<input type='hidden' name='DesignationDes' value='{0}'>",(Session["DesignationDes"] == null?"": Session["DesignationDes"].ToString()));
        //	sb.AppendFormat("<input type='hidden' name='AadharID' value='{0}'>", Session["AadharID"].ToString());
        //	//sb.AppendFormat("<input type='hidden' name='AlertList' value='{0}'>", Session["AlertList"].ToString());
        //	sb.AppendFormat("<input type='hidden' name='IsKioskUser' value='{0}'>", ISKIOSKUSER);
        //	sb.AppendFormat("<input type='hidden' name='IsDepartmentalKioskUser' value='{0}'>", ISDEPARTMENTALKIOSKUSER);
        //	sb.AppendFormat("<input type='hidden' name='UserCDR' value='{0}'>", UserCDR);
        //	sb.AppendFormat("<input type='hidden' name='DESIGNATION' value='{0}'>", DESIGNATION);
        //	sb.AppendFormat("<input type='hidden' name='DESIG_NAME' value='{0}'>", DESIG_NAME);
        //	sb.AppendFormat("<input type='hidden' name='MOBILE' value='{0}'>", MOBILE);
        //	sb.AppendFormat("<input type='hidden' name='EmailId' value='{0}'>", EmailId);
        //	sb.AppendFormat("<input type='hidden' name='OfficeName' value='{0}'>", OfficeName);
        //	sb.AppendFormat("<input type='hidden' name='DefaultDashboard' value='{0}'>", DefaultDashboard);

        //	sb.AppendFormat("<input type='hidden' name='IsAgency' value='{0}'>", user.IsAgency);
        //	sb.AppendFormat("<input type='hidden' name='BhamashahId' value='{0}'>", user.BhamashahId);
        //	sb.AppendFormat("<input type='hidden' name='DatOFBirth' value='{0}'>", user.DatOFBirth);
        //	sb.AppendFormat("<input type='hidden' name='Gender' value='{0}'>", user.Gender);
        //	//sb.AppendFormat("<input type='hidden' name='TelephoneNumber' value='{0}'>", user.TelephoneNumber );
        //	sb.AppendFormat("<input type='hidden' name='Address1' value='{0}'>", user.Address1);
        //	sb.AppendFormat("<input type='hidden' name='PINCode1' value='{0}'>", user.PINCode1);
        //	sb.AppendFormat("<input type='hidden' name='District1' value='{0}'>", user.District1);
        //	//sb.AppendFormat("<input type='hidden' name='PhotURL' value='{0}'>", user.PhotURL );
        //	sb.AppendFormat("<input type='hidden' name='Address2' value='{0}'>", user.Address2);
        //	sb.AppendFormat("<input type='hidden' name='PINCode2' value='{0}'>", user.PINCode2);
        //	sb.AppendFormat("<input type='hidden' name='District2' value='{0}'>", user.District2);
        //	sb.AppendFormat("<input type='hidden' name='City2' value='{0}'>", user.City2);
        //	sb.AppendFormat("<input type='hidden' name='AgencyName' value='{0}'>", user.AgencyName);
        //	sb.AppendFormat("<input type='hidden' name='AgencyDistrict' value='{0}'>", user.AgencyDistrict);
        //	sb.AppendFormat("<input type='hidden' name='AgencyCity' value='{0}'>", user.AgencyCity);
        //	sb.AppendFormat("<input type='hidden' name='AgencyAddress' value='{0}'>", user.AgencyAddress);
        //	sb.AppendFormat("<input type='hidden' name='AgencySPOC' value='{0}'>", user.AgencySPOC);
        //	sb.AppendFormat("<input type='hidden' name='AgencyContact' value='{0}'>", user.AgencyContact);
        //	sb.AppendFormat("<input type='hidden' name='IsSSO' value='{0}'>", user.IsSSO);
        //	sb.AppendFormat("<input type='hidden' name='IsBhamashah' value='{0}'>", user.IsBhamashah);
        //	sb.AppendFormat("<input type='hidden' name='SSOToken' value='{0}'>", SSOToken);

        //	//sb.AppendFormat("<input type='hidden' name='KioskUserId' value='{0}'>", Session["KioskUserId"].ToString());
        //	//sb.AppendFormat("<input type='hidden' name='KioskSSOId' value='{0}'>", Session["KioskSSOId"].ToString());
        //	//sb.AppendFormat("<input type='hidden' name='SSOID' value='{0}'>", Session["SSOID"].ToString());
        //	// Other params go here
        //	sb.Append("</form>");

        //	// sb.Append("<iframe name='my-iframe' src='fmdss.html'></iframe>");
        //	//<iframe name="my-iframe" src="iframe.php"></iframe>

        //	sb.Append("</body>");
        //	sb.Append("</html>");
        //	Response.Write(sb.ToString());
        //	Response.End();

        //	return null;
        //}
        public ActionResult iframindex()
		{
			return View();
		}

        #endregion

        #region Sub Module Links
        public ActionResult SubModuleFMDSSMobile(string linkparam)
        {
            int ModuleId;
            string[] spl;
             string[] spl2 = linkparam.Split('|');
             linkparam = spl2[0];
            //string placename = FMDSS.Models.MySecurity.SecurityCode.EncodeUrl("Nahargarh Zoological Park,Jaipur");
            string placename = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(spl2[1]);
            Session["linkparam"] = linkparam;
            linkparam = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(linkparam);
            spl = linkparam.Split('|');
            ModuleId = Convert.ToInt32(spl[0]);
            Session["mainLink"] = spl[1];
            MiddleWareModules middleWareModules = new MiddleWareModules();
            DataTable dt = middleWareModules.GetPlaceList(ModuleId);
            ViewBag.PlaceList = dt;
            string exDate = "";
            middleWareModules.ExcludeDateList = new List<string>();
            string currentYear = DateTime.Now.Year.ToString();
            int cYear = Convert.ToInt32(currentYear);
            for (int y = cYear; y <= cYear + 1; y++)
            {
                for (int i = 7; i < 10; i++)
                {

                    for (int j = 1; j <= (i == 9 ? 30 : 31); j++)
                    {
                        exDate = (j < 10 ? "0" + j : "" + j) + "/" + "0" + i + "/" + y.ToString();
                        middleWareModules.ExcludeDateList.Add(exDate);
                    }

                }
            }
            String excludeList = Newtonsoft.Json.JsonConvert.SerializeObject(middleWareModules.ExcludeDateList);

            ViewBag.ExcludeDateList = middleWareModules.ExcludeDateList;
            ViewBag.CurrentDate = middleWareModules.GetCurrentDate(); //DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                                      //linkparam = FMDSS.Models.MySecurity.SecurityCode.EncodeUrl(linkparam);
            ViewBag.PlaceName = placename;


            //
            return View();
        }
        public ActionResult SubModuleFMDSS1(string linkparam)
        {
            int ModuleId;
            string[] spl;
            Session["linkparam"] = linkparam;
            linkparam = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(linkparam);
            spl = linkparam.Split('|');
            ModuleId = Convert.ToInt32(spl[0]);
            Session["mainLink"] = spl[1];
            MiddleWareModules middleWareModules = new MiddleWareModules();
            DataTable dt = middleWareModules.GetPlaceList(ModuleId);
            ViewBag.PlaceList = dt;
            string exDate = "";
            middleWareModules.ExcludeDateList = new List<string>();
            string currentYear = DateTime.Now.Year.ToString();
            int cYear = Convert.ToInt32(currentYear);
            for (int y = cYear; y <= cYear + 1; y++)
            {
                for (int i = 7; i < 10; i++)
                {

                    for (int j = 1; j <= (i == 9 ? 30 : 31); j++)
                    {
                        exDate = (j < 10 ? "0" + j : "" + j) + "/" + "0" + i + "/" + y.ToString();
                        middleWareModules.ExcludeDateList.Add(exDate);
                    }

                }
            }
            String excludeList = Newtonsoft.Json.JsonConvert.SerializeObject(middleWareModules.ExcludeDateList);

            ViewBag.ExcludeDateList = middleWareModules.ExcludeDateList;
            ViewBag.CurrentDate = middleWareModules.GetCurrentDate(); //DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                                      //linkparam = FMDSS.Models.MySecurity.SecurityCode.EncodeUrl(linkparam);
            ViewBag.IsMobile = 0;
           
           
                                                              //
            return View();
        }
        
        public ActionResult GetPlacesLinks(int ModuleId, string PageTitle)
        {

            MiddleWareModules middleWareModules = new MiddleWareModules();
            if (Session["SSODetail"] != null)
            {
                UserProfile User = (UserProfile)Session["SSODetail"];
                User.isRedirected = true;
                Session["SSODetail"] = User;
                Session["IsLogged"] = true;
                string mainLink = Session["mainLink"].ToString();
                middleWareModules.MWSMG_List = middleWareModules.GetPlaceLinks(ModuleId, PageTitle);
                middleWareModules.MWSMG_List.ToList().ForEach(s => s.MainLink = mainLink + "&returnURL=" + FMDSS.Models.MySecurity.SecurityCode.EncodeUrl(s.PageLinkUrl));
            }
            else
            {
                string mainLink = Session["mainLink"].ToString();
                Session["IsLogged"] = false;
                middleWareModules.MWSMG_List = middleWareModules.GetPlaceLinks(ModuleId, PageTitle);
                middleWareModules.MWSMG_List.ToList().ForEach(s => s.MainLink = mainLink + "&returnURL=" + FMDSS.Models.MySecurity.SecurityCode.EncodeUrl(s.PageLinkUrl));
            }
            
            return PartialView("_PartialPlaceLinks", middleWareModules);
        }
        #endregion
        #region Permit Availibility
        //public ActionResult GetPermitAvailibility(int PlaceId, DateTime BookingDate)
        public ActionResult GetPermitAvailibility(int PlaceId, string BookingDate)
        {
            //string bookingDate = BookingDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string bookingDate = BookingDate;
            //UserProfile User = (UserProfile)Session["SSODetail"];
            //User.isRedirected = true;
            //Session["SSODetail"] = User;

            MiddleWareModules middleWareModules = new MiddleWareModules();
            middleWareModules.AvailList = middleWareModules.GetPermitAvailibility(PlaceId, bookingDate);
            middleWareModules.AvailList.ToList().ForEach(s => s.ShiftName = (s.ShiftType == 1 ? "Morning" : (s.ShiftType == 2 ? "Evening" : "Full Day")));
            middleWareModules.PermitAvailList = new List<BookingPermit>();
            BookingPermit bp;

            int zoneid = 0; string zoneName = "";
            int ShiftType1 = 0; string ShiftName1 = ""; int Gypsy1 = 0; int Canter1 = 0;
            int ShiftType2 = 0; string ShiftName2 = ""; int Gypsy2 = 0; int Canter2 = 0;
            int ShiftType3 = 0; string ShiftName3 = ""; int Gypsy3 = 0; int Canter3 = 0;
            int PlaceID = 0;
            if (PlaceId != 65)
            {
                foreach (BookingPermitDetail bpd in middleWareModules.AvailList)
                {

                    if (zoneid != bpd.ZoneId && zoneid == 0)
                    {
                        zoneid = bpd.ZoneId;
                        zoneName = bpd.ZoneName;
                        ShiftType1 = bpd.ShiftType;
                        ShiftName1 = bpd.ShiftName;
                        Gypsy1 = bpd.Gypsy;
                        Canter1 = bpd.Canter;
                        PlaceID = bpd.PlaceID;
                    }
                    else if (zoneid == bpd.ZoneId)
                    {
                        zoneName = bpd.ZoneName;
                        PlaceID = bpd.PlaceID;
                        if (ShiftType2 == 0)
                        {
                            ShiftType2 = bpd.ShiftType;
                            ShiftName2 = bpd.ShiftName;
                            Gypsy2 = bpd.Gypsy;
                            Canter2 = bpd.Canter;
                        }
                        else
                        if (ShiftType3 == 0)
                        {
                            ShiftType3 = bpd.ShiftType;
                            ShiftName3 = bpd.ShiftName;
                            Gypsy3 = bpd.Gypsy;
                            Canter3 = bpd.Canter;
                        }
                    }
                    else
                    {
                        bp = new BookingPermit();

                        bp.PlaceID = PlaceId;
                        bp.ZoneId = zoneid;
                        bp.ZoneName = zoneName;
                        bp.ShiftType1 = ShiftType1;
                        bp.ShiftName1 = ShiftName1;
                        bp.Gypsy1 = Gypsy1;
                        bp.Canter1 = Canter1;

                        bp.ShiftType2 = ShiftType2;
                        bp.ShiftName2 = ShiftName2;
                        bp.Gypsy2 = Gypsy2;
                        bp.Canter2 = Canter2;

                        bp.ShiftType3 = ShiftType3;
                        bp.ShiftName3 = ShiftName3;
                        bp.Gypsy3 = Gypsy3;
                        bp.Canter3 = Canter3;


                        middleWareModules.PermitAvailList.Add(bp);
                        zoneName = ""; PlaceID = 0;
                        ShiftType1 = 0; ShiftName1 = ""; Gypsy1 = 0; Canter1 = 0;
                        ShiftType2 = 0; ShiftName2 = ""; Gypsy2 = 0; Canter2 = 0;
                        ShiftType3 = 0; ShiftName3 = ""; Gypsy3 = 0; Canter3 = 0;

                        zoneid = bpd.ZoneId;
                        zoneName = bpd.ZoneName;
                        ShiftType1 = bpd.ShiftType;
                        ShiftName1 = bpd.ShiftName;
                        Gypsy1 = bpd.Gypsy;
                        Canter1 = bpd.Canter;
                        PlaceID = bpd.PlaceID;
                    }

                }
                if (zoneName != "")
                {
                    bp = new BookingPermit();

                    bp.ZoneId = zoneid;
                    bp.ZoneName = zoneName;
                    bp.ShiftType1 = ShiftType1;
                    bp.ShiftName1 = ShiftName1;
                    bp.Gypsy1 = Gypsy1;
                    bp.Canter1 = Canter1;

                    bp.ShiftType2 = ShiftType2;
                    bp.ShiftName2 = ShiftName2;
                    bp.Gypsy2 = Gypsy2;
                    bp.Canter2 = Canter2;

                    bp.ShiftType3 = ShiftType3;
                    bp.ShiftName3 = ShiftName3;
                    bp.Gypsy3 = Gypsy3;
                    bp.Canter3 = Canter3;
                    bp.PlaceID = PlaceID;
                    middleWareModules.PermitAvailList.Add(bp);
                }
            }
            return PartialView("_PartialPermitAvailibility", middleWareModules);
        }
        #endregion
        #region Booking Fee Details
        public ActionResult GetBookingFee(int PlaceId, int BookingType, int NoOfPersons=1)
		{
			MiddleWareModules middleWareModules = new MiddleWareModules();
			DataTable dt = middleWareModules.GetBookingFeeDetails(PlaceId, BookingType,NoOfPersons);
			string strPlaceList = "1,21,22,30,58,59,60,61,66,67,72";
			string[] spl = strPlaceList.Split(',');
			bool isExist = Array.Exists(spl, x => x.Equals(PlaceId.ToString()));
			if (isExist==true)
			{
				middleWareModules.bookingFeeOther=Globals.Util.GetListFromTable<BookingFeesOther>(dt);
			}
			else
			{
				if(dt.Rows[0]["PlaceId"].ToString() == "65")
				{
					int i = -1;
					List<BookingFees> list= Globals.Util.GetListFromTable<BookingFees>(dt);
					string IndianGypsyFee = "0"; string ForeignerGypsyFee = "0";
					middleWareModules.bookingFeeList = new List<BookingFees>();
					foreach (var itm in list)
					{
						i++;
						//< td class="inth">@item.IndianGypsyFee</td>
						//                 <td class="inth">@item.ForeignerGypsyFee</td>
						//                 <td class="inth">@item.IndianCanterFee</td>
						//                 <td class="inth">@item.ForeignerCanterFee</td>
						if (i % 2 == 0)
						{
							IndianGypsyFee= itm.IndianGypsyFee;
							ForeignerGypsyFee= itm.ForeignerGypsyFee;
						}
						else
						{
							BookingFees bookingFees = new BookingFees();
							bookingFees.PlaceId = itm.PlaceId;
							bookingFees.IndianGypsyFee = IndianGypsyFee;
							bookingFees.ForeignerGypsyFee = ForeignerGypsyFee;
							bookingFees.IndianCanterFee = itm.IndianGypsyFee;
							bookingFees.ForeignerCanterFee = itm.ForeignerGypsyFee;
							
							middleWareModules.bookingFeeList.Add(bookingFees);
						}											
					}
				}
				else
				middleWareModules.bookingFeeList = Globals.Util.GetListFromTable<BookingFees>(dt);
			}
				



			return PartialView("_PartialBookingFee", middleWareModules);
		}
		#endregion
		#region Nursery Middle ware
		public ActionResult NurseryMiddleware()
		{
            ViewBag.FMDSS2_API_Url = ConfigurationManager.AppSettings["FMDSS2_API"];
           
            
            return View();
		}
        public ActionResult BookingSliderView()
        {
            return View();
        }
        public ActionResult TestView()
		{
			return View();
		}
		#endregion
	}
}
