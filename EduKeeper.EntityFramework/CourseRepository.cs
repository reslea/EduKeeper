using EduKeeper.Entities;
using EduKeeper.EntityFramework.Interfaces;
using EduKeeper.Infrastructure.DTO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduKeeper.EntityFramework
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private int pageSize = 20;

        public CourseRepository(EduKeeperContext context) : base(context) { }

        public IPagedList<CourseDTO> GetPage(int userId, string searchTerm, int pageNumber = 1)
        {
            IQueryable<Course> courses;

            if (String.IsNullOrEmpty(searchTerm))
                courses = _dbset;

            else
            {
                courses = _dbset.Where(c => c.Description.ToLower().Contains(searchTerm.ToLower()) ||
                    c.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            return courses.OrderBy(c => c.Id)
                    .Select(course => new CourseDTO
                    {
                        Id = course.Id,
                        Description = course.Description,
                        Title = course.Title,
                        IsJoined = course.Users.Any(u => u.Id == userId)
                    })
                    .ToPagedList(pageNumber, pageSize);
        }

        public void Add(int ownerId, string title, string description)
        {
            User user = _entities.Users.Single(u => u.Id == ownerId);

            var users = new List<User>();
            users.Add(user);

            var course = _dbset.Create();
            course.OwnerId = ownerId;
            course.Title = title;
            course.Description = description;
            course.Users = users;

            _dbset.Add(course);
        }

        public override Course Edit(Course entity)
        {
            Course result = result = _dbset.Find(entity.Id);

            result.Description = entity.Description;
            result.OwnerId = entity.OwnerId;
            result.Title = entity.Title;

            return result;
        }

        public void Join(int courseId, int userId)
        {
            Course course = _dbset.SingleOrDefault(c => c.Id == courseId);

            if (course != null)
            {
                //its not tested
                course.Users.Add(new User() { Id = userId});
            }
        }

        public void Leave(int courseId, int userId)
        {
            Course course = _dbset.SingleOrDefault(c => c.Id == courseId);

            if (course != null)
            {
                //its not tested
                course.Users.Remove(new User() { Id = userId });
            }
        }

        public List<string> Autocomplete(string term)
        {
            return _dbset.Where(c => c.Title.StartsWith(term))
                        .Take(10)
                        .Select(c => c.Title).ToList();
        }

        public List<CourseDTO> GetJoined(int userId)
        {
            return _dbset.Where(course => course.Users.Any(u => u.Id == userId))
                .Select(course => new CourseDTO() 
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    IsJoined = true
                })
                .ToList();
        }

        public void LogVisited(List<int> visitedCourses, int userId)
        {
            var sbCourses = new StringBuilder();
            foreach (int courseId in visitedCourses)
            {
                sbCourses.Append('#');
                sbCourses.Append(courseId);
            }
            
            var user = _entities.Users.Single(u => u.Id == userId);

            user.VisitedCourses = sbCourses.ToString();

            user.LastVisited = DateTime.Now;
        }
    }
}
