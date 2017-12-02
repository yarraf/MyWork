using Core.Common.Mapping;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract;
using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Data.Contract.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ratp.Hidalgo.App.Mapping
{
    /// <summary>
    /// Transformer les données de type <see cref="PreordreFinalDto"/> to <see cref="ProgrammationDocumentPGE"/>
    /// </summary>
    public class PreordreFinalDtoToProgrammationDocumentPgeMapping : IMapper<PreordreFinalDto, ProgrammationDocumentPGE>, IMapper<ProgrammationDocumentPGE, ProgrammationDocumentPgeDto>, IMapper<ProgrammationDocumentPgeDto, ProgrammationDocumentPGE>
    {
        private IHidalgoUnitOfWork hidalgoUow { get; set; }
        private LieuToLieuDtoMapping lieuMapping;
        private LigneToLigneDtoMapping ligneMapping;
        private ProgrammationDtoToProgrammationMapping programmationMapping;
        private IEnumerable<Documents> listeDocument;
        private IEnumerable<DocumentDto> ListePgeFissuration { get; set; }
        private IEnumerable<DocumentDto> ListePgeInfiltration { get; set; }
        private IEnumerable<DocumentDto> ListePgeDesordreStructureMaconnerieBeton { get; set; }
        private IEnumerable<DocumentDto> ListePgeAgeOuvrage { get; set; }
        private IEnumerable<DocumentDto> ListePgeLargeurOuvrage { get; set; }
        private IEnumerable<DocumentDto> ListePgeHistoriqueRegenerationMaconnerie { get; set; }
        private IEnumerable<DocumentDto> ListePgeHistoriqueRegenerationEnduit { get; set; }
        private IEnumerable<DocumentDto> ListePgeAgressiviteChimiqueTerrainEncaissant { get; set; }
        private IEnumerable<DocumentDto> ListePgeSolibiliteTerrain { get; set; }
        private IEnumerable<DocumentDto> ListePgePourrissementBoisBlindage { get; set; }
        public IEnumerable<CategoriePgeDto> ListeCategoriePge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitofwork"></param>
        /// <param name="listePgeFissuration"></param>
        /// <param name="listePgeInfiltration"></param>
        /// <param name="listePgeDesordreStructureMaconnerieBeton"></param>
        /// <param name="listePgeAgeOuvrage"></param>
        /// <param name="listePgeLargeurOuvrage"></param>
        /// <param name="listePgeHistoriqueRegenerationMaconnerie"></param>
        /// <param name="listePgeHistoriqueRegenerationEnduit"></param>
        /// <param name="listePgeAgressiviteChimiqueTerrainEncaissant"></param>
        /// <param name="listePgeSolibiliteTerrain"></param>
        /// <param name="listePgePourrissementBoisBlindage"></param>
        /// <param name="listeDocument"></param>
        public PreordreFinalDtoToProgrammationDocumentPgeMapping(IHidalgoUnitOfWork unitofwork,
             IEnumerable<DocumentDto> listePgeFissuration = null,
             IEnumerable<DocumentDto> listePgeInfiltration = null,
             IEnumerable<DocumentDto> listePgeDesordreStructureMaconnerieBeton = null,
             IEnumerable<DocumentDto> listePgeAgeOuvrage = null,
             IEnumerable<DocumentDto> listePgeLargeurOuvrage = null,
             IEnumerable<DocumentDto> listePgeHistoriqueRegenerationMaconnerie = null,
             IEnumerable<DocumentDto> listePgeHistoriqueRegenerationEnduit = null,
             IEnumerable<DocumentDto> listePgeAgressiviteChimiqueTerrainEncaissant = null,
             IEnumerable<DocumentDto> listePgeSolibiliteTerrain = null,
             IEnumerable<DocumentDto> listePgePourrissementBoisBlindage = null,
             IEnumerable<Documents> listeDocument = null,
             IEnumerable<CategoriePgeDto> listeCategoriePge = null
             )

        {
            hidalgoUow = unitofwork;
            lieuMapping = new Mapping.LieuToLieuDtoMapping();
            ligneMapping = new Mapping.LigneToLigneDtoMapping();
            programmationMapping = new Mapping.ProgrammationDtoToProgrammationMapping();
            this.listeDocument = listeDocument;
            this.ListePgeFissuration = listePgeFissuration;
            this.ListePgeInfiltration = listePgeInfiltration;
            this.ListePgeDesordreStructureMaconnerieBeton = listePgeDesordreStructureMaconnerieBeton;
            this.ListePgeAgeOuvrage = listePgeAgeOuvrage;
            this.ListePgeLargeurOuvrage = listePgeLargeurOuvrage;
            this.ListePgeHistoriqueRegenerationMaconnerie = listePgeHistoriqueRegenerationMaconnerie;
            this.ListePgeHistoriqueRegenerationEnduit = listePgeHistoriqueRegenerationEnduit;
            this.ListePgeAgressiviteChimiqueTerrainEncaissant = listePgeAgressiviteChimiqueTerrainEncaissant;
            this.ListePgeSolibiliteTerrain = listePgeSolibiliteTerrain;
            this.ListePgePourrissementBoisBlindage = listePgePourrissementBoisBlindage;
            this.ListeCategoriePge = listeCategoriePge;
        }

        public ProgrammationDocumentPgeDto Map(ProgrammationDocumentPGE source)
        {
            var target = new ProgrammationDocumentPgeDto();
            this.Map(source, target);
            return target;
        }

        public ProgrammationDocumentPGE Map(PreordreFinalDto source)
        {
            var target = new ProgrammationDocumentPGE();
            this.Map(source, target);
            return target;
        }

        public void Map(ProgrammationDocumentPGE source, ProgrammationDocumentPgeDto target)
        {
            //TODO YAF, changer le mapping de cette prop, remplacer IdProgrammation par ID de l'objet
            target.Id = source.Id;
            target.IdDocument = source.IdDocument.GetValueOrDefault();
            target.NumeroAffaire = source.NumeroAffaire;
            target.Programmation = programmationMapping.Map(source.Programmations);
            target.Rang = source.NumRang;
            target.Median = source.NumMedian;
            target.IsEx = source.IsExAequo;
            target.IsValidEx = source.IsValideEx;
            target.Commentaire = source.Commentaire;
            target.Surface = source.Surface;
            target.IsTravauxPlusieursAnnee = source.IsTravauxPlusieursAnnee;
            target.Annee = source.Annee;
            target.Budget = source.Budget;
            target.Categorie = this.GetCategoriePge(source.Categorie.GetValueOrDefault());

            if (this.hidalgoUow != null)
            {
                var pv = this.hidalgoUow.HidalgoRepository.GetPvByDocumentPge(source.NumeroAffaire);
                if (pv != null)
                {
                    //foreach (var item in pv.Planification)
                    //{
                    //    target.Lieu = lieuMapping.Map(item.GroupeOuvrage.Lieux);
                    //    target.Ligne = ligneMapping.Map(item.GroupeOuvrage.Lieux.Lignes);
                    //    break;
                    //}
                    foreach (var item in pv.GroupeOuvrage)
                    {
                        target.Lieu = lieuMapping.Map(item.Lieux);
                        target.Ligne = ligneMapping.Map(item.Lieux.Lignes);
                        break;
                    }
                }
            }
        }

        public void Map(PreordreFinalDto source, ProgrammationDocumentPGE target)
        {
            target.IdDocument = source.IdDocument;
            target.NumeroAffaire = source.NumeroAffaire;
            target.NumRang = source.Rang;
            target.NumMedian = source.Median;
            target.Categorie = this.GetCategorie(this.ListeCategoriePge.Where(x => x.NumeroAffaire == source.NumeroAffaire).First().Categorie);

            //target.Programmations = this.hidalgoUow.HidalgoRepository.GetOneProgrammation();
            //TODO YAF: on verra si possible de modifié la valeur au niveau de client
            target.IsValideEx = false;
            target.IsExAequo = source.IsEx;

            if (this.listeDocument != null && this.listeDocument.Any())
            {
                var pge = this.listeDocument.Where(x => x.NumeroAffaire == source.NumeroAffaire).FirstOrDefault();
                if (pge != null)
                {
                    if (pge.DesordresDocuments != null)
                    {
                        double sommeSurface = 0;
                        foreach (var item in pge.DesordresDocuments)
                        {
                            if (item.Desordres != null && item.Desordres.Localisations != null && item.Desordres.Localisations.Ouvrages != null)
                            {
                                sommeSurface += this.hidalgoUow.HidalgoRepository.GetSurfaceOuvragesByPge(item.Desordres.Localisations.Ouvrages.Id);
                            }
                        }

                        target.Surface = sommeSurface;
                    }
                }
            }

            //Liste des valeurs de paramètres
            foreach (var item in Enum.GetValues(typeof(EParametreHidalgo)))
            {
                switch ((EParametreHidalgo)item)
                {
                    case EParametreHidalgo.Fissurations:
                        target.ProgrammationValeurParametresDocument.Add(new ProgrammationValeurParametresDocument { IDDocumentPge = target.Id, IDParametreHidalgo = (int)item, Note = this.ListePgeFissuration.FirstOrDefault(x => x.NumeroAffaire == source.NumeroAffaire).Note });
                        break;
                    case EParametreHidalgo.Infiltrations:
                        target.ProgrammationValeurParametresDocument.Add(new ProgrammationValeurParametresDocument { IDDocumentPge = target.Id, IDParametreHidalgo = (int)item, Note = this.ListePgeInfiltration.FirstOrDefault(x => x.NumeroAffaire == source.NumeroAffaire).Note });
                        break;
                    case EParametreHidalgo.Age_de_ouvrage:
                        target.ProgrammationValeurParametresDocument.Add(new ProgrammationValeurParametresDocument { IDDocumentPge = target.Id, IDParametreHidalgo = (int)item, Note = this.ListePgeAgeOuvrage.FirstOrDefault(x => x.NumeroAffaire == source.NumeroAffaire).Note });
                        break;
                    case EParametreHidalgo.Agressivite_chimique_du_terrain_encaissant:
                        target.ProgrammationValeurParametresDocument.Add(new ProgrammationValeurParametresDocument { IDDocumentPge = target.Id, IDParametreHidalgo = (int)item, Note = this.ListePgeAgressiviteChimiqueTerrainEncaissant.FirstOrDefault(x => x.NumeroAffaire == source.NumeroAffaire).Note });
                        break;
                    case EParametreHidalgo.Largeur_de_ouvrage:
                        target.ProgrammationValeurParametresDocument.Add(new ProgrammationValeurParametresDocument { IDDocumentPge = target.Id, IDParametreHidalgo = (int)item, Note = this.ListePgeLargeurOuvrage.FirstOrDefault(x => x.NumeroAffaire == source.NumeroAffaire).Note });
                        break;
                    case EParametreHidalgo.Désordre_surstructure_maçonnerie_beton:
                        target.ProgrammationValeurParametresDocument.Add(new ProgrammationValeurParametresDocument { IDDocumentPge = target.Id, IDParametreHidalgo = (int)item, Note = this.ListePgeDesordreStructureMaconnerieBeton.FirstOrDefault(x => x.NumeroAffaire == source.NumeroAffaire).Note });
                        break;
                        //case EParametreHidalgo.Hi:
                        //    target.ProgrammationValeurParametresDocument.Add(new ProgrammationValeurParametresDocument { IDDocumentPge = target.Id, IDParametreHidalgo = (int)item, Note = this.ListePgeH.FirstOrDefault(x => x.NumeroAffaire == source.NumeroAffaire).Note });
                        //    break;
                }
            }
        }

        public void Map(ProgrammationDocumentPgeDto source, ProgrammationDocumentPGE target)
        {
            target.Id = source.Id;
            target.IdDocument = source.IdDocument;
            target.NumeroAffaire = source.NumeroAffaire;
            target.NumRang = source.Rang;
            target.NumMedian = source.Median;
            target.IsExAequo = source.IsEx;
            target.IsValideEx = source.IsValidEx;
            target.Commentaire = source.Commentaire;
            target.Surface = source.Surface;
            target.IsTravauxPlusieursAnnee = source.IsTravauxPlusieursAnnee;
            target.Annee = source.Annee;
            target.Budget = source.Budget;
            //target.Categorie = this.GetCategorie(this.ListeCategoriePge.Where(x => x.NumeroAffaire == source.NumeroAffaire).First().Categorie ?? null);
        }

        public ProgrammationDocumentPGE Map(ProgrammationDocumentPgeDto source)
        {
            var target = new ProgrammationDocumentPGE();
            this.Map(source, target);
            return target;
        }

        /// <summary>
        /// Préparer la acat&gorie pour l'enregistrement
        /// </summary>
        /// <param name="categorie"></param>
        /// <returns></returns>
        private ECategoriesPge? GetCategorie(string categorie)
        {
            if(string.IsNullOrEmpty(categorie))
            {
                return null;
            }

            switch (categorie)
            {
                case "CAT 1":
                    return ECategoriesPge.CAT_1;
                case "CAT 2":
                    return ECategoriesPge.CAT_2;
                case "CAT 3":
                    return ECategoriesPge.CAT_3;
                case "CAT 4":
                    return ECategoriesPge.CAT_4;
            }

            return null;
        }

        /// <summary>
        /// Transformer la catégorie en string pour l'affichage
        /// </summary>
        /// <param name="categorie"></param>
        /// <returns></returns>
        private string GetCategoriePge(ECategoriesPge categorie)
        {
            string cat = null;
            switch (categorie)
            {
                case ECategoriesPge.CAT_1:
                    cat = "CAT 1";
                    break;
                case ECategoriesPge.CAT_2:
                    cat = "CAT 2";
                    break;
                case ECategoriesPge.CAT_3:
                    cat = "CAT 3";
                    break;
                case ECategoriesPge.CAT_4:
                    cat = "CAT 4";
                    break;

                default:
                    return null;
            }

            return cat;
        }
    }
}