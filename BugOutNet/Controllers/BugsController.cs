using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Helpers;
using BugOutNetLibrary.Logging;
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
                    this.AddNewBug( model );           
                }
                catch( Exception ex )
                {
                    Logger.Error( ex );
                    return View( "Error" );
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BugViewModel model)
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    model.SaveStatus = String.Empty;

                    var bug = _db.Bugs.Find( model.Id );

                    if( bug != null )
                    {
                        EditBug( bug, model );
                        model.SaveStatus = "<label style='color:green;'>Save Successfull</label>";
                    }
                    else
                    {
                        return new HttpStatusCodeResult( HttpStatusCode.NotFound, "Bug : " + model.Name + " not found." );
                    }
                }
                catch( Exception ex )
                {
                    Logger.Error( ex );
                    model.SaveStatus = "<label style='color:red;'>Save Failed</label>";
                }
            }
            else
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            return View( "_EditBug", model );

        }

        [UserActionFilter]
        public ActionResult Delete(int id)
        {
            try
            {
                var bug = _db.Bugs.Find( id );

                if( bug != null )
                {
                    _db.Bugs.Remove( bug );
                    _db.SaveChanges();

                    return new HttpStatusCodeResult( HttpStatusCode.OK );

                }
            }
            catch( Exception ex )
            {
                Logger.Error( ex );
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.Message );
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
                    var bugModel = GetBugModel( id.Value );
                    return PartialView( viewName, bugModel );

                default:
                    break;

            }

            return PartialView( viewName );
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="HttpException">404;Couldn't find file.</exception>
        [UserActionFilter]
        public ActionResult GetFile( int id )
        {
            var file = _db.BugAttachments.FirstOrDefault( attachment => attachment.Id == id );

            if( file != null )
            {
                return File( file.Attachment, "application/octet-stream", file.FileName );
            }
            else
            {
                throw new HttpException( 404, "Couldn't find file." );
            }
        }

        /// <summary>
        /// Gets the bug model.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private BugViewModel GetBugModel(int id)
        {
            BugViewModel model = null;

            var bug = _db.Bugs.Find( id );

            if( bug != null )
            {
                model = new BugViewModel();
                model.Id = bug.Id;
                model.Description = bug.Description;
                model.Name = bug.Name;
                model.ProjectId = bug.ProjectId;
                model.CategoryId = bug.CategoryId;
                model.PriorityId = bug.PriorityId;
                model.StatusId = bug.StatusId;
                model.AssigntedToId = bug.AssignedToId;

                model.Attachments.AddRange( bug.BugAttachments.Select( attachment => new BugAttachmentViewModel
                {
                    Id = attachment.Id,
                    FileName = attachment.FileName
                } ) );

                model.Comments.AddRange( bug.BugComments.Select( comment => new BugCommentViewModel
                {
                    Id = comment.Id,
                    Comment = comment.Comment,
                    Created = comment.Created,
                    CreatedBy = _db.Users.Where( user => user.Id == comment.CreatorId ).FirstOrDefault() == null ?  String.Empty :
                                _db.Users.Where( user => user.Id == comment.CreatorId ).FirstOrDefault().UserName
                } ) );
                
            }

            return model;

        }

        /// <summary>
        /// Adds the new bug.
        /// </summary>
        /// <param name="bug">The bug.</param>
        /// <param name="model">The model.</param>
        private void AddNewBug( BugViewModel model )
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

            // make sure our Id is new after insert  
            _db.SaveChanges();

            if( model.FileUpload != null &&
                model.FileUpload.InputStream != null )
            {
                BugAttachment newAttachment = new BugAttachment();
                newAttachment.BugId = newBug.Id;
                newAttachment.Attachment = HashHelper.GetBytesFromUpload( model.FileUpload );
                newAttachment.FileName = model.FileUpload.FileName;
                newAttachment.Created = DateTime.Now;
                newAttachment.UploadedById = newBug.CreatorId;

                _db.BugAttachments.Add( newAttachment );
                _db.SaveChanges();
            }

        }

        /// <summary>
        /// Edits the bug.
        /// </summary>
        /// <param name="bug">The bug.</param>
        /// <param name="model">The model.</param>
        private void EditBug( Bug bug, BugViewModel model)
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

            // add new attachment if needed
            if( model.FileUpload != null &&
                model.FileUpload.InputStream != null )
            {
                BugAttachment newAttachment = new BugAttachment();
                newAttachment.BugId = bug.Id;
                newAttachment.Attachment = HashHelper.GetBytesFromUpload( model.FileUpload );
                newAttachment.FileName = model.FileUpload.FileName;
                newAttachment.Created = DateTime.Now;
                newAttachment.UploadedById = bug.CreatorId;

                _db.BugAttachments.Add( newAttachment );
                _db.SaveChanges();

            }
            
            // add new comment if needed
            if( !String.IsNullOrWhiteSpace(model.NewComment))
            {
                _db.BugComments.Add( new BugComment
                {
                    BugId = bug.Id,
                    Comment = model.NewComment.Trim(),
                    Created = DateTime.Now,
                    CreatorId = bug.CreatorId
                } );

                _db.SaveChanges();

            }
        }

    }
}