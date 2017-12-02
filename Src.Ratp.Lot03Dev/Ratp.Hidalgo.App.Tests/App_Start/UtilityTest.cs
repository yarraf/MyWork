using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Ratp.Hidalgo.App.Tests.App_Start
{
    public static class UtilityTest
    {
        /// <summary>
        /// Permet de charger la conf unity
        /// </summary>
        public static void ChargementUnity()
        {
            var unityConfigurationSection = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            IUnityContainer container = new UnityContainer();
            foreach (ContainerElement containerElement in unityConfigurationSection.Containers)
            {
                container.LoadConfiguration(containerElement.Name);
            }

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }

        /// <summary>
        ///  Créer une instance de service à tester
        /// </summary>
        /// <typeparam name="T">type de l'objet a instancier</typeparam>
        /// <returns>un objet de type</returns>
        public static T CreateInstanceService<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }
    }
}
