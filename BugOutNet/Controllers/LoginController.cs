using BugOutNetLibrary.Constants;
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
        private Entities _db = new Entities();

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
        //[ValidateAntiForgeryToken]
        public ActionResult PerformLogin( string textUsername, string textPassword, bool cbRemember = false )
        {
            try
            {
                // see if our passed in information matches our records
                // check if we have a user by this name
                var validUser = _db.Users.FirstOrDefault( user => user.UserName == textUsername );

                // if we do we need to do some password validation
                if( validUser != null )
                {
                    var hashedPassword = HashHelper.HashPassword( textPassword + validUser.Salt );

                    // compare binary passwords
                    var isValid = hashedPassword.Length == validUser.Password.Length && !hashedPassword.Where( ( t, i ) => t != validUser.Password[i] ).Any();

                    if( isValid )
                    {
                        // if the user has selected remember me we need to set the cookie
                        if( cbRemember )
                        {
                            this.WriteLogonCookie( validUser.Id );
                        }

                        validUser.AccessFailedCount = 0;
                        validUser.LastLogin = DateTime.Now;
                        validUser.AutoLogin = cbRemember;
                        _db.SaveChanges();

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
                        _db.SaveChanges();

                        return Json( new { Success = false }, JsonRequestBehavior.AllowGet );

                    }
                }
                else
                {
                    return Json( new { Success = false }, JsonRequestBehavior.AllowGet );
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
            var cookie = Request.Cookies[Constants.BugOutCookeName];

            if( cookie != null)
            {
                // expire the cookie and overwrite it
                cookie.Expires = DateTime.Now.AddDays( -1 );
                Response.Cookies.Add( cookie );
            }

            SessionManager.User = null;
            return RedirectToAction( "Index", "Home" );
        }

        /// <summary>
        /// Adds the token.
        /// </summary>
        /// <param name="created">The created.</param>
        /// <param name="expired">if set to <c>true</c> [expired].</param>
        /// <param name="lastChecked">The last checked.</param>
        /// <param name="token">The token.</param>
        /// <param name="id">The identifier.</param>
        private void AddToken(DateTime created, bool expired, DateTime lastChecked, string token, int id)
        {
            _db.Tokens.Add( new Token
            {
                Created = created,
                Expired = expired,
                LastChecked = lastChecked,
                Token1 = token,
                UserId = id
            } );

            _db.SaveChanges();
        }

        /// <summary>
        /// Writes the cookie.
        /// </summary>
        /// <param name="cookie">The cookie.</param>
        /// <param name="token">The token.</param>
        /// <param name="lastChecked">The last checked.</param>
        /// <param name="expires">The expires.</param>
        private void WriteCookie(HttpCookie cookie, string token, string lastChecked, DateTime expires)
        {
            // create a new cookie if needed
            if( cookie == null )
            {
                cookie = new HttpCookie( Constants.BugOutCookeName );
            }
            
            // set our values
            cookie.Values["token"] = token;
            cookie.Values["lastchecked"] = lastChecked;
            cookie.Expires = expires;

            // if it's not there add it, otherwise update it
            if( Request.Cookies[Constants.BugOutCookeName] == null )
            {
                Response.Cookies.Add( cookie );
            }
            else
            {
                Response.SetCookie( cookie );
            }            
        }

        /// <summary>
        /// Writes the logon cookie.
        /// </summary>
        private void WriteLogonCookie( int id )
        {
            var cookie = Request.Cookies[Constants.BugOutCookeName];
            var guid = Guid.NewGuid().ToString();
            var now = DateTime.Now;

            // if we don't have a cookie create a DB record and write the cookie
            if( cookie == null )
            {
                // add the token to the DB
                AddToken( now, false, now, guid, id );

                // write the cookie
                WriteCookie( cookie, guid, now.ToString("yyyy-MM-dd"), now.AddMonths( 3 ) );
            }
            // we have an existing cookie so check the token
            else
            {
                // get the existing token
                var oldGuid = cookie.Values["token"].ToString();

                // then check if we have this token in the DB
                var token = _db.Tokens.FirstOrDefault( t => t.Token1 == oldGuid );

                // we don't have this token in the DB
                if( token == null )
                {
                    // add the token to the DB
                    AddToken( now, false, now, oldGuid, id );

                    // write the cookie to match the DB token
                    WriteCookie( cookie, oldGuid, now.ToString( "yyyy-MM-dd" ), now.AddMonths( 3 ) );
                }
            }
        }

    }
}