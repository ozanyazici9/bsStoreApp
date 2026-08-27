using System.Text;
using Asp.Versioning;
using AspNetCoreRateLimit;
using Entities.DataTransferObjects;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories;
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
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc(opt =>
            {
                opt.Conventions.Controller<BooksController>().HasApiVersion(new ApiVersion(1, 0));

                opt.Conventions.Controller<BooksV2Controller>().HasApiVersion(new ApiVersion(2, 0));
            })
            .AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
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

    public static void ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        var rateLimitRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "1m",
                Limit = 60,
            },
        };

        services.Configure<IpRateLimitOptions>(opt =>
        {
            opt.GeneralRules = rateLimitRules;
        });

        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services
            .AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 6;

                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetSection("secretKey").Value;

        services
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                };
            });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "BTK Akademi",
                    Version = "v1",
                    Description = "BTK Akademi ASP.NET Core API",
                    TermsOfService = new Uri("https://www.btkakademi.gov.tr/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Ozan Yazıcı",
                        Email = "ozanyazici9@gmail.com",
                        Url = new Uri("https://www.ozanyazici.com.tr/"),
                    },
                }
            );
            s.SwaggerDoc("v2", new OpenApiInfo { Title = "BTK Akademi", Version = "v2" });

            s.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please to add JWT with Bearer",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                }
            );

            s.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
            });
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IBookServices, BookManager>();
        services.AddScoped<ICategoryService, CategoryManager>();
        services.AddScoped<IAuthenticationService, AuthenticationManager>();
    }
}
