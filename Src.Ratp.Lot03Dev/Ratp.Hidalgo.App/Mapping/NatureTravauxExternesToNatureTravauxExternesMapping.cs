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
    /// Transforme des données de type <see cref="NatureTravauxExternes"/> to <see cref="NatureTravauxExternesDto"/>
    /// </summary>
    public class NatureTravauxExternesToNatureTravauxExternesMapping : IMapper<NatureTravauxExternes, NatureTravauxExternesDto>
    {
        /// <inheritdoc />
        public NatureTravauxExternesDto Map(NatureTravauxExternes source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "NatureTravauxExternes"));
            }

            NatureTravauxExternesDto target = new NatureTravauxExternesDto();
            this.Map(source, target);
            return target;
        }

        /// <inheritdoc />
        public void Map(NatureTravauxExternes source, NatureTravauxExternesDto target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "NatureTravauxExternes"));
            }

            if (target == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "NatureTravauxExternesDto"));
            }

            target.Id = source.Id;
            target.Libelle = source.Libelle;
        }
    }
}
