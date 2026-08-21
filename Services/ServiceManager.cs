using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IBookServices> _bookService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<ICategoryService> _categoryService;

    public ServiceManager(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IDataShaper<BookDto> shapper,
        ILoggerService logger,
        IConfiguration configuration,
        UserManager<User> userManager,
        ICategoryService categoryService
    )
    {
        _bookService = new Lazy<IBookServices>(() =>
            new BookManager(repositoryManager, mapper, shapper)
        );

        _categoryService = new Lazy<ICategoryService>(() =>
            new CategoryManager(repositoryManager, mapper)
        );


        _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticationManager(logger, mapper, userManager, configuration)
        );
    }

    public IBookServices BookService => _bookService.Value;

    public ICategoryService CategoryService => _categoryService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
