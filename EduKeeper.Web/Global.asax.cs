using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EduKeeper.Infrastructure.ErrorUtilities;

namespace EduKeeper.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterAutoMapper();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception.Message.Contains("__browserLink"))
                return;

            if (exception.GetType() == typeof(AccessDeniedException))
            {
                // Leaving the server in an error state can cause unintended side effects as the server continues its attempts to handle the error.
                Server.ClearError();

                // It could be rendered page has already been written to response buffer before encountering error, so clear it.
                Response.Clear();
                // Access denied
                Response.StatusCode = 500;
                Response.Redirect("~/Account/Error");
                return;
            }

            System.Diagnostics.Debug.WriteLine(exception);
            Response.Redirect("/Account/Error");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            int userId = (int)Session["CurrentUserId"];

            //save last visit date
        }
    }
}