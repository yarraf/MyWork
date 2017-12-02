using Ratp.Hidalgo.App.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ratp.Hidalgo.Web.ViewModel
{
    /// <summary>
    /// ViewModel pour gérer le les travaux externes
    /// </summary>
    public class TravauxExternesVm
    {
        /// <summary>
        /// Obtient ou définit la ligne
        /// </summary>
        public LigneDto Ligne { get; set; }

        /// <summary>
        /// Obtient ou définit le lieu
        /// </summary>
        public LieuDto Lieu { get; set; }

        /// <summary>
        /// Obtient ou définit la date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Obtient ou définit la nature travaux externes
        /// </summary>
        public NatureTravauxExternesDto NatureTravauxExt { get; set; }
    }
}