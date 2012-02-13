using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using GraboWebProject.Models;

namespace GraboWebProject.Controllers
{
    public class HomeController : Controller
    {
        public static string CONTENT_DIR = "C:/Users/HaNdyMaN/webProject/GraboWebProject/Content/";
        private GrabooDBEntities entities = new GrabooDBEntities();

        public ActionResult Index()
        {
            ControllerContext.HttpContext.Session["loggedInUser"] = null;
            return LogOn();
        }

        public ActionResult Create()
        {
            ControllerContext.HttpContext.Session["loggedInUser"] = null;
            return View();
        }
        
        [HttpPost]
        public ActionResult Create( User user )
        {
            ControllerContext.HttpContext.Session["loggedInUser"] = null;
            List<User> present = entities.Users.Where(x => x.Username == user.Username).ToList();

            if (present.Count > 0)
            {
                ModelState.AddModelError("", String.Format("A user with the username {0} is already present.", user.Username));
                return View(user);
            }
            this.entities.AddToUsers( user );           
            this.entities.SaveChanges();

            return View();
        }

        public ActionResult About()
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

        public ActionResult LogOn()
        {
            ControllerContext.HttpContext.Session["loggedInUser"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            ControllerContext.HttpContext.Session["loggedInUser"] = null;
            if (ModelState.IsValid)
            {
                List<User> present = entities.Users.Where(x => x.Username == model.UserName && x.Password == model.Password).ToList();

                if (present.Count == 1)
                {
                    if (model.RememberMe)
                    {
                        if (System.Web.HttpContext.Current.Response.Cookies["coolCookie"] == null)
                        {
                            HttpCookie cookie = new HttpCookie("coolCookie");
                            string encrypted = Convert.ToBase64String(Encoding.GetEncoding("Unicode").GetBytes(model.Password));
//                            string decrypted =  Encoding.GetEncoding("Unicode").GetString(Convert.FromBase64String(encrypted));
                            cookie.Values.Add(model.UserName, encrypted);
                            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                        }
                        else
                        {
                            string encrypted = Convert.ToBase64String(Encoding.GetEncoding("Unicode").GetBytes(model.Password));
//                            string decrypted =  Encoding.GetEncoding("Unicode").GetString(Convert.FromBase64String(encrypted));
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
                                userPass.Set(model.UserName, encrypted);
                                System.Web.HttpContext.Current.Response.Cookies["coolCookie"].Values.Add(userPass);
                            }

                        }
                    }

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
