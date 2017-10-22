using BC.Intranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BC.Intranet.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            model.PageTitle = "Välkommen till Mowido Admin";

            return View(model);
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