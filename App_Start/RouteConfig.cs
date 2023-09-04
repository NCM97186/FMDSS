using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FMDSS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            //);


            routes.MapRoute(
                  name: "NPOnlineBooking",
                  url: "onlinebooking/{id}",
                  defaults: new { controller = "NationalPark", action = "Index" },
                  namespaces: new[] { "FMDSS.Controllers" }
              );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional },
               namespaces: new[] { "FMDSS.Controllers" }
           );


            routes.MapRoute(
             name: "Default1",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "ForesterAction", action = "ForesterAction", id = UrlParameter.Optional });
        }
    }
}