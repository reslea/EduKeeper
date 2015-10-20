using EduKeeper.Entities;
using EduKeeper.Infrastructure.DTO;
using EduKeeper.Infrastructure.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EduKeeper.EntityFramework.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(EduKeeperContext context) : base(context) { }

        public IQueryable<CourseDTO> GetAll(int userId, string searchTerm)
        {
            IQueryable<Course> courses;

            if (String.IsNullOrEmpty(searchTerm))
                courses = DbSet;
            else
                courses = DbSet.Where(c => 
                    c.Description.ToLower().Contains(searchTerm.ToLower()) ||
                    c.Title.ToLower().Contains(searchTerm.ToLower()));


            return courses.OrderBy(c => c.Id)
                    .Select(course => new CourseDTO
                    {
                        Id = course.Id,
                        Description = course.Description,
                        Title = course.Title,
                        IsUserJoined = course.Users.Any(u => u.Id == userId)
                    });
        }

        public void Add(int ownerId, string title, string description)
        {
            User user = Entities.Users.Single(u => u.Id == ownerId);

            var users = new List<User> { user };

            var course = DbSet.Create();
            course.OwnerId = ownerId;
            course.Title = title;
            course.Description = description;
            course.Users = users;

            Entities.Entry(course).State = EntityState.Added;

            DbSet.Add(course);
        }

        public override Course Edit(Course entity)
        {
            Course result = DbSet.Find(entity.Id);

            result.Description = entity.Description;
            result.OwnerId = entity.OwnerId;
            result.Title = entity.Title;

            return result;
        }

        public void Join(int courseId, int userId)
        {
            var course = DbSet.SingleOrDefault(c => c.Id == courseId);

            var user = Entities.Users.SingleOrDefault(u => u.Id == userId);

            if (course == null || user == null) return;
            
            course.Users.Add(user);
            course.Users.Add(user);
        }

        public void Leave(int courseId, int userId)
        {
            Course course = DbSet.SingleOrDefault(c => c.Id == courseId);

            User user = Entities.Users.SingleOrDefault(u => u.Id == userId);

            if (course != null && user != null)
            {
                course.Users.Remove(user);
            }
        }

        public List<string> Autocomplete(string term)
        {
            return DbSet.Where(c => c.Title.StartsWith(term))
                        .Take(10)
                        .Select(c => c.Title).ToList();
        }

        public List<CourseDTO> GetJoined(int userId)
        {
            return DbSet.Where(course => course.Users.Any(u => u.Id == userId))
                .Select(course => new CourseDTO
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description
                })
                .ToList();
        }
    }
}
