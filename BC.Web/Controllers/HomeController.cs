using BC.Web.Models;
using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;

namespace BC.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        [OutputCache(Duration = 216000, VaryByCustom = "none")]
        public ActionResult Index()
        {
            BillionCompanyDbContext db = BillionCompanyDbContext.Create();

            var model = new HomeViewModel();
            model.PageTitle = Resources.Resources.PageTitle;
            model.Header = "Välkommen till Mowido!";

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