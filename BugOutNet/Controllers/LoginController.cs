using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PerformLogin( FormCollection collection )
        {
            int i = 0;

            i++;



            return RedirectToAction( "Index", "Bugs" );

        }
    }
}