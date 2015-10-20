using AutoMapper;
using EduKeeper.Infrastructure.DTO;
using EduKeeper.Infrastructure.ErrorUtilities;
using EduKeeper.Infrastructure.RepositoryInterfaces;
using EduKeeper.Infrastructure.ServicesInretfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using EduKeeper.Infrastructure;

namespace EduKeeper.Services
{
    public class FileService : IFileService
    {
        protected IFileRepository Repository { get; set; }

        protected IPostService PostService { get; set; }

        private IUserContext UserContext { get; set; }

        public FileService( IFileRepository fileRepository, 
                            IUserContext userContext, 
                            IPostService postService)
        {
            Repository = fileRepository;
            UserContext = userContext;
            PostService = postService;
        }

        public FileDTO Get(int id)
        {
            var file = Repository.Get(id);

            return Mapper.Map<FileDTO>(file);
        }

        public FileDTO Get(Guid identifier)
        {
            var file = Repository.Get(identifier);

            return Mapper.Map<FileDTO>(file);
        }

        public List<FileDTO> GetFromPost(int postId)
        {
            CheckAccessToPost(postId);

            var files = Repository.GetFromPost(postId);

            return Mapper.Map<List<FileDTO>>(files);
        }

        public void Attach(int postId, int courseId, HttpFileCollectionBase files)
        {
            CheckAccessToPost(postId);

            if (files.Count == 0)
                return;

            for (var i = 0; i < files.Count; i++)
            {
                if (files[i] == null) continue;
                if (files[i].ContentLength > 29 * 1024 * 1024) continue; // 100 MB

                var identifier = Guid.NewGuid();
                var guidName = identifier.ToString();

                var courseDirectory = GetCourseDirectory(courseId);

                var extention = Path.GetExtension(files[i].FileName);

                var path = Path.Combine(courseDirectory, guidName + extention);

                files[i].SaveAs(path);

                Repository.Add(new Entities.File()
                {
                    Name = files[i].FileName.Substring(0, files[i].FileName.Length < 100 ? files[i].FileName.Length  : 100),
                    Path = path,
                    PostId = postId,
                    Identifier = identifier
                });
            }

            Repository.Save();
        }

        public void Remove(int postId, int fileId)
        {
            CheckAccessToPost(postId);

            Repository.Remove(fileId);
            Repository.Save();
        }

        public void UpdateAvatar(int userId, HttpPostedFileBase file)
        {
            if (userId != UserContext.CurrentUserId.Value)
                throw new AccessDeniedException();

            if (file == null) return;
            if (file.ContentLength > 1024 * 1024) return; // 1 MB
            if (!file.ContentType.Contains("image/")) return;

            var strLocation = HttpContext.Current.Server.MapPath("~/UsersContent/");

            var filePath = Path.Combine(strLocation, userId + ".jpg");

            file.SaveAs(filePath);
        }
        
        public void CreateAvatar(int userId)
        {
            var strLocation = HttpContext.Current.Server.MapPath("~/UsersContent/");
            
            var sourseFilePath = Path.Combine(strLocation, "0.jpg");

            var filePath = Path.Combine(strLocation, userId + ".jpg");

            File.Copy(sourseFilePath, filePath);
        }

        private void CheckAccessToPost(int postId)
        {
            PostService.CheckAccessToPost(postId);
        }

        private static string GetCourseDirectory(int courseId)
        {
            NameValueCollection appSettings;
            var storageDirectory = string.Empty;
            var courseDirectory = string.Empty;

            try
            {
                appSettings = ConfigurationManager.AppSettings;
            }
            catch (ConfigurationErrorsException) { return null; }

            if (appSettings.Count != 0)
                storageDirectory = appSettings["StorageDirectory"];

            if (!String.IsNullOrEmpty(storageDirectory))
                courseDirectory = Path.Combine(storageDirectory, "Courses", courseId.ToString());

            if (!Directory.Exists(courseDirectory))
                Directory.CreateDirectory(courseDirectory);

            return courseDirectory;
        }

    }
}
