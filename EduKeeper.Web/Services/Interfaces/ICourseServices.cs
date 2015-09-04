using EduKeeper.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduKeeper.Web.Services.Interfaces
{
    public interface ICourseServices
    {
        void AddCourse(CourseModel model);

        CourseCollectionModel GetCourses(string searchTerm, int pageNumber = 1);

        CourseModel GetCourse(int id);

        void JoinCourse(int courseId);

        void LeaveCourse(int courseId);

        List<UserModel> GetCourseParticipants(int courseId);
    }
}