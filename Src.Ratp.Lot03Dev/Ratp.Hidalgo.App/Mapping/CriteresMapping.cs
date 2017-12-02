using Core.Common.Mapping;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using System;
using System.Globalization;

namespace Ratp.Hidalgo.App.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class CriteresMapping : IMapper<VDocumentPge, DocumentDto>
    {
        public void Map(VDocumentPge source, DocumentDto target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "VDocumentPge"));
            }

            if (target == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "DocumentDto"));
            }

            target.NumeroAffaire = source.NumeroAffaire;
            target.Note = source.Note.GetValueOrDefault();
        }

        public DocumentDto Map(VDocumentPge source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "VDocumentPge"));
            }

            DocumentDto target = new DocumentDto();
            this.Map(source, target);
            return target;
        }
    }
}
