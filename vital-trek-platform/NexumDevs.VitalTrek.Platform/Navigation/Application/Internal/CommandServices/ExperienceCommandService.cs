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

public class ExperienceCommandService(
    IExperienceRepository experienceRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer,
    ILogger<ExperienceCommandService> logger)
     : IExperienceCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Experience>> Handle(CreateExperienceCommand command, CancellationToken cancellationToken)
    {
        var experience = new Experience(command);

        try
        {
            await experienceRepository.AddAsync(experience, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Experience>.Success(experience);
        }
        catch (OperationCanceledException)
        {
            return Result<Experience>.Failure(NavigationError.OperationCancelled,
                _localizer[nameof(NavigationError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Experience>.Failure(NavigationError.DatabaseError,
                _localizer[nameof(NavigationError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Experience>.Failure(NavigationError.InternalServerError,
                _localizer[nameof(NavigationError.InternalServerError)]);
        }
    }
}
