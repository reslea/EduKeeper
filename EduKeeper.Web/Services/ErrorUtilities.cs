using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using EduKeeper.Infrastructure.ErrorUtilities;
using Ninject;
using System;
using System.Linq;
using System.Web;

namespace EduKeeper.Web.Services
{

    public class ErrorUtilities : IErrorUtilities
    {
        [Inject]
        public IUserContext UserContext { get; set; }

        public string GetErrorDescriptionFromAttribute(ErrorCase errorCase)
        {
            var memberInfo = typeof(ErrorCase).
                GetMember(errorCase.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var attribute = (ErrorDescriptionAttribute)memberInfo.
                    GetCustomAttributes(typeof(ErrorDescriptionAttribute), false).FirstOrDefault();

                if (attribute != null) return attribute.Text;
            }

            return null;
        }

        public string GetRedirectionPage(ErrorCase errorCase)
        {
            switch (errorCase)
            {
                case ErrorCase.UserNotFound:
                    return "/Account/Login";
                case ErrorCase.InvalidUserData:
                    return "/Account/Login";
                case ErrorCase.DuplicateEmail:
                    return "/Account/Registration";
                case ErrorCase.UnauthorizedAccess:
                    return "/Account/Login";
                case ErrorCase.CourseNotExist:
                    return "/Study/Courses";
                case ErrorCase.UndefilenError:
                    return "/Account/Login";
                default:
                    return "/Account/Login";
            }
        }

        public Error GetError(ErrorCase errorCase)
        {
            var errorAction = HttpContext.Current.Request.UrlReferrer != null ? 
                HttpContext.Current.Request.UrlReferrer.AbsolutePath : string.Empty;

            var errorDescription = GetErrorDescriptionFromAttribute(errorCase);
            var errorPageToRedirect = GetRedirectionPage(errorCase);
            
            
            return  new Error()
            {
                DateAdded = DateTime.Now,
                ErrorDescription = errorDescription,
                RedirectActionName = errorPageToRedirect,
                ErrorActionName = errorAction,
                UserId = UserContext.CurrentUserId
            };
        }

        public Error LogError(ErrorCase errorCase)
        {
            var error = GetError(errorCase);
            //DataAccess.LogError(error);
            return error;
        }
    }
}