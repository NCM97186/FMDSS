//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Apply Auction
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@


using FMDSS.Models;
using FMDSS.Models.CitizenService.ProductionServices.EducationService;
using FMDSS.Models.ForesterAction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using FMDSS.Filters;
using System.IO;

namespace FMDSS.Controllers.CitizenService.PermissionService.EducationService
{

    [MyAuthorization]
    public class ApplicantCordinatorController : BaseController
    {
        //
        // GET: /ApplicantCordinator/
        Research research = new Research();
        SMS_EMail_Services _objMail = new SMS_EMail_Services();
        public ActionResult AddApplicantCordinator()
        {
            return View();
        }

        /// <summary>
        /// Read for xml of Past Research Activity
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getResearchPastactivity()
        {
            var result = new List<ResearchDetail>();
            ResearchDetail researchobj = null;
            try
            {

                if (Session["filename"] != null)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {


                            researchobj = new ResearchDetail()
                            {
                                Id = Int64.Parse(dr["ID"].ToString()),
                                ResearchId = dr["ResearchId"].ToString(),
                                Activity_TakenBy = dr["Activity_TakenBy"].ToString(),
                                Subjectforresearch = dr["Research_Sub"].ToString(),
                                Durationfrom = dr["Research_From"].ToString(),
                                Durationto = dr["Research_To"].ToString(),
                                ResearchLocation = dr["Research_Location"].ToString(),
                                ResearchBenefits = dr["Research_Benefit_Forest"].ToString(),
                                ResearchPurpose = dr["Research_Purpose"].ToString()


                            };
                            result.Add(researchobj);


                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Used for get Research detail
        /// </summary>
        /// <param name="researchid"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetResearchInfo(string researchid)
        {
            ResearchDetail researchdet = null;

            try
            {

                if (!String.IsNullOrEmpty(researchid))
                {

                    DataTable dt = research.GetResearchDetail(researchid);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                researchdet = new ResearchDetail();
                                researchdet.Subjectforresearch = dr["R_Subject"].ToString();
                                researchdet.ResearchPurpose = dr["R_Procedure"].ToString();
                                researchdet.Durationfrom = dr["DurationFrom"].ToString();
                                researchdet.Durationto = dr["DurationTo"].ToString();
                                researchdet.ResearchLocation = dr["PlaceName"].ToString();
                                researchdet.ResearchWildlife = dr["Benefits"].ToString();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }
            return Json(researchdet, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// User for add past research activities of applicant or researcher
        /// </summary>
        /// <param name="researchDetail"></param>
        /// <returns>return json result</returns>
        [HttpPost]
        public JsonResult GetActivity(ResearchDetail researchDetail)
        {

            if (researchDetail != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    string filename = DateTime.Now.Ticks.ToString();

                    if (Session["rname"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("Research");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["rname"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["rname"].ToString() + ".xml"));
                    }
                    researchDetail.Id = researchDetail.Id + 1;

                    XmlElement Research_TYPE = xmldoc.CreateElement("Research_TYPE");
                    XmlElement ID = xmldoc.CreateElement("ID");
                    XmlElement Activity_TakenBy = xmldoc.CreateElement("Activity_TakenBy");
                    XmlElement ResearchId = xmldoc.CreateElement("ResearchId");
                    XmlElement Research_Sub = xmldoc.CreateElement("Research_Sub");
                    XmlElement Research_From = xmldoc.CreateElement("Research_From");
                    XmlElement Research_To = xmldoc.CreateElement("Research_To");
                    XmlElement Research_Location = xmldoc.CreateElement("Research_Location");
                    XmlElement Research_Purpose = xmldoc.CreateElement("Research_Purpose");
                    XmlElement Research_Benefit_Forest = xmldoc.CreateElement("Research_Benefit_Forest");

                    ID.InnerText = researchDetail.Id.ToString();
                    Activity_TakenBy.InnerText = researchDetail.Activity_TakenBy;
                    ResearchId.InnerText = researchDetail.ResearchId.ToString();
                    Research_Sub.InnerText = researchDetail.Subjectforresearch;
                    Research_From.InnerText = researchDetail.Durationfrom;
                    Research_To.InnerText = researchDetail.Durationto;
                    Research_Location.InnerText = researchDetail.ResearchLocation;
                    Research_Purpose.InnerText = researchDetail.ResearchPurpose;
                    Research_Benefit_Forest.InnerText = researchDetail.ResearchWildlife;

                    Research_TYPE.AppendChild(ID);
                    Research_TYPE.AppendChild(Activity_TakenBy);
                    Research_TYPE.AppendChild(ResearchId);
                    Research_TYPE.AppendChild(Research_Sub);
                    Research_TYPE.AppendChild(Research_From);
                    Research_TYPE.AppendChild(Research_To);
                    Research_TYPE.AppendChild(Research_Location);
                    Research_TYPE.AppendChild(Research_Purpose);
                    Research_TYPE.AppendChild(Research_Benefit_Forest);

                    xmldoc.DocumentElement.AppendChild(Research_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["rname"].ToString() + ".xml"));

                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }

            }

            return Json(researchDetail, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  This  ActionResult Method will get all submit form data, call the model method  for insert data in database and finally return on view
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>

        //[HttpPost]
        //public ActionResult addfinalResearch(string Command, HttpPostedFileBase ProjectDescription)
        //{
        //    //ActionRequest ar = new ActionRequest();
        //    Research researchobj = (Research)Session["Research"];
        //    string status = "";
        //    try
        //    {
        //        if (Command == "Cancel")
        //        {
        //            return RedirectToAction("dashboard", "dashboard");
        //        }

        //        if (Command == "Preview")
        //        {
        //            return RedirectToAction("ResearchPreview", "ApplicantCordinator");
        //        }

        //        if (Command == "Save")
        //        {
        //            if (ProjectDescription != null && ProjectDescription.ContentLength > 0)
        //            {
        //                researchobj.ProjectDescription = Path.GetFileName(ProjectDescription.FileName);
        //                String FileFullName = DateTime.Now.Ticks + "_" + researchobj.ProjectDescription;
        //                researchobj.ProjectDescription = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
        //                researchobj.ProjectDescriptionName = FileFullName;
        //                researchobj.ProjectDescription = @"~/PermissionDocument/" + FileFullName.Trim();
        //                ProjectDescription.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
        //            }
        //            else
        //            {
        //                researchobj.ProjectDescription = "";
        //                researchobj.ProjectDescriptionName = "";
        //            }

        //            DataSet ds = new DataSet();
        //            if (Session["rname"] != null)
        //            {
        //                ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["rname"].ToString() + ".xml"));

        //                status = research.addResearch(researchobj);  
        //            }
        //            else
        //            {
        //                status = research.addResearch(researchobj); 
        //            }

        //            if (Session["rname"] != null)
        //            {
        //                if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["rname"].ToString() + ".xml")) == true)
        //                {
        //                    System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["rname"].ToString() + ".xml"));
        //                    Session["rname"] = null;
        //                }
        //            }
        //        }
        //        if (status != "")
        //        {
        //            Session["ResearchStatuss"] = status;



        //            if (Session["SSODetail"] != null)
        //            {
        //                UserProfile user = (UserProfile)Session["SSODetail"];

        //                if (user != null)
        //                {
        //                    if (!String.IsNullOrEmpty(user.EmailId) && !String.IsNullOrEmpty(user.MobileNumber))
        //                    {
        //                        SendSMSEmailForSuccessTransaction("GETUSERDETAILSFORSENDSMSANDEMAILforResearchStudy", researchobj.RequestedId);

        //                    }
        //                }

        //            }

        //            DataSet ds1 = research.FindReviewer();

        //            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
        //            {
        //                string permission = ds1.Tables[0].Rows[0]["SubPermissionDesc"].ToString();
        //                string userName = ds1.Tables[0].Rows[0]["Name"].ToString();
        //                string emailId = ds1.Tables[0].Rows[0]["EmailId"].ToString();
        //                string mobileNo = ds1.Tables[0].Rows[0]["Mobile"].ToString();

        //                string reqMessage = "A Citizen raised a permission request for Research Permission by the Request ID " + researchobj.RequestedId;



        //            }
        //            if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
        //            {
        //                researchobj.KioskUserId = Convert.ToString(Session["KioskUserId"]);
        //                KioskPaymentDetails _obj = new KioskPaymentDetails();
        //                _obj.ModuleId = 1;
        //                _obj.ServiceTypeId = 1;
        //                _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
        //                _obj.SubPermissionId = 1;
        //                _obj.RequestedId = researchobj.RequestedId;
        //                DataTable dtKiosk = _obj.FetchKisokValue(_obj);
        //                if (dtKiosk.Rows.Count > 0)
        //                {

        //                    _obj.RequestedId = researchobj.RequestedId;
        //                    _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
        //                    _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
        //                    _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
        //                    _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
        //                    _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
        //                    _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
        //                    return PartialView("KioskPaymentDetail", _obj);
        //                }
        //            }
        //            else
        //            {
        //                researchobj.KioskUserId = "0";
        //            }
        //        }
        //        else
        //        {
        //            TempData["Statuss"] = "Not inserted";
        //        }
        //    }



        //    catch (Exception ex)
        //    {
        //        Console.Write("Error" + ex);
        //    }

        //    return RedirectToAction("dashboard", "dashboard", new { messagetype = "4" });

        //}
        public void SendSMSEmailForSuccessTransaction(string ACTION, string RequestId)
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable DT = objSMSandEMAILtemplate.GetUserDetails(RequestId, ACTION);
            if (DT.Rows.Count > 0)
            {
                if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                {
                    //body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketEmailTemplate"].ToString()));

                    //objSMS_EMail_Services.sendEMail("Zoo Ticket Booking for RequestID : " + Convert.ToString(DT.Rows[0]["RequestID"]), body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["ZooTicketEmail_CC"].ToString());

                    body = string.Empty;

                    #region SMS Email
                    string UserMailBody = Common.Citizen_RequestApprovalEmailBody(DT.Rows[0]["Name"].ToString(), RequestId, DT.Rows[0]["ResearchType"].ToString());
                    string subject = "Regarding " + DT.Rows[0]["ResearchType"];
                    objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["EmailId"].ToString(), string.Empty);
                    #endregion

                }

                if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
                {
                    //body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketSMSTemplate"].ToString()));

                    //SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["Mobile"]), body);

                    //body = string.Empty;

                    string UserSmsBody = Common.Citizen_RequestApproval_SMSBody(DT.Rows[0]["Name"].ToString(), RequestId, DT.Rows[0]["ResearchType"].ToString());
                    SMS_EMail_Services.sendSingleSMS(DT.Rows[0]["Mobile"].ToString(), UserSmsBody);
                }

            }


            #endregion




        }
    }
}
