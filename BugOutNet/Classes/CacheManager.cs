using BugOutNetLibrary.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Classes
{
    public static class CacheManager
    {
        #region Fields

        private static ObjectCache s_Cache;
        private static CacheItemPolicy s_Policy;

        private static string ProjectsCacheKey = "Project-All";
        private static string CagtegoriesCacheKey = "Catergory-All";
        private static string StatusesCacheKey = "Status-All";
        private static string PrioritiesCacheKey = "Priority-All";

        #endregion

        /// <summary>
        /// Initializes the <see cref="CacheManager"/> class.
        /// </summary>
        static CacheManager()
        {
            s_Cache = MemoryCache.Default;
            s_Policy = new CacheItemPolicy();
            s_Policy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private static void Add( string key, object value )
        {
            s_Cache.Add( key, value, s_Policy );
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        private static void Remove( string key )
        {
            s_Cache.Remove( key );
        }

        /// <summary>
        /// Determines whether [contains] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static bool Contains( string key )
        {
            return s_Cache.Contains( key );
        }

        /// <summary>
        /// Gets the projects.
        /// </summary>
        public static void GetProjects()
        {
            using( var db = new Entities() )
            {
                var projects = db.Projects.Select( project =>
                new SelectListItem
                {
                    Text = project.Name,
                    Value = project.Id.ToString()
                } ).ToList();
            }
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public static void GetCategories()
        {
            using( var db = new Entities() )
            {
                var projects = db.Categories.Select( category =>
                new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                } ).ToList();
            }
        }

        /// <summary>
        /// Gets the statuses.
        /// </summary>
        public static void GetStatuses()
        {
            using( var db = new Entities() )
            {
                var projects = db.Statuses.Select( status =>
                new SelectListItem
                {
                    Text = status.Name,
                    Value = status.Id.ToString()
                } ).ToList();
            }
        }

        /// <summary>
        /// Gets the priorities.
        /// </summary>
        public static void GetPriorities()
        {
            using( var db = new Entities() )
            {
                var projects = db.Priorities.Select( priority =>
                new SelectListItem
                {
                    Text = priority.Name,
                    Value = priority.Id.ToString()
                } ).ToList();
            }
        }


    }
}