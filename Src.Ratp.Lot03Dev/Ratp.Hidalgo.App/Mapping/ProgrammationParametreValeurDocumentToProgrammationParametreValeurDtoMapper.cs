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
    public class ProgrammationParametreValeurDocumentToProgrammationParametreValeurDtoMapper : IMapper<ValeursParametresPgeDto ,ProgrammationValeurParametresDocument >, IMapper<ProgrammationValeurParametresDocument, ValeursParametresPgeDto>
    {

        public ProgrammationValeurParametresDocument Map(ValeursParametresPgeDto source)
        {
            var target = new ProgrammationValeurParametresDocument();
            this.Map(source, target);
            return target;
        }

        public void Map(ValeursParametresPgeDto source, ProgrammationValeurParametresDocument target)
        {

           /* target.IDDocumentPge = source.
            target.IDCritere
            target.IDParametreHidalgo
            target.Note*/
        }

        public ValeursParametresPgeDto Map(ProgrammationValeurParametresDocument source)
        {
            var target = new ValeursParametresPgeDto();
            this.Map(source, target);
            return target;
        }

        public void Map(ProgrammationValeurParametresDocument source, ValeursParametresPgeDto target)
        {
            target.ProgrammationDocumentPGE = source.ProgrammationDocumentPGE;
            target.Parametre = source.ParametresHidalgo.Libelle;
            target.Critere = source.ParametresHidalgo.Criteres.libelle;
            target.Note = source.Note;

            //switch (source.IDCritere)
            //{
            //    case 1:
            //        target.Critere = "C1";
            //        break;
            //    case 2:
            //        target.Critere = "C2";
            //        break;
            //    case 3:
            //        target.Critere = "C3";
            //        break;
            //    case 4:
            //        target.Critere = "C4";
            //        break;
            //}
        }
    }
}
