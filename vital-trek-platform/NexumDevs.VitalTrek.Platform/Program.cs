using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Resources.Shared;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using NexumDevs.VitalTrek.Platform.TourManagement.Resources;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;

using NexumDevs.VitalTrek.Platform.TourManagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.QueryService;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.Internal.CommandServices;
using NexumDevs.VitalTrek.Platform.TourManagement.Application.Internal.QueryServices;
using NexumDevs.VitalTrek.Platform.TourManagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.TourManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();


builder.Services.AddSingleton<ProblemDetailsFactory>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "NexumDevs.VitalTrek.Platform",
            Version = "v1",
            Description = "Vital Trek Platform API",
            TermsOfService = new Uri("https://vital-trek.com/tos"),
            Contact = new OpenApiContact
            {
                Name = "Vital Trek",
                Email = "contact@vitaltrek.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        { [new OpenApiSecuritySchemeReference("bearer", document)] = [] });
    options.EnableAnnotations();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<ITourAssignmentRepository, TourAssignmentRepository>();
builder.Services.AddScoped<IStringLocalizer>(sp =>
    sp.GetRequiredService<IStringLocalizer<TourManagementMessages>>());
builder.Services.AddSingleton<IStringLocalizer<TourManagementMessages>, StringLocalizer<TourManagementMessages>>();

builder.Services.AddScoped<ITourCommandService, TourCommandService>();
builder.Services.AddScoped<ITourQueryService, TourQueryService>();


builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseGlobalExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();