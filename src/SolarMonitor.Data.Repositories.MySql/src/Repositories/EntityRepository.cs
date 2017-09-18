using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Linq.Expressions;

using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class EntityRepository<T> : IEntityRepository<T>
      where T : class, IEntity, new()
    {
        protected readonly DbContext _entitiesContext;

        public EntityRepository(ApplicationDbContext entitiesContext)
        {
            if (entitiesContext == null)
            {
                throw new ArgumentNullException("entitiesContext");
            }

            _entitiesContext = entitiesContext;
        }

        // public interface
        public T GetSingle(int key)
        {
            return GetAll().FirstOrDefault(x => x.Id == key);
        }

        public virtual PaginatedList<T> Paginate<TKey>(
            int pageIndex, int pageSize,
            Expression<Func<T, TKey>> keySelector)
        {

            return Paginate(pageIndex, pageSize, keySelector, null);
        }

        public virtual PaginatedList<T> Paginate<TKey>(
            int pageIndex, int pageSize,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query =
                AllIncluding(includeProperties).OrderBy(keySelector);

            query = (predicate == null)
                ? query
                : query.Where(predicate);

            return query.ToPaginatedList(pageIndex, pageSize);
        }

        public virtual T Create(T entity)
        {

            EntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Added;
            _entitiesContext.SaveChanges();
            return entity;
        }

        public virtual void Update(T entity)
        {

            EntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }


        public virtual void Delete(T entity)
        {

            EntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteGraph(T entity)
        {
            DbSet<T> dbSet = _entitiesContext.Set<T>();
            dbSet.Attach(entity);
            dbSet.Remove(entity);
            _entitiesContext.SaveChanges();
        }

        // protected functions - useful for other repository implementations
        protected DbSet<T> DataSet()
        {
            return _entitiesContext.Set<T>();
        }

        protected virtual IQueryable<T> GetAll()
        {
            return _entitiesContext.Set<T>();
        }

        protected virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entitiesContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        protected virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {

            return _entitiesContext.Set<T>().Where(predicate);
        }
    }
}