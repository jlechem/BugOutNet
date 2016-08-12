using BugOutNetLibrary.Managers;
using System.Web.Mvc;
using System.Web.Routing;


namespace BugOutNet.CustomActionFilters
{
    public class AdminActionFilter : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting( ActionExecutingContext filterContext )
        {
            // The action filter logic.
            if( SessionManager.User == null )
            {
                // no logged in user means a re-direct to the login page
                filterContext.Result = new RedirectToRouteResult( new RouteValueDictionary( new { action = "Index", controller = "Home" } ) );
            }
            else
            {
                // make sure user has admin role
                if( !SessionManager.User.IsAdmin)
                {
                    // no logged in user means a re-direct to the bugs page
                    filterContext.Result = new RedirectToRouteResult( new RouteValueDictionary( new { action = "Index", controller = "Bugs" } ) );
                }
            }

            base.OnActionExecuting( filterContext );

        }
    }
}