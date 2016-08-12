using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using BugOutNetLibrary.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class ProjectsController : Controller
    {
        #region Fields

        private Entities _db = new Entities();

        #endregion

        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <returns></returns>
        [AdminActionFilter]
        public ActionResult GetProjects( string sidx, string sort, int page, int rows )
        {
            sort = ( sort == null ) ? "" : sort;
            int pageIndex = Convert.ToInt32( page ) - 1;
            int pageSize = rows;

            var projects = _db.Projects.Select(
                project => new ProjectGridModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Created = project.Created,
                    CreatedBy = _db.Users.FirstOrDefault( user => user.Id == project.CreatorId ).UserName
                } );

            int totalRecords = projects.Count();
            var totalPages = (int)Math.Ceiling( (float)totalRecords / (float)rows );

            if( sort.ToUpper() == "DESC" )
            {
                projects = projects.OrderByDescending( t => t.Id );
                projects = projects.Skip( pageIndex * pageSize ).Take( pageSize );
            }
            else
            {
                projects = projects.OrderBy( t => t.Id );
                projects = projects.Skip( pageIndex * pageSize ).Take( pageSize );
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = projects
            };

            return Json( projects, JsonRequestBehavior.AllowGet );

        }

        public ActionResult Edit(ProjectGridModel model)
        {
            if( model != null && ModelState.IsValid )
            {

            }

            return View();

        }

        [HttpPost]
        public ActionResult Create( [Bind( Exclude = "Id" )] ProjectGridModel model )
        {
            if( model != null && ModelState.IsValid )
            {

            }

            return View();
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Delete(int Id )
        {
            return View();
        }

    }
}