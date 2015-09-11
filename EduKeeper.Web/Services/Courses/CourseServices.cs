using AutoMapper;
using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using EduKeeper.Web.Models;
using EduKeeper.Web.Services.Interfaces;
using PagedList;
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

        public CourseModel GetCourse(int id)
        {
            var course = dataAccess.GetCourse(id);
            
            return Mapper.Map<CourseModel>(course);
        }

        public void JoinCourse(int courseId)
        {
            int userId = SessionWrapper.Current.User.Id;
            dataAccess.JoinCourse(courseId, userId);
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


        public CourseCollectionModel GetCourses(string searchTerm, int pageNumber = 1)
        {
            var courses = dataAccess.GetCourses(searchTerm, pageNumber);
            int pageCount = courses.PageCount;

            return new CourseCollectionModel
            {
                Courses = courses,
                PageCount = pageCount
            };
        }

        public PostDTO PostMessage(string message, int courseId)
        { 
            int userId = SessionWrapper.Current.User.Id;
            return dataAccess.PostMessage(message, courseId, userId);
        }
    }
}