using Core.Common.Mapping;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using System;
using System.Globalization;

namespace Ratp.Hidalgo.App.Mapping
{
    /// <summary>
    /// Trasformation des données de type <see cref="TypeOuvrage"/> to <see cref="TypeOuvragesDto"/>
    /// </summary>
    class TypeOuvragesToTypeOuvragesDtoMapping : IMapper<TypesOuvrages, TypeOuvragesDto>
    {
        /// <inheritdoc />
        public TypeOuvragesDto Map(TypesOuvrages source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "TypeOuvrage"));
            }

            TypeOuvragesDto target = new TypeOuvragesDto();
            this.Map(source, target);
            return target;
        }

        /// <inheritdoc />
        public void Map(TypesOuvrages source, TypeOuvragesDto target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "TypeOuvrage"));
            }

            if (target == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "TypeOuvragesDto"));
            }

            target.Id = source.Id;
            target.Libelle = source.Libelle;
            target.Enabled = "disabled";
        }
    }
}
