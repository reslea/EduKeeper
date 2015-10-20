using EduKeeper.Entities;
using EduKeeper.Infrastructure.DTO;
using System.Linq;

namespace EduKeeper.Infrastructure.RepositoryInterfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IQueryable<CommentDTO> GetPage(int postId, int pageNumber = 1);
    }
}
