using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Profiles.Domain.Model.Entities;

namespace NexumDevs.VitalTrek.Platform.Profiles.Application.CommandServices;

public interface ITouristProfileCommandService
{
    Task<TouristProfile> Handle(UpdateTouristProfileCommand command, CancellationToken cancellationToken);
    Task<TouristProfile> Handle(UpdateIdentityDocumentCommand command, CancellationToken cancellationToken);
    Task<TouristProfile> Handle(UpdateMedicalInfoCommand command, CancellationToken cancellationToken);
    Task<EmergencyContact> Handle(AddEmergencyContactCommand command, CancellationToken cancellationToken);
    Task<EmergencyContact> Handle(UpdateEmergencyContactCommand command, CancellationToken cancellationToken);
    Task Handle(RemoveEmergencyContactCommand command, CancellationToken cancellationToken);
}
