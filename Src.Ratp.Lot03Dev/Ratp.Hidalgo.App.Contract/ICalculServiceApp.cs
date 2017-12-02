using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Data.Contract.Enum;
using System.Collections.Generic;

namespace Ratp.Hidalgo.App.Contract
{
    /// <summary>
    /// Interface pour gérer les lignes
    /// </summary>
    public interface ICalculServiceApp
    {
        /// <summary>
        /// Méthode permettant d'obtenir la liste des lignes gestionnaire
        /// </summary>
        /// <returns>Lignes trouvéess</returns>
        IEnumerable<LigneDto> GetAllLigneByFamille();

        /// <summary>
        /// Méthode permettant d'obtenir la liste des lieux gestionnaire par Lignes gestionnaire
        /// </summary>
        /// <param name="idLigneGestionnaire">L'identifiant de la Lignes gestionnaire</param>
        /// <returns>lieux trouvés</returns>
        IEnumerable<LieuDto> GetAllLieuxByLigneGestionnaire(int idLigneGestionnaire);

        /// <summary>
        /// Obtient les natures travaux externes
        /// </summary>
        /// <returns>liste des natures travaux externes</returns>
        IEnumerable<NatureTravauxExternesDto> GetAllNatureTravauxExternes();

        /// <summary>
        /// Obtient la liste des documents PGE
        /// </summary>
        /// <param name="natureCalibrage">Nature calibrage il s'agit de Maconnerie/Enduit</param>
        /// <returns>les documents pge trouvés</returns>
        IEnumerable<Documents> GetAllDocumentPge(ENatureCalibrage natureCalibrage);

        void Save(ProgrammationDto newProgrammationsDto);

        /// <summary>
        /// Obtient la liste des documents pge par programmation
        /// </summary>
        /// <param name="idProgrammation"></param>
        /// <returns></returns>
        IEnumerable<ProgrammationDocumentPgeDto> GetAllDocumentPgeByProgrammation(int idProgrammation);

        /// <summary>
        /// Modifier une programmation
        /// </summary>
        /// <param name="programmationDto"></param>
        void UpdateProgrammation(ProgrammationDto programmationDto);

        /// <summary>
        /// Modifier une programmation
        /// </summary>
        /// <param name="programmationDto"></param>
        void SaveProgrammationDocumentPge(IEnumerable<ProgrammationDocumentPgeDto> listProgrammationDocumentPgeDto);

        /// <summary>
        /// Obtient la liste de toutes les programmations 
        /// </summary>
        /// <param name="programmationDto"></param>
        IEnumerable<ProgrammationDto> GetAllProgrammation();

        /// <summary>
        /// Supprime la programmation  
        /// </summary>
        /// <param name="idProgrammation"></param>
        void RemoveOneProgrammation(int idProgrammation);

        /// <summary>
        /// Obtient la derniere programmation  
        /// </summary>
        /// <param name="idProgrammation"></param>
        ProgrammationDto GetProgrammationByNatureTypeAnnee(string natureTravaux, string typeOuvrage, int? anneeProgrammation);

        /// <summary>
        /// Obtient la liste des valeurs des parametres de chaque critere pour toutes les PGE données  
        /// </summary>
        /// <param name="listPGE"></param>
        Dictionary<ProgrammationDocumentPgeDto,IEnumerable<ValeursParametresPgeDto>> GetAllValeurParametreByPge(ProgrammationDocumentPgeDto[] listProgrammationDocumentPgeDto);

    }
}
