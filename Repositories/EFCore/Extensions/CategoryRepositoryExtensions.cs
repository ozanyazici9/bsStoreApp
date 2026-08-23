using System.Linq.Dynamic.Core;
using Entities.Models;

namespace Repositories.EFCore.Extensions;

public static class CategoryRepositoryExtensions
{
    public static IQueryable<Category> SearchCategory(
        this IQueryable<Category> queryable,
        string searchTerm
    )
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return queryable;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return queryable.Where(c => c.CategoryName.ToLower().Contains(lowerCaseTerm));
    }
}
