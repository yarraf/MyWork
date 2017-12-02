using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    public class PreordreFinalDto
    {
        public int IdDocument { get; set; }
        public string NumeroAffaire { get; set; }
        public int Rang { get; set; }
        public int Median { get; set; }
        public bool IsEx { get; set; }
    }
}
