using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WiFiLocationServer.DB;

namespace WiFiLocationServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult LocationShow()
        {
            return View();
        }

        public ActionResult CoordDataShow()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}
