using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Web.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Ratp.Hidalgo.Data.Contract.Enum;
using System.ComponentModel.DataAnnotations;
using Ratp.Hidalgo.Web.Const;

namespace Ratp.Hidalgo.Web.ViewModel
{
    /// <summary>
    /// ViewModel pour gérer l'interface de la nouvelle programmation
    /// </summary>
    public class ProgrammationVm
    {
        private NatureCalibrage selectedNatureTravaux;

        public int Idprogrammation {
            get
            {
                return this.Programmation.Id;
            }
            set
            {
                this.Programmation.Id = value;
            }
        }

        public ProgrammationVm()
        {
            this.Programmation = new ProgrammationDto();
        }

        public ProgrammationVm(ProgrammationDto programmation)
        {
            this.Programmation = programmation;
        }

        public ProgrammationDto Programmation { get; set; }

        /// <summary>
        /// Obtient or définit l'année de programmation
        /// </summary>
        public int? AnneeProgrammation
        {
            get
            {
                return this.Programmation.Anneeprogrammation;
            }
            set
            {
                if (value != null)
                {
                    this.Programmation.Anneeprogrammation = value.Value;
                }
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des lignes gestionnaires
        /// </summary>
        public IEnumerable<LigneDto> LignesGestionnaires { get; set; }

        /// <summary>
        /// liste Ligne
        /// </summary>
        public IEnumerable<SelectListItem> Lignes { get; set; }

        /// <summary>
        /// liste Lieux gestionnaire
        /// </summary>
        public IEnumerable<SelectListItem> Lieux { get; set; }

        /// <summary>
        /// liste des annees
        /// </summary>
        public IEnumerable<SelectListItem> Annees { get; set; }

        [DisplayName("Ligne")]
        /// <summary>
        /// Obtient ou définit la ligne seléctionnée
        /// </summary>
        public int? SelectedLigne { get; set; }

        [DisplayName("Lieu")]
        /// <summary>
        /// Obtient ou définit le lieu seléctionné
        /// </summary>
        public int? SelectedLieu { get; set; }

        /// <summary>
        /// Obtient ou définit la propriete SelectedAnnee
        /// </summary>
        [DisplayName("Année")]
        /// <summary>
        /// Obtient ou définit l'annee
        /// </summary>
        public int? SelectedAnnee { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des RNM
        /// </summary>
        public IEnumerable<RnmVm> ListeRnm { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des travaux externes
        /// </summary>
        public IEnumerable<TravauxExternesVm> ListeTravauxExt { get; set; }

        [DisplayName("Type d'ouvrage")]
        /// <summary>
        /// Obtient ou définit le type d'ouvrage séléctionné
        /// </summary>
        public TypeOuvragesDto SelectedTypeOuvrage { get; set; }

        [DisplayName("Nature Travaux")]
        /// <summary>
        /// Obtient ou définit la natute travaux
        /// </summary>
        public NatureCalibrage SelectedNatureTravaux
        {
            get
            {
                return this.selectedNatureTravaux;
            }
            set
            {
                if (value != null)
                {
                    this.Programmation.NatureTravaux = (ENatureCalibrage)value.Id;
                }
            }
        }

        [DisplayName("Prix Unitaire des Travaux ")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 9999999999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange2)]
        public decimal? PrixUnitaireTravaux
        {
            get
            {
                return this.Programmation.PrixUnitaire;
            }

            set
            {
                this.Programmation.PrixUnitaire = value;
            }
        }

        [DisplayName("Budget disponible ")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 999999999999999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange3)]
        public decimal? BudgetDisponible
        {
            get
            {
                return this.Programmation.Budget;
            }

            set
            {
                this.Programmation.Budget = value;
            }
        }
    }
}