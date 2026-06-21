using NexumDevs.VitalTrek.Platform.Monitoring.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Aggregate;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Monitoring.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace NexumDevs.VitalTrek.Platform.Monitoring.Application.Internal.CommandServices;

public class IncidentCommandService(
    IIncidentRepository incidentRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer, // Inject IStringLocalizer
    ILogger<IncidentCommandService> logger)
    : IIncidentCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;
   

    

    /// <inheritdoc />
    public async Task<Result<Incident>> Handle(CreateIncidentCommand command, CancellationToken cancellationToken)
    {
        
       
        var incident = new Incident(command);
        try
        {
            await incidentRepository.AddAsync(incident, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
          
            return Result<Incident>.Success(incident);
        }
        catch (OperationCanceledException)
        {
            return Result<Incident>.Failure(MonitoringError.OperationCancelled,
                _localizer[nameof(MonitoringError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            // Log the exception details here if an ILogger is injected
            return Result<Incident>.Failure(MonitoringError.DatabaseError,
                _localizer[nameof(MonitoringError.DatabaseError)]);
        }
        catch (Exception)
        {
            // Log the exception details here if an ILogger is injected
            return Result<Incident>.Failure(MonitoringError.InternalServerError,
                _localizer[nameof(MonitoringError.InternalServerError)]);
        }
    }
}