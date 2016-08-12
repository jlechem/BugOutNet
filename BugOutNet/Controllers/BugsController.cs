using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Repos;
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
        [UserActionFilter]
        public ActionResult GetBugs(int projectId)
        {
            var bugs = BugRepo.GetBugs( projectId );
            return Json( bugs, JsonRequestBehavior.AllowGet );
        }

    }
}