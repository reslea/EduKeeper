using AutoMapper;
using EduKeeper.Infrastructure;
using EduKeeper.Infrastructure.DTO;
using EduKeeper.Web.Models;
using EduKeeper.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Web;

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

        public PostDTO PostMessage(string message, int courseId, HttpFileCollectionBase files)
        {
            int userId = SessionWrapper.Current.UserId;
            var createdPost = dataAccess.PostMessage(message, courseId, userId);

            var savedFiles = FilesToSave(courseId, createdPost, files);

            dataAccess.AttachToPost(createdPost.Id, savedFiles);

            createdPost.Files = Mapper.Map<List<FileDTO>>(savedFiles);

            return createdPost;
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

        private List<EduKeeper.Entities.File> FilesToSave(int courseId, PostDTO createdPost, HttpFileCollectionBase files)
        {
            if (createdPost == null || files.Count == 0)
                return null;

            var result = new List<EduKeeper.Entities.File>();

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] == null) continue;
                if (files[i].ContentLength > 100 * 1024 * 1024) continue; // 100 MB

                string storageDirectory = String.Empty;
                string extention = String.Empty;
                string path = String.Empty;
                Guid identifier = Guid.NewGuid();
                string guidName = identifier.ToString();

                string courseDirectory = GetCourseDirectory(courseId);

                extention = Path.GetExtension(files[i].FileName);

                path = String.Concat(courseDirectory, guidName, extention);

                files[i].SaveAs(path);

                result.Add(new EduKeeper.Entities.File()
                {
                    Name = files[i].FileName,
                    Path = path,
                    PostId = createdPost.Id,
                    Identifier = identifier
                });
            }
            return result;
        }

        private string GetCourseDirectory(int courseId)
        {
            NameValueCollection appSettings = null;
            string storageDirectory = String.Empty;
            string courseDirectory = String.Empty;

            try
            {
                appSettings = ConfigurationManager.AppSettings;
            }
            catch (ConfigurationErrorsException) { return null; }

            if (appSettings.Count != 0)
                storageDirectory = appSettings["StorageDirectory"];

            if (!String.IsNullOrEmpty(storageDirectory))
                courseDirectory = String.Format("{0}Courses\\{1}\\", storageDirectory, courseId);

            if (!Directory.Exists(courseDirectory))
                Directory.CreateDirectory(courseDirectory);

            return courseDirectory;
        }
    }
}