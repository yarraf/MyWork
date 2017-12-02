using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratp.Hidalgo.App.Contract;
using Ratp.Hidalgo.App.Tests.App_Start;
using Ratp.Hidalgo.Data.Contract.Enum;
using Ratp.Hidalgo.App.Contract.DTO;

namespace Ratp.Hidalgo.App.Tests
{
    /// <summary>
    /// Classe de tests pour Hidalgo module
    /// </summary>
    [TestClass]
    public class HidalgoServiceTest
    {
        /// <summary>
        /// méthode de tests permettant de récupérer la liste des lignes.
        /// </summary>
        [TestMethod]
        public void GetAllLignes()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            var resultOfService = hidalgoService.GetAllLignes();

            //Assert
            Assert.AreEqual(resultOfService.Count(), 64);
        }

        /// <summary>
        /// méthode de tests permettant de récupérer la liste des lignes.
        /// </summary>
        [TestMethod]
        public void GetAllLignesAsync()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            var resultOfService = hidalgoService.GetAllLignesAsync();

            //Assert
            Assert.AreEqual(resultOfService.Result.Count(), 62);
        }

        /// <summary>
        /// méthode de tests permettant de récupérer la liste des documents.
        /// </summary>
        [TestMethod]
        public void GetNoteMaxAllDocumentPge()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            //var resultOfService = hidalgoService.GetAllDocumentPge();

            //Assert
            //Assert.AreEqual(resultOfService.Count(), 404);
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.GetNotesMaxForAllDocumentPge_Fissuration
        /// </summary>
        [TestMethod]
        public void GetNoteMaxPge_FissurationMaconnerieTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            var resultOfService = hidalgoService.GetNotesMaxForAllDocumentPge_Fissuration(EFamilleDesordre.Fissurations, ENatureCalibrage.Maçonnerie);

            //Assert
            Assert.AreEqual(resultOfService.Count(), 223);
        }


        /// <summary>
        /// Test de la méthode HidalgoApp.GetNotesMaxForAllDocumentPge_Fissuration
        /// </summary>
        [TestMethod]
        public void GetNoteMaxPge_FissurationEnduitTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            var resultOfService = hidalgoService.GetNotesMaxForAllDocumentPge_Fissuration(EFamilleDesordre.Fissurations, ENatureCalibrage.Enduit);

            //Assert
            Assert.AreEqual(resultOfService.Count(), 109);
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.GetNotesMaxForAllDocumentPge_Infultration 
        /// </summary>
        [TestMethod]
        public void GetNoteMaxPge_InfultrationTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            //Act
            var resultOfService = hidalgoService.GetNotesMaxForAllDocumentPge_Infultration(EFamilleDesordre.Infiltrations, ENatureCalibrage.Maçonnerie);
            //Assert
            Assert.AreEqual(resultOfService.Count(), 223);
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.GetNotesMaxForAllDocumentPge_DesordreStructureMaconnerieBeton
        /// </summary>
        [TestMethod]
        public void GetNoteMaxPge_DesordreStructureMaconnerieBetonTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            //Act
            var resultOfService = hidalgoService.GetNotesMaxForAllDocumentPge_DesordreStructureMaconnerieBeton(EFamilleDesordre.Desordre_structure_Maçonnerie_Beton, ENatureCalibrage.Maçonnerie);
            //Assert
            Assert.AreEqual(resultOfService.Count(), 223);
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.GetNoteMaxForAllDocumentPge_AgeOuvrage
        /// </summary>
        //[TestMethod]
        //public void GetNoteMaxPge_AgeOuvrageTest()
        //{
        //    //Arrang
        //    var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
        //    var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

        //    //Act
        //    var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_AgeOuvrage(2017, ENatureCalibrage.Maçonnerie, listeDocumentPge);

        //    //Assert
        //    Assert.IsTrue(resultOfService != null);
        //}

        /// <summary>
        /// Test de la méthode HidalgoApp.GetNoteMaxForAllDocumentPge_LargeurOuvrage
        /// </summary>
        //[TestMethod]
        //public void GetNoteMaxForAllDocumentPge_LargeurOuvrageTest()
        //{
        //    //Arrang
        //    var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
        //    var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

        //    //Act
        //    var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_LargeurOuvrage(ENatureCalibrage.Maçonnerie, listeDocumentPge);

        //    //Assert
        //    Assert.IsTrue(resultOfService != null);
        //}

        ///// <summary>
        ///// Test de la méthode HidalgoApp.GetNoteMaxForAllDocumentPge_AgressiviteChimiqueTerrainEncaissant
        ///// </summary>
        //[TestMethod]
        //public void GetNoteMaxForAllDocumentPge_AgessivitechimiqueTerrainEncaissantTest()
        //{
        //    //Arrang
        //    var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
        //    var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

        //    //Act
        //    var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_AgressiviteChimiqueTerrainEncaissant(ENatureCalibrage.Maçonnerie, listeDocumentPge);

        //    //Assert
        //    Assert.IsTrue(resultOfService != null);
        //}


        /// <summary>
        /// Test de la méthode HidalgoApp.GetNoteMaxForAllDocumentPge_SolubiliteTerrain()
        /// </summary>
        //[TestMethod]
        //public void GetNoteMaxForAllDocumentPge_GetNoteMaxForAllDocumentPge_SolubiliteTerrainTest()
        //{
        //    //Arrang
        //    var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
        //    var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

        //    //Act
        //    var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_SolubiliteTerrain(ENatureCalibrage.Maçonnerie, listeDocumentPge);

        //    //Assert
        //    Assert.IsTrue(resultOfService != null);
        //}

        /// <summary>
        /// Test de la méthode HidalgoApp.GetNoteMaxForAllDocumentPge_SolubiliteTerrain()
        /// </summary>
        //[TestMethod]
        //public void GetNoteMaxForAllDocumentPge_GetNoteMaxForAllDocumentPge_PourrissementBoisBlindageTest()
        //{
        //    //Arrang
        //    var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
        //    var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

        //    //Act
        //    var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_PourrissementBoisBlindage(ENatureCalibrage.Maçonnerie, listeDocumentPge);

        //    //Assert
        //    Assert.IsTrue(resultOfService != null);
        //}

        /// <summary>
        /// Test de la méthode HidalgoApp.CalculCritereC1()
        /// </summary>
        //[TestMethod]
        //public void CalculCritereC1Test()
        //{
        //    //Arrang
        //    var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
        //    var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

        //    //Act
        //    var resultOfService = hidalgoService.CalculCritereC1(this.GetOneProgrammationToTest(), listeDocumentPge);

        //    //Assert
        //    Assert.IsTrue(resultOfService != null);
        //}

        /// <summary>
        /// Méthode permettant d'initialiser un objet de type <see cref="ProhrammationDto"/>
        /// </summary>
        /// <returns>Objet initialisé pour le test</returns>
        private ProgrammationDto GetOneProgrammationToTest()
        {
            return new ProgrammationDto
            {
                NatureTravaux = ENatureCalibrage.Maçonnerie,
            };
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.GetAllTypeOuvrages()
        /// </summary>
        [TestMethod]
        public void GetAllTypeOuvrageTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            var resultOfService = hidalgoService.GetAllTypeOuvrages();

            //Assert
            Assert.IsTrue(resultOfService != null);
            Assert.AreEqual(resultOfService.Count(), 68);
        }

        [TestMethod]
        public void GetAllDocumentPgeMaconnerieTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            var resultOfService = hidalgoService.GetAllDocumentPgeByNatureTravaux(ENatureCalibrage.Maçonnerie);

            //Assert
            Assert.IsTrue(resultOfService != null);
            Assert.AreEqual(resultOfService.Count(), 79);
        }

        [TestMethod]
        public void GetAllDocumentPgeEnduitTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            var resultOfService = hidalgoService.GetAllDocumentPgeByNatureTravaux(ENatureCalibrage.Enduit);

            //Assert
            Assert.IsTrue(resultOfService != null);
            Assert.AreEqual(resultOfService.Count(), 37);
        }
    }
}
