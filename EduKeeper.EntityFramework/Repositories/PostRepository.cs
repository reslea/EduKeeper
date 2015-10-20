using EduKeeper.Entities;
using EduKeeper.Infrastructure.DTO;
using EduKeeper.Infrastructure.RepositoryInterfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EduKeeper.EntityFramework.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(EduKeeperContext context)
            : base(context) { }
        
        #region we-need-to-go-deeper
        private readonly Expression<Func<Post, PostDTO>> _exprWithCommentsAndFiles =
                post => new PostDTO()
                {
                    AuthorId = post.AuthorId,
                    AuthorName = post.Author.FirstName + " " + post.Author.LastName,
                    CourseId = post.CourseId,
                    Id = post.Id,
                    Message = post.Message,
                    Comments = post.Comments
                        .OrderByDescending(comment => comment.Id)
                        .Select( comment => new CommentDTO()
                            {
                                AuthorId = comment.AuthorId.Value,
                                AuthorName = comment.Author.FirstName + " " + comment.Author.LastName,
                                DateWritten = comment.DateWritten,
                                Id = comment.Id,
                                Message = comment.Message,
                                PostId = comment.PostId
                            })                           

                        .Take(11) //We need 10 commants to show and one to check if we have next page
                        .OrderBy(comment => comment.Id)
                        .ToList(), // queries?

                    Files = post.Files
                        .OrderByDescending(file => file.Id)
                        .Select(file => new FileDTO()
                        {
                            Identifier = file.Identifier,
                            Name = file.Name,
                            Path = file.Path,
                            PostId = file.PostId

                        }).ToList() // queries?
                };
        #endregion

        public IQueryable<PostDTO> GetLatestForCourse(int courseId)
        {
            return DbSet
                .Where(post => post.Course.Id == courseId)
                .Include(post => post.Author)
                .OrderByDescending(post => post.Id)
                .Select(_exprWithCommentsAndFiles);
        }

        public IQueryable<PostDTO> GetLatestForJoinedCourses(int userId)
        {
            return DbSet
                .Where(post => post.Course.Users.Any(user => user.Id == userId))
                .OrderByDescending(post => post.Id)
                .Select(_exprWithCommentsAndFiles);
        }
    }
}
