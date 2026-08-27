using Services.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly IBookServices _bookService;
    private readonly IAuthenticationService _authenticationService;
    private readonly ICategoryService _categoryService;

    public ServiceManager(IBookServices bookService, IAuthenticationService authenticationService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _authenticationService = authenticationService;
        _categoryService = categoryService;
    }

    public IBookServices BookService => _bookService;

    public ICategoryService CategoryService => _categoryService;

    public IAuthenticationService AuthenticationService => _authenticationService;
}
