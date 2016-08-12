using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class CategoriesController : Controller
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
        public ActionResult Get( string sidx, string sort, int page, int rows )
        {
            sort = ( sort == null ) ? "" : sort;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            var projects = _db.Categories.Select(
                project => new CategoryGridModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Created = project.Created,
                    CreatedBy = _db.Users.FirstOrDefault( user => user.Id == project.CreatorId ).UserName
                } );

            int totalRecords = projects.Count();
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sort.ToUpper() == "DESC" )
            {
                projects = projects.OrderByDescending( t => t.Id );
                projects = projects.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                projects = projects.OrderBy( t => t.Id );
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
        public ActionResult Edit( CategoryGridModel model )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {

                    var category = _db.Categories.Find( model.Id );

                    if( category != null )
                    {
                        category.Name = model.Name;
                        category.Description = model.Name;

                        _db.SaveChanges();

                        return new HttpStatusCodeResult( HttpStatusCode.OK );

                    }
                    else
                    {
                        return HttpNotFound( "Category: " + model.Name + " not found." );
                    }
                }
                catch( Exception ex )
                {
                    return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
                }
            }

            return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );

        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AdminActionFilter]
        //[ValidateAntiForgeryToken]
        public ActionResult Create( [Bind( Exclude = "Id" )] CategoryGridModel model )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    _db.Categories.Add(
                        new Category
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

            return new HttpStatusCodeResult( HttpStatusCode.InternalServerError );

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

                var category = _db.Categories.Find( Id );

                if( category == null )
                {
                    return HttpNotFound( "Category Id: " + Id + " not found." );
                }
                else
                {
                    _db.Categories.Remove( category );
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