

namespace FMDSS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using log4net;
    using FMDSS.Models;
    using System.Configuration;
    using System.Web.Routing;
    using System.Data;

    /// <summary>
    /// Class BaseController
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        cls_UserLogs oLogs = new cls_UserLogs();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //#region Check Double User Not login at a time same sso id by Rajveer
            //UserProfile usr = new UserProfile();
            //DataTable UserProfileSessionMaintainDataset = new DataTable();
            //UserProfileSessionMaintainDataset = usr.UserProfileSessionMaintain("SELECT", Convert.ToString(Session["SSOID"]), Convert.ToString(Session["Create16DigitSession"]), string.Empty);//Add by Rajveer

            //if (UserProfileSessionMaintainDataset != null && UserProfileSessionMaintainDataset.Rows.Count > 0)
            //{

            //}
            //else
            //{
            //    Session["UserId"] = null;
            //}
            //#endregion


            if (Session["UserId"] == null)
            {
                filterContext.Result = new EmptyResult();
                Response.Redirect(ConfigurationManager.AppSettings["RedirectTologin"].ToString());

            }
            else
            {

                if (filterContext.HttpContext.Request.HttpMethod == "GET")
                {

                    if (Session["CURRENT_ROLE"] != null)
                    {
                        List<Menus> OBJListMenus = new List<Menus>();
                        OBJListMenus = (List<Menus>)Session["Menus"];

                        var CurrentMenus = OBJListMenus.Where(item => item.RoleId == Convert.ToInt16(Session["CURRENT_ROLE"])).ToList();
                        bool FoundStatus = false;

                        var Page = CurrentMenus.Where(i => i.PageURL == "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper() + "/" + filterContext.ActionDescriptor.ActionName.ToUpper());

                        foreach (var CurrentRow in CurrentMenus)
                        {
                            if (CurrentRow.PageURL.ToUpper() == "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper() + "/" + filterContext.ActionDescriptor.ActionName.ToUpper())
                            {
                                FoundStatus = true;
                                break;
                            }
                        }



                        if (FoundStatus == false)
                        {

                            BaseModel OBJ = new BaseModel();
                            if (OBJ.GetCurrentURLStatus("/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName))
                            {
                                FoundStatus = false;
                            }
                            else
                            {
                                FoundStatus = true;
                            }

                            if (FoundStatus == false)
                            {
                                Session.Abandon();
                                Session.Clear();
                                filterContext.Result = new EmptyResult();
                                Response.Redirect(ConfigurationManager.AppSettings["UnauthorizedAccessURL"].ToString());
                            }
                        }

                    }
                    else
                    {

                        Session.Abandon();
                        Session.Clear();
                        filterContext.Result = new EmptyResult();
                        Response.Redirect(ConfigurationManager.AppSettings["UnauthorizedAccessURL"].ToString());

                    }
                }

                string IPAddress = this.Request.UserHostAddress;// GetIPAddress(filterContext.HttpContext.Request);
                base.OnActionExecuting(filterContext);
                string actionParam = string.Empty;
                foreach (var param in filterContext.ActionParameters)
                {
                    actionParam += param.Key + "=" + Encryption.decrypt(Convert.ToString(param.Value));
                }

                ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.Info(string.Format("Start: Action={0} Controller={1} Param={2} IP Address={3}", filterContext.ActionDescriptor.ActionName, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, IPAddress, string.Join(",", actionParam)));

                /////Insert logs in Database


                oLogs.SaveUserLogs(filterContext.ActionDescriptor.ActionName, DateTime.Now, "Start", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName + "?" + string.Join(",", actionParam), "", Convert.ToString(Session["SSOID"]), IPAddress);


                HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                HttpContext.Response.Cache.SetValidUntilExpires(false);
                HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Response.Cache.SetNoStore();

            }
        }

        public static string GetIPAddress(HttpRequestBase request)
        {
            string ip;
            try
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    if (ip.IndexOf(",") > 0)
                    {
                        string[] ipRange = ip.Split(',');
                        int le = ipRange.Length - 1;
                        ip = ipRange[le];
                    }
                }
                else
                {
                    ip = request.UserHostAddress;
                }
            }
            catch { ip = null; }

            return ip;
        }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(string.Format("End: Action={0} Controller={1}", filterContext.ActionDescriptor.ActionName, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName));
            ///Database Log
            //oLogs.SaveUserLogs(filterContext.ActionDescriptor.ActionName, DateTime.Now, "End", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, "", Convert.ToString(Session["SSOID"]));
        }

        /// <summary>
        /// Renders the partial view to string.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns>returns the view as html string.</returns>
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }

    public class BaseOnlinebookingRanthmboreController : Controller
    {
        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        cls_UserLogs oLogs = new cls_UserLogs();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            #region Check Double User Not login at a time same sso id by Rajveer
            UserProfile usr = new UserProfile();
            int flag = 0;
            DataTable UserProfileSessionMaintainDataset = new DataTable();
            string IP_Address = string.Empty;
            base.OnActionExecuting(filterContext);
            if (filterContext.RequestContext.HttpContext.Request.UrlReferrer!=null && filterContext.RequestContext.HttpContext.Request.UrlReferrer.Host == "localhost")
            {
                IP_Address = filterContext.RequestContext.HttpContext.Request.UrlReferrer.Host;
                // Do what you want for localhost
            }
            else
            {
                IP_Address = filterContext.RequestContext.HttpContext.Request.UserHostAddress;
            }
            UserProfileSessionMaintainDataset = usr.UserProfileSessionMaintain("SELECT", Convert.ToString(Session["SSOID"]), Convert.ToString(Session["Create16DigitSession"]), IP_Address);//Add by Rajveer

            if (UserProfileSessionMaintainDataset != null && UserProfileSessionMaintainDataset.Rows.Count > 0)
            {
                ///change by amit on 26-11-2019
               // Session.Abandon();
               // Session.Clear();
               // flag = 0;
                ///End change by amit on 26-11-2019
            }
            else
            {
                Session["UserId"] = null;
                flag = 1;
                // filterContext.Result = new EmptyResult();
                Response.Redirect(ConfigurationManager.AppSettings["RedirectToErrorPage"].ToString());
            }
            #endregion


            if (Session["UserId"] == null && flag == 0)
            {
                filterContext.Result = new EmptyResult();
                Response.Redirect(ConfigurationManager.AppSettings["RedirectTologin"].ToString());

            }
            else
            {

                if (filterContext.HttpContext.Request.HttpMethod == "GET")
                {

                    if (Session["CURRENT_ROLE"] != null)
                    {
                        List<Menus> OBJListMenus = new List<Menus>();
                        OBJListMenus = (List<Menus>)Session["Menus"];

                        var CurrentMenus = OBJListMenus.Where(item => item.RoleId == Convert.ToInt16(Session["CURRENT_ROLE"])).ToList();
                        bool FoundStatus = false;

                        var Page = CurrentMenus.Where(i => i.PageURL == "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper() + "/" + filterContext.ActionDescriptor.ActionName.ToUpper());

                        foreach (var CurrentRow in CurrentMenus)
                        {
                            if (CurrentRow.PageURL.ToUpper() == "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper() + "/" + filterContext.ActionDescriptor.ActionName.ToUpper())
                            {
                                FoundStatus = true;
                                break;
                            }
                        }



                        if (FoundStatus == false)
                        {

                            BaseModel OBJ = new BaseModel();
                            if (OBJ.GetCurrentURLStatus("/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName))
                            {
                                FoundStatus = false;
                            }
                            else
                            {
                                FoundStatus = true;
                            }

                            if (FoundStatus == false)
                            {
                                Session.Abandon();
                                Session.Clear();
                                filterContext.Result = new EmptyResult();
                                Response.Redirect(ConfigurationManager.AppSettings["UnauthorizedAccessURL"].ToString());
                            }
                        }

                    }
                    else
                    {

                        Session.Abandon();
                        Session.Clear();
                        filterContext.Result = new EmptyResult();
                        Response.Redirect(ConfigurationManager.AppSettings["UnauthorizedAccessURL"].ToString());

                    }
                }

                string IPAddress = this.Request.UserHostAddress;// GetIPAddress(filterContext.HttpContext.Request);
                base.OnActionExecuting(filterContext);
                string actionParam = string.Empty;
                foreach (var param in filterContext.ActionParameters)
                {
                    actionParam += param.Key + "=" + Encryption.decrypt(Convert.ToString(param.Value));
                }

                ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.Info(string.Format("Start: Action={0} Controller={1} Param={2} IP Address={3}", filterContext.ActionDescriptor.ActionName, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, IPAddress, string.Join(",", actionParam)));

                /////Insert logs in Database


                oLogs.SaveUserLogs(filterContext.ActionDescriptor.ActionName, DateTime.Now, "Start", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName + "?" + string.Join(",", actionParam), "", Convert.ToString(Session["SSOID"]), IPAddress);


                HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                HttpContext.Response.Cache.SetValidUntilExpires(false);
                HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Response.Cache.SetNoStore();

            }
        }

        public static string GetIPAddress(HttpRequestBase request)
        {
            string ip;
            try
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    if (ip.IndexOf(",") > 0)
                    {
                        string[] ipRange = ip.Split(',');
                        int le = ipRange.Length - 1;
                        ip = ipRange[le];
                    }
                }
                else
                {
                    ip = request.UserHostAddress;
                }
            }
            catch { ip = null; }

            return ip;
        }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(string.Format("End: Action={0} Controller={1}", filterContext.ActionDescriptor.ActionName, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName));
            ///Database Log
            //oLogs.SaveUserLogs(filterContext.ActionDescriptor.ActionName, DateTime.Now, "End", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, "", Convert.ToString(Session["SSOID"]));
        }

        /// <summary>
        /// Renders the partial view to string.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns>returns the view as html string.</returns>
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
