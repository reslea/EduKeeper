using EduKeeper.Entities;
using System.Linq;

namespace EduKeeper.Infrastructure.RepositoryInterfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Find();

        T Add(T entity);

        T Get(int id);

        T Edit(T entity);

        T CreateEntity();

        void Remove(int id);

        void Save();
    }
}
