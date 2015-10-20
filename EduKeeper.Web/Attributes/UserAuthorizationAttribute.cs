using EduKeeper.Infrastructure;
using Ninject;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EduKeeper.Web.Attributes
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class UserAuthorizationAttribute : AuthorizeAttribute
    {
        [Inject]
        public IUserContext UserContext { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!UserContext.CurrentUserId.HasValue || UserContext.CurrentUserId.Value == 0)
            {
                int userId;

                if (Int32.TryParse(HttpContext.Current.User.Identity.Name, out userId))
                    UserContext.CurrentUserId = userId;
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