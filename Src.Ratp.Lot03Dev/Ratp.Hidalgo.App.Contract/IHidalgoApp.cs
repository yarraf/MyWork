using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Data.Contract.Enum;
using Ratp.Hidalgo.Data.Contract.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHidalgoApp
    {
        /// <summary>
        /// Obtient la liste des lignes
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lignes> GetAllLignes();

        /// <summary>
        /// Obtient la liste des lignes en mode asynchrone
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Lignes>> GetAllLignesAsync();

        #region Critere C1 : PERFORMANCE
        /// <summary>
        /// Obtient la liste des documents avec la note max des désordres appartenant à la famille Fissuration
        /// </summary>
        /// <param name="familleDesordre">Famille desordre</param>
        /// <param name="natureCalibrage">Nature Calibrage</param>
        /// <returns>Documents Pge</returns>
        IEnumerable<DocumentDto> GetNotesMaxForAllDocumentPge_Fissuration(EFamilleDesordre familleDesordre, ENatureCalibrage natureCalibrage);

        /// <summary>
        /// Obtient la liste des documents avec la note max des désordres appartenant à la famille Infultration
        /// </summary>
        /// <param name="familleDesordre">famille desordre</param>
        /// <param name="natureCalibrage">Nature Calibrage il s'agit de maconnerie/enduit</param>
        /// <returns>Documents Pge</returns>
        IEnumerable<DocumentDto> GetNotesMaxForAllDocumentPge_Infultration(EFamilleDesordre familleDesordre, ENatureCalibrage natureCalibrage);

        /// <summary>
        /// Obtient la liste des documents avec la note max des désordres appartenant à la famille Desordre nsur Structure Maconnerie/Beton
        /// </summary>
        /// <param name="familleDesordre">famille desordre</param>
        /// <param name="natureCalibrage">Nature Calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>Documents Pge</returns>
        IEnumerable<DocumentDto> GetNotesMaxForAllDocumentPge_DesordreStructureMaconnerieBeton(EFamilleDesordre familleDesordre, ENatureCalibrage natureCalibrage);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre l'age de l'ouvrage
        /// </summary>
        /// <param name="anneeProgrammation">l'année de programmation</param>
        /// <param name="natuteCalibrage">Nature calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Ratp.Hidalgo.App.Contract.DTO.DocumentDto> GetNoteMaxForAllDocumentPge_AgeOuvrage(int anneeProgrammation, ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Historique de régénération de maçonnerie
        /// </summary>
        /// <param name="anneeProgrammation">l'année de programmation</param>
        /// <param name="natuteCalibrage">Nature calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Ratp.Hidalgo.App.Contract.DTO.DocumentDto> GetNoteMaxForAllDocumentPge_HistoriqueRegenerationMaconnerie(int anneeProgrammation, ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Historique de régénération de Enduit
        /// </summary>
        /// <param name="anneeProgrammation">l'année de programmation</param>
        /// <param name="natuteCalibrage">Nature calibrage il s'agit de maconnerie/Ensuit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Ratp.Hidalgo.App.Contract.DTO.DocumentDto> GetNoteMaxForAllDocumentPge_HistoriqueRegenerationEnduit(int anneeProgrammation, ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre largeur de l'ouvrage
        /// </summary>
        /// <param name="natureCalibrage">Nature Calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Ratp.Hidalgo.App.Contract.DTO.DocumentDto> GetNoteMaxForAllDocumentPge_LargeurOuvrage(ENatureCalibrage natureCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Agressivité chimique du terrain encaissant (p1.09)
        /// </summary>
        /// <param name="natureCalibrage"> Nature Calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Ratp.Hidalgo.App.Contract.DTO.DocumentDto> GetNoteMaxForAllDocumentPge_AgressiviteChimiqueTerrainEncaissant(ENatureCalibrage natureCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Solubilité du terrain
        /// </summary>
        /// <param name="natureCalibrage">Nature Travaux il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Ratp.Hidalgo.App.Contract.DTO.DocumentDto> GetNoteMaxForAllDocumentPge_SolubiliteTerrain(ENatureCalibrage natureCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Pourrissement du bois de blindage (p1.11)
        /// </summary>
        /// <param name="natureCalibrage">Nature Calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Ratp.Hidalgo.App.Contract.DTO.DocumentDto> GetNoteMaxForAllDocumentPge_PourrissementBoisBlindage(ENatureCalibrage natureCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des PGE avec la valeur de critère calculé
        /// </summary>
        /// <param name="programmationdto">La programmation</param>
        /// <returns>PGE avec critère</returns>
        IEnumerable<CritereDto> CalculCritereC1(ProgrammationDto programmationdto, IEnumerable<Documents> listeDocumentPge);
        #endregion

        #region Critere C2 : IMPCAT DE LA DEFAILLANCE
        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Fréquentation
        /// </summary>
        /// <param name="natuteCalibrage">Nature calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_Frequentation(ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Correspondance
        /// </summary>
        /// <param name="natuteCalibrage">Nature calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_Correspondance(ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Périmètre L2 - L6
        /// </summary>
        /// <param name="natuteCalibrage">Nature calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_PerimetreL2L6(ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Image RATP
        /// </summary>
        /// <param name="natuteCalibrage">Nature calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_ImageRatp(ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des PGE avec la valeur de critère calculé
        /// </summary>
        /// <param name="programmationdto">La programmation</param>
        /// <returns>PGE avec critère</returns>
        IEnumerable<CritereDto> CalculCritereC2(ProgrammationDto programmationdto, IEnumerable<Documents> listeDocumentPge);
        #endregion

        #region C3:  OPPORTUNITE DE TRAVAUX
        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Politique de rénovation
        /// </summary>
        /// <param name="programmationDto">l'objet programmation</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_PolitiqueRenivation(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre Travaux Externes
        /// </summary>
        /// <param name="programmationDto">L'objet de programmation</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_TravauxExternes(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des PGE avec la valeur de critère calculé
        /// </summary>
        /// <param name="programmationdto">La programmation</param>
        /// <returns>PGE avec critère</returns>
        IEnumerable<CritereDto> CalculCritereC3(ProgrammationDto programmationdto, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Calcul pessimiste
        /// </summary>
        /// <param name="programmationDto">l'objet programmation </param>
        /// <returns>Liste des Pge avec la catégorie classé</returns>
        IEnumerable<CategoriePgeDto> CalculPessimisteOptimiste(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge);
        #endregion

        #region C4: GROUPEMENT DE TRAVAUX 
        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre  Travaux connexes 
        /// </summary>
        /// <param name="programmationDto">Programe à calculé</param>
        /// <returns>les documents pge trouvés</returns>
        //IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_TravauxConnexes(ProgrammationDto programmationDto);

        /// <summary>
        /// Obtient la liste des documents PGE pour le paramètre  Travaux internes 
        /// </summary>
        /// <param name="programmationDto">l'objet à calculer</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_TravauxInternes(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtient la liste des PGE avec la valeur de critère calculé
        /// </summary>
        /// <param name="programmationdto">La programmation</param>
        /// <returns>PGE avec critère</returns>
        IEnumerable<CritereDto> CalculCritereC4(ProgrammationDto programmationdto, IEnumerable<Documents> listeDocumentPge);

        /// <summary>
        /// Obtenir la note des paramètres du critère C4
        /// </summary>
        /// <param name="programmationdto">le programme à calculer</param>
        void CalculNotesParametresCritere4(ProgrammationDto programmationdto, IEnumerable<Documents> listeDocumentPge);
        #endregion

        #region ETAPE CALCUl 2 
        /// <summary>
        /// Calcul des indices de crédibilité pour l'étape 2 du calcul
        /// </summary>
        /// <param name="programmationDto">programme à calculer </param>
        /// <returns>liste des Pge classé par preordre final & median</returns>
        int CalculEtape2(ProgrammationDto programmationDto);

        /// <summary>
        /// Calcul de DISTILLATION DESCENDANTE
        /// </summary>
        /// <param name="listeDocumentsPge"></param>
        /// <returns></returns>
        IList<ClassementDistillationDto> CalculDistillationDescendante(IList<Documents> listeDocumentsPge);

        /// <summary>
        /// CALCUL DE DISTILLATION ASCENDANTE
        /// </summary>
        /// <param name="listeDocumentsPge"></param>
        /// <returns></returns>
        IList<ClassementDistillationDto> CalculDistillationAscendante(IList<Documents> listeDocumentsPge);

        /// <summary>
        /// Obtient la liste des documents de type pge
        /// </summary>
        /// <param name="natureTravaux">La nature travaux il s'agit de deux valeurs Maconnerie/Enduit</param>
        /// <returns>liste des Documents de type pge</returns>
        IEnumerable<Documents> GetAllDocumentPgeByNatureTravaux(ENatureCalibrage natureTravaux);
        #endregion

        /// <summary>
        /// Obtient la liste des types d'ouvrages
        /// </summary>
        /// <returns>types d'ouvrages trouvés</returns>
        IEnumerable<TypeOuvragesDto> GetAllTypeOuvrages();
    }
}
