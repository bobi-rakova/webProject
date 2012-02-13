using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraboWebProject.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        private GrabooDBEntities entities = new GrabooDBEntities();
        private List<Product> foundProducts = null;

        public ActionResult Index()
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
        public ActionResult Find(FormCollection values)
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                string criteria = values["criteria"];

                foundProducts = entities.Products.Where(x => x.Name.Contains(criteria)).ToList();

                return View(foundProducts);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SelectProduct()
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                string selected = (string)Request["selected"];

                var foundProduct = entities.Products.Where(x => x.Name == selected).ToList().ElementAt(0);

                return View(foundProduct);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
