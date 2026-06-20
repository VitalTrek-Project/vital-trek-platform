using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Navigation.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Resources.Errors;
using NexumDevs.VitalTrek.Platform.Shared.Application.Model;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Navigation.Application.Internal.CommandServices;

public class ProgressCommandService(
    IProgressRepository progressRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer,
    ILogger<ProgressCommandService> logger) 
    : IProgressCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Progress>> Handle(CreateProgressCommand command, CancellationToken cancellationToken)
    {
        var progress = new Progress(command);

        try
        {
            await progressRepository.AddAsync(progress, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            return Result<Progress>.Success(progress);
        }
        catch (OperationCanceledException)
        {
            return Result<Progress>.Failure(NavigationError.OperationCancelled,
                _localizer[nameof(NavigationError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Progress>.Failure(NavigationError.DatabaseError,
                _localizer[nameof(NavigationError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Progress>.Failure(NavigationError.InternalServerError,
                _localizer[nameof(NavigationError.InternalServerError)]);
        }
    }
}
