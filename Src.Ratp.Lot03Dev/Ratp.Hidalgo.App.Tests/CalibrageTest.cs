using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratp.Hidalgo.App.Contract;
using Ratp.Hidalgo.App.Tests.App_Start;
using System.Linq;


namespace Ratp.Hidalgo.App.Tests
{
    /// <summary>
    /// Classe de test du module calibrage
    /// </summary>
    [TestClass]
    public class CalibrageTest
    {
        /// <summary>
        /// Léthode de test pour la méthode GetAllCoeffcientPonderations de la classe <see cref="CalibrageApp"/> 
        /// </summary>
        [TestMethod]
        public void GetAllCoefficientPonderationTest()
        {
            ICalibrageApp serviceCalibrageApp = UtilityTest.CreateInstanceService<ICalibrageApp>();
            var result = serviceCalibrageApp.GetAllCoeffcientPonderations();

            Assert.IsTrue(result != null);
            Assert.AreEqual(result.Count(), 40);
        }
    }
}
