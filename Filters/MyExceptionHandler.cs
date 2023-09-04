using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FMDSS.Filters
{
    public class MyExceptionHandler:ActionFilterAttribute,IExceptionFilter
    {
       
        public void OnException(ExceptionContext filterContext) {
            
            Exception e = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            new FMDSS.Models.Common().ErrorLog(e.Message, "", 0, DateTime.Now, 0);
            HttpContext.Current.Session["Exception"] = e.Message; 
            filterContext.Result = new ViewResult()
            {
                ViewName = "SomeException" 
            }; 
            
        } 
    }
}