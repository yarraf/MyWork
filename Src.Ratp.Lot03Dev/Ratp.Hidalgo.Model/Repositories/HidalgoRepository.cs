using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Ratp.Hidalgo.Data.Contract.Repositories;
using Ratp.Hidalgo.Model.Properties;
using System.Threading.Tasks;
using Ratp.Hidalgo.Data.Contract.Entities;

namespace Ratp.Hidalgo.Model.Repositories
{
    /// <summary>
    /// Implémentation de repository
    /// </summary>
    public class HidalgoRepository : IHidalgoRepository
    {
        /// <inheritdoc />
        public IEnumerable<Lignes> GetAllLignes()
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                var lignes = context.Lignes.ToList();
                return lignes;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Lignes>> GetAllLignesAsync()
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                var result = await context.Lignes.ToListAsync();
                return result;
            }
        }

        /// <summary>
        /// Méthode déspose permettant de détuit les objets inutile, et en suite liberer la mémoire.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<Documents> GetAllDocumentPge()
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                //Obtenir la liste des documents de type PGE
                var result = context.Documents.Where(i => i.IdDocumentType == 10)
                                             .Include(i => i.DesordresDocuments)
                                             .Include(i => i.DesordresDocuments.Select(s => s.Desordres))
                                             .Include(i => i.DesordresDocuments.Select(s => s.Desordres.FamillesDesordresTypes))
                                             .ToList();
                return result;
            }
        }

        /// <inheritdoc />
        public IEnumerable<Desordres> GetAllDesordresByDocument(int idDocument)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                var listedocumentdesordres = context.DesordresDocuments.Where(x => x.IdDocument == idDocument).Select(i => i.IdDesordre).ToList();
                if (listedocumentdesordres != null && listedocumentdesordres.Any())
                {
                    var desordres = context.Desordres.Include(i => i.FamillesDesordresTypes)
                       .Where(i => i.FamillesDesordresTypes.IdFamilleDesordre == 2 && listedocumentdesordres.Contains(i.Id))
                                        .ToList();
                    return desordres;
                }
            }

            return null;
        }

        /// <inheritdoc />
        public IEnumerable<TypesOuvrages> GetAllTypeOuvrages()
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                return context.TypesOuvrages.ToList();
            }
        }

        /// <inheritdoc />
        public int SaveProgrammation(Programmations newProgrammation)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                context.Programmations.Add(newProgrammation);

                context.SaveChanges();

                return newProgrammation.IdProgrammation;
            }
        }

        /// <inheritdoc />
        public IEnumerable<ProgrammationDocumentPGE> GetAllDocumentPgeByProgrammation(int idProgrammation)
        {
            IEnumerable<ProgrammationDocumentPGE> listePgeCalcule = null;

            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                listePgeCalcule = context.ProgrammationDocumentPGE
                    .Where(x => x.IdProgrammation == idProgrammation)
                    .Include(i => i.Programmations)
                    .Include(i => i.Documents)
                    .ToList();
            }

            return listePgeCalcule;
        }

        /// <inheritdoc />
        public ProcesVerbaux GetPvByDocumentPge(string numAffaire)
        {
            ProcesVerbaux resultat = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                resultat = context.ProcesVerbaux
                    .Include(i => i.GroupeOuvrage)
                    .Include(i => i.GroupeOuvrage.Select(ii => ii.Lieux.Descriptions))          // récupérer que les lieux gestionnaire
                    .Include(i => i.GroupeOuvrage.Select(ii => ii.Lieux.Lignes))    // récupérer que les lignes gestionnaire
                    .Include(i => i.Localisations)
                    .Include(i => i.Localisations.Select(ii => ii.Desordres.Select(x => x.DesordresDocuments.Select(xx => xx.Documents))))
                    .Where(x => x.Localisations.Any(p => p.Desordres.Any(pp => pp.DesordresDocuments.Any(ppp => ppp.Documents.IdDocumentType == 10 && ppp.Documents.NumeroAffaire == numAffaire)))).FirstOrDefault();
            }

            return resultat;
        }

        /// <inheritdoc />
        public void UpdateProgrammation(Programmations programmation)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                var programmationPoco = context.Programmations.Find(programmation.IdProgrammation);
                if (programmationPoco != null)
                {
                    programmationPoco.PrixUnitaire = programmation.PrixUnitaire;
                    programmationPoco.BudgetDisponible = programmation.BudgetDisponible;
                }
                context.SaveChanges();
            }
        }

        /// <inheritdoc />
        public Programmations GetOneProgrammation(int idProgrammation)
        {
            Programmations programmation = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                programmation = context.Programmations.Where(x => x.IdProgrammation == idProgrammation).SingleOrDefault();
            }

            return programmation;
        }

        public Programmations GetOneProgrammationByTypeNatureAnnee(string TypesOuvrages, string natureTravaux, int? anneeProgrammation)
        {
            Programmations programmation = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                if (anneeProgrammation == 0)
                {
                    programmation = context.Programmations
                        .Where(x => x.NatureTravaux.Libelle == natureTravaux && x.TypeOuvrage.Libelle == TypesOuvrages)
                        .OrderByDescending(x => x.DateCreation)
                        .First();
                }
                else
                {
                    programmation = context.Programmations
                        .Where(x => x.NatureTravaux.Libelle == natureTravaux && x.TypeOuvrage.Libelle == TypesOuvrages && x.AnneeProgrammation == anneeProgrammation)
                        .OrderByDescending(x => x.DateCreation)
                        .FirstOrDefault();
                }
            }

            return programmation;
        }

        /// <inheritdoc />
        public double GetSurfaceOuvragesByPge(int idOuvrage)
        {
            double surface = 0;
            double outConvertDouble;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                string id = idOuvrage.ToString();
                var listeOuvrageAnnexe = context.DataAnnexe.Where(x => x.IdOuvrage == id).ToList();
                if (listeOuvrageAnnexe != null && listeOuvrageAnnexe.Any())
                {
                    ///TODO YAF à tester cette convertion suite à la modification de type de la prop surface dans DataAnnexe
                    //surface = listeOuvrageAnnexe.Sum(x => x.Surface_developpe.GetValueOrDefault());
                    foreach (var item in listeOuvrageAnnexe)
                    {
                        if (double.TryParse(item.Surface_developpe, out outConvertDouble))
                        {
                            surface += outConvertDouble;
                        }
                    }
                }
            }

            return surface;
        }

        /// <inheritdoc />
        public ProgrammationDocumentPGE AddProgrammationDocumentPGE(ProgrammationDocumentPGE pge)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                context.ProgrammationDocumentPGE.Add(pge);
                context.SaveChanges();
            }
            return pge;
        }

        /// <inheritdoc />
        public void UpdateProgrammationDocumentPGE(ProgrammationDocumentPGE pge)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                var pgeToUpdate = context.ProgrammationDocumentPGE.Find(pge.Id);
                if (pgeToUpdate != null)
                {
                    //Information de la PGE
                    pgeToUpdate.NumRang = pge.NumRang;
                    pgeToUpdate.NumMedian = pge.NumMedian;
                    pgeToUpdate.IsExAequo = pge.IsExAequo; // = true
                    pgeToUpdate.IsValideEx = pge.IsValideEx; //== true
                    pgeToUpdate.Commentaire = pge.Commentaire;

                    //Information lié aux travaux sur plusieurs années
                    pgeToUpdate.Annee = pge.Annee;
                    pgeToUpdate.Budget = pge.Budget;
                    pgeToUpdate.IsTravauxPlusieursAnnee = pge.IsTravauxPlusieursAnnee;

                }

                context.SaveChanges();
            }
        }

        /// <inheritdoc />
        public ProgrammationDocumentPGE GetOneProgrammationDocumentPge(int idProgrammationDocumentPge)
        {
            ProgrammationDocumentPGE programmationDocumentPGE = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                programmationDocumentPGE = context.ProgrammationDocumentPGE.Where(x => x.Id == idProgrammationDocumentPge).SingleOrDefault();
            }

            return programmationDocumentPGE;
        }

        /// <inheritdoc />
        public IEnumerable<ProgrammationValeurParametresDocument> GetAllValeurParametreByPge(int idDocumentPge)
        {
            IEnumerable<ProgrammationValeurParametresDocument> listeValeurParametrePge = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                listeValeurParametrePge = context.ProgrammationValeurParametresDocument.Where(x => x.IDDocumentPge == idDocumentPge)
                    .Include(i => i.ParametresHidalgo)
                    .Include(i => i.ParametresHidalgo.Criteres).ToList();
            }

            return listeValeurParametrePge;
        }

        /// <inheritdoc />
        public IEnumerable<Programmations> GetAllProgrammation()
        {
            IEnumerable<Programmations> listProgrammation = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                //listProgrammation = context.Programmations.OrderByDescending(x => x.UserModification).ToList(); 
                listProgrammation = context.Programmations.OrderByDescending(x => x.IdProgrammation).ToList();
            }

            return listProgrammation;
        }

        /// <inheritdoc />
        IEnumerable<ProgrammationDetails> IHidalgoRepository.GetAllProgrammationDetailsByProgrammation(int idProgrammation)
        {
            IEnumerable<ProgrammationDetails> listProgrammationDetails;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                listProgrammationDetails = context.ProgrammationDetails.Where(x => x.IdProgrammation == idProgrammation).ToList();
            }
            return listProgrammationDetails;
        }

        /// <inheritdoc />
        public void RemoveOneProgrammation(Programmations programmation, IEnumerable<ProgrammationDetails> listProgrammationDetails, IEnumerable<ProgrammationDocumentPGE> listProgrammationDocumentPGE, IEnumerable<ProgrammationValeurParametresDocument> listProgrammationValeurParametresDocument)
        {
            //TODO RDT : Base de données passage en casecade au moin pour les [ProgrammationValeurParametresDocument] et si possible aussi pour 

            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                //TODO YAF: Rework à faire : préconiser l'Unit of work

                ///Suppression des dependances
                //var programmationDetailToDelete = context.ProgrammationDetails.Find(listProgrammationDetails.Select(x => x.IdProgrammationDetail).ToArray());

                ///Suppression des ProgrammationDetails
                foreach (ProgrammationDetails element in listProgrammationDetails)
                {
                    var programmationDetailToDelete = context.ProgrammationDetails.Find(element.IdProgrammationDetail);
                    context.ProgrammationDetails.Remove(programmationDetailToDelete);
                }//context.ProgrammationDetails.RemoveRange(listProgrammationDetails);

                ///Suppression des Valeurs des Parametres des PGE
                foreach (ProgrammationValeurParametresDocument element in listProgrammationValeurParametresDocument)
                {
                    var programmationValeurToDelete = context.ProgrammationValeurParametresDocument.Find(element.IDParametreHidalgo, element.IDDocumentPge);
                    context.ProgrammationValeurParametresDocument.Remove(programmationValeurToDelete);
                }//context.ProgrammationValeurParametresDocument.RemoveRange(listProgrammationValeurParametresDocument);

                ///Suppression des PGE
                foreach (ProgrammationDocumentPGE element in listProgrammationDocumentPGE)
                {
                    var programmationPgeToDelete = context.ProgrammationDocumentPGE.Find(element.Id);
                    context.ProgrammationDocumentPGE.Remove(programmationPgeToDelete);
                }//context.ProgrammationDocumentPGE.RemoveRange(listProgrammationDocumentPGE);

                ///Suppression de la programmation
                var programmationToDelete = context.Programmations.Find(programmation.IdProgrammation);
                context.Programmations.Remove(programmationToDelete);


                context.SaveChanges();
            }
        }

        public IEnumerable<ProgrammationValeurParametresDocument> GetAllProgrammationValeurParametresDocument(int idProgrammation)
        {
            IEnumerable<ProgrammationValeurParametresDocument> listeValeurParametrePge = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                listeValeurParametrePge = context.ProgrammationValeurParametresDocument
                    .Where(x => x.ProgrammationDocumentPGE.IdProgrammation == idProgrammation)
                    .Include(i => i.ProgrammationDocumentPGE)
                    .Include(i => i.ParametresHidalgo)
                    .Include(i => i.ParametresHidalgo.Criteres).ToList();
            }
            return listeValeurParametrePge;
        }
    }
}