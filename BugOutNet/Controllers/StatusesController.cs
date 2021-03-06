﻿using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Logging;
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
    public class StatusesController : Controller
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

            var statuses = _db.Statuses.Select(
                status => new StatusesGridModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    Created = status.Created,
                    CreatedBy = _db.Users.FirstOrDefault( user => user.Id == status.CreatorId ).UserName
                } );

            int totalRecords = statuses.Count();
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sord.ToUpper( CultureInfo.InvariantCulture ) == "DESC" )
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {
                    case "ID":
                        statuses = statuses.OrderByDescending( t => t.Id );
                        break;

                    case "NAME":
                        statuses = statuses.OrderByDescending( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        statuses = statuses.OrderByDescending( t => t.Description );
                        break;

                    case "CREATED":
                        statuses = statuses.OrderByDescending( t => t.Created );
                        break;

                    default:
                        statuses = statuses.OrderByDescending( t => t.Id );
                        break;

                }

                statuses = statuses.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {

                    case "ID":
                        statuses = statuses.OrderBy( t => t.Id );
                        break;

                    case "NAME":
                        statuses = statuses.OrderBy( t => t.Name );
                        break;

                    case "DESCRIPTION":
                        statuses = statuses.OrderBy( t => t.Description );
                        break;

                    case "CREATED":
                        statuses = statuses.OrderBy( t => t.Created );
                        break;

                    default:
                        statuses = statuses.OrderBy( t => t.Id );
                        break;
                }

                statuses = statuses.Skip( pageIndex * pageSize ).Take( pageSize );
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = statuses
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
        public ActionResult Edit( StatusesGridModel model )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    var status = _db.Statuses.Find( model.Id );

                    if( status != null )
                    {
                        status.Name = model.Name;
                        status.Description = model.Description;

                        _db.SaveChanges();

                        return new HttpStatusCodeResult( HttpStatusCode.OK );
                    }
                    else
                    {
                        return HttpNotFound( "Status: " + model.Name + " not found." );
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
        public ActionResult Create( [Bind( Exclude = "Id" )] StatusesGridModel model )
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    _db.Statuses.Add(
                        new Status
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
        public ActionResult Delete( int Id )
        {
            try
            {
                var status = _db.Statuses.Find( Id );

                if( status == null )
                {
                    return HttpNotFound( "Status Id: " + Id + " not found." );
                }
                else
                {
                    _db.Statuses.Remove( status );
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