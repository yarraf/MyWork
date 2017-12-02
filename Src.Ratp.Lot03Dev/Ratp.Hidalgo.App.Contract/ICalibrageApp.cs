using Ratp.Hidalgo.App.Contract.DTO;
using System.Collections.Generic;

namespace Ratp.Hidalgo.App.Contract
{
    /// <summary>
    /// Contrat pour le module applicatif de calibrage
    /// </summary>
    public interface ICalibrageApp
    {
        /// <summary>
        /// Obtient la liste des coefficients pondération & paramètres & critères
        /// </summary>
        /// <returns>Liste des coefficient pondération</returns>
        IEnumerable<CoefficientPonderationDto> GetAllCoeffcientPonderations();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeCoefficientPonderation"></param>
        void UpdateListeCoefficientsPonderationMaçonnerie(IEnumerable<CoefficientPonderationDto> listeCoefficientPonderation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listeCoefficientPonderation"></param>
        void UpdateListeCoefficientsPonderationEnduit(IEnumerable<CoefficientPonderationDto> listeCoefficientPonderation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coefficientPonderationDto"></param>
        void UpdateCoefficientPonderationMaconnerie(CoefficientPonderationDto coefficientPonderationDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coefficientPonderationDto"></param>
        void UpdateCoefficientPonderationEnduit(CoefficientPonderationDto coefficientPonderationDto);

    }
}
