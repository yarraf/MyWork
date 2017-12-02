using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    /// <summary>
    /// DTO Coefficient Pondération & paramétres et critères
    /// </summary>
    public class CoefficientPonderationDto
    {
        /// <summary>
        /// Obtient ou définit la propriete Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtient ou définit la propriete Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit la propriete Libelle
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la proriete Valeur
        /// </summary>
        public decimal? ValeurMaconnerie { get; set; }

        /// <summary>
        /// Obtient ou définit la proriete Valeur
        /// </summary>
        public decimal? ValeurEnduit { get; set; }
    }
}
