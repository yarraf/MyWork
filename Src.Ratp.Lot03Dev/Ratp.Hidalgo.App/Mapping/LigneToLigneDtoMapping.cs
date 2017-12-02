using Core.Common.Mapping;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Mapping
{
    /// <summary>
    /// Classe pour transformer les données de type <see cref="Lignes"/> to <see cref="LigneDto"/>
    /// </summary>
    public class LigneToLigneDtoMapping : IMapper<Lignes, LigneDto>
    {
        /// <inheritdoc />
        public LigneDto Map(Lignes source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "Lignes"));
            }

            LigneDto target = new LigneDto();
            this.Map(source, target);
            return target;
        }

        /// <inheritdoc />
        public void Map(Lignes source, LigneDto target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "Lignes"));
            }

            if (target == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "LigneDto"));
            }

            target.Id = source.Id;
            target.Name = source.Nom;
            if (source.CheminIcone.Contains("\\"))
            {
                var cheminsplit = source.CheminIcone.Split('\\');
                target.Chemin = string.Format("{0}/{1}", cheminsplit[0], cheminsplit[2]);
            }

            if (source.Id == 1 || source.Id == 14)
            {
                target.Checked = true;
                target.Activee = "disabled";
            }
        }
    }
}
