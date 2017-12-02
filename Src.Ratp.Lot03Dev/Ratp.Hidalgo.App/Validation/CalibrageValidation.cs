using log4net;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.App.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Validation
{
    /// <summary>
    /// Classe de validaation des règles de saisie des paramètres pondération
    /// </summary>
    public class CalibrageValidation
    {
        /// <summary>
        /// Fiels pour stocker l'instance 
        /// </summary>
        private IEnumerable<CoefficientPonderationDto> ListeCoefficientPonderation;

        /// <summary>
        /// Field pour stocker l'information de ILogger
        /// </summary>
        private readonly ILog Logger;

        /// <summary>
        /// Initialisation des paramètre
        /// </summary>
        /// <param name="listeCoefficientPonderation">liste des paramètres de pondération</param>
        public CalibrageValidation(IEnumerable<CoefficientPonderationDto> listeCoefficientPonderation, ILog logger)
        {
            this.ListeCoefficientPonderation = listeCoefficientPonderation;
            this.Logger = logger;
        }

        /// <summary>
        /// Méthode de validation de la règle somme des critères appartenant à la nature Maàonnerie# 0
        /// </summary>
        /// <returns>true si la validation est bonne</returns>
        public bool IsValideSommeCritereMaconnerie()
        {
            //Vérifier la règle cp1 = cp2 = cp3 = cp4 = 0
            if (this.ListeCoefficientPonderation.Any())
            {
                bool cp1 = (int)ListeCoefficientPonderation.Where(x => x.Code == "Cp1").Single().ValeurMaconnerie == 0;
                bool cp2 = (int)ListeCoefficientPonderation.Where(x => x.Code == "Cp2").Single().ValeurMaconnerie == 0;
                bool cp3 = (int)ListeCoefficientPonderation.Where(x => x.Code == "Cp3").Single().ValeurMaconnerie == 0;
                bool cp4 = (int)ListeCoefficientPonderation.Where(x => x.Code == "Cp4").Single().ValeurMaconnerie == 0;

                if (cp1 && cp2 && cp3 && cp4)
                {
                    this.Logger.Error(Resource1.MsrErrValidationSommeCritere);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Méthode de validation de la règle somme des critères appartenant à la nature Enduit# 0
        /// </summary>
        /// <returns>true si la validation est bonne</returns>
        public bool IsValideSommeCritereEnduit()
        {
            //Vérifier la règle cp1 = cp2 = cp3 = cp4 = 0
            if (this.ListeCoefficientPonderation.Any())
            {
                bool cp1 = (int)ListeCoefficientPonderation.Where(x => x.Code == "Cp1").Single().ValeurEnduit == 0;
                bool cp2 = (int)ListeCoefficientPonderation.Where(x => x.Code == "Cp2").Single().ValeurEnduit == 0;
                bool cp3 = (int)ListeCoefficientPonderation.Where(x => x.Code == "Cp3").Single().ValeurEnduit == 0;
                bool cp4 = (int)ListeCoefficientPonderation.Where(x => x.Code == "Cp4").Single().ValeurEnduit == 0;

                if (cp1 && cp2 && cp3 && cp4)
                {
                    this.Logger.Error(Resource1.MsrErrValidationSommeCritere);
                    return false;
                }
            }

            return true;
        }
    }
}
