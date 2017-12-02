using Ratp.Hidalgo.Data.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    public class ValeursParametresPgeDto
    {
        public int Id { get; set; }
        public ProgrammationDocumentPGE ProgrammationDocumentPGE { get; set; }
        public string Parametre { get; set; }
        public string Critere { get; set; }
        public Nullable<double> Note { get; set; }
    }
}
