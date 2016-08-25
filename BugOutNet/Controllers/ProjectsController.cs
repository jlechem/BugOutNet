using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Logging;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
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
    public class ProjectsController : Controller
    {
        #region Fields

        private Entities _db = new Entities();

        #endregion

        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <returns></returns>
        [AdminActionFilter]
        //[ValidateAntiForgeryToken]
        public ActionResult GetProjects( string sidx, string sord, int page, int rows )
        {
            sord = ( sord == null ) ? "" : sord;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            var projects = _db.Projects.Select(
                project => new ProjectGridModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Created = project.Created.HasValue ? project.Created.Value : (DateTime?)null,
                    CreatedBy = _db.Users.FirstOrDefault( user => user.Id == project.CreatorId ).UserName
                } );

            int totalRecords = projects.Count();
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sord.ToUpper(CultureInfo.InvariantCulture) == "DESC" )
            {
                switch(sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {
                    case "ID":
                        projects = projects.OrderByDescending( t => t.Id );
                        break;

                    case "NAME":
                        projects = projects.OrderByDescending( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        projects = projects.OrderByDescending( t => t.Description );
                        break;

                    case "CREATED":
                        projects = projects.OrderByDescending( t => t.Created );
                        break;
                        
                    default:
                        projects = projects.OrderByDescending( t => t.Id );
                        break;

                }
                
                projects = projects.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {

                    case "ID":
                        projects = projects.OrderBy( t => t.Id );
                        break;

                    case "NAME":
                        projects = projects.OrderBy( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        projects = projects.OrderBy( t => t.Description );
                        break;

                    case "CREATED":
                        projects = projects.OrderBy( t => t.Created );
                        break;

                    default:
                        projects = projects.OrderBy( t => t.Id );
                        break;
                }
            
                projects = projects.Skip( pageIndex * pageSize ).Take( pageSize );
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = projects
            };

            return Json( jsonData, JsonRequestBehavior.AllowGet );

        }

        /// <summary>
        /// Edits the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AdminActionFilter]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectGridModel model)
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {

                    var project = _db.Projects.Find( model.Id );

                    if( project != null )
                    {
                        project.Name = model.Name;
                        project.Description = model.Description;

                        _db.SaveChanges();

                        return new HttpStatusCodeResult( HttpStatusCode.OK );

                    }
                    else
                    {
                        return HttpNotFound( "Project: " + model.Name + " not found." );
                    }
                }
                catch( Exception ex )
                {
                    Logger.Error( ex );
                    return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
                }
            }

            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );

        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AdminActionFilter]
        //[ValidateAntiForgeryToken]
        public ActionResult Create( [Bind( Exclude = "Id" )] ProjectGridModel model )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    _db.Projects.Add(
                        new Project
                        {
                            Created = DateTime.Now,
                            CreatorId = SessionManager.User.Id,
                            IsActive = true,
                            Name = model.Name,
                            Description = model.Description
                        } );

                    _db.SaveChanges();
                    
                }
                catch(Exception ex)
                {
                    Logger.Error( ex );
                    return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
                }

                return new HttpStatusCodeResult( HttpStatusCode.OK );

            }

            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );

        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [AdminActionFilter]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int Id )
        {
            try
            {

                var project = _db.Projects.Find( Id );

                if( project == null )
                {
                    return HttpNotFound( "Project Id: " + Id + " not found." );
                }
                else
                {
                    _db.Projects.Remove( project );
                    _db.SaveChanges();
                }
            }
            catch( Exception ex )
            {
                Logger.Error( ex );
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
            }

            return new HttpStatusCodeResult( HttpStatusCode.OK );

        }
    }
}