using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Services.Contracts;

public interface IBookServices
{
    IEnumerable<Book> GetAllBooks(bool trackChanges);
    Book GetOneBookById(int id, bool trackChanges);
    Book CreateOneBook(Book book);
    void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges);
    void DeleteOneBook(int id, bool trackChanges);
}
