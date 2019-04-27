using Express.Common.SiteOperate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataAnalysis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurUser.LoginName))
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            return View();
        }

        public ActionResult DashBoard()
        {

            return View();
        }

        public ActionResult Map()
        {
            return View();
        }
        public ActionResult Table()
        {

            return View();
        }
    }
}