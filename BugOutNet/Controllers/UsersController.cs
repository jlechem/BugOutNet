using BugOutNet.Classes;
using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Helpers;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using BugOutNetLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class UsersController: Controller
    {
        Entities _db = new Entities();

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="sidx">The sidx.</param>
        /// <param name="sord">The sord.</param>
        /// <param name="page">The page.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        [AdminActionFilter]
        public ActionResult Get( string sidx, string sord, int page, int rows )
        {
            // check if we have a sort, null means nothing
            sord = ( sord == null ) ? "" : sord;

            // get the page index into an int (which page we're on)
            int pageIndex = Convert.ToInt32( page ) - 1;

            // get the number of items per page
            int pageSize = rows;

            // get some data from an Entity Framework source
            // you can get it from anywhere
            var users = _db.Users.Select(
                user => new UsersGridModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    EmailAddress = user.EmailAddress,
                    Name = user.FirstName + " " + user.LastName,
                    Created = user.Created
                } );

            // get the total count of records to displau
            int totalRecords = users.Count();

            // get the total number of pages to display
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            // check which direction if any we're sorting
            if( sord.ToUpper( CultureInfo.InvariantCulture ) == "DESC" )
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {
                    case "ID":
                        users = users.OrderByDescending( t => t.Id );
                        break;

                    case "USERNAME":
                        users = users.OrderByDescending( t => t.Username );
                        break;

                    case "NAME":
                        users = users.OrderByDescending( t => t.Name );
                        break;

                    case "EMAILADDRESS":
                        users = users.OrderByDescending( t => t.EmailAddress );
                        break;

                    case "CREATED":
                        users = users.OrderByDescending( t => t.Created );
                        break;

                    default:
                        users = users.OrderByDescending( t => t.Id );
                        break;

                }

                users = users.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {

                    case "ID":
                        users = users.OrderBy( t => t.Id );
                        break;

                    case "USERNAME":
                        users = users.OrderBy( t => t.Username );
                        break;

                    case "NAME":
                        users = users.OrderBy( t => t.Name );
                        break;

                    case "EMAILADDRESS":
                        users = users.OrderBy( t => t.EmailAddress );
                        break;

                    case "CREATED":
                        users = users.OrderBy( t => t.Created );
                        break;

                    default:
                        users = users.OrderBy( t => t.Id );
                        break;
                }

                users = users.Skip( pageIndex * pageSize ).Take( pageSize );
            }

            // we need to send back JSON in a very specific format
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = users
            };

            // send it back, the AllowGet isn't required since MVC 4 or 5 but it's good practice
            return Json( jsonData, JsonRequestBehavior.AllowGet );

        }

        [HttpPost]
        [AdminActionFilter]
        public ActionResult Add(UserViewModel model)
        {
            if( model != null && ModelState.IsValid )
            {
                try
                {
                    string salt = HashHelper.CreateSalt( 10 );

                    // save the bug into the DB
                    _db.Users.Add(
                        new User
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsAdmin = model.IsAdmin,
                            IsBlocked = model.IsBlocked,
                            IsVerified = model.IsVerified,
                            EmailAddress = model.EmailAddress,
                            Created = DateTime.Now,
                            Salt = salt,
                            Password = HashHelper.HashPassword( model.Password + salt ),
                            DefaultProjectId = model.DefaultProjectId,
                            AccessFailedCount = 0,
                            AutoLogin = false
                        } );

                    _db.SaveChanges();

                    // anytime we edit/add/delete users and succeed we need to refresh the cache
                    CacheManager.GetUsers( true );

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

            return new HttpStatusCodeResult( HttpStatusCode.OK );
        }

        [HttpPost]
        [AdminActionFilter]
        public ActionResult Edit( UserEditViewModel model )
        {
            HttpStatusCodeResult result = new HttpStatusCodeResult( HttpStatusCode.OK );

            if( model != null && ModelState.IsValid )
            {
                try
                {
                    string salt = HashHelper.CreateSalt( 10 );

                    User user = _db.Users.Find( model.Id );

                    if( user != null )
                    {
                        user.UserName = model.UserName;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.EmailAddress = model.EmailAddress;
                        user.IsAdmin = model.IsAdmin;
                        user.IsBlocked = model.IsBlocked;
                        user.IsVerified = model.IsVerified;
                        user.DefaultProjectId = model.DefaultProjectId;
                        user.Salt = salt;
                        user.Password = HashHelper.HashPassword( model.Password + salt );

                        _db.SaveChanges();

                        // anytime we edit/add/delete users and succeed we need to refresh the cache
                        CacheManager.GetUsers( true );

                    }
                    else
                    {
                        result = new HttpStatusCodeResult( HttpStatusCode.NotFound, "User with Id: " + model.Id + " not found." );
                    }
                }
                catch( Exception ex )
                {
                    result = new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
                }
            }
            else
            {
                result = new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            return result;
        }

        [AdminActionFilter]
        public ActionResult Delete(int id )
        {
            if( id == SessionManager.User.Id)
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest, "You cannot delete your own account." );
            }

            try
            {
                var user = _db.Users.Find( id );

                if( user == null )
                {
                    return HttpNotFound( "User Id: " + id + " not found." );
                }
                else
                {
                    _db.Users.Remove( user );
                    _db.SaveChanges();

                    // anytime we edit/add/delete users and succeed we need to refresh the cache
                    CacheManager.GetUsers( true );

                }
            }
            catch( Exception ex )
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
            }

            return new HttpStatusCodeResult( HttpStatusCode.OK );
        }

        /// <summary>
        /// Gets the partial view.
        /// </summary>
        /// <param name="viewname">The viewname.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [AdminActionFilter]
        public ActionResult GetPartialView(string viewname, int? id = null )
        {
            string partialViewName = String.Empty;

            switch( viewname.ToLower( CultureInfo.InvariantCulture ) )
            {
                case "users":
                    partialViewName = "~/Views/Admin/_UsersGrid.cshtml";
                    break;

                case "adduser":
                    partialViewName = "~/Views/Admin/_AddUser.cshtml";
                    break;

                case "edituser":
                    partialViewName = "~/Views/Admin/_EditUser.cshtml";

                    UserEditViewModel model = GetUserViewModel( id.Value );

                    if( model != null )
                    {
                        return PartialView( partialViewName, model );
                    }

                    return PartialView( partialViewName );

                default:
                    break;

            }

            return PartialView( partialViewName );

        }

        /// <summary>
        /// Gets the user view model.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private UserEditViewModel GetUserViewModel(int id)
        {
            UserEditViewModel model = null;

            User user = _db.Users.Find( id );

            if( user != null )
            {
                model = new UserEditViewModel();

                model.Id = user.Id;
                model.UserName = user.UserName;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.EmailAddress = user.EmailAddress;
                model.IsAdmin = user.IsAdmin;
                model.IsBlocked = user.IsVerified;
                model.IsVerified = user.IsVerified;
                model.DefaultProjectId = user.DefaultProjectId;
            }

            return model;
        }

    }
}