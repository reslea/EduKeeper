using EduKeeper.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace EduKeeper.Web.Services.Interfaces
{
    public interface IUserServices
    {
        bool Registrate(UserModel model);

        UserModel SignIn(LoginModel model);

        UserModel GetAuthentificatedUser();

        void ChangePicture(HttpPostedFileBase file);

        UserModel UpdateUser(UserModel model);

        void AddAuthCookieToResponse(LoginModel model);

        int? GetUserIdFromCookie();

        void LogVisitedCourses();
    }
}