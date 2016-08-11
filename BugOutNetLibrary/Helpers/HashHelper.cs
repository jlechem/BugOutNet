using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Helpers
{
    public static class HashHelper
    {
        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static byte[] HashPassword( string password )
        {
            using( SHA512 shaM = new SHA512Managed() )
            {
                byte[] data = Encoding.UTF8.GetBytes( password );
                return shaM.ComputeHash( data );
            }
        }
    }
}