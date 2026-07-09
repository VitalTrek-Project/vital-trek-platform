using System.ComponentModel.DataAnnotations;

namespace NexumDevs.VitalTrek.Platform.Profiles.Interfaces.Rest.Resources;

public record StaffProfileResource(Guid Id, Guid UserId, Guid AgencyId, string FullName, string? PhotoUrl, string? Position, string? ContactPhone);

public record UpdateStaffProfileResource([Required] string FullName, string? PhotoUrl, string? Position, string? ContactPhone);

public record StaffPreferencesResource(Guid Id, Guid UserId, bool CriticalAlertsEnabled, bool PendingRedemptionsEnabled, bool NewBookingsEnabled);

public record UpdateStaffNotificationPreferencesResource(bool PendingRedemptionsEnabled, bool NewBookingsEnabled);
