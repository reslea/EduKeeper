using AutoMapper.QueryableExtensions;
using EduKeeper.Entities;
using EduKeeper.Infrastructure.RepositoryInterfaces;
using EduKeeper.Infrastructure.DTO;
using System.Linq;

namespace EduKeeper.EntityFramework.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository 
    {
        public CommentRepository(EduKeeperContext context) : base(context) { }

        public IQueryable<CommentDTO> GetPage(int postId, int pageNumber = 1)
        {
            return DbSet
                .Where(comment => comment.Post.Id == postId)
                .OrderByDescending(comment => comment.Id)
                .ProjectTo<CommentDTO>();
        }
    }
}
