using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ratp.Hidalgo.Web.ViewModel
{
    public class RootObjectRnmVm
    {
        /// <summary>
        /// Obtient ou définit la liste des RNM
        /// </summary>
        public IEnumerable<RnmVm> ListeRnm { get; set; }
    }
}