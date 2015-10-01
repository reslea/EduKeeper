using EduKeeper.Entities;
using PagedList;
using System;
using System.Linq.Expressions;

namespace EduKeeper.EntityFramework.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
    IPagedList<T> GetPage(int pageNumber = 1);

    IPagedList<T> FindBy(Expression<Func<T, bool>> predicate, int pageNumber = 1);

    T Add(T entity);

    T Get(int id);

    T Edit(T entity);

    void Remove(int id);

    void Save();
    }
}
