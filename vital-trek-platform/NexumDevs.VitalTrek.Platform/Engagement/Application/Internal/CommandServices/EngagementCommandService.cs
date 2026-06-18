using Microsoft.Extensions.Localization;
using NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;
using NexumDevs.VitalTrek.Platform.Engagement.Domain;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Errors;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Repositories;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Repositories;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.Internal.CommandServices;

/// <summary>
/// Command service responsible for handling engagement use cases,
/// including awarding points to a tourist's gamification profile.
/// </summary>
public class EngagementCommandService : IEngagementCommandService
{
    private readonly IGamificationProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _localizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="EngagementCommandService"/> class.
    /// </summary>
    /// <param name="profileRepository">Repository used to manage gamification profiles.</param>
    /// <param name="unitOfWork">Unit of Work used to persist changes.</param>
    /// <param name="localizer">Localizer used for localized error messages.</param>
    public EngagementCommandService(
        IGamificationProfileRepository profileRepository,
        IUnitOfWork unitOfWork,
        IStringLocalizer localizer)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    /// <summary>
    /// Awards points to a tourist's gamification profile for a completed expedition.
    /// If the profile does not exist yet, it is created automatically (lazy initialization).
    /// </summary>
    /// <param name="command">The command containing the tourist identifier, expedition identifier, and points to award.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The updated <see cref="GamificationProfile"/>.</returns>
    /// <exception cref="EngagementError">
    /// Thrown when the points value is invalid or the expedition has already been awarded.
    /// </exception>
    public async Task<GamificationProfile> Handle(AwardPointsCommand command, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.FindByTouristIdAsync(command.TouristId, cancellationToken);

        if (profile is null)
        {
            profile = new GamificationProfile(command.TouristId);
            await _profileRepository.AddAsync(profile, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        if (profile.AwardedExpeditions.Any(e => e.ExpeditionId == command.ExpeditionId))
            throw new EngagementError(EngagementErrors.ExpeditionAlreadyAwarded, _localizer[EngagementErrors.ExpeditionAlreadyAwarded]);

        if (command.Points <= 0)
            throw new EngagementError(EngagementErrors.InvalidPoints, _localizer[EngagementErrors.InvalidPoints]);

        profile.AwardPoints(command.ExpeditionId, command.Points);

        _profileRepository.Update(profile);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return profile;
    }
}
