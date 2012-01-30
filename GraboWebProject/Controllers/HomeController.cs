using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraboWebProject.Controllers
{
    public class HomeController : Controller
    {
        private GrabooDBEntities entities = new GrabooDBEntities();

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View( entities.Products.ToList() );
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
