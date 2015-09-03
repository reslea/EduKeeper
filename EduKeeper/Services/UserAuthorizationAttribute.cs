using EduKeeper.Infrastructure;
using EduKeeper.Services.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EduKeeper.Services
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
       Inherited = true, AllowMultiple = true)]
    public class UserAuthorizationAttribute : AuthorizeAttribute
    {
        [Inject]
        public IUserServices userServices { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionWrapper.Current.User == null || SessionWrapper.Current.User.Id == 0)
            {
                var user = userServices.GetUserFromCookie();
                if (user != null)
                    SessionWrapper.Current.User = user;
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