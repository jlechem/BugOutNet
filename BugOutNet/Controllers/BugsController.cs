using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
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
        private Entities _db = new Entities();

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
        public ActionResult GetBugs(int projectId, string sidx, string sort, int page, int rows )
        {
            IQueryable<Bug> bugs = _db.Bugs;

            // 0 means all projects
            if( projectId <= 0 )
            {
                bugs = bugs.Where( bug => bug.AssignedToId == SessionManager.User.Id );
            }
            // > 0 means a specific project
            else
            {
                bugs = bugs.Where( bug => bug.AssignedToId == SessionManager.User.Id &&
                                   bug.ProjectId == projectId );
            }

            sort = ( sort == null ) ? "" : sort;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            int totalRecords = bugs.Count();            
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sort.ToUpper() == "DESC" )
            {
                bugs = bugs.OrderByDescending( t => t.Id );
                bugs = bugs.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                bugs = bugs.OrderBy( t => t.Id );
                bugs = bugs.Skip( pageIndex * pageSize ).Take( pageSize );
            }

            var bugModel = bugs.Select( bug =>
                new BugGridModel
                {
                    Id = bug.Id,
                    AssignedTo = bug.User.UserName,
                    Category = bug.Category.Name,
                    Description = bug.Description,
                    Name = bug.Name,
                    Project = bug.Project.Name,
                    Priority = bug.Priority.Name,
                    Status = bug.Status.Name,
                    LastUpdatedOn = bug.LatUpdated
                } );

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = bugModel
            };

            return Json( jsonData, JsonRequestBehavior.AllowGet );

        }

    }
}