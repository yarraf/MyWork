using Ratp.Hidalgo.App.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract;
using Ratp.Hidalgo.Data.Contract.Entities;
using Core.Common.Mapping;
using Ratp.Hidalgo.App.Mapping;
using Ratp.Hidalgo.App.Validation;
using log4net;
using Ratp.Hidalgo.App.Properties;
using System.ComponentModel.DataAnnotations;

namespace Ratp.Hidalgo.App
{
    /// <summary>
    /// Implémentation de le contrat de type <see cref="ICalibrageApp"/>
    /// </summary>
    public class CalibrageApp : ICalibrageApp
    {
        /// <summary>
        /// Field pour stocker l'information de l'unit of work du module Hidalgo 
        /// </summary>
        private IHidalgoUnitOfWork _uow;

        /// <summary>
        /// Field pour stocker l'information de ILogger
        /// </summary>
        private readonly ILog Logger;

        /// <summary>
        /// Initialisation 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public CalibrageApp(IHidalgoUnitOfWork unitOfWork)
        {
            this._uow = unitOfWork;
            this.Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <inheritdoc />
        public IEnumerable<CoefficientPonderationDto> GetAllCoeffcientPonderations()
        {
            IEnumerable<CoefficientPonderationDto> result = null;

            var listeCoefficientPonderation = this._uow.CalibrageRepositorie.GetAllCoefficientPonderation();
            this.Logger.InfoFormat("Début:    CalibrageApp.GetAllCoeffcientPonderations Appel au mapping");
            IMapper<CoefficientPonderationParametresCriteresCalibrage, CoefficientPonderationDto> mapper = new CoefficientPonderationParametresCriteresCalibrageToCoefficientPonderationDto();
            result = listeCoefficientPonderation.Select(mapper.Map).ToList();
            this.Logger.InfoFormat("Fin:    CalibrageApp.GetAllCoeffcientPonderations");

            return result;
        }

        /// <inheritdoc />
        public void UpdateCoefficientPonderationMaconnerie(CoefficientPonderationDto coefficientPonderationDto)
        {
            var listeCoefficientPonderation = this._uow.CalibrageRepositorie.GetAllCoefficientPonderation();
            var coefficientPonderationMetier = listeCoefficientPonderation.Where(x => x.Code.ToLower() == coefficientPonderationDto.Code.ToLower()).FirstOrDefault();
            if (coefficientPonderationMetier != null)
            {
                coefficientPonderationDto.Id = coefficientPonderationMetier.Id;
                coefficientPonderationDto.Libelle = coefficientPonderationDto.Libelle;
                coefficientPonderationDto.Code = coefficientPonderationDto.Code;
            }

            IMapper<CoefficientPonderationDto, CoefficientPonderationParametresCriteresCalibrage> mapper = new CoefficientPonderationParametresCriteresCalibrageToCoefficientPonderationDto();
            this.Logger.InfoFormat(string.Format("Modification de la valeur du paramètre coefficient de pondération {0}", coefficientPonderationDto.Code));
            this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(mapper.Map(coefficientPonderationDto));
            this.Logger.InfoFormat(string.Format("La valeur duparamètre {0} est modifié avec succès", coefficientPonderationDto.Code));
        }

        /// <inheritdoc />
        public void UpdateCoefficientPonderationEnduit(CoefficientPonderationDto coefficientPonderationDto)
        {
            var listeCoefficientPonderation = this._uow.CalibrageRepositorie.GetAllCoefficientPonderation();
            var coefficientPonderationMetier = listeCoefficientPonderation.Where(x => x.Code.ToLower() == coefficientPonderationDto.Code.ToLower()).FirstOrDefault();
            if (coefficientPonderationMetier != null)
            {
                coefficientPonderationDto.Id = coefficientPonderationMetier.Id;
                coefficientPonderationDto.Libelle = coefficientPonderationDto.Libelle;
                coefficientPonderationDto.Code = coefficientPonderationDto.Code;
            }

            IMapper<CoefficientPonderationDto, CoefficientPonderationParametresCriteresCalibrage> mapper = new CoefficientPonderationParametresCriteresCalibrageToCoefficientPonderationDto();
            this.Logger.InfoFormat("Début:    CalibrageApp.UpdateCoefficientPonderationEnduit");
            this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(mapper.Map(coefficientPonderationDto));
            this.Logger.InfoFormat("Fin:    CalibrageApp.UpdateCoefficientPonderationEnduit");
        }

        /// <inheritdoc />
        public void UpdateListeCoefficientsPonderationMaçonnerie(IEnumerable<CoefficientPonderationDto> listeCoefficientPonderation)
        {
            string messageErreur;
            IMapper<CoefficientPonderationDto, CoefficientPonderationParametresCriteresCalibrage> mapper = new CoefficientPonderationParametresCriteresCalibrageToCoefficientPonderationDto();
            if (listeCoefficientPonderation == null)
            {
                throw new ArgumentNullException(string.Format(Resource1.MsgErrArgumentNull, listeCoefficientPonderation));
            }

            //Validation de la règle
            CalibrageValidation validationCalibrage = new Validation.CalibrageValidation(listeCoefficientPonderation, this.Logger);
            if (!validationCalibrage.IsValideSommeCritereEnduit())
            {
                throw new ValidationException(Resource1.MsrErrValidationSommeCritere);
            }

            foreach (var item in listeCoefficientPonderation)
            {
                var objMetier = mapper.Map(item);

                if (ValidationDecimalMax.ValidateDecimal(objMetier.ValeurMaconnerie, objMetier.Code, out messageErreur))
                {
                    this.Logger.InfoFormat("Début:    CalibrageApp.UpdateListeCoefficientsPonderation");
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametresMaçonnerie(objMetier);
                    this.Logger.InfoFormat("Fin:    CalibrageApp.UpdateListeCoefficientsPonderation");
                }
                else
                {
                    throw new Exception(messageErreur);
                }
            }
        }

        /// <inheritdoc />
        public void UpdateListeCoefficientsPonderationEnduit(IEnumerable<CoefficientPonderationDto> listeCoefficientPonderation)
        {
            string messageErreur;

            //Vérifier si l'arguùent est null
            if (listeCoefficientPonderation == null)
            {
                this.Logger.Error(string.Format(Resource1.MsgErrArgumentNull, listeCoefficientPonderation));
                throw new ArgumentNullException(string.Format(Resource1.MsgErrArgumentNull, listeCoefficientPonderation));
            }

            //Validation de la règle
            CalibrageValidation validationCalibrage = new Validation.CalibrageValidation(listeCoefficientPonderation, this.Logger);
            if (!validationCalibrage.IsValideSommeCritereEnduit())
            {
                throw new ValidationException(Resource1.MsrErrValidationSommeCritere);
            }

            IMapper<CoefficientPonderationDto, CoefficientPonderationParametresCriteresCalibrage> mapper = new CoefficientPonderationParametresCriteresCalibrageToCoefficientPonderationDto();
            foreach (var item in listeCoefficientPonderation)
            {
                var objMetier = mapper.Map(item);

                if (ValidationDecimalMax.ValidateDecimal(objMetier.ValeurEnduit, objMetier.Code, out messageErreur))
                {
                    this.Logger.InfoFormat("Début:    CalibrageApp.UpdateListeCoefficientsPonderation");
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametresEnduit(objMetier);
                    this.Logger.InfoFormat("Fin:    CalibrageApp.UpdateListeCoefficientsPonderation");
                }
                else
                {
                    throw new Exception(messageErreur);
                }
            }
        }
    }
}