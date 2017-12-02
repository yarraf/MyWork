using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;

namespace Ratp.Hidalgo.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            var unityConfigurationSection = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container
                                                                                 .LoadConfiguration(unityConfigurationSection.Containers[0].Name)
                                                                                 .LoadConfiguration(unityConfigurationSection.Containers[1].Name)
                                                                                 ));

            ServiceLocator.SetLocatorProvider(() => new Microsoft.Practices.Unity.UnityServiceLocator(container));
        }
    }
}