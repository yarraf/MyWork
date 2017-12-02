using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Enum;
using Ratp.Hidalgo.Web.Const;
using Ratp.Hidalgo.Web.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ratp.Hidalgo.Web.ViewModel
{
    public class CoefficientPonderationVm
    {
        public CoefficientPonderationDto _Dto;

        public CoefficientPonderationVm(CoefficientPonderationDto dto)
        {
            this._Dto = dto;
        }

        public CoefficientPonderationVm()
        { }

        /// <summary>
        /// Obtient la propriete la nature de calibrage il s'agit de Maçonnerie & Enduit
        /// </summary>
        public ENatureCalibrage NatureCalibrage { get; set; }

        [DisplayName("Désordres Fissuration, Infiltration, Désordres sur structure maçonnerie / béton cp1.01")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpA1 { get; set; }

        [DisplayName("Date de construction cp1.04")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpA4 { get; set; }

        [DisplayName("Hist. de régéné. de maçonnerie cp1.05 ")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpA5 { get; set; }

        [DisplayName("Hist. d’opé. d’entre: Enduit cp1.06 ")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpA6 { get; set; }

        [DisplayName("Largeur du tunnel cp1.07")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [ValidationEnduit(ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpA7 { get; set; }

        [DisplayName("Agres. chimi. du terrain encaiss cp1.09 ")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpA9 { get; set; }

        [DisplayName("Solubilité du terrain encaissant cp1.10")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpA10 { get; set; }

        [DisplayName("Pourrissement du bois de blindage cp1.11")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpA11 { get; set; }

        [DisplayName("Fréquentation cp2.01")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpB1 { get; set; }

        [DisplayName("Correspondance cp2.02")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpB2 { get; set; }

        [DisplayName("Périmètre L2–L6 cp2.03")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpB3 { get; set; }

        [DisplayName("Image RATP cp2.04")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string CpB4 { get; set; }


        [DisplayName("Performance (C1) cp1")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string Cp1 { get; set; }

        [DisplayName("Impact de la défaillance (C2) cp2")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string Cp2 { get; set; }

        [DisplayName("Opportunité de travaux (C3) cp3")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string Cp3 { get; set; }

        [DisplayName("Groupement de travaux (C4) cp4")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [Range(0, 99999.99, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        public string Cp4 { get; set; }

        [DisplayName("Seuil de veto critère « Performance » v1")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [ValidationEntier(10, ErrorMessage = MessagesValidationModel.MsgErrValidationStandard)]
        [RangeValidation(1, 5, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        [ValidationZero(false, true, ErrorMessage = MessagesValidationModel.MsgErrValidationV)]
        public string V1 { get; set; }

        [DisplayName("Durée de vie (ans)")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [ValidationEntier(10, ErrorMessage = MessagesValidationModel.MsgErrValidationStandard)]
        [RangeValidation(1, 5, ErrorMessage = MessagesValidationModel.MsgErrValidationRange)]
        [ValidationZero(true, false, ErrorMessage = MessagesValidationModel.MsgErrValidationStandard)]
        public string Dv { get; set; }

        [DisplayName("Programme théorique (m²)")]
        [Required(ErrorMessage = MessagesValidationModel.MsgErrValidationRequired)]
        [ValidationEntier(10, ErrorMessage = MessagesValidationModel.MsgErrValidationStandard)]
        [RangeValidation(1, 10, ErrorMessage = MessagesValidationModel.MsgErrValidationRangeEntier)]
        [ValidationZero(true, false, ErrorMessage = MessagesValidationModel.MsgErrValidationStandard)]
        public string Pt { get; set; }

        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }

        public IEnumerable<CriteresVm> ListeValeurMinMaxCriteres { get; set; }

        public IEnumerable<ValeurCritereVm> ListeSeuilReferenceCriteres { get; set; }
    }
}