using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Helpers;
using BugOutNetLibrary.Models.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [AdminActionFilter]
        public ActionResult Index()
        {
            return View();
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
                    viewName = "~/Views/Admin/_Users.chstml";
                    break;

                case "settings":
                    viewName = "~/Views/Admin/_Settings.cshtml";
                    break;

                default:
                    viewName = "~/Views/Admin/_Projects.cshtml";
                    break;

            }

            return PartialView( viewName );

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