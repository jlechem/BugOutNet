using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Managers
{
    public static class SettingsManager
    {
        /// <summary>
        /// Gets the application settings value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetAppSettingsValue( string key )
        {
            var value = ConfigurationManager.AppSettings[key].ToString();

            if( String.IsNullOrWhiteSpace( value ) )
            {
                throw new ConfigurationErrorsException( "Missing key value in config file: " + key );
            }

            return value;

        }

        /// <summary>
        /// Gets the maximum login attempts.
        /// </summary>
        /// <value>
        /// The maximum login attempts.
        /// </value>
        public static int MaxLoginAttempts
        {
            get
            {
                return int.Parse( GetAppSettingsValue( "MaxLoginAttempts" ) );
            }
        }

    }
}