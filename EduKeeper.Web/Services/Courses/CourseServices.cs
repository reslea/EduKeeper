using AutoMapper;
using EduKeeper.Infrastructure;
using EduKeeper.Web.Models;
using EduKeeper.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduKeeper.Web.Services.Courses
{
    public class CourseServices : ICourseServices
    {
        private IDataAccess dataAccess;

        public CourseServices(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;    
        }

        public List<CourseModel> GetCourses()
        {
            var coursesFromDb = dataAccess.GetCourses();
            return Mapper.Map<List<CourseModel>>(coursesFromDb);
        }

        public CourseModel GetCourse(int id)
        {
            throw new NotImplementedException();
        }

        public void JoinCourse(int courseId)
        {
            throw new NotImplementedException();
        }

        public void LeaveCourse(int courseId)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> GetCourseParticipants(int courseId)
        {
            throw new NotImplementedException();
        }

        public void AddCourse(CourseModel model)
        {
            int ownerId = SessionWrapper.Current.User.Id;
            dataAccess.AddCourse(ownerId, model.Title, model.Description);
        }
    }
}