using Ratp.Hidalgo.App.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ratp.Hidalgo.Web.ViewModel
{
    /// <summary>
    /// ViewModel pour géréer l'interface RNM
    /// </summary>
    public class RnmVm
    {
        public LigneDto Ligne { get; set; }
        public LieuDto Lieu { get; set; }
        public int Annee { get; set; }
    }
}