using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Contracts;

namespace Repositories.EFCore;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _context;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;
    }

    public IBookRepository Book => new BookRepository(_context);

    public void Save()
    {
        _context.SaveChanges();
    }
}
