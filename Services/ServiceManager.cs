using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IBookServices> _bookService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerService logger, IMapper mapper)
    {
        _bookService = new Lazy<IBookServices>(() => new BookManager(repositoryManager, logger, mapper));
    }

    public IBookServices BookService => _bookService.Value;
}
