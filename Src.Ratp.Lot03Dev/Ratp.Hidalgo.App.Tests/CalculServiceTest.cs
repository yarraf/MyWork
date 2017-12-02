using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratp.Hidalgo.App.Tests.App_Start;
using Ratp.Hidalgo.App.Contract;
using System.Linq;

namespace Ratp.Hidalgo.App.Tests
{
    [TestClass]
    public class CalculServiceTest
    {
        [TestMethod]
        /// <summary>
        /// Méthode de test
        /// </summary>
        public void GetAllDocumentPgeForNatureTravauxMaconnerieTest()
        {
            //Arrang
            var calculService = UtilityTest.CreateInstanceService<ICalculServiceApp>();

            var listeDocumentPge = calculService.GetAllDocumentPge(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            Assert.IsTrue(listeDocumentPge != null);
            Assert.AreEqual(listeDocumentPge.Count(), 223);
        }

        [TestMethod]
        /// <summary>
        /// Méthode de test
        /// </summary>
        public void GetAllDocumentPgeForNatureTravauxEnduitTest()
        {
            //Arrang
            var calculService = UtilityTest.CreateInstanceService<ICalculServiceApp>();

            var listeDocumentPge = calculService.GetAllDocumentPge(Data.Contract.Enum.ENatureCalibrage.Enduit);

            Assert.IsTrue(listeDocumentPge != null);
            Assert.AreEqual(listeDocumentPge.Count(), 109);
        }
    }
}
