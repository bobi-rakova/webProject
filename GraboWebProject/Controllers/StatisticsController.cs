using System;
using System.IO;
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
            HttpPostedFileWrapper picture = (HttpPostedFileWrapper)Request.Files.Get("pic");

            int index = entities.Products.Count();
            FileStream output = new FileStream(HomeController.CONTENT_DIR + picture.FileName, FileMode.Create);
            Stream input = picture.InputStream;
            byte[] buff = new byte[4096];
            while (input.Read(buff, 0, 4096) > 0)
            {
                output.Write(buff, 0, 4096);
            }

            product.Image = picture.FileName;
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
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            purchase.User_Id = user.Id;
            this.entities.AddToPurchases( purchase );
            this.entities.SaveChanges();

            return View();
        }
                
        public ActionResult GetProducts()
        {
            var data = from product in this.entities.Products
                       select new 
                       {
                           Id = product.Id,
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
