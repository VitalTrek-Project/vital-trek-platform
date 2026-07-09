using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;

public record UpdateMedicalInfoCommand(
    Guid UserId,
    BloodType? BloodType,
    string? Allergies,
    string? MedicalConditions,
    string? Medications);
