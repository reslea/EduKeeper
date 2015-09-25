﻿using EduKeeper.Infrastructure.DTO;
using EduKeeper.Web.Models;
using EduKeeper.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Web;

namespace EduKeeper.Web.Services
{
    public class SessionWrapper : ISessionWrapper
    {
        public int UserId { get; set; }

        private List<int> _visitedCourses;

        public List<int> VisitedCourses
        {
            get
            {
                if (_visitedCourses == null)
                    _visitedCourses = new List<int>();
                return _visitedCourses;
            }
            set { _visitedCourses = value; }
        }

        public static SessionWrapper Current
        {
            get
            {
                SessionWrapper session = (SessionWrapper)HttpContext.Current.Session["_SessionWrapper"];

                if (session == null)
                {
                    session = new SessionWrapper();
                    HttpContext.Current.Session["_SessionWrapper"] = session;

                }
                return session;
            }
        }

        public static SessionWrapper GetSessionWrapper(HttpApplication app)
        {
            return (SessionWrapper)app.Session["_SessionWrapper"];
        }
    }
}