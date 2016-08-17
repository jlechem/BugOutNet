using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ActionResult Get( string sidx, string sord, int page, int rows )
        {
            // check if we have a sort, null means nothing
            sord = ( sord == null ) ? "" : sord;

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
            if( sord.ToUpper( CultureInfo.InvariantCulture ) == "DESC" )
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {
                    case "ID":
                        users = users.OrderByDescending( t => t.Id );
                        break;

                    case "USERNAME":
                        users = users.OrderByDescending( t => t.Username );
                        break;

                    case "NAME":
                        users = users.OrderByDescending( t => t.Name );
                        break;

                    case "EMAILADDRESS":
                        users = users.OrderByDescending( t => t.EmailAddress );
                        break;

                    case "CREATED":
                        users = users.OrderByDescending( t => t.Created );
                        break;

                    default:
                        users = users.OrderByDescending( t => t.Id );
                        break;

                }

                users = users.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                switch( sidx.ToUpper( CultureInfo.InvariantCulture ) )
                {

                    case "ID":
                        users = users.OrderBy( t => t.Id );
                        break;

                    case "USERNAME":
                        users = users.OrderBy( t => t.Username );
                        break;

                    case "NAME":
                        users = users.OrderBy( t => t.Name );
                        break;

                    case "EMAILADDRESS":
                        users = users.OrderBy( t => t.EmailAddress );
                        break;

                    case "CREATED":
                        users = users.OrderBy( t => t.Created );
                        break;

                    default:
                        users = users.OrderBy( t => t.Id );
                        break;
                }

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