﻿using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Helpers;
using BugOutNetLibrary.Models.DB;
using System;
using System.Collections.Generic;
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
        public ActionResult CreateUser( string textUsername, string textPassword, bool cbRemember = false )
        {
            return View();
        }

        private static string CreateSalt( int size )
        {
            //Generate a cryptographic random number.
            using( RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider() )
            {
                byte[] buff = new byte[size];
                rng.GetBytes( buff );

                // Return a Base64 string representation of the random number.
                return Convert.ToBase64String( buff );
            }
        }

        /// <summary>
        /// Creates the admin account.
        /// </summary>
        public void CreateAdminAccount()
        {
            using( var db = new Entities() )
            {
                User newUser = new User();

                newUser.UserName = "admin";
                newUser.Salt = CreateSalt( 10 );
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
                newUser.Salt = CreateSalt( 10 );
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
