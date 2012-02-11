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
            return View();
        }

        [HttpPost]
        public ActionResult Find(FormCollection values)
        {
            string criteria = values["criteria"];

            foundProducts = entities.Products.Where(x => x.Name.Contains(criteria)).ToList();

            return View(foundProducts);
        }

        public ActionResult SelectProduct(FormCollection collection)
        {
            string selected = (string)Request["selected"];

            var foundProduct = entities.Products.Where(x => x.Name == selected).ToList().ElementAt(0);

            return View(foundProduct);
        }

    }
}
