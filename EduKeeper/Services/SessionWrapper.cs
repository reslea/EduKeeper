using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EduKeeper.Models;
using EduKeeper.Services.Interfaces;
using EduKeeper.Services;

namespace EduKeeper.Services
{
    public class SessionWrapper : ISessionWrapper
    {
        public UserModel User { get; set; }

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
    }
}