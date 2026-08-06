using Asp.Versioning;
using Entities.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;

namespace bsStoreApp.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureSqlContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<RepositoryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerService, LoggerManager>();

    public static void ConfigureActionFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidationFilterAttribute>();
        services.AddSingleton<LogFilterAttribute>();
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination")
            );
        });
    }

    public static void ConfigureDataShapper(this IServiceCollection services) =>
        services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
        });
    }
}
