using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Data.Contract.Enum;
using Ratp.Hidalgo.Data.Contract.Repositories;
using Ratp.Hidalgo.Model.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;


namespace Ratp.Hidalgo.Model.Repositories
{
    /// <summary>
    /// Repositorie de  CRITERE « PERFORMANCE » (C1)
    /// </summary>
    public class CriterePerformanceRepositorie : ICriterePerformanceRepositorie
    {
        /// <inheritdoc />
        public IEnumerable<Documents> GetAllDocumentPge(ENatureCalibrage typeNatureTravaux)
        {
            IEnumerable<Documents> resultOfDocuments = null;

            //TODO YAR 24/05/2017 : filter en dure doit se remplacer par une enum lié à la base de données, à remplacer par enm
            ///TODO d'après le CD Demandes travaux: nous avons une rectif pour les données de la nature travaux dont on trouve 
            /// Réfection d’ouvrages maçonnés = Reprise maçonnerie = 31
            ///Régénération des maçonneries = Confortement des maçonneries = 6
            ///Confortement des terrains(7) =  Comblement de vide(5) = Confortement des terrains = 5 et 7
            int[] idNatureTravauxMaconnerie = new[] { 31, 6, 7, 5 };
            int[] idNaturesTravauxEnduit = new[] { 25 };
            int[] idTypeOuvrage = new[] { 1, 14 };
            int[] pgestatutsAutorise = new[] { (int)EStatutsDocument.EN_COURS, (int)EStatutsDocument.RELANCE };
            int dateAchevePge = 2009;

            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    IQueryable<Documents> query = context.Documents
                        .Include(i => i.DesordresDocuments)
                        .Include(i => i.DesordresDocuments.Select(x => x.Desordres.Localisations.Ouvrages))
                        .Include(i => i.DesordresDocuments.Select(x => x.DocumentsLies))
                        .Include(i => i.ValidationsDocuments);

                    //Filtre pour nature travaux maconnerie
                    if (typeNatureTravaux == ENatureCalibrage.Maçonnerie)
                    {
                        query = query.Where(x => x.DesordresDocuments.Any(xx => xx.Id == x.DesordresDocuments.Max(m => m.Id) && idNatureTravauxMaconnerie.Contains(xx.IdNatureTravaux.Value) && idTypeOuvrage.Contains(xx.Desordres.Localisations.Ouvrages.IdTypeOuvrage)));
                    }
                    else if (typeNatureTravaux == ENatureCalibrage.Enduit)   //filtre pour nature travaux Enduit
                    {
                        query = query.Where(x => x.DesordresDocuments.Any(xx => xx.Id == x.DesordresDocuments.Max(m => m.Id) && idNaturesTravauxEnduit.Contains(xx.IdNatureTravaux.Value) && idTypeOuvrage.Contains(xx.Desordres.Localisations.Ouvrages.IdTypeOuvrage)));
                    }

                    query = query.Where(x => x.IdDocumentType == 10 &&
                                            x.DateCreation.HasValue &&
                                            x.DateCreation.Value.Year > dateAchevePge &&
                                            x.DesordresDocuments.Any(xx => xx.DocumentsLies.Any(xxx => xxx.DateStatut == xx.DocumentsLies.Max(m => m.DateStatut) && pgestatutsAutorise.Contains(xxx.IdStatutDocument))));

                    resultOfDocuments = query.Distinct().OrderBy(x => x.NumeroAffaire).ToList();

                }
                catch (NullReferenceException exNull)
                {
                    //Log
                }

                catch (Exception ex)
                {
                    //Log
                }
            }

            return resultOfDocuments;
        }

        /// <inheritdoc />
        public IEnumerable<VDocumentPge> GetNoteMaxPgeByFamilleDesordre(EFamilleDesordre familleDesordre, ENatureCalibrage natureCalibrage)
        {
            //TODO YAR 24/05/2017 : filter en dure doit se remplacer par une enum lié à la base de données, à remplacer par enm
            int[] idNatureTravauxMaconnerie = new[] { 31, 6, 7, 5 };
            int[] idNaturesTravauxEnduit = new[] { 25 };
            int[] idTypeOuvrage = new[] { 1, 14 };
            int[] pgestatutsAutorise = new[] { (int)EStatutsDocument.EN_COURS, (int)EStatutsDocument.RELANCE };
            int dateAchevePge = 2009;

            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                //try
                //{
                IQueryable<Documents> query = context.Documents
                    .Include(i => i.DesordresDocuments)
                    .Include(i => i.DesordresDocuments.Select(x => x.Desordres.Localisations.Ouvrages))
                    .Include(i => i.DesordresDocuments.Select(x => x.Desordres.FamillesDesordresTypes))
                    .Include(i => i.DesordresDocuments.Select(x => x.DocumentsLies))
                    .Include(i => i.ValidationsDocuments);

                if (natureCalibrage == ENatureCalibrage.Maçonnerie)
                {
                    query = query.Where(x => x.DesordresDocuments.Any(xx => xx.Id == x.DesordresDocuments.Max(m => m.Id) && idNatureTravauxMaconnerie.Contains(xx.IdNatureTravaux.Value) && idTypeOuvrage.Contains(xx.Desordres.Localisations.Ouvrages.IdTypeOuvrage)));
                }
                else if (natureCalibrage == ENatureCalibrage.Enduit)   //filtre pour nature travaux Enduit
                {
                    query = query.Where(x => x.DesordresDocuments.Any(xx => xx.Id == x.DesordresDocuments.Max(m => m.Id) && idNaturesTravauxEnduit.Contains(xx.IdNatureTravaux.Value) && idTypeOuvrage.Contains(xx.Desordres.Localisations.Ouvrages.IdTypeOuvrage)));
                }

                query = query.Where(x => x.IdDocumentType == 10 &&
                                        x.DateCreation.HasValue &&
                                        x.DateCreation.Value.Year > dateAchevePge &&
                                          //x.DesordresDocuments.Any(xx => xx.Id == x.DesordresDocuments.Max(m => m.Id) && xx.DocumentsLies.Any(xxx => pgestatutsAutorise.Contains(xxx.IdStatutDocument))));
                                          x.DesordresDocuments.Any(xx => xx.DocumentsLies.Any(xxx => xxx.DateStatut == xx.DocumentsLies.Max(m => m.DateStatut) && pgestatutsAutorise.Contains(xxx.IdStatutDocument))));

                var result = query.Distinct().OrderBy(x => x.NumeroAffaire).ToList();

                IList<VDocumentPge> listeDocument = new List<VDocumentPge>();
                //filtre par famille des désordres
                if (result != null && result.Any())
                {
                    foreach (var doc in result)
                    {
                        VDocumentPge document = new VDocumentPge();
                        document.NumeroAffaire = doc.NumeroAffaire;
                        document.Id = doc.Id;
                        document.IdDocumentType = doc.IdDocumentType;

                        if (doc.DesordresDocuments.Any(x => x.Desordres.FamillesDesordresTypes.IdFamilleDesordre == (int)familleDesordre))
                        {
                            document.Note = doc.DesordresDocuments.Where(xx => xx.Desordres.FamillesDesordresTypes.IdFamilleDesordre == (int)familleDesordre).Max(x => x.Desordres.Note);
                        }
                        else
                        {
                            document.Note = 0;
                        }

                        listeDocument.Add(document);
                    }
                }

                return listeDocument;
            }
            //catch (NullReferenceException exNull)
            //{
            //    //Log
            //}

            //catch (Exception ex)
            //{
            //    //Log
            //}

        }

        /// <inheritdoc />
        public IEnumerable<Ouvrages> GetAllOuvragesById(IEnumerable<int> listeIdOuvrage)
        {
            IEnumerable<Ouvrages> result = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    //Filtre sur les ouvrages de type Tunnel et Couloir
                    //TODO YAR 23/05/2017: en attedant d'utiliser les Enum pour TypesOuvrages
                    result = context.Ouvrages.Where(x => listeIdOuvrage.Contains(x.Id) && (x.IdTypeOuvrage == 1 || x.IdTypeOuvrage == 14)).Include(i => i.Descriptions).ToList();
                }
                catch (TimeoutException tex)
                {
                    //Log
                }

                catch (Exception ex)
                {
                    //Log
                }
            }
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<DataAnnexe> GetAllDataAnnexeByIdOuvrage(IEnumerable<int> listeIdOuvrage)
        {
            IEnumerable<DataAnnexe> result = null;
            var listeIdOuvrageConverted = listeIdOuvrage.Select(x => x.ToString()).ToList();
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    //Filtre sur les ouvrages de type Tunnel et Couloir
                    //TODO YAR 23/05/2017: en attedant d'utiliser les Enum pour TypesOuvrages
                    result = context.DataAnnexe.Where(x => listeIdOuvrageConverted.Contains(x.IdOuvrage)).ToList();
                }
                catch (TimeoutException tex)
                {
                    //Log
                }

                catch (Exception ex)
                {
                    //Log
                }
            }
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<Ouvrages> GetAllOuvragesWithInfoLignes(IEnumerable<int> listeIdOuvrage)
        {
            IEnumerable<Ouvrages> result = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    result = context.Ouvrages.Where(x => listeIdOuvrage.Contains(x.Id) && (x.IdTypeOuvrage == 1 || x.IdTypeOuvrage == 14))
                        .Include(i => i.Lieux)
                        .Include(i => i.Lieux.Lignes)
                        .Include(i => i.Descriptions)
                        .ToList();
                }
                catch (TimeoutException tex)
                {
                    //Log
                }

                catch (Exception ex)
                {
                    //Log
                }
            }

            return result;
        }
    }
}
