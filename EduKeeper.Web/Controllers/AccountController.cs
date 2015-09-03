using System;
using EduKeeper.Infrastructure;
using EduKeeper.Web.Models;
using EduKeeper.Web.Services;
using EduKeeper.Web.Services.Interfaces;
using System.Web.Mvc;

namespace EduKeeper.Web.Controllers
{
    [UserAuthorization] 
    public class AccountController : Controller
    {
        private IUserServices userServices;
        private IErrorUtilities errorUtilities;    
        //
        // GET: /Account/
        public AccountController(IUserServices userServices, IErrorUtilities errorUtilities)
        {
            this.userServices = userServices;
            this.errorUtilities = errorUtilities;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Registration(UserModel model)
        {
            model.Password = Security.ComputeSha256(model.Password);
            if (ModelState.IsValid)
            {
                if (userServices.RegistrateUser(model))
                {
                    SessionWrapper.Current.User = model;
                    userServices.AddAuthCookieToResponse(model);
                    return RedirectToAction("EditProfile");
                }
                else
                {
                    return RedirectToAction("Error", "Account", new { errorCase = ErrorCase.DuplicateEmail });
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = userServices.GetUser(model);

            if (user != null)
            {
                SessionWrapper.Current.User = user;
                userServices.AddAuthCookieToResponse(model);
                return RedirectToAction("EditProfile");
            }

            return RedirectToAction("Error", new { errorCase = ErrorCase.InvalidUserData });
        }

        public ActionResult EditProfile()
        {
            var user = SessionWrapper.Current.User;
            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(UserModel model)
        {
            userServices.ChangePicture(model.PictureToUpdate);
            
            var updatedUser = userServices.UpdateUser(model);
            SessionWrapper.Current.User = updatedUser;
            
            return View(updatedUser);
        }
                
        [AllowAnonymous]
        public ActionResult Error(ErrorCase errorCase = ErrorCase.UserNotFound)
        {
            var error = errorUtilities.LogError(errorCase);

            return View(error);
        }
    }
}
