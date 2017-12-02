using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
   public class TypeOuvragesDto
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public string Enabled { get; set; }
    }
}