using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Repositories.Contracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    IQueryable<Book> GetAllBooks(bool trackChanges);
    Book GetOneBookById(int id, bool trackChanges);
    void CreateOneBook(Book book);
    void UpdateOneBook(Book book);
    void DeleteOneBook(Book book);
}
