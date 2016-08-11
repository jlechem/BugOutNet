using BugOutNetLibrary.Helpers;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Performs the login.
        /// </summary>
        /// <param name="textUsername">The text username.</param>
        /// <param name="textPassword">The text password.</param>
        /// <param name="cbRemember">if set to <c>true</c> [cb remember].</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PerformLogin( string textUsername, string textPassword, bool cbRemember = false )
        {
            try
            {
                // see if our passed in information matches our records
                using( var db = new Entities() )
                {
                    // check if we have a user by this name
                    var validUser = db.Users.FirstOrDefault( user => user.UserName == textUsername );

                    // if we do we need to do some password validation
                    if( validUser != null )
                    {
                        var hashedPassword = HashHelper.HashPassword( textPassword + validUser.Salt );

                        var isValid = hashedPassword.Length == validUser.Password.Length && !hashedPassword.Where( ( t, i ) => t != validUser.Password[i] ).Any();

                        if( isValid )
                        {
                            validUser.AccessFailedCount = 0;
                            validUser.LastLogin = DateTime.Now;
                            validUser.AutoLogin = cbRemember;
                            db.SaveChanges();

                            SessionManager.User = validUser;

                            return Json( new { Success = true }, JsonRequestBehavior.AllowGet );
                        }
                        else
                        {
                            validUser.AccessFailedCount++;

                            if( validUser.AccessFailedCount > SettingsManager.MaxLoginAttempts )
                            {
                                validUser.IsBlocked = true;
                            }

                            validUser.LastLogin = DateTime.Now;
                            db.SaveChanges();

                            return Json( new { Success = false }, JsonRequestBehavior.AllowGet );

                        }
                    }
                    else
                    {
                        return Json( new { Success = false }, JsonRequestBehavior.AllowGet );
                    }
                }
            }
            catch( Exception ex )
            {
                return Json( new { Success = false, ErrorMessage = ex.ToString() }, JsonRequestBehavior.AllowGet );
            }
        }

        /// <summary>
        /// Logoffs this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logoff()
        {
            // clear the session, remove the cookie, and send them to the logon page
            SessionManager.User = null;
            return RedirectToAction( "Index", "Home" );
        }

        /// <summary>
        /// Reads the logon cookie.
        /// </summary>
        private void ReadLogonCookie()
        {

        }

        /// <summary>
        /// Writes the logon cookie.
        /// </summary>
        private void WriteLogonCookie()
        {

        }

    }
}