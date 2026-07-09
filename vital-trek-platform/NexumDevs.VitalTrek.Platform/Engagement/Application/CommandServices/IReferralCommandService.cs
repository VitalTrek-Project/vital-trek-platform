using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;

public interface IReferralCommandService
{
    Task<Referral> Handle(RedeemReferralCodeCommand command, CancellationToken cancellationToken);
}
