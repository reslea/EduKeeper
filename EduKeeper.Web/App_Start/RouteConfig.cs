﻿using System.Web.Mvc;
using System.Web.Routing;

namespace EduKeeper.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Study", action = "Courses", id = UrlParameter.Optional }
            );
        }
    }
}