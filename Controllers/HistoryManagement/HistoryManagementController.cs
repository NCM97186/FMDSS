using FMDSS.Models;
using FMDSS.Models.HistoryManangementModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.HistoryManagement
{
    public class HistoryManagementController : Controller
    {
        //
        // GET: /HistoryManagement/

        public ActionResult Index()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            HistoryDetailsModel model = new HistoryDetailsModel();
            HistoryRepo repo = new HistoryRepo();
            try
            {
                DataTable dt = new DataTable();
                dt = repo.SelectHistoryManagement(model.model, "LIST");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HistoryModel>>(str);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(HistoryModel model, HttpPostedFileBase fileUpload)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            HistoryRepo repo = new HistoryRepo();
            try
            {
                string FilePath = "~/HistoryDocuments/";
                if (fileUpload != null && !string.IsNullOrEmpty(fileUpload.FileName))
                {
                    string FileFullName = DateTime.Now.Ticks + "_" + fileUpload.FileName;
                    model.FileUploader = Path.Combine(FilePath, FileFullName);
                    Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));
                }
               

                DataTable dt = new DataTable();
                dt = repo.SelectHistoryManagement(model, "Save");
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["status"]) > 0)
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> आवेदक का फॉर्म सफलतापूर्वक सबमिट हो गया है! <br/>(Applicant form submited successfully) </div>";

                    }
                    else
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            return RedirectToAction("Index");
        }
    }
}
