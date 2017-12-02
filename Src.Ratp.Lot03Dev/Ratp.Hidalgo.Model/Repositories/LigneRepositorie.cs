using Ratp.Hidalgo.Data.Contract.Repositories;
using System.Collections.Generic;
using System.Linq;
using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Model.Properties;
using System.Data.Entity;
using System;

namespace Ratp.Hidalgo.Model.Repositories
{
    /// <summary>
    /// Implémentation de l'interface <see cref="ILigneRepositorie"/>
    /// </summary>
    public class LigneRepositorie : ILigneRepositorie
    {
        /// <inheritdoc />
        public IEnumerable<Lignes> GetAllLigneByFamille()
        {
            IEnumerable<Lignes> listeLigneGestionnaire = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                //listeLigneGestionnaire = context.Lignes.Where(x => x.IdFamilleLigne == 1 && x.IdFamilleLigne == 2 && x.Actif).ToList();
                listeLigneGestionnaire = context.Lignes.Where(x => x.Actif && x.IdFamilleLigne == 1 || x.IdFamilleLigne == 2).ToList().OrderBy(x => x.OrdreAffichage);
            }

            return listeLigneGestionnaire;
        }

        /// <inheritdoc />
        public IEnumerable<Lieux> GetAllLieuxByLigne(int idLigneGestionnaire)
        {
            IEnumerable<Lieux> listeLieuxGestionnaire = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                listeLieuxGestionnaire = context.Lieux.Where(x => (x.Actif.HasValue && x.Actif.Value) && x.IdLigneGestionnaire == idLigneGestionnaire)
                                                     .Include(x => x.Descriptions)
                                                     .ToList().OrderBy(x=> x.Ordre);

                foreach (var Lieux in listeLieuxGestionnaire)
                {
                    if (Lieux.Ordre == null)
                    {
                        Lieux.Ordre = 99999;
                    }
                }
            }

            return listeLieuxGestionnaire.OrderBy(x => x.Ordre); 
        }

        /// <inheritdoc />
        public IEnumerable<NatureTravauxExternes> GetAllNatureTravauxExternes()
        {
            IEnumerable<NatureTravauxExternes> listeNatureTravauxExternes = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                listeNatureTravauxExternes = context.NatureTravauxExternes.ToList().OrderBy(x => x.Id);
            }

            return listeNatureTravauxExternes;
        }
    }
}
