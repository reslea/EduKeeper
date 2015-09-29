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

            if (!ModelState.IsValid)
                return View();

            if (!userServices.Registrate(model))                
                return RedirectToAction("Error", "Account", new { errorCase = ErrorCase.DuplicateEmail });
                
            SessionWrapper.Current.UserId = model.Id;
            userServices.AddAuthCookieToResponse(model);
            return RedirectToAction("Courses", "Study");
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

            var user = userServices.SignIn(model);

            if (user == null)
                return RedirectToAction("Error", new { errorCase = ErrorCase.InvalidUserData });

            SessionWrapper.Current.UserId = user.Id;
            userServices.AddAuthCookieToResponse(model);
            return RedirectToAction("Courses", "Study");
        }

        public ActionResult EditProfile()
        {
            var user = userServices.GetAuthentificatedUser();
            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(UserModel model)
        {
            var updatedUser = userServices.UpdateUser(model);
            SessionWrapper.Current.UserId = updatedUser.Id;
            
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
