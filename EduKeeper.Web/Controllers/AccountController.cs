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
        public IUserServices UserServices;
        public IErrorUtilities ErrorUtilities;    
        //
        // GET: /Account/
        public AccountController(IUserServices userServices, IErrorUtilities errorUtilities)
        {
            this.UserServices = userServices;
            this.ErrorUtilities = errorUtilities;
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
                if (UserServices.Registrate(model))
                {
                    SessionWrapper.Current.UserId = model.Id;
                    UserServices.AddAuthCookieToResponse(model);
                    return RedirectToAction("Courses", "Study");
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

            var user = UserServices.SignIn(model);

            if (user != null)
            {
                SessionWrapper.Current.UserId = user.Id;
                UserServices.AddAuthCookieToResponse(model);
                return RedirectToAction("Courses", "Study");
            }

            return RedirectToAction("Error", new { errorCase = ErrorCase.InvalidUserData });
        }

        public ActionResult EditProfile()
        {
            var user = UserServices.GetAuthentificatedUser();
            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(UserModel model)
        {
            var updatedUser = UserServices.UpdateUser(model);
            SessionWrapper.Current.UserId = updatedUser.Id;
            
            return View(updatedUser);
        }
                
        [AllowAnonymous]
        public ActionResult Error(ErrorCase errorCase = ErrorCase.UserNotFound)
        {
            var error = ErrorUtilities.LogError(errorCase);

            return View(error);
        }
    }
}
