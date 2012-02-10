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
            

            var myPurchases = entities.Purchases.Where( x => x.User_Id == userId ).ToList();

            return View( myPurchases );
        }

        public ActionResult About()
        {
            return View();
        }

    }
    
}
