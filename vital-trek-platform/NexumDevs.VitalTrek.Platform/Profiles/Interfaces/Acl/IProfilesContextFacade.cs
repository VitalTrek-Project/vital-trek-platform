namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Acl;

/// <summary>
/// Anti-corruption layer exposing read-only Profiles facts to other bounded contexts,
/// without letting them query Profiles' own tables directly. Mirrors
/// <c>Iam.Interfaces.Acl.IIamContextFacade</c>.
/// </summary>
public interface IProfilesContextFacade
{
    /// <summary>Preferred language for a tourist, consumed by Loyalty when composing notifications.</summary>
    Task<string?> FetchPreferredLanguageAsync(Guid touristUserId, CancellationToken cancellationToken);

    /// <summary>Dietary restrictions for a tourist, consumed by expedition/booking meal planning.</summary>
    Task<IReadOnlyList<string>> FetchDietaryRestrictionsAsync(Guid touristUserId, CancellationToken cancellationToken);

    /// <summary>Whether the tourist's profile is complete enough to join an expedition.</summary>
    Task<bool> IsEligibleForExpeditionAsync(Guid touristUserId, CancellationToken cancellationToken);
}
