using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ratp.Hidalgo.Web.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace Ratp.Hidalgo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
