using FMDSS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.SMS
{

    public class ReadAllMessagesController : BaseController
    {
        Notification nObj = new Notification();
        //
        // GET: /ReadAllMessages/
        // UserId
        public ActionResult ReadAllMessages()
        {
            Notification notification = null;
            List<Notification> result = new List<Notification>();

            try
            {
                if (Session["UserID"] != null)
                {


                    DataTable dt = nObj.GetAllmessage(Convert.ToInt64(Session["UserID"]));
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                notification = new Notification()
                                {

                                    MessageId = Convert.ToInt64(dr["ID"].ToString()),
                                    //MessageId = Convert.ToInt64(dr["IsRead"].ToString()),
                                    EmailFrom = dr["Email_From"].ToString(),
                                    //EmailTo = dr["Email_To"].ToString(),
                                    Subject = dr["Email_Subject"].ToString(),
                                    EnteredOn = dr["EnteredOn"].ToString()
                                    
                                };
                                result.Add(notification);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }

            return View(result);
        }

     
         [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetNewMessage()
        {
            Notification notification = null;
            var result = new List<Notification>();
          
            try
            {
                if (Session["UserID"] != null)
                {

                    string today = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    DataTable dt = nObj.GetNewmessage(Convert.ToInt64(Session["UserID"]), today);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                notification = new Notification()
                                {

                               // MessageId = Convert.ToInt64(dr["ID"].ToString()),
                                EmailFrom = dr["Email_From"].ToString(),
                                EnteredOn = dr["EnteredOn"].ToString(),
                                Subject = dr["Email_Subject"].ToString(),
                                //Message = dr["Email_Text"].ToString()
                                
                            };
                                result.Add(notification);
                            }
                           
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
       
         [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ViewEmail(string MessageId)
        {
            Notification notification = new Notification();
 
            try
            {
                    DataSet ds1 = nObj.GetEmailDetail(Convert.ToInt32(MessageId));
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    notification.EmailFrom = ds1.Tables[0].Rows[0]["Email_From"].ToString();
                    notification.EmailTo = ds1.Tables[0].Rows[0]["Email_To"].ToString();
                    notification.Subject = ds1.Tables[0].Rows[0]["Subject"].ToString();
                    notification.Message = ds1.Tables[0].Rows[0]["Email_Text"].ToString();
                    notification.EnteredOn = ds1.Tables[0].Rows[0]["EnteredOn"].ToString();
                }
            }
         
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }
            return Json(notification, JsonRequestBehavior.AllowGet);
        }
    }
}
