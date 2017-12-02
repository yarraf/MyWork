using Ratp.Hidalgo.Data.Contract.Entities;
using System.Collections.Generic;

namespace Ratp.Hidalgo.Data.Contract.Repositories
{
    /// <summary>
    /// interface pour gérer des lignes gestionnaires
    /// </summary>
    public interface ILigneRepositorie
    {
        /// <summary>
        /// Méthode permettant d'obtenir la liste des lignes de ma famille RER et Métro
        /// </summary>
        /// <returns>ligne trouvées</returns>
        IEnumerable<Lignes> GetAllLigneByFamille();

        /// <summary>
        /// Obtient la liste des lieux gestionnaire par la ligne gestionnaires séelectionnée
        /// </summary>
        /// <param name="idLigneGestionnaire">la ligne selectionnee</param>
        /// <returns>les lieux trouvés</returns>
        IEnumerable<Lieux> GetAllLieuxByLigne(int idLigneGestionnaire);

        /// <summary>
        /// Obtient la liste des natures travaux externes
        /// </summary>
        /// <returns>Natures travaux externes trouvées</returns>
        IEnumerable<NatureTravauxExternes> GetAllNatureTravauxExternes();
    }
}
