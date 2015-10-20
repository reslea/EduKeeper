using EduKeeper.Infrastructure.DTO;
using PagedList;

namespace EduKeeper.Infrastructure.ServicesInretfaces
{
    public interface IPostService
    {
        PostDTO Add(int courseId, string message);

        PostDTO Get(int postId);

        void Remove(int postId);

        void CheckAccessToPost(int postId);

        IPagedList<PostDTO> GetLatestForCourse(int courseId, int pageNumber);

        IPagedList<PostDTO> GetLatestForJoinedCourses(int pageNumber);
    }
}
