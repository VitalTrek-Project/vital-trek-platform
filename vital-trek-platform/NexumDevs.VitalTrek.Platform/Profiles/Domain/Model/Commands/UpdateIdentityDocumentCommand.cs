using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

public record UpdateIdentityDocumentCommand(Guid UserId, IdentityDocumentType Type, string Number);
