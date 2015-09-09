using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EduKeeper.Entities;
using EduKeeper.Infrastructure;
using System.Collections.Generic;
using PagedList;
using System.Data.Entity;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace EduKeeper.EntityFramework
{
    public class DataAccess : IDataAccess
    {
        public bool RegistrateUser(User user)
        {
            using (var context = new EduKeeperContext())
            {
                user.RegDate = DateTime.Now;

                if (!context.Users.Any(u => u.Email == user.Email))
                {
                    context.Users.Add(user);
                }
                else
                {
                    return false;
                }
                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException) { return false; }
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


        public User AuthenticateUser(string email)
        {
            if (String.IsNullOrEmpty(email))
                return null;

            using (var context = new EduKeeperContext())
            {
                return context.Users
                    .SingleOrDefault(u => u.Email == email);
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

        public IPagedList<Course> GetCourses(string searchTerm, int pageNumber = 1)
        {
            using (var context = new EduKeeperContext())
            {
                if (String.IsNullOrEmpty(searchTerm))
                {
                    return context.Courses
                        .OrderBy(c => c.Id)
                        .ToPagedList(pageNumber, 10);
                }
                else
                {
                    return context.Courses
                        .Where(c => c.Description.ToLower().Contains(searchTerm.ToLower()) ||
                            c.Title.ToLower().Contains(searchTerm.ToLower()))

                            .OrderBy(c => c.Id)
                            .ToPagedList(pageNumber, 10);
                }
            }
        }

        public void AddCourse(int ownerId, string title, string description)
        {
            using (var context = new EduKeeperContext())
            {
                User user = context.Users.SingleOrDefault(u => u.Id == ownerId);

                var users = new List<User>();
                users.Add(user);

                var course = new Course()
                {
                    Owner = user,
                    Title = title,
                    Description = description,
                    Users = users
                };

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
                Course course = context.Courses.SingleOrDefault(c => c.Id == courseId);
                User user = context.Users.SingleOrDefault(u => u.Id == userId);
                course.Users.Add(user);
                context.SaveChanges();
            }
        }

        public void LeaveCourse(int courseId, int userId)
        {
            using (var context = new EduKeeperContext())
            {
                Course course = context.Courses.SingleOrDefault(c => c.Id == courseId);
                User user = context.Users.SingleOrDefault(u => u.Id == userId);
                course.Users.Remove(user);
                user.Courses.Remove(course);
                context.SaveChanges();
            }
        }


        public User GetUser(int id)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Users.Single(u => u.Id == id);
            }
        }

        public List<LabelWrapper> AutocompleteCourse(string term)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Courses
                    .Where(c => c.Title.StartsWith(term))
                    .Take(10)
                    .Select(c => new LabelWrapper
                    {
                        label = c.Title
                    }).ToList();
            }
        }

        public IPagedList<PostDTO> GetPosts(int userId, int courseId, int pageNumber = 1)
        {
            Mapper.CreateMap<Post, PostDTO>()
                .ForMember(d => d.AuthorName, opt => opt.MapFrom(p => p.Author.FirstName + " " + p.Author.LastName))
                .ForMember(d => d.AuthorId, opt => opt.MapFrom(p => p.Author.Id))
                .ForMember(d => d.CourseId, opt => opt.MapFrom(p => p.Course.Id))
                .ForMember(d => d.Message, opt => opt.MapFrom(p => p.Message))
                .ForMember(d => d.Id, opt => opt.MapFrom(p => p.Id));

            using (var context = new EduKeeperContext())
            {
                if (context.Users.SingleOrDefault(u => u.Id == userId).Courses.Any(c => c.Id == courseId))
                {
                    return context.Posts.Where(p => p.Course.Id == courseId)
                        .Include(u => u.Author)
                        .OrderByDescending(p => p.Id) //it is needed for ToPagedList()
                        .ProjectTo<PostDTO>()
                        .ToPagedList(pageNumber, 10);
                }
                else return null;
            }
        }

        public string GetCourseTitle(int courseId)
        {
            using (var context = new EduKeeperContext())
            {
                return context.Courses.Single(c => c.Id == courseId).Title;
            }
        }

        public void PostMessage(string message, int courseId, int userId)
        {
            using (var context = new EduKeeperContext())
            {
                var newPost = new Post
                {
                    Message = message,  
                    AuthorId = userId,
                    CourseId = courseId
                };
                if (context.Courses.Single(c => c.Id == courseId).Users.Any(u => u.Id == userId))
                {
                    context.Posts.Add(newPost);
                    context.SaveChanges();
                }
            }
        }
    }
}
