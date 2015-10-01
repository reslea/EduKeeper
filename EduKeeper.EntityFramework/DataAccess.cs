using System;
using System.Linq;
using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using System.Collections.Generic;
using PagedList;
using System.Data.Entity;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EduKeeper.Infrastructure.DTO;
using System.Text;

namespace EduKeeper.EntityFramework
{
    public class DataAccess : IDataAccess
    {
        //Too many methods in one class?
        //single responsibility ? 

        public bool RegistrateUser(User user)
        {
            using (var context = new EduKeeperContext())
            {
                user.RegDate = DateTime.Now;

                //check email here or just catch exception from context.SaveChanges() 
                //cause email has unique constraint?
                if (!context.Users.Any(u => u.Email == user.Email))
                {
                    context.Users.Add(user);
                }
                else
                    return false;

                context.SaveChanges();
                return true;
            }
        }

        public User AuthenticateUser(string email, string password)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Users
                        .SingleOrDefault(u => u.Email == email && u.Password == password);
            }
        }

        public int? GetAuthenticatedId(string email)
        {
            if (String.IsNullOrEmpty(email))
                return null;

            using (var context = new EduKeeperContext())
            {
                return context.Users
                    .SingleOrDefault(u => u.Email == email).Id;
            }
        }

        public User GetAuthenticatedUser(int id)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Users
                    .SingleOrDefault(u => u.Id == id);
            }
        }

        public User UpdateUserData(User user)
        {
            User result = null;

            using (var context = new EduKeeperContext())
            {
                result = context.Users.Find(user.Id);

                result.Age = user.Age;
                result.Email = user.Email;
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.Sex = user.Sex;

                context.SaveChanges();
            }

            return result;
        }

        public void LogError(Error error)
        {
            using (var context = new EduKeeperContext())
            {
                context.Errors.Add(error);
                context.SaveChanges();
            }
        }

        public IPagedList<CourseDTO> GetCourses(int userId, string searchTerm, int pageNumber = 1)
        {
            using (var context = new EduKeeperContext())
            {
                IQueryable<Course> courses;

                if (String.IsNullOrEmpty(searchTerm))
                    courses = context.Courses;

                else //if we have what to search
                {
                    courses = context.Courses
                        .Where(c => c.Description.ToLower().Contains(searchTerm.ToLower()) ||
                            c.Title.ToLower().Contains(searchTerm.ToLower()));
                }

                return courses
                    .OrderBy(c => c.Id)
                    .Select(course => new CourseDTO
                    {
                        Id = course.Id,
                        Description = course.Description,
                        Title = course.Title,
                        IsJoined = course.Users.Any(u => u.Id == userId)
                    })
                    .ToPagedList(pageNumber, 10);
            }
        }

        public void AddCourse(int ownerId, string title, string description)
        {
            using (var context = new EduKeeperContext())
            {
                User user = context.Users.Single(u => u.Id == ownerId);

                var users = new List<User>();
                users.Add(user);

                var course = context.Courses.Create();
                course.OwnerId = ownerId;
                course.Title = title;
                course.Description = description;
                course.Users = users;

                context.Courses.Add(course);
                context.SaveChanges();
            }
        }

        public Course GetCourse(int courseId)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Courses.SingleOrDefault(c => c.Id == courseId);
            }
        }

        public void JoinCourse(int courseId, int userId)
        {
            using (var context = new EduKeeperContext())
            {
                //maybe there is the way to do this in one query but I didn`n find it

                Course course = context.Courses.SingleOrDefault(c => c.Id == courseId);
                User user = context.Users.Single(u => u.Id == userId);

                if (course != null)
                {
                    course.Users.Add(user);
                    //im not sure, it needs to be tested
                    //course.Users.Add(new User() { Id = userId });
                    context.SaveChanges();
                }
            }
        }

        public void LeaveCourse(int courseId, int userId)
        {
            using (var context = new EduKeeperContext())
            {
                //maybe there is the way to do this in one query but I didn`n find it
                Course course = context.Courses.SingleOrDefault(c => c.Id == courseId);
                User user = context.Users.Single(u => u.Id == userId);

                if (course != null)
                {
                    course.Users.Remove(user);
                    context.SaveChanges();
                }
            }
        }

        public List<string> AutocompleteCourse(string term)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Courses
                    .Where(c => c.Title.StartsWith(term))
                    .Take(10)
                    .Select(c => c.Title).ToList();
            }
        }

        public IPagedList<PostDTO> GetPosts(int userId, int courseId, int pageNumber = 1)
        {
            using (var context = new EduKeeperContext())
            {
                if (!context.Users.Any(user => user.Id == userId &&
                    user.Courses.Any(course => course.Id == courseId)))
                    return null;

                var posts = context.Posts
                    .Where(post => post.Course.Id == courseId)
                    .Include(post => post.Author)
                    .OrderByDescending(post => post.Id) //it is needed for ToPagedList()
                    .ProjectTo<PostDTO>()
                    .ToPagedList(pageNumber, 10);


                IEnumerable<int> postIds = posts.Select(p => p.Id);

                if (posts.Count == 0)
                    return null;

                var comments = context.Comments
                    .Where(comment => postIds.Contains(comment.PostId))
                    .Include(comment => comment.Author)
                    .ProjectTo<CommentDTO>()
                    .GroupBy(comment => comment.PostId, c => c)
                    .Select(group => group
                        .OrderByDescending(comment => comment.Id)
                        .Take(11));



                foreach (var item in comments)
                {
                    var post = posts.Single(p => p.Id == item.First().PostId);

                    post.Comments = item.Take(10)
                        .Reverse()
                        .ToList();


                    if (item.Count() == 11)
                        post.IsHasMore = true;
                }
                return  posts;
            }
        }

        public string GetCourseTitle(int courseId)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Courses.Single(c => c.Id == courseId).Title;
            }
        }

        public PostDTO AddPost(string message, int courseId, int userId)
        {
            using (var context = new EduKeeperContext())
            {
                if (!context.Courses.Any(c => c.Id == courseId
                    && c.Users.Any(u => u.Id == userId)))
                    throw new AccessViolationException("user has no access to this course");

                var post = context.Posts.Create();

                post.Message = message;
                post.AuthorId = userId;
                post.CourseId = courseId;
                post.DateWritten = DateTime.Now;

                context.Posts.Add(post);
                context.SaveChanges();

                return Mapper.Map<PostDTO>(post);
            }
        }

        public CommentDTO PostComment(string message, int postId, int userId)
        {
            using (var context = new EduKeeperContext())
            {
                var comment = context.Comments.Create();

                comment.Message = message;
                comment.AuthorId = userId;
                comment.PostId = postId;
                comment.DateWritten = DateTime.Now;

                if (context.Posts.Any(p => p.Id == postId
                    && p.Course.Users.Any(u => u.Id == userId)))
                {
                    context.Comments.Add(comment);
                    context.SaveChanges();
                }

                return Mapper.Map<CommentDTO>(comment);
            }
        }

        public IPagedList<PostDTO> GetNews(int userId, int pageNumber)
        {
            //!!!! NOTHING CALLS THIS VOID, FEED PAGE IS ON DEVELOPING
            /*
             *  should search last posts and posts with last comments, 
             *  then group them for getting 30 new posts (actually new posts or posts with new comments) 
             */
            using (var context = new EduKeeperContext())
            {
                return context.Posts
                    .Where(post => post.Course.Users.Any(user => user.Id == userId))
                    .OrderByDescending(post => post.Id)
                    .Select(post => new PostDTO()
                    {
                        AuthorId = post.AuthorId.Value,
                        AuthorName = post.Author.FirstName + " " + post.Author.LastName,
                        CourseId = post.CourseId,
                        Id = post.Id,
                        Message = post.Message,
                        Comments = post.Comments
                        .OrderByDescending(comment => comment.Id)
                        .Select(comment => new CommentDTO()
                        {
                            AuthorId = comment.AuthorId.Value,
                            AuthorName = comment.Author.FirstName + " " + comment.Author.LastName,
                            DateWritten = comment.DateWritten,
                            Id = comment.Id,
                            Message = comment.Message,
                            PostId = comment.PostId

                        })
                        .Take(11)
                        .OrderBy(comment => comment.Id)
                        .ToList()

                    })
                    .ToPagedList(pageNumber, 30);


            }
        }

        public List<CourseDTO> GetJoinedCourses(int userId)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Users.Where(u => u.Id == userId)
                    .SelectMany(u => u.Courses)
                    .Select(c => new CourseDTO()
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        IsJoined = true
                    }).ToList();
            }
        }

        public bool IsPartisipant(int userId, int courseId)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Users
                    .Any(u => u.Id == userId &&
                        u.Courses.Any(c => c.Id == courseId));

            }
        }

        public void LogVisitedCourses(List<int> visitedCourses, int userId)
        {
            var sbCourses = new StringBuilder();
            foreach (int courseId in visitedCourses)
            {
                sbCourses.Append('#');
                sbCourses.Append(courseId);
            }

            using (var context = new EduKeeperContext())
            {
                var user = context.Users.Single(u => u.Id == userId);

                user.VisitedCourses = sbCourses.ToString();
                // 20 mins session lives after unactivity (if it`s not stored in DB)
                user.LastVisited = DateTime.Now.AddMinutes(-20);

                context.SaveChanges();
            }
        }

        public IPagedList<CommentDTO> GetComments(int userId, int postId, int pageNumber = 1)
        {
            using (var context = new EduKeeperContext())
            {
                //or maybe just get courseId with one of parameters
                if (!context.Users.Any(user => user.Id == userId &&
                    user.Courses.Any(course => course.Messages.Any(p => p.Id == postId))))
                    return null;

                return context.Comments
                        .Where(comment => comment.Post.Id == postId)
                        .OrderByDescending(comment => comment.Id)
                        .ProjectTo<CommentDTO>()
                        .ToPagedList(pageNumber, 10);
            }
        }

        public void AttachFiles(int postId, List<File> savedFiles)
        {
            using (var context = new EduKeeperContext())
            {
                foreach (var item in savedFiles)
                {
                    context.Files.Add(item);
                }

                context.SaveChanges();
            }
        }

        public FileDTO GetFile(int userId, Guid fileIdentifier)
        {
            using (var context = new EduKeeperContext())
            {
                if (!context.Users.Any(user => user.Courses
                    .Any(course => course.Messages
                        .Any(post => post.Files
                            .Any(file => file.Identifier == fileIdentifier)))))
                {
                    return null;
                }

                File loadedFile = context.Files
                    .Single(file => file.Identifier == fileIdentifier);

                return Mapper.Map<FileDTO>(loadedFile);
            }
        }
    }
}
