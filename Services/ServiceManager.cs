using AutoMapper;
using Entities.DataTransferObjects;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IBookServices> _bookService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, IDataShaper<BookDto> shapper)
    {
        _bookService = new Lazy<IBookServices>(() => new BookManager(repositoryManager, mapper, shapper));
    }

    public IBookServices BookService => _bookService.Value;
}
