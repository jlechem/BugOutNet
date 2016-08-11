﻿using BugOutNetLibrary.Models.DB;
using System.Web;

namespace BugOutNetLibrary.Managers
{
    public static class SessionManager
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public static User User
        {
            get
            {
                return HttpContext.Current.Session["User"] == null ? null : (User)HttpContext.Current.Session["User"];
            }
            set
            {
                HttpContext.Current.Session["User"] = value;
            }
        }

    }
}