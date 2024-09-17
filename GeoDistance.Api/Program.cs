using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;

using GeoDistance.Api.Middleware;
using GeoDistance.Core.Configuration;
using GeoDistance.Core.Services;

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

using Serilog;

using Swashbuckle.AspNetCore.SwaggerGen;

var assembly = Assembly.GetEntryAssembly();
var assemblyName = assembly?.GetName();
var projectName = assemblyName?.Name;
var version = assemblyName?.Version?.ToString();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(AppContext.BaseDirectory);

Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");

builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration));

ConfigureServices(builder.Services, builder.Configuration);

var webApplication = builder.Build();
webApplication.Logger.LogInformation("Версия приложения {Version}", version);
await ConfigureApplication(webApplication);
return;

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddMvcCore()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        })
        .AddApiExplorer();

    AddSwaggerGen(services);
    AddCors(services);

    services.AddHealthChecks();
    services.AddHttpClient();

    services.AddScoped<IDistanceService, DistanceService>();
    services.AddScoped<IIataModelValidationService, IataModelValidationService>();
    services.AddScoped<IGeoCoordinateService, GeoCoordinateService>();
    AddConfigurations(services, configuration);

    services.AddMemoryCache();
}

static void AddConfigurations(IServiceCollection services, IConfiguration configuration)
{
    var cacheConfiguration = configuration.GetSection(CacheConfiguration.KEY);
    services.AddOptions<CacheConfiguration>()
        .Bind(cacheConfiguration);

    var serverConfiguration = configuration.GetSection(GeoCoordinateServerConfiguration.KEY);
    services.AddOptions<GeoCoordinateServerConfiguration>()
        .Bind(serverConfiguration);
}

void AddCors(IServiceCollection services)
{
    // https://docs.microsoft.com/ru-ru/aspnet/core/security/cors
    services.AddCors(options => options.AddDefaultPolicy(b => ConfigureCorsPolicy(b, builder.Configuration)));
}

void ConfigureCorsPolicy(CorsPolicyBuilder corsPolicyBuilder, IConfiguration configuration)
{
    var origins = configuration.GetSection("AllowedOrigins").Get<string[]>();
    if (origins?.Any() != true)
        return;

    corsPolicyBuilder.WithOrigins(origins)
        .AllowAnyHeader()
        .AllowAnyMethod();
}

void AddSwaggerGen(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(type => type.ToString());
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = projectName,
            Version = "v1",
        });

        c.SupportNonNullableReferenceTypes();
        IncludeXmlComments(c);
    });
}

void IncludeXmlComments(SwaggerGenOptions options)
{
    var baseDirectory = AppContext.BaseDirectory;
    var xmlPaths = new List<string>
    {
        Path.Combine(baseDirectory, $"{projectName}.xml"),
    };

    xmlPaths.ForEach(s =>
    {
        if (File.Exists(s))
            options.IncludeXmlComments(s, true);
    });
}

async Task ConfigureApplication(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (app.Environment.EnvironmentName == "Development")
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
        app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", $"{projectName} v1"));
    }

    app.UseSerilogRequestLogging();
    app.UseExceptionHandler("/error");

    app.UseRouting();
    app.UseCors();

    app.UseMiddleware<LoggingMiddleware>();

    app.MapControllers();
    app.MapHealthChecks("/healthz", new HealthCheckOptions { Predicate = x => x.Tags.Contains("live") })
        .AllowAnonymous();

    app.MapHealthChecks("/ready", new HealthCheckOptions { Predicate = x => x.Tags.Contains("ready") })
        .AllowAnonymous();

    SetThreads();

    await app.RunAsync();
}

void SetThreads()
{
    const int threads = 2500;

    ThreadPool.GetMaxThreads(out var workerThreads, out var ioCompletionThreads);
    ThreadPool.SetMaxThreads(Math.Max(threads, workerThreads), Math.Max(threads, ioCompletionThreads));
    ThreadPool.SetMinThreads(threads, threads);
}

namespace GeoDistance.Api
{
    public class Program
    {
    }
}