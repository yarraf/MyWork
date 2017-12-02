using Ratp.Hidalgo.Data.Contract.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    /// <summary>
    /// Classe DTO pour transferer l'objet dto de la couche présentation à la couche business 
    /// </summary>
    public class ProgrammationDocumentPgeDto
    {
        public int Id { get; set; }
        public ProgrammationDto Programmation { get; set; }

        public int IdDocument { get; set; }
        public string NumeroAffaire { get; set; }
        public int Rang { get; set; }
        public int Median { get; set; }
        public LigneDto Ligne { get; set; }
        public LieuDto Lieu { get; set; }
        public bool? IsEx { get; set; }
        public bool? IsValidEx { get; set; }
        public double? Surface { get; set; }
        public string Commentaire { get; set; }
        public bool? IsTravauxPlusieursAnnee { get; set; }
        public int? Annee { get; set; }
        public Decimal? Budget { get; set; }
        public string Categorie { get; set; }
    }
}
