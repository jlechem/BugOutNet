using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.GridModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Repos
{
    public static class BugRepo
    {
        /// <summary>
        /// Gets the bugs.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public static IEnumerable<BugGridModel> GetBugs(int projectId )
        {
            IEnumerable<BugGridModel> bugs;

            using( var db = new Entities() )
            {
                // 0 means all projects
                if( projectId <= 0 )
                {
                    bugs = db.Bugs.Where( bug => bug.AssignedToId == SessionManager.User.Id ).Select( bug =>
                    new BugGridModel
                    {
                        Id = bug.Id,
                        AssignedTo = bug.User.UserName,
                        Category = bug.Category.Name,
                        Description = bug.Description,
                        Name = bug.Name,
                        Project = bug.Project.Name,
                        Priority = bug.Priority.Name,
                        Status = bug.Status.Name,
                        LastUpdatedOn = bug.LatUpdated.Value.ToShortDateString()
                    } );
                }
                // > 0 means a specific project
                else
                {
                    bugs = db.Bugs.Where( bug => bug.AssignedToId == SessionManager.User.Id &&
                                    bug.ProjectId == projectId).Select( bug =>
                   new BugGridModel
                   {
                       Id = bug.Id,
                       AssignedTo = bug.User.UserName,
                       Category = bug.Category.Name,
                       Description = bug.Description,
                       Name = bug.Name,
                       Project = bug.Project.Name,
                       Priority = bug.Priority.Name,
                       Status = bug.Status.Name,
                       LastUpdatedOn = bug.LatUpdated.Value.ToShortDateString()
                   } );
                }

                return bugs;

            }
        }
    }
}
