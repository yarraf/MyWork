using Ratp.Hidalgo.Web.Infra;
using System.Web;
using System.Web.Mvc;

namespace Ratp.Hidalgo.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new UserFilter());
        }
    }
}
