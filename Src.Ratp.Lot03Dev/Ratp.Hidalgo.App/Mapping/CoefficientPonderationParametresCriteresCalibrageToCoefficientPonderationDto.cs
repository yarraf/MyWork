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
    /// 
    /// </summary>
    public class CoefficientPonderationParametresCriteresCalibrageToCoefficientPonderationDto : IMapper<CoefficientPonderationParametresCriteresCalibrage, CoefficientPonderationDto>, IMapper<CoefficientPonderationDto, CoefficientPonderationParametresCriteresCalibrage>
    {
        public CoefficientPonderationParametresCriteresCalibrage Map(CoefficientPonderationDto source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "CoefficientPonderationParametresCriteresCalibrage"));
            }

            CoefficientPonderationParametresCriteresCalibrage target = new CoefficientPonderationParametresCriteresCalibrage();
            this.Map(source, target);
            return target;
        }

        public CoefficientPonderationDto Map(CoefficientPonderationParametresCriteresCalibrage source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "CoefficientPonderationParametresCriteresCalibrage"));
            }

            CoefficientPonderationDto target = new CoefficientPonderationDto();
            this.Map(source, target);
            return target;
        }

        public void Map(CoefficientPonderationDto source, CoefficientPonderationParametresCriteresCalibrage target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "CoefficientPonderationDto"));
            }

            if (target == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "CoefficientPonderationParametresCriteresCalibrage"));
            }

            target.Id = source.Id;
            target.Code = source.Code;
            target.ValeurMaconnerie = source.ValeurMaconnerie;
            target.ValeurEnduit = source.ValeurEnduit;
            target.Libelle = source.Libelle;
        }

        /// <summary>
        /// Trasforme des données de type <see cref="CoefficientPonderationParametresCriteresCalibrage"/> to <see cref="CoefficientPonderationDto"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void Map(CoefficientPonderationParametresCriteresCalibrage source, CoefficientPonderationDto target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "CoefficientPonderationParametresCriteresCalibrage"));
            }

            if (target == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "L'objet {0} fourni en entrée ne doit pas être vide ou null", "CoefficientPonderationIhm"));
            }

            target.Id = source.Id;
            target.Code = source.Code;
            target.Libelle = source.Libelle;
            target.ValeurMaconnerie = source.ValeurMaconnerie;
            target.ValeurEnduit = source.ValeurEnduit;
        }
    }
}
