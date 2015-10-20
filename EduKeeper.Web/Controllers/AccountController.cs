using AutoMapper;
using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using EduKeeper.Infrastructure.ErrorUtilities;
using EduKeeper.Infrastructure.ServicesInretfaces;
using EduKeeper.Web.Attributes;
using EduKeeper.Web.Models;
using System.Web.Mvc;

namespace EduKeeper.Web.Controllers
{
    [UserAuthorization] 
    public class AccountController : Controller
    {
        protected IErrorUtilities ErrorUtilities { get; set; }

        protected IUserContext UserContext { get; set; }   
     
        protected IUserService UserService { get; set; }

        protected IFileService FileService { get; set; }

        public AccountController(   IErrorUtilities errorUtilities,
                                    IUserContext userContext,
                                    IUserService userService,
                                    IFileService fileService)
        {
            ErrorUtilities = errorUtilities;
            UserContext = userContext;
            UserService = userService;
            FileService = fileService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Registration(string email = null)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registration(UserModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var convertedUser = Mapper.Map<User>(model);

            if (!UserService.RegistrateUser(convertedUser))
                return RedirectToAction("Error", "Account", new { errorCase = ErrorCase.DuplicateEmail });

            UserContext.CurrentUserId = convertedUser.Id;
            
            return RedirectToAction("Courses", "Study");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = UserService.Authentificate(model.Email, model.Password);

            if (user == null)
                return RedirectToAction("Error", new { errorCase = ErrorCase.InvalidUserData });

            return RedirectToAction("Courses", "Study");
        }

        public ActionResult EditProfile()
        {
            int userId = UserContext.CurrentUserId.Value;
            var user = UserService.GetAuthentificated(userId);

            var userModel = Mapper.Map<UpdateUserModel>(user);
            
            return View(userModel);
        }

        [HttpPost]
        public ActionResult EditProfile(UpdateUserModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var convertedUser = Mapper.Map<User>(model);

            var  updatedUser = UserService.Update(convertedUser);

            if (model.PictureToUpdate != null)
                FileService.UpdateAvatar(model.Id, model.PictureToUpdate);

            var updatedUserModel = Mapper.Map<UpdateUserModel>(updatedUser);

            return View(updatedUserModel);
        }
                
        [AllowAnonymous]
        public ActionResult Error(ErrorCase errorCase = ErrorCase.UserNotFound)
        {
            var error = ErrorUtilities.LogError(errorCase);

            Response.StatusCode = 500;
            return View(error);
        }
    }
}
