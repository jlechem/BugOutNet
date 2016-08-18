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
        public ActionResult Get( string sidx, string sord, int page, int rows )
        {
            sord = ( sord == null ) ? "" : sord;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            var categories = _db.Categories.Select(
                category => new CategoryGridModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Created = category.Created,
                    CreatedBy = _db.Users.FirstOrDefault( user => user.Id == category.CreatorId ).UserName
                } );

            int totalRecords = categories.Count();
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sord.ToUpper( CultureInfo.InvariantCulture ) == "DESC" )
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {
                    case "ID":
                        categories = categories.OrderByDescending( t => t.Id );
                        break;

                    case "NAME":
                        categories = categories.OrderByDescending( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        categories = categories.OrderByDescending( t => t.Description );
                        break;

                    case "CREATED":
                        categories = categories.OrderByDescending( t => t.Created );
                        break;

                    default:
                        categories = categories.OrderByDescending( t => t.Id );
                        break;

                }

                categories = categories.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {

                    case "ID":
                        categories = categories.OrderBy( t => t.Id );
                        break;

                    case "NAME":
                        categories = categories.OrderBy( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        categories = categories.OrderBy( t => t.Description );
                        break;

                    case "CREATED":
                        categories = categories.OrderBy( t => t.Created );
                        break;

                    default:
                        categories = categories.OrderBy( t => t.Id );
                        break;
                }

                categories = categories.Skip( pageIndex * pageSize ).Take( pageSize );
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = categories
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
                        category.Description = model.Description;

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