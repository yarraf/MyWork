using Ratp.Hidalgo.App.Contract;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Web.Properties;
using Ratp.Hidalgo.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Ratp.Hidalgo.Web.Const;
using Ratp.Hidalgo.Data.Contract.Enum;
using Ratp.Hidalgo.Web.Infra;

namespace Ratp.Hidalgo.Web.Controllers
{
    public class CalibrageController : Controller
    {
        private static IEnumerable<CoefficientPonderationDto> ListeinitialCoefficientPonderation { get; set; }

        /// <summary>
        /// Field pour stocker l'informations <see cref="CalibrageApp"/>
        /// </summary>
        private ICalibrageApp CalibrageApp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calibrageApp"></param>
        public CalibrageController(ICalibrageApp calibrageApp)
        {
            this.CalibrageApp = calibrageApp;
        }

        // GET: Calibrage
        public ActionResult Index()
        {
            var AllCoefficientPonderations = this.CalibrageApp.GetAllCoeffcientPonderations();
            return View(this.GetModelPersist(AllCoefficientPonderations, 0));
        }

        //[UserFilter]
        /// <summary>
        /// Obtenir les données de calibrage existant 
        ///  // GET: Calibrage
        /// </summary>
        /// <returns>model à afficher</returns>
        public ActionResult ValidInfoCalibrage(int type)
        {
            var allCoefficientPonderations = this.CalibrageApp.GetAllCoeffcientPonderations();
            TempData["listeCoefficientPonderationParametres"] = allCoefficientPonderations;
            return View(this.GetModelPersist(allCoefficientPonderations, type));
        }

        /// <summary>
        /// Action pour valider les données de l'interface calibrage
        ///  // POST: Calibrage
        /// </summary>
        /// <param name="model">model à enregistrer</param>
        /// <returns>le model enregistré</returns>
        [HttpPost]
        public ActionResult ValidInfoCalibrage(CoefficientPonderationVm model)
        {
            if (!string.IsNullOrEmpty(RouteData.Values["type"].ToString()))
            {
                var natureCalibrage = int.Parse(RouteData.Values["type"].ToString());
                if (natureCalibrage == (int)ENatureCalibrage.Enduit)
                {
                    this.ModelState.Remove("CpA5");
                }

                if (natureCalibrage == (int)ENatureCalibrage.Maçonnerie)
                {
                    this.ModelState.Remove("CpA6");
                }
            }

            if (this.ModelState.IsValid)
            {
                if (!this.IsValideSommeValeurCritere(model))
                {
                    return View(model);
                }

                //Vérifier la valeur du largeur du tunnel
                if (!string.IsNullOrEmpty(RouteData.Values["type"].ToString()))
                {
                    model.NatureCalibrage = (ENatureCalibrage)int.Parse(RouteData.Values["type"].ToString());
                    if (!this.IsValideModelEnduitMaconnerie(model))
                    {
                        return View(model);
                    }
                }

                if (model.NatureCalibrage == ENatureCalibrage.Maçonnerie)
                {
                    this.CalibrageApp.UpdateListeCoefficientsPonderationMaçonnerie(this.ListCoefficientPonderationMaconnerieUpdated(model));
                    return RedirectToAction("ValidInfoCalibrage");
                }
                else if (model.NatureCalibrage == ENatureCalibrage.Enduit)
                {
                    this.CalibrageApp.UpdateListeCoefficientsPonderationEnduit(this.ListCoefficientPonderationEnduitUpdated(model));
                    return RedirectToAction("ValidInfoCalibrage");
                }
            }

            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult ValeurSeuilsReferenceCriteres()
        {
            //var listeparametres = TempData["listeCoefficientPonderationParametres"] as IEnumerable<CoefficientPonderationDto>;
            var listeparametres = this.CalibrageApp.GetAllCoeffcientPonderations();
            return PartialView("_ViewValeurMinMaxCriteres", this.ListeSeuilReferenceCriteres(listeparametres));
        }

        [ChildActionOnly]
        public PartialViewResult ValeurMinMaxCritere()
        {
            //var listeparametres = TempData["listeCoefficientPonderationParametres"] as IEnumerable<CoefficientPonderationDto>;
            var listeparametres = this.CalibrageApp.GetAllCoeffcientPonderations();
            return PartialView("_ViewInfoMinMaxCriteres", this.ListValeursMinMaxCriteres(listeparametres));
        }

        [ChildActionOnly]
        public PartialViewResult GetStatistiqueSeuil()
        {
            //var listeparametres = TempData["listeCoefficientPonderationParametres"] as IEnumerable<CoefficientPonderationDto>;
            var listeparametres = this.CalibrageApp.GetAllCoeffcientPonderations();
            return PartialView("_ViewGraphique", this.ListeSeuilReferenceCriteres(listeparametres));
        }

        /// <summary>
        /// Méthode permettant de transformer les données de calibrage existant dans un objet de type <see cref="CoefficientPonderationVm"/>
        /// </summary>
        /// <param name="listeCoefficientPonderation">La liste des paramètres récupérés depuis la base de données</param>
        /// <returns>Ovbjet de type CoefficientPonderationVm</returns>
        private CoefficientPonderationVm GetModelPersist(IEnumerable<CoefficientPonderationDto> listeCoefficientPonderation, int natureCalibrae)
        {
            var model = new CoefficientPonderationVm();
            model.NatureCalibrage = (ENatureCalibrage)natureCalibrae;
            model.ListeValeurMinMaxCriteres = this.ListValeursMinMaxCriteres(listeCoefficientPonderation);
            model.ListeSeuilReferenceCriteres = this.ListeSeuilReferenceCriteres(listeCoefficientPonderation);
            foreach (var item in listeCoefficientPonderation)
            {
                switch (item.Code)
                {
                    case "CpA1":
                        model.CpA1 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpA4":
                        model.CpA4 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpA5":
                        model.CpA5 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpA6":
                        model.CpA6 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpA7":
                        model.CpA7 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpA9":
                        model.CpA9 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpA10":
                        model.CpA10 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpA11":
                        model.CpA11 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpB1":
                        model.CpB1 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpB2":
                        model.CpB2 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpB3":
                        model.CpB3 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "CpB4":
                        model.CpB4 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "Cp1":
                        model.Cp1 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "Cp2":
                        model.Cp2 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "Cp3":
                        model.Cp3 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "Cp4":
                        model.Cp4 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConverted() : item.ValeurEnduit.GetValueConverted();
                        break;
                    case "V1":
                        model.V1 = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConvertedWitoutdecimal() : item.ValeurEnduit.GetValueConvertedWitoutdecimal();
                        break;
                    case "Dv":
                        model.Dv = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConvertedWitoutdecimal() : item.ValeurEnduit.GetValueConvertedWitoutdecimal();
                        break;
                    case "Pt":
                        model.Pt = model.NatureCalibrage == ENatureCalibrage.Maçonnerie ? item.ValeurMaconnerie.GetValueConvertedWitoutdecimal() : item.ValeurEnduit.GetValueConvertedWitoutdecimal();
                        break;
                }
            }

            return model;
        }

        /// <summary>
        /// Initialisation des nouvelles valeur pour préparer la mise à jour des coefficients des pondérations des paramètres de la nature Maçonnerie
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private IEnumerable<CoefficientPonderationDto> ListCoefficientPonderationMaconnerieUpdated(CoefficientPonderationVm model)
        {
            IList<CoefficientPonderationDto> listeParametresUpdated = new List<CoefficientPonderationDto>();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(RouteData.Values["type"].ToString()))
                {
                    model.NatureCalibrage = (ENatureCalibrage)int.Parse(RouteData.Values["type"].ToString());
                }

                var listeCoefficientPonderationOld = this.CalibrageApp.GetAllCoeffcientPonderations();
                foreach (var item in listeCoefficientPonderationOld)
                {
                    switch (item.Code)
                    {
                        case "CpA1":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpA1);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA4":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpA4);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA5":
                            if (model.NatureCalibrage == Data.Contract.Enum.ENatureCalibrage.Maçonnerie)
                            {
                                item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpA5);
                                listeParametresUpdated.Add(item);
                            }

                            break;
                        case "CpA6":
                            if (model.NatureCalibrage == Data.Contract.Enum.ENatureCalibrage.Enduit)
                            {
                                item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpA6);
                            }

                            listeParametresUpdated.Add(item);

                            break;
                        case "CpA7":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpA7);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA9":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpA9);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA10":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpA10);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA11":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpA11);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpB1":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpB1);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpB2":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpB2);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpB3":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpB3);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpB4":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.CpB4);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Cp1":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.Cp1);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Cp2":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.Cp2);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Cp3":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.Cp3);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Cp4":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.Cp4);
                            listeParametresUpdated.Add(item);
                            break;
                        case "V1":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.V1);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Dv":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.Dv);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Pt":
                            item.ValeurMaconnerie = this.GetValueDecimalConverted(model.Pt);
                            listeParametresUpdated.Add(item);
                            break;
                    }
                }
            }

            return listeParametresUpdated;
        }

        /// <summary>
        /// Initialisation des nouvelles valeur pour préparer la mise à jour des coefficients des pondérations des paramètres de la nature Enduit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private IEnumerable<CoefficientPonderationDto> ListCoefficientPonderationEnduitUpdated(CoefficientPonderationVm model)
        {
            IList<CoefficientPonderationDto> listeParametresUpdated = new List<CoefficientPonderationDto>();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(RouteData.Values["type"].ToString()))
                {
                    model.NatureCalibrage = (ENatureCalibrage)int.Parse(RouteData.Values["type"].ToString());
                }

                var listeCoefficientPonderationOld = this.CalibrageApp.GetAllCoeffcientPonderations();
                foreach (var item in listeCoefficientPonderationOld)
                {
                    switch (item.Code)
                    {
                        case "CpA1":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpA1);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA4":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpA4);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA5":
                            if (model.NatureCalibrage == Data.Contract.Enum.ENatureCalibrage.Maçonnerie)
                            {
                                item.ValeurEnduit = this.GetValueDecimalConverted(model.CpA5);
                                listeParametresUpdated.Add(item);
                            }

                            break;
                        case "CpA6":
                            if (model.NatureCalibrage == Data.Contract.Enum.ENatureCalibrage.Enduit)
                            {
                                item.ValeurEnduit = this.GetValueDecimalConverted(model.CpA6);
                            }

                            listeParametresUpdated.Add(item);

                            break;
                        case "CpA7":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpA7);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA9":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpA9);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA10":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpA10);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpA11":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpA11);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpB1":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpB1);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpB2":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpB2);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpB3":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpB3);
                            listeParametresUpdated.Add(item);
                            break;
                        case "CpB4":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.CpB4);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Cp1":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.Cp1);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Cp2":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.Cp2);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Cp3":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.Cp3);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Cp4":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.Cp4);
                            listeParametresUpdated.Add(item);
                            break;
                        case "V1":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.V1);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Dv":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.Dv);
                            listeParametresUpdated.Add(item);
                            break;
                        case "Pt":
                            item.ValeurEnduit = this.GetValueDecimalConverted(model.Pt);
                            listeParametresUpdated.Add(item);
                            break;
                    }
                }
            }

            return listeParametresUpdated;
        }

        /// <summary>
        /// Initialiser une liste des valeurs min et max des critères
        /// </summary>
        /// <param name="listeCoefficientPonderation"></param>
        /// <returns></returns>
        private IEnumerable<CriteresVm> ListValeursMinMaxCriteres(IEnumerable<CoefficientPonderationDto> listeCoefficientPonderation)
        {
            var listeValeurMinMaxcritere = new List<CriteresVm>();

            if (!string.IsNullOrEmpty(RouteData.Values["type"].ToString()))
            {
                var natureCalibrage = int.Parse(RouteData.Values["type"].ToString());
                if (natureCalibrage == (int)ENatureCalibrage.Enduit)
                {
                    if (listeCoefficientPonderation != null && listeCoefficientPonderation.Any())
                    {
                        CriteresVm c1 = new ViewModel.CriteresVm
                        {
                            LibelleCritere = "C1",
                            ValeurMinCritere = listeCoefficientPonderation.Single(x => x.Code == "MinC1").ValeurEnduit.GetValueConverted(),
                            ValeurMaxCritere = listeCoefficientPonderation.Single(x => x.Code == "MaxC1").ValeurEnduit.GetValueConverted()
                        };

                        CriteresVm c2 = new ViewModel.CriteresVm
                        {
                            LibelleCritere = "C2",
                            ValeurMinCritere = listeCoefficientPonderation.Single(x => x.Code == "MinC2").ValeurEnduit.GetValueConverted(),
                            ValeurMaxCritere = listeCoefficientPonderation.Single(x => x.Code == "MaxC2").ValeurEnduit.GetValueConverted()
                        };

                        CriteresVm c3 = new ViewModel.CriteresVm
                        {
                            LibelleCritere = "C3",
                            ValeurMinCritere = listeCoefficientPonderation.Single(x => x.Code == "MinC3").ValeurEnduit.GetValueConverted(),
                            ValeurMaxCritere = listeCoefficientPonderation.Single(x => x.Code == "MaxC3").ValeurEnduit.GetValueConverted()
                        };

                        listeValeurMinMaxcritere.Add(c1);
                        listeValeurMinMaxcritere.Add(c2);
                        listeValeurMinMaxcritere.Add(c3);

                    }
                }

                if (natureCalibrage == (int)ENatureCalibrage.Maçonnerie)
                {
                    if (listeCoefficientPonderation != null && listeCoefficientPonderation.Any())
                    {
                        CriteresVm c1 = new ViewModel.CriteresVm
                        {
                            LibelleCritere = "C1",
                            ValeurMinCritere = listeCoefficientPonderation.Single(x => x.Code == "MinC1").ValeurMaconnerie.GetValueConverted(),
                            ValeurMaxCritere = listeCoefficientPonderation.Single(x => x.Code == "MaxC1").ValeurMaconnerie.GetValueConverted()
                        };

                        CriteresVm c2 = new ViewModel.CriteresVm
                        {
                            LibelleCritere = "C2",
                            ValeurMinCritere = listeCoefficientPonderation.Single(x => x.Code == "MinC2").ValeurMaconnerie.GetValueConverted(),
                            ValeurMaxCritere = listeCoefficientPonderation.Single(x => x.Code == "MaxC2").ValeurMaconnerie.GetValueConverted()
                        };

                        CriteresVm c3 = new ViewModel.CriteresVm
                        {
                            LibelleCritere = "C3",
                            ValeurMinCritere = listeCoefficientPonderation.Single(x => x.Code == "MinC3").ValeurMaconnerie.GetValueConverted(),
                            ValeurMaxCritere = listeCoefficientPonderation.Single(x => x.Code == "MaxC3").ValeurMaconnerie.GetValueConverted()
                        };

                        listeValeurMinMaxcritere.Add(c1);
                        listeValeurMinMaxcritere.Add(c2);
                        listeValeurMinMaxcritere.Add(c3);

                    }
                }
            }



            return listeValeurMinMaxcritere;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeCoefficientPonderation"></param>
        /// <returns></returns>
        private IEnumerable<ValeurCritereVm> ListeSeuilReferenceCriteres(IEnumerable<CoefficientPonderationDto> listeCoefficientPonderation)
        {
            var listeSeuilReferenceCritere = new List<ValeurCritereVm>();
            if (!string.IsNullOrEmpty(RouteData.Values["type"].ToString()))
            {
                var natureCalibrage = int.Parse(RouteData.Values["type"].ToString());
                if (natureCalibrage == (int)ENatureCalibrage.Maçonnerie)
                {
                    if (listeCoefficientPonderation != null && listeCoefficientPonderation.Any())
                    {
                        ValeurCritereVm valeurB0 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B0C1").ValeurMaconnerie.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B0C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B0C2").ValeurMaconnerie.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B0C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B0C3").ValeurMaconnerie.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B0C3").Id,
                            NatureValeur = "b0",
                            Color = Resource1.B0_Color

                        };

                        ValeurCritereVm valeurB1 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B1C1").ValeurMaconnerie.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B1C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B1C2").ValeurMaconnerie.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B1C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B1C3").ValeurMaconnerie.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B1C3").Id,
                            NatureValeur = "b1",
                            Color = Resource1.B1_Color
                        };

                        ValeurCritereVm valeurB2 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B2C1").ValeurMaconnerie.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B2C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B2C2").ValeurMaconnerie.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B2C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B2C3").ValeurMaconnerie.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B2C3").Id,
                            NatureValeur = "b2",
                            Color = Resource1.B2_Color
                        };

                        ValeurCritereVm valeurB3 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B3C1").ValeurMaconnerie.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B3C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B3C2").ValeurMaconnerie.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B3C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B3C3").ValeurMaconnerie.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B3C3").Id,
                            NatureValeur = "b3",
                            Color = Resource1.B3_Color
                        };

                        ValeurCritereVm valeurB4 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B4C1").ValeurMaconnerie.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B4C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B4C2").ValeurMaconnerie.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B4C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B4C3").ValeurMaconnerie.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B4C3").Id,
                            NatureValeur = "b4"
                        };

                        listeSeuilReferenceCritere.Add(valeurB4);
                        listeSeuilReferenceCritere.Add(valeurB3);
                        listeSeuilReferenceCritere.Add(valeurB2);
                        listeSeuilReferenceCritere.Add(valeurB1);
                        listeSeuilReferenceCritere.Add(valeurB0);
                    }
                }

                if (natureCalibrage == (int)ENatureCalibrage.Enduit)
                {
                    if (listeCoefficientPonderation != null && listeCoefficientPonderation.Any())
                    {
                        ValeurCritereVm valeurB0 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B0C1").ValeurEnduit.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B0C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B0C2").ValeurEnduit.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B0C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B0C3").ValeurEnduit.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B0C3").Id,
                            NatureValeur = "b0",
                            Color = Resource1.B0_Color

                        };

                        ValeurCritereVm valeurB1 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B1C1").ValeurEnduit.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B1C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B1C2").ValeurEnduit.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B1C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B1C3").ValeurEnduit.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B1C3").Id,
                            NatureValeur = "b1",
                            Color = Resource1.B1_Color
                        };

                        ValeurCritereVm valeurB2 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B2C1").ValeurEnduit.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B2C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B2C2").ValeurEnduit.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B2C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B2C3").ValeurEnduit.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B2C3").Id,
                            NatureValeur = "b2",
                            Color = Resource1.B2_Color
                        };

                        ValeurCritereVm valeurB3 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B3C1").ValeurEnduit.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B3C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B3C2").ValeurEnduit.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B3C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B3C3").ValeurEnduit.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B3C3").Id,
                            NatureValeur = "b3",
                            Color = Resource1.B3_Color
                        };

                        ValeurCritereVm valeurB4 = new ViewModel.ValeurCritereVm
                        {
                            C1 = listeCoefficientPonderation.Single(x => x.Code == "B4C1").ValeurEnduit.GetValueConverted(),
                            IdC1 = listeCoefficientPonderation.Single(x => x.Code == "B4C1").Id,
                            C2 = listeCoefficientPonderation.Single(x => x.Code == "B4C2").ValeurEnduit.GetValueConverted(),
                            IdC2 = listeCoefficientPonderation.Single(x => x.Code == "B4C2").Id,
                            C3 = listeCoefficientPonderation.Single(x => x.Code == "B4C3").ValeurEnduit.GetValueConverted(),
                            IdC3 = listeCoefficientPonderation.Single(x => x.Code == "B4C3").Id,
                            NatureValeur = "b4"
                        };

                        listeSeuilReferenceCritere.Add(valeurB4);
                        listeSeuilReferenceCritere.Add(valeurB3);
                        listeSeuilReferenceCritere.Add(valeurB2);
                        listeSeuilReferenceCritere.Add(valeurB1);
                        listeSeuilReferenceCritere.Add(valeurB0);
                    }
                }
            }

            return listeSeuilReferenceCritere;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        private string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression).Member.Name;
        }

        /// <summary>
        /// Méthode permettant de converter une chaine de caractère to decimal
        /// </summary>
        /// <param name="input">valeur to converter</param>
        /// <returns>valeur converted</returns>
        private decimal? GetValueDecimalConverted(string input)
        {
            if (input != null)
            {
                decimal output;
                if (decimal.TryParse(input.Replace('.', ','), out output))
                {
                    return output;
                }
            }

            return null;
        }

        /// <summary>
        /// Méthode êrmettant de valider la propriete largeur du tunnel
        /// </summary>
        /// <param name="model">model à valider</param>
        /// <returns>true si la vérification est bonne</returns>
        private bool IsValideModelEnduitMaconnerie(CoefficientPonderationVm model)
        {
            if (model.NatureCalibrage == ENatureCalibrage.Enduit)
            {
                if (this.GetValueDecimalConverted(model.CpA7) < 0)
                {
                    ModelState.AddModelError("CpA7", MessagesValidationModel.MsgErrValidationEnduit);
                    return false;
                }
            }
            else
            {
                if (this.GetValueDecimalConverted(model.CpA7) < 1)
                {
                    ModelState.AddModelError("CpA7", MessagesValidationModel.MsgErrValidationMaçonnerie);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Méthode permettant de valider la règle cp1 = cp2 = cp3 = cp4 = 0
        /// </summary>
        /// <param name="model">model à valider</param>
        /// <returns>true si la validation est respectée</returns>
        private bool IsValideSommeValeurCritere(CoefficientPonderationVm model)
        {
            if ((int)this.GetValueDecimalConverted(model.Cp1) == 0 && (int)this.GetValueDecimalConverted(model.Cp2) == 0 && (int)this.GetValueDecimalConverted(model.Cp3) == 0 && (int)this.GetValueDecimalConverted(model.Cp4) == 0)
            {
                this.ModelState.AddModelError("somme", Resource1.MsgErrValidationSommeCriteres);
                return false;
            }

            return true;

        }

    }
}