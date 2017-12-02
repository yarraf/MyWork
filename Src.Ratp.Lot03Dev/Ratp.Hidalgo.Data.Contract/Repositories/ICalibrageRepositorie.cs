using log4net;
using Ratp.Hidalgo.Data.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.Data.Contract.Repositories
{
    /// <summary>
    /// Repositorie de la partie calibrage, 
    /// </summary>
    public interface ICalibrageRepositorie
    {
        /// <summary>
        /// Obtient la liste des coefficients de pondération des paramètres de l'interface calibrage
        /// </summary>
        /// <returns>Liste des Coefficients de pondération</returns>
        IEnumerable<CoefficientPonderationParametresCriteresCalibrage> GetAllCoefficientPonderation();

        CoefficientPonderationParametresCriteresCalibrage GetOneCoefficientPonderationById(int id);

        void UpdateCoefficientPonderationParametresMaçonnerie(CoefficientPonderationParametresCriteresCalibrage coefficientPonderation);
        void UpdateCoefficientPonderationParametresEnduit(CoefficientPonderationParametresCriteresCalibrage coefficientPonderation);
        void UpdateCoefficientPonderationParametreMaconnerie(CoefficientPonderationParametresCriteresCalibrage coefficientPonderationParametre);
        void UpdateCoefficientPonderationParametreEnduit(CoefficientPonderationParametresCriteresCalibrage coefficientPonderationParametre);
    }
}
