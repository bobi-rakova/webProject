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
            if (user != null)
            {
                HttpCookie usersCookie = null;
                HttpCookieCollection cookies = System.Web.HttpContext.Current.Request.Cookies;
                for (int i = 0; i < cookies.Count; i++)
                {
                    if (cookies.Get(i).Name.Equals("coolCookie"))
                    {
                        usersCookie = cookies.Get(i);
                    }
                }
                var myPurchases = entities.Purchases.Where(x => x.User_Id == userId).ToList();
                return View(myPurchases);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AddProduct()
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                HttpPostedFileWrapper picture = (HttpPostedFileWrapper)Request.Files.Get("pic");

                if (picture != null && picture.FileName.Length > 0 && picture.InputStream.Length < 10485760)
                {
                    string picName = picture.FileName;
                    string extension = picName.Substring(picName.LastIndexOf('.'));

                    if (IsPresent(picName, HomeController.CONTENT_DIR))
                    {
                        string name = picName.Substring(0, picName.LastIndexOf('.'));
                        int index = 2;
                        while (IsPresent(name + index + extension, HomeController.CONTENT_DIR))
                        {
                            index++;
                        }
                        picName = name + index + extension;
                    }

                    using (FileStream output = new FileStream(HomeController.CONTENT_DIR + picName, FileMode.Create))
                    {
                        using (Stream input = picture.InputStream)
                        {
                            byte[] buff = new byte[4096];
                            int n;
                            while ((n = input.Read(buff, 0, 4096)) > 0)
                            {
                                output.Write(buff, 0, n);
                            }
                        }
                    }

                    product.Image = picName;
                }
                this.entities.AddToProducts(product);
                this.entities.SaveChanges();

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public bool IsPresent(string file, string dir)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name.Equals(file))
                {
                    return true;
                }
            }
            return false;
        }

        public ActionResult CreatePurchase()
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CreatePurchase(Purchase purchase)
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                purchase.User_Id = user.Id;
                this.entities.AddToPurchases(purchase);
                this.entities.SaveChanges();

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetProducts()
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
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

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
