﻿using EduKeeper;
using EduKeeper.Web;
using EduKeeper.Web.Services;
using EduKeeper.Web.Services.Interfaces;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace EduKeeper
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

            System.Diagnostics.Debug.WriteLine(exception);
            Response.Redirect("/Account/Error");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            var wrapper = SessionWrapper.GetSessionWrapper(this);

            wrapper.UserId.ToString();
            //var x = SessionWrapper.Current == null ?
            //    null : SessionWrapper.Current.VisitedCourses;
        }
    }
}