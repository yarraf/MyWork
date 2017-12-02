using Ratp.Hidalgo.Data.Contract.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    public class ProgrammationDto
    {
        public int Id { get; set; }
        public ENatureCalibrage NatureTravaux { get; set; }

        public IList<LigneDto> Lignes { get; set; }

        public int Anneeprogrammation { get; set; }

        public ENatureCalibrage NatureCalibrage { get; set; }

        public IList<RnmDto> Rnms { get; set; }

        public IList<TravauxExternesDto> TravauxExternes { get; set; }

        public decimal? PrixUnitaire { get; set; }

        public decimal? Budget { get; set; }

        public int UserCreation { get; set; }
        public DateTime DateCreation { get; set; }
        public int? UserModification { get; set; }
        public DateTime? DateModification { get; set; }

    }
}
