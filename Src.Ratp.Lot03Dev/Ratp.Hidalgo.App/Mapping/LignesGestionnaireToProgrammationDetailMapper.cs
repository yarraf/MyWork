using Core.Common.Mapping;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Data.Contract.Enum;
using System;

namespace Ratp.Hidalgo.App.Mapping
{
    /// <summary>
    /// Transforme des données de type <see cref="LigneDto"/> to <see cref="ProgrammationDetails"/>
    /// </summary>
    public class LignesGestionnaireToProgrammationDetailMapper : IMapper<LigneDto, ProgrammationDetails>
    {
        /// <inheritdoc />
        public ProgrammationDetails Map(LigneDto source)
        {
            var target = new ProgrammationDetails();
            this.Map(source, target);
            return target;
        }

        /// <inheritdoc />
        public void Map(LigneDto source, ProgrammationDetails target)
        {
            target.IdProgrammationDetail = source.Id;
            target.IdLigne = source.Id;
            target.DateCreation = source.DateCreation;
            target.DateModification = source.DateModification;
            target.UserCreation = source.UserCreation;
            target.UserModification = source.UserModification;
            target.IdCategorie = (int)ECategorieProgrammationDetails.POLITIQUERENOVATION;
        }
    }
}
