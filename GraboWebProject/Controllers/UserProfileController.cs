using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraboWebProject.Controllers
{
    public class UserProfileController : Controller
    {
        private int userId = 1;
        private GrabooDBEntities entities = new GrabooDBEntities();

        //
        // GET: /UserProfile/
        public ActionResult Index()
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                User targetUser = entities.Users.Where(x => x.Id == userId).FirstOrDefault();

                return View(targetUser);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // GET: /UserProfile/Edit
        public ActionResult Edit( int userId )
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                User targetUser = entities.Users.Where(x => x.Id == userId).FirstOrDefault();
                return View(targetUser);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit( User user )
        {
            User userLogged = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (userLogged != null)
            {
                User targetUser = entities.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                targetUser.FirstName = user.FirstName;
                targetUser.LastName = user.LastName;
                targetUser.Age = user.Age;
                targetUser.Gender = user.Gender;

                this.entities.ObjectStateManager.ChangeObjectState(targetUser, System.Data.EntityState.Modified);

                this.entities.SaveChanges();
                return View(targetUser);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // GET: /UserProfile/EditHealthProfile
        public ActionResult HealthProfile( int userId )
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                User targetUser = entities.Users.Where(x => x.Id == userId).FirstOrDefault();

                return View(targetUser);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult HealthProfile( User user )
        {
            User userLogged = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (userLogged != null)
            {
                User targetUser = entities.Users.Where(x => x.Id == userId).FirstOrDefault();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult UpdateIngredients( int name, bool isChecked )
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                User targetUser = entities.Users.Where(x => x.Id == userId).FirstOrDefault();

                if (isChecked == true)
                {
                    Allergy allergy = new Allergy()
                    {
                        User_Id = targetUser.Id,
                        Ingredient_Id = name
                    };

                    targetUser.Allergies.Add(allergy);
                }
                else
                {
                    Allergy a = targetUser.Allergies.Where(x => x.Ingredient_Id == name).FirstOrDefault();
                    targetUser.Allergies.Remove(a);
                }

                entities.SaveChanges();

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        } 

        public ActionResult AddAllergy( int userId )
        {
            User user = (User)ControllerContext.HttpContext.Session["loggedInUser"];
            if (user != null)
            {
                User targetUser = entities.Users.Where(x => x.Id == userId).FirstOrDefault();

                return View(targetUser);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetIngredients()
        {
            User user = ( User )ControllerContext.HttpContext.Session["loggedInUser"];

            if (user != null)
            {
                var data = from ingredient in this.entities.Ingredients
                           select new
                           {
                               Id = ingredient.Id,
                               Name = ingredient.Name,
                               Description = ingredient.Description,
                               IsAvailable = 0
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
