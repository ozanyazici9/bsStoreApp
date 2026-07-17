using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IBookServices> _bookService;

    public ServiceManager(IRepositoryManager repositoryManager)
    {
        _bookService = new Lazy<IBookServices>(() => new BookManager(repositoryManager));
    }

    public IBookServices BookService => _bookService.Value;
}
