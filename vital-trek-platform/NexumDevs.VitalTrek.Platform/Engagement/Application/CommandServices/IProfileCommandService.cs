using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Aggregates;
using NexumDevs.VitalTrek.Platform.Engagement.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Engagement.Application.CommandServices;

public interface IProfileCommandService
{
    Task<PointsTransaction> Handle(RecordPointsEventCommand command, CancellationToken cancellationToken);
    Task<Review> Handle(SubmitReviewCommand command, CancellationToken cancellationToken);
}
