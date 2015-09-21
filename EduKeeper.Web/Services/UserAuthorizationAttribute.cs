using EduKeeper.EntityFramework;
using EduKeeper.Infrastructure;
using EduKeeper.Web.Services.Interfaces;
using Ninject;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EduKeeper.Web.Services
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
       Inherited = true, AllowMultiple = true)]
    public class UserAuthorizationAttribute : AuthorizeAttribute
    {
        [Inject]
        public IUserServices UserServices { get; set; }

        [Inject]
        public IDataAccess DataAccess { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionWrapper.Current.UserId == 0)
            {
                var userId = UserServices.GetUserIdFromCookie();
                
                if (userId.HasValue)
                    SessionWrapper.Current.UserId = userId.Value;
            }
            base.OnAuthorization(filterContext);
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