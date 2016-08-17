using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using BugOutNetLibrary.Models.ViewModels;
using BugOutNetLibrary.Repos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
        public ActionResult GetBugs(int projectId, string sidx, string sord, int page, int rows )
        {
            sord = ( sord == null ) ? "" : sord;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            var tempBugs = _db.Bugs.Where( x => x.Id > 0 );

            // 0 means all projects
            if( projectId > 0 )
            {
                tempBugs = tempBugs.Where( bug => bug.ProjectId == projectId );
            }

            var bugs = tempBugs.Select( bug =>
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

            int totalRecords = bugs.Count();            
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sord.ToUpper( CultureInfo.InvariantCulture ) == "DESC" )
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {
                    case "ID":
                        bugs = bugs.OrderByDescending( t => t.Id );
                        break;

                    case "NAME":
                        bugs = bugs.OrderByDescending( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        bugs = bugs.OrderByDescending( t => t.Description );
                        break;

                    case "PROJECT":
                        bugs = bugs.OrderByDescending( t => t.Project );
                        break;

                    case "CATEGORY":
                        bugs = bugs.OrderByDescending( t => t.Category );
                        break;

                    case "PRIORITY":
                        bugs = bugs.OrderByDescending( t => t.Priority );
                        break;

                    case "STATUS":
                        bugs = bugs.OrderByDescending( t => t.Status );
                        break;

                    case "ASSIGNEDTO":
                        bugs = bugs.OrderByDescending( t => t.AssignedTo );
                        break;

                    default:
                        bugs = bugs.OrderByDescending( t => t.Id );
                        break;

                }

                bugs = bugs.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {

                    case "ID":
                        bugs = bugs.OrderBy( t => t.Id );
                        break;

                    case "NAME":
                        bugs = bugs.OrderBy( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        bugs = bugs.OrderBy( t => t.Description );
                        break;

                    case "PROJECT":
                        bugs = bugs.OrderBy( t => t.Project );
                        break;

                    case "CATEGORY":
                        bugs = bugs.OrderBy( t => t.Category );
                        break;

                    case "PRIORITY":
                        bugs = bugs.OrderBy( t => t.Priority );
                        break;

                    case "STATUS":
                        bugs = bugs.OrderBy( t => t.Status );
                        break;

                    case "ASSIGNEDTO":
                        bugs = bugs.OrderBy( t => t.AssignedTo );
                        break;

                    default:
                        bugs = bugs.OrderBy( t => t.Id );
                        break;
                }

                bugs = bugs.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = bugs
            };

            return Json( jsonData, JsonRequestBehavior.AllowGet );

        }

        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [UserActionFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Add( BugViewModel model )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    // save the bug into the DB
                    _db.Bugs.Add(
                        new Bug
                        {
                            Name = model.Name,
                            Description = model.Description,
                            AssignedToId = model.AssigntedToId,
                            ProjectId = model.ProjectId,
                            CategoryId = model.CategoryId,
                            PriorityId = model.PriorityId,
                            StatusId = model.StatusId,
                            Created = DateTime.Now,
                            LatUpdated = DateTime.Now,
                            CreatorId = SessionManager.User.Id,
                            LastUpdatedId = SessionManager.User.Id
                        } );

                    _db.SaveChanges();
                }
                catch(Exception ex)
                {
                    return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
                } 
            }
            else
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            return RedirectToAction("Index","Bugs");

        }

        /// <summary>
        /// Gets the bug view.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [UserActionFilter]
        public ActionResult GetBugView( string name )
        {
            string viewName = String.Empty;

            switch( name )
            {
                case "bugs":
                    viewName = "_Bugs";
                    break;

                case "addbug":
                    viewName = "_AddBug";
                    break;

                default:
                    break;

            }

            return PartialView( viewName );
        }

    }
}