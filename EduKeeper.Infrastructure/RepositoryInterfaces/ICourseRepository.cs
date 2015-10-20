using EduKeeper.Entities;
using EduKeeper.Infrastructure.DTO;
using System.Collections.Generic;
using System.Linq;

namespace EduKeeper.Infrastructure.RepositoryInterfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        IQueryable<CourseDTO> GetAll(int userId, string searchTerm);

        void Add(int ownerId, string title, string description);

        void Join(int courseId, int userId);

        void Leave(int courseId, int userId);

        List<string> Autocomplete(string term);

        List<CourseDTO> GetJoined(int userId);
    }
}
