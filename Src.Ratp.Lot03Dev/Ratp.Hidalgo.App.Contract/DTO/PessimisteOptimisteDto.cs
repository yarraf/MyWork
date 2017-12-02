using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    public class PessimisteOptimisteDto
    {
        public string NumeroAffaire { get; set; }
        public float B0 { get; set; }
        public float B1 { get; set; }
        public float B2 { get; set; }
        public float B3 { get; set; }
        public float B4 { get; set; }
    }
}
