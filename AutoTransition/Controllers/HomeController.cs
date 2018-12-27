using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoTransition.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult Info()
        {
            return View();
        }

        public ActionResult CompleteLoadsInfo()
        {
            return View();
        }

        public ActionResult DangerousLoadsInfo()
        {
            return View();
        }

        public ActionResult RefrigeratedTransportInfo()
        {
            return View();
        }

        public ActionResult GroupageLoadsInfo()
        {
            return View();
        }
    }
}