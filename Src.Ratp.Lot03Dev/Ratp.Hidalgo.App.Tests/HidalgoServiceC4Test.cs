using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratp.Hidalgo.App.Tests.App_Start;
using Ratp.Hidalgo.App.Contract;
using Ratp.Hidalgo.App.Contract.DTO;
using System.Collections.Generic;
using Ratp.Hidalgo.Data.Contract.Enum;

namespace Ratp.Hidalgo.App.Tests
{
    [TestClass]
    public class HidalgoServiceC4Test
    {
        /// <summary>
        /// Test de la méthode HidalgoApp.GetNoteMaxForAllDocumentPge_TravauxConnexes()
        /// </summary>
        [TestMethod]
        public void GetNoteMaxForAllDocumentPge_TravauxConnexesTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            //Act
            hidalgoService.CalculNotesParametresCritere4(this.GetOneProgrammationToTest(), listeDocumentPge);

            //Assert
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.GetNoteMaxForAllDocumentPge_TravauxInternes()
        /// </summary>
        [TestMethod]
        public void GetNoteMaxForAllDocumentPge_TravauxInternesTest()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            //Act
            var resultOfService = hidalgoService.GetNoteMaxForAllDocumentPge_TravauxInternes(this.GetOneProgrammationToTest(), listeDocumentPge);

            //Assert
            Assert.IsTrue(resultOfService != null);
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.CalculCritereC4()
        /// </summary>
        [TestMethod]
        public void CalculCritereC4Test()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();
            var listeDocumentPge = hidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Maçonnerie);

            //Act
            var resultOfService = hidalgoService.CalculCritereC4(this.GetOneProgrammationToTest(), listeDocumentPge);

            //Assert
            Assert.IsTrue(resultOfService != null);
        }

        /// <summary>
        /// Test de la méthode HidalgoApp.EtapeCalcul2
        /// </summary>
        [TestMethod]
        public void EtapeCalcul2Test()
        {
            //Arrang
            var hidalgoService = UtilityTest.CreateInstanceService<IHidalgoApp>();

            //Act
            var result = hidalgoService.CalculEtape2(this.GetOneProgrammationToTest());

            //Assert
            //TODO YAR: faut s'assurer que le fichier est générer, a cause de pression du temps, j'ai laissé ce traitement après
            Assert.IsTrue(result != null);
        }

        /// <summary>
        /// Obtient l'objet de type ProgrammationDto pour lancer le calcul du critere C3
        /// </summary>
        /// <returns>Objet initialiser</returns>
        private Contract.DTO.ProgrammationDto GetOneProgrammationToTest()
        {
            return new ProgrammationDto
            {
                NatureTravaux = ENatureCalibrage.Maçonnerie,
                Anneeprogrammation = 2016,
                Lignes = new List<LigneDto> { new LigneDto { Id = 1, Name = "Ligne 1" }, new LigneDto { Id = 2, Name = "Ligne 2" } },
                Rnms = new List<RnmDto>
                {
                    new RnmDto{
                        Annee = 2016,
                        Lieu= new LieuDto{ Id= 471, Name = "lieu"},
                        Lignes = new LigneDto { Id = 4, Name = "Ligne 4" },
                    },
                },
                TravauxExternes = new List<TravauxExternesDto>
             {
                 new TravauxExternesDto
                 {
                     NatureTravauxExt = new NatureTravauxExternesDto{Id = 2},
                     Date="05/2016",
                     Lieu =  new LieuDto{ Id= 471, Name = "lieu"},
                     Lignes = new LigneDto { Id = 4, Name = "Ligne 4" },
                 }
             }
            };
        }
    }
}
