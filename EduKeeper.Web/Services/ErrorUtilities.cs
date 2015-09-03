using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using Ninject;
using System;
using System.Linq;
using System.Reflection;
using System.Web;

namespace EduKeeper.Web.Services
{

    public class ErrorUtilities : IErrorUtilities
    {
        [Inject]
        public IDataAccess DataAccess { get; set; }

        public string GetErrorDescriptionFromAttribute(ErrorCase errorCase)
        {
            MemberInfo memberInfo = typeof(ErrorCase).
                GetMember(errorCase.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                ErrorDescriptionAttribute attribute = (ErrorDescriptionAttribute)memberInfo.
                    GetCustomAttributes(typeof(ErrorDescriptionAttribute), false).FirstOrDefault();

                return attribute.Text;
            }

            return null;
        }

        public string GetRedirectionPage(ErrorCase errorCase)
        {
            switch (errorCase)
            {
                case ErrorCase.UserNotFound: 
                    return "/Registration";
                case ErrorCase.InvalidUserData:  
                    return "/Login";
                case ErrorCase.DuplicateEmail: 
                    return "/Registration";
                case ErrorCase.UnauthorizedAccess:
                    return "/Login";
                default: 
                    return "/Registration";
            }
        }

        public Error GetError(ErrorCase errorCase)
        {
            string errorAction = HttpContext.Current.Request.UrlReferrer != null ? 
                HttpContext.Current.Request.UrlReferrer.AbsolutePath.ToString() : String.Empty;

            string errorDescription = GetErrorDescriptionFromAttribute(errorCase);
            string errorPageToRedirect = GetRedirectionPage(errorCase);
            
            
            return  new Error()
            {
                ErrorDescription = errorDescription,
                RedirectActionName = errorPageToRedirect,
                ErrorActionName = errorAction,
                //UserId = (SessionWrapper.Current.User != null ? SessionWrapper.Current.User.Id : 0)
            };
        }

        public Error LogError(ErrorCase errorCase)
        {
            var error = GetError(errorCase);
            DataAccess.LogError(error);
            return error;
        }
    }
}