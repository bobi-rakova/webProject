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
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            HttpCookie usersCookie = null;
            HttpCookieCollection cookies = System.Web.HttpContext.Current.Request.Cookies;
            for (int i = 0; i < cookies.Count; i++)
            {
                if (cookies.Get(i).Name.Equals("coolCookie"))
                {
                    usersCookie = cookies.Get(i);
                }
            }
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

        public ActionResult CreatePurchase()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePurchase( Purchase purchase )
        {
            this.entities.AddToPurchases( purchase );
            this.entities.SaveChanges();

            return View();
        }
                
        public ActionResult GetProducts()
        {
            var data = from product in this.entities.Products
                       select new 
                       {
                           Name = product.Name,
                           Producer = product.Producer,
                           Carbohydrates = product.Carbohydrates,
                           Fat = product.Fat,
                           Proteins = product.Proteins,
                           Description = product.Description                           
                       };

           return Json( data, JsonRequestBehavior.AllowGet );
        }
    }
}
