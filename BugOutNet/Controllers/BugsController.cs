using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using BugOutNetLibrary.Models.ViewModels;
using BugOutNetLibrary.Repos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        public ActionResult GetBugs( int projectId, string sidx, string sord, int page, int rows )
        {
            SessionManager.SelectedProjectId = projectId;

            sord = ( sord == null ) ? "" : sord;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            var tempBugs = _db.Bugs.Where( x => x.Id > 0 );

            // 0 means all projects
            if( projectId > 0 )
            {
                tempBugs = tempBugs.Where( bug => bug.ProjectId == projectId );
            }

            var bugs = from bug in tempBugs
                       join project in _db.Projects on bug.ProjectId equals project.Id
                       join category in _db.Categories on bug.CategoryId equals category.Id
                       join status in _db.Statuses on bug.StatusId equals status.Id
                       join priority in _db.Priorities on bug.PriorityId equals priority.Id
                       select new BugGridModel
                       {
                           Id = bug.Id,
                           Name = bug.Name,
                           Description = bug.Description,
                           AssignedTo = bug.User.UserName,
                           Project = project.Name,
                           Category = category.Name,
                           Priority = priority.Name,
                           Status = status.Name,
                           LastUpdatedOn = bug.LatUpdated
                       };

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
        public ActionResult Add( BugViewModel model  )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    Bug newBug = new Bug();
                    newBug.Name = model.Name;
                    newBug.Description = model.Description;
                    newBug.AssignedToId = model.AssigntedToId;
                    newBug.ProjectId = model.ProjectId;
                    newBug.CategoryId = model.CategoryId;
                    newBug.PriorityId = model.PriorityId;
                    newBug.StatusId = model.StatusId;
                    newBug.Created = DateTime.Now;
                    newBug.LatUpdated = DateTime.Now;
                    newBug.CreatorId = SessionManager.User.Id;
                    newBug.LastUpdatedId = SessionManager.User.Id;

                    // save the bug into the DB
                    _db.Bugs.Add( newBug );

                    // make sure our Id is now new    
                    _db.SaveChanges();

                    if( model.FileUpload.InputStream != null )
                    {
                        BugAttachment newAttachment = new BugAttachment();
                        newAttachment.BugId = newBug.Id;

                        using( var inputStream = model.FileUpload.InputStream )
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;

                            if( memoryStream == null )
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo( memoryStream );
                            }

                            newAttachment.Attachment = memoryStream.ToArray();
                            newAttachment.FileName = model.FileUpload.FileName;
                            newAttachment.Created = DateTime.Now;
                            newAttachment.UploadedById = newBug.CreatorId;

                            _db.BugAttachments.Add( newAttachment );
                            _db.SaveChanges();

                        }
                    }               
                }
                catch( Exception ex )
                {
                    return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
                }
            }
            else
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            return RedirectToAction( "Index", "Bugs" );

        }

        [HttpPost]
        [UserActionFilter]
        public ActionResult Edit(BugViewModel model)
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    var bug = _db.Bugs.Find( model.Id );

                    if( bug != null )
                    {
                        // updaate the bug into the DB
                        bug.Name = model.Name;
                        bug.Description = model.Description;
                        bug.AssignedToId = model.AssigntedToId;
                        bug.ProjectId = model.ProjectId;
                        bug.CategoryId = model.CategoryId;
                        bug.PriorityId = model.PriorityId;
                        bug.StatusId = model.StatusId;
                        bug.LatUpdated = DateTime.Now;
                        bug.LastUpdatedId = SessionManager.User.Id;
                            
                        _db.SaveChanges();
                    }
                    else
                    {

                    }
                }
                catch( Exception ex )
                {
                    return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
                }
            }
            else
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            return RedirectToAction( "Index", "Bugs" );

        }

        [UserActionFilter]
        public ActionResult Delete(int id)
        {
            var bug = _db.Bugs.Find( id );

            if( bug != null )
            {
                _db.Bugs.Remove( bug );
                _db.SaveChanges();

                return new HttpStatusCodeResult( HttpStatusCode.OK );

            }

            return new HttpStatusCodeResult( HttpStatusCode.NotFound, "Bug with Id: " + id + " not found." );

        }

        /// <summary>
        /// Gets the bug view.
        /// </summary>
        /// <param name="bugstyle">The bugstyle.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [UserActionFilter]
        public ActionResult GetBugView( string bugstyle, int? id = null )
        {
            string viewName = String.Empty;

            switch( bugstyle.ToLower( CultureInfo.InvariantCulture ) )
            {
                case "bugs":
                    viewName = "_Bugs";
                    break;

                case "addbug":
                    viewName = "_AddBug";
                    break;

                case "editbug":

                    viewName = "_EditBug";

                    var bug = _db.Bugs.Find( id.Value );

                    if( bug != null )
                    {
                        BugViewModel model = new BugViewModel();
                        model.Id = bug.Id;
                        model.Description = bug.Description;
                        model.Name = bug.Name;
                        model.ProjectId = bug.ProjectId;
                        model.CategoryId = bug.CategoryId;
                        model.PriorityId = bug.PriorityId;
                        model.StatusId = bug.StatusId;
                        model.AssigntedToId = bug.AssignedToId;

                        return PartialView( viewName, model );

                    }

                    return PartialView( viewName );

                default:
                    break;

            }

            return PartialView( viewName );
        }

    }
}