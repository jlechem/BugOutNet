using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class PrioritiesController : Controller
    {
        #region Fields

        private Entities _db = new Entities();

        #endregion

        /// <summary>
        /// Gets the prioriites.
        /// </summary>
        /// <param name="sidx">The sidx.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="page">The page.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        [AdminActionFilter]
        //[ValidateAntiForgeryToken]
        public ActionResult Get( string sidx, string sord, int page, int rows )
        {
            sord = ( sord == null ) ? "" : sord;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            var priorities = _db.Priorities.Select(
                priority => new PrioritiesGridModel
                {
                    Id = priority.Id,
                    Name = priority.Name,
                    Description = priority.Description,
                    Created = priority.Created,
                    CreatedBy = _db.Users.FirstOrDefault( user => user.Id == priority.CreatorId ).UserName
                } );

            int totalRecords = priorities.Count();
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sord.ToUpper( CultureInfo.InvariantCulture ) == "DESC" )
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {
                    case "ID":
                        priorities = priorities.OrderByDescending( t => t.Id );
                        break;

                    case "NAME":
                        priorities = priorities.OrderByDescending( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        priorities = priorities.OrderByDescending( t => t.Description );
                        break;

                    case "CREATED":
                        priorities = priorities.OrderByDescending( t => t.Created );
                        break;

                    default:
                        priorities = priorities.OrderByDescending( t => t.Id );
                        break;

                }

                priorities = priorities.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {

                    case "ID":
                        priorities = priorities.OrderBy( t => t.Id );
                        break;

                    case "NAME":
                        priorities = priorities.OrderBy( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        priorities = priorities.OrderBy( t => t.Description );
                        break;

                    case "CREATED":
                        priorities = priorities.OrderBy( t => t.Created );
                        break;

                    default:
                        priorities = priorities.OrderBy( t => t.Id );
                        break;
                }

                priorities = priorities.Skip( pageIndex * pageSize ).Take( pageSize );
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = priorities
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
        public ActionResult Edit( PrioritiesGridModel model )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    var priority = _db.Priorities.Find( model.Id );

                    if( priority != null )
                    {
                        priority.Name = model.Name;
                        priority.Description = model.Description;

                        _db.SaveChanges();

                        return new HttpStatusCodeResult( HttpStatusCode.OK );
                    }
                    else
                    {
                        return HttpNotFound( "Priority: " + model.Name + " not found." );
                    }
                }
                catch( Exception ex )
                {
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
        public ActionResult Create( [Bind( Exclude = "Id" )] PrioritiesGridModel model )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    _db.Priorities.Add(
                        new Priority
                        {
                            Created = DateTime.Now,
                            CreatorId = SessionManager.User.Id,
                            Name = model.Name,
                            Description = model.Description
                        } );

                    _db.SaveChanges();

                }
                catch( Exception ex )
                {
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
        public ActionResult Delete( int Id )
        {
            try
            {
                var priority = _db.Priorities.Find( Id );

                if( priority == null )
                {
                    return HttpNotFound( "Priority Id: " + Id + " not found." );
                }
                else
                {
                    _db.Priorities.Remove( priority );
                    _db.SaveChanges();
                }
            }
            catch( Exception ex )
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
            }

            return new HttpStatusCodeResult( HttpStatusCode.OK );

        }
    }
}