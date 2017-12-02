using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Data.Contract.Enum;
using System;
using System.Collections.Generic;

namespace Ratp.Hidalgo.Data.Contract.Repositories
{
    /// <summary>
    /// Repositorie de  CRITERE « PERFORMANCE » (C1)
    /// </summary>
    public interface ICriterePerformanceRepositorie
    {
        /// <summary>
        /// Obtient la note max des désordres par la famille désordre
        /// </summary>
        /// <param name="familleDesordre">Famille des desordres</param>
        /// <param name="natureCalibrage">Nature calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>Documents Pge</returns>
        IEnumerable<VDocumentPge> GetNoteMaxPgeByFamilleDesordre(EFamilleDesordre familleDesordre, ENatureCalibrage natureCalibrage);

        /// <summary>
        /// Obtient la liste des documents PGE
        /// </summary>
        /// <param name="typeNatureTravaux">le type nature travaux Maçonnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Documents> GetAllDocumentPge(ENatureCalibrage typeNatureTravaux);

        /// <summary>
        /// Obtient un ouvrage par son identifiant
        /// </summary>
        /// <param name="listeIdOuvrage">l'identifiant de l'ouvrage</param>
        /// <returns>l'ouvrage trouvé</returns>
        IEnumerable<Ouvrages> GetAllOuvragesById(IEnumerable<int> listeIdOuvrage);

        /// <summary>
        /// Obtient un ouvrage par son identifiant
        /// </summary>
        /// <param name="listeIdOuvrage">l'identifiant de l'ouvrage</param>
        /// <returns>l'ouvrage trouvé</returns>
        IEnumerable<DataAnnexe> GetAllDataAnnexeByIdOuvrage(IEnumerable<int> listeIdOuvrage);

        /// <summary>
        /// Obtient un ouvrage par son identifiant avec le chargement des lignes
        /// </summary>
        /// <param name="listeIdOuvrage">l'identifiant de l'ouvrage</param>
        /// <returns>l'ouvrage trouvé</returns>
        IEnumerable<Ouvrages> GetAllOuvragesWithInfoLignes(IEnumerable<int> listeIdOuvrage);
    }
}
