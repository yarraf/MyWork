using Ratp.Hidalgo.App.Contract;
using System.Collections.Generic;
using Ratp.Hidalgo.Data.Contract;
using System.Threading.Tasks;
using System.Linq;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Enum;
using Ratp.Hidalgo.App.Mapping;
using Core.Common.Mapping;
using Ratp.Hidalgo.Data.Contract.Entities;
using log4net;
using Core.Common.Core;
using System.Configuration;
using System;

namespace Ratp.Hidalgo.App
{
    /// <summary>
    /// Implémentation de l'interface IHidalgoApp
    /// </summary>
    /// 
    public class HidalgoApp : IHidalgoApp
    {
        private int rang = 0;
        private int classementPreordreFinal = 1;
        public static float seuilCoup = 0.65F;
        private Documents Pge_i { get; set; }
        private Documents Pge_k { get; set; }
        public CritereDto[] listeCritereC1 { get; private set; }
        public CritereDto[] listeCritereC2 { get; private set; }
        public CritereDto[] listeCritereC3 { get; private set; }
        public IList<Documents> ListePgeDocument { get; set; }
        public IEnumerable<DocumentDto> ListePgeFissuration { get; set; }
        public IEnumerable<DocumentDto> ListePgeInfiltration { get; set; }
        public IEnumerable<DocumentDto> ListePgeDesordreStructureMaconnerieBeton { get; set; }
        public IEnumerable<DocumentDto> ListePgeAgeOuvrage { get; set; }
        public IEnumerable<DocumentDto> ListePgeLargeurOuvrage { get; set; }
        public IEnumerable<DocumentDto> ListePgeHistoriqueRegenerationMaconnerie { get; set; }
        public IEnumerable<DocumentDto> ListePgeHistoriqueRegenerationEnduit { get; set; }
        public IEnumerable<DocumentDto> ListePgeAgressiviteChimiqueTerrainEncaissant { get; set; }
        public IEnumerable<DocumentDto> ListePgeSolibiliteTerrain { get; set; }
        public IEnumerable<DocumentDto> ListePgePourrissementBoisBlindage { get; set; }
        public IEnumerable<CoefficientPonderationParametresCriteresCalibrage> ListeCoefficientPonderation { get; set; }
        public IEnumerable<DocumentDto> ListePgeFrequentation { get; set; }
        public IEnumerable<DocumentDto> ListePgeCorrespondance { get; set; }
        public IEnumerable<DocumentDto> ListePgePerimetreL2L6 { get; set; }
        public IEnumerable<DocumentDto> ListePgeImageRatp { get; set; }
        public IEnumerable<DocumentDto> ListePgePolitiquerenovation { get; set; }
        public IEnumerable<DocumentDto> ListePgeTravauxExternes { get; set; }
        public IList<DocumentDto> ListePgeTravauxConnexes { get; set; }
        public IList<DocumentDto> ListePgeTravauxInternes { get; set; }
        public IList<CategoriePgeDto> ListeCategoriePge { get; set; }
        public Dictionary<string, float> ListeIndicesCridibilite { get; set; }
        public Dictionary<string, float> ListeIndicesDiscordance { get; set; }
        private Dictionary<string, float> listeIndiceCredibiliteDescendante;
        private Dictionary<string, float> listeIndiceCredibiliteAscendante;
        private Dictionary<string, char> listeSymbole = new Dictionary<string, char>();
        private IList<ClassementDistillationDto> listeClassementDistillationDescendante = new List<ClassementDistillationDto>();
        private IList<ClassementDistillationDto> listeClassementDistillationAscendante = new List<ClassementDistillationDto>();
        private IList<PreordreFinalDto> listePreordreFinal = new List<PreordreFinalDto>();
        private IList<Documents> listeOldPge = new List<Documents>();
        /// <summary>
        /// Field pour stocker l'instance de l'unitOfWork du Hidalgo
        /// </summary>
        private IHidalgoUnitOfWork _uow;

        /// <summary>
        /// Field pour stocker l'information de l'instance Logger
        /// </summary>
        private readonly ILog Logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uow"></param>
        public HidalgoApp(IHidalgoUnitOfWork uow)
        {
            _uow = uow;
            this.Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <inheritdoc />
        public IEnumerable<Lignes> GetAllLignes()
        {
            return _uow.HidalgoRepository.GetAllLignes();
        }

        public async Task<IEnumerable<Lignes>> GetAllLignesAsync()
        {
            var result = await _uow.HidalgoRepository.GetAllLignesAsync();
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetAllDocumentPge()
        {
            IList<DocumentDto> listeDocumentIhm = new List<DocumentDto>();

            //Récupérer la liste des documents PGE avec leur note Max
            var documentsPge = this._uow.HidalgoRepository.GetAllDocumentPge();
            if (documentsPge != null && documentsPge.Any())
            {


                //******************
                //foreach (var doc in documentsPge)
                //{
                //    var desordres = this._uow.HidalgoRepository.GetAllDesordresByDocument(doc.Id);
                //    if (desordres != null && desordres.Any())
                //    {
                //        int noteMax = (int)desordres.Max(x => x.NoteDesordre);

                //        if (!listeDocumentIhm.Any(x => x.NumeroAffaire == doc.NumeroAffaire))
                //        {
                //            listeDocumentIhm.Add(new DocumentIhm { NumeroAffaire = doc.NumeroAffaire, Note = noteMax });
                //        }
                //    }
                //}
            }

            return listeDocumentIhm;
        }

        #region  C1 : PERFORMANCE
        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNotesMaxForAllDocumentPge_Fissuration(EFamilleDesordre familleDesordre, ENatureCalibrage natureCalibrage)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", "Fissuration (p1.01)"));
            var documents = this._uow.CriterePerformanceRepositorie.GetNoteMaxPgeByFamilleDesordre(familleDesordre, natureCalibrage);

            //Mapping
            IMapper<VDocumentPge, DocumentDto> mapper = new CriteresMapping();

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(documents.Select(mapper.Map).ToList(), string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "Fissuration.xml"));
            return documents.Select(mapper.Map).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNotesMaxForAllDocumentPge_Infultration(EFamilleDesordre familleDesordre, ENatureCalibrage natureCalibrage)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", "Infiltration (p1.02)"));
            var documents = this._uow.CriterePerformanceRepositorie.GetNoteMaxPgeByFamilleDesordre(familleDesordre, natureCalibrage);

            //Mapping
            IMapper<VDocumentPge, DocumentDto> mapper = new CriteresMapping();

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(documents.Select(mapper.Map).ToList(), string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "Infultration.xml"));

            return documents.Select(mapper.Map).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNotesMaxForAllDocumentPge_DesordreStructureMaconnerieBeton(EFamilleDesordre familleDesordre, ENatureCalibrage natureCalibrage)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", "Désordre sur structure maçonnerie / béton (p1.03) "));
            var documents = this._uow.CriterePerformanceRepositorie.GetNoteMaxPgeByFamilleDesordre(familleDesordre, natureCalibrage);

            //Mapping
            IMapper<VDocumentPge, DocumentDto> mapper = new CriteresMapping();

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(documents.Select(mapper.Map).ToList(), string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "DesordreStructureMaconnerieBeton.xml"));

            return documents.Select(mapper.Map).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_AgeOuvrage(int anneeProgrammation, ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", " Age de l’ouvrage (p1.04) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllOuvragesById(listeIdOuvrage);
                        int anneeAncienne = DateTime.Now.Year;
                        int anneeCourant = 0;

                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                if (!string.IsNullOrEmpty(ouvrage.Descriptions.DateConstruction))
                                {
                                    //Exemple 1585
                                    if (ouvrage.Descriptions.DateConstruction.Length == 4)
                                    {
                                        anneeCourant = int.Parse(ouvrage.Descriptions.DateConstruction);
                                    }

                                    //01/1958
                                    if (ouvrage.Descriptions.DateConstruction.Length == 7)
                                    {
                                        anneeCourant = int.Parse(ouvrage.Descriptions.DateConstruction.Substring(3));
                                    }

                                    if (anneeAncienne > anneeCourant)
                                    {
                                        anneeAncienne = anneeCourant;
                                    }
                                }
                            }
                        }

                        documentDto.Note = this.GetNoteAgeOuvrage(anneeProgrammation - anneeAncienne);
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "AgeOuvrage.xml"));

            return listeDocumentDtoPge;
        }

        ///TODO YAR 22/05/2017 : en attente le développement du module gestion demande des travaux
        /// Importer les données depuis le fichier excel d'annexe
        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_HistoriqueRegenerationMaconnerie(int anneeProgrammation, ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", "  Historique de régénérations de maçonnerie (p1.05)  "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null && !listeIdOuvrage.Any(x => x == docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault()))
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        //Obtenir la liste des données du fichier annexe
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage);
                        int anneeAncienne = DateTime.Now.Year;
                        int anneeCourant = 0;
                        int note;

                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                if (!string.IsNullOrEmpty(ouvrage.HistoriqueConfortementMac_Annee))
                                {
                                    if (int.TryParse(ouvrage.HistoriqueConfortementMac_Annee, out note))
                                    {
                                        anneeCourant = note;
                                    }

                                    if (anneeAncienne > anneeCourant)
                                    {
                                        anneeAncienne = anneeCourant;
                                    }
                                }
                            }

                            documentDto.Note = this.GetNoteHistoriqueRegenerationMaconnerie(anneeProgrammation - anneeAncienne);
                        }

                        else
                        {
                            documentDto.Note = 0;
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "HistoriqueRegenerationMaconnerie.xml"));

            return listeDocumentDtoPge;
        }

        ///TODO YAR 22/05/2017 : en attente le développement du module gestion demande des travaux
        /// Importer les données depuis le fichier excel d'annexe
        /// <inheritdoc />

        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_HistoriqueRegenerationEnduit(int anneeProgrammation, ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", "  Historique de régénérations de enduit (p1.06)  "));

            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        //Obtenir la liste des données du fichier annexe
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage);
                        int anneeAncienne = DateTime.Now.Year;
                        int anneeCourant = 0;

                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                if (!string.IsNullOrEmpty(ouvrage.HistoriqueRefectionEnduit_Anne))
                                {
                                    if (int.TryParse(ouvrage.HistoriqueRefectionEnduit_Anne, out anneeCourant))
                                    {
                                        anneeCourant = int.Parse(ouvrage.HistoriqueRefectionEnduit_Anne);
                                    }

                                    if (anneeAncienne > anneeCourant)
                                    {
                                        anneeAncienne = anneeCourant;
                                    }
                                }
                            }

                            documentDto.Note = this.GetNoteHistoriqueRegenerationEnduit(anneeProgrammation - anneeAncienne);
                        }
                        else
                        {
                            documentDto.Note = 0;
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "HistoriqueRegenerationEnduit.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />           
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_LargeurOuvrage(ENatureCalibrage natureCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", "  Largeur de l’ouvrage (p1.07)  "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage.Distinct());
                        int noteMax = 0;
                        int note;
                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                if (!string.IsNullOrEmpty(ouvrage.LargeurOuvrage_Note))
                                {
                                    if (int.TryParse(ouvrage.LargeurOuvrage_Note, out note))
                                    {
                                        if (noteMax < note)
                                        {
                                            noteMax = note;
                                        }
                                    }
                                }
                            }

                            documentDto.Note = noteMax;
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "LargeurOuvrage.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_AgressiviteChimiqueTerrainEncaissant(ENatureCalibrage natureCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", "  Agressivité chimique du terrain encaissant (p1.09) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage);
                        int noteMax = 0;

                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                if (!string.IsNullOrEmpty(ouvrage.AgressiviteChimiqueTerrainEncaissant_Note))
                                {
                                    int outNote;

                                    if (int.TryParse(ouvrage.AgressiviteChimiqueTerrainEncaissant_Note, out outNote))
                                    {
                                        if (noteMax < outNote)
                                        {
                                            noteMax = outNote;
                                        }
                                    }
                                }
                            }

                            documentDto.Note = noteMax;
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "AgressiviteChimisueterrainEncaissant.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_SolubiliteTerrain(ENatureCalibrage natureCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", " Solubilité du terrain : présence de gypse (p1.10) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage);
                        int noteMax = 0;
                        int note;
                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                if (!string.IsNullOrEmpty(ouvrage.SolubiliteTerrain_Note))
                                {
                                    //retenir la note la plus elevés
                                    if (int.TryParse(ouvrage.SolubiliteTerrain_Note, out note))
                                    {
                                        if (noteMax < note)
                                        {
                                            noteMax = note;
                                        }
                                    }
                                }
                            }

                            documentDto.Note = noteMax;
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "SolubiliteTerrain.xml"));

            return listeDocumentDtoPge;
        }

        ///TODO YAR 23/05/2017 : à tester après l'importation des données du fichier annexe
        /// Importer les données depuis le fichier excel d'annexe
        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_PourrissementBoisBlindage(ENatureCalibrage natureCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C1: {0}", "Pourrissement du bois de blindage (p1.11) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage);
                        int noteMax = 0;

                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                if (!string.IsNullOrEmpty(ouvrage.PourrissementBoisBlindage_Note))
                                {
                                    int outNote;

                                    if (int.TryParse(ouvrage.PourrissementBoisBlindage_Note, out outNote))
                                    {
                                        if (noteMax < outNote)
                                        {
                                            noteMax = outNote;
                                        }
                                    }
                                }
                            }

                            documentDto.Note = noteMax;
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "PourrissementBoisBlindage.xml"));
            return listeDocumentDtoPge;
        }

        /// <summary>
        /// Méthode de calcul de critère en mode asynchrone
        /// </summary>
        public void CalculAsync()
        {
            Task.Factory.StartNew(() =>
            {

            });
        }

        /// <inheritdoc />
        public IEnumerable<CritereDto> CalculCritereC1(ProgrammationDto programmationdto, IEnumerable<Documents> listePgeDocument)
        {
            this.Logger.Info("********** Calcul du critère C1 **********");
            IList<CritereDto> listeCriteres = new List<CritereDto>();

            double cpA1 = 0, cpA4 = 0, cpA5 = 0, cpA6 = 0, cpA7 = 0, cpA9 = 0, cpA10 = 0, cpA11 = 0;

            Task.WaitAll(
                //Task.Factory.StartNew(() => this.ListePgeDocument = this._uow.CriterePerformanceRepositorie.GetAllDocumentPge(programmationdto.NatureTravaux).ToList()),
                //Task.Factory.StartNew(() => this.ListeCoefficientPonderation = this._uow.CalibrageRepositorie.GetAllCoefficientPonderation()),
                Task.Factory.StartNew(() => this.ListePgeFissuration = this.GetNotesMaxForAllDocumentPge_Fissuration(EFamilleDesordre.Fissurations, programmationdto.NatureTravaux)),
                Task.Factory.StartNew(() => this.ListePgeInfiltration = this.GetNotesMaxForAllDocumentPge_Infultration(EFamilleDesordre.Infiltrations, programmationdto.NatureTravaux)),
                Task.Factory.StartNew(() => this.ListePgeDesordreStructureMaconnerieBeton = this.GetNotesMaxForAllDocumentPge_DesordreStructureMaconnerieBeton(EFamilleDesordre.Desordre_structure_Maçonnerie_Beton, programmationdto.NatureTravaux)),
            Task.Factory.StartNew(() => this.ListePgeAgeOuvrage = this.GetNoteMaxForAllDocumentPge_AgeOuvrage(programmationdto.Anneeprogrammation, programmationdto.NatureTravaux, listePgeDocument)),
            Task.Factory.StartNew(() => this.ListePgeLargeurOuvrage = this.GetNoteMaxForAllDocumentPge_LargeurOuvrage(programmationdto.NatureTravaux, listePgeDocument)),
            Task.Factory.StartNew(() => programmationdto.NatureTravaux == ENatureCalibrage.Maçonnerie ? this.ListePgeHistoriqueRegenerationMaconnerie = this.GetNoteMaxForAllDocumentPge_HistoriqueRegenerationMaconnerie(programmationdto.Anneeprogrammation, programmationdto.NatureTravaux, listePgeDocument) : this.ListePgeHistoriqueRegenerationEnduit = this.GetNoteMaxForAllDocumentPge_HistoriqueRegenerationEnduit(programmationdto.Anneeprogrammation, programmationdto.NatureTravaux, listePgeDocument)),
            Task.Factory.StartNew(() => this.ListePgeAgressiviteChimiqueTerrainEncaissant = this.GetNoteMaxForAllDocumentPge_AgressiviteChimiqueTerrainEncaissant(programmationdto.NatureTravaux, listePgeDocument)),
            Task.Factory.StartNew(() => this.ListePgeSolibiliteTerrain = this.GetNoteMaxForAllDocumentPge_SolubiliteTerrain(programmationdto.NatureTravaux, listePgeDocument)),
            Task.Factory.StartNew(() => this.ListePgePourrissementBoisBlindage = this.GetNoteMaxForAllDocumentPge_PourrissementBoisBlindage(programmationdto.NatureTravaux, listePgeDocument))
            );

            //Obtenir les éléments de calcul notamment les coefficients de pondération pour lancer le calcul
            switch (programmationdto.NatureTravaux)
            {
                case ENatureCalibrage.Maçonnerie:
                    cpA1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA1").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpA4 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA4").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpA5 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA5").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpA7 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA7").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpA9 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA9").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpA10 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA10").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpA11 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA11").Single().ValeurMaconnerie.GetValueOrDefault();
                    break;
                case ENatureCalibrage.Enduit:
                    cpA1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA1").Single().ValeurEnduit.GetValueOrDefault();
                    cpA4 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA4").Single().ValeurEnduit.GetValueOrDefault();
                    cpA6 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA6").Single().ValeurEnduit.GetValueOrDefault();
                    cpA7 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA7").Single().ValeurEnduit.GetValueOrDefault();
                    cpA9 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA9").Single().ValeurEnduit.GetValueOrDefault();
                    cpA10 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA10").Single().ValeurEnduit.GetValueOrDefault();
                    cpA11 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpA11").Single().ValeurEnduit.GetValueOrDefault();

                    break;
            }

            for (int i = 0; i < (programmationdto.NatureTravaux == ENatureCalibrage.Maçonnerie ? listePgeDocument.Count() : ListePgeFissuration.Count()); i++)
            {
                CritereDto critereC1 = new CritereDto { Critere = CritereEnum.C1 };
                var param1 = ListePgeFissuration.ToArray()[i].Note;
                var param2 = ListePgeInfiltration.ToArray()[i].Note;
                var param3 = ListePgeDesordreStructureMaconnerieBeton.ToArray()[i].Note;

                //retenir la note max entre les 3 params
                var noteMaxParam = this.GetNoteMax(param1, param2, param3);

                critereC1.NumeroAffaire = listePgeDocument.ToArray()[i].NumeroAffaire;

                //Calcul de C1: 𝐶1=𝑐𝑝1.01×𝑚𝑎𝑥[𝑝1.01; 𝑝1.02; 𝑝1.03]+Σ𝑐𝑝1.𝑦×𝑝1.𝑦
                critereC1.valeur = (cpA1 * noteMaxParam) +
                    (cpA4 * this.ListePgeAgeOuvrage.ToArray()[i].Note) +
                    (programmationdto.NatureTravaux == ENatureCalibrage.Maçonnerie ? (cpA5 * this.ListePgeHistoriqueRegenerationMaconnerie.ToArray()[i].Note) : (cpA6 * this.ListePgeHistoriqueRegenerationEnduit.ToArray()[i].Note)) +
                    (cpA7 * this.ListePgeLargeurOuvrage.ToArray()[i].Note) +
                    (cpA9 * this.ListePgeAgressiviteChimiqueTerrainEncaissant.ToArray()[i].Note) +
                    (cpA10 * this.ListePgeSolibiliteTerrain.ToArray()[i].Note) +
                    (cpA11 * this.ListePgePourrissementBoisBlindage.ToArray()[i].Note);

                listeCriteres.Add(critereC1);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<CritereDto>>.XmlSerialize(listeCriteres, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CalculC1.xml"));
            this.Logger.Info(string.Format("Calcul critère C1 terminé, vérifiez le fichier :{0}", string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CalculC1.xml")));

            return listeCriteres;
        }
        #endregion

        #region C2 : IMPACT DE LA DEFAILLANCE
        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_Frequentation(ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C2: {0}", "Fréquentation (p2.01) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllOuvragesWithInfoLignes(listeIdOuvrage);

                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            documentDto.Note = this.GetNoteOfParametreFrequentation(listeOuvrage);
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "Frequentation.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_Correspondance(ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C2: {0}", "Correspondance (p2.02) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage);
                        int noteCorrexpondance = 0;
                        int note;
                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                //TODO YAR: dans le cas de plusieurs ouvrages trouvés pour un PGE, 
                                //dans ce cas je prends la notre du premier ouvrage.
                                if (int.TryParse(ouvrage.Correspondance_Note, out note))
                                {
                                    noteCorrexpondance = note;
                                }

                                break;
                            }

                            documentDto.Note = noteCorrexpondance;
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "Correspondance.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_PerimetreL2L6(ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C2: {0}", "Périmètre L2 – L6 (p2.03) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage);
                        int notePerimetre = 0;
                        int note;
                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            foreach (var ouvrage in listeOuvrage)
                            {
                                //TODO YAR: dans le cas de plusieurs ouvrages trouvés pour un PGE, 
                                //dans ce cas je prends la notre du premier ouvrage.
                                if (int.TryParse(ouvrage.PerimetreL2L6_Note, out note))
                                {
                                    notePerimetre = note;
                                }

                                break;
                            }

                            documentDto.Note = notePerimetre;
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "Perimetre.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_ImageRatp(ENatureCalibrage natuteCalibrage, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C2: {0}", "Image RATP (p2.04)"));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        //TODO YAR: obtenir la note directement depuis le tableau annexe
                        //var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllDataAnnexeByIdOuvrage(listeIdOuvrage);

                        // TODO YAR: utiliser une méthod eprivé qui permettant d'obtenir la note par la vérification de la Lignes
                        var listeOuvrage = this._uow.CriterePerformanceRepositorie.GetAllOuvragesWithInfoLignes(listeIdOuvrage);

                        if (listeOuvrage != null && listeOuvrage.Any())
                        {
                            documentDto.Note = this.GetNoteOfParametreImageRatp(listeOuvrage);
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "ImageRatp.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />
        public IEnumerable<CritereDto> CalculCritereC2(ProgrammationDto programmationdto, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info("********** Calcul du critère C2 **********");
            IList<CritereDto> listeCriteres = new List<CritereDto>();

            double cpB1 = 0, cpB2 = 0, cpB3 = 0, cpB4 = 0;

            Task.WaitAll(
                //Task.Factory.StartNew(() => this.ListeCoefficientPonderation = this._uow.CalibrageRepositorie.GetAllCoefficientPonderation()),
                Task.Factory.StartNew(() => this.ListePgeFrequentation = this.GetNoteMaxForAllDocumentPge_Frequentation(programmationdto.NatureTravaux, listeDocumentPge)),
                Task.Factory.StartNew(() => this.ListePgeCorrespondance = this.GetNoteMaxForAllDocumentPge_Correspondance(programmationdto.NatureTravaux, listeDocumentPge)),
                Task.Factory.StartNew(() => this.ListePgePerimetreL2L6 = this.GetNoteMaxForAllDocumentPge_PerimetreL2L6(programmationdto.NatureTravaux, listeDocumentPge)),
                Task.Factory.StartNew(() => this.ListePgeImageRatp = this.GetNoteMaxForAllDocumentPge_ImageRatp(programmationdto.NatureTravaux, listeDocumentPge))
                );

            //Obtenir les éléments de calcul notamment les coefficients de pondération pour lancer le calcul
            switch (programmationdto.NatureTravaux)
            {
                case ENatureCalibrage.Maçonnerie:
                    cpB1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpB1").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpB2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpB2").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpB3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpB3").Single().ValeurMaconnerie.GetValueOrDefault();
                    cpB4 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpB4").Single().ValeurMaconnerie.GetValueOrDefault();
                    break;
                case ENatureCalibrage.Enduit:
                    cpB1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpB1").Single().ValeurEnduit.GetValueOrDefault();
                    cpB2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpB2").Single().ValeurEnduit.GetValueOrDefault();
                    cpB3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpB3").Single().ValeurEnduit.GetValueOrDefault();
                    cpB4 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "CpB4").Single().ValeurEnduit.GetValueOrDefault();
                    break;
            }

            for (int i = 0; i < listeDocumentPge.Count(); i++)
            {
                CritereDto critereC2 = new CritereDto { Critere = CritereEnum.C2 };

                critereC2.NumeroAffaire = listeDocumentPge.ToArray()[i].NumeroAffaire;

                //Calcul de C2: 𝐶2 = ∑ 𝑐𝑝2.𝑦 ×𝑝2.𝑦 𝑦=4 𝑦=1 
                critereC2.valeur = cpB1 * this.ListePgeFrequentation.ToArray()[i].Note + cpB2 * this.ListePgeCorrespondance.ToArray()[i].Note + cpB3 * this.ListePgePerimetreL2L6.ToArray()[i].Note + cpB4 * this.ListePgeImageRatp.ToArray()[i].Note;

                listeCriteres.Add(critereC2);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<CritereDto>>.XmlSerialize(listeCriteres, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CalculC2.xml"));
            this.Logger.Info(string.Format("Calcul critère C2 terminé, Vérifiez le fichier :{0}", string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CalculC2.xml")));

            return listeCriteres;
        }
        #endregion

        #region  C3: OPPORTUNITE DE TRAVAUX
        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_PolitiqueRenivation(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C3: {0}", "Politique de rénovation de la RATP (p3.01) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            var mapperLigne = new LigneToLigneDtoMapping();

            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrageWithLieuLigne = this._uow.CriterePerformanceRepositorie.GetAllOuvragesWithInfoLignes(listeIdOuvrage);

                        if (listeOuvrageWithLieuLigne != null && listeOuvrageWithLieuLigne.Any())
                        {
                            var lieuGestionnaire = listeOuvrageWithLieuLigne.First().Lieux;
                            var ligneGestionnaire = lieuGestionnaire.Lignes;
                            if (!programmationDto.Lignes.Any(xx => xx.Id == ligneGestionnaire.Id))
                            {
                                //la PGE n’est pas concernée ni par l’automatisation de l’exploitation, ni par le programme RNM.
                                if (!programmationDto.Rnms.Any(p => p.Lieu.Id == lieuGestionnaire.Id))
                                {
                                    documentDto.Note = 1;
                                }

                                // la PGE n’est pas concernée par l’automatisation de l’exploitation, mais par le programme RNM au-delà de 3 ans par rapport à l’année de programmation
                                if (programmationDto.Rnms.Any(p => p.Lieu.Id == lieuGestionnaire.Id) && programmationDto.Rnms.Any(p => p.Annee > programmationDto.Anneeprogrammation + 3))
                                {
                                    documentDto.Note = 4;
                                }

                                // la PGE n’est pas concernée par l’automatisation de l’exploitation, mais par le programme RNM dans 3 ans
                                if (programmationDto.Rnms.Any(p => p.Lieu.Id == lieuGestionnaire.Id) && programmationDto.Rnms.Any(p => p.Annee == programmationDto.Anneeprogrammation + 3))
                                {
                                    documentDto.Note = 5;
                                }

                                // la PGE est concernée par le programme RNM avant de 2 ans, sans importer si la PGE est concernée par l’automatisation de l’exploitation ou non
                                if (programmationDto.Rnms.Any(p => p.Lieu.Id == lieuGestionnaire.Id) && programmationDto.Rnms.Any(p => p.Annee <= programmationDto.Anneeprogrammation + 2))
                                {
                                    documentDto.Note = 7;
                                }
                            }
                            else
                            {
                                // la PGE est concernée par le programme RNM avant de 2 ans, sans importer si la PGE est concernée par l’automatisation de l’exploitation ou non
                                if (programmationDto.Rnms.Any(p => p.Lieu.Id == lieuGestionnaire.Id) && programmationDto.Rnms.Any(p => p.Annee <= programmationDto.Anneeprogrammation + 2))
                                {
                                    documentDto.Note = 7;
                                }

                                //la PGE est concernée par l’automatisation de l’exploitation, et non concernée par le programme RNM ou, si oui, pas avant de 2 ans. 
                                if ((!programmationDto.Rnms.Any(p => p.Lieu.Id == lieuGestionnaire.Id)) || (programmationDto.Rnms.Any(p => p.Lieu.Id == lieuGestionnaire.Id) && programmationDto.Rnms.Any(p => p.Annee > programmationDto.Anneeprogrammation + 2)))
                                {
                                    documentDto.Note = 6;
                                }
                            }
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "PolitiqueRenovationRatp.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_TravauxExternes(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info(string.Format("Calcul critère C3: {0}", " Travaux Externes (p3.02) "));
            IList<DocumentDto> listeDocumentDtoPge = new List<DocumentDto>();
            var mapperLigne = new LigneToLigneDtoMapping();

            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                documentDto.NumeroAffaire = doc.NumeroAffaire;
                if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                {
                    IList<int> listeIdOuvrage = new List<int>();
                    foreach (var docDesordre in doc.DesordresDocuments)
                    {
                        if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                        {
                            listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                        }
                    }

                    if (listeIdOuvrage.Any())
                    {
                        var listeOuvrageWithLieuLigne = this._uow.CriterePerformanceRepositorie.GetAllOuvragesWithInfoLignes(listeIdOuvrage);
                        if (listeOuvrageWithLieuLigne != null && listeOuvrageWithLieuLigne.Any())
                        {
                            // Ouvrage non affecté par les travaux externes 
                            if (listeOuvrageWithLieuLigne.Any(x => !programmationDto.TravauxExternes.Any(xx => xx.Lieu.Id == x.Lieux.Id))
                                || (listeOuvrageWithLieuLigne.Any(x => programmationDto.TravauxExternes.Any(xx => xx.Lieu.Id == x.Lieux.Id && int.Parse(xx.Date.Substring(3)) != programmationDto.Anneeprogrammation)))
                                || (listeOuvrageWithLieuLigne.Any(x => programmationDto.TravauxExternes.Any(xx => xx.Lieu.Id == x.Lieux.Id && xx.NatureTravauxExt.Id != 2))))
                            {
                                documentDto.Note = 1;
                            }

                            //Ouvrage affecté par les travaux externes 
                            if ((listeOuvrageWithLieuLigne.Any(x => programmationDto.TravauxExternes.Any(xx => xx.Lieu.Id == x.Lieux.Id && int.Parse(xx.Date.Substring(3)) == programmationDto.Anneeprogrammation && xx.NatureTravauxExt.Id == 2))))
                            {
                                documentDto.Note = 7;
                            }
                        }
                    }
                }

                listeDocumentDtoPge.Add(documentDto);
            }
            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(listeDocumentDtoPge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "TravauxExternes.xml"));

            return listeDocumentDtoPge;
        }

        /// <inheritdoc />
        public IEnumerable<CritereDto> CalculCritereC3(ProgrammationDto programmationdto, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info("********** Calcul du critère C2 **********");
            IList<CritereDto> listeCriteres = new List<CritereDto>();

            Task.WaitAll(
                //Task.Factory.StartNew(() => this.ListePgeDocument = this._uow.CriterePerformanceRepositorie.GetAllDocumentPge(programmationdto.NatureTravaux).ToList()),
                Task.Factory.StartNew(() => this.ListePgePolitiquerenovation = this.GetNoteMaxForAllDocumentPge_PolitiqueRenivation(programmationdto, listeDocumentPge)),
                Task.Factory.StartNew(() => this.ListePgeTravauxExternes = this.GetNoteMaxForAllDocumentPge_TravauxExternes(programmationdto, listeDocumentPge)));

            for (int i = 0; i < listeDocumentPge.Count(); i++)
            {
                CritereDto critereC3 = new CritereDto { Critere = CritereEnum.C3 };

                critereC3.NumeroAffaire = listeDocumentPge.ToArray()[i].NumeroAffaire;

                //Calcul de C3: 𝐶3 = C3 = max [p3.01 ; p3.02]
                if (this.ListePgePolitiquerenovation.ToArray()[i].Note > this.ListePgeTravauxExternes.ToArray()[i].Note)
                {
                    critereC3.valeur = this.ListePgePolitiquerenovation.ToArray()[i].Note;
                }
                else
                {
                    critereC3.valeur = this.ListePgeTravauxExternes.ToArray()[i].Note;
                }

                listeCriteres.Add(critereC3);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<CritereDto>>.XmlSerialize(listeCriteres, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CalculC3.xml"));
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(this.ListePgeTravauxExternes, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "TravauxExternes.xml"));
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(this.ListePgePolitiquerenovation, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "Politiquerenovation.xml"));

            this.Logger.Info(string.Format("Calcul critère C3 terminé,vérifiez le fichier :{0}", string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CalculC3.xml")));

            return listeCriteres;
        }
        #endregion

        /// <inheritdoc />
        public IEnumerable<CategoriePgeDto> CalculPessimisteOptimiste(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge)
        {
            IList<ClassementDto> listeClassementPgeSeuil = new List<ClassementDto>();
            this.ListeCategoriePge = new List<CategoriePgeDto>();
            IList<PessimisteOptimisteDto> listePessimiste = new List<PessimisteOptimisteDto>();
            IList<PessimisteOptimisteDto> listeOptimiste = new List<PessimisteOptimisteDto>();
            IList<ClassementDto> listeClassement = new List<ClassementDto>();

            this.Logger.Info("Calculs (méthode ELECTRE TRI) : Calculs PESSIMISTE & OPTIMISTE");

            for (int i = 0; i < listeDocumentPge.Count(); i++)
            {
                PessimisteOptimisteDto pessimiste = new PessimisteOptimisteDto { NumeroAffaire = listeDocumentPge.ToArray()[i].NumeroAffaire };
                PessimisteOptimisteDto optimiste = new PessimisteOptimisteDto { NumeroAffaire = listeDocumentPge.ToArray()[i].NumeroAffaire };
                ClassementDto classementDto = new ClassementDto { NumeroAffaire = listeDocumentPge.ToArray()[i].NumeroAffaire };
                CategoriePgeDto categorie = new CategoriePgeDto { NumeroAffaire = listeDocumentPge.ToArray()[i].NumeroAffaire };

                //Calcul PESSIMISTE
                pessimiste.B0 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B0, ETypeCalcul.PESSIMISTE);
                pessimiste.B1 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B1, ETypeCalcul.PESSIMISTE);
                pessimiste.B2 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B2, ETypeCalcul.PESSIMISTE);
                pessimiste.B3 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B3, ETypeCalcul.PESSIMISTE);
                pessimiste.B4 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B4, ETypeCalcul.PESSIMISTE);

                //CALCUL OPTIMISTE
                optimiste.B0 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B0, ETypeCalcul.OPTIMISTE);
                optimiste.B1 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B1, ETypeCalcul.OPTIMISTE);
                optimiste.B2 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B2, ETypeCalcul.OPTIMISTE);
                optimiste.B3 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B3, ETypeCalcul.OPTIMISTE);
                optimiste.B4 = this.GetIndiceCredibilite(programmationDto.NatureTravaux, listeCritereC1.ToArray()[i].valeur, listeCritereC2.ToArray()[i].valeur, listeCritereC3.ToArray()[i].valeur, ESeuilReference.B4, ETypeCalcul.OPTIMISTE);

                //Classement des pges par rapport au seuil
                classementDto.ClassementPgeB0 = this.GetClassement(pessimiste.B0, optimiste.B0);
                classementDto.ClassementPgeB1 = this.GetClassement(pessimiste.B1, optimiste.B1);
                classementDto.ClassementPgeB2 = this.GetClassement(pessimiste.B2, optimiste.B2);
                classementDto.ClassementPgeB3 = this.GetClassement(pessimiste.B3, optimiste.B3);
                classementDto.ClassementPgeB4 = this.GetClassement(pessimiste.B4, optimiste.B4);

                //AFFECTATION DE LA CATEGORIE

                if (classementDto.ClassementPgeB0 == '>' && classementDto.ClassementPgeB1 == '<' && classementDto.ClassementPgeB2 == '<' && classementDto.ClassementPgeB3 == '<' && classementDto.ClassementPgeB4 == '<')
                {
                    categorie.Categorie = "CAT 1";
                }

                if (classementDto.ClassementPgeB0 == '>' && classementDto.ClassementPgeB1 == '>' && classementDto.ClassementPgeB2 == '<' && classementDto.ClassementPgeB3 == '<' && classementDto.ClassementPgeB4 == '<')
                {
                    categorie.Categorie = "CAT 2";
                }

                if (classementDto.ClassementPgeB0 == '>' && classementDto.ClassementPgeB1 == '>' && classementDto.ClassementPgeB2 == '>' && classementDto.ClassementPgeB3 == '<' && classementDto.ClassementPgeB4 == '<')
                {
                    categorie.Categorie = "CAT 3";
                }

                if (classementDto.ClassementPgeB0 == '>' && classementDto.ClassementPgeB1 == '>' && classementDto.ClassementPgeB2 == '>' && classementDto.ClassementPgeB3 == '>' && classementDto.ClassementPgeB4 == '<')
                {
                    categorie.Categorie = "CAT 4";
                }

                if (string.IsNullOrEmpty(categorie.Categorie))
                {
                    categorie.Categorie = string.Empty;
                }

                this.ListeCategoriePge.Add(categorie);

                //TODO: cette génération n'est été pas prévu, je l'ai fait par demande d'angel, aide au calcul
                listePessimiste.Add(pessimiste);
                listeOptimiste.Add(optimiste);
                listeClassement.Add(classementDto);

            }

            //Serialisation les PGE en XML
            //TODO: cette génération n'est été pas prévu, je l'ai fait par demande d'angel, aide au calcul
            SerializeObject<IEnumerable<PessimisteOptimisteDto>>.XmlSerialize(listePessimiste, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "MatriceIndiceCredibilite_FomulationPESSIMISTE.xml"));
            SerializeObject<IEnumerable<PessimisteOptimisteDto>>.XmlSerialize(listeOptimiste, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "MatriceIndiceCredibilite_FomulationOPTIMISTE.xml"));
            SerializeObject<IEnumerable<ClassementDto>>.XmlSerialize(listeClassement, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "Classement.xml"));
            SerializeObject<IEnumerable<CategoriePgeDto>>.XmlSerialize(this.ListeCategoriePge, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CategoriePge.xml"));
            this.Logger.Info(string.Format("Le calcul PESSIMISTE & OPTIMISTE est terminé, Vérifier le fichier :{0}", string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CategoriePge.xml")));

            return this.ListeCategoriePge;
        }

        #region C4: GROUPEMENT DE TRAVAUX
        /// <inheritdoc />
        public void CalculNotesParametresCritere4(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge)
        {
            IEnumerable<CategoriePgeDto> listeResultatCalcul1 = null;
            this.ListePgeTravauxConnexes = new List<DocumentDto>();
            this.ListePgeTravauxInternes = new List<DocumentDto>();

            Task.WaitAll(
                //Task.Factory.StartNew(() => listeDocumentPge = this._uow.CriterePerformanceRepositorie.GetAllDocumentPge(programmationDto.NatureCalibrage)),
                Task.Factory.StartNew(() => listeResultatCalcul1 = this.CalculPessimisteOptimiste(programmationDto, listeDocumentPge))
                );

            this.Logger.Info(string.Format("Calcul critère C4: {0}", "TRAVAUX CONNEXES(p4.01) "));
            this.Logger.Info(string.Format("Calcul critère C4: {0}", "TRAVAUX INTERNES(p4.02) "));
            foreach (var doc in listeDocumentPge)
            {
                #region OBTENIR NOTE DU PARAMETRE TRAVAUX CONNEXES
                var Param_TravauxConnexes = new DocumentDto();
                Param_TravauxConnexes.NumeroAffaire = doc.NumeroAffaire;

                //Obtenir la note par lun ouvrage est concerné par une PGE classé  en CAT 1/2/3.
                if (listeResultatCalcul1.Any(x => x.NumeroAffaire == doc.NumeroAffaire))
                {
                    var resultPgeOfCalcul1 = listeResultatCalcul1.FirstOrDefault(x => x.NumeroAffaire == doc.NumeroAffaire);
                    switch (resultPgeOfCalcul1.Categorie)
                    {
                        case "CAT 2":
                        case "CAT 1":
                            Param_TravauxConnexes.Note = 0;
                            break;

                        case "CAT 3":
                            Param_TravauxConnexes.Note = 1;
                            break;

                        case "CAT 4":
                            Param_TravauxConnexes.Note = 2;
                            break;

                        //les ouvrages qui n'ont pas de catégorie 
                        default:

                            if (doc.DesordresDocuments != null && doc.DesordresDocuments.Any())
                            {
                                //Obtenir la liste des ovrages du PGE
                                if (this.GetAllOuvrages(doc).Any())
                                {
                                    var listeOuvragePgeWithoutCategortie = this._uow.CriterePerformanceRepositorie.GetAllOuvragesWithInfoLignes(this.GetAllOuvrages(doc));

                                    //Ouvrage entre deux ouvrages concernés par deux PGE classées en CAT 3 lors de l’étape de calcul 1. 
                                    var listeDocumentPgeInCat3 = listeDocumentPge.Where(x => listeResultatCalcul1.Any(xx => xx.NumeroAffaire == x.NumeroAffaire && xx.Categorie == "CAT 3")).ToList();
                                    foreach (var docCat in listeDocumentPgeInCat3)
                                    {
                                        var listeOuvragePgeWithCategorie = this._uow.CriterePerformanceRepositorie.GetAllOuvragesWithInfoLignes(this.GetAllOuvrages(docCat));

                                        if (listeOuvragePgeWithoutCategortie.Any(x => !string.IsNullOrEmpty(x.Descriptions.PkDebut) && !string.IsNullOrEmpty(x.Descriptions.PkFin)) && listeOuvragePgeWithCategorie.Any(x => !string.IsNullOrEmpty(x.Descriptions.PkDebut) && !string.IsNullOrEmpty(x.Descriptions.PkFin)))
                                        {
                                            if (listeOuvragePgeWithoutCategortie.Any(x => this.IsConverted(x.Descriptions.PkFin) && this.IsConverted(x.Descriptions.PkDebut) &&
                                            x.Descriptions.PkDebut != null &&
                                            x.Descriptions.PkFin != null &&
                                            listeOuvragePgeWithCategorie.Any(xx => xx.Descriptions.PkDebut != null && xx.Descriptions.PkFin != null &&
                                            double.Parse(xx.Descriptions.PkDebut.Replace('.', ',')) < double.Parse(x.Descriptions.PkDebut.Replace('.', ',')) &&
                                            double.Parse(xx.Descriptions.PkFin.Replace('.', ',')) > double.Parse(x.Descriptions.PkFin.Replace('.', ',')) &&
                                            this.IsConverted(xx.Descriptions.PkDebut) &&
                                            this.IsConverted(xx.Descriptions.PkFin))))
                                            {
                                                Param_TravauxConnexes.Note = 2;
                                                break;
                                            }
                                        }
                                    }

                                    //Ouvrage placé entre deux ouvrages concernés par deux PGE classées en CAT 4 lors de l’étape de calcul 1. 
                                    var listeDocumentPgeInCat4 = listeDocumentPge.Where(x => listeResultatCalcul1.Any(xx => xx.NumeroAffaire == x.NumeroAffaire && xx.Categorie == "CAT 4")).ToList();
                                    foreach (var docCat in listeDocumentPgeInCat4)
                                    {
                                        var listeOuvragePgeWithCategorie = this._uow.CriterePerformanceRepositorie.GetAllOuvragesWithInfoLignes(this.GetAllOuvrages(docCat));
                                        if (listeOuvragePgeWithoutCategortie.Any(x => !string.IsNullOrEmpty(x.Descriptions.PkFin) && !string.IsNullOrEmpty(x.Descriptions.PkDebut) && listeOuvragePgeWithCategorie.Any(xx => !string.IsNullOrEmpty(xx.Descriptions.PkDebut) && !string.IsNullOrEmpty(xx.Descriptions.PkFin) && double.Parse(xx.Descriptions.PkDebut.Replace('.', ',')) < double.Parse(x.Descriptions.PkDebut.Replace('.', ',')) && double.Parse(xx.Descriptions.PkFin.Replace('.', ',')) > double.Parse(x.Descriptions.PkFin.Replace('.', ',')))))
                                        {
                                            Param_TravauxConnexes.Note = 4;
                                            break;
                                        }
                                    }

                                    //Ouvrage placé entre deux ouvrages concernés par des PGE classées en CAT 3 et 4 lors de l’étape de calcul 1. 
                                    //TODO YAR à terminé 

                                }
                            }
                            break;
                    }
                }

                this.ListePgeTravauxConnexes.Add(Param_TravauxConnexes);

                #endregion

                #region NOTES DU PARAMETRE TRAVAUX INTERNES
                var documentDtoTravauxInternes = new DocumentDto();
                if (listeResultatCalcul1.Any(x => x.NumeroAffaire == doc.NumeroAffaire))
                {
                    documentDtoTravauxInternes.NumeroAffaire = doc.NumeroAffaire;

                    var listeDocunbrNaturTravaux = doc.DesordresDocuments.GroupBy(x => x.IdNatureTravaux).Select(x => new { Key = x.Key, NbrTravaux = x.Count() });

                    // si la PGE propose plus d’une nature de travaux.
                    if (listeDocunbrNaturTravaux.Count() > 1)
                    {
                        documentDtoTravauxInternes.Note = 7;
                    }

                    // si la PGE propose une seule nature de travaux 
                    else if (listeDocunbrNaturTravaux.Count() == 1)
                    {
                        documentDtoTravauxInternes.Note = 0;
                    }

                    this.ListePgeTravauxInternes.Add(documentDtoTravauxInternes);
                }

                #endregion
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(this.ListePgeTravauxConnexes, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "TravauxConnexes.xml"));
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(this.ListePgeTravauxInternes, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "TravauxInternes.xml"));
        }

        /// <inheritdoc />
        public IEnumerable<DocumentDto> GetNoteMaxForAllDocumentPge_TravauxInternes(ProgrammationDto programmationDto, IEnumerable<Documents> listeDocumentPge)
        {
            IEnumerable<CategoriePgeDto> listeResultatCalcul1 = null;
            IList<DocumentDto> resultDocumentPges = new List<DocumentDto>();

            Task.WaitAll(
                //Task.Factory.StartNew(() => listeDocumentPge = this._uow.CriterePerformanceRepositorie.GetAllDocumentPge(programmationDto.NatureCalibrage)),
                Task.Factory.StartNew(() => listeResultatCalcul1 = this.CalculPessimisteOptimiste(programmationDto, listeDocumentPge))
                );

            foreach (var doc in listeDocumentPge)
            {
                var documentDto = new DocumentDto();
                if (listeResultatCalcul1.Any(x => x.NumeroAffaire == doc.NumeroAffaire))
                {
                    documentDto.NumeroAffaire = doc.NumeroAffaire;

                    var listeDocunbrNaturTravaux = doc.DesordresDocuments.GroupBy(x => x.IdNatureTravaux).Select(x => new { Key = x.Key, NbrTravaux = x.Count() });

                    // si la PGE propose plus d’une nature de travaux.
                    if (listeDocunbrNaturTravaux.Count() > 1)
                    {
                        documentDto.Note = 7;
                    }

                    // si la PGE propose une seule nature de travaux 
                    else if (listeDocunbrNaturTravaux.Count() == 1)
                    {
                        documentDto.Note = 0;
                    }
                }

                resultDocumentPges.Add(documentDto);
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<DocumentDto>>.XmlSerialize(resultDocumentPges, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "TravauxInternes.xml"));

            return resultDocumentPges;
        }

        /// <inheritdoc />
        public IEnumerable<CritereDto> CalculCritereC4(ProgrammationDto programmationdto, IEnumerable<Documents> listeDocumentPge)
        {
            this.Logger.Info("********** Calcul du critère C4 **********");
            IList<CritereDto> listeCriteres = new List<CritereDto>();

            //Obtenir les notes des paramètres du critère C4
            this.CalculNotesParametresCritere4(programmationdto, listeDocumentPge);

            if (this.ListePgeTravauxConnexes != null && this.ListePgeTravauxInternes != null)
            {
                for (int i = 0; i < this.ListePgeTravauxConnexes.Count(); i++)
                {
                    var critereDto = new CritereDto { NumeroAffaire = ListePgeTravauxConnexes.ToArray()[i].NumeroAffaire, Critere = CritereEnum.C4 };

                    float noteParam1 = this.ListePgeTravauxConnexes.ToArray()[i].Note;
                    float noteParam2 = this.ListePgeTravauxInternes.ToArray()[i].Note;
                    critereDto.valeur = (noteParam1 + noteParam2) / 2;
                    listeCriteres.Add(critereDto);
                }
            }

            //Serialisation les PGE en XML
            SerializeObject<IEnumerable<CritereDto>>.XmlSerialize(listeCriteres, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CalculC4.xml"));
            this.Logger.Info(string.Format("Calcul critère C4 terminé, le fichier des valeurs est dans :{0}", string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "CalculC4.xml")));

            return listeCriteres;
        }

        #endregion

        /// <inheritdoc />
        public IEnumerable<Documents> GetAllDocumentPgeByNatureTravaux(ENatureCalibrage natureTravaux)
        {
            return this._uow.CriterePerformanceRepositorie.GetAllDocumentPge(natureTravaux);
        }

        #region ETAPE 2 CALCUL
        public int CalculEtape2(ProgrammationDto programmationDto)
        {
            double cp1 = 0, cp2 = 0, cp3 = 0, cp4 = 0, v1 = 0;
            this.ListeIndicesCridibilite = new Dictionary<string, float>();
            this.ListeIndicesDiscordance = new Dictionary<string, float>();
            CritereDto[] listeCritereC4 = null;

            this.Logger.Info(string.Format("Epate de calcul 1 : classement des PGE dans des catégories de priorité pour la nature travaux: {0}", programmationDto.NatureTravaux.ToString()));

            this.ListePgeDocument = this._uow.CriterePerformanceRepositorie.GetAllDocumentPge(programmationDto.NatureTravaux).ToList();
            this.ListeCoefficientPonderation = this._uow.CalibrageRepositorie.GetAllCoefficientPonderation();

            Task.WaitAll(
                Task.Factory.StartNew(() => listeCritereC1 = this.CalculCritereC1(programmationDto, this.ListePgeDocument).ToArray()),
                Task.Factory.StartNew(() => listeCritereC2 = this.CalculCritereC2(programmationDto, this.ListePgeDocument).ToArray()),
                Task.Factory.StartNew(() => listeCritereC3 = this.CalculCritereC3(programmationDto, this.ListePgeDocument).ToArray()));

            //Calcul C4
            Task.WaitAll(
            Task.Factory.StartNew(() => listeCritereC4 = this.CalculCritereC4(programmationDto, this.ListePgeDocument).ToArray()));

            #region Coefficient pondération 
            //Obtenir les éléments de calcul notamment les coefficients de pondération pour lancer le calcul
            switch (programmationDto.NatureTravaux)
            {
                case ENatureCalibrage.Maçonnerie:
                    cp1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "Cp1").Single().ValeurMaconnerie.GetValueOrDefault();
                    cp2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "Cp2").Single().ValeurMaconnerie.GetValueOrDefault();
                    cp3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "Cp3").Single().ValeurMaconnerie.GetValueOrDefault();
                    cp4 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "Cp4").Single().ValeurMaconnerie.GetValueOrDefault();
                    v1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "V1").Single().ValeurMaconnerie.GetValueOrDefault();
                    break;
                case ENatureCalibrage.Enduit:
                    cp1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "Cp1").Single().ValeurEnduit.GetValueOrDefault();
                    cp2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "Cp2").Single().ValeurEnduit.GetValueOrDefault();
                    cp3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "Cp3").Single().ValeurEnduit.GetValueOrDefault();
                    cp4 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "Cp4").Single().ValeurEnduit.GetValueOrDefault();
                    v1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "V1").Single().ValeurMaconnerie.GetValueOrDefault();
                    break;
            }
            #endregion

            Logger.Info("Indices de Discordance :"); // RDT : Angel demande les indices de discordance

            for (int i = 0; i < this.ListePgeDocument.Count(); i++)
            {
                string key = this.ListePgeDocument.ToArray()[i].NumeroAffaire;
                //this.ListeIndicesCridibilite.Add(string.Format("{0}_{1}", key, this.ListePgeDocument.ToArray()[i].NumeroAffaire), 1);
                for (int k = 0; k < this.ListePgeDocument.Count(); k++)
                {
                    //Indice de concordance partiel icj(xi, xk) :
                    var indiceConcordancePartialC1 = listeCritereC1[k].valeur - listeCritereC1[i].valeur > 0 ? 0 : 1;
                    var indiceConcordancePartialC2 = listeCritereC2[k].valeur - listeCritereC2[i].valeur > 0 ? 0 : 1;
                    var indiceConcordancePartialC3 = listeCritereC3[k].valeur - listeCritereC3[i].valeur > 0 ? 0 : 1;
                    var indiceConcordancePartialC4 = listeCritereC4[k].valeur - listeCritereC4[i].valeur > 0 ? 0 : 1;

                    //Indice de concordance global iC(xi, xk) : 
                    float sommeCoefficientAndCritere = (float)(cp1 * indiceConcordancePartialC1 + cp2 * indiceConcordancePartialC2 + cp3 * indiceConcordancePartialC3 + cp4 * indiceConcordancePartialC4);
                    double sommeCp = (cp1 + cp2 + cp3 + cp4);
                    float indiceConcordanceGlobal = default(float);

                    if (sommeCp != default(double))
                    {
                        indiceConcordanceGlobal = (float)(sommeCoefficientAndCritere / sommeCp);
                    }

                    //Indice de discordance id1(xi, xk) : 
                    var indiceDiscordance = this.GetIndiceDiscordance(listeCritereC1[k].valeur, listeCritereC1[i].valeur, v1);

                    //Indice de crédibilité 𝝈(𝒙𝒊,𝒙𝒌): 
                    var indiceCredibilite = this.GetIndiceCredibiiteEtapeCalcul2(indiceDiscordance, indiceConcordanceGlobal);

                    //j'ai composé une clé pour identifier la valeur de xi et xk des PGE
                    this.ListeIndicesCridibilite.Add(string.Format("{0}_{1}", key, this.ListePgeDocument.ToArray()[k].NumeroAffaire), indiceCredibilite);
                    this.ListeIndicesDiscordance.Add(string.Format("{0}_{1}", key, this.ListePgeDocument.ToArray()[k].NumeroAffaire), indiceDiscordance);
                }
            }

            //TODO: suite au demande d'angel a besoin le detail des indices de cridibilité
            if (this.ListeIndicesCridibilite != null && this.ListeIndicesCridibilite.Any())
            {
                SerializeObject<Dictionary<string, float>>.XmlSerialize(this.ListeIndicesDiscordance, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "MatriceIndicesDiscordance.xml"));
                SerializeObject<Dictionary<string, float>>.XmlSerialize(this.ListeIndicesCridibilite, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "MatriceIndicesCridibilite.xml"));
            }

            IEnumerable<ClassementDistillationDto> listeDistillationAscendante = null;
            IEnumerable<ClassementDistillationDto> listeDistillationDescendante = null;

            // ETAPE 2: DISTIATION DESCENDANTE & ASCENDANTE
            this.Logger.Info("ETAPE 2: DISTIATION DESCENDANTE & ASCENDANTE");
            this.listeIndiceCredibiliteDescendante = new Dictionary<string, float>(this.ListeIndicesCridibilite);
            this.listeIndiceCredibiliteAscendante = new Dictionary<string, float>(this.ListeIndicesCridibilite);

            IList<Documents> listeDocumentdescendante = new List<Documents>(this.ListePgeDocument);
            IList<Documents> listeDocumentaescendante = new List<Documents>(this.ListePgeDocument);

            Task.WaitAll(Task.Factory.StartNew(() => listeDistillationDescendante = this.CalculDistillationDescendante(listeDocumentdescendante)));


            //rénitialiser le compteur de classement
            rang = 0;

            Task.WaitAll(Task.Factory.StartNew(() => listeDistillationAscendante = this.CalculDistillationAscendante(listeDocumentaescendante)));

            //Obtenir le peordre final          
            this.GetPeordreFinal(this.ListePgeDocument, listeDistillationDescendante, listeDistillationAscendante);

            //Obtenir le preordre Median
            this.PreordreMedian(listeDistillationDescendante, listeDistillationAscendante);

            //Exporter la liste final
            SerializeObject<IEnumerable<PreordreFinalDto>>.XmlSerialize(this.listePreordreFinal, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "PreordreFinal.xml"));
            this.Logger.Info(string.Format("Le calcul est terminé, le fichier des valeurs est dans :{0}", string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "PreordreFinal.xml")));

            //Exporter la liste final Median
            SerializeObject<IEnumerable<PreordreFinalDto>>.XmlSerialize(this.GetOrdreFinal(this.listePreordreFinal), string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "PreordreFinalMedian.xml"));
            this.Logger.Info(string.Format("Le calcul est terminé, le fichier des valeurs est dans :{0}", string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "PreordreFinalMedian.xml")));

            //Enregistrer les valeurs max et min de C1 C2 C3
            this.SaveValeurCriteresMaxMin(programmationDto.NatureTravaux);

            //Enregistrer le resultat de calcul
            if (programmationDto == null)
            {
                throw new ArgumentNullException("newProgrammationsDto est null");
            }

            this.Logger.Info(string.Format("Début d'eregistrement d'une nouvelle programmation de l'année {0}", programmationDto.Anneeprogrammation));
            IMapper<ProgrammationDto, Programmations> mapper = new ProgrammationDtoToProgrammationMapping();
            IMapper<PreordreFinalDto, ProgrammationDocumentPGE> mapperDocument = new PreordreFinalDtoToProgrammationDocumentPgeMapping(this._uow, this.ListePgeFissuration, this.ListePgeInfiltration, this.ListePgeDesordreStructureMaconnerieBeton, this.ListePgeAgeOuvrage, this.ListePgeLargeurOuvrage, this.ListePgeHistoriqueRegenerationMaconnerie, this.ListePgeHistoriqueRegenerationEnduit, this.ListePgeAgressiviteChimiqueTerrainEncaissant, this.ListePgeSolibiliteTerrain, this.ListePgePourrissementBoisBlindage, this.ListePgeDocument, this.ListeCategoriePge);

            var newProgrammation = mapper.Map(programmationDto);
            newProgrammation.ProgrammationDocumentPGE = this.GetOrdreFinal(this.listePreordreFinal).Select(x => mapperDocument.Map(x)).ToList();

            var idNewProgrammation = this._uow.HidalgoRepository.SaveProgrammation(newProgrammation);
            this.Logger.Info(string.Format("Enregistrement est terminé"));
            return idNewProgrammation;
        }
        #endregion

        #region CALCUL DISTILLATION DESCENDANTE

        private Dictionary<string, int> GetDistillationDescendante(IEnumerable<Documents> listeDocumentsPge)
        {
            Dictionary<string, int> listeDistillationDescendante = new Dictionary<string, int>();
            var seuil_0 = listeIndiceCredibiliteDescendante.Values.Max();
            var seuil_fixe = -0.15 * seuil_0 + 0.3;
            float seuil_1 = 0;

            for (int i = 0; i < listeDocumentsPge.Count(); i++)
            {
                this.Pge_i = listeDocumentsPge.ToArray()[i];

                for (int k = 0; k < listeDocumentsPge.Count(); k++)
                {
                    bool isRespectedConditionPreodre1 = false, isRespectedConditionPreordre2 = false;
                    this.Pge_k = listeDocumentsPge.ToArray()[k];

                    //Pour tous les 𝝈(𝒙𝒊,𝒙𝒌) < 𝝀𝟎 −𝒔(𝝀𝟎), choisir celui qui est de valeur maximum et l’attribuer à 𝝀𝟎+𝟏 
                    //Calcul du 𝝀𝟎 −𝒔(𝝀𝟎)
                    var valeurAcomparer = seuil_0 - (seuil_fixe);

                    //obtenir la valeur de l'indice de crédibilité
                    //var indiceCredibilite = listeIndiceCredibiliteDescendante[string.Format("{0}_{1}", listeDocumentsPge.ToArray()[i].NumeroAffaire, listeDocumentsPge.ToArray()[k].NumeroAffaire)];
                    var indiceCredibilite = this.listeIndiceCredibiliteDescendante[string.Format("{0}_{1}", this.Pge_i.NumeroAffaire, this.Pge_k.NumeroAffaire)];

                    // choisir  celui qui est de valeur maximum et l’attribuer à 𝜆𝑙+1 
                    if (indiceCredibilite < valeurAcomparer)
                    {
                        seuil_1 = indiceCredibilite;
                    }
                    else
                    {
                        seuil_1 = 0;
                    }

                    //Calculer les valeurs 𝐪𝐮𝐥+𝟏(𝐱𝐢) de toutes les PGE xi de l’ensemble 𝐃𝐥 
                    if (indiceCredibilite > seuil_1)
                    {
                        isRespectedConditionPreodre1 = true;
                    }

                    var indiceCredibilite2 = listeIndiceCredibiliteDescendante[string.Format("{0}_{1}", this.Pge_k.NumeroAffaire, this.Pge_i.NumeroAffaire)];
                    var valAcomparer = indiceCredibilite2 + (-0.15 * indiceCredibilite + 0.3);

                    if (indiceCredibilite > valAcomparer)
                    {
                        isRespectedConditionPreordre2 = true;
                    }

                    if (isRespectedConditionPreodre1 && isRespectedConditionPreordre2)
                    {
                        listeDistillationDescendante.Add(string.Format("{0}_{1}", this.Pge_i.NumeroAffaire, this.Pge_k.NumeroAffaire), 1);
                    }
                    else
                    {
                        listeDistillationDescendante.Add(string.Format("{0}_{1}", this.Pge_i.NumeroAffaire, this.Pge_k.NumeroAffaire), 0);
                    }
                }
            }

            return listeDistillationDescendante;
        }
        public IList<ClassementDistillationDto> CalculDistillationDescendante(IList<Documents> listeDocumentsPge)
        {
            this.Logger.Info("ETAPE 2: DISTIATION DESCENDANTE");

            while (listeDocumentsPge.Count() > 0)
            {
                try
                {


                    Dictionary<string, float> listeIndiceCredibiliteEtape2 = new Dictionary<string, float>();
                    IList<QuallificationPge> listeQualificationsPge = new List<QuallificationPge>();
                    IList<QuallificationPge> listeQualificationsPgeEtape2 = new List<QuallificationPge>();
                    Dictionary<string, int> listeDistillationDescendanteEtape2 = new Dictionary<string, int>();

                    var listeDistillationDescendante = this.GetDistillationDescendante(listeDocumentsPge);

                    //Tant que la liste des PGE n'est pas encore soustrait completement l'opération est refait

                    //Calcul Puissance et Faiblesse et la qualification de chaque PGE
                    for (int i = 0; i < listeDocumentsPge.Count(); i++)
                    {
                        int puissaance = 0, faiblesse = 0;
                        var qualificationPge = new QuallificationPge { NumeroAffaire = listeDocumentsPge.ToArray()[i].NumeroAffaire };
                        for (int k = 0; k < listeDocumentsPge.Count(); k++)
                        {
                            puissaance += listeDistillationDescendante[string.Format("{0}_{1}", listeDocumentsPge.ToArray()[i].NumeroAffaire, listeDocumentsPge.ToArray()[k].NumeroAffaire)];
                            faiblesse += listeDistillationDescendante[string.Format("{0}_{1}", listeDocumentsPge.ToArray()[k].NumeroAffaire, listeDocumentsPge.ToArray()[i].NumeroAffaire)];
                        }

                        qualificationPge.Faiblesse = faiblesse;
                        qualificationPge.Puissance = puissaance;
                        qualificationPge.Qualification = puissaance - faiblesse;
                        listeQualificationsPge.Add(qualificationPge);
                    }

                    int qualificationMax = listeQualificationsPge.Max(x => x.Qualification);
                    var listePgeExiste = listeQualificationsPge.Where(x => x.Qualification == qualificationMax).ToList();
                    if (listePgeExiste != null && listePgeExiste.Count() == 1)
                    {
                        var numdocument = listeQualificationsPge.First().NumeroAffaire;
                        rang += 1;
                        listeClassementDistillationDescendante.Add(new ClassementDistillationDto { NumeroAffaire = numdocument, Classement = rang });

                        //soutraire la PGE de la liste des indices de crédibilité pour refaire l'opération
                        for (int i = 0; i < listeDocumentsPge.Count(); i++)
                        {
                            listeIndiceCredibiliteDescendante.Remove(string.Format("{0}_{1}", numdocument, this.ListePgeDocument.ToArray()[i].NumeroAffaire));
                            listeIndiceCredibiliteDescendante.Remove(string.Format("{0}_{1}", this.ListePgeDocument.ToArray()[i].NumeroAffaire, numdocument));
                        }

                        //soustraire la pge pour refaire l'opération sur le reste des PGE
                        Documents documentToDelete = listeDocumentsPge.First(x => x.NumeroAffaire == numdocument);
                        listeDocumentsPge.Remove(documentToDelete);

                        //this.ListePgeDocument.Remove(documentToDelete);
                        //if (listeDocumentsPge.Remove(documentToDelete))
                        //{
                        //    this.CalculDistillationDescendante(listeDocumentsPge);
                        //}
                    }
                    else
                    {
                        //ETAPE 2
                        //Obtenir la nouvelle liste des indices de crédibilité pour les PGE existe
                        var listeDocumentEtape2 = listeDocumentsPge.Where(x => listePgeExiste.Any(xx => xx.NumeroAffaire == x.NumeroAffaire)).ToList();

                        for (int i = 0; i < listeDocumentEtape2.Count(); i++)
                        {
                            for (int k = 0; k < listeDocumentEtape2.Count(); k++)
                            {
                                var key = string.Format("{0}_{1}", listeDocumentEtape2.ToArray()[i].NumeroAffaire, listeDocumentEtape2.ToArray()[k].NumeroAffaire);
                                var valeur = listeIndiceCredibiliteDescendante[key];
                                listeIndiceCredibiliteEtape2.Add(key, valeur);
                            }
                        }

                        var seuil_2 = listeIndiceCredibiliteEtape2.Values.Max();
                        var seuil_2_fixe = -0.15 * seuil_2 + 0.3;
                        float seuil_x = 0;

                        for (int i = 0; i < listePgeExiste.Count(); i++)
                        {
                            for (int k = 0; k < listePgeExiste.Count(); k++)
                            {
                                bool isRespectedConditionPreodre1 = false, isRespectedConditionPreordre2 = false;

                                //Pour tous les 𝝈(𝒙𝒊,𝒙𝒌) < 𝝀𝟎 −𝒔(𝝀𝟎), choisir celui qui est de valeur maximum et l’attribuer à 𝝀𝟎+𝟏 
                                //Calcul du 𝝀𝟎 −𝒔(𝝀𝟎)
                                var valeurAcomparer = seuil_2 - (seuil_2_fixe);

                                //obtenir la valeur de l'indice de crédibilité
                                var indiceCredibilite = listeIndiceCredibiliteEtape2[string.Format("{0}_{1}", listePgeExiste.ToArray()[i].NumeroAffaire, listePgeExiste.ToArray()[k].NumeroAffaire)];

                                // choisir  celui qui est de valeur maximum et l’attribuer à 𝜆𝑙+1 
                                if (indiceCredibilite < valeurAcomparer)
                                {
                                    seuil_x = indiceCredibilite;
                                }
                                else
                                {
                                    seuil_x = 0;
                                }

                                //Calculer les valeurs 𝐪𝐮𝐥+𝟏(𝐱𝐢) de toutes les PGE xi de l’ensemble 𝐃𝐥 
                                if (indiceCredibilite > seuil_x)
                                {
                                    isRespectedConditionPreodre1 = true;
                                }

                                var indiceCredibilite2 = listeIndiceCredibiliteEtape2[string.Format("{0}_{1}", listePgeExiste.ToArray()[k].NumeroAffaire, listePgeExiste.ToArray()[i].NumeroAffaire)];
                                var valAcomparer = indiceCredibilite2 + (-0.15 * indiceCredibilite + 0.3);

                                if (indiceCredibilite > valAcomparer)
                                {
                                    isRespectedConditionPreordre2 = true;
                                }

                                if (isRespectedConditionPreodre1 && isRespectedConditionPreordre2)
                                {
                                    listeDistillationDescendanteEtape2.Add(string.Format("{0}_{1}", listePgeExiste.ToArray()[i].NumeroAffaire, listePgeExiste.ToArray()[k].NumeroAffaire), 1);
                                }
                                else
                                {
                                    listeDistillationDescendanteEtape2.Add(string.Format("{0}_{1}", listePgeExiste.ToArray()[i].NumeroAffaire, listePgeExiste.ToArray()[k].NumeroAffaire), 0);
                                }
                            }
                        }

                        //Calcul Puissance et Faiblesse et la qualification de chaque PGE
                        for (int i = 0; i < listePgeExiste.Count(); i++)
                        {
                            int puissaance = 0, faiblesse = 0;
                            var qualificationPge = new QuallificationPge { NumeroAffaire = listePgeExiste.ToArray()[i].NumeroAffaire };
                            for (int k = 0; k < listePgeExiste.Count(); k++)
                            {
                                puissaance += listeDistillationDescendanteEtape2[string.Format("{0}_{1}", listePgeExiste.ToArray()[i].NumeroAffaire, listePgeExiste.ToArray()[k].NumeroAffaire)];
                                faiblesse += listeDistillationDescendanteEtape2[string.Format("{0}_{1}", listePgeExiste.ToArray()[k].NumeroAffaire, listePgeExiste.ToArray()[i].NumeroAffaire)];
                            }

                            qualificationPge.Faiblesse = faiblesse;
                            qualificationPge.Puissance = puissaance;
                            qualificationPge.Qualification = puissaance - faiblesse;
                            listeQualificationsPgeEtape2.Add(qualificationPge);
                        }

                        int qualificationMaxEtape2 = listeQualificationsPgeEtape2.Max(x => x.Qualification);
                        var listePgeExisteEtape2 = listeQualificationsPgeEtape2.Where(x => x.Qualification == qualificationMaxEtape2).ToList();


                        //Soustraire la valeur Max de la qualification
                        //sousutraire les pge qui reponds au critère
                        rang += 1;
                        foreach (var pge in listePgeExisteEtape2)
                        {
                            var numdocument = pge.NumeroAffaire;

                            listeClassementDistillationDescendante.Add(new ClassementDistillationDto { NumeroAffaire = numdocument, Classement = rang });


                            for (int i = 0; i < listeDocumentsPge.Count(); i++)
                            {
                                listeIndiceCredibiliteDescendante.Remove(string.Format("{0}_{1}", numdocument, this.ListePgeDocument.ToArray()[i].NumeroAffaire));
                                listeIndiceCredibiliteDescendante.Remove(string.Format("{0}_{1}", this.ListePgeDocument.ToArray()[i].NumeroAffaire, numdocument));
                            }

                            //soustraire la pge pour refaire l'opération sur le reste des PGE
                            Documents documentToDelete = listeDocumentsPge.First(x => x.NumeroAffaire == numdocument);
                            //this.ListePgeDocument.Remove(documentToDelete);
                            listeDocumentsPge.Remove(documentToDelete);
                        }

                        //if (listeDocumentsPge.Any())
                        //{
                        //    this.CalculDistillationDescendante(listeDocumentsPge);
                        //}
                    } // fin Etape2
                }
                catch (StackOverflowException ex)
                {
                    this.Logger.Error(string.Format("Erreur est détecté dans la méthode CalculDistillationDescendante: /t{0}", ex.Message));
                }
                catch (Exception ex)
                {
                    this.Logger.Error(string.Format("Une exception est levé,  la méthode ciblé CalculDistillationDescendante: /t{0}", ex.Message));
                }
            }

            //TODO : suite au demande d'angel, a besoin de savoir le détail de la matrice de distillation descendante.
            SerializeObject<IEnumerable<ClassementDistillationDto>>.XmlSerialize(listeClassementDistillationDescendante, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "MatriceDistillationDescendante.xml"));
            return listeClassementDistillationDescendante;
        }
        #endregion

        #region CALCUL DISTILLATION ASCENDANTE
        public IList<ClassementDistillationDto> CalculDistillationAscendante(IList<Documents> listeDocumentsPge)
        {
            this.Logger.Info("ETAPE 2: DISTIATION DESCENDANTE & ASCENDANTE");
            try
            {
                while (listeDocumentsPge.Count() > 0)
                {

                    Dictionary<string, int> listeDistillationAscendante = new Dictionary<string, int>();
                    Dictionary<string, int> listeDistillationAscendanteEtape2 = new Dictionary<string, int>();
                    Dictionary<string, float> listeIndiceCredibiliteEtape2 = new Dictionary<string, float>();
                    IList<QuallificationPge> listeQualificationsPge = new List<QuallificationPge>();
                    IList<QuallificationPge> listeQualificationsPgeEtape2 = new List<QuallificationPge>();

                    var seuil_0 = listeIndiceCredibiliteAscendante.Values.Max();
                    var seuil_fixe = -0.15 * seuil_0 + 0.3;
                    float seuil_1 = 0;

                    for (int i = 0; i < listeDocumentsPge.Count(); i++)
                    {
                        for (int k = 0; k < listeDocumentsPge.Count(); k++)
                        {
                            bool isRespectedConditionPreodre1 = false, isRespectedConditionPreordre2 = false;

                            //Pour tous les 𝝈(𝒙𝒊,𝒙𝒌) < 𝝀𝟎 −𝒔(𝝀𝟎), choisir celui qui est de valeur maximum et l’attribuer à 𝝀𝟎+𝟏 
                            //Calcul du 𝝀𝟎 −𝒔(𝝀𝟎)
                            var valeurAcomparer = seuil_0 - (seuil_fixe);

                            //obtenir la valeur de l'indice de crédibilité
                            var indiceCredibilite = listeIndiceCredibiliteAscendante[string.Format("{0}_{1}", listeDocumentsPge.ToArray()[i].NumeroAffaire, listeDocumentsPge.ToArray()[k].NumeroAffaire)];

                            // choisir  celui qui est de valeur maximum et l’attribuer à 𝜆𝑙+1 
                            if (indiceCredibilite < valeurAcomparer)
                            {
                                seuil_1 = indiceCredibilite;
                            }
                            else
                            {
                                seuil_1 = 0;
                            }

                            //Calculer les valeurs 𝐪𝐮𝐥+𝟏(𝐱𝐢) de toutes les PGE xi de l’ensemble 𝐃𝐥 
                            if (indiceCredibilite > seuil_1)
                            {
                                isRespectedConditionPreodre1 = true;
                            }

                            var indiceCredibilite2 = listeIndiceCredibiliteAscendante[string.Format("{0}_{1}", listeDocumentsPge.ToArray()[k].NumeroAffaire, listeDocumentsPge.ToArray()[i].NumeroAffaire)];
                            var valAcomparer = indiceCredibilite2 + (-0.15 * indiceCredibilite + 0.3);

                            if (indiceCredibilite > valAcomparer)
                            {
                                isRespectedConditionPreordre2 = true;
                            }

                            if (isRespectedConditionPreodre1 && isRespectedConditionPreordre2)
                            {
                                listeDistillationAscendante.Add(string.Format("{0}_{1}", listeDocumentsPge.ToArray()[i].NumeroAffaire, listeDocumentsPge.ToArray()[k].NumeroAffaire), 1);
                            }
                            else
                            {
                                listeDistillationAscendante.Add(string.Format("{0}_{1}", listeDocumentsPge.ToArray()[i].NumeroAffaire, listeDocumentsPge.ToArray()[k].NumeroAffaire), 0);
                            }
                        }
                    }

                    //Calcul Puissance et Faiblesse et la qualification de chaque PGE
                    for (int i = 0; i < listeDocumentsPge.Count(); i++)
                    {
                        int puissaance = 0, faiblesse = 0;
                        var qualificationPge = new QuallificationPge { NumeroAffaire = listeDocumentsPge.ToArray()[i].NumeroAffaire };
                        for (int k = 0; k < listeDocumentsPge.Count(); k++)
                        {
                            puissaance += listeDistillationAscendante[string.Format("{0}_{1}", listeDocumentsPge.ToArray()[i].NumeroAffaire, listeDocumentsPge.ToArray()[k].NumeroAffaire)];
                            faiblesse += listeDistillationAscendante[string.Format("{0}_{1}", listeDocumentsPge.ToArray()[k].NumeroAffaire, listeDocumentsPge.ToArray()[i].NumeroAffaire)];
                        }

                        qualificationPge.Faiblesse = faiblesse;
                        qualificationPge.Puissance = puissaance;
                        qualificationPge.Qualification = puissaance - faiblesse;
                        listeQualificationsPge.Add(qualificationPge);
                    }

                    //Identification de la valeur 𝐪𝐮𝐥+𝟏(𝐱𝐢) minimum : 𝒒𝒖𝑫𝒍 
                    int qualificationMin = listeQualificationsPge.Min(x => x.Qualification);

                    var listePgeExiste = listeQualificationsPge.Where(x => x.Qualification == qualificationMin).ToList();

                    if (listePgeExiste != null && listePgeExiste.Count() == 1)
                    {
                        var numdocument = listeQualificationsPge.First().NumeroAffaire;
                        rang += 1;
                        listeClassementDistillationAscendante.Add(new ClassementDistillationDto { NumeroAffaire = numdocument, Classement = rang });


                        //soutraire la PGE de la liste des indices de crédibilité pour refaire l'opération
                        IList<string> listeKeyASupprimer = new List<string>();
                        for (int i = 0; i < this.ListePgeDocument.Count(); i++)
                        {
                            listeIndiceCredibiliteAscendante.Remove(string.Format("{0}_{1}", numdocument, this.ListePgeDocument.ToArray()[i].NumeroAffaire));
                            listeIndiceCredibiliteAscendante.Remove(string.Format("{0}_{1}", this.ListePgeDocument.ToArray()[i].NumeroAffaire, numdocument));
                        }

                        //soustraire la pge pour refaire l'opération sur le reste des PGE
                        Documents documentToDelete = this.ListePgeDocument.First(x => x.NumeroAffaire == numdocument);
                        //this.ListePgeDocument.Remove(documentToDelete);
                        listeDocumentsPge.Remove(documentToDelete);

                        //Eeffaire le calcul
                        //this.CalculDistillationAscendante(listeDocumentsPge);
                    }
                    else
                    {
                        //ETAPE 2
                        //Obtenir la nouvelle liste des indices de crédibilité pour les PGE existe
                        var listeDocumentEtape2 = listeDocumentsPge.Where(x => listePgeExiste.Any(xx => xx.NumeroAffaire == x.NumeroAffaire)).ToList();

                        for (int i = 0; i < listeDocumentEtape2.Count(); i++)
                        {
                            for (int k = 0; k < listeDocumentEtape2.Count(); k++)
                            {
                                var key = string.Format("{0}_{1}", listeDocumentEtape2.ToArray()[i].NumeroAffaire, listeDocumentEtape2.ToArray()[k].NumeroAffaire);
                                var valeur = listeIndiceCredibiliteAscendante[key];
                                listeIndiceCredibiliteEtape2.Add(key, valeur);
                            }
                        }

                        var seuil_2 = listeIndiceCredibiliteEtape2.Values.Max();
                        var seuil_2_fixe = -0.15 * seuil_2 + 0.3;
                        float seuil_x = 0;

                        for (int i = 0; i < listePgeExiste.Count(); i++)
                        {
                            for (int k = 0; k < listePgeExiste.Count(); k++)
                            {
                                bool isRespectedConditionPreodre1 = false, isRespectedConditionPreordre2 = false;

                                //Pour tous les 𝝈(𝒙𝒊,𝒙𝒌) < 𝝀𝟎 −𝒔(𝝀𝟎), choisir celui qui est de valeur maximum et l’attribuer à 𝝀𝟎+𝟏 
                                //Calcul du 𝝀𝟎 −𝒔(𝝀𝟎)
                                var valeurAcomparer = seuil_2 - (seuil_2_fixe);

                                //obtenir la valeur de l'indice de crédibilité
                                var indiceCredibilite = listeIndiceCredibiliteEtape2[string.Format("{0}_{1}", listePgeExiste.ToArray()[i].NumeroAffaire, listePgeExiste.ToArray()[k].NumeroAffaire)];

                                // choisir  celui qui est de valeur maximum et l’attribuer à 𝜆𝑙+1 
                                if (indiceCredibilite < valeurAcomparer)
                                {
                                    seuil_x = indiceCredibilite;
                                }
                                else
                                {
                                    seuil_x = 0;
                                }

                                //Calculer les valeurs 𝐪𝐮𝐥+𝟏(𝐱𝐢) de toutes les PGE xi de l’ensemble 𝐃𝐥 
                                if (indiceCredibilite > seuil_x)
                                {
                                    isRespectedConditionPreodre1 = true;
                                }

                                var indiceCredibilite2 = listeIndiceCredibiliteEtape2[string.Format("{0}_{1}", listePgeExiste.ToArray()[k].NumeroAffaire, listePgeExiste.ToArray()[i].NumeroAffaire)];
                                var valAcomparer = indiceCredibilite2 + (-0.15 * indiceCredibilite + 0.3);

                                if (indiceCredibilite > valAcomparer)
                                {
                                    isRespectedConditionPreordre2 = true;
                                }

                                if (isRespectedConditionPreodre1 && isRespectedConditionPreordre2)
                                {
                                    listeDistillationAscendanteEtape2.Add(string.Format("{0}_{1}", listePgeExiste.ToArray()[i].NumeroAffaire, listePgeExiste.ToArray()[k].NumeroAffaire), 1);
                                }
                                else
                                {
                                    listeDistillationAscendanteEtape2.Add(string.Format("{0}_{1}", listePgeExiste.ToArray()[i].NumeroAffaire, listePgeExiste.ToArray()[k].NumeroAffaire), 0);
                                }
                            }
                        }

                        //Calcul Puissance et Faiblesse et la qualification de chaque PGE
                        for (int i = 0; i < listePgeExiste.Count(); i++)
                        {
                            int puissaance = 0, faiblesse = 0;
                            var qualificationPge = new QuallificationPge { NumeroAffaire = listePgeExiste.ToArray()[i].NumeroAffaire };
                            for (int k = 0; k < listePgeExiste.Count(); k++)
                            {
                                puissaance += listeDistillationAscendanteEtape2[string.Format("{0}_{1}", listePgeExiste.ToArray()[i].NumeroAffaire, listePgeExiste.ToArray()[k].NumeroAffaire)];
                                faiblesse += listeDistillationAscendanteEtape2[string.Format("{0}_{1}", listePgeExiste.ToArray()[k].NumeroAffaire, listePgeExiste.ToArray()[i].NumeroAffaire)];
                            }

                            qualificationPge.Faiblesse = faiblesse;
                            qualificationPge.Puissance = puissaance;
                            qualificationPge.Qualification = puissaance - faiblesse;
                            listeQualificationsPgeEtape2.Add(qualificationPge);
                        }

                        int qualificationMinEtape2 = listeQualificationsPgeEtape2.Min(x => x.Qualification);
                        var listePgeExisteEtape2 = listeQualificationsPgeEtape2.Where(x => x.Qualification == qualificationMinEtape2).ToList();


                        //Soustraire la valeur Max de la qualification
                        //sousutraire les pge qui reponds au critère
                        rang += 1;
                        foreach (var pge in listePgeExisteEtape2)
                        {
                            var numdocument = pge.NumeroAffaire;

                            listeClassementDistillationAscendante.Add(new ClassementDistillationDto { NumeroAffaire = numdocument, Classement = rang });

                            for (int i = 0; i < listeDocumentsPge.Count(); i++)
                            {
                                listeIndiceCredibiliteAscendante.Remove(string.Format("{0}_{1}", numdocument, this.ListePgeDocument.ToArray()[i].NumeroAffaire));
                                listeIndiceCredibiliteAscendante.Remove(string.Format("{0}_{1}", this.ListePgeDocument.ToArray()[i].NumeroAffaire, numdocument));
                            }

                            //soustraire la pge pour refaire l'opération sur le reste des PGE
                            Documents documentToDelete = listeDocumentsPge.First(x => x.NumeroAffaire == numdocument);

                            //this.ListePgeDocument.Remove(documentToDelete);
                            listeDocumentsPge.Remove(documentToDelete);
                        }

                        //this.CalculDistillationAscendante(listeDocumentsPge);
                    }
                }
            }
            catch (StackOverflowException ex)
            {
                this.Logger.Error(string.Format("Une exception est levée dans la méthode CalculDistillationAscendante: /t{0}", ex.Message));
            }

            catch (Exception ex)
            {
                this.Logger.Error(string.Format("Une exception est levé,  la méthode ciblé CalculDistillationAscendante: /t{0}", ex.Message));
            }

            SerializeObject<IEnumerable<ClassementDistillationDto>>.XmlSerialize(listeClassementDistillationAscendante, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "MatriceDistillationAscendante.xml"));
            return listeClassementDistillationAscendante;
        }
        #endregion

        public IEnumerable<TypeOuvragesDto> GetAllTypeOuvrages()
        {
            var listeTypesOuvragesMetier = this._uow.HidalgoRepository.GetAllTypeOuvrages();
            IMapper<TypesOuvrages, TypeOuvragesDto> mapper = new TypeOuvragesToTypeOuvragesDtoMapping();
            IList<TypeOuvragesDto> listetypeOuvrageDto = new List<TypeOuvragesDto>();
            foreach (var item in listeTypesOuvragesMetier)
            {
                switch (item.Libelle)
                {
                    case "Tunnel":
                    case "Couloir":
                        if (!listetypeOuvrageDto.Any())
                        {
                            listetypeOuvrageDto.Insert(0, new TypeOuvragesDto { Libelle = "Tunnel + Couloir", Id = 1 });
                        }
                        else
                        {
                            if (listetypeOuvrageDto.ToArray()[0] != null && listetypeOuvrageDto.ToArray()[0].Id != 1)
                                listetypeOuvrageDto.Insert(0, new TypeOuvragesDto { Libelle = "Tunnel + Couloir", Id = 1 });
                        }

                        break;
                    default:
                        listetypeOuvrageDto.Add(mapper.Map(item));
                        break;

                }
            }

            return listetypeOuvrageDto;
        }

        #region PRIVE METHODES
        private void SaveValeurCriteresMaxMin(ENatureCalibrage natureTravaux)
        {
            var valeurMaxC1 = this.listeCritereC1.Max(x => x.valeur);
            var valeurMaxC2 = this.listeCritereC2.Max(x => x.valeur);
            var valeurMaxC3 = this.listeCritereC3.Max(x => x.valeur);

            var valeurMinC1 = this.listeCritereC1.Min(x => x.valeur);
            var valeurMinC2 = this.listeCritereC2.Min(x => x.valeur);
            var valeurMinC3 = this.listeCritereC3.Min(x => x.valeur);

            var coefficientParamPonderationMaxC1 = this.ListeCoefficientPonderation.Where(x => x.Code == "MaxC1").First();
            var coefficientParamPonderationMaxC2 = this.ListeCoefficientPonderation.Where(x => x.Code == "MaxC2").First();
            var coefficientParamPonderationMaxC3 = this.ListeCoefficientPonderation.Where(x => x.Code == "MaxC3").First();
            var coefficientParamPonderationMinC1 = this.ListeCoefficientPonderation.Where(x => x.Code == "MinC1").First();
            var coefficientParamPonderationMinC2 = this.ListeCoefficientPonderation.Where(x => x.Code == "MinC2").First();
            var coefficientParamPonderationMinC3 = this.ListeCoefficientPonderation.Where(x => x.Code == "MinC3").First();
            var coefficientParamPonderationMaxB4C1 = this.ListeCoefficientPonderation.Where(x => x.Code == "B4C1").First();
            var coefficientParamPonderationMaxB4C2 = this.ListeCoefficientPonderation.Where(x => x.Code == "B4C2").First();
            var coefficientParamPonderationMaxB4C3 = this.ListeCoefficientPonderation.Where(x => x.Code == "B4C3").First();

            switch (natureTravaux)
            {
                case ENatureCalibrage.Maçonnerie:
                    coefficientParamPonderationMaxC1.ValeurMaconnerie = (decimal)valeurMaxC1;
                    coefficientParamPonderationMaxC2.ValeurMaconnerie = (decimal)valeurMaxC2;
                    coefficientParamPonderationMaxC3.ValeurMaconnerie = (decimal)valeurMaxC3;

                    coefficientParamPonderationMinC1.ValeurMaconnerie = (decimal)valeurMinC1;
                    coefficientParamPonderationMinC2.ValeurMaconnerie = (decimal)valeurMinC2;
                    coefficientParamPonderationMinC3.ValeurMaconnerie = (decimal)valeurMinC3;

                    coefficientParamPonderationMaxB4C1.ValeurMaconnerie = (decimal)valeurMaxC1;
                    coefficientParamPonderationMaxB4C2.ValeurMaconnerie = (decimal)valeurMaxC2;
                    coefficientParamPonderationMaxB4C3.ValeurMaconnerie = (decimal)valeurMaxC3;

                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMaxC1);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMaxC2);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMaxC3);

                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMinC1);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMinC2);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMinC3);

                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMaxB4C1);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMaxB4C2);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreMaconnerie(coefficientParamPonderationMaxB4C3);
                    break;

                case ENatureCalibrage.Enduit:

                    coefficientParamPonderationMaxC1.ValeurEnduit = (decimal)valeurMaxC1;
                    coefficientParamPonderationMaxC2.ValeurEnduit = (decimal)valeurMaxC2;
                    coefficientParamPonderationMaxC3.ValeurEnduit = (decimal)valeurMaxC3;

                    coefficientParamPonderationMinC1.ValeurEnduit = (decimal)valeurMinC1;
                    coefficientParamPonderationMinC2.ValeurEnduit = (decimal)valeurMinC2;
                    coefficientParamPonderationMinC3.ValeurEnduit = (decimal)valeurMinC3;

                    coefficientParamPonderationMaxB4C1.ValeurEnduit = (decimal)valeurMaxC1;
                    coefficientParamPonderationMaxB4C2.ValeurEnduit = (decimal)valeurMaxC2;
                    coefficientParamPonderationMaxB4C3.ValeurEnduit = (decimal)valeurMaxC3;

                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMaxC1);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMaxC2);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMaxC3);

                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMinC1);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMinC2);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMinC3);

                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMaxB4C1);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMaxB4C2);
                    this._uow.CalibrageRepositorie.UpdateCoefficientPonderationParametreEnduit(coefficientParamPonderationMaxB4C3);

                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private IEnumerable<int> GetAllOuvrages(Documents doc)
        {
            IList<int> listeIdOuvrage = new List<int>();
            foreach (var docDesordre in doc.DesordresDocuments)
            {
                if (docDesordre.Desordres != null && docDesordre.Desordres.Localisations != null)
                {
                    listeIdOuvrage.Add(docDesordre.Desordres.Localisations.IdOuvragePrincipal.GetValueOrDefault());
                }
            }

            return listeIdOuvrage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valeur1"></param>
        /// <param name="valeur2"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        private float GetIndiceDiscordance(double valeur1, double valeur2, double v1)
        {
            var resultatdiscordance = valeur1 - valeur2;
            float indiceDiscordance = default(float);
            if (resultatdiscordance > v1)
            {
                indiceDiscordance = 1;
            }
            else if (resultatdiscordance <= 0)
            {
                indiceDiscordance = 0;
            }
            else
            {
                indiceDiscordance = (float)(resultatdiscordance / v1);
            }

            //Logger.Info(string.Format("Valeur1 : {0} ; valeur2 : {1} ; v1 : {2} ; Resultat Valeur1-valeur2 : {3}  => Indice : {4} ", valeur1, valeur2, v1, resultatdiscordance, indiceDiscordance)); // RDT Angel demande les indices de discordance (je ne le genere pas en XML)
            return indiceDiscordance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indiceDiscordance"></param>
        /// <param name="indiceConcordanceGlobale"></param>
        /// <returns></returns>
        private float GetIndiceCredibiiteEtapeCalcul2(params float[] valeurs)
        {
            if (valeurs[0] > valeurs[1])
            {
                if ((1 - valeurs[1] <= 0) || (1 - valeurs[0] <= 0))
                {
                    return default(float);
                }

                return valeurs[1] * ((1 - valeurs[0]) / (1 - valeurs[1]));
            }
            else if (valeurs[0] < valeurs[1])
            {
                return valeurs[1];
            }

            return 0;
        }

        /// <summary>
        /// Obtient la note max entre les paramètres
        /// </summary>
        /// <param name="notes">tableau des notes à comparer</param>
        /// <returns>la note max</returns>
        private double GetNoteMax(params double[] notes)
        {
            return notes.AsEnumerable().Max();
        }

        /// <summary>
        /// Méthode permettant d'obrtenir la note selon l'age de louvrage
        /// </summary>
        /// <param name="ageouvrage">l'age de l'ouvrage</param>
        /// <returns>la note par rapport au l'age de l'ouvrage</returns>
        private int GetNoteAgeOuvrage(int ageouvrage)
        {
            if (ageouvrage >= 0 && ageouvrage <= 14)
            {
                return 1;
            }

            if (ageouvrage >= 15 && ageouvrage <= 34)
            {
                return 2;
            }

            if (ageouvrage >= 35 && ageouvrage <= 54)
            {
                return 3;
            }

            if (ageouvrage >= 55 && ageouvrage <= 74)
            {
                return 4;
            }

            if (ageouvrage >= 75 && ageouvrage <= 100)
            {
                return 5;
            }

            if (ageouvrage > 100)
            {
                return 7;
            }

            return 0;
        }

        /// <summary>
        /// Méthode permettant d'obtenir la note par rapport au différence de la date travaux et date programmation
        /// </summary>
        /// <param name="difDateTravaux">différence entre l'année de programmation et date fin de travaux </param>
        /// <returns>la note</returns>
        private int GetNoteHistoriqueRegenerationMaconnerie(int difDateTravaux)
        {
            if (difDateTravaux >= 0 && difDateTravaux <= 14)
            {
                return -12;
            }

            if (difDateTravaux >= 15 && difDateTravaux <= 29)
            {
                return -10;
            }

            if (difDateTravaux >= 30 && difDateTravaux <= 39)
            {
                return -8;
            }

            if (difDateTravaux >= 40 && difDateTravaux <= 49)
            {
                return -6;
            }

            if (difDateTravaux >= 50 && difDateTravaux <= 59)
            {
                return -4;
            }


            if (difDateTravaux >= 60 && difDateTravaux <= 69)
            {
                return -2;
            }

            if (difDateTravaux >= 70)
            {
                return 0;
            }

            //TODO YAF: 22/05/2017  à reverifier
            return 0;
        }

        /// <summary>
        /// Méthode permettant d'obtenir la note par rapport au différence de la date travaux et date programmation
        /// </summary>
        /// <param name="difDateTravaux">différence entre l'année de programmation et date fin de travaux </param>
        /// <returns>la note</returns>
        private int GetNoteHistoriqueRegenerationEnduit(int difDateTravaux)
        {
            if (difDateTravaux >= 0 && difDateTravaux <= 5)
            {
                return -6;
            }

            if (difDateTravaux >= 6 && difDateTravaux <= 11)
            {
                return -5;
            }

            if (difDateTravaux >= 12 && difDateTravaux <= 17)
            {
                return -4;
            }

            if (difDateTravaux >= 18 && difDateTravaux <= 23)
            {
                return -3;
            }

            if (difDateTravaux >= 24 && difDateTravaux <= 29)
            {
                return -2;
            }

            if (difDateTravaux >= 30 && difDateTravaux <= 35)
            {
                return -1;
            }

            if (difDateTravaux > 35)
            {
                return 0;
            }

            //TODO YAF: 22/05/2017  à reverifier
            return 0;
        }

        /// <summary>
        /// Méthode permettant d'obtenir la note du paramètre C2 Fréquentation
        /// </summary>
        /// <param name="ligneName">le nom de la Lignes</param>
        /// <returns>la note trouvé</returns>
        private int GetNoteOfParametreFrequentation(IEnumerable<Ouvrages> listeOuvrage)
        {
            int note = 0;
            foreach (var ouvrage in listeOuvrage)
            {
                switch (ouvrage.Lieux.Lignes.Nom.ToLower())
                {
                    case "Lignes 3b":
                    case "Lignes 7b":
                        note = 1;
                        break;

                    case "Lignes 10":
                    case "Lignes 11":
                        note = 2;
                        break;

                    case "Lignes 12":
                    case "Lignes 14":
                        note = 3;
                        break;

                    case "Lignes 2":
                    case "Lignes 3":
                    case "Lignes 5":
                    case "Lignes 6":
                    case "Lignes 7":
                    case "Lignes 8":
                    case "Lignes 9":
                        note = 4;
                        break;

                    case "Lignes 1":
                    case "Lignes 4":
                    case "Lignes 13":
                        note = 5;
                        break;

                    case "rer a saint germain en laye - gare de lyon":
                    case "rer a gare de lyon - boissy saint leger":
                    case "rer b gare du nord - saint-rémy-lès-chevreuse":
                    case "rer a val de fontenay - chessy ;arne la vallée":
                    case "rer b bourg la Reine - robinson":
                        note = 7;
                        break;
                }
            }

            return note;
        }

        /// <summary>
        /// Méthode permettant d'obtenir la note du paramètre C2 IMAGE RATP
        /// </summary>
        /// <param name="ligneName">le nom de la Lignes</param>
        /// <returns>la note trouvé</returns>
        private int GetNoteOfParametreImageRatp(IEnumerable<Ouvrages> listeOuvrage)
        {
            int note = 0;
            foreach (var ouvrage in listeOuvrage)
            {
                switch (ouvrage.Lieux.Lignes.Nom.ToLower())
                {
                    case "Lignes 2":
                    case "Lignes 3":
                    case "Lignes 3b":
                    case "Lignes 5":
                    case "Lignes 6":
                    case "Lignes 7":
                    case "Lignes 7b":
                    case "Lignes 8":
                    case "Lignes 9":
                    case "Lignes 10":
                    case "Lignes 11":
                    case "Lignes 12":
                    case "Lignes 13":
                        note = 0;
                        break;

                    case "Lignes 1":
                    case "Lignes 4":
                    case "Lignes 14":
                        note = 3;
                        break;
                }
            }

            return note;
        }

        /// <summary>
        /// Obtenir le symbole de classement des pge
        /// </summary>
        /// <param name="pessimite"></param>
        /// <param name="optimiste"></param>
        /// <returns></returns>
        private char GetClassement(float pessimite, float optimiste)
        {
            if (pessimite >= seuilCoup && optimiste < seuilCoup)
            {
                return '>';
            }

            if (pessimite < seuilCoup && optimiste >= seuilCoup)
            {
                return '<';
            }

            return 'X';
        }

        /// <summary>
        /// Calcul PESSIMISTE & OPTIMISTE
        /// </summary>
        /// <param name="natureTravaux"></param>
        /// <param name="listeCoefficientPonderation"></param>
        /// <param name="valeurPgeC1"></param>
        /// <param name="valeurPgeC2"></param>
        /// <param name="valeurPgeC3"></param>
        /// <param name="seuilreferenceACalculer"></param>
        /// <param name="typeCalcul"></param>
        /// <returns></returns>
        private float GetIndiceCredibilite(ENatureCalibrage natureTravaux, double valeurPgeC1, double valeurPgeC2, double valeurPgeC3, ESeuilReference seuilreferenceACalculer, ETypeCalcul typeCalcul)
        {
            int indiceConcordancePartialC1, indiceConcordancePartialC2, indiceConcordancePartialC3;
            double resulC1, resulC2, resulC3;
            double b0C1 = 0, b0C2 = 0, b0C3 = 0, b1C1 = 0, b1C2 = 0, b1C3 = 0, b2C1 = 0, b2C2 = 0, b2C3 = 0, b3C1 = 0, b3C2 = 0, b3C3 = 0, b4C1 = 0, b4C2 = 0, b4C3 = 0;
            float indiceConcordanceGlobale, indiceCredibilite = 0;

            switch (natureTravaux)
            {
                case ENatureCalibrage.Maçonnerie:
                    b0C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B0C1").Single().ValeurMaconnerie.GetValueOrDefault();
                    b0C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B0C2").Single().ValeurMaconnerie.GetValueOrDefault();
                    b0C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B0C3").Single().ValeurMaconnerie.GetValueOrDefault();

                    b1C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B1C1").Single().ValeurMaconnerie.GetValueOrDefault();
                    b1C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B1C2").Single().ValeurMaconnerie.GetValueOrDefault();
                    b1C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B1C3").Single().ValeurMaconnerie.GetValueOrDefault();

                    b2C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B2C1").Single().ValeurMaconnerie.GetValueOrDefault();
                    b2C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B2C2").Single().ValeurMaconnerie.GetValueOrDefault();
                    b2C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B2C3").Single().ValeurMaconnerie.GetValueOrDefault();

                    b3C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B3C1").Single().ValeurMaconnerie.GetValueOrDefault();
                    b3C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B3C2").Single().ValeurMaconnerie.GetValueOrDefault();
                    b3C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B3C3").Single().ValeurMaconnerie.GetValueOrDefault();

                    b4C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B4C1").Single().ValeurMaconnerie.GetValueOrDefault();
                    b4C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B4C2").Single().ValeurMaconnerie.GetValueOrDefault();
                    b4C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B4C3").Single().ValeurMaconnerie.GetValueOrDefault();
                    break;
                case ENatureCalibrage.Enduit:
                    b0C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B0C1").Single().ValeurEnduit.GetValueOrDefault();
                    b0C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B0C2").Single().ValeurEnduit.GetValueOrDefault();
                    b0C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B0C3").Single().ValeurEnduit.GetValueOrDefault();

                    b1C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B1C1").Single().ValeurEnduit.GetValueOrDefault();
                    b1C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B1C2").Single().ValeurEnduit.GetValueOrDefault();
                    b1C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B1C3").Single().ValeurEnduit.GetValueOrDefault();

                    b2C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B2C1").Single().ValeurEnduit.GetValueOrDefault();
                    b2C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B2C2").Single().ValeurEnduit.GetValueOrDefault();
                    b2C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B2C3").Single().ValeurEnduit.GetValueOrDefault();

                    b3C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B3C1").Single().ValeurEnduit.GetValueOrDefault();
                    b3C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B3C2").Single().ValeurEnduit.GetValueOrDefault();
                    b3C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B3C3").Single().ValeurEnduit.GetValueOrDefault();

                    b4C1 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B4C1").Single().ValeurEnduit.GetValueOrDefault();
                    b4C2 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B4C2").Single().ValeurEnduit.GetValueOrDefault();
                    b4C3 = (double)this.ListeCoefficientPonderation.Where(x => x.Code == "B4C3").Single().ValeurEnduit.GetValueOrDefault();
                    break;
            }

            switch (seuilreferenceACalculer)
            {
                case ESeuilReference.B0:
                    resulC1 = typeCalcul == ETypeCalcul.PESSIMISTE ? b0C1 - valeurPgeC1 : valeurPgeC1 - b0C1;
                    resulC2 = typeCalcul == ETypeCalcul.PESSIMISTE ? b0C2 - valeurPgeC2 : valeurPgeC2 - b0C2;
                    resulC3 = typeCalcul == ETypeCalcul.PESSIMISTE ? b0C3 - valeurPgeC3 : valeurPgeC3 - b0C3;

                    indiceConcordancePartialC1 = resulC1 > 0 ? 0 : 1;
                    indiceConcordancePartialC2 = resulC2 > 0 ? 0 : 1;
                    indiceConcordancePartialC3 = resulC3 > 0 ? 0 : 1;

                    //Calcul de l'indice de concordance globale : 𝑖𝐶(𝑥𝑖,𝑏ℎ) =∑ 𝑐𝑝𝑗𝑖𝑐𝑗(𝑥𝑖,𝑏ℎ) 3 𝑗=1 𝑐𝑝  3 
                    float sommeIndicesB0 = indiceConcordancePartialC1 + indiceConcordancePartialC2 + indiceConcordancePartialC3;
                    indiceConcordanceGlobale = sommeIndicesB0 / 3.0F;

                    //calcul de l'indice de crédibilité 𝝈(𝒙𝒊,𝒃𝒉): 
                    indiceCredibilite = indiceConcordanceGlobale;
                    break;

                case ESeuilReference.B1:
                    resulC1 = typeCalcul == ETypeCalcul.PESSIMISTE ? b1C1 - valeurPgeC1 : valeurPgeC1 - b1C1;
                    resulC2 = typeCalcul == ETypeCalcul.PESSIMISTE ? b1C2 - valeurPgeC2 : valeurPgeC2 - b1C2;
                    resulC3 = typeCalcul == ETypeCalcul.PESSIMISTE ? b1C3 - valeurPgeC3 : valeurPgeC3 - b1C3;

                    indiceConcordancePartialC1 = resulC1 > 0 ? 0 : 1;
                    indiceConcordancePartialC2 = resulC2 > 0 ? 0 : 1;
                    indiceConcordancePartialC3 = resulC3 > 0 ? 0 : 1;

                    //Calcul de l'indice de concordance globale : 𝑖𝐶(𝑥𝑖,𝑏ℎ) =∑ 𝑐𝑝𝑗𝑖𝑐𝑗(𝑥𝑖,𝑏ℎ) 3 𝑗=1 𝑐𝑝  3 
                    float sommeIndicesB1 = indiceConcordancePartialC1 + indiceConcordancePartialC2 + indiceConcordancePartialC3;
                    indiceConcordanceGlobale = (sommeIndicesB1 / 3.0F);

                    //calcul de l'indice de crédibilité 𝝈(𝒙𝒊,𝒃𝒉): 
                    indiceCredibilite = indiceConcordanceGlobale;
                    break;

                case ESeuilReference.B2:
                    resulC1 = typeCalcul == ETypeCalcul.PESSIMISTE ? b2C1 - valeurPgeC1 : valeurPgeC1 - b2C1;
                    resulC2 = typeCalcul == ETypeCalcul.PESSIMISTE ? b2C2 - valeurPgeC2 : valeurPgeC2 - b2C2;
                    resulC3 = typeCalcul == ETypeCalcul.PESSIMISTE ? b2C3 - valeurPgeC3 : valeurPgeC3 - b2C3;

                    indiceConcordancePartialC1 = resulC1 > 0 ? 0 : 1;
                    indiceConcordancePartialC2 = resulC2 > 0 ? 0 : 1;
                    indiceConcordancePartialC3 = resulC3 > 0 ? 0 : 1;

                    //Calcul de l'indice de concordance globale : 𝑖𝐶(𝑥𝑖,𝑏ℎ) =∑ 𝑐𝑝𝑗𝑖𝑐𝑗(𝑥𝑖,𝑏ℎ) 3 𝑗=1 𝑐𝑝  3 
                    float sommeIndicesB2 = indiceConcordancePartialC1 + indiceConcordancePartialC2 + indiceConcordancePartialC3;
                    indiceConcordanceGlobale = sommeIndicesB2 / 3;

                    //calcul de l'indice de crédibilité 𝝈(𝒙𝒊,𝒃𝒉): 
                    indiceCredibilite = indiceConcordanceGlobale;
                    break;

                case ESeuilReference.B3:
                    resulC1 = typeCalcul == ETypeCalcul.PESSIMISTE ? b3C1 - valeurPgeC1 : valeurPgeC1 - b3C1;
                    resulC2 = typeCalcul == ETypeCalcul.PESSIMISTE ? b3C2 - valeurPgeC2 : valeurPgeC2 - b3C2;
                    resulC3 = typeCalcul == ETypeCalcul.PESSIMISTE ? b3C3 - valeurPgeC3 : valeurPgeC3 - b3C3;

                    indiceConcordancePartialC1 = resulC1 > 0 ? 0 : 1;
                    indiceConcordancePartialC2 = resulC2 > 0 ? 0 : 1;
                    indiceConcordancePartialC3 = resulC3 > 0 ? 0 : 1;

                    //Calcul de l'indice de concordance globale : 𝑖𝐶(𝑥𝑖,𝑏ℎ) =∑ 𝑐𝑝𝑗𝑖𝑐𝑗(𝑥𝑖,𝑏ℎ) 3 𝑗=1 𝑐𝑝  3 
                    float sommeIndicesb3 = indiceConcordancePartialC1 + indiceConcordancePartialC2 + indiceConcordancePartialC3;
                    indiceConcordanceGlobale = sommeIndicesb3 / 3;

                    //calcul de l'indice de crédibilité 𝝈(𝒙𝒊,𝒃𝒉): 
                    indiceCredibilite = indiceConcordanceGlobale;
                    break;

                case ESeuilReference.B4:
                    resulC1 = typeCalcul == ETypeCalcul.PESSIMISTE ? b4C1 - valeurPgeC1 : valeurPgeC1 - b4C1;
                    resulC2 = typeCalcul == ETypeCalcul.PESSIMISTE ? b4C2 - valeurPgeC2 : valeurPgeC2 - b4C2;
                    resulC3 = typeCalcul == ETypeCalcul.PESSIMISTE ? b4C3 - valeurPgeC3 : valeurPgeC3 - b4C3;

                    indiceConcordancePartialC1 = resulC1 > 0 ? 0 : 1;
                    indiceConcordancePartialC2 = resulC2 > 0 ? 0 : 1;
                    indiceConcordancePartialC3 = resulC3 > 0 ? 0 : 1;

                    //Calcul de l'indice de concordance globale : 𝑖𝐶(𝑥𝑖,𝑏ℎ) =∑ 𝑐𝑝𝑗𝑖𝑐𝑗(𝑥𝑖,𝑏ℎ) 3 𝑗=1 𝑐𝑝  3 
                    float sommeIndicesb4 = indiceConcordancePartialC1 + indiceConcordancePartialC2 + indiceConcordancePartialC3;
                    indiceConcordanceGlobale = sommeIndicesb4 / 3;

                    //calcul de l'indice de crédibilité 𝝈(𝒙𝒊,𝒃𝒉): 
                    indiceCredibilite = indiceConcordanceGlobale;
                    break;
            }

            return indiceCredibilite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsConverted(string pk)
        {
            if (!string.IsNullOrEmpty(pk))
            {
                if (pk.Contains("."))
                {
                    double outparam;
                    if (double.TryParse(pk.Replace('.', ','), out outparam))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Méthode permettant d'obtenir le preordre final
        /// </summary>
        /// <param name="listePge"></param>
        /// <param name="distillationDescendante"></param>
        /// <param name="distillationAscendante"></param>
        /// <returns></returns>
        private IEnumerable<PreordreFinalDto> GetPeordreFinal(IEnumerable<Documents> listePge, IEnumerable<ClassementDistillationDto> distillationDescendante, IEnumerable<ClassementDistillationDto> distillationAscendante)
        {
            this.Logger.Info("ETAPE 2: PREORDRE FINAL");
            //Réf
            char iquivalent = '|';
            char mieux = '>';
            char moins = '<';
            char incomparable = 'R';

            //Matrice de classement final
            for (int i = 0; i < listePge.Count(); i++)
            {
                for (int k = 0; k < listePge.Count(); k++)
                {
                    var pge_i = listePge.ToArray()[i];
                    var pge_k = listePge.ToArray()[k];
                    var key = string.Format("{0}_{1}", pge_i.NumeroAffaire, pge_k.NumeroAffaire);
                    if (pge_i.NumeroAffaire == pge_k.NumeroAffaire || distillationDescendante.ToArray()[i].Classement == distillationDescendante.ToArray()[k].Classement && distillationAscendante.ToArray()[i].Classement == distillationAscendante.ToArray()[k].Classement)
                    {
                        listeSymbole.Add(key, iquivalent);
                    }
                    else if (distillationDescendante.ToArray()[i].Classement > distillationDescendante.ToArray()[k].Classement && distillationAscendante.ToArray()[i].Classement > distillationAscendante.ToArray()[k].Classement)
                    {
                        listeSymbole.Add(key, mieux);
                    }
                    else if (distillationDescendante.ToArray()[i].Classement < distillationDescendante.ToArray()[k].Classement && distillationAscendante.ToArray()[i].Classement < distillationAscendante.ToArray()[k].Classement)
                    {
                        listeSymbole.Add(key, moins);
                    }
                    else
                    {
                        listeSymbole.Add(key, incomparable);
                    }
                }
            }

            //PREORDRE FINAL
            this.GetTopPreordrePges(this.ListePgeDocument);
            this.GetSecondClassementPreordreFinal(this.ListePgeDocument, this.listePreordreFinal);

            //TODO: suite au besoin d'angel, j'ai généré ce fichier pour consulter le détail de preordre final avant l'étape final de calcul
            SerializeObject<IEnumerable<PreordreFinalDto>>.XmlSerialize(this.listePreordreFinal, string.Format(ConfigurationManager.AppSettings["UrlSchemasXml"], "PreordreFinal.xml"));
            return this.listePreordreFinal;
        }

        /// <summary>
        /// Obtenir les PGE mieux classés
        /// </summary>
        /// <param name="listePge"></param>
        /// <returns>Pge mieux classés</returns>
        private void GetTopPreordrePges(IList<Documents> listePge)
        {
            for (int i = 0; i < listePge.Count(); i++) //PGE i
            {
                var pge_i = listePge.ToArray()[i];
                bool isTop = true;
                for (int k = 0; k < listePge.Count(); k++) //PGE k
                {
                    string oldPge = string.Empty;
                    var pge_k = listePge.ToArray()[k];
                    var key = string.Format("{0}_{1}", pge_i.NumeroAffaire, pge_k.NumeroAffaire);
                    if (listeSymbole[key] == '<')
                    {
                        isTop = false;
                        break;
                    }
                }

                if (isTop)
                {
                    //Obtenir les pge mieux classées: Classement = 1
                    this.listePreordreFinal.Add(new PreordreFinalDto { IdDocument = pge_i.Id, NumeroAffaire = pge_i.NumeroAffaire, Rang = 1, Median = 1 });
                }
            }
        }

        /// <summary>
        /// Obtenir la liste des PGEs moins classés par rapport aux mieux classés
        /// </summary>
        /// <param name="listePge">liste des Pge pour l'itération</param>
        /// <param name="listePgeAComparer">liste des pge pour comparerr si la condition est respecté pour décidier si le pge est moins classé </param>
        private void GetSecondClassementPreordreFinal(IList<Documents> listePge, IEnumerable<PreordreFinalDto> listePgeAComparer)
        {
            IList<Documents> listeOut = new List<Documents>();
            classementPreordreFinal += 1;
            for (int i = 0; i < listePge.Count(); i++) //PGE i
            {

                var pge_i = listePge.ToArray()[i];
                bool isrespected = true;
                for (int k = 0; k < listePge.Count(); k++) //PGE k
                {
                    var pge_k = listePge.ToArray()[k];
                    var key = string.Format("{0}_{1}", pge_i.NumeroAffaire, pge_k.NumeroAffaire);
                    if (listeSymbole[key] == '<')
                    {
                        if (this.listePreordreFinal.Any(x => x.NumeroAffaire == pge_k.NumeroAffaire))
                        {
                            isrespected = true;
                        }
                        else
                        {
                            isrespected = false;
                            break;
                        }
                    }

                    if (!isrespected)
                    {
                        break;
                    }
                }

                if (isrespected)
                {
                    //Obtenir les pge mieux classées: Classement = 1
                    if (!this.listePreordreFinal.Any(x => x.NumeroAffaire == pge_i.NumeroAffaire))
                    {
                        this.listePreordreFinal.Add(new PreordreFinalDto { IdDocument = pge_i.Id, NumeroAffaire = pge_i.NumeroAffaire, Rang = classementPreordreFinal, Median = classementPreordreFinal });
                    }
                }
            }

            if (listePge.Count() != this.listePreordreFinal.Count())
            {
                GetSecondClassementPreordreFinal(listePge, this.listePreordreFinal);
            }
        }

        /// <summary>
        /// Ce préordre concerne les PGE présentant le caractère « R » dans la « matrice de classement final » 
        /// </summary>
        /// <param name="listeDescendante"></param>
        /// <param name="listeAscendante"></param>
        private void PreordreMedian(IEnumerable<ClassementDistillationDto> listeDescendante, IEnumerable<ClassementDistillationDto> listeAscendante)
        {
            this.Logger.Info("ETAPE 2: PREORDRE MEDIAN");
            //for (int i = 0; i < this.ListePgeDocument.Count(); i++)
            foreach (var currentRang in this.listePreordreFinal.Select(x => x.Rang).ToList().Distinct())
            {
                var listePgeIdentiqueClassement = this.listePreordreFinal.Where(x => x.Rang == currentRang).ToList();

                if (listePgeIdentiqueClassement != null && listePgeIdentiqueClassement.Count > 0)
                {
                    if (this.IsIncomparables(listePgeIdentiqueClassement))
                    {
                        for (int item = 0; item < listePgeIdentiqueClassement.Count(); item++)
                        {
                            var pge_i = listePgeIdentiqueClassement.ToArray()[item];
                            var classementPreordreFinal = pge_i.Rang;
                            var classementDescendante = listeDescendante.FirstOrDefault(x => x.NumeroAffaire == pge_i.NumeroAffaire).Classement;
                            var classementAscendnate = listeAscendante.FirstOrDefault(x => x.NumeroAffaire == pge_i.NumeroAffaire).Classement;
                            var DifferencePosition = Math.Abs(classementDescendante - classementAscendnate);

                            //si la liste identiqueclassement contient plusqu'un PGE
                            if (item + 1 < listePgeIdentiqueClassement.Count())
                            {
                                //PGE item +1
                                var pge_k = listePgeIdentiqueClassement.ToArray()[item + 1];
                                var classementPreordreFinal_k = pge_i.Rang;
                                var classementDescendante_k = listeDescendante.FirstOrDefault(x => x.NumeroAffaire == pge_k.NumeroAffaire).Classement;
                                var classementAscendnate_k = listeAscendante.FirstOrDefault(x => x.NumeroAffaire == pge_k.NumeroAffaire).Classement;
                                var DifferencePosition_k = Math.Abs(classementDescendante_k - classementAscendnate_k);

                                var preodreFinal_i = this.listePreordreFinal.FirstOrDefault(x => x.NumeroAffaire == pge_i.NumeroAffaire);
                                var preodreFinal_k = this.listePreordreFinal.FirstOrDefault(x => x.NumeroAffaire == pge_k.NumeroAffaire);

                                /// Si la « différence de positions » des PGE xi et xk présentant le caractère « R » dans la « matrice de classement final » (tableau 87)) est la même, alors le « rang de préordre médian » des PGE xi et xk est égal au « rang de préordre final ». C'est-à-dire, les PGE xi et xk sont équivalentes ; 
                                if (DifferencePosition_k == DifferencePosition)
                                {
                                    preodreFinal_i.Median = preodreFinal_i.Rang;
                                    preodreFinal_k.Median = preodreFinal_i.Rang;
                                }

                                //Si la « différence de positions » de la PGE xk est supérieure à la « différence de positions » de la PGE xi, alors le « rang de préordre médian » de la PGE xi est égal au « rang de préordre final » et le « rang de préordre médian » de la PGE xk est égal au « rang de préordre final + 1 ». C'est-à-dire, la PGE xi est meilleure que la PGE xk. 
                                else if (DifferencePosition_k > DifferencePosition)
                                {
                                    preodreFinal_i.Median = preodreFinal_i.Rang;
                                    preodreFinal_k.Median = preodreFinal_i.Rang + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Méthode permettant de vérifier si les PGE son incomaparables
        /// </summary>
        /// <param name="listePgePreordreFinal"></param>
        /// <returns>true si les PGE sont incomparables</returns>
        private bool IsIncomparables(IEnumerable<PreordreFinalDto> listePgePreordreFinal)
        {
            bool isIncomparable = true;
            foreach (var pge_i in listePgePreordreFinal)
            {
                foreach (var pge_k in listePgePreordreFinal)
                {
                    if (pge_i.NumeroAffaire != pge_k.NumeroAffaire)
                    {
                        var key = string.Format("{0}_{1}", pge_i.NumeroAffaire, pge_k.NumeroAffaire);
                        if (this.listeSymbole[key] != 'R')
                        {
                            isIncomparable = false;
                            break;
                        }
                    }
                }
            }

            return isIncomparable;
        }

        /// <summary>
        /// Obtenir l'ordre final de la liste des PGE
        /// </summary>
        /// <param name="listePreodreFinal">liste preordre final des PGEs</param>
        /// <returns>Liste des PGE final</returns>
        private IEnumerable<PreordreFinalDto> GetOrdreFinal(IEnumerable<PreordreFinalDto> listePreodreFinal)
        {
            IList<PreordreFinalDto> listeFinal = new List<PreordreFinalDto>();
            var rangs = listePreodreFinal.Select(x => x.Rang).ToList().Distinct();
            var maxMedian = 0;
            var oldIndexRang = 0;
            var newIndexRang = 0;
            foreach (var indexRang in rangs)
            {

                var listePgeOfRang = listePreodreFinal.Where(x => x.Rang == indexRang).ToList();
                maxMedian = listePgeOfRang.Max(x => x.Median);

                if (maxMedian == indexRang)
                {
                    oldIndexRang = listeFinal.Count() != 0 ? listeFinal.Max(x => x.Rang) + 1 : oldIndexRang + 1;

                    foreach (var pge in listePgeOfRang)
                    {
                        listeFinal.Add(new PreordreFinalDto { IdDocument = pge.IdDocument, NumeroAffaire = pge.NumeroAffaire, Rang = oldIndexRang, Median = pge.Median, IsEx = listePgeOfRang.Count() > 1 });
                    }
                }
                else
                {
                    oldIndexRang = listeFinal.Count() != 0 ? listeFinal.Max(x => x.Rang) + 1 : oldIndexRang + 1;
                    newIndexRang = oldIndexRang + 1;

                    foreach (var pge in listePgeOfRang.Where(x => x.Median == indexRang).ToList())
                    {
                        listeFinal.Add(new PreordreFinalDto { IdDocument = pge.IdDocument, NumeroAffaire = pge.NumeroAffaire, Rang = oldIndexRang, Median = pge.Median, IsEx = listePgeOfRang.Where(x => x.Median == indexRang).ToList().Count() > 1 });
                    }

                    foreach (var pge in listePgeOfRang.Where(x => x.Median == maxMedian).ToList())
                    {
                        listeFinal.Add(new PreordreFinalDto { IdDocument = pge.IdDocument, NumeroAffaire = pge.NumeroAffaire, Rang = newIndexRang, Median = pge.Median, IsEx = listePgeOfRang.Where(x => x.Median == maxMedian).ToList().Count > 1 });
                    }
                }
            }

            return listeFinal;
        }
        #endregion
    }
}