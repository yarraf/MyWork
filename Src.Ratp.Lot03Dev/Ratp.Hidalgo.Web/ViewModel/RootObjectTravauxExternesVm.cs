using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ratp.Hidalgo.Web.ViewModel
{
    public class RootObjectTravauxExternesVm
    {
        /// <summary>
        /// Obtient ou définit la liste des travaux externes
        /// </summary>
        public IEnumerable<TravauxExternesVm> ListeTravauxExt { get; set; }
    }
}