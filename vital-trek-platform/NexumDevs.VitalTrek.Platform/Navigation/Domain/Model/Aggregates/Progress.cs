using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;

public partial class Progress
{
    public Progress()
    {
        ExpeditionId = 0;
        CompletedCheckpoints = 0;
        TotalCheckpoints = 0;
        Percentage = 0.0;
    }

    public Progress(CreateProgressCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ExpeditionId = command.ExpeditionId;
        CompletedCheckpoints = command.CompletedCheckpoints;
        TotalCheckpoints = command.TotalCheckpoints;
        Percentage = command.Percentage;
    }
    
    public int Id { get; }
    public int ExpeditionId { get; set; }
    public Expedition Expedition { get; internal set; }
    public int CompletedCheckpoints { get; private set; }
    public int TotalCheckpoints { get; private set; }
    public double Percentage { get; private set; }
}
