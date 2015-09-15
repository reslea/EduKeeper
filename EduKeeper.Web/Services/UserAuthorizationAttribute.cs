using EduKeeper.EntityFramework;
using EduKeeper.Infrastructure;
using EduKeeper.Web.Services.Interfaces;
using Ninject;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace EduKeeper.Web.Services
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
       Inherited = true, AllowMultiple = true)]
    public class UserAuthorizationAttribute : AuthorizeAttribute
    {
        [Inject]
        public IUserServices userServices { get; set; }

        [Inject]
        public IDataAccess dataAccess { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = userServices.GetUserFromCookie();

            if (SessionWrapper.Current.User == null ||
                SessionWrapper.Current.User.Id == 0)
            {
                if (user == null)
                {
                    base.OnAuthorization(filterContext);
                    return;
                }

                SessionWrapper.Current.User = user;
                SessionWrapper.Current.JoinedCourses = dataAccess.GetJoinedCourses(user.Id);
            }

            SessionWrapper.Current.JoinedCourses = dataAccess.GetJoinedCourses(user.Id);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new 
                { controller = "Account", action = "Login" }));
            }
        }
    }
}