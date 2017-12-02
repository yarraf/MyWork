using Core.Common.Mapping;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Data.Contract.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Mapping
{
    public class RnmToProgrammationDetailMapper : IMapper<RnmDto, ProgrammationDetails>, IMapper<TravauxExternesDto, ProgrammationDetails>
    {
        /// <inheritdoc />
        public ProgrammationDetails Map(TravauxExternesDto source)
        {
            var target = new ProgrammationDetails();
            this.Map(source, target);
            return target;
        }

        /// <inheritdoc />
        public ProgrammationDetails Map(RnmDto source)
        {
            var target = new ProgrammationDetails();
            this.Map(source, target);
            return target;
        }

        /// <inheritdoc />
        public void Map(TravauxExternesDto source, ProgrammationDetails target)
        {
            target.IdProgrammationDetail = source.Id;
            target.IdLieu = source.Lieu.Id;
            target.IdLigne = source.Lignes.Id;
            target.IdCategorie = (int)ECategorieProgrammationDetails.TRAVAUXEXTERNES;
            if (!string.IsNullOrEmpty(source.Date))
            {
                var date = source.Date.Split('/');
                target.Date = new DateTime(int.Parse(date[1]), int.Parse(date[0]), 1);
            }

            target.IdNatureTravauxExternes = source.NatureTravauxExt.Id;

            target.DateCreation = source.DateCreation;
            target.DateModification = source.DateModification;
            target.UserCreation = source.UserCreation;
            target.UserModification = source.UserModification;
        }

        /// <inheritdoc />
        public void Map(RnmDto source, ProgrammationDetails target)
        {
            target.IdProgrammationDetail = source.Id;
            target.IdLigne = source.Lieu.Id;
            target.IdLieu = source.Lieu.Id;
            target.IdCategorie = (int)ECategorieProgrammationDetails.RNM;

            //TODO YAR: besoin de stocke que l'années: alors un champ 'Annee' à ajouter dans la table, pour stocker l'information de l'annéess
            target.Date = new DateTime(source.Annee, 1, 1);

            target.DateCreation = source.DateCreation;
            target.DateModification = source.DateModification;
            target.UserCreation = source.UserCreation;
            target.UserModification = source.UserModification;
        }
    }
}
