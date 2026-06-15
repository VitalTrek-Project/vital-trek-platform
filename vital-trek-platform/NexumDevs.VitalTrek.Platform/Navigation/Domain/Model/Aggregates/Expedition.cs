using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;

public partial class Expedition
{
    public Expedition()
    {
        TourID = null!;
        GuideID = null!;
        Status = null!;
        StartedAt = null!;
        FinishedAt = null!;
    }

    public Expedition(CreateExpeditionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        TourID = command.TourID;
        GuideID = command.GuideID;
        Status = command.Status;
        StartedAt = command.StartedAt;
        FinishedAt = command.FinishedAt;
    }
    
    public int Id { get; }
    
    public TourId TourID { get; private set; }
    public GuideId GuideID { get; private set; }
    public string Status { get; private set; }
    public string StartedAt { get; private set; }
    public string FinishedAt { get; private set; }
}
