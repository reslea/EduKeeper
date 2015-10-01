using EduKeeper.Entities;
using EduKeeper.Infrastructure.DTO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduKeeper.EntityFramework.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        IPagedList<CourseDTO> GetPage(int userId, string searchTerm, int pageNumber = 1);

        void Add(int ownerId, string title, string description);

        void Join(int courseId, int userId);

        void Leave(int courseId, int userId);

        List<string> Autocomplete(string term);

        List<CourseDTO> GetJoined(int userId);

        void LogVisited(List<int> visitedCourses, int userId);
    }
}
