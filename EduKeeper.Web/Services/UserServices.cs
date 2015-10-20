using AutoMapper;
using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using EduKeeper.Web.Models;
using EduKeeper.Web.Services;
using EduKeeper.Web.Services.Interfaces;
using System;
using System.IO;
using System.Web;
using System.Web.Security;

namespace EduKeeper.Web.Services
{
    public class UserServices
    {
        public void ChangePicture(HttpPostedFileBase file)
        {
            if (file == null) return;
            if (file.ContentLength > 1024 * 1024) return; // 1 MB

            string strLocation = HttpContext.Current.Server.MapPath("~/UsersContent/");
            int userId = 0;

            string filePath = String.Format("{0}\\{1}.jpg", strLocation, userId);

            file.SaveAs(filePath);
        }

        public UserModel UpdateUser(UserModel model)
        {
            model.Id = 0;
            ChangePicture(model.PictureToUpdate);
            User user = Mapper.Map<User>(model);

            return null;
            //return Mapper.Map<UserModel>(DataAccess.UpdateUserData(user));
        }

        public void AddAuthCookieToResponse(LoginModel model)
        {
            FormsAuthentication.SetAuthCookie(model.Id.ToString(), model.RememberMe);
        }
    }
}