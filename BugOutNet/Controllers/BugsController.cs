using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class BugsController : Controller
    {
        // GET: Bugs
        [UserActionFilter]
        public ActionResult Index()
        {
            return View();

        }

        /// <summary>
        /// Gets the bugs.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public ActionResult GetBugs(int projectId)
        {
            return View();
        }

    }
}