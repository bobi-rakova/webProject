using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraboWebProject.Controllers
{
    public class HomeController : Controller
    {
        private int userId = 1;
        private GrabooDBEntities entities = new GrabooDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create( User user )
        {
            this.entities.AddToUsers( user );           
            this.entities.SaveChanges();

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

    }
    
}
