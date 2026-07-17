using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;

namespace Services.Contracts;

public interface IBookServices
{
    IEnumerable<Book> GetAllBooksAsync(bool trackChanges);
    Book GetOneBookByIdAsync(int id, bool trackChanges);
    Book CreateOneBook(Book book);
    void UpdateOneBook(int id, Book book, bool trackChanges);
    void DeleteOneBook(int id, bool trackChanges);
}
