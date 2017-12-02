using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    public class HistoriqueDto
    {
        public int UserCreation { get; set; }
        public int? UserModification { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }
    }
}
