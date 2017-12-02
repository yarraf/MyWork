using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.Data.Contract.Entities
{
    public class VDocumentPge
    {
        public int Id { get; set; }
        public string NumeroAffaire { get; set; }
        public int IdDocumentType { get; set; }
        public int? Note { get; set; }
    }
}
