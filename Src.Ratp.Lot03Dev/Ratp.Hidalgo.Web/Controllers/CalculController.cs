using Ratp.Hidalgo.App.Contract;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Enum;
using Ratp.Hidalgo.Web.Infra;
using Ratp.Hidalgo.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace Ratp.Hidalgo.Web.Controllers
{
    /// <summary>
    /// Controller pour la nouvelle programmation
    /// </summary>
    public class CalculController : Controller
    {
        /// <summary>
        /// Field pour stocker l'informations <see cref="CalculServiceApp"/>
        /// </summary>
        private ICalculServiceApp CalculService { get; set; }
        private IHidalgoApp HidalgoService { get; set; }

        /// <summary>
        /// Initialiser les paramètre
        /// </summary>
        /// <param name="serviceCalcul">Service Applicatif ServiceCalcul</param>
        /// <param name="hidalgoApp">Service Applicatif ServiceHidalgo</param>
        public CalculController(ICalculServiceApp serviceCalcul, IHidalgoApp hidalgoApp)
        {
            this.CalculService = serviceCalcul;
            this.HidalgoService = hidalgoApp;
        }

        /// <summary>
        /// Action permettant de rediriger vers la page d'accueil
        /// </summary>
        /// <returns>view accueil</returns>
        //[UserFilter]
        // GET: Calcul
        public ActionResult Index()
        {
            //TODO YAR: remplacer par l'appel au service MARION pour obtenir les infos
            this.ViewBag.USERCONNECT = "USER USER";
            ProgrammationVm model = new ProgrammationVm();
            var listeLignes = this.CalculService.GetAllLigneByFamille();
            model.LignesGestionnaires = listeLignes;
            model.Lignes = listeLignes.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });

            model.Lieux = new SelectList(new List<LieuDto>(), "Value", "Text");
            model.Annees = new List<SelectListItem> { new SelectListItem { Text = "2017", Value = "2017" } };
            return View(model);
        }

        /// <summary>
        /// Méthode permettant de sauvegarder une nouvelle programmation
        /// </summary>
        /// <param name="model">l'objet à sauvegarder</param>
        /// <returns>view</returns>
        [HttpPost]
        public ActionResult SaveNouvelleProgrammation(ProgrammationVm model)
        {
            string userId = TempData["USERID"].ToString();
            if (model != null && this.UserConnect(userId))
            {
                var programmationDto = this.GetProgrammationDto(model);
                if (programmationDto.Id == 0)
                {
                    var result = this.HidalgoService.CalculEtape2(programmationDto);
                    model.Idprogrammation = result;
                    return this.Content(model.Idprogrammation.ToString());
                }
                else
                {
                    this.CalculService.UpdateProgrammation(programmationDto);
                    return RedirectToAction("Index", model);
                }
            }

            return View("~/Views/Shared/Error");
        }

        /// <summary>
        /// Méthode permettant de charger la derniere programmation
        /// </summary>
        /// <param name="nature">NatureTravaux de la programmation recherchée</param>
        /// <param name="type">TypeOuvrage de la programmation recherchée</param>
        /// <param name="Annee">Annee de la programmation recherchée</param>
        /// <returns>view</returns>
        //[HttpPost]
        //public ActionResult LoadLastProgrammation(string nature, string type, int Annee)
        //{

        //    string userId = TempData["USERID"].ToString();
        //    if (this.UserConnect(userId))
        //    {
        //        int annee = 0;
        //        //int.TryParse(Annee, out annee);
        //        var programmation = this.CalculService.GetProgrammationByNatureTypeAnnee(nature, type, Annee);
        //        return RedirectToAction("Index", programmation);
        //    }
        //    return View("Archive");
        //}


        // /// <summary>
        // /// Méthode permettant la consultation des PGE selectionnées
        // /// </summary>
        // /// <param name="listePGE">l'objet à sauvegarder</param>
        public ActionResult ConsulterDocumentPge(ProgrammationDocumentPgeDto[] listePGE)
        {
            return View(this.CalculService.GetAllValeurParametreByPge(listePGE));

        }

        /// <summary>
        /// Méthode permettant la modification de la derniere PGE renseigné
        /// </summary>
        public ActionResult ModificationProgrammation()
        {
            ProgrammationVm model = new ProgrammationVm();
            return View(model);
        }

        /// <summary>
        /// Méthode permettant consultation de la dernière programmation définitive et le suivi des travaux (etape 5)
        /// </summary>
        public ActionResult ConsultationSuiviTravaux()
        {
            ProgrammationVm model = new ProgrammationVm();
            return View(model);
        }

        /// <summary>
        /// Méthode permettant l'affichage de toutes les programmation
        /// </summary>
        public ActionResult Archive()
        {
            return View();
        }

        /// <summary>
        /// Méthode permettant la consultation des pge par categorie (etape 5)
        /// </summary>
        public ActionResult Categorie(int idProgrammation)
        {
            IEnumerable<ProgrammationDocumentPgeDto> listePGE = this.CalculService.GetAllDocumentPgeByProgrammation(idProgrammation);
            ProgrammationDocumentPGECategorieVm model = new ProgrammationDocumentPGECategorieVm();
            model.ListePGE = listePGE;
            return View(model);
        }

        /// <summary>
        /// Méthode permettant consultation de la dernière programmation définitive et le suivi des travaux (etape 5)
        /// </summary>
        /// <param name="nature">NatureTravaux de la programmation recherchée</param>
        /// <param name="type">TypeOuvrage de la programmation recherchée</param>
        /// <param name="Annee">Annee de la programmation recherchée</param>
        public ActionResult AfficherProgrammation(ProgrammationVm modelModification)
        {
            //int annee = 0;
            //int.TryParse(Annee, out annee);
            var programmation = this.CalculService.GetProgrammationByNatureTypeAnnee(Enum.GetName(typeof(ENatureCalibrage), modelModification.Programmation.NatureTravaux), "Tunnel + Couloire", modelModification.AnneeProgrammation);
            return View(programmation);
        }

        /// <summary>
        /// Méthode permettant d'obtenir la liste des lieux gestiaonnaire par ligne choisi
        /// </summary>
        /// <param name="idLigne">la ligne gestionnaire</param>
        /// <returns>liste des lieux trouvés converter en JSON</returns>
        public ActionResult GetLieuxGestionnaire(string idLigne)
        {
            if (!string.IsNullOrEmpty(idLigne))
            {
                var model = this.CalculService.GetAllLieuxByLigneGestionnaire(int.Parse(idLigne));
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Méthode permettant de récupérer la liste des PGE
        /// </summary>
        /// <param name="model"></param>
        /// <returns>liste des model transmettre à la vue</returns>
        public ActionResult ListeOfPge(ProgrammationVm model)
        {
            var listeDocumentPge = this.HidalgoService.CalculEtape2(model.Programmation);
            return View(listeDocumentPge);
        }

        public ActionResult GetDocumentsPgeWithCategories(string idProgrammation)
        {
            if (string.IsNullOrEmpty(idProgrammation))
            {
                throw new ArgumentNullException("l'identifiant de la programmation est null");
            }

            var model = this.CalculService.GetAllDocumentPgeByProgrammation(int.Parse(idProgrammation));

            IList<ProgrammationDocumentPGECategorieVm> listePgeByCategorie = new List<ProgrammationDocumentPGECategorieVm>();

            foreach (var item in model.OrderByDescending(x => x.Categorie).Select(x => x.Categorie).Distinct().ToList())
            {
                switch (item)
                {
                    case "CAT 4":
                        ProgrammationDocumentPGECategorieVm docByCat4 = new ProgrammationDocumentPGECategorieVm();
                        docByCat4.CoutByCategorie = model.Where(x => x.Categorie == "CAT 4").Sum(x => x.Budget.GetValueOrDefault());
                        docByCat4.ListePGE = model.Where(x => x.Categorie == "CAT 4").ToList();

                        listePgeByCategorie.Add(docByCat4);
                        break;

                    case "CAT 3":
                        ProgrammationDocumentPGECategorieVm docByCat3 = new ProgrammationDocumentPGECategorieVm();
                        docByCat3.CoutByCategorie = model.Where(x => x.Categorie == "CAT 3").Sum(x => x.Budget.GetValueOrDefault());
                        docByCat3.ListePGE = model.Where(x => x.Categorie == "CAT 3").ToList();

                        listePgeByCategorie.Add(docByCat3);
                        break;

                    case "CAT 2":
                        ProgrammationDocumentPGECategorieVm docByCat2 = new ProgrammationDocumentPGECategorieVm();
                        docByCat2.CoutByCategorie = model.Where(x => x.Categorie == "CAT 2").Sum(x => x.Budget.GetValueOrDefault());
                        docByCat2.ListePGE = model.Where(x => x.Categorie == "CAT 2").ToList();

                        listePgeByCategorie.Add(docByCat2);
                        break;
                    case "CAT 1":
                        ProgrammationDocumentPGECategorieVm docByCat1 = new ProgrammationDocumentPGECategorieVm();
                        docByCat1.CoutByCategorie = model.Where(x => x.Categorie == "CAT 1").Sum(x => x.Budget.GetValueOrDefault());
                        docByCat1.ListePGE = model.Where(x => x.Categorie == "CAT 1").ToList();

                        listePgeByCategorie.Add(docByCat1);

                        break;
                }
            }

            return View(listePgeByCategorie);
        }

        /// <summary>
        /// Méthode permettant d'obtenir un objet de type <see cref="ProgrammationDto"/> pour le transmettre au serveur applicatif
        /// </summary>
        /// <param name="programmationVm">objet de type ProgrammationVM l'objet récupéré depuis la vue</param>
        /// <returns>ProgrammationDto initialisé</returns>
        private ProgrammationDto GetProgrammationDto(ProgrammationVm programmationVm)
        {
            var programmationDto = programmationVm.Programmation;

            //Historique
            programmationDto.UserCreation = int.Parse(TempData["USERID"].ToString());
            programmationDto.UserModification = int.Parse(TempData["USERID"].ToString());
            programmationDto.DateCreation = DateTime.Now;
            programmationDto.DateModification = DateTime.Now;

            programmationDto.Lignes = new List<LigneDto>();
            programmationDto.Rnms = new List<RnmDto>();
            programmationDto.TravauxExternes = new List<TravauxExternesDto>();

            //Mapping Lignes
            if (programmationVm.LignesGestionnaires != null)
            {
                foreach (var item in programmationVm.LignesGestionnaires)
                {
                    if (item.Checked)
                    {
                        programmationDto.Lignes.Add(new LigneDto { Name = item.Name, Id = item.Id, UserCreation = int.Parse(TempData["USERID"].ToString()), UserModification = int.Parse(TempData["USERID"].ToString()), DateCreation = DateTime.Now, DateModification = DateTime.Now });
                    }
                }
            }

            //Mapping RNM
            if (programmationVm.ListeRnm != null)
            {
                foreach (var item in programmationVm.ListeRnm)
                {
                    programmationDto.Rnms.Add(new RnmDto
                    {
                        Lignes = item.Ligne,
                        Annee = item.Annee,
                        Lieu = item.Lieu,
                        UserCreation = int.Parse(TempData["USERID"].ToString()),
                        UserModification = int.Parse(TempData["USERID"].ToString()),
                        DateCreation = DateTime.Now,
                        DateModification = DateTime.Now
                    });
                }
            }

            //Mapping travaux externes
            if (programmationVm.ListeTravauxExt != null)
            {
                foreach (var item in programmationVm.ListeTravauxExt)
                {
                    programmationDto.TravauxExternes.Add(new TravauxExternesDto { Lignes = item.Ligne, Date = item.Date, Lieu = item.Lieu, NatureTravauxExt = item.NatureTravauxExt, UserCreation = int.Parse(TempData["USERID"].ToString()), UserModification = int.Parse(TempData["USERID"].ToString()), DateCreation = DateTime.Now, DateModification = DateTime.Now });
                }
            }

            return programmationDto;
        }

        /// <summary>
        /// Méthode permettant de vérifier si l'utilisateur est toujours connecté à la sessiosn
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool UserConnect(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Méthode pour générer les exceptions lié au controller
        /// </summary>
        /// <param name="filterContext">la source de l'exceptions</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, "CalculControler", "Action");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

        }
    }
}