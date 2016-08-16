using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Caching
{
    public static class CacheManager
    {
        #region Fields

        private static ObjectCache s_Cache;
        private static CacheItemPolicy s_Policy;

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
        public static void Add( string key, object value )
        {
            s_Cache.Add( key, value, s_Policy );
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void Remove( string key )
        {
            s_Cache.Remove( key );
        }

        /// <summary>
        /// Determines whether [contains] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            return s_Cache.Contains( key );
        }

    }
}