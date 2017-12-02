using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Ratp.Hidalgo.App.Tests.App_Start
{
    /// <summary>
    /// Classe permettant d'initialiser la configuration unity pour le projet de test.
    /// </summary>
    [TestClass]
    public static class AssemblyInitialize
    {
        /// <summary>
        /// Méthode d'initialisation de la configuration unity.
        /// </summary>
        /// <param name="context">Contexte du test.</param>
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            UtilityTest.ChargementUnity();
        }

        /// <summary>
        /// Méthode d'initialisation de la configuration unity.
        /// </summary>
        [AssemblyCleanup]
        public static void Clean()
        {
        }
    }
}
