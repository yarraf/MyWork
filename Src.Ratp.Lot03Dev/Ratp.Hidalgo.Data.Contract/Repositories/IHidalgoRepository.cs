using Ratp.Hidalgo.Data.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.Data.Contract.Repositories
{
    /// <summary>
    /// interface repository pour accès au base de données
    /// </summary>
    public interface IHidalgoRepository : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lignes> GetAllLignes();

        /// <summary>
        /// Obtient une liste des lignes en mode asynchrone
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Lignes>> GetAllLignesAsync();

        /// <summary>
        /// Obtient la liste des documents PGE
        /// </summary>
        /// <returns>Documents Pge</returns>
        IEnumerable<Documents> GetAllDocumentPge();

        /// <summary>
        /// Obtient la liste des désordres par document PGE
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns>Documents Pge</returns>
        IEnumerable<Desordres> GetAllDesordresByDocument(int idDocument);

        /// <summary>
        /// Obtient la liste des types d'ouvrages
        /// </summary>
        /// <returns>Types Ouvrage</returns>
        IEnumerable<TypesOuvrages> GetAllTypeOuvrages();

        /// <summary>
        /// Sauvegarder une programmation
        /// </summary>
        /// <param name="newProgrammation"></param>
        /// <returns></returns>
        int SaveProgrammation(Programmations newProgrammation);

        /// <summary>
        /// Obtient la liste des PGE calculé par programmation
        /// </summary>
        /// <param name="idProgrammation"></param>
        /// <returns></returns>
        IEnumerable<ProgrammationDocumentPGE> GetAllDocumentPgeByProgrammation(int idProgrammation);

        ProcesVerbaux GetPvByDocumentPge(string numAffaire);

        /// <summary>
        /// Modifier la programmation
        /// </summary>
        /// <param name="programmation">la programmation à modifier </param>
        void UpdateProgrammation(Programmations programmation);

        /// <summary>
        /// Obtenir une programmation par son identifiant
        /// </summary>
        /// <param name="idProgrammation">l'id de la programmation</param>
        /// <returns>Programmation trouvée</returns>
        Programmations GetOneProgrammation(int idProgrammation);

        /// <summary>
        /// Obtenir une programmation par son identifiant
        /// </summary>
        /// <param name="typeOuvrage">Type d'ouvrage reherche</param>
        /// <param name="natureTravaux">Nature des travaux</param>
        /// <param name="anneeProgrammation">Annee de programation (facultatif)</param>
        /// <returns>Programmation trouvée</returns>
        Programmations GetOneProgrammationByTypeNatureAnnee(string typeOuvrage, string natureTravaux, int? anneeProgrammation);

        /// <summary>
        /// Obtenir la liste de toutes les programmations
        /// </summary>
        /// <returns>Programmations trouvées</returns>
        IEnumerable<Programmations> GetAllProgrammation();

        /// <summary>
        /// Supprime la programmation
        /// </summary>
        void RemoveOneProgrammation(Programmations programmation, IEnumerable<ProgrammationDetails> listProgrammationDetails, IEnumerable<ProgrammationDocumentPGE> listProgrammationDocumentPGE, IEnumerable<ProgrammationValeurParametresDocument> listProgrammationValeurParametresDocument);

        /// <summary>
        /// Obtenir la liste des ProgrammationDetails par la programmation
        /// </summary>
        /// <returns>ProgrammationDetails trouvées</returns>
        IEnumerable<ProgrammationDetails> GetAllProgrammationDetailsByProgrammation(int idProgrammation);

        /// <summary>
        /// Obtenir la somme de surface des ouvrages
        /// </summary>
        /// <param name="idOuvrage">l'identifiant de l'ouvrage</param>
        /// <returns>somme de surface des ouvrage</returns>
        double GetSurfaceOuvragesByPge(int idOuvrage);

        /// <summary>
        /// Ajoute la PGE à la base de données
        /// </summary>
        /// <param name="pge">la PGE à ajouter</param>
        /// <returns>PGE ajoutee</returns>
        ProgrammationDocumentPGE AddProgrammationDocumentPGE(ProgrammationDocumentPGE pge);

        /// <summary>
        /// Modifie la PGE à la base de données
        /// </summary>
        /// <param name="pge">la PGE à modifier</param>
        /// <returns>PGE modifiee</returns>
        void UpdateProgrammationDocumentPGE(ProgrammationDocumentPGE pge);

        /// <summary>
        /// Obtenir une programmationDocumentPge par son identifiant
        /// </summary>
        /// <param name="idProgrammationDocumentPge">l'id de la programmationDocumentPge</param>
        /// <returns>ProgrammationDocumentPge trouvée</returns>
        ProgrammationDocumentPGE GetOneProgrammationDocumentPge(int idProgrammationDocumentPge);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDocumentPge"></param>
        /// <returns></returns>
        IEnumerable<ProgrammationValeurParametresDocument> GetAllValeurParametreByPge(int idDocumentPge);

        /// <summary>
        /// Obtenir la liste des valeur des paramètres d'une programmation
        /// </summary>
        /// <param name="idProgrammation"></param>
        /// <returns></returns>
        IEnumerable<ProgrammationValeurParametresDocument> GetAllProgrammationValeurParametresDocument(int idProgrammation);

    }
}
