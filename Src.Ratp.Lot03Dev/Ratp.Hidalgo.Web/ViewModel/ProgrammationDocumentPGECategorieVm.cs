using Ratp.Hidalgo.App.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ratp.Hidalgo.Web.ViewModel
{
    public class ProgrammationDocumentPGECategorieVm
    {
        public decimal? CoutByCategorie { get; set; }
        public IEnumerable<ProgrammationDocumentPgeDto> ListePGE { get; set; }
    }
}