using Ratp.Hidalgo.App.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ratp.Hidalgo.Web.Controllers
{
    public class HomeController : Controller
    {
        private IHidalgoApp HidalgoService { get; set; }

        public HomeController(IHidalgoApp serviceHidalgo)
        {
            this.HidalgoService = serviceHidalgo;
        }
        public ActionResult Index()
        {
          //var model =   this.HidalgoService.GetAllDocumentPgeByNatureTravaux(Data.Contract.Enum.ENatureCalibrage.Enduit);
          //  return View(model);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}