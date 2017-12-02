using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ratp.Hidalgo.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "accueil",
               url: "{controller}/{action}/{type}",
               defaults: new { controller = "Home", action = "index", type = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "maconnerie",
                url: "{controller}/{action}/{type}",
                defaults: new { controller = "Calibrage", action = "ValidInfoCalibrage", type = 0 }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Calibrage", action = "ValidInfoCalibrage", id = UrlParameter.Optional }
            );
        }
    }
}
