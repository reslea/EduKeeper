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
    public class UserServices : IUserServices
    {
        public IDataAccess DataAccess;

        public UserServices(IDataAccess dataAccess)
        {
            this.DataAccess = dataAccess;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true if user registered false if user cannot be registered</returns>
        public bool Registrate(UserModel model)
        {
            User user = Mapper.Map<User>(model);
            return DataAccess.RegistrateUser(user);
        }

        public UserModel SignIn(LoginModel model)
        {
            model.Password = Security.ComputeSha256(model.Password);

            var userFromDb = DataAccess.AuthenticateUser(model.Email, model.Password);

            return Mapper.Map<UserModel>(userFromDb);
        }

        public UserModel GetAuthentificatedUser()
        {
            var userId = SessionWrapper.Current.UserId;
            var user = DataAccess.GetAuthenticatedUser(userId);

            return Mapper.Map<UserModel>(user);
        }

        public void ChangePicture(HttpPostedFileBase file)
        {
            if (file == null) return;
            if (file.ContentLength > 1024 * 1024) return; // 1 MB

            string strLocation = HttpContext.Current.Server.MapPath("~/UsersContent/");
            int userId = SessionWrapper.Current.UserId;

            string filePath = String.Format("{0}\\{1}.jpg", strLocation, userId);

            file.SaveAs(filePath);
        }

        public UserModel UpdateUser(UserModel model)
        {
            model.Id = SessionWrapper.Current.UserId;
            ChangePicture(model.PictureToUpdate);
            User user = Mapper.Map<User>(model);

            return Mapper.Map<UserModel>(DataAccess.UpdateUserData(user));
        }

        public void AddAuthCookieToResponse(LoginModel model)
        {
            FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
        }

        public int? GetUserIdFromCookie()
        {
            string email = HttpContext.Current.User.Identity.Name;
            if (!String.IsNullOrEmpty(email))
            {
                return DataAccess.GetAuthenticatedId(email);
            }
            return null;
        }

        public void LogVisitedCourses()
        {
            var visitedCourses = SessionWrapper.Current.VisitedCourses;
            int userId = SessionWrapper.Current.UserId;

            DataAccess.LogVisitedCourses(visitedCourses, userId);
        }
    }
}