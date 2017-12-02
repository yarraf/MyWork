using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Ratp.Hidalgo.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "withAtt",
                routeTemplate: "RatpHidalgoWeb/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "withAt2t",
                routeTemplate: "Ratp.Hidalgo.Web/api/{controller}/{type}",
                defaults: new { type = 0 }
            );


            config.Routes.MapHttpRoute(
             name: "DefaultApi",
             routeTemplate: "api/{controller}/{id}",
             defaults: new { id = RouteParameter.Optional }
         );

            var jsonformatter = config.Formatters.JsonFormatter;
            jsonformatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            var xmlformatter = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            config.Formatters.Remove(xmlformatter);
        }
    }
}
