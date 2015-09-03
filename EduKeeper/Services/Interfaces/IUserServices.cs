using EduKeeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace EduKeeper.Services.Interfaces
{
    public interface IUserServices
    {
        bool RegistrateUser(UserModel model);

        UserModel GetUser(LoginModel model);

        void ChangePicture(HttpPostedFileBase file);

        UserModel UpdateUser(UserModel model);

        void AddAuthCookieToResponse(LoginModel model);

        UserModel GetUserFromCookie();
    }
}