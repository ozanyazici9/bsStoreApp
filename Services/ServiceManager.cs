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

    public ServiceManager(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IDataShaper<BookDto> shapper,
        ILoggerService logger,
        IConfiguration configuration,
        UserManager<User> userManager
    )
    {
        _bookService = new Lazy<IBookServices>(() =>
            new BookManager(repositoryManager, mapper, shapper)
        );

        _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticationManager(logger, mapper, userManager, configuration)
        );
    }

    public IBookServices BookService => _bookService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
