using BugOutNetLibrary.Enums;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models;
using BugOutNetLibrary.Models.DB;
using System;
using System.Threading.Tasks;

namespace BugOutNetLibrary.Logging
{
    public static class Logger
    {
        public static void Debug(string message)
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = message,
                ErrorStatus = ErrorStatus.Debug
            } );
        }

        public static void Info( string message )
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = message,
                ErrorStatus = ErrorStatus.Info
            } );
        }

        public static void Warn( string message )
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = message,
                ErrorStatus = ErrorStatus.Warn
            } );
        }

        public static void Error( string message )
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = message,
                ErrorStatus = ErrorStatus.Error
            } );
        }

        public static void Error(string message, Exception exception )
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = message,
                ErrorStatus = ErrorStatus.Error,
                StackTrace = exception.ToString()
            } );
        }

        public static void Error( Exception exception )
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = exception.Message,
                ErrorStatus = ErrorStatus.Error,
                StackTrace = exception.ToString()
            } );
        }

        public static void Fatal( string message )
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = message,
                ErrorStatus = ErrorStatus.Fatal
            } );
        }

        public static void Fatal( string message, Exception exception )
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = message,
                ErrorStatus = ErrorStatus.Fatal,
                StackTrace = exception.ToString()
            } );
        }

        public static void Fatal( Exception exception )
        {
            Log( new ExceptionModel
            {
                Created = DateTime.Now,
                ErrorMessage = exception.Message,
                ErrorStatus = ErrorStatus.Fatal,
                StackTrace = exception.ToString()
            } );
        }

        /// <summary>
        /// Logs the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        private static void Log( ExceptionModel model )
        {
            Task.Run( () =>
            {
                using( var db = new Entities() )
                {
                    db.Errors.Add( new Error
                    {
                        ErrorMessage = model.ErrorMessage,
                        StackTrace = model.StackTrace,
                        ErrorStatusId = (int)model.ErrorStatus,
                        Created = model.Created
                    } );

                    db.SaveChanges();

                }
            } );
        }

    }
}