using EduKeeper.Entities;
using EduKeeper.Infrastructure.RepositoryInterfaces;
using System.Data.Entity;
using System.Linq;

namespace EduKeeper.EntityFramework
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {        
        protected EduKeeperContext Entities { get; private set; }

        protected IDbSet<T> DbSet { get; private set; }

        protected Repository(EduKeeperContext context)
        {
            Entities = context;
            DbSet = context.Set<T>();
        }

        public IQueryable<T> Find()
        {
            return DbSet.AsQueryable();
        }

        public virtual T Add(T entity)
        {
            Entities.Entry(entity).State = EntityState.Added;

            return DbSet.Add(entity);
        }

        public virtual T Get(int id)
        {
            return DbSet.SingleOrDefault(entity => entity.Id == id);
        }

        public virtual T Edit(T entity)
        {
            if (Entities.Entry(entity).State == EntityState.Detached)
                Entities.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public virtual void Remove(int id)
        {
            var entity = DbSet.First(e => e.Id == id);
            DbSet.Remove(entity);

            Entities.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Save()
        {
            Entities.SaveChanges();
        }

        public T CreateEntity()
        {
            return DbSet.Create();
        }
    }
}
