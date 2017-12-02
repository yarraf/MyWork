using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    public class ClassementDto
    {
        public string NumeroAffaire { get; set; }
        public char ClassementPgeB0 { get; set; }
        public char ClassementPgeB1 { get; set; }
        public char ClassementPgeB2 { get; set; }
        public char ClassementPgeB3 { get; set; }
        public char ClassementPgeB4 { get; set; }
    }
}
