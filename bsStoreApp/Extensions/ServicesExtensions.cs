using Asp.Versioning;
using Entities.DataTransferObjects;
using Marvin.Cache.Headers;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Presentation.Controllers;
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
        services
            .AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddMvc(opt =>
            {
                opt.Conventions.Controller<BooksController>().HasApiVersion(new ApiVersion(1, 0));
                opt.Conventions.Controller<BooksV2Controller>()
                    .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            });
    }

    public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();

    public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
        services.AddHttpCacheHeaders(
            expirationOpt =>
            {
                expirationOpt.MaxAge = 90;
                //public değeri ekleniyor. Bu, sadece client tarayıcısının değil, aradaki paylaşımlı cache'lerin de (CDN, reverse proxy, ISP cache'i gibi) bu response'u cache'lemesine izin verildiği anlamına geliyor. Alternatifi Private olsaydı, sadece son kullanıcının kendi tarayıcısı cache'leyebilirdi — kullanıcıya özel veri (örneğin kişisel profil bilgisi) döndüren endpoint'lerde Private tercih edilir.
                expirationOpt.CacheLocation = CacheLocation.Public;
            },
            validationOpt =>
            {
                //must-revalidate eklenmiyor. Bunun anlamı: 90 saniyelik süre dolduktan sonra client, sunucuya sorup doğrulamadan da (biraz "bayat" olsa dahi) elindeki cache'lenmiş veriyi kullanmaya devam edebilir. Eğer true olsaydı, süre dolar dolmaz client zorunlu olarak sunucuya gidip ETag/Last-Modified ile doğrulama yapmak zorunda kalırdı, aksi halde o veriyi kullanamazdı.
                validationOpt.MustRevalidate = false;
            }
        );
}
