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
        /// <returns></returns>
        public static IQueryable<Bug> GetBugs()
        {
            using( var db = new Entities() )
            {
                return db.Bugs;
            }
        }
    }
}
