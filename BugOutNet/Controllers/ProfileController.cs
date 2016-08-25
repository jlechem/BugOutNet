using BugOutNet.CustomActionFilters;
using BugOutNetLibrary.Helpers;
using BugOutNetLibrary.Logging;
using BugOutNetLibrary.Managers;
using BugOutNetLibrary.Models.DB;
using BugOutNetLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugOutNet.Controllers
{
    public class ProfileController : Controller
    {
        private Entities _db = new Entities();

        [UserActionFilter]
        public ActionResult Index()
        {
            UserEditViewModel model = new UserEditViewModel
            {
                Id = SessionManager.User.Id,
                Address1 = SessionManager.User.Address,
                Address2 = SessionManager.User.Address2,
                Avatar = SessionManager.User.Avatar,
                EmailAddress = SessionManager.User.EmailAddress,
                FirstName = SessionManager.User.FirstName,
                LastName = SessionManager.User.LastName,
                UserName = SessionManager.User.UserName
            };

            return View( model );
        }

        [HttpPost]
        [UserActionFilter]
        public ActionResult Edit( UserEditViewModel model )
        {
            try
            {
                model.SaveResult = String.Empty;

                if( model != null && ModelState.IsValid)
                {
                    var user = _db.Users.Find( model.Id );

                    if( user != null )
                    {
                        bool isValid = true;
                        bool isValidImage = true;
                        bool changingPassword = false;

                        // check if they have a password change, if so they need to match
                        if( !String.IsNullOrWhiteSpace(model.Password) ||
                            !String.IsNullOrWhiteSpace(model.ConfirmPassword))
                        {
                            if( model.Password != model.ConfirmPassword)
                            {
                                ModelState.AddModelError( "e", "Passwords don't match" );
                                isValid = false;
                            }                            

                            if( !String.IsNullOrWhiteSpace(model.Password) && 
                                model.Password.Length < 7)
                            {
                                ModelState.AddModelError( "e", "Password must be at least 7 characters" );
                                isValid = false;
                            }

                            changingPassword = true;

                        }
                        
                        // check if they uploaded an avatar, if so we need to keep the file size down
                        if( model.NewAvatar != null &&
                            model.NewAvatar.InputStream != null )
                        {
                            using( var stream = model.NewAvatar.InputStream )
                            {
                                try
                                {
                                    // get the image, this will thow an exception if it's not a valid image file
                                    Image image = Image.FromStream( stream );

                                    if( image.Size.IsEmpty )
                                    {
                                        ModelState.AddModelError( "e", "Avatar image cannot be empty" );
                                        isValidImage = false;
                                    }

                                    // resize the image if needed
                                    if( image.Size.Height > 20 ||
                                        image.Size.Width > 20 )
                                    {
                                        image = ImageHelper.ResizeImage( image, 20, 20 );
                                        model.Avatar = user.Avatar = ImageHelper.ConvertImageToBytes( image );
                                    }
                                }
                                catch( ArgumentException ex )
                                {
                                    Logger.Error( ex );
                                    ModelState.AddModelError( "e", "Invalid avatar image file" );
                                    isValid = false;
                                }
                            }
                        }
                        
                        if( !isValid || !isValidImage )
                        {
                            return View( "Index", model );
                        }

                        // update password if needed
                        if( changingPassword )
                        {
                            var salt = HashHelper.CreateSalt( 10 );
                            user.Salt = salt;
                            user.Password = HashHelper.HashPassword( model.Password + salt );
                        }

                        // update the DB
                        user.Id = model.Id;
                        user.Address = model.Address1;
                        user.Address2 = model.Address2;
                        user.EmailAddress = model.EmailAddress;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;

                        _db.SaveChanges();

                        // update the session values
                        SessionManager.User = user;

                        // update the save
                        model.SaveResult = "<span style='color:greeen'>Profile updated</span>";
                    }
                }

                return View( "Index", model );
            }
            catch(Exception ex)
            {
                Logger.Error( ex );
                model.SaveResult = String.Format( "<span style='color:red'>Error updating profile:{0}</span>", ex.Message );
                return View( "Index", model );
            }
        }

    }
}