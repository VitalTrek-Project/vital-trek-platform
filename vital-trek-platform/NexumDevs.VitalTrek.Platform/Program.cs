using NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Services;
using NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.Monitoring.Infrastructure.Services;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Resources.Shared;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
// Added for ProblemDetailsFactory
// Added for base ProblemDetailsFactory
// Added for IamMessages
// Added for ProfilesMessages
using ProblemDetailsFactory = NexumDevs.VitalTrek.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

// Add ProblemDetails services
builder.Services.AddProblemDetails();


// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add Database Connection

// Configure Database Context and route EF logs through the app logger pipeline.
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Explicitly register IStringLocalizer for ErrorMessages and Commons
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services
    .AddSingleton<IStringLocalizer<CommonMessages>,
        StringLocalizer<CommonMessages>>(); // Corrected from Common to Commons


// Register the custom ProblemDetailsFactory
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

// Dependency Injection

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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



// TokenSettings Configuration

//builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));



// Mediator Configuration

// Add Mediator Injection Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));

// Add Cortex Mediator for Event Handling
builder.Services.AddCortexMediator(
    [typeof(Program)]);


var app = builder.Build();

// Apply pending migrations on startup (safe to call even when schema is up to date)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
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

// Apply CORS Policy
app.UseCors("AllowAllPolicy");

// Add Authorization Middleware to Pipeline
// app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();