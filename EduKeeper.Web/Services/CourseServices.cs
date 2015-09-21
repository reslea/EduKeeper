﻿using AutoMapper;
using EduKeeper.Infrastructure;
using EduKeeper.Infrastructure.DTO;
using EduKeeper.Web.Models;
using EduKeeper.Web.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace EduKeeper.Web.Services
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
            int userId = SessionWrapper.Current.UserId;
            dataAccess.JoinCourse(courseId, userId);
        }

        public void LeaveCourse(int courseId)
        {
            int userId = SessionWrapper.Current.UserId;
            dataAccess.LeaveCourse(courseId, userId);
        }

        public List<UserModel> GetCourseParticipants(int courseId)
        {
            throw new NotImplementedException();
        }

        public void AddCourse(CourseModel model)
        {
            int ownerId = SessionWrapper.Current.UserId;
            dataAccess.AddCourse(ownerId, model.Title, model.Description);
        }

        public PostDTO PostMessage(string message, int courseId)
        { 
            int userId = SessionWrapper.Current.UserId;
            return dataAccess.PostMessage(message, courseId, userId);
        }

        public CommentDTO PostComment(string message, int postId)
        {
            int userId = SessionWrapper.Current.UserId;
            return dataAccess.PostComment(message, postId, userId);
        }

        public LeftMenuModel GetLeftMenu()
        {
            var userId = SessionWrapper.Current.UserId;
            var joinedCourses = dataAccess.GetJoinedCourses(userId);
            var user = dataAccess.GetAuthenticatedUser(userId);

            return new LeftMenuModel() 
            { 
                User = Mapper.Map<UserModel>(user), 
                Courses = joinedCourses
            };

        }
    }
}