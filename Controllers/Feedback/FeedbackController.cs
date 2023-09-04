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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Filters;
using System.Data;

namespace FMDSS.Controllers.Feedback
{
      [MyAuthorization]
    public class FeedbackController : BaseController
    {
        //
        // GET: /Feedback/
        SMS_EMail_Services _objMail = new SMS_EMail_Services();
        FeedBack _objFeed = new FeedBack();
        UserProfile user = null;
        public ActionResult Helpfacilitationguidance()
        {

            if (Session["SSODetail"] != null)
            {
                user = (UserProfile)Session["SSODetail"];
              
            }

            return View(user);
        }

        public ActionResult AdminHelpfacilitationguidance()
        {
            List<FeedBack> result = new List<FeedBack>();
            try
            {
                  #region Bind Query Detail

                FeedBack feedBack = null;
                DataTable feeddt = _objFeed.BindSubmitedQuery("VIEW");
                if (feeddt != null)
                    {
                        if (feeddt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in feeddt.Rows)
                            {

                                feedBack = new FeedBack()
                                {

                                    FeedBackId = Convert.ToInt64(dr["ID"].ToString()),
                                    Name = dr["Name"].ToString(),
                                    Email = dr["Email"].ToString(),
                                    Phone = dr["Phone"].ToString(),
                                    Query = dr["Query"].ToString(),
                                    EnteredOn = dr["EnteredOn"].ToString()
                                };
                                result.Add(feedBack);
                            }

                        }
                    }
                  #endregion

            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }

            return View(result);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetQqueryById(string feedBackId)
        {
            FeedBack objFeedback = new FeedBack();
            FeedBack feedBack = null;
            string Query = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(feedBackId))
                {

                    DataTable dt = objFeedback.FetchQueryById(Convert.ToInt64(feedBackId));
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                             feedBack = new FeedBack();
                             feedBack.FeedBackId = Convert.ToInt64(dr["ID"].ToString());
                             feedBack.Query = dr["Query"].ToString();
                        }
                    }
                    else
                    {
                       
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }

            return Json(feedBack, JsonRequestBehavior.AllowGet);

        }

        public JsonResult PublishedAnswer()
        {
            List<FeedBack> result = new List<FeedBack>();
            FeedBack objFeedback = new FeedBack();
            FeedBack feedBack = null;
            string Query = string.Empty;
            try
            {

                #region Bind Query Detail


                DataTable feeddt = _objFeed.BindSubmitedQuery("Publish");
                if (feeddt != null)
                {
                    if (feeddt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in feeddt.Rows)
                        {

                            feedBack = new FeedBack()
                            {

                                FeedBackId = Convert.ToInt64(dr["ID"].ToString()),
                                Name = dr["Name"].ToString(),
                                Email = dr["Email"].ToString(),
                                Phone = dr["Phone"].ToString(),
                                Query = dr["Query"].ToString(),
                                Answer = dr["Answer"].ToString(),
                                EnteredOn = dr["EnteredOn"].ToString()
                            };
                            result.Add(feedBack);
                        }

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SubmitQueryData(FormCollection fm, string Command)
        {

            try
            {
                if (Command == "Cancel")
                {
                    return RedirectToAction("dashboard", "dashboard");
                }

                FeedBack feedBack = new FeedBack();

                feedBack.FeedBackId= 0;

                if (Session["UserID"] != null)
                {
                    //user = (UserProfile)Session["SSODetail"];
                    feedBack.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (fm["Name"].ToString() == "")
                {
                    feedBack.Name = "";
                }
                else
                {
                    feedBack.Name = fm["Name"].ToString();
                }
                if (fm["Email"].ToString() == "")
                {
                    feedBack.Email = "";
                }
                else
                {
                    feedBack.Email = fm["Email"].ToString();
                }

                if (fm["Phone"].ToString() == "")
                {
                    feedBack.Phone = "";
                }
                else
                {
                    feedBack.Phone = fm["Phone"].ToString();
                }

                if (fm["Query"].ToString() == "")
                {
                    feedBack.Query = "";
                }
                else
                {
                    feedBack.Query = fm["Query"].ToString();
                }

                if (Command == "Submit")
                {
                    Int64 status = feedBack.AddFeedbackData();
                    if (status > 0)
                    {
                        TempData["Status"] = "Your Query has been received. We will contact you shortly.";

                        //if (!String.IsNullOrEmpty(feedBack.Email) && !String.IsNullOrEmpty(feedBack.Phone))
                        //        {
                        //            string message = "Your Query has been received. We will contact you shortly";
                        //            SMS_EMail_Services.sendSingleSMS(feedBack.Phone, message);
                        //            //string CitizenMailBody = Common.GenerateSMSBody(user.FullName, "", "Query");
                        //            //_objMail.sendEMail("Request for " + "Apply for Research" + " Permission", CitizenMailBody, user.EmailId.ToString(), "");

                        //        }
                         
                    }
                    else
                    {
                        TempData["Status"] = "Not inserted";
                    }


                }
                return RedirectToAction("Helpfacilitationguidance", "Feedback");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult UpdateFeedBackForm(FormCollection fm, string Command)
        {

            try
            {
              

                FeedBack feedBack = new FeedBack();
                feedBack.Status = "Publish";

                if (fm["Hd_Id"].ToString() == "")
                {
                    feedBack.FeedBackId = 0;
                }
                else
                {
                    feedBack.FeedBackId = Convert.ToInt64(fm["Hd_Id"].ToString());
                }
               

                if (Session["UserID"] != null)
                {
                    //user = (UserProfile)Session["SSODetail"];
                    feedBack.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (fm["Answer"].ToString() == "")
                {
                    feedBack.Answer = "";
                }
                else
                {
                    feedBack.Answer = fm["Answer"].ToString();
                }

                if (Command == "Answer")
                {
                    Int64 status = feedBack.AddAnswer();
                    if (status > 0)
                    {
                        TempData["Status"] = "Answer for Query has been Saved.";

                        

                    }
                    else
                    {
                        TempData["Status"] = "Not inserted";
                    }


                }
                return RedirectToAction("AdminHelpfacilitationguidance", "Feedback");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
