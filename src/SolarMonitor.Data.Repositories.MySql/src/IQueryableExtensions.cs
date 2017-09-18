using System.Linq;

namespace SolarMonitor.Data.Repositories.MySql
{
    public static class IQueryableExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(
              this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalCount = query.Count();
            var collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new PaginatedList<T>(pageIndex, pageSize, totalCount, collection);
        }
    }
}
