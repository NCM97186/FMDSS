using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using FMDSS.Models;
using FMDSS.Filters;


namespace FMDSS.Controllers.UserProfiling
{
    [MyAuthorization]
    public class ProfileController : BaseController
    {
        List<SelectListItem> districts = new List<SelectListItem>();
        BindMasterData MasterData = new BindMasterData();
        UserProfile User = new UserProfile();

        public ActionResult Profiling()
        {
            try
            {
                //if (Session["SSODetail"] != null)
                //  User = (UserProfile)Session["SSODetail"];
                DataTable dtResult = new DataTable();
                dtResult = User.AuthenticateUser(Session["SSOID"].ToString());
                if (dtResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        User = new UserProfile()
                        {
                            SSOId = dr["Ssoid"].ToString(),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            FullName = dr["Name"].ToString(),
                            EmailId = dr["EmailId"].ToString(),
                            MobileNumber = dr["Mobile"].ToString(),
                            Designation = dr["Designation"].ToString(),
                            Address1 = dr["Postal_address1"].ToString(),
                            PINCode1 = dr["Postal_code1"].ToString(),
                            District1 = dr["District1"].ToString(),
                            Address2 = dr["Postal_Address2"].ToString(),
                            PINCode2 = dr["Postal_Code2"].ToString(),
                            District2 = dr["District2"].ToString(),
                            City2 = dr["City2"].ToString(),
                            Roles = dr["RoleId"].ToString(),
                            IsKioskUser = Convert.ToBoolean(dr["IsKioskUser"]),
                            IsAgency = Convert.ToBoolean(dr["IsAgency"]),
                        };
                    }
                    DataTable dt = new DataTable();
                    dt = MasterData.getDistricts();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        districts.Add(new SelectListItem { Text = @dr["Dist_Name"].ToString(), Value = @dr["Dist_Code"].ToString() });
                    }
                    ViewBag.District2 = districts;
                    ViewBag.AgencyDistrict = districts;
                }
                return View(User);
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }

        [HttpPost]
        public JsonResult getTehsils(string dist)
        {
            BindMasterData bindMaster = new BindMasterData();
            DataTable dt = bindMaster.getTehsils(dist);
            ViewBag.fname = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Method is responsible to update user details
        /// </summary>
        /// <param name="login"></param>
        /// <param name="formUser"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateUserDetails(UserProfile login, FormCollection formUser)
        {
            login.SSOId = Session["SSOid"].ToString(); login.IsSSO = false; login.IsBhamashah = false;
            if (formUser["Applicant_type"].ToString() == "1")
                login.IsAgency = false;
            else
                login.IsAgency = true;
            DataTable dt = login.InsertUpdateUserInfo().Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewData["i"] = Convert.ToInt64(dt.Rows[0][1].ToString());
                }
            }
            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}
