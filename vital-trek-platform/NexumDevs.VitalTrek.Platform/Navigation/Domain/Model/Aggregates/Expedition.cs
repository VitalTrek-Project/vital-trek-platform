using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;

public partial class Expedition
{
    public Expedition()
    {
        TourID = null!;
        GuideID = null!;
        ExpeditionName = null!;
        Status = null!;
    }

    public Expedition(CreateExpeditionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        TourID = command.TourID;
        GuideID = command.GuideID;
        ExpeditionName = command.ExpeditionName;
        Status = command.Status;
    }
    
    public int Id { get; }
    
    public TourId TourID { get; private set; }
    public GuideId GuideID { get; private set; }
    public string ExpeditionName { get; private set; }
    public string Status { get; private set; }
}
