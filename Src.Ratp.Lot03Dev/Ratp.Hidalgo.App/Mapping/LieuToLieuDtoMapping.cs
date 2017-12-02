using Core.Common.Mapping;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using System;
using System.Globalization;

namespace Ratp.Hidalgo.App.Mapping
{
    /// <summary>
    /// Transforme des données de type <see cref="Lieux/> To <see cref="LieuDto"/>
    /// </summary>
    class LieuToLieuDtoMapping: IMapper<Lieux, LieuDto>
    {
        /// <inheritdoc />
        public LieuDto Map(Lieux source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "Lieux"));
            }

            LieuDto target = new LieuDto();
            this.Map(source, target);
            return target;
        }

        /// <inheritdoc />
        public void Map(Lieux source, LieuDto target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "Lieux"));
            }

            if (target == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "LieuDto"));
            }

            target.Id = source.Id;
            if(source.Descriptions != null)
            {
                target.Name = source.Descriptions.Libelle;
            }
        }
    }
}