using NexumDevs.VitalTrek.Platform.Profiles.Application.QueryServices;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Queries;
using NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Acl;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.Acl;

public class ProfilesContextFacade(
    ITouristProfileQueryService profileQueryService,
    ITouristPreferencesQueryService preferencesQueryService)
    : IProfilesContextFacade
{
    public async Task<string?> FetchPreferredLanguageAsync(Guid touristUserId, CancellationToken cancellationToken)
    {
        var profile = await profileQueryService.Handle(new GetTouristProfileByUserIdQuery(touristUserId), cancellationToken);
        return profile?.PreferredLanguage;
    }

    public async Task<IReadOnlyList<string>> FetchDietaryRestrictionsAsync(Guid touristUserId, CancellationToken cancellationToken)
    {
        var preferences = await preferencesQueryService.Handle(new GetTouristPreferencesByUserIdQuery(touristUserId), cancellationToken);
        return preferences?.DietaryRestrictions.ToList() ?? [];
    }

    public async Task<bool> IsEligibleForExpeditionAsync(Guid touristUserId, CancellationToken cancellationToken)
    {
        var result = await profileQueryService.Handle(new GetTouristProfileCompletenessQuery(touristUserId), cancellationToken);
        return result.CanJoinExpedition;
    }
}
