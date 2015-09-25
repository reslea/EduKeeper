using EduKeeper.Entities;
using System.Collections.Generic;
using PagedList;
using System.Linq;
using EduKeeper.Infrastructure.DTO;

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

        //User GetUser(int id);
        
        int? GetAuthenticatedId(string email);

        User AuthenticateUser(string email, string password);

        User UpdateUserData(User user);

        User GetAuthenticatedUser(int id);

        void LogError(Error error);

        IPagedList<CourseDTO> GetCourses(int userId, string searchTerm, int pageNumber = 1);

        void AddCourse(int ownerId, string title, string description);

        Course GetCourse(int courseId);

        void JoinCourse(int courseId, int userId);

        void LeaveCourse(int courseId, int userId);

        List<LabelWrapper> AutocompleteCourse(string term);

        IPagedList<PostDTO> GetPosts(int userId, int courseId, int pageNumber = 1);

        string GetCourseTitle(int courseId);

        PostDTO PostMessage(string message, int courseId, int userId);

        CommentDTO PostComment(string message, int postId, int userId);

        List<CourseDTO> GetJoinedCourses(int userId);

        bool IsPartisipant(int userId, int courseId);

        void LogVisitedCourses(List<int> visitedCourses, int userId);

        IPagedList<CommentDTO> GetComments(int userId, int postId, int pageNumber = 1);

        IPagedList<PostDTO> GetNews(int userId, int pageNumber);

        void AttachToPost(int postId, List<Document> savedFiles);
    }
}