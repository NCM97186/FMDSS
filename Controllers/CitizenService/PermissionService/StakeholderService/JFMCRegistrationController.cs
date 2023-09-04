//////////////////////////////////////////
///////////Bug no :-388



using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionService.StakeholderService;
using System.Data.SqlClient;
using System.IO;

namespace FMDSS.Controllers.CitizenService.PermissionService.StakeholderService
{
    public class JFMCRegistrationController : BaseController
    {
        List<SelectListItem> RangeName = new List<SelectListItem>();
        DAL dal = new DAL();
        System.Text.StringBuilder sbRequestId = new System.Text.StringBuilder();             
        /// <summary>
        /// Show JFMC Registration from
        /// </summary>
        /// <returns></returns>       
        public ActionResult JFMCRegistration()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            List<StakeholderServices> lstJfmc = new List<StakeholderServices>();
            DataSet dsJfmc = new DataSet();
            Session.Remove("RegistId");
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Option", "1"), 
                                              new SqlParameter("@Id", Convert.ToInt64(Session["UserID"].ToString()))
                                           };
                dal.Fill(dsJfmc, "Sp_Citizen_GetJFMCRegistrationlist",parameters);
                getDistricts();
                //ViewBag.DistrictName = dsJfmc.Tables[0];
                foreach (System.Data.DataRow dr in dsJfmc.Tables[0].Rows)
                {
                    lstJfmc.Add(new StakeholderServices
                    {
                        Id= Convert.ToInt64(@dr["id"].ToString()),                       
                        RANGE_CODE = @dr["RANGE_NAME"].ToString(),
                        village = @dr["VILL_NAME"].ToString(),
                        Committee_Name = @dr["Committee_Name"].ToString(),
                        Secretary_Name = @dr["Secretary_Name"].ToString()                                       
                    });
                }
                ViewBag.Jfmclist = lstJfmc;
                DataTable dtRange = new Common().Select_Range(Convert.ToInt64(Session["UserID"].ToString()));
                foreach (DataRow dr in dtRange.Rows)
                {
                    RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.ddlRange = RangeName;                              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View();
        }
       /// <summary>
       /// Bind district drop down
       /// </summary>
        public void getDistricts()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            try {
                List<SelectListItem> lstDistrict = new List<SelectListItem>();
                DataSet dsDistrict = new DataSet();
                SqlParameter[] parameters = { new SqlParameter("@action", "District")                                             
                                           };
                dal.Fill(dsDistrict, "Sp_Common_Select_District", parameters);
                ViewBag.DistrictName = dsDistrict.Tables[0];
                foreach (System.Data.DataRow dr in ViewBag.DistrictName.Rows)
                {
                    lstDistrict.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.DistrictName = lstDistrict;                                   
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }            
        }
        /// <summary>
        /// Bind block drop down on district code
        /// </summary>
        /// <param name="dist_code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getBlock(string dist_code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            List<SelectListItem> lstblock = new List<SelectListItem>();
            try {
              
                DataSet dsBlock = new DataSet();
                SqlParameter[] paramBlock = { new SqlParameter("@distid", dist_code), };
                dal.Fill(dsBlock, "Sp_Common_Select_Block", paramBlock);
                ViewBag.fname = dsBlock.Tables[0];

                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    if (@dr["BLK_NAME"].ToString() != "--Select--") {
                        lstblock.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });                    
                    }                    
                }               
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(new SelectList(lstblock, "Value", "Text"));
        }
        /// <summary>
        /// Bind gram panchayat on district code
        /// </summary>
        /// <param name="dist_code"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getGPs(string dist_code,string block )
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            List<SelectListItem> lstgetGPs = new List<SelectListItem>();
            try {                
                DataSet dsBlock = new DataSet();
                SqlParameter[] paramBlock = {new SqlParameter("@DISID",dist_code), 
                                        new SqlParameter("@BLKID",block),          
                                       };
                dal.Fill(dsBlock, "[Sp_Common_Select_GramPanchayat]", paramBlock);
                ViewBag.fname = dsBlock.Tables[0];
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    if (@dr["GP_NAME"].ToString() != "--Select--")
                    {
                        lstgetGPs.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }                    
                }
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }            
            return Json(new SelectList(lstgetGPs, "Value", "Text"));
        }
        /// <summary>
        /// Bind village on dist code block code and gp code
        /// </summary>
        /// <param name="District_code"></param>
        /// <param name="Block_code"></param>
        /// <param name="GP_Code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getVillage(string District_code,string Block_code,string GP_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            List<SelectListItem> lstVillage = new List<SelectListItem>();
            try {               
                DataSet dsBlock = new DataSet();
                SqlParameter[] paramBlock = {new SqlParameter("@DISID",District_code), 
                                        new SqlParameter("@BLKID",Block_code),                                                            
                                        new SqlParameter("@GPID",GP_Code),  
                                       };
                dal.Fill(dsBlock, "Sp_Common_Select_Village", paramBlock);
                ViewBag.fname = dsBlock.Tables[0];

                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    if (@dr["VILL_NAME"].ToString() != "--Select--")
                    {
                        lstVillage.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    
                }               
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }            
            return Json(new SelectList(lstVillage, "Value", "Text"));
        }

        /// <summary>
        /// Bind village on dist code block code and gp code
        /// </summary>
        /// <param name="District_code"></param>
        /// <param name="Block_code"></param>
        /// <param name="GP_Code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getRegistrationId(string Range_code, string Vill_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> lstVillage = new List<SelectListItem>();           
            try
            {                              
                if (Range_code != "0" && Convert.ToString(Range_code) != "")
                {
                    sbRequestId.Append(Range_code);
                }
                if (Vill_Code != "0" && Convert.ToString(Vill_Code) != "")
                {
                    sbRequestId.Append(Vill_Code);
                }
                sbRequestId.Append(DateTime.Now.Year.ToString());
                sbRequestId.Append(DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length-5,5));               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }             
            return Json(new { list1 = sbRequestId.ToString() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Click to submit data into database
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="fileMom"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(FormCollection frm, StakeholderServices Stkhld, HttpPostedFileBase Upload_MOM)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
          try{
              string Document = string.Empty;
              var path = "";                               
              if (Upload_MOM != null && Upload_MOM.ContentLength > 0)
              {
                  Document = Path.GetFileName(Upload_MOM.FileName);
                  String FileFullName = DateTime.Now.Ticks + "_" + Document;
                  path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                  Stkhld.Upload_MOM = path;
                  Upload_MOM.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
              }
              else
              {
                  Stkhld.Upload_MOM = "";
              }           
              if (frm["Bank_Name"]==null || frm["Bank_Name"]=="") {
                  Stkhld.Bank_Name = "";
              }
              if (frm["Branch_Name"] == null || frm["Branch_Name"] == "")
              {
                  Stkhld.Branch_Name = "";
              }
              if (frm["Branch_Id"] == null || frm["Branch_Id"] == "")
              {
                  Stkhld.Branch_Id = "";
              }
              if (frm["Account_No"] == null || frm["Account_No"] == "")
              {
                  Stkhld.Account_No = "0";
              }
              if (frm["Account_Type"] == null || frm["Account_Type"] == "")
              {
                  Stkhld.Account_Type = "";
              }
              if (frm["hdnId"] == null || frm["hdnId"] == "")
              {
                  Stkhld.Id = 0;
              }
              else {
                  Stkhld.Id = Convert.ToInt64(frm["hdnId"].ToString()); 
              }                                                                              
              Session["RegistId"] = Stkhld.Registration_Id;              
              Int64 ststus = Stkhld.InsertJFMC(Stkhld);

              if (ststus == Stkhld.Id)
              {
                  TempData["JFMC"] = "VFPMC Updated Sucessfully with Id " + ststus;
              }
              else {
                  TempData["JFMC"] = "VFPMC Registrated Sucessfully with Id " + ststus;
              }              
             }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            var RegistrationId = Encryption.encrypt(Session["RegistId"].ToString());
            var JfmcName = Encryption.encrypt(Stkhld.Committee_Name);
            return RedirectToAction("JFMCMember", "JFMCMember", new { RegistrationId, JfmcName });         
        }
        /// <summary>
        /// Edit function of jfmc 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult Edit(string Id) {
            DataTable dtJfmc = new DataTable();
            StakeholderServices _objedit = new StakeholderServices();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {                
                SqlParameter[] parameters = { new SqlParameter("@Option", "2"), 
                                              new SqlParameter("@Id", Id)
                                            };
                dal.Fill(dtJfmc, "Sp_Citizen_GetJFMCRegistrationlist", parameters);
                if (dtJfmc != null) {
                    if (dtJfmc.Rows.Count > 0) {                        
                        _objedit.RANGE_CODE = dtJfmc.Rows[0]["Range_Code"].ToString();
                        _objedit.Vill_Code = dtJfmc.Rows[0]["Vill_Code"].ToString();
                        _objedit.Minutes_of_Meeting = dtJfmc.Rows[0]["Minutes_of_Meeting"].ToString();
                        _objedit.Committee_Name = dtJfmc.Rows[0]["Committee_Name"].ToString();
                        _objedit.Registration_Id = dtJfmc.Rows[0]["Registration_Id"].ToString();
                        _objedit.Secretary_Name = dtJfmc.Rows[0]["Secretary_Name"].ToString();
                        _objedit.Ssoid = dtJfmc.Rows[0]["Secretary_ssoid"].ToString();
                        _objedit.Lattitude = Convert.ToDecimal(dtJfmc.Rows[0]["Lattitude"].ToString());
                        _objedit.Longitude = Convert.ToDecimal(dtJfmc.Rows[0]["Longitude"].ToString());
                        _objedit.Village_Area = Convert.ToDecimal(dtJfmc.Rows[0]["Village_Area"].ToString());
                        _objedit.Plantation_Area = Convert.ToDecimal(dtJfmc.Rows[0]["Plantation_Area"].ToString());
                        _objedit.Total_Area_Managed = Convert.ToDecimal(dtJfmc.Rows[0]["Total_Area_Managed"].ToString());
                        _objedit.Bank_Account_Operated = dtJfmc.Rows[0]["Bank_Account_Operated"].ToString();
                        _objedit.Bank_Name = dtJfmc.Rows[0]["Bank_Name"].ToString();
                        _objedit.Branch_Name = dtJfmc.Rows[0]["Branch_Name"].ToString();
                        _objedit.Branch_Id = dtJfmc.Rows[0]["Branch_Id"].ToString();
                        _objedit.Account_No = dtJfmc.Rows[0]["Account_No"].ToString();
                        _objedit.Account_Type = dtJfmc.Rows[0]["Account_Type"].ToString();
                        _objedit.MembershipFees = Convert.ToInt64(dtJfmc.Rows[0]["MembershipFees"].ToString());
                        _objedit.Income_Deposit_ForestOffice = Convert.ToInt64(dtJfmc.Rows[0]["Income_Deposit_ForestOffice"].ToString());
                        _objedit.Income_Generated_Activity = Convert.ToInt64(dtJfmc.Rows[0]["Income_Generated_Activity"].ToString());
                        _objedit.Income_Sale_ForestProduce = Convert.ToInt64(dtJfmc.Rows[0]["Income_Sale_ForestProduce"].ToString());
                        _objedit.Other_Sources = dtJfmc.Rows[0]["Other_Sources"].ToString();
                        _objedit.Total_Income = Convert.ToInt64(dtJfmc.Rows[0]["Total_Income"].ToString());
                        _objedit.Benefits_Plant_Grass_Quantity = Convert.ToInt64(dtJfmc.Rows[0]["Benefits_Plant_Grass_Quantity"].ToString());
                        _objedit.Benefits_Plant_Grass_Amount = Convert.ToInt64(dtJfmc.Rows[0]["Benefits_Plant_Grass_Amount"].ToString());
                        _objedit.Minor_Forest_Produce_Quantity = Convert.ToInt64(dtJfmc.Rows[0]["Minor_Forest_Produce_Quantity"].ToString());
                        _objedit.Minor_Forest_Produce_Amount = Convert.ToInt64(dtJfmc.Rows[0]["Minor_Forest_Produce_Amount"].ToString());
                        _objedit.Other_ForestProduce_Quantity = Convert.ToInt64(dtJfmc.Rows[0]["Other_ForestProduce_Quantity"].ToString());
                        _objedit.Other_ForestProduce_Amount = Convert.ToInt64(dtJfmc.Rows[0]["Other_ForestProduce_Amount"].ToString());
                        _objedit.No_Of_Beneficiaries_LastThreeYear = Convert.ToInt64(dtJfmc.Rows[0]["No_Of_Beneficiaries_LastThreeYear"].ToString());
                        _objedit.No_Of_Beneficiaries_CurrentYear = Convert.ToInt64(dtJfmc.Rows[0]["No_Of_Beneficiaries_CurrentYear"].ToString());
                        _objedit.Efficient_Grades =dtJfmc.Rows[0]["Efficient_Grades"].ToString();
                        _objedit.No_Of_NGO_Working = dtJfmc.Rows[0]["No_Of_NGO_Working"].ToString();
                        _objedit.Self_Help_Group_Men = Convert.ToInt32(dtJfmc.Rows[0]["Self_Help_Group_Men"].ToString());
                        _objedit.Self_Help_Group_Women = Convert.ToInt32(dtJfmc.Rows[0]["Self_Help_Group_Women"].ToString());
                        _objedit.Self_Help_Group_Others = dtJfmc.Rows[0]["Self_Help_Group_Others"].ToString();
                        _objedit.Self_Help_Group_Total = Convert.ToInt32(dtJfmc.Rows[0]["Self_Help_Group_Total"].ToString());
                        _objedit.Overall_Remarks = dtJfmc.Rows[0]["Overall_Remarks"].ToString();
                        _objedit.TypeVfmc = dtJfmc.Rows[0]["Vfmc_Type"].ToString();

                    }                
                }
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(_objedit, JsonRequestBehavior.AllowGet);
        }
    }
}
