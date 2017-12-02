using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
  public  class TravauxExternesDto : HistoriqueDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Obtient ou définit la Lignes
        /// </summary>
        public LigneDto Lignes { get; set; }

        /// <summary>
        /// Obtient ou définit le lieu
        /// </summary>
        public LieuDto Lieu { get; set; }

        /// <summary>
        /// Obtient ou définit la date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Obtient ou définit la nature travaux externes
        /// </summary>
        public NatureTravauxExternesDto NatureTravauxExt { get; set; }
    }
}
