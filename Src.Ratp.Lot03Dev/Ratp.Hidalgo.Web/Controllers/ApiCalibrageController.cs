using Microsoft.Practices.ServiceLocation;
using Ratp.Hidalgo.App.Contract;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Web.ViewModel;
using System.Collections.Generic;
using System.Web.Http;
using log4net;
using Ratp.Hidalgo.Data.Contract.Enum;
using System.Linq;
using Ratp.Hidalgo.Web.Models;
using System.Collections;
using Ratp.Hidalgo.Web.Outils;
using System.Configuration;
using System;

namespace Ratp.Hidalgo.Web.Controllers
{
    public class ApiCalibrageController : ApiController
    {
        private ICalibrageApp CalibrageApp { get; set; }
        private IHidalgoApp HidalgoService { get; set; }
        private ICalculServiceApp CalculService { get; set; }

        /// <summary>
        /// Obtient l'instance de l'objet log pour tracaer les messages d'erreur, info
        /// </summary>
        private readonly ILog Logger;

        public ApiCalibrageController()
        {
            this.CalibrageApp = ServiceLocator.Current.GetInstance<ICalibrageApp>();
            this.CalculService = ServiceLocator.Current.GetInstance<ICalculServiceApp>();
            this.HidalgoService = ServiceLocator.Current.GetInstance<IHidalgoApp>();
            this.Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        // GET: api/ApiCalibrage
        [Route("api/GetAllLignes")]
        public IEnumerable<LigneDto> Get()
        {
            var model = this.CalculService.GetAllLigneByFamille();
            return model;
        }

        // GET: api/ApiCalibrage/5
        [Route("api/GetAllLieuxByLigne/{idLigne}")]
        public IEnumerable<LieuDto> Get(int idLigne)
        {
            var model = this.CalculService.GetAllLieuxByLigneGestionnaire(idLigne);
            return model;
        }

        // GET: api/ApiCalibrage/GetAllAnnee?annee=2017
        [Route("api/GetAllAnnees/{annee}")]
        public IEnumerable<int> GetAllAnnee(int annee)
        {
            List<int> annees = new List<int>() { annee };
            for (var i = 0; i < 20; i++)
            {
                annees.Add(annees[0] + i + 1);
            }

            return annees;
        }

        //GET: api/ApiCalibrage/GetAllNatureTravauxExternes
        [Route("api/GetAllNatureTravauxExt")]
        public IEnumerable<NatureTravauxExternesDto> GetAllNatureTravauxExternes()
        {
            var model = this.CalculService.GetAllNatureTravauxExternes();
            return model;
        }

        // GET: api/GetAllTypeOuvrages
        [Route("api/GetAllTypeOuvrages")]
        public IEnumerable<TypeOuvragesDto> GetAllTypeOuvrages()
        {
            var model = this.HidalgoService.GetAllTypeOuvrages();
            return model;
        }

        // GET: api/GetAllNatureTravaux
        [Route("api/GetAllNatureTravaux")]
        public IEnumerable<NatureCalibrage> GetAllNatureTravaux()
        {
            IList<NatureCalibrage> listeNatureCalibrage = new List<NatureCalibrage>();
            foreach (var item in System.Enum.GetValues(typeof(ENatureCalibrage)))
            {
                listeNatureCalibrage.Add(new NatureCalibrage { Id = (int)item, Nature = item.ToString() });
            }

            return listeNatureCalibrage;
        }

        // POST: api/ApiCalibrage
        public IHttpActionResult Post([FromBody]DataTransfert model)
        {
            if (model != null)
            {
                var startIndex = model.index.IndexOf("_");
                var code = model.index.Substring(startIndex + 1);
                decimal valeur;

                if (decimal.TryParse(model.value.Replace('.', ','), out valeur) && !string.IsNullOrEmpty(model.value))
                {
                    switch (model.nature)
                    {
                        case Data.Contract.Enum.ENatureCalibrage.Maçonnerie:
                            this.CalibrageApp.UpdateCoefficientPonderationMaconnerie(new CoefficientPonderationDto { ValeurMaconnerie = valeur, Code = code });
                            break;
                        case Data.Contract.Enum.ENatureCalibrage.Enduit:
                            this.CalibrageApp.UpdateCoefficientPonderationEnduit(new CoefficientPonderationDto { ValeurEnduit = valeur, Code = code });
                            break;
                    }
                }
            }

            return Ok(model);
        }

        // PUT: api/ApiCalibrage/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiCalibrage/5
        public void Delete(int id)
        {
        }

        [Route("api/GetAllDocumentPgeByProgrammation/{idProgrammation}")]
        public IEnumerable<ProgrammationDocumentPgeDto> GetAllDocumentPgeByProgrammation(int idProgrammation)
        {
            IEnumerable<ProgrammationDocumentPgeDto> listePGE = this.CalculService.GetAllDocumentPgeByProgrammation(idProgrammation);
            //Gestion Excel
            GestionExcel excel = new GestionExcel(idProgrammation, listePGE);
            if (excel.OpenExcel())
            {
                if (excel.CreerOuOuvrirFichier())
                {
                    try
                    {
                        excel.EditerDonneePge();
                    }
                    catch (Exception e)
                    {
                        //logger
                        this.Logger.Error("Impossible d'ajouter les données au fichier Excel.");
                    }
                    finally
                    {
                        excel.FermerExcel();
                    }

                }
                else
                {
                    this.Logger.Error("Impossible d'ouvrir ou de créer le fichier Excel pour la programmation " + idProgrammation + " à l'emplacement : " + ConfigurationManager.AppSettings["CheminDossierExcel"]);
                }
            }
            return listePGE;
        }

        /// <summary>
        /// Méthode permettant l'execution d'une instance Excel
        /// </summary>
        [Route("api/ExecuteInstanceExcel/{idProgrammation}")]
        public void ExecuteInstanceExcel(int idProgrammation)
        {
            GestionExcel Excel = new GestionExcel(idProgrammation);
            Excel.AfficherExcel();
        }


        //Methode de sauvegarde pour l etape 4 : Requete Ajax
        [Route("api/SaveDocumentPgeByListDocumentPge")]
        public void SaveDocumentPgeByListDocumentPge(ProgrammationDocumentPgeDto[] listePGE)
        {
            this.CalculService.SaveProgrammationDocumentPge(listePGE);
        }

        //TODO RDT deplacement dans un dossier approprié
        //Methode pour la vue Archivage Requete Ajax
        [Route("api/GetAllProgrammation")]
        public IEnumerable<ProgrammationDto> GetAllProgrammation()
        {
            return this.CalculService.GetAllProgrammation();
        }

        //Appel Ajax : Suppression de la programmation (avec ses dependances)
        [Route("api/RemoveProgrammation/{idProgrammation}")]
        public void RemoveProgrammation(int idProgrammation)
        {
            this.CalculService.RemoveOneProgrammation(idProgrammation);

        }

        /// <summary>
        /// Méthode permettant la consultation des PGE selectionnées
        /// </summary>
        /// <param name="listePGE">liste des PGE concernées</param>
        [Route("api/GetAllValeurParametres")]
        public Dictionary<ProgrammationDocumentPgeDto, IEnumerable<ValeursParametresPgeDto>> ConsulterDocumentPge(ProgrammationDocumentPgeDto[] listePGE)
        {
            return this.CalculService.GetAllValeurParametreByPge(listePGE);

        }
        [Route("api/GetLignesOrdreAsc")]
        [HttpPost]
        public IHttpActionResult GetLignesOrdreAsc(RootObjectRnmVm model)
        {
            return Ok(model.ListeRnm.OrderBy(x => x.Ligne.Id).ToList());
        }

        [Route("api/GetLieuxOrdreAsc")]
        [HttpPost]
        public IHttpActionResult GetLieuxOrdreAsc(RootObjectRnmVm model)
        {
            return Ok(model.ListeRnm.OrderBy(x => x.Lieu.Id).ToList());
        }

        [Route("api/GetLieuxOrdreDesc")]
        [HttpPost]
        public IHttpActionResult GetLieuxOrdreDesc(RootObjectRnmVm model)
        {
            return Ok(model.ListeRnm.OrderByDescending(x => x.Lieu.Id).ToList());
        }

        [Route("api/GetAnneesOrdreAsc")]
        [HttpPost]
        public IHttpActionResult GetAnneesOrdreAsc(RootObjectRnmVm model)
        {
            return Ok(model.ListeRnm.OrderBy(x => x.Annee).ToList());
        }

        [Route("api/GetLignesTeOrdreAsc")]
        [HttpPost]
        public IHttpActionResult GetLignesTeOrdreAsc(RootObjectTravauxExternesVm model)
        {
            return Ok(model.ListeTravauxExt.OrderBy(x => x.Ligne.Id).ToList());
        }

        [Route("api/GetListeTravauxExterneByLieuxAsc")]
        [HttpPost]
        public IHttpActionResult GetLieuxTeOrdreAsc(RootObjectTravauxExternesVm model)
        {
            return Ok(model.ListeTravauxExt.OrderBy(x => x.Lieu.Id).ToList());
        }

        [Route("api/GetListeTravauxExterneByLieuxDesc")]
        [HttpPost]
        public IHttpActionResult GetLieuxTeOrdreDesc(RootObjectTravauxExternesVm model)
        {
            return Ok(model.ListeTravauxExt.OrderByDescending(x => x.Lieu.Id).ToList());
        }

        [Route("api/GetListeTravauxExterneByNaturesTxAsc")]
        [HttpPost]
        public IHttpActionResult GetNatureTxTeOrdreAsc(RootObjectTravauxExternesVm model)
        {
            return Ok(model.ListeTravauxExt.OrderBy(x => x.NatureTravauxExt.Id).ToList());
        }

        [Route("api/GetListeTravauxExterneByDateAsc")]
        [HttpPost]
        public IHttpActionResult GetDateTeOrdreAsc(RootObjectTravauxExternesVm model)
        {
            return Ok(model.ListeTravauxExt.OrderBy(x => x.Date).ToList());
        }

        [Route("api/GetAllLignesTest")]
        public IHttpActionResult GetAllLignesTest()
        {
            IEnumerable<LigneDto> listeLignes = new List<LigneDto>() { new LigneDto { Id = 1, Name = "Ligne1", Checked = true },
            new LigneDto { Id = 2, Name = "Ligne2", Checked = true }};

            return Ok(listeLignes);
        }
    }
}
