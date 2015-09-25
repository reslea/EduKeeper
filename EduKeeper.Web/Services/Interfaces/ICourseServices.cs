using EduKeeper.Infrastructure.DTO;
using EduKeeper.Web.Models;
using System.Collections.Generic;
using System.Web;

namespace EduKeeper.Web.Services.Interfaces
{
    public interface ICourseServices
    {
        void AddCourse(CourseModel model);

        CourseModel GetCourse(int id);

        void JoinCourse(int courseId);

        void LeaveCourse(int courseId);

        List<UserModel> GetCourseParticipants(int courseId);

        PostDTO PostMessage(string message, int courseId, HttpFileCollectionBase files);

        CommentDTO PostComment(string message, int postId);

        LeftMenuModel GetLeftMenu();

        object AddFile(int postId, System.Web.HttpFileCollectionBase files);
    }
}