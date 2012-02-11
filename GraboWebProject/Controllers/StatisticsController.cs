using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraboWebProject.Controllers
{
    public class StatisticsController : Controller
    {
        private int userId = 1;
        private GrabooDBEntities entities = new GrabooDBEntities();

        //
        // GET: /Statistics/

        public ActionResult Index()
        {
            var myPurchases = entities.Purchases.Where( x => x.User_Id == userId ).ToList();
            return View( myPurchases );
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            this.entities.AddToProducts(product);
            this.entities.SaveChanges();

            return View();
        }

    }
}
