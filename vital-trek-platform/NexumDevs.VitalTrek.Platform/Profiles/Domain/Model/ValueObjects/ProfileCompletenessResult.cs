namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.ValueObjects;

/// <summary>
/// Result of evaluating whether a tourist profile has enough information to join an
/// expedition, and what is still missing otherwise. Consumed by the booking/reservation
/// flow (TourManagement) through <see cref="Interfaces.Acl.IProfilesContextFacade"/>.
/// </summary>
/// <param name="CanJoinExpedition">Whether the profile meets the minimum requirements to book an expedition.</param>
/// <param name="CompletionPercentage">Overall profile completion, from 0 to 100.</param>
/// <param name="MissingFields">Field keys still missing, for the UI to prompt the tourist.</param>
public record ProfileCompletenessResult(bool CanJoinExpedition, int CompletionPercentage, IReadOnlyList<string> MissingFields);
