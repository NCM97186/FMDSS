//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : JFMCMemberController
//  Description  : File contains calling functions from UI
//  Date Created : 09-03-2016
//  History      : 
//  Version      : 1.0
//  Author       : Rajkumar Singh
//********************************************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using System.Data.SqlClient;
using FMDSS.Models.ForesterDevelopment;
using FMDSS.Filters;
using System.Configuration;
using System.Xml;

namespace FMDSS.Controllers.ForesterDevelopment
{
    [MyAuthorization]
    public class JFMCMemberController : BaseController
    {      
        DAL dal = new DAL();       
        /// <summary>
        /// VFMC Member page
        /// </summary>
        /// <param name="RegistrationId"></param>
        /// <param name="JfmcName"></param>
        /// <returns></returns>
            public ActionResult JFMCMember(string RegistrationId,string JfmcName)
            {
               string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
               string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
               JFMCMember jfmc = new JFMCMember();
               Session["JFMCMemberAdd"] = null;
               try {
                   if (RegistrationId != null && JfmcName != null)
                   {
                       RegistrationId = Encryption.decrypt(RegistrationId);
                       jfmc.JFMCRegistrationId = Convert.ToString(RegistrationId);
                       JfmcName = Encryption.decrypt(JfmcName);
                       jfmc.VFMCName = JfmcName;
                       Session["JFMCRegistrationId"] = jfmc.JFMCRegistrationId; 
                       return View(jfmc);
                   }
                   else
                   {
                       return View();
                   }
               }
               catch (Exception ex) {
                   new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
                   return null;
               }                                  
            } 
        
       

            /// <summary>
            /// final submission of data
            /// </summary>
            /// <param name="_objjfmc"></param>
            /// <param name="frm"></param>
            /// <param name="Command"></param>
            /// <returns></returns>
            [HttpPost]
            public ActionResult Submit(JFMCMember _objjfmc,FormCollection frm,string Command) {
                Int64 status=0;
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
                try
                {
                    if (Command == "Save")
                    {                       
                        DataTable dtjfmc = new DataTable("Table");
                        dtjfmc.Columns.Add("SSOId", typeof(String));
                        dtjfmc.Columns.Add("FullName", typeof(String));
                        dtjfmc.Columns.Add("AadharId", typeof(String));
                        dtjfmc.Columns.Add("BhamashahId", typeof(String));
                        dtjfmc.Columns.Add("DatOFBirth", typeof(String));
                        dtjfmc.Columns.Add("Gender", typeof(String));
                        dtjfmc.Columns.Add("EmailId", typeof(String));
                        dtjfmc.Columns.Add("MobileNo", typeof(String));
                        dtjfmc.Columns.Add("Designation", typeof(String));
                        dtjfmc.Columns.Add("Address", typeof(String));
                        dtjfmc.Columns.Add("Roles", typeof(String));
                        dtjfmc.AcceptChanges();
                        List<JFMCMember> jfmc = new List<JFMCMember>();
                        if (Session["JFMCMemberAdd"] != null)
                        {
                            jfmc = (List<JFMCMember>)Session["JFMCMemberAdd"];
                        }
                        foreach (JFMCMember objjfmc in jfmc)
                        {
                            DataRow dr = dtjfmc.NewRow();
                            dr["SSOId"] = objjfmc.SSOId;
                            dr["FullName"] = objjfmc.FullName;
                            dr["AadharId"] = objjfmc.AadharId;
                            dr["BhamashahId"] = objjfmc.BhamashahId;
                            dr["DatOFBirth"] = objjfmc.DatOFBirth;
                            dr["Gender"] = objjfmc.Gender;
                            dr["EmailId"] = objjfmc.EmailId;
                            dr["MobileNo"] = objjfmc.MobileNumber;
                            dr["Designation"] = objjfmc.Designation;
                            dr["Address"] = objjfmc.Address1;
                            dr["Roles"] = objjfmc.Roles;
                            dtjfmc.Rows.Add(dr);
                            dtjfmc.AcceptChanges();
                        }                                        
                        status = _objjfmc.InsertJFMCMember(_objjfmc, dtjfmc);
                       
                        if (status > 0)
                        {
                            ViewData["JFMC"] = "VFPMC Member Registrated Sucessfully with Id " + status;
                            Session["JFMCMemberAdd"] = null;
                        }
                        else
                        {
                            ViewData["JFMC"] = "No inserted";
                            Session["JFMCMemberAdd"] = null;
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
                }             
                return View("JFMCMember");
            }

            public JsonResult GetJFMCMember(string jfmcRegistration)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                JFMCMember JfmcMember = new JFMCMember();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                List<JFMCMember> lstJfmc = new List<JFMCMember>();
                DataTable dtMember = new DataTable();
                dtMember = JfmcMember.GetJFMCMember(Convert.ToString(jfmcRegistration));
                try
                {
                    foreach (System.Data.DataRow dr in dtMember.Rows) {
                        lstJfmc.Add(new JFMCMember { 
                          rowid = Guid.NewGuid().ToString(),
                          SSOId = Session["SSOID"].ToString(),
                          FullName = dr["FullName"].ToString(),
                          AadharId = dr["AadharId"].ToString(),
                          DatOFBirth = dr["DateOfBirth"].ToString(),
                          Gender = dr["Gender"].ToString(),
                          MobileNumber = dr["MobileNo"].ToString(),
                          Designation = dr["Designation"].ToString(),
                          Roles = dr["Roles"].ToString(),
                          Address1 = dr["Address1"].ToString(),                        
                        });                                            
                    }
                    Session["JFMCMemberAdd"] = lstJfmc;                   
                }
                catch (Exception ex) { new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
                finally { }
                return Json(new { list1 = lstJfmc, JsonRequestBehavior.AllowGet });
            }
            /// <summary>
            /// Add vfmc member 
            /// </summary>
            /// <param name="member"></param>
            /// <returns></returns>
            public JsonResult AddMember(JFMCMember JM)
            {              
                JFMCMember JfmcMember = new JFMCMember();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                List<JFMCMember> lstJfmc = new List<JFMCMember>();               
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                try
                {                                         
                    JfmcMember.SSOId = Session["SSOID"].ToString();
                    JfmcMember.FullName = JM.FullName;
                    JfmcMember.AadharId = JM.AadharId;
                    JfmcMember.DatOFBirth = JM.DatOFBirth;
                    JfmcMember.Gender = JM.Gender;
                    JfmcMember.MobileNumber = JM.MobileNumber;
                    JfmcMember.Designation = "10";
                    JfmcMember.Roles = "CITIZEN";
                    JfmcMember.Address1 = JM.Address1;                        
                    if (Session["JFMCMemberAdd"] != null)
                    {
                        lstJfmc = (List<JFMCMember>)Session["JFMCMemberAdd"];
                        if (!lstJfmc.Any(element => element.DatOFBirth == JfmcMember.DatOFBirth))
                        {
                            JfmcMember.rowid = Guid.NewGuid().ToString();
                            lstJfmc.Add(JfmcMember);
                        }
                    }
                    else
                    {
                        JfmcMember.rowid = Guid.NewGuid().ToString();
                        lstJfmc.Add(JfmcMember);
                        Session["JFMCMemberAdd"] = lstJfmc;
                    }                                     
                }
                catch (Exception ex) { new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
                finally { }
                return Json(new { list1 = lstJfmc, JsonRequestBehavior.AllowGet });
            }
       /// <summary>
       /// Add vfmc member 
       /// </summary>
       /// <param name="member"></param>
       /// <returns></returns>
        public JsonResult AddJFMCMember(string member)
          { 
            UserProfile JFMCUser= new UserProfile();
            JFMCMember JfmcMember = new JFMCMember();         
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<JFMCMember> lstJfmc = new List<JFMCMember>();
            string valid = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            try
            {
                RAJSSO.SSO SSO = new RAJSSO.SSO();
                Session["SSOID"] = member;
                Session["loggedin"] = true;
                RAJSSO.SSOWS.SSOUserDetail ssouser = SSO.GetUserDetailXML(Session["SSOID"].ToString(), ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[0], ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[1]);
                if (ssouser != null && ssouser.displayName != null)
                {
                    //JFMCUser = new UserProfile()
                    //    {
                    //        SSOId = Session["SSOID"].ToString(),
                    //        FullName = ssouser.displayName,
                    //        AadharId = ssouser.aadhaarId,
                    //        BhamashahId = ssouser.bhamashahId,
                    //        DatOFBirth = ssouser.dateOfBirth,
                    //        Gender = ssouser.gender,
                    //        PhotURL = ssouser.jpegPhoto,
                    //        EmailId = ssouser.mailPersonal,
                    //        MobileNumber = ssouser.mobile,
                    //        Designation = "10",
                    //        Address1 = ssouser.postalAddress,
                    //        PINCode1 = ssouser.postalCode,
                    //        District1 = ssouser.l,
                    //        Roles = "CITIZEN",
                    //    };

                    JfmcMember.SSOId = Session["SSOID"].ToString();
                    JfmcMember.FullName = ssouser.displayName;
                    JfmcMember.AadharId = ssouser.aadhaarId;
                    JfmcMember.BhamashahId = ssouser.bhamashahId;
                    JfmcMember.DatOFBirth = ssouser.dateOfBirth;
                    JfmcMember.Gender = ssouser.gender;
                    JfmcMember.EmailId = ssouser.mailPersonal;
                    JfmcMember.MobileNumber = ssouser.mobile;
                    JfmcMember.Designation = "10";
                    JfmcMember.Roles = "CITIZEN";
                    valid = "valid";

                    if (Session["JFMCMemberAdd"] != null)
                    {
                        lstJfmc = (List<JFMCMember>)Session["JFMCMemberAdd"];
                        if (!lstJfmc.Any(element => element.SSOId == JfmcMember.SSOId))
                        {
                            JfmcMember.rowid = Guid.NewGuid().ToString();
                            lstJfmc.Add(JfmcMember);
                        }
                    }
                    else
                    {
                        JfmcMember.rowid = Guid.NewGuid().ToString();
                        lstJfmc.Add(JfmcMember);
                        Session["JFMCMemberAdd"] = lstJfmc;
                    }          
                }
                else {
                    if (Session["JFMCMemberAdd"] != null)
                    {
                        lstJfmc = (List<JFMCMember>)Session["JFMCMemberAdd"];
                        valid = "Invalid";
                    }                   
                }                                    
            }
            catch (Exception ex) { new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            finally { }
            return Json(new { list1 = lstJfmc, list2 = valid, JsonRequestBehavior.AllowGet });                        
            }
        /// <summary>
        /// Delete VFMC Member from list
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteJFMCMember(string Id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            DataTable dtJfmc = new DataTable();
            List<JFMCMember> lstJfmc = new List<JFMCMember>();
            try
            {
                if (Session["JFMCMemberAdd"] != null)
                {
                    lstJfmc = (List<JFMCMember>)Session["JFMCMemberAdd"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        // DataRow dr = (DataRow)dtMP.Select("rowID=" + Id).First();
                        JFMCMember jfmcm = lstJfmc.Single(a => a.rowid == Id);
                        lstJfmc.Remove(jfmcm);
                    }
                    // dtMP.Rows.Remove((DataRow)dtMP.Select("rowID=" + Id).First());
                    Session["JFMCMemberAdd"] = lstJfmc;
                }
            }
            catch (Exception ex) { new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString())); }
            return Json(lstJfmc, JsonRequestBehavior.AllowGet);
        }

        }


    }

