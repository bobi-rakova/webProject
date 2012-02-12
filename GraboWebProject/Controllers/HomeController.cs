using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraboWebProject.Models;

namespace GraboWebProject.Controllers
{
    public class HomeController : Controller
    {
        public static string CONTENT_DIR = "C:/Users/HaNdyMaN/webProject/GraboWebProject/Content/";
        private int userId = 1;
        private GrabooDBEntities entities = new GrabooDBEntities();

        public ActionResult Index()
        {
            return LogOn();
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

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (model.RememberMe)
                {
                    if (System.Web.HttpContext.Current.Response.Cookies["coolCookie"] == null)
                    {
                        HttpCookie cookie = new HttpCookie("coolCookie");
                        cookie.Values.Add(model.UserName, model.Password);
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        HttpCookie coolCookie = System.Web.HttpContext.Current.Response.Cookies["coolCookie"];
                        bool userPresent = false;
                        NameValueCollection nameValues = coolCookie.Values;
                        for (int i = 0; i < nameValues.Count; i++)
                        {
                            if (nameValues.GetKey(i).Equals(model.UserName))
                            {
                                userPresent = true;
                                break;
                            }
                        }

                        if (!userPresent)
                        {
                            NameValueCollection userPass = new NameValueCollection();
                            userPass.Set(model.UserName, model.Password);
                            System.Web.HttpContext.Current.Response.Cookies["coolCookie"].Values.Add(userPass);
                        }

                    }
                }
                //if ( Url.IsLocalUrl( returnUrl ) && returnUrl.Length > 1 && returnUrl.StartsWith( "/" )
                //    && !returnUrl.StartsWith( "//" ) && !returnUrl.StartsWith( "/\\" ) )
                //{
                //    return Redirect( returnUrl );
                //}

                List<User> present = entities.Users.Where(x => x.Username == model.UserName && x.Password == model.Password).ToList();

                if (present.Count == 1)
                {
                    ControllerContext.HttpContext.Session["loggedInUser"] = present.ElementAt(0);
                    return RedirectToAction("Index", "Statistics");
                }

                else if (present.Count == 0)
                {
                    ModelState.AddModelError("", "The user couldn't be found.");
                }

                else
                {
                    // bre
                }

            }

            // If we got this far, something failed, redisplay form
//            return View(model);
            return View(model);
        }

    }
    
}
