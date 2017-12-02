using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    public class CritereDto
    {
        public string NumeroAffaire { get; set; }
        public double valeur { get; set; }
        public CritereEnum Critere { get; set; }
    }
}
