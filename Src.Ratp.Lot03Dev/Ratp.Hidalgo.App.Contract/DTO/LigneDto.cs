using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    /// <summary>
    /// Classe DTO Lignes
    /// </summary>
    public class LigneDto : HistoriqueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public string Activee { get; set; }
        public string Chemin { get; set; }
    }
}
