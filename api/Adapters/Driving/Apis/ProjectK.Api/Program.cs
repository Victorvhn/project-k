using Hellang.Middleware.ProblemDetails;
using ProjectK.Api;
using ProjectK.Api.Configurations.Extensions;
using ProjectK.Api.Middlewares;
using ProjectK.Core;
using ProjectK.Database;
using ProjectK.Database.Extensions;
using ProjectK.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog(builder.Configuration);

builder.Services
    .AddCoreDependencies()
    .AddDatabaseDependencies()
    .AddApiDependencies()
    .AddInfrastructureDependencies();

builder.Services.AddAllowedHosts(builder.Configuration);
builder.Services.AddProblemDetailsMiddleware(builder.Environment);
builder.Services.AddMediatorConfig();
builder.Services.AddDatabase(builder.Configuration, builder.Environment);
builder.Services.AddControllersConventions();
builder.Services.AddRatingLimit(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddApiVersions();
builder.Services.AddHealthCheck(builder.Configuration);
builder.Services.AddFluentValidation(typeof(Program));
builder.Services.AddAutoMapperProfiles();
builder.Services.AddAuthentication(builder.Configuration);

var app = builder.Build();

app.UseProblemDetails();
app.UseSerilogRequestLogging();
await app.UseDatabase();
app.UseHttpsRedirection();
app.UseHsts();

app.UseCors();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<UserResolutionMiddleware>();

app.UseHealthChecks();
app.UseSwaggerWithVersions();
app.UseRequestContextLogging();

app.Run();