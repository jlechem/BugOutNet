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

        /// <summary>
        /// Creates the salt.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string CreateSalt( int size )
        {
            //Generate a cryptographic random number.
            using( RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider() )
            {
                byte[] buff = new byte[size];
                rng.GetBytes( buff );

                // Return a Base64 string representation of the random number.
                return Convert.ToBase64String( buff );
            }
        }

    }
}