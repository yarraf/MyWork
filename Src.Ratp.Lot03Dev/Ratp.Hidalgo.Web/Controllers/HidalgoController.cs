using Ratp.Hidalgo.App.Contract;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract.Enum;
using Ratp.Hidalgo.Web.Infra;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Ratp.Hidalgo.Web.Controllers
{
    public class HidalgoController : Controller
    {
        private IHidalgoApp _hidalgoApp;

        public HidalgoController(IHidalgoApp hidalgoApp)
        {
            _hidalgoApp = hidalgoApp;
        }

        //[UserFilter]
        // GET: Hidalgo
        public ActionResult Index()
        {
            return View();
        }
    }
}