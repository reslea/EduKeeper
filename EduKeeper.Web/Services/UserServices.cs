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
        private IDataAccess dataAccess;

        public UserServices(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        static UserServices()
        {
            Mapper.CreateMap<UserModel, User>();
            Mapper.CreateMap<User, UserModel>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true if user registered false if user cannot be registered</returns>
        public bool RegistrateUser(UserModel model)
        {
            User user = Mapper.Map<User>(model);
            return dataAccess.RegistrateUser(user);
        }

        public UserModel GetUser(LoginModel model)
        {
            model.Password = Security.ComputeSha256(model.Password);

            return GetUserModelFromDb(model);
        }

        public void ChangePicture(HttpPostedFileBase file)
        {
            if (file == null) return;

            try
            {
                string filename = Path.GetFileName(file.FileName);
                string strLocation = HttpContext.Current.Server.MapPath("~/UsersContent/");
                file.SaveAs(strLocation + @"\" + SessionWrapper.Current.User.Id + ".jpg");
            }
            catch (FormatException) { }
        }

        public UserModel UpdateUser(UserModel model)
        {
            User user = Mapper.Map<User>(model);

            return Mapper.Map<UserModel>(dataAccess.UpdateUserData(user));
        }

        public void AddAuthCookieToResponse(LoginModel model)
        {
            var authCookie = FormsAuthentication.GetAuthCookie(model.Email, model.RememberMe);
            
            if(model.RememberMe)
                authCookie.Expires = DateTime.UtcNow.AddMonths(1);
            
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public UserModel GetUserFromCookie()
        {
            UserModel result = null;
            string email = HttpContext.Current.User.Identity.Name;
            if (!String.IsNullOrEmpty(email))
            {
                return GetUserModelFromDb(email);
            }
            return result;
        }

        public UserModel GetUserModelFromDb(LoginModel model)
        {
            var userFromDb = dataAccess.AuthenticateUser(model.Email, model.Password);

            return Mapper.Map<UserModel>(userFromDb);
        }

        public UserModel GetUserModelFromDb(string email)
        {
            var userFromDb = dataAccess.AuthenticateUser(email);

            return Mapper.Map<UserModel>(userFromDb);
        }
    }
}