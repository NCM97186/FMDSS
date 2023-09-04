using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Services;
using System.Text;
using System.Web.Mvc;
using FMDSS.Models;
using System.Data;
using FMDSS.App_Start;

namespace FMDSS.Controllers.UserProfiling
{
    public class KioskLoginController : BaseController
    {
        //
        // GET: /KioskLogin/
        UserProfile User = null;
        public ActionResult KioskLogin()
        {
            return View();
        }

        /// <summary>
        /// Function created to update the rate of forest produce
        /// </summary>
        /// <param name="frmData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult KisokSSOLoginSubmit(string ctznId)
        {
            try
            {
                RAJSSO.SSO SSO = new RAJSSO.SSO();
               // Session["SSOID"] = ctznId;
                Session["loggedin"] = true;

                RAJSSO.SSOWS.SSOUserDetail ssouser = SSO.GetUserDetailXML(ctznId, ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[0], ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[1]);
                if (ssouser != null && ssouser.displayName != null)
                {
                    User = new UserProfile()
                    {
                        SSOId = ctznId,
                        FullName = ssouser.displayName,
                        AadharId = ssouser.aadhaarId,
                        BhamashahId = ssouser.bhamashahId,
                        DatOFBirth = ssouser.dateOfBirth,
                        Gender = ssouser.gender,
                        PhotURL = ssouser.jpegPhoto,
                        EmailId = ssouser.mailPersonal,
                        MobileNumber = ssouser.mobile,
                        Designation = "10",
                        Address1 = ssouser.postalAddress,
                        PINCode1 = ssouser.postalCode,
                        District1 = ssouser.l,
                        Roles = "CITIZEN",
                        IsSSO = true,
                        IsBhamashah = false
                    };

                    DataTable dt = User.InsertUpdateUserInfo().Tables[0];
                    bool flag = false;
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            flag = Convert.ToBoolean(dt.Rows[0]["FIRSTTIMELOGIN"]);
                            Session["TempUserId"] = Convert.ToInt64(dt.Rows[0]["USERID"]);
                            Session["TempKioskCtznName"] = ssouser.displayName;
                            Session["otpNumber"] = generateRandomNumber();
                            //SMS_EMail_Services.sendSingleSMS("9166046444", Session["otpNumber"].ToString());
                            string ServiceName = KioskServiceName(Convert.ToString(Session["EmitrServiceId"]));
                            string msg = "OTP to avail " + ServiceName + " of Forest Department through e-mitra kiosk is " + Convert.ToString(Session["otpNumber"]) + ".OTP is confidential. Please do not share this with anyone.";
                            SMS_EMail_Services.sendSingleSMS(dt.Rows[0]["Mobile"].ToString(), msg);
                        }
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                { Session["ValidSSO"] = "false"; return Json("failure", JsonRequestBehavior.AllowGet); }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string KioskServiceName(string val)
        {
            string status = "";
            switch (val)
            {
                case "2203":
                    status = "Forest-NOC for Mining";
                    break;
                case "2204":
                    status = "Forest-NOC for activities other than Mining";
                    break;
                case "2205":
                    status = "Forest-Research in Wildlife";
                    break;
                case "2216":
                    status = "Forest-Research in any Forest";
                    break;
                case "2217":
                    status = "Forest-Visit Service in Wildlife";
                    break;
                case "2218":
                    status = "Forest-Visit Service in any Forest in Rajasthan";
                    break;
                case "2226":
                    status = "Forest-Register Parivad";
                    break;
                case "2230":
                    status = "Forest-NOC for Cable Lines";
                    break;
                case "2224":
                    status = "Forest-NOC for  Electricity Lines";
                    break;
                case "2225":
                    status = "Forest-NOC for  Industry Set-up";
                    break;
                case "2231":
                    status = "Forest-NOC for Hospital";
                    break;
                case "2232":
                    status = "Forest-NOC for Power Plant";
                    break;
                case "2233":
                    status = "Forest-NOC for  School Permission";
                    break;
                case "2234":
                    status = "Forest - NOC for  Road/ Highway";
                    break;
                case "2246":
                    status = "Forest-NOC for Sawmill Permission";
                    break;
                case "2235":
                    status = "Forest-NOC for  Telephone Lines";
                    break;
            }
            return status;
        }
        [HttpPost]
        public JsonResult KisokFMDSSLoginSubmit(string ctznId)
        {
            try
            {
                User = new UserProfile();
                DataTable dt = User.ValidateFMDSSUser(ctznId.Trim());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Session["UserId"] = Convert.ToInt64(dt.Rows[0]["USERID"]);
                        Session["KioskCtznName"] = dt.Rows[0]["Name"];
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
            catch { return Json("failure", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public ActionResult KisokBHAMALoginSubmit(string ctznId)
        {
            try
            {
                BhamashaError BhamashaError = new BhamashaError();
                try
                {
                    BhamashaRoot BhamashaData = cls_Bhamasha.GetBhamashaInfo(ctznId.Trim());
                    Session["BhamashahFamilyId"] = ctznId.Trim();
                    if (BhamashaData.Cmsg == "")
                    {
                        return PartialView("_MemberDetails", BhamashaData);
                    }
                    else
                    {

                        if (BhamashaData.Cmsg == "101")
                        {
                            BhamashaError.errorcode = "101";
                            BhamashaError.errorDescription = "Scheme name is null or not valid";
                        }
                        if (BhamashaData.Cmsg == "110")
                        {
                            BhamashaError.errorcode = "110";
                            BhamashaError.errorDescription = "Family Id is not valid. It should have 7 characters";
                        }
                        if (BhamashaData.Cmsg == "116")
                        {
                            BhamashaError.errorcode = "116";
                            BhamashaError.errorDescription = "Family Id is not valid.";
                        }
                        if (BhamashaData.Cmsg == "107")
                        {
                            BhamashaError.errorcode = "107";
                            BhamashaError.errorDescription = "Aadhar number should be 12 digit number.";
                        }
                        if (BhamashaData.Cmsg == "112")
                        {
                            BhamashaError.errorcode = "112";
                            BhamashaError.errorDescription = "fatal error occurs.";
                        }
                        return Json(BhamashaError, JsonRequestBehavior.AllowGet); //PartialView(BhamashaError);
                    }
                }
                catch (Exception ex)
                {
                    BhamashaError.errorcode = ex.Message;
                    BhamashaError.errorDescription = ex.Message;
                    return Json(BhamashaError, JsonRequestBehavior.AllowGet); //PartialView(BhamashaError);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Function created to update the rate of forest produce
        /// </summary>
        /// <param name="frmData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveBhamashahMember(string address, string DistrictName)
        {
            string sMsg = string.Empty;
            try
            {
                if (Session["BhamaShahMember"] != null)
                {
                    BhamashaRoot BhamashaData = (BhamashaRoot)Session["BhamaShahMember"];
                    User = new UserProfile()
                    {
                        SSOId = BhamashaData.PersonalInfo.Member[0].Aadhar,
                        FullName = BhamashaData.PersonalInfo.Member[0].NameEng,
                        AadharId = BhamashaData.PersonalInfo.Member[0].Aadhar,
                        BhamashahId = (string)Session["BhamashahFamilyId"],
                        DatOFBirth = (string)Session["BhamashahMemberDOB"],
                        Gender = BhamashaData.PersonalInfo.Member[0].Gender,
                        EmailId = BhamashaData.PersonalInfo.Member[0].Email,
                        MobileNumber = BhamashaData.PersonalInfo.Member[0].Mobile,
                        Designation = "10",
                        Address1 = address,
                        District1 = DistrictName,
                        Roles = "CITIZEN",
                        IsSSO = false,
                        IsBhamashah = true
                    };
                    DataTable dt = User.InsertUpdateUserInfo().Tables[0];
                    bool flag = false;

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            flag = Convert.ToBoolean(dt.Rows[0]["FIRSTTIMELOGIN"]);
                            Session["UserId"] = Convert.ToInt64(dt.Rows[0]["USERID"]);
                            Session["KioskCtznName"] = BhamashaData.PersonalInfo.Member[0].NameEng;
                            sMsg = "BHAMASHAH";
                            //Session["otpNumber"] = generateRandomNumber();
                            //SMS_EMail_Services.sendSingleSMS(dt.Rows[0]["Mobile"].ToString(), Session["otpNumber"].ToString());
                        }
                    }
                    else
                        sMsg = "failure";

                    // return RedirectToAction("Dashboard", "Dashboard", false);
                }
                return Json(sMsg, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="frmData"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult OTPValidation(FormCollection frmData)
        //{
        //    try
        //    {
        //        if (Convert.ToString(frmData["txtOTP"]).Contains(Convert.ToString(Session["otpNumber"])))
        //        {
        //            Session["otpNumber"] = null;
        //            Session["UserId"] = Session["TempUserId"];
        //            Session["KioskCtznName"] = Session["TempKioskCtznName"];
        //            Session["TempUserId"] = null; Session["TempKioskCtznName"] = null;
        //            return RedirectToAction("Dashboard", "Dashboard", false);
        //        }
        //        else
        //        {
        //            Session["ValidOTP"] = "false";
        //            return View("KioskLogin");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}


        [HttpPost]
        public ActionResult OTPValidation(FormCollection frmData)
        {
            try
            {
                if (Convert.ToString(frmData["txtOTP"]).Contains(Convert.ToString(Session["otpNumber"])))
                {
                    Session["otpNumber"] = null;
                    Session["UserId"] = Session["TempUserId"];
                    Session["UserID"] = Session["TempUserId"];
                    Session["KioskCtznName"] = Session["TempKioskCtznName"];
                    Session["TempUserId"] = null; Session["TempKioskCtznName"] = null;
                    if (Session["EmitrServiceId"] != null)
                    {

                        // User = new UserProfile();
                        DataTable dtService = new UserProfile().FmdsServiceDetails(Convert.ToString(Session["EmitrServiceId"]));
                        if (dtService.Rows.Count > 0)
                        {
                            Response.Redirect("~/" + dtService.Rows[0]["Controller_Name"].ToString() + "/" + dtService.Rows[0]["ViewAction_Name"].ToString(), false);                           
                        }

                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Dashboard", false);
                    }
                    return null;
                    // return RedirectToAction("Dashboard", "Dashboard", false);
                }
                else
                {
                    Session["ValidOTP"] = "false";
                    return View("KioskLogin");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frmData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GoBackToCitizen(FormCollection frmData)
        {
            try
            {
                Session["otpNumber"] = null; Session["ValidOTP"] = null;
                if (Session["EmitrServiceId"] != null)
                {
                    return RedirectToAction("KioskDashboard", "KioskDashboard", false);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Dashboard", false);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string generateRandomNumber()
        {
            string numbers = "1234567890";
            string otp = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, numbers.Length);
                    character = numbers.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
    }
}
