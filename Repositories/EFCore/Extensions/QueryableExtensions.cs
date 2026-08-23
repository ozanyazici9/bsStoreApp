using System.Linq.Dynamic.Core;
using Entities.Contracts;

namespace Repositories.EFCore.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, string orderByQueryString)
        where T : IEntity
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return queryable.OrderBy(q => q.Id);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<T>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return queryable.OrderBy(q => q.Id);

        return queryable.OrderBy(orderQuery);
    }
}
