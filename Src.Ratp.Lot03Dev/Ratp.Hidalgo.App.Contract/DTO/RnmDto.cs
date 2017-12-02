using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
   public class RnmDto: HistoriqueDto
    {
        public int Id { get; set; }
        public LigneDto Lignes { get; set; }
        public LieuDto Lieu { get; set; }
        public int Annee { get; set; }
    }
}
