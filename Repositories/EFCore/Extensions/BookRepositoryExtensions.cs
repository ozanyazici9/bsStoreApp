using Entities.Models;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions;

public static class BookRepositoryExtensions
{
    public static IQueryable<Book> FilterBooks(
        this IQueryable<Book> books,
        uint minPrice,
        uint maxPrice
    ) => books.Where(b => b.Price >= minPrice && b.Price <= maxPrice);

    public static IQueryable<Book> SearchBook(this IQueryable<Book> books, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return books;

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return books.Where(b => b.Title.ToLower().Contains(lowerCaseTerm));
    }
}
