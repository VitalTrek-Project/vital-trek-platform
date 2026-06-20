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

public class ExpeditionCommandService(
    IExpeditionRepository expeditionRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer,
    ILogger<ExpeditionCommandService> logger)
    : IExpeditionCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Expedition>> Handle(CreateExpeditionCommand command, CancellationToken cancellationToken)
    {
        var expedition = new Expedition(command);

        try
        {
            await expeditionRepository.AddAsync(expedition, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Expedition>.Success(expedition);
        }
        catch (OperationCanceledException)
        {
            return Result<Expedition>.Failure(NavigationError.OperationCancelled,
                _localizer[nameof(NavigationError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Expedition>.Failure(NavigationError.DatabaseError,
                _localizer[nameof(NavigationError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Expedition>.Failure(NavigationError.InternalServerError,
                _localizer[nameof(NavigationError.InternalServerError)]);
        }
    }
}
