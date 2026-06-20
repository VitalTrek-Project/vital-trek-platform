using NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Services;
using NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Services;

using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.Engagement.Resources;
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

using NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.CommandServices;
using NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Navigation.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
// Added for ProblemDetailsFactory
// Added for base ProblemDetailsFactory
// Added for IamMessages
// Added for ProfilesMessages
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
// Monitoring Bounded Context
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentCommandService, IncidentCommandService>();
builder.Services.AddScoped<IIncidentQueryService, IncidentQueryService>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();
builder.Services.AddScoped<ILocationReadingRepository, LocationReadingRepository>();
builder.Services.AddScoped<IVitalSignReadingRepository, VitalSignReadingRepository>();
builder.Services.AddScoped<ILocationReadingCommandService, LocationReadingCommandService>();
builder.Services.AddScoped<ILocationReadingQueryService, LocationReadingQueryService>();
builder.Services.AddScoped<IVitalSignReadingCommandService, VitalSignReadingCommandService>();
builder.Services.AddScoped<IVitalSignReadingQueryService, VitalSignReadingQueryService>();
builder.Services.AddScoped<IAnomalyDetectionService, ThresholdAnomalyDetectionService>();
// Navigation Bounded Context
builder.Services.AddScoped<IExpeditionRepository, ExpeditionRepository>();
builder.Services.AddScoped<IExpeditionCommandService, ExpeditionCommandService>();
builder.Services.AddScoped<IExpeditionQueryService, ExpeditionQueryService>();

builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<IExperienceCommandService, ExperienceCommandService>();
builder.Services.AddScoped<IExperienceQueryService, ExperienceQueryService>();

builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
builder.Services.AddScoped<IProgressCommandService, ProgressCommandService>();
builder.Services.AddScoped<IProgressQueryService, ProgressQueryService>();

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IWeatherCommandService, WeatherCommandService>();
builder.Services.AddScoped<IWeatherQueryService, WeatherQueryService>();

builder.Services.AddScoped<IBinnacleReadingRepository, BinnacleReadingRepository>();
builder.Services.AddScoped<IBinnacleReadingCommandService, BinnacleReadingCommandService>();
builder.Services.AddScoped<IBinnacleReadingQueryService, BinnacleReadingQueryService>();
// Engagement Bounded Context
builder.Services.AddScoped<IGamificationProfileRepository, GamificationProfileRepository>();
builder.Services.AddScoped<IStringLocalizer>(sp =>
    sp.GetRequiredService<IStringLocalizer<EngagementMessages>>());
builder.Services.AddSingleton<IStringLocalizer<EngagementMessages>, StringLocalizer<EngagementMessages>>();
builder.Services.AddScoped<IEngagementCommandService, EngagementCommandService>();
builder.Services.AddScoped<IEngagementQueryService, EngagementQueryService>();

// TokenSettings Configuration

//builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));



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
