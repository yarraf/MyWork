using Core.Common.Mapping;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Mapping
{
    public class ProgrammationDtoToProgrammationMapping : IMapper<ProgrammationDto, Programmations>, IMapper<Programmations, ProgrammationDto>
    {
        private IMapper<RnmDto, ProgrammationDetails> mapperRnmDetail;
        private IMapper<TravauxExternesDto, ProgrammationDetails> mapperTravauxExternes;
        private IMapper<LigneDto, ProgrammationDetails> mapperLignesGestionnaire;
        public ProgrammationDtoToProgrammationMapping()
        {
            this.mapperRnmDetail = new RnmToProgrammationDetailMapper();
            this.mapperTravauxExternes = new RnmToProgrammationDetailMapper();
            this.mapperLignesGestionnaire = new LignesGestionnaireToProgrammationDetailMapper();
        }

        /// <inheritdoc />
        public Programmations Map(ProgrammationDto source)
        {
            var target = new Programmations();
            this.Map(source, target);
            return target;
        }

        /// <inheritdoc />
        public void Map(ProgrammationDto source, Programmations target)
        {
            target.IdProgrammation = source.Id;
            target.AnneeProgrammation = source.Anneeprogrammation;
            target.IdNatureTravaux = (int)source.NatureTravaux;
            target.IdTypeOuvrage = 1;

            target.PrixUnitaire = source.PrixUnitaire;
            target.BudgetDisponible = source.Budget;

            //Affectation pour le test
            target.UserCreation = source.UserCreation;
            target.UserModification = source.UserModification;

            target.DateCreation = source.DateCreation;
            target.DateModification = source.DateModification;

            //Programmation Détails RNM
            if (source.Rnms != null && source.Rnms.Any())
            {
                foreach (var rnm in source.Rnms)
                {
                    target.ProgrammationDetails.Add(mapperRnmDetail.Map(rnm));
                }
            }

            //Programmation Détail TRavaux Externes
            if (source.TravauxExternes != null && source.TravauxExternes.Any())
            {
                foreach (var travauxExt in source.TravauxExternes)
                {
                    target.ProgrammationDetails.Add(mapperTravauxExternes.Map(travauxExt));
                }
            }

            //Mapping des lignes gestionnairess
            if (source.Lignes != null && source.Lignes.Any())
            {
                foreach (var Lignes in source.Lignes)
                {
                    target.ProgrammationDetails.Add(mapperLignesGestionnaire.Map(Lignes));
                }
            }

        }

        /// <inheritdoc />
        public void Map(Programmations source, ProgrammationDto target)
        {
            target.Id = source.IdProgrammation;
            target.Anneeprogrammation = source.AnneeProgrammation;
            //target.NatureTravaux = source.IdNatureTravaux; // TODO RDT assigner NatureTravaux
            //target.IdTypeOuvrage = 1; // TODO RDT assigner Type D'ouvrage

            target.PrixUnitaire = source.PrixUnitaire;
            target.Budget = source.BudgetDisponible;

            //Affectation pour le test
            target.UserCreation = source.UserCreation;
            target.UserModification = source.UserModification;

            target.DateCreation = source.DateCreation;
            target.DateModification = source.DateModification;
        }

        /// <inheritdoc />
        public ProgrammationDto Map(Programmations source)
        {
            var target = new ProgrammationDto();
            this.Map(source, target);
            return target;
        }
    }
}
