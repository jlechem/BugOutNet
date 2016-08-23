using BugOutNetLibrary.Models.DB;
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

        /// <summary>
        /// Gets or sets the selected project identifier.
        /// </summary>
        /// <value>
        /// The selected project identifier.
        /// </value>
        public static int SelectedProjectId
        {
            get
            {
                if( HttpContext.Current.Session["SelectedProjectId"] == null )
                {
                    HttpContext.Current.Session["SelectedProjectId"] = 0;
                }

                return (int)HttpContext.Current.Session["SelectedProjectId"];
            }
            set
            {
                HttpContext.Current.Session["SelectedProjectId"] = value;
            }
        }

    }
}