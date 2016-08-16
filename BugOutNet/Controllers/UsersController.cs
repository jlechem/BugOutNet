using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class UsersController: Controller
    {
        Entities _db = new Entities();

        [AdminActionFilter]
        public ActionResult Get( string sidx, string sort, int page, int rows )
        {
            // check if we have a sort, null means nothing
            sort = ( sort == null ) ? "" : sort;

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
            if( sort.ToUpper() == "DESC" )
            {
                // you need to look at the sidx (sorting index) this will be a column name
                // use the correct item in your source to sort the column correctly
                users = users.OrderByDescending( t => t.Id );
                users = users.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                // you need to look at the sidx (sorting index) this will be a column name
                // use the correct item in your source to sort the column correctly
                users = users.OrderBy( t => t.Id );
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
    }
}