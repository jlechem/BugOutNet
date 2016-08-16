using BugOutNetLibrary.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BugOutNetLibrary.Managers;

namespace BugOutNet.Controllers
{
    public class HomeController : Controller
    {
        private Entities _db = new Entities();

        private const string BUGOUTCOOKIE = "BugOutNet";

        public ActionResult Index()
        {
            bool tokenIsValid = false;

            string token = String.Empty;

            // OK we need to check if they've got an auto login or not!!!

            // read the cookie and get the token and expiration date (we'll set this for eternity I guess)
            var cookie = Request.Cookies[BUGOUTCOOKIE];

            if( cookie != null )
            {
                // get the token and some other info
                token = cookie["token"].ToString();
                var lastChecked = cookie["lastchecked"].ToString();

                // check this token against the DB
                tokenIsValid = _db.Tokens.Count( t => t.Token1 == token ) > 0;

            }

            // if the token exists and is valid we send the user to the bugs page
            if( tokenIsValid )
            {
                // get the user info from the DB
                var user = ( from tokens in _db.Tokens
                             where tokens.Token1 == token
                             join users in _db.Users on tokens.UserId equals users.Id
                             select users ).FirstOrDefault();
                
                // if we found a user set the session with it
                if( user != null )
                {
                    SessionManager.User = user;
                    return RedirectToAction( "Index", "Bugs" );
                }
                // no user we have an issue, send an email or something and return them to the login page
                else
                {
                    return View();
                }
            }
            // otherwise we need to destroy the cookie and token and send them to the login page
            else
            {
                // destroy cookie
                Request.Cookies.Remove( BUGOUTCOOKIE );
                Request.Cookies.Clear();

                // destory token
                var dbToken = _db.Tokens.FirstOrDefault( t => t.Token1 == token );

                if( dbToken != null)
                {
                    _db.Tokens.Remove( dbToken );
                    _db.SaveChanges();
                }

                // destroy session
                SessionManager.User = null;

                return View();
            }
        }
    }
}