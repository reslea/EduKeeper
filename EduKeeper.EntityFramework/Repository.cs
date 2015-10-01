using EduKeeper.Entities;
using EduKeeper.EntityFramework.Interfaces;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EduKeeper.EntityFramework
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public int DefaultPageSize = 10;
        protected EduKeeperContext _entities;
        protected readonly IDbSet<T> _dbset;

        public Repository(EduKeeperContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }

        public virtual IPagedList<T> GetPage(int pageNumber = 1)
        {
            return _dbset.ToPagedList(1, DefaultPageSize);
        }

        public virtual IPagedList<T> FindBy(Expression<Func<T, bool>> predicate, int pageNumber = 1)
        {
            return _dbset.Where(predicate).ToPagedList(1, 10);
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Get(int id)
        {
            return _dbset.SingleOrDefault(entity => entity.Id == id);
        }

        public virtual T Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual void Remove(int id)
        {
            _dbset.Remove(_dbset.Where(entity => entity.Id == id).First());
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}
