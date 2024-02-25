using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Cysharp.Serialization.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectK.Api.Configurations.ModelConventions;
using ProjectK.Api.Configurations.Swagger;
using ProjectK.Api.Filters;
using ProjectK.Api.ModelBinders;
using ProjectK.Api.Profiles.v1;
using ProjectK.Api.SchemaFilters;
using Serilog;
using Serilog.Events;
using ServiceCollectionExtensions = ProjectK.Database.Extensions.ServiceCollectionExtensions;

namespace ProjectK.Api.Configurations.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static void AddControllersConventions(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new MonthlyRequestBinderProvider());
                options.Filters.Add<LanguageFilter>();
                options.Filters.Add<NotificationFilter>();
                options.Filters.Add<TransactionFilter>();
                options.Conventions.Add(new RoutingControllerModelConvention());
                options.Conventions.Add(new RoutingActionModelConvention());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = GetProblemDetails(context);

                    LogValidationProblem(context, problemDetails);

                    return new ObjectResult(problemDetails);
                };
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new UlidJsonConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
    }

    private static void LogValidationProblem(ActionContext context, ValidationProblemDetails problemDetails)
    {
        var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger =
            loggerFactory.CreateLogger(context.ActionDescriptor.DisplayName ?? "ModelStateMiddleware");

        logger.LogWarning(
            "Model state invalid for '{@ActionName}' on '{@ControllerName}' due to errors: '{@ProblemDetailsErrors}'",
            context.ActionDescriptor.DisplayName, context.ActionDescriptor.RouteValues["controller"],
            problemDetails.Errors);
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var acceptExpiredTokens = configuration.GetValue<bool?>("Authentication:AcceptExpiredTokens") ?? false;
        var secret = configuration.GetValue<string>("Authentication:Secret");

        ArgumentException.ThrowIfNullOrWhiteSpace(secret);

        var key = Encoding.UTF8.GetBytes(secret);
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = !acceptExpiredTokens,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "ProjectKWebApp",
                    ValidAudience = "ProjectK",
                    LifetimeValidator = acceptExpiredTokens
                        ? null
                        : (_, expires, _, _) => expires != null && expires > DateTime.UtcNow,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        services.AddAuthorization();
    }

    public static void AddRatingLimit(this IServiceCollection services, IConfiguration configuration)
    {
        var maxRequests = configuration.GetValue<int?>("ProjectK.Api:RateLimit:MaxRequests") ?? 10;
        var maxRequestsWindow = configuration.GetValue<TimeSpan?>("ProjectK.Api:RateLimit:MaxRequestsWindow") ??
                                TimeSpan.FromSeconds(1);

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString()!,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = maxRequests,
                        QueueLimit = 0,
                        Window = maxRequestsWindow
                    }));
        });
    }

    // TODO: This is not working, fix it
    public static void AddAllowedHosts(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedHosts = configuration.GetValue<string>("ProjectK.Api:AllowedOrigins")?.Split(';') ?? ["*"];

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedHosts)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<UlidSchemaFilter>();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = """
                              JWT Authorization header using the Bearer scheme.
                              <br/>Enter 'Bearer' [space] and then your token in the text input below.
                              <br/>Example: 'Bearer 12345random'
                              <br/><br/>
                              """,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }

    public static void AddApiVersions(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version"));
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(ServiceCollectionExtensions.GetDefaultConnectionString(configuration));
    }

    public static void AddFluentValidation(this IServiceCollection services, Type programType)
    {
        ArgumentNullException.ThrowIfNull(programType);

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(programType, ServiceLifetime.Singleton);
    }

    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(RequestToDomainDtoProfile),
            typeof(EntityToResponseProfile),
            typeof(CoreDtoToResponseProfile)
        );
    }

    public static void AddSerilog(this ConfigureHostBuilder host, IConfiguration configuration)
    {
        // This is a workaround for heroku, for some reason Serilog in the appsettings is not being injected properly. 
        host.UseSerilog((_, loggerConfig) =>
        {
            loggerConfig.MinimumLevel.Is(configuration.GetValue<LogEventLevel?>("SerilogConfig:MinimumLevel") ??
                                         LogEventLevel.Debug);
            loggerConfig.WriteTo.Console();
            loggerConfig.WriteTo.Seq(configuration.GetValue<string>("SerilogConfig:Seq:ServerUrl")!,
                apiKey: configuration.GetValue<string>("SerilogConfig:Seq:ApiKey"));
            loggerConfig.Enrich.FromLogContext();
            loggerConfig.Enrich.WithMachineName();
            loggerConfig.Enrich.WithThreadId();
            loggerConfig.Enrich.WithEnvironmentName();
        });
    }

    public static void AddProblemDetailsMiddleware(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (_, _) => environment.IsDevelopment();

            options.OnBeforeWriteDetails = (ctx, details) => { details.Instance = ctx.Request.Path; };

            options.Rethrow<NotSupportedException>();
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });
    }

    private static ValidationProblemDetails GetProblemDetails(ActionContext context)
    {
        var factory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        var problemDetails = factory.CreateValidationProblemDetails(
            context.HttpContext,
            context.ModelState,
            400,
            instance: context.HttpContext.Request.Path
        );

        return problemDetails;
    }
}
