using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Helpers;
using BugOutNetLibrary.Logging;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using BugOutNetLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class AdminController : Controller
    {
        private Entities _db = new Entities();

        // GET: Admin
        [AdminActionFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AdminActionFilter]
        public ActionResult GetErrors( string sidx, string sord, int page, int rows )
        {
            sord = ( sord == null ) ? "" : sord;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            var errors = _db.Errors.Select(
                error => new ErrorGridModel
                {
                    Id = error.Id,
                    ErrorMessage = error.ErrorMessage,
                    StackTrace = error.StackTrace,
                    Created = error.Created
                } );

            int totalRecords = errors.Count();
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sord.ToUpper( CultureInfo.InvariantCulture ) == "DESC" )
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {
                    case "ID":
                        errors = errors.OrderByDescending( t => t.Id );
                        break;

                    case "ERRORMESSAGE":
                        errors = errors.OrderByDescending( t => t.ErrorMessage );
                        break;

                    case "STACKTRACE":
                        errors = errors.OrderByDescending( t => t.StackTrace );
                        break;

                    case "CREATED":
                        errors = errors.OrderByDescending( t => t.Created );
                        break;

                    default:
                        errors = errors.OrderByDescending( t => t.Id );
                        break;

                }

                errors = errors.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {

                    case "ID":
                        errors = errors.OrderBy( t => t.Id );
                        break;

                    case "ERRORMESSAGE":
                        errors = errors.OrderBy( t => t.ErrorMessage );
                        break;

                    case "STACKTRACE":
                        errors = errors.OrderBy( t => t.StackTrace );
                        break;

                    case "CREATED":
                        errors = errors.OrderBy( t => t.Created );
                        break;

                    default:
                        errors = errors.OrderBy( t => t.Id );
                        break;
                }

                errors = errors.Skip( pageIndex * pageSize ).Take( pageSize );
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = errors
            };

            return Json( jsonData, JsonRequestBehavior.AllowGet );
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [AdminActionFilter]
        public ActionResult CreateUser( string textUsername, string textPassword, bool cbRemember = false )
        {
            return View();
        }

        /// <summary>
        /// Gets the menu data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [AdminActionFilter]
        public ActionResult GetMenuData(string name)
        {
            string viewName = String.Empty;

            object model = null;

            switch( name.ToLower(CultureInfo.InvariantCulture))
            {
                case "projects":
                    viewName = "~/Views/Admin/_Projects.cshtml";
                    break;

                case "categories":
                    viewName = "~/Views/Admin/_Categories.cshtml";
                    break;

                case "statuses":
                    viewName = "~/Views/Admin/_Statuses.cshtml";
                    break;

                case "priorities":
                    viewName = "~/Views/Admin/_Priorities.cshtml";
                    break;

                case "users":
                    viewName = "~/Views/Admin/_Users.cshtml";
                    break;

                case "settings":
                    viewName = "~/Views/Admin/_Settings.cshtml";
                    model = GetUserSettingsModel();
                    break;

                case "errors":
                    viewName = "~/Views/Admin/_Errors.cshtml";
                    break;

                default:
                    viewName = "~/Views/Admin/_Projects.cshtml";
                    break;

            }

            return PartialView( viewName, model );

        }

        [HttpPost]
        [AdminActionFilter]
        public ActionResult EditSettings( SiteSettingsViewModel model)
        {
            HttpStatusCodeResult result = new HttpStatusCodeResult( HttpStatusCode.OK );

            if( model != null && ModelState.IsValid )
            {
                try
                {
                    SaveSettings( model );
                }
                catch(Exception ex)
                {
                    Logger.Error( ex );
                    result =  new HttpStatusCodeResult( HttpStatusCode.InternalServerError, ex.ToString() );
                }
            }
            else
            {
                result = new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            return result;

        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settingsModel">The settings model.</param>
        private void SaveSettings( SiteSettingsViewModel settingsModel )
        {
            var config = WebConfigurationManager.OpenWebConfiguration( "~" );

            config.AppSettings.Settings["UserName"].Value = settingsModel.UserName;
            config.AppSettings.Settings["SmtpServer"].Value = settingsModel.SmtpServer;
            config.AppSettings.Settings["Port"].Value = settingsModel.Port.ToString();
            config.AppSettings.Settings["Password"].Value = settingsModel.Password;

            config.Save( ConfigurationSaveMode.Modified );

        }

        /// <summary>
        /// Gets the user settings model.
        /// </summary>
        /// <returns></returns>
        private SiteSettingsViewModel GetUserSettingsModel()
        {
            SiteSettingsViewModel model = new SiteSettingsViewModel();

            try
            {
                model.UserName = ConfigurationManager.AppSettings["UserName"];
                model.SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                int port = -1;
                int.TryParse( ConfigurationManager.AppSettings["Port"].ToString(), out port );
                model.Port = port;
                model.Password = ConfigurationManager.AppSettings["Password"];
            }
            catch( Exception ex )
            {
                Logger.Error( ex );
            }

            return model;

        }

        /// <summary>
        /// Creates the admin account.
        /// </summary>
        [AdminActionFilter]
        public void CreateAdminAccount()
        {
            using( var db = new Entities() )
            {
                User newUser = new User();

                newUser.UserName = "admin";
                newUser.Salt = HashHelper.CreateSalt( 10 );
                newUser.Password = HashHelper.HashPassword( "password" + newUser.Salt );
                newUser.EmailAddress = "admin@admin.com";
                newUser.IsVerified = true;
                newUser.IsBlocked = false;
                newUser.AccessFailedCount = 0;
                newUser.LastLogin = DateTime.Now;
                newUser.AutoLogin = false;
                newUser.IsAdmin = true;

                db.Users.Add( newUser );

                newUser = new User();
                newUser.UserName = "jlechem";
                newUser.Salt = HashHelper.CreateSalt( 10 );
                newUser.Password = HashHelper.HashPassword( "#Icarus69" + newUser.Salt );
                newUser.EmailAddress = "jlechem@gmail.com";
                newUser.IsVerified = true;
                newUser.IsBlocked = false;
                newUser.AccessFailedCount = 0;
                newUser.LastLogin = DateTime.Now;
                newUser.AutoLogin = false;
                newUser.IsAdmin = true;

                db.Users.Add( newUser );

                db.SaveChanges();

            }
        }

    }
}