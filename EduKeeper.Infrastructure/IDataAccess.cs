using EduKeeper.Entities;
using System.Collections.Generic;

namespace EduKeeper.Infrastructure
{
    public interface IDataAccess
    {
        /// <summary>
        /// Register new user in DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if user is registered, false if email is duplicated</returns>
        bool RegistrateUser(User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>registered user Id or null if login data is invalid</returns>
        User AuthenticateUser(string email);

        User AuthenticateUser(string email, string password);

        User UpdateUserData(User user);

        void LogError(Error error);

        List<Course> GetCourses();

        void AddCourse(int ownerId, string title, string description);

        Course GetCourse(int courseId);

        void JoinCourse(int courseId, int userId);

        void LeaveCourse(int courseId, int userId);
    }
}