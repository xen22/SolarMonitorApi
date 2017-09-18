// taken from "ProASP.NET Web API" by Tugberk Ugurlu, p. 132

using System;
using System.Linq;
using System.Linq.Expressions;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T GetSingle(int key);
        PaginatedList<T> Paginate<TKey>(
              int pageIndex, int pageSize,
              Expression<Func<T, TKey>> keySelector);

        PaginatedList<T> Paginate<TKey>(
              int pageIndex, int pageSize,
              Expression<Func<T, TKey>> keySelector,
              Expression<Func<T, bool>> predicate,
              params Expression<Func<T, object>>[] includeProperties);

        T Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteGraph(T entity);
    }
}
