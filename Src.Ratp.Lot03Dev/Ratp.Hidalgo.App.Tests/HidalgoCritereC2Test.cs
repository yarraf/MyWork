using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratp.Hidalgo.App.Contract;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.App.Tests.App_Start;
using Ratp.Hidalgo.Data.Contract.Enum;

namespace Ratp.Hidalgo.App.Tests
{
    /// <summary>
    /// Classe de test des méthodes pour calculer critere C2
    /// </summary>
    [TestClass]
    public class HidalgoCritereC2Test
    {
        /// <summary>
        /// Méthode de test C2: Fréquentations
        /// </summary>
        [TestMethod]
        public void GetNoteMaxForAllDocumentPge_FrequentationTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            //Act
            var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_Frequentation(ENatureCalibrage.Maçonnerie, listeDocumentPge);

            //Assert
            Assert.IsTrue(resultOfService != null);
        }

        /// <summary>
        /// Méthode de test C2: Correspondance
        /// </summary>
        [TestMethod]
        public void GetNoteMaxForAllDocumentPge_CorrespondanceTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            //Act
            var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_Correspondance(ENatureCalibrage.Maçonnerie, listeDocumentPge);

            //Assert
            Assert.IsTrue(resultOfService != null);
        }

        /// <summary>
        /// Méthode de test C2: Périmetre L2 L6
        /// </summary>
        [TestMethod]
        public void GetNoteMaxForAllDocumentPge_PerimetreTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            //Act
            var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_PerimetreL2L6(ENatureCalibrage.Maçonnerie, listeDocumentPge);

            //Assert
            Assert.IsTrue(resultOfService != null);
        }

        /// <summary>
        /// Méthode de test C2: Image RATP
        /// </summary>
        [TestMethod]
        public void GetNoteMaxForAllDocumentPge_IgameRatpTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            //Act
            var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_ImageRatp(ENatureCalibrage.Maçonnerie, listeDocumentPge);

            //Assert
            Assert.IsTrue(resultOfService != null);
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.CalculCritereC2()
        /// </summary>
        [TestMethod]
        public void CalculCritereC2Test()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            //Act
            var resultOfService = hidalgoService.CalculCritereC2(this.GetOneProgrammationToTest(), listeDocumentPge);

            //Assert
            Assert.IsTrue(resultOfService != null);
        }

        /// <summary>
        /// Obtient l'objet de type ProgrammationDto pour lancer le calcul du critere C2
        /// </summary>
        /// <returns>Objet initialiser</returns>
        private Contract.DTO.ProgrammationDto GetOneProgrammationToTest()
        {
            return new ProgrammationDto
            {
                NatureTravaux = ENatureCalibrage.Maçonnerie,
            };
        }
    }
}
